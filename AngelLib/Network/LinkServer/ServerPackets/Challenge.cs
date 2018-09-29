using AngelLib.Configuration.LinkServer;
using AngelLib.Utils;
using System;

namespace AngelLib.Network.LinkServer.ServerPackets
{
    public class Challenge : ServerPacket
    {
        private byte _packetId = (byte)LinkServerPacket.Challenge;
        private byte _packetLength = 0x3A;
        private byte _keyLength = 16;                   // ?????????
        private byte _serverLoad = 0;                   // 0 - 255
        private short _unknown1 = 0;                    // 0000
        private byte _serverFlags;                      // Server flags
        private uint _unknown2 = 0;                     // 00000000
        private byte[] _key;                            // Random 8 bytes key
        private byte[] _gameVersion = { 0, 1, 5, 6 };   // 1.5.6
        private byte _authMethod = 0x01;                // SHA-256
        private byte _clientSignatureLength;
        private byte[] _clientSignature;                // Timestamps
        private byte _expMultiplier;                    // Exp 0B:1x, ..., 64:10x

        public Challenge(Settings settings)
        {
            _serverFlags = 0x50;
            if (settings.IsPvP)
                _serverFlags |= 0x80;
            if (settings.SpiritBonus)
                _serverFlags |= 0x08;
            if (settings.DropBonus)
                _serverFlags |= 0x04;
            if (settings.MoneyBonus)
                _serverFlags |= 0x02;
            Random random = new Random();
            _key = new byte[8];
            random.NextBytes(_key);
            _clientSignature = HexParser.GetBytes(settings.ClientSignature);
            _clientSignatureLength = (byte)_clientSignature.Length;
            _expMultiplier = (byte)(settings.ExpMultiplier * 10);
        }

        public override byte[] GetBytes()
        {
            Octet octet = new Octet();
            octet.Marshal(_packetId);
            octet.Marshal(_packetLength);
            octet.Marshal(_keyLength);
            octet.Marshal(_serverLoad);
            octet.Marshal(_unknown1);
            octet.Marshal(_serverFlags);
            octet.Marshal(_unknown2);
            octet.Marshal(_key);
            octet.Marshal(_gameVersion);
            octet.Marshal(_authMethod);
            octet.Marshal(_clientSignatureLength);
            octet.Marshal(_clientSignature);
            octet.Marshal(_expMultiplier);
            return octet.GetBytes();
        }

        public byte[] GetChallengeKey()
        {
            return _key;
        }
    }
}
