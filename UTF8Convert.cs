using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 加密大师
{
    class UTF8Convert
    {
        /// <summary>
        /// 字符串转换为u38u24u1911u5432形式
        /// </summary>
        /// <param name="original"></param>
        /// <param name="splitChars"></param>
        /// <returns></returns>
        static string Chars2UTF8ValuesStr(string original,string splitChars="u")
        {
            byte[] originalBytes = Encoding.UTF8.GetBytes(original);
            string target = "";
            foreach (byte b in originalBytes)
            {
                target += string.Format("{0}{1:x}", splitChars,b);
            }
            return target;
        }

        /// <summary>
        /// u38u24u1911u5432转换为字符串形式
        /// </summary>
        /// <param name="original"></param>
        /// <param name="splitChars"></param>
        /// <returns></returns>
        static string UTF8ValuesStr2Chars(string original,string splitChars="u")
        {
            List<byte> chList = new List<byte>();
            string[] spilts = original.Split(splitChars.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            foreach (string chip in spilts)
                chList.Add(Convert.ToByte(chip, 16));
            
            return Encoding.UTF8.GetString(chList.ToArray());
        }
    }
}
