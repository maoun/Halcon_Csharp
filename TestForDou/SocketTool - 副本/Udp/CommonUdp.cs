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
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.IO;

#endregion

namespace SocketTool.Udp
{
    internal class CommonUdp : IGenesisUDP
    {
        private Thread m_UDPThread;         //UDP receive thread
        private string m_CurrentIP;         //Current local IP
        private int m_CurrentPort;          //Current local port
        private IPEndPoint m_LocalEndPoint; //Current local endpoint (contians the IP and port)
        private Socket m_Socket;            //UDP Socket
        private int m_iState;               //Current state of the UDP server
		private bool m_bEncrypt;			//Should we encrypt connections?
        private ConnectionList m_Clients;   //Remote hosts connected to us
        private ConnectionList m_Servers;   //Remote hosts we are connected to
        private DateTime m_NextClearOldConnections;
        private DateTime m_NextPingConnections;
        private DateTime m_NextReliableResend;
        private Timer m_TickTimer;
		private ConnTimeout m_ConnTimeouts;
		private DateTime m_NextTimeoutCheck;
		private string m_Name;

        /// <summary>
        /// Called when a debug message comes from the UDP server
        /// </summary>
        public event DebugHandler OnDebugMessage;

        /// <summary>
        /// Called every time data is received, contains raw, unparsed packet data
        /// </summary>
        public event ReceivedHandler OnDataReceived;

        /// <summary>
        /// Called whenever the server starts or stops listening
        /// </summary>
        public event ListenHandler OnListenStateChanged;

        /// <summary>
        /// Called whenever the internal UDP socket throws an exception
        /// </summary>
        public event SocketErrorHandler OnSocketError;

        /// <summary>
        /// Called whenever a connectionless command is received.
        /// </summary>
        public event IncomingCommandHandler OnConnectionlessCommand;

        /// <summary>
        /// Called when a remote host has sent its login data.
        /// </summary>
        public event ConnectionAuthHandler OnConnectionAuth;

        /// <summary>
        /// Called when a host we are connecting to asks for our login data.
        /// </summary>
        public event SendLoginHandler OnLoginRequested;

        /// <summary>
        /// Called whenever a command is received.
        /// </summary>
        public event IncomingCommandHandler OnCommandReceived;

        /// <summary>
        /// Called whenever a connection to a remote host is established or lost
        /// </summary>
        public event ConnectionStateChangeHandler OnConnectionStateChanged;

        /// <summary>
        /// Called whenever login credentials sent to a remote host have been accepted or denied.
        /// </summary>
        public event AuthenticatedHandler OnAuthFeedback;

		/// <summary>
		/// Called whenever we try to conect to a remote host but the host
		/// doesnt respond fast enough.
		/// </summary>
		public event RequestTimedOutHandler OnConnectionRequestTimedOut;

        /// <summary>
        /// Constructor, initialises the UDP server
        /// </summary>
        public CommonUdp(string Name)
        {
            m_iState = UdpConsts.UDP_STATE_IDLE;
            m_Clients = new ConnectionList(this);
            m_Servers = new ConnectionList(this);
            m_TickTimer = new Timer(new TimerCallback(TimerTick), null, 500, 500);
            m_NextClearOldConnections = DateTime.Now;
            m_NextPingConnections = DateTime.Now;
            m_NextReliableResend = DateTime.Now;
			m_ConnTimeouts = new ConnTimeout(this);
			m_NextTimeoutCheck = DateTime.Now;
			m_Name = Name;
        }

        #region Public accessors
        /// <summary>
        /// Gets the state of the UDP server
        /// </summary>
        public int State
        {
            get { return m_iState; }
        }

		/// <summary>
		/// Gets or sets whether or not connections should be encrypted.
		/// </summary>
		public bool Encrypt
		{
			get{ return m_bEncrypt; }
			set{ m_bEncrypt = value; }
		}

		public ConnectionList Clients
		{
			get{ return m_Clients; }
		}

		public ConnectionList Servers
		{
			get{ return m_Servers; }
		}
        #endregion

        #region Server side start/stop listening methods
        /// <summary>
        /// Starts the UDP system listening for packets
        /// </summary>
        /// <returns>UDP_OK or error code</returns>
        public int StartListen(string IP, int Port)
        {
            DebugDump("Starting UDP server...");

            //Set the address to bind to
            m_CurrentIP = IP;
            DebugDump("Set server IP to " + IP);
            m_CurrentPort = Port;
            DebugDump("Set server port to " + Port.ToString( ));

            //Attempt to start the receive loop.
            try
            {
                m_UDPThread = new Thread(new ThreadStart(ReceiveLoop));
				m_UDPThread.Name = "RecieveThread";
                m_UDPThread.Start();
                DebugDump("Started the listen thread.");
            }
            catch (Exception e)
            {
                DebugDump("Exception: " + e.Message);
                return UdpConsts.UDP_FAIL;
            }
            return UdpConsts.UDP_OK;
        }

