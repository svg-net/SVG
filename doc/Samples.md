Sample code provided with the SVG sources
=========================================

There are a few code samples for SVG usage in the folder _Samples_ under the project root.
All of these samples are included in the solution provided with the source code for easier usage.
The following samples are currently available:

SVGViewer
---------
This is a form-based .NET application that is able to read an SVG file and display it, along with it's
source XML. It also allows to edit the XML, so it can be used to see how arbitrary SVG code will be 
displayed using the library. 

Apart from being useful as a tool, the source code shows how to load an SVG image, render it, and
save it in another format (PNG in this case). You will find the relevant code in _SvgViewer.cs_

SvgSample
---------
This is a small command line application that reads in a sample SVG, changes the style of one of it's
elements, and saves it to a PNG file. It shows how to access and alter an element of the SVG source tree.


XMLOutputTester
---------------
This is a simple form-based application that shows how to create a new SVG document from scratch (in this
example representing a filled circle), display it's XML source and save it to a file.


Entities
--------
Another small command line application that loads an example file that references entities not present
in the file, and provides these entities with matching styles dynamically in the `SvgDocument.Open` call.

SvgConsole
----------
This is a simple command-line application to convert one or many input SVG files to PNG images. It shows how to open a SVG document and create bitmap, and how to save resulting bitmap to a PNG file.

---
As you can see, there are currently very few code samples. We encourage you to add your own sample code
in pull requests, or provide code snippets that can be made into samples.

This project lives from public contributions, and we appreciate any help!

---
