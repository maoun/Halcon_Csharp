<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dispatched.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Map.Dispatched" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>一键布控</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="css/custom.css" />
    <link rel="stylesheet" type="text/css" href="../Style/customMap.css" />
    <link  rel="Stylesheet" type="text/css" href="../Styles/hphm/autohphm.css"/>
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

        //模糊查询号码
        function cgtxt(name, value) {
            var n = parseInt(name.substring(name.length - 1)) + 1;
            var m = parseInt(name.substring(name.length - 1)) - 1;
            var keycode = cgtxt.caller.arguments[0].which;
            var na = "haopai_name" + n;
            var nam = "num" + m;
            if (name == "haopai_name6") {
                $("input[id='haopai_name1']").focus();
            }
            if (keycode >= 65 && keycode <= 90 || keycode >= 48 && keycode <= 57 || keycode >= 96 && keycode <= 105) {
                if (value.length == 1) {
                    if (keycode >= 65 && keycode <= 90) {//找到输入是小写字母的ascII码的范围
                        $("input[name='" + name + "']").val(String.fromCharCode(keycode));//转换
                    }
                    if (name != "haopai_name6") {
                        $("input[id='" + na + "']").focus();
                    }
                }
            } else if (keycode == 8) {
                $("input[name='" + nam + "']").focus();
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
        //一键布控回车事件
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
        $(function () {
            $("body").delegate("#txtplate", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#ext-gen46").click();
                }
            })
        })
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridStation.view.findRowIndex(this.triggerElement),
                cellIndex = GridStation.view.findCellIndex(this.triggerElement),
                record = StoreInfo.getAt(rowIndex),
                fieldName = GridStation.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body onload="MapCenter();">
    <ext:Hidden ID="yxtime" runat="server" />
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
        <ext:Store ID="StoreMdlx" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="Dispatched" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="border" Cls="new-layout">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Title='<%# GetLangStr("Dispatched1","地图浏览")%>' AutoRender="true" Region="Center"
                    Header="false" Height="800" Width="1000" Cls="map-bg">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar12" runat="server" Cls="top-toolbar">
                            <Items>
                                <ext:Button ID="Linkreload" runat="server" Icon="Reload" Text='<%# GetLangStr("Dispatched2","重载地图")%>'>
                                    <Listeners>
                                        <Click Handler="window.location.reload();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Button ID="TbutMove" runat="server" Icon="Erase" Text='<%# GetLangStr("Dispatched3","清除")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.ClearCircle();BMAP.ClearLabel();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButCenetr" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("Dispatched4","中心点")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.GotoCenter();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButRanging" runat="server" Icon="PencilGo" Text='<%# GetLangStr("Dispatched5","测距")%>'>
                                    <Listeners>
                                        <Click Handler=" BMAP.DistanceTool();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButPlan" runat="server" Icon="Vector" Text='<%# GetLangStr("Dispatched6","测面积")%>'>
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
                <ext:FormPanel ID="FormPanel2" Cls="ui-right-wrap Middle-arrow" Padding="5" Region="east" runat="server"
                    Split="true" Collapsible="true" RowHeight="1" Width="400" Layout="Accordion" MonitorValid="true">
                    <TopBar>
                        <ext:Toolbar ID="toolbarrquery" runat="server">
                            <Items>
                                <ext:FormPanel ID="Pantrack" runat="server" Padding="4" Title="" Layout="Absolute"
                                    Icon="Car" Width="400" Height="270" ButtonAlign="Center" MonitorValid="true">
                                    <Items>
                                        <ext:LinkButton runat="server" X="5" Y="0" IconCls="custom-iconbiao"
                                            Text='<%# GetLangStr("Dispatched7","布控范围标注(请在左侧地图上标注)")%>'>
                                            <Listeners>
                                                <Click Handler=" BMAP.ClearCircle();BMAP.Clear();BMAP.SaveAreaMarker({Operate:'Dispatched'});;" />
                                            </Listeners>
                                        </ext:LinkButton>
                                        <ext:Label X="10" Y="33" Text='<%# GetLangStr("Dispatched8","号牌号码:") %>' runat="server" />
                                        <ext:Panel ID="Panel4" X="85" Y="30"  Width="120" runat="server" Height="28" StyleSpec="margin-left:2px;">
                                            <Content>
                                                <veh:VehicleHead ID="cboplate" runat="server" />
                                            </Content>
                                        </ext:Panel>
                                         <ext:Panel runat="server" Layout="ColumnLayout" X="135" Y="30"  Height="28">
                                            <Items>
                                                <ext:TextField ID="txtplate" runat="server" Hidden="false" X="130" Y="30" Width="140" EmptyText='<%# GetLangStr("Dispatched23","六位号牌号码") %>' MaxLength="6">
                                                    <Listeners>
                                                        <Change Fn="change" />
                                                    </Listeners>
                                                </ext:TextField>
                                                <ext:Panel runat="server" ID="pnhphm" Hidden="true" X="130" Y="30" Width="140"  Height="28">
                                                    <Content>
                                                        <input name="num1" class="haopai_name1" value="" id="haopai_name1" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                        <input name="num2" class="haopai_name2" value="" id="haopai_name2" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                        <input name="num3" class="haopai_name3" value="" id="haopai_name3" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                        <input name="num4" class="haopai_name4" value="" id="haopai_name4" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                        <input name="num5" class="haopai_name5" value="" id="haopai_name5" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                        <input name="num6" class="haopai_name6" value="" id="haopai_name6" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                    </Content>
                                                </ext:Panel>
                                            </Items>
                                        </ext:Panel>
                                        <ext:Checkbox runat="server" ID="ChkLike" Checked="false" BoxLabel='<%# GetLangStr("Dispatched22","模糊布控") %>' StyleSpec="margin-left:2px;" X="280" Y="30" Width="60" Cls="font-16 check-active">
                                            <DirectEvents>
                                                <Check OnEvent="changtype" />
                                            </DirectEvents>
                                        </ext:Checkbox>
                                        <ext:ComboBox ID="cbocllx" X="10" Y="70" LabelWidth="70" Editable="false" FieldLabel='<%# GetLangStr("Dispatched9","号牌种类") %>' runat="server" BlankText='<%# GetLangStr("Dispatched10","请选择")%>' TypeAhead="true" SelectOnFocus="true"
                                            EmptyText='<%# GetLangStr("Dispatched11","请选择...")%>' DisplayField="CODEDESC"
                                            ValueField="CODE" Width="175" StoreID="cllx" Region="West" Cls="ui-input" AllowBlank="false">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("Dispatched24","清除选中")%>' AutoDataBind="true" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:ComboBox runat="server" X="190" Y="70" LabelWidth="70" FieldLabel='<%# GetLangStr("Dispatched25","布控类型")%>' AutoDataBind="true"  ID="cmbMdlx" StoreID="StoreMdlx" Editable="false"
                                            DisplayField="CODEDESC" ValueField="CODE" EmptyText='<%# GetLangStr("Dispatched26","选择比对类型")%>' 
                                            Width="200" AllowBlank="false">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("Dispatched24","清除选中")%>' AutoDataBind="true" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:Panel runat="server" X="10" Y="110">
                                            <Content>
                                                <div id="selectDate">
                                                    <span style="float: left; margin-left:2px; height: 24px; line-height: 24px!important; text-align: center"><%# GetLangStr("Dispatched13","有效时间:")%></span>
                                                    <li class="laydate-icon" runat="server" id="datestart" style="overflow: hidden; width: 275px; margin-left: 11px; float: left; list-style: none; cursor: pointer; height: 28px; line-height: 22px!important"></li>
                                                </div>
                                            </Content>
                                        </ext:Panel>
                                        <ext:TextField ID="txtbdyy" StyleSpec="margin-left: 2px;" FieldLabel='<%# GetLangStr("Dispatched12","对比原因") %>'
                                            X="10" Y="148" Width="377" LabelWidth="70" runat="server" Cls="ui-input" AllowBlank="false">
                                        </ext:TextField>
                                        <ext:TextField ID="txtbkr" StyleSpec="margin-left: 2px;" FieldLabel='<%# GetLangStr("Dispatched20","布控联系人") %>'
                                            X="10" Y="188" Width="180" LabelWidth="80" runat="server" Cls="ui-input" AllowBlank="false">
                                        </ext:TextField>
                                        <ext:TextField ID="txtlxdh" StyleSpec="margin-left: 2px;" FieldLabel='<%# GetLangStr("Dispatched21","联系电话") %>'
                                            X="200" Y="188" Width="180" LabelWidth="70" runat="server" Cls="ui-input" AllowBlank="false">
                                        </ext:TextField>
                                    </Items>
                                    <Listeners>
                                        <ClientValidation Handler="#{ButAddgrid}.setDisabled(!valid);" />
                                    </Listeners>
                                    <Buttons>
                                        <ext:Button ID="ButAddgrid" runat="server" Text='<%# GetLangStr("Dispatched14","布控")%>'
                                            IconCls="ui-input w-80px search-btn border-radius-30">
                                            <DirectEvents>
                                                <Click OnEvent="ButQueryClick">
                                                    <EventMask ShowMask="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButReset" runat="server" Text="重新布控"
                                            IconCls="ui-input w-80px search-btn border-radius-30" Hidden="true">
                                            <DirectEvents>
                                                <Click OnEvent="ButResetClick">
                                                    <EventMask ShowMask="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Buttons>
                                </ext:FormPanel>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridStation" runat="server" StripeRows="true" Title="" TrackMouseOver="true"
                            Collapsible="true" AutoHeight="false" AutoScroll="true" Cls="data-list-container table-ui display-table w-100 Hide-panel-header">
                            <Store>
                                <ext:Store ID="StoreInfo" runat="server">
                                    <Reader>
                                        <ext:JsonReader IDProperty="STATION_ID">
                                            <Fields>
                                                <ext:RecordField Name="STATION_ID" />
                                                <ext:RecordField Name="STATION_NAME" />
                                                <ext:RecordField Name="xpoint" />
                                                <ext:RecordField Name="ypoint" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <%--<ext:RowNumbererColumn Header="序号" Width="50" Align="Center" Editable="false" />--%>
                                    <ext:Column Header='<%# GetLangStr("Dispatched15","卡口名称")%>' AutoDataBind="true" DataIndex="STATION_NAME" Width="350" Align="Center" Editable="false" />
                                    <ext:Column Header='<%# GetLangStr("Dispatched16","x")%>' AutoDataBind="true" DataIndex="xpoint" Hidden="true" Width="100" Align="Center" Editable="false" />
                                    <ext:Column Header='<%# GetLangStr("Dispatched17","y")%>' AutoDataBind="true" DataIndex="ypoint" Hidden="true" Width="100" Align="Center" Editable="false" />
                                    <ext:Column ColumnID="ID" Header='<%# GetLangStr("Dispatched18","卡口ID")%>' AutoDataBind="true" DataIndex="STATION_ID" Hidden="true" Width="200" Align="Center" Editable="false" />
                                </Columns>
                            </ColumnModel>
                            <Listeners>
                                <GroupCommand Handler="if(command === 'SelectGroup'){ this.getSelectionModel().selectRecords(records, true); return;} Ext.Msg.alert(command, 'Group id: ' + groupId + '<br />Count - ' + records.length);" />
                            </Listeners>
                            <SelectionModel>
                                <ext:CheckboxSelectionModel runat="server" RowSpan="2">
                                    <Listeners>
                                        <%--<AfterCheckAllClick Handler="check" />--%>
                                    </Listeners>
                                </ext:CheckboxSelectionModel>
                            </SelectionModel>
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
</body>
</html>

