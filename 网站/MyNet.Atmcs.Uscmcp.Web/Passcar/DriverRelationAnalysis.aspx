<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DriverRelationAnalysis.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Passcar.DriverRelationAnalysis" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>驾驶员管理分析</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />   
         <script src="../build/dist/echarts.js" type="text/javascript"></script>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="CurrentDataCount" />
    <form id="form1" runat="server">

        <ext:Viewport ID="Viewport1" runat="server" Layout="FitLayout">
            <Items>
                <ext:Panel ID="Panel9" runat="server" Layout="Fit" Region="Center" HideBorders="true">
                    <Content>
                        <center>
                        <div id="main" style="height: 600px; width: 90%" runat="server"></div>
                            </center>
                    </Content>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>