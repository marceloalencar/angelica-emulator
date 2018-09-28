using AngelLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelLib.Network.ClientPackets
{
    public class Login
    {
        private byte _packetId = 0x03;                  // 0x03 = LoginAnnounce
        private byte _packetLength;
        private byte _usernameLength;
        private string _username;                       // ASCII username
        private byte _hashLength;
        private byte[] _hash;                           // Hashed password

        public Login(Octet data)
        {
            _packetLength = data.UnMarshalByte();
            _usernameLength = data.UnMarshalByte();
            _username = Encoding.UTF8.GetString(data.UnMarshalBytes(_usernameLength));
            _hashLength = data.UnMarshalByte();
            _hash = data.UnMarshalBytes(_hashLength);
        }

        public string Username { get => _username; set => _username = value; }
        public byte[] Hash { get => _hash; set => _hash = value; }
    }
}
