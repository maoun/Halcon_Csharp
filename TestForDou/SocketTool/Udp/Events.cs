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

#endregion

namespace SocketTool.Udp
{
    public delegate void DebugHandler(object o, DebugEventArgs e);
    public delegate void ReceivedHandler(object o, ReceivedEventArgs e);
    public delegate void ListenHandler(object o, ListenEventArgs e);
    public delegate void SocketErrorHandler(object o, SocketEventArgs e);
    public delegate void IncomingCommandHandler(object o, CommandEventArgs e);
    public delegate void ConnectionAuthHandler(object o, ConnectionAuthEventArgs e);
    public delegate void SendLoginHandler(object o, LoginSendEventArgs e);
    public delegate void ConnectionStateChangeHandler(object o, ConnectionStateChangeEventArgs e);
	public delegate void AuthenticatedHandler(object o, AuthenticatedEventArgs e);
	public delegate void RequestTimedOutHandler(object o, RequestTimedOutEventArgs e);

    /// <summary>
    /// Holds the debug event arguments.
    /// </summary>
    public class DebugEventArgs : EventArgs
    {
        public readonly string DebugMessage;

        public DebugEventArgs(string DebugMsg)
        {
            DebugMessage = DebugMsg;
        }
    }

    /// <summary>
    /// Holds the received data event arguments.
    /// </summary>
    public class ReceivedEventArgs : EventArgs
    {
        public readonly IPEndPoint RemoteHost;
        public readonly string Data;

        public ReceivedEventArgs(IPEndPoint Remote, string ReceivedData)
        {
            Data = ReceivedData;
            RemoteHost = Remote;
        }
    }

    /// <summary>
    /// Holds the listen state change event arguments.
    /// </summary>
    public class ListenEventArgs : EventArgs
    {
        /// <summary>
        /// True if UDP server is listening for packets
        /// </summary>
        public readonly bool Listening;

        public ListenEventArgs(bool islistening)
        {
            Listening = islistening;
        }
    }

    /// <summary>
    /// Holds the listen socket error event arguments.
    /// </summary>
    public class SocketEventArgs : EventArgs
    {
        public readonly int ErrorCode;
        public readonly string Message;

        public SocketEventArgs(int code, string msg)
        {
            ErrorCode = code;
            Message = msg;
        }
    }

    /// <summary>
    /// Holds the incoming command event arguments.
    /// </summary>
    public class CommandEventArgs : EventArgs
    {
        public readonly IConnection Sender;
        public readonly ICommand SentCommand;
        public readonly IPEndPoint SenderEndPoint;

        public CommandEventArgs(ICommand c, IConnection cn, IPEndPoint rep)
        {
            SentCommand = c;
            Sender = cn;
            SenderEndPoint = rep;
        }
    }

    /// <summary>
    /// Holds the login sending event arguments
    /// </summary>
    public class LoginSendEventArgs : EventArgs
    {
        /// <summary>
        /// The connection object representing the server requesting login details.
        /// </summary>
        public readonly IConnection ServerConnection;

        /// <summary>
        /// False if the server is unable to accept the connection, else true.
        /// </summary>
        public bool Connected;

        /// <summary>
        /// If the connection could not be accepted, holds the reason for rejection.
        /// </summary>
        public string Reason;

        public LoginSendEventArgs(IConnection cn, bool cd, string r)
        {
            ServerConnection = cn;
            Connected = cd;
            Reason = r;
        }
    }

    /// <summary>
    /// Holds the connection auth event arguments.
    /// </summary>
    public class ConnectionAuthEventArgs : EventArgs
    {
        /// <summary>
        /// If true, allows the connection to be established
        /// </summary>
        public bool AllowConnection;

        private string m_disallow;
        /// <summary>
        /// If allow is set to false, this can be set to give a reason why
        /// the reason will be sent to the remote host that requested the connection.
        /// Max length is 200 characters.
        /// </summary>
        public string DisallowReason
        {
            get { return m_disallow; }
            set
            {
                m_disallow = value;

                if (m_disallow.Length > 200)
                    m_disallow = m_disallow.Substring(0, 200);
            }
        }

        /// <summary>
        /// Gets the conection entry of the remote host
        /// </summary>
        public readonly IConnection ClientConnection;

        /// <summary>
        /// Command sent to the UDP server containing the auth data
        /// </summary>
        public readonly ICommand AuthCommand;

        public ConnectionAuthEventArgs(IConnection conn, ICommand cmd)
        {
            AllowConnection = false;
            DisallowReason = "Rejected by remote host software.";
            ClientConnection = conn;
            AuthCommand = cmd;
        }
    }

    /// <summary>
    /// Holds the login sending event arguments
    /// </summary>
    public class ConnectionStateChangeEventArgs : EventArgs
    {
        /// <summary>
        /// The actual connection object.
        /// </summary>
        public readonly IConnection Connection;

        /// <summary>
        /// Value that determines if the connection has become active (true) or closed (false).
        /// </summary>
        public readonly bool Connected;

        /// <summary>
        /// If a connection has closed, the reason for the closure.
        /// </summary>
        public readonly string Disconnect_Reason;

        public ConnectionStateChangeEventArgs(IConnection cn, bool ced, string reason)
        {
            Connection = cn;
            Connected = ced;
            Disconnect_Reason = reason;
        }
    }

    /// <summary>
    /// Holds the authenticated event arguments
    /// </summary>
    public class AuthenticatedEventArgs : EventArgs
    {
        public readonly IConnection ServerConnection;

        /// <summary>
        /// True if authentication was successful, else false.
        /// </summary>
        public readonly Boolean Accepted;

        /// <summary>
        /// If authentication failed, the reason for the failure.
        /// </summary>
        public readonly string Reason;

        public AuthenticatedEventArgs(IConnection cn, bool a, string r)
        {
            ServerConnection = cn;
            Accepted = a;
            Reason = r;
        }
    }

	/// <summary>
	/// Holds the connection request timed out event args
	/// </summary>
	public class RequestTimedOutEventArgs : EventArgs
	{
		public readonly string ServerIP;
		public readonly int ServerPort;
        public readonly string RequestID;
		
		public RequestTimedOutEventArgs(string IP, int Port, string RID)
		{
			ServerIP = IP;
			ServerPort = Port;
            RequestID = RID;
		}
	}
}
