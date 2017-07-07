<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeccancySingleRepeatCheck.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.PeccancySingleRepeatCheck" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>违法车辆复审</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Css/chooser.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/NewPageStyle.css" rel="stylesheet" type="text/css" />
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
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script language="javascript" src="../Scripts/CJL.0.1.min.js" type="text/javascript" charset="utf-8"></script>
    <script language="javascript" src="../Scripts/Zoom.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="utf-8"></script>
    <script type="text/javascript">
        $(function () {
            $("body").delegate("#txtPlateId", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#cmbPlateType").click();
                }
            })
        })
        var changeUpper = function (obj, value) {
            obj.setValue(value.toUpperCase());
        };
    </script>
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
        //function getTxzzt() {
        //    var txz = document.getElementById("lbTxzzt");//得到通行证文本
        //    if (txz.innerText == "取到通行证") {
        //        txz.style = "color:green;";
        //    } else {
        //        txz.style = "color:red;";
        //}
        //}
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PeccancyRepeatSingleCheck" />
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
                <ext:FormPanel ID="PeccancyCheckPanel" Region="Center" runat="server" Title='<%# GetLangStr("PeccancySingleRepeatCheck1","违法车辆")%>'
                    Icon="Lorry" DefaultAnchor="100%">
                    <Items>
                        <ext:BorderLayout ID="BorderLayout1" runat="server">
                            <East Collapsible="true" Split="true">
                                <ext:Panel ID="ZoomPanel" runat="server" Title='<%# GetLangStr("PeccancySingleRepeatCheck2","图片放大")%>' Width="300">
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
                                        <ext:Panel ID="ImageDetailPanel" runat="server" Title='<%# GetLangStr("PeccancySingleRepeatCheck3","图片信息")%>' Header="false">
                                            <Content>
                                                <center>
                                                    <div class="container">
                                                        <br />
                                                        <br />
                                                        <img id="idImage" class="izImage" alt="车辆图片(双击图片进行放大)" src="../Images/NoImage.png" ondblclick="OpenPicPage(this.src);" />
                                                    </div>
                                                </center>
                                            </Content>
                                        </ext:Panel>
                                        <ext:Panel ID="VideoDetailPanel" runat="server" Title='<%# GetLangStr("PeccancySingleRepeatCheck4","视频信息")%>' Header="false">
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
                                                        <font size="2"><a href="ActiveX/player.exe"><%# GetLangStr("PeccancySingleRepeatCheck5","播放控件下载")%></a></font>
                                                    </p>
                                                </center>
                                            </Content>
                                        </ext:Panel>
                                        <ext:Panel ID="MediaPlayerPanel" runat="server" Title='<%# GetLangStr("PeccancySingleRepeatCheck6","视频信息")%>' Header="false">
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
                                    Collapsible="false" Layout="Fit" Title='<%# GetLangStr("PeccancySingleRepeatCheck7","图片列表")%>'>
                                    <Items>
                                        <ext:DataView ID="ImageView" runat="server" StoreID="StoreImage" AutoHeight="true"
                                            MultiSelect="true" OverClass="x-view-over" ItemSelector="div.thumb-wrap" EmptyText='<%# GetLangStr("PeccancySingleRepeatCheck8","没有图片")%>'>
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
                    Width="390" DefaultAnchor="100%">
                    <Items>
                        <ext:Panel ID="PanAmply" runat="server" Title='<%# GetLangStr("PeccancySingleRepeatCheck9","车辆信息")%>' DefaultAnchor="100%" Padding="5">
                            <Items>
                                <ext:Panel ID="CompositeField2" runat="server" Layout="ColumnLayout" Style="margin-bottom: 5px">
                                    <Items>
                                        <ext:Label runat="server" Text='<%# GetLangStr("PeccancySingleRepeatCheck10","号牌号码：") %>' Style="margin-top: 5px; margin-right: 40px" />
                                        <ext:Panel ID="Panel2" runat="server">
                                            <Content>
                                                <veh:VehicleHead ID="VehicleHead" runat="server" />
                                            </Content>
                                        </ext:Panel>

                                        <ext:TextField ID="txtPlateId" runat="server" Width="224" EmptyText="六位号牌号码">
                                            <Listeners>
                                                <Change Fn="changeUpper" />
                                            </Listeners>
                                        </ext:TextField>
                                    </Items>
                                </ext:Panel>
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("PeccancySingleRepeatCheck11","号牌种类") %>' ID="cmbPlateType" StoreID="StorePlateType"
                                    Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("PeccancySingleRepeatCheck12","选择号牌种类")%>' Width="370">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancySingleRepeatCheck13","违法时间")%>' ID="TxtPeccancyDate" Width="370" ReadOnly="true" />
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("PeccancySingleRepeatCheck14","违法地点")%>' ID="cmbLocation" StoreID="StoreLocation"
                                    Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("PeccancySingleRepeatCheck15","选择违法地点")%>' Width="370">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox ID="cmbPeccancyType" FieldLabel='<%# GetLangStr("PeccancySingleRepeatCheck16","违法行为")%>' runat="server" StoreID="StorePeccancyType"
                                    Editable="false" DisplayField="col2" ValueField="col1" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("PeccancySingleRepeatCheck17","选择违法行为")%>' Width="370">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancySingleRepeatCheck18","速度/限速")%>' ID="TxtSpeed" Width="370" ReadOnly="true" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancySingleRepeatCheck19","审核用户")%>' ID="TxtShyh" Width="370" ReadOnly="true" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancySingleRepeatCheck20","执勤民警")%>' ID="TxtZqmj" Width="370" ReadOnly="true" />
                            </Items>
                        </ext:Panel>

                        <ext:TabPanel ID="TabPanel2" runat="server" Height="300">
                            <Items>
                                <ext:Panel ID="Panel4" runat="server" Title="通行证信息" DefaultAnchor="100%" Padding="5">
                                    <Items>
                                        <ext:Label runat="server" ID="lbTxzzt" FieldLabel="通行证状态" Width="370"></ext:Label>
                                        <ext:TextField runat="server" FieldLabel="起止时间" ID="txtQzsj" Width="370" ReadOnly="true" />
                                        <ext:TextField runat="server" FieldLabel="通行路段" ID="txtTxld" Width="370" ReadOnly="true" />
                                        <ext:TextField runat="server" FieldLabel="通行时段" ID="txtTxsd" Width="370" ReadOnly="true" />
                                    </Items>
                                </ext:Panel>
                                <ext:Panel ID="Panel3" runat="server" Title='<%# GetLangStr("PeccancySingleCheck19","车管信息")%>' DefaultAnchor="100%" Padding="5">
                                    <Items>
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancySingleCheck20","车身颜色")%>' ID="TxtVehCsys" Width="370" ReadOnly="true" />
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancySingleCheck21","车辆品牌")%>' ID="TxtVehClpp" Width="370" ReadOnly="true" />
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancySingleCheck22","车辆类型")%>' ID="TxtVehCllx" Width="370" ReadOnly="true" />
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancySingleCheck23","使用性质")%>' ID="TxtVehSyxz" Width="370" ReadOnly="true" />
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancySingleCheck24","车主姓名")%>' ID="TxtVehCzxm" Width="370" ReadOnly="true" />
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancySingleCheck25","车辆状态")%>' ID="TxtVehClzt" Width="370" ReadOnly="true" />
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancySingleCheck26","有效期止")%>' ID="TxtVehYxqz" Width="370" ReadOnly="true" />
                                        <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancySingleCheck27","联系电话")%>' ID="TxtVehLxdh" Width="370" ReadOnly="true" />
                                    </Items>
                                </ext:Panel>
                            </Items>
                        </ext:TabPanel>
                    </Items>
                    <BottomBar>
                        <ext:Toolbar ID="Toolbar4" runat="server" Layout="Container">
                            <Items>
                                <ext:Panel ID="pnlPackCenter" runat="server" Layout="HBoxLayout">
                                    <Defaults>
                                        <ext:Parameter Name="margins" Value="0 5 0 0" Mode="Value" />
                                    </Defaults>
                                    <LayoutConfig>
                                        <ext:HBoxLayoutConfig Padding="5" Align="Middle" Pack="Center" />
                                    </LayoutConfig>
                                    <Items>
                                        <ext:Button ID="ButFirst" runat="server" Icon="PreviousGreen">
                                            <Listeners>
                                                <Click Handler="PeccancyRepeatSingleCheck.FirstInfo()" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="ButLast" runat="server" Text='<%# GetLangStr("PeccancySingleRepeatCheck31","上一条")%>' Icon="ReverseGreen">
                                            <Listeners>
                                                <Click Handler="PeccancyRepeatSingleCheck.LastInfo()" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="ButNext" runat="server" Text='<%# GetLangStr("PeccancySingleRepeatCheck32","下一条")%>' Icon="PlayGreen">
                                            <Listeners>
                                                <Click Handler="PeccancyRepeatSingleCheck.NextInfo()" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="ButEnd" runat="server" Icon="NextGreen">
                                            <Listeners>
                                                <Click Handler="PeccancyRepeatSingleCheck.EndInfo()" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel ID="Panel1" runat="server" Layout="HBoxLayout">
                                    <Defaults>
                                        <ext:Parameter Name="margins" Value="0 5 0 0" Mode="Value" />
                                    </Defaults>
                                    <LayoutConfig>
                                        <ext:HBoxLayoutConfig Padding="5" Align="Middle" Pack="Center" />
                                    </LayoutConfig>
                                    <Items>
                                        <%--     <ext:Button ID="ButQuery" runat="server" Text="信息查询" Icon="ControlPlayBlue" Height="30">
                                        <DirectEvents>
                                            <Click OnEvent="ButQueryClick" />
                                        </DirectEvents>
                                    </ext:Button>--%>
                                        <ext:Button ID="ButCheckOk" runat="server" Text='<%# GetLangStr("PeccancySingleRepeatCheck34","审核有效")%>' Icon="Accept" Height="30">
                                            <DirectEvents>
                                                <Click OnEvent="ButCheckOkClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButCheckNo" runat="server" Text='<%# GetLangStr("PeccancySingleRepeatCheck35","审核无效")%>' Icon="Decline" Height="30">
                                            <DirectEvents>
                                                <Click OnEvent="ButCheckNoClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <%--    <ext:Button ID="ButCheckRpeat" runat="server" Text="套牌车辆" Icon="CarAdd" Height="30">
                                        <DirectEvents>
                                            <Click OnEvent="ButCheckRpeatClick" />
                                        </DirectEvents>
                                    </ext:Button>--%>
                                    </Items>
                                </ext:Panel>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
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