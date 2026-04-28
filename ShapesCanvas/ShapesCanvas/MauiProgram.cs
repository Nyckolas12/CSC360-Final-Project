using Microsoft.Extensions.Logging;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Shapes;



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

    public class ShapeDrawable : IDrawable
    {
        // Single list of (Shape, Color) pairs — no more parallel lists.
        private readonly List<ShapeRecord> _shapes = new();

        public void CreateShape(float x, float y)
        {
            // Delegate all construction logic to the factory.
            ShapeRecord record = ShapeFactory.CreateRandom(x, y);
            _shapes.Add(record);
        }

        public void RemoveShape(float x, float y)
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

            if (target is not null)
                _shapes.Remove(target);
        }

        public void ClearShapes() => _shapes.Clear();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            foreach (ShapeRecord record in _shapes)
            {
                canvas.FillColor = record.Color;
                Shape s = record.Shape;

                if (s is Rectangle)
                    canvas.FillRectangle((float)s.AnchorX, (float)s.AnchorY,
                                         (float)s.WidthRequest, (float)s.HeightRequest);
                else
                    canvas.FillEllipse((float)s.AnchorX, (float)s.AnchorY,
                                       (float)s.WidthRequest, (float)s.HeightRequest);
            }
        }
    }

  
}
