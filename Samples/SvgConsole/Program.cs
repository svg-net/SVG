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
        public FileInfo[] InputFiles { get; set; }
        public DirectoryInfo InputDirectory { get; set; }
        public FileInfo[] OutputFiles { get; set; }
        public DirectoryInfo OutputDirectory { get; set; }
        public float? Width { get; set; }
        public float? Height { get; set; }
    }

    class Program
    {
        static void Log(string message)
        {
            Console.WriteLine(message);
        }

        static void Error(Exception ex)
        {
            Log($"{ex.Message}");
            Log($"{ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Error(ex.InnerException);
            }
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

            if (svgDocument == null)
            {
                Log($"Error: Failed to load input file: {inputPath.FullName}");
                return;
            }

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

            if (settings.InputFiles != null)
            {
                foreach (var file in settings.InputFiles)
                {
                    paths.Add(file);
                }
            }

            if (settings.InputDirectory != null)
            {
                var directory = settings.InputDirectory;
                GetFiles(directory, "*.svg", paths);
                GetFiles(directory, "*.svgz", paths);
            }

            if (settings.OutputDirectory != null && !string.IsNullOrEmpty(settings.OutputDirectory.FullName))
            {
                if (!Directory.Exists(settings.OutputDirectory.FullName))
                {
                    Directory.CreateDirectory(settings.OutputDirectory.FullName);
                }
            }

            if (settings.OutputFiles != null)
            {
                if (paths.Count > 0 && paths.Count != settings.OutputFiles.Length)
                {
                    Log($"Error: The number of the output files must match the number of the input files.");
                    return;
                }
            }

            for (int i = 0; i < paths.Count; i++)
            {
                var inputPath = paths[i];
                var outputFile = settings.OutputFiles != null ? settings.OutputFiles[i] : null;
                try
                {
                    var outputPath = string.Empty;

                    if (outputFile != null)
                    {
                        outputPath = outputFile.FullName;
                    }
                    else
                    {
                        var inputExtension = inputPath.Extension;
                        outputPath = Path.ChangeExtension(inputPath.FullName, ".png");
                        if (settings.OutputDirectory != null && !string.IsNullOrEmpty(settings.OutputDirectory.FullName))
                        {
                            outputPath = Path.Combine(settings.OutputDirectory.FullName, Path.GetFileName(outputPath));
                        }
                    }

                    Directory.SetCurrentDirectory(Path.GetDirectoryName(inputPath.FullName));

                    Save(inputPath, outputPath, settings.Width, settings.Height);
                }
                catch (Exception ex)
                {
                    Log($"Error: {inputPath.FullName}");
                    Error(ex);
                }
            }
        }

        static async Task<int> Main(string[] args)
        {
            var optionInputFiles = new Option(new[] { "--inputFiles", "-f" }, "The relative or absolute path to the input files")
            {
                Argument = new Argument<FileInfo[]>(getDefaultValue: () => null)
            };

            var optionInputDirectory = new Option(new[] { "--inputDirectory", "-d" }, "The relative or absolute path to the input directory")
            {
                Argument = new Argument<DirectoryInfo>(getDefaultValue: () => null)
            };

            var optionOutputDirectory = new Option(new[] { "--outputDirectory", "-o" }, "The relative or absolute path to the output directory")
            {
                Argument = new Argument<DirectoryInfo>(getDefaultValue: () => null)
            };

            var optionOutputFiles = new Option(new[] { "--outputFiles" }, "The relative or absolute path to the output files")
            {
                Argument = new Argument<FileInfo[]>(getDefaultValue: () => null)
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
                Description = "Converts svg files to encoded png images."
            };

            rootCommand.AddOption(optionInputFiles);
            rootCommand.AddOption(optionInputDirectory);
            rootCommand.AddOption(optionOutputDirectory);
            rootCommand.AddOption(optionOutputFiles);
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
                    Error(ex);
                }
            });

            return await rootCommand.InvokeAsync(args);
        }
    }
}
