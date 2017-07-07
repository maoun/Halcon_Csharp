<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TgsShowStation.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Map.TgsShowStation" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>卡口过车浏览统计</title>
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
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="TgsShowStation" />
    <ext:Viewport ID="ViewPort1" MaxHeight="100" runat="server">
        <Items>
            <ext:Panel ID="PanFlow" runat="server" Height="80" Title="流量统计" Width="600" Padding="0">
                <TopBar>
                    <ext:Toolbar ID="ToolbarFlow" runat="server">
                        <Items>
                            <ext:Label ID="LblFlow" Text="流量" runat="server">
                            </ext:Label>
                            <ext:Button ID="Button1" runat="server" Text="设备状态">
                               <DirectEvents>
                                   <Click OnEvent="showdev"></Click>
                               </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    <form id="form1" runat="server">
        <div id="main" style="position: relative; overflow: hidden; width: 200px; height: 300px"></div>
    </form>
    <ext:Window runat="server" Hidden="true" ID="winstation" width="500"  height="300">
        <Items>
            <ext:Panel ID="PanDevice" runat="server" Title="设备状态"
                Padding="0">
                <Items>
                    <ext:GridPanel ID="GridDevice" Region="Center" runat="server" StripeRows="true" Title="设备状态"
                        Collapsible="true" AutoHeight="true" Header="false">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <ext:Label ID="LblDevice" runat="server">
                                    </ext:Label>
                                    <ext:ToolbarFill />
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="StoreDevice" runat="server">
                                <AutoLoadParams>
                                    <ext:Parameter Name="start" Value="={0}" />
                                    <ext:Parameter Name="limit" Value="={15}" />
                                </AutoLoadParams>
                                <UpdateProxy>
                                    <ext:HttpWriteProxy Method="GET" Url="TgsShow.aspx">
                                    </ext:HttpWriteProxy>
                                </UpdateProxy>
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="col0" Type="String" />
                                            <ext:RecordField Name="col1" Type="String" />
                                            <ext:RecordField Name="col2" Type="String" />
                                            <ext:RecordField Name="col3" Type="String" />
                                            <ext:RecordField Name="col4" Type="String" />
                                            <ext:RecordField Name="col5" Type="String" />
                                            <ext:RecordField Name="col6" Type="String" />
                                            <ext:RecordField Name="col7" Type="String" />
                                            <ext:RecordField Name="col8" Type="Date" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:Column Width="60" DataIndex="col2" Hidden="true" ColumnID="Company" Header="设备编号" />
                                <ext:Column Width="180" DataIndex="col3" ColumnID="Company" Header="设备名称" />
                                <ext:Column Width="100" DataIndex="col4" ColumnID="Company" Header="设备IP" />
                                <ext:Column Width="100" DataIndex="col7" ColumnID="Company" Header="状态" />
                                <ext:DateColumn Header="更新时间" DataIndex="col8" Width="120" Format="yyyy-MM-dd HH:mm" />
                            </Columns>
                        </ColumnModel>

                    </ext:GridPanel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Window>
</body>
</html>
