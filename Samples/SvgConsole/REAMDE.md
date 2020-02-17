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
cd SvgConsole-netcoreapp2.2-linux-x64
```

### macOS

```
cd Samples/SvgConsole
dotnet publish -f netcoreapp2.2 -c Release -r osx-x64 -o SvgConsole-osx-x64-netcoreapp2.2
cd SvgConsole-osx-x64-netcoreapp2.2
```

### Other

https://docs.microsoft.com/en-us/dotnet/core/rid-catalog
https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish

## Usage

```
Usage: SvgConsole <input.svg> [<output.png>|<directory>]
```

```
./SvgConsole ~/svg/Example.svg
```

```
./SvgConsole ~/svg/Example.svg ~/png/Example.png
```

```
./SvgConsole ~/svg/Example.svg  ~/png
```
