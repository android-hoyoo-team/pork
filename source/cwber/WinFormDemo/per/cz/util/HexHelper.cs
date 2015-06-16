using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormDemo.per.cz.util
{
    class HexHelper
    {
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }
        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().ToUpper();
        }
        public static byte HexStringToByte(string s)
        {
            if (s.StartsWith("0x"))
            {
                byte x = Convert.ToByte(s.Substring(2, 1), 16);
                byte y = Convert.ToByte(s.Substring(3, 1), 16);
                return (byte)(x * 16 + y);
            }
            throw new FormatException();
        }
    }
}
