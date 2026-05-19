using System.Collections.Generic;

namespace ShapesCanvas.Commands
{
    /// <summary>
    /// Concrete command: clears every shape from the receiver.
    /// Undo restores the previous snapshot.
    /// </summary>
    public class ClearShapesCommand : IShapeCommand
    {
        private readonly ShapeDrawable _receiver;
        private List<ShapeRecord>? _snapshot;

        public ClearShapesCommand(ShapeDrawable receiver)
        {
            _receiver = receiver;
        }

        public void Execute()
        {
            _snapshot = _receiver.GetSnapshot();
            _receiver.ClearShapes();
        }

        public void Undo()
        {
            if (_snapshot is not null)
                _receiver.RestoreShapes(_snapshot);
        }
    }
}
