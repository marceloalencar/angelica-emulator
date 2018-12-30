using AngelLib.Utils;

namespace AngelLib.Network.LinkServer.ServerPackets
{
    public class OnlineAnnounce : ServerPacket
    {
        private byte _packetId = (byte) LinkServerPacket.OnlineAnnounce;
        private byte _packetLength = 29;
        private uint _userId;

        public OnlineAnnounce(uint userId)
        {
            _userId = userId;
        }

        public override byte[] GetBytes()
        {
            Octet data = new Octet();
            data.Marshal(_packetId);
            data.Marshal(_packetLength);
            data.Marshal(_userId);
            data.Marshal(0);
            data.Marshal(0);
            data.Marshal((byte)0x03);
            data.Marshal(0);
            data.Marshal(-1);
            data.Marshal(new byte[8]);
            return data.GetBytes();
        }
    }
}
