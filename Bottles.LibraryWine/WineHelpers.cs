using System;
using System.IO;
using System.Collections.Generic;

namespace Bottles.LibraryWine
{
    public static class WineHelpers
    {
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