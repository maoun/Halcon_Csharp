<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GisRoadLinkBrowse.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.GisRoadLinkBrowse" %>

<%@ Register Src="../UIDepartment.ascx" TagName="UIDepartment" TagPrefix="dpart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <link href="../Style/customMap.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body, html {
            font-family: Arial,Verdana;
            font-size: 13px;
            margin: 0;
            overflow: hidden;
        }
          #cboplate_Panel1 table{
            border-radius:0;
        }
        #map_canvas {
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            position: absolute;
        }
    </style>
    <script src="../Scripts/bmap.js" type="text/javascript" charset="UTF-8"></script>
    <script src="../Scripts/bmapFile.js" type="text/javascript" charset="UTF-8"></script>
    <script type="text/javascript">
        function MapCenter() {
            BMAP.MapInit();
            setTimeout(function () {
                BMAP.GotoCenter();
            }, 500);
        }
    </script>
</head>
<body onload="MapCenter();">
    <form id="form1" runat="server">
        <ext:Hidden ID="CheckData" runat="server" />
        <ext:Hidden ID="SaveFlag" runat="server" />
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="GisRoadLinkBrowse" />
        <ext:Store ID="RoadInfoWin" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="ROADID">
                    <Fields>
                        <ext:RecordField Name="ROADID" />
                        <ext:RecordField Name="DLMC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="RoadInfo" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="ROADID">
                    <Fields>
                        <ext:RecordField Name="ROADID" />
                        <ext:RecordField Name="DLMC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="Viewport1" runat="server" Layout="border">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Title="地图浏览" AutoRender="true" Region="Center"
                    Header="false" Height="800" Width="1000">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar12" runat="server" Cls="top-toolbar">
                            <Items>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="Linkreload" runat="server" Icon="Reload" Text="重载地图">
                                    <Listeners>
                                        <Click Handler="window.location.reload();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Button ID="TbutMove" runat="server" Icon="Erase" Text="清除">
                                    <Listeners>
                                        <Click Handler="BMAP.ClearCircle();BMAP.ClearLabel();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButCenetr" runat="server" Icon="ControlBlank" Text="中心点">
                                    <Listeners>
                                        <Click Handler="BMAP.GotoCenter();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButRanging" runat="server" Icon="PencilGo" Text="测距">
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
                <ext:FormPanel ID="FormPanel2" runat="server" Title="地图操作" Region="East" Split="true"
                    Collapsible="true" Collapsed="false" Width="410" Layout="Accordion" AutoScroll="true">
                    <TopBar>
                        <ext:Toolbar ID="toolbarrquery" runat="server">
                            <Items>
                                <ext:Panel ID="Pantrack" runat="server" Padding="10" Title="路段查询" Width="404" Icon="Zoom"
                                    Height="200" Layout="Form">
                                    <Items>
                                        <ext:Panel ID="Panel2" runat="server" Height="32" Layout="AnchorLayout" Border="false">
                                            <Content>
                                                <dpart:UIDepartment ID="uiDepartment" runat="server" FieldLabel="所属辖区" Width="375" />
                                            </Content>
                                        </ext:Panel>
                                        <ext:ComboBox runat="server" StoreID="RoadInfo" DisplayField="DLMC" ValueField="ROADID"
                                            ID="cboRoadInfo" Editable="false" BlankText="请选择" TypeAhead="true" Mode="Local"
                                            ForceSelection="true" SelectOnFocus="true" FieldLabel="所在道路" EmptyText="请选择所在道路..."
                                            AnchorHorizontal="98%">
                                        </ext:ComboBox>
                                        <ext:TextField runat="server" ID="txtRoadName" EmptyText="输入路段名称" FieldLabel="路段名称"
                                            AnchorHorizontal="98%">
                                        </ext:TextField>
                                    </Items>
                                    <Buttons>
                                        <ext:Button ID="butQuery" runat="server" Text="查询" Icon="ControlPlayBlue">
                                            <DirectEvents>
                                                <Click OnEvent="butQueryClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="butReset" runat="server" Text="重置" Icon="Reload">
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
                            Title="路段信息" Collapsible="true" AutoHeight="true" AutoScroll="true">
                            <Store>
                                <ext:Store ID="StoreInfo" runat="server" GroupField="col1">
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
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
                                        <ext:Button ID="ButAddgrid" runat="server" Text="新增" Icon="PageAdd">
                                            <Listeners>
                                                <Click Handler=" BMAP.ClearCircle();BMAP.AddRoadPoint({Operate:'AddRoadSegLine'});;" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column Header="所在道路" DataIndex="col1" Width="100" Align="Center" />
                                    <ext:Column Header="路段名称" DataIndex="col2" Width="150" Align="Center" />
                                    <ext:Column Header="路段方向" DataIndex="col3" Width="60" Align="Center" />
                                    <ext:CommandColumn Header="编辑" Width="36">
                                        <Commands>
                                            <ext:GridCommand Icon="NoteEdit" CommandName="Edit" />
                                        </Commands>
                                    </ext:CommandColumn>
                                    <ext:CommandColumn Header="删除" Width="36">
                                        <Commands>
                                            <ext:GridCommand Icon="Delete" CommandName="Delete" />
                                        </Commands>
                                    </ext:CommandColumn>
                                    <ext:CommandColumn Header="重绘" Width="36">
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
                            <View>
                                <ext:GroupingView ID="GroupingView1" runat="server" ForceFit="false" MarkDirty="false"
                                    ShowGroupName="false" EnableNoGroups="true" HideGroupedColumn="true" GroupByText="用该列进行分组"
                                    ShowGroupsText="显示分组" />
                            </View>
                            <LoadMask ShowMask="true" />
                            <Listeners>
                                <Command Handler="OnEvl.Editor(command,record.data)" />
                            </Listeners>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
        <ext:Window ID="win" Hidden="true" runat="server" Title="路段管理" Width="350" Height="200">
            <Items>
                <ext:FormPanel runat="server" ID="extForm" Padding="10" MonitorValid="true">
                    <Items>
                        <ext:TextField runat="server" ID="txtRoadSegId" FieldLabel="路段编号" Width="200" Disabled="true" />
                        <ext:TextField runat="server" ID="txtRoadSegName" FieldLabel="路段名称" Width="200" AllowBlank="false" />
                        <ext:ComboBox runat="server" StoreID="RoadInfoWin" DisplayField="DLMC" ValueField="ROADID"
                            ID="cmbRoadInfo" Editable="false" BlankText="请选择" TypeAhead="true" Mode="Local"
                            ForceSelection="true" SelectOnFocus="true" FieldLabel="所属道路" EmptyText="请选择所属道路..."
                            AllowBlank="false" Width="200" />
                        <ext:ComboBox runat="server" ID="cmbDirection" BlankText="请选择" FieldLabel="路段方向"
                            EmptyText="请选择路段方向..." Editable="false" TypeAhead="true"
                            Mode="Local" Width="200">
                            <Items>
                                <ext:ListItem Text="上行" Value="1" />
                                <ext:ListItem Text="下行" Value="0" />
                            </Items>
                        </ext:ComboBox>
                        <ext:Label ID="lab_message" runat="server" Text="" Width="200" />
                    </Items>
                    <Listeners>
                        <ClientValidation Handler="btnSave.setDisabled(!valid);" />
                    </Listeners>
                    <Buttons>
                        <ext:Button ID="btnSave" runat="server" Text="保存" Width="100">
                            <Listeners>
                                <Click Handler="OnEvl.SaveRoadInfoData()" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnCancle" runat="server" Text="取消" Width="100">
                            <Listeners>
                                <Click Handler="win.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>
