using System;
using System.Collections.Generic;

namespace Bottles.LibraryWine
{
    public class WineRegister
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
        
        public static bool GetKeyValues(ref Wine wine, string key)
        {
            var result = wine.ExecCommand($"reg query '{key}' /f");

            if (result is bool)
                return (bool)result;
            return false;
        }
        
        public static bool AddKey(ref Wine wine, string key, string value, string data, KTypes kType)
        {
            var result = wine.ExecCommand(
                $"reg add '{key}' /v '{value}' /d '{data}' /t '{kType}' /f");
            
            if (result is bool)
                return (bool)result;
            return false;
        }
        
        public static bool DeleteKey(ref Wine wine, string key, string value)
        {
            var result = wine.ExecCommand($"reg delete '{key}' /v '{value}' /f");
            
            if (result is bool)
                return (bool)result;
            return false;
        }
    }
}