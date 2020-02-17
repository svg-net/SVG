# SvgConsole

## .NET Core

Install latest [.NET Core SDK](https://dotnet.microsoft.com/download).

## Publish

### Windows

```
cd Samples/SvgConsole
dotnet publish -f netcoreapp2.2 -c Release -r win-x64 -o SvgConsole-win-x64-netcoreapp2.2
cd SvgConsole-win-x64-netcoreapp2.2
```

### Linux

```
cd Samples/SvgConsole
dotnet publish -f netcoreapp2.2 -c Release -r linux-x64 -o SvgConsole-linux-x64-netcoreapp2.2
cd SvgConsole-linux-x64-netcoreapp2.2
```

### macOS

```
cd Samples/SvgConsole
dotnet publish -f netcoreapp2.2 -c Release -r osx-x64 -o SvgConsole-osx-x64-netcoreapp2.2
cd SvgConsole-osx-x64-netcoreapp2.2
```

### Other

* [.NET Core RID Catalog](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog)
* [dotnet publish](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish)

## Usage

```
SvgConsole:
  Converts a svg file to an encoded png image.

Usage:
  SvgConsole [options]

Options:
  -if, --inputFiles <inputfiles>                The relative or absolute path to the input files
  -id, --inputDirectories <inputdirectories>    The relative or absolute path to the input directories
  -of, --outputFile <outputfile>                The relative or absolute path to the output file
  -od, --outputDirectory <outputdirectory>      The relative or absolute path to the output directory
  --width <width>                               The output image width override
  --height <height>                             The output image height override
  --version                                     Show version information
  -?, -h, --help                                Show help and usage information
```

```
./SvgConsole -if ~/svg/Example.svg
```

```
./SvgConsole -if ~/svg/Example.svg -od ~/png
```

```
./SvgConsole -id ~/svg/
```

```
./SvgConsole -id ~/svg -od ~/png
```
