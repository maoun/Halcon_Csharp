/*
 * Genesis Socket Server and Client
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 * 
 * Please see included license.txt file for information on redistribution and usage.
 */

#region Using directives

using System;
using System.Collections;
using System.Text;
using System.Net;
using System.Threading;

#endregion

namespace SocketTool.Udp
{
    /// <summary>
    /// Represents a single connection to this server
    /// </summary>
    internal class Connection : IConnection
    {
		private ReliableQueue m_RQueue;
		private string m_EncryptionKey;
		private IPEndPoint m_RemoteEP;
		private uint m_LastSentPacket;
		private uint m_LastReceivedPacket;
		private uint m_LastSentPacketR;
		private uint m_LastReceivedPacketR;
		private uint m_LastReceivedPacketSeq;
		private bool m_Authed;
		private bool m_Server;
		private DateTime m_TimeoutTime;
		private object m_UserObject;
		private string m_RequestID;

        /// <summary>
        /// Reference to the main UDP engine object that created this connection.
        /// </summary>
        private CommonUdp m_Parent;

		/// <summary>
		/// Current compound command being built (a compound command is one made up of
		/// more than one reliable packet).
		/// </summary>
		private Command CompoundCommand;

		#region Public accessors
		/// <summary>
		/// Reliable packet queue used for sending packets reliably.
		/// </summary>
		public ReliableQueue RQueue
		{
			get{ return m_RQueue; }
			set{ m_RQueue = value; }
		}

		/// <summary>
		/// On a connection created at the client side, gets the request id generated when the connection was
		/// first requested to the server.
		/// </summary>
		public string RequestID
		{
			get{ return m_RequestID; }
			set{ m_RequestID = value; }
		}

        /// <summary>
        /// Symmetric encryption key used for this connection.
        /// </summary>
        public string EncryptionKey
		{
			get{ return m_EncryptionKey; }
			set{ m_EncryptionKey = value; }
		}

        /// <summary>
        /// An IP end point that contains the addressing information for the remote
        /// host that this connection points to.
        /// </summary>
        public IPEndPoint RemoteEP
		{
			get{ return m_RemoteEP; }
			set{ m_RemoteEP = value; }
		}

        /// <summary>
        /// Sequence number of the last unreliable packet sent on this connection.
        /// </summary>
        public uint LastSentPacket
		{
			get{ return m_LastSentPacket; }
			set{ m_LastSentPacket = value; }
		}

        /// <summary>
        /// Sequence number of the last unreliable packet received from this connection.
        /// </summary>
        public uint LastReceivedPacket
		{
			get{ return m_LastReceivedPacket; }
			set{ m_LastReceivedPacket = value; }
		}

        /// <summary>
        /// Sequence number of the last reliable packet sent on this connection.
        /// </summary>
        public uint LastSentPacketR
		{
			get{ return m_LastSentPacketR; }
			set{ m_LastSentPacketR = value; }
		}

        /// <summary>
        /// Sequence number of the last reliable packet received from this connection.
        /// </summary>
        public uint LastReceivedPacketR
		{
			get{ return m_LastReceivedPacketR; }
			set{ m_LastReceivedPacketR = value; }
		}

        /// <summary>
        /// Sequence number of the last unreliable sequenced packet received.
        /// </summary>
        public uint LastReceivedPacketSeq
		{
			get{ return m_LastReceivedPacketSeq; }
			set{ m_LastReceivedPacketSeq = value; }
		}

        /// <summary>
        /// True if this connection has authenticated with the remote host successfully.
        /// </summary>
        public bool Authed
		{
			get{ return m_Authed; }
			set{ m_Authed = value; }
		}

        /// <summary>
        /// If true, this is a server connection on the client (initiated by the local application)
        /// If false, this is a client connection on a server (initiated by a remote application)
        /// </summary>
        public bool Server
		{
			get{ return m_Server; }
			set{ m_Server = value; }
		}

        /// <summary>
        /// Timestamp that corresponds to when this connection
        /// will be deemed "timed out" by the system and removed.
        /// </summary>
        public DateTime TimeoutTime
		{
			get{ return m_TimeoutTime; }
			set{ m_TimeoutTime = value; }
		}

