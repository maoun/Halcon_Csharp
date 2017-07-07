<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviceStatistics.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.DeviceStatistics" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>设备信息统计</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <link rel="Stylesheet" href="../Styles/datetime.css" type="text/css" />
    <script language="javascript" src="Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script language="Javascript" src="FusionCharts/FusionCharts.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="utf-8"></script>
    <script type="text/javascript" language="javascript">
        function clearTime(start, end) {
            document.getElementById("start").innerText = start;
            document.getElementById("end").innerText = end;
        }
        var employeeDetailsRender = function () {
            return '<img class="imgEdit" ext:qtip="点击查看详细信息" style="cursor:pointer;" src="../Images/vcard_edit.png"  />';
        };

        function open() {
            var row = GridDeviceManager.getSelectionModel().getSelections();
            var id = row[0].data.col0.toString();
            //document.getElementById("Hidden1").value = id;

            OnEvl.onclick(id);
        }
        function tubiao() {

            if (GridDeviceManager.getStore().data.length == 0) {
                Ext.MessageBox.alert('提示', '请先查询信息'); return false;
            }
            else {
                var s1 = GridDeviceManager.getStore().data.items[0].data.col0;
                var s2 = GridDeviceManager.getStore().data.items[0].data.col1;
                var s3 = GridDeviceManager.getStore().data.items[0].data.col2;
                var s4 = GridDeviceManager.getStore().data.items[0].data.col3;
                OnEvl.tubiao(s1, s2, s3, s4);
                Window1.show();
            }

        }

        function updata() {

            var row = GridDeviceManager.getSelectionModel().getSelections();

            if (row[0] == null) {
                Ext.MessageBox.alert('提示', '请选择需要处理记录'); return false;
            }
            else {

                var id = row[0].data.col0.toString();

                OnEvl.update(id);
            }

        }
        function updatedevice() {
            var row = GridDeviceManager.getSelectionModel().getSelections();
            var id = row[0].data.col0.toString();
            OnEvl.updatedevice(id);
        }
        function delecte() {
            var row = GridDeviceManager.getSelectionModel().getSelections();

            if (row[0] == null) {
                Ext.MessageBox.alert('提示', '请选择需要处理记录'); return false;
            }
            else {

                var id = row[0].data.col0.toString();
                var type = row[0].data.col4.toString();
                Ext.MessageBox.confirm("删除?", "确定删除该条数据?", function (code) {
                    if (code == "yes") {
                        OnDel.delecte(id, type);

                    } else {

                        return false;
                    }
                });
            }

        }
        function selectNode(value) {

            OnEvl.onclickTree(value);

        }
        function xuanze() {

            var value = comyangshi.getValue();
            var s1 = GridDeviceManager.getStore().data.items[0].data.col0;
            var s2 = GridDeviceManager.getStore().data.items[0].data.col1;
            var s3 = GridDeviceManager.getStore().data.items[0].data.col2;
            var s4 = GridDeviceManager.getStore().data.items[0].data.col3;
            OnEvl.xuanze(value, s1, s2, s3, s4);
        }
        var getRowClass = function (record, rowIndex, p, ds) {
            var reColor = "";
            if (record.data.col5 == 0) {

                reColor = "x-grid-row-summary";
            }

            return reColor;
        };
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridDeviceManager.view.findRowIndex(this.triggerElement),
                cellIndex = GridDeviceManager.view.findCellIndex(this.triggerElement),
                record = StoreDeviceManager.getAt(rowIndex),
                fieldName = GridDeviceManager.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input id="Hidden1" type="hidden" runat="server" />
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="DeviceStatistics" />
        <ext:Hidden ID="GridData" runat="server" />
        <%--    <ext:Button ID="butt"  runat="server" OnClick="Button4_Click" Visible="false">
    </ext:Button>--%>
        <ext:Store ID="StoreLocation" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreDepart" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreDeviceType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreCardType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreComType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreNetworkType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="Panel1" Region="Center" runat="server" Layout="FitLayout"
                    Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server" Layout="Container">
                            <Items>
                                <ext:Toolbar ID="Toolbar4" runat="server">
                                    <Items>
                                        <ext:Panel runat="server" BodyBorder="false">
                                            <Content>
                                                <div runat="server" id="selectDate" style="width: 475px">
                                                    <span class="laydate-span" style="margin-left: 0px; height: 24px;">&nbsp;&nbsp;<%# GetLangStr("DeviceStatistics1","查询时间") %></span>
                                                    <li runat="server" class="laydate-icon" id="start" style="width: 150px; margin-left: 16px; height: 22px;"></li>
                                                </div>
                                                <div>
                                                    <span class="laydate-span" style="margin-left: 20px; height: 24px;">--</span>
                                                    <li runat="server" class="laydate-icon" id="end" style="width: 150px; margin-left: 16px; height: 22px;"></li>
                                                </div>
                                            </Content>
                                        </ext:Panel>
                                        <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("DeviceStatistics2","查询") %>'>
                                            <DirectEvents>
                                                <Click OnEvent="TbutQueryClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("DeviceStatistics3","重置") %>'>
                                            <DirectEvents>
                                                <Click OnEvent="ButResetClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButRefresh" runat="server" Icon="DriveGo"  Hidden="true"  Text='<%# GetLangStr("DeviceStatistics4","刷新") %>'>
                                            <DirectEvents>
                                                <Click OnEvent="ButRefreshClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="Button4" runat="server" Icon="DriveLink" Text='<%# GetLangStr("DeviceStatistics5","显示图表信息") %>'>
                                            <Listeners>
                                                <Click Fn="tubiao" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                                <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StoreDeviceManager">
                                    <Items>
                                        <ext:Toolbar ID="ToolExport" runat="server">
                                            <Items>
                                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                                <ext:Button ID="Button2" runat="server" Text='<%# GetLangStr("DeviceStatistics8","导出Excel") %>' AutoPostBack="true" OnClick="ToExcel"
                                                    Icon="PageExcel">
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </Items>
                                </ext:PagingToolbar>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridDeviceManager" runat="server" StripeRows="true" BodyStyle="height:100%;width:100%" TrackMouseOver="true">
                            <Store>
                                <ext:Store ID="StoreDeviceManager" runat="server" OnRefreshData="MyData_Refresh">
                                    <AutoLoadParams>
                                        <ext:Parameter Name="start" Value="={0}" />
                                        <ext:Parameter Name="limit" Value="={15}" />
                                    </AutoLoadParams>
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
                                                <ext:RecordField Name="col10" Type="String" />
                                                <ext:RecordField Name="col11" Type="String" />
                                                <ext:RecordField Name="col12" Type="String" />
                                                <ext:RecordField Name="col13" Type="String" />
                                                <ext:RecordField Name="col14" Type="String" />
                                                <ext:RecordField Name="col15" Type="String" />
                                                <ext:RecordField Name="col16" Type="String" />
                                                <ext:RecordField Name="col17" Type="String" />
                                                <ext:RecordField Name="col18" Type="String" />
                                                <ext:RecordField Name="col19" Type="String" />
                                                <ext:RecordField Name="col20" Type="String" />
                                                <ext:RecordField Name="col21" Type="String" />
                                                <ext:RecordField Name="col22" Type="String" />
                                                <ext:RecordField Name="col23" Type="String" />
                                                <ext:RecordField Name="col24" Type="String" />
                                                <ext:RecordField Name="col25" Type="String" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                                    <ext:Column Header='<%# GetLangStr("DeviceStatistics12","领用")%>' AutoDataBind="true" DataIndex="col0" Width="150" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("DeviceStatistics13","维修") %>' AutoDataBind="true" DataIndex="col1" Width="150" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("DeviceStatistics14","入库") %>' AutoDataBind="true" DataIndex="col2" Width="150" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("DeviceStatistics15","归还") %>' AutoDataBind="true" DataIndex="col3" Width="150" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <Listeners>
                                <RowDblClick Handler="open()" />
                            </Listeners>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="false">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <View>
                                <ext:GridView ID="GroupingView1" runat="server" ForceFit="true">
                                    <GetRowClass Fn="getRowClass" />
                                </ext:GridView>
                            </View>
                            <ToolTips>
                                <ext:ToolTip
                                    ID="RowTip"
                                    runat="server"
                                    Target="={GridDeviceManager.getView().mainBody}"
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
            </Items>
        </ext:Viewport>
        <ext:Window ID="Window1" runat="server" Icon="House" Title='<%# GetLangStr("DeviceStatistics16","图表显示") %>' Hidden="true" Height="500px"
            Width="520px" MonitorValid="true">
            <Items>
                <ext:ComboBox ID="comyangshi" runat="server" FieldLabel='<%# GetLangStr("DeviceStatistics17","选择类型") %>' EmptyText='<%# GetLangStr("DeviceStatistics22","选择样式...") %>'
                    Width="300">
                    <Items>
                        <ext:ListItem Text='<%# GetLangStr("DeviceStatistics18","2D圆饼图") %>' AutoDataBind="true" Value="01" />
                        <ext:ListItem Text='<%# GetLangStr("DeviceStatistics19","线状图") %>' Value="02" />
                        <ext:ListItem Text='<%# GetLangStr("DeviceStatistics20","3D柱形图") %>' Value="03" />
                        <ext:ListItem Text='<%# GetLangStr("DeviceStatistics21","3D圆饼图") %>' Value="04" />
                    </Items>
                    <Listeners>
                        <Select Fn="xuanze" />
                    </Listeners>
                </ext:ComboBox>
                <ext:Panel runat="server" Height="400px" Width="500px" ID="panel">
                </ext:Panel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>

<script type="text/javascript">
    laydate.skin('danlan');
    var start = {
        elem: '#start',
        format: 'YYYY-MM-DD hh:mm:ss',
        //min: laydate.now(), //设定最小日期为当前日期
        max: '2099-06-16 23:59:59', //最大日期
        istime: true,
        istoday: true,
        choose: function (datas) {
            end.min = datas; //开始日选好后，重置结束日的最小日期
            end.start = datas //将结束日的初始值设定为开始日
            $("#end").click();//开始时间选中后，自动弹出结束时间
            var tt = $("#start").html();
            DeviceStatistics.GetDateTime(true, tt);
            //alert(tt);
        }
    };
    var end = {
        elem: '#end',
        format: 'YYYY-MM-DD hh:mm:ss',
        min: laydate.now(),
        max: '2099-06-16 23:59:59',
        istime: true,
        istoday: true,
        choose: function (datas) {
            start.max = datas; //结束日选好后，重置开始日的最大日期
            var tt = $("#end").html();
            DeviceStatistics.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>