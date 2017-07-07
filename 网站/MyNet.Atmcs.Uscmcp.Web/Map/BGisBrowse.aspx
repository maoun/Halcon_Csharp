<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BGisBrowse.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.BGisBrowse" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <link rel="stylesheet" type="text/css" href="../Style/customMap.css" />
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
    <script type="text/javascript" src="../Scripts/bmapFile.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/bmap.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>
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
    <script type="text/javascript">
        var selectionChaged = function (dv, nodes) {

            if (nodes.length > 0) {
                var order = nodes[0].innerText;
                var n = order.indexOf(',');
                var m = order.substring(0, n);
                if (showTrack != null) {
                    showTrack.ToCenter(m);
                }
            }
        };
    </script>
    <script type="text/javascript">
        var GetTasks = function () {
            var msg = "",
                    selNodes = TreePanel1.getChecked();

            Ext.each(selNodes, function (node) {
                if (msg.length > 0) {
                    msg += ", ";
                }
                msg += JSON.stringify(node.attributes);
            });
            BMAP.Clear();
            BMAP.ClearLine();
            BGisBrowse.MapShowTypeCheck(msg)
        };
    </script>
    <script language="javascript" type="text/javascript">

        var XmlHttp = new ActiveXObject("Microsoft.XMLhttp");
        var timer = null;
        function sendAJAX(par) {
            var url = "GisResponse.aspx?par=" + par;
            XmlHttp.Open("POST", url, true);
            XmlHttp.send(null);
            XmlHttp.onreadystatechange = ServerProcess;
        }
        function ServerProcess() {
            if (XmlHttp.readystate == 4 || XmlHttp.readystate == 'complete') {
                if (XmlHttp.responsetext == 'ALARM') {
                    BGisBrowse.ShowAlarmInfo();
                }
                else if (XmlHttp.responsetext == 'NOALARM') {
                    BGisBrowse.ClearAlarmInfo();
                }
                else if (XmlHttp.responsetext == 'GIS') {
                    //GisBrowse.ShowRoadState();
                }
            }
        }
        function StartOrStop(flag) {
            switch (flag) {
                case 0:
                    BMAP.RemoveAlarmMarker();
                    clearInterval(timer);
                    break;
                case 1:
                    BGisBrowse.ClearAlarmInfo();
                    clearInterval(timer);
                    timer = setInterval('sendAJAX(\'Alarm\')', 2000);
                    break;
                case 2:
                    timer = setInterval('sendAJAX(\'Gis\')', 600000);
                    break;
            }
        }
    </script>
    <script type="text/javascript">
        function soundPlay(url) {
            var sound = document.createElement("bgsound");
            sound.id = "soun";
            document.body.appendChild(sound);
            sound.autostart = "false";
            sound.loop = "1";
            sound.src = url;
        }
    </script>
</head>
<body onload="MapCenter();">
    <form id="form1" runat="server">
        <ext:Hidden ID="plateNo" runat="server" />
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="BGisBrowse" />
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
        <ext:Viewport ID="Viewport1" runat="server" Layout="border" Cls="layout">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Title="地图浏览" AutoRender="true" Region="Center"
                    Header="false" Height="800" Width="1000" Cls="map">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar12" runat="server" Cls="top-toolbar">
                            <Items>
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
                <ext:TreePanel ID="TreePanel1" runat="server" Width="200" Height="450" Icon="BookOpen"
                    Title="地图专题操作" RootVisible="false" AutoScroll="true" Region="East" Split="true"
                    Padding="10" Cls="right">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button ID="Button2" runat="server" Text="展开" Icon="SectionExpanded">
                                    <Listeners>
                                        <Click Handler="#{TreePanel1}.expandAll();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="Button3" runat="server" Text="收缩" Icon="SectionExpanded">
                                    <Listeners>
                                        <Click Handler="#{TreePanel1}.collapseAll();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Root>
                    </Root>
                    <BottomBar>
                        <ext:StatusBar ID="StatusBar1" runat="server" AutoClear="1500" />
                    </BottomBar>
                    <Listeners>
                        <CheckChange Fn="GetTasks" />
                    </Listeners>
                </ext:TreePanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

<style type="text/css">
    .layout {
        position: relative;
    }

    .map {
        position: relative;
        width: 100% !important;
        height: 100% !important;
    }

        .map .x-panel-tbar, .map .x-small-editor, .map .x-panel-body {
            width: 100% !important;
        }

    .right {
        position: absolute;
        top: 20px !important;
        right: 0 !important;
        z-index: 999;
        background-color: rgba(255,255,255,0.5);
    }
</style>