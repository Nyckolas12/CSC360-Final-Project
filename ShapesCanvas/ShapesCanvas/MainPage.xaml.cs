using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Graphics;



namespace ShapesCanvas
{
    public partial class MainPage : ContentPage
    {

        private readonly Random _random = new();
        private readonly List<IShape> _shapes = new();
        private readonly List<Rect> _shapeBounds = new();
       
        public MainPage()
        {
            InitializeComponent();
            
        }

        private void DrawShape(object sender, TappedEventArgs e)
        {

            Point? point = e.GetPosition(this);
            ((ShapeDrawable)(PlayArea.Drawable)).CreateShape((float)point.Value.X, (float)point.Value.Y);
            PlayArea.Invalidate();

        }

        private void ErazeShape(object sender, TappedEventArgs e)
        {
            Point? point = e.GetPosition(this);
            ((ShapeDrawable)(PlayArea.Drawable)).RemoveShape((float)point.Value.X, (float)point.Value.Y);
            PlayArea.Invalidate();
        }

        private void OnClearButtonClicked(object sender, EventArgs e)
        {
            ((ShapeDrawable)PlayArea.Drawable).ClearShapes();
            PlayArea.Invalidate(); 
        }

      

        
    }
   
}
