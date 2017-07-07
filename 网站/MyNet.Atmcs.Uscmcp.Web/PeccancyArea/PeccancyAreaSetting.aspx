<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeccancyAreaSetting.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.Peccancy.PeccancyAreaSetting" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%# GetLangStr("PeccancyAreaSetting1","区间超速配置") %></title>
    <link href="Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <script type="text/javascript">
        var change = function (value) {
            return value + "Km/h";
        };
        var changeJuli = function (value) {
            return value + "Km";
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PeccancyAreaSetting" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Store ID="StoreStartStation" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreEndStation" runat="server" AutoLoad="false" OnRefreshData="TgsRefresh">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
            <Listeners>
                <Load Handler="#{CmdJskkid}.setValue(#{CmdJskkid}.store.getAt(0).get('col0'));" />
            </Listeners>
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
                <ext:FormPanel ID="Panel1" Region="North" runat="server" Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server">
                            <Items>
                                <%--    Html="<font color='white'>&nbsp;&nbsp;开始卡口：</font>"   Html="<font color='white'>&nbsp;&nbsp;结束卡口：</font>"--%>
                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("PeccancyAreaSetting2","开始卡口：") %>'>
                                </ext:Label>
                                <ext:ComboBox runat="server" ID="CmdKskkid" StoreID="StoreStartStation" Editable="false"
                                    DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All" EmptyText='<%# GetLangStr("PeccancyAreaSetting3","选择开始卡口") %>'
                                    Width="270" AllowBlank="false">
                                    <Listeners>
                                        <Select Handler="#{CmdJskkid}.clearValue(); #{StoreEndStation}.reload();" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("PeccancyAreaSetting4","结束卡口：") %>'>
                                </ext:Label>
                                <ext:ComboBox runat="server" ID="CmdJskkid" StoreID="StoreEndStation" Editable="false"
                                    DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All" EmptyText='<%# GetLangStr("PeccancyAreaSetting5","选择结束卡口") %>'
                                    Width="270" AllowBlank="false" />
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("PeccancyAreaSetting6","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" Timeout="10000">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("PeccancyAreaSetting7","重置") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <ext:GridPanel ID="GridPecAreasetting" Region="Center" runat="server" StripeRows="true"
                    Title='<%# GetLangStr("PeccancyAreaSetting8","区间配置") %>'>
                    <TopBar>
                        <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StorePecAreasetting" HideRefresh="true">
                            <Items>
                            </Items>
                        </ext:PagingToolbar>
                    </TopBar>
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
                                        <ext:RecordField Name="col14" Type="String" />
                                        <ext:RecordField Name="col15" Type="String" />
                                        <ext:RecordField Name="col16" Type="String" />
                                        <ext:RecordField Name="col17" Type="String" />
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn Width="40" Align="Center"></ext:RowNumbererColumn>
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting9","区间编号") %>' DataIndex="col17" Hidden="true" AutoDataBind="true" />
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting10","开始卡口") %>' DataIndex="col2" Width="220" Align="Center" AutoDataBind="true" />
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting11","结束卡口") %>' DataIndex="col4" Width="220" Align="Center" AutoDataBind="true" />
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting12","区间距离") %>' DataIndex="col5" Width="100" Align="Center" AutoDataBind="true">
                                <Renderer Fn="changeJuli" />
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting13","小车限速") %>' DataIndex="col7" Width="100" Align="Center" AutoDataBind="true">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting14","大车限速") %>' DataIndex="col9" Width="100" Align="Center" AutoDataBind="true">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting15","小车限低速") %>' DataIndex="col6" Width="120" Align="Center" AutoDataBind="true">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting16","大车限低速") %>' DataIndex="col8" Width="120" Align="Center" AutoDataBind="true">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting17","行驶方向") %>' DataIndex="col14" Width="100" Align="Center" AutoDataBind="true" />
                            <ext:Column Header='<%# GetLangStr("PeccancyAreaSetting18","是否违法") %>' DataIndex="col16" Width="100" Align="Center" AutoDataBind="true" />
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
                    <View>
                        <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                        </ext:GridView>
                    </View>
                </ext:GridPanel>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Frame="true"
                    Title="详细信息" Width="360" Icon="Table" DefaultAnchor="100%" MonitorValid="true">
                    <Items>
                        <ext:Panel ID="Panel9" runat="server" DefaultAnchor="100%" Title='<%# GetLangStr("PeccancyAreaSetting19","区间配置") %>' Padding="5"
                            Header="true" AutoScroll="true">
                            <Items>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting20","记录编号") %>' ID="TxtID" Width="340" Disabled="true" Hidden="true" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting21","区间编号") %>' ID="TxtAreaID" Width="340" Hidden="true" />
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting22","开始卡口") %>' ID="CmbStartStation" StoreID="StoreStartStationDict"
                                    Editable="false" DisplayField="col2" ValueField="col1" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("PeccancyAreaSetting23","选择开始卡口") %>' Width="340" AllowBlank="false">
                                    <Listeners>
                                        <Select Handler="#{CmbEndStation}.clearValue(); #{StoreEndStationDict}.reload();" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting24","结束卡口") %>' ID="CmbEndStation" StoreID="StoreEndStationDict"
                                    Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("PeccancyAreaSetting25","选择结束卡口") %>' Width="340" AllowBlank="false" />
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting26","行驶方向") %>' ID="ComDirection" StoreID="StoreDirection"
                                    Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("PeccancyAreaSetting27","选择方向") %>' Width="340" AllowBlank="false" />
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting28","是否违法") %>' ID="ComIsPeccancy" StoreID="StoreShow"
                                    Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("PeccancyAreaSetting29","选择是否") %>' Width="340" AllowBlank="false" />
                                <ext:Panel runat="server" Layout="ColumnLayout">
                                    <Items>
                                        <ext:NumberField runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting30","区间距离") %>' ID="TxtAreaLength" Width="300"
                                            MinValue="0" MaxValue="1000" AllowBlank="false" ColumnWidth=".85" />
                                        <ext:Label runat="server" ColumnWidth=".02"></ext:Label>
                                        <ext:Label runat="server" Text="Km" ColumnWidth=".1" StyleSpec="margin-top:5px;font-size:15px;"></ext:Label>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel runat="server" Layout="ColumnLayout" StyleSpec="margin-top:5px;">
                                    <Items>
                                        <ext:NumberField runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting31","小车限速") %>' ID="TxtSS" Width="300" MinValue="0"
                                            MaxValue="200" AllowBlank="false" ColumnWidth=".85" />
                                        <ext:Label runat="server" ColumnWidth=".02"></ext:Label>
                                        <ext:Label runat="server" Text="Km/h" ColumnWidth=".1" StyleSpec="margin-top:5px;font-size:15px;"></ext:Label>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel runat="server" Layout="ColumnLayout" StyleSpec="margin-top:5px;">
                                    <Items>
                                        <ext:NumberField runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting32","大车限速") %>' ID="TxtBS" Width="300" MinValue="0"
                                            MaxValue="200" AllowBlank="false" ColumnWidth=".85" />
                                        <ext:Label runat="server" ColumnWidth=".02"></ext:Label>
                                        <ext:Label runat="server" Text="Km/h" ColumnWidth=".1" StyleSpec="margin-top:5px;font-size:15px;"></ext:Label>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel runat="server" Layout="ColumnLayout" StyleSpec="margin-top:5px;">
                                    <Items>
                                        <ext:NumberField runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting33","小车限低速") %>' ID="TxtSLS" Width="300" MinValue="0"
                                            MaxValue="200" AllowBlank="false" ColumnWidth=".85" />
                                        <ext:Label runat="server" ColumnWidth=".02"></ext:Label>
                                        <ext:Label runat="server" Text="Km/h" ColumnWidth=".1" StyleSpec="margin-top:5px;font-size:15px;"></ext:Label>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel runat="server" Layout="ColumnLayout" StyleSpec="margin-top:5px;">
                                    <Items>
                                        <ext:NumberField runat="server" FieldLabel='<%# GetLangStr("PeccancyAreaSetting34","大车限低速") %>' ID="TxtBLS" Width="300" MinValue="0"
                                            MaxValue="200" AllowBlank="false" ColumnWidth=".85" />
                                        <ext:Label runat="server" ColumnWidth=".02"></ext:Label>
                                        <ext:Label runat="server" Text="Km/h" ColumnWidth=".1" StyleSpec="margin-top:5px;font-size:15px;"></ext:Label>
                                    </Items>
                                </ext:Panel>
                            </Items>
                        </ext:Panel>
                    </Items>
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>

                                <ext:Button ID="ButAdd" runat="server" Text='<%# GetLangStr("PeccancyAreaSetting35","新增") %>' Icon="TableAdd">
                                    <Listeners>
                                        <Click Handler="PeccancyAreaSetting.InsertPecAreasetting()" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButUpdate" runat="server" Text='<%# GetLangStr("PeccancyAreaSetting36","保存") %>' Icon="TableSave">
                                    <DirectEvents>
                                        <Click OnEvent="UpdatePecAreasetting">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButDelete" runat="server" Text='<%# GetLangStr("PeccancyAreaSetting37","删除") %>' Icon="TableDelete">
                                    <DirectEvents>
                                        <Click OnEvent="DeletePecAreasetting">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
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