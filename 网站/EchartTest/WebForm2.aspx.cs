using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EchartTest
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        private ServiceReference1.querypasscarPortTypeClient client = new ServiceReference1.querypasscarPortTypeClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            bool result = client.addTgs("609381056000", "", "", "609381056000");

        }
    }
}