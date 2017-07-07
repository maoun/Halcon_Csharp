<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportantManager.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.ImportantManager" %>

<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<%@ Register Src="../UIDepartment.ascx" TagName="UIDepartment" TagPrefix="dpart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%# GetLangStr("ImportantManager1","重点车辆管理") %></title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <style type="text/css">
        #WindowEditor1_Panel1 {
            background: white;
        }

            #WindowEditor1_Panel1 .x-btn {
                border-radius: 0px;
                border: none;
            }

            #WindowEditor1_Panel1 button {
                height: 24px;
            }
    </style>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8"></script>
    <script type="text/javascript" language="javascript">

        var employeeDetailsRender = function () {
            return '<img class="imgEdit" ext:qtip=" <%# GetLangStr("ImportantManager2","点击查看详细信息") %>" style="cursor:pointer;" src="Images/vcard_edit.png"; />';
        };

        function open() {

            var row = GridDeviceManager.getSelectionModel().getSelections();
            var id = row[0].data.col0.toString();
            OnEvl.onclick(id);
        }
        function updata() {

            var row = GridDeviceManager.getSelectionModel().getSelections();

            if (row[0] == null) {
                Ext.MessageBox.alert('<%# GetLangStr("ImportantManager3","提示") %>', '<%# GetLangStr("ImportantManager4","请选择需要处理记录") %>'); return false;
            }
            else {

                var id = row[0].data.col0.toString();
                OnEvl.update(id);
            }
        }
        function updatedevice() {
            var row = GridDeviceManager.getSelectionModel().getSelections();
            var id = row[0].data.col0.toString();
            OnEvl.updatedevice(id);
        }
        function delecte() {
            var row = GridDeviceManager.getSelectionModel().getSelections();

            if (row[0] == null) {
                Ext.MessageBox.alert('<%# GetLangStr("ImportantManager3","提示") %>', '<%# GetLangStr("ImportantManager4","请选择需要处理记录") %>'); return false;
            }
            else {

                var id = row[0].data.col0.toString();
                Ext.MessageBox.confirm('<%# GetLangStr("ImportantManager5","删除?") %>', '<%# GetLangStr("ImportantManager6","确定删除该条数据?") %>', function (code) {
                    if (code == "yes") {
                        OnDel.delecte(id);

                    } else {

                        return false;
                    }
                });
            }

        }
        function selectNode(value) {

            OnEvl.onclickTree(value);
        }
        function selectNode2(value, value2) {

            OnEvl.onclickTree2(value, value2);
        }
        function openphoto() {

            var a = BasicField.PostedFile.FileName;
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $("body").delegate("#TxtplateId", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#CmbPlateType").click();
                }
            })
        })
        $(function () {
            $("body").delegate("#TxtHphm", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#CmbHpzl").click();
                }
            })
        })
        var changeUpper = function (obj, value) {
            obj.setValue(value.toUpperCase());
        };
    </script>

    <script type="text/javascript">
        function showWindow() {
            Window2.show();
        }

        var setSelect = function (kakouNames) {
            Window2.hide();
            OnDel.showkkid(kakouNames);
        };
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

        function clearTime() {

            CmbVehileType.triggers[0].hide();
            CmbPlateType.triggers[0].hide();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PoliceVehicles" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Hidden ID="Hidden1" runat="server" />
        <ext:Hidden ID="Hidden_id" runat="server" />
        <ext:Hidden ID="Hidden_panduan" runat="server" />
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
        <ext:Store ID="StoreJingXian" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreGPS" runat="server">
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
        <ext:Store ID="StoreVehileType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StorePlateType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="Panel1" Region="Center" runat="server" Layout="FitLayout"
                    Height="40" AutoScroll="true">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server" Layout="Container">
                            <Items>
                                <ext:Toolbar ID="Toolbar4" runat="server">
                                    <Items>
                                        <ext:Label ID="Label2" runat="server" Text='<%# GetLangStr("ImportantManager7","号牌号码：") %>' StyleSpec="margin-left:10px;">
                                        </ext:Label>
                                        <ext:Panel ID="Panel5" runat="server" Height="29">
                                            <Content>
                                                <veh:VehicleHead ID="WindowEditor1" runat="server" />
                                            </Content>
                                        </ext:Panel>
                                        <ext:TextField ID="TxtplateId" runat="server" Width="70" EmptyText='<%# GetLangStr("ImportantManager8","六位号牌号码") %>' MaxLength="6">
                                            <Listeners>
                                                <Change Fn="changeUpper " />
                                            </Listeners>
                                        </ext:TextField>
                                        <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("ImportantManager9","号牌种类：") %>'>
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbPlateType" runat="server" Editable="false" StoreID="StorePlateType"
                                            DisplayField="CODEDESC" ValueField="CODE" TypeAhead="true" Mode="Local" ForceSelection="true"
                                            EmptyText='<%# GetLangStr("ImportantManager10","请选择...") %>' SelectOnFocus="true" Width="130">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ImportantManager11","清除选中") %>' />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>

                                        <ext:Label Hidden="true" ID="Label1" runat="server" Text='<%# GetLangStr("ImportantManager12","车辆类型：") %>' StyleSpec="margin-left:10px;">
                                        </ext:Label>
                                        <ext:ComboBox Hidden="true" ID="CmbVehileType" runat="server" Editable="false" StoreID="StoreVehileType"
                                            DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                            EmptyText='<%# GetLangStr("ImportantManager13","请选择车辆类型..") %>' SelectOnFocus="true" Width="130">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ImportantManager11","清除选中") %>' />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:Label ID="Label5" runat="server" Text='<%# GetLangStr("ImportantManager14","所属机构：") %>' StyleSpec="margin-left:10px;">
                                        </ext:Label>
                                        <ext:Panel ID="Panel2" runat="server" Layout="AnchorLayout" Border="false">
                                            <Content>
                                                <dpart:UIDepartment ID="uiDepartment" runat="server" Width="200" ListWidth="300" />
                                            </Content>
                                        </ext:Panel>
                                        <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("ImportantManager15","查询") %>'>
                                            <DirectEvents>
                                                <Click OnEvent="TbutQueryClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("ImportantManager16","重置") %>'>
                                            <DirectEvents>
                                                <Click OnEvent="ButResetClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButRefresh" runat="server" Hidden="true" Icon="Reload" Text='<%# GetLangStr("ImportantManager17","刷新") %>'>
                                            <DirectEvents>
                                                <Click OnEvent="ButRefreshClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="10" />
                                        <ext:Button ID="ButDevAdd" runat="server" Text='<%# GetLangStr("ImportantManager18","新增") %>' Icon="Add">
                                            <DirectEvents>
                                                <Click OnEvent="ButDevAdd_Click" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButDevModify" runat="server" Text='<%# GetLangStr("ImportantManager19","修改") %>' Icon="Accept">
                                            <Listeners>
                                                <Click Handler="updata()" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="ButDevDelete" runat="server" Text='<%# GetLangStr("ImportantManager20","删除") %>' Icon="Delete">
                                            <Listeners>
                                                <Click Handler="delecte()" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:ToolbarFill />
                                        <ext:Button ID="ButExcel" runat="server" Text='<%# GetLangStr("ImportantManager21","导出Excel") %>' AutoPostBack="true" OnClick="ToExcel"
                                            Icon="PageExcel">
                                        </ext:Button>
                                        <%--  <ext:Button ID="ButPrint" runat="server" Icon="Printer" Text='<%# GetLangStr("ImportantManager14","打印") %>'>
                                            <DirectEvents>
                                                <Click OnEvent="ButPrintClick" />
                                            </DirectEvents>
                                        </ext:Button>--%>
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridDeviceManager" runat="server" StripeRows="true" Title='<%# GetLangStr("ImportantManager22","车辆信息") %>' Header="false">
                            <TopBar>
                                <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StoreDeviceManager">
                                </ext:PagingToolbar>
                            </TopBar>
                            <Store>
                                <ext:Store ID="StoreDeviceManager" runat="server" OnRefreshData="MyData_Refresh">
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
                                    <ext:RowNumbererColumn Width="40" Align="Center"></ext:RowNumbererColumn>
                                    <ext:Column Header='<%# GetLangStr("ImportantManager23","号牌号码") %>' AutoDataBind="true" DataIndex="col1" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("ImportantManager24","号牌种类") %>' AutoDataBind="true" DataIndex="col2" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("ImportantManager25","车辆类型") %>' AutoDataBind="true" DataIndex="col3" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("ImportantManager26","司机姓名") %>' AutoDataBind="true" DataIndex="col4" Width="150" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("ImportantManager27","呼叫号码") %>' AutoDataBind="true" DataIndex="col5" Width="150" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("ImportantManager28","联系电话") %>' AutoDataBind="true" DataIndex="col8" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("ImportantManager29","所属机构") %>' AutoDataBind="true" DataIndex="col9" Width="300" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="false">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <Listeners>
                                <RowDblClick Handler="open()" />
                            </Listeners>
                            <View>
                                <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                                </ext:GridView>
                            </View>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="West" Split="true" Title='<%# GetLangStr("ImportantManager30","车辆类型列表") %>'
                    Width="160" Icon="Table">
                    <Items>
                        <ext:TreePanel ID="TreePanel1" runat="server" Icon="Drive" Shadow="None" UseArrows="true"
                            AutoScroll="true" Animate="true" ContainerScroll="true" EnableDD="true" RootVisible="false"
                            Header="false" Height="600">
                        </ext:TreePanel>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
        <ext:Window ID="Window1" runat="server" Icon="House" Hidden="true" Height="430px"
            Width="600px" MonitorValid="true" Title='<%# GetLangStr("ImportantManager31","重点车辆基本信息") %>'>
            <Content>
                <ext:TabPanel runat="server" Width="600px">
                    <Items>
                        <ext:Panel
                            ID="Tab1"
                            runat="server"
                            Title='<%# GetLangStr("ImportantManager32","基本信息") %>'
                            AutoHeight="true"
                            Padding="6">
                            <TopBar>
                                <ext:Toolbar ID="ToolbarFour" runat="server">
                                    <Items>
                                        <ext:Panel ID="Panel4" runat="server" Padding="10">
                                            <Items>
                                                <ext:Container ID="Container7" runat="server" Layout="Column" Height="400">
                                                    <Items>
                                                        <ext:Container ID="Container8" runat="server" LabelAlign="Left" Layout="Form" ColumnWidth=".5">
                                                            <Items>
                                                                <ext:Panel ID="Panel3" runat="server" Height="25" Layout="AnchorLayout" Border="false">
                                                                    <Content>
                                                                        <dpart:UIDepartment ID="uiDepartment1" runat="server" AllowBlank="false" FieldLabel='<%# GetLangStr("ImportantManager33","所属机构") %>' Width="270" ListWidth="200" />
                                                                    </Content>
                                                                </ext:Panel>

                                                                <%--<ext:TextField ID="TxtHphm" runat="server" FieldLabel="号牌号码" AnchorHorizontal="95%"
                                                    MaxLength="8" AllowBlank="false" />--%>
                                                                <ext:Panel ID="CompositeField2" runat="server" Width="280" Layout="ColumnLayout" Style="margin-top: 5px; margin-bottom: 5px">
                                                                    <Items>
                                                                        <ext:Label runat="server" Text='<%# GetLangStr("ImportantManager34","号牌号码:") %>' StyleSpec="margin-left:10px;" Style="margin-top: 5px; margin-right: 28px" />
                                                                        <ext:Panel ID="Panel6" runat="server">
                                                                            <Content>
                                                                                <veh:VehicleHead ID="VehicleHead" runat="server" />
                                                                            </Content>
                                                                        </ext:Panel>
                                                                        <ext:TextField ID="TxtHphm" runat="server" Width="125" AllowBlank="false">
                                                                            <Listeners>
                                                                                <Change Fn="changeUpper" />
                                                                            </Listeners>
                                                                        </ext:TextField>
                                                                    </Items>
                                                                </ext:Panel>
                                                                <ext:ComboBox ID="CmbHpzl" runat="server" FieldLabel='<%#GetLangStr("ImportantManager35","号牌种类") %>' AnchorHorizontal="95%"
                                                                    AllowBlank="false" Editable="false" StoreID="StorePlateType" DisplayField="CODEDESC"
                                                                    ValueField="CODE" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("ImportantManager10","请选择...") %>'
                                                                    SelectOnFocus="true">
                                                                    <Triggers>
                                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ImportantManager11","清除选中") %>' />
                                                                    </Triggers>
                                                                    <Listeners>
                                                                        <Select Handler="this.triggers[0].show();" />
                                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                                    </Listeners>
                                                                </ext:ComboBox>
                                                                <ext:ComboBox ID="CmbClzl" runat="server" FieldLabel='<%# GetLangStr("ImportantManager36","车辆类型") %>' AnchorHorizontal="95%" AllowBlank="false"
                                                                    Editable="false" StoreID="StoreVehileType" DisplayField="col1" ValueField="col0"
                                                                    TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("ImportantManager37","选择车辆类型...")%>' SelectOnFocus="true">
                                                                    <Triggers>
                                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ImportantManager11","清除选中") %>' />
                                                                    </Triggers>
                                                                    <Listeners>
                                                                        <Select Handler="this.triggers[0].show();" />
                                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                                    </Listeners>
                                                                </ext:ComboBox>
                                                                <ext:TextField ID="TxtClxh" runat="server" FieldLabel='<%# GetLangStr("ImportantManager38","车辆型号") %>' AnchorHorizontal="95%" />
                                                                <ext:TextField ID="TxtClpp" runat="server" FieldLabel='<%# GetLangStr("ImportantManager39","车辆品牌") %>' AnchorHorizontal="95%" />
                                                                <ext:DateField ID="DateField1" runat="server" FieldLabel='<%# GetLangStr("ImportantManager40","购置日期") %>' AnchorHorizontal="95%" AllowBlank="false" MaxDate="" />
                                                                <ext:TextField ID="TxtLxdh" runat="server" FieldLabel='<%# GetLangStr("ImportantManager41","联系电话") %>' AnchorHorizontal="95%"
                                                                    MaxLength="11" />
                                                                <ext:TextField ID="TxtCjh" runat="server" FieldLabel='<%# GetLangStr("ImportantManager42","车架号") %>' AnchorHorizontal="95%" />
                                                            </Items>
                                                        </ext:Container>
                                                        <ext:Container ID="Container9" runat="server" LabelAlign="Left" Layout="Form" ColumnWidth=".5">
                                                            <Items>
                                                                <ext:TextField ID="TxtFdjh" runat="server" FieldLabel='<%# GetLangStr("ImportantManager43","发动机号") %>' AnchorHorizontal="95%" />
                                                                <ext:TextField ID="TxtHdkl" runat="server" FieldLabel='<%# GetLangStr("ImportantManager44","核定客量") %>' AnchorHorizontal="95%" />
                                                                <ext:TextField ID="TxtSjxm" runat="server" FieldLabel='<%# GetLangStr("ImportantManager45","司机姓名") %>' AnchorHorizontal="95%" />
                                                                <ext:TextField ID="TxtSjhm" runat="server" FieldLabel='<%# GetLangStr("ImportantManager46","手机号码") %>' AnchorHorizontal="95%"
                                                                    MaxLength="11" />
                                                                <ext:TextField ID="TxtHjhm" runat="server" FieldLabel='<%# GetLangStr("ImportantManager47","呼叫号码") %>' AnchorHorizontal="95%"
                                                                    MaxLength="11" />
                                                                <ext:ComboBox ID="CmbGPS" runat="server" FieldLabel='<%# GetLangStr("ImportantManager48","有无GPS") %>' AnchorHorizontal="95%"
                                                                    Editable="false" StoreID="StoreGPS" DisplayField="col1" ValueField="col0" TypeAhead="true"
                                                                    Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("ImportantManager10","请选择....") %>' SelectOnFocus="true">
                                                                    <Triggers>
                                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("ImportantManager11","清除选中") %>' />
                                                                    </Triggers>
                                                                    <Listeners>
                                                                        <Select Handler="this.triggers[0].show();" />
                                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                                    </Listeners>
                                                                </ext:ComboBox>

                                                                <ext:TextField ID="TxtRyzk" runat="server" AnchorHorizontal="95%" FieldLabel='<%# GetLangStr("ImportantManager49","人员状况") %>' />
                                                                <ext:TextField ID="TxtClzt" runat="server" AnchorHorizontal="95%" FieldLabel='<%# GetLangStr("ImportantManager50","车辆状态") %>' />
                                                            </Items>
                                                        </ext:Container>
                                                    </Items>
                                                </ext:Container>
                                            </Items>
                                        </ext:Panel>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:Panel>
                        <ext:Panel
                            ID="Tab2"
                            runat="server"
                            Title='<%# GetLangStr("ImportantManager51","限行信息") %>'
                            AutoHeight="true"
                            Padding="6">
                            <TopBar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:Panel runat="server">
                                            <Items>
                                                <ext:Panel runat="server" Height="60" Layout="ColumnLayout" Padding="10">
                                                    <Items>
                                                        <ext:TimeField ID="TimeStart" runat="server" LabelWidth="70" FieldLabel='<%# GetLangStr("ImportantManager52","限行时间") %>' ColumnWidth=".3" AnchorHorizontal="95%" AllowBlank="false" MaxDate="" />
                                                        <ext:Label runat="server" Text=" --- " />
                                                        <ext:TimeField ID="TimeEnd" runat="server" LabelWidth="10" LabelSeparator="" ColumnWidth=".15" AnchorHorizontal="95%" AllowBlank="false" MaxDate="" />
                                                        <ext:TextField runat="server" Hidden="true" ColumnWidth="1" />
                                                    </Items>
                                                </ext:Panel>
                                                <ext:Panel runat="server">
                                                    <Items>
                                                        <ext:Button runat="server" Text='<%# GetLangStr("ImportantManager53","检测点标注") %>'>
                                                            <Listeners>
                                                                <Click Handler="showWindow();" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:TextArea ID="TxtKkid" runat="server" Width="500" Height="200" AutoScroll="true"></ext:TextArea>
                                                    </Items>
                                                </ext:Panel>
                                            </Items>
                                        </ext:Panel>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:Panel>
                    </Items>
                </ext:TabPanel>

                <ext:FormPanel runat="server" ID="from" Header="false" MonitorValid="true" Frame="true" Collapsible="true" Collapsed="false" DefaultAnchor="100%">
                    <Items>
                    </Items>
                    <Listeners>
                        <ClientValidation Handler="ButUpdate.setDisabled(!valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Content>
            <Buttons>
                <ext:Button ID="ButUpdate" runat="server" Icon="Add">
                    <DirectEvents>
                        <Click OnEvent="GengXin_Click" />
                    </DirectEvents>
                </ext:Button>
                <ext:Button ID="Button5" runat="server" Icon="Cancel" Text='<%# GetLangStr("ImportantManager54","取消") %>'>
                    <Listeners>
                        <Click Handler="#{Window1}.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>
    </form>
    <ext:Window runat="server" ID="Window2" Title='<%# GetLangStr("ImportantManager55","卡口选择") %>' Hidden="true" Maximizable="true"
        Modal="true" Width="1000" Height="600" ButtonAlign="Center">

        <AutoLoad
            Url="../Map/MapStation.aspx?kkmax=0"
            Mode="IFrame"
            ShowMask="true"
            MaskMsg='<%# GetLangStr("ImportantManager56","加载中...") %>' />
    </ext:Window>
</body>
</html>