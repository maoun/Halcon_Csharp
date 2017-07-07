<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PassCarInfoCount.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.PassCarInfoCount" %>

<%@ Register Assembly="netchartdir" Namespace="ChartDirector" TagPrefix="chart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>过往车辆信息统计</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Css/chooser.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #TreePanel1 {
            background-color: rgba(68,138,202,0.9);
            border-radius: 14px;
        }
    </style>
    <script language="javascript" src="../Common/Print/LodopFuncs.js" type="text/javascript" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>

    <script type="text/javascript">
        var template = '<img src="../images/state/bfb.gif"  width="{0}" height="10"  />'
        var template2 = '<span>{0}</span>';

        var change = function (value) {
            return String.format(template, value * 2);
        };
        var pctChange = function (value) {
            return String.format(template2, value + "%");
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
    <script type="text/javascript">
        var filterTree = function (el, e) {
            var tree = TreePanel1,
                text = this.getRawValue();

            tree.clearFilter();

            if (Ext.isEmpty(text, false)) {
                return;
            }

            if (e.getKey() === Ext.EventObject.ESC) {
                clearFilter();
            } else {
                var re = new RegExp(".*" + text + ".*", "i");

                tree.filterBy(function (node) {
                    return re.test(node.text);
                });
            }
        };

        var clearFilter = function () {
            var field = TriggerField1,
                tree = TreePanel1;
            field.setValue("");
            tree.clearFilter();
            tree.getRootNode().collapseChildNodes(true);
            tree.getRootNode().ensureVisible();
            //tree.expandAll();
        };
    </script>
    <%--卡口列表js--%>
    <script type="text/javascript" language="javascript">
        //清除时间
        function clearTime(start, end) {
            document.getElementById("start").innerText = start;
            document.getElementById("end").innerText = end;

        }
        function directclear() {
            clearSelect(TreeStation, FieldStation);
        }
        //清理选中
        var clearSelect = function (tree, field) {
            var ids = field.getValue();
            if (ids.length > 0) {
                try {
                    //设置 取消勾选
                    tree.setChecked({ ids: ids, silent: false });
                } catch (e) {
                }
            }
            //tree.getRootNode().collapseChildNodes(true);
        };
        // 获得选中value
        var getValues = function (tree) {
            var msg = [],
                selNodes = tree.getChecked();
            Ext.each(selNodes, function (node) {
                msg.push(node.id);
            });
            return msg.join(",");
        };
        // 获得选中text
        var getText = function (tree) {
            var msg = [],
                selNodes = tree.getChecked();
            Ext.each(selNodes, function (node) {
                msg.push(node.text);
            });
            return msg.join(",");
        };

        var syncValue = function (value) {
            var tree = this.component;
            if (tree.rendered) {
                if (value) {
                    var ids = value.split(",");
                    try {
                        tree.setChecked({ ids: ids, silent: true });
                        tree.getSelectionModel().clearSelections();
                        Ext.each(ids, function (id) {
                            var node = tree.getNodeById(id);
                            if (node) {
                                node.ensureVisible(function () {
                                    tree.getSelectionModel().select(tree.getNodeById(this.id), null, true);
                                }, node);
                            }
                        }, this);
                    } catch (e) { }
                }
            }
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PassCarInfoCount" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Viewport ID="Viewport2" runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="Panel1" Region="North" runat="server">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server">
                            <Items>
                                <ext:Label ID="lblStartTime" runat="server" Text='<%# GetLangStr("PassCarInfoCount1","统计类型：") %>'>
                                </ext:Label>
                                <ext:ComboBox ID="CmbCountType" runat="server" Width="100">

                                    <Items>
                                        <ext:ListItem Text='<%# GetLangStr("PassCarInfoCount2","号牌种类") %>' Value="hpzlms" AutoDataBind="true" />
                                        <ext:ListItem Text='<%# GetLangStr("PassCarInfoCount3","记录类型") %>' Value="jllxms" AutoDataBind="true" />
                                        <ext:ListItem Text='<%# GetLangStr("PassCarInfoCount4","所属机构") %>' Value="cjjgms" AutoDataBind="true" />
                                        <ext:ListItem Text='<%# GetLangStr("PassCarInfoCount5","数据来源") %>' Value="sjlyms" AutoDataBind="true" />
                                        <ext:ListItem Text='<%# GetLangStr("PassCarInfoCount6","车牌省份") %>' Value="bdydbjms" AutoDataBind="true" />
                                        <%--   <ext:ListItem Text='<%# GetLangStr("PassCarInfoCount7","发证机关") %>' Value="bdydbjms"  AutoDataBind="true" />--%>
                                    </Items>
                                </ext:ComboBox>
                                <ext:Panel runat="server" BodyBorder="false">
                                    <Content>
                                        <div runat="server" id="selectDate" style="width: 475px">
                                            <span style="float: left; margin-left: 0px; height: 24px; line-height: 24px!important; text-align: center">&nbsp;&nbsp;<%# GetLangStr("PassCarInfoCount8","查询时间") %></span><li runat="server" class="laydate-icon" id="start" style="width: 150px; margin-left: 16px; float: left; list-style: none; cursor: pointer; height: 22px; line-height: 22px!important"></li>
                                        </div>
                                        <div>
                                            <span style="float: left; margin-left: 20px; height: 24px; line-height: 24px!important; text-align: center">--</span><li runat="server" class="laydate-icon" id="end" style="width: 150px; margin-left: 16px; float: left; list-style: none; cursor: pointer; height: 22px; line-height: 22px!important"></li>
                                        </div>
                                    </Content>
                                </ext:Panel>
                                <ext:Label ID="Label4" runat="server" Text='<%# GetLangStr("PassCarInfoCount9","卡口名称：") %>' StyleSpec="true">
                                </ext:Label>
                                <ext:DropDownField ID="FieldStation" runat="server"
                                    Editable="false" Width="400px" TriggerIcon="SimpleArrowDown" Mode="ValueText">
                                    <Component>
                                        <ext:TreePanel runat="server" Height="400" Shadow="None" ID="TreeStation"
                                            UseArrows="true" AutoScroll="true" Animate="true" EnableDD="true" ContainerScroll="true" RootVisible="true"
                                            StyleSpec="background-color: rgba(68,138,202,0.9); border-radius: 20px;">
                                            <Root>
                                            </Root>
                                            <Buttons>
                                                <ext:Button runat="server" Text='<%# GetLangStr("PassCarInfoCount10","清除") %>'>
                                                    <Listeners>
                                                        <Click Handler="clearSelect(TreeStation,FieldStation);" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Button runat="server" Text='<%# GetLangStr("PassCarInfoCount11","关闭") %>'>
                                                    <Listeners>
                                                        <Click Handler="#{FieldStation}.collapse();" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Buttons>
                                            <Listeners>
                                                <CheckChange Handler="this.dropDownField.setValue(getValues(this), getText(this), false);" />
                                            </Listeners>
                                            <SelectionModel>
                                                <ext:MultiSelectionModel runat="server" />
                                            </SelectionModel>
                                        </ext:TreePanel>
                                    </Component>
                                    <Listeners>
                                        <Expand Handler="this.component.getRootNode().expand(false);" Single="true" Delay="20" />
                                    </Listeners>
                                    <SyncValue Fn="syncValue" />
                                </ext:DropDownField>
                                <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("PassCarInfoCount12","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" Timeout="60000">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("PassCarInfoCount13","重置") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Button ID="ButPie" runat="server" Icon="ChartPie" ToolTip='<%# GetLangStr("PassCarInfoCount14","显示图表") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButPie_Click">
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButPrint" runat="server" Icon="Printer" ToolTip='<%# GetLangStr("PassCarInfoCount15","打印统计报表") %>' Hidden="true">
                                    <DirectEvents>
                                        <Click OnEvent="ButPrint_Click">
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButPrintChart" runat="server" Icon="Report" ToolTip='<%# GetLangStr("PassCarInfoCount16","打印统计图表") %>' Hidden="true">
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
                                <ext:Panel ID="Panel3" runat="server" DefaultBorder="false" Title='<%# GetLangStr("PassCarInfoCount17","列表统计") %>' AutoScroll="true">
                                    <Items>
                                        <ext:GridPanel ID="GridCount" runat="server" StripeRows="true" Header="false" Collapsible="true"
                                            AutoHeight="true">
                                            <Store>
                                                <ext:Store ID="StoreCount" runat="server" GroupField="col2">
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
                                                    <ext:Column Header='<%# GetLangStr("PassCarInfoCount18","卡口名称") %>' AutoDataBind="true" DataIndex="col0" Width="130" />
                                                    <ext:Column Header='<%# GetLangStr("PassCarInfoCount19","行驶方向") %>' AutoDataBind="true" DataIndex="col2" Width="180" />
                                                    <ext:GroupingSummaryColumn DataIndex="col3" ColumnID="counttype" Header='<%# GetLangStr("PassCarInfoCount20","统计类型") %>' AutoDataBind="true" Width="130"
                                                        Hideable="false" SummaryType="Count">
                                                        <SummaryRenderer Handler="return ((value === 0 || value > 1) ? '(' + value +'项)' : '(1项)');" />
                                                    </ext:GroupingSummaryColumn>
                                                    <ext:GroupingSummaryColumn Width="80" Header='<%# GetLangStr("PassCarInfoCount21","统计数量") %>' AutoDataBind="true" DataIndex="col4" SummaryType="Sum">
                                                        <Renderer Handler="return value" />
                                                        <SummaryRenderer Handler="return '合计：' + value" />
                                                    </ext:GroupingSummaryColumn>
                                                    <ext:Column Header='<%# GetLangStr("PassCarInfoCount22","百分比") %>' AutoDataBind="true" DataIndex="col5" Width="80">
                                                        <Renderer Fn="pctChange" />
                                                    </ext:Column>
                                                    <ext:Column Header='<%# GetLangStr("PassCarInfoCount23","图例") %>' AutoDataBind="true" DataIndex="col5" Width="200">
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
                                                    ShowGroupName="false" EnableNoGroups="true" HideGroupedColumn="true" GroupByText='<%# GetLangStr("PassCarInfoCount24","用该列进行分组") %>'
                                                    ShowGroupsText='<%# GetLangStr("PassCarInfoCount25","显示分组") %>' />
                                            </View>
                                            <Plugins>
                                                <ext:GroupingSummary ID="GroupingSummary1" runat="server">
                                                </ext:GroupingSummary>
                                            </Plugins>
                                            <LoadMask ShowMask="true" />
                                        </ext:GridPanel>
                                    </Items>
                                </ext:Panel>
                            </Center>
                        </ext:BorderLayout>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
        <ext:Window ID="winShow" runat="server" Width="600px" Height="400px" Hidden="true">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Header="false" AutoRender="true">
                    <Content>
                        <center>
                            <chart:WebChartViewer ID="WebChartViewer1" runat="server" />
                        </center>
                    </Content>
                </ext:Panel>
            </Items>
        </ext:Window>
    </form>
</body>
<object id="LODOP" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0"
    height="0">
    <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
</object>
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
            PassCarInfoCount.GetDateTime(true, tt);
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
            PassCarInfoCount.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>
</html>