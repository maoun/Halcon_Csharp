<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviceOperation.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.DeviceOperation" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>设备运维信息</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8">></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8">></script>
    <script type="text/javascript" language="javascript">
        function luruxinxi() {
            var row = GridDeviceManager.getSelectionModel().getSelections();

            if (row[0] == null) {
                Ext.MessageBox.alert('提示', '请选择一条设备信息'); return false;
            }
            else {

                var id = row[0].data.col0.toString();
                OnEvl.Add_luru(id);

            }
        }
        /*更新方法*/
        function updata() {
            var row = GridPanel1.getSelectionModel().getSelections();
            if (row[0] == null) {
                Ext.MessageBox.alert('提示', '请选择一条运维信息'); return false;
            }
            else {

                var id = row[0].data.col0.toString();
                OnEvl.update(id);
            }
        }
        function open() {
            var row = GridPanel1.getSelectionModel().getSelections();
            if (row[0] == null) {
                Ext.MessageBox.alert('提示', '请选择一条设备信息'); return false;
            }
            else {
                var id = row[0].data.col0.toString();
                OnEvl.open_luru(id);
            }
        }
        function updatedevice() {
            var row = GridPanel1.getSelectionModel().getSelections();
            var id = row[0].data.col0.toString();
            OnEvl.updatedevice(id);
        }
        function delecte() {
            var row = GridPanel1.getSelectionModel().getSelections();

            if (row[0] == null) {
                Ext.MessageBox.alert('提示', '请选择需要处理记录'); return false;
            }
            else {

                var id = row[0].data.col0.toString();
                var codeid = row[0].data.col1.toString();
                var sqr = row[0].data.col2.toString();

                var sqdw = row[0].data.col6.toString();
               
                Ext.MessageBox.confirm("删除?", "确定删除该条数据?", function (code) {
                    if (code == "yes") {
                        OnDel.delecte(id, codeid,sqr, sqdw);
                    } else {
                        return false;
                    }
                });
            }
        }
        function clearTime() {

            CmbDeviceType.triggers[0].hide();

        }
        function setTime(start, end) {
            document.getElementById("start1").innerText = start;
            document.getElementById("end1").innerText = end;
        
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:Hidden ID="hidder" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="hidder1" runat="server" />
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="DeviceOperation" />
        <ext:Hidden ID="GridData" runat="server" />
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
        <ext:Store ID="StoreDepart" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
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
        <ext:Store ID="StoreDeviceName" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreDeviceDanWei" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreCardType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
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
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreNetworkType" runat="server">
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
                <ext:FormPanel ID="FormPanel1" runat="server" Region="West" Split="true" Title='<%# GetLangStr("DeviceOperation31","设备信息列表") %>' AutoDataBind="true"
                    Width="200" Icon="Table">
                    <Items>
                        <ext:TreePanel ID="TreePanel1" runat="server" Icon="Drive" Shadow="None" UseArrows="true"
                            AutoScroll="true" Animate="true" ContainerScroll="true" EnableDD="true" RootVisible="true"
                            Header="false" Height="600">
                        </ext:TreePanel>
                    </Items>
                </ext:FormPanel>

                <ext:FormPanel ID="Panel1" Region="Center" runat="server" AutoDataBind="true" Layout="RowLayout">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("DeviceOperation2","设备名称：") %>' StyleSpec="margin-left: 10px; ">
                                </ext:Label>
                                <ext:TextField ID="TxtDeviceName" runat="server" Width="100" />
                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("DeviceOperation3","设备类型：") %>' StyleSpec="margin-left: 10px; ">
                                </ext:Label>
                                <ext:ComboBox ID="CmbDeviceType" runat="server" Editable="false" StoreID="StoreDeviceType"
                                    DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                    EmptyText='<%# GetLangStr("DeviceOperation4","选择设备类型...") %>' SelectOnFocus="true" Width="123">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("DeviceOperation8","清除选中") %>' AutoDataBind="true" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("DeviceOperation5","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("DeviceOperation6","重置") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButRefresh" runat="server"  Hidden="true"  Icon="DriveGo" Text='<%# GetLangStr("DeviceOperation7","刷新") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButRefreshClick">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridDeviceManager" runat="server" StripeRows="true" RowHeight=".5" AutoScroll="true">
                            <TopBar>
                                <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="5" StoreID="StoreDeviceManager">
                                    <Items>
                                    </Items>
                                </ext:PagingToolbar>
                            </TopBar>
                            <Store>
                                <ext:Store ID="StoreDeviceManager" runat="server" OnRefreshData="MyData_Refresh">
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
                                    <ext:RowNumbererColumn Width="40" />
                                    <ext:Column Header='<%# GetLangStr("DeviceOperation10","设备编号") %>' AutoDataBind="true" DataIndex="col1" Width="100" Align="Left" Hidden="true" />
                                    <ext:Column Header='<%# GetLangStr("DeviceOperation11","设备名称") %>' AutoDataBind="true" DataIndex="col2" Width="200" Align="Left" />
                                    <ext:Column Header='<%# GetLangStr("DeviceOperation12","设备类型") %>' AutoDataBind="true" DataIndex="col27" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("DeviceOperation13","设备型号") %>' AutoDataBind="true" DataIndex="col26" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("DeviceOperation14","设备IP") %>' AutoDataBind="true" DataIndex="col9" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("DeviceOperation15","设备端口") %>' AutoDataBind="true" DataIndex="col10" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("DeviceOperation16","维护单位") %>' AutoDataBind="true" DataIndex="col24" Width="200" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <Listeners>
                                <RowDblClick Handler="open()" />
                            </Listeners>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="false">
                                    <DirectEvents>
                                        <RowSelect OnEvent="SelectLed" Buffer="250">
                                            <ExtraParams>
                                                <ext:Parameter Name="id" Value="record.data.col0" Mode="Raw" />
                                                  <ext:Parameter Name="sbName" Value="record.data.col2" Mode="Raw" />
                                            </ExtraParams>
                                        </RowSelect>
                                    </DirectEvents>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <View>
                                <ext:GridView ID="GroupingView1" runat="server" ForceFit="true">
                                </ext:GridView>
                            </View>
                        </ext:GridPanel>
                        <ext:GridPanel ID="GridPanel1" runat="server" StripeRows="true" AutoScroll="true"
                            RowHeight=".5">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar3" runat="server" Layout="Container">
                                    <Items>
                                        <ext:Toolbar ID="Toolbar2" runat="server">
                                            <Items>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10" />
                                                <ext:Button ID="ButDevAdd" runat="server" Text='<%# GetLangStr("DeviceOperation18","录入信息") %>' AutoDataBind="true" Icon="DriveAdd">
                                                    <Listeners>
                                                        <Click Handler="luruxinxi()" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Button ID="Button5" runat="server" Text='<%# GetLangStr("DeviceOperation19","修改信息") %>' AutoDataBind="true" Icon="DriveEdit">
                                                    <Listeners>
                                                        <Click Handler="updata()" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Button ID="Button6" runat="server" Text='<%# GetLangStr("DeviceOperation20","删除信息") %>' AutoDataBind="true" Icon="DriveDelete">
                                                    <Listeners>
                                                        <Click Handler="delecte()" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                        <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar2" runat="server" PageSize="5" StoreID="SroPenation">
                                            <Items>
                                                <ext:Toolbar ID="ToolExport" runat="server">
                                                    <Items>
                                                        <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                                        <%--     <ext:Button ID="Button1" runat="server" Text='<%# GetLangStr("DeviceOperation22","导出XML") %>' AutoPostBack="true" OnClick="ToXml"
                                                            Icon="PageCode">
                                                        </ext:Button>--%>
                                                        <ext:Button ID="Button2" runat="server" Text='<%# GetLangStr("DeviceOperation23","导出Excel") %>' AutoPostBack="true" OnClick="ToExcel"
                                                            Icon="PageExcel">
                                                        </ext:Button>
                                                        <%-- <ext:Button ID="Button3" runat="server" Text='<%# GetLangStr("DeviceOperation24","导出CSV") %>' AutoPostBack="true" OnClick="ToCsv"
                                                            Icon="PageAttach">
                                                        </ext:Button>--%>
                                                        <%--  <ext:Button ID="ButPrint" runat="server" Icon="Printer" Text='<%# GetLangStr("DeviceOperation25","打印") %>'>
                                                            <DirectEvents>
                                                                <Click OnEvent="ButPrintClick" />
                                                            </DirectEvents>
                                                        </ext:Button>--%>
                                                    </Items>
                                                </ext:Toolbar>
                                            </Items>
                                        </ext:PagingToolbar>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Store>
                                <ext:Store ID="SroPenation" runat="server" OnRefreshData="MyData_Openation">
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
                            <ColumnModel ID="ColumnModel2" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40" />
                                    <ext:Column Header='<%# GetLangStr("DeviceOperation34","维护方式") %>' AutoDataBind="true" DataIndex="col5" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("DeviceOperation27","申请人") %>'   AutoDataBind="true" DataIndex="col2" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("DeviceOperation28","申请单位") %>' AutoDataBind="true" DataIndex="col6" Width="150" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("DeviceOperation29","录入时间") %>' AutoDataBind="true" DataIndex="col3" Width="150" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("DeviceOperation30","维护说明") %>' AutoDataBind="true" DataIndex="col4" Width="250" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <Listeners>
                                <RowDblClick Handler="open()" />
                            </Listeners>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" SingleSelect="false">
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
        <ext:Window ID="Window1" runat="server" Icon="House" Title='<%# GetLangStr("DeviceOperation32","录入运维信息") %>' AutoDataBind="true" Hidden="true"
            Height="390px" Width="290px" MonitorValid="true">
            <Content>
                <ext:FormPanel ID="FormPanel2" runat="server" Padding="6" MonitorValid="true" Frame="true"
                    Height="340">
                    <Items>
                        <ext:ComboBox ID="com_operationtype" runat="server" Editable="false" StoreID="StoreDeviceName"
                            DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                            EmptyText='<%# GetLangStr("DeviceOperation33","选择运维方式...") %>' SelectOnFocus="true" AllowBlank="false" FieldLabel='<%# GetLangStr("DeviceOperation34","运维方式") %>'
                            AnchorHorizontal="95%">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("DeviceOperation8","清除选中") %>' AutoDataBind="true" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:ComboBox ID="com_operationdanwei" runat="server" Editable="false" StoreID="StoreDeviceDanWei"
                            DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                            EmptyText='<%# GetLangStr("DeviceOperation35","选择单位...") %>' SelectOnFocus="true" AllowBlank="false" FieldLabel='<%# GetLangStr("DeviceOperation36","申请单位") %>'
                            AnchorHorizontal="95%">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("DeviceOperation8","清除选中") %>' AutoDataBind="true" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:TextField ID="txt_lurupeople" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation27","申请人") %>' AnchorHorizontal="95%"
                            AllowBlank="false" />
                        <ext:CompositeField ID="CompositeField1" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation29","录入时间") %>' AnchorHorizontal="95%">
                            <Items>

                                <ext:Panel runat="server" BodyBorder="false">
                                    <Content>
                                        <li runat="server" class="laydate-icon" id="start" style="width: 120px; margin-left: 0px; float: left; list-style: none; cursor: pointer; height: 22px;"></li>
                                    </Content>
                                </ext:Panel>
                                <%--  <ext:DateField ID="date_lurutiem" runat="server" AllowBlank="false" Width="80" Format="yyyy-MM-dd" />
                                <ext:TimeField ID="time_luru" runat="server" Width="50" AllowBlank="false" Format="yyyy-MM-dd">
                                </ext:TimeField>--%>
                            </Items>
                        </ext:CompositeField>
                        <ext:TextField ID="txt_shuoming" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation39","录入说明") %>' AllowBlank="false"
                            AnchorHorizontal="95%" />
                        <ext:TextField ID="txt_shpeople" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation40","审核人") %>' AnchorHorizontal="95%"
                            AllowBlank="false" />
                        <ext:CompositeField ID="CompositeField2" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation41","审核时间") %>' AnchorHorizontal="95%">
                            <Items>
                                <ext:Panel runat="server" BodyBorder="false">
                                    <Content>
                                        <li runat="server" class="laydate-icon" id="end" style="width: 120px; margin-left: 0px; float: left; list-style: none; cursor: pointer; height: 22px;"></li>
                                    </Content>
                                </ext:Panel>
                            </Items>
                        </ext:CompositeField>
                        <ext:TextField ID="txt_shyijian" runat="server" AnchorHorizontal="95%" FieldLabel='<%# GetLangStr("DeviceOperation42","审核意见") %>'
                            AllowBlank="false" />
                    </Items>
                    <Listeners>
                        <ClientValidation Handler="butAdd1.setDisabled(!valid);" />
                    </Listeners>
                    <Buttons>
                        <ext:Button ID="butAdd1" runat="server" Icon="Add">
                            <DirectEvents>
                                <Click OnEvent="ButAdd_Click" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="Button4" runat="server" Icon="TextItalic" Text='<%# GetLangStr("DeviceOperation43","取消") %>'>
                            <Listeners>
                                <Click Handler="#{Window1}.hide();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Content>
        </ext:Window>
        <ext:Window ID="Window2" runat="server" Icon="House" Title='<%# GetLangStr("DeviceOperation44","查看运维信息") %>' AutoDataBind="true" Hidden="true"
            Height="390px" Width="290px" MonitorValid="true">
            <Content>
                <ext:FormPanel ID="FormPanel3" runat="server" Padding="6" MonitorValid="true" Frame="true"
                    Height="340">
                    <Items>
                        <ext:ComboBox ID="ComboBox2" runat="server" Editable="false" StoreID="StoreDeviceName"
                            DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                            EmptyText='<%# GetLangStr("DeviceOperation33","选择运维方式...") %>' AutoDataBind="true" SelectOnFocus="true" AllowBlank="false" FieldLabel='<%# GetLangStr("DeviceOperation34","运维方式") %>'
                            AnchorHorizontal="95%">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("DeviceOperation8","清除选中") %>' AutoDataBind="true" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:ComboBox ID="ComboBox3" runat="server" Editable="false" StoreID="StoreDeviceDanWei"
                            DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                            EmptyText='<%# GetLangStr("DeviceOperation47","选择单位...") %>' SelectOnFocus="true" AllowBlank="false" FieldLabel='<%# GetLangStr("DeviceOperation36","申请单位") %>'
                            AnchorHorizontal="95%">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("DeviceOperation8","清除选中") %>' AutoDataBind="true" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:TextField ID="TextField1" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation27","申请人") %>' AnchorHorizontal="95%"
                            AllowBlank="false" />
                       <%-- <ext:CompositeField ID="CompositeField3" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation29","录入时间") %>' AnchorHorizontal="95%">
                            <Items>
                                <ext:DateField ID="DateField1" runat="server" AllowBlank="false" Width="80" />
                                <ext:TimeField ID="TimeField1" runat="server" Width="50" AllowBlank="false">
                                </ext:TimeField>
                            </Items>
                        </ext:CompositeField>--%>


                          <ext:CompositeField ID="CompositeField3" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation29","录入时间") %>' AnchorHorizontal="95%">
                          <Items>
                                <ext:DateField ID="DateField1" runat="server" AllowBlank="false" Width="80" />
                                <ext:TimeField ID="TimeField1" runat="server" Width="50" AllowBlank="false">
                                </ext:TimeField>
                            </Items>
                        </ext:CompositeField>
                        <ext:TextField ID="TextField2" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation39","录入说明") %>' AllowBlank="false"
                            AnchorHorizontal="95%" />
                        <ext:TextField ID="TextField3" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation40","审核人") %>' AnchorHorizontal="95%"
                            AllowBlank="false" />

                        <ext:CompositeField ID="CompositeField4" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation41","审核时间") %>' AnchorHorizontal="95%">
                            <Items>
                                <ext:DateField ID="DateField2" runat="server" AllowBlank="false" Width="80" />
                                <ext:TimeField ID="TimeField2" runat="server" Width="50" AllowBlank="false">
                                </ext:TimeField>
                            </Items>
                        </ext:CompositeField>

                        <ext:TextField ID="TextField4" runat="server" AnchorHorizontal="95%" FieldLabel='<%# GetLangStr("DeviceOperation42","审核意见") %>'
                            AllowBlank="false" />
                    </Items>
                    <Buttons>
                        <ext:Button ID="Button8" runat="server" Icon="TextItalic" Text='<%# GetLangStr("DeviceOperation43","取消") %>'>
                            <Listeners>
                                <Click Handler="#{Window2}.hide();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Content>
        </ext:Window>
        <ext:Window ID="Window3" runat="server" Icon="House" Title='<%# GetLangStr("DeviceOperation56","修改运维信息") %>' AutoDataBind="true" Hidden="true"
            Height="390px" Width="290px" MonitorValid="true">
            <Content>
                <ext:FormPanel ID="FormPanel4" runat="server" Padding="6" MonitorValid="true" Frame="true"
                    Height="340">
                    <Items>
                        <ext:ComboBox ID="com_upte_lrtype" runat="server" Editable="false" StoreID="StoreDeviceName"
                            DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                            EmptyText='<%# GetLangStr("DeviceOperation33","选择运维方式...") %>' SelectOnFocus="true" AllowBlank="false" FieldLabel='<%# GetLangStr("DeviceOperation58","运维方式") %>'
                            AnchorHorizontal="95%">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("DeviceOperation8","清除选中") %>' AutoDataBind="true" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:ComboBox ID="com_upte_lrdanwei" runat="server" Editable="false" StoreID="StoreDeviceDanWei"
                            DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                            EmptyText='<%# GetLangStr("DeviceOperation59","选择单位...") %>' SelectOnFocus="true" AllowBlank="false" FieldLabel='<%# GetLangStr("DeviceOperation36","申请单位") %>'
                            AnchorHorizontal="95%">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("DeviceOperation8","清除选中") %>' AutoDataBind="true" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:TextField ID="txt_upde_lrpeople" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation27","申请人") %>' AnchorHorizontal="95%"
                            AllowBlank="false" />


                        <ext:CompositeField ID="CompositeField5" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation29","录入时间") %>' AnchorHorizontal="95%">
                            <Items>
                              <%--  <ext:DateField ID="date_upde_lrdate" runat="server" AllowBlank="false" Width="80" />
                                <ext:TimeField ID="time_upde_lrtime" runat="server" Width="50" AllowBlank="false">
                                </ext:TimeField>--%>
                                 <ext:Panel runat="server" BodyBorder="false">
                                    <Content>
                                        <li runat="server" class="laydate-icon" id="start1" style="width: 120px; margin-left: 0px; float: left; list-style: none; cursor: pointer; height: 22px;"></li>
                                    </Content>
                                </ext:Panel>
                            </Items>
                        </ext:CompositeField>




                        <ext:TextField ID="txt_upde_lrshuoming" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation39","录入说明") %>' AllowBlank="false"
                            AnchorHorizontal="95%" />
                        <ext:TextField ID="txt_upde_shpeople" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation40","审核人") %>' AnchorHorizontal="95%"
                            AllowBlank="false" />
                        <ext:CompositeField ID="CompositeField6" runat="server" FieldLabel='<%# GetLangStr("DeviceOperation41","审核时间") %>' AnchorHorizontal="95%">
                            <Items>
                              <%--  <ext:DateField ID="date_upde_shdate" runat="server" AllowBlank="false" Width="80" />
                                <ext:TimeField ID="time_upde_shtime" runat="server" Width="50" AllowBlank="false">
                                </ext:TimeField>--%>
                                 <ext:Panel runat="server" BodyBorder="false">
                                    <Content>
                                        <li runat="server" class="laydate-icon" id="end1" style="width: 120px; margin-left: 0px; float: left; list-style: none; cursor: pointer; height: 22px;"></li>
                                    </Content>
                                </ext:Panel>
                            </Items>
                        </ext:CompositeField>
                        <ext:TextField ID="txt_upde_shyijian" runat="server" AnchorHorizontal="95%" FieldLabel='<%# GetLangStr("DeviceOperation42","审核意见") %>'
                            AllowBlank="false" />
                    </Items>
                    <Listeners>
                        <ClientValidation Handler="btyupdate.setDisabled(!valid);" />
                    </Listeners>
                    <Buttons>
                        <ext:Button ID="btyupdate" runat="server" Text='<%# GetLangStr("DeviceOperation67","修改") %>' Icon="Add">
                            <Listeners>
                                <Click Handler="OnEvl.updatedeviceqw()" />
                            </Listeners>
                            <%--<DirectEvents>

                                <Click OnEvent="Unnamed_Event"  ></Click>
                            </DirectEvents>--%>
                        </ext:Button>
                        <ext:Button ID="Button9" runat="server" Icon="TextItalic" Text='<%# GetLangStr("DeviceOperation68","取消") %>'>
                            <Listeners>
                                <Click Handler="#{Window3}.hide();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Content>
        </ext:Window>
    </form>
</body>
</html>
<script type="text/javascript">
    laydate.skin('danlan');
    var start = {
        elem: '#start',
        format: 'YYYY-MM-DD hh:mm:ss',
        //min: laydate.now(), //设定最小日期为当前日期
        max: '2099-06-16 23:59:59', //最大日期
        istime: true,
        istoday: true,
        choose: function (datas) {
            //end.min = datas; //开始日选好后，重置结束日的最小日期
            //end.start = datas //将结束日的初始值设定为开始日
            //$("#end").click();//开始时间选中后，自动弹出结束时间
            var tt = $("#start").html();
            DeviceOperation.GetDateTime(true, tt);
            //alert(tt);
        }
    };
    var end = {
        elem: '#end',
        format: 'YYYY-MM-DD hh:mm:ss',
        min: laydate.now(),
        max: '2099-06-16 23:59:59',
        istime: true,
        istoday: true,
        choose: function (datas) {
            //start.max = datas; //结束日选好后，重置开始日的最大日期
            var tt = $("#end").html();
            DeviceOperation.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>



<script type="text/javascript">
    laydate.skin('danlan');
    var start1 = {
        elem: '#start1',
        format: 'YYYY-MM-DD hh:mm:ss',
        //min: laydate.now(), //设定最小日期为当前日期
        max: '2099-06-16 23:59:59', //最大日期
        istime: true,
        istoday: true,
        choose: function (datas) {
            //end.min = datas; //开始日选好后，重置结束日的最小日期
            //end.start = datas //将结束日的初始值设定为开始日
            //$("#end").click();//开始时间选中后，自动弹出结束时间
            var tt = $("#start1").html();
            DeviceOperation.GetDateTime1(true, tt);
            //alert(tt);
        }
    };
    var end1 = {
        elem: '#end1',
        format: 'YYYY-MM-DD hh:mm:ss',
        min: laydate.now(),
        max: '2099-06-16 23:59:59',
        istime: true,
        istoday: true,
        choose: function (datas) {
            //start.max = datas; //结束日选好后，重置开始日的最大日期
            var tt = $("#end1").html();
            DeviceOperation.GetDateTime1(false, tt);
        }
    };
    laydate(start1);
    laydate(end1);
</script>


