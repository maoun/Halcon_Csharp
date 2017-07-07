<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TGSStationManager.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.TGSStationManager" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>卡口电警管理</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Css/chooser.css" rel="stylesheet" type="text/css" />
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
            color: white;
        }

        .list-item2 {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: white;
        }

        .list-item h3 {
            display: block;
            font: inherit;
            font-weight: bold;
            color: white;
        }
    </style>
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" language="javascript">

        var employeeDetailsRender = function () {
            return '<img class="imgEdit" ext:qtip="点击查看详细信息" style="cursor:pointer;" src="../Images/vcard_edit.png"  />';
        };
        var getRowClass = function (record, rowIndex, p, ds) {
            var reColor = "";
            if (record.data.col15 == 0) {

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
        <ext:Hidden ID="HideServiceId" runat="server">
        </ext:Hidden>
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="TGSStationManager" />
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
        <ext:Store ID="StoreAppFlag" runat="server">
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
                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("TGSStationManager1","设备名称：") %>' StyleSpec="margin-left:10px;">
                                </ext:Label>
                                <ext:TextField ID="TxtDeviceName" runat="server" Width="100" />
                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("TGSStationManager2","设备类型：") %>' StyleSpec="margin-left:10px;">
                                </ext:Label>
                                <ext:ComboBox ID="CmbDeviceType" runat="server" Editable="false" StoreID="StoreDeviceType"
                                    DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                    EmptyText='<%# GetLangStr("TGSStationManager3","选择设备类型...") %>' SelectOnFocus="true" Width="173">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("TGSStationManager4","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("TGSStationManager5","重置") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButRefresh" runat="server" Icon="DriveGo"  Hidden="true"  Text='<%# GetLangStr("TGSStationManager6","刷新") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButRefreshClick" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridDeviceManager" runat="server" StripeRows="true"
                            Height="600" TrackMouseOver="true">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar2" runat="server" Layout="Container">
                                    <Items>
                                        <ext:Toolbar ID="Toolbar5" runat="server">
                                            <Items>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="20" />
                                                <ext:Button ID="ButDevAdd" runat="server" Text='<%# GetLangStr("TGSStationManager8","添加连接设备") %>' Icon="DriveAdd">
                                                    <DirectEvents>
                                                        <Click OnEvent="ButDevAdd_Click" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButDevModify" runat="server" Text='<%# GetLangStr("TGSStationManager9","修改信息") %>' Icon="DriveEdit">
                                                    <Listeners>
                                                        <Click Handler="TGSStationManager.Modify();" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Button ID="ButDevDelete" runat="server" Text='<%# GetLangStr("TGSStationManager10","删除连接设备") %>' Icon="DriveDelete">
                                                    <Listeners>
                                                        <Click Handler="TGSStationManager.DoConfirm()" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                        <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StoreDeviceManager">
                                            <Items>

                                                <ext:Button ID="ButExcel" runat="server" Text='<%# GetLangStr("TGSStationManager12","导出Excel") %>' AutoPostBack="true" OnClick="ToExcel"
                                                    Icon="PageExcel">
                                                </ext:Button>
                                            </Items>
                                        </ext:PagingToolbar>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Store>
                                <ext:Store ID="StoreDeviceManager" runat="server" OnRefreshData="MyData_Refresh"
                                    GroupField="col3">
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
                                    <ext:RowNumbererColumn Width="40" />
                                    <ext:Column Header='<%# GetLangStr("TGSStationManager14","监测点名称") %>' AutoDataBind="true" DataIndex="col3" Width="100" Align="Center" Hidden="true" />
                                    <ext:TemplateColumn DataIndex="" MenuDisabled="true" Header='<%# GetLangStr("TGSStationManager15","设备状态") %>' AutoDataBind="true" Width="80">
                                        <Template ID="Template1" runat="server">
                                            <Html>
                                                <tpl for=".">
                                                    <center>
							                            <img src="../images/state/normal.gif" />
                                                    </center>
						                        </tpl>
                                            </Html>
                                        </Template>
                                    </ext:TemplateColumn>
                                    <ext:Column Header='<%# GetLangStr("TGSStationManager16","所属方向") %>' AutoDataBind="true" DataIndex="col17" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("TGSStationManager17","设备名称") %>' AutoDataBind="true" DataIndex="col5" Width="200" Align="Left" />
                                    <ext:Column Header='<%# GetLangStr("TGSStationManager18","设备类型") %>' AutoDataBind="true" DataIndex="col19" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("TGSStationManager19","设备型号") %>' AutoDataBind="true" DataIndex="col22" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("TGSStationManager20","设备IP") %>' AutoDataBind="true" DataIndex="col6" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("TGSStationManager21","设备端口") %>' AutoDataBind="true" DataIndex="col7" Width="70" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("TGSStationManager22","设备通道") %>' AutoDataBind="true" DataIndex="col8" Width="70" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("TGSStationManager23","图片路径") %>' AutoDataBind="true" DataIndex="col9" Width="150" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("TGSStationManager24","外部编号") %>' AutoDataBind="true" DataIndex="col25" Width="150" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <View>
                                <ext:GroupingView ID="GroupingView1" runat="server" ForceFit="true" MarkDirty="false"
                                    ShowGroupName="false" EnableNoGroups="true" HideGroupedColumn="true" GroupByText='<%# GetLangStr("TGSStationManager25","用该列进行分组") %>'
                                    ShowGroupsText='<%# GetLangStr("TGSStationManager26","显示分组") %>'>
                                    <GetRowClass Fn="getRowClass" />
                                </ext:GroupingView>
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
                <ext:FormPanel ID="FormPanel1" runat="server" Region="West" Split="true" Title='<%# GetLangStr("TGSStationManager27","服务器信息列表") %>'
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
        <ext:Window ID="Window4" runat="server" Icon="House" Hidden="true" Height="500px"
            Width="420px" MonitorValid="true">
            <Items>
                <ext:FormPanel runat="server" ID="FormPanel3" MonitorValid="true" Padding="10">
                    <Items>
                        <ext:ComboBox ID="CmbStationType" runat="server" FieldLabel='<%# GetLangStr("TGSStationManager28","监测点类型") %>' AnchorHorizontal="100%"
                            Editable="false" StoreID="StoreStationType" DisplayField="col1" ValueField="col0"
                            TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("TGSStationManager29","选择监测点类型...") %>' SelectOnFocus="true">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:CompositeField ID="CompositeField1" runat="server" FieldLabel='<%# GetLangStr("TGSStationManager30","查询监测点") %>'>
                            <Items>
                                <ext:TextField ID="TxtQStationName" runat="server" EmptyText='<%# GetLangStr("TGSStationManager31","请输入查询监测点名称查询") %>' Width="200">
                                </ext:TextField>
                                <ext:Button ID="ButQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("TGSStationManager32","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButQueryClick" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:CompositeField>
                        <ext:ComboBox ID="CmbStation" runat="server" FieldLabel='<%# GetLangStr("TGSStationManager33","监测点名称") %>' AnchorHorizontal="100%"
                            Editable="false" StoreID="StoreStation" DisplayField="col3" ValueField="col1"
                            TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("TGSStationManager34","选择监测点...") %>' SelectOnFocus="true" TriggerAction="All" ItemSelector="div.list-item">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();TGSStationManager.SelectDevice();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                            <Template runat="server">
                                <Html>
                                    <tpl for=".">
						            <div class="list-item">
							             <h3>{col3}</h3>
                                         已绑定设备{col9}，已绑定方向{col10}
						            </div>
					             </tpl>
                                </Html>
                            </Template>
                        </ext:ComboBox>
                        <ext:ComboBox ID="CmbDevice" runat="server" FieldLabel='<%# GetLangStr("TGSStationManager35","连接的设备") %>' AnchorHorizontal="100%"
                            Editable="false" StoreID="StoreDevice" DisplayField="col1" ValueField="col0"
                            EmptyText='<%# GetLangStr("TGSStationManager36","选择连接设备...") %>' TriggerAction="All" TypeAhead="true" Mode="Local" ForceSelection="true" SelectOnFocus="true" ItemSelector="div.list-item2">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                            <Template ID="Template2" runat="server">
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
                        <ext:ComboBox ID="CmbDirection" runat="server" FieldLabel='<%# GetLangStr("TGSStationManager37","所属方向") %>' AnchorHorizontal="100%"
                            Editable="false" StoreID="StoreDirection" DisplayField="col2" ValueField="col0"
                            AllowBlank="false" EmptyText='<%# GetLangStr("TGSStationManager38","选择设备所属方向...") %>'>
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:NumberField ID="NumChannel" runat="server" FieldLabel='<%# GetLangStr("TGSStationManager39","设备通道号") %>' EmptyText='<%# GetLangStr("TGSStationManager40","例如：1") %>'
                            AllowBlank="false" AnchorHorizontal="100%" MaxLength="2">
                        </ext:NumberField>
                        <ext:ComboBox ID="CmbIsSacn" runat="server" FieldLabel='<%# GetLangStr("TGSStationManager41","是否文件扫描") %>' AnchorHorizontal="100%"
                            AllowBlank="false" Editable="false" StoreID="StoreisUse" DisplayField="col1"
                            ValueField="col0" EmptyText='<%# GetLangStr("TGSStationManager42","是文件扫描方式时有效") %>'>
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:TextField ID="TxtImagePath" runat="server" FieldLabel='<%# GetLangStr("TGSStationManager43","图片保存路径") %>' EmptyText="d:\capture\192.168.11.1"
                            AnchorHorizontal="100%">
                        </ext:TextField>
                        <ext:TextField ID="TxtIpaddress" runat="server" FieldLabel='<%# GetLangStr("TGSStationManager44","本地连接IP") %>' EmptyText='<%# GetLangStr("TGSStationManager45","192.168.11.1") %>'
                            AnchorHorizontal="100%">
                        </ext:TextField>
                        <ext:NumberField ID="NumPort" runat="server" FieldLabel="本地连接端口" EmptyText='<%# GetLangStr("TGSStationManager47","例如：8000") %>' MaxLength="5"
                            AnchorHorizontal="100%">
                        </ext:NumberField>
                        <ext:MultiCombo ID="MulCmbAppFlag" runat="server" FieldLabel='<%# GetLangStr("TGSStationManager48","使用特殊规则") %>' AnchorHorizontal="100%" Editable="false" StoreID="StoreAppFlag" DisplayField="col1"
                            ValueField="col0" EmptyText='<%# GetLangStr("TGSStationManager49","选择用标示...") %>'>
                        </ext:MultiCombo>
                        <ext:ComboBox ID="CmbIsuse" runat="server" FieldLabel='<%# GetLangStr("TGSStationManager50","是否启用") %>' AnchorHorizontal="100%"
                            AllowBlank="false" Editable="false" StoreID="StoreisUse" DisplayField="col1"
                            ValueField="col0" EmptyText='<%# GetLangStr("TGSStationManager51","选择是否启用...") %>'>
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
                <ext:Button ID="Button5" runat="server" Icon="Add" Text='<%# GetLangStr("TGSStationManager52","确定") %>'>
                    <DirectEvents>
                        <Click OnEvent="UpdateDevice" />
                    </DirectEvents>
                </ext:Button>
                <ext:Button ID="Button6" runat="server" Icon="TextItalic" Text='<%# GetLangStr("TGSStationManager53","取消") %>'>
                    <Listeners>
                        <Click Handler="#{Window4}.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>
    </form>
</body>
</html>