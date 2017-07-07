<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmMonitor.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.AlarmMonitor" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <title><%# GetLangStr("AlarmMonitor17","报警车辆监视") %></title>
    <link rel="stylesheet" type="text/css" href="../Css/chooser.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/jquery.mCustomScrollbar.css" />
    <link href="../Styles/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .search-item {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }

            .search-item h3 {
                display: block;
                font: inherit;
                font-weight: bold;
                color: #222;
            }

                .search-item h3 span {
                    float: right;
                    font-weight: normal;
                    margin: 0 0 5px 5px;
                    width: 100px;
                    display: block;
                    clear: none;
                }

        .ext-ie .x-form-text {
            position: static !important;
        }
    </style>
    <script type="text/javascript" language="javascript" src="../Scripts/common.js" charset="UTF-8"></script>
    <%--<script type="text/javascript" language="javascript" src="../Scripts/jquery.dad.min.js"></script>--%>
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8"> </script>
    <script type="text/javascript" src="../Scripts/jquery.mousewheel.min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.mCustomScrollbar.js" charset="UTF-8"></script>
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-ui-1.10.4.min.js" charset="UTF-8"></script>
    <script type="text/javascript" language="javascript" src="../Scripts/jquery.mousewheel.min.js" charset="UTF-8"></script>
    <script type="text/javascript" language="javascript" src="../Scripts/jquery.mCustomScrollbar.js" charset="UTF-8"></script>
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-ui-1.10.4.min.js" charset="UTF-8"></script>
    <!--图片放大开始-->
    <script type="text/javascript" src="../Scripts/Zoom/jquery.photo.gallery.js"></script>
    <!--图片放大结束-->
    <script type="text/javascript">
        (function ($) {
            $(window).load(function () {
                $("#ext-gen19").addClass("content");
                $("#ext-gen69").addClass("content");
                $(".content").mCustomScrollbar();
            });
        })(jQuery);
    </script>
    <script type="text/javascript">
        var DataAmply = function () {

            return '<img class="imgEdit" ext:qtip= "<%# GetLangStr("AlarmMonitor18","查看详细信息") %>"> style="cursor:pointer;" src="../images/button/vcard_edit.png" />';
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
        var getRowClass = function (record, rowIndex, p, ds) {
            var reColor = "";
            if (record.data.col5 == 0) {
                reColor = "x-grid-row-summary";
            }
            return reColor;
        };
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
        function StartOrStop(flag) {
            switch (flag) {
                case 0:
                    break;
                case 1:
                    break;
            }
        }
    </script>
    <script type="text/javascript">
        var refreshTree = function (tree) {
            AlarmMonitor.RefreshMenu({
                success: function (result) {
                    var nodes = eval(result);
                    if (nodes.length > 0) {
                        tree.initChildren(nodes);
                    }
                    else {
                        tree.getRootNode().removeChildren();
                    }
                }
            });
        }
    </script>
    <script type="text/javascript">
        var filterTree = function (el, e) {
            var tree = TreePanel1,
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

        var clearFilter = function () {
            var field = TriggerField1,
                tree = TreePanel1;

            field.setValue("");
            tree.clearFilter();
            tree.getRootNode().collapseChildNodes(true);
            tree.getRootNode().ensureVisible();
            //tree.expandAll();
        };
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridAlarm.view.findRowIndex(this.triggerElement),
                cellIndex = GridAlarm.view.findCellIndex(this.triggerElement),
                record = StoreAlarm.getAt(rowIndex),
                fieldName = GridAlarm.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);
            if (fieldName == "col6") {

                data = data.toString().substring(0, 10) + " " + data.toString().substring(11, 19);
            }

            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:Hidden ID="RevStation" runat="server" />
        <ext:Hidden ID="AlarmType" runat="server" />
        <ext:Hidden ID="AlarmXh" runat="server" />
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="AlarmMonitor" />
        <ext:Store ID="StoreClbj" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <West Split="true" AutoHide="true">
                        <ext:TreePanel ID="TreePanel1" runat="server" Width="300"
                            Shadow="None" UseArrows="true" AutoScroll="true" Animate="true" EnableDD="true"
                            ContainerScroll="true" RootVisible="true" Collapsed="false">
                            <Listeners>
                                <CheckChange Handler="#{RevStation}.setValue(getTasks(this), false);" />
                            </Listeners>
                            <TopBar>
                                <ext:Toolbar runat="server" Layout="FitLayout">
                                    <Items>
                                        <ext:TriggerField ID="TriggerField1" runat="server" EnableKeyEvents="true" StyleSpec="border-radius: 20px;">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" />
                                            </Triggers>
                                            <Listeners>
                                                <KeyUp Fn="filterTree" Buffer="500" />
                                                <TriggerClick Handler="clearFilter();" />
                                            </Listeners>
                                        </ext:TriggerField>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:TreePanel>
                    </West>
                    <Center>
                        <ext:FormPanel ID="Panel2" Region="Center" runat="server" Title='<%# GetLangStr("AlarmMonitor1","报警车辆信息") %>' Icon="CarRed"
                            Header="true" DefaultAnchor="100%">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:Button ID="StartServerTime" runat="server" Text='<%# GetLangStr("AlarmMonitor2","开始接收") %>'>
                                            <Listeners>
                                                <Click Handler="#{TaskManager1}.startTask('servertime');" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="StopServerTime" runat="server" Text='<%# GetLangStr("AlarmMonitor3","停止接收") %>'>
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
                                        <ext:LayoutRow RowHeight="0.10">
                                            <ext:Panel ID="TextPanel" runat="server" Title="" Header="false" Frame="true">
                                            </ext:Panel>
                                        </ext:LayoutRow>
                                        <ext:LayoutRow RowHeight="0.55">
                                            <ext:Panel ID="ImagePanel" runat="server" Title='<%# GetLangStr("AlarmMonitor4","图片信息") %>' Header="false">
                                            </ext:Panel>
                                        </ext:LayoutRow>
                                        <ext:LayoutRow RowHeight="0.40">
                                            <ext:GridPanel ID="GridAlarm" runat="server" StripeRows="true" Title='<%# GetLangStr("AlarmMonitor5","列表显示") %>'
                                                MinColumnWidth="90">
                                                <Store>
                                                    <ext:Store ID="StoreAlarm" runat="server">
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
                                                                    <ext:RecordField Name="col29" />
                                                                </Fields>
                                                            </ext:JsonReader>
                                                        </Reader>
                                                    </ext:Store>
                                                </Store>
                                                <ColumnModel ID="ColumnModel3" runat="server">
                                                    <Columns>
                                                        <ext:Column Width="150" DataIndex="col2" Header='<%# GetLangStr("AlarmMonitor6","报警卡口") %>' AutoDataBind="true" Align="Left" />
                                                        <ext:Column Width="80" DataIndex="col3" Header='<%# GetLangStr("AlarmMonitor7","号牌号码") %>' AutoDataBind="true" Align="Center" />
                                                        <ext:Column Width="110" DataIndex="col5" Header='<%# GetLangStr("AlarmMonitor8","号牌种类") %>' AutoDataBind="true" />
                                                        <ext:DateColumn Width="180" DataIndex="col6" Header='<%# GetLangStr("AlarmMonitor9","报警时间") %>' AutoDataBind="true" Align="Center" Format="yyyy-MM-dd HH:mm:ss" />
                                                        <ext:Column Width="100" DataIndex="col8" Header='<%# GetLangStr("AlarmMonitor10","行驶方向") %>' AutoDataBind="true" Align="Center" />
                                                        <ext:Column Width="40" DataIndex="col28" Header='<%# GetLangStr("AlarmMonitor11","布控类型") %>' AutoDataBind="true" Align="Center" />
                                                        <ext:Column Width="100" DataIndex="col19" Header='<%# GetLangStr("AlarmMonitor12","报警类型") %>' AutoDataBind="true" Align="Center" />
                                                        <ext:Column Width="100" DataIndex="col27" Header='<%# GetLangStr("AlarmMonitor13","报警原因") %>' AutoDataBind="true" />
                                                        <ext:Column Width="150" DataIndex="col24" Header='<%# GetLangStr("AlarmMonitor16","处理结果") %>' AutoDataBind="true" />
                                                        <ext:Column ColumnID="Details" Header='<%# GetLangStr("AlarmMonitor14","详细") %>' AutoDataBind="true" Width="50" Align="Center" Fixed="true"
                                                            MenuDisabled="true" Resizable="false">
                                                            <Renderer Fn="DataAmply" />
                                                        </ext:Column>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                                        <DirectEvents>
                                                            <RowSelect OnEvent="SelectAlarm" Buffer="250">
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
                                                    <CellClick OnEvent="ShowDetails" Buffer="250" Failure="Ext.MessageBox.alert('加载失败','提示');">
                                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="={#{GridAlarm}.body}" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="data" Value="params[0].getStore().getAt(params[1]).data" Mode="Raw" />
                                                        </ExtraParams>
                                                    </CellClick>
                                                </DirectEvents>
                                                <View>
                                                    <ext:GridView ID="GroupingView1" runat="server" ForceFit="true">
                                                        <GetRowClass Fn="getRowClass" />
                                                    </ext:GridView>
                                                </View>
                                                <ToolTips>
                                                    <ext:ToolTip
                                                        ID="RowTip"
                                                        runat="server"
                                                        Target="={GridAlarm.getView().mainBody}"
                                                        Delegate=".x-grid3-cell"
                                                        TrackMouse="true">
                                                        <Listeners>
                                                            <Show Fn="showTip" />
                                                        </Listeners>
                                                    </ext:ToolTip>
                                                </ToolTips>
                                            </ext:GridPanel>
                                        </ext:LayoutRow>
                                    </Rows>
                                </ext:RowLayout>
                            </Items>
                        </ext:FormPanel>
                    </Center>
                    <%--                    <East Split="true" AutoHide="true">
                        <ext:TreePanel ID="TreePanel2" runat="server" Title='<%# GetLangStr("AlarmMonitor15","报警类型") %>' Icon="ApplicationSideList" Width="180"
                            Shadow="None" UseArrows="true" AutoScroll="true" Animate="true" EnableDD="true"
                            ContainerScroll="true" RootVisible="false" Collapsed="false">
                            <Listeners>
                                <CheckChange Handler="#{AlarmType}.setValue(getTasks(this), false);" />
                            </Listeners>
                        </ext:TreePanel>
                    </East>--%>
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
    <div id="customEl" class="x-hidden">
        <ext:Panel
            ID="CustomEl1"
            runat="server"
            Border="false"
            AutoDataBind="true"
            Height="120"
            Width="180">
            <Content>
                <ext:Label runat="server" Text='<%# stub %>' />
                <br />
                <ext:Label runat="server" Text='<%# stub1 %>' />
                <br />
                <ext:Label runat="server" Text='<%# stub2 %>' />
                <br />
            </Content>
            <BottomBar>
                <ext:Toolbar runat="server">
                    <Items>
                        <ext:ToolbarTextItem ID="BarLabel" runat="server" />
                        <ext:ToolbarFill runat="server" />
                        <ext:Button ID="ButDeal" runat="server" Icon="Images">
                            <DirectEvents>
                                <Click OnEvent="OnDeal" />
                            </DirectEvents>
                        </ext:Button>
                        <%--ext:Button ID="ButClose" runat="server" Icon="Cancel">
                            <DirectEvents>
                                <Click OnEvent="OnClose" />
                            </DirectEvents>
                        </--%>
                    </Items>
                </ext:Toolbar>
            </BottomBar>
        </ext:Panel>
    </div>
</body>
</html>