        /// <summary>
        /// Object that can be assigned to by a third party application for storing various data about a connection.
        /// </summary>
        public object UserObject
		{
			get{ return m_UserObject; }
			set{ m_UserObject = value; }
		}
		#endregion

		/// <summary>
		/// Initialises the connection object, setting variables to defaults.
		/// </summary>
		/// <param name="udp">Reference to the main core engine that created the connection.</param>
        public Connection(CommonUdp udp)
        {
            EncryptionKey = "";
            RemoteEP = null;
            LastSentPacket = 0;
            LastReceivedPacket = 0;
            LastSentPacketR = 1;
            LastReceivedPacketR = 0;
			LastReceivedPacketSeq = 0;
            Authed = false; //Either (server) client is authed or (client) is authed with server
            Server = false; //Connection is a server connection
            m_Parent = udp;
            TimeoutTime = DateTime.Now.AddSeconds(UdpConsts.CONNECTION_TIMEOUT);
            CompoundCommand = null;
			m_RequestID = "";

            RQueue = new ReliableQueue();
        }

		#region Raw Packet parsing
        /// <summary>
        /// Processes a raw command packet that has been received and identified as being
        /// sent to this connection.
        /// </summary>
        /// <param name="command_packet">Command packet data</param>
        public void ProcessCommandPacket(string command_packet)
        {
            try
            {
                int pcmd = 0;

                Command new_cmd = new Command();
                new_cmd.OPCode = command_packet.Substring(pcmd, 2);
                pcmd += 2;
                new_cmd.SequenceNum = Util.BytesToUint(command_packet.Substring(pcmd, 4));
                pcmd += 4;
                new_cmd.Flags = (byte)command_packet[pcmd];
                pcmd++;
                new_cmd.NumFields = Util.BytesToShort(command_packet.Substring(pcmd, 2));
                pcmd += 2;
                new_cmd.FieldSizes = new short[new_cmd.NumFields];
                for (short i = 0; i < new_cmd.NumFields; i++)
                {
                    new_cmd.FieldSizes[i] = Util.BytesToShort(command_packet.Substring(pcmd, 2));
                    pcmd += 2;
                }

                //Is a reliable packet? Send an acknowledgement back
                if ((new_cmd.Flags & UdpConsts.FLAGS_RELIABLE) > 0)
                {
                    m_Parent.DebugDump("Got a reliable packet (" + new_cmd.SequenceNum.ToString() + ") - sent acknowledgement to sender.");
                    SendUnreliableCommand(0, UdpConsts.OPCODE_RELIABLEACK, new string[] { new_cmd.SequenceNum.ToString() });

                    //Repeat reliable packet - sender must not have received our acknowledgement
                    if (LastReceivedPacketR == new_cmd.SequenceNum)
                    {
                        m_Parent.DebugDump("Repeated reliable packet - not processed.");
                        return;
                    }

                    //Update last received reliable command number
                    LastReceivedPacketR = new_cmd.SequenceNum;
                }
                else
                {
                    LastReceivedPacket = new_cmd.SequenceNum; //Update unreliable command number

                    if ((new_cmd.Flags & UdpConsts.FLAGS_SEQUENCED) > 0)
                    {
                        //If this sequenced packet has arrived too late (a newer one got there first) then don't process it
                        if (LastReceivedPacketSeq > new_cmd.SequenceNum)
                        {
                            m_Parent.DebugDump("Unreliable sequenced packet arrived out of order - ignored.");
                            return;
                        }

                        //Update the last sequenced packet number if we should
                        LastReceivedPacketSeq = new_cmd.SequenceNum;
                    }
                }

                //Encryption check
                if ((new_cmd.Flags & UdpConsts.FLAGS_ENCRYPTED) > 0)
                {
                    if (EncryptionKey != "")
                        if (Util.XORCrypt(command_packet.Substring(pcmd, 2), EncryptionKey) != UdpConsts.ENCRYPT_CHECK_STRING)
                        {
                            //!!BAD ENCRYPTION KEY
                            m_Parent.DebugDump("Received an encrypted packet but the stored encryption key failed to decrypt it!");
                            return;
                        }
                }
                pcmd += 2;

                //Populate the AllFields property of the command
                new_cmd.AllFields = command_packet.Substring(pcmd);

                if ((new_cmd.Flags & UdpConsts.FLAGS_RELIABLE) > 0)
                {
                    //Is this reliable packet a compound piece?
                    if ((new_cmd.Flags & UdpConsts.FLAGS_COMPOUNDPIECE) > 0)
                    {
                        //Assume this is the first piece of the split command
                        if (CompoundCommand == null)
                        {
                            m_Parent.DebugDump("Got first part of a compound command, stored.");
                            CompoundCommand = new Command();
                            CompoundCommand.AllFields = new_cmd.AllFields;
                            CompoundCommand.FieldSizes = new_cmd.FieldSizes;
                            CompoundCommand.Flags = new_cmd.Flags;

                            //Remove the compound piece flag (will be treated as a single packet)
                            CompoundCommand.Flags &= (byte)(~UdpConsts.FLAGS_COMPOUNDPIECE);

                            CompoundCommand.NumFields = new_cmd.NumFields;
                            CompoundCommand.SequenceNum = new_cmd.SequenceNum;
                            CompoundCommand.OPCode = new_cmd.OPCode;
                            return;
                        }
                        else
                        {
                            //Additional pieces have FLAGS_COMPOUNDPIECE set also....

                            m_Parent.DebugDump("Got an additional compound piece.");
                            //Is not the first nor last part so just addon the fields
                            CompoundCommand.AllFields += new_cmd.AllFields;
                            return;
                        }
                    }

                    //The last compound piece has FLAGS_COMPOUNDEND set.
                    if ((new_cmd.Flags & UdpConsts.FLAGS_COMPOUNDEND) > 0)
                    {
                        m_Parent.DebugDump("Got last compound piece - sending to command processing.");

                        //Add the fields on
                        CompoundCommand.AllFields += new_cmd.AllFields;

                        //Swap the incomplete part with the whole command
                        new_cmd = CompoundCommand;
                        CompoundCommand = null;
                    }
                }

                new_cmd.Initialize();

                //If the packet is encrypted decrypt the fields now...
                if ((new_cmd.Flags & UdpConsts.FLAGS_ENCRYPTED) > 0)
                {
                    new_cmd.AllFields = "";
                    //Decrypt the fields
                    for (int i = 0; i < new_cmd.NumFields; i++)
                    {
                        new_cmd.Fields[i] = Util.XORCrypt(new_cmd.Fields[i], EncryptionKey);

                        //Rebuild the allfields property with the decrypted field data
                        new_cmd.AllFields += new_cmd.Fields[i];
                    }
                }

                //Send to command processing
                ProcessCompletedCommand(new_cmd);
            }
			catch
            {
                m_Parent.DebugDump("Exception whilst parsing input from " + RemoteEP.ToString() + " as command, probably not a command. Ignoring.");
            }
        }
		#endregion

