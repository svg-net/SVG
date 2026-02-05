using System.IO;

using NUnit.Framework;

namespace Svg.UnitTests
{
    [TestFixture]
    public class SvgElementCollectionTests
    {
        [Test]
        public void AddChildren()
        {
            SvgDocument svg = new SvgDocument();

            // add and remove some groups

            SvgGroup groupFirst = new SvgGroup();
            SvgGroup groupMiddle = new SvgGroup();
            SvgGroup groupToRemove = new SvgGroup();
            SvgGroup groupLast = new SvgGroup();

            svg.Children.Add(groupMiddle);
            svg.Children.Add(groupToRemove);
            svg.Children.Insert(2, groupLast);
            svg.Children.Insert(0, groupFirst);
            svg.Children.Remove(groupToRemove);

            // check the order is correct
            Assert.AreEqual(3, svg.Children.Count);
            Assert.AreEqual(groupFirst, svg.Children[0]);
            Assert.AreEqual(groupMiddle, svg.Children[1]);
            Assert.AreEqual(groupLast, svg.Children[2]);

            // check the parent is correct
            Assert.AreEqual(svg, groupFirst.Parent);
            Assert.AreEqual(svg, groupMiddle.Parent);
            Assert.AreEqual(svg, groupLast.Parent);
            Assert.IsNull(groupToRemove.Parent);

            // add and remove some shapes

            SvgCircle shapeFirst = new SvgCircle();
            SvgEllipse shapeMiddle = new SvgEllipse();
            SvgRectangle shapeLast = new SvgRectangle();
            SvgPolygon shapeToRemove = new SvgPolygon();

            groupFirst.Children.Add(shapeMiddle);
            groupFirst.Children.Add(shapeToRemove);
            groupFirst.Children.Insert(2, shapeLast);
            groupFirst.Children.Insert(0, shapeFirst);
            groupFirst.Children.Remove(shapeToRemove);

            // check the order is correct
            Assert.AreEqual(3, groupFirst.Children.Count);
            Assert.AreEqual(shapeFirst, groupFirst.Children[0]);
            Assert.AreEqual(shapeMiddle, groupFirst.Children[1]);
            Assert.AreEqual(shapeLast, groupFirst.Children[2]);

            // check the parent is correct
            Assert.AreEqual(groupFirst, shapeFirst.Parent);
            Assert.AreEqual(groupFirst, shapeMiddle.Parent);
            Assert.AreEqual(groupFirst, shapeLast.Parent);
            Assert.IsNull(shapeToRemove.Parent);
        }
    }
}
