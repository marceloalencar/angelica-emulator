// AngelEmu - Perfect World Server Emulator
// Copyright (C) 2018  Marcelo Alencar
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
namespace AngelLib.Utils
{
    using System;

    public class ConsoleWriter
    {
        public static void WriteRegion(string name)
        {
            int num = 79;
            string str = "[ " + name + " ]";
            while (str.Length < num)
            {
                str = "=" + str + "=";
            }
            if (str.Length > num)
            {
                str = str.Substring(0, num - 1);
            }
            Console.WriteLine(str);
        }

        public static void WriteLicense()
        {
            Console.WriteLine(@"AngelEmu - Perfect World Server Emulator
Copyright (C) 2018  Marcelo Alencar

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.

");
        }
    }
}

