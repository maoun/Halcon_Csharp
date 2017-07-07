<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeccancyCheck.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.PeccancyCheck" %>

<%@ Register Src="../UIDepartment.ascx" TagName="UIDepartment" TagPrefix="dpart" %>
<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>违法车辆审核</title>
    <meta name="GENERATOR" content="MSHTML 8.00.7600.16853" />
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js"></script>
    <link href="../Css/chooser.css" rel="stylcesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <link rel="stylesheet" href="../Css/showphotostyle.css" type="text/css" />
    <link rel="Stylesheet" href="../Styles/datetime.css" type="text/css" />
    <link rel="Stylesheet" type="text/css" href="../Styles/hphm/autohphm.css" />
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
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="UTF-8"></script>

    <script src="../Scripts/showphoto.js" language="JavaScript" type="text/javascript" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <%--  <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>--%>
    <script type="text/javascript">
        function clearTime(start, end) {
            document.getElementById("start").innerText = start;
            document.getElementById("end").innerText = end;
            CmbPlateType.triggers[0].hide();
            CmbPecType.triggers[0].hide();
            CmbDataSource.triggers[0].hide();
            CmbDirection.triggers[0].hide();
            CmbDealType.triggers[0].hide();
            CmbLocation.triggers[0].hide();
        }
      
        var DataAmply = function () {
            return '<img class="imgEdit" ext:qtip=  "查看详细信息" style="cursor:pointer;" src="../images/button/vcard_edit.png" />';
        };
        var VideoAmply = function (value) {
            return '';
        };
        var cellClick = function (grid, rowIndex, columnIndex, e) {
            var t = e.getTarget(),
            //record = grid.getStore().getAt(rowIndex),  // Get the Record
                columnId = grid.getColumnModel().getColumnId(columnIndex); // Get column id

            if (columnId == "Details") {
                return true;
            }
            return false;
        };
        var prepare = function (grid, command, record, row, col, value) {
            //debugger;

            if (value == null && command.command == "VideShow") {
                command.hidden = true;
                command.hideMode = "visibility";
            }
        };
        var saveData = function () {
            GridData.setValue(Ext.encode(GridAlarmInfo.getRowsValues(false)));
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $("body").delegate("#TxtplateId", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#CmbPlateType").click();
                }
            })
        })

        var changeUpper = function (obj, value) {
            obj.setValue(value.toUpperCase());
        };
    </script>
    <script type="text/javascript">
        function ShowImage(image1, image2, image3, palteid, platetype) {
            document.getElementById("zjwj1").src = image1;
            document.getElementById("zjwj2").src = image2;
            document.getElementById("zjwj3").src = image3;
            ChangeBackColor("divplateId", platetype, palteid);
        }
    </script>
    <script type="text/javascript">
        var IMGDIR = '../images/sets';
        var attackevasive = '0';
        var gid = 0;
        var fid = parseInt('0');
        var tid = parseInt('0');
    </script>
    <script type="text/javascript">
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
    </script>
    <script type="text/javascript">
        var filterTree = function (el, e) {
            var tree = uiDepartment_TreeDepartment,
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
            var rowIndex = GridPanel1.view.findRowIndex(this.triggerElement),
                cellIndex = GridPanel1.view.findCellIndex(this.triggerElement),
                record = StorePeccancy.getAt(rowIndex),
                fieldName = GridPanel1.getColumnModel().getDataIndex(cellIndex),
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
        <div id="append_parent" />
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PeccancyCheck" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Hidden ID="realCount" runat="server" />
        <ext:Hidden ID="curpage" runat="server" />
        <ext:Hidden ID="allPage" runat="server" />
        <ext:Viewport ID="Viewport2" runat="server" Layout="border">
            <Items>
                <%--上方--%>
                <ext:FormPanel ID="Panel1" Region="North" runat="server" Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server" Layout="Container">
                            <Items>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:Panel runat="server" BodyBorder="false">
                                            <Content>
                                                <div runat="server" id="selectDate" style="width: 475px">
                                                    <span class="laydate-span" style="margin-left: 0px; height: 24px;">&nbsp;&nbsp;<%# GetLangStr("PeccancyCheck1","查询时间：") %></span>
                                                    <li runat="server" class="laydate-icon" id="start" style="width: 150px; margin-left: 1px; height: 22px;"></li>
                                                </div>
                                                <div>
                                                    <span class="laydate-span" style="margin-left: 20px; height: 24px;">--</span>
                                                    <li runat="server" class="laydate-icon" id="end" style="width: 150px; margin-left: 16px; height: 22px;"></li>
                                                </div>
                                            </Content>
                                        </ext:Panel>

                                        <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("PeccancyCheck3","号牌种类：") %>' StyleSpec="margin-left: 10px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbPlateType" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PeccancyCheck4","请选择...")%>'
                                            SelectOnFocus="true" Width="150">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PeccancyQuery36","清除选中：") %>' AutoDataBind="true" />
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

                                        <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("PeccancyCheck2","号牌号码：") %>' StyleSpec="margin-left: 10px; ">
                                        </ext:Label>
                                        <ext:Panel ID="Panel2" runat="server" Height="30">
                                            <Content>
                                                <veh:VehicleHead ID="WindowEditor1" runat="server" />
                                            </Content>
                                        </ext:Panel>

                                        <ext:Panel runat="server" Layout="ColumnLayout" Height="30">
                                            <Items>
                                                <ext:TextField ID="TxtplateId" runat="server" Width="150" EmptyText='<%# GetLangStr("PeccancyQuery37","六位号牌号码") %>' MaxLength="6">
                                                    <Listeners>
                                                        <Change Fn="changeUpper " />
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
                                                <ext:Checkbox ID="ChkLike" runat="server" Checked="false" BoxLabel='<%# GetLangStr("PeccancyCheck5","模糊查询")%>' StyleSpec="margin-left: 2px;">
                                                    <DirectEvents>
                                                        <Check OnEvent="changtype"></Check>
                                                    </DirectEvents>
                                                </ext:Checkbox>
                                            </Items>
                                        </ext:Panel>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar6" runat="server" Flat="false">
                                    <Items>
                                        <ext:Label ID="Label5" runat="server" Text='<%# GetLangStr("PeccancyCheck6","违法行为：") %>' StyleSpec="margin-left: 10px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbPecType" runat="server" Editable="false" DisplayField="col2"
                                            ValueField="col1" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PeccancyCheck7","选择违法行为...")%>'
                                            ListWidth="400" SelectOnFocus="true" Width="390">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PeccancyQuery36","清除选中：") %>' AutoDataBind="true" />
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
                                                                <ext:RecordField Name="col1" Type="String" />
                                                                <ext:RecordField Name="col2" Type="String" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                        <ext:Label ID="Label7" runat="server" Text='<%# GetLangStr("PeccancyCheck8","数据来源：") %>' StyleSpec="margin-left: 10px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbDataSource" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PeccancyCheck9","选择数据来源...")%>'
                                            SelectOnFocus="true" Width="150">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PeccancyQuery36","清除选中：") %>' AutoDataBind="true" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();#{CmbLocation}.clearValue(); #{StoreLocation}.reload();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                            <Store>
                                                <ext:Store ID="StoreDataSource" runat="server">
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
                                        <ext:Label ID="Label8" runat="server" Text='<%# GetLangStr("PeccancyCheck10","所属机构：") %>' StyleSpec="margin-left: 10px;">
                                        </ext:Label>
                                        <ext:Panel ID="Panel3" runat="server" Height="30" Layout="AnchorLayout" Border="false">
                                            <Content>
                                                <dpart:UIDepartment ID="uiDepartment" runat="server" Width="250" ListWidth="300" />
                                            </Content>
                                        </ext:Panel>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:Label ID="Label4" runat="server" Text='<%# GetLangStr("PeccancyCheck11","违法地点：") %>' StyleSpec="margin-left: 10px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbLocation" runat="server" Editable="false" DisplayField="col2"
                                            Width="390" ValueField="col1" TypeAhead="true" Mode="Local" ForceSelection="true"
                                            ListWidth="390" EmptyText='<%# GetLangStr("PeccancyCheck12","选择违法地点...")%>' SelectOnFocus="true">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PeccancyQuery36","清除选中：") %>' AutoDataBind="true" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();#{CmbDirection}.clearValue(); #{StoreDirection}.reload();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                            <Store>
                                                <ext:Store ID="StoreLocation" runat="server" OnRefreshData="DataSourceRefresh">
                                                    <Reader>
                                                        <ext:JsonReader IDProperty="col1">
                                                            <Fields>
                                                                <ext:RecordField Name="col1" Type="String" />
                                                                <ext:RecordField Name="col2" Type="String" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                        <ext:Label ID="Label10" runat="server" Text='<%# GetLangStr("PeccancyCheck13","行驶方向：") %>' StyleSpec="margin-left: 10px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbDirection" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PeccancyCheck14","选择行驶方向...")%>'
                                            ListWidth="150" SelectOnFocus="true" Width="150">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PeccancyQuery36","清除选中：") %>' AutoDataBind="true" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                            <Store>
                                                <ext:Store ID="StoreDirection" runat="server" OnRefreshData="TgsRefresh">
                                                    <%-- 在左边里面的加了不会加载 AutoLoad="false" OnRefreshData="TgsRefresh"--%>
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
                                        <ext:Label ID="Label6" runat="server" Text='<%# GetLangStr("PeccancyCheck15","处理状态：") %>' StyleSpec="margin-left: 10px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbDealType" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PeccancyCheck16","选择处理状态...")%>'
                                            SelectOnFocus="true" Width="110">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PeccancyQuery36","清除选中：") %>' AutoDataBind="true" />
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
                                        <ext:Label ID="Label9" runat="server" Text='<%# GetLangStr("PeccancyCheck17","提取条数：") %>' StyleSpec="margin-left: 5px; ">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbQueryNum" runat="server" Editable="false" DisplayField="col1"
                                            Width="60" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                            EmptyText='<%# GetLangStr("PeccancyCheck18","选择...")%>' SelectOnFocus="true">
                                            <Store>
                                                <ext:Store ID="StoreQueryNum" runat="server">
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
                                        <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
                                        <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("PeccancyCheck19","查询")%>'>
                                            <DirectEvents>
                                                <Click OnEvent="TbutQueryClick" Timeout="60000">
                                                    <EventMask ShowMask="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("PeccancyCheck20","重置")%>'>
                                            <DirectEvents>
                                                <Click OnEvent="ButResetClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <%--中部--%>
                <ext:FormPanel ID="FormPanel2" Region="Center" runat="server" AutoScroll="true" Layout="FitLayout">
                    <Items>
                        <ext:GridPanel ID="GridPanel1" runat="server" StripeRows="true" Header="false" BodyStyle="height:100%;width:100%" TrackMouseOver="true">
                            <TopBar>
                                <ext:Toolbar runat="server" Layout="Container">
                                    <Items>
                                        <ext:Toolbar runat="server">
                                            <Items>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10" />
                                                <ext:Button ID="ButFisrt" runat="server" StyleSpec="margin-left:10px;" Text='<%# GetLangStr("PeccancyQuery38","首页")%>' Disabled="true" Hidden="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutFisrt" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButLast" runat="server" StyleSpec="margin-left:10px;" Icon="ControlRewindBlue" Text='<%# GetLangStr("PeccancyQuery39","上一页")%>' Disabled="true" Hidden="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutLast" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButNext" runat="server" StyleSpec="margin-left:10px;" Icon="ControlFastforwardBlue" Text='<%# GetLangStr("PeccancyQuery40","下一页")%>' Hidden="true"
                                                    Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutNext" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButEnd" runat="server" StyleSpec="margin-left:10px;" Text='<%# GetLangStr("PeccancyQuery41","尾页")%>' Disabled="true" Hidden="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutEnd" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Label ID="lblTitle" runat="server" Text='<%# GetLangStr("PeccancyQuery42","查询结果：当前是第")%>' StyleSpec="margin-left:10px;" />
                                                <ext:Label ID="lblCurpage" runat="server" Text="1" Cls="pageNumLabel" />
                                                <ext:Label ID="Label2" runat="server" Text='<%# GetLangStr("PeccancyQuery43","页,共有")%>' />
                                                <ext:Label ID="lblAllpage" runat="server" Text="0" Cls="pageNumLabel" />
                                                <ext:Label ID="Label11" runat="server" Text='<%# GetLangStr("PeccancyQuery43","页,共有")%>' />
                                                <ext:Label ID="lblRealcount" runat="server" Text="0" Cls="pageNumLabel" />
                                                <ext:Label ID="Label12" runat="server" Text='<%# GetLangStr("PeccancyQuery44","条记录")%>' />
                                                <ext:ToolbarFill ID="ToolbarFill1" runat="server"></ext:ToolbarFill>
                                                <ext:Button ID="ButCheck" runat="server" Icon="TableSave" Text='<%# GetLangStr("PeccancyCheck22","开始审核")%>'>
                                                    <DirectEvents>
                                                        <Click OnEvent="ButCheckClick" />
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Store>
                                <ext:Store ID="StorePeccancy" runat="server">

                                    <UpdateProxy>
                                        <ext:HttpWriteProxy Method="GET" Url="PeccancyCheck.aspx">
                                        </ext:HttpWriteProxy>
                                    </UpdateProxy>
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
                                                <ext:RecordField Name="col30" />
                                                <ext:RecordField Name="col31" />
                                                <ext:RecordField Name="col32" />
                                                <ext:RecordField Name="col33" />
                                                <ext:RecordField Name="col34" />
                                                <ext:RecordField Name="col35" />
                                                <ext:RecordField Name="col36" />
                                                <ext:RecordField Name="col37" />
                                                <ext:RecordField Name="col38" />
                                                <ext:RecordField Name="col39" />
                                                <ext:RecordField Name="col40" />
                                                <ext:RecordField Name="col41" />
                                                <ext:RecordField Name="col42" />
                                                <ext:RecordField Name="col43" />
                                                <ext:RecordField Name="col44" />
                                                <ext:RecordField Name="col45" />
                                                <ext:RecordField Name="col46" />
                                                <ext:RecordField Name="col47" />
                                                <ext:RecordField Name="col48" />
                                                <ext:RecordField Name="col49" />
                                                <ext:RecordField Name="col50" />
                                                <ext:RecordField Name="col51" />
                                                <ext:RecordField Name="col52" />
                                                <ext:RecordField Name="col53" />
                                                <ext:RecordField Name="col54" />
                                                <ext:RecordField Name="col55" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                                    <ext:Column ColumnID="Details" Header='<%# GetLangStr("PeccancyCheck23","详细")%>' AutoDataBind="true" Width="50" Align="Center" Fixed="true"
                                        MenuDisabled="true" Resizable="false">
                                        <Renderer Fn="DataAmply" />
                                    </ext:Column>
                                    <ext:Column ColumnID="Video" Header='<%# GetLangStr("PeccancyCheck24","视频")%>' AutoDataBind="true" Width="40" DataIndex="col26" Hidden="true">
                                        <Renderer Fn="VideoAmply" />
                                        <Commands>
                                            <ext:ImageCommand CommandName="VideShow" Icon="Television" Text="">
                                                <ToolTip Text='<%# GetLangStr("PeccancyCheck24","视频")%>' AutoDataBind="true" />
                                            </ext:ImageCommand>
                                        </Commands>
                                        <PrepareCommand Fn="prepare" />
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("PeccancyCheck26","记录编号")%>' AutoDataBind="true" Width="80" Sortable="true" DataIndex="col0" Hidden="true" />
                                    <ext:Column Header='<%# GetLangStr("PeccancyCheck27","违法地点")%>' AutoDataBind="true" Width="120" Sortable="true" DataIndex="col8" Align="Left">
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("PeccancyCheck28","号牌号码")%>' AutoDataBind="true" Width="75" Sortable="true" DataIndex="col3" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("PeccancyCheck29", "号牌种类")%>' AutoDataBind="true" Width="100" Sortable="true" DataIndex="col2" Align="Center">
                                    </ext:Column>
                                    <ext:DateColumn Header='<%# GetLangStr("PeccancyCheck30","违法时间" )%>' AutoDataBind="true" DataIndex="col6" Width="120" Format="yyyy-MM-dd HH:mm:ss" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("PeccancyCheck31","违法行为")%>' AutoDataBind="true" Width="150" Sortable="true" DataIndex="col5" Align="Left">
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("PeccancyCheck32","行驶方向")%>' AutoDataBind="true" Width="100" Sortable="true" DataIndex="col11" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("PeccancyCheck33","速度/限速")%>' AutoDataBind="true" Width="80" Sortable="true" DataIndex="col12" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("PeccancyCheck34","处理状态")%>' AutoDataBind="true" Width="100" Sortable="true" DataIndex="col20" Align="Center">
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("PeccancyCheck35", "所属机构")%>' AutoDataBind="true" Width="160" Sortable="true" DataIndex="col14" Align="Center">
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
                            <LoadMask ShowMask="true" />
                            <Listeners>
                                <CellClick Fn="cellClick" />
                                <Command Handler="PeccancyCheck.VideoShow(command, record.data.col26);" />
                            </Listeners>
                            <DirectEvents>
                                <CellClick OnEvent="ShowDetails" Failure="Ext.MessageBox.alert('加载失败', '提示');">
                                    <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="={#{GridPanel1}.body}" />
                                    <ExtraParams>
                                        <ext:Parameter Name="data" Value="params[0].getStore().getAt(params[1]).data" Mode="Raw" />
                                    </ExtraParams>
                                </CellClick>
                            </DirectEvents>
                            <View>
                                <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                                </ext:GridView>
                            </View>
                            <ToolTips>
                                <ext:ToolTip
                                    ID="RowTip"
                                    runat="server"
                                    Target="={GridPanel1.getView().mainBody}"
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
                <%--右部--%>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Frame="true"
                    Title='<%# GetLangStr("PeccancyCheck46","详细信息")%>' Width="0" Height="0" Icon="Images" DefaultAnchor="100%" Collapsible="true"
                    AutoScroll="true" Collapsed="true">
                    <Content>
                        <div class="photoblock-many">
                            <center>
                                <div id="divplateId" style="width: 100%; font-size: 30pt; font-family:'<%# GetLangStr("PeccancyCheck47","微软雅黑")%>'; color: white; background-color: blue;">
                                </div>
                                <div class="container" style="width: 100%; height: 220px">
                                    <div class="fis">
                                        <img id="zjwj1" style="cursor: pointer" onclick="$.openPhotoGallery(this);" class="photo"
                                            src="../images/NoImage.png" alt="+'<%# GetLangStr("PeccancyQuery45", "车辆图片(图片点击滚轮缩放)")%>'+" width="100%" height="220" />
                                    </div>

                                    <div class="fis">
                                        <img id="zjwj2" style="cursor: pointer" onclick="$.openPhotoGallery(this);" class="photo"
                                            src="../images/NoImage.png" alt="+'<%# GetLangStr("PeccancyQuery45", "车辆图片(图片点击滚轮缩放)")%>'+" width="100%" height="220" />
                                    </div>

                                    <div class="fis">
                                        <img id="zjwj3" style="cursor: pointer" onclick="$.openPhotoGallery(this);" class="photo"
                                            src="../images/NoImage.png" alt="+'<%# GetLangStr("PeccancyQuery45", "车辆图片(图片点击滚轮缩放)")%>'+" width="100%" height="220" />
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
</html>

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
            end.start = datas //将结束日的初始值设定为开始日
            $("#end").click();//开始时间选中后，自动弹出结束时间
            var tt = $("#start").html();
            PeccancyCheck.GetDateTime(true, tt);
            //alert(tt);
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
            PeccancyCheck.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>
<script type="text/javascript">
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
</script>