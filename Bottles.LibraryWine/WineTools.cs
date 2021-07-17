using System;

namespace Bottles.LibraryWine
{
    public static class WineTools
    {
        public static string GetVersion(ref Wine wine)
        {
            var result = wine.ExecCommand("--version", getOutput: true);
            if(result is bool)
                return result.ToString();

            return (string)result;
        }
    }
}