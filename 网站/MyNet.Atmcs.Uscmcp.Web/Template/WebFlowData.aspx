<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFlowData.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.WebFlowData" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>实时过车流量统计</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <script src="../build/dist/echarts.js" type="text/javascript" charset="UTF-8"></script>
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
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="WebFlowData" />
    <form id="form1" runat="server">
        <div id="main" style="height: 350px; width: 100%"></div>
    </form>
</body>
</html>