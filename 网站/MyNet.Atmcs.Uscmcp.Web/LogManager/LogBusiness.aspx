<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogBusiness.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.LogBusiness" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>交互日志查询</title>
    <link type="text/css" href="../Styles/datetime.css" rel="Stylesheet" />
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <script language="javascript" src="Scripts/common.js" type="text/javascript" charset="UTF-8"></script>
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
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="LogBusiness" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Viewport runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="FormPanel1" runat="server" Height="30" Frame="true" Region="North">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Panel runat="server" BodyBorder="false">
                                    <Content>
                                        <div runat="server" id="selectDate" style="width: 475px">
                                            <span class="laydate-span" style="margin-left: 0px; height: 24px;">&nbsp;&nbsp;<%# GetLangStr("LogBusiness1","查询时间")%></span>
                                            <li runat="server" class="laydate-icon" id="start" style="width: 150px; margin-left: 16px; height: 22px;"></li>
                                        </div>
                                        <div>
                                            <span class="laydate-span" style="margin-left: 20px; height: 24px;">--</span>
                                            <li class="laydate-icon" runat="server" id="end" style="width: 150px; margin-left: 16px; height: 22px;"></li>
                                        </div>
                                    </Content>
                                </ext:Panel>
                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("LogBusiness2","查询类型：") %>' StyleSpec="margin-left: 10px;">
                                </ext:Label>
                                <ext:ComboBox ID="CmbQueryType" runat="server" Editable="false" DisplayField="CODEDESC"
                                    ValueField="CODE" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("LogBusiness3","选择查询条件...")%>'
                                    SelectOnFocus="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("LogBusiness4","清除选中")%>' AutoDataBind="true" />
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
                                                        <ext:RecordField Name="CODE" Type="String" />
                                                        <ext:RecordField Name="CODEDESC" Type="String" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("LogBusiness5","查询")%>'>
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
                    Region="Center">
                    <Store>
                        <ext:Store ID="StoreRunning" runat="server" OnRefreshData="MyData_Refresh">
                            <AutoLoadParams>
                                <ext:Parameter Name="start" Value="={0}" />
                                <ext:Parameter Name="limit" Value="={15}" />
                            </AutoLoadParams>
                            <UpdateProxy>
                                <ext:HttpWriteProxy Method="GET" Url="LogBusiness.aspx">
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
                                        <ext:RecordField Name="col8" />
                                        <ext:RecordField Name="col9" />
                                        <ext:RecordField Name="col10" />
                                        <ext:RecordField Name="col11" />
                                        <ext:RecordField Name="col12" />
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                            <ext:Column ColumnID="Details" Header='<%# GetLangStr("LogBusiness6", "详细")%>' AutoDataBind="true" Align="Center" Fixed="true"
                                MenuDisabled="true" Resizable="false" Hidden="false">
                                <Renderer Fn="DataAmply" />
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("LogBusiness7", "记录编号")%>' AutoDataBind="true" Sortable="true" DataIndex="col0" Hidden="true" />
                            <ext:Column Header='<%# GetLangStr("LogBusiness8", "操作用户")%>' AutoDataBind="true" Sortable="true" DataIndex="col3">
                            </ext:Column>
                            <ext:Column ColumnID="userevent" Header='<%# GetLangStr("LogBusiness9", "操作事件")%>' AutoDataBind="true" Sortable="true" DataIndex="col5">
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("LogBusiness10", "用户IP" )%>' AutoDataBind="true" Sortable="true" DataIndex="col1">
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("LogBusiness11", "接口地址")%>' AutoDataBind="true" Sortable="true" DataIndex="col9">
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("LogBusiness12","接口名称")%>' AutoDataBind="true" Sortable="true" DataIndex="col10">
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("LogBusiness13","记录时间")%>' AutoDataBind="true" Sortable="true" DataIndex="col12">
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" />
                    </SelectionModel>
                    <LoadMask ShowMask="true" />
                    <TopBar>
                        <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StoreRunning">
                            <Items>
                                <ext:Button ID="ButExcel" runat="server" Text='<%# GetLangStr("LogBusiness14","导出Excel")%>' AutoPostBack="true" OnClick="ToExcel"
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
        <ext:Window runat="server" ID="winDetails" Width="800" Height="600" Layout="FitLayout" Title='<%# GetLangStr("LogBusiness15","交互日志详细信息")%>' AutoDataBind="true" Hidden="true">
            <Items>
                <ext:Panel runat="server" Title='<%# GetLangStr("LogBusiness16","详细信息")%>' StyleSpec="background:#b5cbd8;" Height="550">
                    <Items>
                        <ext:Container runat="server" Layout="RowLayout" Height="550">
                            <Items>
                                <ext:Container ID="Container1" runat="server" Layout="ColumnLayout" Height="30">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("LogBusiness10","用户IP")%>' AutoDataBind="true" ID="txtUserIp" ReadOnly="true" ColumnWidth="0.33" />
                                        <%--   <ext:TextField runat="server" FieldLabel="用户ID" ID="txtUserId" ReadOnly="true" ColumnWidth="0.33" />--%>
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("LogBusiness18","用户名称")%>' AutoDataBind="true" ID="txtUserNmae" ReadOnly="true" ColumnWidth="0.33" />
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("LogBusiness19","服务名称")%>' AutoDataBind="true" ID="txtServiceName" ReadOnly="true" ColumnWidth="0.33" />
                                    </Items>
                                </ext:Container>
                                <ext:Container ID="Container2" Height="30" runat="server" Layout="ColumnLayout" StyleSpec="margin-top:3px;">
                                    <Items>
                                        <%--<ext:TextField runat="server" FieldLabel="功能模块编号" ID="txtFunctionId" ReadOnly="true" ColumnWidth="0.33" />--%>
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("LogBusiness20","操作模块名称")%>' AutoDataBind="true" ID="txtFunctionName" ReadOnly="true" ColumnWidth="0.33" />
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("LogBusiness13","记录时间")%>' AutoDataBind="true" ID="txtJlsj" ReadOnly="true" ColumnWidth="0.33" />
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("LogBusiness22","服务地址")%>' ID="txtServiceDz" ReadOnly="true" ColumnWidth="0.33" />
                                    </Items>
                                </ext:Container>
                                <%--   <ext:Container ID="Container3" runat="server" Layout="ColumnLayout" StyleSpec="margin-top:5px;">
                                            <Items>
                                                <ext:TextField runat="server" FieldLabel="服务编号" ID="txtServiceId" ReadOnly="true" ColumnWidth="0.33" />
                                            </Items>
                                        </ext:Container>--%>
                                <ext:Container ID="Container4" Height="30" runat="server" Layout="ColumnLayout" StyleSpec="margin-top:3px;">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("LogBusiness23","调用者传参")%>' ID="txtUserParameter" ReadOnly="true" ColumnWidth="0.66" />
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("LogBusiness24","服务接口名称")%>' ID="txtServiceInterfaceName" ReadOnly="true" ColumnWidth="0.33" />
                                    </Items>
                                </ext:Container>
                                <ext:Container ID="Container5" runat="server" StyleSpec="margin-top:3px;">
                                    <Items>
                                        <ext:TextArea runat="server" FieldLabel='<%# GetLangStr("LogBusiness25","返回结果")%>' ID="txaResult" Height="300" Width="785">
                                        </ext:TextArea>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:Container>
                    </Items>
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
            LogBusiness.GetDateTime(true, tt);
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
            LogBusiness.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>