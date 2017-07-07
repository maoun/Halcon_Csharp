<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebPeccancyData.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.WebPeccancyData" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <script type="text/javascript" language="javascript" src="../build/dist/echarts.js" charset="UTF-8"></script>
    <style type="text/css">
        #main {
            min-width: 1150px;
        }

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
</head>
<body style="overflow-x: hidden; overflow-y: hidden">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="WebPeccancyData" />
    <form id="form1" runat="server">
        <div id="main" style="height: 350px"></div>
    </form>
</body>
</html>