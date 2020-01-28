using System;
using System.Collections.Generic;
using Svg;
using System.IO;

namespace Entities
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\sample.svg");

            var sampleDoc = SvgDocument.Open<SvgDocument>(filePath,  new Dictionary<string, string> 
                {
                    {"entity1", "fill:red" },
                    {"entity2", "fill:yellow" }
                });

            sampleDoc.Draw().Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\sample.png"));
        }
    }
}
