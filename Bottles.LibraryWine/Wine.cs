using System;
using System.IO;
using System.Collections.Generic;

namespace Bottles.LibraryWine
{
    public class Wine
    {
        private string WinePath { get; set; }
        private string WinePrefixPath { get; set; }
        private int VerboseLevel { get; set; }

        private enum VerboseLevels
        {
            N_All = 0,
            N_WARN_Y_ALL = 1,
            FIXME_N_ALL = 2,
            Y_ALL = 3
        }

        private enum DllOverrideTypes
        {
            BUILTIN = 0,
            NATIVE = 1,
            BUILTIN_NATIVE = 2,
            NATIVE_BUILTIN = 3
        }

        private enum RegeditKTypes
        {
            REG_SZ = 0,
            REG_DWORD = 1,
            REG_MULTI_SZ = 2,
            REG_BINARY = 3,
            REG_EXPAND_SZ = 4,
            REG_NONE = 5
        }

        public Dictionary<string, WindowsVersion> WindowsVersions = new Dictionary<string, WindowsVersion>
        {
            { "win10", new WindowsVersion(
                productName: "Microsoft Windows 10",
                csdVersion: "",
                currentBuild: "17763",
                currentBuildNumber: "17763",
                currentVersion: "10.0"
            ) },
            { "win81", new WindowsVersion(
                productName: "Microsoft Windows 8.1",
                csdVersion: "",
                currentBuild: "9600",
                currentBuildNumber: "9600",
                currentVersion: "6.3"
            ) },
            { "win8", new WindowsVersion(
                productName: "Microsoft Windows 8",
                csdVersion: "",
                currentBuild: "9200",
                currentBuildNumber: "9200",
                currentVersion: "6.2"
            ) },
            { "win7", new WindowsVersion(
                productName: "Microsoft Windows 7",
                csdVersion: "Service Pack 1",
                currentBuild: "7601",
                currentBuildNumber: "7601",
                currentVersion: "6.1"
            ) },
            { "win2008r2", new WindowsVersion(
                productName: "Microsoft Windows 2008 R2",
                csdVersion: "Service Pack 1",
                currentBuild: "7601",
                currentBuildNumber: "7601",
                currentVersion: "6.1"
            ) },
            { "win2008", new WindowsVersion(
                productName: "Microsoft Windows 2008",
                csdVersion: "Service Pack 2",
                currentBuild: "6002",
                currentBuildNumber: "6002",
                currentVersion: "6.0"
            ) },
            { "winxp", new WindowsVersion(
                productName: "Microsoft Windows XP",
                csdVersion: "Service Pack 2",
                currentBuild: "3790",
                currentBuildNumber: "3790",
                currentVersion: "5.2"
            ) },
        };


        public Wine(string winePath, string winePrefixPath, int verboseLevel = 0)
        {
            this.WinePath = winePath;
            this.WinePrefixPath = winePrefixPath;
            this.VerboseLevel = 3;

            if (Enum.IsDefined(typeof(VerboseLevels), verboseLevel))
                this.VerboseLevel = verboseLevel;

            if (!ValidateWinePath())
                throw new Exception("Wine Path is not valid");
        }

        private bool ValidateWinePath()
        {
            if (string.IsNullOrEmpty(this.WinePath))
                return false;
            if (!Directory.Exists(this.WinePath))
                return false;

            foreach (string dir in new string[] { "share", "bin", "lib" })
            {
                if (!Directory.Exists(Path.Combine(this.WinePath, dir)))
                    return false;
            }
            return true;
        }
    }
}