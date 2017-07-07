<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FootHold.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.Map.FootHold" %>

<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>落脚点查询</title>

    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="css/custom.css" />

    <link rel="stylesheet" type="text/css" href="../Style/customMap.css" />
    <script language="JavaScript" src="../Scripts/showphoto.js" type="text/javascript"></script>
    <script type="text/javascript">
        var showTip = function () {
            var rowIndex = GridStation.view.findRowIndex(this.triggerElement),
                cellIndex = GridStation.view.findCellIndex(this.triggerElement),
                record = StoreInfo.getAt(rowIndex),
                fieldName = GridStation.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
        var changeTime = function (value) {
            return value + "秒";
        };
    </script>
    <style type="text/css">
        body, html {
            font-family: Arial,Verdana;
            font-size: 13px;
            margin: 0;
            overflow: hidden;
        }

        .x-shadow {
            display: none !important;
        }

        #cboplate_Panel1 {
            background: white;
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

    <!--梁引入如下js和css-->
    <script type="text/javascript" src="js/Qquery1.91-min.js" charset="UTF-8"></script>

    <script type="text/javascript" src="js/jquery.nicescroll.js" charset="UTF-8"></script>
    <link rel="stylesheet" type="text/css" href="css/Ui-skin.css" />
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <!--梁引入如下js和css 结束-->

    <script type="text/javascript">

        var change = function (obj, value) {
            obj.setValue(value.toUpperCase());
        };
    </script>
    <script type="text/javascript">
        $(function () {
            $("body").delegate("#txtplate", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#ext-gen57").click();
                }
            })
        })
    </script>

    <script type="text/javascript">
        function clearTime(start, end) {
            document.getElementById("start").innerText = start;
            document.getElementById("end").innerText = end;
            cbocllx.triggers[0].hide();
        }
    </script>
    <style type="text/css">
        body .ui-right-wrap .x-grid3-body .x-grid3-td-numberer {
            background-image: none !important;
            background-image: none;
        }
        /*body .ui-right-wrap checkbox{background-image:none!important; background-image:none;}*/
    </style>
