// AngelEmu - Perfect World Server Emulator
// Copyright (C) 2018  Marcelo Alencar
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
using System;
using System.Text;

namespace AngelLib.Utils
{
    [Serializable]
    public class Octet
    {
        private bool _littleEndian;

        private byte[] data;

        public int Length => data.Length;

        public void LittleEndian()
        {
            _littleEndian = true;
        }

        public void BigEndian()
        {
            _littleEndian = false;
        }

        public Octet()
        {
            _littleEndian = false;
            data = new byte[0];
        }

        public Octet(bool Endian)
        {
            _littleEndian = Endian;
            data = new byte[0];
        }

        public Octet(byte[] data)
        {
            _littleEndian = false;
            this.data = (byte[])data.Clone();
        }

        public Octet(int size)
        {
            _littleEndian = false;
            data = new byte[size];
        }

        public byte[] GetBytes()
        {
            return data;
        }

        public void Marshal(byte b)
        {
            byte[] array = new byte[data.Length + 1];
            data.CopyTo(array, 0);
            array[data.Length] = b;
            data = array;
        }

        public void Marshal(byte[] b)
        {
            byte[] array = new byte[data.Length + b.Length];
            data.CopyTo(array, 0);
            b.CopyTo(array, data.Length);
            data = array;
        }

        public void Marshal(uint i)
        {
            byte[] bytes = BitConverter.GetBytes(i);
            if (!_littleEndian)
            {
                Array.Reverse(bytes);
            }
            byte[] array = new byte[data.Length + bytes.Length];
            data.CopyTo(array, 0);
            bytes.CopyTo(array, data.Length);
            data = array;
        }

        public void Marshal(int i)
        {
            byte[] bytes = BitConverter.GetBytes(i);
            if (!_littleEndian)
            {
                Array.Reverse(bytes);
            }
            byte[] array = new byte[data.Length + bytes.Length];
            data.CopyTo(array, 0);
            bytes.CopyTo(array, data.Length);
            data = array;
        }

