using System;


namespace Globitrust.Net.P2PMessaging
{
	/// <summary>
	/// The method definition for send completion handling.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="args">The arguments.</param>
	public delegate void OnMessageSent(object sender, MessageSentEventArgs args);

	/// <summary>
	/// The method definition for message receipt handling.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="args">The arguments.</param>
	public delegate void OnMessageReceived(object sender, MessageReceivedEventArgs args);
}
