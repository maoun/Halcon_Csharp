<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapStation.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Map.MapStation" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>卡口选择</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="css/custom.css" />
    <link rel="stylesheet" type="text/css" href="../Style/customMap.css" />
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
    <style type="text/css">
        body .ui-right-wrap .x-grid3-body .x-grid3-td-checker {
            background-image: none !important;
            background-image: none;
        }
    </style>
    <script type="text/javascript" src="../Scripts/bmapFile.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/bmap.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/Heatmap_min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8"></script>
    <script type="text/javascript">
        //选中
        var setSelect = function (nodeName) {
            window.parent.setSelect(nodeName);
        }

        function MapCenter() {
            BMAP.MapInit();
            setTimeout(function () {
                BMAP.GotoCenter();
            }, 500);

            //判断浏览区是否支持canvas
            function isSupportCanvas() {
                var elem = document.createElement('canvas');
                return !!(elem.getContext && elem.getContext('2d'));
            }

        }
        //function Check() {
        //   var ckAll= document.getElementById("ckAll");
        //}
    </script>
    <!--梁引入如下js和css-->
    <script type="text/javascript" src="js/Qquery1.91-min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="js/jquery.nicescroll.js" charset="UTF-8"></script>
    <link rel="stylesheet" type="text/css" href="css/Ui-skin.css" />
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <!--梁引入如下js和css 结束-->
    <script type="text/javascript">
        function soundPlay(url) {
            var sound = document.createElement("bgsound");
            sound.id = "soun";
            document.body.appendChild(sound);
            sound.autostart = "false";
            sound.loop = "1";
            sound.src = url;
        }
        function CKAll(rowCount) {
            //选中行，
            for (var i = 0; i < rowCount; i++) {
                GridStation.getSelectionModel().selectRow(i);

            }
        }
    </script>
