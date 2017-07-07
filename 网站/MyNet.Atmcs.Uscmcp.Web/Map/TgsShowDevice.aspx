<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TgsShowDevice.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Map.TgsShowDevice" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript">
        var showTip = function () {
            var rowIndex = GridDevice.view.findRowIndex(this.triggerElement),
                cellIndex = GridDevice.view.findCellIndex(this.triggerElement),
                record = StoreDevice.getAt(rowIndex),
                fieldName = GridDevice.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>设备状态</title>
</head>
<body>
    <form id="form1" runat="server">
    
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="TgsShowDevice" />
        <ext:Viewport ID="ViewPort1" runat="server" AutoWidth="true" >
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
                                            <ext:Column Width="120" DataIndex="col4" ColumnID="Company" Header="设备IP" />
                                            <ext:Column Width="120" DataIndex="col7" ColumnID="Company" Header="状态" />                                           
                                            <ext:DateColumn Header="更新时间" DataIndex="col8" Width="120" Format="yyyy-MM-dd HH:mm" />
                                        </Columns>
                                    </ColumnModel>
                               <%--      <SelectionModel>
                                <ext:RowSelectionModel runat="server"/>
                            </SelectionModel>
                                    <View>

                                <ext:GridView runat="server" StripeRows="true" TrackOver="true" />
                            </View>
                            <ToolTips>
                                <ext:ToolTip
                                    ID="RowTip"
                                    runat="server"
                                    Target="={GridDevice.getView().mainBody}"
                                    Delegate=".x-grid3-cell"
                                    TrackMouse="true">
                                    <Listeners>
                                        <Show Fn="showTip" />
                                    </Listeners>
                                </ext:ToolTip>
                            </ToolTips>--%>
                                </ext:GridPanel>
                            </Items>
                        </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
