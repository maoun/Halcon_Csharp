using System;


namespace Globitrust.Net.P2PMessaging
{
	/// <summary>
	/// An enumeration of possible send statuses.
	/// </summary>
	public enum SendStatus
	{
		/// <summary>
		/// The send failed.
		/// </summary>
		Failed = 1,

		/// <summary>
		/// The message has not been sent yet.
		/// </summary>
		NotYetSent = 2,

		/// <summary>
		/// The send was successful and the delivery has been confirmed.
		/// </summary>
		Delivered = 3,

		/// <summary>
		/// The message was sent.
		/// </summary>
		Sent = 4
	}
}
