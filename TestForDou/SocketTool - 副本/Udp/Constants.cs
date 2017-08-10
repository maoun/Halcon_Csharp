/*
 * Genesis Socket Server and Client
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 * 
 * Please see included license.txt file for information on redistribution and usage.
 */
#region Using directives

using System;
using System.Text;

#endregion

namespace SocketTool.Udp
{
	/// <summary>
	/// Enumeration containing filter flags for the broadcast methods.
	/// </summary>
	[Flags]
	public enum BroadcastFilter : int
	{
		None		= 0,					//Filter out everything
		Servers		= 1,					//Send to servers we are connected to.
		Clients		= 2,					//Send to clients connected to us.
		All			= Servers | Clients,	//Send to both servers and clients (every connection).
		AuthedOnly	= 4,					//Only send to authed clients or servers we are authed with.
	}

    public class UdpConsts
    {
        //Command packet handling constants
        public static int MAX_EXCEPTIONS = 10;				//Maximum concurrent exceptions in receive loop before it shuts down
        public static int MAX_COMMAND_LEN = 512;			//Maimum command packet length (larger are split up)
        public static int MAX_FIELD_LEN = 2000;				//Maximum field length in a command
        public static int MAX_FIELDS = 50;					//Max number of fields allowed in a command
        public static int CONNECTION_TIMEOUT = 15;			//Seconds between pings before a connection is deemed timed out
        public static int CONNECTION_CLEAN_TIME = 3;		//Seconds between timed out connection cleanups
        public static int CONNECTION_PING_DELAY = 5;		//Seconds between pinging server connections
        public static int CONNECTION_RELIABLE_RETRY = 2;	//Seconds between resending reliable packets
		public static int CONNECTION_TIMEOUT_CHECK = 2;
		public static int CONNECTION_TIMEOUT_TIME = 5;
		public static int BAN_CHECK_TIME = 20;				//Seconds between checks of expired bans.
		public static int DEFAULT_MAX_WARNINGS = 3;			//Number of warnings an IP can get before being banned.
		public static int MAX_RELIABLE_QUEUED = 25;			//Maximum number of reliable commands that can be queued.

        //Used to ensure the encryption key was ok.
        public static string ENCRYPT_CHECK_STRING = "GC"; //Sent with every packet to validate the encryption key

        //Internal standard command opcodes
        public static string OPCODE_COMPOUNDPIECE = "00";
        public static string OPCODE_CONNECTIONREQUEST = "01";
        public static string OPCODE_CONNECTIONACK = "02";
        public static string OPCODE_LOGINDETAILS = "03";
        public static string OPCODE_LOGINACK = "04";
        public static string OPCODE_DISCONNECT = "05";
        public static string OPCODE_PING = "06";
        public static string OPCODE_RELIABLEACK = "07";

        //Command packet flags
        public static byte FLAGS_NONE = 0;
        public static byte FLAGS_CONNECTIONLESS = 1;
        public static byte FLAGS_ENCRYPTED = 2;
        public static byte FLAGS_COMPOUNDPIECE = 4;
        public static byte FLAGS_COMPOUNDEND = 8;
        public static byte FLAGS_RELIABLE = 16;
        public static byte FLAGS_SEQUENCED = 32;

        //Function return constants
        public static int UDP_OK = 0;
        public static int UDP_FAIL = 1000;
        public static int UDP_NOTFOUND = 1001;
        public static int UDP_ALREADYCONNECTED = 1002;
        public static int UDP_ALREADYINQUEUE = 1003;
        public static int UDP_UNRELIABLETOOLONG = 1004;
        public static int UDP_RELIABLENODESTINATION = 1005;
		public static int UDP_UNABLETORESOLVE = 1006;
		public static int UDP_RELIABLEQUEUEFULL = 1007;

        //UDP engine states
        public static int UDP_STATE_IDLE = 0;
        public static int UDP_STATE_LISTENING = 1;
        public static int UDP_STATE_CLOSING = 2;
    }
}
