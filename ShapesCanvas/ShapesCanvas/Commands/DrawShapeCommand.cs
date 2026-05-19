namespace ShapesCanvas.Commands
{
    /// <summary>
    /// Concrete command: creates a random shape at (x, y) and adds it to the receiver.
    /// Undo removes the exact record that was added.
    /// </summary>
    public class DrawShapeCommand : IShapeCommand
    {
        private readonly ShapeDrawable _receiver;
        private readonly float _x;
        private readonly float _y;
        private ShapeRecord? _created;

        public DrawShapeCommand(ShapeDrawable receiver, float x, float y)
        {
            _receiver = receiver;
            _x = x;
            _y = y;
        }

        public void Execute()
        {
            _created = ShapeFactory.CreateRandom(_x, _y);
            _receiver.AddShape(_created);
        }

        public void Undo()
        {
            if (_created is not null)
                _receiver.RemoveShape(_created);
        }
    }
}
