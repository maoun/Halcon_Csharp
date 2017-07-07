<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeccancyOperateAnalysis.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.PeccancyOperateAnalysis" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>违法操作统计</title>
    <link type="text/css" href="../Styles/datetime.css" rel="Stylesheet" />
    <meta http-equiv="Content-Type" content="text/html; charset=GBK" />
    <script type="text/javascript" src="../Scripts/common.js"></script>
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

                                <ext:Panel runat="server" Height="40">
                                    <Content>
                                        <table style="width: 400px">
                                            <tr>
                                                <td style="width: 50px">
                                                    <span class="laydate-span" style="height: 30px; font-size: 15px; margin-left: 12px; margin-right: 2px; margin-top: 5px;">查询时间：</span></td>
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
                                <ext:Label ID="Label1" runat="server" Text="操作类型：" StyleSpec="margin-left: 10px;">
                                </ext:Label>
                                <ext:ComboBox ID="CmbQueryType" runat="server" Editable="false" DisplayField="col1"
                                    ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PeccancyOperateAnalysis3","请选择...")%>'
                                    SelectOnFocus="true" Width="100px">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
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
                                <ext:Label ID="Label2" runat="server" Text='操作用户：' StyleSpec="margin-left: 10px;">
                                </ext:Label>
                                <ext:ComboBox ID="cmbCzyh" runat="server" Editable="false" DisplayField="col1"
                                    ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='请选择...'
                                    SelectOnFocus="true" Width="100px">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
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
                                <ext:Label ID="Label3" runat="server" Text="次数>= " StyleSpec="margin-left: 10px;">
                                </ext:Label>
                                <ext:TextField runat="server" ID="txtCishu" Width="100"></ext:TextField>
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("PeccancyOperateAnalysis4","查询")%>' StyleSpec="margin-left: 10px;">
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
                                        <ext:RecordField Name="col7" />
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                            <ext:Column ColumnID="Details" Header='<%# GetLangStr("PeccancyOperateAnalysis6", "详细")%>' AutoDataBind="true" Align="Center" Fixed="true"
                                MenuDisabled="true" Resizable="false">
                                <Renderer Fn="DataAmply" />
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("PeccancyOperateAnalysis7", "记录编号")%>' AutoDataBind="true" Align="Center" Sortable="true" DataIndex="col0" Hidden="true" />
                            <ext:Column Header='<%# GetLangStr("PeccancyOperateAnalysis8", "操作用户")%>' AutoDataBind="true" Align="Center" Sortable="true" DataIndex="col1" />
                            <ext:Column Header='<%# GetLangStr("PeccancyOperateAnalysis42", "操作类型")%>' AutoDataBind="true" Align="Center" Sortable="true" DataIndex="col3" />
                            <ext:Column Header="号牌号码" ColumnID="userevent" AutoDataBind="true" Align="Center" Sortable="true" DataIndex="col4" />
                            <ext:Column Header="次数" AutoDataBind="true" Align="Center" Sortable="true" DataIndex="col5" />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" />
                    </SelectionModel>
                    <LoadMask ShowMask="true" />
                    <TopBar>
                        <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StoreRunning">
                            <Items>
                                <ext:Button ID="ButExcel" runat="server" Text='<%# GetLangStr("PeccancyOperateAnalysis14","导出Excel")%>' AutoPostBack="true" OnClick="ToExcel"
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
                        <ext:GridView runat="server" ForceFit="true"></ext:GridView>
                    </View>
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