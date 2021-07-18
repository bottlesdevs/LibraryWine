using System;
using System.IO;
using System.Collections.Generic;

namespace Bottles.LibraryWine
{
    public static class WineHelpers
    {
        public static Dictionary<string, Models.WindowsVersion> WindowsVersions = new Dictionary<string, Models.WindowsVersion>
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

        public static bool ValidateWinePath(string winePath)
        {
            if (string.IsNullOrEmpty(winePath))
                return false;
            if (!Directory.Exists(winePath))
                return false;

            foreach (string dir in new string[] { "share", "bin", "lib" })
            {
                if (!Directory.Exists(Path.Combine(winePath, dir)))
                    return false;
            }
            return true;
        }

        public static bool ValidateWinePrefixPath(string winePrefixPath)
        {
            if (string.IsNullOrEmpty(winePrefixPath))
                return false;
            if (!Directory.Exists(winePrefixPath))
            {
                try
                {
                    Directory.CreateDirectory(winePrefixPath);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }


        public static bool CheckArchCompatibility()
        {
            return false;
        }
    }
}