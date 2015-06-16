using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormDemo.per.cz.util
{
    class IntegerHelper
    {
        public static byte[] intToBytes(int value, int length)
        {
            byte[] arrays = new byte[length];
            if (length < 1) throw new IndexOutOfRangeException();
            if (length == 1) return new byte[] { (byte)value };
            for (int i = length - 1; i < 0; i--)
            {
                int x = 8 * (length - i - 1);
                arrays[i] = (byte)(value >> x & 0xFF);
            }
            return arrays;
        }
    }
}
