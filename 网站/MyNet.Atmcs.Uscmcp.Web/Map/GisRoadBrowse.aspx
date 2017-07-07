<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GisRoadBrowse.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.GisRoadBrowse" %>

<%@ Register Src="../UIDepartment.ascx" TagName="UIDepartment" TagPrefix="dpart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>道路管理</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Style/customMap.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body, html {
            font-family: Arial,Verdana;
            font-size: 13px;
            margin: 0;
            overflow: hidden;
        }

        #cboplate_Panel1 table {
            border-radius: 0;
        }

        #map_canvas {
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            position: absolute;
        }
    </style>
    <script type="text/javascript" src="../Scripts/bmapFile.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/bmap.js" charset="UTF-8"></script>
    <script type="text/javascript">
        function MapCenter() {
            BMAP.MapInit();
            setTimeout(function () {
                BMAP.GotoCenter();
            }, 500);
        }
    </script>
    <script language="javascript" type="text/javascript">
        function SelectMap(type) {
            BMAP.Clear();
            BGisBrowse.SelectMapTo(type);
        }
        function clearTime() {

            cboRoadType.triggers[0].hide();

        }
    </script>
    <!--梁引入如下js和css-->
    <script type="text/javascript" src="js/Qquery1.91-min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="js/jquery.nicescroll.js" charset="UTF-8"></script>
    <link rel="stylesheet" type="text/css" href="css/Ui-skin.css" />
    <!--梁引入如下js和css 结束-->
    <style type="text/css">
        body .ui-right-wrap .x-grid3-body .x-grid3-td-checker {
            background-image: none !important;
            background-image: none;
        }
    </style>

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
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridRoadManager.view.findRowIndex(this.triggerElement),
                cellIndex = GridRoadManager.view.findCellIndex(this.triggerElement),
                record = StoreInfo.getAt(rowIndex),
                fieldName = GridRoadManager.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body onload="MapCenter();">
    <form id="form1" runat="server" class="new-layout">
        <ext:Hidden ID="CheckData" runat="server" />
        <ext:Hidden ID="plateNo" runat="server" />
        <ext:Store ID="RoadType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Department" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="DEPARTID" />
                        <ext:RecordField Name="DEPARTNAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="GisRoadBrowse" />
        <ext:Store ID="StoreMarkType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreType" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="Viewport1" runat="server" Layout="border">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Title='<%# GetLangStr("GisRoadBrowse1","地图浏览") %>' AutoRender="true" Region="Center"
                    Header="false" Height="800" Width="1000" Cls="map-bg">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar12" runat="server" Cls="Top-Bar top-toolbar">
                            <Items>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="Linkreload" runat="server" Icon="Reload" Text='<%# GetLangStr("GisRoadBrowse2","重载地图") %>'>
                                    <Listeners>
                                        <Click Handler="window.location.reload();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Button ID="TbutMove" runat="server" Icon="Erase" Text='<%# GetLangStr("GisRoadBrowse3","清除") %>'>
                                    <Listeners>
                                        <Click Handler="BMAP.ClearCircle();BMAP.ClearLabel();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButCenetr" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("GisRoadBrowse4","中心点") %>'>
                                    <Listeners>
                                        <Click Handler="BMAP.GotoCenter();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButRanging" runat="server" Icon="PencilGo" Text='<%# GetLangStr("GisRoadBrowse5","测距") %>'>
                                    <Listeners>
                                        <Click Handler=" BMAP.DistanceTool();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Content>
                        <div style="width: 100%; height: 100%; border: 1px solid gray" id="map_canvas">
                        </div>
                    </Content>
                </ext:Panel>
                <ext:FormPanel ID="FormPanel2" Cls="ui-right-wrap Middle-arrow" runat="server" Region="East"
                    Split="true" Collapsible="true" Width="400" Layout="Accordion">
                    <TopBar>
                        <ext:Toolbar ID="toolbarrquery" runat="server">
                            <Items>
                                <ext:Panel ID="Pantrack" runat="server" Padding="4" Title="" Layout="Absolute" ButtonAlign="Center"
                                    Width="400" Height="160">
                                    <Items>
                                        <ext:ComboBox runat="server" StoreID="RoadType" X="15" Y="0" Width="300" DisplayField="CODEDESC" ValueField="CODE"
                                            ID="cboRoadType" Editable="false" BlankText='<%# GetLangStr("GisRoadBrowse7","请选择") %>' TypeAhead="true" Mode="Local"
                                            ForceSelection="true" SelectOnFocus="true" FieldLabel='<%# GetLangStr("GisRoadBrowse8","道路类型") %>'
                                            EmptyText='<%# GetLangStr("GisRoadBrowse9","请选择道路类型...") %>' AnchorHorizontal="98%" Cls="ui-input">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("GisRoadBrowse44","清除选中") %>' AutoDataBind="true" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>

                                        <ext:Panel ID="Panel2" runat="server" X="15" Y="35" Layout="AnchorLayout" Border="false">
                                            <Content>
                                                <dpart:UIDepartment ID="uiDepartment" runat="server" FieldLabel='<%# GetLangStr("GisRoadBrowse10","所属辖区") %>' Width="373" Cls="ui-input" />
                                            </Content>
                                        </ext:Panel>
                                        <ext:TextField runat="server" ID="txtRoadName" X="15" Y="70" EmptyText='<%# GetLangStr("GisRoadBrowse11","请输入道路名称...") %>' Width="300" FieldLabel='<%# GetLangStr("GisRoadBrowse12","道路名称") %>' AnchorHorizontal="98%" Cls="ui-input"></ext:TextField>
                                    </Items>
                                    <Buttons>
                                        <ext:Button ID="ButAddgrid" runat="server" Width="80" IconCls="ui-input w-80px search-btn border-radius-30" Text='<%# GetLangStr("GisRoadBrowse13","新增") %>'>
                                            <Listeners>
                                                <Click Handler=" BMAP.ClearCircle();BMAP.ClearLine();BMAP.AddRoadPoint({Operate:'AddRoadLine'});;" />
                                            </Listeners>
                                            <ToolTips>
                                                <ext:ToolTip runat="server" Title='<%# GetLangStr("GisRoadBrowse45","说明") %>' Html='<%# GetLangStr("GisRoadBrowse46","点击后，鼠标移至在左侧地图上开始绘制道路") %>' />
                                            </ToolTips>
                                        </ext:Button>
                                        <ext:Button ID="ButtonQuery" runat="server" Text='<%# GetLangStr("GisRoadBrowse14","查询") %>' Width="80" IconCls="ui-input w-80px search-btn border-radius-30">
                                            <DirectEvents>
                                                <Click OnEvent="butQueryClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButShuaxin" runat="server" Text='<%# GetLangStr("GisRoadBrowse15","重置") %>' Width="80" IconCls="ui-input w-80px search-btn border-radius-30">
                                            <DirectEvents>
                                                <Click OnEvent="butResetClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                    </Buttons>
                                </ext:Panel>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridRoadManager" runat="server" StripeRows="true" Icon="TextAlignJustify"
                            Title='<%# GetLangStr("GisRoadBrowse16","道路信息") %>'
                            Cls="data-list-container table-ui display-table w-100 Hide-panel-header">
                            <Store>
                                <ext:Store ID="StoreInfo" runat="server">
                                    <Reader>
                                        <ext:JsonReader IDProperty="ROADID">
                                            <Fields>
                                                <ext:RecordField Name="DLBH" />
                                                <ext:RecordField Name="DLMC" />
                                                <ext:RecordField Name="DLLX" />
                                                <ext:RecordField Name="SSXQ" />
                                                <ext:RecordField Name="ROADID" Type="String" />
                                                <ext:RecordField Name="ISMARK" />
                                                <ext:RecordField Name="DLLXNAME" />
                                                <ext:RecordField Name="GXSJ" Type="Date" />
                                                <ext:RecordField Name="SSXQNAME" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>

                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Header='<%# GetLangStr("GisRoadBrowse47","序号") %>' AutoDataBind="true" Width="40"></ext:RowNumbererColumn>
                                    <ext:Column Header='<%# GetLangStr("GisRoadBrowse16","道路类型") %>' AutoDataBind="true" DataIndex="DLLXNAME" Width="80" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("GisRoadBrowse17","道路名称") %>' AutoDataBind="true" DataIndex="DLMC" Width="130" Align="Center" />
                                    <ext:CommandColumn Header='<%#GetLangStr("GisRoadBrowse18","编辑") %>' AutoDataBind="true" Width="36">
                                        <Commands>
                                            <ext:GridCommand Icon="NoteEdit" CommandName="Edit" />
                                        </Commands>
                                    </ext:CommandColumn>
                                    <ext:CommandColumn Header='<%# GetLangStr("GisRoadBrowse19","删除") %>' AutoDataBind="true" Width="36">
                                        <Commands>
                                            <ext:GridCommand Icon="Delete" CommandName="Delete" />
                                        </Commands>
                                    </ext:CommandColumn>
                                    <ext:CommandColumn Header='<%# GetLangStr("GisRoadBrowse20","重绘") %>' AutoDataBind="true" Width="36">
                                        <Commands>
                                            <ext:GridCommand Icon="CommentEdit" CommandName="UpdateRoadLine" />
                                        </Commands>
                                    </ext:CommandColumn>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server">
                                    <DirectEvents>
                                        <RowDeselect OnEvent="ApplyClick" Buffer="250">
                                        </RowDeselect>
                                        <RowSelect OnEvent="ApplyClick" Buffer="250">
                                        </RowSelect>
                                    </DirectEvents>
                                </ext:CheckboxSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" />
                            <Listeners>
                                <Command Handler="OnEvl.Editor(command,record.data.ROADID,record.data.DLMC)" />
                            </Listeners>
                            <View>
                                <ext:GridView ForceFit="true" runat="server"></ext:GridView>
                            </View>
                            <ToolTips>
                                <ext:ToolTip
                                    ID="RowTip"
                                    runat="server"
                                    Target="={GridRoadManager.getView().mainBody}"
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
        <ext:Window ID="win" Hidden="true" runat="server" Title='<%# GetLangStr("GisRoadBrowse21","道路管理") %>' Width="380" Height="260">
            <Items>
                <ext:FormPanel runat="server" ID="extForm" Padding="10" MonitorValid="true">
                    <Items>
                        <ext:Panel ID="Panel1" runat="server" Height="25" Layout="AnchorLayout" Border="false">
                            <Content>
                                <dpart:UIDepartment ID="uiDepartment1" runat="server" FieldLabel='<%# GetLangStr("GisRoadBrowse10","所属辖区") %>' Width="305" ListWidth="300" />
                            </Content>
                        </ext:Panel>
                        <ext:TextField runat="server" ID="txtRoadIDwin" FieldLabel='<%# GetLangStr("GisRoadBrowse22","道路ID") %>' Width="200" Visible="false" />
                        <ext:TextField runat="server" ID="txtRoadNamewin" FieldLabel='<%# GetLangStr("GisRoadBrowse12","道路名称") %>' Width="200" AllowBlank="false" />
                        <ext:ComboBox runat="server" StoreID="RoadType" DisplayField="CODEDESC" ValueField="CODE"
                            ID="cboRoadTypewin" Editable="false" BlankText='<%# GetLangStr("GisRoadBrowse24","请选择") %>' TypeAhead="true" Mode="Local"
                            ForceSelection="true" SelectOnFocus="true" FieldLabel='<%# GetLangStr("GisRoadBrowse8","道路类型") %>' EmptyText='<%# GetLangStr("GisRoadBrowse9","请选择道路类型...") %>' AllowBlank="false"
                            Width="200">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("GisRoadBrowse44","清除选中") %>' AutoDataBind="true" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="this.triggers[0].show();" />
                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:TextField runat="server" ID="txtDLBHwin" FieldLabel='<%# GetLangStr("GisRoadBrowse28","道路编号") %>' EmptyText='<%# GetLangStr("GisRoadBrowse48","可选项") %>' Width="200" />
                        <ext:Label ID="lab_message" runat="server" Text="" Width="200" />
                    </Items>
                    <Listeners>
                        <ClientValidation Handler="btnSave.setDisabled(!valid);" />
                    </Listeners>
                    <Buttons>
                        <ext:Button ID="btnSave" runat="server" Text='<%# GetLangStr("GisRoadBrowse29","保存") %>' Width="100">
                            <DirectEvents>
                                <Click OnEvent="Save_Click" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="btnCancle" runat="server" Text='<%# GetLangStr("GisRoadBrowse30","取消") %>' Width="100">
                            <DirectEvents>
                                <Click OnEvent="Cancle_Click" />
                            </DirectEvents>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>