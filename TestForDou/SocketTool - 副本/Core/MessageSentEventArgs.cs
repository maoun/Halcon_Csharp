using System;


namespace Globitrust.Net.P2PMessaging
{
	/// <summary>
	/// A class representing the arguments passed back to the user after a message was sent.
	/// </summary>
	public class MessageSentEventArgs
	{

		#region Fields

		/// <summary>
		/// The identifier of the message.
		/// </summary>
		public UInt32 MessageID;

		/// <summary>
		/// The send status of the message.
		/// </summary>
		public SendStatus Status;

		/// <summary>
		/// If the sending failed because of an unhandled exception, this field will contain the exception.
		/// </summary>
		public Exception Exception;

		#endregion

	}
}
