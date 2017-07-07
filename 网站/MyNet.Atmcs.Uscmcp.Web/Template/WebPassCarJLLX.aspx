<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebPassCarJLLX.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.WebPassCarJLLX" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>

    <style type="text/css">
        #main > div {
            width: 100% !important;
        }

        #main .zr-element {
            width: 100% !important;
        }

        #main .echarts-tooltip {
            width: auto !important;
        }
    </style>  
      <script type="text/javascript" language="javascript" src="../build/dist/echarts.js" charset="UTF-8"></script>
</head>
<body style="overflow-x: hidden; overflow-y: hidden">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="WebPassCarVehHead" />
    <form id="form1" runat="server">
        <div id="main" style="height: 400px"></div>
    </form>
</body>
</html>