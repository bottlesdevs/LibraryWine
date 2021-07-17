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

        public static bool AddKey(string key, string value, string data, KTypes type)
        {
            try
            {
                // try to add key
                
                return true;
            }
            catch (Exception ex)
            {
                // log/show ex.Message;

                return false;
            }
        }
    }
}