using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Svg;
using System.IO;
using System.Drawing;

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
