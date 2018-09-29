﻿namespace AngelLib.Network.LinkServer
{
    public enum LinkServerPacket : byte
    {
        Challenge    = 0x01,
        KeyExchange  = 0x02,
        LoginRequest = 0x03,
        ErrorInfo    = 0x05
    }
}
