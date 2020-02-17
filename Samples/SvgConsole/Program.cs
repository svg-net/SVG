using System;
using System.IO;
using Svg;

namespace SvgConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1 && args.Length != 2)
            {
                Console.WriteLine($"Usage: {nameof(SvgConsole)} <input.svg> [<output.png>|<directory>]");
                return;
            }

            try
            {
                var inputPath = args[0];
                var outputPath = string.Empty;

                if (args.Length == 1)
                {
                    outputPath = Path.Combine(Path.GetDirectoryName(inputPath), Path.GetFileNameWithoutExtension(inputPath) + ".png");
                }
                else
                {
                    outputPath = args[1];
                    if (outputPath.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase) == false)
                    {
                        outputPath = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(inputPath) + ".png");
                    }
                }

                var svgDocument = SvgDocument.Open(inputPath);
                if (svgDocument != null)
                {
                    var bitmap = svgDocument.Draw();
                    bitmap.Save(outputPath);
                }
                else
                {
                    Console.WriteLine("Failed to open svg document.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
