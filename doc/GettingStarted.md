## Getting the library

Depending on the way you want to work with the library you can consume the SVG library through Nuget, or roll your own binary from the sources or a personal fork of the sources. 

### Which version to choose?
There are 2 major supported versions at the moment. A version 2.4.* and a version 3.0.*. The 2.4.* is a .NET Framework specific version (non .NET Core compatible) which can be considered rather stable for use within a .NET project. The 3.0 version is a more recent version with added .NET Core compatibility and the possibility to run the package in .NET Core projects on Windows, Linux and Mac. The .NET framework compatibility is also maintained, which allows you to use the package in the regular .NET framework (version 3.5 and above). The 3.0 version also contains some bugfixes which are not (yet) in the 2.4 version, but this is a limited set and if required these fixes can be merged into the 2.4 version on request.

If you are going to use the package for the first time, your best bet is to go for the 3.0 version, this allows you for maximum flexibility and portability. If you are already using version 2.4 or use other libraries depending on the 2.4 versions you can also upgrade, but there is a possibility that you might encounter compatibility issues/errors. The library is under unit-tests, but a 100% guaranteed equality between the 3.0 and 2.4 versions cannot be given. If you are working with the .NET framework version you are likely to encounter no big issues, but if you switch to the .NET core version or switch platforms (e.g. to Mac or Linux) you need to test and validate the calling code to be sure everything keeps working as expected.

### Installing through Nuget
The library is available as Nuget in the public nuget feed (https://www.nuget.org/packages/Svg/). Depending on your development stack you can add it to your code base.

For Visual Studio 2013 - 2019 you can add it by searching it in the Nuget wizard or by using the following command in the Nuget Console.
```
Install-Package Svg
```

When using the dotnet-cli, Visual Studio Code or other editors you can add the package to your solution by running the following command in the terminal/console:
```
dotnet add package Svg
```

If you would like to add a specific version you can add the --version parameter in your command or pick a specific version in the wizard. If you want to use pre-release versions in Visual Studio you need to check the box regarding pre-release packages to be able to select pre-release versions.

### Rolling your own version
If you would like to roll your own version you can download the sources through GitHub, clone the repository to your local machine or create a fork in your own account and download/clone this to your machine. This will give you more flexibility in choosing the target framework(s) and compiler flags.

Once you downloaded the sources you can use the IDE of your choice to open the solution file (Svg.sln) or the Svg library project (Svg.csproj) and use your IDE to compile the version you would like to have.

If you would like to use the dotnet-cli to build the sources you can use the following command in the Sources/ folder to build the library for .NET Core 2.2 with the compiler setting for release:
```
dotnet build -c release -f netcoreapp2.2 Svg.csproj
```
This will put the output in the bin/Release/netcoreapp2.2/ folder.

## Special instructions for Mac and Linux
The library depends on GDI+ (see: https://github.com/vvvv/SVG/wiki/Q&A#im-getting-a-svggdipluscannotbeloadedexception-if-running-under-linux-or-macos) for rendering. .NET Core does not support GDI+ out of the box for non-Windows systems. For Mac and Linux you need to add a special compatibility package. This is not included by default in the packages, since this will break rendering on Windows systems.

I you distribute your application as platform independent you might want to add the following instructions (or a reference to this guide) in your installation instructions to aid Mac and Linux users that want to utilize your application/library.

### Linux (Ubuntu)
For Linux you need to install libgdiplu from the quamotion/ppa feed on your machine/container:
```
sudo add-apt-repository ppa:quamotion/ppa
sudo apt-get update
sudo apt-get install -y libgdiplus
```

### MacOs
Mac does not require you to install a system-wide package, but allows you to use a compatibility package that is included in the application. This package can be included in the SVG component if you roll your own version from source, this can be achieved by altering the Svg.csproj file and un-comment the following block of code:
```
<!-- 
<ItemGroup Condition="'$(TargetFramework)' == 'netcoreapp2.2'">
  <PackageReference Include="runtime.osx.10.10-x64.CoreCompat.System.Drawing" Version="5.8.64" />
</ItemGroup> 
-->
```
This will link the CoreCompat package in the project, if you make a project reference to the Svg.csproject the consuming application/library will automatically also include the CoreCompat package.

If you are not building from the source or do not want to make the Svg library dependent on the CoreCompat package you can add the reference in the "ultimate" consumer of the package (the application that will be executed), by the following command in a terminal/console within the consuming application folder:
```
dotnet add package runtime.osx.10.10-x64.CoreCompat.System.Drawing
```

## Linking the library in your application
If you installed or build the library, it's time to add it to your application. If you used the Nuget approach, the reference should already be set correctly (please note that for Mac and Linux the compatibility tooling/package needs to be done manually).

If you rolled your own version you can link the .csproj to your own project via your IDE. If you want to do it through the dotnet-cli you can run:
```
dotnet add reference SVG/sources/Svg.csproj
```
(where SVG is the root folder you downloaded the sources to). This approach will also take over all references required to the target project (e.g. when you added the corecompat package for Mac). This will also compile the Svg sources when you build your own project, which might be useful if you plan to make changes in the Svg project yourself.

If you don't want to reference the project, you can get the Svg.dll file from the outpot folders after you compiled the project with the steps outlined above and reference the Svg.dll file. The Svg library does not utilize other external references and by only using the Svg.dll file you will be able to use the library. However please keep in mind that the Mac and Linux versions require additional tooling/packages.

## Using the library (examples)
This part will be extended in the future, for now please refer to the Q&A for examples of how to use the library: https://github.com/vvvv/SVG/wiki/Q&A

## Troubleshooting
If you encounter any problems or difficulties, please refer to the Q&A part of the wiki (https://github.com/vvvv/SVG/wiki/Q&A). If the Q&A does not solve your problem, please open a ticket with your request.