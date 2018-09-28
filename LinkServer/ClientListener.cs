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
        private TcpListener _listener;
        private ManualResetEvent _tcpClientConnected;
        private List<ClientConnection> _clients;

        public ClientListener()
        {
             _tcpClientConnected = new ManualResetEvent(false);
            _clients = new List<ClientConnection>();
        }

        internal void StartServer(int port, Settings settings)
        {
            _settings = settings;
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            Console.WriteLine("Listening for clients on port {0}", port);
            while (true)
            {
                _tcpClientConnected.Reset();
                Console.WriteLine("Waiting for connections...");
                _listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), _listener);
                _tcpClientConnected.WaitOne();
                CleanDisconnectedClients();
            }
        }

        private void AcceptClient(IAsyncResult ar)
        {
            Console.WriteLine("Client connected");
            TcpListener listener = (TcpListener)ar.AsyncState;
            TcpClient tcpClient = listener.EndAcceptTcpClient(ar);
            ClientConnection client = new ClientConnection(tcpClient, _settings);
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