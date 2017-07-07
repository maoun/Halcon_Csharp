using System.Collections;
using MyNet.Common.MQ;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class MQManager
    {
        public event OnMQComeEventHandler OnMQComeEvent;

        private RabbitMQReceive mq;
        /// <summary>
        /// 
        /// </summary>
        public MQManager()
        {
            mq = new RabbitMQReceive(5672, "192.168.1.235", "admin", "admin");
        }
        /// <summary>
        /// 开始接受消息
        /// </summary>
        public void Start()
        {
            mq.ReceiveMessageEvent += mq_ReceiveMessageEvent;
            mq.StartReceive("EHL_ALARM_QUEUE");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="e"></param>
        private void mq_ReceiveMessageEvent(byte[] bytes, string e)
        {
            string ms = System.Text.Encoding.UTF8.GetString(bytes);
            Hashtable hs = GetDataJson(ms);
            if (OnMQComeEvent != null)
            {
                OnMQComeEvent(hs);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public System.Collections.Hashtable GetDataJson(string data)
        {
            try
            {
                Json json = new Json(data);
                System.Collections.Hashtable carhs = json["resultCar"] as System.Collections.Hashtable;
                carhs.Add("layoutId", json["layoutId"]);
                carhs.Add("alarmTime", json["alarmTime"]);
                return carhs;
            }
            catch
            {
                return null;
            }
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="carinfo"></param>
    public delegate void OnMQComeEventHandler(System.Collections.Hashtable carinfo);
}