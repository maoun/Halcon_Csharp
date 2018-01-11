/*************************************************************************************
		* CLR版本 ：       4.0.30319.42000
		* 类 名 称：       HexToAscii
		* 机器名称：       AFOHC-608062000
		* 命名空间：       检测有无
		* 文 件 名：       HexToAscii
		* 创建时间：       2017/12/5 9:59:02
		* 作    者：       Mao
		* 说    明：       进制转换
		* 修改时间：
		* 修 改 人：
*************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 检测有无
{
    class HexToAscii
    {
        public static byte[] HexToAsci(string strHex)
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
                return buff;
            }
            catch
            {
                //MessageBox.Show("不是十六进制数");
                //string result = null;
                return null;
            }
        }
        public static string AsciiToHex(string strAsscii)
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
