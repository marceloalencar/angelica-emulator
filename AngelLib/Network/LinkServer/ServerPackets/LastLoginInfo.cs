using AngelLib.Utils;
using System;

namespace AngelLib.Network.LinkServer.ServerPackets
{
    public class LastLoginInfo : ServerPacket
    {
        private byte _packetId = (byte)LinkServerPacket.LastLoginInfo;
        private byte _packetLength = 20;
        private uint _userId;                // User ID from database
        private uint _unknown;               // Marker ?
        private uint _lastLoginTimestamp;    // Unix timestamp
        private byte[] _lastLoginIPAddr;
        private byte[] _currentIPAddr;

        public LastLoginInfo(uint userId)
        {
            _userId = userId;
            _unknown = 0;
            _lastLoginTimestamp = UnixTime.ToTimestamp(DateTime.Now);
            _lastLoginIPAddr = new byte[4];
            _currentIPAddr = new byte[4];
        }

        public override byte[] GetBytes()
        {
            Octet data = new Octet();
            data.Marshal(_packetId);
            data.Marshal(_packetLength);
            data.Marshal(_userId);
            data.Marshal(_unknown);
            data.Marshal(_lastLoginTimestamp);
            data.Marshal(_lastLoginIPAddr);
            data.Marshal(_currentIPAddr);
            return data.GetBytes();
        }
    }
}
