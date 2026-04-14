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
        private  List<IShape> _shapes;
        private readonly Random _random = new();
        private GraphicsView _graphicsView = new GraphicsView();
        List<Shape> ShapeCollection = new List<Shape>(); 
        List<Color> ColorCollection = new List<Color>();
        

        public ShapeDrawable() {
            _shapes = new List<IShape>();
            _shapes.Add(new Rectangle());
            _shapes.Add(new Ellipse());
            _graphicsView.Drawable = this;
        }
        public ShapeDrawable(List<IShape> shapes)
        {
            _shapes = new List<IShape>();
            

        }

        public void CreateShape(float x, float y)
        {
            float rand = _random.NextSingle();
            Rectangle rect = new Rectangle();
            rect.HeightRequest = _random.Next(100);
            rect.WidthRequest = _random.Next(100);
            rect.AnchorX = x - rect.WidthRequest / 2;
            rect.AnchorY = y - rect.HeightRequest / 2;
            
           

            Ellipse ellipse = new Ellipse();
            ellipse.HeightRequest = _random.Next(100);
            ellipse.WidthRequest = _random.Next(100);
            ellipse.AnchorX = x - ellipse.WidthRequest / 2;
            ellipse.AnchorY = y - ellipse.HeightRequest / 2;

            if (rand <= 0.5)
            {
                ShapeCollection.Add(rect);
            }
            else
            {
                ShapeCollection.Add(ellipse);
            }

            Color color = new Color(_random.Next(255), _random.Next(255), _random.Next(255));
            ColorCollection.Add(color);
            
        }

        public void RemoveShape(float x , float y)
        {
            Shape ShapeRemove = null;
            foreach (Shape shape in ShapeCollection)
            {
                if(shape.AnchorX <= x && shape.AnchorX + shape.WidthRequest >= x)
                {
                    if (shape.AnchorY <= y && shape.AnchorY + shape.HeightRequest >= y)
                    {
                        ShapeRemove = shape;
                    }
                }

               
            }

            if( ShapeRemove != null ) ColorCollection.RemoveAt(ShapeCollection.IndexOf(ShapeRemove));
            if (ShapeRemove != null ) ShapeCollection.Remove(ShapeRemove);
        }

        public void ClearShapes()
        {
            ColorCollection.Clear();
            ShapeCollection.Clear();
        }



        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            for (int i = 0; i < ShapeCollection.Count; i++)
            {
                Shape shape = ShapeCollection.ElementAt(i);
                canvas.FillColor = ColorCollection.ElementAt(i);
                if (shape.GetType() == typeof(Rectangle))
                {
                    canvas.FillRectangle((float)shape.AnchorX, (float)shape.AnchorY, (float)shape.WidthRequest, (float)shape.HeightRequest);
                }
                else
                {
                    canvas.FillEllipse((float)shape.AnchorX, (float)shape.AnchorY, (float)shape.WidthRequest, (float)shape.HeightRequest);
                }
            }

        }
    }
}
