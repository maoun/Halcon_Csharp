<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PathCarQuery.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Map.PathCarQuery" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>轨迹查询</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" href="../Css/chooser.css" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
      <link  rel="Stylesheet" href="../Styles/datetime.css" type="text/css" />
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

        #cboplate_Panel1 table {
            border-radius: 0;
        }
    </style>
    <style type="text/css">
        body .ui-right-wrap .x-grid3-body .x-grid3-td-checker {
            background-image: none !important;
            background-image: none;
        }
    </style>
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="UTF-8"></script>
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
    <script type="text/jscript">
        function DrawLine(x1, y1, x2, y2, content, num, xh, sjjg) {
            var n = sjjg * num;
            setTimeout(function () {
                var points = x1 + ',' + y1 + '|' + x2 + ',' + y2;

                PathCarQuery.setrow(xh);
                BMAP.openWindow(content, x2, y2);
                if (x1 != "") {
                    BMAP.addPolyline2('#ff0000', points, '');
                }
            }, n);

        }
    </script>
    <!--梁引入如下js和css-->
    <script type="text/javascript" src="js/Qquery1.91-min.js" charset="UTF-8"></script>
    <script language="JavaScript" src="../Scripts/showphoto.js" type="text/javascript" charset="UTF-8"></script>
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
    </script>
    <script type="text/javascript">
        //轨迹查询回车事件
        $(function () {
            $("body").delegate("#txtplate", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#cbocllx").click();
                }
            })
        })
    </script>
    <script type="text/javascript">

        var change = function (obj, value) {
            obj.setValue(value.toUpperCase());
        };
    </script>
    <script type="text/javascript">
        function clearTime(start, end) {
            document.getElementById("start").innerText = start;
            document.getElementById("end").innerText = end;
            cbocllx.triggers[0].hide();
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $("body").delegate("#txtplate", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#ext-gen50").click();
                }
            })
        })
    </script>
    <script type="text/javascript">
        var IMGDIR = '../images/sets';
        var attackevasive = '0';
        var gid = 0;
        var fid = parseInt('0');
        var tid = parseInt('0');
    </script>
