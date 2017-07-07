<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TgsPassCarAmply.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.TgsPassCarAmply" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>过往车辆监视</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <script type="text/javascript">
        var DataAmply = function () {
            return '<img class="imgEdit" ext:qtip="查看详细信息" style="cursor:pointer;" src="img/vcard_edit.png" />';
        };

        var cellClick = function (grid, rowIndex, columnIndex, e) {
            var t = e.getTarget(),
                record = grid.getStore().getAt(rowIndex),  // Get the Record
                columnId = grid.getColumnModel().getColumnId(columnIndex); // Get column id

            if (t.className == "imgEdit" && columnId == "Details") {
                return true;
            }
            return false;
        };
        var saveData = function () {
            GridData.setValue(Ext.encode(GridAlarmInfo.getRowsValues(false)));
        }
    </script>
    <script type="text/javascript">
        var getTasks = function (tree) {
            var msg = [],
           selNodes = tree.getChecked();
            msg.push("");
            Ext.each(selNodes, function (node) {
                if (msg.length > 1) {
                    msg.push(",");
                }
                msg.push("'");
                msg.push(node.id);
                msg.push("'");
            });
            msg.push("");
            return msg.join("");
        };
    </script>
    <script type="text/javascript">
        function resizeimg(obj) {
            var maxW = ImagePanel.body.dom.clientWidth;
            var maxH = ImagePanel.body.dom.clientHeight;
            var imgW = obj.width;
            var imgH = obj.height;
            var picwidth = imgW;
            var picheight = imgH;
            var ratioA = imgW / imgH;
            var ratioB = maxW / maxH;
            if (ratioB > 1) {
                if (imgH >= maxH) {
                    picheight = maxH;
                    picwidth = maxH * ratioA;

                }
            }
            else {
                if (imgW >= maxW) {
                    picwidth = maxW;
                    picheight = maxW / ratioA;
                }

            }
            obj.width = picwidth;
            obj.height = picheight;
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

    <script src="../Map/js/jquery-1.7.min.js" type="text/javascript" charset="UTF-8"></script>
    <script src="../Map/js/sockjs-0.3.min.js" type="text/javascript" charset="UTF-8"></script>
    <script src="../Map/js/stomp.js" type="text/javascript" charset="UTF-8"></script>
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <%--  <script type="text/javascript">
          var ws = new SockJS('http://192.168.1.235:15674/stomp');
          var client = Stomp.over(ws);
          client.heartbeat.outgoing = 0;
          client.heartbeat.incoming = 0;
          client.debug = function (e) {
              try {
                  TgsPassCarAmply.InsertMq(e);
              }
              catch (err)
              { }
              //$('#second div').append($("<code>").text(e));

          };
          // default receive callback to get message from temporary queues
          client.onreceive = function (m) {
              $('#first div').append($("<code>").text(m.body));
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
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
        <ext:Hidden ID="RevStation" runat="server" />
        <ext:Hidden ID="PasscarXh" runat="server" />
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="TgsPassCarAmply" />
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <West Collapsible="true" Split="true">
                        <ext:TreePanel ID="TreePanel1" runat="server" Title="卡口列表" Icon="Monitor" Width="200"
                            Shadow="None" UseArrows="true" AutoScroll="true" Animate="true" EnableDD="true"
                            ContainerScroll="true" RootVisible="false" Collapsed="true">
                            <Listeners>
                                <CheckChange Handler="#{RevStation}.setValue(getTasks(this), false);" />
                            </Listeners>
                        </ext:TreePanel>
                    </West>
                    <Center>
                        <ext:FormPanel ID="Panel2" Region="Center" runat="server" Title="过往车辆信息" Icon="CarRed"
                            Header="true" DefaultAnchor="100%">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:Button ID="StartServerTime" runat="server" Text="开始接受">
                                            <Listeners>
                                                <Click Handler="#{TaskManager1}.startTask('servertime');" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="StopServerTime" runat="server" Text="停止接受">
                                            <Listeners>
                                                <Click Handler="#{TaskManager1}.stopTask('servertime');" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Items>
                                <ext:RowLayout ID="RowLayout1" runat="server" Split="true">
                                    <Rows>
                                        <ext:LayoutRow RowHeight="0.30">
                                            <ext:Panel ID="TextPanel" runat="server" Title="" Header="false" Frame="true">
                                            </ext:Panel>
                                        </ext:LayoutRow>
                                        <ext:LayoutRow RowHeight="0.70">
                                            <ext:Panel ID="ImagePanel" runat="server" Title="图片信息" Header="false">
                                            </ext:Panel>
                                        </ext:LayoutRow>
                                        <ext:LayoutRow>
                                            <ext:GridPanel ID="GridPassCar" runat="server" StripeRows="true" Title="列表显示" Collapsible="true"
                                                Height="300">
                                                <Store>
                                                    <ext:Store ID="StorePassCar" runat="server">
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
                                                                    <ext:RecordField Name="col28" />
                                                                </Fields>
                                                            </ext:JsonReader>
                                                        </Reader>
                                                    </ext:Store>
                                                </Store>
                                                <ColumnModel ID="ColumnModel3" runat="server">
                                                    <Columns>
                                                        <ext:Column Width="150" DataIndex="col3" Header="卡口名称" />
                                                        <ext:Column Width="80" DataIndex="col4" Header="号牌号码" />
                                                        <ext:Column Width="110" DataIndex="col6" Header="号牌种类" />
                                                        <ext:Column Width="140" DataIndex="col7" Header="过车时间" />
                                                        <%--<ext:DateColumn Width="120" DataIndex="col7" Header="过车时间" Format="yyyy-MM-dd HH:mm:ss" />--%>
                                                        <ext:Column Width="80" DataIndex="col17" Header="行驶方向" />
                                                        <ext:Column Width="40" DataIndex="col18" Header="车道" />
                                                        <ext:Column Width="150" DataIndex="col26" Header="所属机构" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                                        <DirectEvents>
                                                            <RowSelect OnEvent="SelectPassCar" Buffer="250">
                                                                <ExtraParams>
                                                                    <ext:Parameter Name="sdata" Value="record.data" Mode="Raw" />
                                                                </ExtraParams>
                                                            </RowSelect>
                                                        </DirectEvents>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <Listeners>
                                                    <CellClick Fn="cellClick" />
                                                </Listeners>
                                                <DirectEvents>
                                                    <CellClick OnEvent="ShowDetails" Buffer="250" Failure="Ext.MessageBox.alert('加载失败', '提示');">
                                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="={#{GridAlarm}.body}" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="data" Value="params[0].getStore().getAt(params[1]).data" Mode="Raw" />
                                                        </ExtraParams>
                                                    </CellClick>
                                                </DirectEvents>
                                            </ext:GridPanel>
                                        </ext:LayoutRow>
                                    </Rows>
                                </ext:RowLayout>
                            </Items>
                        </ext:FormPanel>
                    </Center>
                </ext:BorderLayout>
            </Items>
        </ext:Viewport>
        <ext:TaskManager ID="TaskManager1" runat="server">
            <Tasks>
                <ext:Task TaskID="servertime" Interval="5000" OnStart="
                        #{StartServerTime}.setDisabled(true);
                        #{StopServerTime}.setDisabled(false)"
                    OnStop="
                        #{StartServerTime}.setDisabled(false);
                        #{StopServerTime}.setDisabled(true)">
                    <DirectEvents>
                        <Update OnEvent="RefreshTime">
                        </Update>
                    </DirectEvents>
                </ext:Task>
            </Tasks>
        </ext:TaskManager>
    </form>
</body>
</html>