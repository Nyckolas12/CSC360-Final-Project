# ShapesCanvas — Design Patterns Overview

## What the project is

**ShapesCanvas** is an interactive drawing application built with **.NET MAUI**. The
user taps (or clicks) on a canvas to drop randomly-sized, randomly-colored shapes,
erase the shape under the pointer, clear the whole canvas, and undo the last action.

The app is small on purpose: the goal is to show how three classic Gang-of-Four
design patterns — one from each major category — work together in a real, running
codebase rather than in isolated textbook snippets.

| GoF Category   | Pattern Used | Where it lives                         |
| -------------- | ------------ | -------------------------------------- |
| **Creational** | Factory      | `ShapeFactory.cs`                      |
| **Behavioral** | Command      | `Commands/`                            |
| **Structural** | Decorator    | `Rendering/`                           |

### High-level flow

```
User taps the canvas
        │
        ▼
MainPage  ──creates──►  Command  ──asks──►  ShapeFactory  (builds a random ShapeRecord)
   │                       │
   │ hands command to      │ mutates
   ▼                       ▼
CommandInvoker        ShapeDrawable (the "receiver" / model — holds the shapes)
   │ (keeps undo history)        │
   │                            │ on redraw, each shape is wrapped by
   ▼                            ▼
   Undo()                  ShapeRenderer + BorderDecorator  ──►  paints to the canvas
```

---

## 1. Creational — Factory (`ShapeFactory.cs`)

### Role mapping
- **Product** — `ShapeRecord` (a `Shape` paired with a `Color`).
- **Factory** — the static `ShapeFactory` class.
- **Factory methods** — `Create(ShapeType, x, y)` and `CreateRandom(x, y)`.
- **Supported products** — `ShapeType.Rectangle`, `ShapeType.Ellipse`.

### What it does
`ShapeFactory` is the single place that knows *how to build a shape*: it picks a
random size, a random color, computes the anchor so the shape is centered on the
tap point, and returns a ready-to-use `ShapeRecord`. Callers never use `new
Rectangle(...)` themselves — they just ask the factory.

```csharp
// DrawShapeCommand asks the factory; it never builds shapes itself.
_created = ShapeFactory.CreateRandom(_x, _y);
```

### How it improves the project
- **One source of truth for construction.** Sizing, coloring, and centering logic
  is centralized, so it can't drift or be duplicated across call sites.
- **Easy to extend.** Adding a triangle means adding one enum value and one
  `Build…` method — no changes to the commands or the UI.
- **Decoupling.** The rest of the app depends on the abstract `ShapeRecord`, not on
  concrete shape types or random-generation details.

---

## 2. Behavioral — Command (`Commands/`)

### Role mapping
- **Command interface** — `IShapeCommand` (`Execute()` + `Undo()`).
- **Concrete commands** — `DrawShapeCommand`, `EraseShapeCommand`,
  `ClearShapesCommand`.
- **Receiver** — `ShapeDrawable` (performs the actual add/remove/clear work).
- **Invoker** — `CommandInvoker` (runs commands and keeps the undo history).
- **Client** — `MainPage` (creates and dispatches commands in response to input).

### What it does
Every user action is reified as an object that knows how to perform itself *and* how
to reverse itself. The invoker runs the command and pushes it onto a history stack;
`Undo()` pops the last command and calls its reverse logic.

```csharp
// MainPage turns a tap into a command and hands it to the invoker.
_invoker.Run(new DrawShapeCommand(Canvas, x, y));
...
_invoker.Undo(); // pops the last command and reverses it
```

Each command captures exactly what it needs to undo itself:
- `DrawShapeCommand` remembers the record it created, and removes it on undo.
- `EraseShapeCommand` remembers the record it erased, and re-adds it on undo.
- `ClearShapesCommand` snapshots the whole canvas, and restores it on undo.

### How it improves the project
- **Undo for free, uniformly.** Because every action implements the same
  `Execute()/Undo()` contract, the invoker handles undo generically — it doesn't
  care what the command does.
- **Decoupled UI.** `MainPage` doesn't know *how* a shape is drawn or erased; it only
  knows how to issue commands. The "what" lives in the command, the "how" in the
  receiver.
