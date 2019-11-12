using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PWClientHashGrabber2
{
    class Util
    {
        internal static string GetHash(string selectedPath)
        {
            StringBuilder sbResult = new StringBuilder();
            using (Stream str = new FileStream(selectedPath + "\\elements.data", FileMode.Open))
            {
                byte[] array = new byte[4];
                str.Read(array, 3, 1);
                str.Read(array, 2, 1);
                str.Read(array, 1, 1);
                str.Read(array, 0, 1);
                byte[] value = array.ToArray();
                sbResult.Append(BitConverter.ToString(value).Replace("-", string.Empty));
            }
            using (Stream str = new FileStream(selectedPath + "\\tasks.data", FileMode.Open))
            {
                byte[] array = new byte[1];
                str.Seek(4L, SeekOrigin.Begin);
                str.Read(array, 0, 1);
                byte[] value = array.ToArray();
                sbResult.Append(BitConverter.ToString(value).Replace("-", string.Empty));
            }
            using (Stream str = new FileStream(selectedPath + "\\gshop.data", FileMode.Open))
            {
                byte[] array = new byte[4];
                str.Read(array, 3, 1);
                str.Read(array, 2, 1);
                str.Read(array, 1, 1);
                str.Read(array, 0, 1);
                byte[] value = array.ToArray();
                sbResult.Append(BitConverter.ToString(value).Replace("-", string.Empty));
            }
            using (Stream str = new FileStream(selectedPath + "\\gshop1.data", FileMode.Open))
            {
                byte[] array = new byte[4];
                str.Read(array, 3, 1);
                str.Read(array, 2, 1);
                str.Read(array, 1, 1);
                str.Read(array, 0, 1);
                byte[] value = array.ToArray();
                sbResult.Append(BitConverter.ToString(value).Replace("-", string.Empty));
            }
            using (Stream str = new FileStream(selectedPath + "\\gshop2.data", FileMode.Open))
            {
                byte[] array = new byte[4];
                str.Read(array, 3, 1);
                str.Read(array, 2, 1);
                str.Read(array, 1, 1);
                str.Read(array, 0, 1);
                byte[] value = array.ToArray();
                sbResult.Append(BitConverter.ToString(value).Replace("-", string.Empty));
            }

            byte[] result = Encoding.ASCII.GetBytes(sbResult.ToString().ToLower());
            return BitConverter.ToString(result).Replace("-", string.Empty);
        }
    }
}
