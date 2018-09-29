using AngelLib.Utils;
using System;
using System.Text;

namespace AngelLib.Network.ServerPackets
{
    public class ErrorInfo : ServerPacket
    {
        private byte _packetId = 0x05;             // 0x05 = ErrorInfo
        private byte _packetLength;
        private int _errorCode;                //configs/servererror.txt) 1 = 10150
        private byte _msgLength;
        private byte[] _msg;
        private byte _unknown2 = 0x2E;

        public ErrorInfo(int errorCode)
        {
            _errorCode = errorCode;
            _msg = Encoding.ASCII.GetBytes("Login error");
            _msgLength = (byte)_msg.Length;
            _packetLength = (byte)(6 + _msg.Length);
        }

        public override byte[] GetBytes()
        {
            Octet octet = new Octet();
            octet.Marshal(_packetId);
            octet.Marshal(_packetLength);
            octet.Marshal(_errorCode);
            octet.Marshal(_msgLength);
            octet.Marshal(_msg);
            octet.Marshal(_unknown2);
            return octet.GetBytes();
        }
    }
}
