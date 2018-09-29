using AngelLib.Utils;
using System;

namespace AngelLib.Network.ServerPackets
{
    public class SMKey : ServerPacket
    {
        private byte _packetId = 0x02;             // 0x02 = SMKey
        private byte _packetLength = 0x12;
        private byte _keyLength = 16;
        private byte[] _key;
        private byte _unknown = 0x00;

        public SMKey()
        {
            Random random = new Random();
            _key = new byte[_keyLength];
            random.NextBytes(_key);
        }

        public override byte[] GetBytes()
        {
            Octet octet = new Octet();
            octet.Marshal(_packetId);
            octet.Marshal(_packetLength);
            octet.Marshal(_keyLength);
            octet.Marshal(_key);
            octet.Marshal(_unknown);
            return octet.GetBytes();
        }

        public byte[] GetSMKey()
        {
            return _key;
        }
    }
}
