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
  Converts svg files to encoded png images.

Usage:
  SvgConsole [options]

Options:
  -f, --inputFiles <inputfiles>              The relative or absolute path to the input files
  -d, --inputDirectory <inputdirectory>      The relative or absolute path to the input directory
  -o, --outputDirectory <outputdirectory>    The relative or absolute path to the output directory
  --outputFiles <outputfiles>                The relative or absolute path to the output files
  --width <width>                            The output image width override
  --height <height>                          The output image height override
  --version                                  Show version information
  -?, -h, --help                             Show help and usage information
```

```
./SvgConsole -f ~/svg/Example.svg
```

```
./SvgConsole -f ~/svg/Example.svg --outputFiles ~/png/Example.png
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
