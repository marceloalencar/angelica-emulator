using AngelLib.Utils;
using System.Text;

namespace AngelLib.Network.LinkServer.ClientPackets
{
    public class Login
    {
        private byte _packetId = (byte)LinkServerPacket.LoginRequest;
        private byte _packetLength;
        private byte _usernameLength;
        private string _username;                       // ASCII username
        private byte _hashLength;
        private byte[] _hash;                           // Hashed password

        public Login(Octet data)
        {
            _packetLength = data.UnMarshalByte();
            _usernameLength = data.UnMarshalByte();
            _username = Encoding.ASCII.GetString(data.UnMarshalBytes(_usernameLength));
            _hashLength = data.UnMarshalByte();
            _hash = data.UnMarshalBytes(_hashLength);
        }

        public string Username { get => _username; set => _username = value; }
        public byte[] Hash { get => _hash; set => _hash = value; }
    }
}
