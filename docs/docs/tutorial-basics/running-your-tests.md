---
sidebar_position: 3
---

# Running your tests

As TUnit is built on-top of the newer Microsoft.Testing.Platform, and combined with the fact that TUnit tests are source generated, running your tests is available in a variety of ways. 

## [dotnet run](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-run)

For a simple execution of a project, `dotnet run` is the preferred method, allowing easier passing in of command line flags.

```powershell
cd 'C:/Your/Test/Directory'
dotnet run -c Release --report-trx --coverage
```

## [dotnet test](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test)

`dotnet test` requires any command line flags to be specified as application arguments, meaning after a `--` - Otherwise you'll get an error about unknown switches.

```powershell
cd 'C:/Your/Test/Directory'
dotnet test -c Release -- --report-trx --coverage
```

## [dotnet exec](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet)

If your test project has already been built, you can use `dotnet exec` or just `dotnet` with the `.dll` path

```powershell
cd 'C:/Your/Test/Directory/bin/Release/net8.0'
dotnet exec YourTestProject.dll --report-trx --coverage
```

or

```powershell
cd 'C:/Your/Test/Directory/bin/Release/net8.0'
dotnet YourTestProject.dll --report-trx --coverage
```

## Published Test Project

When you publish your test project, you'll be given an executable.
On windows this'll be a `.exe` and on Linux/MacOS there'll be no extension.

This can be invoked directly and passed any flags.

```powershell
cd 'C:/Your/Test/Directory/bin/Release/net8.0/win-x64/publish'
./YourTestProject.exe --report-trx --coverage
```

# IDE Support

## Visual Studio
Visual Studio is supported on the Preview version currently. 

- Install the [latest preview version](https://visualstudio.microsoft.com/vs/preview/)
- Open Visual Studio and go to Tools > Manage Preview Features
- Enable "Use testing platform server mode"

![Visual Studio Settings](../../static/img/visual-studio.png)

## Rider
Rider is supported. 

The [Enable Testing Platform support](https://www.jetbrains.com/help/rider/Reference__Options__Tools__Unit_Testing__VSTest.html) option must be selected in Settings > Build, Execution, Deployment > Unit Testing > VSTest.

![Rider Settings](../../static/img/rider.png)

### Test Discovery Issue

There is a known issue where Rider may fail to discover tests when used with certain versions of TUnit (see [#576](https://github.com/thomhurst/TUnit/issues/576)). This problem has been observed across different TUnit versions in combination with Rider.

If you encounter this issue, try downgrading TUnit by minor versions (e.g., from 0.1.10xx to 0.1.9xx) and rebuild the test project between each version until the tests are successfully discovered.

As of this update, Rider 2024.2.6 has been confirmed to be compatible with TUnit 0.1.1082.

## VS Code
Visual Studio Code is supported.

- Install the extension Name: [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
- Go to the C# Dev Kit extension's settings
- Enable Dotnet > Test Window > Use Testing Platform Protocol

![Visual Studio Code Settings](../../static/img/visual-studio-code.png)