        /// <summary>
        /// Stops the UDP server listening.
        /// </summary>
        /// <returns>UDP_OK or error code</returns>
        public int StopListen()
        {
            //Fail and return if the socket is not listening.
            if (m_iState != UdpConsts.UDP_STATE_LISTENING)
                return UdpConsts.UDP_FAIL;

			//Update application of my status change
			if (OnListenStateChanged != null)
				OnListenStateChanged(null, new ListenEventArgs(false));

            DebugDump("Clearing client connection records.");
            m_Clients.RemoveAllConnections("Server shutting down.");

            DebugDump("Clearing server connection records.");
            m_Servers.RemoveAllConnections("Client shutting down.");

			DebugDump("Clearing timeout list.");
			m_ConnTimeouts.RemoveAllEntries( );

            DebugDump("Stopping the UDP server...");
            m_iState = UdpConsts.UDP_STATE_CLOSING;

            DebugDump("Closing the socket...");
			m_Socket.Shutdown(SocketShutdown.Both);
            m_Socket.Close( );
            DebugDump("Closed OK.");

            m_UDPThread = null;

            return UdpConsts.UDP_OK;
        }
        #endregion

        #region Receive Loop
        /// <summary>
        /// Receive loop continually receives data.
        /// </summary>
        private void ReceiveLoop()
        {
            int iExCount = 0;

			if(m_CurrentIP == "")
				m_LocalEndPoint = new IPEndPoint(IPAddress.Any, m_CurrentPort);
			else
				m_LocalEndPoint = new IPEndPoint(IPAddress.Parse(m_CurrentIP), m_CurrentPort);

            DebugDump("Created the local IP endpoint at " + m_LocalEndPoint.ToString( ));
            try
            {
                m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                DebugDump("Created the socket.");

                m_Socket.Bind(m_LocalEndPoint);
                DebugDump("Bound the socket to the local end point, now entering receive loop...");

                m_iState = UdpConsts.UDP_STATE_LISTENING;

                if (OnListenStateChanged != null)
                    OnListenStateChanged(null, new ListenEventArgs(true));

                while (true)
                {
                    try
                    {
                        //Make space to store the data from the socket
                        Byte[] received = new Byte[UdpConsts.MAX_COMMAND_LEN];
                        
                        //Create an end point, just give it temporary values
                        EndPoint remoteEP = new IPEndPoint(m_LocalEndPoint.Address, m_LocalEndPoint.Port);

                        //Read bytes from the socket
                        int bytesReceived = m_Socket.ReceiveFrom(received, ref remoteEP);
                        IPEndPoint remoteIPEP = (IPEndPoint)remoteEP;

                        //string str_received = Encoding.UTF8.GetString(received);
                        string str_received = Util.BytesToString(received);
                        str_received = str_received.Substring(0, bytesReceived);

                        //Fire the received event if it is being used (allowing raw data to be caught)
                        if(OnDataReceived != null)
                            OnDataReceived(null, new ReceivedEventArgs((IPEndPoint)remoteEP, str_received));

                        //Handle connectionless packets
                        try
                        {
                            int pcmd = 0;

                            //Allocate a temporary command object
                            Command new_cmd = new Command();

                            //Parse the start of the command
                            new_cmd.OPCode = str_received.Substring(pcmd, 2);
                            pcmd += 2;
                            new_cmd.SequenceNum = Util.BytesToUint(str_received.Substring(pcmd, 4));
                            pcmd += 4;
                            new_cmd.Flags = (byte)str_received[pcmd];
                            pcmd++;

                            //If the command packet is connectionless then parse it now.
                            if ((new_cmd.Flags & UdpConsts.FLAGS_CONNECTIONLESS) > 0)
                            {
                                new_cmd.NumFields = Util.BytesToShort(str_received.Substring(pcmd, 2));
                                pcmd += 2;
                                new_cmd.FieldSizes = new short[new_cmd.NumFields];
                                for (short i = 0; i < new_cmd.NumFields; i++)
                                {
                                    new_cmd.FieldSizes[i] = Util.BytesToShort(str_received.Substring(pcmd, 2));
                                    pcmd += 2;
                                }
                                pcmd += 2;
                                new_cmd.AllFields = str_received.Substring(pcmd);
                                new_cmd.Initialize();

                                //Process the connectionless command
                                ProcessConnectionlessComand(remoteIPEP, new_cmd);
                            }
                            else
                            {
                                //Locate connection this command belongs to and process it there.
                                Connection conn;

                                //Check both the client and server lists
                                m_Clients.ConnectionByRemoteEndpoint(remoteIPEP, out conn);
                                if(conn == null)
                                    m_Servers.ConnectionByRemoteEndpoint(remoteIPEP, out conn);

                                if (conn == null)
                                    DebugDump("Connection-related packet with no matching connection arrived - ignored.");
                                else
                                    conn.ProcessCommandPacket(str_received);
                            }
                        }
                        catch
                        {
                            DebugDump("Exception whilst parsing input from " + remoteIPEP.ToString() + " as command, probably not a command. Ignoring.");
                        }

                        //Reset the exception count to 0
                        iExCount = 0;
                    }
                    catch (SocketException se)
                    {
                        if ((se.ErrorCode != 10061) && (se.ErrorCode != 10054))
                        {
                            //Fire the socket error event.
                            if (se.ErrorCode != 10004)
                            {
                                if (OnSocketError != null)
                                    OnSocketError(null, new SocketEventArgs(se.ErrorCode, se.Message));
                            }

                            DebugDump("Socket Exception in receive loop (" + se.ErrorCode.ToString() + "): " + se.Message);
                            if (m_iState != UdpConsts.UDP_STATE_LISTENING)
                                break;

                            iExCount++;
                        }
                    }
                    catch (Exception e)
                    {
                        DebugDump("Exception in receive loop: " + e.Message);
                        if (m_iState != UdpConsts.UDP_STATE_LISTENING)
                            break;

                        iExCount++;
                    }

                    if (iExCount == UdpConsts.MAX_EXCEPTIONS)
                    {
                        DebugDump("Got too many consecutive exceptions in the receive loop, terminating.");
                        break;
                    }
                }
            }
            catch (SocketException se)
            {
                //Fire the socket error event.
                if (OnSocketError != null)
                    OnSocketError(null, new SocketEventArgs(se.ErrorCode, se.Message));

                DebugDump("Socket Exception (" + se.ErrorCode.ToString() + "): " + se.Message);
            }
            catch (Exception e)
            {
                DebugDump("Exception: " + e.Message);
            }

            //We are out of the loop but the server thinks we are not? make sure it does.
            if((m_iState != UdpConsts.UDP_STATE_IDLE) && (m_iState != UdpConsts.UDP_STATE_CLOSING))
            {
				StopListen();
            }

            DebugDump("Set the system state to idle.");

            //Set the state to idle.
            m_iState = UdpConsts.UDP_STATE_IDLE;

            DebugDump("Exited the receive loop.");
        }
        #endregion