        public void Marshal(DateTime i)
        {
            Marshal((uint)(i - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }

        public void Marshal(float i)
        {
            byte[] bytes = BitConverter.GetBytes(i);
            if (!_littleEndian)
            {
                Array.Reverse(bytes);
            }
            byte[] array = new byte[data.Length + bytes.Length];
            data.CopyTo(array, 0);
            bytes.CopyTo(array, data.Length);
            data = array;
        }

        public void Marshal(long i)
        {
            byte[] bytes = BitConverter.GetBytes(i);
            if (!_littleEndian)
            {
                Array.Reverse(bytes);
            }
            byte[] array = new byte[data.Length + bytes.Length];
            data.CopyTo(array, 0);
            bytes.CopyTo(array, data.Length);
            data = array;
        }

        public void Marshal(short i)
        {
            byte[] bytes = BitConverter.GetBytes(i);
            if (!_littleEndian)
            {
                Array.Reverse(bytes);
            }
            byte[] array = new byte[data.Length + bytes.Length];
            data.CopyTo(array, 0);
            bytes.CopyTo(array, data.Length);
            data = array;
        }

        public void MarshalCuint(ushort i)
        {
            byte[] bytes = BitConverter.GetBytes(i);
            byte[] array;
            if (i < 127)
            {
                bytes = new byte[1] {(byte)i};
                array = new byte[data.Length + 1];
            }
            else
            {
                bytes = BitConverter.GetBytes(i);
                array = new byte[data.Length + bytes.Length];
                bytes[1] |= 128;
            }
            Array.Reverse(bytes);
            data.CopyTo(array, 0);
            bytes.CopyTo(array, data.Length);
            data = array;
        }

        public void MarshalCuintFront(ushort i)
        {
            byte[] bytes = BitConverter.GetBytes(i);
            byte[] array;
            if (i < 127)
            {
                bytes = new byte[1] {(byte)i};
                array = new byte[data.Length + 1];
            }
            else
            {
                bytes = BitConverter.GetBytes(i);
                array = new byte[data.Length + bytes.Length];
                bytes[1] |= 128;
            }
            Array.Reverse(bytes);
            bytes.CopyTo(array, 0);
            data.CopyTo(array, bytes.Length);
            data = array;
        }

        public ushort UnMarshalCuint()
        {
            ushort result;
            byte[] array;
            if ((data[0] & 0x80) > 0)
            {
                data[0] &= 127;
                Array.Reverse(data, 0, 2);
                result = BitConverter.ToUInt16(data, 0);
                array = new byte[data.Length - 2];
                Array.Copy(data, 2, array, 0, array.Length);
            }
            else
            {
                result = data[0];
                array = new byte[data.Length - 1];
                Array.Copy(data, 1, array, 0, array.Length);
            }
            data = array;
            return result;
        }

        public void Marshal(ushort i)
        {
            byte[] bytes = BitConverter.GetBytes(i);
            if (!_littleEndian)
            {
                Array.Reverse(bytes);
            }
            byte[] array = new byte[data.Length + bytes.Length];
            data.CopyTo(array, 0);
            bytes.CopyTo(array, data.Length);
            data = array;
        }

        public string UnMarshalUnicode()
        {
            string text = "";
            int num = 0;
            while (BitConverter.ToUInt16(data, num) != 0)
            {
                text += (char)BitConverter.ToUInt16(data, num);
                num += 2;
            }
            byte[] array = new byte[data.Length - (num + 2)];
            Array.Copy(data, num + 2, array, 0, array.Length);
            data = array;
            return text;
        }

        public byte UnMarshalByte()
        {
            byte result = data[0];
            byte[] array = new byte[data.Length - 1];
            Array.Copy(data, 1, array, 0, array.Length);
            data = array;
            return result;
        }

        public byte[] UnMarshalBytes(int count)
        {
            byte[] array = new byte[count];
            Array.Copy(data, 0, array, 0, count);
            byte[] array2 = new byte[data.Length - count];
            Array.Copy(data, count, array2, 0, array2.Length);
            data = array2;
            return array;
        }

        public uint UnMarshalUInt32()
        {
            if (!_littleEndian)
            {
                Array.Reverse(data, 0, 4);
            }
            uint result = BitConverter.ToUInt32(data, 0);
            byte[] array = new byte[data.Length - 4];
            Array.Copy(data, 4, array, 0, array.Length);
            data = array;
            return result;
        }

        public int UnMarshalInt32()
        {
            if (!_littleEndian)
            {
                Array.Reverse(data, 0, 4);
            }
            int result = BitConverter.ToInt32(data, 0);
            byte[] array = new byte[data.Length - 4];
            Array.Copy(data, 4, array, 0, array.Length);
            data = array;
            return result;
        }

        public float UnMarshalFloat()
        {
            if (!_littleEndian)
            {
                Array.Reverse(data, 0, 4);
            }
            float result = BitConverter.ToSingle(data, 0);
            byte[] array = new byte[data.Length - 4];
            Array.Copy(data, 4, array, 0, array.Length);
            data = array;
            return result;
        }

        public long UnMarshalLong()
        {
            if (!_littleEndian)
            {
                Array.Reverse(data, 0, 8);
            }
            long result = BitConverter.ToInt64(data, 0);
            byte[] array = new byte[data.Length - 8];
            Array.Copy(data, 8, array, 0, array.Length);
            data = array;
            return result;
        }

        public ushort UnMarshalUInt16()
        {
            if (!_littleEndian)
            {
                Array.Reverse(data, 0, 2);
            }
            ushort result = BitConverter.ToUInt16(data, 0);
            byte[] array = new byte[data.Length - 2];
            Array.Copy(data, 2, array, 0, array.Length);
            data = array;
            return result;
        }

        public string UnMarshalWString()
        {
            int num = UnMarshalByte();
            return Encoding.Unicode.GetString(UnMarshalBytes(num));
        }

        public void MarshalWString(string text)
        {
            int num = text.Length * 2;
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            Marshal((byte)num);
            Marshal(bytes);
        }

    }
}