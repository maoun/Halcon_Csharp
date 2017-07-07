<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebPassCarBarData.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.WebPassCarBarData" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Styles/chartbar.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function WindowShow(type, datetime) {
            this.parent.WindowShow(type, datetime);
        }
    </script>
    <style type="text/css">
        #Panel5 IFrame {
            min-width: 1150px !important;
        }
    </style>
</head>
<body style="overflow-x: hidden; overflow-y: hidden">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="WebPassCarBarData" />
    <form id="form1" runat="server">
        <ext:Panel runat="server" ID="divCount" Cls="datasheetbox1">
        </ext:Panel>
    </form>
</body>
</html>