namespace ShapesCanvas.Commands
{
    /// <summary>
    /// Concrete command: erases the topmost shape at (x, y) from the receiver.
    /// Undo re-adds the same record.
    /// </summary>
    public class EraseShapeCommand : IShapeCommand
    {
        private readonly ShapeDrawable _receiver;
        private readonly float _x;
        private readonly float _y;
        private ShapeRecord? _erased;

        public EraseShapeCommand(ShapeDrawable receiver, float x, float y)
        {
            _receiver = receiver;
            _x = x;
            _y = y;
        }

        public void Execute()
        {
            _erased = _receiver.FindShapeAt(_x, _y);
            if (_erased is not null)
                _receiver.RemoveShape(_erased);
        }

        public void Undo()
        {
            if (_erased is not null)
                _receiver.AddShape(_erased);
        }
    }
}
