/*
 * Genesis Socket Server and Client
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 * 
 * Please see included license.txt file for information on redistribution and usage.
 */
#region Using directives

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;
using System.Security;

#endregion

namespace SocketTool.Udp
{
    internal class Util
    {
        /// <summary>
        /// Gets the method name of the method that called the method that called this one.
        /// </summary>
        /// <returns>Name of method as string</returns>
        public static string GetMethod(int skipframes)
        {
            StackFrame sf = new StackFrame(skipframes, true);
            return sf.GetMethod().Name + "()";
        }

        #region Byte/number conversion
        /// <summary>
        /// Converts a string to a byte array
        /// </summary>
        /// <param name="s">String to convert</param>
        /// <returns>Byte array made from the string</returns>
        public static byte[] StringToBytes(string s)
        {
            byte[] tmp = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
                tmp[i] = (byte)s[i];

            return tmp;
        }

        /// <summary>
        /// Converts a byte array to a string
        /// </summary>
        /// <param name="b">Byte array to convert</param>
        /// <returns>A string</returns>
        public static string BytesToString(byte[] b)
        {
            string retval = "";

            for (int i = 0; i < b.Length; i++)
                retval += (char)b[i];

            return retval;
        }

        /// <summary>
        /// Given an integer, returns an eight byte representation of that long
        /// i.e 65535 = "ÿÿ\0\0\0\0\0\0" (FF FF 00 00 00 00 00 00);
        /// </summary>
        public static string LongToBytes(long num)
        {
            byte[] ab = BitConverter.GetBytes(num);

            string retval = "";

            for (int i = 0; i < ab.Length; i++)
            {
                retval += (char)ab[i];
            }

            return retval;
        }

        /// <summary>
        /// Given a byte array uses the first 8 to make a long, and returns it.
        /// </summary>
        public static long BytesToLong(string input)
        {
            byte[] bytes = new byte[8];

            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)input[i];

            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// Given a short, returns a two byte representation in a string
        /// i.e 65535 = "ÿÿ" (FF FF);
        /// </summary>
        public static string ShortToBytes(short ch)
        {
            byte[] ab = BitConverter.GetBytes(ch);

            string retval = "";

            for (int i = 0; i < ab.Length; i++)
            {
                retval += (char)ab[i];
            }
            return retval;
        }

        /// <summary>
        /// Given a byte array uses the first 2 to make a long, and returns it.
        /// </summary>
        public static short BytesToShort(string input)
        {
            byte[] bytes = new byte[2];

            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)input[i];

            return BitConverter.ToInt16(bytes, 0);
        }


        /// <summary>
        /// Given an integer, returns a four byte representation of that integer
        /// i.e 65535 = "ÿÿ\0\0" (FF FF 00 00);
        /// </summary>
        public static string IntToBytes(int num)
        {
            byte[] ab = BitConverter.GetBytes(num);

            string retval = "";

            for (int i = 0; i < ab.Length; i++)
                retval += (char)ab[i];

            return retval;
        }

        /// <summary>
        /// Given a byte array uses the first 4 to make an integer, and returns it.
        /// </summary>
        public static int BytesToInt(string input)
        {
            byte[] bytes = new byte[4];

            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)input[i];

            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Converts an unsigned integer into its byte components
        /// </summary>
        /// <param name="num">Number to read</param>
        /// <returns>Bytes that make up the number</returns>
        public static string UintToBytes(uint num)
        {
            byte[] ab = BitConverter.GetBytes(num);

            StringBuilder sb = new StringBuilder(4);
            string retval = "";

            for (int i = 0; i < ab.Length; i++)
            {
                retval += (char)ab[i];
            }

            /*FileStream fst = new FileStream("c:\\test.log", FileMode.Append);
            BinaryWriter wrt = new BinaryWriter(fst);
            wrt.Write(tmp, 0, 4);
            wrt.Flush();
            wrt.Close();*/

            return retval;
        }

        /// <summary>
        /// Given a byte array uses the first 4 to make an unsigned integer, and returns it. 
        /// </summary>
        public static uint BytesToUint(string input)
        {
            byte[] bytes = new byte[4];

            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)input[i];

            return BitConverter.ToUInt32(bytes, 0);
        }
        #endregion

        /// <summary>
        /// Creates a command packet header
        /// </summary>
        /// <param name="seq_num">Sequence number of packet</param>
        /// <param name="flags">Command flags</param>
        /// <param name="opcode">Command OPCode</param>
        /// <param name="encrypt_key">Encryption key (use blank string for none)</param>
        /// <param name="fields">Command fields</param>
        /// <returns>The packet header</returns>
        public static string CreatePacketHeader(uint seq_num, byte flags, string opcode, string encrypt_key, string[] fields)
        {
            string retval = "";

            if (fields == null)
                fields = new string[0];

            retval += opcode;
            retval += UintToBytes(seq_num);
            retval += (char)flags;
            retval += ShortToBytes((short)fields.Length);
            if (fields.Length > 0)
                for (int i = 0; i < fields.Length; i++)
                    retval += ShortToBytes((short)fields[i].Length);

            //Add the constant string to check the encryption
            if (encrypt_key != "")
                retval += XORCrypt(UdpConsts.ENCRYPT_CHECK_STRING, encrypt_key);
            else
                retval += UdpConsts.ENCRYPT_CHECK_STRING;

            return retval;
        }

        /// <summary>
        /// Encrypts a string using XOR
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key">Encryption key</param>
        /// <returns>Encrypted or decrypted string</returns>
        public static string XORCrypt(string data, string key)
        {
			//Null key? Don't encrypt.
			if(key == "")
				return data;

            string retValue = "";

            int i = 0;
            int x = 0;

            int[] cipher = new int[data.Length];
            x = 0;

            for (i = 0; i < data.Length; i++)
            {
                retValue = retValue + (char)((data[i] ^ key[x]));
                cipher[i] = (data[i] ^ key[x]);
                x++;

                if (x >= key.Length)
                    x = 0;
            }
            return retValue;
        }

        /// <summary>
        /// Generates a random encryption key
        /// </summary>
        /// <returns>Randomly generated encryption key</returns>
        public static string GenerateEncryptionKey( )
        {
            int size = 40; //320 bit
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(26 * random.NextDouble() + 65));
                builder.Append(ch);
            }
            return builder.ToString();
        }

		/// <summary>
		/// Returns a string array containing all local IP addresses
		/// </summary>
		public static string[] GetLocalAddresses()
		{
			// Get host name
			string strHostName = Dns.GetHostName();

			// Find host by name
			IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);

			string[] retval = new string[iphostentry.AddressList.Length];

			int i = 0;
			foreach (IPAddress ipaddress in iphostentry.AddressList)
			{
				retval[i] = ipaddress.ToString();
				i++;
			}
			return retval;
		}
    }
}
