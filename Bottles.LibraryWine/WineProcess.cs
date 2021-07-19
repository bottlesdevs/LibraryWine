using System;
using System.IO;
using System.Collections.Generic;

namespace Bottles.LibraryWine
{
    public class WineProcess
    {
        public string Name { get; set; }
        public string Pid { get; set; } 
        public string ParentPid { get; set; }
        private Wine _Wine { get; set; }

        private List<string> ProtectedProcesses = new List<string> {
            "explorer.exe",
            "services.exe",
            "rpcss.exe",
            "svchost.exe",
            "winedevice.exe",
            "plugplay.exe",
            "winedbg.exe",
            "conhost.exe"
        };

        public WineProcess(ref Wine wine, string name, string pid, string parentPid="")
        {
            this.Name = name;
            this.Pid = GetHexPid(pid);
            this._Wine = wine;

            if (parentPid != "")
                this.ParentPid = parentPid;
        }

        private string GetHexPid(string pid)
        {
            return string.Format("{0:X}", pid);
        }

        public void Kill()
        {
            string sequence = "<< END_OF_INPUTS\n" +
                $"attach {Pid}\n" +
                "kill\\\n" + 
                "quit\\\n" + 
                "END_OF_INPUTS";
                
            if (!ProtectedProcesses.Contains(Name))
            {
                _Wine.ExecCommand(
                    "winedbg",
                    sequence
                );
            }
        }
    }
}