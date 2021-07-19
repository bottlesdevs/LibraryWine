# LibraryWine
C# library for interacting with Wine

[![CodeFactor](https://www.codefactor.io/repository/github/bottlesdevs/librarywine/badge)](https://www.codefactor.io/repository/github/bottlesdevs/librarywine)
[![NuGet](https://img.shields.io/nuget/v/LibraryWine.svg?style=square&label=nuget)](https://www.nuget.org/packages/LibraryWine/)

> This library is a work in progress.

## Usage
```c#
using Bottles.LibraryWine;
// ..
var wine = new Wine(
    winePath: "/path/to/wine", // folder
    winePrefixPath: "/path/to/wineprefix", // empty or existing
    verboseLevel: Wine.VerboseLevels.N_ALL,
    isProton: false
);
// ..

// Terminal selection
wine.Terminal = Wine.SupportedTerminals.GNOME_TERMINAL; // default: NONE (cli)

// Working with processes
var processes = WineTools.GetRunningProcesses(ref wine); // return List<Models.RegisterKeyValue>
foreach (var p in processes)
{
    Console.WriteLine($"Name: {p.Name}, Pid: {p.Pid}, Parent: {p.ParentPid}");
    if (p.Name.Contains("winecfg"))
        p.Kill();
}

// Boot Management
WineTools.WineBootInit(ref wine);
WineTools.WineBootEndSession(ref wine);
WineTools.WineBootForce(ref wine);
WineTools.WineBootKill(ref wine);
WineTools.WineBootRestart(ref wine);
WineTools.WineBootShutdown(ref wine);
WineTools.WineBootUpdate(ref wine);

// Executables
WineTools.RunExe(
    ref wine,
    executable: "/path/to/exe",
    arguments: "--test" // optional
);
WineTools.RunMsi(ref wine, "/path/to/exe");
WineTools.RunBat(ref wine, "/path/to/exe");

// Register
WineRegister.GetKeyValues(
    ref wine, 
    key: "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion"
);
WineRegister.AddKey(
    ref wine,
    key: "HKEY_CURRENT_USER\\Software\\Wine\\Explorer",
    value: "Desktop",
    data: "Default",
    kType: WineRegister.KTypes.REG_SZ
);
WineRegister.DeleteKey(
    ref wine,
    key: "HKEY_CURRENT_USER\\Software\\Wine\\Explorer",
    value: "Desktop"
);

// Mapped keys
WineRegister.SetWindowsVersion(
    ref: wine,
    version: WineRegister.WindowsVersions.WIN10
);
WineRegister.SetAppDefaultVersion(
    ref: wine,
    executable: "steam.exe",
    version: WineRegister.WindowsVersions.WIN7
);
WineRegister.SetVirtualDesktop(
    ref: wine,
    enabled: true,
    resolution: "1920x1080"
);
WineRegister.SetWindowDecorations(
    ref: wine,
    enabled: true
);
WineRegister.SetWindowManaged(
    ref wine,
    enabled: true
);
WineRegister.SetFullscreenMouseCapture(
    ref wine,
    enabled: true
);
WineRegister.SetDpi(
    ref wine,
    dpi: 96
);

// DLL Overrides
WineRegister.GetDllOverrides(ref wine);
WineRegister.AddDllOverride(
    ref wine,
    dll: "ucrtbase",
    type: WineRegister.DllOverrideTypes.BUILTIN_NATIVE
);
WineRegister.DeleteDllOverride(
    ref wine,
    dll: "ucrtbase"
);
```