		#region Command processing
        /// <summary>
        /// This function is called internally when a command has been parsed and is ready to be processed
        /// by the UDP engine and possibly the host application.
        /// </summary>
        /// <param name="cmd">Object containing data about the command to be processed.</param>
        public void ProcessCompletedCommand(Command cmd)
        {
            //Client sent login details
            if (!Authed)
            {
                if (cmd.OPCode == UdpConsts.OPCODE_LOGINDETAILS)
                {
                    m_Parent.ConnectionAuthing(this, cmd);
                    return;
                }
            }

            //Remote host sent us a ping
            if (cmd.OPCode == UdpConsts.OPCODE_PING)
            {
                if(Authed)
                {
                    m_Parent.DebugDump("Received ping from " + this.RemoteEP.ToString());

                    UpdateTimeout();

                    if (!Server) //If this is not a connection to a server
                        SendUnreliableCommand(0, UdpConsts.OPCODE_PING, null);
                }
                return;
            }

            //Server sent acknowledgement of our connection
            if (cmd.OPCode == UdpConsts.OPCODE_LOGINACK)
            {
                if (cmd.Fields[0] == "OK")
                {
                    Authed = true;
                    m_Parent.DebugDump("Authenticated with " + this.RemoteEP.ToString() + " OK. Connected!");

                    //Send the authenticated event to the third party application
                    m_Parent.AuthenticatedWithConnection(this, true, "");
                }
                else
                {
                    m_Parent.DebugDump("Authentication to " + this.RemoteEP.ToString() + " Failed!");

                    //Send the not authed event
                    m_Parent.AuthenticatedWithConnection(this, false, cmd.Fields[1]);

                    //Disconnect the connection (but don't send a disconnection packet back)
                    m_Parent.RemoveConnection(this, false, cmd.Fields[1]);
                }
                return;
            }

            //Remote host disconnected from us
            if (cmd.OPCode == UdpConsts.OPCODE_DISCONNECT)
            {
                m_Parent.RemoveConnection(this, false, cmd.Fields[0]);
                return;
            }

            //Reliable packet acknowledgement
            if (cmd.OPCode == UdpConsts.OPCODE_RELIABLEACK)
            {
                m_Parent.DebugDump("Received reliable ACK for packet " + cmd.Fields[0] + ".");

                //Get the current reliable command
                ReliableEntry rcmd = null;
                RQueue.GetCurrentReliableCommand(out rcmd);

                //If the sequence number from the one stored in the first field
                //of the ACK is the same as the one in the queue, remove it.
                if (rcmd != null)
                {
                    try
                    {
                        if (rcmd.SequenceNum == Convert.ToUInt32(cmd.Fields[0]))
                        {
                            RQueue.NextReliableCommand();

                            ReliableEntry next_rel = null;
                            RQueue.GetCurrentReliableCommand(out next_rel);

                            if (next_rel != null)
                            {
                                m_Parent.DebugDump("Moving reliable packet queue - next packet is " + next_rel.SequenceNum.ToString());

                                //Send the next reliable packet
                                m_Parent.SendData(RemoteEP.Address.ToString(), RemoteEP.Port, next_rel.CommandPacket);
                            }
                            else
                                m_Parent.DebugDump("Moving reliable packet queue - no more packets on reliable queue.");
                        }
                    }
                    catch (Exception e)
                    {
                        m_Parent.DebugDump("Exception: " + e.Message);
                    }
                }
            }

            //Give unrecognised commands to the application
            if(Authed)
                m_Parent.CommandReceived(this, cmd);
        }
		#endregion

