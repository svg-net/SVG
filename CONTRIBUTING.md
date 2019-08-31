# Contributing to SVG.NET

SVG.NET is a community project and lives from contributions by the community.
Any help to improve and evolve the library is very welcome! 
Contributions may include bug reports, bug fixes, new features, infrastructure enhancements, or 
documentation updates.

## How to contribute

### Reporting Bugs

If you found a bug in SVG.NET, you can [create an issue](https://help.github.com/articles/creating-an-issue/).
If you are able to build the library locally, please check, if the problem still exists in the
[master branch](https://github.com/vvvv/SVG) before filing the bug. 
If you can reproduce the problem, please provide enough information so that it can be reproduced by other developers.
This includes:
  * The Operating System
  * The used .NET / .NET Core version
  * An example image that shows the incorrect behavior, or a respective code snippet (preferably in the form of a failing unit test)
  * The stack trace in case of an unexpected exception
For better readability, you may use [markdown code formatting](https://help.github.com/articles/creating-and-highlighting-code-blocks/) for any included code.

### Proposing Enhancements

If you need a specific feature that is not yet implemented, you can also create a respective issue. 
Of course - implementing it yourself is the best chance to get it done! 

### Contributing Code

The preferred workflow for contributing code is to 
[fork](https://help.github.com/articles/fork-a-repo/) the [repository](https://github.com/vvvv/SVG) on GitHub, clone it, 
develop on a feature branch, and [create a pull request](https://help.github.com/articles/creating-a-pull-request-from-a-fork) when done.
There are a few things to consider for contributing code:
  * Please use the same coding style as in the rest of the code
  * Use spaces instead of tabs
  * Provide unit tests for bug fixes, or provide a test svg with a respective png counterpart showing the correct rendering 
    (refer to existing test images for naming conventions) 
  * Provide meaningful commit messages and/or PR comments
  * Check that the automatic tests on [AppVeyor](https://ci.appveyor.com/project/tebjan/svg) all pass for your pull request
  * Be ready to adapt your changes after a code review 
  * When copying existing code from other sources or repositories, please keep licensing in mind
  
### Contributing Documentation

This projects is in need of documentation - any help to add documentation infrastructure, 
inline documentation, how-tos or sample code is appreciated!
For specifics, please refer to the [issue related to documentation](https://github.com/vvvv/SVG/issues/401).

Thanks for taking the time to contribute to SVG.NET!
