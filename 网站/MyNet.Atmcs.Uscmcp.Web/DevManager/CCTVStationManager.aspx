<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CCTVStationManager.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.CCTVStationManager" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>视频监控连接管理</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <style type="text/css">
        .x-grid-row-summary {
            color: #948d8e;
            text-decoration: line-through;
        }

        .list-item {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }

        .list-item2 {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }

        .list-item h3 {
            display: block;
            font: inherit;
            font-weight: bold;
            color: #222;
        }
    </style>
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" language="javascript">

        var employeeDetailsRender = function () {
            return '<img class="imgEdit" ext:qtip="点击查看详细信息" style="cursor:pointer;" src="../Images/vcard_edit.png"  />';
        };
        var getRowClass = function (record, rowIndex, p, ds) {
            var reColor = "";
            if (record.data.col30 == 0) {

                reColor = "x-grid-row-summary";
            }

            return reColor;
        };
        function clearTime() {
            CmbDeviceType.triggers[0].hide();

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
        <ext:Hidden ID="HideIpaddress" runat="server">
        </ext:Hidden>
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="CCTVStationManager" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Store ID="StoreDevice" runat="server">
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
        <ext:Store ID="StoreStationType" runat="server">
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
        <ext:Store ID="StoreStation" runat="server">
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
                        <ext:RecordField Name="col2" />
                        <ext:RecordField Name="col3" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreisUse" runat="server">
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
                <ext:FormPanel ID="Panel1" Region="Center" runat="server"
                    Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("CCTVStationManager2","设备名称：") %>' StyleSpec="margin-left:10px;">
                                </ext:Label>
                                <ext:TextField ID="TxtDeviceName" runat="server" Width="100" />
                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("CCTVStationManager3","设备类型：") %>'>
                                </ext:Label>
                                <ext:ComboBox ID="CmbDeviceType" runat="server" Editable="false" StoreID="StoreDeviceType"
                                    DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                    EmptyText='<%# GetLangStr("CCTVStationManager4","选择设备类型...") %>' SelectOnFocus="true" Width="173">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("CCTVStationManager5","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("CCTVStationManager6","重置") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButRefresh" runat="server" Icon="DriveGo" Hidden="true" Text='<%# GetLangStr("CCTVStationManager7","刷新") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButRefreshClick" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridDeviceManager" runat="server" StripeRows="true" Height="600">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar2" runat="server" Layout="Container">
                                    <Items>
                                        <ext:Toolbar ID="Toolbar5" runat="server">
                                            <Items>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="20" />
                                                <ext:Button ID="ButDevAdd" runat="server" Text='<%# GetLangStr("CCTVStationManager9","添加连接设备") %>' Icon="DriveAdd">
                                                    <DirectEvents>
                                                        <Click OnEvent="ButDevAdd_Click" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButDevModify" runat="server" Text='<%# GetLangStr("CCTVStationManager10","修改信息") %>' Icon="DriveEdit">
                                                    <Listeners>
                                                        <Click Handler="CCTVStationManager.Modify();" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Button ID="ButDevDelete" runat="server" Text='<%# GetLangStr("CCTVStationManager11","删除连接设备") %>' Icon="DriveDelete">
                                                    <Listeners>
                                                        <Click Handler="CCTVStationManager.DoConfirm()" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                        <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StoreDeviceManager">
                                            <Items>

                                                <ext:Toolbar ID="ToolExport" runat="server">
                                                    <Items>
                                                        <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                                        <ext:Button ID="Button2" runat="server" Text='<%# GetLangStr("CCTVStationManager13","导出Excel") %>' AutoPostBack="true" OnClick="ToExcel"
                                                            Icon="PageExcel">
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
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
                                                <ext:RecordField Name="col5" Type="Int" />
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
                                                <ext:RecordField Name="col18" Type="Int" />
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
                                                <ext:RecordField Name="col29" Type="String" />
                                                <ext:RecordField Name="col30" Type="String" />
                                                <ext:RecordField Name="col31" Type="String" />
                                                <ext:RecordField Name="col32" Type="String" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40" />
                                    <ext:Column Header='<%# GetLangStr("CCTVStationManager15","监测点名称") %>' AutoDataBind="true" DataIndex="col2" Width="150" Align="Left" />
                                    <ext:TemplateColumn DataIndex="" MenuDisabled="true" Header='<%# GetLangStr("CCTVStationManager16","设备状态") %>' AutoDataBind="true" Width="80">
                                        <Template ID="Template1" runat="server">
                                            <Html>
                                                <tpl for=".">
                                                    <center>
							                            <img src="../images/state/{col17}.gif" />
                                                    </center>
						                        </tpl>
                                            </Html>
                                        </Template>
                                    </ext:TemplateColumn>
                                    <ext:Column Header='<%# GetLangStr("CCTVStationManager17","所属方向") %>' AutoDataBind="true" DataIndex="col4" Width="80" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CCTVStationManager18","实时视频设备") %>' AutoDataBind="true" DataIndex="col7" Width="150" Align="Center" />
                                    <ext:Column Header="实时设备类型" DataIndex="col14" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CCTVStationManager20","实时设备IP") %>' AutoDataBind="true" DataIndex="col8" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CCTVStationManager21","实时通道") %>' AutoDataBind="true" DataIndex="col5" Width="80" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CCTVStationManager22","录像回放设备") %>' AutoDataBind="true" DataIndex="col20" Width="150" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CCTVStationManager23","回放设备类型") %>' AutoDataBind="true" DataIndex="col25" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CCTVStationManager24","回放设备IP") %>' AutoDataBind="true" DataIndex="col21" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CCTVStationManager25","回放通道")%>' AutoDataBind="true" DataIndex="col18" Width="80" Align="Center" />
                                </Columns>
                            </ColumnModel>
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
            </Items>
        </ext:Viewport>
        <ext:Window ID="Window4" runat="server" Icon="House" Hidden="true" Height="450px"
            Width="450px" MonitorValid="true">
            <Items>
                <ext:FormPanel runat="server" ID="FormPanel3" MonitorValid="true" Padding="10">
                    <Items>
                        <ext:ComboBox ID="CmbStationType" runat="server" FieldLabel='<%# GetLangStr("CCTVStationManager26","监测点类型") %>' AnchorHorizontal="100%"
                            Editable="false" StoreID="StoreStationType" DisplayField="col1"
                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("CCTVStationManager27","选择监测点类型...") %>'
                            SelectOnFocus="true">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:CompositeField ID="CompositeField1" runat="server" FieldLabel='<%# GetLangStr("CCTVStationManager28","查询监测点名称") %>'>
                            <Items>
                                <ext:TextField ID="TxtQStationName" runat="server" EmptyText='<%# GetLangStr("CCTVStationManager29","请输入查询监测点名称查询") %>'
                                    Width="200">
                                </ext:TextField>
                                <ext:Button ID="ButQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("CCTVStationManager30","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButQueryClick" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:CompositeField>
                        <ext:ComboBox ID="CmbStation" runat="server" FieldLabel='<%# GetLangStr("CCTVStationManager31","监测点名称") %>' AnchorHorizontal="100%"
                            Editable="false" StoreID="StoreStation" DisplayField="col3" ValueField="col1"
                            TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("CCTVStationManager32","选择监测点...") %>' SelectOnFocus="true" TriggerAction="All" ItemSelector="div.list-item">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();CCTVStationManager.SelectDevice();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                            <Template ID="Template2" runat="server">
                                <Html>
                                    <tpl for=".">
						            <div class="list-item">
							             <h3>{col3}</h3>
                                         已绑定设备{col10}，已绑定方向{col11}
						            </div>
					             </tpl>
                                </Html>
                            </Template>
                        </ext:ComboBox>
                        <ext:ComboBox ID="CmbDevice" runat="server" FieldLabel='<%# GetLangStr("CCTVStationManager33","实时监视设备") %>' AnchorHorizontal="100%"
                            Editable="false" StoreID="StoreDevice" DisplayField="col1" ValueField="col0"
                            EmptyText='<%# GetLangStr("CCTVStationManager34","选择连接监视设备...") %>' TriggerAction="All" ItemSelector="div.list-item2">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                            <Template ID="Template3" runat="server">
                                <Html>
                                    <tpl for=".">
						            <div class="list-item2">
							             <h3>{col1}</h3>
                                    {col3}-{col5},  IP:{col6}，端口:{col7}
						            </div>
					             </tpl>
                                </Html>
                            </Template>
                        </ext:ComboBox>
                        <ext:NumberField ID="MNumChannel" runat="server" FieldLabel='<%# GetLangStr("CCTVStationManager35","监控通道号") %>' EmptyText='<%# GetLangStr("CCTVStationManager36","例如：1") %>' AllowBlank="false"
                            AnchorHorizontal="100%">
                        </ext:NumberField>
                        <ext:ComboBox ID="CmbDeviceRecord" runat="server" FieldLabel='<%# GetLangStr("CCTVStationManager37","录像回放设备") %>' AnchorHorizontal="100%"
                            Editable="false" StoreID="StoreDevice" DisplayField="col1" ValueField="col0"
                            EmptyText='<%# GetLangStr("CCTVStationManager38","选择连接录像回放设备...") %>' TriggerAction="All" ItemSelector="div.list-item2">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                            <Template ID="Template4" runat="server">
                                <Html>
                                    <tpl for=".">
						            <div class="list-item2">
							              <h3>{col1}</h3>
                                    {col3}-{col5},  IP:{col6}，端口:{col7}
						            </div>
					             </tpl>
                                </Html>
                            </Template>
                        </ext:ComboBox>
                        <ext:NumberField ID="RNumChannel" runat="server" FieldLabel='<%# GetLangStr("CCTVStationManager39","回放通道号") %>' EmptyText='<%# GetLangStr("CCTVStationManager40","例如：1") %>' AllowBlank="false"
                            AnchorHorizontal="100%">
                        </ext:NumberField>
                        <ext:ComboBox ID="CmbDirection" runat="server" FieldLabel='<%# GetLangStr("CCTVStationManager80","所属方向") %>' AnchorHorizontal="100%"
                            Editable="false" StoreID="StoreDirection" DisplayField="col2" ValueField="col0" AllowBlank="false" EmptyText='<%# GetLangStr("CCTVStationManager41","选择设备所属方向...") %>'>
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:TextField ID="TxtMasterId" runat="server" FieldLabel='<%# GetLangStr("CCTVStationManager42","矩阵编号") %>' MaxLength="4" EmptyText='<%# GetLangStr("CCTVStationManager43","例如：9999") %>'
                            AnchorHorizontal="100%">
                        </ext:TextField>
                        <ext:ComboBox ID="CmbIsuse" runat="server" FieldLabel='<%# GetLangStr("CCTVStationManager44","是否启用") %>' AnchorHorizontal="100%"
                            AllowBlank="false" Editable="false" StoreID="StoreisUse" DisplayField="col1" ValueField="col0" EmptyText='<%# GetLangStr("CCTVStationManager45","选择是否启用...") %>'>
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                        </ext:ComboBox>
                    </Items>
                    <Listeners>
                        <ClientValidation Handler="Button5.setDisabled(!valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button ID="Button5" runat="server" Icon="Add" Text='<%# GetLangStr("CCTVStationManager46","确定") %>'>
                    <DirectEvents>
                        <Click OnEvent="UpdateDevice" />
                    </DirectEvents>
                </ext:Button>
                <ext:Button ID="Button6" runat="server" Icon="TextItalic" Text='<%# GetLangStr("CCTVStationManager47","取消") %>'>
                    <Listeners>
                        <Click Handler="#{Window4}.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>
    </form>
</body>
</html>