using AngelLib.Utils;

namespace AngelLib.Network.LinkServer.ClientPackets
{
    public class RoleList
    {
        private byte _packetId = (byte)LinkServerPacket.RoleList;
        private byte _packetLength;
        private int _userId;
        private int _unknown1;
        private int _unknown2;

        public RoleList(Octet data)
        {
            _packetLength = data.UnMarshalByte();
            _userId = data.UnMarshalInt32();
            _unknown1 = data.UnMarshalInt32();
            _unknown2 = data.UnMarshalInt32();
        }
    }
}
