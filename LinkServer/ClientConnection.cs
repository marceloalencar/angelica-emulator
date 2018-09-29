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
using System;
using System.IO;
using System.Net.Sockets;
using AngelLib.Utils;
using AngelLib.Configuration.LinkServer;
using AngelLib.Network.ServerPackets;
using System.Threading;
using AngelLib.Network.ClientPackets;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace LinkServer
{
    internal class ClientConnection
    {
        private TcpClient _tcpClient;
        private Settings _settings;
        private byte[] _challengeKey;
        private bool _isEncrypted;
        private byte[] _smKey;
        private byte[] _cmKey;

        public bool IsConnected;

        public ClientConnection(TcpClient tcpClient, Settings settings)
        {
            _tcpClient = tcpClient;
            _settings = settings;
            _isEncrypted = false;
            IsConnected = true;
        }

        internal void StartSession()
        {
            try
            {
                using (Stream stream = _tcpClient.GetStream())
                {
                    Challenge challengePacket = new Challenge(Program.serverSettings);
                    _challengeKey = challengePacket.GetChallengeKey();
                    stream.Write(challengePacket.GetBytes(), 0, challengePacket.GetBytes().Length);
                    byte[] recvbuf = new byte[8192];
                    while (_tcpClient.Connected && IsConnected)
                    {
                        int buflen = stream.Read(recvbuf, 0, 8192);
                        if (!_isEncrypted)
                            ClearPacketHandler(recvbuf, buflen);
                        else
                            EncryptedPacketHandler(recvbuf, buflen);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                IsConnected = false;
                _tcpClient.Close();
            }
        }

        private void ClearPacketHandler(byte[] recvbuf, int buflen)
        {
            if (buflen == 0)
            {
                Console.WriteLine("Client sent empty packet. Disconnecting...");
                IsConnected = false;
                return;
            }
            byte[] arrData = new byte[buflen];
            Array.Copy(recvbuf, arrData, buflen);
            Octet data = new Octet(arrData);
            byte packetid = data.UnMarshalByte();
            switch (packetid)
            {
                case 0x03:
                    Login loginPacket = new Login(data);
                    Console.WriteLine("User {0} is trying to login", loginPacket.Username);
                    //Password: test
                    //SHA-256(password) = 9F86D081884C7D659A2FEAA0C55AD015A3BF4F1B2B0B822CD15D6C15B0F00A08
                    byte[] tmpPass = Encoding.ASCII.GetBytes("test");
                    SHA256 sha256 = SHA256.Create();
                    byte[] hash = sha256.ComputeHash(tmpPass);
                    if (hash.SequenceEqual(loginPacket.Hash))
                    {
                        SMKey smkeyPacket = new SMKey();
                        SendReply(smkeyPacket.GetBytes());
                        _smKey = smkeyPacket.GetSMKey();
                        _isEncrypted = true;
                    }
                    else
                    {
                        ErrorInfo errorPacket = new ErrorInfo(2);
                        Console.WriteLine(BitConverter.ToString(errorPacket.GetBytes()));
                        SendReply(errorPacket.GetBytes());
                        Console.WriteLine("Login error!");
                    }
                    break;
                default:
                    IsConnected = false;
                    Console.WriteLine("Unknown packet. Disconnecting...");
                    Console.WriteLine(BitConverter.ToString(recvbuf, 0, buflen));
                    break;
            }
        }

        private void EncryptedPacketHandler(byte[] recvbuf, int buflen)
        {
            throw new NotImplementedException();
        }

        public void SendReply(byte[] data)
        {
            Stream stream = _tcpClient.GetStream();
            stream.Write(data, 0, data.Length);
        }
    }
}