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

### TODO

- EnumBaseConverter
- SvgPreserveAspectRatioConverter
- SvgNumberCollectionConverter
- SvgOrientConverter
- SvgPointCollectionConverter
- SvgUnitCollectionConverter
- SvgUnitConverter
- SvgViewBoxConverter
- SvgPaintServerFactory
- SvgPathBuilder
- ColorConverter
