<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PassCarOcrCount.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.PassCarOcrCount" %>

<%@ Register Assembly="netchartdir" Namespace="ChartDirector" TagPrefix="chart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>识别率统计</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="Css/chooser.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        function popMsg(msg) {
            PassCarOcrCount.AddWindow(msg);
        }
    </script>
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
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PassCarOcrCount" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Viewport ID="Viewport2" runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="Panel1" Region="North" runat="server"
                    Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Label ID="Label2" runat="server" Text='<%# GetLangStr("PassCarOcrCount1","统计时间：") %>' StyleSpec="margin-left:10px;">
                                </ext:Label>
                                <ext:ComboBox ID="CmbYear" runat="server" Editable="false" DisplayField="col1" ValueField="col0"
                                    TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PassCarOcrCount2","选择年...") %>' SelectOnFocus="true"
                                    Width="80">

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
                                <ext:ComboBox ID="CmbMonth" runat="server" Editable="false" DisplayField="col1" ValueField="col0"
                                    TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PassCarOcrCount3","选择月...") %>' SelectOnFocus="true"
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
                                <ext:Label ID="Label4" runat="server" Text='<%# GetLangStr("PassCarOcrCount4","卡口名称：") %>' StyleSpec="true">
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
                                                <ext:Button runat="server" Text='<%# GetLangStr("PassCarOcrCount5","清除") %>'>
                                                    <Listeners>
                                                        <Click Handler="clearSelect(TreeStation,FieldStation);" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Button runat="server" Text='<%# GetLangStr("PassCarOcrCount6","关闭") %>'>
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
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("PassCarOcrCount7","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" Timeout="60000">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("PassCarOcrCount8","重置") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <ext:Panel ID="Panel2" runat="server" Region="Center" DefaultBorder="false" AutoScroll="true">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <ext:TabStrip ID="TabStrip1" runat="server">
                                    <Items>
                                        <ext:TabStripItem ActionItemID="pnlData" runat="server" Title='<%# GetLangStr("PassCarOcrCount9","图示统计") %>' AutoDataBind="true" />
                                        <ext:TabStripItem ActionItemID="GridFlow" runat="server" Title='<%# GetLangStr("PassCarOcrCount10","列表统计") %>' AutoDataBind="true" />
                                    </Items>
                                </ext:TabStrip>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                <%--   <ext:Button ID="ButPrint" runat="server" Icon="Printer" ToolTip='<%# GetLangStr("PassCarOcrCount11","统计打印") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButPrintClick" />
                                    </DirectEvents>
                                </ext:Button>--%>
                                <ext:Button ID="ButExcel" runat="server" Icon="PageExcel" ToolTip='<%# GetLangStr("PassCarOcrCount12","导出Excel") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButExcelClick" IsUpload="true" />
                                        <%-- 加这个属性isUpload="true" 就能导出Excel了 --%>
                                    </DirectEvents>
                                </ext:Button>
                                <%-- <ext:Button ID="ButChart" runat="server" Icon="Report" ToolTip='<%# GetLangStr("PassCarOcrCount13","图表打印") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButChartClick" />
                                    </DirectEvents>
                                </ext:Button>--%>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridFlow" runat="server" StripeRows="true" Title='<%# GetLangStr("PassCarOcrCount14","列表统计") %>' Collapsible="true"
                            AutoHeight="true" AutoScroll="true">
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
                        <ext:Panel ID="pnlData" runat="server" Title='<%# GetLangStr("PassCarOcrCount15","图示统计") %>' AutoRender="true" AutoScroll="true">
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