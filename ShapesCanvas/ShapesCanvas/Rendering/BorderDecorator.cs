using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace ShapesCanvas.Rendering
{
    /// <summary>
    /// Concrete decorator in the Decorator pattern.
    /// Lets the wrapped renderer paint the fill first, then strokes a crisp outline
    /// on top so overlapping shapes stay visually distinct. The wrapped shape class
    /// is never modified — the border is layered on at render time.
    /// </summary>
    public class BorderDecorator : ShapeRendererDecorator
    {
        private readonly ShapeRecord _record;
        private readonly Color _borderColor;
        private readonly float _thickness;

        public BorderDecorator(IShapeRenderer inner, ShapeRecord record,
                               Color borderColor, float thickness = 2f)
            : base(inner)
        {
            _record = record;
            _borderColor = borderColor;
            _thickness = thickness;
        }

        public override void Render(ICanvas canvas)
        {
            // 1. Delegate to the wrapped renderer (the fill, or another decorator).
            Inner.Render(canvas);

            // 2. Add this decorator's own contribution: the outline.
            canvas.StrokeColor = _borderColor;
            canvas.StrokeSize = _thickness;
            Shape s = _record.Shape;

            if (s is Rectangle)
                canvas.DrawRectangle((float)s.AnchorX, (float)s.AnchorY,
                                     (float)s.WidthRequest, (float)s.HeightRequest);
            else
                canvas.DrawEllipse((float)s.AnchorX, (float)s.AnchorY,
                                   (float)s.WidthRequest, (float)s.HeightRequest);
        }
    }
}
