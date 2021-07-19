using System;
using System.Collections.Generic;

namespace Bottles.LibraryWine
{
    public class WineRegister
    {
        public enum KTypes
        {
            REG_SZ,
            REG_DWORD,
            REG_MULTI_SZ,
            REG_BINARY,
            REG_EXPAND_SZ,
            REG_NONE
        }
        private static List<string> DllOverrideTypesStrings = new List<string>()
        {
            "builtin", "native", "builtin,native", "native,builtin"
        };
        public enum DllOverrideTypes
        {
            BUILTIN = 0,
            NATIVE = 1,
            BUILTIN_NATIVE = 2,
            NATIVE_BUILTIN = 3
        }
        private static Dictionary<string, Models.WindowsVersion> WindowsVersionsModels = new Dictionary<string, Models.WindowsVersion>
        {
            { "win10", new Models.WindowsVersion()
                {
                    ProductName = "Microsoft Windows 10",
                    CSDVersion = "",
                    CurrentBuild = "17763",
                    CurrentBuildNumber = "17763",
                    CurrentVersion = "10.0"
                }
            },
            { "win81", new Models.WindowsVersion()
                {
                    ProductName = "Microsoft Windows 8.1",
                    CSDVersion = "",
                    CurrentBuild = "9600",
                    CurrentBuildNumber = "9600",
                    CurrentVersion = "6.3"
                }
            },
            { "win8", new Models.WindowsVersion()
                {
                    ProductName = "Microsoft Windows 8",
                    CSDVersion = "",
                    CurrentBuild = "9200",
                    CurrentBuildNumber = "9200",
                    CurrentVersion = "6.2"
                }
            },
            { "win7", new Models.WindowsVersion()
                {
                    ProductName = "Microsoft Windows 7",
                    CSDVersion = "Service Pack 1",
                    CurrentBuild = "7601",
                    CurrentBuildNumber = "7601",
                    CurrentVersion = "6.1"
                }
            },
            { "win2008r2", new Models.WindowsVersion()
                {
                    ProductName = "Microsoft Windows 2008 R2",
                    CSDVersion = "Service Pack 1",
                    CurrentBuild = "7601",
                    CurrentBuildNumber = "7601",
                    CurrentVersion = "6.1"
                }
            },
            { "win2008", new Models.WindowsVersion()
                {
                    ProductName = "Microsoft Windows 2008",
                    CSDVersion = "Service Pack 2",
                    CurrentBuild = "6002",
                    CurrentBuildNumber = "6002",
                    CurrentVersion = "6.0"
                }
            },
            { "winxp", new Models.WindowsVersion()
                {
                    ProductName = "Microsoft Windows XP",
                    CSDVersion = "Service Pack 2",
                    CurrentBuild = "3790",
                    CurrentBuildNumber = "3790",
                    CurrentVersion = "5.2"
                }
            }
        };
        public enum WindowsVersions
        {
            WIN10,
            WIN81,
            WIN8,
            WIN7,
            WIN2008R2,
            WIN2008,
            WINXP
        }
#region Management
        public static List<Models.RegisterKeyValue> GetKeyValues(ref Wine wine, string key)
        {
            List<Models.RegisterKeyValue> keyValues = new List<Models.RegisterKeyValue>();

            var result = wine.ExecCommand(
                $"reg query {key}",
                getOutput: true
            );

            if (result is bool)
                return keyValues;
                            
            result = result.ToString().Replace("  ", " ");

            string[] output = result.ToString().Split("\n");

            foreach (var line in output)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.Contains("REG_SZ") ||
                    line.Contains("REG_DWORD") ||
                    line.Contains("REG_MULTI_SZ") ||
                    line.Contains("REG_BINARY") ||
                    line.Contains("REG_EXPAND_SZ")) {}
                else
                    continue;

                string[] items = line.Substring(2).Split("  ");

                try
                {
                    keyValues.Add(new Models.RegisterKeyValue()
                    {
                        Name = items[0],
                        Type = (KTypes)Enum.Parse(typeof(KTypes), items[1]),
                        Data = items[2]
                    });
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return keyValues;
        }
        
        public static void AddKey(ref Wine wine, string key, string value, string data, KTypes kType)
        {
            wine.ExecCommand(
                $"reg add {key} /v {value} /d {data} /t {kType} /f");
        }
        