- **Extensible.** New actions (move, resize, redo) drop in as new command classes
  without touching the invoker or existing commands.

---

## 3. Structural — Decorator (`Rendering/`)  ← the newly added pattern

This is the pattern that was **missing** from the original project. The codebase had
a Creational pattern (Factory) and a Behavioral pattern (Command), but **no
Structural pattern**. The Decorator fills that gap.

### Role mapping
- **Component interface** — `IShapeRenderer` (`Render(ICanvas)`).
- **Concrete component** — `ShapeRenderer` (paints the solid fill of a shape).
- **Base decorator** — `ShapeRendererDecorator` (holds a wrapped `IShapeRenderer`
  and forwards to it by default).
- **Concrete decorator** — `BorderDecorator` (lets the inner renderer paint, then
  strokes an outline on top).

### What it does
Rendering used to be a single inline loop inside `ShapeDrawable.Draw`. It is now
built from small, composable pieces. A base renderer draws the fill; a decorator
*wraps* that renderer and adds an outline — without modifying the shape classes or
the base renderer.

```csharp
// ShapeDrawable.Draw — compose at render time:
IShapeRenderer renderer = new ShapeRenderer(record);            // fill
renderer = new BorderDecorator(renderer, record, Colors.Black); // + outline
renderer.Render(canvas);                                        // one uniform call
```

Because every layer implements the same `IShapeRenderer.Render` method, the caller
makes the *same call* whether the shape is bare or wrapped in any number of
decorators. New visual effects (drop shadow, glow, dashed border) can be added as
additional decorators and stacked in any order — each one wraps the previous.

### How it improves the project
- **Open/Closed in action.** New rendering effects are added by writing a new
  decorator, never by editing `ShapeRenderer` or the `Shape` types.
- **Composability over inheritance.** Effects are combined at runtime by nesting
  wrappers, avoiding a combinatorial explosion of subclasses (BorderedShape,
  ShadowedShape, BorderedShadowedShape, …).
- **Visible payoff.** The `BorderDecorator` gives every shape a crisp outline, so
  overlapping same-colored shapes stay visually distinct.
- **Separation of concerns.** Drawing logic moved out of the model
  (`ShapeDrawable`) into dedicated renderer classes, keeping the model focused on
  holding state.

---

## How the three patterns reinforce each other

The patterns aren't just present side-by-side — they hand off to one another along
the lifecycle of a single shape:

1. **Command** captures the user's intent (`DrawShapeCommand`).
2. That command calls the **Factory** to construct the shape (`ShapeFactory`).
3. The shape lives in the receiver (`ShapeDrawable`), and on every repaint the
   **Decorator** chain renders it (`ShapeRenderer` → `BorderDecorator`).
4. The **Command** invoker can later reverse the whole thing via `Undo()`.

Together they keep each responsibility isolated — *creating* shapes, *acting* on
them, and *rendering* them are three independent concerns — which makes the app
easier to read, safer to change, and straightforward to extend.

---

## File reference

| File | Pattern | Role |
| ---- | ------- | ---- |
| `ShapesCanvas/ShapeFactory.cs` | Factory | Factory + Product (`ShapeRecord`) |
| `ShapesCanvas/Commands/IShapeCommand.cs` | Command | Command interface |
| `ShapesCanvas/Commands/DrawShapeCommand.cs` | Command | Concrete command |
| `ShapesCanvas/Commands/EraseShapeCommand.cs` | Command | Concrete command |
| `ShapesCanvas/Commands/ClearShapesCommand.cs` | Command | Concrete command |
| `ShapesCanvas/Commands/CommandInvoker.cs` | Command | Invoker (undo history) |
| `ShapesCanvas/MainPage.xaml.cs` | Command | Client |
| `ShapesCanvas/MauiProgram.cs` (`ShapeDrawable`) | Command | Receiver |
| `ShapesCanvas/Rendering/IShapeRenderer.cs` | Decorator | Component interface |
| `ShapesCanvas/Rendering/ShapeRenderer.cs` | Decorator | Concrete component |
| `ShapesCanvas/Rendering/ShapeRendererDecorator.cs` | Decorator | Base decorator |
| `ShapesCanvas/Rendering/BorderDecorator.cs` | Decorator | Concrete decorator |
