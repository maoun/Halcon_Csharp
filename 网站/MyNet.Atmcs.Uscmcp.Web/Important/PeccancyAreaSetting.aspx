<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeccancyAreaSetting.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.PeccancyAreaSetting" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>区间超速配置</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Css/chooser.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PeccancyAreaSetting" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Hidden ID="EndStationId" runat="server" />
        <ext:Store ID="StoreStartStation" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col10" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreEndStation" runat="server" AutoLoad="false" OnRefreshData="TgsRefresh">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col2" />
                        <ext:RecordField Name="col11" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreStartStationDict" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreEndStationDict" runat="server" AutoLoad="false" OnRefreshData="DictRefresh">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreDirection" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreShow" runat="server">
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
                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("PeccancyAreaSetting1","开始卡口：") %>'>
                                </ext:Label>
                                <ext:ComboBox runat="server" ID="CmdKskkid" StoreID="StoreStartStation" Editable="false"
                                    DisplayField="col10" ValueField="col1" Mode="Local" TriggerAction="All" EmptyText='<%# GetLangStr("PeccancyAreaSetting2","选择开始卡口")%>'
                                    Width="270" AllowBlank="false">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();#{CmdJskkid}.clearValue(); #{StoreEndStation}.reload();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("PeccancyAreaSetting3","结束卡口：") %>'>
                                </ext:Label>
                                <ext:ComboBox runat="server" ID="CmdJskkid" StoreID="StoreEndStation" Editable="false"
                                    DisplayField="col11" ValueField="col2" Mode="Local" TriggerAction="All" EmptyText='<%# GetLangStr("PeccancyAreaSetting4","选择结束卡口") %>'
                                    Width="270" AllowBlank="false">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("PeccancyAreaSetting5","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" Timeout="60000">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("PeccancyAreaSetting6","重置") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <ext:GridPanel ID="GridPecAreasetting" Region="Center" runat="server" StripeRows="true"
                    Title='<%# GetLangStr("PeccancyAreaSetting7","区间配置") %>'>
                    <Store>
                        <ext:Store ID="StorePecAreasetting" runat="server" OnRefreshData="MyData_Refresh">
                            <AutoLoadParams>
                                <ext:Parameter Name="start" Value="={0}" />
                                <ext:Parameter Name="limit" Value="={15}" />
                            </AutoLoadParams>
                            <UpdateProxy>
                                <ext:HttpWriteProxy Method="GET" Url="PeccancyAreaSetting.aspx">
                                </ext:HttpWriteProxy>
                            </UpdateProxy>
                            <Reader>
                                <ext:JsonReader IDProperty="col0">
                                    <Fields>
                                        <ext:RecordField Name="col0" Type="String" />
                                        <ext:RecordField Name="col1" Type="String" />
                                        <ext:RecordField Name="col2" Type="String" />
                                        <ext:RecordField Name="col3" Type="String" />
                                        <ext:RecordField Name="col4" Type="String" />
                                        <ext:RecordField Name="col5" Type="String" />
                                        <ext:RecordField Name="col6" Type="String" />
                                        <ext:RecordField Name="col7" Type="String" />
                                        <ext:RecordField Name="col8" Type="String" />
                                        <ext:RecordField Name="col9" Type="String" />
                                        <ext:RecordField Name="col10" Type="String" />
                                        <ext:RecordField Name="col11" Type="String" />
                                        <ext:RecordField Name="col12" Type="String" />
                                        <ext:RecordField Name="col13" Type="String" />
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting8","区间编号") %>' AutoDataBind="true" DataIndex="col0" Width="150" />
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting9","开始卡口") %>' AutoDataBind="true" DataIndex="col10" Width="150" />
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting10","结束卡口") %>' AutoDataBind="true" DataIndex="col11" Width="150" />
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting11","区间距离") %>' AutoDataBind="true" DataIndex="col3" Width="80" />
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting12","小车限速") %>' AutoDataBind="true" DataIndex="col4" Width="80" />
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting13","大车限速") %>' AutoDataBind="true" DataIndex="col5" Width="80" />
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting14","小车限低速") %>' AutoDataBind="true" DataIndex="col6" Width="80" />
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting15","大车限低速") %>' AutoDataBind="true" DataIndex="col7" Width="80" />
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting16","行驶方向") %>' AutoDataBind="true" DataIndex="col12" Width="90" />
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting17","是否违法") %>' AutoDataBind="true" DataIndex="col13" Width="90" />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                            <DirectEvents>
                                <RowSelect OnEvent="SelectPecAreasetting" Buffer="250">
                                    <ExtraParams>
                                        <ext:Parameter Name="sdata" Value="record.data" Mode="Raw" />
                                    </ExtraParams>
                                </RowSelect>
                            </DirectEvents>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <TopBar>
                        <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StorePecAreasetting">
                            <Items>
                                <ext:Label ID="Label2" runat="server" Text='<%# GetLangStr("PeccancyAreaSetting18","页大小") %>' StyleSpec="margin-left:10px;" />
                                <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="10" />
                                <ext:ComboBox ID="CmbPaging" runat="server" Width="80">
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
                    </TopBar>
                    <View>
                        <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                        </ext:GridView>
                    </View>
                </ext:GridPanel>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Frame="true"
                    Title='<%# GetLangStr("PeccancyAreaSetting19","详细信息") %>' Width="360" Icon="Table" DefaultAnchor="100%" MonitorValid="true">
                    <Items>
                        <ext:Panel ID="Panel9" runat="server" DefaultAnchor="100%" Title='<%# GetLangStr("PeccancyAreaSetting7","区间配置") %>' Padding="5"
                            Header="true" AutoScroll="true">
                            <Items>
                                <%--<ext:TextField runat="server" FieldLabel="记录编号" ID="TxtID" Width="340" Disabled="true" />--%>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting8","区间编号") %>' ID="TxtAreaID" Width="340" />
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting9","开始卡口") %>' ID="CmbStartStation" StoreID="StoreStartStationDict"
                                    Editable="false" DisplayField="col2" ValueField="col1" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("PeccancyAreaSetting2","选择开始卡口") %>' Width="340" AllowBlank="false">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();#{CmbEndStation}.clearValue(); #{StoreEndStationDict}.reload();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting10","结束卡口") %>' ID="CmbEndStation" StoreID="StoreEndStationDict"
                                    Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("PeccancyAreaSetting4","选择结束卡口") %>' Width="340" AllowBlank="false">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting16","行驶方向") %>' ID="ComDirection" StoreID="StoreDirection"
                                    Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("PeccancyAreaSetting27","选择方向") %>' Width="340" AllowBlank="false">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting17","是否违法") %>' ID="ComIsPeccancy" StoreID="StoreShow"
                                    Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("PeccancyAreaSetting29","选择是否") %>' Width="340" AllowBlank="false">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:NumberField runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting11","区间距离") %>' ID="TxtAreaLength" Width="340"
                                    MinValue="0" MaxValue="1000" AllowBlank="false" />
                                <ext:NumberField runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting12","小车限速") %>' ID="TxtSS" Width="340" MinValue="0"
                                    MaxValue="200" AllowBlank="false" />
                                <ext:NumberField runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting13","大车限速") %>' ID="TxtBS" Width="340" MinValue="0"
                                    MaxValue="200" AllowBlank="false" />
                                <ext:NumberField runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting14","小车限低速") %>' ID="TxtSLS" Width="340" MinValue="0"
                                    MaxValue="200" AllowBlank="false" />
                                <ext:NumberField runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting15","大车限低速") %>' ID="TxtBLS" Width="340" MinValue="0"
                                    MaxValue="200" AllowBlank="false" />
                            </Items>
                        </ext:Panel>
                    </Items>
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button ID="ButUpdate" runat="server" Text='<%# GetLangStr("PeccancyAreaSetting35","保存") %>' Icon="TableSave">
                                    <Listeners>
                                        <Click Handler="PeccancyAreaSetting.UpdatePecAreasetting()" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButAdd" runat="server" Text='<%# GetLangStr("PeccancyAreaSetting36","新增") %>' Icon="TableAdd">
                                    <Listeners>
                                        <Click Handler="PeccancyAreaSetting.InsertPecAreasetting()" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButDelete" runat="server" Text='<%# GetLangStr("PeccancyAreaSetting37","删除") %>' Icon="TableDelete">
                                    <Listeners>
                                        <Click Handler="PeccancyAreaSetting.DeletePecAreasetting()" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Listeners>
                        <ClientValidation Handler="ButUpdate.setDisabled(!valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>