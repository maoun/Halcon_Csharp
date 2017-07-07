<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrequentOverCar.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Map.FrequentOverCar" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>频繁过车</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="../Style/customMap.css" />
    <link rel="stylesheet" type="text/css" href="css/custom.css" />
    <link rel="stylesheet" type="text/css" href="css/Ui-skin.css" />
    <link rel="Stylesheet" href="../Styles/datetime.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../Styles/Clzpp/mapcarPicker.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/Clzpp/base.css" />
    <link rel="Stylesheet" type="text/css" href="../Styles/hphm/autohphm.css" />
    <script type="text/javascript" src="../Scripts/Clzpp/carData.js"></script>
    <script type="text/javascript" src="../Scripts/Clzpp/mapcarPicker.js"></script>
    <script type="text/javascript" src="../Scripts/Clzpp/jquery-2.1.3.min.js"></script>
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

        #Panel1 {
            position: fixed;
            top: 51px;
        }

        #toolbarrquery {
            margin-top: 50px;
        }

        #toggle {
            background: url(../Image/xia.png) no-repeat 0px -14px !important;
            height: 20px;
            margin-left: 335px;
            margin-top: 0px;
            cursor: pointer;
        }

        #cbocsys, #cbocllx {
            border: 1px solid #7eadd9 !important;
            width: 75px !important;
            height: 25px !important;
            margin-top: 1px !important;
        }

        #cbotimes {
            border: 1px solid #7eadd9 !important;
            width: 75px !important;
            height: 25px !important;
            margin-top: 1px !important;
        }

        #cbocsys, #cbocllx, #cbotimes {
            margin: 0;
            padding: 0;
            border: none;
        }
    </style>
    <style type="text/css">
        body .ui-right-wrap .x-grid3-body .x-grid3-td-numberer {
            background-image: none !important;
            background-image: none;
        }

        body .ui-right-wrap .x-grid3-body .x-grid3-td-checker {
            background-image: none !important;
            background-image: none;
        }
    </style>

    <script type="text/javascript">
        var GetScreen = function () {
            W = $(window).width();// screen.width;
            H = $(window).height(); //screen.height;
            wingcjl.x = W - 1110;
            wingcjl.y = H - 258;

            winhphm.x = W - 750;
            winhphm.y = H - 258;
        }
    </script>
    <script type="text/javascript">
        var change = function (obj, value) {
            obj.setValue(value.toUpperCase());
            switch (obj.id) {
                case "labelhm1":
                    document.getElementById('labelhm2').focus();
                    break;
                case "labelhm2":
                    document.getElementById('labelhm3').focus();
                    break;
                case "labelhm3":
                    document.getElementById('labelhm4').focus();
                    break;
                case "labelhm4":
                    document.getElementById('labelhm5').focus();
                    break;
                case "labelhm5":
                    document.getElementById('labelhm6').focus();
                    break;
            }

        };
    </script>
    <script type="text/javascript" src="../Scripts/bmapFile.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/bmap.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/Heatmap_min.js" charset="UTF-8"></script>
    <!--梁引入如下js和css-->
    <script type="text/javascript" src="js/Qquery1.91-min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="js/jquery.nicescroll.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <script type="text/javascript">
        $(function () {
            $("body").delegate("#ext-gen108 .x-combo-list-item", "click", function (event) {
                $("#cboclxh").click();
            })
        })
    </script>
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
        function MenuItemClick(url) {
            //获取内容部分高度
            var h0 = $("#panelQuery").css("height");
            var h = (0 - parseInt($("#panelQuery").css("height"))) + "px";
            $("#panelQuery").animate({ marginTop: h });
            panelMain.autoLoad.url = url;
            panelMain.reload();
            $("#panelMain_IFrame").css("height", h0);
            $("#panelTop").css("display", "block");
        }
    </script>
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
            });
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
    <script type="text/javascript">
        var showTip = function () {
            var rowIndex = GridRoadManager.view.findRowIndex(this.triggerElement),
                cellIndex = GridRoadManager.view.findCellIndex(this.triggerElement),
                record = StoreInfo.getAt(rowIndex),
                fieldName = GridRoadManager.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
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
</head>
<body onload="MapCenter();">
    <form id="form2" runat="server">
        <ext:Hidden ID="curpage" runat="server" />
        <ext:Hidden ID="H" runat="server" />
        <ext:Hidden ID="W" runat="server" />
        <ext:Hidden ID="totalpage" runat="server" />
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
        <ext:Store ID="csys" runat="server">
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

        <ext:Store ID="StoreClpp" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="BRANDID" />
                        <ext:RecordField Name="BRANDNAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>

        <ext:Store ID="Clxh" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="MODELID" />
                        <ext:RecordField Name="MODELNAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="FrequentOverCar" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="border" Cls="new-layout">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Title='<%# GetLangStr("FrequentOverCar1","地图浏览")%>' AutoRender="true" Region="Center"
                    Header="false" Height="800" Width="1000" Cls="map-bg">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar12" runat="server" Cls="top-toolbar">
                            <Items>
                                <ext:Button ID="Linkreload" runat="server" Icon="Reload" Text='<%# GetLangStr("FrequentOverCar2","重载地图")%>'>
                                    <Listeners>
                                        <Click Handler="window.location.reload();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Button ID="TbutMove" runat="server" Icon="Erase" Text='<%# GetLangStr("FrequentOverCar3","清除")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.ClearCircle();BMAP.ClearLabel();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButCenetr" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("FrequentOverCar4","中心点")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.GotoCenter();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButRanging" runat="server" Icon="PencilGo" Text='<%# GetLangStr("FrequentOverCar5","测距")%>'>
                                    <Listeners>
                                        <Click Handler=" BMAP.DistanceTool();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButPlan" runat="server" Icon="Vector" Text='<%# GetLangStr("FrequentOverCar6","测面积")%>'>
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
                <ext:FormPanel ID="FormPanel2" Padding="5" Cls="ui-right-wrap Middle-arrow" Region="east" runat="server" Title=""
                    Split="true" Collapsible="true" RowHeight="1" Width="400" Layout="Accordion">
                    <TopBar>
                        <ext:Toolbar runat="server" Layout="ContainerLayout">
                            <Items>
                                <ext:Toolbar ID="toolbarrquery" runat="server">
                                    <Items>
                                        <ext:Panel ID="Pantrack" runat="server" Padding="4" Title="" Layout="RowLayout"
                                            Icon="Car" Width="400" Height="300">
                                            <Items>
                                                <ext:Panel ID="Panel1" runat="server" Padding="4" Width="400" AutoHeight="true" Cls="row-line sub-form-element">
                                                    <Content>
                                                        <div id="toggle"></div>
                                                    </Content>
                                                    <Items>
                                                        <ext:LinkButton runat="server" X="5" Y="0" IconCls="custom-iconbiao" Text='<%# GetLangStr("FrequentOverCar7","卡口标注(请在左侧地图上标注)")%>'>
                                                            <Listeners>
                                                                <Click Handler=" BMAP.ClearCircle();BMAP.Clear();BMAP.ClearLine();BMAP.SaveAreaMarker({Operate:'Freduent'});;" />
                                                            </Listeners>
                                                        </ext:LinkButton>
                                                    </Items>
                                                </ext:Panel>
                                                <ext:Panel ID="panelkk" runat="server" FormGroup="true" Collapsed="true">
                                                    <Items>
                                                        <ext:GridPanel ID="kkgp" X="5" Y="80" Height="300" Width="380" runat="server">
                                                            <Store>
                                                                <ext:Store ID="StoreKK" runat="server">
                                                                    <Reader>
                                                                        <ext:JsonReader IDProperty="STATION_ID">
                                                                            <Fields>
                                                                                <ext:RecordField Name="STATION_NAME" />
                                                                                <ext:RecordField Name="STATION_ID" />
                                                                                <ext:RecordField Name="xpoint" />
                                                                                <ext:RecordField Name="ypoint" />
                                                                            </Fields>
                                                                        </ext:JsonReader>
                                                                    </Reader>
                                                                </ext:Store>
                                                            </Store>
                                                            <TopBar>
                                                            </TopBar>
                                                            <ColumnModel ID="ColumnModel2" runat="server">
                                                                <Columns>
                                                                    <ext:Column Header='<%# GetLangStr("FrequentOverCar8","周边卡口")%>' AutoDataBind="true" DataIndex="STATION_NAME" Width="300" Align="Left" Editable="false" />
                                                                    <ext:Column Header='<%# GetLangStr("FrequentOverCar9","x坐标")%>' AutoDataBind="true" DataIndex="xpoint" Hidden="true" Width="100" Align="Center" />
                                                                    <ext:Column Header='<%# GetLangStr("FrequentOverCar10","y坐标")%>' AutoDataBind="true" DataIndex="ypoint" Hidden="true" Width="100" Align="Center" />
                                                                </Columns>
                                                            </ColumnModel>
                                                            <SelectionModel>
                                                                <ext:CheckboxSelectionModel runat="server" RowSpan="1">
                                                                    <Listeners>
                                                                        <%--<AfterCheckAllClick Handler="chek" />--%>
                                                                    </Listeners>
                                                                </ext:CheckboxSelectionModel>
                                                            </SelectionModel>
                                                        </ext:GridPanel>
                                                    </Items>
                                                </ext:Panel>
                                                <ext:Panel runat="server" Height="280" Layout="Absolute">
                                                    <Items>
                                                        <ext:Panel runat="server" X="0" Y="0">
                                                            <Content>
                                                                <div id="selectDate">
                                                                    <span class="laydate-span" style="margin-left: 12px; height: 24px;"><%# GetLangStr("FrequentOverCar11","开始时间:")%></span>
                                                                    <li class="laydate-icon" id="start" style="width: 253px; margin-left: 20px; height: 22px;" runat="server"></li>
                                                                </div>
                                                                <div class="clear" style="clear: both"></div>
                                                                <div style="margin-top: 15px">
                                                                    <span class="laydate-span" style="margin-left: 12px; height: 24px;"><%# GetLangStr("FrequentOverCar12","结束时间:")%></span>
                                                                    <li class="laydate-icon" id="end" style="width: 253px; margin-left: 20px; height: 22px;" runat="server"></li>
                                                                </div>
                                                            </Content>
                                                        </ext:Panel>
                                                        <ext:ComboBox ID="cbotimes" X="10" Y="70" FieldLabel='<%# GetLangStr("FrequentOverCar13","次数(>=)") %>' StyleSpec="margin-left: 10px;" runat="server" BlankText='<%# GetLangStr("FrequentOverCar14","请选择...")%>' TypeAhead="true" SelectOnFocus="true"
                                                            EmptyText='<%# GetLangStr("FrequentOverCar15","请选择次数...")%>' LabelWidth="70" Width="180" Text="10" Editable="false" Region="West" Cls="ui-input">
                                                            <Items>
                                                                <ext:ListItem Text="2" />
                                                                <ext:ListItem Text="5" />
                                                                <ext:ListItem Text="10" />
                                                                <ext:ListItem Text="15" />
                                                                <ext:ListItem Text="20" />
                                                                <ext:ListItem Text="25" />
                                                                <ext:ListItem Text="30" />
                                                                <ext:ListItem Text="35" />
                                                                <ext:ListItem Text="40" />
                                                                <ext:ListItem Text="45" />
                                                                <ext:ListItem Text="50" />
                                                                <ext:ListItem Text="55" />
                                                                <ext:ListItem Text="60" />
                                                                <ext:ListItem Text="65" />
                                                                <ext:ListItem Text="70" />
                                                                <ext:ListItem Text="75" />
                                                                <ext:ListItem Text="80" />
                                                                <ext:ListItem Text="85" />
                                                                <ext:ListItem Text="90" />
                                                                <ext:ListItem Text="95" />
                                                                <ext:ListItem Text="100" />
                                                            </Items>
                                                        </ext:ComboBox>
                                                        <ext:ComboBox ID="cbocllx" X="200" Y="70" LabelWidth="63" StoreID="cllx" Editable="false" FieldLabel='<%# GetLangStr("FrequentOverCar16"," 号牌种类") %>' StyleSpec="margin-left: 10px;" runat="server" BlankText='<%# GetLangStr("FrequentOverCar14","请选择...")%>' TypeAhead="true" SelectOnFocus="true"
                                                            EmptyText='<%# GetLangStr("FrequentOverCar14","请选择...")%>' DisplayField="CODEDESC" ValueField="CODE" Width="180" Region="West" Cls="ui-input">
                                                            <Triggers>
                                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("FrequentOverCar17","清除选中")%>' AutoDataBind="true" />
                                                            </Triggers>
                                                            <Listeners>
                                                                <Select Handler="this.triggers[0].show();" />
                                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };this.EmptyText='请选择...';" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                        <ext:Panel ID="Panel8" runat="server" Height="30" Width="360" X="10" Y="105" HideBorders="true" Layout="ColumnLayout">
                                                            <Items>
                                                                <ext:Panel runat="server">
                                                                    <Content>
                                                                        <input type="hidden" runat="server" id="clpp" value="" />
                                                                        <input type="hidden" runat="server" id="clzpp" value="" />
                                                                        <div style="margin-top: 5px;">
                                                                            <span><%# GetLangStr("FrequentOverCar55"," 车辆品牌：") %></span>
                                                                        </div>
                                                                    </Content>
                                                                </ext:Panel>
                                                                <ext:Panel runat="server">
                                                                    <Content>
                                                                        <input type="text" runat="server" id="ClppChoice" style="height: 24px; width: 273px;" />
                                                                    </Content>
                                                                </ext:Panel>
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:ComboBox ID="cbocsys" X="10" Y="185" LabelWidth="70" StoreID="csys" FieldLabel='<%# GetLangStr("FrequentOverCar18"," 车身颜色") %>' StyleSpec="margin-left: 10px;" runat="server" BlankText='<%# GetLangStr("FrequentOverCar14","请选择...")%>' TypeAhead="true" SelectOnFocus="true"
                                                            EmptyText='<%# GetLangStr("FrequentOverCar14","请选择...")%>' Width="180" Text="" DisplayField="CODEDESC" Editable="false" ValueField="CODE" Region="West" Cls="ui-input">
                                                            <Triggers>
                                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("FrequentOverCar17","清除选中")%>' AutoDataBind="true" />
                                                            </Triggers>
                                                            <Listeners>
                                                                <Select Handler="this.triggers[0].show();" />
                                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };this.EmptyText='请选择...';" />
                                                            </Listeners>
                                                        </ext:ComboBox>

                                                        <ext:Label X="0" Y="145" Width="60" Text='<%# GetLangStr("FrequentOverCar20","号牌号码:" )%>' StyleSpec="margin-left: 10px;" runat="server" />
                                                        <ext:Panel ID="Panel4" X="90" Y="145" runat="server" Height="29" Width="310" StyleSpec="margin-left:6px;">
                                                            <Content>
                                                                <veh:VehicleHead ID="cboplate" runat="server" />
                                                            </Content>
                                                        </ext:Panel>
                                                        <ext:Panel runat="server" X="149" Y="145" ID="pnhphm" Width="150" Height="30" Layout="ColumnLayout">
                                                            <Content>
                                                                <input name="num1" class="haopai_name1" value="" id="haopai_name1" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                                <input name="num2" class="haopai_name2" value="" id="haopai_name2" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                                <input name="num3" class="haopai_name3" value="" id="haopai_name3" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                                <input name="num4" class="haopai_name4" value="" id="haopai_name4" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                                <input name="num5" class="haopai_name5" value="" id="haopai_name5" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                                <input name="num6" class="haopai_name6" value="" id="haopai_name6" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                            </Content>
                                                        </ext:Panel>
                                                        <ext:Button ID="ButAddgrid" runat="server" X="80" Y="245" Text='<%# GetLangStr("FrequentOverCar21","查找")%>' Width="80" Region="West" IconCls="ui-input w-80px search-btn border-radius-30">
                                                            <DirectEvents>
                                                                <Click OnEvent="Query_event">
                                                                    <EventMask ShowMask="true" />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Panel>
                                            </Items>
                                        </ext:Panel>
                                    </Items>
                                </ext:Toolbar>

                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:Button ID="ButFirst" runat="server" Text='<%# GetLangStr("FrequentOverCar22","首页")%>'>
                                            <DirectEvents>
                                                <Click OnEvent="TbutFisrt">
                                                    <EventMask ShowMask="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButLast" runat="server" Icon="ResultsetPrevious">
                                            <DirectEvents>
                                                <Click OnEvent="TbutLast">
                                                    <EventMask ShowMask="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButNext" runat="server" Icon="ResultsetNext">
                                            <DirectEvents>
                                                <Click OnEvent="TbutNext">
                                                    <EventMask ShowMask="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButEnd" runat="server" Text='<%# GetLangStr("FrequentOverCar23","尾页")%>'>
                                            <DirectEvents>
                                                <Click OnEvent="TbutEnd">
                                                    <EventMask ShowMask="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                                        <ext:Label ID="labpage" runat="server" Text='<%# GetLangStr("FrequentOverCar24","当前0页,共0页")%>' StyleSpec="margin-left: 10px;" />
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridRoadManager" runat="server" StripeRows="true" Title=""
                            Collapsible="true" AutoHeight="false" AutoScroll="true" Cls="data-list-container table-ui display-table w-100 Hide-panel-header">
                            <Store>
                                <ext:Store ID="StoreInfo" runat="server">
                                    <Reader>
                                        <ext:JsonReader IDProperty="col1">
                                            <Fields>
                                                <ext:RecordField Name="hphm" />
                                                <ext:RecordField Name="lxmc" />
                                                <ext:RecordField Name="xypoint" />
                                                <ext:RecordField Name="hpzl" />
                                                <ext:RecordField Name="kkid" />
                                                <ext:RecordField Name="clpp" />
                                                <ext:RecordField Name="csys" />
                                                <ext:RecordField Name="cs" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Header='<%# GetLangStr("FrequentOverCar25","序号")%>' AutoDataBind="true" Align="Center" Editable="false" Width="40" />
                                    <ext:Column Header='<%# GetLangStr("FrequentOverCar26","车牌")%>' AutoDataBind="true" DataIndex="hphm" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("FrequentOverCar27","卡口")%>' AutoDataBind="true" DataIndex="lxmc" Width="180" Align="left" />
                                    <ext:Column Header='<%# GetLangStr("FrequentOverCar28","次数")%>' AutoDataBind="true" DataIndex="cs" Width="50" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("FrequentOverCar18","车身颜色")%>' AutoDataBind="true" DataIndex="csys" Width="50" Align="Center" Hidden="true" />
                                    <ext:Column Header='<%# GetLangStr("FrequentOverCar30","xypoint")%>' AutoDataBind="true" Hidden="true" DataIndex="xypoint" Width="50" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel runat="server">
                                    <Listeners>
                                        <RowSelect Handler="FrequentOverCar.SelectRow(record.data.xypoint,record.data.hphm,record.data.clpp,record.data.csys,record.data.hpzl,'')" />
                                    </Listeners>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <ToolTips>
                                <ext:ToolTip ID="RowTip" runat="server" Target="={GridRoadManager.getView().mainBody}"
                                    Delegate=".x-grid3-cell" TrackMouse="true">
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
        <ext:Window ID="winPassWeb" Modal="true" runat="server" Hidden="true" Height="600px"
            Width="800px" Title='<%# GetLangStr("FrequentOverCar31","过车信息")%>' Resizable="false" Layout="FitLayout">
            <Items>
                <ext:FormPanel runat="server" ID="extForm" Layout="RowLayout">
                    <Items>
                        <ext:Label runat="server" ID="lblPassInfo" RowHeight=".1" StyleSpec="font-size: 16px; font-weight:bold"></ext:Label>
                        <ext:Image runat="server" ID="imgPassInfo" RowHeight=".9"></ext:Image>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <ext:Window ID="winhphm" runat="server" Title='<%# GetLangStr("FrequentOverCar32","车辆信息")%>' Hidden="true"
            X="700"
            Y="400"
            Width="350" Height="260">
            <Items>
                <ext:Panel runat="server" Height="260" Layout="RowLayout" ID="panelhphm">
                    <Items>
                        <ext:Panel runat="server" RowHeight=".5" Layout="Absolute">
                            <Items>
                                <ext:TextField X="0" Y="0" ID="clppwin" runat="server" FieldLabel='<%# GetLangStr("FrequentOverCar33","车辆品牌")%>' AutoDataBind="true" ReadOnly="true" LabelWidth="60" Width="350" />
                                <ext:TextField X="0" Y="40" ID="hpzlwin" runat="server" FieldLabel='<%# GetLangStr("FrequentOverCar34","号牌种类")%>' AutoDataBind="true"  ReadOnly="true" LabelWidth="60" Width="350" />
                                <ext:TextField X="0" Y="80" ID="csyswin" runat="server" FieldLabel='<%# GetLangStr("FrequentOverCar18","车身颜色")%>' AutoDataBind="true"  ReadOnly="true" LabelWidth="60" Width="350" />
                                <%--<ext:TextField X="175" Y="40" ID="clztwin" runat="server" FieldLabel="车辆状态" ReadOnly="true" LabelWidth="60" Width="175" />--%>
                                <%--<ext:TextField x="0" Y="80" runat="server" FieldLabel="发证机构"  ReadOnly="true" LabelWidth="60" Width="175" />--%>
                                <%-- <ext:TextField runat="server" FieldLabel="车主姓名" />
                                <ext:TextField runat="server" FieldLabel="联系电话" />--%>
                                <%--<ext:TextField x="175" Y="80" runat="server" FieldLabel="使用性质" ReadOnly="true"  LabelWidth="60" Width="175" />--%>
                            </Items>
                        </ext:Panel>
                        <ext:Panel runat="server" RowHeight=".5" Layout="Absolute">
                            <Items>
                                <ext:Button X="0" Y="0" runat="server" Text='<%# GetLangStr("FrequentOverCar36","车辆轨迹")%>' Width="120">
                                    <Listeners>
                                        <Click Handler="FrequentOverCar.showwin('PathCarQuery.aspx','车辆轨迹');" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button X="175" Y="0" runat="server" Text='<%# GetLangStr("FrequentOverCar37","落脚点")%>' Width="120">
                                    <Listeners>
                                        <Click Handler="FrequentOverCar.showwin('FootHold.aspx','落脚点');" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button X="0" Y="40" runat="server" Text='<%# GetLangStr("FrequentOverCar38","违法信息查询")%>' Width="120">
                                    <Listeners>
                                        <Click Handler="FrequentOverCar.showwin('../Passcar/PeccancyInfoQuery.aspx','违法信息查询');" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button X="175" Y="40" runat="server" Text='<%# GetLangStr("FrequentOverCar39","过车记录查询")%>' Width="120">
                                    <Listeners>
                                        <Click Handler="FrequentOverCar.showwin('../Passcar/PassCarInfoQuery.aspx','过车记录查询');" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Panel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Window>
        <ext:Window
            ID="wingcjl"
            runat="server"
            Title="过车记录"
            Hidden="true"
            X="340"
            Y="400"
            Width="350"
            Height="260">
            <Items>
                <ext:Panel runat="server">
                    <Items>
                        <ext:GridPanel runat="server" ID="gridgcjl" AutoScroll="true" Height="350">
                            <Store>
                                <ext:Store ID="Storegcjl" runat="server">
                                    <Reader>
                                        <ext:JsonReader IDProperty="col1">
                                            <Fields>
                                                <ext:RecordField Name="gcsj" />
                                                <ext:RecordField Name="lkmc" />
                                                <ext:RecordField Name="zjwj" />
                                                <ext:RecordField Name="hphm" />
                                                <ext:RecordField Name="xpoint" />
                                                <ext:RecordField Name="ypoint" />
                                                <ext:RecordField Name="clwz" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel3" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Header='<%# GetLangStr("FrequentOverCar25","序号")%>' AutoDataBind="true" Align="Center" Editable="false" Hidden="true" />
                                    <ext:Column Header='<%# GetLangStr("FrequentOverCar41","过车时间")%>' AutoDataBind="true" DataIndex="gcsj" Width="150" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("FrequentOverCar27","卡口")%>' AutoDataBind="true" DataIndex="lkmc" Width="180" Align="left" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel runat="server">
                                    <Listeners>
                                        <RowSelect Handler="FrequentOverCar.ShowMoreInfo(record.data.xpoint,record.data.ypoint,record.data.hphm,record.data.gcsj,record.data.lkmc,record.data.zjwj,record.data.clwz)" />
                                    </Listeners>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <ToolTips>
                                <ext:ToolTip ID="ToolTip1" runat="server" Target="={GridRoadManager.getView().mainBody}"
                                    Delegate=".x-grid3-cell" TrackMouse="true">
                                    <Listeners>
                                        <Show Fn="showTip" />
                                    </Listeners>
                                </ext:ToolTip>
                            </ToolTips>
                        </ext:GridPanel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Window>
    </form>
