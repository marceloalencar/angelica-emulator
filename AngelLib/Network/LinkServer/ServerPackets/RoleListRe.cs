using AngelLib.Utils;

namespace AngelLib.Network.LinkServer.ServerPackets
{
    public class RoleListRe : ServerPacket
    {
        private byte _packetId = (byte)LinkServerPacket.RoleListRe;
        private ushort _packetLength;
        private uint _unknown1 = 0;
        private int _slot;
        private uint _userId;
        private uint _randomUint = 0;
        private byte[] _charData;

        public RoleListRe(int slot, uint userId)
        {
            _userId = userId;
            if (slot == -1)
            {
                _slot = -1;
                _charData = new byte[1];
            }
            _packetLength = (ushort)(16 + _charData.Length);
        }

        public override byte[] GetBytes()
        {
            Octet data = new Octet();
            data.Marshal(_packetId);
            data.MarshalCuint(_packetLength);
            data.Marshal(_unknown1);
            data.Marshal(_slot);
            data.Marshal(_userId);
            data.Marshal(_randomUint);
            data.Marshal(_charData);
            return data.GetBytes();
        }
    }
}
