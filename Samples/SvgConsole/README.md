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
  -f, --files <files>                The relative or absolute path to the input files
  -d, --directories <directories>    The relative or absolute path to the input directories
  -o, --output <output>              The relative or absolute path to the output directory
  --width <width>                    The output image width override
  --height <height>                  The output image height override
  --version                          Show version information
  -?, -h, --help                     Show help and usage information
```

```
./SvgConsole -f ~/svg/Example.svg
```

```
./SvgConsole -f ~/svg/Example.svg -o ~/png
```

```
./SvgConsole -d ~/svg/
```

```
./SvgConsole -d ~/svg -o ~/png
```
