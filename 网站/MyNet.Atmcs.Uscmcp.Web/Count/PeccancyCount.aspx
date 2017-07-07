<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeccancyCount.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.PeccancyCount" %>

<%@ Register Assembly="netchartdir" Namespace="ChartDirector" TagPrefix="chart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>违法车辆信息统计</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="Css/chooser.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="../Common/Print/LodopFuncs.js" type="text/javascript" charset="UTF-8"></script>
    <script type="text/javascript">
        var getTasks = function (tree) {
            var msg = [], msgn = [],
           selNodes = tree.getChecked();
            msg.push("");
            msgn.push("")
            Ext.each(selNodes, function (node) {
                if (msg.length > 1) {
                    msg.push(",");
                    msgn.push(",");
                }
                msg.push(node.text);
                msgn.push(node.id);
            });
            msg.push("");
            msgn.push("");
            GridData.setValue(msgn.join(""));
            return msg.join("");
        };
        function ClearCheckState() {
            TreePanel1.clearChecked();
        }
    </script>
    <script type="text/javascript">
        var template = '<img src="../images/state/bfb.gif"  width="{0}" height="10"  /><span>{1}%</span>'
        var template2 = '<span>{0}%</span>';

        var change = function (value) {
            return String.format(template, value * 2, value);
        };
        var pctChange = function (value) {
            return String.format(template2, value);
        };
    </script>
    <script language="javascript" type="text/javascript">
        function Preview(title, ctime) {
            CreatePrintPage(title, ctime);
            LODOP.PREVIEW();
        };
        function PreviewChart(ctime) {
            CreatePrintChart(ctime);
            LODOP.PREVIEW();
        };
        function Setup() {
            CreatePrintPage();
            LODOP.PRINT_SETUP();
        };
        function DirPrint() {
            CreatePrintPage();
            LODOP.PRINT();
        };
        function Design() {
            CreatePrintPage();
            LODOP.PRINT_DESIGN();
        };
        function CreatePrintPage(title, ctime) {
            LODOP.PRINT_INIT("报表打印");
            LODOP.SET_PRINT_TEXT_STYLE(1, "宋体", 10, 1, 0, 0, 1);
            LODOP.SET_PRINT_STYLE("FontSize", 16);
            LODOP.SET_PRINT_STYLE("Bold", 1);
            LODOP.ADD_PRINT_TEXT(20, 180, 400, 39, title);
            LODOP.SET_PRINT_STYLE("FontSize", 12);
            LODOP.ADD_PRINT_TEXT(50, 200, 400, 39, ctime);
            var strBodyStyle = "<style>table { border: 1 solid #f0f0f0 }</style>";
            var strFormHtml = strBodyStyle + "<body>" + document.getElementById("GridCount").outerHTML + "</body>";
            LODOP.ADD_PRINT_HTM(70, 20, 730, 1000, strFormHtml);

        };
        function CreatePrintChart(ctime) {
            LODOP.PRINT_INIT("报表打印");
            LODOP.SET_PRINT_TEXT_STYLE(1, "宋体", 10, 1, 0, 0, 1);
            LODOP.SET_PRINT_STYLE("FontSize", 12);
            LODOP.SET_PRINT_STYLE("Bold", 1);
            LODOP.ADD_PRINT_TEXT(30, 200, 400, 39, ctime);
            LODOP.ADD_PRINT_HTM(70, 100, 730, 1000, document.getElementById("WebChartViewer1").outerHTML);

        };
    </script>
    <style type="text/css">
        /*east-panel .x-layout-collapsed-west*/
        .x-layout-collapsed {
            background: url('/Images/Banner/simple/collapsed-est.png') no-repeat center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PeccancyCount" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Viewport ID="Viewport2" runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="Panel1" Region="North" runat="server"
                    Height="30">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server">
                            <Items>

                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("PeccancyCount1","时间范围：") %>' StyleSpec="margin-left:10px;">
                                </ext:Label>
                                <ext:DateField ID="DateStartTime" runat="server" Vtype="daterange">
                                    <Listeners>
                                        <Render Handler="this.endDateField = '#{DateEndTime}'" />
                                    </Listeners>
                                </ext:DateField>
                                <ext:TimeField ID="TimeStart" runat="server" Increment="1" Width="61" />
                                <ext:Label ID="lblEndTime" runat="server" Html="<font >&nbsp;&nbsp;-&nbsp;&nbsp;</font>">
                                </ext:Label>
                                <ext:DateField ID="DateEndTime" runat="server" Vtype="daterange">
                                    <Listeners>
                                        <Render Handler="this.startDateField = '#{DateStartTime}'" />
                                    </Listeners>
                                </ext:DateField>
                                <ext:TimeField ID="TimeEnd" runat="server" Increment="1" Width="61" />
                                <ext:Label ID="lblStartTime" runat="server" Html='<%# GetLangStr("PeccancyCount2","统计类型：") %>' StyleSpec="margin-left:10px;">
                                </ext:Label>
                                <ext:ComboBox ID="CmbCountType" runat="server" Width="100">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                    <Items>
                                        <ext:ListItem Text='<%# GetLangStr("PeccancyCount3","号牌种类") %>' AutoDataBind="true" Value="hpzlms" />
                                        <ext:ListItem Text='<%# GetLangStr("PeccancyCount4","违法行为") %>' AutoDataBind="true" Value="wfxwms" />
                                        <ext:ListItem Text='<%# GetLangStr("PeccancyCount5","数据来源") %>' AutoDataBind="true" Value="sjlyms" />
                                        <ext:ListItem Text='<%# GetLangStr("PeccancyCount6","所属机构") %>' AutoDataBind="true" Value="cjjgms" />
                                        <ext:ListItem Text='<%# GetLangStr("PeccancyCount7","车辆所属") %>' AutoDataBind="true" Value="bdydbjms" />
                                        <ext:ListItem Text='<%# GetLangStr("PeccancyCount8","审核情况") %>' AutoDataBind="true" Value="shzs" />
                                        <ext:ListItem Text='<%# GetLangStr("PeccancyCount9","通知情况") %>' AutoDataBind="true" Value="tzzs" />
                                        <ext:ListItem Text='<%# GetLangStr("PeccancyCount10","校正情况") %>' AutoDataBind="true" Value="jzzs" />
                                        <ext:ListItem Text='<%# GetLangStr("PeccancyCount11","处罚情况") %>' AutoDataBind="true" Value="cfzs" />
                                        <ext:ListItem Text='<%# GetLangStr("PeccancyCount12","传输情况") %>' AutoDataBind="true" Value="cszs" />
                                    </Items>
                                </ext:ComboBox>
                                <ext:DropDownField ID="Field3" runat="server" Editable="false" Width="350" TriggerIcon="Combo">
                                    <Component>
                                        <ext:TreePanel ID="TreePanel1" runat="server" Title='<%# GetLangStr("PeccancyCount13","违法地点列表") %>' Icon="Accept" Height="300"
                                            Shadow="None" UseArrows="true" AutoScroll="true" Animate="true" EnableDD="true"
                                            ContainerScroll="true" RootVisible="false" StyleSpec="background-color: rgba(68,138,202,0.8);border-radius: 15px;">
                                            <Buttons>
                                                <ext:Button ID="Button1" runat="server" Text='<%#GetLangStr("PeccancyCount14","关闭") %>'>
                                                    <Listeners>
                                                        <Click Handler="#{Field3}.collapse();" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Buttons>
                                            <Listeners>
                                                <CheckChange Handler="this.dropDownField.setValue(getTasks(this), false);" />
                                            </Listeners>
                                        </ext:TreePanel>
                                    </Component>
                                    <Listeners>
                                        <Expand Handler="this.component.getRootNode().expand(true);" Single="true" Delay="10" />
                                    </Listeners>
                                </ext:DropDownField>
                                <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("PeccancyCount15","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" Timeout="60000">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("PeccancyCount16","重置") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Button ID="ButPie" runat="server" Icon="ChartPie" ToolTip='<%# GetLangStr("PeccancyCount17","显示图表") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButPie_Click">
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButPrint" runat="server" Icon="Printer" ToolTip='<%# GetLangStr("PeccancyCount18","打印统计报表") %>' Hidden="true">
                                    <DirectEvents>
                                        <Click OnEvent="ButPrint_Click">
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButPrintChart" runat="server" Icon="Report" ToolTip='<%# GetLangStr("PeccancyCount19","打印统计图表") %>' Hidden="true">
                                    <DirectEvents>
                                        <Click OnEvent="ButPrintChart_Click">
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <ext:Panel ID="Panel2" runat="server" Region="Center" DefaultBorder="false" AutoScroll="true">
                    <Items>
                        <ext:BorderLayout ID="BorderLayout1" runat="server">
                            <Center>
                                <ext:GridPanel ID="GridCount" runat="server" StripeRows="true" Title='<%# GetLangStr("PeccancyCount20","列表统计") %>' Collapsible="true"
                                    AutoHeight="true" AutoScroll="true">
                                    <Store>
                                        <ext:Store ID="StoreCount" runat="server" GroupField="col1">
                                            <Reader>
                                                <ext:JsonReader>
                                                    <Fields>
                                                        <ext:RecordField Name="col0" />
                                                        <ext:RecordField Name="col1" />
                                                        <ext:RecordField Name="col2" />
                                                        <ext:RecordField Name="col3" />
                                                        <ext:RecordField Name="col4" />
                                                        <ext:RecordField Name="col5" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <ext:Column Header='<%# GetLangStr("PeccancyCount21","监测地点") %>' AutoDataBind="true" DataIndex="col1" Width="180" />
                                            <ext:GroupingSummaryColumn DataIndex="col2" ColumnID="counttype" Header='<%# GetLangStr("PeccancyCount22","统计类型") %>' AutoDataBind="true" Width="130"
                                                Hideable="false" SummaryType="Count">
                                                <SummaryRenderer Handler="return ((value === 0 || value > 1) ? '(' + value +'项)' : '(1项)');" />
                                            </ext:GroupingSummaryColumn>
                                            <ext:GroupingSummaryColumn Width="80" Header='<%# GetLangStr("PeccancyCount23","统计数量") %>' AutoDataBind="true" DataIndex="col3" SummaryType="Sum">
                                                <Renderer Handler="return value" />
                                                <SummaryRenderer Handler="return '合计：' + value" />
                                            </ext:GroupingSummaryColumn>
                                            <ext:Column Header='<%# GetLangStr("PeccancyCount24","百分比") %>' AutoDataBind="true" DataIndex="col4" Width="80">
                                                <Renderer Fn="pctChange" />
                                            </ext:Column>
                                            <ext:Column Header='<%# GetLangStr("PeccancyCount25","图例") %>' AutoDataBind="true" DataIndex="col4" Width="200">
                                                <Renderer Fn="change" />
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                            <DirectEvents>
                                                <RowSelect OnEvent="ShowDetails" Buffer="250">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="data" Value="record.data" Mode="Raw" />
                                                    </ExtraParams>
                                                </RowSelect>
                                            </DirectEvents>
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <View>
                                        <ext:GroupingView ID="GroupingView1" runat="server" ForceFit="true" MarkDirty="false"
                                            ShowGroupName="false" EnableNoGroups="true" HideGroupedColumn="true" GroupByText='<%# GetLangStr("PeccancyCount26","用该列进行分组") %>'
                                            ShowGroupsText='<%# GetLangStr("PeccancyCount27","显示分组") %>' />
                                    </View>
                                    <Plugins>
                                        <ext:GroupingSummary ID="GroupingSummary1" runat="server">
                                        </ext:GroupingSummary>
                                    </Plugins>
                                    <LoadMask ShowMask="true" />
                                </ext:GridPanel>
                            </Center>
                            <East Collapsible="true" Split="true">
                                <ext:Panel ID="PanChart" runat="server" Width="450" Title='<%# GetLangStr("PeccancyCount28","图示统计") %>' Collapsed="true" CtCls="east-panel"
                                    AutoScroll="true">
                                    <Items>
                                        <ext:Panel ID="pnlData" runat="server" Header="false" AutoRender="true">
                                            <Content>
                                                <center>
                                                    <chart:WebChartViewer ID="WebChartViewer1" runat="server" />
                                                </center>
                                            </Content>
                                        </ext:Panel>
                                    </Items>
                                </ext:Panel>
                            </East>
                        </ext:BorderLayout>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
    <object id="LODOP" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
    </object>
</body>
</html>