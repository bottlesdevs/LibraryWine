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
using System.Linq;
using System.Collections.Generic;

namespace Bottles.LibraryWine
{
    public class WineTools
    {
#region Informations
        public static string GetVersion(ref Wine wine)
        {
            var result = wine.ExecCommand("--version", getOutput: true);
            if (result is bool)
                return result.ToString();

            return (string)result;
        }

        public static List<WineProcess> GetRunningProcesses(ref Wine wine)
        {
            var processes = new List<WineProcess>();
            var result = wine.ExecCommand(
                "winedbg",
                @"--command ""info proc""",
                getOutput: true
            );

            if (result is bool)
                return processes;
            
            string[] output = result.ToString().Split("\n");

            if (output.Length < 2)
                return processes;
                
            output = output.Skip(1).ToArray();
            
            string lastPid = "";

            foreach (var line in output)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string parentPid = "";

                if (line.Contains("\\_"))
                    parentPid = lastPid;
                
                string[] processInfo = line.Replace(
                    "\\_ ", "").Replace(
                        "'", "").Split(
                            ' ', StringSplitOptions.RemoveEmptyEntries);
                            
                processes.Add(new WineProcess(
                    ref wine,
                    name: processInfo[2],
                    pid: processInfo[0],
                    parentPid: parentPid
                ));

                lastPid = processInfo[0];
            }

            return processes;
        }
#endregion

#region Boot
        private static void WineBoot(ref Wine wine, Wine.BootStates state, bool silent = true)
        {
            string stateFlag = wine.BootStatesStrings[(int)state];
            Dictionary<string, string> envVars = new Dictionary<string, string>();

            if (silent)
                envVars.Add("DISPLAY", ":3.0");

            wine.ExecCommand($"wineboot {stateFlag}", envVars: envVars);
        }

        public static void WineBootEndSession(ref Wine wine, bool silent = true)
        {
            WineBoot(ref wine, Wine.BootStates.END_SESSION, silent);
        }
        
        public static void WineBootForce(ref Wine wine, bool silent = true)
        {
            WineBoot(ref wine, Wine.BootStates.FORCE, silent);
        }

        public static void WineBootInit(ref Wine wine, bool silent = true)
        {
            WineBoot(ref wine, Wine.BootStates.INIT, silent);
        }

        public static void WineBootKill(ref Wine wine, bool silent = true)
        {
            WineBoot(ref wine, Wine.BootStates.KILL, silent);
        }

        public static void WineBootRestart(ref Wine wine, bool silent = true)
        {
            WineBoot(ref wine, Wine.BootStates.RESTART, silent);
        }

        public static void WineBootShutdown(ref Wine wine, bool silent = true)
        {
            WineBoot(ref wine, Wine.BootStates.SHUTDOWN, silent);
        }

        public static void WineBootUpdate(ref Wine wine, bool silent = true)
        {
            WineBoot(ref wine, Wine.BootStates.UPDATE, silent);
        }
#endregion

#region Tools
        public static void RunExe(ref Wine wine, string executable, string arguments = "")
        {
            wine.ExecCommand(executable, arguments);
        }

        public static void RunMsi(ref Wine wine, string executable, string arguments = "")
        {
            string command = $"msiexec /i {executable}";
            wine.ExecCommand(command, arguments);
        }

        public static void RunBat(ref Wine wine, string executable, string arguments = "")
        {
            WineCmd(ref wine, executable, arguments);
        }

        public static void WineCfg(ref Wine wine)
        {
            wine.ExecCommand("winecfg");
        }

        public static void WineDbg(ref Wine wine)
        {
            wine.ExecCommand("winedbg", useTerminal: true);
        }

        public static object WineCmd(ref Wine wine, string command = "", string arguments = "", bool getOutput = false)
        {
            if (command == "")
                command = "cmd";
            else
                command = $"cmd /c {command}";
#if FLATPAK
            if (!getOutput)
                command = $"wineconsole {command}";
#endif
            var result = wine.ExecCommand(
                command, 
                useTerminal: true, 
                arguments: arguments, 
                getOutput: getOutput
            );
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
