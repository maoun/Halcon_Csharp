<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarStrandQuery.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Map.CarStrandQuery" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>相似车辆串并查询</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="css/custom.css" />
    <link rel="stylesheet" type="text/css" href="../Style/customMap.css" />
    <link rel="stylesheet" href="../Css/showphotostyle.css" type="text/css" />
    <link rel="Stylesheet" href="../Styles/datetime.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../Styles/Clzpp/mapcarPicker.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/Clzpp/base.css" />
    <link rel="Stylesheet" type="text/css" href="../Styles/hphm/autohphm.css" />
    <script type="text/javascript" src="../Scripts/Clzpp/carData.js"></script>
    <script type="text/javascript" src="../Scripts/Clzpp/mapcarPicker.js"></script>
    <script type="text/javascript" src="../Scripts/Clzpp/jquery-2.1.3.min.js"></script>
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
    </script>
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

        #cboplate_Field2 {
            width: 25px !important;
            height: 25px !important;
            border: 1px solid #7eadd9 !important;
            margin-top: 1px !important;
        }

        #cboplate_Field2, #labelhm1, #labelhm2, #labelhm3, #labelhm4, #labelhm5, #labelhm6, #cboclpp, #cboclxh, #cbocsys, #cbocllx {
            margin: 0;
            padding: 0;
            border: none;
        }

        #cboclpp, #cboclxh, #cbocsys, #cbocllx {
            width: 85px !important;
            height: 25px !important;
            border: 1px solid #7eadd9;
            margin-top: 1px !important;
        }

        #labelhm1, #labelhm2, #labelhm3, #labelhm4, #labelhm5, #labelhm6 {
            width: 18px !important;
            height: 27px !important;
            border: 1px solid #7eadd9;
        }

        #cboplate_Panel1 {
            /*background: white !important;*/
            width: 208px\9 !important;
        }
    </style>
    <style type="text/css">
        body .ui-right-wrap .x-grid3-body .x-grid3-td-checker {
            background-image: none !important;
            background-image: none;
        }

        body .ui-right-wrap .x-grid3-body .x-grid3-td-numberer {
            background-image: none !important;
            background-image: none;
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
            margin-left: 355px;
            margin-top: -8px;
            cursor: pointer;
        }

        #Panel1_Content .active {
            cursor: pointer;
            background: url(../Image/xia.png) no-repeat 0px 5px !important;
        }

        #FormPanel2 {
            height: 100%;
        }

        #ext-gen80 {
            height: auto;
        }

        #Panel4 {
            height: 29px;
            overflow: hidden;
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
        $(function () {
            $("#ext-gen113").css("display", "none")

            $("body").delegate("#ext-gen108 .x-combo-list-item", "click", function (event) {

                $("#cboclxh").click();

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
    <script type="text/jscript">
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
</head>
<body onload="MapCenter();">
    <form id="form1" runat="server">
        <div id="append_parent">
        </div>
        <ext:Hidden ID="curpage" runat="server" />
        <ext:Hidden ID="totalpage" runat="server" />
        <ext:Store ID="stationstroe" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="STATION_ID" />
                        <ext:RecordField Name="STATION_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="csysstore" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="cllxstore" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Clpp" runat="server">
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

        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="CarStrandQuery" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="border" Cls="new-layout">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Title='<%# GetLangStr("CarStrandQuery1","地图浏览")%>' AutoRender="true" Region="Center"
                    Header="false" Height="800" Width="1000" Cls="map-bg">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar12" runat="server" Cls="top-toolbar">
                            <Items>

                                <ext:Button ID="Linkreload" runat="server" Icon="Reload" Text='<%# GetLangStr("CarStrandQuery2","重载地图")%>'>
                                    <Listeners>
                                        <Click Handler="window.location.reload();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Button ID="TbutMove" runat="server" Icon="Erase" Text='<%# GetLangStr("CarStrandQuery3","清除")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.ClearCircle();BMAP.ClearLabel();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButCenetr" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("CarStrandQuery4","中心点")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.GotoCenter();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButRanging" runat="server" Icon="PencilGo" Text='<%# GetLangStr("CarStrandQuery5","测距")%>'>
                                    <Listeners>
                                        <Click Handler=" BMAP.DistanceTool();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButPlan" runat="server" Icon="Vector" Text='<%# GetLangStr("CarStrandQuery6","测面积")%>'>
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
                <ext:FormPanel ID="FormPanel2" Cls="ui-right-wrap Middle-arrow" Padding="5" Region="east" runat="server" Title=""
                    Split="true" Collapsible="true" Width="400" Layout="Accordion">
                    <TopBar>
                        <ext:Toolbar runat="server" Layout="ContainerLayout">
                            <Items>
                                <ext:Toolbar ID="toolbarrquery" runat="server" Width="450">
                                    <Items>
                                        <ext:Panel ID="Pantrack" runat="server" Padding="4" Title="" Layout="RowLayout"
                                            Icon="Car" Width="400" Height="260">
                                            <Items>
                                                <ext:Panel ID="Panel1" runat="server" Cls="row-line sub-form-element">
                                                    <Content>
                                                        <div id="toggle"></div>
                                                    </Content>
                                                    <Items>
                                                        <ext:LinkButton runat="server" X="0" Y="0" ID="click" IconCls="custom-iconbiao" Text='<%# GetLangStr("CarStrandQuery7","根据卡点标注案发地点（请在左侧地图标注）")%>'>
                                                            <Listeners>
                                                                <Click Handler=" BMAP.ClearCircle();BMAP.Clear();BMAP.SaveMarker({type:'AddEventPoint'});;" />
                                                            </Listeners>
                                                        </ext:LinkButton>
                                                        <ext:TextField runat="server" ID="arear" LabelWidth="110" MaxHeight="20" Width="150" FieldLabel="&nbsp&nbsp查找半径(米)<=" Text="5000"></ext:TextField>
                                                    </Items>
                                                </ext:Panel>

                                                <ext:Panel ID="panelkk" runat="server" FormGroup="true" Collapsed="true">

                                                    <Items>

                                                        <ext:GridPanel ID="kkgp" X="10" Y="80" Height="300" Width="400" runat="server">
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
                                                                    <ext:Column Header='<%# GetLangStr("CarStrandQuery8","周边卡口")%>' AutoDataBind="true" DataIndex="STATION_NAME" Width="300" Align="Center" Editable="false" />
                                                                    <ext:Column Header='<%# GetLangStr("CarStrandQuery9","x坐标")%>' AutoDataBind="true" DataIndex="xpoint" Hidden="true" Width="100" Align="Center" />
                                                                    <ext:Column Header='<%# GetLangStr("CarStrandQuery10","y坐标")%>' AutoDataBind="true" DataIndex="ypoint" Hidden="true" Width="100" Align="Center" />
                                                                </Columns>
                                                            </ColumnModel>
                                                            <SelectionModel>
                                                                <ext:CheckboxSelectionModel />
                                                            </SelectionModel>
                                                        </ext:GridPanel>
                                                    </Items>
                                                </ext:Panel>
                                                <ext:Panel ID="Panel2" runat="server" Padding="4"
                                                    Width="400" AutoHeight="true" Cls="row-line sub-form-element">
                                                    <Items>
                                                        <ext:Panel runat="server" X="0" Y="10" Height="80">
                                                            <Content>
                                                                <div id="selectDate">
                                                                    <span style="float: left; margin-left: 10px; height: 24px; line-height: 24px!important; text-align: center"><%# GetLangStr("CarStrandQuery12","开始时间:")%></span>
                                                                    <li class="laydate-icon" id="start" style="width: 265px; margin-left: 16px; float: left; list-style: none; cursor: pointer; height: 22px; line-height: 22px!important" runat="server"></li>
                                                                </div>
                                                                <div class="clear" style="clear: both"></div>
                                                                <div style="margin-top: 15px">
                                                                    <span style="float: left; margin-left: 10px; height: 24px; line-height: 24px!important; text-align: center"><%# GetLangStr("CarStrandQuery13","结束时间:")%></span>
                                                                    <li class="laydate-icon" id="end" style="width: 265px; margin-left: 16px; float: left; list-style: none; cursor: pointer; height: 22px; line-height: 22px!important" runat="server"></li>
                                                                </div>
                                                            </Content>
                                                        </ext:Panel>
                                                    </Items>
                                                </ext:Panel>
                                                <ext:Panel ID="PanelSecond" runat="server" Layout="Absolute" Width="400" Height="180" ButtonAlign="Center">
                                                    <Items>
                                                        <ext:Label X="10" Y="10" Width="70" Text='<%# GetLangStr("CarStrandQuery14","号牌号码:") %>' runat="server" />
                                                        <ext:Panel ID="Panel4" X="90" Y="10" runat="server" Height="29" Width="310">
                                                            <Content>
                                                                <veh:VehicleHead ID="cboplate" runat="server" />
                                                            </Content>
                                                        </ext:Panel>

                                                        <ext:Panel runat="server" X="139" Y="10" ID="pnhphm" Width="150" Height="30" Layout="ColumnLayout">
                                                            <Content>
                                                                <input name="num1" class="haopai_name1" value="" id="haopai_name1" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                                <input name="num2" class="haopai_name2" value="" id="haopai_name2" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                                <input name="num3" class="haopai_name3" value="" id="haopai_name3" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                                <input name="num4" class="haopai_name4" value="" id="haopai_name4" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                                <input name="num5" class="haopai_name5" value="" id="haopai_name5" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                                <input name="num6" class="haopai_name6" value="" id="haopai_name6" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                            </Content>
                                                        </ext:Panel>
                                                        <%-- <ext:TextField ID="labelhm1" runat="server"  MaxLength="1" X="139" Y="10" Height="24" Width="20"  Cls="ui-input">
                                                            <Listeners>
                                                                <KeyUp Handler="cgtxt" />
                                                                 <Change Fn="change" />
                                                            </Listeners>
                                                        </ext:TextField>
                                                        <ext:TextField ID="labelhm2" runat="server" MaxLength="1" X="159" Y="10" Height="24" Width="30" Cls="ui-input">
                                                            <Listeners>
                                                                 <Change Fn="change" />
                                                            </Listeners>
                                                        </ext:TextField>
                                                        <ext:TextField ID="labelhm3" onkeyup="cgtxt(this.name,this.value)" runat="server" MaxLength="1" X="179" Y="10" Height="24" Width="30" Cls="ui-input">
                                                            <Listeners>
                                                                 <Change Fn="change" />
                                                            </Listeners>
                                                        </ext:TextField>
                                                        <ext:TextField ID="labelhm4" runat="server" MaxLength="1" X="199" Y="10" Height="24" Width="30" Cls="ui-input">
                                                            <Listeners>
                                                                 <Change Fn="change" />
                                                            </Listeners>
                                                        </ext:TextField>
                                                        <ext:TextField ID="labelhm5" runat="server" MaxLength="1" X="219" Y="10" Height="24" Width="30" Cls="ui-input">
                                                            <Listeners>
                                                                 <Change Fn="change" />
                                                            </Listeners>
                                                        </ext:TextField>
                                                        <ext:TextField ID="labelhm6" runat="server" MaxLength="1" X="239" Y="10" Height="24" Width="30" Cls="ui-input">
                                                            <Listeners>
                                                                <KeyUp Fn="change" />
                                                            </Listeners>
                                                        </ext:TextField>--%>
                                                         <ext:Panel ID="Panel8" runat="server" Height="30" Width="500" X="10" Y="50" HideBorders="true" Layout="ColumnLayout">
                                            <Items>
                                                <ext:Panel runat="server">
                                                    <Content>
                                                        <input type="hidden" runat="server" id="Hidden1" value="" />
                                                        <input type="hidden" runat="server" id="clzpp" value="" />
                                                        <div style="margin-top: 5px; width:70px;">
                                                            <span>车辆品牌:</span>
                                                        </div>
                                                    </Content>
                                                </ext:Panel>
                                                <ext:Panel runat="server">
                                                    <Content>
                                                        <input type="text" runat="server" id="ClppChoice" style=" height: 20px; width: 295px;" />
                                                    </Content>
                                                </ext:Panel>
                                            </Items>
                                        </ext:Panel>
                                                        <ext:ComboBox ID="cbocsys" X="10" Y="95" LabelWidth="65" FieldLabel='<%# GetLangStr("CarStrandQuery19","车身颜色") %>' StyleSpec="margin-left: 10px;" EmptyText='<%# GetLangStr("CarStrandQuery20","选择车身颜色...")%>' DisplayField="CODEDESC"
                                                            ValueField="CODE" Editable="false" runat="server" StoreID="csysstore" Width="185" Cls="ui-input">
                                                            <Triggers>
                                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                                            </Triggers>
                                                            <Listeners>
                                                                <Select Handler="this.triggers[0].show();" />
                                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                        <ext:ComboBox ID="cbocllx" X="200" Y="95" LabelWidth="65" Editable="false" FieldLabel='<%# GetLangStr("CarStrandQuery21","号牌种类") %>' StyleSpec="margin-left: 10px;" EmptyText='<%# GetLangStr("CarStrandQuery22","请选择...")%>' DisplayField="CODEDESC"
                                                            ValueField="CODE" runat="server" StoreID="cllxstore" Width="220" Cls="ui-input">
                                                            <Triggers>
                                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                                            </Triggers>
                                                            <Listeners>
                                                                <Select Handler="this.triggers[0].show();" />
                                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                            </Listeners>
                                                        </ext:ComboBox>
                                                        <ext:Button ID="ButAddgrid" X="170" Y="130" runat="server" Text='<%# GetLangStr("CarStrandQuery23","查找")%>'
                                                            Region="West" IconCls="ui-input w-80px search-btn border-radius-30">
                                                            <DirectEvents>
                                                                <Click OnEvent="QueryClick_event">
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
                                        <ext:Button ID="ButFirst" runat="server" Text='<%# GetLangStr("PathCarQuery21","首页")%>'>
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
                                        <ext:Button ID="ButEnd" runat="server" Text='<%# GetLangStr("PathCarQuery24","尾页")%>'>
                                            <DirectEvents>
                                                <Click OnEvent="TbutEnd" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                                        <ext:Label ID="labpage" runat="server" Text='<%# GetLangStr("PathCarQuery25","当前0页,共0页")%>' StyleSpec="margin-left: 10px;" />
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridStation" runat="server" StripeRows="true" Collapsible="true" Title=""
                            AutoScroll="true" RowHeight="1" Cls="data-list-container table-ui display-table w-100 Hide-panel-header">
                            <Store>
                                <ext:Store ID="StoreInfo" runat="server">
                                    <Reader>
                                        <ext:JsonReader IDProperty="col1">
                                            <Fields>
                                                <ext:RecordField Name="lkmc" />
                                                <ext:RecordField Name="gwsj" />
                                                <ext:RecordField Name="fxmc" />
                                                <ext:RecordField Name="hphm" />
                                                <ext:RecordField Name="xpoint" />
                                                <ext:RecordField Name="ypoint" />
                                                <ext:RecordField Name="zjwj" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <TopBar>
                                <ext:Toolbar runat="server" Layout="ContainerLayout">
                                    <Items>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Header='<%# GetLangStr("CarStrandQuery24","序号")%>' Hidden="true" AutoDataBind="true" Width="45" />
                                    <ext:Column Header='<%# GetLangStr("CarStrandQuery25","号牌号码")%>' AutoDataBind="true" DataIndex="hphm" Width="90" Align="Center" Editable="false" />
                                    <ext:Column Header='<%# GetLangStr("CarStrandQuery26","卡口名称")%>' AutoDataBind="true" DataIndex="lkmc" Width="90" Align="Center" Editable="false">
                                    </ext:Column>

                                    <ext:Column Header='<%# GetLangStr("CarStrandQuery27","通过时间")%>' AutoDataBind="true" DataIndex="gwsj" Width="80" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CarStrandQuery28","行驶方向")%>' AutoDataBind="true" DataIndex="fxmc" Width="80" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CarStrandQuery29","x坐标")%>' AutoDataBind="true" DataIndex="xpoint" Hidden="true" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CarStrandQuery30","y坐标")%>' AutoDataBind="true" DataIndex="ypoint" Hidden="true" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CarStrandQuery31","文件")%>' AutoDataBind="true" Width="60" Hidden="true" DataIndex="zjwj" Align="Center" />
                                </Columns>
                            </ColumnModel>

                            <SelectionModel>
                                <ext:RowSelectionModel runat="server" Mode="Single">
                                    <Listeners>
                                        <RowSelect Handler=" CarStrandQuery.SelectRow(record.data.xpoint,record.data.ypoint,record.data.hphm,record.data.zjwj,record.data.gwsj,record.data.fxmc,record.data.lkmc)" />
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
    $(function () {
        $('.ScroolBar').niceScroll({
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
            CarStrandQuery.GetDateTime(true, tt);
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
            CarStrandQuery.GetDateTime(false, tt);
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
                $("#toolbarrquery").css("height", "300px");
                $("#panelkk").height(300).slideUp(300);
                $("#ext-gen12").height(500);
                $("#GridStation").css("height", "85%");

            }

            else {
                $("#toggle").removeClass("active");
                $("#toolbarrquery").css("height", "500px");
                $("#Pantrack").css("height", "200px");
                $("#panelkk").height(300).slideDown(300);
                $("#GridStation").css("height", "1%");
            }
        }
    })
</script>