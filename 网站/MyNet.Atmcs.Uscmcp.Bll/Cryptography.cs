using System.Text;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class Cryptography
    {
        /// <summary>
        /// @作者: fengd
        /// @版本号：1.0
        /// @函数说明：给长度为1到20个字符的字符串加密
        /// @参数：String originString:待加密字符串
        /// @返回：String 字符串：加密后的64位字符串
        /// @创建日期：2007-10-12
        /// @修改者：zhaobin
        /// @修改日期：2008-02-07
        /// </summary>
        public static string Encrypt(string originString)
        {
            StringBuilder encryptedArray = new StringBuilder();
            sbyte[] originByteArray = Common.ToSByteArray(Common.ToByteArray(originString));
            int arrayLen = originByteArray.Length;
            int index = 0;
            for (int i = 0; i < arrayLen; i++)
            {
                sbyte temp = originByteArray[i];
                int encryptingKey = randomNum(65, 90);
                if ((temp + encryptingKey - 33) < 91)
                {
                    encryptedArray.Append((char)(temp + encryptingKey - 33));
                    encryptedArray.Append((char)randomNum(65, 70));
                }
                else if ((temp + encryptingKey - 59) < 91)
                {
                    encryptedArray.Append((char)(temp + encryptingKey - 59));
                    encryptedArray.Append((char)randomNum(71, 75));
                }
                else if ((temp + encryptingKey - 85) < 91)
                {
                    encryptedArray.Append((char)(temp + encryptingKey - 85));
                    encryptedArray.Append((char)randomNum(76, 80));
                }
                else if ((temp + encryptingKey - 111) < 91)
                {
                    encryptedArray.Append((char)(temp + encryptingKey - 111));
                    encryptedArray.Append((char)randomNum(81, 85));
                }
                else if ((temp + encryptingKey - 137) < 91)
                {
                    encryptedArray.Append((char)(temp + encryptingKey - 137));
                    encryptedArray.Append((char)randomNum(86, 90));
                }
                encryptedArray.Append((char)(encryptingKey));
                //UPGRADE_TODO: Method 'java.lang.Integer.parseInt' 已转换为具有不同行为的 'System.Convert.ToInt32'。. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
                temp &= (sbyte)System.Convert.ToInt32("7F", 16);
                index = 3 * (i + 1);
            }
            for (int i = index; i < 62; i++)
            {
                int randomKey = randomNum(65, 90);
                encryptedArray.Append((char)(randomKey));
            }
            if ((index + 65) < 91)
            {
                encryptedArray.Append((char)(index + 65));
                encryptedArray.Append((char)randomNum(65, 73));
            }
            else if ((index + 65) < 117)
            {
                encryptedArray.Append((char)(index + 39));
                encryptedArray.Append((char)randomNum(74, 82));
            }
            else if ((index + 65) < 143)
            {
                encryptedArray.Append((char)(index + 13));
                encryptedArray.Append((char)randomNum(83, 90));
            }
            return (encryptedArray.ToString());
        }

        /// <summary>
        /// @作者: fengd
        /// @版本号：1.0
        /// @函数说明：给字符串解密
        /// @参数：String encryptedString:64位长的待解密字符串
        /// @返回：String 字符串：解密后的字符串
        /// @创建日期：2007-10-12
        /// @修改者：zhaobin
        /// @修改日期：2008-02-07
        /// </summary>
        public static string Decrypt(string encryptedString)
        {
            sbyte[] fullString = Common.ToSByteArray(Common.ToByteArray(encryptedString));
            int trueEncStringLen = 0;
            if (fullString[63] > 64 && fullString[63] < 74)
            {
                trueEncStringLen = fullString[62] - 65;
            }
            else if (fullString[63] > 73 && fullString[63] < 83)
            {
                trueEncStringLen = fullString[62] - 39;
            }
            else if (fullString[63] > 82 && fullString[63] < 91)
            {
                trueEncStringLen = fullString[62] - 13;
            }
            StringBuilder originString = new StringBuilder();
            for (int i = 0; i < trueEncStringLen; )
            {
                int originer = 0;
                int replacer = fullString[i];
                int ranEle = fullString[i + 2];
                if (fullString[i + 1] > 64 && fullString[i + 1] < 71)
                {
                    originer = replacer - ranEle + 33;
                    originString.Append((char)originer);
                }
                else if (fullString[i + 1] > 70 && fullString[i + 1] < 76)
                {
                    originer = replacer - ranEle + 59;
                    originString.Append((char)originer);
                }
                else if (fullString[i + 1] > 75 && fullString[i + 1] < 81)
                {
                    originer = replacer - ranEle + 85;
                    originString.Append((char)originer);
                }
                else if (fullString[i + 1] > 80 && fullString[i + 1] < 86)
                {
                    originer = replacer - ranEle + 111;
                    originString.Append((char)originer);
                }
                else if (fullString[i + 1] > 85 && fullString[i + 1] < 91)
                {
                    originer = replacer - ranEle + 137;
                    originString.Append((char)originer);
                }
                i = i + 3;
            }
            return (originString.ToString());
        }

        /// <summary>
        /// @作者: fengd
        /// @版本号：1.0
        /// @函数说明：得到定制范围内的一个随机整数
        /// @参数：int min ：最小值 || int max ：最大值
        /// @返回：int 整型值 ：位于闭区间内的一个任意整数
        /// @创建日期：2007-10-12
        /// @修改者：zhaobin
        /// @修改日期：2008-02-07
        /// </summary>
        public static int randomNum(int min, int max)
        {
            if (min >= max)
            {
                return 0;
            }
            else
            {
                int range = max - min;
                //UPGRADE_WARNING: 在 C# 中，收缩转换可能产生意外的结果。. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
                int result = (int)(range * (Common.Random.NextDouble()) + min);
                return result;
            }
        }


      


    }
}