using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace ShapesCanvas.Rendering
{
    /// <summary>
    /// Concrete component in the Decorator pattern.
    /// Paints the solid fill of a single <see cref="ShapeRecord"/> — the core
    /// behavior that decorators wrap and build upon.
    /// </summary>
    public class ShapeRenderer : IShapeRenderer
    {
        private readonly ShapeRecord _record;

        public ShapeRenderer(ShapeRecord record) => _record = record;

        public void Render(ICanvas canvas)
        {
            canvas.FillColor = _record.Color;
            Shape s = _record.Shape;

            if (s is Rectangle)
                canvas.FillRectangle((float)s.AnchorX, (float)s.AnchorY,
                                     (float)s.WidthRequest, (float)s.HeightRequest);
            else
                canvas.FillEllipse((float)s.AnchorX, (float)s.AnchorY,
                                   (float)s.WidthRequest, (float)s.HeightRequest);
        }
    }
}
