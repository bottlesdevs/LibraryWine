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
using System.IO;

namespace Bottles.LibraryWine
{
    public static class WineHelpers
    {
        public static bool ValidateWinePath(string winePath)
        {
            if (string.IsNullOrEmpty(winePath))
                return false;
            if (!Directory.Exists(winePath))
                return false;

            foreach (string dir in new string[] { "share", "bin", "lib" })
            {
                if (!Directory.Exists(Path.Combine(winePath, dir)))
                    return false;
            }
            return true;
        }

        public static bool ValidateWinePrefixPath(string winePrefixPath)
        {
            if (string.IsNullOrEmpty(winePrefixPath))
                return false;
            if (!Directory.Exists(winePrefixPath))
            {
                try
                {
                    Directory.CreateDirectory(winePrefixPath);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }


        public static bool CheckArchCompatibility()
        {
            return false;
        }
    }
}