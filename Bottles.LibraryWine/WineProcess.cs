using System;
using System.IO;
using System.Collections.Generic;

namespace Bottles.LibraryWine
{
    public class WineProcess
    {
        private string Name { get; set; }
        private string Pid { get; set; } 
        private int ParentPid { get; set; }

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

        public WineProcess(string name, int pid, int parentPid=-1)
        {
            this.Name = name;
            this.Pid = GetHexPid(pid);

            if (parentPid > -1)
                this.ParentPid = parentPid;
        }

        private string GetHexPid(int pid)
        {
            return string.Format("{0:X}", pid);
        }

        public void Kill()
        {
            /*
                winedbg << END_OF_INPUTS\n\
                attach {self.pid}\n\
                kill\n\
                quit\n\
                END_OF_INPUTS
            */
        }
    }
}