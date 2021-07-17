using System;

namespace Bottles.LibraryWine
{
    public static class WineTools
    {
        public static string GetVersion(ref Wine wine)
        {
            var result = wine.ExecCommand("--version", getOutput: true);
            if (result is bool)
                return result.ToString();

            return (string)result;
        }

        public static void WineCfg(ref Wine wine)
        {
            wine.ExecCommand("winecfg");
        }

        public static void WineDbg(ref Wine wine)
        {
            wine.ExecCommand("winedbg", useTerminal: true);
        }

        public static void WineCmd(ref Wine wine)
        {
            wine.ExecCommand("cmd", useTerminal: true);
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
    }
}
