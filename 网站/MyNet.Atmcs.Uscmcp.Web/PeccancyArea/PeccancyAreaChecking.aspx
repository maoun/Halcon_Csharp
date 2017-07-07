<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeccancyAreaChecking.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.PeccancyAreaChecking" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>违法车辆审核</title>
    <link href="../Styles/chooser.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script language="javascript" src="../Scripts/CJL.0.1.min.js" type="text/javascript"
        charset="utf-8"></script>
    <script language="javascript" src="../Scripts/Zoom.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        function ShowImage(imageurl, imagetype) {
            if (imagetype == "1") {
                TabPanel1.hideTabStripItem(1);
                TabPanel1.hideTabStripItem(2);
                TabPanel1.unhideTabStripItem(0);
                TabPanel1.setActiveTab(0);
                ZoomPanel.setDisabled(false);
                document.getElementById("idImage").src = imageurl;
                iz = new ImageZoom("idImage", "idViewer");
                iz.reset({ scale: 0 });
            }
            else {

                if (imagetype == "2") {
                    TabPanel1.hideTabStripItem(0);
                    TabPanel1.hideTabStripItem(2);
                    TabPanel1.unhideTabStripItem(1);
                    TabPanel1.setActiveTab(1);
                    ZoomPanel.setDisabled(true);
                    var Player = document.getElementById("Player1");
                    Player.videoStop();
                    Player.setVideoFileName(imageurl);
                    Player.loadVideo();
                }
                else {

                    TabPanel1.hideTabStripItem(0);
                    TabPanel1.hideTabStripItem(1);
                    TabPanel1.unhideTabStripItem(2);
                    TabPanel1.setActiveTab(2);
                    ZoomPanel.setDisabled(true);
                    var MediaPlayer = document.getElementById("MediaPlayer");
                    MediaPlayer.URL = imageurl;
                }

            }

        }
    </script>
    <style type="text/css">
        .container {
            position: relative;
        }

        .izImage, .izViewer {
            border: 1px solid #000;
            background: #fff url('../images/loading.gif') no-repeat center;
        }

        .izImage {
            height: 400px;
        }

        .izViewer {
            width: 300px;
            height: 300px;
            top: 0;
            position: absolute;
            display: none;
        }

            .izViewer div {
                position: absolute;
                border: 0 dashed #999;
                top: 0;
                left: 0;
                z-index: 999;
                width: 100%;
                height: 100%;
            }

        .x-btn {
            border-radius: 0px;
        }
    </style>
    <style type="text/css">
        .images-view .x-panel-body {
            background: white;
            font: 11px Arial, Helvetica, sans-serif;
        }

        .images-view .thumb {
            background: #dddddd;
            padding: 3px;
        }

            .images-view .thumb img {
                height: 60px;
                width: 80px;
            }

        .images-view .thumb-wrap {
            float: left;
            margin: 4px;
            margin-right: 0;
            padding: 5px;
            text-align: center;
        }

            .images-view .thumb-wrap span {
                display: block;
                overflow: hidden;
                text-align: center;
            }

        .images-view .x-view-over {
            border: 1px solid #dddddd;
            background: #efefef url(../images/row-over.gif) repeat-x left top;
            padding: 4px;
        }

        .images-view .x-view-selected {
            background: #eff5fb url(../images/selected.gif) no-repeat right bottom;
            border: 1px solid #99bbe8;
            padding: 4px;
        }

            .images-view .x-view-selected .thumb {
                background: transparent;
            }

        .images-view .loading-indicator {
            font-size: 11px;
            background-image: url(../images/loading.gif);
            background-repeat: no-repeat;
            background-position: left;
            padding-left: 20px;
            margin: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PeccancySingleCheck" />
        <ext:Hidden ID="CheckData" runat="server" />
        <ext:Hidden ID="CurrentIndex" runat="server" />
        <ext:Hidden ID="CurrentId" runat="server" />
        <ext:Store ID="StorePlateType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
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
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StorePeccancyType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreImage" runat="server">
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
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="PeccancyCheckPanel" Region="Center" runat="server" Title="违法车辆"
                    Icon="Lorry" DefaultAnchor="100%">
                    <Items>
                        <ext:BorderLayout ID="BorderLayout1" runat="server">
                            <East Collapsible="true" Split="true">
                                <ext:Panel ID="ZoomPanel" runat="server" Title="图片放大" Width="300">
                                    <Content>
                                        <center>
                                            <div id="idViewer" class="izViewer">
                                                <div style="height: 50%; border-bottom-width: 1px;">
                                                </div>
                                                <div style="width: 50%; border-right-width: 1px;">
                                                </div>
                                            </div>
                                            <center>
                                    </Content>
                                </ext:Panel>
                            </East>
                            <Center>
                                <ext:TabPanel ID="TabPanel1" runat="server" Region="Center">
                                    <Items>
                                        <ext:Panel ID="ImageDetailPanel" runat="server" Title="图片信息" Header="false">
                                            <Content>
                                                <center>
                                                    <div class="container">
                                                        <br />
                                                        <br />
                                                        <img id="idImage" class="izImage" alt="车辆图片(双击图片进行放大)" src="Images/NoImage.png" ondblclick="OpenPicPage(this.src);" />
                                                    </div>
                                                </center>
                                            </Content>
                                        </ext:Panel>
                                        <ext:Panel ID="VideoDetailPanel" runat="server" Title="视频信息" Header="false">
                                            <Content>
                                                <center>
                                                    <div class="container">
                                                        <br />
                                                        <br />
                                                        <object classid="clsid:BB2C64C1-EAB4-4713-B754-BAFCB5495B03" id="Player1" width="600"
                                                            height="450">
                                                        </object>
                                                    </div>
                                                    <br />
                                                    <p>
                                                        <font size="2"><a href="ActiveX/player.exe">播放控件下载</a></font>
                                                    </p>
                                                </center>
                                            </Content>
                                        </ext:Panel>
                                        <ext:Panel ID="MediaPlayerPanel" runat="server" Title="视频信息" Header="false">
                                            <Content>
                                                <center>
                                                    <div class="container">
                                                        <br />
                                                        <br />
                                                        <object id='MediaPlayer' width='600' height='450' classid="CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6">
                                                            <param name='AutoStart' value='1'>
                                                            <param name='rate' value='1'>
                                                            <param name='balance' value='0'>
                                                            <param name='currentPosition' value='0'>
                                                            <param name='defaultFrame' value=''>
                                                            <param name='playCount' value='1'>
                                                            <param name='currentMarker' value='0'>
                                                            <param name='invokeURLs' value='-1'>
                                                            <param name='baseURL' value=''>
                                                            <param name='volume' value='50'>
                                                            <param name='mute' value='0'>
                                                            <param name='uiMode' value='mini'>
                                                            <param name='stretchToFit' value='1'>
                                                            <param name='windowlessVideo' value='0'>
                                                            <param name='enabled' value='1'>
                                                            <param name='enableContextMenu' value='1'>
                                                            <param name='fullScreen' value='0'>
                                                            <param name='SAMIStyle' value=''>
                                                            <param name='SAMILang' value=''>
                                                            <param name='SAMIFilename' value=''>
                                                            <param name='captioningID' value=''>
                                                            <param name='enableErrorDialogs' value='0'>
                                                            <param name='_cx' value='8467'>
                                                            <param name='_cy' value='8467'>
                                                            <param name='AutoSize' value='1'>
                                                        </object>
                                                        ");
                                                    </div>
                                                    <br />
                                                </center>
                                            </Content>
                                        </ext:Panel>
                                    </Items>
                                </ext:TabPanel>
                            </Center>
                            <South Collapsible="true" MinHeight="130" Split="true">
                                <ext:Panel runat="server" ID="ImagePanel" Cls="images-view" Frame="true" Height="130"
                                    Collapsible="false" Layout="Fit" Title="图片列表">
                                    <Items>
                                        <ext:DataView ID="ImageView" runat="server" StoreID="StoreImage" AutoHeight="true"
                                            MultiSelect="true" OverClass="x-view-over" ItemSelector="div.thumb-wrap" EmptyText="没有图片">
                                            <Template ID="Template1" runat="server">
                                                <Html>
                                                    <tpl for=".">
								                    <div class="thumb-wrap">
									                <div class="thumb"><img src="{col3}" title="{col1}" onclick="ShowImage('{col0}','{col2}')" ></div>
								                    </div>
							                     </tpl>
                                                    <div class="x-clear"></div>
                                                </Html>
                                            </Template>
                                        </ext:DataView>
                                    </Items>
                                </ext:Panel>
                            </South>
                        </ext:BorderLayout>
                    </Items>
                </ext:FormPanel>
                <ext:FormPanel ID="FrmAmply" runat="server" Region="East" Split="true" Frame="true"
                  Width="410"  DefaultAnchor="100%">
                    <Items>
                        <ext:Panel ID="PanAmply" runat="server" Title="车辆信息" DefaultAnchor="100%" Padding="5">
                            <Items>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;车牌号牌：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtHphm" runat="server" Width="112" />
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;号牌种类：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtHpzl" runat="server" Width="112"  ReadOnly="true"/>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;起止时间：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtQdsj" runat="server" Width="152"   ReadOnly="true"/>
                                        <ext:TextField ID="TxtSdsj" runat="server" Width="152"  ReadOnly="true"/>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar3" runat="server">
                                    <Items>
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;起点卡口：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtQdkk" runat="server" Width="305"  ReadOnly="true" />
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar5" runat="server">
                                    <Items>
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;始点卡口：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtSdkk" runat="server" Width="305"  ReadOnly="true" />
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar2" runat="server">
                                    <Items>
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;违法地点：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtWfdd" runat="server" Width="305"  ReadOnly="true"/>
                                     
                                    </Items>
                                </ext:Toolbar>
                                 <ext:Toolbar ID="Toolbar15" runat="server">
                                    <Items>   <ext:Label runat="server" Html="<font color='white'>&nbsp;违法时间：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtWfsj" runat="server" Width="305"  ReadOnly="true"/>
                                            </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar6" runat="server">
                                    <Items>
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;违法行为：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtWfxw" runat="server" Width="305"  ReadOnly="true"/>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar7" runat="server">
                                    <Items>
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;行驶方向：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtXsfx" runat="server" Width="305"  ReadOnly="true"/>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar8" runat="server">
                                    <Items>
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;区间距离：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtQjjl" runat="server" Width="112"  ReadOnly="true"/>
                                        <ext:Label runat="server" Html="<font color='white'>速度/限速：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtSdxs" runat="server" Width="112"  ReadOnly="true"/>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar9" runat="server">
                                    <Items>
                                        <ext:Label ID="LabQjys" runat="server" Html="<font color='white'>&nbsp;区间用时：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtQjys" runat="server" Width="112"  ReadOnly="true"/>
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;正常用时：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtZcys" runat="server" Width="112"  ReadOnly="true"/>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar10" runat="server">
                                    <Items>
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;超速比例：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtCsbl" runat="server" Width="112"  ReadOnly="true"/>
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;审核状态：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtShzt" runat="server" Width="112"  ReadOnly="true"/>
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Panel>
                        <ext:Panel ID="Panel3" runat="server" Title="车管信息" DefaultAnchor="100%" Padding="5">
                            <Items>
                                <ext:Toolbar ID="Toolbar11" runat="server">
                                    <Items>
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;车身颜色：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtVehCsys" runat="server" Width="112"  ReadOnly="true"/>
                                        <ext:Label ID="Label1" runat="server" Html="<font color='white'>&nbsp;车辆品牌：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtVehClpp" runat="server" Width="112"  ReadOnly="true"/>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar12" runat="server">
                                    <Items>
                                        <ext:Label ID="Label2" runat="server" Html="<font color='white'>&nbsp;车辆类型：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtVehCllx" runat="server" Width="112"  ReadOnly="true"/>
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;使用性质：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtVehSyxz" runat="server" Width="112"  ReadOnly="true" />
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar13" runat="server">
                                    <Items>
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;车主姓名：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtVehCzxm" runat="server" Width="112"  ReadOnly="true"/>
                                        <ext:Label runat="server" Html="<font color='white'>&nbsp;车辆状态：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtVehClzt" runat="server" Width="112"  ReadOnly="true"/>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar14" runat="server">
                                    <Items>
                                        <ext:Label ID="Label3" runat="server" Html="<font color='white'>&nbsp;有效期止：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtVehYxqz" runat="server" Width="112"  ReadOnly="true"/>
                                        <ext:Label ID="Label4" runat="server" Html="<font color='white'>&nbsp;联系电话：</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtVehLxdh" runat="server" Width="112"  ReadOnly="true"/>
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Panel>
                    </Items>
                    <BottomBar>
                       <ext:Toolbar ID="Toolbar4" runat="server" Layout="Container">
                        <Items>
                            <ext:Panel ID="pnlPackCenter" runat="server" Layout="HBoxLayout">
                                <Defaults>
                                    <ext:Parameter Name="margins" Value="0 0 0 0" Mode="Value" />
                                </Defaults>
                                <LayoutConfig>
                                    <ext:HBoxLayoutConfig Padding="0" Align="Middle" Pack="Center" />
                                </LayoutConfig>
                                <Items>
                                    <ext:Button ID="ButFirst" runat="server" Text="首页" Icon="PreviousGreen">
                                        <Listeners>
                                            <Click Handler="PeccancySingleCheck.FirstInfo()" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button ID="ButLast" runat="server" Text="上一条" Icon="ReverseGreen">
                                        <Listeners>
                                            <Click Handler="PeccancySingleCheck.LastInfo()" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button ID="ButNext" runat="server" Text="下一条" Icon="PlayGreen">
                                        <Listeners>
                                            <Click Handler="PeccancySingleCheck.NextInfo()" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button ID="ButEnd" runat="server" Text="尾页" Icon="NextGreen">
                                        <Listeners>
                                            <Click Handler="PeccancySingleCheck.EndInfo()" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="Panel1" runat="server" Layout="HBoxLayout">
                                <Defaults>
                                    <ext:Parameter Name="margins" Value="0 0 0 0" Mode="Value" />
                                </Defaults>
                                <LayoutConfig>
                                    <ext:HBoxLayoutConfig Padding="0" Align="Middle" Pack="Center" />
                                </LayoutConfig>
                                <Items>
                                 <ext:Button ID="ButQuery" runat="server" Text="查询" Icon="ControlPlayBlue" Height="30">
                                        <DirectEvents>
                                            <Click OnEvent="ButQueryClick" />
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="ButCheckOk" runat="server" Text="审核有效" Icon="Accept" Height="30">
                                        <DirectEvents>
                                            <Click OnEvent="ButCheckOkClick" />
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="ButCheckNo" runat="server" Text="审核无效" Icon="Decline" Height="30">
                                        <DirectEvents>
                                            <Click OnEvent="ButCheckNoClick" />
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="ButCheckRpeat" runat="server" Text="套牌" Icon="CarAdd" Height="30">
                                        <DirectEvents>
                                            <Click OnEvent="ButCheckRpeatClick" />
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Toolbar>
                    </BottomBar>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
        <input id="HidHpzl" runat="server" name="HidHpzl" style="z-index: 102; left: 393px; width: 26px; position: absolute; top: 6773px"
            type="hidden" />
        <input id="HidVehCsys" runat="server" name="HidVehCsys" style="z-index: 102; left: 393px; width: 26px; position: absolute; top: 6773px"
            type="hidden" />
        <input id="HidVehCllx" runat="server" name="HidVehCllx" style="z-index: 102; left: 393px; width: 26px; position: absolute; top: 6773px"
            type="hidden" />
        <input id="HidVehSyxz" runat="server" name="HidVehSyxz" style="z-index: 102; left: 393px; width: 26px; position: absolute; top: 6773px"
            type="hidden" />
        <input id="HidVehClzt" runat="server" name="HidVehClzt" style="z-index: 102; left: 393px; width: 26px; position: absolute; top: 6773px"
            type="hidden" />
    </form>
</body>
</html>