		#region Generic command sending methods
        /// <summary>
        /// Sends a string of data to a remote host
        /// </summary>
        /// <param name="IP">IP address of remote host</param>
        /// <param name="Port">Port of remote host</param>
        /// <param name="Data">Data to send</param>
        /// <returns>UDP_OK or error code</returns>
        public int SendData(string IP, int Port, string Data)
        {
            if (m_iState != UdpConsts.UDP_STATE_LISTENING)
                return UdpConsts.UDP_FAIL;

            try
            {
                //byte[] databytes = Encoding.UTF8.GetBytes(Data);
                byte[] databytes = Util.StringToBytes(Data);

                EndPoint remoteEP = new IPEndPoint(IPAddress.Parse(IP), Port);
                m_Socket.SendTo(databytes, remoteEP);
            }
            catch (SocketException se)
            {
                DebugDump("Socket Exception (" + se.ErrorCode.ToString() + "): " + se.Message);
                return UdpConsts.UDP_FAIL;
            }
            catch (Exception e)
            {
                DebugDump("Exception: " + e.Message);
                return UdpConsts.UDP_FAIL;
            }
            return UdpConsts.UDP_OK;
        }

        /// <summary>
        /// Sends a connectionless command packet to a remote host
        /// </summary>
        /// <param name="IP">IP address of remote host</param>
        /// <param name="Port">Port of remote host</param>
        /// <param name="opcode">Packet OPCode</param>
        /// <param name="fields">Packet fields</param>
        /// <returns>UDP_OK or error code</returns>
        public int SendConnectionlessCommand(string IP, int Port, string opcode, string[] fields)
        {
            return SendCommand(IP, Port, "", 0, UdpConsts.FLAGS_CONNECTIONLESS, opcode, fields);
        }

		/// <summary>
		/// Sends an unreliable command to all open connections (optionally filtered).
		/// </summary>
		public int SendUnreliableCommandToAll(BroadcastFilter filter, byte flags, string opcode, string[] fields)
		{
			int i;
			Connection[] cons;

			//Don't send anything.
			if(filter == BroadcastFilter.None)
				return UdpConsts.UDP_OK;

			//We are sending to servers?
			if((filter & BroadcastFilter.Servers) > BroadcastFilter.None)
			{
				this.GetConnections(true, out cons);

				if(cons != null)
				{
					for(i = 0; i < cons.Length; i++)
					{
						//Does the connection need to be authed?
						if((filter & BroadcastFilter.AuthedOnly) > BroadcastFilter.None)
							if(!cons[i].Authed)
								continue;

						cons[i].SendUnreliableCommand(flags, opcode, fields);
					}
				}
			}

			//We are sending to clients?
			if((filter & BroadcastFilter.Clients) > BroadcastFilter.None)
			{
				this.GetConnections(false, out cons);

				if(cons != null)
				{
					for(i = 0; i < cons.Length; i++)
					{
						if((filter & BroadcastFilter.AuthedOnly) > BroadcastFilter.None)
							if(!cons[i].Authed)
								continue;

						cons[i].SendUnreliableCommand(flags, opcode, fields);
					}
				}
			}

			return UdpConsts.UDP_OK;
		}

