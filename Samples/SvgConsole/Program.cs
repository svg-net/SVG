using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;
using Svg;

namespace SvgConsole
{
    class Settings
    {
        public FileInfo[]? Files { get; set; }
        public DirectoryInfo[]? Directories { get; set; }
        public DirectoryInfo? Output { get; set; }
        public float? Width { get; set; }
        public float? Height { get; set; }
    }

    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var optionFile = new Option(new[] { "--files", "-f" }, "The relative or absolute path to the input files")
            {
                Argument = new Argument<FileInfo[]?>(defaultValue: () => null)
            };

            var optionDirectory = new Option(new[] { "--directories", "-d" }, "The relative or absolute path to the input directories")
            {
                Argument = new Argument<DirectoryInfo[]?>(defaultValue: () => null)
            };

            var optionOutput = new Option(new[] { "--output", "-o" }, "The relative or absolute path to the output directory")
            {
                Argument = new Argument<DirectoryInfo?>(defaultValue: () => null)
            };

            var optionWidth = new Option(new[] { "--width", "-w" }, "The output image width override")
            {
                Argument = new Argument<float?>(defaultValue: () => null)
            };

            var optionHeight = new Option(new[] { "--height", "-h" }, "The output image height override")
            {
                Argument = new Argument<float?>(defaultValue: () => null)
            };

            var rootCommand = new RootCommand()
            {
                Description = "Converts a svg file to an encoded png image."
            };

            rootCommand.AddOption(optionFile);
            rootCommand.AddOption(optionDirectory);
            rootCommand.AddOption(optionOutput);
            rootCommand.AddOption(optionWidth);
            rootCommand.AddOption(optionHeight);

            rootCommand.Handler = CommandHandler.Create((Settings settings) =>
            {
                try
                {
                    Run(settings);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            });

            return await rootCommand.InvokeAsync(args);
        }

        static void GetFiles(DirectoryInfo directory, string pattern, List<FileInfo> paths)
        {
            var files = Directory.EnumerateFiles(directory.FullName, pattern);
            if (files != null)
            {
                foreach (var path in files)
                {
                    paths.Add(new FileInfo(path));
                }
            }
        }

        static void Save(FileInfo inputPath, string outputPath, float? width, float? height)
        {
            var svgDocument = SvgDocument.Open(inputPath.FullName);

            if (width.HasValue)
            {
                svgDocument.Width = width.Value;
            }

            if (height.HasValue)
            {
                svgDocument.Height = height.Value;
            }

            using var bitmap = svgDocument.Draw();

            bitmap.Save(outputPath);
        }

        static void Run(Settings settings)
        {
            var paths = new List<FileInfo>();

            if (settings.Files != null)
            {
                foreach (var file in settings.Files)
                {
                    paths.Add(file);
                }
            }

            if (settings.Directories != null)
            {
                foreach (var directory in settings.Directories)
                {
                    GetFiles(directory, "*.svg", paths);
                    GetFiles(directory, "*.svgz", paths);
                }
            }

            if (settings.Output != null && !string.IsNullOrEmpty(settings.Output.FullName))
            {
                if (!Directory.Exists(settings.Output.FullName))
                {
                    Directory.CreateDirectory(settings.Output.FullName);
                }
            }

            for (int i = 0; i < paths.Count; i++)
            {
                var inputPath = paths[i];
                try
                {
                    var extension = inputPath.Extension;
                    string outputPath = inputPath.FullName.Remove(inputPath.FullName.Length - extension.Length) + ".png";
                    if (settings.Output != null && !string.IsNullOrEmpty(settings.Output.FullName))
                    {
                        outputPath = Path.Combine(settings.Output.FullName, Path.GetFileName(outputPath));
                    }

                    Directory.SetCurrentDirectory(Path.GetDirectoryName(inputPath.FullName));

                    Save(inputPath, outputPath, settings.Width, settings.Height);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{inputPath.FullName}");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }
    }
}
