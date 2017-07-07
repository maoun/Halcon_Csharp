<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebPeccancyBarData.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.WebPeccancyBarData" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>违法类型统计</title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <link href="../Styles/chartbar.css" rel="stylesheet" type="text/css" />
 
</head>
<body style="overflow-x: hidden; overflow-y: hidden">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="WebPeccancyBarData" />
    <form id="form1" runat="server">
        <ext:Panel runat="server" ID="divCount" Cls="datasheetbox1">
        </ext:Panel>
    </form>
</body>
</html>
