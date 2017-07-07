<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TgsFlowAmply.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.TgsFlowAmply" %>

<%@ Register Assembly="netchartdir" Namespace="ChartDirector" TagPrefix="chart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>流量信息统计</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="FlowShow" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Viewport ID="Viewport2" runat="server" Layout="border">
            <Items>
                <ext:Panel ID="Panel2" runat="server" Region="Center" DefaultBorder="false" AutoScroll="true">
                    <Items>
                        <ext:GridPanel ID="GridFlow" runat="server" StripeRows="true" Title="列表统计" Collapsible="true"
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
                        <ext:Panel ID="pnlData" runat="server" Title="图示统计" AutoRender="true" AutoScroll="true">
                            <Content>
                                <center>
                                    <chart:WebChartViewer ID="WebChartViewer1"  runat="server" />
                                </center>
                            </Content>
                            <TopBar>
                                <ext:Toolbar ID="Toolbar2" runat="server">
                                    <Items>
                                        <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text="显示图表">
                                            <DirectEvents>
                                                <Click OnEvent="TbutQueryClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:Panel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
