<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmInfoQuery.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.AlarmInfoQuery" %>

<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%# GetLangStr("AlarmInfoQuery47","报警信息查询") %></title>
    <meta name="GENERATOR" content="MSHTML 8.00.7600.16853" />
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js"></script>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" href="../Css/chooser.css" type="text/css" />
    <link rel="Stylesheet" href="../Styles/datetime.css" type="text/css" />
    <link rel="Stylesheet" href="../Styles/hphm/autohphm.css" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <link rel="stylesheet" href="../Css/showphotostyle.css" type="text/css" />
    <!--图片放大开始-->
    <script type="text/javascript" src="../Scripts/Zoom/jquery.photo.gallery.js"></script>
    <!--图片放大结束-->
    <!--卡口选择插件引用开始-->

    <link href="../KakouSelect/css/zTreeStyle/zTreeStyleAlarm.css" rel="stylesheet" />
    <script type="text/javascript" src="../KakouSelect/js/jquery.ztree.core.js"></script>
    <script type="text/javascript" src="../KakouSelect/js/jquery.ztree.excheck.js"></script>
    <!--卡口选择插件引用结束-->

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
            width: 33.33333%;
            height: 220px;
        }

        #FormPanel1-xcollapsed {
            display: none !important;
        }

        #vehicleHead_Panel1 .x-btn {
            border-radius: 0px;
            border: none;
        }

        #vehicleHead_Panel1 {
            background: white;
        }

            #vehicleHead_Panel1 button {
                height: 24px;
            }

            #vehicleHead_Panel1 #ext-gen210 {
                margin-top: -2px;
            }
    </style>

    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="UTF-8"></script>
    <script src="../Scripts/showphoto.js" language="JavaScript" type="text/javascript" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <%-- <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>--%>
    <script type="text/javascript">
        $(function () {
            $("body").delegate("#TxtplateId", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#CmbPlateType").click();
                }
            })
        })
    </script>
    <script type="text/javascript">
        var DataAmply = function () {
            return '<img class="imgEdit" ext:qtip=""+<%# GetLangStr("AlarmInfoQuery48","查看详细信息") %>+"" style="cursor:pointer;" src="../images/button/vcard_edit.png" />';
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
        var change = function (obj, value) {
            obj.setValue(value.toUpperCase());
        };

        var changetype = function (value) {
            if (value == '1') {
                return '<%# GetLangStr("AlarmInfoQuery49", "报警") %>'
            }
            else {
                return '<%#  GetLangStr("AlarmInfoQuery50", "预警") %>'
            }
        };
    </script>
    <script type="text/javascript">
        function ShowImage(image1, image2, image3, palteid, platetype) {
            document.getElementById("zjwj1").src = image1;
            document.getElementById("zjwj2").src = image2;
            //document.getElementById("zjwj3").src = image3;
            ChangeBackColor("divplateId", platetype, palteid);
            // ButCsv.Disabled = false;
            ButExcel.Disabled = false;
            // ButXml.Disabled = false;
            // ButPrint.Disabled = false;

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

    <%--卡口列表js--%>
    <script type="text/javascript" language="javascript">
        function clearTime(start, end) {
            document.getElementById("start").innerText = start;
            document.getElementById("end").innerText = end;
            CmbPlateType.triggers[0].hide();
            CmbAlarmType.triggers[0].hide();

        }
        function directclear() {
            try {
                clearSelect(TreeStation, FieldStation);
                // clearSelect(document.getElementById("TreeStation"), document.getElementById("FieldStation"));
            } catch (e) {

            }

        }
        //清理选中
        var clearSelect = function (tree, field) {
            var ids = field.getValue();
            if (ids.length > 0) {
                try {
                    //设置 取消勾选
                    tree.setChecked({ ids: ids, silent: false });
                } catch (e) {
                }
            }
            //tree.getRootNode().collapseChildNodes(true);
        };
        // 获得选中value
        var getValues = function (tree) {
            var msg = [],
                selNodes = tree.getChecked();
            Ext.each(selNodes, function (node) {
                msg.push(node.id);
            });
            return msg.join(",");
        };
        // 获得选中text
        var getText = function (tree) {
            var msg = [],
                selNodes = tree.getChecked();
            Ext.each(selNodes, function (node) {
                msg.push(node.text);
            });
            return msg.join(",");
        };

        var syncValue = function (value) {
            var tree = this.component;
            if (tree.rendered) {
                if (value) {
                    var ids = value.split(",");
                    try {
                        tree.setChecked({ ids: ids, silent: true });
                        tree.getSelectionModel().clearSelections();
                        Ext.each(ids, function (id) {
                            var node = tree.getNodeById(id);
                            if (node) {
                                node.ensureVisible(function () {
                                    tree.getSelectionModel().select(tree.getNodeById(this.id), null, true);
                                }, node);
                            }
                        }, this);
                    } catch (e) { }
                }
            }
        };
    </script>

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
            var rowIndex = GridAlarmInfo.view.findRowIndex(this.triggerElement),
                cellIndex = GridAlarmInfo.view.findCellIndex(this.triggerElement),
                record = StoreAlarmInfo.getAt(rowIndex),
                fieldName = GridAlarmInfo.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);
            if (fieldName == "col6") {

                data = data.toString().substring(0, 10) + " " + data.toString().substring(11, 19);
            }
            this.body.dom.innerHTML = data;
        };
    </script>

    <!--卡口选择插件Js开始-->
    <script type="text/javascript">
        //选中
        var setSelect = function (kakouNames) {
            Window1.hide();
            $.post("Getjson.ashx", "", function (data) {
                zNodes = eval(data);
                if (zNodes != null) {
                    $.fn.zTree.init($("#treeDemo"), setting, zNodes);
                }
            });
            $("#kakou").val(kakouNames);
        };
        var setting = {
            check: {
                enable: true,
                chkboxType: { "Y": "ps", "N": "ps" }
            },
            view: {
                dblClickExpand: false
            },
            data: {
                simpleData: {
                    enable: true
                }
            },
            callback: {
                beforeClick: beforeClick,
                onCheck: onCheck
            }
        };

        function beforeClick(treeId, treeNode) {
            var zTree = $.fn.zTree.getZTreeObj("treeDemo");
            zTree.checkNode(treeNode, !treeNode.checked, null, true);
            return false;
        }
        var xianshi = null;
        //选择卡口复选框时触发
        function onCheck(e, treeId, treeNode) {
            try {
                var zTree = $.fn.zTree.getZTreeObj("treeDemo");

                nodes = zTree.getCheckedNodes(true);

                var v = "";
                var ids = "";
                for (var i = 0, l = nodes.length; i < l; i++) {
                    if (nodes[i].isParent == true) {//把部门去除
                        continue;
                    }
                    v += nodes[i].name + ",";
                    ids += nodes[i].id + ",";
                }
                if (v.length > 0) v = v.substring(0, v.length - 1);
                if (ids.length > 0) ids = ids.substring(0, ids.length - 1);
                var cityObj = $("#kakou");
                cityObj.attr("value", v);
                //alert(ids);
                $("#kakouId").val(ids);

            } catch (e) {

            }

        }

        //显示卡口下拉
        function showMenu(event) {
            $("#selectKakou").css("display", "none");
            var cityObj = $("#kakou");
            var cityOffset = $("#kakou").offset();
            if ($("#menuContent").css("display") == "block") {
                $("#menuContent").css("display", "none");
            }
            else {
                $("#menuContent").css({ left: (cityOffset.left) + "px", top: (cityOffset.top) + cityObj.outerHeight() + "px" }).slideDown("fast");

            }
            event.stopPropagation();
        }
        //隐藏卡口下拉
        function hideMenu() {
            $("#menuContent").css("display", "none");
        }
        function hideMenuSelect() {
            $("#selectKakou").css("display", "none");
        }
        //清除
        function clearMenu() {
            $("#kakou").val("");

            var zTree = $.fn.zTree.getZTreeObj("treeDemo");
            zTree.checkAllNodes(false);
            AlarmInfoQuery.ClearKakou();
        }
        var zNodes = null;

        $(document).ready(function () {
            $.post("../Passcar/Getjson.ashx", "", function (data) {
                zNodes = eval(data);
                if (zNodes != null) {
                    $.fn.zTree.init($("#treeDemo"), setting, zNodes);
                }
            });

        });
        document.onclick = function () {

            //$("#menuContent").hide();

        }
        function showSelect(event) {
            var code = event.keyCode;
            if (code == 13 || code == 32) {
                AlarmInfoQuery.GetKakou();
            }
        }
        function setUl(lis) {
            $("#menuContent").css("display", "none");
            var cityObj = $("#kakou");
            var cityOffset = $("#kakou").offset();

            $("#selectKakou").css({ left: (cityOffset.left) + "px", top: (cityOffset.top) + cityObj.outerHeight() + "px" }).slideDown("fast");
            var json = eval(lis);
            var strs = "";
            if (json[0].name == "none") {
                strs += "<li style='margin-top:150px;margin-left:120px;'> <%#  GetLangStr("AlarmInfoQuery51", "当前没查询到数据") %> </li>";
        } else {
            for (var i = 0; i < json.length; i++) {
                strs += "<li onclick='setInput(this)' style='margin-left:10px;margin-top:5px;color:white;height:28px;line-height:28px;text-align:left;text-indent:8px;font-size:14px;overflow:hidden;' class='" + json[i].id + "'>" + json[i].name + " </li>";
            }
        }
        $("#showKakou").html(strs);

    }
    //卡口模糊查询的时候，点击下面卡口给文本框赋值
    function setInput(li) {
        $("#kakou").val(""); $("#kakouId").val("");
        $("#kakou").val(li.innerText);
        $("#kakouId").val(li.className);
        AlarmInfoQuery.SetSession();
        $.post("Getjson.ashx", "", function (data) {
            zNodes = eval(data);
            if (zNodes != null) {
                $.fn.zTree.init($("#treeDemo"), setting, zNodes);
            }
        });
    }
    function returnKakou() {
        $("#menuContent").css("display", "block");
        $("#selectKakou").css("display", "none");
    }
    </script>
    <!--卡口选择插件Js结束-->
