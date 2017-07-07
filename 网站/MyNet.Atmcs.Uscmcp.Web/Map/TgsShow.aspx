<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TgsShow.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.TgsShow" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>过往车辆监视</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Css/chooser.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .container {
            position: relative;
            width: 100%;
        }

        .izImage, .izViewer {
            border: 1px solid #000;
            background: #fff url('o_loading.gif') no-repeat center;
        }

        .izImage {
            height: 500px;
        }

        .izViewer {
            width: 200px;
            height: 200px;
            position: absolute;
            left: 0;
            top: 0;
            display: none;
        }
    </style>

    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Map/js/showphoto.js" language="JavaScript" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        var IMGDIR = 'images/sets';
        var attackevasive = '0';
        var gid = 0;
        var fid = parseInt('0');
        var tid = parseInt('0');
    </script>
    <%-- <script language="javascript" type="text/javascript">

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
                if (XmlHttp.responsetext == 'TGS') {
                    TgsShow.DefaultImage();
                }

            }
        }

        function OpenPicPage(url) {
            var ImageView = window.open('ImageVideo.html?imgUrl=' + url, 'ImageView', 'toolbar=no,location=no,menubar=no,scrollbars=no,resizable=yes,width=1240,height=700');
            ImageView.focus();
        }
        function StartOrStop() {
            timer = setInterval('sendAJAX(\'Tgs\')', 2000);
        }
    </script>--%>
    <script src="../Map/js/jquery-1.7.min.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Map/js/sockjs-0.3.min.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Map/js/stomp.js" type="text/javascript" charset="utf-8"></script>
    <link href="main.css" rel="stylesheet" />

    <script type="text/javascript">
        var ws = new SockJS('http://192.168.1.235:15674/stomp');
        var client = Stomp.over(ws);
        client.heartbeat.outgoing = 0;
        client.heartbeat.incoming = 0;
        var curr_time = new Date();
        client.debug = function (e) {
            try {
                //if (parseInt((new Date() - curr_time)) > 1000) {
                //    curr_time = new Date();
                //TgsShow.GetImagFromMq(e);

                //$('#second div').text(e);
                TgsShow.Mq(e);
                //}
            }
            catch (err)
            { }
            //$('#second div').append($("<code>").text(e));

        };
        // default receive callback to get message from temporary queues
        client.onreceive = function (m) {
            //$('#first div').append($("<code>").text(m.body));
            client.send('EHL_MONITOR_QUEUE', { 'reply-to': '/temp-queue/foo' }, new Date());
        }

        var on_connect = function (x) {
            id = client.subscribe("EHL_MONITOR_QUEUE", function (m) {
                var reversedText = m.body.split("").reverse().join("");
                client.send(m.headers['reply-to'], { "content-type": "text/plain" }, reversedText);
            });
        };
        var on_error = function () {
            console.log('error');
        };
        client.connect('admin', 'admin', on_connect, on_error, '/');

        //$('#first form').submit(function () {
        //    var text = $('#first form input').val();
        //    if (text) {
        //        client.send('EHL_MONITOR_QUEUE', { 'reply-to': '/temp-queue/foo' }, text);
        //        $('#first form input').val("");
        //    }
        //    return false;
        //});
    </script>
    <script type="text/javascript">
        function closeWindows() {
            try {
                //TgsShow.Page_Close();
            }
            catch (err)
            { }
        }
    </script>
