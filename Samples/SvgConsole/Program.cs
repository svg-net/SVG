using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Svg;

namespace SvgConsole
{
    class Program
    {
        static IEnumerable<string> GetFiles(string inputPath)
        {
            foreach (var file in Directory.EnumerateFiles(inputPath, "*.svg"))
            {
                yield return file;
            }

            foreach (var directory in Directory.EnumerateDirectories(inputPath))
            {
                foreach (var file in GetFiles(directory))
                {
                    yield return file;
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 1 && args.Length != 2)
            {
                Console.WriteLine($"Usage: {nameof(SvgConsole)} <input.svg|directory> [<output.png|directory>]");
                return;
            }

            try
            {
                var inputPath = args[0];
                var isInputPathDirectory = File.GetAttributes(inputPath).HasFlag(FileAttributes.Directory);
                var inputPaths = default(List<string>);
                var outputPath = string.Empty;

                if (isInputPathDirectory)
                {
                    inputPaths = GetFiles(inputPath).ToList();
                }
                else
                {
                    inputPaths = new List<string>();
                    inputPaths.Add(inputPath);
                }

                if (args.Length == 2)
                {
                    outputPath = args[1];
                }

                var useInputPathForOutput = args.Length == 1;
                var isOutputDirectory = outputPath.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase) == false;

                foreach (var path in inputPaths)
                {
                    try
                    {
                        var output = string.Empty;

                        if (useInputPathForOutput)
                        {
                            output = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + ".png");
                        }
                        else
                        {
                            if (isOutputDirectory)
                            {
                                output = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(path) + ".png");
                            }
                            else
                            {
                                Console.WriteLine($"Please provide output directory.");
                            }
                        }

                        var svgDocument = SvgDocument.Open(path);

                        using (var bitmap = svgDocument.Draw())
                        {
                            bitmap.Save(output);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{path}");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
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
