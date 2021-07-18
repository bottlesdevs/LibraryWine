using System;
using System.Collections.Generic;

namespace Bottles.LibraryWine
{
    public class WineRegedit
    {
        public enum KTypes
        {
            REG_SZ,
            REG_DWORD,
            REG_MULTI_SZ,
            REG_BINARY,
            REG_EXPAND_SZ,
            REG_NONE
        }

        public static object GetKeyValues(string key)
        {
            return true;
        }
        
        public static bool AddKey(string key, string value, string data, KTypes type)
        {
            return true;
        }
        
        public static bool DeleteKey(string key, string value)
        {
            return true;
        }
    }
}