</head>
<body>

    <form id="form1" runat="server">
        <div id="append_parent" />
        <div id="second" class="box" runat="server" />
        <ext:Hidden ID="HidNowHphm" runat="server"></ext:Hidden>
        <ext:Hidden ID="GridData" runat="server" />
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="TgsShow" />
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <ext:TabPanel ID="TabPanel1" runat="server" Region="Center">
                    <Items>
                        <ext:Panel ID="panOne" runat="server" Title="图片监视" Border="false" Padding="1" AutoScroll="true">
                            <TopBar>
                                <ext:Toolbar ID="ToolbarOne" runat="server">
                                    <Items>
                                        <ext:Label ID="LblOne" runat="server">
                                        </ext:Label>
                                        <ext:ToolbarFill />
                                        <ext:Button ID="ButOne" runat="server" Icon="BulletPicture" Text="实时过车查看">
                                            <Listeners>
                                                <Click Handler="var w = window.screen.availWidth;var h = window.screen.availHeight;  newwin=window.open('../Map/TgsPassCarAmply.aspx','实时过车监控','scrollbars');if(document.all) {newwin.moveTo(0,0); newwin.resizeTo(w,h);}" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:Panel>
                        <ext:Panel ID="panFour" runat="server" Title="四画面显示" Border="false" Padding="6" AutoScroll="true"
                            Hidden="true">
                            <TopBar>
                                <ext:Toolbar ID="ToolbarFour" runat="server">
                                    <Items>
                                        <ext:Label ID="LalFour" runat="server">
                                        </ext:Label>
                                        <ext:ToolbarFill />
                                        <ext:Button ID="ButFour" runat="server" Icon="BulletPicture" Text="实时过车查看">
                                            <Listeners>
                                                <Click Handler="var w = window.screen.availWidth;var h = window.screen.availHeight;  newwin=window.open('TgsPassCarAmply.aspx','实时过车监控','scrollbars'); if(document.all) {newwin.moveTo(0,0); newwin.resizeTo(w,h);newwin.depended='yes';newwin.alwaysRaised ='yes'}" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:Panel>
                        <ext:Panel ID="panNine" runat="server" Title="九画面显示" Border="false" Padding="6" AutoScroll="true"
                            Hidden="true">
                            <TopBar>
                                <ext:Toolbar ID="ToolbarNine" runat="server">
                                    <Items>
                                        <ext:Label ID="LblNine" runat="server">
                                        </ext:Label>
                                        <ext:ToolbarFill />
                                        <ext:Button ID="ButNine" runat="server" Icon="BulletPicture" Text="实时过车查看">
                                            <Listeners>
                                                <Click Handler="var w = window.screen.availWidth;var h = window.screen.availHeight;  newwin=window.open('TgsPassCarAmply.aspx','实时过车监控','scrollbars');if(document.all) {newwin.moveTo(0,0); newwin.resizeTo(w,h);}" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:Panel>
                        <ext:Panel ID="PanDevice" runat="server" Height="300" Title="设备状态" Width="400"
                            Padding="0">
                            <Items>
                                <ext:GridPanel ID="GridDevice" Region="Center" runat="server" StripeRows="true" Title="设备状态"
                                    Collapsible="true" AutoHeight="true" Header="false">
                                    <TopBar>
                                        <ext:Toolbar ID="Toolbar1" runat="server">
                                            <Items>
                                                <ext:Label ID="LblDevice" runat="server">
                                                </ext:Label>
                                                <ext:ToolbarFill />
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <Store>
                                        <ext:Store ID="StoreDevice" runat="server">
                                            <AutoLoadParams>
                                                <ext:Parameter Name="start" Value="={0}" />
                                                <ext:Parameter Name="limit" Value="={15}" />
                                            </AutoLoadParams>
                                            <UpdateProxy>
                                                <ext:HttpWriteProxy Method="GET" Url="TgsShow.aspx">
                                                </ext:HttpWriteProxy>
                                            </UpdateProxy>
                                            <Reader>
                                                <ext:JsonReader>
                                                    <Fields>
                                                        <ext:RecordField Name="col0" Type="String" />
                                                        <ext:RecordField Name="col1" Type="String" />
                                                        <ext:RecordField Name="col2" Type="String" />
                                                        <ext:RecordField Name="col3" Type="String" />
                                                        <ext:RecordField Name="col4" Type="String" />
                                                        <ext:RecordField Name="col5" Type="String" />
                                                        <ext:RecordField Name="col6" Type="String" />
                                                        <ext:RecordField Name="col7" Type="String" />
                                                        <ext:RecordField Name="col8" Type="Date" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <ext:Column Width="60" DataIndex="col2" Hidden="true" ColumnID="Company" Header="设备编号" />
                                            <ext:Column Width="150" DataIndex="col3" ColumnID="Company" Header="设备名称" />
                                            <ext:Column Width="95" DataIndex="col4" ColumnID="Company" Header="设备IP" />
                                            <ext:Column Width="100" DataIndex="col7" ColumnID="Company" Header="状态" />
                                            <%-- <ext:TemplateColumn DataIndex="" MenuDisabled="true" Header="状态" Width="50">
                                                <Template ID="Template1" runat="server">
                                                    <Html>
                                                        <tpl for=".">
                                            <center>
							                    <img src="img/state/{col7}.jpg"  width="16" height="16" />
                                            </center>
						                </tpl>
                                                    </Html>
                                                </Template>
                                            </ext:TemplateColumn>--%>
                                            <ext:DateColumn Header="更新时间" DataIndex="col8" Width="120" Format="yyyy-MM-dd HH:mm" />
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                            </Items>
                        </ext:Panel>
                        <ext:Panel ID="PanFlow" runat="server" Height="300" Title="当前流量统计" Width="400" Padding="0">
                            <TopBar>
                                <ext:Toolbar ID="ToolbarFlow" runat="server">
                                    <Items>
                                        <ext:Label ID="LblFlow" runat="server">
                                        </ext:Label>
                                        <ext:ToolbarFill />
                                        <ext:Button ID="ButFlow" runat="server" Icon="BulletPicture" Text="详细流量统计">
                                            <Listeners>
                                                <Click Handler="var w = window.screen.availWidth;var h = window.screen.availHeight;  newwin=window.open('../Map/TgsFlowAmply.aspx','实时流量','scrollbars');if(document.all) {newwin.moveTo(0,0); newwin.resizeTo(w,h);}" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Items>
                                <ext:GridPanel ID="GriFlow" Region="Center" runat="server" StripeRows="true" Title="流量统计"
                                    Collapsible="true" AutoHeight="true" Header="false">
                                    <Store>
                                        <ext:Store ID="StoreFlow" runat="server">
                                            <AutoLoadParams>
                                                <ext:Parameter Name="start" Value="={0}" />
                                                <ext:Parameter Name="limit" Value="={15}" />
                                            </AutoLoadParams>
                                            <UpdateProxy>
                                                <ext:HttpWriteProxy Method="GET" Url="TgsShow.aspx">
                                                </ext:HttpWriteProxy>
                                            </UpdateProxy>
                                            <Reader>
                                                <ext:JsonReader>
                                                    <Fields>
                                                        <ext:RecordField Name="col0" Type="String" />
                                                        <ext:RecordField Name="col1" Type="String" />
                                                        <ext:RecordField Name="col2" Type="String" />
                                                        <ext:RecordField Name="col3" Type="String" />
                                                        <ext:RecordField Name="col4" Type="String" />
                                                        <ext:RecordField Name="col5" Type="String" />
                                                        <ext:RecordField Name="col6" Type="String" />
                                                        <ext:RecordField Name="col7" Type="String" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel ID="ColumnModel2" runat="server">
                                        <Columns>
                                            <ext:Column Width="80" DataIndex="col0" ColumnID="Company" Header="行驶方向" />
                                            <ext:Column Width="95" DataIndex="col1" ColumnID="Company" Header="统计日期" />
                                            <ext:Column Width="80" DataIndex="col2" ColumnID="Company" Header="统计小时" />
                                            <ext:Column Width="60" DataIndex="col3" ColumnID="Company" Header="总流量" />
                                            <ext:Column Width="80" DataIndex="col4" ColumnID="Company" Header="小车流量" />
                                            <ext:Column Width="80" DataIndex="col5" ColumnID="Company" Header="大车流量" />
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                            </Items>
                        </ext:Panel>
                    </Items>
                </ext:TabPanel>
            </Items>
        </ext:Viewport>
        <ext:TaskManager ID="TaskManager1" runat="server">
            <Tasks>
                <ext:Task TaskID="servertime" Interval="3000">
                    <DirectEvents>
                        <Update OnEvent="RefreshTime">
                        </Update>
                    </DirectEvents>
                </ext:Task>
                <ext:Task TaskID="imagetime" Interval="600">
                    <DirectEvents>
                        <Update OnEvent="RefreshImage">
                        </Update>
                    </DirectEvents>
                </ext:Task>
            </Tasks>
        </ext:TaskManager>
    </form>
</body>
<%-- <script type="text/javascript">
         window.onunload = checkLeave;
         //window.onbeforeunload = checkLeave;
   </script>--%>
</html>