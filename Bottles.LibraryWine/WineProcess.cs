/* 
 * Copyright (c) 2021 Mirko Brombin <send@mirko.pm>.
 * Copyright (c) 2021 Bottles Developers <https://github.com/bottlesdevs>.
 * 
 * This program is free software: you can redistribute it and/or modify  
 * it under the terms of the GNU General Public License as published by  
 * the Free Software Foundation, version 3.
 *
 * This program is distributed in the hope that it will be useful, but 
 * WITHOUT ANY WARRANTY; without even the implied warranty of 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License 
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */
 
using System;
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
            return $"0x{pid}";
        }

        public void Kill()
        {
            string[] sequence = {
                $"attach {Pid}",
                "kill",
                "quit"
            };

            string output = "";
            
            if (!ProtectedProcesses.Contains(Name))
            {
                output = _Wine.ExecCommand(
                    "winedbg",
                    getOutput: true,
                    sequence: sequence
                ).ToString();

                Console.WriteLine(output);
            }
        }
    }
}