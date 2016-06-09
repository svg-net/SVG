using System.Drawing;

namespace Svg
{
    public class PathData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="types"></param>
        public PathData(PointF[] points, byte[] types)
        {
            Points = points;
            Types = types;
        }

        // Summary:
        //     Gets or sets an array of System.Drawing.PointF structures that represents
        //     the points through which the path is constructed.
        //
        // Returns:
        //     An array of System.Drawing.PointF objects that represents the points through
        //     which the path is constructed.
        public PointF[] Points { get; private set; }
        //
        // Summary:
        //     Gets or sets the types of the corresponding points in the path.
        //
        // Returns:
        //     An array of bytes that specify the types of the corresponding points in the
        //     path.
        public byte[] Types { get; private set; }
    }
}