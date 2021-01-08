# SVG Benchmarks

### Run `All` Benchmarks

```
dotnet run -c Release -f netcoreapp3.1 -- -f '*'
```

### Run `SvgDocument` Benchmarks

```
dotnet run -c Release -f netcoreapp3.1 -- -f '*SvgDocument_*'
```

### Run `SvgPathBuilder` Benchmarks

```
dotnet run -c Release -f netcoreapp3.1 -- -f '*SvgPathBuilder_*'
```

### Run `CoordinateParser` Benchmarks

```
dotnet run -c Release -f netcoreapp3.1 -- -f '*CoordinateParser_*'
```

### Run `SvgTransformConverter` Benchmarks

```
dotnet run -c Release -f netcoreapp3.1 -- -f '*SvgTransformConverter_*'
```

### Run `SvgUnitConverter` Benchmarks

```
dotnet run -c Release -f netcoreapp3.1 -- -f '*SvgUnitConverter_*'
```

### Run `SvgUnitCollectionConverter` Benchmarks

```
dotnet run -c Release -f netcoreapp3.1 -- -f '*SvgUnitCollectionConverter_*'
```

### TODO

- EnumBaseConverter
- SvgPreserveAspectRatioConverter
- SvgNumberCollectionConverter
- SvgOrientConverter
- SvgPointCollectionConverter
- SvgViewBoxConverter
- SvgPaintServerFactory
- ColorConverter
