<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogRunning.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.LogRunning" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>操作日志查询</title>
    <link type="text/css" href="../Styles/datetime.css" rel="Stylesheet" />
    <meta http-equiv="Content-Type" content="text/html; charset=GBK" />
    <script language="javascript" src="Scripts/common.js" type="text/javascript" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>
    <script type="text/javascript">
        var DataAmply = function () {
            return '<img class="imgEdit" ext:qtip="查看详细信息" style="cursor:pointer;" src="../images/button/vcard_edit.png" />';
        };
        var cellClick = function (grid, rowIndex, columnIndex, e) {
            var t = e.getTarget(),
                record = grid.getStore().getAt(rowIndex),  // Get the Record
                columnId = grid.getColumnModel().getColumnId(columnIndex); // Get column id

            if (t.className == "imgEdit" && columnId == "Details") {
                return true;
            }

            return false;
        };
        var saveData = function () {
            GridData.setValue(Ext.encode(GridPanel1.getRowsValues(false)));
        }
    </script>
    <script type="text/javascript">
        var showTip = function () {
            var rowIndex = GridPanel1.view.findRowIndex(this.triggerElement),
                cellIndex = GridPanel1.view.findCellIndex(this.triggerElement),
                record = StoreRunning.getAt(rowIndex),
              //  fieldName = GridPanel1.getColumnModel().getDataIndex(cellIndex),
                data = record.get('col2');
            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="LogRunning" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Viewport runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="FormPanel1" runat="server" Height="30" Frame="true" Region="North">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <%--  <ext:Panel runat="server" BodyBorder="false">
                                    <Content>
                                        <div runat="server" id="selectDate" style="width: 475px">
                                            <span class="laydate-span" style="margin-left: 0px; height: 24px;">&nbsp;&nbsp;<%# GetLangStr("LogRunning1","查询时间：")%></span>
                                            <li runat="server" class="laydate-icon" id="start" style="width: 150px; margin-left: 16px; height: 22px;"></li>

                                            <span class="laydate-span" style="margin-left: 20px; height: 24px;">--</span>
                                            <li runat="server" class="laydate-icon" id="end" style="width: 150px; margin-left: 16px; height: 22px;"></li>
                                        </div>
                                    </Content>
                                </ext:Panel>--%>
                                <ext:Panel runat="server" Height="40">
                                    <Content>
                                        <table style="width: 400px">
                                            <tr>
                                                <td style="width: 50px">
                                                    <span class="laydate-span" style="height: 30px; font-size: 15px; margin-left: 12px; margin-right: 2px; margin-top: 5px;">Query time：</span></td>
                                                <td style="width: 150px">
                                                    <li class="laydate-icon" id="start" runat="server" style="width: 150px; height: 25px; margin-left: 5px;"></li>
                                                </td>
                                                <td style="width: 20px;"><span class="laydate-span" style="height: 30px; margin-left: 16px; margin-right: 16px">--</span>
                                                </td>
                                                <td style="width: 150px">
                                                    <li class="laydate-icon" id="end" runat="server" style="width: 150px; height: 25px;"></li>
                                                </td>
                                            </tr>
                                        </table>
                                    </Content>
                                </ext:Panel>
                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("LogRunning1","查询类型：") %>' StyleSpec="margin-left: 10px;">
                                </ext:Label>
                                <ext:ComboBox ID="CmbQueryType" runat="server" Editable="false" DisplayField="col1"
                                    ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("LogRunning2","请选择...")%>'
                                    SelectOnFocus="true" Width="100px">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("LogRunning3","清除选中")%>' AutoDataBind="true" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                    <Store>
                                        <ext:Store ID="StoreQuery" runat="server">
                                            <Reader>
                                                <ext:JsonReader>
                                                    <Fields>
                                                        <ext:RecordField Name="col0" Type="String" />
                                                        <ext:RecordField Name="col1" Type="String" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>
                                <ext:Label ID="Label2" runat="server" Text='<%# GetLangStr("LogRunning4","操作用户：")%>' StyleSpec="margin-left: 10px;">
                                </ext:Label>
                                <ext:ComboBox ID="cmbCzyh" runat="server" Editable="false" DisplayField="col1"
                                    ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("LogRunning2","请选择...")%>'
                                    SelectOnFocus="true" Width="100px">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("LogRunning3","清除选中")%>' AutoDataBind="true" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                    <Store>
                                        <ext:Store ID="StoreCzyh" runat="server">
                                            <Reader>
                                                <ext:JsonReader>
                                                    <Fields>
                                                        <ext:RecordField Name="col0" Type="String" />
                                                        <ext:RecordField Name="col1" Type="String" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("LogRunning5","查询")%>' StyleSpec="margin-left: 10px;">
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <ext:GridPanel ID="GridPanel1" runat="server" StripeRows="true" AutoExpandColumn="userevent"
                    TrackMouseOver="true" Region="Center">
                    <Store>
                        <ext:Store ID="StoreRunning" runat="server" OnRefreshData="MyData_Refresh">
                            <AutoLoadParams>
                                <ext:Parameter Name="start" Value="={0}" />
                                <ext:Parameter Name="limit" Value="={15}" />
                            </AutoLoadParams>
                            <UpdateProxy>
                                <ext:HttpWriteProxy Method="GET" Url="LogRunning.aspx">
                                </ext:HttpWriteProxy>
                            </UpdateProxy>
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
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                            <ext:Column ColumnID="Details" Header='<%# GetLangStr("LogRunning6", "详细")%>' AutoDataBind="true" Width="50" Align="Center" Fixed="true"
                                MenuDisabled="true" Resizable="false" Hidden="true">
                                <Renderer Fn="DataAmply" />
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("LogRunning7", "记录编号")%>' AutoDataBind="true" Align="Center" Sortable="true" DataIndex="col0" Hidden="true" />
                            <ext:Column Header='<%# GetLangStr("LogRunning8", "操作用户")%>' AutoDataBind="true" Align="Center" Sortable="true" DataIndex="col1" Width="90" />
                            <ext:Column Header='<%# GetLangStr("LogRunning9", "操作类型")%>' AutoDataBind="true" Align="Center" Sortable="true" DataIndex="col6" Width="90" />
                            <ext:Column Header='<%# GetLangStr("LogRunning10", "操作事件")%>' Align="left" AutoDataBind="true" Sortable="true" DataIndex="col2" ColumnID="userevent" />
                            <ext:Column Header='<%# GetLangStr("LogRunning11", "用户IP" )%>' AutoDataBind="true" Align="Center" Sortable="true" DataIndex="col3" Width="100" />
                            <ext:DateColumn Header='<%# GetLangStr("LogRunning12","记录时间")%>' AutoDataBind="true" Align="Center" Sortable="true" DataIndex="col4" Format="yyyy-MM-dd HH:mm:ss" Width="150" />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" />
                    </SelectionModel>
                    <LoadMask ShowMask="true" />
                    <TopBar>
                        <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StoreRunning">
                            <Items>
                                <ext:Button ID="ButExcel" runat="server" Text='<%# GetLangStr("LogRunning13","导出Excel")%>' AutoPostBack="true" OnClick="ToExcel"
                                    Icon="PageExcel">
                                </ext:Button>
                            </Items>
                        </ext:PagingToolbar>
                    </TopBar>
                    <Listeners>
                        <CellClick Fn="cellClick" />
                    </Listeners>
                    <DirectEvents>
                        <CellClick OnEvent="ShowDetails" Failure="Ext.MessageBox.alert('加载失败', '提示');">
                            <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="={#{GridPanel1}.body}" />
                            <ExtraParams>
                                <ext:Parameter Name="data" Value="params[0].getStore().getAt(params[1]).data" Mode="Raw" />
                            </ExtraParams>
                        </CellClick>
                    </DirectEvents>
                    <View>
                        <ext:GridView runat="server"></ext:GridView>
                    </View>
                    <ToolTips>
                        <ext:ToolTip ID="RowTip" runat="server" Target="={#{GridPanel1}.getView().mainBody}"
                            Delegate=".x-grid3-row" TrackMouse="true">
                            <Listeners>
                                <Show Fn="showTip" />
                            </Listeners>
                        </ext:ToolTip>
                    </ToolTips>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
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
            LogRunning.GetDateTime(true, tt);
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
            LogRunning.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>