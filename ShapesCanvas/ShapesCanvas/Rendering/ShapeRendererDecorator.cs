using Microsoft.Maui.Graphics;

namespace ShapesCanvas.Rendering
{
    /// <summary>
    /// Abstract decorator in the Decorator pattern.
    /// Holds a reference to a wrapped <see cref="IShapeRenderer"/> and, by default,
    /// simply forwards the render call. Concrete decorators override <see cref="Render"/>
    /// to add extra painting before or after the inner renderer runs.
    /// </summary>
    public abstract class ShapeRendererDecorator : IShapeRenderer
    {
        protected readonly IShapeRenderer Inner;

        protected ShapeRendererDecorator(IShapeRenderer inner) => Inner = inner;

        public virtual void Render(ICanvas canvas) => Inner.Render(canvas);
    }
}
