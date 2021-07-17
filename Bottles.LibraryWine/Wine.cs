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

        // create enum for verbose levels (0, 1, 2, 3)
        public enum VerboseLevels
        {
            N_All = 0,
            N_WARN_Y_ALL = 1,
            FIXME_N_ALL = 2,
            Y_ALL = 3
        }

        
        public Wine(string winePath, string winePrefixPath, int verboseLevel=0)
        {
            this.WinePath = winePath;
            this.WinePrefixPath = winePrefixPath;

            if (Enum.IsDefined(typeof(VerboseLevels), verboseLevel))
                this.VerboseLevel = verboseLevel;
            else
                this.VerboseLevel = 3;     

            
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