		#region Send methods
		/// <summary>
		/// Sends a comand packet to the specified connection unreliably (might not get there)
		/// </summary>
		/// <param name="conn">Connection to send to</param>
		/// <param name="flags">Packet flags</param>
		/// <param name="opcode">Packet OPCode</param>
		/// <param name="fields">Packet fields</param>
		/// <returns>UDP_OK or error code</returns>
		public int SendUnreliableCommand(byte flags, string opcode, string[] fields)
		{
			//Make sure the reliable flag is off
			flags &= (byte)(~UdpConsts.FLAGS_RELIABLE);
			return m_Parent.SendCommand(m_RemoteEP.Address.ToString(), m_RemoteEP.Port, m_EncryptionKey, m_LastSentPacket, flags, opcode, fields);
		}

		/// <summary>
		/// Sends a reliable comand packet to the specified connection
		/// </summary>
		/// <param name="conn">Connection to send to</param>
		/// <param name="flags">Packet flags</param>
		/// <param name="opcode">Packet OPCode</param>
		/// <param name="fields">Packet fields</param>
		/// <returns>UDP_OK or error code</returns>
		public int SendReliableCommand(byte flags, string opcode, string[] fields)
		{
			//Force the reliable command flag ON
			flags |= UdpConsts.FLAGS_RELIABLE;

			//Force the sequenced flag OFF, reliable is automatically sequenced.
			flags &= (byte)(~UdpConsts.FLAGS_SEQUENCED);

			//Send the packet
			return m_Parent.SendCommand(m_RemoteEP.Address.ToString(), m_RemoteEP.Port, m_EncryptionKey, m_LastSentPacketR, flags, opcode, fields);
		}
		#endregion

		/// <summary>
		/// Adds a reliable packet to the reliable packet queue on the specified connection
		/// </summary>
		public int CacheReliablePacket(string packet)
		{
			ReliableEntry re = new ReliableEntry();
			re.SequenceNum = m_LastSentPacketR;
			re.CommandPacket = packet;
			return m_RQueue.AddReliableCommand(re);
		}