</head>
<body onload="MapCenter();">
    <form id="form2" runat="server">
        <ext:Store ID="cllx" runat="server">
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
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="FootHold" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="border" Cls="new-layout">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Title='<%# GetLangStr("FootHold1","地图浏览")%>' AutoRender="true" Region="Center"
                    Header="false" Height="800" Width="1000" Cls="map-bg">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar12" runat="server" Cls="Top-Bar top-toolbar">
                            <Items>
                                <ext:Button ID="Linkreload" runat="server" Icon="Reload" Text='<%# GetLangStr("FootHold2","重载地图")%>'>
                                    <Listeners>
                                        <Click Handler="window.location.reload();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Button ID="TbutMove" runat="server" Icon="Erase" Text='<%# GetLangStr("FootHold3","清除")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.ClearCircle();BMAP.ClearLabel();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButCenetr" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("FootHold4","中心点")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.GotoCenter();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButRanging" runat="server" Icon="PencilGo" Text='<%# GetLangStr("FootHold5","测距")%>'>
                                    <Listeners>
                                        <Click Handler=" BMAP.DistanceTool();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButPlan" runat="server" Icon="Vector" Text='<%# GetLangStr("FootHold6","测面积")%>'>
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
                <ext:FormPanel ID="FormPanel2" Cls="ui-right-wrap Middle-arrow" Padding="0" Region="east" runat="server"
                    Split="true" Collapsible="true" RowHeight="1" Width="400" Layout="Accordion">
                    <TopBar>
                        <ext:Toolbar ID="toolbarrquery" runat="server">
                            <Items>
                                <ext:Panel ID="Pantrack" runat="server" Padding="4" Width="400" AutoHeight="true">
                                    <TopBar>
                                        <ext:Toolbar runat="server" Layout="ContainerLayout">
                                            <Items>
                                                <ext:Toolbar runat="server">
                                                    <Items>
                                                        <ext:Label Text='<%# GetLangStr("FootHold7","号牌号码:") %>' Width="82" runat="server" StyleSpec="margin-left:5px;" />
                                                        <ext:Panel ID="Panel4" runat="server" Height="29">
                                                            <Content>
                                                                <veh:VehicleHead ID="cboplate" runat="server" />
                                                            </Content>
                                                        </ext:Panel>
                                                        <ext:TextField ID="txtplate" Width="82" runat="server" Cls="ui-input" EmptyText='<%# GetLangStr("FootHold8","6位号牌号码") %>' MaxLength="6">
                                                            <Listeners>
                                                                <Change Fn="change" />
                                                            </Listeners>
                                                        </ext:TextField>
                                                        <ext:Label ID="Label2" runat="server" Text='<%# GetLangStr("FootHold9","号牌种类:") %>' StyleSpec="margin-left:10px;">
                                                        </ext:Label>
                                                        <ext:ComboBox ID="cbocllx" Height="29" Editable="false"
                                                            runat="server" BlankText='<%# GetLangStr("FootHold10","请选择...")%>' TypeAhead="true" SelectOnFocus="true"
                                                            EmptyText='<%# GetLangStr("FootHold10","请选择...")%>' DisplayField="CODEDESC" ValueField="CODE" Width="115" StoreID="cllx" Region="West" Cls="ui-input">
                                                            <Triggers>
                                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("FootHold11","清除选中") %>' AutoDataBind="true" />
                                                            </Triggers>
                                                            <Listeners>
                                                                <Select Handler="this.triggers[0].show();" />
                                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                    </Items>
                                                </ext:Toolbar>
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <Items>
                                        <ext:Panel ID="Panel1" runat="server" Title='<%# GetLangStr("FootHold12","高级查询")%>' Layout="RowLayout" Height="120" Collapsed="true" FormGroup="true">
                                            <Items>
                                                <ext:Panel runat="server" X="0" Y="20" Height="65px">
                                                    <Content>
                                                        <div id="selectDate">
                                                            <span style="margin-left: 5px; float: left; height: 29px; line-height: 24px!important; text-align: center"><%# GetLangStr("FootHold13","开始时间:")%></span>
                                                            <li runat="server" class="laydate-icon" id="start" style="width: 292px; float: left; list-style: none; cursor: pointer; height: 22px; line-height: 22px!important"></li>
                                                        </div>
                                                        <div class="clear" style="clear: both; height: 5px;"></div>
                                                        <div>
                                                            <span style="margin-left: 5px; float: left; height: 29px; line-height: 24px!important; text-align: center"><%# GetLangStr("FootHold14","结束时间:")%></span>
                                                            <li runat="server" class="laydate-icon" id="end" style="width: 292px; float: left; list-style: none; cursor: pointer; height: 22px; line-height: 22px!important"></li>
                                                        </div>
                                                    </Content>
                                                </ext:Panel>
                                                <ext:Panel runat="server" Height="5" />
                                                <ext:Panel runat="server" Layout="ColumnLayout" Height="24px">
                                                    <Items>
                                                        <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("FootHold15","落脚次数:")%>' StyleSpec="margin-left:5px;" Height="24px" />
                                                        <ext:TextField ID="txtljcs" Height="24" Width="68px" runat="server" Cls="ui-input" />
                                                        <ext:Label ID="lbljcs" runat="server" Text='<%# GetLangStr("FootHold16","次") %>' />
                                                        <ext:Label ID="Label4" runat="server" Text='<%# GetLangStr("FootHold17","落脚时长:") %>' StyleSpec="margin-left:52px;" Height="24px" />
                                                        <ext:TextField ID="txtljsc" Height="24" Width="68px" runat="server" Cls="ui-input" />
                                                        <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("FootHold18","秒/次 ") %>' />
                                                    </Items>
                                                </ext:Panel>
                                            </Items>
                                            <Listeners>
                                                <Expand Handler="FootHold.panelopen();" />
                                                <Collapse Handler="FootHold.panelclose();" />
                                            </Listeners>
                                        </ext:Panel>
                                        <ext:Panel FormGroup="true" runat="server" Layout="ColumnLayout" AutoHeight="true" ButtonAlign="Center">
                                            <Buttons>
                                                <ext:Button ID="ButAddgrid" runat="server" Text='<%# GetLangStr("FootHold19","查询")%>' Width="80" IconCls="ui-input w-80px search-btn border-radius-30">
                                                    <DirectEvents>
                                                        <Click OnEvent="unnamed_event">
                                                            <EventMask ShowMask="true" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButReset" runat="server" Text='<%# GetLangStr("FootHold20","重置")%>' Width="80" IconCls="ui-input w-80px search-btn border-radius-30">
                                                    <Listeners>
                                                        <Click Handler="FootHold.ButResetClick();" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Buttons>
                                        </ext:Panel>
                                    </Items>
                                </ext:Panel>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridStation" runat="server" StripeRows="true"
                            AutoHeight="false" AutoScroll="true" Cls="data-list-container table-ui display-table w-100 Hide-panel-header">
                            <Store>
                                <ext:Store ID="StoreInfo" runat="server">
                                    <Reader>
                                        <ext:JsonReader IDProperty="STATION_ID">
                                            <Fields>
                                                <ext:RecordField Name="kkid" />
                                                <ext:RecordField Name="kkmc" />
                                                <ext:RecordField Name="xpoint" />
                                                <ext:RecordField Name="ypoint" />
                                                <ext:RecordField Name="ljsj" />
                                                <ext:RecordField Name="ljcs" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>

                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Header='<%# GetLangStr("FootHold21","序号")%>' AutoDataBind="true" Width="50" Align="Center" Editable="false" />
                                    <ext:Column Header='<%# GetLangStr("FootHold22","落脚点名称")%>' AutoDataBind="true" DataIndex="kkmc" Width="100" Align="Center" Editable="false" />
                                    <ext:Column Header='<%# GetLangStr("FootHold23","落脚次数")%>' AutoDataBind="true" DataIndex="ljcs" Width="100" Align="Center" Editable="false" />
                                    <ext:Column Header='<%# GetLangStr("FootHold24","累计时间")%>' AutoDataBind="true" DataIndex="ljsj" Width="100" Align="Center" Editable="false">
                                        <Renderer Fn="changeTime" />
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("FootHold25","x")%>' AutoDataBind="true" DataIndex="xpoint" Hidden="true" Width="100" Align="Center" Editable="false" />
                                    <ext:Column Header='<%# GetLangStr("FootHold26","y")%>' AutoDataBind="true" DataIndex="ypoint" Hidden="true" Width="100" Align="Center" Editable="false" />
                                    <ext:Column ColumnID="ID" Header='<%# GetLangStr("FootHold27","卡口ID")%>' AutoDataBind="true" DataIndex="kkid" Hidden="true" Width="200" Align="Center" Editable="false" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel runat="server">
                                    <Listeners>
                                        <RowSelect Handler="FootHold.SelectRow(record.data.xpoint,record.data.ypoint,record.data.kkid,record.data.kkmc,record.data.ljcs)" />
                                    </Listeners>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <View>

                                <ext:GridView runat="server" StripeRows="true" TrackOver="true" />
                            </View>
                            <ToolTips>
                                <ext:ToolTip
                                    ID="RowTip"
                                    runat="server"
                                    Target="={GridStation.getView().mainBody}"
                                    Delegate=".x-grid3-cell"
                                    TrackMouse="true">
                                    <Listeners>
                                        <Show Fn="showTip" />
                                    </Listeners>
                                </ext:ToolTip>
                            </ToolTips>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>

    <script type="text/javascript">
        function isScroll() {
            $('.OverCar-data-list').niceScroll({
                cursorcolor: "#7683a4",//#CC0071 光标颜色
                cursoropacitymax: 1, //改变不透明度非常光标处于活动状态（scrollabar“可见”状态），范围从1到0
                touchbehavior: false, //使光标拖动滚动像在台式电脑触摸设备
                cursorwidth: "3px", //像素光标的宽度
                cursorborder: "0", // 	游标边框css定义
                cursorborderradius: "5px",//以像素为光标边界半径
                autohidemode: true //是否隐藏滚动条
            }).resize();
        }
        function isScrollRight() {
            $('.x-grid3-scroller').niceScroll({
                cursorcolor: "#7683a4",//#CC0071 光标颜色
                cursoropacitymax: 1, //改变不透明度非常光标处于活动状态（scrollabar“可见”状态），范围从1到0
                touchbehavior: false, //使光标拖动滚动像在台式电脑触摸设备
                cursorwidth: "3px", //像素光标的宽度
                cursorborder: "0", // 	游标边框css定义
                cursorborderradius: "5px",//以像素为光标边界半径
                autohidemode: false //是否隐藏滚动条
            });
        }
    </script>
</body>
</html>

<script type="text/javascript">
    laydate.skin('danlan');
    var start = {
        elem: '#start',
        format: 'YYYY-MM-DD hh:mm:ss',
        //min: laydate.now(), //设定最小日期为当前日期
        max: laydate.now(),//'2099-06-16 23:59:59', //最大日期
        istime: true,
        istoday: false,
        isclear: false,
        choose: function (datas) {
            end.min = datas; //开始日选好后，重置结束日的最小日期
            end.start = datas //将结束日的初始值设定为开始日
            $("#end").click();//开始时间选中后，自动弹出结束时间
            var tt = $("#start").html();
            FootHold.GetDateTime(true, tt);
            //alert(tt);
        }
    };
    var end = {
        elem: '#end',
        format: 'YYYY-MM-DD hh:mm:ss',
        min: laydate.now(),
        max: laydate.now(),//'2099-06-16 23:59:59',
        istime: true,
        istoday: false,
        isclear: false,
        choose: function (datas) {
            start.max = datas; //结束日选好后，重置开始日的最大日期
            var tt = $("#end").html();
            FootHold.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>