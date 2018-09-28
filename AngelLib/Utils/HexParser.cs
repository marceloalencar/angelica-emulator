using System.Globalization;

namespace AngelLib.Utils
{
    public class HexParser
    {
        public static byte[] GetBytes(string input)
        {
            byte[] numArray = new byte[input.Length / 2];
            for (int i = 0; i < input.Length; i += 2)
            {
                numArray[i / 2] = byte.Parse(input.Substring(i, 2), NumberStyles.HexNumber);
            }
            return numArray;
        }
    }
}
