<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebPeccancyGrid.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.WebPeccancyGrid" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
        <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
</head>
<body style="overflow-x: hidden; overflow-y: hidden">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="WebPeccancyGrid" />
    <ext:TableGrid runat="server" Table="data" StripeRows="true" AutoScroll="true" AutoHeight="true" />
    <div style="font-size:1em;margin-left:9px; font-weight:bold;color:black;">违法类型统计</div>
    <div id="show" runat="server" style="width: 100%; height: 100%">
    </div>
</body>
</html>