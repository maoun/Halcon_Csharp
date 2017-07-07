<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LocationUpload.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.LocationUpload" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>违法数据上传配置</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />

    <style type="text/css">
        .red-text {
            color: red;
            font-size: 12px;
        }

        .blue-text {
            color: blue;
            font-size: 12px;
        }
    </style>
    <script type="text/javascript">
        var saveData = function () {
            GridData.setValue(Ext.encode(GridPanel1.getRowsValues(false)));
        }
    </script>
    <script type="text/javascript">
        var departmentRenderer = function (value) {
            if (!Ext.isEmpty(value)) {
                return value;
            }

            return value;
        };
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridLocation.view.findRowIndex(this.triggerElement),
                cellIndex = GridLocation.view.findCellIndex(this.triggerElement),
                record = StoreLocationInfo.getAt(rowIndex),
                fieldName = GridLocation.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="LocationDirection" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Hidden ID="realCount" runat="server" />
        <ext:Hidden ID="curpage" runat="server" />
        <ext:Hidden ID="allPage" runat="server" />
        <ext:Hidden ID="CurrentSystemId" runat="server" />
        <ext:Store ID="StoreCombo" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreDirection" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreSystem" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="ViewPort1" runat="server">
            <Items>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <Center MarginsSummary="0 5 0 5">
                        <ext:Panel ID="Panel2" runat="server" Frame="true" Layout="Fit">
                            <Items>
                                <ext:GridPanel ID="GridLocation" runat="server" Height="250" StripeRows="true" TrackMouseOver="true">
                                    <TopBar>
                                        <ext:Toolbar runat="server" Layout="Container">
                                            <Items>
                                                <ext:Toolbar runat="server">
                                                    <Items>
                                                        <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10" />
                                                        <ext:Button ID="ButFisrt" runat="server" StyleSpec="margin-left:10px;" Text='<%# GetLangStr("LocationUpload12","首页")%>' Disabled="true">
                                                            <DirectEvents>
                                                                <Click OnEvent="TbutFisrt" />
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="ButLast" runat="server" StyleSpec="margin-left:10px;" Icon="ControlRewindBlue" Text='<%# GetLangStr("LocationUpload13","上一页")%>' Disabled="true">
                                                            <DirectEvents>
                                                                <Click OnEvent="TbutLast" />
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="ButNext" runat="server" StyleSpec="margin-left:10px;" Icon="ControlFastforwardBlue" Text='<%# GetLangStr("LocationUpload14","下一页")%>'
                                                            Disabled="true">
                                                            <DirectEvents>
                                                                <Click OnEvent="TbutNext" />
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="ButEnd" runat="server" StyleSpec="margin-left:10px;" Text='<%# GetLangStr("LocationUpload15","尾页")%>' Disabled="true">
                                                            <DirectEvents>
                                                                <Click OnEvent="TbutEnd" />
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Label ID="lblTitle" runat="server" Text='<%# GetLangStr("LocationUpload16","查询结果：当前是第")%>' StyleSpec="margin-left:10px;" />
                                                        <ext:Label ID="lblCurpage" runat="server" Text="" Cls="pageNumLabel" />
                                                        <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("LocationUpload17","页,共有")%>' />
                                                        <ext:Label ID="lblAllpage" runat="server" Text="" Cls="pageNumLabel" />
                                                        <ext:Label ID="Label9" runat="server" Text='<%# GetLangStr("LocationUpload17","页,共有")%>' StyleSpec="font-weight:bolder;" />
                                                        <ext:Label ID="lblRealcount" runat="server" Text="" Cls="pageNumLabel" />
                                                        <ext:Label ID="Label12" runat="server" Text='<%# GetLangStr("LocationUpload18","条记录")%>' />
                                                    </Items>
                                                </ext:Toolbar>
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <Store>
                                        <ext:Store ID="StoreLocationInfo" runat="server" OnRefreshData="MyData_Refresh">
                                            <Reader>
                                                <ext:JsonReader IDProperty="col0">
                                                    <Fields>
                                                        <ext:RecordField Name="col0" />
                                                        <ext:RecordField Name="col1" />
                                                        <ext:RecordField Name="col2" />
                                                        <ext:RecordField Name="col3" />
                                                        <ext:RecordField Name="col4" />
                                                        <ext:RecordField Name="col5" />
                                                        <ext:RecordField Name="col6" />
                                                        <ext:RecordField Name="col7" />
                                                        <ext:RecordField Name="col8" />
                                                        <ext:RecordField Name="col9" />
                                                        <ext:RecordField Name="col10" />
                                                        <ext:RecordField Name="col11" />
                                                        <ext:RecordField Name="col12" />
                                                        <ext:RecordField Name="col13" />
                                                        <ext:RecordField Name="col14" />
                                                        <ext:RecordField Name="col15" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <Plugins>
                                        <ext:RowEditor ID="RowEditor2" runat="server" SaveText='<%# GetLangStr("LocationUpload1","更新") %>' CancelText='<%# GetLangStr("LocationUpload2","退出") %>'>
                                            <DirectEvents>
                                                <AfterEdit OnEvent="UpdateLocationData" />
                                            </DirectEvents>
                                        </ext:RowEditor>
                                    </Plugins>
                                    <View>
                                        <ext:GridView ID="GridView2" runat="server" ForceFit="true" MarkDirty="false" />
                                    </View>
                                    <ColumnModel ID="ColumnModel3" runat="server">
                                        <Columns>
                                            <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                                            <ext:Column ColumnID="Id" DataIndex="col0" Width="140" Header='<%# GetLangStr("LocationUpload3","地点编号") %>' AutoDataBind="true" Align="Center" />
                                            <ext:Column DataIndex="col1" Width="250" Header='<%# GetLangStr("LocationUpload4","地点名称") %>' AutoDataBind="true">
                                                <Editor>
                                                    <ext:TextField ID="TxtLocationName" runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col15" Header='<%# GetLangStr("LocationUpload5","所属机构") %>' AutoDataBind="true" Width="240">
                                                <Renderer Fn="departmentRenderer" />
                                                <Editor>
                                                    <ext:ComboBox ID="CmbDepartment" runat="server" Shadow="Drop" Mode="Local" TriggerAction="All"
                                                        ForceSelection="true" StoreID="StoreCombo" DisplayField="col1" ValueField="col0"
                                                        SelectOnFocus="true">
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col4" Width="100" Header='<%# GetLangStr("LocationUpload6","路段代码") %>' AutoDataBind="true" Align="Center">
                                                <Editor>
                                                    <ext:TextField ID="TxtLocationSection" runat="server" MaxLength="5" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col5" Width="100" Header='<%# GetLangStr("LocationUpload7","道路代码") %>' AutoDataBind="true" Align="Center">
                                                <Editor>
                                                    <ext:TextField ID="TxtLocationRoad" runat="server" MaxLength="4" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col6" Width="100" Header='<%# GetLangStr("LocationUpload8","公里米数") %>' AutoDataBind="true" Align="Center">
                                                <Editor>
                                                    <ext:TextField ID="TxtLocationKiloMeter" runat="server" MaxLength="3" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col3" Width="100" Header='<%# GetLangStr("LocationUpload9","行政区划") %>' AutoDataBind="true" Align="Center">
                                                <Editor>
                                                    <ext:TextField ID="TxtAreaID" runat="server" MaxLength="6" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col7" Width="100" Header='<%# GetLangStr("LocationUpload10","执勤民警") %>' AutoDataBind="true" Align="Center">
                                                <Editor>
                                                    <ext:TextField ID="TxtLocationPolice" runat="server" MaxLength="6" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col8" Width="100" Header='<%# GetLangStr("LocationUpload11","设备编号") %>' AutoDataBind="true" Align="Center">
                                                <Editor>
                                                    <ext:TextField ID="TxtLocationDevice" runat="server" MaxLength="30" />
                                                </Editor>
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                            <DirectEvents>
                                                <RowSelect OnEvent="RowSelect" Buffer="250">
                                                    <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{Details}" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="id" Value="this.getSelected().id" Mode="Raw" />
                                                        <ext:Parameter Name="name" Value="record.data.col1" Mode="Raw" />
                                                    </ExtraParams>
                                                </RowSelect>
                                            </DirectEvents>
                                        </ext:RowSelectionModel>
                                    </SelectionModel>

                                    <LoadMask ShowMask="true" />
                                    <ToolTips>
                                        <ext:ToolTip
                                            ID="RowTip"
                                            runat="server"
                                            Target="={GridLocation.getView().mainBody}"
                                            Delegate=".x-grid3-cell"
                                            TrackMouse="true">
                                            <Listeners>
                                                <Show Fn="showTip" />
                                            </Listeners>
                                        </ext:ToolTip>
                                    </ToolTips>
                                </ext:GridPanel>
                            </Items>
                        </ext:Panel>
                    </Center>
                </ext:BorderLayout>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>