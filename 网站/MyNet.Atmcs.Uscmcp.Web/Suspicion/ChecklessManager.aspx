<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChecklessManager.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.ChecklessManager" %>

<%@ Register Src="../UIDepartment.ascx" TagName="UIDepartment" TagPrefix="dpart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>畅行车辆管理</title>
    <meta http-equiv="Content-Type" content="text/html" charset="gb32312" />
    <link href="Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="utf-8"></script>
    <script type="text/javascript">
        var template = '<span style="color:{0};">{1}</span>';
        var change = function (value) {
            return String.format(template, (value == "比对") ? "green" : "red", value);
        };
        var changetime = function (value) {
            var mydate = Ext.util.Format.date(new Date(), 'Y-m-d');
            return String.format(template, (value > mydate) ? "green" : "red", value);
        };
    </script>
    <script type="text/javascript">
        function ShowMessage(messgeinfo) {
            alert(messgeinfo);
            document.getElementById("LblPoress").value = messgeinfo;
        }
        $(function () {
            $("body").delegate("#TxtplateId", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#CmbPlateType").click();
                }
            })
        })
        $(function () {
            $("body").delegate("#txtHphm", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#cmbHpzl").click();
                }
            })
        })
    </script>
    <script type="text/javascript">
        var filterTree = function (el, e) {
            var tree = uiDepartment_TreeDepartment,
                text = this.getRawValue();
            tree.clearFilter();
            if (Ext.isEmpty(text, false)) {
                return;
            }

            if (e.getKey() === Ext.EventObject.ESC) {
                clearFilter();
            } else {
                var re = new RegExp(".*" + text + ".*", "i");

                tree.filterBy(function (node) {
                    return re.test(node.text);
                });
            }
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="ChecklessManager" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Store ID="StorePlateType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreColor" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreMdlx" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreBdbj" runat="server">
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
                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("ChecklessManager1","车辆号牌：") %>'>
                                </ext:Label>
                                <ext:TextField ID="TxtplateId" runat="server" Width="100" />
                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("ChecklessManager2","车牌类型：") %>'>
                                </ext:Label>
                                <ext:ComboBox ID="CmbPlateType" runat="server" Editable="false" StoreID="StorePlateType"
                                    DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                    EmptyText='<%# GetLangStr("ChecklessManager3","选择车牌类型...") %>' SelectOnFocus="true" Width="123">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%#GetLangStr("ChecklessManager4","清除选中") %>' />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Label ID="Label5" runat="server" Text='<%# GetLangStr("ChecklessManager5","对比类型：") %>'>
                                </ext:Label>
                                <ext:ComboBox ID="CmbQueryMdlx" runat="server" Editable="false" StoreID="StoreMdlx"
                                    DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                    EmptyText='<%# GetLangStr("ChecklessManager6","选择比对类型...") %>' SelectOnFocus="true" Width="123">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%#GetLangStr("ChecklessManager4","清除选中") %>' />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%#GetLangStr("ChecklessManager7","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" Timeout="60000">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%#GetLangStr("ChecklessManager8","重置") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButRefresh" runat="server" Icon="DriveGo" Text='<%#GetLangStr("ChecklessManager9","刷新") %>' Hidden="true">
                                    <DirectEvents>
                                        <Click OnEvent="ButRefreshClick" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <ext:FormPanel ID="FormPanel2" Region="Center" runat="server" Layout="FitLayout">
                    <Items>
                        <ext:GridPanel ID="GridCheckless" runat="server" StripeRows="true" AutoScroll="true">
                            <TopBar>
                                <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StoreCheckless" HideRefresh="true">
                                    <Items>
                                        <ext:Toolbar ID="ToolExport" runat="server">
                                            <Items>
                                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                                <ext:Button ID="Button1" runat="server" Text='<%#GetLangStr("ChecklessManager10","导出XML") %>' AutoPostBack="true" OnClick="ToXml" Hidden="true"
                                                    Icon="PageCode">
                                                </ext:Button>
                                                <ext:Button ID="Button2" runat="server" Text='<%#GetLangStr("ChecklessManager11","导出Excel") %>' AutoPostBack="true" OnClick="ToExcel"
                                                    Icon="PageExcel">
                                                </ext:Button>
                                                <ext:Button ID="Button3" runat="server" Text='<%#GetLangStr("ChecklessManager12","导出CSV") %>' AutoPostBack="true" OnClick="ToCsv" Hidden="true"
                                                    Icon="PageAttach">
                                                </ext:Button>
                                                <ext:Button ID="ButPrint" runat="server" Icon="Printer" Text='<%#GetLangStr("ChecklessManager13","打印") %>' Hidden="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="ButPrintClick" />
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </Items>
                                </ext:PagingToolbar>
                            </TopBar>
                            <Store>
                                <ext:Store ID="StoreCheckless" runat="server" OnRefreshData="MyData_Refresh">
                                    <AutoLoadParams>
                                        <ext:Parameter Name="start" Value="={0}" />
                                        <ext:Parameter Name="limit" Value="={15}" />
                                    </AutoLoadParams>
                                    <UpdateProxy>
                                        <ext:HttpWriteProxy Method="GET" Url="ChecklessManager.aspx">
                                        </ext:HttpWriteProxy>
                                    </UpdateProxy>
                                    <Reader>
                                        <ext:JsonReader>
                                            <Fields>
                                                <ext:RecordField Name="col0" Type="String" />
                                                <ext:RecordField Name="col1" Type="String" />
                                                <ext:RecordField Name="col2" Type="String" />
                                                <ext:RecordField Name="col3" Type="String" />
                                                <ext:RecordField Name="col4" Type="String" />
                                                <ext:RecordField Name="col5" Type="String" />
                                                <ext:RecordField Name="col6" Type="String" />
                                                <ext:RecordField Name="col7" Type="String" />
                                                <ext:RecordField Name="col8" Type="string" />
                                                <ext:RecordField Name="col9" Type="String" />
                                                <ext:RecordField Name="col10" Type="String" />
                                                <ext:RecordField Name="col11" Type="String" />
                                                <ext:RecordField Name="col12" Type="String" />
                                                <ext:RecordField Name="col13" Type="String" />
                                                <ext:RecordField Name="col14" Type="String" />
                                                <ext:RecordField Name="col15" Type="Date" />
                                                <ext:RecordField Name="col16" Type="String" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column Header='<%# GetLangStr("ChecklessManager14","比对编号") %>' AutoDataBind="true" DataIndex="col0" Width="0" Hidden="true" />
                                    <ext:Column Header='<%# GetLangStr("ChecklessManager15","号牌号码") %>' AutoDataBind="true" DataIndex="col1" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("ChecklessManager16","号牌种类") %>' AutoDataBind="true" DataIndex="col3" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("ChecklessManager17","车身颜色") %>' AutoDataBind="true" DataIndex="col4" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("ChecklessManager18","车辆品牌") %>' AutoDataBind="true" DataIndex="col5" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("ChecklessManager19","有效时间") %>' AutoDataBind="true" DataIndex="col8" Width="100" Align="Center">
                                        <Renderer Fn="changetime" />
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("ChecklessManager20","比对类型") %>' AutoDataBind="true" DataIndex="col7" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("ChecklessManager21","比对标识") %>' AutoDataBind="true" DataIndex="col13" Width="100" Align="Center">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:DateColumn Header='<%# GetLangStr("ChecklessManager22","更新时间") %>' AutoDataBind="true" DataIndex="col15" Format="yyyy-MM-dd HH:mm:ss" Width="100" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    <DirectEvents>
                                        <RowSelect OnEvent="SelectCheckless" Buffer="250">
                                            <ExtraParams>
                                                <ext:Parameter Name="sdata" Value="record.data" Mode="Raw" />
                                            </ExtraParams>
                                        </RowSelect>
                                    </DirectEvents>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <View>
                                <ext:GridView runat="server" ForceFit="true"></ext:GridView>
                            </View>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Frame="true"
                    Width="300" Icon="Table" DefaultAnchor="100%" MonitorValid="true">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:TabStrip ID="TabStrip1" runat="server" Height="40">
                                    <Items>
                                        <ext:TabStripItem ActionItemID="pnlAmply" Title='<%# GetLangStr("ChecklessManager24","车辆信息") %>' AutoDataBind="true" />
                                        <ext:TabStripItem ActionItemID="pnlImport" Title='<%# GetLangStr("ChecklessManager25","批量录入") %>' AutoDataBind="true" />
                                    </Items>
                                    <DirectEvents>
                                        <TabChange OnEvent="Unnamed_Event"></TabChange>
                                    </DirectEvents>
                                </ext:TabStrip>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:FormPanel ID="pnlAmply" runat="server" Header="false" Title='<%# GetLangStr("ChecklessManager26","车辆信息") %>' DefaultAnchor="100%"
                            Padding="5">
                            <Items>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("ChecklessManager27","对比编号") %>'
                                    ID="txtID" Width="280" ReadOnly="true" Hidden="true" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("ChecklessManager28","号牌号码") %>' ID="txtHphm" AllowBlank="false" Width="280" />
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("ChecklessManager29","号牌种类") %>' ID="cmbHpzl" StoreID="StorePlateType"
                                    Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("ChecklessManager30","选择号牌种类") %>' Width="280" AllowBlank="false">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%#GetLangStr("ChecklessManager4","清除选中") %>' />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("ChecklessManager31","车身颜色") %>' ID="cmbCsys" StoreID="StoreColor"
                                    Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("ChecklessManager32","选择车身颜色") %>' Width="280">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%#GetLangStr("ChecklessManager4","清除选中") %>' />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("ChecklessManager33","车辆品牌") %>' ID="txtClpp" Width="280" />
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("ChecklessManager34","对比类型") %>' ID="cmbMdlx" StoreID="StoreMdlx" Editable="false"
                                    DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All" EmptyText='<%#GetLangStr("ChecklessManager4","选择对比类型") %>'
                                    Width="280" AllowBlank="false">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%#GetLangStr("ChecklessManager4","清除选中") %>' />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <%--  <ext:CompositeField ID="CompositeField1" runat="server" FieldLabel="有效时间"  Width="280">
                                    <Items>
                                        <ext:DateField ID="DateYxsj" runat="server" Vtype="daterange" AllowBlank="false">
                                        </ext:DateField>
                                      <ext:TimeField ID="TimeYxsj" runat="server" Increment="1" Width="77" />
                                    </Items>
                                </ext:CompositeField>--%>
                                <ext:DateField ID="DateYxsj" runat="server" Vtype="daterange" Format="yyyy-MM-dd" AllowBlank="false" ColumnWidth="1" FieldLabel='<%#GetLangStr("ChecklessManager75","有效时间") %>' Width="280">
                                </ext:DateField>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("ChecklessManager35","对比原因") %>' ID="txtSjyy" Width="280" />
                                <ext:Panel ID="Panel2" runat="server" Height="28" Layout="AnchorLayout" Border="false">
                                    <Content>
                                        <dpart:UIDepartment ID="uiDepartment" runat="server" FieldLabel='<%# GetLangStr("ChecklessManager36","数据来源") %>' Width="278" ListWidth="200" />
                                    </Content>
                                </ext:Panel>
                                <ext:ComboBox ID="cmbbdbj" FieldLabel='<%# GetLangStr("ChecklessManager37","比对标识") %>' runat="server" StoreID="StoreBdbj" Editable="false"
                                    DisplayField="col1" ValueField="col0" Mode="Local" EmptyText='<%# GetLangStr("ChecklessManager38","选择比对或者撤销") %>' Width="280"
                                    AllowBlank="false">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ChecklessManager4","清除选中") %>' />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("ChecklessManager39","备注信息") %>' ID="TxtBz" Width="280" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("ChecklessManager40","更新时间") %>' ID="TxtGxsj" Width="280" ReadOnly="true" Hidden="true" />
                            </Items>
                            <TopBar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:Button ID="ButAdd" runat="server" Text='<%#GetLangStr("ChecklessManager41","增加") %>' Icon="Add" ToolTip='<%# GetLangStr("ChecklessManager41","增加") %>'>
                                            <Listeners>
                                                <Click Handler="ChecklessManager.InfoSave()" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="ButUpdate" runat="server" Text='<%# GetLangStr("ChecklessManager42","保存") %>' Icon="TableSave">
                                            <Listeners>
                                                <Click Handler="ChecklessManager.UpdateData()" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="ButDelete" runat="server" Text='<%# GetLangStr("ChecklessManager43","删除") %>' Icon="Delete">
                                            <Listeners>
                                                <Click Handler="ChecklessManager.DoConfirm()" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:FormPanel>
                        <ext:FormPanel ID="pnlImport" runat="server" Header="false" Title='<%# GetLangStr("ChecklessManager44","车辆实际") %>' DefaultAnchor="100%"
                            Padding="5" LabelAlign="Left">
                            <Items>
                                <ext:FileUploadField ID="ExcelFile" runat="server" EmptyText='<%# GetLangStr("ChecklessManager45","选择Excel文件") %>' FieldLabel='<%# GetLangStr("ChecklessManager25","批量录入") %>'
                                    ButtonText="" Icon="TableAdd" />
                            </Items>
                            <Buttons>
                                <ext:Button ID="ButDownload" runat="server" Text='<%#GetLangStr("ChecklessManager46","模板下载") %>' Icon="DiskDownload">
                                    <Listeners>
                                        <Click Handler="ChecklessManager.Download()" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButSave" runat="server" Text='<%#GetLangStr("ChecklessManager42","保存") %>' Icon="TableSave">
                                    <DirectEvents>
                                        <Click OnEvent="StartLongAction" />
                                    </DirectEvents>
                                </ext:Button>
                            </Buttons>
                        </ext:FormPanel>
                        <ext:ProgressBar ID="Progress1" runat="server" Width="300" />
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
        <ext:TaskManager ID="TaskManager1" runat="server">
            <Tasks>
                <ext:Task TaskID="SaveExcelData" Interval="1000" AutoRun="false" OnStart=" #{ButSave}.setDisabled(true);"
                    OnStop=" #{ButSave}.setDisabled(false);">
                    <DirectEvents>
                        <Update OnEvent="RefreshProgress" />
                    </DirectEvents>
                </ext:Task>
            </Tasks>
        </ext:TaskManager>
    </form>
</body>
</html>