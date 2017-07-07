<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogError.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.LogError" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>错误日志查询</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link type="text/css" href="../Styles/datetime.css" rel="Stylesheet" />
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
    <script type="text/javascript">
        var showTip = function () {
            var rowIndex = GridPanel1.view.findRowIndex(this.triggerElement),
                cellIndex = GridPanel1.view.findCellIndex(this.triggerElement),
                record = StoreRunning.getAt(rowIndex),
              fieldName = GridPanel1.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);
            if (fieldName == "col3") {

                data = data.toString().substring(0, 10) + " " + data.toString().substring(11, 19);
            }
            this.body.dom.innerHTML = data;
        };
    </script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="LogError" />
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
                                            <span class="laydate-span" style="margin-left: 0px; height: 24px;"><%# GetLangStr("LogError1","查询时间")%></span>
                                            <li class="laydate-icon" runat="server" id="start" style="width: 150px; margin-left: 16px; height: 22px;"></li>
                                        </div>
                                        <div>
                                            <span class="laydate-span" style="margin-left: 20px; height: 24px;">--</span>
                                            <li class="laydate-icon" id="end" runat="server" style="width: 150px; margin-left: 16px; height: 22px;"></li>
                                        </div>
                                    </Content>
                                </ext:Panel>
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("LogError2","查询")%>'>
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
                <ext:GridPanel ID="GridPanel1" runat="server" StripeRows="true" Region="Center" AutoExpandColumn="userevent">
                    <Store>
                        <ext:Store ID="StoreRunning" runat="server" OnRefreshData="MyData_Refresh">
                            <AutoLoadParams>
                                <ext:Parameter Name="start" Value="={0}" />
                                <ext:Parameter Name="limit" Value="={15}" />
                            </AutoLoadParams>
                            <UpdateProxy>
                                <ext:HttpWriteProxy Method="GET" Url="LogError.aspx">
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
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                            <ext:Column ColumnID="Details" Header='<%# GetLangStr("LogError3","详细")%>' AutoDataBind="true" Align="Center" Fixed="true"
                                MenuDisabled="true" Resizable="false" Hidden="true">
                                <Renderer Fn="DataAmply" />
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("LogError4","错误源")%>' AutoDataBind="true" Align="Left" Sortable="true" DataIndex="col1" Width="200" />
                            <ext:Column ColumnID="userevent" Header='<%# GetLangStr("LogError5","错误信息")%>' AutoDataBind="true" Align="left" Sortable="true" DataIndex="col2">
                            </ext:Column>
                            <ext:DateColumn Header='<%# GetLangStr("LogError6","产生日期")%>' AutoDataBind="true" Align="Center" Sortable="true" DataIndex="col3" Format="yyyy-MM-dd HH:mm:ss" Width="150">
                            </ext:DateColumn>
                            <ext:Column Header='<%# GetLangStr("LogError7","错误描述")%>' AutoDataBind="true" Align="left" Sortable="true" DataIndex="col4" Width="180">
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
                                <ext:Button ID="ButExcel" runat="server" Text='<%# GetLangStr("LogError8","导出Excel")%>' AutoPostBack="true" OnClick="ToExcel"
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
                        <ext:GridView ID="GridView1" runat="server">
                        </ext:GridView>
                    </View>
                    <ToolTips>
                        <ext:ToolTip ID="RowTip" runat="server" Target="={#{GridPanel1}.getView().mainBody}"
                            Delegate=".x-grid3-cell" TrackMouse="true">
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
            LogError.GetDateTime(true, tt);
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
            LogError.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>