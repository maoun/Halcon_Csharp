using System;
using System.Net;


namespace Globitrust.Net.P2PMessaging
{
	/// <summary>
	/// A class representing the arguments passed back to the user after a message was sent.
	/// </summary>
	public class MessageReceivedEventArgs
	{

		#region Fields

		/// <summary>
		/// The identifier of the message.
		/// </summary>
		public UInt32 MessageID;

		/// <summary>
		/// The data that was received.
		/// </summary>
		public byte[] Data;

		/// <summary>
		/// The host that sent the message.
		/// </summary>
		public IPEndPoint RemoteHost;

		#endregion

	}
}
