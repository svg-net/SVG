using System;
using System.Drawing;
using System.IO;
using Svg;

namespace SvgSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var sampleDoc = SvgDocument.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\sample.svg"));
            sampleDoc.GetElementById<SvgUse>("Commonwealth_Star").Fill = new SvgColourServer(Color.Black);
            sampleDoc.Draw().Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\sample.png"));
        }
    }
}
