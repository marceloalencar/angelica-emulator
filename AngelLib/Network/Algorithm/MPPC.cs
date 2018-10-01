using System;
using System.Collections;
using System.IO;

namespace AngelLib.Network.Algorithm
{
    public class MPPC
    {
        private BitArray srcbinary;
        private int i;
        private int j;
        private StreamWriter writer;

        public MPPC()
        {
        }

        private byte[] BitsToBytesCompressor(BitArray target, int lenght)
        {
            int[] num = new int[lenght / 8];
            byte[] bytes = new byte[lenght / 8];
            for (int i = 0; i < lenght / 8; i++)
            {
                for (int j = 7; j >= 0; j--)
                {
                    if (target[i * 8 + j])
                    {
                        num[i] += Convert.ToInt32(Math.Pow(Convert.ToDouble(2), Convert.ToDouble(j)));
                    }
                }
                bytes[i] = BitConverter.GetBytes(num[i])[0];
            }
            return bytes;
        }

        private void Copy(BitArray inputbits, int from, BitArray outputbits, int to, int length)
        {
            for (int i = 0; i < length; i++)
            {
                outputbits[to + i] = inputbits[from + i];
            }
        }

        public byte[] PseudoCompress(byte[] src)
        {
            int i;
            BitArray bitArrays = new BitArray((int)src.Length * 9 + 11);
            int num = 0;
            int num1 = 0;
            BitArray bitArrays1 = new BitArray(src);
            try
            {
                ReOrderBitArray(bitArrays1);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
            while (num < bitArrays1.Length)
            {
                if (!bitArrays1[num])
                {
                    Copy(bitArrays1, num, bitArrays, num + num1, 8);
                    num += 8;
                }
                else
                {
                    bitArrays[num + num1] = true;
                    num1++;
                    bitArrays[num + num1] = false;
                    Copy(bitArrays1, num + 1, bitArrays, num + num1 + 1, 7);
                    num += 8;
                }
            }
            for (i = 0; i < 4; i++)
            {
                bitArrays[num + num1 + i] = true;
            }
            num += 4;
            for (i = 0; i < 6; i++)
            {
                bitArrays[num + num1 + i] = false;
            }
            num += 6;
            bitArrays.Length = ((num + num1) % 8 == 0 ? num + num1 : num + num1 + (8 - (num + num1) % 8));
            ReOrderBitArray(bitArrays);
            return BitsToBytesCompressor(bitArrays, bitArrays.Length);
        }

        private void ReOrderBitArray(BitArray src)
        {
            for (int i = 0; i < src.Length; i += 8)
            {
                for (int j = 0; j < 4; j++)
                {
                    bool item = src[i + j];
                    src[i + j] = src[i + (7 - j)];
                    src[i + (7 - j)] = item;
                }
            }
        }
    }
}
