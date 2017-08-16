using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexToAsscii
{
    class HexToAsscii
    {
        public static string HexToAsci(string strHex)
        {
            //hex转ascii           
            strHex = strHex.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");//去掉换行和回车
            try
            {
                byte[] buff = new byte[strHex.Length / 2];
                int index = 0;
                for (int i = 0; i < strHex.Length; i += 2)
                {
                    buff[index] = Convert.ToByte(strHex.Substring(i, 2), 16);
                    ++index;
                }
                string result = Encoding.Default.GetString(buff);
                return result;
            }
            catch
            {
                Console.WriteLine("不是十六进制数字");
                return "";
            }
        }

        public static string AssciiToHex(string strAsscii)
        {
            ////ascii转hex
            //ascii转字符串
            byte[] array = System.Text.Encoding.ASCII.GetBytes(strAsscii);
            string strB = System.Text.Encoding.ASCII.GetString(array);
            //字符串转hex
            byte[] bytes = System.Text.Encoding.Default.GetBytes(strB);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
            }
            return str;
        }
    }
}
