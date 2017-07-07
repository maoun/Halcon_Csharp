<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExtraListManager.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.ExtraListManager" %>
<%@ Register Src="../UIDepartment.ascx" TagName="UIDepartment" TagPrefix="dpart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>重点车辆管理</title>
    <link href="Css/chooser.css" rel="stylesheet" type="text/css" />
     <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <script language="javascript" src="Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        var template = '<span style="color:{0};">{1}</span>';
        var change = function (value) {
            return String.format(template, (value == "比对") ? "green" : "red", value);
        };
        var changetime = function (value) {
            var mydate = Ext.util.Format.date(new Date(), 'Y-m-d H:i:s');
            return String.format(template, (value > mydate) ? "green" : "red", value);
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="ExtraListManager" />
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
                            <ext:Label ID="Label3" runat="server" Html="<font>&nbsp;&nbsp;车牌号牌：</font>">
                            </ext:Label>
                            <ext:TextField ID="TxtplateId" runat="server" Width="100"  EmptyText="六位号牌号码" />
                            <ext:Label ID="Label1" runat="server" Html="<font>&nbsp;&nbsp;号牌种类：</font>">
                            </ext:Label>
                            <ext:ComboBox ID="CmbPlateType" runat="server" Editable="false" StoreID="StorePlateType"
                                DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                EmptyText="请选择..." SelectOnFocus="true" Width="123"> <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.triggers[0].show();" />
                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                    </Listeners>
                            </ext:ComboBox>
                            <ext:Label ID="Label5" runat="server" Html="<font>&nbsp;&nbsp;比对类型：</font>">
                            </ext:Label>
                            <ext:ComboBox ID="CmbQueryMdlx" runat="server" Editable="false" StoreID="StoreMdlx"
                                DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                EmptyText="选择比对类型..." SelectOnFocus="true" Width="123">
                            </ext:ComboBox>
                            <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text="查询">
                                <DirectEvents>
                                       <Click OnEvent="TbutQueryClick"  Timeout="60000" >
                                                    <EventMask  ShowMask="true"/>
                                            </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text="重置">
                                        <DirectEvents>
                                            <Click OnEvent="ButResetClick" />
                                        </DirectEvents>
                                    </ext:Button>
                            <ext:Button ID="ButRefresh" runat="server"  Hidden="true"  Icon="DriveGo" Text="刷新">
                                <DirectEvents>
                                    <Click OnEvent="ButRefreshClick" />
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:FormPanel>
            <ext:GridPanel ID="GridExtraList" Region="Center" runat="server" StripeRows="true"
                Title="比对信息" Collapsible="true" >
                <Store>
                    <ext:Store ID="StoreExtraList" runat="server" OnRefreshData="MyData_Refresh">
                     <AutoLoadParams>
                            <ext:Parameter Name="start" Value="={0}" />
                            <ext:Parameter Name="limit" Value="={15}" />
                        </AutoLoadParams>
                        <UpdateProxy>
                            <ext:HttpWriteProxy Method="GET" Url="ExtraListManager.aspx">
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
                                    <ext:RecordField Name="col8" Type="String" />
                                    <ext:RecordField Name="col9" Type="String" />
                                    <ext:RecordField Name="col10" Type="String" />
                                    <ext:RecordField Name="col11" Type="String" />
                                    <ext:RecordField Name="col12" Type="String" />
                                    <ext:RecordField Name="col13" Type="String" />
                                    <ext:RecordField Name="col14" Type="String" />
                                    <ext:RecordField Name="col15" Type="Date" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel ID="ColumnModel1" runat="server">
                    <Columns>
                        <ext:Column Header="比对编号" DataIndex="col0" Width="0" Hidden="true" />
                        <ext:Column Header="号牌号码" DataIndex="col1" Width="90" />
                        <ext:Column Header="号牌种类" DataIndex="col3" Width="110" />
                        <ext:Column Header="车身颜色" DataIndex="col4" Width="80" />
                        <ext:Column Header="车辆品牌" DataIndex="col5" Width="80" />
                        <ext:Column Header="有效时间" DataIndex="col8" Width="160">
                            <Renderer Fn="changetime" />
                        </ext:Column>
                        <ext:Column Header="比对类型" DataIndex="col7" Width="80" />
                        <ext:Column Header="比对标识" DataIndex="col13" Width="80">
                            <Renderer Fn="change" />
                        </ext:Column>
                        <ext:DateColumn Header="更新时间" DataIndex="col15" Width="160" Format="yyyy-MM-dd HH:mm:ss" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                        <DirectEvents>
                            <RowSelect OnEvent="SelectExtraList" Buffer="250">
                                <ExtraParams>
                                    <ext:Parameter Name="sdata" Value="record.data" Mode="Raw" />
                                </ExtraParams>
                            </RowSelect>
                        </DirectEvents>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <TopBar>
                    <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StoreExtraList">
                        <Items>
                            <ext:Label ID="Label2" runat="server" Text="页大小" />
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
                            <ext:Button ID="ButXml" runat="server" Text="导出XML" AutoPostBack="true" OnClick="ToXml"
                                        Icon="PageCode">
                                    </ext:Button>
                                    <ext:Button ID="ButExcel" runat="server" Text="导出Excel" AutoPostBack="true" OnClick="ToExcel"
                                        Icon="PageExcel">
                                    </ext:Button>
                                    <ext:Button ID="ButCsv" runat="server" Text="导出CSV" AutoPostBack="true" OnClick="ToCsv"
                                        Icon="PageAttach">
                                    </ext:Button>
                                    <ext:Button ID="ButPrint" runat="server" Icon="Printer" Text="打印">
                                        <DirectEvents>
                                            <Click OnEvent="ButPrintClick" />
                                        </DirectEvents>
                                    </ext:Button>
                        </Items>
                    </ext:PagingToolbar>
                </TopBar>
                <View>
                            <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                            </ext:GridView>
                        </View>
            </ext:GridPanel>
            <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Frame="true"
                Title="详细信息" Width="300" Icon="Table" DefaultAnchor="100%" MonitorValid="true"  AutoScroll="true">
                <Items>
                    <ext:Panel ID="Panel9" runat="server" Header="false" Title="车辆信息" DefaultAnchor="100%"  Padding="5">
                        <Items>
                            <ext:TextField runat="server" FieldLabel="比对编号" ID="TxtID" Width="280" ReadOnly="true" />
                            <ext:TextField runat="server" FieldLabel="号牌号码" ID="txtHphm" AllowBlank="false" Width="280" />
                            <ext:ComboBox runat="server" FieldLabel="号牌种类" ID="cmbHpzl" StoreID="StorePlateType"
                                Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                EmptyText="选择号牌种类" Width="280" AllowBlank="false" />
                            <ext:ComboBox runat="server" FieldLabel="车身颜色" ID="cmbCsys" StoreID="StoreColor"
                                Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                EmptyText="选择车身颜色" Width="280" />
                            <ext:TextField runat="server" FieldLabel="车辆品牌" ID="txtClpp" Width="280" />
                            <ext:ComboBox runat="server" FieldLabel="比对类型" ID="cmbMdlx" StoreID="StoreMdlx" Editable="false"
                                DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All" EmptyText="选择比对类型"
                                Width="280" AllowBlank="false" />
                            <ext:CompositeField ID="CompositeField1" runat="server" FieldLabel="有效时间" Width="280">
                                <Items>
                                    <ext:DateField ID="DateYxsj" runat="server" Vtype="daterange" AllowBlank="false">
                                    </ext:DateField>
                                    <ext:TimeField ID="TimeYxsj" runat="server" Increment="1" Width="77" />
                                </Items>
                            </ext:CompositeField>
                            <ext:TextField runat="server" FieldLabel="比对原因" ID="txtSjyy" Width="280" />
                            <ext:Panel ID="Panel2" runat="server" Height="32"  Layout="AnchorLayout" Border="false">
                                <Content>
                                   <dpart:UIDepartment id="uiDepartment" runat="server" FieldLabel="数据来源"  Width="278"  ListWidth="300"  />
                                 </Content>
                           </ext:Panel>
                            <ext:ComboBox ID="cmbbdbj" FieldLabel="比对标识" runat="server" StoreID="StoreBdbj" Editable="false"
                                DisplayField="col1" ValueField="col0" Mode="Local" EmptyText="选择比对或者撤销" Width="280"
                                AllowBlank="false">
                            </ext:ComboBox>
                            <ext:TextField runat="server" FieldLabel="备注信息" ID="TxtBz" Width="280"/>
                            <ext:TextField runat="server" FieldLabel="更新时间" ID="TxtGxsj" Width="280" ReadOnly="true" />
                        </Items>
                    </ext:Panel>
                </Items>
               <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                    <ext:Button ID="ButAdd" runat="server" Text="增加" Icon="Add" ToolTip="增加">
                        <Listeners>
                            <Click Handler="ExtraListManager.InfoSave()" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="ButUpdate" runat="server" Text="保存" Icon="TableSave">
                        <Listeners>
                            <Click Handler="ExtraListManager.UpdateData()" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="ButDelete" runat="server" Text="删除" Icon="Delete">
                        <Listeners>
                            <Click Handler="ExtraListManager.DoConfirm()" />
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
