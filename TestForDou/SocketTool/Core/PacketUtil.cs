using System;
using System.IO;
using System.Net;

namespace SocketTool.Core
{
    public class PacketUtil
    {
        protected BinaryWriter binWriter;

        public int TotalPacketLength {get;set;}

        public Byte[] PacketBytes { get; set; }

        private static int PacketLength = 1024;

        public BinaryWriter beginWriteContent()
        {
            //PacketBytes = new Byte[PacketLength];
            MemoryStream memoryStream = new MemoryStream(PacketBytes);
            binWriter = new BinaryWriter(memoryStream);          

            return binWriter;
        }

        public void Write(Byte[] data, int len)
        {
            TotalPacketLength += len;
            binWriter.Write(data, 0, len);
        }

        //固定长度的字符长，如果不定长，则补零
        public int Write(string str, int length)
        {
            int len = 0;
            if (str != null)
            {
                if (str.Length > length)
                    str = str.Substring(0, length);
                len = Write(str);
            }
            while (len < length)
            {
                Write((byte)0);
                len++;
            }
            return length;
        }



        public void Write(Int32 data)
        {
            data = IPAddress.HostToNetworkOrder(data);
            binWriter.Write(data);

            TotalPacketLength += 4;
        }

        public void Write(Int16 data)
        {
            data = IPAddress.HostToNetworkOrder(data);
            binWriter.Write(data);

            TotalPacketLength += 2;
        }

        public void Write(UInt32 data)
        {
            Byte[] vBytes = BitConverter.GetBytes(data);
            //Array.Reverse(vBytes);
            binWriter.Write(vBytes);

            TotalPacketLength += 4;
        }

        public int Write(String str)
        {
            Byte[] strBytes = System.Text.Encoding.Default.GetBytes(str);
            int len = strBytes.Length;
            binWriter.Write(strBytes);

            TotalPacketLength += len;

            return len;
        }

        public void Write(Byte b)
        {
            binWriter.Write(b);

            TotalPacketLength += 1;
        }

        public int WriteAscII(string str)
        {
            char[] stringArray = str.ToCharArray();
            int len = stringArray.Length;
            binWriter.Write(stringArray);

            TotalPacketLength += len;
            return len;
        }

        public void Write(DateTime d)
        {
            Write((Int16)d.Year);
            binWriter.Write((Byte)d.Month);
            binWriter.Write((Byte)d.Day);
            binWriter.Write((Byte)d.Hour);
            binWriter.Write((Byte)d.Minute);
            binWriter.Write((Byte)d.Second);


            TotalPacketLength += 7;
        }


    }
}
