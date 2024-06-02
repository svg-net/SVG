![W3C SVG Logo](https://www.w3.org/Icons/SVG/svg-logo-v.png)
# SVG.NET[![NuGet version](https://badge.fury.io/nu/svg.svg)](https://badge.fury.io/nu/svg) [![Gitter](https://badges.gitter.im/vvvv/SVG.svg)](https://gitter.im/vvvv/SVG?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge) ![Testsuite](https://github.com/svg-net/SVG/workflows/Testsuite/badge.svg?branch=master) ![DocBuild](https://github.com/svg-net/SVG/workflows/DocBuild/badge.svg?branch=master)

SVG.NET is a C# library to read, write and render SVG 1.1 images in applications based on the .NET framework.

It is compatible with platforms implementing .NET Standard 2.0 and works under Windows, Linux, and macOS
(the latter two with some limitations with respect to rendering).

SVG.NET is available as a NuGet package and can be referenced by any .NET application supporting .NET Standard 2.0.

## Documentation
For information on installation and usage of the library, and for release notes please check the [documentation pages](https://svg-net.github.io/SVG/).
Note that the documentation is very rudimentory - any help to improve it is greatly appreciated!

## Contributing
This project is in need of contributors.
Assistance in areas such as code reviews and testing is particularly needed. 
If you can contribute, your help would be invaluable.
For those interested in leading or contributing to specific initiatives like the codebase segmentation, please let us know.

Please feel free to fork the repository and open pull requests for any fix, improvement or feature you want to add.
You may check the [contributing guide](https://github.com/svg-net/SVG/blob/master/CONTRIBUTING.md) for more information on how to do this. 

## History
This project is a public fork of the SVG.NET C# library originally created by Microsoft on (now defunct) CodePlex. 
It started out as a private fork by [vvvv](https://vvvv.org) with some minor modification to enable the writing of proper SVG strings.
After almost two years of fixes and improvements the company decided to share the codebase with the public in order to improve it even further.
In 2021, the repository has been transferred from the company organization `vvvv` to the new organization `svg-net` (e.g. SVG.NET). 

## Projects using the library

* [vvvv](https://vvvv.org) a hybrid visual/textual live-programming environment for easy prototyping and development.
* [Posh](https://github.com/vvvv/Posh) a windowing/interaction/drawing layer for C#/.NET desktop applications with their GUI in a browser. 
* [Timeliner](https://github.com/vvvv/Timeliner) a Posh based timeline that can be controlled by and sends out its values via OSC.
* [Chordious](https://chordious.com) a fretboard diagram generator for fretted stringed instruments.
* [HttpMaster](https://www.httpmaster.net) a Windows tool for HTTP testing and debugging.

If you want your project in this list, send a pull request on this file, or a link and short description to tebjan (at) vvvv.org.

## License
Licensed under the MS-PL license.

This project has dependencies on other open-source projects. These projects are referenced via NuGet packages and might be subject to different licenses.

|Project|Author|Sources|License|
|--------|-----|---|---------|
|ExCSS|Tyler Brinks (@tylerbrinks)|[GitHub](https://github.com/TylerBrinks/ExCSS)|[MIT](https://github.com/TylerBrinks/ExCSS/blob/master/license.txt)|