		/// <summary>
		/// Sends a reliable command to all open connections (optionally filtered).
		/// </summary>
		public int SendReliableCommandToAll(BroadcastFilter filter, byte flags, string opcode, string[] fields)
		{
			int i;
			Connection[] cons;

			//Don't send anything.
			if(filter == BroadcastFilter.None)
				return UdpConsts.UDP_OK;

			//We are sending to servers?
			if((filter & BroadcastFilter.Servers) > BroadcastFilter.None)
			{
				this.GetConnections(true, out cons);

				if(cons != null)
				{
					for(i = 0; i < cons.Length; i++)
					{
						//Does the connection need to be authed?
						if((filter & BroadcastFilter.AuthedOnly) > BroadcastFilter.None)
							if(!cons[i].Authed)
								continue;

						cons[i].SendReliableCommand(flags, opcode, fields);
					}
				}
			}

			//We are sending to clients?
			if((filter & BroadcastFilter.Clients) > BroadcastFilter.None)
			{
				this.GetConnections(false, out cons);

				if(cons != null)
				{
					for(i = 0; i < cons.Length; i++)
					{
						if((filter & BroadcastFilter.AuthedOnly) > BroadcastFilter.None)
							if(!cons[i].Authed)
								continue;

						cons[i].SendReliableCommand(flags, opcode, fields);
					}
				}
			}

			return UdpConsts.UDP_OK;
		}

        /// <summary>
        /// Sends a formatted command packet (or packets) to the specified remote host
        /// </summary>
        /// <param name="IP">IP address of remote host</param>
        /// <param name="Port">Port of remote host</param>
        /// <param name="key">Encryption key</param>
        /// <param name="seq_num">Starting sequence number of packets</param>
        /// <param name="flags">Packet flags</param>
        /// <param name="opcode">Packet OPCode</param>
        /// <param name="fields">Packet fields</param>
        /// <returns>UDP_OK or error code</returns>
        public int SendCommand(string IP, int Port, string key, uint seq_num, byte flags, string opcode, string[] fields)
        {
            try
            {
                ArrayList commands = new ArrayList();
                string curpacket = "";
                bool send_now = true;
				int retval;

                //Attempt to get a connection object that corresponds to where
                //we are ending this command
                Connection destination = null;
                ConnectionByIPPort(IP, Port, out destination);

                //Can't send reliable packets to remote hosts without a connection entry.
                if(destination == null)
                {
                    if ((flags & UdpConsts.FLAGS_RELIABLE) > 0)
                        return UdpConsts.UDP_RELIABLENODESTINATION;
                }

                //Make sure fields isn't null, set it to a zero length string array
				if (fields == null)
					fields = new string[0];
				else
				{
					//Clone the fields array so we only work with a copy.
					fields = (string[])fields.Clone( );
				}

                //Encrypt if we should (if there is a key)
                if (key != "")
                {
                    flags |= UdpConsts.FLAGS_ENCRYPTED;
                    for (int i = 0; i < fields.Length; i++)
                        fields[i] = Util.XORCrypt(fields[i], key);
                }

                //Limit field sizes
                for (int i = 0; i < fields.Length; i++)
                    if (fields[i].Length > UdpConsts.MAX_FIELD_LEN)
                        fields[i] = fields[i].Substring(0, UdpConsts.MAX_FIELD_LEN);

                //Limit field number
                if(fields.Length > UdpConsts.MAX_FIELDS)
                {
                    string[] new_fields = new string[UdpConsts.MAX_FIELDS];
                    for (int i = 0; i < UdpConsts.MAX_FIELDS; i++)
                        new_fields[i] = fields[i];

                    fields = null;
                    fields = new_fields;
                }

                //Create the ideal packet
                curpacket = Util.CreatePacketHeader(seq_num, flags, opcode, key, fields);
                for (int i = 0; i < fields.Length; i++)
                    curpacket += fields[i];

                //Is the ideal packet too big to fit?
                if (curpacket.Length > UdpConsts.MAX_COMMAND_LEN)
                {
                    //Unreliable command packet is too long - cannot split unreliable commands
                    if ((flags & UdpConsts.FLAGS_RELIABLE) == 0)
                        return UdpConsts.UDP_UNRELIABLETOOLONG;

                    //Set the compound flag on
                    flags |= UdpConsts.FLAGS_COMPOUNDPIECE;

                    //Recreate the ideal packet with the new flag setting
                    curpacket = Util.CreatePacketHeader(seq_num, flags, opcode, key, fields);
                    for (int i = 0; i < fields.Length; i++)
                        curpacket += fields[i];

                    //Add the first command to the commands array
                    commands.Add(curpacket.Substring(0, UdpConsts.MAX_COMMAND_LEN));

                    //Make some variables for the command loop
                    uint seq_offset = 1;
                    string remainder_header = Util.CreatePacketHeader((seq_num + seq_offset), flags, UdpConsts.OPCODE_COMPOUNDPIECE, key, null);
                    string remaining_field_data = curpacket.Substring(UdpConsts.MAX_COMMAND_LEN, (curpacket.Length - UdpConsts.MAX_COMMAND_LEN));

                    //While the remainder data is to large, kepe breaking it up
                    while ((remaining_field_data.Length + remainder_header.Length) > UdpConsts.MAX_COMMAND_LEN)
                    {
                        //Add the additional command packet
                        commands.Add(remainder_header + remaining_field_data.Substring(0, (UdpConsts.MAX_COMMAND_LEN - remainder_header.Length)));

                        //Increment the sequence offset
                        seq_offset++;

                        //Get the remaining data we have not yet sent
                        remainder_header = Util.CreatePacketHeader((seq_num + seq_offset), flags, UdpConsts.OPCODE_COMPOUNDPIECE, key,  null);
                        remaining_field_data = remaining_field_data.Substring((UdpConsts.MAX_COMMAND_LEN - remainder_header.Length), (remaining_field_data.Length - (UdpConsts.MAX_COMMAND_LEN - remainder_header.Length)));
                    }

                    //Switch the flags - set to compound end (last piece)
                    flags &= (byte)(~UdpConsts.FLAGS_COMPOUNDPIECE);
                    flags |= UdpConsts.FLAGS_COMPOUNDEND;
                    commands.Add(Util.CreatePacketHeader((seq_num + seq_offset), flags, UdpConsts.OPCODE_COMPOUNDPIECE, key, null) + remaining_field_data);

                    //Now send all the command packets
                    for (int i = 0; i < commands.Count; i++)
                    {
                        //If there is no destination connection we can't track sequence numbers
                        //this should only be the case for connectionless commands.
                        if (destination != null)
                        {
                            if ((flags & UdpConsts.FLAGS_RELIABLE) > 0)
                            {
                                send_now = (!destination.RQueue.CommandsWaiting());

                                //If it is reliable then add the packet to the reliable queue if we can
                                retval = destination.CacheReliablePacket((string)commands[i]);
								if(retval != UdpConsts.UDP_OK)
									return retval;							

                                destination.LastSentPacketR++;
                            }
                            else
                                destination.LastSentPacket++;
                        }

                        //Send the data down the wire
                        if(send_now)
                            SendData(IP, Port, (string)commands[i]);
                    }
                }
                else
                {
                    //Only a single command, just send the ideal packet
                    if (destination != null)
                    {
                        if ((flags & UdpConsts.FLAGS_RELIABLE) > 0)
                        {
                            send_now = (!destination.RQueue.CommandsWaiting());

							//Attempt to add the reliable packet to the queue.
                            retval = destination.CacheReliablePacket(curpacket);
							if(retval != UdpConsts.UDP_OK)
								return retval;

                            destination.LastSentPacketR++;
                        }
                        else
                            destination.LastSentPacket++;
                    }

                    if(send_now)
                       SendData(IP, Port, curpacket);
                }

                //Clear the commands array
                commands.Clear();

                return UdpConsts.UDP_OK;
            }
            catch (Exception e)
            {
                DebugDump("Error occured during command send (" + e.Message + ")");
                return UdpConsts.UDP_FAIL;
            }
        }
		#endregion

