using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesCanvas
{
    
    public record ShapeRecord(Shape Shape, Color Color);

    /// <summary>
    ///  creates randomly-sized, randomly-colored shapes centered on (x, y).
    /// </summary>
    public static class ShapeFactory
    {
        private static readonly Random _random = new();

        // Supported shape types — extend this enum to add new shapes.
        public enum ShapeType { Rectangle, Ellipse }

        /// <summary>
        /// returns a ShapeRecord for a random shape type.
        /// </summary>
        public static ShapeRecord CreateRandom(float x, float y)
        {
            ShapeType type = _random.NextSingle() <= 0.5f
                ? ShapeType.Rectangle
                : ShapeType.Ellipse;

            return Create(type, x, y);
        }

        /// <summary>
        ///  returns a ShapeRecord for the shape type.
        /// </summary>
        public static ShapeRecord Create(ShapeType type, float x, float y)
        {
            Shape shape = type switch
            {
                ShapeType.Rectangle => BuildRectangle(x, y),
                ShapeType.Ellipse => BuildEllipse(x, y),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };

            Color color = new Color(
                _random.Next(256),
                _random.Next(256),
                _random.Next(256)
            );

            return new ShapeRecord(shape, color);
        }

        

        private static Rectangle BuildRectangle(float x, float y)
        {
            var r = new Rectangle
            {
                WidthRequest = _random.Next(20, 120),
                HeightRequest = _random.Next(20, 120)
            };
            // AnchorX/Y store the top-left corner so the shape centers on the tap point.
            r.AnchorX = x - r.WidthRequest / 2;
            r.AnchorY = y - r.HeightRequest / 2;
            return r;
        }

        private static Ellipse BuildEllipse(float x, float y)
        {
            var e = new Ellipse
            {
                WidthRequest = _random.Next(20, 120),
                HeightRequest = _random.Next(20, 120)
            };
            e.AnchorX = x - e.WidthRequest / 2;
            e.AnchorY = y - e.HeightRequest / 2;
            return e;
        }
    }
}
