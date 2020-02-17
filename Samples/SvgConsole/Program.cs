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
        public FileInfo[] Files { get; set; }
        public DirectoryInfo[] Directories { get; set; }
        public FileInfo OutputFile { get; set; }
        public DirectoryInfo OutputDirectory { get; set; }
        public float? Width { get; set; }
        public float? Height { get; set; }
    }

    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var optionInputFiles = new Option(new[] { "--inputFiles", "-if" }, "The relative or absolute path to the input files")
            {
                Argument = new Argument<FileInfo[]>(getDefaultValue: () => null)
            };

            var optionInputDirectories = new Option(new[] { "--inputDirectories", "-id" }, "The relative or absolute path to the input directories")
            {
                Argument = new Argument<DirectoryInfo[]>(getDefaultValue: () => null)
            };

            var optionOutputFile = new Option(new[] { "--outputFile", "-of" }, "The relative or absolute path to the output file")
            {
                Argument = new Argument<DirectoryInfo>(getDefaultValue: () => null)
            };

            var optionOutputDirectory = new Option(new[] { "--outputDirectory", "-od" }, "The relative or absolute path to the output directory")
            {
                Argument = new Argument<DirectoryInfo>(getDefaultValue: () => null)
            };

            var optionWidth = new Option(new[] { "--width" }, "The output image width override")
            {
                Argument = new Argument<float?>(getDefaultValue: () => null)
            };

            var optionHeight = new Option(new[] { "--height" }, "The output image height override")
            {
                Argument = new Argument<float?>(getDefaultValue: () => null)
            };

            var rootCommand = new RootCommand()
            {
                Description = "Converts a svg file to an encoded png image."
            };

            rootCommand.AddOption(optionInputFiles);
            rootCommand.AddOption(optionInputDirectories);
            rootCommand.AddOption(optionOutputFile);
            rootCommand.AddOption(optionOutputDirectory);
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

            using (var bitmap = svgDocument.Draw())
            {
                bitmap.Save(outputPath);
            }
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

            if (settings.OutputDirectory != null && !string.IsNullOrEmpty(settings.OutputDirectory.FullName))
            {
                if (!Directory.Exists(settings.OutputDirectory.FullName))
                {
                    Directory.CreateDirectory(settings.OutputDirectory.FullName);
                }
            }

            for (int i = 0; i < paths.Count; i++)
            {
                var inputPath = paths[i];
                try
                {
                    Directory.SetCurrentDirectory(Path.GetDirectoryName(inputPath.FullName));

                    string outputPath = string.Empty;

                    if (settings.OutputFile != null)
                    {
                        outputPath = settings.OutputFile.FullName;
                    }
                    else
                    {
                        var inputExtension = inputPath.Extension;
                        outputPath = inputPath.FullName.Remove(inputPath.FullName.Length - inputExtension.Length) + ".png";
                        if (settings.OutputDirectory != null && !string.IsNullOrEmpty(settings.OutputDirectory.FullName))
                        {
                            outputPath = Path.Combine(settings.OutputDirectory.FullName, Path.GetFileName(outputPath));
                        }
                    }

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
