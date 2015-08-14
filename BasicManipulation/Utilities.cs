using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BasicManipulation
{
    public class Utilities
    {
        /** http://iswwwup.com/t/bd67c699ce19/how-to-draw-an-arrow-in-wpf-programatically.html */
        private const double _maxArrowLengthPercent = 0.3; // factor that determines how the arrow is shortened for very short lines
        private const double _lineArrowLengthFactor = 3.73205081; // 15 degrees arrow:  = 1 / Math.Tan(15 * Math.PI / 180); 

        public static PointCollection CreateLineWithArrowPointCollection(Point startPoint, Point endPoint, double lineWidth)
        {
            Vector direction = endPoint - startPoint;

            Vector normalizedDirection = direction;
            normalizedDirection.Normalize();

            Vector normalizedlineWidenVector = new Vector(-normalizedDirection.Y, normalizedDirection.X); // Rotate by 90 degrees
            Vector lineWidenVector = normalizedlineWidenVector * lineWidth * 0.5;

            double lineLength = direction.Length;

            double defaultArrowLength = lineWidth * _lineArrowLengthFactor;

            // Prepare usedArrowLength
            // if the length is bigger than 1/3 (_maxArrowLengthPercent) of the line length adjust the arrow length to 1/3 of line length

            double usedArrowLength;
            if (lineLength * _maxArrowLengthPercent < defaultArrowLength)
                usedArrowLength = lineLength * _maxArrowLengthPercent;
            else
                usedArrowLength = defaultArrowLength;

            // Adjust arrow thickness for very thick lines
            double arrowWidthFactor;
            if (lineWidth <= 1.5)
                arrowWidthFactor = 3;
            else if (lineWidth <= 2.66)
                arrowWidthFactor = 4;
            else
                arrowWidthFactor = 1.5 * lineWidth;

            Vector arrowWidthVector = normalizedlineWidenVector * arrowWidthFactor;


            // Now we have all the vectors so we can create the arrow shape positions
            var pointCollection = new PointCollection(7);

            Point endArrowCenterPosition = endPoint - (normalizedDirection * usedArrowLength);

            pointCollection.Add(endPoint); // Start with tip of the arrow
            pointCollection.Add(endArrowCenterPosition + arrowWidthVector);
            pointCollection.Add(endArrowCenterPosition + lineWidenVector);
            pointCollection.Add(startPoint + lineWidenVector);
            pointCollection.Add(startPoint - lineWidenVector);
            pointCollection.Add(endArrowCenterPosition - lineWidenVector);
            pointCollection.Add(endArrowCenterPosition - arrowWidthVector);

            return pointCollection;
        }

        public static Rect GetRectOfObject(FrameworkElement _element)
        {
            Rect rectangleBounds = new Rect();
            rectangleBounds = _element.RenderTransform.TransformBounds(new Rect(0, 0, _element.Width, _element.Height));
            return rectangleBounds;
        }
    }
}
