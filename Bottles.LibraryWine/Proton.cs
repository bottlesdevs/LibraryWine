using System;
using System.IO;

namespace Bottles.LibraryWine
{
    public class Proton : Wine
    {
        public Proton(
            string winePath, 
            string winePrefixPath, 
            VerboseLevels verboseLevel = VerboseLevels.N_ALL) : base(winePath, winePrefixPath, verboseLevel, true)
        {
            if(Directory.Exists(Path.Combine(winePath, "dist")))
                winePath = Path.Combine(winePath, "dist");
            else if (Directory.Exists(Path.Combine(winePath, "files")))
                winePath = Path.Combine(winePath, "files");
            else
                throw new Exception("Proton Path is not valid");

            if (!WineHelpers.ValidateWinePath(winePath))
                throw new Exception("Proton Path is not valid");
        }

        public void Something()
        {
            // show proton config window
        }
    }
}