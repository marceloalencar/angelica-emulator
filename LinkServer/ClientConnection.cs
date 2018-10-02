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
using AngelLib.Network.Algorithm;
using AngelLib.Network.LinkServer;
using AngelLib.Network.LinkServer.ClientPackets;
using AngelLib.Network.LinkServer.ServerPackets;
using AngelLib.Utils;
using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace LinkServer
{
    internal class ClientConnection
    {
        private TcpClient _tcpClient;
        private Database _database;
        private Settings _settings;
        private bool _isEncrypted;
        private bool _isCompressed;
        private string _username;
        private byte[] _passwordHash;
        private byte[] _challengeKey;
        private byte[] _smKey;
        private byte[] _cmKey;
        private RC4 C2S_Crypto;
        private RC4 S2C_Crypto;
        private MPPC MPPC_Compressor;

        public bool IsConnected;

        public ClientConnection(TcpClient tcpClient, Settings settings, Database database)
        {
            _tcpClient = tcpClient;
            _database = database;
            _settings = settings;
            _isEncrypted = false;
            _isCompressed = false;
            IsConnected = true;
            MPPC_Compressor = new MPPC();
        }

        internal void StartSession()
        {
            try
            {
                using (Stream stream = _tcpClient.GetStream())
                {
                    Challenge challengePacket = new Challenge(_settings);
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
                case (byte)LinkServerPacket.LoginRequest:
                    Login loginPacket = new Login(data);
                    _username = loginPacket.Username;
                    _passwordHash = loginPacket.Hash;
                    Console.WriteLine("User {0} is trying to login", _username);
                    string passHash = BitConverter.ToString(_passwordHash).Replace("-", string.Empty);
                    string dbHashString = _database.GetUserPasswd(_username, passHash);
                    if (dbHashString.Equals(passHash))
                    {
                        SMKey smkeyPacket = new SMKey();
                        SendReply(smkeyPacket.GetBytes());
                        _smKey = smkeyPacket.GetSMKey();
                        HMACMD5 hmacmd5 = new HMACMD5(Encoding.ASCII.GetBytes(_username));
                        byte[] array = new byte[_passwordHash.Length + _smKey.Length];
                        _passwordHash.CopyTo(array, 0);
                        _smKey.CopyTo(array, _passwordHash.Length);
                        byte[] RC4_C2SKEY = hmacmd5.ComputeHash(array);
                        C2S_Crypto = new RC4(RC4_C2SKEY);
                        _isEncrypted = true;
                    }
                    else
                    {
                        ErrorInfo errorPacket = new ErrorInfo(2);
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
            if (buflen == 0)
            {
                Console.WriteLine("Client sent empty packet. Disconnecting...");
                IsConnected = false;
                return;
            }
            byte[] arrData = new byte[buflen];
            Array.Copy(recvbuf, arrData, buflen);
            C2S_Crypto.decrypt(arrData);
            Octet data = new Octet(arrData);
            byte packetid = data.UnMarshalByte();
            switch (packetid)
            {
                case (byte)LinkServerPacket.KeyExchange:
                    CMKey smkeyPacket = new CMKey(data);
                    _cmKey = smkeyPacket.GetCMKey();
                    HMACMD5 hmacmd5 = new HMACMD5(Encoding.ASCII.GetBytes(_username));
                    byte[] array = new byte[_passwordHash.Length + _cmKey.Length];
                    _passwordHash.CopyTo(array, 0);
                    _cmKey.CopyTo(array, _passwordHash.Length);
                    byte[] RC4_S2CKEY = hmacmd5.ComputeHash(array);
                    S2C_Crypto = new RC4(RC4_S2CKEY);
                    _isCompressed = true;

                    OnlineAnnounce announcePacket = new OnlineAnnounce();
                    SendReply(announcePacket.GetBytes());
                    break;
                default:
                    IsConnected = false;
                    Console.WriteLine("Unknown packet. Disconnecting...");
                    Console.WriteLine(BitConverter.ToString(arrData, 0, arrData.Length));
                    break;
            }
        }

        public void SendReply(byte[] data)
        {
            Stream stream = _tcpClient.GetStream();
            if (!_isCompressed)
                stream.Write(data, 0, data.Length);
            else
            {
                byte[] compressed = MPPC_Compressor.Compress(data);
                S2C_Crypto.decrypt(compressed);
                stream.Write(compressed, 0, compressed.Length);
            }
        }
    }
}