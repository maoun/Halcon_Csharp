<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VehicleRelationAnalysis.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Passcar.VehicleRelationAnalysis" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <script src="../build/dist/echarts.js" type="text/javascript"  charset="UTF-8"></script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="VehicleRelationAnalysis" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="ColumnLayout">
            <Items>
                <ext:Panel ID="panelMain" runat="server" Layout="Fit" ColumnWidth=".6" HideBorders="true">
                    <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
                </ext:Panel>
                <ext:Panel ID="panelWest" runat="server" Layout="Fit" ColumnWidth=".4" HideBorders="true">
                    <Content>
                        <center>
                        <div id="main" style=" height:600px; width: 90%"></div>
                        </center>
                    </Content>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
