using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Model
{
    [Serializable]
    /// <summary>
    /// 用户实体
    /// </summary>
    public class UserInfo
    {

        private bool loginType = false;
        /// <summary>
        /// 登陆类型 true  .net登陆，false java登陆
        /// </summary>
        public bool LoginType
        {
            get { return loginType; }
            set { loginType = value; }
        }

        private string userCode;

        /// <summary>
        /// 用户code
        /// </summary>
        public string UserCode
        {
            get { return userCode; }
            set { userCode = value; }
        }

        private string userName;

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string name;

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private DateTime time;

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }

        private string sex;

        /// <summary>
        /// 用户性别
        /// </summary>
        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        private string sexMs;

        /// <summary>
        /// 用户性别描述
        /// </summary>
        public string SexMs
        {
            get { return sexMs; }
            set { sexMs = value; }
        }

        private string role;

        /// <summary>
        /// 用户角色
        /// </summary>
        public string Role
        {
            get { return role; }
            set { role = value; }
        }

        private string userPolice;

        /// <summary>
        /// 用户警号
        /// </summary>
        public string UserPolice
        {
            get { return userPolice; }
            set { userPolice = value; }
        }

        private string departName;

        /// <summary>
        /// 所属机构
        /// </summary>
        public string DepartName
        {
            get { return departName; }
            set { departName = value; }
        }

        private string userClass;

        /// <summary>
        ///
        /// </summary>
        public string UserClass
        {
            get { return userClass; }
            set { userClass = value; }
        }

        private string deptCode;

        /// <summary>
        /// 部门编号
        /// </summary>
        public string DeptCode
        {
            get { return deptCode; }
            set { deptCode = value; }
        }

        private string systemCode;

        /// <summary>
        /// 用户对应系统编号
        /// </summary>
        public string SystemCode
        {
            get { return systemCode; }
            set { systemCode = value; }
        }

        private string videoTemplate1;

        /// <summary>
        /// 视频类模版地址1
        /// </summary>
        public string VideoTemplate1
        {
            get { return videoTemplate1; }
            set { videoTemplate1 = value; }
        }

        private string videoTemplate2;

        /// <summary>
        /// 视频类模版地址2
        /// </summary>
        public string VideoTemplate2
        {
            get { return videoTemplate2; }
            set { videoTemplate2 = value; }
        }

        private string dataTemplate1;

        /// <summary>
        /// / 数据类模版地址1
        /// </summary>
        public string DataTemplate1
        {
            get { return dataTemplate1; }
            set { dataTemplate1 = value; }
        }

        private string dataTemplate2;

        /// <summary>
        /// / 数据类模版地址2
        /// </summary>
        public string DataTemplate2
        {
            get { return dataTemplate2; }
            set { dataTemplate2 = value; }
        }

        private string dataTemplate3;

        /// <summary>
        /// / 数据类模版地址3
        /// </summary>
        public string DataTemplate3
        {
            get { return dataTemplate3; }
            set { dataTemplate3 = value; }
        }

        private string dataTemplate4;

        /// <summary>
        /// 数据类模版地址4
        /// </summary>
        public string DataTemplate4
        {
            get { return dataTemplate4; }
            set { dataTemplate4 = value; }
        }

        private string listTemplate1;

        /// <summary>
        /// 列表模版地址1
        /// </summary>
        public string ListTemplate1
        {
            get { return listTemplate1; }
            set { listTemplate1 = value; }
        }

        private string listTemplate2;

        /// <summary>
        /// 列表模版地址2
        /// </summary>
        public string ListTemplate2
        {
            get { return listTemplate2; }
            set { listTemplate2 = value; }
        }

        private string listTemplate3;

        /// <summary>
        /// 列表模版地址3
        /// </summary>
        public string ListTemplate3
        {
            get { return listTemplate3; }
            set { listTemplate3 = value; }
        }

        private string userTemplate;

        /// <summary>
        /// 用户自定义模版地址
        /// </summary>
        public string UserTemplate
        {
            get { return userTemplate; }
            set { userTemplate = value; }
        }

        private string userBackGround;

        /// <summary>
        /// 用户自定义背景图片
        /// </summary>
        public string UserBackGround
        {
            get { return userBackGround; }
            set { userBackGround = value; }
        }

        private string firstTemplate;

        /// <summary>
        /// 自定义首页
        /// </summary>
        public string FirstTemplate
        {
            get { return firstTemplate; }
            set { firstTemplate = value; }
        }

        private string screenNum = "1";

        /// <summary>
        /// 屏幕个数
        /// </summary>
        public string ScreenNum
        {
            get { return screenNum; }
            set { screenNum = value; }
        }

        private string screen1 = "";

        /// <summary>
        /// 屏幕1
        /// </summary>
        public string Screen1
        {
            get { return screen1; }
            set { screen1 = value; }
        }

        private string screen2 = "";

        /// <summary>
        /// 屏幕2
        /// </summary>
        public string Screen2
        {
            get { return screen2; }
            set { screen2 = value; }
        }

        private string screen3 = "";

        /// <summary>
        /// 屏幕3
        /// </summary>
        public string Screen3
        {
            get { return screen3; }
            set { screen3 = value; }
        }

        private string screen4 = "";

        /// <summary>
        /// 屏幕4
        /// </summary>
        public string Screen4
        {
            get { return screen4; }
            set { screen4 = value; }
        }

        private string screen5 = "";

        /// <summary>
        /// 屏幕5
        /// </summary>
        public string Screen5
        {
            get { return screen5; }
            set { screen5 = value; }
        }

        private string screen6 = "";

        /// <summary>
        /// 屏幕6
        /// </summary>
        public string Screen6
        {
            get { return screen6; }
            set { screen6 = value; }
        }

        private string screen7 = "";

        /// <summary>
        /// 屏幕7
        /// </summary>
        public string Screen7
        {
            get { return screen7; }
            set { screen7 = value; }
        }

        private string screen8 = "";

        /// <summary>
        /// 屏幕8
        /// </summary>
        public string Screen8
        {
            get { return screen8; }
            set { screen8 = value; }
        }

        private string screen9 = "";

        /// <summary>
        /// 屏幕9
        /// </summary>
        public string Screen9
        {
            get { return screen9; }
            set { screen9 = value; }
        }
        private string nowIp = "";
        /// <summary>
        /// 当前登陆IP
        /// </summary>
        public string NowIp
        {
            get { return nowIp; }
            set { nowIp = value; }
        }
    }
}
