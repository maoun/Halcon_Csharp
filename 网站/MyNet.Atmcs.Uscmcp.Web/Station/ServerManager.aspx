<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServerManager.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.ServerManager" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>服务器信息管理</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <script language="javascript" src="Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" language="javascript">

        var employeeDetailsRender = function () {
            return '<img class="imgEdit" ext:qtip="点击查看详细信息" style="cursor:pointer;" src="../Images/vcard_edit.png"  />';
        };
        var getRowClass = function (record, rowIndex, p, ds) {
            var reColor = "";
            if (record.data.col5 == 0) {

                reColor = "x-grid-row-summary";
            }

            return reColor;
        };
        function clearTime() {

            CmbDeviceType.triggers[0].hide();
            CmbSBXH.triggers[0].hide();

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:Hidden ID="HidSaveFlag" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="HiddenId" runat="server">
        </ext:Hidden>
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="ServerManager" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Store ID="StoreSBXH" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                        <ext:RecordField Name="col3" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreLocation" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreJSDW" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreWHDW" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreZZCS" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
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
        <ext:Store ID="StoreDeviceType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreTXLX" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreComType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreSY" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreTXXY" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="Panel1" Region="Center" runat="server"
                    Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("ServerManager1","服务器名称：") %>' StyleSpec="margin-left:10px;">
                                </ext:Label>
                                <ext:TextField ID="TxtDeviceName" runat="server" Width="100" />
                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("ServerManager2","服务器类型：") %>' StyleSpec="margin-left:10px;">
                                </ext:Label>
                                <ext:ComboBox ID="CmbDeviceType" runat="server" Editable="false" StoreID="StoreDeviceType"
                                    DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                    EmptyText='<%# GetLangStr("ServerManager3","选择服务器类型...") %>' SelectOnFocus="true" Width="173">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ServerManager9","清除选中") %>' AutoDataBind="true"/>
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();ServerManager.SelectDevice();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Label ID="Label4" runat="server" Text='<%# GetLangStr("ServerManager4","服务器型号：") %>' StyleSpec="margin-left:10px;">
                                </ext:Label>
                                <ext:ComboBox ID="CmbSBXH" runat="server" Editable="false" StoreID="StoreSBXH" DisplayField="col3"
                                    ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("ServerManager5","选择服务器型号...") %>'
                                    SelectOnFocus="true" Width="123">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ServerManager9","清除选中") %>' AutoDataBind="true" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("ServerManager6","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("ServerManager7","重置") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButRefresh" runat="server" Icon="DriveGo"   Hidden="true" Text='<%# GetLangStr("ServerManager8","刷新") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButRefreshClick" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridDeviceManager" runat="server" StripeRows="true"
                            Height="600">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar2" runat="server" Layout="Container">
                                    <Items>
                                        <ext:Toolbar ID="Toolbar5" runat="server">
                                            <Items>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="20" />
                                                <ext:Button ID="ButDevAdd" runat="server" Text='<%# GetLangStr("ServerManager10","添加新服务器") %>' Icon="DriveAdd">
                                                    <DirectEvents>
                                                        <Click OnEvent="ButDevAdd_Click" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButDevModify" runat="server" Text='<%# GetLangStr("ServerManager11","修改信息") %>' Icon="DriveEdit">
                                                    <Listeners>
                                                        <Click Handler="ServerManager.Modify();" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Button ID="ButDevDelete" runat="server" Text='<%# GetLangStr("ServerManager12","删除服务器") %>' Icon="DriveDelete">
                                                    <Listeners>
                                                        <Click Handler="ServerManager.DoConfirm()" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                        <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StoreDeviceManager">
                                            <Items>

                                                <ext:Button ID="ButExcel" runat="server" Text='<%# GetLangStr("ServerManager14","导出Excel") %>' AutoPostBack="true" OnClick="ToExcel"
                                                    Icon="PageExcel">
                                                </ext:Button>

                                                <%--   <ext:Button ID="ButPrint" runat="server" Icon="Printer" Text='<%# GetLangStr("ServerManager15","打印") %>'>
                                                    <DirectEvents>
                                                        <Click OnEvent="ButPrintClick" />
                                                    </DirectEvents>
                                                </ext:Button>--%>
                                            </Items>
                                        </ext:PagingToolbar>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Store>
                                <ext:Store ID="StoreDeviceManager" runat="server" OnRefreshData="MyData_Refresh">
                                    <AutoLoadParams>
                                        <ext:Parameter Name="start" Value="={0}" />
                                        <ext:Parameter Name="limit" Value="={15}" />
                                    </AutoLoadParams>
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
                                                <ext:RecordField Name="col18" Type="String" />
                                                <ext:RecordField Name="col19" Type="String" />
                                                <ext:RecordField Name="col20" Type="String" />
                                                <ext:RecordField Name="col21" Type="String" />
                                                <ext:RecordField Name="col22" Type="String" />
                                                <ext:RecordField Name="col23" Type="String" />
                                                <ext:RecordField Name="col24" Type="String" />
                                                <ext:RecordField Name="col25" Type="String" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40" />
                                    <ext:Column Header='<%# GetLangStr("ServerManager16","服务器编号") %>' AutoDataBind="true" DataIndex="col0" Width="100" Align="Center" Hidden="true" />
                                    <ext:Column Header='<%# GetLangStr("ServerManager17","服务器名称") %> ' AutoDataBind="true" DataIndex="col1" Width="240" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("ServerManager18","服务器类型") %>' AutoDataBind="true" DataIndex="col3" Width="150" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("ServerManager19","服务器型号") %>' AutoDataBind="true" DataIndex="col5" Width="120" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("ServerManager20","服务器IP") %>' AutoDataBind="true" DataIndex="col6" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("ServerManager21","服务器端口") %>' AutoDataBind="true" DataIndex="col7" Width="80" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <Listeners>
                                <RowDblClick Handler="ServerManager.RowDblClickShow();" />
                            </Listeners>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <View>
                                <ext:GridView ID="GroupingView1" runat="server" ForceFit="true">
                                    <GetRowClass Fn="getRowClass" />
                                </ext:GridView>
                            </View>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="West" Split="true" Title='<%# GetLangStr("ServerManager22","服务器信息列表") %>'
                    Width="250" Icon="Table">
                    <Items>
                        <ext:TreePanel ID="TreePanel1" runat="server" Icon="Drive" Shadow="None" UseArrows="true"
                            AutoScroll="true" Animate="true" ContainerScroll="true" EnableDD="true" RootVisible="true"
                            Header="false" Height="800">
                        </ext:TreePanel>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
        <ext:Window ID="Window4" runat="server" Icon="House" Hidden="true" Height="330px"
            Width="590px" MonitorValid="true" Title='<%# GetLangStr("ServerManager23","服务器信息维护") %>'>
            <Items>
                <ext:FormPanel runat="server" ID="FormPanel3" MonitorValid="true">
                    <Items>
                        <ext:TabPanel ID="TabPanel1" runat="server" ActiveTabIndex="0" Width="590" Height="250">
                            <Items>
                                <ext:Panel ID="Tab1" runat="server" Title='<%# GetLangStr("ServerManager24","服务器信息") %>' Padding="6" AutoScroll="true">
                                    <Items>
                                        <ext:Container ID="Container4" runat="server" Layout="Column" Height="200">
                                            <Items>
                                                <ext:Container ID="Container5" runat="server" LabelAlign="Left" Layout="Form" ColumnWidth=".5">
                                                    <Items>
                                                        <ext:TextField ID="Text_SBBH" runat="server" FieldLabel='<%# GetLangStr("ServerManager25","服务器编号") %>' AnchorHorizontal="95%"
                                                            AllowBlank="false" ReadOnly="true" Disabled="true">
                                                        </ext:TextField>
                                                        <ext:ComboBox ID="Cob_SBLX" runat="server" FieldLabel='<%# GetLangStr("ServerManager26","服务器类型") %>' AnchorHorizontal="95%"
                                                            AllowBlank="false" Editable="false" StoreID="StoreDeviceType" DisplayField="col1"
                                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("ServerManager27","选择服务器类型...") %>'
                                                            SelectOnFocus="true">
                                                            <Triggers>
                                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ServerManager9","清除选中") %>' AutoDataBind="true" />
                                                            </Triggers>
                                                            <Listeners>
                                                                <Select Handler="this.triggers[0].show();ServerManager.SelectDevice();" />
                                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                        <ext:TextField ID="Text_SBMC" runat="server" FieldLabel='<%# GetLangStr("ServerManager28","服务器名称") %>' EmptyText='<%# GetLangStr("ServerManager29","例如：区间检测服务1") %>'
                                                            AnchorHorizontal="95%" AllowBlank="false">
                                                        </ext:TextField>
                                                        <ext:ComboBox ID="Cob_SBXH" runat="server" FieldLabel='<%# GetLangStr("ServerManager30","服务器型号") %>' AnchorHorizontal="95%"
                                                            AllowBlank="false" Editable="false" StoreID="StoreSBXH" DisplayField="col3" ValueField="col0"
                                                            TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("ServerManager31","选择服务器型号...") %>' SelectOnFocus="true">
                                                            <Triggers>
                                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ServerManager9","清除选中") %>' AutoDataBind="true" />
                                                            </Triggers>
                                                            <Listeners>
                                                                <Select Handler="this.triggers[0].show();" />
                                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                        <ext:TextField ID="Text_IP" runat="server" FieldLabel='<%# GetLangStr("ServerManager20","服务器IP") %>' EmptyText='<%# GetLangStr("ServerManager33","例如：192.168.0.1") %>'
                                                            AnchorHorizontal="95%">
                                                        </ext:TextField>
                                                        <ext:TextField ID="Text_DK" runat="server" FieldLabel='<%# GetLangStr("ServerManager34","服务器端口") %>' EmptyText='<%# GetLangStr("ServerManager35","例如：8000") %>'
                                                            AnchorHorizontal="95%">
                                                        </ext:TextField>
                                                    </Items>
                                                </ext:Container>
                                                <ext:Container ID="Container6" runat="server" LabelAlign="Left" Layout="Form" ColumnWidth=".5">
                                                    <Items>
                                                        <ext:ComboBox ID="Cob_TXLX" runat="server" FieldLabel='<%# GetLangStr("ServerManager36","通讯类型") %>' AnchorHorizontal="95%"
                                                            Editable="false" StoreID="StoreTXLX" DisplayField="col2" ValueField="col1" TypeAhead="true"
                                                            Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("ServerManager37","选择通讯类型...") %>' SelectOnFocus="true">
                                                            <Triggers>
                                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ServerManager9","清除选中") %>' AutoDataBind="true" />
                                                            </Triggers>
                                                            <Listeners>
                                                                <Select Handler="this.triggers[0].show();" />
                                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                        <ext:ComboBox ID="Cob_TXXY" runat="server" FieldLabel='<%# GetLangStr("ServerManager38","通讯协议") %>' AnchorHorizontal="95%"
                                                            Editable="false" StoreID="StoreTXXY" DisplayField="col2" ValueField="col1" TypeAhead="true"
                                                            Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("ServerManager39","选择通讯协议...") %>' SelectOnFocus="true">
                                                            <Triggers>
                                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ServerManager9","清除选中") %>' AutoDataBind="true" />
                                                            </Triggers>
                                                            <Listeners>
                                                                <Select Handler="this.triggers[0].show();" />
                                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                        <ext:TextField ID="Text_YHM" runat="server" FieldLabel='<%# GetLangStr("ServerManager40","用户名") %>' EmptyText="例如：admin"
                                                            AnchorHorizontal="95%">
                                                        </ext:TextField>
                                                        <ext:TextField ID="Text_MM" runat="server" FieldLabel='<%# GetLangStr("ServerManager41","密码") %>' EmptyText="例如：admin" AnchorHorizontal="95%">
                                                        </ext:TextField>
                                                        <ext:TextField ID="Text_TYDS" runat="server" FieldLabel='<%# GetLangStr("ServerManager42","服务器通道数") %>' EmptyText='<%# GetLangStr("ServerManager43","例如：9") %>'
                                                            AnchorHorizontal="95%"  AllowBlank="false">
                                                        </ext:TextField>
                                                        <ext:ComboBox ID="Cob_SFSY" runat="server" FieldLabel='<%# GetLangStr("ServerManager57","是否使用") %>' AnchorHorizontal="95%"
                                                            AllowBlank="false" Editable="false" StoreID="StoreSY" DisplayField="col2" ValueField="col1"
                                                            TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("ServerManager44","选择是否使用...") %>' SelectOnFocus="true">
                                                            <Triggers>
                                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ServerManager9","清除选中") %>' AutoDataBind="true" />
                                                            </Triggers>
                                                            <Listeners>
                                                                <Select Handler="this.triggers[0].show();" />
                                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                    </Items>
                                                </ext:Container>
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel ID="Panel2" runat="server" Title='<%# GetLangStr("ServerManager45","资产信息") %>' Padding="6" AutoScroll="true">
                                    <Items>
                                        <ext:Container ID="Container1" runat="server" Layout="Column" Height="100">
                                            <Items>
                                                <ext:Container ID="Container2" runat="server" LabelAlign="Left" Layout="Form" ColumnWidth="1">
                                                    <Items>
                                                        <ext:ComboBox ID="Cob_JSDW" runat="server" FieldLabel='<%# GetLangStr("ServerManager46","建设单位") %>' AnchorHorizontal="95%"
                                                            AllowBlank="false" Editable="false" StoreID="StoreJSDW" DisplayField="col1" ValueField="col0"
                                                            TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("ServerManager47","选择建设单位...") %>' SelectOnFocus="true">
                                                            <Triggers>
                                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ServerManager9","清除选中") %>' AutoDataBind="true" />
                                                            </Triggers>
                                                            <Listeners>
                                                                <Select Handler="this.triggers[0].show();" />
                                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                        <ext:ComboBox ID="Cob_WHDW" runat="server" FieldLabel='<%# GetLangStr("ServerManager48","维护单位") %>' AnchorHorizontal="95%"
                                                            AllowBlank="false" Editable="false" StoreID="StoreWHDW" DisplayField="col1" ValueField="col0"
                                                            TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("ServerManager49","选择维护单位...") %>' SelectOnFocus="true">
                                                            <Triggers>
                                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ServerManager9","清除选中") %>' AutoDataBind="true" />
                                                            </Triggers>
                                                            <Listeners>
                                                                <Select Handler="this.triggers[0].show();" />
                                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                        <ext:ComboBox ID="Cob_ZZCS" runat="server" FieldLabel='<%# GetLangStr("ServerManager50","设备制造厂商") %>' AnchorHorizontal="95%"
                                                            AllowBlank="false" Editable="false" StoreID="StoreZZCS" DisplayField="col1" ValueField="col0"
                                                            TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("ServerManager51","选择设备制造厂商...") %>' SelectOnFocus="true">
                                                            <Triggers>
                                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ServerManager9","清除选中") %>' AutoDataBind="true" />
                                                            </Triggers>
                                                            <Listeners>
                                                                <Select Handler="this.triggers[0].show();" />
                                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                    </Items>
                                                </ext:Container>
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Panel>
                            </Items>
                        </ext:TabPanel>
                    </Items>
                    <Listeners>
                        <ClientValidation Handler="Button5.setDisabled(!valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button ID="Button5" runat="server" Icon="Add" Text='<%# GetLangStr("ServerManager52","确定") %>'>
                    <DirectEvents>
                        <Click OnEvent="UpdateDevice" />
                    </DirectEvents>
                </ext:Button>
                <ext:Button ID="Button6" runat="server" Icon="TextItalic" Text='<%# GetLangStr("ServerManager53","取消") %>'>
                    <Listeners>
                        <Click Handler="#{Window4}.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>
    </form>
</body>
</html>