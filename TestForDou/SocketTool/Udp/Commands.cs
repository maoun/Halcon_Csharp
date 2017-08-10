/*
 * Genesis Socket Server and Client
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 * 
 * Please see included license.txt file for information on redistribution and usage.
 */
#region Using directives

using System;
using System.Text;
using System.Collections;

#endregion

namespace SocketTool.Udp
{
    /// <summary>
    /// Represents a single command
    /// </summary>
    internal class Command : ICommand
    {
		private string m_OPCode;
		private uint m_SequenceNum;
		private byte m_Flags;
		private short m_NumFields;
		private short[] m_FieldSizes;
		private string[] m_Fields;
		private string m_AllFields;

		/// <summary>
		/// Two byte OPCode that controlls how the command is used by the application
		/// </summary>
		public string OPCode
		{
			get{ return m_OPCode; }
			set{ m_OPCode = value; }
		}

		/// <summary>
		/// Sequence number of the command packet.
		/// Used as an identifier for sequencing and reliability.
		/// </summary>
        public uint SequenceNum
		{
			get{ return m_SequenceNum; }
			set{ m_SequenceNum = value; }
		}

		/// <summary>
		/// Command packet flags that control how the packet is processed.
		/// Such as making it reliable or sequenced (See the FLAGS_xxx constants.)
		/// </summary>
        public byte Flags
		{
			get{ return m_Flags; }
			set{ m_Flags = value; }
		}

		/// <summary>
		/// Number of data fields in this command
		/// </summary>
        public short NumFields
		{
			get{ return m_NumFields; }
			set{ m_NumFields = value; }
		}

		/// <summary>
		/// The sizes of the fields in the command
		/// </summary>
        public short[] FieldSizes
		{
			get{ return m_FieldSizes; }
			set{ m_FieldSizes = value; }
		}

		/// <summary>
		/// The data fields of the command, broken up and placed in a string array for convinience.
		/// </summary>
        public string[] Fields
		{
			get{ return m_Fields; }
			set{ m_Fields = value; }
		}

		/// <summary>
		/// A string containing all of the data fields concatonated together.
		/// </summary>
		public string AllFields
		{
			get{ return m_AllFields; }
			set{ m_AllFields = value; }
		}

        /// <summary>
        /// This function will populate the Fields property of a command object
        /// based on the FieldSizes and AllFields properties.
        /// </summary>
        public int Initialize()
        {
            if (NumFields == 0)
                return UdpConsts.UDP_OK;

            try
            {
                int curpos = 0;
                Fields = new string[NumFields];
                for (int i = 0; i < NumFields; i++)
                {
                    Fields[i] = AllFields.Substring(curpos, FieldSizes[i]);
                    curpos += FieldSizes[i];
                }
                return UdpConsts.UDP_OK;
            }
            catch
            {
                Fields = null;
                return UdpConsts.UDP_FAIL;
            }
        }
    }

    /// <summary>
    /// A single entry in the reliable packet queue
    /// </summary>
    internal class ReliableEntry
    {
		/// <summary>
		/// Sequence number of the reliable packet
		/// </summary>
        public uint SequenceNum;

		/// <summary>
		/// Complete packet data including all header information, used if the system
		/// needs to resend a reliable packet.
		/// </summary>
        public string CommandPacket;
    }

    /// <summary>
    /// Implementation of a reliable packet queue
    /// </summary>
    internal class ReliableQueue : Queue
    {
		/// <summary>
		/// Initialises the reliable packet queue
		/// </summary>
        public ReliableQueue()
        {
            this.Clear();
        }

        /// <summary>
        /// Adds a command packet to the reliable queue.
        /// </summary>
        /// <param name="cmd">Command packet to add to the queue</param>
        /// <returns>UDP_OK or error code</returns>
        public int AddReliableCommand(ReliableEntry cmd)
        {
			if(this.Count >= UdpConsts.MAX_RELIABLE_QUEUED)
				return UdpConsts.UDP_RELIABLEQUEUEFULL;

            lock (this.SyncRoot)
            {
                if (this.Count > 0)
                {
                    ReliableEntry tmp = this.Peek() as ReliableEntry;
                    if (tmp != null)
                    {
                        if (tmp.SequenceNum == cmd.SequenceNum)
                            return UdpConsts.UDP_ALREADYINQUEUE;
                    }
                }

                this.Enqueue(cmd);
            }
            return UdpConsts.UDP_OK;
        }

        /// <summary>
        /// Ges the current reliable command from the queue, but does not remove it from the queue.
        /// </summary>
        /// <param name="cmd_out">Command at start of queue</param>
        /// <returns>UDP_OK or error code</returns>
        public int GetCurrentReliableCommand(out ReliableEntry cmd_out)
        {
            cmd_out = null;
            
            lock (this.SyncRoot)
            {
                //No reliable packets on the queue
                if (this.Count == 0)
                    return UdpConsts.UDP_NOTFOUND;

                cmd_out = this.Peek() as ReliableEntry;
            }

            return UdpConsts.UDP_OK;
        }

        /// <summary>
        /// Removes the command first in the queue, moving the queue along by one place.
        /// </summary>
        /// <returns>UDP_OK or error code</returns>
        public int NextReliableCommand()
        {
            lock (this.SyncRoot)
            {
                //No reliable packets on the queue
                if (this.Count == 0)
                    return UdpConsts.UDP_NOTFOUND;

                this.Dequeue();
            }

            return UdpConsts.UDP_OK;
        }

        /// <summary>
        /// Clears the reliable queue of all command packets.
        /// </summary>
        /// <returns>UDP_OK or error code</returns>
        public int ClearReliableQueue()
        {
            lock (this.SyncRoot)
            {
                this.Clear();
            }
            return UdpConsts.UDP_OK;
        }

        /// <summary>
        /// Returns true if a command is waiting to be processed
        /// </summary>
        public bool CommandsWaiting()
        {
            lock (this.SyncRoot)
            {
                if (this.Count > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}
