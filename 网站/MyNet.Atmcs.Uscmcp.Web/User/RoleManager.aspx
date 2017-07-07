<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleManager.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.RoleManager" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户角色管理</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <script type="text/javascript">
        var getTasks = function (tree) {
            var msg = [],
         selNodes = tree.getChecked();
            msg.push("");
            Ext.each(selNodes, function (node) {
                if (msg.length > 1) {
                    msg.push(",");
                }
                msg.push("");
                msg.push(node.id);
                msg.push("");

            });
            msg.push("");
            return msg.join("");
        };
    </script>
    <script type="text/javascript">
        function ClearCheckState() {
            TreePanel1.clearChecked();
            getCheckValue();
        }
        function SetCheckState(id) {
            var selNodes = TreePanel1.getRootNode();
            findchildnode(selNodes, id);
            getCheckValue();
        }
        function findchildnode(node, id) {
            var childnodes = node.childNodes;
            var nd;
            for (var i = 0; i < childnodes.length; i++) { //从节点中取出子节点依次遍历
                nd = childnodes[i];
                if (nd.id == id) {
                    nd.getUI().toggleCheck(true);
                }
                if (nd.hasChildNodes()) { //判断子节点下是否存在子节点
                    findchildnode(nd, id); //如果存在子节点 递归
                }
            }
        }
        function getCheckValue() {
            var selNodes = TreePanel1.getChecked();
            var nd;
            var nodevalue = "";
            for (var i = 0; i < selNodes.length; i++) { //从节点中取出子节点依次遍历
                nd = selNodes[i];
                nodevalue += nd.id + ",";
            }
            nodevalue = nodevalue.substr(0, nodevalue.length - 1);
            GridData.setValue(nodevalue);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="RoleManager" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Hidden ID="CurrentRoleId" runat="server" />
        <ext:Hidden ID="hidNowUsername" runat="server" />
        <ext:Store ID="StoreDepart" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="Panel1" Region="North" runat="server"
                    Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("RoleManager1","角色编号：")%>' StyleSpec="margin-left: 10px;">
                                </ext:Label>
                                <ext:TextField ID="TxtRoleId" runat="server" Width="100" />
                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("RoleManager2","角色名称：")%>' StyleSpec="margin-left: 10px;">
                                </ext:Label>
                                <ext:TextField ID="TxtRoleName" runat="server" Width="100" />
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("RoleManager3","查询")%>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("RoleManager4","重置")%>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarFill />
                                <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
                                <ext:Button ID="ButRoleAdd" runat="server" Text='<%# GetLangStr("RoleManager5","新增角色")%>' Icon="BasketAdd">
                                    <DirectEvents>
                                        <Click OnEvent="ButRoleAdd_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButRoleDelete" runat="server" Text='<%# GetLangStr("RoleManager6","删除角色")%>' Icon="BasketDelete">
                                    <DirectEvents>
                                        <Click OnEvent="ButRoleDelete_Click" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <ext:GridPanel ID="GridRole" Region="Center" runat="server" StripeRows="true"
                    Height="400">
                    <Store>
                        <ext:Store ID="StoreRole" runat="server" OnRefreshData="MyData_Refresh">
                            <Reader>
                                <ext:JsonReader IDProperty="col1">
                                    <Fields>
                                        <ext:RecordField Name="col0" Type="String" />
                                        <ext:RecordField Name="col1" Type="String" />
                                        <ext:RecordField Name="col2" Type="String" />
                                        <ext:RecordField Name="col3" Type="String" />
                                        <ext:RecordField Name="col4" Type="String" />
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                        </ext:Store>
                    </Store>
                    <Plugins>
                        <ext:RowEditor ID="RowEditor2" runat="server" SaveText='<%# GetLangStr("RoleManager7","更新")%>' CancelText='<%# GetLangStr("RoleManager8","退出")%>'>
                            <DirectEvents>
                                <AfterEdit OnEvent="UpdateData" />
                            </DirectEvents>
                        </ext:RowEditor>
                    </Plugins>
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn Width="40" />
                            <ext:Column Header='<%# GetLangStr("RoleManager9","角色编号")%>' AutoDataBind="true" DataIndex="col1" Width="100" Align="Center" />
                            <ext:Column Header='<%# GetLangStr("RoleManager10","角色名称" )%>' AutoDataBind="true" DataIndex="col2" Width="120" Align="Center">
                                <Editor>
                                    <ext:TextField ID="TxtERoleName" runat="server" AllowBlank="false" />
                                </Editor>
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("RoleManager11","角色描述")%>' AutoDataBind="true" DataIndex="col3" Width="150" Align="Center">
                                <Editor>
                                    <ext:TextField ID="TxtERoleDesc" runat="server" />
                                </Editor>
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("RoleManager12","备注")%>' AutoDataBind="true" DataIndex="col4" Width="150" Align="Center">
                                <Editor>
                                    <ext:TextField ID="TxtERoleRemark" runat="server" />
                                </Editor>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                            <DirectEvents>
                                <RowSelect OnEvent="SelectRole" Buffer="250">
                                    <ExtraParams>
                                        <ext:Parameter Name="sdata" Value="record.data" Mode="Raw" />
                                    </ExtraParams>
                                </RowSelect>
                            </DirectEvents>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <TopBar>
                        <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StoreRole">
                        </ext:PagingToolbar>
                    </TopBar>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                        </ext:GridView>
                    </View>
                </ext:GridPanel>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Frame="true"
                    Title='<%# GetLangStr("RoleManager13","详细信息")%>' Width="250" Icon="User" DefaultAnchor="100%">
                    <Items>
                        <ext:TreePanel ID="TreePanel1" runat="server" Title='<%# GetLangStr("RoleManager14","权限列表")%>' Icon="Accept" Shadow="None"
                            UseArrows="true" Animate="true" EnableDD="true" ContainerScroll="true" RootVisible="false"
                            Collapsed="false">
                            <Listeners>
                                <CheckChange Handler="#{GridData}.setValue(getTasks(this), false);" />
                            </Listeners>
                        </ext:TreePanel>
                    </Items>
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button ID="ButUpdate" runat="server" Text='<%# GetLangStr("RoleManager15","保存")%>' Icon="TableSave">
                                    <Listeners>
                                        <Click Handler="RoleManager.UpdateRoleFunc()" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>