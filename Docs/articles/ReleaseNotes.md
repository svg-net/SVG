# SVG.NET Release Notes
The release versions are NuGet releases.

## Unreleased

### Enhancements
* Added support for `Switch` element with "systemLanguage" selector attribute (see [#1176](https://github.com/svg-net/SVG/issues/1176)).
* Enabled `IsTrimmable` and `IsAotCompatible` attributes on the assembly (see [#1184](https://github.com/svg-net/SVG/pull/1184)).

### Fixes
* Fixed: `<CompilerGeneratedFilesOutputPath>` in `Svg.Custom` to generate `Generated` directory under project. (see [PR #1153](https://github.com/svg-net/SVG/pull/1153))

## [Version 3.4.7](https://www.nuget.org/packages/Svg/3.4.7)  (2024-02-22)

### Changes
* Added: A class, `SvgOptions`, with external css parameters for styling or transforming the `SvgDocument`.
* Added: Support for .NET 4.7.2, .NET 4.8.1 and .NET 8.0.
* [Breaking Change] The support for .NET 8 resulted in a breaking change to the signature of methods in `CoordinateParser`.
  * The changes are needed to fix a compiler error `CS8352`.
  * The use of read-only arguments as referenced variable is not necessary.
  * The use of `ref` in the function arguments resulting in `CS8352` error are all removed.

### Enhancements
* made exceptions serializable to be able to cross AppDomain boundaries (see [#826](https://github.com/svg-net/SVG/pull/826))

### Fixes
* Fixed: XML namespace prefixes are also applied for nodes declaring them (see [PR #1106](https://github.com/svg-net/SVG/pull/1106)).
* Fixed: Parameter is not valid (see [PR #1131](https://github.com/svg-net/SVG/pull/1131)).

### Infrastructure
* Split tests SVG/PNG files into Issues and W3C Test Suite. W3C Test Suite files are not automatically downloaded.
* Repository clean-up, reducing the download zip size from 22MB to 4.5MB.
* Added: A new project, `Tests/Svg.Tests.Common` for sharing codes between test applications.
* Moved: The `ReleaseNotes.md` to `Docs/ReleaseNotes.md`.

## [Version 3.4.6](https://www.nuget.org/packages/Svg/3.4.6)  (2023-11-16)

### Fixes
* fixed loading of embedded images (see [#1101](https://github.com/svg-net/SVG/issues/1101))

## [Version 3.4.5](https://www.nuget.org/packages/Svg/3.4.5)  (2023-10-31)

### Changes
* removed out of support framework versions .NET 5 (replaced with .NET 6) (see PR [#1045](https://github.com/svg-net/SVG/pull/1045))

### Fixes
* fixed build error in C# 11 (see [PR #1030](https://github.com/svg-net/SVG/pull/1030))
* fixed out of memory exception on SVGs with gradients (see [PR #1038](https://github.com/svg-net/SVG/pull/1038))
* fixed missing styles when `DeepCopy` the `SvgElement` (see [PR #1053](https://github.com/svg-net/SVG/pull/1053))
* fix the color string format incompatible with the Edge/Chrome browsers in case of no System.Drawing.Common (see [PR #1055](https://github.com/svg-net/SVG/pull/1055))
* fixed duplication of excessive `SvgElement`s in `DeepCopy` (see [#1054](https://github.com/svg-net/SVG/issues/1054))

### Infrastructure
* Update ExCSS to 4.2.2 (see [PR #1091](https://github.com/svg-net/SVG/pull/1091))

## [Version 3.4.4](https://www.nuget.org/packages/Svg/3.4.4)  (2022-10-29)

### Fixes
* fixed localized family names in `SvgFontManager` (see [PR #993](https://github.com/svg-net/SVG/pull/993))
* fixed out of memory Exception in `SvgImage.Render()` (see [#1003](https://github.com/svg-net/SVG/issues/1003))
* fixed argument Exception in `Draw(Graphics)` (see [#1004](https://github.com/svg-net/SVG/issues/1004))
* updated `System.Drawing.Common` to prevent Remote Code Execution Vulnerability (see [#1025](https://github.com/svg-net/SVG/issues/1025))

### Performance
* performance optimization for `SvgPath.Path()` (see [#1018](https://github.com/svg-net/SVG/issues/1018), [#1013](https://github.com/svg-net/SVG/issues/1013))

## [Version 3.4.3](https://www.nuget.org/packages/Svg/3.4.3)  (2022-07-16)

### Changes
* removed out of support framework versions .NET 4.5.2/4.6.1 (replaced with 4.6.2) and .NET Core 2.1 (see PR [#980](https://github.com/svg-net/SVG/pull/980))

### Enhancements
* update XMLOutputTester (rename to SVGBuilder) (see [PR #983](https://github.com/svg-net/SVG/pull/983))

### Fixes
* fixed out of memory in Svg 3.4.2 (see [#989](https://github.com/svg-net/SVG/issues/989))

### Infrastructure
* the ExCSS library is now included via NuGet instead of copying the sources
  (see [#596](https://github.com/svg-net/SVG/issues/596))

## [Version 3.4.2](https://www.nuget.org/packages/Svg/3.4.2)  (2022-04-11)

### Changes
* made anti-aliasing behavior more consistent with rendering in browsers (see [#960](https://github.com/svg-net/SVG/issues/960))

### Fixes
* fixed issue for `SvgTextBase.Bounds` with blank `SvgTextSpan` (see [PR #963](https://github.com/svg-net/SVG/pull/963))
* fixed precision issue in `SvgUnit` (see [PR #969](https://github.com/svg-net/SVG/pull/969))
* fixed `InheritGradient` values are not inherited (see [#966](https://github.com/svg-net/SVG/issues/966))

## [Version 3.4.1](https://www.nuget.org/packages/Svg/3.4.1)  (2022-03-19)

### Fixes
* fixed `text-decoration` conversion (see [#941](https://github.com/svg-net/SVG/issues/941))
* fixed `text-transformation` (see [#952](https://github.com/svg-net/SVG/issues/952))
* fixed invalid value is set in `Attributes` of `SvgTextBase` (see [#954](https://github.com/svg-net/SVG/issues/954))
* fixed precision issue in `SvgUnit` (see [PR #961](https://github.com/svg-net/SVG/pull/961))

## [Version 3.4.0](https://www.nuget.org/packages/Svg/3.4.0)  (2022-01-09)

### Performance
* reduce very high memory usage (see [#909](https://github.com/svg-net/SVG/issues/909))

### Enhancements
* support relative commands in Paths (see [PR #925](https://github.com/svg-net/SVG/pull/925))

### Fixes
* fixed color blend position (see [#902](https://github.com/svg-net/SVG/issues/902))
* remove override method that is no longer needed in `SvgPath` (see [#830](https://github.com/svg-net/SVG/issues/830))
* fixed size of inner `<svg>` (see [#917](https://github.com/svg-net/SVG/issues/917))

## [Version 3.3.0](https://www.nuget.org/packages/Svg/3.3.0)  (2021-09-23)

### Changes
* changed namespace of `SvgSymbol` from `Svg.Document_Structure` to `Svg` (see [PR #556](https://github.com/svg-net/SVG/pull/556))
* mitigated XXE vulnerability: changed default behavior of DTD resolution so external references are not resolved by default
  (see [#869](https://github.com/svg-net/SVG/issues/869))
* improved safety of rendering untrusted files: changed default behavior so external references to images, text definitions, 
  and other resources are not resolved by default (see [#872](https://github.com/svg-net/SVG/issues/872))
* made `SvgExtentions.Traverse` and `SvgExtentions.TraverseDepthFirst` internal to avoid polluting the API
  (see [#889](https://github.com/svg-net/SVG/issues/889))

### Enhancements
* minimize `XmlTextReader` customization (see [PR #836](https://github.com/svg-net/SVG/pull/836))
* manage namespaces and prefixes (see [#604](https://github.com/svg-net/SVG/issues/604))
* add `Open(XmlReader)` method in `SvgDocument` (see [#877](https://github.com/svg-net/SVG/issues/877))

### Fixes
* fixed filled polyline not displayed with `stroke-width=0` (see [#785](https://github.com/svg-net/SVG/issues/785))
* added basic implementation of filter classes (see [#768](https://github.com/svg-net/SVG/issues/768))
* prevent stack overflow in size calculation for empty SVG (see [#755](https://github.com/svg-net/SVG/issues/755))
* fixed different prefix is assigned using `XmlTextWriter` (see [#817](https://github.com/svg-net/SVG/issues/817))
* fixed scaling if opacity is not 1 (see [#863](https://github.com/svg-net/SVG/issues/863))
* fixed unnecessary dependency on `System.ValueTuple` (see [#879](https://github.com/svg-net/SVG/issues/879))
* prevent `ISvgRenderer.SmoothingMode` reset when `RequiresSmoothRendering` is `true`
  (see [#882](https://github.com/svg-net/SVG/issues/882))
* ignore `x` and `y` attributes in outermost svg as per standard (see [#886](https://github.com/svg-net/SVG/issues/886))
* fixed index is outside array boundary (see [#888](https://github.com/svg-net/SVG/issues/888))

## [Version 3.2.3](https://www.nuget.org/packages/Svg/3.2.3) (2021-01-12)

### Enhancements
* implement font-weight bolder and lighter (see [PR #727](https://github.com/svg-net/SVG/pull/727))
* check if BaseUri is absolute (see [PR #738](https://github.com/svg-net/SVG/pull/738))
* added support for `miter-clip` and `arcs` line joins, and for `fr` radial gradient property
  (SVG2 features, see [PR #621](https://github.com/svg-net/SVG/pull/621))

### Fixes
* fixed filter Inherited (see [#541](https://github.com/svg-net/SVG/issues/541) and [PR #689](https://github.com/svg-net/SVG/pull/689))
* fixed calculate required layout rectangle (see [#732](https://github.com/svg-net/SVG/issues/732) and [PR #741](https://github.com/svg-net/SVG/pull/741))
* fixed the parsing of significant whitespace (see [#744](https://github.com/svg-net/SVG/issues/744) and [PR #745](https://github.com/svg-net/SVG/pull/745))
* fixed build error in .NET Standard 2.1 (see [#746](https://github.com/svg-net/SVG/issues/746) and [PR #750](https://github.com/svg-net/SVG/pull/750))
* fixed opacity issue (see [#747](https://github.com/svg-net/SVG/issues/747) and [PR #751](https://github.com/svg-net/SVG/pull/751))
* fixed bounds calculation in polygon (see [#758](https://github.com/svg-net/SVG/issues/758) and [PR #759](https://github.com/svg-net/SVG/pull/759))
* make sure that paths with a single `moveto` command don't display line caps
 (see [#634](https://github.com/vvvv/SVG/issues/634)
* fixed rendering of text-anchor `middle` (see [#769](https://github.com/vvvv/SVG/issues/769)
* fixed opacity not correctly inherited issue (see [#777](https://github.com/vvvv/SVG/issues/777) and [PR #788](https://github.com/vvvv/SVG/pull/788))
* fixed fallback color (see [#779](https://github.com/vvvv/SVG/issues/779) and [PR #792](https://github.com/vvvv/SVG/pull/792))
* fixed size zero pattern issue (see [PR #793](https://github.com/vvvv/SVG/pull/793) and [PR #794](https://github.com/vvvv/SVG/pull/794))

## [Version 3.1.1](https://www.nuget.org/packages/Svg/3.1.1) (2020-05-01)

### Enhancements
* added new filter effects classes (see [PR #641](https://github.com/svg-net/SVG/pull/641))
* added FilterUnits and PrimitiveUnits properties to SvgFilter class (see [PR #641](https://github.com/svg-net/SVG/pull/641))
* added X, Y, Width and Height properties to SvgFilterPrimitive class (see [PR #641](https://github.com/svg-net/SVG/pull/641))
* added SvgNumberCollection data type similar to SvgPointCollection (see [PR #641](https://github.com/svg-net/SVG/pull/641))
* added MaskUnits, MaskContentUnits, X, Y, Width and Height properties to SvgMask (see [PR #654](https://github.com/svg-net/SVG/pull/654))
* added FontStretch property to SvgElement (see [PR #654](https://github.com/svg-net/SVG/pull/654))
* moved ColorInterpolationFilters property to SvgElement because its a presentation attribute (see [PR #667](https://github.com/svg-net/SVG/pull/667))
* added ColorInterpolation property to SvgElement (see [PR #667](https://github.com/svg-net/SVG/pull/667))
* added Href property to SvgFilter (see [PR #679](https://github.com/svg-net/SVG/pull/679))
* supports localized family names (see [PR #706](https://github.com/svg-net/SVG/pull/706))

### Fixes
* fixed CoordinateParser handling of invalid state (see [PR #640](https://github.com/svg-net/SVG/pull/640))
* fixed CoordinateParser handling of invalid state (see [PR #642](https://github.com/svg-net/SVG/pull/642))
* set correct default values for SvgFilter properties (see [PR #641](https://github.com/svg-net/SVG/pull/641))
* dispose Matrix in SvgFilter (see [PR #644](https://github.com/svg-net/SVG/pull/644))
* dispose resources in ImageBuffer (see [PR #646](https://github.com/svg-net/SVG/pull/646))
* fixed StdDeviation property type of the SvgGaussianBlur class (see [PR #648](https://github.com/svg-net/SVG/pull/648))
* fixed Providing entities in SvgDocument.Open does not work (see [#651](https://github.com/svg-net/SVG/issues/651))
* fixed initial values of attributes related to text (see [PR #655](https://github.com/svg-net/SVG/pull/655))
* fixed 'inherit' does not work at visibility and display (see [PR #656](https://github.com/svg-net/SVG/pull/656))
* fixed Won't display gradients if they're wider than 698 px (see [#252](https://github.com/svg-net/SVG/issues/252) and [PR #658](https://github.com/svg-net/SVG/pull/658))
* fixed 'clip-rule' attribute. (see [PR #662](https://github.com/svg-net/SVG/pull/662))
* fixed SvgFontStyle values (see [PR #661](https://github.com/svg-net/SVG/pull/661))
* fixed EnumConverters (see [PR #663](https://github.com/svg-net/SVG/pull/663))
* fixed Parameter is not valid (see [#664](https://github.com/svg-net/SVG/issues/664) and [PR #665](https://github.com/svg-net/SVG/pull/665))
* fixed Endless loop and out of memory on a specific file (see [#675](https://github.com/svg-net/SVG/issues/675) and [PR #681](https://github.com/svg-net/SVG/pull/681))
* fixed 'none' does not work at clip-path and filter (see [PR #686](https://github.com/svg-net/SVG/pull/686))
* fixed argument of Path method (see [PR #690](https://github.com/svg-net/SVG/pull/690))
* fixed w3c example styling-css-08-f (see [PR #692](https://github.com/svg-net/SVG/pull/692))
* fixed Output namespace in inner `<svg>` element (see [PR #702](https://github.com/svg-net/SVG/pull/702))
* fixed SvgNodeReader does not resolve entity reference (see [#707](https://github.com/svg-net/SVG/issues/707) and [PR #713](https://github.com/svg-net/SVG/pull/713))

## [Version 3.0.102](https://www.nuget.org/packages/Svg/3.0.102) (2020-01-13)

### Changes
* removed support for .NET 3.5
* upgraded the used Fizzler libary to 1.2.0 (supports Netstandard 1.0 and 2.0)

### Enhancements
* check that there is a `moveto` command at the beginning of a path (see [PR #616](https://github.com/svg-net/SVG/pull/616))
* add support for `<a>` element (see [#626](https://github.com/svg-net/SVG/issues/626) and [PR #628](https://github.com/svg-net/SVG/pull/628)))
* added ColorConverter from dotnet runtime codebase to make Netstandard 2.0 target more complete (see [PR #630](https://github.com/svg-net/SVG/pull/630))

### Fixes
* fixed nested svg tags not rendered properly (see [#622](https://github.com/svg-net/SVG/issues/622))
* added handling of invalid property in parser (see [#632](https://github.com/svg-net/SVG/issues/632))

## [Version 3.0.84](https://www.nuget.org/packages/Svg/3.0.84) (2019-11-09)

_**Note:**_
* this is the last release version that still supports .NET version 3.5
* the support for .NET Standard introduced in this version is preliminary and incomplete
* a compatibility warning from the Fizzler library is shown during build;
  this can be safely ignored and will be gone in the next version
 
### Enhancements
* added preliminary support for .NET Standard 2.0 (see [#346](https://github.com/svg-net/SVG/issues/346));
  Drawing2D is not fully supported
* added support for href namespace (see [PR #579](https://github.com/svg-net/SVG/pull/579)) 
* support non-standard mime types for embedded images (see [#578](https://github.com/svg-net/SVG/issues/578))

### Infrastructure
* the Fizzler library is now included via NuGet instead of copying the sources
* added Gitter chat room for SVG.NET

### Documentation
* moved documentation to GitHub pages
* added auto-generated API documentation

### Fixes
* fixed scaling of embedded images (see [#592](https://github.com/svg-net/SVG/issues/592))
* fixed issue for stroke dasharray with odd number of values (see [PR #584](https://github.com/svg-net/SVG/pull/584)) 
* fixed parsing of some color attributes (see [PR #580](https://github.com/svg-net/SVG/pull/580)) 
* fixed behavior of 'Inherit' value for several attributes (see [#541](https://github.com/svg-net/SVG/issues/541))


## [Version 3.0.49](https://www.nuget.org/packages/Svg/3.0.49) (2019-08-30)

**Note:** this is the first version that supports .NET Core alongside .NET.
To build it yourself, you need at least Visual Studio 2017 due to the added multi-platform support.

### Enhancements
* added support for .NET Core 2.2 (see PR [#448](https://github.com/svg-net/SVG/pull/448))[Faq.md](Faq.md)
* handle missing gdi+ library on MacOs or Linux by a descriptive exception (see [#501](https://github.com/svg-net/SVG/issues/501))
* allow ID start with a number (see [#138](https://github.com/svg-net/SVG/issues/138))
* added support for embedded SVG in data URIs (see [#71](https://github.com/svg-net/SVG/issues/71)
  and [#220](https://github.com/svg-net/SVG/issues/220))
* support `auto-start-reverse` value for marker orientation (see PR [#458](https://github.com/svg-net/SVG/pull/458)) 
* added support for the SvgScript tag (see [PR #558](https://github.com/svg-net/SVG/pull/558)) 

### Infrastructure
* use NUnit instead of MSTest for unit tests (see [#420](https://github.com/svg-net/SVG/issues/420))
* added automatic git versioning
* xml documentation is included in the nuget package

### Documentation
* added "Getting Started" Wiki page

### Fixes

* added check for invalid bounds (see [#554](https://github.com/svg-net/SVG/issues/554))
* added support for "Grey" color (see [PR #551](https://github.com/svg-net/SVG/pull/551)) 
* updated core compat package to resolve font issues on Mac (see [#548](https://github.com/svg-net/SVG/issues/548))
* fixed parsing of white spaces in color matrix (see [PR #540](https://github.com/svg-net/SVG/pull/540))
* fixed zero matrix transformation issues (see [PR #537](https://github.com/svg-net/SVG/pull/537))
* avoid adding a null system font (see [#528](https://github.com/svg-net/SVG/issues/528))
* fixed missing text drawing (see [#84](https://github.com/svg-net/SVG/issues/84))
* fixed y2 default value for SvgLinearGradientServer (see [PR #530](https://github.com/svg-net/SVG/pull/530))
* fixed incorrect parsing of some float values for non-English cultures
  (see [PR #525](https://github.com/svg-net/SVG/pull/525) and [#526](https://github.com/svg-net/SVG/pull/526))
* fixed pattern drawing (see [#280](https://github.com/svg-net/SVG/issues/280))
* prevent crash on reading entities (see [#518](https://github.com/svg-net/SVG/issues/518))
* fixed saving of attributes with default value (see [PR #520](https://github.com/svg-net/SVG/pull/520))
* fixed determination of OS type (see [PR #517](https://github.com/svg-net/SVG/pull/517))
* fixed writing of custom style attributes (see [#507](https://github.com/svg-net/SVG/issues/507))
* handle overlapping caps by joining the lines (see [#508](https://github.com/svg-net/SVG/issues/508))
* correctly handle style attributes in top level svg element (see [#391](https://github.com/svg-net/SVG/issues/391))
* fixed incorrect rendering if stroke-dasharray value is none (see [PR #504](https://github.com/svg-net/SVG/pull/504))
* prevent exception for zero bounds and opacity not one (see [#479](https://github.com/svg-net/SVG/issues/479))
* make sure mask elements are written back to svg (see [#271](https://github.com/svg-net/SVG/issues/271))
* fixed incorrect clip region (see [#363](https://github.com/svg-net/SVG/issues/363))
* fixed overflow error on 1-character text with tspan (see [#488](https://github.com/svg-net/SVG/issues/488))
* fixed crash with unsupported pseudo classes (see [#315](https://github.com/svg-net/SVG/issues/315))
* fixes wrong text position in some scenarios (see PR [#475](https://github.com/svg-net/SVG/pull/475))
* fixed handling of spaces for `xml:space="default"` (see PR [#471](https://github.com/svg-net/SVG/pull/471))
* fixed crash if more than font have the same name (see [#452](https://github.com/svg-net/SVG/issues/452))
* fixed rendering bug for text on path using very large font
  (see PR [#468](https://github.com/svg-net/SVG/pull/468))
* avoid exception in nested SVGs without size (see [#460](https://github.com/svg-net/SVG/issues/460))
* fixed default input values for filter primitives
* fixed parsing of float values in color matrixes and colors on non-English systems
* fixed xlink:href value format (see PR [#455](https://github.com/svg-net/SVG/pull/455))
* support various formats of URL string (see PR [#454](https://github.com/svg-net/SVG/pull/454))
* fixed stack overflow crash on images with relative size
  (see [#436](https://github.com/svg-net/SVG/issues/436))

## [Version 2.4.3](https://www.nuget.org/packages/Svg/2.4.3) (2019-05-07)
### Fixes
* fixed boundary drawing with corner and stroke (see PR [#444](https://github.com/svg-net/SVG/pull/444))
* fixed rendering with fill opacity 0 (see [#437](https://github.com/svg-net/SVG/issues/437))
* fixed opacity attribute (see PR [#433](https://github.com/svg-net/SVG/pull/433))
* fixed bounds calculation with stroke (see PR [#433](https://github.com/svg-net/SVG/pull/433))

## [Version 2.4.2](https://www.nuget.org/packages/Svg/2.4.2) (2019-03-01)
### Enhancements
* added font manager to allow user-defined font handling
  (see PR [#414](https://github.com/svg-net/SVG/pull/414))

### Fixes
* fixed handling of invalid hex color and whitespace after hex color (see [#399](https://github.com/svg-net/SVG/issues/399))
* fixed default font size (caused text not to be displayed, see [#419](https://github.com/svg-net/SVG/issues/419))
* fixed writing of RGBA colors (see [#129](https://github.com/svg-net/SVG/issues/129))
* fixed writing of custom styles (see [#129](https://github.com/svg-net/SVG/issues/129))
* fixed handling of default values for radial gradients (see [#397](https://github.com/svg-net/SVG/issues/397))
* allow empty value for style property (see [#318](https://github.com/svg-net/SVG/issues/318))
* added handling of referenced viewBox scaling in "use" elements
* handle special case where path consists of a single move command
  (see [#223](https://github.com/svg-net/SVG/issues/223))
* correctly write fill-rule, clip-rule and named color attributes as lower case
  (see [#272](https://github.com/svg-net/SVG/issues/272))
* several fixes for markers:
  * added support for marker attributes in groups
  * partly fixed marker appearance (stroke and fill color, scaling, default orientation)
  * apply transformations in the marker drawing element (see [#215](https://github.com/svg-net/SVG/issues/215))
  * correctly show middle markers for paths with Bezier curves
  * handle markers on paths with successive equal points

## [Version 2.4.1](https://www.nuget.org/packages/Svg/2.4.1) (2019-01-25)
### Changes
* `ExCSS` lives now in the `Svg` namespace to avoid namespace collisions 
  (see [#408](https://github.com/svg-net/SVG/issues/408))

### Fixes
* fixed handling of url IDs enclosed in apostrophes (see [#345](https://github.com/svg-net/SVG/issues/345)) 
* fixed calculation of percentage values (PR [#410](https://github.com/svg-net/SVG/pull/410))
* regression: missing scaling if rendering into a bitmap with defined size 
  (see [#405](https://github.com/svg-net/SVG/issues/405))
* consider transformation for all svg element bounds (see [#331](https://github.com/svg-net/SVG/issues/331))
* prevent crash if `use` element has no reference (see [#323](https://github.com/svg-net/SVG/issues/323))
* fixed handling of `fill=currentColor` (see [#398](https://github.com/svg-net/SVG/issues/398))

## [Version 2.4.0](https://www.nuget.org/packages/Svg/2.4.0) (2019-01-08)

### Enhancements
  * added basic support for CSS text-transform 
  * added optional size parameter to `SvgDocument.Draw()`
  * allow relative paths for image URLs
  * improved path drawing performance
  * added XML Header to conform according to SVG spec ([PR #269](https://github.com/svg-net/SVG/pull/269))
  * added support for removing Byte Order Mark (BOM) ([PR #269](https://github.com/svg-net/SVG/pull/269))

### Infrastructure
  * added copy of license
  * added automatic unit test execution after check-in in AppVeyor

### Fixes
  * fixed display of rounded caps for dashed lines using dasharray (see [#191](https://github.com/svg-net/SVG/issues/191))
  * fixed calculation of percentage units in 'y' (see [#329](https://github.com/svg-net/SVG/issues/329))
  * fixed calculation of percentage units in `stroke-width` (see [#338](https://github.com/svg-net/SVG/issues/338))
  * fixed display of `dasharray` with odd number (see [#58](https://github.com/svg-net/SVG/issues/58))
  * fixed font alignment for "middle" and "end" (see [#385](https://github.com/svg-net/SVG/issues/385))
  * fixed handling of `stroke-dashoffset` (see [#388](https://github.com/svg-net/SVG/issues/388))
  * fixed font shorthand parsing
  * fixed case insensitive enum parsing
  * ignore cycles in `use` elements to prevent crash
  * fixes bounds calculation for `use` elements
  * corrected DPI calculation to fix text positioning for printing 
  * ignore `textLength` attribute if X attribute is list
  * fixed drawing of SvgFont objects
  * fixed adjustment if `lengthAdjust='spacingAndGlyphs'` (see [#373](https://github.com/svg-net/SVG/issues/373))
  * fixed SvgAttribute reflection for .Net core
  * fixed default value for `preserveAspectRatio` attribute 
  * fixed path parsing mistaking 'E' as a command instead of an exponent
  * fixed image opacity
  * fixed usage of `ms colortranslator` class
  * fixed improper use of UTF8Encoding
  * fixed runtime error after accessing added `SvgText` element (see [#332](https://github.com/svg-net/SVG/issues/332)) 
  * fixed rendering error due to invalid `ColorBlend` position
  * fixed inheriting `text-anchor` and `baseline-shift` attributes
  * prevent crashes on zero length segments or paths
  * fixed handling of nested SVGs (see [#244](https://github.com/svg-net/SVG/issues/244))
  * fixed crash in `use` elements with transformation (see [#64](https://github.com/svg-net/SVG/issues/64)) 
  * fixed overflow handling for view boxes (see [#279](https://github.com/svg-net/SVG/issues/279))
  * bounds in path based elements did not consider transformations (see [#281](https://github.com/svg-net/SVG/issues/281))
