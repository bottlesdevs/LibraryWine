using System;

namespace Bottles.LibraryWine
{
    public class WindowsVersion
    {
        public string ProductName { get; set; }
        public string CSDVersion { get; set; }
        public string CurrentBuild { get; set; }
        public string CurrentBuildNumber { get; set; }
        public string CurrentVersion { get; set; }

        public WindowsVersion(string productName, string csdVersion, string currentBuild, string currentBuildNumber, string currentVersion)
        {
            this.ProductName = productName;
            this.CSDVersion = csdVersion;
            this.CurrentBuild = currentBuild;
            this.CurrentBuildNumber = currentBuildNumber;
            this.CurrentVersion = currentVersion;
        }
    }
}