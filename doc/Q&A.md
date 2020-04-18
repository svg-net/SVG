This is currently a collection of answered questions in issues that have been closed meanwhile.
The format of the page is preliminary and maybe changed if more questions accumulate.

## How to get started
Please use our [getting started article](http://vvvv.github.io/SVG/doc/GettingStarted.html) to get started with installation and implementation of the SVG library.

## How to re-render an SVG faster?

(from [#327](https://github.com/vvvv/SVG/issues/327), by @flemingtech)

The rendering type plays a significant roll on rendering speeds. For example, it anti-aliasing is off for the SvgDocument render times are notably faster.

Because of the huge reduction in image quality, this wasn't a viable solution for my needs. Instead what I've come up with so far seems to work since I can't figure out how to get clipping regions to work.

After I load the SVG, I make new SVG with the same initial SvgDocument properties (basically a deep copy followed by deleting all children). As I walk the first document tree I'm looking for elements I know are going to be modified. For each one that I find, I remove it from the first SVG and put it into the 2nd SVG. When I'm doing this, I also apply any parent transforms to the new child since it doesn't need/have all of it's parents.

Once I'm done, I render the first SVG to an Image. When any of the 'animating' elements are changed, the 2nd SVG is rendered on top of a copy of the first SVG's rendering to form a complete composite. This prevents all the non-moving elements for having to re-render, unless of course the target graphics width/height changes. This is giving huge performance gains.

## Can I use SVG.NET in a UWP Windows 10 App?

(from [#219](https://github.com/vvvv/SVG/issues/219), by @jonthysell)

SVG.NET requires the System.Drawing namespace, which is not available in UWP. See http://stackoverflow.com/questions/31545389/windows-universal-app-with-system-drawing-and-possible-alternative.

## How to render an SVG image to a single-color bitmap image?

(from [#366](https://github.com/vvvv/SVG/issues/366), by @UweKeim)

I was able to find a solution with the following fragment:

```csharp
var svgDoc = SvgDocument.Open<SvgDocument>(svgFilePath, null);

// Recursively change all nodes.
processNodes(svgDoc.Descendants(), new SvgColourServer(Color.DarkGreen));

var bitmap = svgDoc.Draw();
```

together with this function:

```csharp
private void processNodes(IEnumerable<SvgElement> nodes, SvgPaintServer colorServer)
{
    foreach (var node in nodes)
    {
        if (node.Fill != SvgPaintServer.None) node.Fill = colorServer;
        if (node.Color != SvgPaintServer.None) node.Color = colorServer;
        if (node.StopColor != SvgPaintServer.None) node.StopColor = colorServer;
        if (node.Stroke != SvgPaintServer.None) node.Stroke = colorServer;

        processNodes(node.Descendants(), colorServer);
    }
}
```

## How to render only a specific SvgElement?

(from [#403](https://github.com/vvvv/SVG/issues/403), by @ievgennaida)

Use `element.RenderElement();`.

## How to render an SVG document to a bitmap in another size?

Use `SvgDocument.Draw(int rasterWidth, int rasterHeight)`. If one of the values is 0, it it set to preserve the aspect ratio, if both values are given, the aspect ratio is ignored.

## Is this code server-safe?

(from [#381](https://github.com/vvvv/SVG/issues/381), by @rangercej, answered by @gvheertum)

I used it in server side code (ASP.NET MVC application and API's) and never had any problems with it. There is however be possible issues regarding use in services and API's, for example the System.Drawing might not always be available in certain situations (if I am not mistaken, some Azure service will not provide the System.Drawing since it relies on GDI calls) and will also be an issue when using it as "portable" code for example in .NET standard or .NET core (but I believe the library is already working on a migration/compatibility with .NET core/standard).

So issues when using System.Drawing are indeed possible when using in a non interactive scenario, since most non-interactive code will often be functioning as service or API, meaning a lot of synchronous calls are possible, which opens a world of possible problems compared to an interactive app which you often only have a few instances loaded. System.Drawing can (and often will) be resource-heavy, so having a lot of synchronous processes will possibly have a huge impact on the performance. So I guess, that is why Microsoft warns about the usage. Rendering a large complex SVG to a big bitmap (eg 5000x5000px) will put a large load on the server, doing this in parallel might cause issues in performance and availability.

System.Drawing was initially not really created for service usage, so you *can* use it, but really need to be aware of possible issues. For example, see this article: https://photosauce.net/blog/post/5-reasons-you-should-stop-using-systemdrawing-from-aspnet

I believe there are some parallelisation tests in the UnitTest suite, since the SVG component did have some concurrency issues in the past. The parallelisation tests show that some parallel work is possible, but upping the limit will show you that resource issues are to be expected under large loads. When failing, System.Drawing will often not fail gracefully, but will often crash with some meaningless error (which makes debugging pretty hard sometimes).  

## How to change the SvgUnit DPI?

(from [#313](https://github.com/vvvv/SVG/issues/313), by @KieranSmartMP)

`SvgUnit` takes the DPI (which is called `Ppi` here) from the document. This is set to the system DPI at creation time, but can be set to another value afterwards, e.g. 
```c#
  doc = SvgDocument();
  doc.Ppi = 200;
  ...
```

## Why does my application crash with "cannot allocate the required memory"?

(from [#250](https://github.com/vvvv/SVG/issues/250), by @Radzhab)

If you try to open a very large SVG file in your application, it may crash, because .NET refuses to allocate that much contiguous memory, even if it could do so in theory. This is done to avoid processes to consume too much memory and slow down the system. Nothing we can do about this - you may catch this exception in your application and inform the user, or try to resize your SVG document and retry.

## How to add a custom attribute to an SVG element?

(from [#481](https://github.com/vvvv/SVG/issues/481), by @lroye)

Custom attributes are publicly accessible as a collection, you can add an attribute like this:
```C#
    element.CustomAttributes[attributeName] = attributeValue;
```

## I'm getting a SvgGdiPlusCannotBeLoadedException if running under Linux or MacOs

(see [#494](https://github.com/vvvv/SVG/pull/495#issuecomment-505429874), by @ErlendSB)

This happens if libgdiplus is not installed under Linux or MacOs - libgdiplus is need for the implementation of System.Drawing.Common. The system will validate gdi+ capabilities when calling SvgDocument.Open(), if the gdi+ capabilities are not available, you will receive a SvgGdiPlusCannotBeLoadedException. 

There is a [packaging project on Github](https://github.com/CoreCompat/libgdiplus-packaging) that helps installing that, here are the installation instructions (copied here for convenience):

Older versions of the package threw a NullReferenceException when calling the SvgDocument.Open function. The cause of these errors was the same. Newer releases (since version 3.0), will yield a more descriptive exception as described above.

### Using libgdiplus on Ubuntu Linux

You can install libgdiplus on Ubuntu Linux using the Quamotion PPA. Follow these steps:
```
sudo add-apt-repository ppa:quamotion/ppa
sudo apt-get update
sudo apt-get install -y libgdiplus
```
### Using libgdiplus on macOS

On macOS, add a reference to the runtime.osx.10.10-x64.CoreCompat.System.Drawing package:

```dotnet add package runtime.osx.10.10-x64.CoreCompat.System.Drawing```

When building from source-code you can also uncomment the 
```
<!-- <ItemGroup Condition="'$(TargetFramework)' == 'netcoreapp2.2'">
    <PackageReference Include="runtime.osx.10.10-x64.CoreCompat.System.Drawing" Version="5.6.20" />
  </ItemGroup> -->
``` 
block in the Svg.csproj file.

### Validating GDI+ capabilities

If you want to make sure the executing system is capable of using the GDI+ features, you can use one of the functions available on the SvgDocument class.

If you only want to get a boolean telling whether the capabilities are available, please use the following code:
```
bool hasGdiCapabilities = SvgDocument.SystemIsGdiPlusCapable();
```

If you want to ensure the capabilities and let an error be thrown when these are not available, please use the following code:
```
SvgDocument.EnsureSystemIsGdiPlusCapable();
```
This function will throw a SvgGdiPlusCannotBeLoadedException if the capabilities are not available.