using AngelLib.Utils;

namespace AngelLib.Network.LinkServer.ServerPackets
{
    public class OnlineAnnounce : ServerPacket
    {
        private byte _packetId = (byte) LinkServerPacket.OnlineAnnounce;
        private byte _packetLength = 29;

        public OnlineAnnounce()
        {

        }

        public override byte[] GetBytes()
        {
            Octet data = new Octet();
            data.Marshal(_packetId);
            data.Marshal((byte)29);
            throw new NotImplementedException();
            return data.GetBytes();
        }
    }
}
