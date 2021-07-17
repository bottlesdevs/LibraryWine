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
    }
}
