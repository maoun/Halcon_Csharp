<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PassCarFlowCount.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.PassCarFlowCount" %>

<%@ Register Assembly="netchartdir" Namespace="ChartDirector" TagPrefix="chart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>流量信息统计</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Css/chooser.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="utf-8"></script>

    <%--卡口列表js--%>
    <script type="text/javascript" language="javascript">

        //清理选中卡口
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
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PassCarFlowCount" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Viewport ID="Viewport2" runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="Panel1" Region="North" runat="server"
                    Height="30">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Label ID="lblStartTime" runat="server" Text='<%# GetLangStr("PassCarFlowCount1","统计类型：") %>' StyleSpec="margin-left:10px;">
                                </ext:Label>
                                <ext:ComboBox ID="CmbCountType" runat="server" Width="100">

                                    <Items>
                                        <ext:ListItem Text='<%# GetLangStr("PassCarFlowCount2","按天统计") %>' Value="0" AutoDataBind="true" />
                                        <ext:ListItem Text='<%# GetLangStr("PassCarFlowCount3","按月统计") %>' Value="1" AutoDataBind="true" />
                                        <ext:ListItem Text='<%# GetLangStr("PassCarFlowCount4","按周统计") %>' Value="2" AutoDataBind="true" />
                                        <ext:ListItem Text='<%# GetLangStr("PassCarFlowCount5","按年统计") %>' Value="3" AutoDataBind="true" />
                                    </Items>
                                    <DirectEvents>
                                        <Select OnEvent="CmbCountType_Select" Buffer="250">
                                            <EventMask ShowMask="true" Target="CustomTarget" />
                                        </Select>
                                    </DirectEvents>
                                </ext:ComboBox>
                                <ext:Label ID="Label2" runat="server" Text='<%# GetLangStr("PassCarFlowCount6","统计时间：") %>'>
                                </ext:Label>
                                <ext:ComboBox ID="CmbYear" runat="server" Editable="false" DisplayField="col1" ValueField="col0"
                                    TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PassCarFlowCount7","选择年...") %>' SelectOnFocus="true"
                                    Width="70">

                                    <Store>
                                        <ext:Store ID="StoreYear" runat="server">
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
                                <ext:ComboBox ID="CmbWeek" runat="server" Editable="false" DisplayField="col1" ValueField="col0"
                                    TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PassCarFlowCount8","选择周...") %>' SelectOnFocus="true"
                                    Width="60" Hidden="true">

                                    <Store>
                                        <ext:Store ID="StoreWeek" runat="server">
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
                                <ext:ComboBox ID="CmbMonth" runat="server" Editable="false" DisplayField="col1" ValueField="col0"
                                    TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PassCarFlowCount9","选择月...") %>' SelectOnFocus="true"
                                    Width="60">

                                    <Store>
                                        <ext:Store ID="StoreMonth" runat="server">
                                            <Reader>
                                                <ext:JsonReader IDProperty="col0">
                                                    <Fields>
                                                        <ext:RecordField Name="col0" Type="String" />
                                                        <ext:RecordField Name="col1" Type="String" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>
                                <ext:ComboBox ID="CmbDay" runat="server" Editable="false" DisplayField="col1" ValueField="col0"
                                    TypeAhead="true" Mode="Local" ForceSelection="true" SelectOnFocus="true"
                                    Width="60">

                                    <Store>
                                        <ext:Store ID="StoreDay" runat="server" AutoLoad="true" OnRefreshData="StoreDayRefresh">
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
                                <ext:Label ID="Label4" runat="server" Text='<%# GetLangStr("PassCarFlowCount10","卡口名称") %>'>
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
                                                <ext:Button runat="server" Text='<%# GetLangStr("PassCarFlowCount11","清除") %>'>
                                                    <Listeners>
                                                        <Click Handler="clearSelect(TreeStation,FieldStation);" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Button runat="server" Text='<%# GetLangStr("PassCarFlowCount12","关闭") %>'>
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
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("PassCarFlowCount13","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" Timeout="60000">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("PassCarFlowCount14","重置") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <ext:Panel ID="Panel2" runat="server" Region="Center" DefaultBorder="false" AutoScroll="true" Layout="FitLayout">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <ext:TabStrip ID="TabStrip1" runat="server">
                                    <Items>
                                        <ext:TabStripItem ActionItemID="pnlData" runat="server" Title='<%# GetLangStr("PassCarFlowCount15","图示统计") %>' AutoDataBind="true" />
                                        <ext:TabStripItem ActionItemID="GridFlow" runat="server" Title='<%# GetLangStr("PassCarFlowCount16","列表统计") %>' AutoDataBind="true" />
                                    </Items>
                                </ext:TabStrip>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                <%--    <ext:Button ID="ButPrint" runat="server" Icon="Printer" ToolTip='<%# GetLangStr("PassCarFlowCount17","统计打印") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButPrintClick" />
                                    </DirectEvents>
                                </ext:Button>--%>
                                <ext:Button ID="ButExcel" runat="server" AutoPostBack="true" OnClick="ButExcelClick" ToolTip='<%# GetLangStr("PassCarFlowCount21","导出Excel") %>'
                                    Icon="PageExcel">
                                </ext:Button>

                                <%--   <ext:Button ID="ButChart" runat="server" Icon="Report" ToolTip='<%# GetLangStr("PassCarFlowCount18","图表打印") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButChartClick" />
                                    </DirectEvents>
                                </ext:Button>--%>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridFlow" runat="server" StripeRows="true" Title='<%# GetLangStr("PassCarFlowCount19","列表统计") %>' Layout="FitLayout">
                            <Store>
                                <ext:Store ID="StoreFlow" runat="server">
                                    <Reader>
                                        <ext:JsonReader>
                                            <Fields>
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                        </ext:GridPanel>
                        <ext:Panel ID="pnlData" runat="server" Title='<%# GetLangStr("PassCarFlowCount20","图示统计") %>' AutoRender="true" AutoScroll="true">
                            <Content>
                                <center>
                                    <chart:WebChartViewer ID="WebChartViewer1" runat="server" />
                                </center>
                            </Content>
                        </ext:Panel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>