using System;
using System.Collections.Generic;

namespace Bottles.LibraryWine
{
    public class WineTools
    {

#region Informations
        public static string GetVersion(ref Wine wine)
        {
            var result = wine.ExecCommand("--version", getOutput: true);
            if (result is bool)
                return result.ToString();

            return (string)result;
        }
#endregion

#region Boot
        private static void WineBoot(ref Wine wine, Wine.BootStates state, bool silent = true)
        {
            string stateFlag = wine.BootStatesStrings[(int)state];
            Dictionary<string, string> envVars = new Dictionary<string, string>();

            if (silent)
                envVars.Add("DISPLAY", ":3.0");

            wine.ExecCommand($"wineboot {stateFlag}", envVars: envVars);
        }

        public static void WineBootEndSession(ref Wine wine, bool silent = true)
        {
            WineBoot(ref wine, Wine.BootStates.END_SESSION, silent);
        }
        
        public static void WineBootForce(ref Wine wine, bool silent = true)
        {
            WineBoot(ref wine, Wine.BootStates.FORCE, silent);
        }

        public static void WineBootInit(ref Wine wine, bool silent = true)
        {
            WineBoot(ref wine, Wine.BootStates.INIT, silent);
        }

        public static void WineBootKill(ref Wine wine, bool silent = true)
        {
            WineBoot(ref wine, Wine.BootStates.KILL, silent);
        }

        public static void WineBootRestart(ref Wine wine, bool silent = true)
        {
            WineBoot(ref wine, Wine.BootStates.RESTART, silent);
        }

        public static void WineBootShutdown(ref Wine wine, bool silent = true)
        {
            WineBoot(ref wine, Wine.BootStates.SHUTDOWN, silent);
        }

        public static void WineBootUpdate(ref Wine wine, bool silent = true)
        {
            WineBoot(ref wine, Wine.BootStates.UPDATE, silent);
        }
#endregion

#region Tools
        public static void RunExe(ref Wine wine, string executable, string arguments = "")
        {
            wine.ExecCommand(executable, arguments);
        }

        public static void RunMsi(ref Wine wine, string executable, string arguments = "")
        {
            string command = $"msiexec /i {executable}";
            wine.ExecCommand(command, arguments);
        }

        public static void RunBat(ref Wine wine, string executable, string arguments = "")
        {
            WineCmd(ref wine, executable, arguments);
        }

        public static void WineCfg(ref Wine wine)
        {
            wine.ExecCommand("winecfg");
        }

        public static void WineDbg(ref Wine wine)
        {
            wine.ExecCommand("winedbg", useTerminal: true);
        }

        public static object WineCmd(ref Wine wine, string command = "", string arguments = "", bool getOutput = false)
        {
            if (command == "")
                command = "cmd";
            else
                command = $"cmd /c {command}";
#if FLATPAK
            if (!getOutput)
                command = $"wineconsole {command}";
#endif
            var result = wine.ExecCommand(
                command, 
                useTerminal: true, 
                arguments: arguments, 
                getOutput: getOutput
            );
            return result;
        }

        public static void WineTaskmgr(ref Wine wine)
        {
            wine.ExecCommand("taskmgr");
        }

        public static void WineControl(ref Wine wine)
        {
            wine.ExecCommand("control");
        }

        public static void WineUninstaller(ref Wine wine)
        {
            wine.ExecCommand("uninstaller");
        }

        public static void WineRegedit(ref Wine wine)
        {
            wine.ExecCommand("regedit");
        }
#endregion
    }
}
