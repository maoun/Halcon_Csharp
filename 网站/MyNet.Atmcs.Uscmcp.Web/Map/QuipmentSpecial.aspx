<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuipmentSpecial.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Map.QuipmentSpecial" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>设备专题</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="../Map/css/custom.css" />
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

        .x-form-cb-label {
            position: relative;
            top: 13px !important;
            left: 20px !important;
        }
    </style>
    <style type="text/css">
        .x-form-cb-label {
            text-indent: 16px;
            margin-left: 60px !important;
            width: 103px;
            height: 50px;
            display: block;
            text-align: center;
            line-height: 45px;
            vertical-align: middle;
        }

        .x-form-check-wrap input {
            vertical-align: middle;
            float: left;
            text-align: center;
            margin-top: 7px;
            margin: 12px 0 0 15px;
        }

        .ui-img-tms {
            margin-top: 30px;
            background-image: url(../Images/Mapimg/tms.png);
            background-repeat: no-repeat;
            background-position: 40px 0px;
            background-size: 45px 45px;
        }

        .ui-img-tgs {
            margin-top: 30px;
            background-image: url(../Images/Mapimg/tgs.png);
            background-repeat: no-repeat;
            background-position: 40px 0px;
            background-size: 45px 45px;
        }

        .ui-img-tcs {
            margin-top: 30px;
            background-image: url(../Images/Mapimg/tcs.png);
            background-repeat: no-repeat;
            background-position: 40px 0px;
            background-size: 45px 45px;
        }

        .ui-img-cctv {
            margin-top: 30px;
            background-image: url(../Images/Mapimg/cctv.png);
            background-repeat: no-repeat;
            background-position: 40px 0px;
            background-size: 45px 45px;
        }

        .x-form-cb-label:first-child {
            background-image: url(../imgs/40.png);
        }

        .BMapLib_sendToPhone {
            display: none;
        }
    </style>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/bmap.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/bmapFile.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/Heatmap_min.js" charset="UTF-8"></script>
    <script type="text/javascript">
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
    </script>
    <!--梁引入如下js和css-->
    <script type="text/javascript" src="js/Qquery1.91-min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="js/jquery.nicescroll.js" charset="UTF-8"></script>
    <link rel="stylesheet" type="text/css" href="css/Ui-skin.css" />
    <link rel="stylesheet" type="text/css" href="css/input.css" />
    <!--梁引入如下js和css 结束-->
</head>
<body onload="MapCenter();" class="new-layout">
    <form id="form2" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="QuipmentSpecial" />
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
        <ext:Viewport ID="Viewport1" runat="server" Layout="border">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Title='<%# GetLangStr("QuipmentSpecial1","地图浏览")%>' AutoRender="true" Region="Center"
                    Header="false" Height="800" Width="1000" Cls="map-bg">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar12" runat="server" Cls="top-toolbar">
                            <Items>
                                <ext:Button ID="Linkreload" runat="server" Icon="Reload" Text='<%# GetLangStr("QuipmentSpecial2","重载地图")%>'>
                                    <Listeners>
                                        <Click Handler="window.location.reload();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Button ID="TbutMove" runat="server" Icon="Erase" Text='<%# GetLangStr("QuipmentSpecial3","清除")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.ClearCircle();BMAP.ClearLabel();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButCenetr" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("QuipmentSpecial4","中心点")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.GotoCenter();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButRanging" runat="server" Icon="PencilGo" Text='<%# GetLangStr("QuipmentSpecial5","测距")%>'>
                                    <Listeners>
                                        <Click Handler=" BMAP.DistanceTool();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButPlan" runat="server" Icon="Vector" Text='<%# GetLangStr("QuipmentSpecial6","测面积")%>'>
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
                <ext:FormPanel ID="FormPanel1" Cls="new-layout ui-right-wrap Middle-arrow" Padding="5" Region="east" runat="server"
                    Split="true" Collapsible="true" RowHeight="1" Width="200" Layout="Accordion">
                    <Items>
                        <ext:Panel ID="Panel1" runat="server" Padding="4" Title="" Layout="Absolute"
                            Width="200" Height="260" Cls="data-list-container table-ui display-table w-100 Hide-panel-header">
                            <Items>
                                <ext:CheckboxGroup ID="gpck" runat="server" ColumnsNumber="1" Vertical="true">
                                    <Items>
                                        <ext:Checkbox Tag="TMS" BoxLabel='<%# GetLangStr("QuipmentSpecial7","电子警察") %>' StyleSpec="margin-left: 10px;" runat="server" Cls="ui-img-tms" />
                                        <ext:Checkbox Tag="TGS" BoxLabel='<%# GetLangStr("QuipmentSpecial8","治安卡口") %>' StyleSpec="margin-left: 10px;" runat="server" Cls="ui-img-tgs" />
                                        <ext:Checkbox Tag="VGS" BoxLabel='<%# GetLangStr("QuipmentSpecial11","虚拟卡口") %>' StyleSpec="margin-left: 10px;" runat="server" Cls="ui-img-tgs" />
                                        <ext:Checkbox Tag="TCS" BoxLabel='<%# GetLangStr("QuipmentSpecial9","电子测速") %>' StyleSpec="margin-left: 10px;" runat="server" Cls="ui-img-tcs" />
                                        <ext:Checkbox Tag="CCTV" BoxLabel='<%# GetLangStr("QuipmentSpecial10","高清视频") %>' StyleSpec="margin-left: 10px;" runat="server" Cls="ui-img-cctv" />
                                    </Items>
                                    <Listeners>
                                        <Change Handler="QuipmentSpecial.ShowDevice()" />
                                    </Listeners>
                                </ext:CheckboxGroup>
                            </Items>
                        </ext:Panel>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
<script type="text/javascript">
    jQuery(function ($) {
        $("body").delegate("#FormPane11 input", "click", function () {

            var img = "url(../imgs/40.png)"
            var imgs = "url(../imgs/401.png)"

            if ($(this).is(':checked')) {

                $(this).next().css("background-image", img)
            }

            else {
                $(this).next().css("background-image", imgs)
            }

        })

    })
</script>

<script type="text/javascript">

    $('.OverCar-data-list').niceScroll({
        cursorcolor: "#7683a4",//#CC0071 光标颜色
        cursoropacitymax: 1, //改变不透明度非常光标处于活动状态（scrollabar“可见”状态），范围从1到0
        touchbehavior: false, //使光标拖动滚动像在台式电脑触摸设备
        cursorwidth: "4px", //像素光标的宽度
        cursorborder: "0", // 	游标边框css定义
        cursorborderradius: "5px",//以像素为光标边界半径
        autohidemode: false //是否隐藏滚动条
    });
</script>