using Microsoft.Extensions.Logging;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Shapes;
using ShapesCanvas.Rendering;



namespace ShapesCanvas
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }

    // Receiver in the Command pattern. Pure mutation/query surface — no policy.
    public class ShapeDrawable : IDrawable
    {
        private readonly List<ShapeRecord> _shapes = new();

        public void AddShape(ShapeRecord record) => _shapes.Add(record);

        public void RemoveShape(ShapeRecord record) => _shapes.Remove(record);

        // Topmost-wins hit test (mirrors the original last-match behavior).
        public ShapeRecord? FindShapeAt(float x, float y)
        {
            ShapeRecord? target = null;
            foreach (ShapeRecord record in _shapes)
            {
                Shape s = record.Shape;
                if (s.AnchorX <= x && s.AnchorX + s.WidthRequest >= x &&
                    s.AnchorY <= y && s.AnchorY + s.HeightRequest >= y)
                {
                    target = record;
                }
            }
            return target;
        }

        public void ClearShapes() => _shapes.Clear();

        public List<ShapeRecord> GetSnapshot() => new(_shapes);

        public void RestoreShapes(IEnumerable<ShapeRecord> shapes)
        {
            _shapes.Clear();
            _shapes.AddRange(shapes);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            foreach (ShapeRecord record in _shapes)
            {
                // Decorator pattern: start with the base fill renderer, then wrap it
                // in a BorderDecorator that layers an outline on top. Render() is the
                // same call whether or not the shape is decorated.
                IShapeRenderer renderer = new ShapeRenderer(record);
                renderer = new BorderDecorator(renderer, record, Colors.Black, 2f);
                renderer.Render(canvas);
            }
        }
    }

  
}