<script type="text/javascript">
    laydate.skin('danlan');
    var datestart = {
        elem: '#datestart',
        format: 'YYYY-MM-DD',
        //min: laydate.now(), //设定最小日期为当前日期
        max: '2099-06-16 23:59:59', //最大日期
        istime: false,
        istoday: false,
        isclear: false,
        choose: function (datas) {
            //end.min = datas; //开始日选好后，重置结束日的最小日期
            //end.start = datas //将结束日的初始值设定为开始日
            var tt = $("#datestart").html();
            Dispatched.GetDateTime(true, tt);
        }
    };
    laydate(datestart);

</script>
<script type="text/javascript">
    function clearTime(start) {
        document.getElementById("datestart").innerText = start;
    };
    function getTime() {
        Dispatched.GetDateTime(true, document.getElementById("datestart").innerText);
    }
</script>
<script type="text/javascript">
    $(function () {

        $('.x-grid3-scroller').css({ 'background': 'red' });
    })
    $('.x-grid3-scroller').niceScroll({
        cursorcolor: "#7683a4",//#CC0071 光标颜色
        cursoropacitymax: 1, //改变不透明度非常光标处于活动状态（scrollabar“可见”状态），范围从1到0
        touchbehavior: false, //使光标拖动滚动像在台式电脑触摸设备
        cursorwidth: "4px", //像素光标的宽度
        cursorborder: "0", // 	游标边框css定义
        cursorborderradius: "5px",//以像素为光标边界半径
        autohidemode: false //是否隐藏滚动条
    });
    $('.OverCar-data-list').niceScroll({
        cursorcolor: "#7683a4",//#CC0071 光标颜色
        cursoropacitymax: 1, //改变不透明度非常光标处于活动状态（scrollabar“可见”状态），范围从1到0
        touchbehavior: false, //使光标拖动滚动像在台式电脑触摸设备
        cursorwidth: "3px", //像素光标的宽度
        cursorborder: "0", // 	游标边框css定义
        cursorborderradius: "5px",//以像素为光标边界半径
        autohidemode: false //是否隐藏滚动条
    });
</script>