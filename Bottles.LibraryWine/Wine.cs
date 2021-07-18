using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace Bottles.LibraryWine
{
    public class Wine
    {
        public string WinePath { get; }
        public string WinePrefixPath { get; }
        public VerboseLevels VerboseLevel { get; }
        public SupportedTerminals Terminal { get; set; }
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
        private Dictionary<string, string[]> SupportedTerminalsStrings = new Dictionary<string, string[]>()
        {
            {"NONE", new string[] {}},
            {"XTERM", new string[] {"xterm", "-e"}},
            {"KONSOLE", new string[] {"konsole", "-e"}},
            {"GNOME_TERMINAL", new string[] {"gnome-terminal", "--"}},
            {"XFCE4_TERMINAL", new string[] {"xfce4-terminal", "--command"}},
            {"MATE_TERMINAL", new string[] {"mate-terminal", "--command"}}
        };

        public enum SupportedTerminals
        {
            NONE = 0,
            XTERM = 1,
            KONSOLE = 2,
            GNOME_TERMINAL = 3,
            XFCE4_TERMINAL = 4,
            MATE_TERMINAL = 5
        }

        public Wine(
            string winePath,
            string winePrefixPath,
            VerboseLevels verboseLevel = VerboseLevels.N_ALL,
            bool isProton = false)
        {
            if (isProton)
            {
                if (Directory.Exists(Path.Combine(winePath, "dist")))
                    winePath = Path.Combine(winePath, "dist");
                else if (Directory.Exists(Path.Combine(winePath, "files")))
                    winePath = Path.Combine(winePath, "files");
                else
                    throw new Exception("Proton Path is not valid");
            }

            if (!WineHelpers.ValidateWinePath(winePath))
                if (isProton)
                    throw new Exception("Proton Path is not valid");
                else
                    throw new Exception("Wine Path is not valid");

            this.WinePath = winePath;
            this.WinePrefixPath = winePrefixPath;
            this.VerboseLevel = verboseLevel;
        }


        public object ExecCommand(
            string command,
            string arguments = "",
            Dictionary<string, string> envVars = null,
            bool getOutput = false,
            bool useTerminal = false,
            string workingDirectory = "")
        {
            if (workingDirectory == "")
                workingDirectory = this.WinePrefixPath;

            string fileName = $"{this.WinePath}/bin/wine64";
            string fileArguments = $"{command} {arguments}";

# if !FLATPAK
            if (useTerminal && Terminal != SupportedTerminals.NONE)
            {
                fileArguments = $"{SupportedTerminalsStrings[Terminal.ToString()][1]} {fileName} {fileArguments}";
                fileName = SupportedTerminalsStrings[Terminal.ToString()][0];
                Console.WriteLine($"Executing: {fileName} {fileArguments}");
            }
# endif

            var startInfo = new ProcessStartInfo() 
            { 
                FileName = fileName, 
                Arguments = fileArguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WorkingDirectory = workingDirectory
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
#if DEBUG
                Console.WriteLine(output);
#endif
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