using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
//using System.Web.UI.WebControls;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Web.Passcar
{
    public partial class WebImageTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void cutclick(object sender, EventArgs e)
        {
            int t = 12; int l = 12; int w = 12; int h = 12;
            string strPath = "../Map/temp/map.png";// (ViewState["path"] != null) ? ViewState["path"].ToString() : "resizeimages/main.jpg";

            Image image = Image.FromFile(Server.MapPath("../Map/temp/map.png"));

            Bitmap bitmap = new Bitmap(800, 1000);
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            Graphics canvas = Graphics.FromImage(bitmap);
            int randnum = rand.Next(0, int.MaxValue);
            Rectangle rec = new Rectangle(l, t, w, h);
            GraphicsUnit units = GraphicsUnit.Pixel;
            canvas.DrawImage(image, 0, 0, rec, units);

            string imgName = (ViewState["imgName"] != null) ? ViewState["imgName"].ToString() : "main.jpg";
            bitmap.Save(Server.MapPath(strPath).Replace(imgName, randnum.ToString() + ".jpg"));
            //bitmap.Save(Server.MapPath("resizeimages/main.jpg").Replace("main.jpg", randnum.ToString() + ".jpg"));
            canvas.Dispose();
            image.Dispose();
            bitmap.Dispose();
            this.b.Visible = true;
            //this.bSrc = "resizeimages/" + randnum + ".jpg";
            if (ViewState["path"] != null)
                showImages(ViewState["path"].ToString(), int.Parse(ViewState["imgwidth"].ToString()), int.Parse(ViewState["imgheight"].ToString()));
            else
                showImages("", 800, 1000);

        }

        public void showImages(string imagePath, int imgwidth, int imgheight)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type=\"text/javascript\">");
            sb.Append("window.onload = function ShowImg(){");
            //sb.Append("window.onload = function ShowImg(imagePath){");
            //sb.Append("var imagePath1 = (typeof(imagePath) != \"undefined\") ? imagePath : \"resizeimages/main.jpg\";");
            if (!string.IsNullOrEmpty(imagePath))
                sb.Append("var imagePath1=\"" + imagePath + "\";");
            else
                sb.Append("var imagePath1=" + "\"resizeimages/main.jpg\";");

            sb.Append("var ic = new ImgCropper(\"bgDiv\", \"dragDiv\", imagePath1, " + imgwidth + ", " + imgheight + ", {");
            sb.Append("Right: \"rRight\", Left: \"rLeft\", Up: \"rUp\", Down: \"rDown\",");
            sb.Append("RightDown: \"rRightDown\", LeftDown: \"rLeftDown\", RightUp: \"rRightUp\", LeftUp: \"rLeftUp\"");
            sb.Append("});");
            sb.Append("}");
            sb.Append("</script>");
            ClientScript.RegisterStartupScript(this.GetType(), "LoadPicScript", sb.ToString());

        }
    }
}