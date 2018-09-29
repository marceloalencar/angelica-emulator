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
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LinkServer
{
    internal class ClientListener
    {
        private Settings _settings;
        private TcpListener _clientListener;
        private Database _database;
        private ManualResetEvent _tcpClientConnected;
        private List<ClientConnection> _clients;

        public ClientListener(Settings settings)
        {
            _settings = settings;
            _tcpClientConnected = new ManualResetEvent(false);
            _clients = new List<ClientConnection>();
        }

        internal void ConnectDb()
        {
            Console.WriteLine("Connecting to database...");
            _database = new Database();
        }

        internal void StartServer()
        {
            _clientListener = new TcpListener(IPAddress.Any, _settings.ClientListenPort);
            _clientListener.Start();
            Console.WriteLine("Listening for clients on port {0}", _settings.ClientListenPort);
            while (true)
            {
                _tcpClientConnected.Reset();
                Console.WriteLine("Waiting for connections...");
                _clientListener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), _clientListener);
                _tcpClientConnected.WaitOne();
                CleanDisconnectedClients();
            }
        }

        private void AcceptClient(IAsyncResult ar)
        {
            Console.WriteLine("Client connected");
            TcpListener listener = (TcpListener)ar.AsyncState;
            TcpClient tcpClient = listener.EndAcceptTcpClient(ar);
            ClientConnection client = new ClientConnection(tcpClient, _settings, _database);
            _tcpClientConnected.Set();
            _clients.Add(client);
            client.StartSession();
        }

        private void CleanDisconnectedClients()
        {
            for (int i = _clients.Count - 1; i >= 0; i--)
            {
                if (_clients[i].IsConnected == false)
                    _clients.RemoveAt(i);
            }
        }
    }
}