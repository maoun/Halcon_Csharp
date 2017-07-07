<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemCode.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.SystemCode" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统代码维护</title>
    <script type="text/javascript">
        var saveData = function () {
            GridData.setValue(Ext.encode(GridPanel1.getRowsValues(false)));
        }

        var changetype = function (value) {
            if (value = '1') {
                return "是"
            }
            else {
                return "否"
            }
        };
    </script>
</head>
<body >
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ScriptManager1" runat="server" DirectMethodNamespace="SystemCode" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Viewport ID="ViewPort1" runat="server">
            <Items>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <North MarginsSummary="0 5 0 5">
                        <ext:Panel ID="Panel2" runat="server" Frame="true" Title="代码类型管理" Icon="ApplicationViewColumns" Height="300" Layout="Fit">
                            <Items>
                                <ext:GridPanel ID="GridPanel1" runat="server" StripeRows="true">
                                    <Store>
                                        <ext:Store ID="StoreCodeId" runat="server">
                                            <AutoLoadParams>
                                                <ext:Parameter Name="start" Value="={0}" />
                                                <ext:Parameter Name="limit" Value="={5}" />
                                            </AutoLoadParams>
                                            <Reader>
                                                <ext:JsonReader IDProperty="col0">
                                                    <Fields>
                                                        <ext:RecordField Name="col0" />
                                                        <ext:RecordField Name="col1" />
                                                        <ext:RecordField Name="col2" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                                            <ext:Column ColumnID="CodeId" Width="300" DataIndex="col0" Header="代码编号" Align="Center" />
                                            <ext:Column DataIndex="col1" Width="300" Header="代码名称" />
                                            <ext:Column DataIndex="col2" Width="300" Header="备注" />
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                            <DirectEvents>
                                                <RowSelect OnEvent="RowSelect" Buffer="250">
                                                    <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{Details}" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="CodeId" Value="this.getSelected().id" Mode="Raw" />
                                                    </ExtraParams>
                                                </RowSelect>
                                            </DirectEvents>
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                      <View>
                                                        <ext:GridView ID="GridView2" runat="server" ForceFit="true">
                                                        </ext:GridView>
                                                    </View>
                                    <BottomBar>
                                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="5" HideRefresh="true" StoreID="StoreCodeId">
                                        </ext:PagingToolbar>
                                    </BottomBar>
                                </ext:GridPanel>
                            </Items>
                        </ext:Panel>
                    </North>
                    <Center Split="true" MarginsSummary="0 5 5 5">
                        <ext:Panel ID="pnlSouth" runat="server" Title="代码表" Icon="Book" Layout="Fit">
                            <Items>
                                <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StripeRows="true">
                                    <Store>
                                        <ext:Store ID="StoreCode" runat="server">
                                            <AutoLoadParams>
                                                <ext:Parameter Name="start" Value="={0}" />
                                                <ext:Parameter Name="limit" Value="={10}" />
                                            </AutoLoadParams>
                                            <Reader>
                                                <ext:JsonReader IDProperty="col1">
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
                                    <Plugins>
                                        <ext:RowEditor ID="RowEditor1" runat="server" SaveText="更新" CancelText="退出">
                                            <DirectEvents>
                                                <AfterEdit OnEvent="UpdateData" />
                                            </DirectEvents>
                                        </ext:RowEditor>
                                    </Plugins>
                                    <View>
                                                        <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                                                        </ext:GridView>
                                                    </View>
                                    <ColumnModel ID="ColumnModel2" runat="server">
                                        <Columns>
                                            <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                                            <ext:Column DataIndex="col0" Width="200" Header="代码编号" Align="Center" />
                                            <ext:Column DataIndex="col1" Width="200" Header="代码值" Align="Center">
                                                <Editor>
                                                    <ext:TextField ID="TxtEd0" runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column ColumnID="CodeDesc" Width="250" DataIndex="col2" Header="代码描述">
                                                <Editor>
                                                    <ext:TextField ID="TxtEd1" runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col5" Width="200" Header="备注">
                                                <Editor>
                                                    <ext:TextField ID="TxtEd2" runat="server" AllowBlank="true" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:BooleanColumn DataIndex="col4" Width="60" Header="是否使用" TrueText="是" FalseText="否" Align="Center">

                                                <Editor>
                                                    <ext:Checkbox ID="ChkEd3" runat="server" />
                                                </Editor>
                                            </ext:BooleanColumn>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" SingleSelect="true" />
                                    </SelectionModel>
                                    <BottomBar>
                                        <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="10" StoreID="StoreCode" HideRefresh="true">
                                            <Items>
                                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                <ext:Button ID="ButAdd" runat="server" Text="增加" Icon="Add" ToolTip="增加">
                                                    <DirectEvents>
                                                        <Click OnEvent="AddColumn" Single="false">
                                                            <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="SystemCodeAdd" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButDelete" runat="server" Text="删除" Icon="Delete" ToolTip="删除">
                                                    <Listeners>
                                                        <Click Handler="SystemCode.DoConfirm()" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:PagingToolbar>
                                    </BottomBar>
                                </ext:GridPanel>
                            </Items>
                        </ext:Panel>
                    </Center>
                </ext:BorderLayout>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>