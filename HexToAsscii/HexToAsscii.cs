/*************************************************************************************
  * CLR版本：       $clrversion$
  * 类 名 称：       $itemname$
  * 机器名称：       $machinename$
  * 命名空间：       $rootnamespace$
  * 文 件 名：       $safeitemname$
  * 创建时间：       $time$
  * 作    者：          xxx
  * 说   明：。。。。。
  * 修改时间：
  * 修 改 人：
 *************************************************************************************/
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
            while (true)
            {
                //去掉空格
                CharEnumerator CEnumerator = strHex.GetEnumerator();
                string tmp = null;
                while (CEnumerator.MoveNext())
                {
                    byte[] array = new byte[1];
                    array = System.Text.Encoding.ASCII.GetBytes(CEnumerator.Current.ToString());
                    int asciicode = (short)(array[0]);
                    if (asciicode != 32)
                    {
                        tmp += CEnumerator.Current.ToString();
                    }
                }
                try
                {
                    byte[] buff = new byte[tmp.Length / 2];
                    int index = 0;
                    for (int i = 0; i < tmp.Length; i += 2)
                    {
                        buff[index] = Convert.ToByte(tmp.Substring(i, 2), 16);
                        ++index;
                    }
                    string result = Encoding.Default.GetString(buff);
                    return result;
                }
                catch
                {
                    Console.WriteLine("不是十六进制数字");
                }
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