        /// <summary>
        /// Resets the timeout time on a connection, renewing the time left for the
        /// connection to live, called whenever a ping command is received.
        /// </summary>
        public void UpdateTimeout()
        {
            TimeoutTime = DateTime.Now.AddSeconds(UdpConsts.CONNECTION_TIMEOUT);
        }

        /// <summary>
        /// Resends the current reliable packet, used if a reliable packet acknowledgement
        /// does not come through fast enough to guarantee delivery of the packet.
        /// </summary>
        public void ResendReliablePacket()
        {
            ReliableEntry rcmd = null;
            RQueue.GetCurrentReliableCommand(out rcmd);

            //If the sequence number from the one stored in the first field
            //of the ACK is the same as the one in the queue, remove it.
            if (rcmd != null)
            {
                m_Parent.DebugDump("Found a reliable packet on the queue, resending.");
                m_Parent.SendData(RemoteEP.Address.ToString(), RemoteEP.Port, rcmd.CommandPacket);
            }
        }
    }

    /// <summary>
    /// Represents a list of connections that are handled by the UDP server
    /// </summary>
    internal class ConnectionList : CollectionBase
    {
        private CommonUdp m_Parent;

        public ConnectionList(CommonUdp udp)
        {
            m_Parent = udp;
        }

        /// <summary>
        /// Adds a new connection to the list
        /// </summary>
        /// <param name="conn">Connection to add</param>
        /// <returns>UDP_OK or error code</returns>
        public int NewConnection(Connection conn)
        {
            if (conn == null)
                return UdpConsts.UDP_FAIL;

            m_Parent.DebugDump("Adding connection entry for " + conn.RemoteEP.ToString() + ".");

            lock (List.SyncRoot)
            {
                List.Add(conn);
            }

            //Send the event
            m_Parent.ConnectionStateChanged(conn, true, "");

            return UdpConsts.UDP_OK;
        }

        /// <summary>
        /// Removes a connection from the list
        /// </summary>
        /// <param name="conn">Connection to remove</param>
        /// <param name="send_disconnect_packet">If true, will send a disconnection packet</param>
        /// <param name="reason">Reason to send (only matters if sending a disconnection packet</param>
        /// <returns>UDP_OK or error code</returns>
        public int RemoveConnection(Connection conn, bool send_disconnect_packet, string reason)
        {
            if (conn == null)
                return UdpConsts.UDP_FAIL;

            lock (List.SyncRoot)
            {
                if (!List.Contains(conn))
                    return UdpConsts.UDP_FAIL;

                if (reason == "") { reason = "Disconnected by remote host."; }
                m_Parent.DebugDump("Removed connection " + conn.RemoteEP.ToString() + ". Reason: " + reason);

                if (send_disconnect_packet)
                {
                    //Clamp reason length
                    if (reason.Length > 200)
                        reason = reason.Substring(0, 200);

                    //Send disconnection packet
                    conn.SendUnreliableCommand(0, UdpConsts.OPCODE_DISCONNECT, new string[] { reason });
                }

                List.Remove(conn);
            }

			//Send the event to the third party app
			m_Parent.ConnectionStateChanged(conn, false, reason);

            return UdpConsts.UDP_OK;
        }

        /// <summary>
        /// Removes all connections in the list
        /// </summary>
        /// <returns>UDP_OK or error code</returns>
        public int RemoveAllConnections(string Reason)
        {
            lock (List.SyncRoot)
            {
                if (List.Count == 0)
                    return UdpConsts.UDP_OK;

				while(List.Count > 0)
                    RemoveConnection((Connection)List[0], true, Reason);
            }
            return UdpConsts.UDP_OK;
        }

