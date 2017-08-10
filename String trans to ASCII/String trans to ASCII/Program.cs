using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace String_trans_to_ASCII
{
    class Program
    {
        static void Main(string[] args)
        {
       
       
        #region
        //字符串转ascii
            Console.WriteLine("输入内容");
            string tmp = Console.ReadLine();
            byte[] array = System.Text.Encoding.ASCII.GetBytes(tmp);
            string ASCIIstrA = null;
            for (int i = 0; i < array.Length; i++)
            {
                int asciicode = (int)(array[i]);
                ASCIIstrA += Convert.ToString(asciicode);//字符串ASCIIstr2 为对应的ASCII字符串
            }
            Console.WriteLine("结果："+ASCIIstrA);
            Console.ReadKey();        
        #endregion

        #region
        ////ascii转字符串
        //    Console.WriteLine("输入内容");
        //    string tmp = Console.ReadLine();
        //    byte[] array = System.Text.Encoding.ASCII.GetBytes(tmp);
        //    string strB = System.Text.Encoding.ASCII.GetString(array);
        //    Console.WriteLine("结果："+strB);
        //    Console.ReadKey();
        #endregion
        }
    }
}
