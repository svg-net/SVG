# SVG.NET

Public fork of the C# SVG rendering library on CodePlex (out of service). 

This started out as a minor modification to enable the writing of proper SVG strings. But now after almost two years we have so many fixes and improvements that we decided to share our current codebase to the public in order to improve it even further.

So please feel free to fork it and open pull requests for any fix, improvement or feature you add. 
You may check the [contributing guide](https://github.com/svg-net/SVG/blob/master/CONTRIBUTING.md) for more information on how to do this. 

## Downloads
The SVG-NET is available as [NuGet Package](https://www.nuget.org/packages/svg).
The respository includes
* **Source:** The main source code directory.
* **Generators:** A Roslyn source code generator for generating the SVG factory elements.
* **Tests:** NUnit based test unit project, benchmark project and other test tools, and
* **Samples:** For sample applications
* **Docs:** For the documentations (in markdown format).

## Projects using the library

* [vvvv](https://vvvv.org) a hybrid visual/textual live-programming environment for easy prototyping and development.
* [Posh](https://github.com/vvvv/Posh) a windowing/interaction/drawing layer for C#/.NET desktop applications with their GUI in a browser. 
* [Timeliner](https://github.com/vvvv/Timeliner) a Posh based timeline that can be controlled by and sends out its values via OSC.
* [Chordious](https://chordious.com) a fretboard diagram generator for fretted stringed instruments.
* [HttpMaster](https://www.httpmaster.net) a Windows tool for HTTP testing and debugging.

If you want your project in this list, send me a pull request on this file or link + short description to tebjan (at) vvvv.org

## License
Licensed under the [MS-PL license](https://github.com/svg-net/SVG?tab=MS-PL-1-ov-file),

This project has dependencies on other open-source projects. These projects are referenced via NuGet packages and might be subject to different licenses.

|Project|Author|Sources|License|
|--------|-----|---|---------|
|ExCSS|Tyler Brinks (@tylerbrinks)|[GitHub](https://github.com/TylerBrinks/ExCSS)|[MIT](https://github.com/TylerBrinks/ExCSS/blob/master/license.txt)|