		#region Client side connection methods (request/cancel)
		/// <summary>
		/// 
		/// </summary>
		/// <param name="RemoteIP"></param>
		/// <param name="RemotePort"></param>
		/// <param name="request_id"></param>
		/// <returns></returns>
        public int RequestConnect(ref string RemoteIP, int RemotePort, out string request_id)
        {
			request_id = "";

            DebugDump("Requesting a connection to " + RemoteIP + ":" + RemotePort.ToString());

			try
			{
				IPAddress.Parse(RemoteIP);
			}
			catch
			{
				//The string was not an IP - possibly a hostname? Try to resolve it.
				string oldname = RemoteIP;
				IPAddress[] ips;
				
				try
				{
					//Attempt to resolve the hostname
					ips = System.Net.Dns.GetHostEntry(RemoteIP).AddressList;
				}
				catch
				{
					DebugDump("Unable to resolve hostname '" + oldname + ".");
					return UdpConsts.UDP_UNABLETORESOLVE;
				}

				RemoteIP = ips[0].ToString();
				DebugDump("Resolved hostname '" + oldname + "' to IP address '" + RemoteIP + "'.");
			}

            Connection temp;
            m_Servers.ConnectionByIPPort(RemoteIP, RemotePort, out temp);
            if (temp != null)
            {
                DebugDump("Unable to try to connect, connection exists in server list.");
                return UdpConsts.UDP_ALREADYCONNECTED;
            }

            m_Clients.ConnectionByIPPort(RemoteIP, RemotePort, out temp);
            if (temp != null)
            {
                DebugDump("Unable to try to connect, connection exists in client list.");
                return UdpConsts.UDP_ALREADYCONNECTED;
            }

            //Generate request ID
			string req_id = m_ConnTimeouts.GetNewRequestID( );

			//Send the request ID back to the caller.
			request_id = req_id;

			//Send the connection request packet
			SendConnectionlessCommand(RemoteIP, RemotePort, UdpConsts.OPCODE_CONNECTIONREQUEST, new string[] {req_id});
            DebugDump("Sent connection request (" + req_id + ") to remote host.");

			//Add it to the timeout list.
			m_ConnTimeouts.AddConnectionEntry(RemoteIP, RemotePort, UdpConsts.CONNECTION_TIMEOUT_TIME, req_id);

            return UdpConsts.UDP_OK;
        }

