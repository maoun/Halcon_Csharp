<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebDeviceShow.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Template.WebDeviceShow" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>设备异常详情</title>
            <link href="../Styles/NewPageStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8">></script>
    <script type="text/javascript" src="../Scripts/common.js" charset="UTF-8">></script>
    <script src="../Scripts/showphoto.js" type="text/javascript" charset="UTF-8">></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8">></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8">></script>
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
        function clearTime(start, end) {
            document.getElementById("start").innerText = start;
            document.getElementById("end").innerText = end;
            CmbDeviceType.triggers[0].hide();
            CmbSBXH.triggers[0].hide();
            ComboBox1.triggers[0].hide();
            ComboBox2.triggers[0].hide();
        }
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridDeviceManager.view.findRowIndex(this.triggerElement),
                cellIndex = GridDeviceManager.view.findCellIndex(this.triggerElement),
                record = StoreDevice.getAt(rowIndex),
                fieldName = GridDeviceManager.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);
            if (fieldName == "col5" || fieldName == "col6") {

                data = data.toString().substring(0, 10) + " " + data.toString().substring(11, 19);
            }
            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="WebDeviceShow" />
        <ext:Store ID="StoreDeviceType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreSDeviceMode" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                        <ext:RecordField Name="col3" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreConnectState" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreWorkState" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <%--中间--%>
                <ext:FormPanel ID="Panel1" Region="Center" runat="server" 
                    Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server" Layout="ContainerLayout">
                            <Items>
                                <ext:Toolbar ID="Toolbar4" runat="server">
                                    <Items>
                                        <ext:Panel runat="server" Height="40">
                                            <Content>
                                                <table style="width: 400px">
                                                    <tr>
                                                        <td style="width: 50px">
                                                            <span style="float: left; height: 30px; font-size: 15px; line-height: 24px!important; text-align: center; margin-left: 10px;">查询时间：</span></td>
                                                        <td style="width: 150px">
                                                            <li class="laydate-icon" id="start" runat="server" style="width: 150px; float: left; list-style: none; cursor: pointer; height: 25px; line-height: 22px!important; margin-left: 5px;"></li>
                                                        </td>
                                                        <td style="width: 20px;"><span style="height: 30px; line-height: 24px!important; text-align: center; margin-left: 0px">--</span>
                                                        </td>
                                                        <td style="width: 150px">
                                                            <li class="laydate-icon" id="end" runat="server" style="width: 150px; float: left; list-style: none; cursor: pointer; height: 25px; line-height: 22px!important"></li>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Content>
                                        </ext:Panel>
                                        <ext:Label ID="Label3" runat="server" Text="设备名称：" StyleSpec="margin-left: 10px; margin-right:10px;">
                                        </ext:Label>
                                        <ext:TextField ID="TxtDeviceName" runat="server" Width="250" Disabled="true" />
                                        <ext:Label ID="Label1" runat="server" Text="设备类型：" StyleSpec="margin-left:10px;margin-right:10px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbDeviceType" runat="server" Editable="false" StoreID="StoreDeviceType"
                                            DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                            EmptyText="选择设备类型..." SelectOnFocus="true" Width="150">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();WebDeviceShow.SelectQDevice();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar runat="server" ID="Toolbar5">
                                    <Items>

                                        <ext:Label ID="Label4" runat="server" Text="设备型号：" StyleSpec="margin-left:10px;margin-right:10px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbSBXH" runat="server" Editable="false" StoreID="StoreSDeviceMode"
                                            DisplayField="col3" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                            EmptyText="选择设备型号..." SelectOnFocus="true" Width="172">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>

                                        <ext:Label ID="Label5" runat="server" Text="网络状态：" StyleSpec="margin-left:15px;margin-right:10px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="ComboBox1" runat="server" Editable="false" StoreID="StoreConnectState"
                                            DisplayField="CODEDESC" ValueField="CODE" TypeAhead="true" Mode="Local" ForceSelection="true"
                                            EmptyText="选择网络状态..." SelectOnFocus="true" Width="85">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:Label ID="Label6" runat="server" Text="工作状态：" StyleSpec="margin-left:10px;margin-right:10px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="ComboBox2" runat="server" Editable="false" StoreID="StoreWorkState"
                                            DisplayField="CODEDESC" ValueField="CODE" TypeAhead="true" Mode="Local" ForceSelection="true"
                                            EmptyText="选择工作状态..." SelectOnFocus="true" Width="150">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text="查询">
                                            <DirectEvents>
                                                <Click OnEvent="TbutQueryClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text="重置">
                                            <DirectEvents>
                                                <Click OnEvent="ButResetClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>

                    <Items>

                        <ext:GridPanel ID="GridDeviceManager" runat="server" StripeRows="true" Title="历史设备状态信息"
                            Height="500" TrackMouseOver="true" >
                            <TopBar>
                                <ext:Toolbar ID="Toolbar2" runat="server" Layout="Container">
                                    <Items>
                                        <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StoreDevice">
                                        </ext:PagingToolbar>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Store>
                                <ext:Store ID="StoreDevice" runat="server">
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
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                                    <ext:Column Header="设备状态" AutoDataBind="true" Width="100" Align="Center" Fixed="true" DataIndex="col0" Resizable="false">
                                        <Renderer Fn="DataState" />
                                    </ext:Column>
                                    <ext:Column Header="设备名称" AutoDataBind="true" DataIndex="col1" Width="280" Align="Left" />
                                    <ext:Column Header="设备IP" AutoDataBind="true" DataIndex="col2" Width="150" Align="Center" />
                                    <ext:Column Header="网络状态" AutoDataBind="true" DataIndex="col3" Width="140" Align="Center" />
                                    <ext:Column Header="工作状态" AutoDataBind="true" DataIndex="col4" Width="140" Align="Center" />
                                    <ext:DateColumn Header="正常工作时间" AutoDataBind="true" DataIndex="col5" Width="180" Align="Center" Format="yyyy-MM-dd HH:mm:ss" />
                                    <ext:DateColumn Header="最后更新时间" AutoDataBind="true" DataIndex="col6" Width="180" Align="Center" Format="yyyy-MM-dd HH:mm:ss" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel></SelectionModel>
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
    </form>
</body>
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
            end.start = datas; //将结束日的初始值设定为开始日
            $("#end").click();//开始时间选中后，自动弹出结束时间
            var tt = $("#start").html();

            WebDeviceShow.GetDateTime(true, tt);

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
            WebDeviceShow.GetDateTime(false, tt);
        }

    };
    laydate(start);
    laydate(end);
</script>
</html>