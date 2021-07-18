using System;

namespace Bottles.LibraryWine
{
    public static class WineTools
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

#region Tools
        public static void WineCfg(ref Wine wine)
        {
            wine.ExecCommand("winecfg");
        }

        public static void WineDbg(ref Wine wine)
        {
            wine.ExecCommand("winedbg", useTerminal: true);
        }

        public static object WineCmd(ref Wine wine, string command = "", bool getOutput = false)
        {
            if (command == "")
                command = "cmd";
            else
                command = $"cmd /c {command}";
#if FLATPAK
            if (!getOutput)
                command = $"wineconsole {command}";
#endif
            var result = wine.ExecCommand(command, useTerminal: true, getOutput: getOutput);
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
