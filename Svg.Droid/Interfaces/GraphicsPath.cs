using System;
using System.Drawing;

namespace Svg
{
    public interface GraphicsPath : IDisposable
    {
        RectangleF GetBounds();
        void StartFigure();
        void AddEllipse(float f, float f1, float f2, float f3);
        void CloseFigure();
        decimal PointCount { get; }
        PointF[] PathPoints { get; }
        FillMode FillMode { get; set; }
        float[] PathTypes { get; set; }
        PathData PathData { get; set; }
        void AddLine(PointF getDevicePoint, PointF endPoint);
        PointF GetLastPoint();
        void AddRectangle(RectangleF rectangle);
        void AddArc(RectangleF rect, float startAngle, float sweepAngle);
        GraphicsPath Clone();
        void Transform(Matrix transform);
        void AddPath(GraphicsPath childPath, bool b);
        void AddString(string text, FontFamily fontFamily, int style, float size, PointF location, StringFormat createStringFormatGenericTypographic);
        void AddBezier(PointF start, PointF firstControlPoint, PointF secondControlPoint, PointF end);
        void AddBezier(float startX, float firstControlPoint, float secondControlPoint, float end, float f, float f1, float endpointX, float endpointY);
        bool IsVisible(PointF pointF);
        void Flatten();
        void AddPolygon(PointF[] polygon);
        void Reset();
    }
}