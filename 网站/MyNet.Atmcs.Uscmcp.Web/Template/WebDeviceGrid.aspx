<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebDeviceGrid.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.WebDeviceGrid" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <script src="../Scripts/common.js"></script>
    <script type="text/javascript">

        var DataState = function (value) {

            var src = "../images/state/unknow.gif"
            switch (value) {
                case "2":
                    src = "../images/state/normal.gif"
                    break;
                case "1":
                    src = "../images/state/alarm.gif"
                    break;
                case "0":
                    src = "../images/state/shutdown.gif"
                    break;
                default:
                    src = "../images/state/unknow.gif"
                    break;
            }
            return "<img class='imgEdit' ext:qtip='设备状态' style='cursor:pointer;' src='" + src + "'  />";

        };
        //保存选中记录到Session中去
        function BaoCunSession(col0, col1, col2, col3, col4, col5, col6, col7) {
            WebDeviceGrid.BaoCunSession(col0, col1, col2, col3, col4, col5, col6, col7);
        }
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridFlow.view.findRowIndex(this.triggerElement),
                cellIndex = GridFlow.view.findCellIndex(this.triggerElement),
                record = StoreFlow.getAt(rowIndex),
                fieldName = GridFlow.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body style="overflow-x: hidden; overflow-y: hidden">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="WebDeviceGrid" />
    <form id="form1" runat="server">
        <ext:Viewport ID="Viewport1" runat="server" Layout="FitLayout">
            <Items>
                <ext:GridPanel ID="GridFlow" runat="server" StripeRows="true" Title='<%# GetLangStr("WebDeviceGrid1","设备状态数据分析")%>' AutoDataBind="true" Layout="FitLayout"
                    AutoScroll="true" MinColumnWidth="110" TrackMouseOver="true">
                    <Store>
                        <ext:Store ID="StoreFlow" runat="server">
                            <AutoLoadParams>
                                <ext:Parameter Name="start" Value="={0}" />
                                <ext:Parameter Name="limit" Value="={10}" />
                            </AutoLoadParams>
                            <UpdateProxy>
                                <ext:HttpWriteProxy Method="GET" Url="WebDeviceGrid.aspx">
                                </ext:HttpWriteProxy>
                            </UpdateProxy>
                            <Reader>
                                <ext:JsonReader>
                                    <Fields>
                                        <ext:RecordField Name="col0" />
                                        <ext:RecordField Name="col1" />
                                        <ext:RecordField Name="col2" />
                                        <ext:RecordField Name="col3" />
                                        <ext:RecordField Name="col4" />
                                        <ext:RecordField Name="col5" />
                                        <ext:RecordField Name="col6" />
                                        <ext:RecordField Name="col7" />
                                        <ext:RecordField Name="col8" />
                                        <ext:RecordField Name="col9" />
                                        <ext:RecordField Name="col10" />
                                        <ext:RecordField Name="col11" />
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                        </ext:Store>
                    </Store>
                    <TopBar>
                        <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="10" StoreID="StoreFlow">
                        </ext:PagingToolbar>
                    </TopBar>

                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                            <ext:Column Header="id" AutoDataBind="true" Hidden="true" Sortable="true" DataIndex="col7">
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("WebDeviceGrid3","设备状态")%>' AutoDataBind="true" Width="100" Align="Center" Fixed="true" DataIndex="col0" Resizable="false">
                                <Renderer Fn="DataState" />
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("WebDeviceGrid5","卡口名称")%>' AutoDataBind="true" Width="150" Sortable="true" DataIndex="col2">
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("WebDeviceGrid4","设备名称")%>' AutoDataBind="true" Width="150" Sortable="true" DataIndex="col1">
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("WebDeviceGrid6","网络状态")%>' AutoDataBind="true" Width="120" Sortable="true" DataIndex="col3">
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("WebDeviceGrid7","工作状态")%>' AutoDataBind="true" Width="120" Sortable="true" DataIndex="col4">
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("WebDeviceGrid8","设备类型")%>' AutoDataBind="true" Width="140" Sortable="true" DataIndex="col5">
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("WebDeviceGrid9","设备型号")%>' AutoDataBind="true" Width="120" Sortable="true" DataIndex="col6">
                            </ext:Column>
                            <ext:Column Header="设备类型id" AutoDataBind="true" Width="120" Sortable="true" DataIndex="col8" Hidden="true">
                            </ext:Column>
                            <ext:Column Header='设备型号id' AutoDataBind="true" Width="120" Sortable="true" DataIndex="col9" Hidden="true">
                            </ext:Column>

                            <ext:Column Header="网络状态id" AutoDataBind="true" Width="120" Sortable="true" DataIndex="col10" Hidden="true">
                            </ext:Column>
                            <ext:Column Header='工作状态id' AutoDataBind="true" Width="120" Sortable="true" DataIndex="col11" Hidden="true">
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" SingleSelect="true">
                            <Listeners>
                                <RowSelect Handler="BaoCunSession(record.data.col0,record.data.col1,record.data.col2,record.data.col10,record.data.col11,record.data.col8,record.data.col9,record.data.col7);" Buffer="250" />
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <ToolTips>
                        <ext:ToolTip
                            ID="RowTip"
                            runat="server"
                            Target="={GridFlow.getView().mainBody}"
                            Delegate=".x-grid3-cell"
                            TrackMouse="true">
                            <Listeners>
                                <Show Fn="showTip" />
                            </Listeners>
                        </ext:ToolTip>
                    </ToolTips>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                        </ext:GridView>
                    </View>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>