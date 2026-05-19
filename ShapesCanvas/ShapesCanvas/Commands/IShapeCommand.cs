namespace ShapesCanvas.Commands
{
    /// <summary>
    /// Command interface in the Command pattern.
    /// Each concrete command captures everything needed to perform an action
    /// on the <see cref="ShapeDrawable"/> receiver and to reverse it.
    /// </summary>
    public interface IShapeCommand
    {
        void Execute();
        void Undo();
    }
}