</body>
<script type="text/jscript">
    var cityPicker = new IIInsomniaCityPicker({
        data: carData,
        target: '#ClppChoice',
        valType: 'k-v',
        hideCityInput: '#clzpp',
        hideProvinceInput: '#clpp',
        callback: function (city_id) {

        }
    });

    cityPicker.init();
</script>
</html>
<script type="text/javascript">
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
<%--<script type="text/javascript">
    laydate({
        elem: '#sta_time', //目标元素。由于laydate.js封装了一个轻量级的选择器引擎，因此elem还允许你传入class、tag但必须按照这种方式 '#id .class'

        event: 'focus' //响应事件。如果没有传入event，则按照默认的click
    });
</script>--%>

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
            FrequentOverCar.GetDateTime(true, tt);
            //alert(tt);
        }
    };
    var end = {
        elem: '#end',
        format: 'YYYY-MM-DD hh:mm:ss',
        min: laydate.now(),
        max: laydate.now(),// '2099-06-16 23:59:59',
        istime: true,
        istoday: false,
        isclear: false,
        choose: function (datas) {
            start.max = datas; //结束日选好后，重置开始日的最大日期
            var tt = $("#end").html();
            FrequentOverCar.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>
<script type="text/javascript">

    $("body").delegate("#toggle", "click", function () {

        if ($("#panelkk").children().css("display") == "block") {

            if (!$(this).hasClass("active")) {

                $("#toggle").addClass("active");
                $("#Pantrack").css("height", "300px");
                $("#kkgp").height(300).slideUp(500);
                $("#ext-gen12").height(500);
                $("#GridStation").css("height", "85%");

            }

            else {
                $("#toggle").removeClass("active");
                $("#Pantrack").css("height", "450px");
                $("#kkgp").height(300).slideDown(500);

                $("#GridStation").css("height", "1%");
            }

        }

    })
</script>