</head>
<body onload="MapCenter();">
    <form id="form2" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="MapStation" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="border" Cls="new-layout">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Title='<%# GetLangStr("MapStation1","地图浏览")%>' AutoRender="true" Region="Center"
                    Header="false" Height="800" Width="1000" Cls="map-bg">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar12" runat="server" Hidden="true">
                            <Items>
                                <ext:Button ID="Linkreload" runat="server" Icon="Reload" Text='<%# GetLangStr("MapStation2","重载地图")%>'>
                                    <Listeners>
                                        <Click Handler="window.location.reload();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Button ID="TbutMove" runat="server" Icon="Erase" Text='<%# GetLangStr("MapStation3","清除")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.ClearCircle();BMAP.ClearLabel();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButCenetr" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("MapStation4","中心点")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.GotoCenter();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButRanging" runat="server" Icon="PencilGo" Text='<%# GetLangStr("MapStation5","测距")%>'>
                                    <Listeners>
                                        <Click Handler=" BMAP.DistanceTool();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButPlan" runat="server" Icon="Vector" Text='<%# GetLangStr("MapStation6","测面积")%>'>
                                    <Listeners>
                                        <Click Handler=" BMAP.CalculateArea();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Content>
                        <div style="width: 100%; height: 100%; border: 1px solid gray" id="map_canvas">
                        </div>
                        <ext:Label ID="showms" runat="server"></ext:Label>
                    </Content>
                </ext:Panel>
                <ext:FormPanel ID="FormPanel2" Cls="ui-right-wrap-notop Middle-arrow" Padding="5" Region="east" runat="server"
                    Split="true" Collapsible="true" RowHeight="1" Width="260" Layout="Accordion">
                    <TopBar>
                        <ext:Toolbar ID="toolbarrquery" runat="server" Layout="ColumnLayout">
                            <Items>
                                <ext:LinkButton runat="server" IconCls="custom-iconbiao" Text="范围框选(在左侧地图上框选)" ToolTip="在左侧地图上框选" ColumnWidth=".8">
                                    <Listeners>
                                        <Click Handler=" BMAP.ClearCircle();BMAP.SaveAreaMarker({Operate:'MapStation'});" />
                                    </Listeners>
                                </ext:LinkButton>
                                <ext:Button runat="server" Text="保存" ColumnWidth=".2" StyleSpec="margin-top:0px;">
                                    <Listeners>
                                        <Click Handler="OnEvl.hidemap();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridStation" runat="server" StripeRows="true"
                            Collapsible="true" AutoHeight="false" AutoScroll="true" Cls="data-list-container table-ui display-table w-100 Hide-panel-header">
                            <Store>
                                <ext:Store ID="StoreInfo" runat="server">
                                    <Reader>
                                        <ext:JsonReader IDProperty="STATION_ID">
                                            <Fields>
                                                <ext:RecordField Name="STATION_ID" />
                                                <ext:RecordField Name="STATION_NAME" />
                                                <ext:RecordField Name="xpoint" />
                                                <ext:RecordField Name="ypoint" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <%--<ext:RowNumbererColumn Header="序号" Width="50" Align="Center" Editable="false" />--%>
                                    <ext:Column Header='<%# GetLangStr("MapStation8","卡口名称")%>' AutoDataBind="true" DataIndex="STATION_NAME" Align="Center" Editable="false" />
                                    <ext:Column Header='<%# GetLangStr("MapStation9","x")%>' AutoDataBind="true" DataIndex="xpoint" Hidden="true" Align="Center" Editable="false" />
                                    <ext:Column Header='<%# GetLangStr("MapStation10","y")%>' AutoDataBind="true" DataIndex="ypoint" Hidden="true" Align="Center" Editable="false" />
                                    <ext:Column ColumnID="ID" Header='<%# GetLangStr("MapStation11","卡口ID")%>' AutoDataBind="true" DataIndex="STATION_ID" Hidden="true" Align="Center" Editable="false" />
                                </Columns>
                            </ColumnModel>
                            <Listeners>
                                <GroupCommand Handler="if(command === 'SelectGroup'){ this.getSelectionModel().selectRecords(records, true); return;} Ext.Msg.alert(command, 'Group id: ' + groupId + '<br />Count - ' + records.length);" />
                            </Listeners>
                            <SelectionModel>

                                <ext:CheckboxSelectionModel runat="server" RowSpan="2" ID="ckAll" HideCheckAll="false">
                                    <DirectEvents>

                                        <AfterCheckAllClick OnEvent="AllCheck"></AfterCheckAllClick>
                                        <%--   <BeforeCheckAllClick OnEvent="AllCheck"></BeforeCheckAllClick>--%>
                                        <%--  <SelectionChange OnEvent="CheckChange"></SelectionChange>--%>
                                    </DirectEvents>
                                    <%-- <Listeners>
                                        <AfterCheckAllClick Handler="MapStation.SeleChange();" />

                                        <SelectionChange Handler="MapStation.SeleChange();" />
                                    </Listeners>--%>
                                </ext:CheckboxSelectionModel>
                            </SelectionModel>
                            <View>
                                <ext:GridView runat="server" ForceFit="true"></ext:GridView>
                            </View>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>

                <%--  <ext:Button runat="server" OnClick="CkAll" AutoPostBack="true" Text="全部选中">
                </ext:Button>--%>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

<script type="text/javascript">
    $(function () {

        $('.x-grid3-scroller').css({ 'background': 'red' });
    })
    $('.x-grid3-scroller').niceScroll({
        cursorcolor: "#7683a4",//#CC0071 光标颜色
        cursoropacitymax: 1, //改变不透明度非常光标处于活动状态（scrollabar“可见”状态），范围从1到0
        touchbehavior: false, //使光标拖动滚动像在台式电脑触摸设备
        cursorwidth: "4px", //像素光标的宽度
        cursorborder: "0", // 	游标边框css定义
        cursorborderradius: "5px",//以像素为光标边界半径
        autohidemode: false //是否隐藏滚动条
    });
    $('.OverCar-data-list').niceScroll({
        cursorcolor: "#7683a4",//#CC0071 光标颜色
        cursoropacitymax: 1, //改变不透明度非常光标处于活动状态（scrollabar“可见”状态），范围从1到0
        touchbehavior: false, //使光标拖动滚动像在台式电脑触摸设备
        cursorwidth: "3px", //像素光标的宽度
        cursorborder: "0", // 	游标边框css定义
        cursorborderradius: "5px",//以像素为光标边界半径
        autohidemode: false //是否隐藏滚动条
    });
</script>