		/// <summary>
		/// Cancels a connect request (just removes the timeout entry)
		/// </summary>
		/// <param name="RemoteIP">IP of the connection request to cancel</param>
		/// <param name="RemotePort">Port of the connection request to cancel</param>
		/// <returns>UDP_OK or error code</returns>
		public int CancelConnect(string RemoteIP, int RemotePort)
		{
			m_ConnTimeouts.RemoveConnectionEntry(RemoteIP, RemotePort);
			return UdpConsts.UDP_OK;
		}

		/// <summary>
		/// Cancels a connect request (just removes the timeout entry)
		/// </summary>
		/// <param name="RequestID">The request ID of the connection attempt to be aborted.</param>
		/// <returns>UDP_OK or error code</returns>
		public int CancelConnect(string RequestID)
		{
			m_ConnTimeouts.RemoveConnectionEntry(RequestID);
			return UdpConsts.UDP_OK;
		}
        #endregion

        #region Connectionless packet handling
        /// <summary>
        /// Processes a connectionless command.
        /// </summary>
        /// <param name="c">Command to process</param>
        /// <returns>UDP_OK or error code.</returns>
        private int ProcessConnectionlessComand(IPEndPoint remote_ep, Command c)
        {
            int retval = UdpConsts.UDP_OK;

            try
            {
                //========================================================================
                //Connection request from a remote host
                if (c.OPCode == UdpConsts.OPCODE_CONNECTIONREQUEST)
                {
                    Connection temp;
                    bool cant_connect = false;

                    m_Servers.ConnectionByIPPort(remote_ep.Address.ToString(), remote_ep.Port, out temp);
                    if (temp != null)
                    {
                        DebugDump("Unable to accept connection, connection exists in server list.");
                        retval = UdpConsts.UDP_ALREADYCONNECTED;
                        cant_connect = true;
                    }

                    m_Clients.ConnectionByIPPort(remote_ep.Address.ToString(), remote_ep.Port, out temp);
                    if (temp != null)
                    {
                        DebugDump("Unable to accept connection, connection exists in client list.");
                        retval = UdpConsts.UDP_ALREADYCONNECTED;
                        cant_connect = true;
                    }

                    //Can't accept this connection as we already have it in the list
                    //Client will just have to wait until it times out.
                    if (cant_connect)
                    {
                        SendConnectionlessCommand(remote_ep.Address.ToString(), remote_ep.Port, UdpConsts.OPCODE_CONNECTIONACK, new string[] { c.Fields[0], "FAIL", "Connection from this client already exists." });
                        return retval;
                    }

                    string encryption_key = "";
					
					//Is encryption on? If so generate an encryption key.
					if(m_bEncrypt)
						encryption_key = Util.GenerateEncryptionKey();

					//Create a new connection object and add it to the clients list...
                    Connection new_conn = new Connection(this);
                    new_conn.EncryptionKey = encryption_key;
                    new_conn.RemoteEP = remote_ep;

                    DebugDump("Remote host at " + remote_ep.ToString() + " requested a connection.");
                    DebugDump("Sending acknowledgement packet.");
                    SendConnectionlessCommand(remote_ep.Address.ToString(), remote_ep.Port, UdpConsts.OPCODE_CONNECTIONACK, new string[] { c.Fields[0], "OK", encryption_key });

                    DebugDump("Adding remote host to clients list.");
                    m_Clients.NewConnection(new_conn);

                    return UdpConsts.UDP_OK;
                }

                //========================================================================
                //Connection acknowledge from a remote host
                if (c.OPCode == UdpConsts.OPCODE_CONNECTIONACK)
                {
                    DebugDump("Got connection acknowledgement from " + remote_ep.ToString() + ".");

					if(!m_ConnTimeouts.EntryExists(c.Fields[0]))
					{
						//Make sure we actually _asked_ for this connection ack.
						DebugDump("Connection accept packet sent by " + remote_ep.ToString() + " but was unrequested - ignored.");
					}
                    else if (c.Fields[1] == "OK")
                    {
						//Server accepted our connection.

                        DebugDump("Connection " + c.Fields[0] + " accepted by " + remote_ep.ToString());

						//Create and store a connection object for the server that accepted us.
                        Connection new_con = new Connection(this);
                        new_con.EncryptionKey = c.Fields[2];
                        new_con.RemoteEP = remote_ep;
                        new_con.Server = true; //Is a connection to a server
						new_con.RequestID = c.Fields[0];
                        m_Servers.NewConnection(new_con);

						//Remove from the timeouts list
						m_ConnTimeouts.RemoveConnectionEntry(c.Fields[0]);

                        //Sending login data is to be handled in the actual application
                        //Basically it needs to send an command packet to the host containing the login data.
                        DebugDump("Server is requesting login information, sending.");
                        if (OnLoginRequested != null)
                            OnLoginRequested(null, new LoginSendEventArgs(new_con, true, ""));
                        else
                            //Application did nothing, so send a blank login packet now.
                            SendCommand(remote_ep.Address.ToString(), remote_ep.Port, new_con.EncryptionKey, 0, 0, UdpConsts.OPCODE_LOGINDETAILS, null);
                    }
                    else
                    {
						//Server did not accept our connection.

                        //Make a temporary connection object
                        Connection new_con = new Connection(this);
                        new_con.EncryptionKey = c.Fields[1];
                        new_con.RemoteEP = remote_ep;
                        new_con.Server = true; //Is a connection to a server

                        DebugDump("Connection not accepted - " + c.Fields[2] + ".");
                        if (OnLoginRequested != null)
                            OnLoginRequested(null, new LoginSendEventArgs(new_con, false, c.Fields[1]));
                    }

                    return UdpConsts.UDP_OK;
                }
            }
            catch (Exception e)
            {
                DebugDump("Exception whilst processing connectionless command (" + e.Message + ")");
                return UdpConsts.UDP_FAIL;
            }

            //========================================================================
            //Call the event (so it can be handled in the application)
            if (OnConnectionlessCommand != null)
                OnConnectionlessCommand(null, new CommandEventArgs(c, null, remote_ep));

            return UdpConsts.UDP_OK;
        }
        #endregion