        /// <summary>
        /// Removes timed out connections
        /// </summary>
        /// <returns>UDP_OK or error code</returns>
        public int RemoveOldConnections()
        {
            ArrayList dead_connections = new ArrayList();

            lock (List.SyncRoot)
            {
                if (List.Count == 0)
                    return UdpConsts.UDP_OK;

                for (int i = 0; i < List.Count; i++)
                {
                    Connection conn = List[i] as Connection;
                    if (conn == null)
                        continue;

                    if (conn.TimeoutTime <= DateTime.Now)
                        dead_connections.Add(conn);
                }
            }

            if (dead_connections.Count > 0)
            {
                m_Parent.DebugDump("Found timed out connections, removing them...");

                for (int i = 0; i < dead_connections.Count; i++)
                    RemoveConnection((Connection)dead_connections[i], false, "Timed out.");

                m_Parent.DebugDump("Timed out connections removed.");
            }

            //Clear the dead connections list
            dead_connections.Clear();

            return UdpConsts.UDP_OK;
        }

        /// <summary>
        /// Sends a ping packet to all connections on the list.
        /// </summary>
        public void PingConnections()
        {
            lock (List.SyncRoot)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    Connection conn = List[i] as Connection;
                    if (conn == null)
                        continue;

                    if(conn.Authed)
                        conn.SendUnreliableCommand(0, UdpConsts.OPCODE_PING, null);
                }
            }
        }

        /// <summary>
        /// Sends a reliable retry on all connections that have a reliable packet waiting in the queue
        /// </summary>
        public void ReliableRetry()
        {
            lock (List.SyncRoot)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    Connection conn = List[i] as Connection;
                    if (conn == null)
                        continue;

                    conn.ResendReliablePacket();
                }
            }
        }

        /// <summary>
        /// Searches for a connection on the queue given an IP and port
        /// </summary>
        /// <param name="IP">IP to search for</param>
        /// <param name="Port">Port to search for</param>
        /// <param name="found">Set to found connection if any are found.</param>
        /// <returns>UDP_OK or error code</returns>
        public int ConnectionByIPPort(string IP, int Port, out Connection found)
        {
            found = null;
            lock (List.SyncRoot)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    Connection conn = List[i] as Connection;
                    if (conn == null)
                        continue;

                    if (conn.RemoteEP.Address.ToString() == IP)
                        if (conn.RemoteEP.Port == Port)
                        {
                            found = conn;
                            return UdpConsts.UDP_OK;
                        }
                }
            }
            return UdpConsts.UDP_NOTFOUND;
        }

		/// <summary>
		/// Searches for a connection on the queue given an IP and port
		/// </summary>
		/// <param name="request_id">Request ID to search for.</param>
		/// <param name="found">Set to found connection if any are found.</param>
		/// <returns>UDP_OK or error code</returns>
		public int ConnectionByRequestID(string request_id, out Connection found)
		{
			found = null;

			if(request_id == "")
				return UdpConsts.UDP_NOTFOUND;

			lock (List.SyncRoot)
			{
				for (int i = 0; i < List.Count; i++)
				{
					Connection conn = List[i] as Connection;
					if (conn == null)
						continue;

					if (conn.RequestID == request_id)
					{
						found = conn;
						return UdpConsts.UDP_OK;
					}
				}
			}
			return UdpConsts.UDP_NOTFOUND;
		}

        /// <summary>
        /// Returns all connection objects in the list
        /// </summary>
        /// <param name="found">Connection array of found connections</param>
        /// <returns>UDP_OK or error code</returns>
        public int GetConnections(out Connection[] found)
        {
            lock (List.SyncRoot)
            {
				found = new Connection[List.Count];

                for (int i = 0; i < List.Count; i++)
                {
                    Connection conn = List[i] as Connection;
                    if (conn == null)
                        continue;

                    found[i] = conn;
                }
            }

            return UdpConsts.UDP_OK;
        }

        /// <summary>
        /// Searches for a connection on the queue given a remote endpoint object
        /// </summary>
        /// <param name="remoteEP">Remote endpoint to search for</param>
        /// <param name="found">Set to the connection that is found</param>
        /// <returns>UDP_OK or error code</returns>
        public int ConnectionByRemoteEndpoint(IPEndPoint remoteEP, out Connection found)
        {
            return ConnectionByIPPort(remoteEP.Address.ToString(), remoteEP.Port, out found);
        }

        /// <summary>
        /// Returns the number of connections in the list
        /// </summary>
        /// <value>UDP_OK or error code</value>
        public int NumConnections
        {
            get
            {
                lock (List.SyncRoot)
                {
                    return List.Count;
                }
            }
        }
    }
}
