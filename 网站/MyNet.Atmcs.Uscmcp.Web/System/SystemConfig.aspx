<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemConfig.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.SystemConfig" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>系统参数管理</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="systemConfig" />
        <ext:Store ID="StoreSystem" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <ext:Panel ID="Panel2" Region="Center" runat="server" Frame="true" Title='<%# GetLangStr("SystemConfig1","系统参数管理")%>' Icon="ApplicationSideList" DefaultAnchor="100%" Layout="FitLayout">
                    <Items>
                        <ext:GridPanel ID="GridPanel1" runat="server" StripeRows="true" AutoExpandColumn="col1">
                            <Store>
                                <ext:Store ID="StoreConfig" runat="server" OnRefreshData="StoreConfig_Refresh">
                                    <AutoLoadParams>
                                        <ext:Parameter Name="start" Value="={0}" />
                                        <ext:Parameter Name="limit" Value="={10}" />
                                    </AutoLoadParams>
                                    <UpdateProxy>
                                        <ext:HttpWriteProxy Method="GET" Url="sysConfig.aspx">
                                        </ext:HttpWriteProxy>
                                    </UpdateProxy>
                                    <Reader>
                                        <ext:JsonReader IDProperty="col1">
                                            <Fields>
                                                <ext:RecordField Name="col1" />
                                                <ext:RecordField Name="col2" />
                                                <ext:RecordField Name="col3" />
                                                <ext:RecordField Name="col4" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn ></ext:RowNumbererColumn>
                                    <ext:Column ColumnID="configid" Width="100" DataIndex="col1" Header='<%# GetLangStr("SystemConfig2","配置编号")%>' AutoDataBind="true" Align="Center" />
                                    <ext:Column DataIndex="col2" Width="100" Header='<%# GetLangStr("SystemConfig3","配置名称")%>' AutoDataBind="true" Align="Center" />
                                    <ext:Column DataIndex="col3" Width="300" Header='<%# GetLangStr("SystemConfig4","配置值")%>' AutoDataBind="true" Align="Center" />
                                    <ext:Column DataIndex="col4" Width="100" Header='<%# GetLangStr("SystemConfig5","备注")%>' AutoDataBind="true" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    <DirectEvents>
                                        <RowSelect OnEvent="RowSelect" Buffer="250">
                                            <ExtraParams>
                                                <ext:Parameter Name="configid" Value="this.getSelected().id" Mode="Raw" />
                                            </ExtraParams>
                                        </RowSelect>
                                    </DirectEvents>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <TopBar>
                                <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="10">
                                </ext:PagingToolbar>
                            </TopBar>
                            <LoadMask ShowMask="true" />
                            <View>
                                <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                                </ext:GridView>
                            </View>
                        </ext:GridPanel>
                    </Items>
                </ext:Panel>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Margins="0 5 5 5"
                    Frame="true" Title='<%# GetLangStr("SystemConfig6","详细信息")%>' Width="350" Icon="Table" DefaultAnchor="100%">
                    <Items>
                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("SystemConfig7","配置编号")%>'  ID="txtConfigId" AllowBlank="false" Disabled="true" Vtype="alphanum" VtypeText="只能输入数字和字母" />
                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("SystemConfig8","配置名称")%>' ID="txtConfigName" AllowBlank="false" />
                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("SystemConfig9","配置值")%>' ID="txtConfigValue" />
                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("SystemConfig10","备注")%>' ID="txtRemark" />
                    </Items>
                    <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <ext:Button ID="ButAdd" runat="server" Text='<%# GetLangStr("SystemConfig11","增加")%>' Icon="Add" ToolTip='<%# GetLangStr("SystemConfig15","增加")%>'>
                                    <Listeners>
                                        <Click Handler="systemConfig.InfoSave()" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButUpdate" runat="server" Text='<%# GetLangStr("SystemConfig12","保存")%>' Icon="TableSave">
                                    <Listeners>
                                        <Click Handler="systemConfig.UpdateData()" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButDelete" runat="server" Text='<%# GetLangStr("SystemConfig13","删除")%>' Icon="Delete">
                                    <Listeners>
                                        <Click Handler="systemConfig.DoConfirm()" />
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