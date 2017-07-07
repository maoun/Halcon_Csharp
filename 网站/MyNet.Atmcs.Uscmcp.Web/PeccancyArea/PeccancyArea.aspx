<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeccancyArea.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.PeccancyArea" %>

<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>区间车辆查询</title>
    <meta name="GENERATOR" content="MSHTML 8.00.7600.16853" />
    <link href="../Styles/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <link rel="stylesheet" href="../Styles/showphotostyle.css" type="text/css" />
    <link rel="Stylesheet" href="../Styles/datetime.css" type="text/css" />
    <link rel="Stylesheet" href="../Styles/hphm/autohphm.css" type="text/css" />
    <script src="../Scripts/showphoto.js" language="JavaScript" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8">></script>
    <script type="text/javascript" src="../Scripts/common.js" charset="UTF-8"></script>
    <script src="../Scripts/showphoto.js" type="text/javascript" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <%--<script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>--%>
    <!--图片放大开始-->

    <script type="text/javascript" src="../Scripts/Zoom/jquery.photo.gallery.js"></script>
    <!--图片放大结束-->
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

        .fis {
            display: inline-block;
            float: left;
            width: 33.333%;
            height: 220px;
        }

        #FormPanel1-xcollapsed {
            display: none !important;
        }
    </style>
    <script type="text/javascript">
        function ShowImage(image1, image2, image3, palteid, platetype) {
            document.getElementById("zjwj1").src = image1;
            document.getElementById("zjwj2").src = image2;
            document.getElementById("zjwj3").src = image3;
            ChangeBackColor("divplateId", platetype, palteid);
        }
        var DataAmply = function () {
            return '<img class="imgEdit" ext:qtip="查看详细信息" style="cursor:pointer;" src="../images/button/vcard_edit.png" />';
        };
        function ChangeBackColor(id, hpzl, hphm) {

            var obj = document.getElementById(id);
            obj.innerText = hphm;
            switch (hpzl) {

                case "01":
                    obj.style.color = "#000000";
                    obj.style.background = "FFFF00";
                    break;
                case "02":
                    obj.style.color = "#FFFFFF";
                    obj.style.background = "000080";
                    break;
                case "06":
                    obj.style.color = "#FFFFFF";
                    obj.style.background = "000000";
                    break;
                case "23":
                    obj.style.color = "FF0000";
                    obj.style.background = "FFFFFF";
                    break;
                default:
                    obj.style.color = "#FFFFFF";
                    obj.style.background = "000080";
                    break;

            }
        }
        var cellClick = function (grid, rowIndex, columnIndex, e) {
            var t = e.getTarget(),
            //record = grid.getStore().getAt(rowIndex),  // Get the Record
                columnId = grid.getColumnModel().getColumnId(columnIndex); // Get column id

            if (columnId == "Details") {
                return true;
            }
            return false;
        };
        $(function () {
            //报警信息查询，点击单行数据，下面显示图片介绍
            $("body").delegate(".x-grid3-row", "click", function () {
                var aDiv = $("#FormPanel1 .photoblock-many").html();
                //如果当前元素有class,导入div
                if (!$(this).hasClass("import")) {
                    //每次点击，删除之前已经存在的div;
                    $(".import").removeClass("import").next().remove();
                    $(this).addClass("import");
                    $(aDiv).insertAfter($(this));
                }
                else {
                    //   $(this).removeClass("import").next().remove();
                }

            })
        })

        var change = function (obj, value) {
            obj.setValue(value.toUpperCase());
        };
        function clearTime(start, end) {
            document.getElementById("start").innerText = start;
            document.getElementById("end").innerText = end;
            CmbPlateType.triggers[0].hide();
            CmbDealType.triggers[0].hide();
            CmbPecType.triggers[0].hide();
            CmbEndStation.triggers[0].hide();
            CmbStartStation.triggers[0].hide();
        }
        var template = '<span style="color:{0};">{1}</span>';
        var pctChange = function (value) {
            return String.format(template, (value > 0) ? "red" : "green", value + "%");
        };
        function ShowImg(col29, col30) {
            var src1 = col29;
            document.getElementById("img1").src = src1;
            var src2 = col30;
            document.getElementById("img2").src = src2;

        }
    </script>
    <script type="text/javascript">
        var IMGDIR = '../images/sets';
        var attackevasive = '0';
        var gid = 0;
        var fid = parseInt('0');
        var tid = parseInt('0');
    </script>
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
                width: 480px;
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

        .images-view .x-panel-body {
            background: transparent;
        }

        #Label2 {
            font-weight: bolder;
        }
    </style>

    <script type="text/javascript">
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
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridPecArea.view.findRowIndex(this.triggerElement),
                cellIndex = GridPecArea.view.findCellIndex(this.triggerElement),
                record = StorePecArea.getAt(rowIndex),
                fieldName = GridPecArea.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="append_parent" />
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PeccancyArea" />

        <ext:Hidden ID="GridData" runat="server" />
        <ext:Hidden ID="realCount" runat="server" />
        <ext:Hidden ID="curpage" runat="server" />
        <ext:Hidden ID="allPage" runat="server" />
        <ext:Viewport ID="Viewport2" runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="Panel1" Region="North" runat="server"
                    Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server" Layout="Container">
                            <Items>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:Panel runat="server">
                                            <Content>
                                                <table style="width: 550px">
                                                    <tr>
                                                        <td style="width: 50px">
                                                            <span class="laydate-span" style="height: 24px; font-size: 15px; margin-left: 12px;">查询时间：</span></td>
                                                        <td style="width: 200px">
                                                            <li class="laydate-icon" id="start" runat="server" style="margin-left: 1px; width: 151px; height: 24px;"></li>
                                                        </td>
                                                        <td>
                                                            <span class="laydate-span" style="margin-right: 300px">--</span>
                                                        </td>
                                                        <td style="width: 200px">
                                                            <li class="laydate-icon" id="end" runat="server" style="width: 151px; height: 24px; margin-left: -300px;"></li>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Content>
                                        </ext:Panel>

                                        <ext:Label ID="Label3" runat="server" Html="<font >&nbsp;&nbsp;车牌号牌：</font>" StyleSpec="margin-left:-130px;">
                                        </ext:Label>
                                        <ext:Panel ID="Panel3" runat="server" Height="30" StyleSpec="margin-left:-45px;">
                                            <Content>
                                                <veh:VehicleHead ID="WindowEditor1" runat="server" />
                                            </Content>
                                        </ext:Panel>

                                        <ext:Panel runat="server" Layout="ColumnLayout" Height="30">
                                            <Items>
                                                <ext:TextField runat="server" Width="180" ID="TxtplateId" EmptyText="六位号牌号码" MaxLength="6">
                                                    <Listeners>
                                                        <Change Fn="change" />
                                                    </Listeners>
                                                </ext:TextField>
                                                <ext:Panel runat="server" ID="pnhphm" Hidden="true" Width="150" Layout="ColumnLayout">
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

                                        <ext:Panel runat="server">
                                            <Items>
                                                <ext:Checkbox ID="ChkLike" Checked="false" runat="server" BoxLabel="模糊查询" StyleSpec="margin-left:10px;">
                                                    <DirectEvents>
                                                        <Check OnEvent="changtype"></Check>
                                                    </DirectEvents>
                                                </ext:Checkbox>
                                            </Items>
                                        </ext:Panel>

                                        <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
                                        <ext:Label ID="Label6" runat="server" Style="margin-left: 50px" Html="<font >&nbsp;&nbsp;处理状态：</font>" StyleSpec="margin-right:5px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbDealType" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText="选择处理状态..."
                                            SelectOnFocus="true" Width="130">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                            <Store>
                                                <ext:Store ID="StoreDealType" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader>
                                                            <Fields>
                                                                <ext:RecordField Name="col0" Type="String" />
                                                                <ext:RecordField Name="col1" Type="String" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar8" runat="server">
                                    <Items>
                                        <ext:Label ID="Label4" runat="server" StyleSpec="margin-right:5px;margin-left:5px;" Html="<font >&nbsp;&nbsp;起点卡口：</font>">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbStartStation" runat="server" Editable="false" DisplayField="col1"
                                            Width="360" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                            ListWidth="360" EmptyText="选择起点卡口..." SelectOnFocus="true">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();#{CmbEndStation}.clearValue(); #{StoreEndStation}.reload();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                            <Store>
                                                <ext:Store ID="StoreStartStation" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader IDProperty="col1">
                                                            <Fields>
                                                                <ext:RecordField Name="col0" Type="String" />
                                                                <ext:RecordField Name="col1" Type="String" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                            <%--  <Listeners>
                                                <Select Handler="#{CmbEndStation}.clearValue(); #{StoreEndStation}.reload();" />
                                            </Listeners>--%>
                                        </ext:ComboBox>
                                        <ext:Label ID="Label5" runat="server" Html="<font >&nbsp;&nbsp;终点卡口：</font>" StyleSpec="margin-right:3px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbEndStation" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText="选择终点卡口..."
                                            ListWidth="360" SelectOnFocus="true" Width="360">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                            <Store>
                                                <ext:Store ID="StoreEndStation" runat="server" AutoLoad="false" OnRefreshData="TgsRefresh">
                                                    <Reader>
                                                        <ext:JsonReader>
                                                            <Fields>
                                                                <ext:RecordField Name="col0" Type="String" />
                                                                <ext:RecordField Name="col1" Type="String" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                    <Listeners>
                                                        <Load Handler="#{CmbEndStation}.setValue(#{CmbEndStation}.store.getAt(0).get('col0'));" />
                                                    </Listeners>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>

                                        <ext:Label ID="Label1" runat="server" Html="<font >&nbsp;&nbsp;号牌种类：</font>" StyleSpec="margin-right:0px;margin-left:3px">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbPlateType" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText="选择号牌种类..."
                                            SelectOnFocus="true" Width="130">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                            <Store>
                                                <ext:Store ID="StorePlateType" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader>
                                                            <Fields>
                                                                <ext:RecordField Name="col0" Type="String" />
                                                                <ext:RecordField Name="col1" Type="String" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar6" runat="server" Flat="false">
                                    <Items>

                                        <ext:Label ID="Label10" runat="server" Html="<font >&nbsp;&nbsp;违法行为：&nbsp;</font>" StyleSpec="margin-right:0px;margin-left:5px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbPecType" runat="server" Editable="false" DisplayField="col2"
                                            ValueField="col1" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText="选择违法行为..."
                                            SelectOnFocus="true" Width="360">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                            <Store>
                                                <ext:Store ID="StorePecType" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader>
                                                            <Fields>
                                                                <ext:RecordField Name="col0" Type="String" />
                                                                <ext:RecordField Name="col1" Type="String" />
                                                                <ext:RecordField Name="col2" Type="String" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>

                                        <ext:Label ID="Label7" runat="server" Html="<font >&nbsp;&nbsp;行驶速度：</font>" StyleSpec="margin-left:0px;margin-right:5px;">
                                        </ext:Label>
                                        <ext:TextField ID="TxtMinSpeed" runat="server" Width="62" />
                                        <ext:Label ID="Label8" runat="server" Html="<font >-</font>">
                                        </ext:Label>
                                        <ext:TextField ID="TxtMaxSpeed" runat="server" Width="62" />

                                        <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Style="margin-left: 230px;" Text="查询" StyleSpec="margin-left:255px">
                                            <DirectEvents>
                                                <Click OnEvent="TbutQueryClick" Timeout="60000">
                                                    <EventMask ShowMask="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text="重置" StyleSpec="margin-left:0px">
                                            <DirectEvents>
                                                <Click OnEvent="ButResetClick">
                                                    <EventMask ShowMask="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <ext:Panel ID="Panel2" runat="server" Region="Center" DefaultBorder="false" Cls="images-view"
                    Frame="true" Layout="FitLayout">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server" Flat="false">
                            <Items>

                                <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10" />
                                <ext:Button ID="ButFisrt" runat="server" Text="首页" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="TbutFisrt" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButLast" runat="server" Icon="ControlRewindBlue" Style="margin-left: 10px;" Text="上一页" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="TbutLast" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButNext" runat="server" Icon="ControlFastforwardBlue" Style="margin-left: 10px;" Text="下一页"
                                    Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="TbutNext" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButEnd" runat="server" Style="margin-left: 10px;" Text="尾页" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="TbutEnd" />
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Label ID="lblTitle" runat="server" Text="查询结果：当前是第" StyleSpec="margin-left:10px;" />
                                <ext:Label ID="lblCurpage" runat="server" Text="" Cls="pageNumLabel" />
                                <ext:Label ID="Label2" runat="server" Text="页,共有" />
                                <ext:Label ID="lblAllpage" runat="server" Text="" Cls="pageNumLabel" />
                                <ext:Label ID="Label9" runat="server" Text="页,共有" />
                                <ext:Label ID="lblRealcount" runat="server" Text="" Cls="pageNumLabel" />
                                <ext:Label ID="Label12" runat="server" Text="条记录" />
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridPecArea" runat="server" Collapsible="true" AnimCollapse="true"
                            Header="false">
                            <%--  <TopBar>

                                <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StorePecArea">
                                    <Items>
                                       <ext:Label ID="Label2" runat="server" Text="页大小" />
                                        <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="10" />
                                        <ext:ComboBox ID="CmbPaging" runat="server" Width="80">
                                            <Items>
                                                <ext:ListItem Text="10" />
                                                <ext:ListItem Text="15" />
                                                <ext:ListItem Text="20" />
                                                <ext:ListItem Text="30" />
                                                <ext:ListItem Text="50" />
                                            </Items>
                                            <SelectedItem Value="10" />
                                            <Listeners>
                                                <Select Handler="#{PagingToolbar1}.pageSize = parseInt(this.getValue()); #{PagingToolbar1}.doLoad();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:PagingToolbar>
                            </TopBar>--%>
                            <Store>
                                <ext:Store ID="StorePecArea" runat="server" IgnoreExtraFields="false" OnRefreshData="MyData_Refresh">
                                    <AutoLoadParams>
                                        <ext:Parameter Name="start" Value="={0}" />
                                        <ext:Parameter Name="limit" Value="={15}" />
                                    </AutoLoadParams>
                                    <UpdateProxy>
                                        <ext:HttpWriteProxy Method="GET" Url="PeccancyArea.aspx">
                                        </ext:HttpWriteProxy>
                                    </UpdateProxy>
                                    <Reader>
                                        <ext:JsonReader>

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
                                                <ext:RecordField Name="col30" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40" />
                                    <ext:CommandColumn Width="40" Header="详细" ColumnID="Details" Align="Center">
                                        <Renderer Fn="DataAmply" />
                                    </ext:CommandColumn>
                                    <ext:Column Header="车牌号码" Width="75" DataIndex="col2" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="号牌种类" Width="110" DataIndex="col4" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="起点卡口" Width="150" DataIndex="col16">
                                    </ext:Column>
                                    <ext:Column Header="终点卡口" DataIndex="col18" Width="150" />
                                    <ext:Column Header="驶入驶出时间" Width="150" DataIndex="col21" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="路程" Width="80" DataIndex="col10" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="耗时" Width="65" DataIndex="col12" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="速度/限速" Width="80" DataIndex="col14" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header="超速比例" Width="80" DataIndex="col26" Align="Center">
                                        <Renderer Fn="pctChange" />
                                    </ext:Column>
                                    <ext:Column Header="违法行为" Width="150" DataIndex="col9">
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    <DirectEvents>
                                        <RowSelect OnEvent="ApplyClick" Buffer="250">
                                            <ExtraParams>
                                                <ext:Parameter Name="data" Value="record.data" Mode="Raw" />
                                            </ExtraParams>
                                        </RowSelect>
                                    </DirectEvents>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <Listeners>
                                <CellClick Fn="cellClick" />
                            </Listeners>
                            <DirectEvents>
                                <CellClick OnEvent="ShowDetails" Failure="Ext.MessageBox.alert('加载失败', '提示');">
                                    <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="={#{GridPecArea}.body}" />
                                    <ExtraParams>
                                        <ext:Parameter Name="data" Value="params[0].getStore().getAt(params[1]).data" Mode="Raw" />
                                    </ExtraParams>
                                </CellClick>
                            </DirectEvents>
                            <View>
                                <ext:GridView ID="GridView1" runat="server" ForceFit="true" TrackOver="true" />
                            </View>
                            <ToolTips>
                                <ext:ToolTip
                                    ID="RowTip"
                                    runat="server"
                                    Target="={GridPecArea.getView().mainBody}"
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
                <%--右部--%>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Frame="true"
                    Title="详细信息" Width="0" Height="0" Icon="Images" DefaultAnchor="100%" Collapsible="true"
                    AutoScroll="true" Collapsed="true">
                    <Content>
                        <div class="photoblock-many">
                            <center>
                                <div id="divplateId" style="width: 100%; font-size: 30pt; font-family: 微软雅黑; color: white; background-color: blue;">
                                </div>
                                <div class="container" style="width: 100%; height: 220px">
                                    <div class="fis">
                                        <img id="zjwj1" style="cursor: pointer" onclick="$.openPhotoGallery(this);" class="photo"
                                            src="../images/NoImage.png" alt="车辆图片(图片点击滚轮缩放)" width="100%" height="220" />
                                    </div>

                                    <div class="fis">
                                        <img id="zjwj2" style="cursor: pointer" onclick="$.openPhotoGallery(this);" class="photo"
                                            src="../images/NoImage.png" alt="车辆图片(图片点击滚轮缩放)" width="100%" height="220" />
                                    </div>

                                    <div class="fis">
                                        <img id="zjwj3" style="cursor: pointer" onclick="$.openPhotoGallery(this);" class="photo"
                                            src="../images/NoImage.png" alt="车辆图片(图片点击滚轮缩放)" width="100%" height="220" />
                                    </div>
                                </div>
                            </center>
                        </div>
                    </Content>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
<script type="text/javascript">
    laydate.skin('danlan');
    var start = {
        elem: '#start',
        format: 'YYYY-MM-DD hh:mm:ss',
        //min: laydate.now(), //设定最小日期为当前日期
        max: '2099-06-16 23:59:59', //最大日期
        istime: true,
        istoday: true,
        choose: function (datas) {
            end.min = datas; //开始日选好后，重置结束日的最小日期
            end.start = datas; //将结束日的初始值设定为开始日
            $("#end").click();//开始时间选中后，自动弹出结束时间
            var tt = $("#start").html();

            PeccancyArea.GetDateTime(true, tt);

        }

    };

    var end = {
        elem: '#end',
        format: 'YYYY-MM-DD hh:mm:ss',
        min: laydate.now(),
        max: '2099-06-16 23:59:59',
        istime: true,
        istoday: true,
        choose: function (datas) {
            start.max = datas; //结束日选好后，重置开始日的最大日期
            var tt = $("#end").html();
            PeccancyArea.GetDateTime(false, tt);
        }

    };

    laydate(start);
    laydate(end);
</script>
</html>