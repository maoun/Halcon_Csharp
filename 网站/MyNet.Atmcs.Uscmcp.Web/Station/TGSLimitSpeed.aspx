<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TGSLimitSpeed.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.TGSLimitSpeed" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>卡口限速设置</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <style type="text/css">
        .x-grid-row-summary {
            color: #948d8e;
            text-decoration: line-through;
        }
    </style>
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" language="javascript">

        var employeeDetailsRender = function () {
            return '<img class="imgEdit" ext:qtip="点击查看详细信息" style="cursor:pointer;" src="../Images/vcard_edit.png"  />';
        };
    </script>
    <script type="text/javascript">
        function ChangeLimit(type) {

            switch (type) {
                case 1:
                    // GridLane.colModel.columns[0].hidden = true;
                    GridLane.colModel.columns[1].hidden = true;
                    GridLane.reload();
                    StoreLane.reload();
                    break;
                case 2:
                    //  GridLane.colModel.columns[0].hidden = false;
                    GridLane.colModel.columns[1].hidden = true;
                    GridLane.reload();
                    StoreLane.reload();
                    break;
                case 3:
                    // GridLane.colModel.columns[0].hidden = false;
                    GridLane.colModel.columns[1].hidden = false;
                    GridLane.reload();
                    StoreLane.reload();
                    break;
            }
        }
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridLane.view.findRowIndex(this.triggerElement),
                cellIndex = GridLane.view.findCellIndex(this.triggerElement),
                record = StoreLane.getAt(rowIndex),
                fieldName = GridLane.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);
            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:Hidden ID="HideStation" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="HideLimitType" runat="server">
        </ext:Hidden>
        <ext:Store ID="StoreDirDev" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="TGSLimitSpeed" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="Panel1" Region="Center" runat="server" Header="false" Height="40" Layout="FitLayout">
                    <Items>
                        <ext:GridPanel ID="GridLane" runat="server" StripeRows="true" TrackMouseOver="true">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar2" runat="server" Layout="Container">
                                    <Items>
                                        <ext:Toolbar ID="Toolbar5" runat="server">
                                            <Items>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="20" />
                                                <ext:Button ID="ButDevAdd" runat="server" Text='<%# GetLangStr("TGSLimitSpeed2","添加限速设置") %>' Icon="Add">
                                                    <DirectEvents>
                                                        <Click OnEvent="AddLane" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButDevDelete" runat="server" Text='<%# GetLangStr("TGSLimitSpeed3","删除限速设置") %>' Icon="Delete">
                                                    <Listeners>
                                                        <Click Handler="TGSLimitSpeed.DoConfirmLimit()" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                        <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StoreLane">
                                            <Items>
                                                <ext:Toolbar ID="ToolExport" runat="server">
                                                    <Items>
                                                        <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                                        <%--          <ext:Button ID="ButPrint" runat="server" Icon="Printer" Text='<%# GetLangStr("TGSLimitSpeed5","打印") %>'>
                                                            <DirectEvents>
                                                                <Click OnEvent="ButPrintClick" />
                                                            </DirectEvents>
                                                        </ext:Button>--%>
                                                    </Items>
                                                </ext:Toolbar>
                                            </Items>
                                        </ext:PagingToolbar>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Store>
                                <ext:Store ID="StoreLane" runat="server" OnRefreshData="MyData_Refresh">
                                    <Reader>
                                        <ext:JsonReader IDProperty="col0">
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
                                                <ext:RecordField Name="col12" />
                                                <ext:RecordField Name="col13" />
                                                <ext:RecordField Name="col14" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <Plugins>
                                <ext:RowEditor ID="RowEditor4" runat="server" SaveText='<%# GetLangStr("TGSLimitSpeed6","更新") %>' CancelText='<%# GetLangStr("TGSLimitSpeed7","退出") %>'>
                                    <DirectEvents>
                                        <BeforeEdit OnEvent="BeforeSpeed" ></BeforeEdit>
                                        <AfterEdit OnEvent="UpdateData" />
                                    </DirectEvents>
                                </ext:RowEditor>
                            </Plugins>
                            <ColumnModel ID="ColumnModel4" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40" />
                                    <ext:Column ColumnID="LimitDirection" DataIndex="col5" Header='<%# GetLangStr("TGSLimitSpeed8","所属方向") %>' AutoDataBind="true" Width="80" Hidden="true">
                                    </ext:Column>
                                    <ext:Column DataIndex="col2" Width="250" Header='<%# GetLangStr("TGSLimitSpeed10","监测点名称") %>' AutoDataBind="true">
                                    </ext:Column>
                                    <ext:Column ColumnID="LimitLaneId" DataIndex="col6" Width="60" Header='<%# GetLangStr("TGSLimitSpeed9","车道编号") %>' AutoDataBind="true" Align="Center">
                                    </ext:Column>
                                    <ext:Column DataIndex="col8" Width="100" Header='<%# GetLangStr("TGSLimitSpeed11","大车限速") %>' AutoDataBind="true" Align="Center">
                                        <Editor>
                                            <ext:NumberField ID="TxtBS" runat="server" AllowBlank="false" MinValue="30" MaxValue="150" />
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="100" DataIndex="col9" Header='<%# GetLangStr("TGSLimitSpeed12","小车限速") %>' AutoDataBind="true" Align="Center">
                                        <Editor>
                                            <ext:NumberField ID="TxtSS" runat="server" AllowBlank="false" MinValue="30" MaxValue="150" />
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column DataIndex="col12" Width="100" Header='<%# GetLangStr("TGSLimitSpeed13","大车实际限速") %>' AutoDataBind="true" Hidden="true" Align="Center">
                                        <Editor>
                                            <ext:NumberField ID="TxtABS" runat="server" AllowBlank="false" MinValue="30" MaxValue="140" />
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column DataIndex="col13" Width="100" Header='<%# GetLangStr("TGSLimitSpeed14","小车实际限速") %>' AutoDataBind="true" Hidden="true" Align="Center">
                                        <Editor>
                                            <ext:NumberField ID="TxtALS" runat="server" AllowBlank="false" MinValue="30" MaxValue="140" />
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column DataIndex="col10" Width="100" Header='<%# GetLangStr("TGSLimitSpeed15","大车限低速") %>' AutoDataBind="true" Align="Center">
                                        <Editor>
                                            <ext:NumberField ID="TxtBLS" runat="server" AllowBlank="false" MinValue="0" MaxValue="80" />
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column DataIndex="col11" Width="100" Header='<%# GetLangStr("TGSLimitSpeed16","小车限低速") %>' AutoDataBind="true" Align="Center">
                                        <Editor>
                                            <ext:NumberField ID="TxtSLS" runat="server" AllowBlank="false" MinValue="0" MaxValue="80" />
                                        </Editor>
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel4" runat="server" SingleSelect="true"/>
                                
                            </SelectionModel>
                            <View>
                                <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                                </ext:GridView>
                            </View>
                            <ToolTips>
                                <ext:ToolTip
                                    ID="RowTip"
                                    runat="server"
                                    Target="={GridLane.getView().mainBody}"
                                    Delegate=".x-grid3-cell"
                                    TrackMouse="true">
                                    <Listeners>
                                        <Show Fn="showTip" />
                                    </Listeners>
                                </ext:ToolTip>
                            </ToolTips>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="West" Split="true" Title='<%# GetLangStr("TGSLimitSpeed17","监测点列表") %>'
                    Width="400" Icon="Table">
                    <Items>
                        <ext:TreePanel ID="TreePanel1" runat="server" Icon="Drive" Shadow="None" UseArrows="true"
                            AutoScroll="true" Animate="true" ContainerScroll="true" EnableDD="true" RootVisible="true"
                            Header="false" Height="800">
                        </ext:TreePanel>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>