using AngelLib.Utils;
using System;

namespace AngelLib.Network.LinkServer.ServerPackets
{
    public class LastLoginInfo : ServerPacket
    {
        private byte _packetId = (byte)LinkServerPacket.LastLoginInfo;
        private byte _packetLength = 20;
        private uint _userId;                // User ID from database
        private uint _randomUint;            // Marker ?
        private uint _lastLoginTimestamp;    // Unix timestamp
        private uint _lastLoginIPAddr;
        private uint _currentIPAddr;       // In reverse order

        public LastLoginInfo(uint userId)
        {
            _userId = userId;
            _randomUint = 0;
            _lastLoginTimestamp = UnixTime.ToTimestamp(DateTime.Now);
            _lastLoginIPAddr = 0;
            _currentIPAddr = 0;
        }

        public override byte[] GetBytes()
        {
            Octet data = new Octet();
            data.Marshal(_packetId);
            data.Marshal(_packetLength);
            data.Marshal(_userId);
            data.Marshal(_randomUint);
            data.Marshal(_lastLoginTimestamp);
            data.Marshal(_lastLoginIPAddr);
            data.Marshal(_currentIPAddr);
            return data.GetBytes();
        }
    }
}