        public static void DeleteKey(ref Wine wine, string key, string value)
        {
            wine.ExecCommand($"reg delete {key} /v {value} /f");
        }
#endregion

#region Mapped Keys

        public static void SetWindowsVersion(ref Wine wine, WindowsVersions version)
        {
            Models.WindowsVersion windowsVersion = WindowsVersionsModels[version.ToString().ToLower()];
            AddKey(
                ref wine,
                "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion",
                "ProductName",
                windowsVersion.ProductName,
                KTypes.REG_SZ
            );
            AddKey(
                ref wine,
                "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion",
                "CSDVersion",
                windowsVersion.CSDVersion,
                KTypes.REG_SZ
            );
            AddKey(
                ref wine,
                "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion",
                "CurrentBuild",
                windowsVersion.CurrentBuild,
                KTypes.REG_SZ
            );
            AddKey(
                ref wine,
                "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion",
                "CurrentBuildNumber",
                windowsVersion.CurrentBuildNumber,
                KTypes.REG_SZ
            );
            AddKey(
                ref wine,
                "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion",
                "CurrentVersion",
                windowsVersion.CurrentVersion,
                KTypes.REG_SZ
            );
        }

        public static void SetAppDefaultVersion(ref Wine wine, string executable, WindowsVersions version)
        {
            AddKey(
                ref wine,
                $"HKEY_CURRENT_USER\\Software\\Wine\\AppDefaults\\{executable}",
                "Version",
                version.ToString().ToLower(),
                KTypes.REG_SZ
            );
        }

        public static void SetVirtualDesktop(ref Wine wine, bool enabled, string resolution)
        {
            if (enabled)
            {
                AddKey(
                    ref wine,
                    "HKEY_CURRENT_USER\\Software\\Wine\\Explorer",
                    "Desktop",
                    "Default",
                    KTypes.REG_SZ
                );
                AddKey(
                    ref wine,
                    "HKEY_CURRENT_USER\\Software\\Wine\\Explorer\\Desktops",
                    "Desktop",
                    resolution,
                    KTypes.REG_SZ
                );
            }
            else
            {
                DeleteKey(
                    ref wine,
                    "HKEY_CURRENT_USER\\Software\\Wine\\Explorer",
                    "Desktop"
                );
            }
        }

        public static void SetWindowDecorations(ref Wine wine, bool enabled)
        {
            string status = enabled ? "Y" : "N";

            AddKey(
                ref wine,
                "HKEY_CURRENT_USER\\Software\\Wine\\X11 Driver",
                "Decorated",
                status,
                KTypes.REG_SZ
            );
        }

        public static void SetWindowManaged(ref Wine wine, bool enabled)
        {
            string status = enabled ? "Y" : "N";
            AddKey(
                ref wine,
                "HKEY_CURRENT_USER\\Software\\Wine\\X11 Driver",
                "Managed",
                status,
                KTypes.REG_SZ
            );
        }

        public static void SetFullscreenMouseCapture(ref Wine wine, bool enabled)
        {
            string status = enabled ? "Y" : "N";
            AddKey(
                ref wine,
                "HKEY_CURRENT_USER\\Software\\Wine\\X11 Driver",
                "GrabbFullscreen",
                status,
                KTypes.REG_SZ
            );
        }

        public static void SetDpi(ref Wine wine, int dpi)
        {
            AddKey(
                ref wine,
                "HKEY_CURRENT_USER\\Control#Panel\\Desktop",
                "LogPixels",
                dpi.ToString(),
                KTypes.REG_DWORD
            );
        }
#endregion

#region DllOverrides
        public static List<Models.RegisterKeyValue> GetDllOverrides(ref Wine wine)
        {
            List<Models.RegisterKeyValue> results = GetKeyValues(
                ref wine,
                "HKEY_CURRENT_USER\\Software\\Wine\\DllOverrides"
            );
            return results;
        }

        public static void AddDllOverride(ref Wine wine, string dll, DllOverrideTypes type)
        {
            AddKey(
                ref wine,
                "HKEY_CURRENT_USER\\Software\\Wine\\DllOverrides",
                dll,
                DllOverrideTypesStrings[(int)type],
                KTypes.REG_SZ
            );
        }

        public static void DeleteDllOverride(ref Wine wine, string dll)
        {
            DeleteKey(
                ref wine,
                "HKEY_CURRENT_USER\\Software\\Wine\\DllOverrides",
                dll
            );
        }
#endregion
    }
}