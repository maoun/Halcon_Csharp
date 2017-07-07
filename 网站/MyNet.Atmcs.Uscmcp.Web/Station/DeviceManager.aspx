<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviceManager.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.DeviceManager" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>设备信息管理</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <style type="text/css">
        .x-grid-row-summary {
            color: #948d8e;
            text-decoration: line-through;
        }
    </style>
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="utf-8"></script>
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
        var showFile = function (fb, v) {
            DeviceManager.Excel();
        }
        function hreaf() {
            window.location.href = "../Export/设备批量导入表.xls"
        }
    </script>
    <script type="text/javascript">
        // 屏蔽backspace
        document.onkeydown = function (e) {
            var code;
            if (!e) { var e = window.event; }
            if (e.keyCode) { code = e.keyCode; }
            else if (e.which) { code = e.which; }
            //BackSpace 8;
            if ((event.keyCode == 8) && ((event.srcElement.type != "text" && event.srcElement.type != "textarea" && event.srcElement.type != "password") || event.srcElement.readOnly == true)) {
                event.keyCode = 0;
                event.returnValue = false;
            }
            return true;
        };
        function clearTime() {

            CmbDeviceType.triggers[0].hide();
            CmbSBXH.triggers[0].hide();

        }
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridDeviceManager.view.findRowIndex(this.triggerElement),
                cellIndex = GridDeviceManager.view.findCellIndex(this.triggerElement),
                record = StoreDeviceManager.getAt(rowIndex),
                fieldName = GridDeviceManager.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:Hidden ID="HidSaveFlag" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="HiddenId" runat="server">
        </ext:Hidden>
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="DeviceManager" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Store ID="StoreSDeviceMode" runat="server">
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
        <ext:Store ID="StoreDeviceMode" runat="server">
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
                        <ext:RecordField Name="col2" />
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
                <%--中间--%>
                <ext:FormPanel ID="Panel1" Region="Center" runat="server" Layout="FitLayout"
                    Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("DeviceManager1","设备名称：") %>' StyleSpec="margin-left: 10px; ">
                                </ext:Label>
                                <ext:TextField ID="TxtDeviceName" runat="server" Width="100" />
                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("DeviceManager2","设备类型：") %>' StyleSpec="margin-left:10px;">
                                </ext:Label>
                                <ext:ComboBox ID="CmbDeviceType" runat="server" Editable="false" StoreID="StoreDeviceType"
                                    DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                    EmptyText='<%# GetLangStr("DeviceManager3","选择设备类型...") %>' SelectOnFocus="true" Width="123">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("DeviceManager9","清除选中") %>' AutoDataBind="true" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();DeviceManager.SelectQDevice();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>

                                <ext:Label ID="Label4" runat="server" Text='<%# GetLangStr("DeviceManager4","设备型号：") %>' StyleSpec="margin-left:10px;">
                                </ext:Label>
                                <ext:ComboBox ID="CmbSBXH" runat="server" Editable="false" StoreID="StoreSDeviceMode"
                                    DisplayField="col3" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                    EmptyText='<%# GetLangStr("DeviceManager5","选择设备型号...") %>' SelectOnFocus="true" Width="123">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("DeviceManager9","清除选中") %>' AutoDataBind="true" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("DeviceManager6","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("DeviceManager7","重置") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButRefresh" runat="server" Icon="DriveGo" Hidden="true" Text='<%# GetLangStr("DeviceManager8","刷新") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButRefreshClick" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridDeviceManager" runat="server" StripeRows="true" TrackMouseOver="true">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar2" runat="server" Layout="Container">
                                    <Items>
                                        <ext:Toolbar ID="Toolbar5" runat="server">
                                            <Items>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="20" />
                                                <ext:Button ID="ButDevAdd" runat="server" Text='<%# GetLangStr("DeviceManager10","添加新设备") %>' Icon="DriveAdd">
                                                    <DirectEvents>
                                                        <Click OnEvent="ButDevAdd_Click" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButDevModify" runat="server" Text='<%# GetLangStr("DeviceManager11","修改信息") %>' Icon="DriveEdit">
                                                    <Listeners>
                                                        <Click Handler="DeviceManager.Modify();" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Button ID="ButDevDelete" runat="server" Text='<%# GetLangStr("DeviceManager12","删除设备") %>' Icon="DriveDelete">
                                                    <Listeners>
                                                        <Click Handler="DeviceManager.DoConfirm()" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:ToolbarFill />
                                            </Items>
                                        </ext:Toolbar>
                                        <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server"
                                            PageSize="15" StoreID="StoreDeviceManager">
                                            <Items>
                                                <ext:Button ID="ButExcel" runat="server" Text='<%# GetLangStr("DeviceManager14","导出Excel") %>'
                                                    AutoPostBack="true" OnClick="ToExcel"
                                                    Icon="PageExcel">
                                                </ext:Button>

                                                <%--    <ext:Button ID="ButPrint" runat="server" Icon="Printer" Text='<%# GetLangStr("DeviceManager15","打印") %>'>
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
                                                <ext:RecordField Name="col26" Type="String" />
                                                <ext:RecordField Name="col27" Type="String" />
                                                <ext:RecordField Name="col28" Type="String" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                                    <ext:Column Header='<%# GetLangStr("DeviceManager16","设备编号") %>' AutoDataBind="true" DataIndex="col0" Width="100" Align="Left" Hidden="true" />
                                    <ext:Column Header='<%# GetLangStr("DeviceManager17","设备名称") %>' AutoDataBind="true" DataIndex="col1" Width="200" Align="Left" />
                                    <ext:Column Header='<%# GetLangStr("DeviceManager18","设备类型") %>' AutoDataBind="true" DataIndex="col3" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("DeviceManager74","设备型号") %>' AutoDataBind="true" DataIndex="col5" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("DeviceManager19","设备IP") %>' AutoDataBind="true" DataIndex="col6" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("DeviceManager20","设备端口") %>' AutoDataBind="true" DataIndex="col7" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("DeviceManager21","外部编号") %>' AutoDataBind="true" DataIndex="col8" Width="100" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <Listeners>
                                <RowDblClick Handler="DeviceManager.RowDblClickShow();" />
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
                            <ToolTips>
                                <ext:ToolTip
                                    ID="RowTip"
                                    runat="server"
                                    Target="={GridDeviceManager.getView().mainBody}"
                                    Delegate=".x-grid3-cell"
                                    TrackMouse="true">
                                    <Listeners>
                                        <Show Fn="showTip" />
                                    </Listeners>
                                </ext:ToolTip>
                            </ToolTips>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>
                <%--左侧--%>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="West" Split="true" Title='<%# GetLangStr("DeviceManager22","设备信息列表") %>'
                    Width="200" Icon="Table">
                    <Items>
                        <ext:TreePanel ID="TreePanel1" runat="server" Icon="Drive" Shadow="None" UseArrows="true"
                            AutoScroll="true" Animate="true" ContainerScroll="true" EnableDD="true" RootVisible="true"
                            Header="false" Height="800">
                        </ext:TreePanel>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
        <ext:Window ID="Window4" runat="server" Icon="House" Hidden="true" Height="460px"
            Width="600px" MonitorValid="true" Title='<%# GetLangStr("DeviceManager23","设备信息维护") %>'>
            <Items>
                <ext:FormPanel runat="server" ID="FormPanel3" MonitorValid="true">
                    <Items>
                        <ext:TabPanel ID="TabPanel1" runat="server" ActiveTabIndex="0" Width="590" Height="340">
                            <Items>
                                <ext:Panel ID="Tab1" runat="server" Title='<%# GetLangStr("DeviceManager24","设备信息") %>' Padding="6" AutoScroll="true">
                                    <Items>
                                        <ext:Container ID="Container4" runat="server" Layout="Column" Height="290">
                                            <Items>
                                                <ext:Container ID="Container5" runat="server" LabelAlign="Left" Layout="Form" ColumnWidth=".5">
                                                    <Items>
                                                        <ext:TextField ID="Text_SBBH" runat="server" FieldLabel='<%# GetLangStr("DeviceManager16","设备编号") %>' EmptyText='<%# GetLangStr("DeviceManager26","例如：00000000001") %>'
                                                            AnchorHorizontal="95%" ReadOnly="true" Disabled="true">
                                                        </ext:TextField>
                                                        <ext:ComboBox ID="Cob_SBLX" runat="server" FieldLabel='<%# GetLangStr("DeviceManager18","设备类型") %>' AnchorHorizontal="95%"
                                                            AllowBlank="false" Editable="false" StoreID="StoreDeviceType" DisplayField="col1"
                                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("DeviceManager27","选择设备类型...") %>'
                                                            SelectOnFocus="true">
                                                            <Listeners>
                                                                <Select Handler="DeviceManager.SelectDevice();" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                        <ext:TextField ID="Text_SBMC" runat="server" FieldLabel='<%# GetLangStr("DeviceManager17","设备名称") %>' EmptyText='<%# GetLangStr("DeviceManager33","例如：东北路口卡口设备") %>'
                                                            AnchorHorizontal="95%" AllowBlank="false">
                                                        </ext:TextField>
                                                        <ext:ComboBox ID="Cob_SBXH" runat="server" FieldLabel='<%# GetLangStr("DeviceManager29","设备型号") %>' AnchorHorizontal="95%"
                                                            AllowBlank="false" Editable="false" StoreID="StoreDeviceMode" DisplayField="col3"
                                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("DeviceManager30","选择设备型号...") %>'
                                                            SelectOnFocus="true">
                                                        </ext:ComboBox>
                                                        <ext:ComboBox ID="Cob_SFSY" runat="server" FieldLabel='<%# GetLangStr("DeviceManager31","是否使用") %>' AnchorHorizontal="95%"
                                                            AllowBlank="false" Editable="false" StoreID="StoreSY" DisplayField="col2" ValueField="col1"
                                                            TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("DeviceManager32","选择是否使用...") %>' SelectOnFocus="true">
                                                        </ext:ComboBox>
                                                        <ext:TextField ID="Text_IP" runat="server" FieldLabel='<%# GetLangStr("DeviceManager19","设备IP") %>' EmptyText='<%# GetLangStr("DeviceManager34","例如：192.168.0.1")%>'
                                                            AnchorHorizontal="95%">
                                                        </ext:TextField>
                                                        <ext:TextField ID="Text_DK" runat="server" FieldLabel='<%# GetLangStr("DeviceManager20","设备端口") %>' EmptyText='<%# GetLangStr("DeviceManager36","例如：8000") %>'
                                                            AnchorHorizontal="95%">
                                                        </ext:TextField>
                                                        <ext:TextField ID="Text_TYDS" runat="server" FieldLabel='<%# GetLangStr("DeviceManager37","设备通道数") %>' EmptyText='<%# GetLangStr("DeviceManager38","例如：9") %>'
                                                            AnchorHorizontal="95%">
                                                        </ext:TextField>
                                                        <ext:TextField ID="TxtDeviceIdExt" runat="server" FieldLabel='<%# GetLangStr("DeviceManager21","外部编号") %>' EmptyText='<%# GetLangStr("DeviceManager40","例如：1234567890123456") %>'
                                                            AnchorHorizontal="95%">
                                                        </ext:TextField>
                                                    </Items>
                                                </ext:Container>
                                                <ext:Container ID="Container6" runat="server" LabelAlign="Left" Layout="Form" ColumnWidth=".5">
                                                    <Items>
                                                        <ext:ComboBox ID="Cob_TXLX" runat="server" FieldLabel='<%# GetLangStr("DeviceManager41","通讯类型") %>' AnchorHorizontal="95%"
                                                            Editable="false" StoreID="StoreTXLX" DisplayField="col2" ValueField="col1" TypeAhead="true"
                                                            Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("DeviceManager42","选择通讯类型...") %>' SelectOnFocus="true">
                                                        </ext:ComboBox>
                                                        <ext:ComboBox ID="Cob_TXXY" runat="server" FieldLabel='<%# GetLangStr("DeviceManager43","通讯协议") %>' AnchorHorizontal="95%"
                                                            Editable="false" StoreID="StoreTXXY" DisplayField="col2" ValueField="col1" TypeAhead="true"
                                                            Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("DeviceManager44","选择通讯协议...") %>' SelectOnFocus="true">
                                                        </ext:ComboBox>
                                                        <ext:TextField ID="Text_YHM" runat="server" FieldLabel='<%# GetLangStr("DeviceManager45","用户名") %>' EmptyText='<%# GetLangStr("DeviceManager46","例如：admin") %>'
                                                            AnchorHorizontal="95%">
                                                        </ext:TextField>
                                                        <ext:TextField ID="Text_MM" runat="server" FieldLabel='<%# GetLangStr("DeviceManager47","密码") %>' EmptyText='<%# GetLangStr("DeviceManager48","例如：admin") %>' AnchorHorizontal="95%">
                                                        </ext:TextField>
                                                        <ext:TextField ID="Text_Ledth" runat="server" FieldLabel='<%# GetLangStr("DeviceManager49","设备长度") %>' EmptyText='<%# GetLangStr("DeviceManager50","例如：100") %>'
                                                            AnchorHorizontal="95%">
                                                        </ext:TextField>
                                                        <ext:TextField ID="Text_Width" runat="server" FieldLabel='<%# GetLangStr("DeviceManager51","设备宽度") %>' EmptyText='<%# GetLangStr("DeviceManager52","例如：100") %>'
                                                            AnchorHorizontal="95%">
                                                        </ext:TextField>
                                                        <ext:TextField ID="Text_CKH" runat="server" FieldLabel='<%# GetLangStr("DeviceManager53","串口号") %>' EmptyText='<%# GetLangStr("DeviceManager54","例如：1") %>' AnchorHorizontal="95%">
                                                        </ext:TextField>
                                                        <ext:TextField ID="Text_CKCS" runat="server" FieldLabel='<%# GetLangStr("DeviceManager55","串口参数") %>' EmptyText='<%# GetLangStr("DeviceManager56","例如：9600,n,8,1") %>'
                                                            AnchorHorizontal="95%">
                                                        </ext:TextField>
                                                    </Items>
                                                </ext:Container>
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel ID="Panel2" runat="server" Title='<%# GetLangStr("DeviceManager57","资产信息") %>' Padding="6" AutoScroll="true">
                                    <Items>
                                        <ext:Container ID="Container1" runat="server" Layout="Column" Height="100">
                                            <Items>
                                                <ext:Container ID="Container2" runat="server" LabelAlign="Left" Layout="Form" ColumnWidth="1">
                                                    <Items>
                                                        <ext:ComboBox ID="Cob_JSDW" runat="server" FieldLabel='<%# GetLangStr("DeviceManager58","建设单位") %>' AnchorHorizontal="95%"
                                                            AllowBlank="false" Editable="false" StoreID="StoreJSDW" DisplayField="col1" ValueField="col0"
                                                            TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("DeviceManager59","选择建设单位...") %>' SelectOnFocus="true">
                                                        </ext:ComboBox>
                                                        <ext:ComboBox ID="Cob_WHDW" runat="server" FieldLabel='<%# GetLangStr("DeviceManager60","维护单位") %>' AnchorHorizontal="95%"
                                                            AllowBlank="false" Editable="false" StoreID="StoreWHDW" DisplayField="col1" ValueField="col0"
                                                            TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("DeviceManager61","选择维护单位...") %>' SelectOnFocus="true">
                                                        </ext:ComboBox>
                                                        <ext:ComboBox ID="Cob_ZZCS" runat="server" FieldLabel='<%# GetLangStr("DeviceManager62","设备制造商") %>' AnchorHorizontal="95%"
                                                            AllowBlank="false" Editable="false" StoreID="StoreZZCS" DisplayField="col1" ValueField="col0"
                                                            TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("DeviceManager63","选择设备制造商...") %>' SelectOnFocus="true">
                                                        </ext:ComboBox>
                                                    </Items>
                                                </ext:Container>
                                                <%--                                            <ext:Container ID="Container3" runat="server" LabelAlign="Left" Layout="Form" ColumnWidth=".55">
                                                <Items>
                                                </Items>
                                            </ext:Container>--%>
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
            <BottomBar>
                <ext:Toolbar runat="server">
                    <Items>
                        <ext:Checkbox ID="CheckSave" runat="server">
                        </ext:Checkbox>
                        <ext:Label ID="LabelSave" runat="server" FieldLabel='<%# GetLangStr("DeviceManager64","保存完成是否关闭窗体") %>'></ext:Label>
                    </Items>
                </ext:Toolbar>
            </BottomBar>
            <Buttons>
                <ext:Button ID="Button5" runat="server" Icon="Add" Text='<%# GetLangStr("DeviceManager65","确定") %>'>
                    <DirectEvents>
                        <Click OnEvent="UpdateDevice" />
                    </DirectEvents>
                </ext:Button>
                <ext:Button ID="Button6" runat="server" Icon="TextItalic" Text='<%# GetLangStr("DeviceManager66","取消") %>'>
                    <Listeners>
                        <Click Handler="#{Window4}.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>
    </form>
</body>
</html>