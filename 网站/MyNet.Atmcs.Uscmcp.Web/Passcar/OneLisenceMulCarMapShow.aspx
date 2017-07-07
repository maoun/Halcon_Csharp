<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OneLisenceMulCarMapShow.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Passcar.OneLisenceMulCarMapShow" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>一牌多车地图</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="css/custom.css" />
    <link rel="stylesheet" type="text/css" href="../Style/customMap.css" />
    <link rel="stylesheet" href="../Css/showphotostyle.css" type="text/css" />
    <style type="text/css">
        body, html {
            font-family: Arial,Verdana;
            font-size: 13px;
            margin: 0;
            overflow: hidden;
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
    <script language="JavaScript" src="../Scripts/showphoto.js" type="text/javascript" charset="UTF-8"></script>
    <script type="text/javascript" src="../Mapjs/jquery.nicescroll.js" charset="UTF-8"></script>
    <link rel="stylesheet" type="text/css" href="../Map/css/Ui-skin.css" />
    <script type="text/javascript" src="../Map/Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <!--梁引入如下js和css 结束-->
</head>
<body onload="MapCenter();">
    <form id="form2" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="OneLisenceMulCarMapShow" />

        <ext:Viewport ID="Viewport1" runat="server" Layout="border" Cls="new-layout">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Title="地图浏览" AutoRender="true" Region="Center"
                    Header="false" Height="800" Width="1000" IconCls="temp/map.png" Cls="map-bg">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar12" runat="server" Cls="top-toolbar">
                            <Items>
                                <ext:Button ID="Linkreload" runat="server" Icon="Reload" Text="重载地图">
                                    <Listeners>
                                        <Click Handler="window.location.reload();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Label runat="server" ID="labhphm" />
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
                                <ext:Button ID="ButPlan" runat="server" Icon="Vector" Text="测面积">
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
                    </Content>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>