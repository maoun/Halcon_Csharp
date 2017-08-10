using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace SocketTool.Core
{
    public class CommUdpClient : IClient
    {
        public String ServerIP { get; set; }

        public int Port { get; set; }

        public event ReceivedHandler OnDataReceived;
        public event SocketErrorHandler OnSocketError;

        UdpClient udpClient = new UdpClient();
        private int LocalPort = 4545;
        public Boolean IsConnected { get; set; }
        private Thread receiver;

        public void Init(String serverIp, int port)
        {
            this.ServerIP = serverIp;
            this.Port = port;
            try
            {

                udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, LocalPort));
                IsConnected = true;
                receiver = new Thread(new ThreadStart(ReceiveWork));
                receiver.Start();
            }
            catch (SocketException se)
            {
                if (OnSocketError != null)
                    OnSocketError(0, new SocketEventArgs((int)se.ErrorCode, se.Message));
            }

        }

        public void Send(byte[] data)
        {
            try
            {
                int sendBytes = 0;
                sendBytes = udpClient.Send(data, data.Length, new IPEndPoint(IPAddress.Parse(ServerIP), Port));
                
            }
            catch (SocketException se)
            {
                Close();
                if (OnSocketError != null)
                    OnSocketError(0, new SocketEventArgs((int)se.ErrorCode, se.Message));

            }
        }

        void Send(UdpClient sender, string s)
        {
            byte[] dgram = Encoding.ASCII.GetBytes(s);
            Console.WriteLine("Sending '" + s + "' (" + dgram.Length.ToString()
                               + " bytes)");
            sender.Send(dgram, dgram.Length);
        }

        void ReceiveWork()
        {
            try
            {
                while (IsConnected)
                {
                    Receive();
                    Thread.Sleep(1000);
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }

        void Receive()
        {
            try
            {
                IPEndPoint from = new IPEndPoint(IPAddress.Any, 0);
                byte[] dgram = udpClient.Receive(ref from);

                if (OnDataReceived != null)
                    OnDataReceived(null, new ReceivedEventArgs(from, dgram));
            }
            catch (SocketException se)
            {
                IsConnected = false;
                if (OnSocketError != null)
                    OnSocketError(0, new SocketEventArgs((int)se.ErrorCode, se.Message));   

            }
            catch (System.Exception ex)
            {
                IsConnected = false;

            }

            /**
            string s = Encoding.ASCII.GetString(dgram, 0, dgram.Length);
            Console.WriteLine
            (
              "Received {0} bytes, s = '{1}', s.Length = {2}",
              dgram.Length, s, s.Length
            );
             */
        }

        public void Close()
        {
            udpClient.Close();
            try
            {
            	receiver.Abort();
            }
            catch (System.Exception ex)
            {
            	
            }
        }

    }
}
