using ShapesCanvas.Commands;

namespace ShapesCanvas
{
    public partial class MainPage : ContentPage
    {
        private readonly CommandInvoker _invoker = new();

        public MainPage()
        {
            InitializeComponent();
        }

        private ShapeDrawable Canvas => (ShapeDrawable)PlayArea.Drawable;

        private void DrawShape(object sender, TappedEventArgs e)
        {
            Point? point = e.GetPosition(this);
            if (point is null) return;

            _invoker.Run(new DrawShapeCommand(Canvas, (float)point.Value.X, (float)point.Value.Y));
            PlayArea.Invalidate();
        }

        private void ErazeShape(object sender, TappedEventArgs e)
        {
            Point? point = e.GetPosition(this);
            if (point is null) return;

            _invoker.Run(new EraseShapeCommand(Canvas, (float)point.Value.X, (float)point.Value.Y));
            PlayArea.Invalidate();
        }

        private void OnClearButtonClicked(object sender, EventArgs e)
        {
            _invoker.Run(new ClearShapesCommand(Canvas));
            PlayArea.Invalidate();
        }

        private void OnUndoButtonClicked(object sender, EventArgs e)
        {
            _invoker.Undo();
            PlayArea.Invalidate();
        }
    }
}
