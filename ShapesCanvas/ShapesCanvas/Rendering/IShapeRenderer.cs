using Microsoft.Maui.Graphics;

namespace ShapesCanvas.Rendering
{
    /// <summary>
    /// Component interface in the Decorator pattern.
    /// Anything that can paint itself onto an <see cref="ICanvas"/> implements this,
    /// so base renderers and decorators are interchangeable.
    /// </summary>
    public interface IShapeRenderer
    {
        void Render(ICanvas canvas);
    }
}
