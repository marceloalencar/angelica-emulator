using System;
using System.Linq;

namespace AngelLib.Network.Algorithm
{
    public class MPPC
    {
        private const int BLOCK_SIZE = 0x2000;

        private readonly byte[] history;
        private readonly ushort[] hash;
        private int histoff;
        private int legacy_in;

        public MPPC()
        {
            history = new byte[BLOCK_SIZE];
            hash = new ushort[BLOCK_SIZE];
            histoff = 0;
            legacy_in = 0;
        }

        public byte[] Compress(byte[] input)
        {
            byte[] ret;
            if (input.Length > 0 || legacy_in > 0)
            {
                ret = new byte[(9 * (legacy_in + input.Length) >> 3) + 6];
                BitStream outbits = Update(input, ret);
                CompressBlock(outbits, legacy_in);
                ret = ret.Take(outbits.Offset >> 3).ToArray();
            }
            else
            {
                ret = input;
            }
            return ret;
        }

        private BitStream Update(byte[] input, byte[] output)
        {
            int remain = BLOCK_SIZE - histoff - legacy_in;
            int isize = input.Length;
            int ioffset = 0;
            BitStream ostream = new BitStream(output);
            if (isize >= remain)
            {
                Buffer.BlockCopy(input, 0, history, histoff + legacy_in, remain);
                isize -= remain;
                ioffset += remain;
                CompressBlock(ostream, remain + legacy_in);
                histoff = 0;
                while (isize >= BLOCK_SIZE)
                {
                    Buffer.BlockCopy(input, ioffset, history, 0, BLOCK_SIZE);
                    CompressBlock(ostream, BLOCK_SIZE);
                    histoff = 0;
                    isize -= BLOCK_SIZE;
                    ioffset += BLOCK_SIZE;
                }
            }
            Buffer.BlockCopy(input, ioffset, history, histoff + legacy_in, isize);
            legacy_in += isize;
            return ostream;
        }

        private void CompressBlock(BitStream stream, int size)
        {
            int r = histoff + size;
            int s = histoff;
            while (r - s > 2)
            {
                int p = GetPredecitAddr(s);
                if (p < s)
                {
                    if (history[p++] == history[s++] && history[p++] == history[s])
                    {
                        if (history[p++] == history[++s])
                        {
                            for (++s; s < r && history[p] == history[s]; ++s) ++p;
                            int len = s - histoff;
                            histoff = s;
                            PutOff(stream, s - p);
                            int val = 0, n = 1;
                            if (len > 3)
                            {
                                int high = GetHighestBit(len);
                                val = len & (1 << high) - 1 | ((1 << (high - 1)) - 1) << high + 1;
                                n = high << 1;
                            }
                            PutBits(stream, val, n);
                        }
                        else
                        {
                            PutLit(stream, history[histoff++]);
                            s = histoff;
                        }
                    }
                    else
                    {
                        PutLit(stream, history[histoff++]);
                    }
                }
                else
                {
                    PutLit(stream, history[histoff++]);
                    s = histoff;
                }
            }
            if (r - s == 1 || r - s == 2)
            {
                for (int i = 0; i < r - s; i++)
                    PutLit(stream, history[histoff++]);
            }
            PutOff(stream, 0);
            stream.Pad();
            legacy_in = 0;
        }

        private void PutBits(BitStream bits, int val, int n)
        {
            //mm0 keeps the last byte of the previous write so the value can be properly written with mov dword (old_bits OR new_bits)
            //mm1 keeps byte offset
            //mm2 keeps bit offset
            //mm3 temporary register, used for keeping the calculated val and shifting mm0
            //mm4 is used for getting the final size of the packed array
            //mm5 is used for moding mm2
            bits.WriteBits(val, n);
        }
        private void PutLit(BitStream bits, int c)
        {
            int newc = ((c & 0xFFFF) | ((c & 0x80) << 1)) & 0x17F;
            int n = 8 | ((c & 0x80) >> 7);
            PutBits(bits, newc, n);
        }
        private void PutOff(BitStream bits, int off)
        {
            int newoff = off | 0x3C0;
            int n = 0x0A;
            if (off > 0x3F)
            {
                if (off > 0x13F)
                {
                    newoff = (off - 0x140) | 0xC000;
                    n = 0x10;
                }
                else
                {
                    newoff = (off - 0x40) | 0xE00;
                    n = 0x0C;
                }
            }
            PutBits(bits, newoff, n);
        }
        private int GetPredecitAddr(int offset)
        {
            int index = (0x9E5F * (history[offset + 2] ^ 16 * (history[offset + 1] ^ 16 * history[offset])) >> 4) & 0x1FFF;
            int ret = hash[index];
            hash[index] = (ushort)offset;
            return ret;
        }

        private int GetHighestBit(int val)
        {
            int ret = 31;
            while ((val & 1 << ret) == 0) ret--;
            return ret;
        }
    }

    class BitStream
    {
        private readonly byte[] buffer;
        public int Offset { get; private set; }

        public BitStream(byte[] buf)
        {
            buffer = buf;
            Offset = 0;
        }

        public void WriteBits(int val, int n)
        {
            int off = Offset & 7;
            int idx = Offset >> 3;
            uint v = (uint)((ulong)val << 32 - n - off) | (uint)buffer[idx] << 24;
            for (int i = 0; i < 4; i++)
                buffer[idx + i] = (byte)(v >> 24 - (i << 3) & 0xFF);
            Offset += n;
        }
        public void Pad()
        {
            int pad_length = (8 - (Offset & 7)) & 7;
            if (pad_length > 0)
                WriteBits(0, pad_length);
        }
    }
}
