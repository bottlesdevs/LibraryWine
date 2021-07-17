using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Bottles.LibraryWine.Models;

namespace Bottles.LibraryWine
{
    public class Wine
    {
        private string WinePath { get; set; }
        private string WinePrefixPath { get; set; }
        private VerboseLevels VerboseLevel { get; set; }

        private List<string> VerboseLevelsString = new List<string>()
        {
            "-all", "-warn+all", "fixme-all", "+all"
        };
        public enum VerboseLevels
        {
            N_ALL = 0,
            N_WARN_Y_ALL = 1,
            FIXME_N_ALL = 2,
            Y_ALL = 3
        }

        private List<string> DllOverrideTypesString = new List<string>()
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

        public Wine(string winePath, string winePrefixPath, VerboseLevels verboseLevel = VerboseLevels.N_ALL)
        {
            if (!WineHelpers.ValidateWinePath(winePath))
                throw new Exception("Wine Path is not valid");

            this.WinePath = winePath;
            this.WinePrefixPath = winePrefixPath;
            this.VerboseLevel = verboseLevel;
        }


        public object ExecCommand(
            string command,
            string arguments = "",
            Dictionary<string, string> envVars = null,
            bool getOutput = false)
        {
            var startInfo = new ProcessStartInfo() 
            { 
                FileName = $"{this.WinePath}/bin/wine64", 
                Arguments = $"{command} {arguments}",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            startInfo.EnvironmentVariables["WINEPREFIX"] = WinePrefixPath;
            startInfo.EnvironmentVariables["WINEDEBUG"] = VerboseLevelsString[(int)VerboseLevel];

            if(envVars != null)
                foreach (var envVar in envVars
                    .Where(envVar => !(envVar.Key.ToLower() == "wineprefix" || 
                                       envVar.Key.ToLower() == "winedebug")))
                    {
                        startInfo.EnvironmentVariables[envVar.Key] = envVar.Value;
                    }
            
            var proc = new Process() { StartInfo = startInfo };
            
            try
            {
                proc.Start();

                var output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();

                Console.WriteLine(output);

                if(getOutput)
                    return output;

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERR: " + ex.Message);
                return false;
            }
        }
    }
}