        #region Event Firing
        /// <summary>
        /// Called by a connection object when a command comes in
        /// </summary>
        /// <param name="cn">Connection that sent the command</param>
        /// <param name="cmd">Command received</param>
        /// <returns>UDP_OK or error code</returns>
        internal int CommandReceived(Connection cn, Command cmd)
        {
            if (OnCommandReceived != null)
                OnCommandReceived(null, new CommandEventArgs(cmd, cn, null));

            return UdpConsts.UDP_OK;
        }

        /// <summary>
        /// Called when a connection is attempting to authenticate
        /// </summary>
        /// <param name="cn">Connection</param>
        /// <param name="cmd">Command</param>
        /// <returns>UDP_OK or error code</returns>
        internal int ConnectionAuthing(Connection cn, Command cmd)
        {
			//Is the connection already authed?
			if(cn.Authed)
			{
				DebugDump("Connection " + cn.RemoteEP.ToString() + " sent an auth packet but is already authed? Command ignored.");
				return UdpConsts.UDP_OK;
			}

            ConnectionAuthEventArgs ea = new ConnectionAuthEventArgs(cn, cmd);

            DebugDump("Connection " + cn.RemoteEP.ToString() + " sent login data...");

            //Have the third party client app process the login data
            if (OnConnectionAuth != null)
                OnConnectionAuth(null, ea);

            //Clamp the disallow reason to 200 characters
            if (ea.DisallowReason.Length > 200)
                ea.DisallowReason = ea.DisallowReason.Substring(0, 200);

            if (!ea.AllowConnection)
            {
                DebugDump("Login data is bad, rejecting connection.");
                cn.SendUnreliableCommand(0, UdpConsts.OPCODE_LOGINACK, new string[] { "FAIL", ea.DisallowReason });
                m_Clients.RemoveConnection(cn, true, ea.DisallowReason);
            }
            else
            {
                DebugDump("Login data is ok, connection authed.");
                cn.SendUnreliableCommand(0, UdpConsts.OPCODE_LOGINACK, new string[] { "OK" });
                cn.Authed = true;
            }
            return UdpConsts.UDP_OK;
        }

        /// <summary>
        /// Called internally when a connection is established or lost
        /// </summary>
        internal int ConnectionStateChanged(Connection cn, bool active, string reason)
        {
            if (OnConnectionStateChanged != null)
                OnConnectionStateChanged(null, new ConnectionStateChangeEventArgs(cn, active, reason));

            return UdpConsts.UDP_OK;
        }

        /// <summary>
        /// Called internally when login has been accepted by a remote host
        /// </summary>
        /// <param name="cn">Connection of the host that authed us</param>
        /// <returns>UDP_OK or error code</returns>
        internal int AuthenticatedWithConnection(Connection cn, bool accepted, string reason)
        {
            if (OnAuthFeedback != null)
                OnAuthFeedback(null, new AuthenticatedEventArgs(cn, accepted, reason));

            return UdpConsts.UDP_OK;
        }

		/// <summary>
		/// Called internally when a connection request times out.
		/// </summary>
		internal int ConnectionRequestTimedOut(string IP, int Port, string ReqID)
		{
			if(OnConnectionRequestTimedOut != null)
                OnConnectionRequestTimedOut(null, new RequestTimedOutEventArgs(IP, Port, ReqID));

			return UdpConsts.UDP_OK;
		}
        #endregion

