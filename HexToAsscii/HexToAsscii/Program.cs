using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexToAsscii
{
    class Program
    {
        static void Main(string[] args)
        {
            #region
            ////hex转ascii
            //while (true)
            //{
            //    //去掉空格
            //    Console.WriteLine("输入Hex：");
            //    string tmp = Console.ReadLine();
            //    CharEnumerator CEnumerator = tmp.GetEnumerator();
            //    string tmp2 = null;
            //    while (CEnumerator.MoveNext())
            //    {

            //        byte[] array = new byte[1];

            //        array = System.Text.Encoding.ASCII.GetBytes(CEnumerator.Current.ToString());

            //        int asciicode = (short)(array[0]);

            //        if (asciicode != 32)
            //        {

            //            tmp2 += CEnumerator.Current.ToString();

            //        }

            //    }
                
            //    try
            //    {
            //        byte[] buff = new byte[tmp2.Length / 2];
            //        int index = 0;
            //        for (int i = 0; i < tmp2.Length; i += 2)
            //        {
            //            buff[index] = Convert.ToByte(tmp2.Substring(i, 2), 16);
            //            ++index;
            //        }
            //        string result = Encoding.Default.GetString(buff);
            //        Console.Write(result + "\n");
            //    }
            //    catch
            //    {
            //        Console.WriteLine("不是十六进制数字");
            //    }


            //}
           
            #endregion
            #region
            //////ascii转hex
            ////ascii转字符串
            //Console.WriteLine("输入内容");
            //string tmp = Console.ReadLine();
            //byte[] array = System.Text.Encoding.ASCII.GetBytes(tmp);
            //string strB = System.Text.Encoding.ASCII.GetString(array);
            ////字符串转hex
            //byte[] bytes = System.Text.Encoding.Default.GetBytes(strB);
            //string str = "";
            //for (int i = 0; i < bytes.Length; i++)
            //{
            //  str += string.Format("{0:X}", bytes[i]);
            //}
            //Console.WriteLine(str);
            //Console.ReadKey();
            ////int iValue;
            ////byte[] bs;
            ////string sValue;

            ////iValue = Convert.ToInt32("0C", 16); // 16进制->10进制
            ////bs = System.BitConverter.GetBytes(iValue); //int->byte[]
            ////sValue = System.Text.Encoding.ASCII.GetString(bs);   //byte[]-> ASCII
            #endregion
            while(true)
            {
                string a = Console.ReadLine();
                string b = HexToAsscii.AssciiToHex(a);
                Console.WriteLine(b);
            }         
        }
    }
}