</head>
<body>
    <form id="form1" runat="server">
        <div id="append_parent" />
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="AlarmInfoQuery" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Hidden ID="realCount" runat="server" />
        <ext:Hidden ID="realMaxTime" runat="server" />
        <ext:Hidden ID="realMinTime" runat="server" />
        <ext:Hidden ID="curpage" runat="server" />
        <ext:Hidden ID="allPage" runat="server" />
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
        <ext:Viewport ID="Viewport2" runat="server" Layout="border">
            <Items>
                <%--上方--%>
                <ext:FormPanel ID="Panel1" Region="North" runat="server"
                    Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server" Layout="Container">
                            <Items>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:Panel runat="server" BodyBorder="false" ID="panelTime">
                                            <Content>
                                                <div runat="server" id="selectDate" style="width: 490px">
                                                    <span style="float: left; margin-left: 1px; font-size: 15px; height: 24px; line-height: 24px!important; text-align: center">&nbsp;&nbsp;<%# GetLangStr("AlarmInfoQuery1","查询时间：") %></span><li runat="server" class="laydate-icon" id="start" style="width: 157px; margin-left: 0px; float: left; list-style: none; cursor: pointer; height: 22px; line-height: 22px!important"></li>
                                                </div>
                                                <div>
                                                    <span style="float: left; margin-left: 1px; height: 24px; line-height: 24px!important; text-align: center">--</span><li runat="server" class="laydate-icon" id="end" style="width: 157px; margin-left: 12px; float: left; list-style: none; cursor: pointer; height: 22px; line-height: 22px!important"></li>
                                                </div>
                                            </Content>
                                        </ext:Panel>
                                        <ext:Label ID="Label8" runat="server" Text='<%# GetLangStr("AlarmInfoQuery2","布控类型：") %>' StyleSpec="margin-left:1px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="DispatchedComboBox" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("AlarmInfoQuery3","选择布控类型...") %>'
                                            SelectOnFocus="true" Width="80">
                                            <Store>
                                                <ext:Store ID="DispatchedStore" runat="server">
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
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("AlarmInfoQuery4","清除选中") %>' />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("AlarmInfoQuery5","号牌种类：") %>' StyleSpec="margin-left:1px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbPlateType" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("AlarmInfoQuery6","请选择...") %>'
                                            SelectOnFocus="true" Width="120">
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
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("AlarmInfoQuery4","清除选中") %>' />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("AlarmInfoQuery7","号牌号码：") %>' StyleSpec="margin-left:1px;">
                                        </ext:Label>
                                        <ext:Panel ID="Panel4" runat="server" Height="29">
                                            <Content>
                                                <veh:VehicleHead ID="vehicleHead" runat="server" />
                                            </Content>
                                        </ext:Panel>
                                        <ext:TextField ID="TxtplateId" runat="server" Width="150" EmptyText='<%# GetLangStr("AlarmInfoQuery8","六位号牌号码") %>' MaxLength="6">
                                            <Listeners>
                                                <Change Fn="change" />
                                            </Listeners>
                                        </ext:TextField>
                                        <ext:Panel runat="server" ID="pnhphm" Hidden="true" Width="150">
                                            <Content>
                                                <input name="num1" class="haopai_name1" value="" id="haopai_name1" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                <input name="num2" class="haopai_name2" value="" id="haopai_name2" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                <input name="num3" class="haopai_name3" value="" id="haopai_name3" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                <input name="num4" class="haopai_name4" value="" id="haopai_name4" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                <input name="num5" class="haopai_name5" value="" id="haopai_name5" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                                <input name="num6" class="haopai_name6" value="" id="haopai_name6" maxlength="1" onkeyup="cgtxt(this.name,this.value);" type="text" runat="server" />
                                            </Content>
                                        </ext:Panel>
                                        <ext:Panel runat="server">
                                            <Items>
                                                <%-- <ext:Checkbox ID="ChkLike" runat="server" BoxLabel='<%# GetLangStr("PeccancyQuery5","模糊查询")%>' StyleSpec="margin-left: 2px;" /> --%>
                                                <ext:Checkbox ID="ChkLike" Checked="false" runat="server" BoxLabel='<%# GetLangStr("AlarmInfoQuery9","模糊查询")%>' StyleSpec="margin-left:1px;">
                                                    <DirectEvents>
                                                        <Check OnEvent="changtype"></Check>
                                                    </DirectEvents>
                                                </ext:Checkbox>
                                            </Items>
                                        </ext:Panel>
                                        <%--ext:Checkbox ID="ChkLike" runat="server" BoxLabel='<%# GetLangStr("AlarmInfoQuery5","模糊查询") %>' StyleSpec=" margin-left: 10px;" />
                                        <%--ext:Label ID="Label8" runat="server" Text='<%# GetLangStr("AlarmInfoQuery20","处理状态：") %>' StyleSpec="margin-left: 40px; ">
                                        </--ext:Label>
                                        <ext:MultiCombo ID="DealCombo"  Editable="false" ForceSelection="true" TypeAhead="true" Mode="Local"  SelectOnFocus="true" EmptyText='<%# GetLangStr("AlarmInfoQuery33","选择处理状态...") %>' runat="server" Width="129">
                                            <Items>
                                                <ext:ListItem Text='<%# GetLangStr("AlarmInfoQuery22","未处理") %>' AutoDataBind="true" Value="0" />
                                                <ext:ListItem Text='<%# GetLangStr("AlarmInfoQuery21","已处理") %>' AutoDataBind="true" Value="1" />
                                            </Items>
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:MultiCombo--%>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar6" runat="server" Flat="false">
                                    <Items>
                                        <ext:Panel runat="server">
                                            <Content>
                                                <div style="width: 487px; height: 30px;">
                                                    <span style="margin-top: 5px; font-size: 15px; margin-left: 1px; margin-right: 1px; float: left;"><%# GetLangStr("AlarmInfoQuery10","卡口名称：") %></span>
                                                    <input id="kakou" onkeyup="showSelect(event)" runat="server" type="text" value="" onclick="showMenu(event);" />
                                                    <input onclick="showMenu(event);" id="kakouXiala" type="button"></input>
                                                    <%--  <input runat="server" type="button" id="htmlBtn" />--%>
                                                    <input id="kakouId" runat="server" hidden="hidden" />
                                                </div>
                                            </Content>
                                        </ext:Panel>
                                        <ext:Label ID="Label6" runat="server" Text='<%# GetLangStr("AlarmInfoQuery11","消息类型：") %>' StyleSpec="margin-left: 1px; ">
                                        </ext:Label>
                                        <ext:MultiCombo ID="McmbType" runat="server" Width="80">
                                            <Items>
                                                <ext:ListItem Text='<%# GetLangStr("AlarmInfoQuery12","预警") %>' AutoDataBind="true" Value="0" />
                                                <ext:ListItem Text='<%# GetLangStr("AlarmInfoQuery13","报警") %>' AutoDataBind="true" Value="1" />
                                            </Items>
                                            <SelectedItems>
                                                <ext:SelectedListItem Value="1" />
                                            </SelectedItems>
                                        </ext:MultiCombo>
                                        <ext:Label ID="Label5" runat="server" Text='<%# GetLangStr("AlarmInfoQuery14","报警类型：") %>' StyleSpec="margin-left:1px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbAlarmType" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("AlarmInfoQuery15","选择报警类型...") %>'
                                            SelectOnFocus="true" Width="120">
                                            <Store>
                                                <ext:Store ID="StoreAlarmType" runat="server">
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
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("AlarmInfoQuery4","清除选中") %>' />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:Label ID="Label10" runat="server" Text='<%# GetLangStr("AlarmInfoQuery16","处理结果：") %>' StyleSpec="margin-left: 1px; ">
                                        </ext:Label>
                                        <ext:ComboBox ID="ResultCombo" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("AlarmInfoQuery17","选择处理结果...") %>'
                                            SelectOnFocus="true" Width="114">
                                            <Store>
                                                <ext:Store ID="ResultStore" runat="server">
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
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("AlarmInfoQuery4","清除选中") %>' />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                        </ext:ComboBox>

                                        <ext:Label ID="Label7" runat="server" Text='<%# GetLangStr("AlarmInfoQuery18","显示条数：") %>' StyleSpec="margin-lefy:10px;" Hidden="true">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbQueryNum" runat="server" Editable="false" DisplayField="col1"
                                            Width="46" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" Hidden="true"
                                            EmptyText='<%# GetLangStr("AlarmInfoQuery19","选择...") %>' SelectOnFocus="true">
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
                                        <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("AlarmInfoQuery20","查询") %>' StyleSpec=" margin-left: 2px;">
                                            <DirectEvents>
                                                <Click OnEvent="TbutQueryClick" Timeout="60000">
                                                    <EventMask ShowMask="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("AlarmInfoQuery21","重置") %>'>
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
                <%--中间--%>
                <ext:FormPanel ID="FormPanel2" Region="Center" runat="server" Layout="FitLayout"
                    AutoScroll="true">
                    <TopBar>
                        <%--分页--%>
                        <ext:Toolbar ID="Toolbar2" runat="server" Flat="false">
                            <Items>
                                <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10" />
                                <ext:Button ID="ButFisrt" runat="server" Text='<%# GetLangStr("AlarmInfoQuery22","首页") %>' Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="TbutFisrt" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButLast" runat="server" Icon="ControlRewindBlue" Style="margin-left: 10px" Text='<%# GetLangStr("AlarmInfoQuery23","上一页") %>' Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="TbutLast" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButNext" runat="server" Icon="ControlFastforwardBlue" Style="margin-left: 10px" Text='<%# GetLangStr("AlarmInfoQuery24","下一页") %>'
                                    Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="TbutNext" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButEnd" runat="server" Style="margin-left: 10px" Text='<%# GetLangStr("AlarmInfoQuery25","尾页") %>' Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="TbutEnd" />
                                    </DirectEvents>
                                </ext:Button>

                                <ext:Label ID="lblTitle" runat="server" Text='<%# GetLangStr("AlarmInfoQuery26","查询结果：当前是第") %>' StyleSpec=" margin-left:10px;" />
                                <ext:Label ID="lblCurpage" runat="server" Text="" Cls="pageNumLabel" />
                                <ext:Label ID="Label2" runat="server" Text='<%# GetLangStr("AlarmInfoQuery27","页,共有") %>' />
                                <ext:Label ID="lblAllpage" runat="server" Text="" Cls="pageNumLabel" />
                                <ext:Label ID="Label9" runat="server" Text='<%# GetLangStr("AlarmInfoQuery27","页,共有") %>' />
                                <ext:Label ID="lblRealcount" runat="server" Text="" Cls="pageNumLabel" />
                                <ext:Label ID="Label12" runat="server" Text='<%# GetLangStr("AlarmInfoQuery28","条记录") %>' />
                                <ext:ToolbarFill runat="server">
                                </ext:ToolbarFill>
                                <ext:Button ID="ButExcel" runat="server" Text='<%# GetLangStr("AlarmInfoQuery29","导出Excel") %>' AutoPostBack="true" OnClick="ToExcel"
                                    Icon="PageExcel">
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <%--数据展示--%>
                        <ext:GridPanel ID="GridAlarmInfo" runat="server" StripeRows="true" BodyStyle="height:100%;width:100%" TrackMouseOver="true">
                            <Store>
                                <ext:Store ID="StoreAlarmInfo" runat="server" OnRefreshData="MyData_Refresh">
                                    <AutoLoadParams>
                                        <ext:Parameter Name="start" Value="={0}" />
                                        <ext:Parameter Name="limit" Value="={15}" />
                                    </AutoLoadParams>
                                    <%--  <UpdateProxy>
                                        <ext:HttpWriteProxy Method="GET" Url="AlarmInfoQuery.aspx">
                                        </ext:HttpWriteProxy>
                                    </UpdateProxy>--%>
                                    <Reader>
                                        <ext:JsonReader IDProperty="col0">
                                            <Fields>
                                                <ext:RecordField Name="col0" Type="String" />
                                                <ext:RecordField Name="col1" Type="String" />
                                                <ext:RecordField Name="col2" Type="String" />
                                                <ext:RecordField Name="col3" Type="String" />
                                                <ext:RecordField Name="col4" Type="Date" />
                                                <ext:RecordField Name="col5" Type="String" />
                                                <ext:RecordField Name="col6" Type="String" />
                                                <ext:RecordField Name="col7" Type="String" />
                                                <ext:RecordField Name="col8" Type="String" />
                                                <ext:RecordField Name="col9" Type="String" />
                                                <ext:RecordField Name="col10" Type="String" />
                                                <ext:RecordField Name="col11" Type="String" />
                                                <ext:RecordField Name="col12" Type="String" />
                                                <ext:RecordField Name="col13" Type="String" />
                                                <ext:RecordField Name="col14" Type="String" />
                                                <ext:RecordField Name="col15" Type="String" />
                                                <ext:RecordField Name="col16" Type="String" />
                                                <ext:RecordField Name="col17" Type="String" />
                                                <ext:RecordField Name="col18" Type="String" />
                                                <ext:RecordField Name="col19" Type="String" />
                                                <ext:RecordField Name="col20" Type="String" />
                                                <ext:RecordField Name="col21" Type="String" />
                                                <ext:RecordField Name="col22" Type="String" />
                                                <ext:RecordField Name="col23" Type="String" />
                                                <ext:RecordField Name="col24" Type="String" />
                                                <ext:RecordField Name="col25" Type="String" />
                                                <ext:RecordField Name="col26" Type="String" />
                                                <ext:RecordField Name="col27" Type="String" />
                                                <ext:RecordField Name="col28" Type="String" />
                                                <ext:RecordField Name="col29" Type="String" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40" Align="Center"></ext:RowNumbererColumn>
                                    <ext:Column ColumnID="Details" Header='<%# GetLangStr("AlarmInfoQuery30","详细") %>' AutoDataBind="true" Width="50" Align="Center" Fixed="true"
                                        MenuDisabled="true" Resizable="false">
                                        <Renderer Fn="DataAmply" />
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("AlarmInfoQuery31","记录编号") %>' AutoDataBind="true" Width="100" Sortable="true" DataIndex="col0" Hidden="true" />
                                    <ext:Column Header='<%# GetLangStr("AlarmInfoQuery32","报警卡口") %>' AutoDataBind="true" Width="150" Sortable="true" DataIndex="col2">
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("AlarmInfoQuery33","号牌号码") %>' AutoDataBind="true" Width="75" Sortable="true" Align="Center" DataIndex="col3">
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("AlarmInfoQuery34","号牌种类") %>' AutoDataBind="true" Width="80" Sortable="true" Align="Center" DataIndex="col5">
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("AlarmInfoQuery35","布控类型") %>' AutoDataBind="true" Width="80" Sortable="true" Align="Center" DataIndex="col28">
                                    </ext:Column>
                                    <ext:DateColumn Header='<%# GetLangStr("AlarmInfoQuery36","报警时间") %>' AutoDataBind="true" DataIndex="col6" Width="120" Format="yyyy-MM-dd HH:mm:ss" />
                                    <ext:Column Header='<%# GetLangStr("AlarmInfoQuery37","报警类型") %>' AutoDataBind="true" Width="150" Sortable="true" Align="Center" DataIndex="col19">
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("AlarmInfoQuery38","报警原因") %>' AutoDataBind="true" Width="200" Sortable="true" DataIndex="col27">
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("AlarmInfoQuery39","处理结果") %>' AutoDataBind="true" Width="80" Sortable="true" Align="Center" DataIndex="col24">
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("AlarmInfoQuery40","消息类型") %>' AutoDataBind="true" Width="80" Sortable="true" Align="Center" DataIndex="col21">
                                        <Renderer Fn="changetype" />
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
                            </Listeners>
                            <DirectEvents>
                                <CellClick OnEvent="ShowDetails" Buffer="250" Failure="Ext.MessageBox.alert('加载失败', '提示');">
                                    <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="={#{GridAlarmInfo}.body}" />
                                    <ExtraParams>
                                        <ext:Parameter Name="data" Value="params[0].getStore().getAt(params[1]).data" Mode="Raw" />
                                    </ExtraParams>
                                </CellClick>
                            </DirectEvents>
                            <View>
                                <ext:GridView ID="GroupingView1" runat="server" ForceFit="true">
                                </ext:GridView>
                            </View>
                            <ToolTips>
                                <ext:ToolTip
                                    ID="RowTip"
                                    runat="server"
                                    Target="={GridAlarmInfo.getView().mainBody}"
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
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Frame="true"
                    Title='<%# GetLangStr("AlarmInfoQuery41","详细信息") %>' Width="0" Height="0" Icon="Images" DefaultAnchor="100%" Collapsible="true"
                    AutoScroll="true" Collapsed="true">
                    <Content>
                        <div class="photoblock-many">
                            <center>
                                <div id="divplateId" style="width: 100%; font-size: 30pt; font-family: 微软雅黑; color: white; background-color: blue;">
                                </div>
                                <div class="container" style="width: 100%; height: 220px">
                                    <div class="fis">
                                        <img id="zjwj1" style="cursor: pointer" onclick="$.openPhotoGallery(this);" class="photo"
                                            src="../images/NoImage.png" alt="<%#  GetLangStr("AlarmInfoQuery52", "车辆图片(图片点击滚轮缩放)") %>" width="100%" height="220" />
                                    </div>
                                    <div class="fis">
                                        <img id="zjwj2" style="cursor: pointer" onclick="$.openPhotoGallery(this);" class="photo"
                                            src="../images/NoImage.png" alt="<%#  GetLangStr("AlarmInfoQuery52", "车辆图片(图片点击滚轮缩放)") %>" width="100%" height="220" />
                                    </div>
                                </div>
                            </center>
                        </div>
                    </Content>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
        <ext:Window ID="winShow" runat="server" Layout="FitLayout" Hidden="true">
            <Items>
                <ext:Panel runat="server" Region="Center" Layout="ContainerLayout" Title='<%# GetLangStr("AlarmInfoQuery42","报警信息") %>' Border="false" BodyStyle="padding:6px;">
                    <Items>
                        <ext:Container Layout="ColumnLayout" runat="server" Height="125">
                            <Items>
                                <ext:Container runat="server" Layout="FormLayout" LabelAlign="Left" ColumnWidth=".25">
                                    <Items>
                                    </Items>
                                </ext:Container>
                                <ext:Container runat="server" Layout="FormLayout" LabelAlign="Left" ColumnWidth=".3">
                                    <Items>
                                    </Items>
                                </ext:Container>
                                <ext:Container runat="server" Layout="FormLayout" LabelAlign="Left" ColumnWidth=".45">
                                    <Items>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:Container>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Window>
        <!-- 显示卡口下拉框开始-->
        <div>
            <div id="menuContent" class="menuContent" style="display: none; position: absolute; z-index: 999; width: 389px; height: 377px;">
                <!--overflow-y: auto; overflow-x: hidden; -->
                <div style="position: relative; margin-top: 0px; width: 100%; height: 90%; overflow-y: auto; overflow-x: hidden;" class="kkselectStyle">
                    <ul id="treeDemo" class="ztree" style="margin-top: 0px; width: 100%; height: 90%;">
                    </ul>
                </div>
                <div style="position: relative; bottom: 0; height: 5%; padding-bottom: 10px; border-radius: 0px 0px 15px 15px;" class="kkselectStyle">
                    <input type="button" value='<%# GetLangStr("AlarmInfoQuery43","清除") %>' class="func_btn" onclick="clearMenu()" style="margin-left: 100px;" />

                    <input type="button" value='<%# GetLangStr("AlarmInfoQuery44","关闭") %>' class="func_btn" onclick="hideMenu()" />
                </div>
            </div>
            <div id="selectKakou" style="display: none; position: absolute; z-index: 999; width: 389px; height: 377px; overflow-y: auto; overflow-x: hidden;">
                <div style="position: relative; margin-top: 0px; width: 100%; height: 90%; overflow-y: auto; overflow-x: hidden;" class="kkselectStyle">
                    <ul id="showKakou" style="margin-top: 0px; width: 100%; height: 90%;">
                    </ul>
                </div>

                <div style="position: relative; bottom: 0; height: 5%; padding-bottom: 10px; border-radius: 0px 0px 15px 15px;" class="kkselectStyle">
                    <input type="button" class="func_btn" value='<%# GetLangStr("AlarmInfoQuery45","返回目录") %>' onclick="returnKakou()" style="margin-left: 100px;" />

                    <input type="button" class="func_btn" value='<%# GetLangStr("AlarmInfoQuery46","关闭") %>' onclick="hideMenuSelect()" />
                </div>
            </div>
        </div>

        <!-- 显示卡口下拉框结束-->
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
            AlarmInfoQuery.GetDateTime(true, tt);
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
            AlarmInfoQuery.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>
<script type="text/javascript">

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
            //$(this).removeClass("import").next().remove();
        }

    })
</script>