        #region Timed methods
        /// <summary>
        /// Called when timer has ticked
        /// </summary>
        private void TimerTick(object o)
        {
            if (m_iState == UdpConsts.UDP_STATE_IDLE)
                return;

            //Clean old client connections
            if (m_NextClearOldConnections < DateTime.Now)
            {
                m_Clients.RemoveOldConnections();
                m_Servers.RemoveOldConnections();
                m_NextClearOldConnections = DateTime.Now.AddSeconds(UdpConsts.CONNECTION_CLEAN_TIME);
            }

            //Ping all connections to servers
            if (m_NextPingConnections < DateTime.Now)
            {
                m_Servers.PingConnections();
                m_NextPingConnections = DateTime.Now.AddSeconds(UdpConsts.CONNECTION_PING_DELAY);
            }

			//Retry sending reliable packets
            if (m_NextReliableResend < DateTime.Now)
            {
                m_Servers.ReliableRetry();
                m_Clients.ReliableRetry();
                m_NextReliableResend = DateTime.Now.AddSeconds(UdpConsts.CONNECTION_RELIABLE_RETRY);
            }

			//Check the connection timeouts
			if (m_NextTimeoutCheck < DateTime.Now)
			{
				m_ConnTimeouts.CheckTimeouts( );
				m_NextTimeoutCheck = DateTime.Now.AddSeconds(UdpConsts.CONNECTION_TIMEOUT_CHECK);
			}
        }
        #endregion

        #region Connection Methods
        /// <summary>
        /// Gets a connection by IP and port
        /// </summary>
        /// <param name="IP">IP to search for</param>
        /// <param name="Port">Port to search for</param>
        /// <param name="conn">Connection found (if any)</param>
        /// <returns>UDP_OK or UDP_NOTFOUND</returns>
        private int ConnectionByIPPort(string IP, int Port, out Connection conn)
        {
            int retval = UdpConsts.UDP_OK;
            conn = null;

            if(m_Clients.ConnectionByIPPort(IP, Port, out conn) != UdpConsts.UDP_OK)
                if(m_Servers.ConnectionByIPPort(IP, Port, out conn) != UdpConsts.UDP_OK)
                    retval = UdpConsts.UDP_NOTFOUND;

            return retval;
        }

		/// <summary>
		/// Gets a connection interface by IP and port
		/// </summary>
		/// <param name="IP">IP to search for</param>
		/// <param name="Port">Port to search for</param>
		/// <param name="conn">Connection found (if any)</param>
		/// <returns>UDP_OK or UDP_NOTFOUND</returns>
		public int ConnectionByIPPort(string IP, int Port, out IConnection conn)
		{
			return ConnectionByIPPort(IP, Port, out conn);
		}

        /// <summary>
        /// Removes a connection
        /// </summary>
        /// <param name="conn">Connection to remove</param>
        /// <param name="send_disconnect_packet">If true, will send a disconnection packet</param>
        /// <param name="reason">Reason to send (only matters if sending a disconnection packet</param>
        /// <returns>UDP_OK or error code</returns>
        private int RemoveConnection(Connection conn, bool send_disconnect_packet, string reason)
        {
            int retval = UdpConsts.UDP_OK;

            retval = m_Servers.RemoveConnection(conn, send_disconnect_packet, reason);
            if (retval != UdpConsts.UDP_OK)
                retval = m_Clients.RemoveConnection(conn, send_disconnect_packet, reason);

            return retval;
        }

		/// <summary>
		/// Removes a connection given tis interface.
		/// </summary>
		/// <param name="conn">Connection to remove</param>
		/// <param name="send_disconnect_packet">If true, will send a disconnection packet</param>
		/// <param name="reason">Reason to send (only matters if sending a disconnection packet</param>
		/// <returns>UDP_OK or error code</returns>
		public int RemoveConnection(IConnection conn, bool send_disconnect_packet, string reason)
		{
			return RemoveConnection((Connection)conn, send_disconnect_packet, reason);
		}

        /// <summary>
        /// Gets all the connections in the specified connection list
        /// </summary>
        /// <param name="servers">If true returns the servers list, else returns the clients list</param>
        /// <param name="found">The output array of found connections</param>
        /// <returns>UDP_OK or error code</returns>
        private int GetConnections(bool servers, out Connection[] found)
        {
            if (!servers)
                return m_Clients.GetConnections(out found);
            else
                return m_Servers.GetConnections(out found);
        }

		/// <summary>
		/// Gets all the connections in the specified connection list
		/// </summary>
		/// <param name="servers">If true returns the servers list, else returns the clients list</param>
		/// <param name="found">The output array of found connections</param>
		/// <returns>UDP_OK or error code</returns>
		public int GetConnections(bool servers, out IConnection[] found)
		{
			Connection[] cons;
			int retval = GetConnections(servers, out cons);

			found = (IConnection[])cons;
			return retval;
		}
        #endregion

		/// <summary>
		/// Returns a string array containing all local IP addresses
		/// </summary>
		public string[] GetLocalAddresses( )
		{
			return Util.GetLocalAddresses( );
		}

        /// <summary>
        /// Dumps a debug message
        /// </summary>
        /// <param name="Message">Debug message to dump</param>
        [Conditional("DEBUG")]
        public void DebugDump(string Message)
        {
            Message = m_Name + ": [" + Util.GetMethod(2) + "]: " + Message + "\r\n";

			Debug.Write(Message);

            //Fire the debug message event
            if (OnDebugMessage != null)
                OnDebugMessage(null, new DebugEventArgs(Message));
        }
    }
}