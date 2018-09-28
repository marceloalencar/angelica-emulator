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
using AngelLib.Configuration.LinkServer;
using AngelLib.Utils;
using System;

namespace LinkServer
{
    internal class Program
    {
        internal static Settings serverSettings;

        private static void Main(string[] args)
        {
            ConsoleWriter.WriteLicense();
            ConsoleWriter.WriteRegion("Starting LinkServer");
            Console.WriteLine("Loading configuration files...");
            serverSettings = Settings.LoadSettings();
            ClientListener listener = new ClientListener();
            listener.StartServer(serverSettings.ClientListenPort, serverSettings);
        }
    }
}
