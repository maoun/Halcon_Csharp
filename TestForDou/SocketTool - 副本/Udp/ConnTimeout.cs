/*
 * Genesis Socket Server and Client
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 * 
 * Please see included license.txt file for information on redistribution and usage.
 */
using System;
using System.Collections;

namespace SocketTool.Udp
{
	internal class TimeoutEntry
	{
		public string ServerIP;
		public int ServerPort;
		public DateTime TimeoutTime;
		public string ConnectionRequestID;
	}

	/// <summary>
	/// Connection timeout list, holds all connections that we have tried to
	/// establish and allows the system to detect unresponsive hosts.
	/// </summary>
	internal class ConnTimeout : CollectionBase
	{
		private CommonUdp m_Parent;

		public ConnTimeout(CommonUdp parent)
		{
			m_Parent = parent;
		}

		/// <summary>
		/// Generates a new request ID number.
		/// </summary>
		public string GetNewRequestID( )
		{
			//Generate a random conection request number
			Random r = new Random(DateTime.Now.Millisecond);
			string newnum = r.Next( ).ToString();
			Connection cn;

			//Make sure we dont get any ID clashes with another connection request (unlikely but just in case).
			while(EntryExists(newnum) || (m_Parent.Servers.ConnectionByRequestID(newnum, out cn) != UdpConsts.UDP_NOTFOUND))
				newnum = r.Next( ).ToString();

			return newnum;
		}

		/// <summary>
		/// Adds a connection into the timeout list
		/// </summary>
		public void AddConnectionEntry(string IP, int Port, int TimeoutSecs, string RequestID)
		{
			lock(List.SyncRoot)
			{
				//Remove any existing connection attempt to this IP/Port.
				if(EntryExists(IP, Port))
					RemoveConnectionEntry(IP, Port);

				//Create the entry
				TimeoutEntry toe = new TimeoutEntry( );
				toe.ServerIP = IP;
				toe.ServerPort = Port;
				toe.TimeoutTime = DateTime.Now.AddSeconds(TimeoutSecs);
				toe.ConnectionRequestID = RequestID;

				//Add to the list
				List.Add(toe);
			}
		}

		/// <summary>
		/// Returns whether or not a connection reauest exists based on its unique request ID.
		/// </summary>
		public bool EntryExists(string RequestID)
		{
			lock(List.SyncRoot)
			{
				for(int i = 0; i < List.Count; i++)
				{
					TimeoutEntry toe = List[i] as TimeoutEntry;

					if(toe == null)
						continue;

					if(toe.ConnectionRequestID == RequestID)
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Finds out whether a time out entry exists or not.
		/// </summary>
		public bool EntryExists(string IP, int Port)
		{
			lock(List.SyncRoot)
			{
				for(int i = 0; i < List.Count; i++)
				{
					TimeoutEntry toe = List[i] as TimeoutEntry;

					if(toe == null)
						continue;

					if((toe.ServerIP == IP) && (toe.ServerPort == Port))
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Removes a connection from the timeout list
		/// </summary>
		public void RemoveConnectionEntry(string IP, int Port)
		{
			lock(List.SyncRoot)
			{
				for(int i = 0; i < List.Count; i++)
				{
					TimeoutEntry toe = List[i] as TimeoutEntry;

					if(toe == null)
						continue;

					if((toe.ServerIP == IP) && (toe.ServerPort == Port))
					{
						List.Remove(toe);
						return;
					}
				}
			}
		}

		/// <summary>
		/// Removes a connection from the timeout list
		/// </summary>
		public void RemoveConnectionEntry(string request_id)
		{
			lock(List.SyncRoot)
			{
				for(int i = 0; i < List.Count; i++)
				{
					TimeoutEntry toe = List[i] as TimeoutEntry;

					if(toe == null)
						continue;

					if(toe.ConnectionRequestID == request_id)
					{
						List.Remove(toe);
						return;
					}
				}
			}
		}

		/// <summary>
		/// Removes all connection timeout entries
		/// </summary>
		public void RemoveAllEntries( )
		{
			lock(List.SyncRoot)
			{
				List.Clear( );
			}
		}

		/// <summary>
		/// Checks the timeout entrys for any that have expired.
		/// </summary>
		public void CheckTimeouts( )
		{
			lock(List.SyncRoot)
			{
				if(List.Count == 0)
					return;

				for(int i = 0; i < List.Count; i++)
				{
					TimeoutEntry toe = List[i] as TimeoutEntry;

					if(toe == null)
						continue;

					if(toe.TimeoutTime < DateTime.Now)
					{
						m_Parent.ConnectionRequestTimedOut(toe.ServerIP, toe.ServerPort, toe.ConnectionRequestID);
						List.Remove(toe);
						return;
					}
				}
			}
		}
	}
}
