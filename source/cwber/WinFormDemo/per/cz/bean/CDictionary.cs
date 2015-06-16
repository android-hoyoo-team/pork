using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace per.cz.bean
{
    public class CDictionary<T,K> : Dictionary<T,K>
    {
        public int GetInt(T key)
        {
            int result = 0;
            if (this.ContainsKey(key)) {
                try
                {
                    result = Int32.Parse(this[key].ToString());
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            throw new KeyNotFoundException(key.ToString());
        }

        public string GetString(T key)
        {
            string result = null;
            if (this.ContainsKey(key))
            {
                try
                {
                    result = this[key].ToString();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            throw new KeyNotFoundException(key.ToString());
        }
        public byte GetByte(T key)
        {
            byte result = 0;
            if (this.ContainsKey(key))
            {
                try
                {
                    result = Byte.Parse(this[key].ToString());
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            throw new KeyNotFoundException(key.ToString());
        }

        public bool GetBoolean(T key) {
            bool result = false;
            if (this.ContainsKey(key))
            {
                try
                {
                    result = Boolean.Parse(this[key].ToString());
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            throw new KeyNotFoundException(key.ToString());
        }
        public byte[] GetByteArray(T key)
        {
            byte[] result = null;
            if (this.ContainsKey(key))
            {
                try
                {
                    BinaryFormatter   formatter   =   new   BinaryFormatter();
                    MemoryStream   rems   =   new   MemoryStream();
                    formatter.Serialize(rems,   this[key]);
                    result =    rems.GetBuffer();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            throw new KeyNotFoundException(key.ToString());
        }
    }
    public class KeyNotFoundException : Exception {
        public string Message;
        public KeyNotFoundException(string key) {
            this.Message = "字典中没有键：" + key;
        }
    }
}
