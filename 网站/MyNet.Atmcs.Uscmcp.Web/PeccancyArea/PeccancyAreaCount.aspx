<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeccancyAreaCount.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.PeccancyAreaCount" %>

<%@ Register Assembly="netchartdir" Namespace="ChartDirector" TagPrefix="chart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>区间违法统计</title>
    <meta name="GENERATOR" content="MSHTML 8.00.7600.16853" />
    <link href="../Styles/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="Stylesheet" href="../Styles/datetime.css" type="text/css" />
    <link rel="stylesheet" href="../Styles/showphotostyle.css" type="text/css" />
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="gb2312"></script>
    <script src="../Scripts/showphoto.js" language="JavaScript" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>
    <script type="text/javascript">
        function ChangeBackColor(id, hpzl) {

            var obj = document.getElementById(id);
            switch (hpzl) {

                case "01":
                    obj.style.color = "#000000";
                    obj.style.background = "FFFF00";
                    break;
                case "02":
                    obj.style.color = "#FFFFFF";
                    obj.style.background = "000080";
                    break;
                case "06":
                    obj.style.color = "#FFFFFF";
                    obj.style.background = "000000";
                    break;
                case "23":
                    obj.style.color = "FF0000";
                    obj.style.background = "FFFFFF";
                    break;
                default:
                    obj.style.color = "#FFFFFF";
                    obj.style.background = "000080";
                    break;

            }
        }
        function clearTime(start, end) {
            document.getElementById("start").innerText = start;
            document.getElementById("end").innerText = end;
            CmbEndStation.triggers[0].hide();
            CmbStartStation.triggers[0].hide();
        }
        function ClearCheckState() {
            TreePanel1.clearChecked();
        }
        var showTip = function () {
            var rowIndex = GridPecArea.view.findRowIndex(this.triggerElement),
                cellIndex = GridPecArea.view.findCellIndex(this.triggerElement),
                record = StorePecArea.getAt(rowIndex),
                fieldName = GridPecArea.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);
            this.body.dom.innerHTML = data;
        };
    </script>
    <script type="text/javascript">
        var IMGDIR = '../images/sets';
        var attackevasive = '0';
        var gid = 0;
        var fid = parseInt('0');
        var tid = parseInt('0');
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridPecArea.view.findRowIndex(this.triggerElement),
                cellIndex = GridPecArea.view.findCellIndex(this.triggerElement),
                record = StorePecArea.getAt(rowIndex),
                fieldName = GridPecArea.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
    <style type="text/css">
        .images-view .x-panel-body {
            background: white;
            font: 11px Arial, Helvetica, sans-serif;
        }

        .images-view .thumb {
            background: #dddddd;
            padding: 3px;
        }

            .images-view .thumb img {
                width: 480px;
            }

        .images-view .thumb-wrap {
            float: left;
            margin: 4px;
            margin-right: 0;
            padding: 5px;
            text-align: center;
        }

            .images-view .thumb-wrap span {
                display: block;
                overflow: hidden;
                text-align: center;
            }

        .images-view .x-view-over {
            border: 1px solid #dddddd;
            background: #efefef url(../images/row-over.gif) repeat-x left top;
            padding: 4px;
        }

        .images-view .x-view-selected {
            background: #eff5fb url(../images/selected.gif) no-repeat right bottom;
            border: 1px solid #99bbe8;
            padding: 4px;
        }

            .images-view .x-view-selected .thumb {
                background: transparent;
            }

        .images-view .loading-indicator {
            font-size: 11px;
            background-image: url(../images/loading.gif);
            background-repeat: no-repeat;
            background-position: left;
            padding-left: 20px;
            margin: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="append_parent" />
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PeccancyAreaCount" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Hidden ID="Hidden1" runat="server" />
        <ext:Hidden ID="realCount" runat="server" />
        <ext:Hidden ID="curpage" runat="server" />
        <ext:Hidden ID="allPage" runat="server" />

        <ext:Viewport ID="Viewport2" runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="Panel1" Region="North" runat="server"
                    Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server" Layout="Container">
                            <Items>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:Panel runat="server" BodyBorder="false" ID="panelTime">
                                            <Content>
                                                <div runat="server" id="selectDate" style="width: 444px">
                                                    <span class="laydate-span" style="height: 24px;">&nbsp;&nbsp;查询时间：</span>
                                                    <li runat="server" class="laydate-icon" id="start" style="width: 150px; height: 22px;"></li>
                                                </div>
                                                <div>
                                                    <span class="laydate-span" style="height: 24px;">--</span>
                                                    <li runat="server" class="laydate-icon" id="end" style="width: 150px; height: 22px;"></li>
                                                </div>
                                            </Content>
                                        </ext:Panel>
                                        <ext:Label ID="Label1" runat="server" Hidden="true" Html="<font >&nbsp;&nbsp;号牌种类：</font>">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbPlateType" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText="请选择..."
                                            SelectOnFocus="true" Width="120" Hidden="true">
                                            <Store>
                                                <ext:Store ID="StorePlateType" runat="server">
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
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:Label ID="Label4" runat="server" Html="<font >&nbsp;&nbsp;起点卡口：</font>">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbStartStation" runat="server" Editable="false" DisplayField="col1"
                                            Width="200" ListWidth="360" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                            EmptyText="选择起点卡口..." SelectOnFocus="true">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();#{CmbEndStation}.clearValue(); #{StoreEndStation}.reload();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                            <Store>
                                                <ext:Store ID="StoreStartStation" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader IDProperty="col1">
                                                            <Fields>
                                                                <ext:RecordField Name="col0" Type="String" />
                                                                <ext:RecordField Name="col1" Type="String" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                        <ext:Label ID="Label5" runat="server" Html="<font >&nbsp;&nbsp;终点卡口：</font>">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbEndStation" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText="选择终点卡口..."
                                            SelectOnFocus="true" Width="200" ListWidth="360">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                            <Store>
                                                <ext:Store ID="StoreEndStation" runat="server" AutoLoad="false" OnRefreshData="TgsRefresh">
                                                    <Reader>
                                                        <ext:JsonReader>
                                                            <Fields>
                                                                <ext:RecordField Name="col0" Type="String" />
                                                                <ext:RecordField Name="col1" Type="String" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                    <Listeners>
                                                        <Load Handler="#{CmbEndStation}.setValue(#{CmbEndStation}.store.getAt(0).get('col0'));" />
                                                    </Listeners>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                        <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text="查询">
                                            <DirectEvents>
                                                <Click OnEvent="TbutQueryClick" Timeout="60000">
                                                    <EventMask ShowMask="true" />
                                                </Click>
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
                </ext:FormPanel>
                <ext:Panel ID="Panel3" runat="server" Region="Center" DefaultBorder="false" Layout="FitLayout">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <ext:TabStrip ID="TabStrip1" runat="server">
                                    <Items>
                                        <ext:TabStripItem ActionItemID="pnlWfxwData" runat="server" Title="违法行为图示" />
                                        <ext:TabStripItem ActionItemID="pnlXssdData" runat="server" Title="行驶速度图示" />
                                        <ext:TabStripItem ActionItemID="GridPecArea" runat="server" Title="列表统计" />
                                    </Items>
                                </ext:TabStrip>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>

                    <Items>
                        <ext:GridPanel ID="GridPecArea" runat="server" StripeRows="true"
                            Header="false" TrackMouseOver="true">
                            <TopBar>
                                <ext:Toolbar runat="server" Layout="Container">
                                    <Items>
                                        <ext:Toolbar runat="server">
                                            <Items>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10" />
                                                <ext:Button ID="ButFisrt" runat="server" Text="首页" Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutFisrt" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButLast" runat="server" Style="margin-left: 10px;" Icon="ControlRewindBlue" Text="上一页" Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutLast" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButNext" runat="server" Style="margin-left: 10px;" Icon="ControlFastforwardBlue" Text="下一页"
                                                    Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutNext" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButEnd" runat="server" Style="margin-left: 10px;" Text="尾页" Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutEnd" />
                                                    </DirectEvents>
                                                </ext:Button>

                                                <ext:Label ID="lblTitle" runat="server" Text="查询结果：当前是第" StyleSpec=" margin-left:10px;" />
                                                <ext:Label ID="lblCurpage" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label2" runat="server" Text="页,共有" />
                                                <ext:Label ID="lblAllpage" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label9" runat="server" Text="页,共有" />
                                                <ext:Label ID="lblRealcount" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label12" runat="server" Text="条记录" />

                                                <%--   <ext:Label ID="titleEnd" runat="server"  StyleSpec=" margin-left:10px;" />--%>
                                            </Items>
                                        </ext:Toolbar>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <%--  <TopBar>
                                <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StorePecArea" HideRefresh="true">
                                    <Items>
                                        <ext:Label ID="Label2" runat="server" Text="页大小" />
                                        <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="10" />
                                        <ext:ComboBox ID="CmbPaging" runat="server" Width="60">
                                            <Items>
                                                <ext:ListItem Text="10" />
                                                <ext:ListItem Text="15" />
                                                <ext:ListItem Text="20" />
                                                <ext:ListItem Text="30" />
                                                <ext:ListItem Text="50" />
                                            </Items>
                                            <SelectedItem Value="15" />
                                            <Listeners>
                                                <Select Handler="#{PagingToolbar1}.pageSize = parseInt(this.getValue()); #{PagingToolbar1}.doLoad();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:PagingToolbar>
                            </TopBar>--%>
                            <Store>
                                <ext:Store ID="StorePecArea" runat="server" IgnoreExtraFields="false" OnRefreshData="MyData_Refresh">
                                    <AutoLoadParams>
                                        <ext:Parameter Name="start" Value="={0}" />
                                        <ext:Parameter Name="limit" Value="={15}" />
                                    </AutoLoadParams>
                                    <UpdateProxy>
                                        <ext:HttpWriteProxy Method="GET" Url="PeccancyArea.aspx">
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
                                                <ext:RecordField Name="col12" />
                                                <ext:RecordField Name="col13" />
                                                <ext:RecordField Name="col14" />
                                                <ext:RecordField Name="col15" />
                                                <ext:RecordField Name="col16" />
                                                <ext:RecordField Name="col17" />
                                                <ext:RecordField Name="col18" />
                                                <ext:RecordField Name="col19" />
                                                <ext:RecordField Name="col20" />
                                                <ext:RecordField Name="col21" />
                                                <ext:RecordField Name="col22" />
                                                <ext:RecordField Name="col23" />
                                                <ext:RecordField Name="col24" />
                                                <ext:RecordField Name="col25" />
                                                <ext:RecordField Name="col26" />
                                                <ext:RecordField Name="col27" />
                                                <ext:RecordField Name="col28" />
                                                <ext:RecordField Name="col29" />
                                                <ext:RecordField Name="col30" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40" />
                                    <ext:Column Header="号牌种类" DataIndex="col1" Align="Center">
                                    </ext:Column>
                                    <ext:DateColumn Header="违法时间" DataIndex="col2" Align="Center" Format="yyyy-MM-dd HH:mm:ss">
                                    </ext:DateColumn>
                                    <ext:Column Header="开始卡口" DataIndex="col4">
                                    </ext:Column>
                                    <ext:Column Header="结束卡口" DataIndex="col6">
                                    </ext:Column>
                                    <ext:Column Header="违法行为" ColumnID="wfxw" DataIndex="col8">
                                    </ext:Column>
                                    <ext:Column Header="所属机构" DataIndex="col10">
                                    </ext:Column>
                                    <ext:Column Header="统计总数" DataIndex="col11" Align="Center">
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel runat="server"></ext:RowSelectionModel>
                            </SelectionModel>
                            <View>
                                <ext:GridView runat="server" ForceFit="true"></ext:GridView>
                            </View>
                            <ToolTips>
                                <ext:ToolTip
                                    ID="ToolTip1"
                                    runat="server"
                                    Target="={GridPecArea.getView().mainBody}"
                                    Delegate=".x-grid3-cell"
                                    TrackMouse="true">
                                    <Listeners>
                                        <Show Fn="showTip" />
                                    </Listeners>
                                </ext:ToolTip>
                            </ToolTips>
                        </ext:GridPanel>

                        <ext:ToolTip
                            ID="RowTip"
                            runat="server"
                            Target="={#{GridPecArea}.getView().mainBody}"
                            Delegate=".x-grid3-cell"
                            TrackMouse="true">
                            <Listeners>
                                <Show Fn="showTip" />
                            </Listeners>
                        </ext:ToolTip>
                        <ext:Panel ID="pnlWfxwData" runat="server" Title="违法行为图示" AutoScroll="true">
                            <Content>

                                <center>
                                    <chart:WebChartViewer ID="WebChartViewer1" runat="server" />
                                </center>
                            </Content>
                        </ext:Panel>

                        <ext:Panel ID="pnlXssdData" runat="server" Title="行驶速度图示" AutoScroll="true">
                            <Content>
                                <center>
                                    <chart:WebChartViewer ID="WebChartViewer2" runat="server" />
                                </center>
                            </Content>
                        </ext:Panel>
                    </Items>
                </ext:Panel>
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
            end.start = datas //将结束日的初始值设定为开始日
            $("#end").click();//开始时间选中后，自动弹出结束时间
            var tt = $("#start").html();
            PeccancyAreaCount.GetDateTime(true, tt);
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
            PeccancyAreaCount.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>
</html>