<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserGuide.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.UserGuide" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <title></title>
    <style>
        /*.pdfobject-container {
            width:80%;
        }*/

        .pdfobject {
            border: 1px solid #666;
        }
    </style>
    <script type="text/javascript" src="../Scripts/PDFObject/pdfobject.js" charset="UTF-8"></script>
</head>
<body  style="background-color:grey">
    <form id="form1" runat="server" style="height:100%">
        <center>
        <div id="example1"  style="height:1000px;  width:80%">
        </div></center>
    </form>
</body>
      <script type="text/javascript">PDFObject.embed("../UserGuide/shuoming.pdf", "#example1", { width: "80%" });</script>
</html>
