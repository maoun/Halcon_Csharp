<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviceStatus.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.DeviceStatus" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>设备状态列表</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <link href="Css/chooser.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">

        var DataState = function (value) {

            var src = "../images/state/unknow.gif"
            switch (value) {
                case "1":
                    src = "../images/state/normal.gif"
                    break;
                case "2":
                    src = "../images/state/alarm.gif"
                    break;
                case "3":
                    src = "../images/state/shutdown.gif"
                    break;
                default:
                    src = "../images/state/unknow.gif"
                    break;
            }
            return "<img class='imgEdit' ext:qtip='设备状态' style='cursor:pointer;' src='" + src + "'  />";

        };
        //var Zaixian = function (value) {
        //    return value + "%";
        //}
        function ChangeHeaderInfo(value) {
            switch (value) {
                case "1":
                    GridState.getColumnModel().columns[0].header = "监测点编号";
                    GridState.getColumnModel().columns[1].header = "监测点名称";
                    break;
                case "2":
                    GridState.getColumnModel().columns[0].header = "设备类型编号";
                    GridState.getColumnModel().columns[1].header = "设备类型";
                    break;
                case "3":
                    GridState.getColumnModel().columns[0].header = "服务器类型编号";
                    GridState.getColumnModel().columns[1].header = "服务器类型";
                    break;
                default:
                    GridState.getColumnModel().columns[0].header = "监测点编号";
                    GridState.getColumnModel().columns[1].header = "监测点名称";
                    break;
            }
        }
        function clearTime() {

            CmbState.triggers[0].hide();

        }
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridState.view.findRowIndex(this.triggerElement),
                cellIndex = GridState.view.findCellIndex(this.triggerElement),
                record = StoreState.getAt(rowIndex),
                fieldName = GridState.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);
            if (fieldName == "col9") {

                data = data.toString().substring(0, 10) + " " + data.toString().substring(11, 19);
            }
            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:Hidden ID="QueryData" runat="server" />
        <ext:Hidden ID="HidQueryFlag" runat="server" />
        <ext:Hidden ID="HidStationType" runat="server" />
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="DeviceStatus" />
        <ext:Viewport ID="Viewport1" runat="server">
            <Items>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="0.2">
                            <ext:Panel ID="Panel1" runat="server">
                                <Items>
                                    <ext:TreePanel ID="TreePanel1" runat="server" Icon="Drive" Shadow="None" UseArrows="true"
                                        AutoScroll="true" Animate="true" ContainerScroll="true" EnableDD="true" RootVisible="true"
                                        Header="false" Height="800">
                                    </ext:TreePanel>
                                </Items>
                            </ext:Panel>
                        </ext:LayoutColumn>
                        <ext:LayoutColumn ColumnWidth="0.8">
                            <ext:Panel ID="Panel2" runat="server" Region="Center" DefaultBorder="false" AutoScroll="true" Layout="FitLayout">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar4" runat="server">
                                        <Items>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                            <ext:Button ID="btnStartAll" runat="server" Text='<%# GetLangStr("DeviceStatus9","开始监测") %>' AutoDataBind="true" Icon="ControlPlayBlue" Disabled="true">
                                                <Listeners>
                                                    <Click Handler="this.disable();#{TaskManager1}.startAll();#{btnStopAll}.enable()" />
                                                </Listeners>
                                                <DirectEvents>
                                                    <Click OnEvent="RefreshTimeClick" />
                                                </DirectEvents>
                                            </ext:Button>
                                            <ext:Button ID="btnStopAll" runat="server" Text='<%# GetLangStr("DeviceStatus10","停止监测") %>' AutoDataBind="true" Icon="ControlStopBlue">
                                                <Listeners>
                                                    <Click Handler="this.disable();#{TaskManager1}.stopAll();#{btnStartAll}.enable();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:Label ID="Label2" runat="server" AutoDataBind="true" Text='<%# GetLangStr("DeviceStatus37","设备状态：") %>'> 
                                            </ext:Label>
                                            <ext:ComboBox ID="CmbState" runat="server" Editable="false" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" EmptyText='<%# GetLangStr("DeviceStatus11","选择设备状态...") %>' AutoDataBind="true" SelectOnFocus="true" Width="123">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("DeviceStatus12","清除选中") %>' AutoDataBind="true"/>
                                                </Triggers>
                                                <Listeners>
                                                    <Select Handler="this.triggers[0].show();" />
                                                    <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                    <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                </Listeners>
                                                <Items>
                                                    <ext:ListItem Text='<%# GetLangStr("DeviceStatus13","网络连接") %>' Value="1" />
                                                    <ext:ListItem Text='<%# GetLangStr("DeviceStatus14","网络中断") %>' Value="0" />
                                                </Items>
                                            </ext:ComboBox>
                                            <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" AutoDataBind="true" Text='<%# GetLangStr("DeviceStatus15","查询") %>'>
                                                <DirectEvents>
                                                    <Click OnEvent="TbutQueryClick" />
                                                </DirectEvents>
                                            </ext:Button>
                                            <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" AutoDataBind="true" Text='<%# GetLangStr("DeviceStatus16","重置") %>'> 
                                                <DirectEvents>
                                                    <Click OnEvent="ButResetClick" />
                                                </DirectEvents>
                                            </ext:Button>
                                            <ext:Button ID="Button2" runat="server" Text='<%# GetLangStr("DeviceStatus17","导出Excel") %>' AutoDataBind="true" AutoPostBack="true" OnClick="ToExcel"
                                                Icon="PageExcel">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Items>
                                    <ext:GridPanel ID="GridState" runat="server" StripeRows="true" TrackMouseOver="true"
                                        AutoRender="true">
                                        <Store>
                                            <ext:Store ID="StoreState" runat="server" GroupField="col1">
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
                                                            <ext:RecordField Name="col8" Type="String" />
                                                            <ext:RecordField Name="col9" Type="String" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel ID="ColumnModel1" runat="server">
                                            <Columns>
                                                <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                                                <ext:Column Header='<%# GetLangStr("DeviceStatus3","监测点名称") %>' AutoDataBind="true" Align="Left" DataIndex="col1" Width="100" />
                                                <ext:Column Header='<%# GetLangStr("DeviceStatus6","设备类型") %>' AutoDataBind="true" Align="Center" DataIndex="col3" Width="100" />
                                                <ext:Column Header='<%# GetLangStr("DeviceStatus35","设备名称") %>' AutoDataBind="true" Align="Left" DataIndex="col4" Width="200" />
                                                <ext:Column Header='<%# GetLangStr("DeviceStatus36","设备IP") %>' AutoDataBind="true" Align="Center" DataIndex="col5" Width="200" />
                                                <ext:Column Header='<%# GetLangStr("DeviceStatus37","设备状态") %>' AutoDataBind="true" Width="100" Align="Center" Fixed="true" DataIndex="col6" Resizable="false">
                                                    <Renderer Fn="DataState" />
                                                </ext:Column>
                                                <ext:Column Header='<%# GetLangStr("DeviceStatus38","网络延时") %>' AutoDataBind="true" Align="Center" DataIndex="col7" Width="80" />
                                                <ext:Column Header='<%# GetLangStr("DeviceStatus39","在线率") %>' AutoDataBind="true" Align="Center" DataIndex="col8" Width="80">
                                                </ext:Column>
                                                <ext:DateColumn Header='<%# GetLangStr("DeviceStatus40","更新时间") %>' AutoDataBind="true" Align="Center" DataIndex="col9" Width="200" Format="yyyy-MM-dd HH:mm:ss" />
                                            </Columns>
                                        </ColumnModel>
                                        <View>
                                            <ext:GroupingView ID="GroupingView1" runat="server" ForceFit="true" MarkDirty="false"
                                                ShowGroupName="false" EnableNoGroups="true" HideGroupedColumn="true" GroupByText='<%# GetLangStr("DeviceStatus18","用该列进行分组") %>'
                                                ShowGroupsText='<%# GetLangStr("DeviceStatus19","显示分组") %>' />
                                        </View>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server"></ext:RowSelectionModel>
                                        </SelectionModel>
                                        <LoadMask ShowMask="true" />
                                        <ToolTips>
                                            <ext:ToolTip
                                                ID="RowTip"
                                                runat="server"
                                                Target="={GridState.getView().mainBody}"
                                                Delegate=".x-grid3-cell"
                                                TrackMouse="true">
                                                <Listeners>
                                                    <Show Fn="showTip" />
                                                </Listeners>
                                            </ext:ToolTip>
                                        </ToolTips>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Items>
        </ext:Viewport>
        <ext:TaskManager ID="TaskManager1" runat="server">
            <Tasks>
                <ext:Task TaskID="servertime" Interval="60000" AutoRun="true" OnStart="
                        #{btnStartAll}.setDisabled(true);
                        #{btnStopAll}.setDisabled(false); "
                    OnStop="
                        #{btnStartAll}.setDisabled(false);
                        #{btnStopAll}.setDisabled(true);">
                    <DirectEvents>
                        <Update OnEvent="RefreshTimes">
                            <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="={Ext.getCmp('#{Panel2}').getBody()}"
                                MinDelay="350" />
                        </Update>
                    </DirectEvents>
                </ext:Task>
            </Tasks>
        </ext:TaskManager>
        <ext:Window ID="Window2" runat="server" Icon="House" Title='<%# GetLangStr("DeviceStatus20","设备状态浏览") %>' Hidden="true"
            Height="520px" Width="650px">
            <Items>
                <ext:GridPanel ID="GridStatelock" runat="server" StripeRows="true" Title='<%# GetLangStr("DeviceStatus37","设备状态") %>' Collapsible="true"
                    AutoScroll="true" Height="490">
                    <Store>
                        <ext:Store ID="Storelock" runat="server" GroupField="col2">
                            <Reader>
                                <ext:JsonReader>
                                    <Fields>
                                        <ext:RecordField Name="col0" Type="String" />
                                        <ext:RecordField Name="col1" Type="String" />
                                        <ext:RecordField Name="col2" Type="String" />
                                        <ext:RecordField Name="col3" Type="String" />
                                        <ext:RecordField Name="col4" Type="String" />
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel2" runat="server">
                        <Columns>
                            <ext:Column Header='<%# GetLangStr("DeviceStatus35","设备名称") %>' AutoDataBind="true" Align="Center" DataIndex="col0" Width="170" />
                            <ext:Column Header='<%# GetLangStr("DeviceStatus36","设备IP") %>' AutoDataBind="true" Align="Center" DataIndex="col1" Width="100" />
                            <ext:Column Header='<%# GetLangStr("DeviceStatus6","设备类型") %>' AutoDataBind="true" Align="Center" DataIndex="col2" Width="100" />
                            <ext:TemplateColumn DataIndex="" MenuDisabled="true" Header='<%# GetLangStr("DeviceStatus37","设备状态") %>' AutoDataBind="true" Width="80">
                                <Template ID="Template3" runat="server">
                                    <Html>
                                        <tpl for=".">
                                            <center>
							                    <img src="../images/state/{col3}.jpg"  width="16" height="16" />
                                            </center>
						                </tpl>
                                    </Html>
                                </Template>
                            </ext:TemplateColumn>
                            <ext:Column Header='<%# GetLangStr("DeviceStatus40","更新时间") %>' AutoDataBind="true" Align="Center" DataIndex="col4" Width="120" />
                        </Columns>
                    </ColumnModel>
                </ext:GridPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>