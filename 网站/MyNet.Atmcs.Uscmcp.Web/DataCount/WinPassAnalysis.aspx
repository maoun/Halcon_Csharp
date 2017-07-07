<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WinPassAnalysis.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.WinPassAnalysis" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>详细信息</title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
     <link href="../Styles/NewPageStyle.css" rel="stylesheet" type="text/css" />
</head>
<body >
    <form id="form1" runat="server" >
        <div>
            <ext:ResourceManager ID="ResourceManager1" runat="server" />
            <ext:Viewport runat="server" Layout="border">
                <Items>
                    <ext:TabPanel runat="server" ID="tabPanel1" Region="Center">
                        <Items>
                            <ext:Panel ID="TabHot" runat="server" Padding="6"  Hidden="true">
                                <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
                            </ext:Panel>
                            <ext:Panel ID="TabAnalysis" runat="server" Padding="6">
                                <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
                            </ext:Panel>
                        </Items>
                    </ext:TabPanel>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>