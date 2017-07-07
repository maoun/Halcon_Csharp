<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PoliceOfficer.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.PoliceOfficer" %>

<%@ Register Src="../UIDepartment.ascx" TagName="UIDepartment" TagPrefix="dpart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>警务人员管理</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <script type="text/javascript" language="javascript">

        var employeeDetailsRender = function () {
            return '<img class="imgEdit" ext:qtip="点击查看详细信息" style="cursor:pointer;" src="../Images/vcard_edit.png"; />';
        };

        function open() {
            var row = GridDeviceManager.getSelectionModel().getSelections();
            var id = row[0].data.col0.toString();
            OnEvl.Onclick(id);
        }
        function updata() {

            var row = GridDeviceManager.getSelectionModel().getSelections();
            if (row[0] == null) {
                Ext.MessageBox.alert('提示', '请选择需要处理记录'); return false;
            }
            else {
                var id = row[0].data.col0.toString();
                OnEvl.Update(id);
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
                Ext.MessageBox.alert('提示', '请选择需要处理记录'); return false;
            }
            else {

                var id = row[0].data.col0.toString();
                var name = row[0].data.col1.toString();
                Ext.MessageBox.confirm("删除?", "确定删除该条数据?", function (code) {
                    if (code == "yes") {
                        OnDel.Delete(id, name);

                    } else {
                        return false;
                    }
                });
            }
        }
        function selectNode(value) {
            OnEvl.onclickTree(value);
        }
        function openphoto() {

            var a = BasicField.PostedFile.FileName;
        }
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

            CmbDutyType.triggers[0].hide();
            CmbSex.triggers[0].hide();

        }
    </script>
    <script type="text/javascript">

        var AllowInputNumber = function () {

            TxtSiren.setValue(TxtSiren.getValue().replace(/[^\d\d]/g, "")); //警号

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PoliceOfficer" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Hidden ID="Hidden1" runat="server" />
        <ext:Hidden ID="Hidden_id" runat="server" />
        <ext:Hidden ID="Hidden_panduan" runat="server" />
        <ext:Hidden ID="Hidden_name" runat="server" />
        <ext:Store ID="StoreRanks" runat="server">

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
        <ext:Store ID="StoreSex" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreFormat" runat="server">
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
        <ext:Store ID="StoreTeQin" runat="server">
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
        <ext:Store ID="StoreDutyType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <%--左侧--%>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="West" Split="true" Title='<%# GetLangStr("PoliceOfficer1","部门信息列表") %>'
                    Width="300" Icon="Table">
                    <Items>
                        <ext:TreePanel ID="TreePanel1" runat="server" Icon="Drive" Shadow="None" UseArrows="true"
                            AutoScroll="true" Animate="true" ContainerScroll="true" EnableDD="true" RootVisible="true"
                            Header="false" Height="600">
                        </ext:TreePanel>
                    </Items>
                </ext:FormPanel>
                <%--中间--%>
                <ext:FormPanel ID="Panel1" Region="Center" runat="server" Layout="FitLayout"
                    Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server" Layout="Container">
                            <Items>
                                <ext:Toolbar ID="Toolbar4" runat="server">
                                    <Items>
                                        <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("PoliceOfficer2","姓名：") %>' StyleSpec="margin-left: 10px; ">
                                        </ext:Label>
                                        <ext:TextField ID="TxtDeviceName" runat="server" Width="80" />
                                        <ext:Label ID="Label4" runat="server" Text='<%# GetLangStr("PoliceOfficer3","警号：") %>' StyleSpec="margin-left: 10px;">
                                        </ext:Label>
                                        <ext:TextField ID="TxTJingHao" runat="server" Width="60" MaxLength="7" />
                                        <ext:Label ID="Label1" runat="server" Hidden="true" Text='<%# GetLangStr("PoliceOfficer4","现职务：") %>' StyleSpec="margin-left: 10px; ">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbDutyType" runat="server" Editable="false" StoreID="StoreDutyType"
                                            DisplayField="CODEDESC" ValueField="CODE" TypeAhead="true" Mode="Local" ForceSelection="true"
                                            EmptyText='<%# GetLangStr("PoliceOfficer5","选择职务..") %>' SelectOnFocus="true" Width="80" Hidden="true">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PoliceOfficer66","清除选中") %>' AutoDataBind="true" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:Label ID="Label6" runat="server" Text='<%# GetLangStr("PoliceOfficer6","性别：") %>' StyleSpec="margin-left: 10px; ">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbSex" runat="server" Editable="false" StoreID="StoreSex" DisplayField="CODEDESC"
                                            ValueField="CODE" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PoliceOfficer7","选择性别..") %>'
                                            SelectOnFocus="true" Width="80">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PoliceOfficer66","清除选中") %>' AutoDataBind="true" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:Label ID="Label5" runat="server" Text='<%# GetLangStr("PoliceOfficer8","所属机关：") %>' StyleSpec="margin-left: 10px; ">
                                        </ext:Label>
                                        <ext:Panel ID="Panel2" runat="server" Height="30" Layout="AnchorLayout" Border="false">
                                            <Content>
                                                <dpart:UIDepartment ID="uiDepartment" runat="server" Width="180" ListWidth="300" />
                                            </Content>
                                        </ext:Panel>
                                        <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("PoliceOfficer9","查询") %>'>
                                            <DirectEvents>
                                                <Click OnEvent="TbutQueryClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%#GetLangStr("PoliceOfficer10","重置") %>'>
                                            <DirectEvents>
                                                <Click OnEvent="ButResetClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButRefresh" runat="server" Icon="Reload" Hidden="true" Text='<%# GetLangStr("PoliceOfficer11","刷新") %>'>
                                            <DirectEvents>
                                                <Click OnEvent="ButRefreshClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar5" runat="server">
                                    <Items>
                                        <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="10" />
                                        <ext:Button ID="ButDevAdd" runat="server" Text='<%# GetLangStr("PoliceOfficer12","新增") %>' Icon="Add">
                                            <DirectEvents>
                                                <Click OnEvent="ButDevAdd_Click" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButDevModify" runat="server" Text='<%# GetLangStr("PoliceOfficer13","修改") %>' Icon="Accept">
                                            <Listeners>
                                                <Click Handler="updata()" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="ButDevDelete" runat="server" Text='<%# GetLangStr("PoliceOfficer14","删除") %>' Icon="Delete">
                                            <Listeners>
                                                <Click Handler="delecte()" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:ToolbarFill />
                                        <ext:Button ID="ButExcel" runat="server" Text='<%# GetLangStr("PoliceOfficer15","导出Excel") %>' AutoPostBack="true" OnClick="ToExcel"
                                            Icon="PageExcel">
                                        </ext:Button>
                                        <%--  <ext:Button ID="ButPrint" runat="server" Icon="Printer" Text='<%# GetLangStr("PoliceOfficer16","打印") %>'>
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
                        <ext:GridPanel ID="GridDeviceManager" runat="server" StripeRows="true" 
                            AutoScroll="true" Header="false" ForceLayout="true">
                            <TopBar>
                                <ext:PagingToolbar HideRefresh="true" ID="Toolbar1" runat="server" PageSize="15">
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
                                    <ext:RowNumbererColumn Width="40" />
                                    <ext:Column Header='<%# GetLangStr("PoliceOfficer18","姓名") %>' AutoDataBind="true" DataIndex="col1" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("PoliceOfficer19","性别") %>' AutoDataBind="true" DataIndex="col2" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("PoliceOfficer20","警号") %>' AutoDataBind="true" DataIndex="col9" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("PoliceOfficer21","现职务") %>' AutoDataBind="true" DataIndex="col12" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("PoliceOfficer45","手台呼号") %>' AutoDataBind="true" DataIndex="col16" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("PoliceOfficer43","办公电话") %>' AutoDataBind="true" DataIndex="col7" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("PoliceOfficer24","所属机关") %>' AutoDataBind="true" DataIndex="col22" Width="200" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <Listeners>
                                <RowDblClick Handler="open()" />
                            </Listeners>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="false">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <View>
                                <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                                </ext:GridView>
                            </View>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
        <ext:Window ID="Window1" runat="server" Icon="House" Hidden="true" Height="450px"
            Width="600px" MonitorValid="true" Title='<%# GetLangStr("PoliceOfficer25","警务人员基本信息") %>'>
            <Items>
                <ext:FormPanel runat="server" ID="from" Icon="ImageAdd" MonitorValid="true" Frame="true"
                    Header="false" Collapsible="true" Collapsed="false" ButtonAlign="Center">
                    <Items>
                        <ext:Panel ID="Panel4" runat="server" Padding="10">
                            <Items>
                                <ext:Container ID="Container7" runat="server" Layout="Column" Height="320px">
                                    <Items>
                                        <ext:Container ID="Container8" runat="server" LabelAlign="Left" Layout="Form" ColumnWidth=".5">
                                            <Items>
                                                <ext:Panel ID="Panel3" runat="server" Height="30" Layout="AnchorLayout" Border="false">
                                                    <Content>
                                                        <dpart:UIDepartment ID="uiDepartment1" Editable="false" runat="server" Width="263" FieldLabel='<%# GetLangStr("PoliceOfficer26","所属单位") %>' ListWidth="300" />
                                                    </Content>
                                                </ext:Panel>
                                                <ext:ComboBox ID="CmbMale" runat="server" FieldLabel='<%# GetLangStr("PoliceOfficer27","性别") %>' AnchorHorizontal="95%"
                                                    Editable="false" StoreID="StoreSEX" DisplayField="CODEDESC" ValueField="CODE" TypeAhead="true"
                                                    Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PoliceOfficer28","选择性别...") %>' SelectOnFocus="true" AllowBlank="false">
                                                    <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PoliceOfficer66","清除选中") %>' AutoDataBind="true" />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.triggers[0].show();" />
                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                    </Listeners>
                                                </ext:ComboBox>
                                                <ext:TextField ID="TxtName" runat="server" FieldLabel='<%# GetLangStr("PoliceOfficer29","姓名") %>' AnchorHorizontal="95%"
                                                    AllowBlank="false" />
                                                <ext:DateField ID="DateField1" FieldLabel='<%# GetLangStr("PoliceOfficer30","出生日期") %>' runat="server" AnchorHorizontal="95%" />
                                                <ext:TextField ID="TxtAddress" runat="server" FieldLabel='<%# GetLangStr("PoliceOfficer31","家庭住址") %>' AnchorHorizontal="95%" />
                                                <ext:TextField ID="TxtIdNo" runat="server" FieldLabel='<%# GetLangStr("PoliceOfficer32","身份证号") %>' MaxLength="18" AnchorHorizontal="95%" />
                                                <ext:TextField ID="TxtSiren" EnableKeyEvents="true" runat="server" FieldLabel='<%# GetLangStr("PoliceOfficer33","警号") %>' MaxLength="7" AnchorHorizontal="95%" AllowBlank="false">
                                                    <Listeners>
                                                        <KeyUp Fn="AllowInputNumber" />
                                                    </Listeners>
                                                </ext:TextField>
                                                <ext:ComboBox ID="CmbRanks" runat="server" FieldLabel='<%# GetLangStr("PoliceOfficer34","警衔") %>' AnchorHorizontal="95%"
                                                    Editable="false" StoreID="StoreRanks" DisplayField="col2" ValueField="col1" TypeAhead="true"
                                                    Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PoliceOfficer35","请选择...") %>' SelectOnFocus="true">
                                                    <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PoliceOfficer66","清除选中") %>' AutoDataBind="true" />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.triggers[0].show();" />
                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                    </Listeners>
                                                </ext:ComboBox>
                                                <ext:ComboBox ID="CmbDuty" runat="server" FieldLabel='<%# GetLangStr("PoliceOfficer36","现职务") %>' AnchorHorizontal="95%"
                                                    Editable="false" StoreID="StoreDutyType" DisplayField="CODEDESC" ValueField="CODE"
                                                    TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PoliceOfficer35","请选择...") %>' SelectOnFocus="true">
                                                    <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PoliceOfficer66","清除选中") %>' AutoDataBind="true" />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.triggers[0].show();" />
                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                    </Listeners>
                                                </ext:ComboBox>
                                                <ext:ComboBox ID="CmbFormat" runat="server" FieldLabel='<%# GetLangStr("PoliceOfficer38","编制类别") %>' AnchorHorizontal="95%"
                                                    Editable="false" StoreID="StoreFormat" DisplayField="col2" ValueField="col1"
                                                    TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PoliceOfficer35","请选择...") %>' SelectOnFocus="true">
                                                    <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PoliceOfficer66","清除选中") %>' AutoDataBind="true" />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.triggers[0].show();" />
                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                    </Listeners>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:Container>
                                        <ext:Container ID="Container9" runat="server" LabelAlign="Left" Layout="Form" ColumnWidth=".5">
                                            <Items>
                                                <ext:ComboBox ID="CmbTeQin" runat="server" FieldLabel='<%# GetLangStr("PoliceOfficer40","特勤级别") %>' AnchorHorizontal="95%"
                                                    Editable="false" StoreID="StoreTeQin" DisplayField="col2" ValueField="col1" TypeAhead="true"
                                                    Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PoliceOfficer41","请选择...") %>' SelectOnFocus="true">
                                                    <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PoliceOfficer66","清除选中") %>' AutoDataBind="true" />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.triggers[0].show();" />
                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                    </Listeners>
                                                </ext:ComboBox>
                                                <ext:TextField ID="TxtPhone" runat="server" FieldLabel='<%# GetLangStr("PoliceOfficer42","手机") %>' AnchorHorizontal="95%"
                                                    MaxLength="11" />
                                                <ext:TextField ID="TxtOfficePhone" runat="server" FieldLabel='<%# GetLangStr("PoliceOfficer43","办公电话") %>' AnchorHorizontal="95%"
                                                    MaxLength="11" />
                                                <ext:TextField ID="TxtHandSets" runat="server" FieldLabel='<%# GetLangStr("PoliceOfficer44","手台")%>' AnchorHorizontal="95%"
                                                    MaxLength="11" />
                                                <ext:TextField ID="TxtHandSetsCode" runat="server" FieldLabel='<%# GetLangStr("PoliceOfficer45","手台呼号") %>' AnchorHorizontal="95%"
                                                    MaxLength="11" />
                                                <ext:TextField ID="TxtHandSetsGroup" runat="server" FieldLabel='<%# GetLangStr("PoliceOfficer46","手台组号") %>' AnchorHorizontal="95%"
                                                    MaxLength="11" />
                                                <ext:TextArea ID="TxtEqip" runat="server" AnchorHorizontal="95%" FieldLabel='<%# GetLangStr("PoliceOfficer47","警用装备") %>'
                                                    Height="100" />
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:Panel>
                    </Items>
                    <Listeners>
                        <ClientValidation Handler="ButUpdate.setDisabled(!valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button ID="ButUpdate" runat="server" Icon="Add">
                    <DirectEvents>
                        <Click OnEvent="GengXin_Click" />
                    </DirectEvents>
                </ext:Button>
                <ext:Button ID="Button5" runat="server" Icon="Cancel" Text='<%# GetLangStr("PoliceOfficer48","取消") %>'>
                    <Listeners>
                        <Click Handler="#{Window1}.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>
    </form>
</body>
</html>