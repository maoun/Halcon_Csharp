<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnalyFollowVehicles.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.AnalyFollowVehicles" %>

<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <link href="../../../../resources/css/examples.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8"></script>
    <script type="text/javascript">
        $(function () {
            $("body").delegate("#TxtplateId", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#CmbPlateType").click();
                }
            })
        })

        var changeUpper = function (obj, value) {
            obj.setValue(value.toUpperCase());
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:Hidden ID="FormatType" runat="server" />
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport ID="Panel1" runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="Panel2" runat="server" Region="North" Title="" AutoHeight="true">
                    <TopBar>
                        <ext:Toolbar runat="server" Layout="ContainerLayout">
                            <Items>
                                <ext:Toolbar runat="server" LabelAlign="Right">
                                    <Items>
                                        <ext:Panel runat="server" X="500" Y="0" Height="60" HideBorders="true" Width="700">
                                            <Content>
                                                <div id="selectDate" style="margin-top: 15px">
                                                    <span style="float: left; margin-left: 0px; height: 24px; line-height: 24px!important; text-align: center">&nbsp;&nbsp;&nbsp;&nbsp;查询时间</span><li runat="server" class="laydate-icon" id="start" style="width: 250px; margin-left: 16px; float: left; list-style: none; cursor: pointer; height: 22px; line-height: 22px!important"></li>
                                                </div>
                                                <div style="margin-top: 5px">
                                                    <span style="float: left; margin-left: 20px; height: 24px; line-height: 24px!important; text-align: center">--</span><li runat="server" class="laydate-icon" id="end" style="width: 250px; margin-left: 16px; float: left; list-style: none; cursor: pointer; height: 22px; line-height: 22px!important"></li>
                                                </div>
                                            </Content>
                                        </ext:Panel>
                                        <ext:Label ID="LBcplx" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;号牌种类：" />
                                        <ext:SelectBox ID="CBcplx" runat="server" MinChars="1"
                                            ValueField="col0" DisplayField="col1" EmptyText="选择号牌种类">
                                            <Store>
                                                <ext:Store ID="Scplx" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader>
                                                            <Fields>
                                                                <ext:RecordField Name="col0" />
                                                                <ext:RecordField Name="col1" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); }" />
                                            </Listeners>
                                        </ext:SelectBox>
                                        <ext:Label ID="LBbsjg" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;伴随间隔：" />
                                        <ext:SpinnerField
                                            ID="SFbsjg"
                                            runat="server"
                                            MinValue="0"
                                            MaxValue="100"
                                            IncrementValue="1"
                                            Width="50" />
                                        <ext:Label ID="LBjgsj" runat="server" Text="秒" />
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar runat="server" LabelAlign="Right">

                                    <Items>
                                        <ext:Label ID="LBkkmc" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;卡口名称：" />
                                        <ext:SelectBox ID="CBkkmc" runat="server" MinChars="1" Width="308"
                                            ValueField="col1" DisplayField="col2" EmptyText="">
                                            <Store>
                                                <ext:Store ID="Skkmc" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader>
                                                            <Fields>
                                                                <ext:RecordField Name="col1" />
                                                                <ext:RecordField Name="col2" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); }" />
                                            </Listeners>
                                        </ext:SelectBox>
                                        <ext:Label ID="LBxsfx" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;行驶方向：" Style="margin-left: 40px" />
                                        <ext:SelectBox ID="CBxsfx" runat="server" MinChars="1"
                                            ValueField="col0" DisplayField="col1" EmptyText="选择方向">
                                            <Store>
                                                <ext:Store ID="Sxsfx" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader>
                                                            <Fields>
                                                                <ext:RecordField Name="col0" />
                                                                <ext:RecordField Name="col1" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); }" />
                                            </Listeners>
                                        </ext:SelectBox>
                                        <ext:Label ID="LBcphm" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;号牌号码：" />
                                        <ext:Panel ID="Panel4" runat="server" Height="28" ColumnWidth=".15">
                                            <Content>
                                                <veh:VehicleHead ID="vehicleHead" runat="server" />
                                            </Content>
                                        </ext:Panel>
                                        <ext:TextField
                                            ID="TFcphm"
                                            runat="server"
                                            InputType="text"
                                            Width="110"  EmptyText="六位号牌号码"  />

                                        <ext:Button ID="BTSearch" Width="75" runat="server" Text="查询"  Icon="ControlPlayBlue"  OnDirectClick="BTSearch_DirectClick" Style="margin-left: 120px">
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <ext:Panel ID="Panel3" runat="server" Region="Center" Title="" AutoHeight="true">
                    <Items>
                        <ext:GridPanel ID="GPmbcl" Region="Center" runat="server" StripeRows="true" Title="查询车辆信息" Collapsible="true" AutoHeight="true">
                            <Store>
                                <ext:Store
                                    ID="Smbcl"
                                    runat="server"
                                    OnRefreshData="Smbcl_RefreshData"
                                    OnSubmitData="Smbcl_Submit">
                                    <Reader>
                                        <ext:JsonReader>
                                            <Fields>
                                                <ext:RecordField Name="kkmc" Type="String" />
                                                <ext:RecordField Name="cphm" Type="String" />
                                                <ext:RecordField Name="cplx" Type="String" />
                                                <ext:RecordField Name="gwsj" Type="Date" />
                                                <ext:RecordField Name="xsfx" Type="String" />
                                                <ext:RecordField Name="cd" Type="String" />
                                                <ext:RecordField Name="clsd" Type="String" />
                                                <ext:RecordField Name="sjly" Type="String" />
                                                <ext:RecordField Name="jllx" Type="String" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel runat="server" ID="ColumnModel2">
                                <Columns>
                                    <ext:Column Header="卡口名称" DataIndex="kkmc" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="号牌号码" DataIndex="cphm" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="号牌种类" DataIndex="cplx" Align="Center">
                                    </ext:Column>
                                    <ext:DateColumn Header="过往时间" DataIndex="gwsj" Format="yyyy-MM-dd HH:mm:ss" Align="Center">
                                    </ext:DateColumn>
                                    <ext:Column Header="行驶方向" DataIndex="xsfx" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="车道" DataIndex="cd" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="车辆速度" DataIndex="clsd" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="数据来源" DataIndex="sjly" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="记录类型" DataIndex="jllx" Align="Center">
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    <DirectEvents>
                                        <RowSelect OnEvent="SelectVehicleData" Buffer="250">
                                            <ExtraParams>
                                                <ext:Parameter Name="sdata" Value="record.data" Mode="Raw" />
                                            </ExtraParams>
                                        </RowSelect>
                                    </DirectEvents>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" />
                            <TopBar>
                                <ext:PagingToolbar HideRefresh="true" runat="server" PageSize="10" ID="PagingToolbar2">
                                    <Items>
                                        <ext:ToolbarFill runat="server" ID="ToolbarFill2" />
                                        <ext:Button runat="server" Text="To XML" Icon="PageCode" ID="Button1">
                                            <Listeners>
                                                <Click Handler="exportData('xml');" />
                                            </Listeners>
                                        </ext:Button>

                                        <ext:Button runat="server" Text="To Excel" Icon="PageExcel" ID="Button5" OnClick="ButExcelClick" />

                                        <ext:Button runat="server" Text="To CSV" Icon="PageAttach" ID="Button6">
                                            <Listeners>
                                                <Click Handler="exportData('csv');" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:PagingToolbar>
                            </TopBar>
                            <View>
                                <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                                </ext:GridView>
                            </View>
                        </ext:GridPanel>
                        <ext:GridPanel Region="Center" runat="server" StripeRows="true" Title="伴随车辆信息列表" Height="400">
                            <Store>
                                <ext:Store
                                    ID="Sbscl"
                                    runat="server"
                                    OnRefreshData="Smbcl_RefreshData"
                                    OnSubmitData="Smbcl_Submit">
                                    <Reader>
                                        <ext:JsonReader>
                                            <Fields>
                                                <ext:RecordField Name="kkmc" Type="String" />
                                                <ext:RecordField Name="cphm" Type="String" />
                                                <ext:RecordField Name="cplx" Type="String" />
                                                <ext:RecordField Name="gwsj" Type="Date" />
                                                <ext:RecordField Name="xsfx" Type="String" />
                                                <ext:RecordField Name="cd" Type="String" />
                                                <ext:RecordField Name="clsd" Type="String" />
                                                <ext:RecordField Name="sjly" Type="String" />
                                                <ext:RecordField Name="jllx" Type="String" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel runat="server" ID="ColumnModel1">
                                <Columns>
                                    <ext:Column Header="卡口名称" DataIndex="kkmc" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="号牌号码" DataIndex="cphm" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="号牌种类" DataIndex="cplx" Align="Center">
                                    </ext:Column>
                                    <ext:DateColumn Header="过往时间" DataIndex="gwsj" Format="yyyy-MM-dd HH:mm:ss" Align="Center">
                                    </ext:DateColumn>
                                    <ext:Column Header="行驶方向" DataIndex="xsfx" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="车道" DataIndex="cd" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="车辆速度" DataIndex="clsd" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="数据来源" DataIndex="sjly" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="记录类型" DataIndex="jllx" Align="Center">
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:CheckboxSelectionModel runat="server" />
                            </SelectionModel>
                            <LoadMask ShowMask="true" />
                            <TopBar>
                                <ext:PagingToolbar HideRefresh="true" runat="server" PageSize="10" ID="PagingToolbar1">
                                    <Items>
                                        <ext:ToolbarFill runat="server" ID="ToolbarFill1" />
                                        <ext:Button runat="server" Text="To XML" Icon="PageCode" ID="Button2">
                                            <Listeners>
                                                <Click Handler="exportData('xml');" />
                                            </Listeners>
                                        </ext:Button>

                                        <ext:Button runat="server" Text="To Excel" Icon="PageExcel" ID="Button3">
                                            <Listeners>
                                                <Click Handler="exportData('xls');" />
                                            </Listeners>
                                        </ext:Button>

                                        <ext:Button runat="server" Text="To CSV" Icon="PageAttach" ID="Button4">
                                            <Listeners>
                                                <Click Handler="exportData('csv');" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:PagingToolbar>
                            </TopBar>
                        </ext:GridPanel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

<script type="text/javascript">
    var exportData = function (format) {
        FormatType.setValue(format);
        var store = GPmbcl.getStore();
        store.directEventConfig.isUpload = true;

        var records = store.reader.readRecords(store.proxy.data).records,
            values = [];

        for (i = 0; i < records.length; i++) {
            var obj = {}, dataR;

            if (store.reader.meta.id) {
                obj[store.reader.meta.id] = records[i].id;
            }

            dataR = Ext.apply(obj, records[i].data);

            if (!Ext.isEmptyObj(dataR)) {
                values.push(dataR);
            }
        }

        store.submitData(values);

        store.directEventConfig.isUpload = false;
    };
</script>
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
            AnalyFollowVehicles.GetDateTime(true, tt);
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
            AnalyFollowVehicles.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>