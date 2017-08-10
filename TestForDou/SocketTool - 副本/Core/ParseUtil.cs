using System;
using System.IO;
using System.Net;

namespace SocketTool.Core
{
    public class ParseUtil
    {
        protected BinaryReader reader;

        public BinaryReader newReader(Byte[] byBuffer, int nReceived)
        {
            MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
            reader = new  BinaryReader(memoryStream);
            return reader;
        }

        public Byte Parse()
        {
            return reader.ReadByte();
        }

        //从字节流中读取时间
        public DateTime ParseDateTime()
        {
            //Int32 year = ParseInt32();// IPAddress.NetworkToHostOrder(reader.ReadInt32());


            int year = reader.ReadInt32();



            Byte month = reader.ReadByte();
            Byte day = reader.ReadByte();
            Byte hour = reader.ReadByte();
            Byte minute = reader.ReadByte();
            Byte sec = reader.ReadByte();

            return new DateTime(year, month, day, hour, minute, sec);
        }

        public UInt32 ParseUInt32()
        {
            Byte[] vBytes = reader.ReadBytes(4);

            Array.Reverse(vBytes);

            return BitConverter.ToUInt32(vBytes, 0);
        }

        public Int32 ParseInt32()
        {
            int flow = reader.ReadInt32();
            return IPAddress.NetworkToHostOrder(flow);
        }
        public Int16 ParseInt16()
        {
            Int16 test = reader.ReadInt16();

            //byte[] bytes = BitConverter.GetBytes(test);

            //string str = Convert.ToString(test, 2);

            Int16 test2 = IPAddress.NetworkToHostOrder(test);

            //bytes = BitConverter.GetBytes(test2);

            //str = Convert.ToString(test2, 2);

            return test2;
        }


        public Int32 ParseIntEx()
        {
            int flow = reader.ReadInt32();
            return flow;
        }

        public string ParseString(int len)
        {
            Byte[] bytes = reader.ReadBytes(len);

            return ParseString(bytes, len);
        }

        public static string ParseString(byte[] bytes, int len)
        {
            if (bytes[0] != '\0')
            {
                string str = System.Text.Encoding.Default.GetString(bytes);

                int index = str.IndexOf('\0');

                if (index > 0)
                    return str.Substring(0, index);
                else
                    return str;

            }
            return "";
        }

        public static string ToHexString(byte[] bytes, int len)
        {
            return ToHexString(bytes, 0, len);
        }

        public static string ToHexString(byte[] bytes, int start, int len)
        {
            string strReturn = "";
            for (int i = start; i < (start + len); i++)
            {
                byte bt = bytes[i];
                strReturn += bt.ToString("x2");
            }
            return strReturn;
        }


        public static byte[] ToByesByHex(string hexStr)
        {
            int len = hexStr.Length;

            byte[] data = new byte[len / 2];            

            for(int k = 0; k < data.Length; k++)
            {
                data[k] = Convert.ToByte(hexStr.Substring(k * 2, 2), 16);
                //k = k* 2;
            }

            return data;
        }
    }
}