</head>
<body onload="MapCenter();">
    <form id="form2" runat="server">
                <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PathCarQuery" />
        <div id="append_parent">
        </div>
        <ext:Hidden ID="curpage" runat="server" />
        <ext:Hidden ID="totalpage" runat="server" />
        <ext:Store ID="Store1" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store2" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="DEPARTID" />
                        <ext:RecordField Name="DEPARTNAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
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
        <ext:Viewport ID="Viewport1" runat="server" Layout="border" Cls="new-layout">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Title='<%# GetLangStr("PathCarQuery1","地图浏览")%>' AutoRender="true" Region="Center"
                    Header="false" Height="800" Width="1000" IconCls="temp/map.png" Cls="map-bg">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar12" runat="server" Cls="top-toolbar">
                            <Items>
                                <ext:Button ID="Linkreload" runat="server" Icon="Reload" Text='<%# GetLangStr("PathCarQuery2","重载地图")%>'>
                                    <Listeners>
                                        <Click Handler="window.location.reload();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Button ID="TbutMove" runat="server" Icon="Erase" Text='<%# GetLangStr("PathCarQuery3","清除")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.ClearCircle();BMAP.ClearLabel();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButCenetr" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("PathCarQuery4","中心点")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.GotoCenter();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButRanging" runat="server" Icon="PencilGo" Text='<%# GetLangStr("PathCarQuery5","测距")%>'>
                                    <Listeners>
                                        <Click Handler=" BMAP.DistanceTool();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButPlan" runat="server" Icon="Vector" Text='<%# GetLangStr("PathCarQuery6","测面积")%>'>
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
            <ext:FormPanel ID="FormPanel2" Cls="ui-right-wrap Middle-arrow" Padding="5" Region="east" runat="server" 
                    Split="true" RowHeight="1" Width="400" Layout="FitLayout">
                    <TopBar>
                        <ext:Toolbar ID="toolbar1" runat="server" Layout="ContainerLayout">
                            <Items>
                                <ext:Toolbar ID="toolbarrquery" runat="server">
                                    <Items>
                                        <ext:Panel ID="Pantrack" runat="server" Padding="4"   Layout="Absolute" Icon="Car" Width="400" Height="200">
                                            <Items>
                                                <ext:Checkbox ID="showimag" Hidden="true" BoxLabel='<%# GetLangStr("PathCarQuery7","图片显示") %>' 
                                                    StyleSpec="margin-left: 10px;" X="5" Y="0" Width="145" runat="server" Cls="font-16  check-active"></ext:Checkbox>
                                                <ext:Panel runat="server" X="0" Y="0">
                                                    <Content>
                                                        <div id="selectDate">
                                                            <span class="laydate-span" style="margin-left: 5px; height: 24px;"><%# GetLangStr("PathCarQuery8","开始时间:")%></span>
                                                            <li runat="server" class="laydate-icon" id="start" style="width: 280px; margin-left: 7px;  height: 22px; "></li>
                                                        </div>
                                                        <div class="clear" style="clear: both"></div>
                                                        <div style="margin-top: 15px">
                                                            <span  class="laydate-span" style=" margin-left: 5px; height: 24px; "><%# GetLangStr("PathCarQuery9","结束时间:")%></span>
                                                            <li runat="server" class="laydate-icon" id="end" style="width: 280px; margin-left: 7px;   height: 22px;"></li>
                                                        </div>
                                                    </Content>
                                                </ext:Panel>
                                                <ext:Label X="5" Y="82" Text='<%# GetLangStr("PathCarQuery10","号牌号码:") %>' runat="server" />
                                                <ext:Panel ID="Panel4" X="80" Y="80" runat="server" Height="29" StyleSpec="margin-left:-3px">
                                                    <Content>
                                                        <veh:VehicleHead ID="cboplate" runat="server" />
                                                    </Content>
                                                </ext:Panel>
                                                <ext:TextField ID="txtplate" X="123" Y="80" Width="82" Height="29" TabIndex="1" runat="server" MaxLength="6"
                                                     Cls="ui-input" StyleSpec="margin-left:0px;" EmptyText='<%# GetLangStr("PathCarQuery11","6位号牌号码") %>'>
                                                    <Listeners>
                                                        <Change Fn="change" />
                                                    </Listeners>
                                                </ext:TextField>
                                                <ext:ComboBox ID="cbocllx" X="210" Y="80" StoreID="cllx" Editable="false" TabIndex="2" FieldLabel='<%# GetLangStr("PathCarQuery12","号牌种类") %>' 
                                                    runat="server" BlankText='<%# GetLangStr("PathCarQuery13","请选择")%>' TypeAhead="true" SelectOnFocus="true"
                                                    EmptyText='<%# GetLangStr("PathCarQuery14","请选择...")%>' LabelWidth="68" DisplayField="CODEDESC"
                                                     ValueField="CODE" Width="170" Region="West" Cls="ui-input">
                                                    <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PathCarQuery15","清除选中") %>' AutoDataBind="true"  />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.triggers[0].show();" />
                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                    </Listeners>

                                                    <Items>
                                                    </Items>
                                                </ext:ComboBox>
                                                <ext:Button ID="ButPlay" runat="server" X="10" Y="125" Cls="" Hidden="true" Width="60" Region="West" IconCls="ui-input w-80px search-btn border-radius-30">
                                                    <%--<DirectEvents>
                                                <Click OnEvent="PathCarQuery.ButPlayClick" />
                                            </DirectEvents>--%>
                                                    <Listeners>
                                                        <Click Handler="PathCarQuery.ButPlayClick();" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Button ID="ButAddgrid" runat="server" X="120" Y="125" Cls="" Text='<%# GetLangStr("PathCarQuery16","查找")%>' Width="80" Region="West" IconCls="ui-input w-80px search-btn border-radius-30">
                                                    <DirectEvents>
                                                        <Click OnEvent="unnamed_event">
                                                            <EventMask ShowMask="true" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButShowMark" runat="server" X="230" Y="125" Text='<%# GetLangStr("PathCarQuery80","重置")%>' Width="80" Region="West" IconCls="ui-input w-80px search-btn border-radius-30">
                                                    <Listeners>
                                                        <Click Handler="PathCarQuery.Reset();" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:RadioGroup X="10" Y="170" ID="rdgpsd" Hidden="true" runat="server" FieldLabel='<%# GetLangStr("PathCarQuery17","播放速度") %>' StyleSpec="margin-left: 10px;">
                                                    <Items>
                                                        <ext:Radio ID="rd01" runat="server" BoxLabel='<%# GetLangStr("PathCarQuery18","慢速") %>' StyleSpec="margin-left: 10px;" />
                                                        <ext:Radio ID="rd02" runat="server" BoxLabel='<%# GetLangStr("PathCarQuery19","正常") %>' StyleSpec="margin-left: 10px;" Checked="true" />
                                                        <ext:Radio ID="rd03" runat="server" BoxLabel='<%# GetLangStr("PathCarQuery20","快速") %>' StyleSpec="margin-left: 10px;" />
                                                    </Items>
                                                </ext:RadioGroup>
                                                <ext:Checkbox ID="chkMapLine" Hidden="true" BoxLabel='<%# GetLangStr("PathCarQuery21","轨迹播放") %>' StyleSpec="margin-left: 10px;" X="5" Y="0" Width="145" Cls="font-16 check-active" runat="server">
                                                </ext:Checkbox>
                                            </Items>
                                        </ext:Panel>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:Button ID="ButFirst" runat="server" Text='<%# GetLangStr("PathCarQuery22","首页")%>'>
                                            <DirectEvents>
                                                <Click OnEvent="TbutFisrt" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButLast" runat="server" Icon="ResultsetPrevious">
                                            <DirectEvents>
                                                <Click OnEvent="TbutLast" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButNext" runat="server" Icon="ResultsetNext">
                                            <DirectEvents>
                                                <Click OnEvent="TbutNext" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButEnd" runat="server" Text='<%# GetLangStr("PathCarQuery23","尾页")%>'>
                                            <DirectEvents>
                                                <Click OnEvent="TbutEnd" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                                        <ext:Label ID="labpage" runat="server" Text='<%# GetLangStr("PathCarQuery24","当前0页,共0页")%>' StyleSpec="margin-left: 10px;" Width="600" />
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:Panel ID="Panel5" runat="server" Layout="RowLayout" RowHeight="1" AutoScroll="true">
                            <Items>
                                <ext:GridPanel ID="GridRoadManager"  RowHeight="1" runat="server" StripeRows="true" 
                                    AutoHeight="false" AutoExpandColumn="sj" AutoScroll="true" Cls="data-list-container table-ui display-table w-100 Hide-panel-header">
                                    <Store>
                                        <ext:Store ID="StoreInfo" runat="server">
                                            <Reader>
                                                <ext:JsonReader IDProperty="gwsj">
                                                    <Fields>
                                                        <ext:RecordField Name="hphm" />
                                                        <ext:RecordField Name="kkid" />
                                                        <ext:RecordField Name="lkmc" />
                                                        <ext:RecordField Name="cdbh" />
                                                        <ext:RecordField Name="fxmc" />
                                                        <ext:RecordField Name="xpoint" />
                                                        <ext:RecordField Name="ypoint" />
                                                        <ext:RecordField Name="gwsj" />
                                                        <ext:RecordField Name="zjwj" />
                                                        <ext:RecordField Name="xh" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <%--<ext:Column Header="车牌图" Width="80" DataIndex="STATION_NAME" Align="Center" Editable="false" />--%>
                                            <%--<ext:Column Header="号牌号码" Width="80" DataIndex="hphm" Align="Center" />--%>
                                             <%--<ext:RowNumbererColumn runat="server"></ext:RowNumbererColumn>--%>
                                            <ext:Column Header='<%# GetLangStr("PathCarQuery25","序号")%>' AutoDataBind="true" Width="55" DataIndex="xh" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("PathCarQuery26","道路名称")%>' AutoDataBind="true" Width="120" DataIndex="lkmc" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("PathCarQuery27","车道")%>' AutoDataBind="true" Width="60" DataIndex="cdbh" Hidden="true" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("PathCarQuery28","方向")%>' AutoDataBind="true" Width="80" DataIndex="fxmc" Align="Center" />
                                            <ext:Column ColumnID="sj" Header='<%# GetLangStr("PathCarQuery29","过往时间")%>' AutoDataBind="true" Width="150" DataIndex="gwsj" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("PathCarQuery30","x")%>' AutoDataBind="true" Width="60" Hidden="true" DataIndex="xpoint" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("PathCarQuery31","y")%>' AutoDataBind="true" Width="60" Hidden="true" DataIndex="ypoint" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("PathCarQuery32","文件")%>' AutoDataBind="true" Width="60" Hidden="true" DataIndex="zjwj" Align="Center" />
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>

                                        <ext:RowSelectionModel runat="server">
                                            <Listeners>
                                                <RowSelect Handler="PathCarQuery.SelectRow(record.data.xpoint,record.data.ypoint,record.data.hphm,record.data.zjwj)" />
                                            </Listeners>
                                        </ext:RowSelectionModel>
                                    </SelectionModel>

                                    <View>

                                        <ext:GridView runat="server" ForceFit="true" StripeRows="true" TrackOver="true" />
                                    </View>
                                    <ToolTips>
                                        <ext:ToolTip
                                            ID="RowTip"
                                            runat="server"
                                            Target="={GridRoadManager.getView().mainBody}"
                                            Delegate=".x-grid3-cell"
                                            TrackMouse="true">
                                            <Listeners>
                                                <Show Fn="showTip" />
                                            </Listeners>
                                        </ext:ToolTip>
                                    </ToolTips>
                                </ext:GridPanel>
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
    var showTip = function () {
        var rowIndex = GridRoadManager.view.findRowIndex(this.triggerElement),
            cellIndex = GridRoadManager.view.findCellIndex(this.triggerElement),
            record = StoreInfo.getAt(rowIndex),
            fieldName = GridRoadManager.getColumnModel().getDataIndex(cellIndex),
            data = record.get(fieldName);

        this.body.dom.innerHTML = data;
    };
</script>
<script type="text/javascript">
    $(function () {
        $('.OverCar-data-list').niceScroll({
            cursorcolor: "#7683a4",//#CC0071 光标颜色
            cursoropacitymax: 1, //改变不透明度非常光标处于活动状态（scrollabar“可见”状态），范围从1到0
            touchbehavior: false, //使光标拖动滚动像在台式电脑触摸设备
            cursorwidth: "4px", //像素光标的宽度
            cursorborder: "0", // 	游标边框css定义
            cursorborderradius: "5px",//以像素为光标边界半径
            autohidemode: false //是否隐藏滚动条
        });
    });
</script>

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
            PathCarQuery.GetDateTime(true, tt);
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
            PathCarQuery.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>