using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace SocketTool.Core
{
    /**
     * 网络测试工具，测试网站是否连通;
     * 
     * 
     */
    public class NetTest
    {
        public static bool DnsTest(string websiteUrl)
        {
            try
            {
                System.Net.IPHostEntry ipHe =
                    System.Net.Dns.GetHostByName(websiteUrl);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool PingTest(string Ip)
        {
            System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

            System.Net.NetworkInformation.PingReply pingStatus =
                ping.Send(IPAddress.Parse(Ip), 1000);

            if (pingStatus.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TcpSocketTest(string websiteUrl)
        {
            try
            {
                System.Net.Sockets.TcpClient client =
                    new System.Net.Sockets.TcpClient(websiteUrl, 80);
                client.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
        public static bool WebRequestTest(string websiteUrl)
        {
            try
            {
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create(websiteUrl);
                System.Net.WebResponse myResponse = myRequest.GetResponse();
            }
            catch (System.Net.WebException)
            {
                return false;
            }
            return true;
        }
    }
}
