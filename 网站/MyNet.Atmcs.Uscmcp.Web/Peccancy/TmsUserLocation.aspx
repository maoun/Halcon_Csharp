<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TmsUserLocation.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.TmsUserLocation" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <style type="text/css">
        .icon-combo-item {
            background-repeat: no-repeat !important;
            background-position: 3px 50% !important;
            padding-left: 24px !important;
        }

        .x-combo-list-item {
            border-color: #fff;
        }
    </style>
    <script type="text/javascript">
        var prepareCommands = function (grid, commands, record, row) {
            {
                commands.push({
                    command: record.get("col5"),
                    iconCls: 'icon-' + record.get("col5").toLowerCase()
                });
            }
        };
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridPanel1.view.findRowIndex(this.triggerElement),
                cellIndex = GridPanel1.view.findCellIndex(this.triggerElement),
                record = StoreContents.getAt(rowIndex),
                fieldName = GridPanel1.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="TmsUserLocation" />
        <ext:Hidden ID="realCount" runat="server" />
        <ext:Hidden ID="curpage" runat="server" />
        <ext:Hidden ID="allPage" runat="server" />
        <ext:Store ID="StoreIcon" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="IconName" />
                        <ext:RecordField Name="IconValue" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreForm" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
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
        <ext:Store ID="Store1" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col1">
                    <Fields>
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store2" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col1">
                    <Fields>
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <%--中部--%>
                <ext:Panel ID="Panel2" Region="Center" runat="server" Frame="true" DefaultAnchor="100%" Layout="FitLayout">
                    <Items>
                        <ext:GridPanel ID="GridPanel1" runat="server" StripeRows="true" AutoExpandColumn="col2" TrackMouseOver="true">
                            <TopBar>
                                <ext:Toolbar runat="server" Layout="Container">
                                    <Items>
                                        <ext:Toolbar runat="server">
                                            <Items>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10" />
                                                <ext:Button ID="ButFisrt" runat="server" StyleSpec="margin-left:10px;" Text='<%# GetLangStr("TmsUserLocation21","首页") %>' Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutFisrt" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButLast" runat="server" StyleSpec="margin-left:10px;" Icon="ControlRewindBlue" Text='<%# GetLangStr("TmsUserLocation22","上一页") %>' Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutLast" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButNext" runat="server" StyleSpec="margin-left:10px;" Icon="ControlFastforwardBlue" Text='<%# GetLangStr("TmsUserLocation23","下一页") %>'
                                                    Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutNext" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButEnd" runat="server" StyleSpec="margin-left:10px;" Text='<%# GetLangStr("TmsUserLocation24","尾页") %>' Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutEnd" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Label ID="lblTitle" runat="server" Text='<%# GetLangStr("TmsUserLocation25","查询结果：当前是第") %>' StyleSpec="margin-left:10px;" />
                                                <ext:Label ID="lblCurpage" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("TmsUserLocation26","页,共有") %>' />
                                                <ext:Label ID="lblAllpage" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label9" runat="server" Text='<%# GetLangStr("TmsUserLocation26","页,共有") %>' StyleSpec="font-weight:bolder;" />
                                                <ext:Label ID="lblRealcount" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label12" runat="server" Text='<%# GetLangStr("TmsUserLocation27","条记录") %>' />
                                            </Items>
                                        </ext:Toolbar>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Store>
                                <ext:Store ID="StoreContents" runat="server" OnRefreshData="StoreContents_Refresh">
                                    <AutoLoadParams>
                                        <ext:Parameter Name="start" Value="={0}" />
                                        <ext:Parameter Name="limit" Value="={10}" />
                                    </AutoLoadParams>
                                    <UpdateProxy>
                                        <ext:HttpWriteProxy Method="GET" Url="TmsUserLocation.aspx">
                                        </ext:HttpWriteProxy>
                                    </UpdateProxy>
                                    <Reader>
                                        <ext:JsonReader IDProperty="col1">
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
                                                <ext:RecordField Name="col16" />
                                                <ext:RecordField Name="col17" />
                                                <ext:RecordField Name="col18" />
                                                <ext:RecordField Name="col19" />
                                                <ext:RecordField Name="col20" />
                                                <ext:RecordField Name="col21" />
                                                <ext:RecordField Name="col22" />
                                                <ext:RecordField Name="col23" />
                                                <ext:RecordField Name="col24" />
                                                <ext:RecordField Name="col25" />
                                                <ext:RecordField Name="col26" />
                                                <ext:RecordField Name="col27" />
                                                <ext:RecordField Name="col28" Type="String" />
                                                <ext:RecordField Name="col29" />
                                                <ext:RecordField Name="col30" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                                    <ext:Column Header='<%# GetLangStr("TmsUserLocation3","用户编号") %>' AutoDataBind="true" DataIndex="col1" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("TmsUserLocation4","用户名") %>' AutoDataBind="true" DataIndex="col27" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("TmsUserLocation5","姓名") %>' AutoDataBind="true" DataIndex="col2" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("TmsUserLocation6","所属机构") %>' AutoDataBind="true" DataIndex="col29" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("TmsUserLocation7","注册时间") %>' AutoDataBind="true" DataIndex="col28" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("TmsUserLocation8","用户备注") %>' AutoDataBind="true" DataIndex="col26" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    <DirectEvents>
                                        <RowSelect OnEvent="RowSelect" Buffer="250">
                                            <ExtraParams>
                                                <ext:Parameter Name="data" Value="record.data" Mode="Raw" />
                                            </ExtraParams>
                                        </RowSelect>
                                    </DirectEvents>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" />
                            <View>
                                <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                                </ext:GridView>
                            </View>
                            <BottomBar>
                            </BottomBar>
                            <ToolTips>
                                <ext:ToolTip
                                    ID="RowTip"
                                    runat="server"
                                    Target="={GridPanel1.getView().mainBody}"
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
                <%--右侧--%>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Margins="0 5 5 5"
                    Frame="true" Title='<%# GetLangStr("TmsUserLocation9","违法审核地点权限分配") %>' Width="350" Icon="User" DefaultAnchor="100%"
                    Layout="Fit">
                    <Items>
                        <ext:TableLayout ID="TableLayout1" runat="server" Columns="1" AnchorHorizontal="100%">
                            <Cells>
                                <ext:Cell>
                                    <ext:Panel ID="Panel1" Title='<%# GetLangStr("TmsUserLocation10","用户的违法地点(鼠标拖动)") %>' runat="server" Border="false" Height="200"
                                        Width="340" Collapsible="true">
                                        <Items>
                                            <ext:MultiSelect ID="MultiSelect1" runat="server" DisplayField="col2" ValueField="col1"
                                                StoreID="Store1" DragGroup="grp1" DropGroup="grp2,grp1" AutoWidth="true" Height="180"
                                                KeepSelectionOnClick="WithCtrlKey">
                                            </ext:MultiSelect>
                                        </Items>
                                    </ext:Panel>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Panel ID="Panel3" Title='<%# GetLangStr("TmsUserLocation11","剩余的违法地点(鼠标拖动)") %>' runat="server" Border="false" Width="340"
                                        Collapsible="true" Layout="Fit">
                                        <Items>
                                            <ext:MultiSelect ID="MultiSelect2" runat="server" DisplayField="col2" ValueField="col1"
                                                StoreID="Store2" DragGroup="grp2" DropGroup="grp1,grp2" Height="300" AutoWidth="true"
                                                KeepSelectionOnClick="WithCtrlKey">
                                            </ext:MultiSelect>
                                        </Items>
                                    </ext:Panel>
                                </ext:Cell>
                            </Cells>
                        </ext:TableLayout>
                    </Items>
                    <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                                <ext:Radio ID="RadioOne" runat="server" BoxLabel='<%# GetLangStr("TmsUserLocation12","初审用户") %>' Checked="true">
                                    <Listeners>
                                        <Focus Handler="TmsUserLocation.TbutRadioOneClick()" />
                                    </Listeners>
                                </ext:Radio>
                                <ext:Radio ID="RadioTwo" runat="server" BoxLabel='<%# GetLangStr("TmsUserLocation13","复审用户") %>'>
                                    <Listeners>
                                        <Focus Handler="TmsUserLocation.TbuRadioTwoClick()" />
                                    </Listeners>
                                </ext:Radio>
                                <ext:Button ID="ButSaveFunc" runat="server" Text='<%# GetLangStr("TmsUserLocation14","保存") %>' Icon="TableEdit" ToolTip='<%# GetLangStr("TmsUserLocation14","保存") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="SubmitSelection">
                                            <ExtraParams>
                                                <ext:Parameter Name="multi1" Value="Ext.encode(#{MultiSelect1}.getValues())" Mode="Raw" />
                                            </ExtraParams>
                                            <EventMask Msg='<%# GetLangStr("TmsUserLocation15","处理中...") %>' AutoDataBind="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>