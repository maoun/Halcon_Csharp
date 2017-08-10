using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using SocketTool;
using System.Collections;

namespace SocketTool.Core
{
    public class CommUdpServer : IServer
    {
        private Thread m_UDPThread;         //UDP receive thread
        private string m_CurrentIP;         //Current local IP
        private int m_CurrentPort;          //Current local port
        private IPEndPoint m_LocalEndPoint; //Current local endpoint (contians the IP and port)
        private Socket m_Socket;            //UDP Socket
        private int m_iState;               //Current state of the UDP server

        public event ReceivedHandler OnDataReceived;
        public event SocketErrorHandler OnSocketError;

        private Hashtable ConnectionMap = new Hashtable();

        private Hashtable NewConnMap = new Hashtable();

        private List<IConnection> ConnectionList = new List<IConnection>();



        public void Init(String serverIp, int port)
        {
            this.m_CurrentIP = serverIp;
            this.m_CurrentPort = port;
        }


        public int Listen()
        {
            try
            {
                m_UDPThread = new Thread(new ThreadStart(ReceiveLoop));
                m_UDPThread.Name = "RecieveThread";
                m_UDPThread.Start();

            }
            catch (Exception e)
            {
            }
            return 1;
        }


        public List<IConnection> GetConnectionList()
        {

            return ConnectionList;
        }

        private void ReceiveLoop()
        {
            int iExCount = 0;

            if (string.IsNullOrEmpty(m_CurrentIP))
                m_LocalEndPoint = new IPEndPoint(IPAddress.Any, m_CurrentPort);
            else
                m_LocalEndPoint = new IPEndPoint(IPAddress.Parse(m_CurrentIP), m_CurrentPort);

            try
            {
                m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                m_Socket.Bind(m_LocalEndPoint);
                m_iState = Constants.UDP_STATE_LISTENING;

                //if (OnListenStateChanged != null)
                    //OnListenStateChanged(null, new ListenEventArgs(true));

                while (true)
                {
                    try
                    {
                        //Make space to store the data from the socket
                        Byte[] received = new Byte[Constants.MAX_COMMAND_LEN];

                        //Create an end point, just give it temporary values
                        EndPoint remoteEP = new IPEndPoint(m_LocalEndPoint.Address, m_LocalEndPoint.Port);

                        //Read bytes from the socket
                        int bytesReceived = m_Socket.ReceiveFrom(received, ref remoteEP);
                        IPEndPoint remoteIPEP = (IPEndPoint)remoteEP;
                        UdpConnection uc = NewConnection(remoteIPEP);
                        //string str_received = Encoding.UTF8.GetString(received);
                        //string str_received = null;//Util.BytesToString(received);
                        //str_received = str_received.Substring(0, bytesReceived);

                        //Fire the received event if it is being used (allowing raw data to be caught)
                        byte[] data = new byte[bytesReceived];
                        Array.Copy(received, data, bytesReceived);
                        if (OnDataReceived != null)
                           OnDataReceived(uc.ID, new ReceivedEventArgs((IPEndPoint)remoteEP, data));
                    }
                        
                    catch (SocketException ex)
                    {
                        if (OnSocketError != null)
                            OnSocketError(0, new SocketEventArgs((int)ex.ErrorCode, ex.Message));      
                    }                
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch
            {

            }
        }


        private UdpConnection NewConnection(IPEndPoint clientIP)
        {
            string strKey = clientIP.ToString();
            UdpConnection ic = (UdpConnection)ConnectionMap[strKey];
            if(ic != null)
            {
                ic.OnlineDate = DateTime.Now;
                return ic;
            }

            UdpConnection uc = new UdpConnection();
            uc.ClientIP = clientIP;
            ConnectionMap[strKey] = uc;
            uc.ID = strKey;
            ConnectionList.Add(uc);

            return uc;
        }

        public void Send(string ID, byte[] data, int length)
        {
            int sendBytes = 0;

            IConnection ic = (IConnection)ConnectionMap[ID];

            if(ic != null)
            {

                try
                {
                	this.m_Socket.SendTo(data, 0, length, SocketFlags.None, ic.ClientIP);
                }
                catch (SocketException ex)
                {
                    if (OnSocketError != null)
                        OnSocketError(0, new SocketEventArgs((int)ex.ErrorCode, ex.Message));                      	
                }
            }

            //return sendBytes;
        }

        public int SendData(string IP, int Port, string Data)
        {
            if (m_iState != Constants.UDP_STATE_LISTENING)
                return Constants.UDP_FAIL;

            try
            {
                //byte[] databytes = Encoding.UTF8.GetBytes(Data);
                byte[] databytes = Util.StringToBytes(Data);

                EndPoint remoteEP = new IPEndPoint(IPAddress.Parse(IP), Port);
                m_Socket.SendTo(databytes, remoteEP);
            }
            catch (SocketException se)
            {
                //DebugDump("Socket Exception (" + se.ErrorCode.ToString() + "): " + se.Message);
                return Constants.UDP_FAIL;
            }
            catch (Exception e)
            {
                //DebugDump("Exception: " + e.Message);
                return Constants.UDP_FAIL;
            }
            return Constants.UDP_OK;
        }
        public void Close()
        {
            //Fail and return if the socket is not listening.
            if (m_iState != Constants.UDP_STATE_LISTENING)
                return;// Constants.UDP_FAIL;

            //Update application of my status change
            //if (OnListenStateChanged != null)
                //OnListenStateChanged(null, new ListenEventArgs(false));

            //m_Clients.RemoveAllConnections("Server shutting down.");

            //m_Servers.RemoveAllConnections("Client shutting down.");

            //m_ConnTimeouts.RemoveAllEntries();

            m_iState = Constants.UDP_STATE_CLOSING;

            m_Socket.Shutdown(SocketShutdown.Both);
            m_Socket.Close();

            m_UDPThread = null;

            return;// Constants.UDP_OK;
        }

    }
}
