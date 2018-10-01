using AngelLib.Utils;

namespace AngelLib.Network.LinkServer.ClientPackets
{
    public class CMKey
    {
        private byte _packetId = (byte)LinkServerPacket.KeyExchange;
        private byte _packetLength;
        private byte _keyLength;
        private byte[] _key;
        private byte _unknown;

        public CMKey(Octet data)
        {
            _packetLength = data.UnMarshalByte();
            _keyLength = data.UnMarshalByte();
            _key = data.UnMarshalBytes(_keyLength);
            _unknown = data.UnMarshalByte();
        }

        public byte[] GetCMKey()
        {
            return _key;
        }
    }
}
