using System;

namespace MyNet.Atmcs.Uscmcp.UI
{
    public partial class VehicleHead : System.Web.UI.UserControl
    {        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 选中省区简称
        /// </summary>
        public string VehicleText
        {
            get { return this.Field2.Text; }
        }

        /// <summary>
        /// 设置省区简称
        /// </summary>
        /// <param name="vehicleText"></param>
        public void SetVehicleText(string vehicleText)
        {
            this.Field2.Text = vehicleText;
        }

        /// <summary>
        /// 设置可用状态
        /// </summary>
        /// <param name="flag"></param>
        public void SetDisable(bool flag)
        {
            this.Field2.Disabled = flag;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            this.Field2.Text = "";
        }
    }
}