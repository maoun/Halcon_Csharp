<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmportSetting.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.AlarmportSetting" %>

<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<%@ Register Src="../UIDepartment.ascx" TagName="UIDepartment" TagPrefix="dpart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>卡口流量报警设置</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <style type="text/css">
        #FieldPerson {
            width: 148px;
        }
    </style>
    <!--卡口选择插件引用开始-->
    <!--<script type="text/javascript" src="../KakouSelect/js/jquery-1.4.4.min.js"></script>-->
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js"></script>
    <link href="../KakouSelect/css/demo.css" rel="stylesheet" />
    <link href="../KakouSelect/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" />
    <script type="text/javascript" src="../KakouSelect/js/jquery.ztree.core.js"></script>
    <script type="text/javascript" src="../KakouSelect/js/jquery.ztree.excheck.js"></script>
    <!--卡口选择插件引用结束-->

    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>

    <script type="text/javascript" src="../Scripts/UIContorl.js" charset="utf-8"></script>
    <script type="text/javascript">
        var template = '<span style="color:{0};">{1}</span>';
        //var change = function (value) {
        //    return String.format(template, (value == "已布控") ? "green" : "red", value);
        //};
        var changetime = function (value) {
            var mydate = Ext.util.Format.date(new Date(), 'Y-m-d H:i:s');
            return String.format(template, (value > mydate) ? "green" : "red", value);
        };
        var saveData = function () {
            GridData.setValue(Ext.encode(GridAlarmport.getRowsValues(false)));
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
        $(function () {
            $("body").delegate("#TxtplateId", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#CmbPlateType").click();
                }
            })
        })
        $(function () {
            $("body").delegate("#txtHphm", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#cmbHpzl").click();
                }
            })
        })
        var changeUpper = function (obj, value) {
            obj.setValue(value.toUpperCase());
        };
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
    </script>

    <script type="text/javascript" language="javascript">
        //清理选中
        function directclear() {
            try {
                clearSelect(TreeStation, FieldStation);
                //clearSelect2(TreePerson, FieldPerson);
                // clearSelect(document.getElementById("TreeStation"), document.getElementById("FieldStation"));
            } catch (e) {

            }

        }
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
        var clearSelect2 = function (tree, field) {
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
        //获得选中value
        var getValues = function (tree) {
            var msg = [],
                selNodes = tree.getChecked();
            Ext.each(selNodes, function (node) {
                msg.push(node.id);
            });
            return msg.join(",");
        };
        //获得选中值text
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

        //function inputExcel() {
        //    try {
        //        var excelText = document.getElementById("ExcelFile");
        //        excelText.setAttribute("size", "16");
        //    } catch (e) {

        //    }

        //}
    </script>

    <!--查询流量报警配置卡口选择插件Js开始-->
    <script type="text/javascript">
        ////选中
        //var setSelect = function (kakouNames) {
        //    Window1.hide();
        //    $.post("Getjson.ashx", "", function (data) {
        //        zNodes = eval(data);
        //        if (zNodes != null) {
        //            $.fn.zTree.init($("#treeDemoQuery"), setting, zNodes);
        //        }
        //    });
        //    $("#kakouQuery").val(kakouNames);
        //};
        $(document).ready(function () {
            $.post("../Passcar/Getjson.ashx", "", function (data) {
                zNodes = eval(data);
                if (zNodes != null) {
                    $.fn.zTree.init($("#treeDemo"), setting, zNodes);
                    $.fn.zTree.init($("#treeDemoQuery"), settingQuery, zNodes);
                }
            });

        });
        var settingQuery = {
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
                onCheck: onCheckQuery
            }
        };

        function beforeClick(treeId, treeNode) {
            var zTree = $.fn.zTree.getZTreeObj("treeDemoQuery");
            zTree.checkNode(treeNode, !treeNode.checked, null, true);
            return false;
        }
        var xianshi = null;
        //选择卡口复选框时触发
        function onCheckQuery(e, treeId, treeNode) {
            try {
                var zTree = $.fn.zTree.getZTreeObj("treeDemoQuery");

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
                var cityObj = $("#kakouQuery");
                cityObj.attr("value", v);
                //alert(ids);
                $("#kakouIdQuery").val(ids);

            } catch (e) {

            }

        }

        //显示卡口下拉
        function showMenuQuery(event) {
            $("#selectKakouQuery").css("display", "none");
            var cityObj = $("#kakouQuery");
            var cityOffset = $("#kakouQuery").offset();
            if ($("#menuContentQuery").css("display") == "block") {
                $("#menuContentQuery").css("display", "none");
            }
            else {
                $("#menuContentQuery").css({ left: "0px", top: (cityOffset.top) + cityObj.outerHeight() + "px" }).slideDown("fast");

            }
            event.stopPropagation();
        }
        //隐藏卡口下拉
        function hideMenuQuery() {
            $("#menuContentQuery").css("display", "none");
        }
        function hideMenuSelectQuery() {
            $("#selectKakouQuery").css("display", "none");
        }
        //清除
        function clearMenuQuery() {
            $("#kakouQuery").val("");

            var zTree = $.fn.zTree.getZTreeObj("treeDemoQuery");
            zTree.checkAllNodes(false);
            AlarmportSetting.ClearKakou();
        }
        var zNodes = null;
        function showSelectQuery(event) {
            var code = event.keyCode;
            if (code == 13 || code == 32) {
                AlarmportSetting.GetKakou("1");
            }
        }
        function setUlQuery(lis) {
            $("#menuContentQuery").css("display", "none");
            var cityObj = $("#kakouQuery");
            var cityOffset = $("#kakouQuery").offset();

            $("#selectKakouQuery").css({ left: (cityOffset.left) + "px", top: (cityOffset.top) + cityObj.outerHeight() + "px" }).slideDown("fast");
            var json = eval(lis);
            var strs = "";
            if (json[0].name == "none") {
                strs += "<li style='margin-top:150px;margin-left:120px;'> 当前没查询到无数据 </li>";
            } else {
                for (var i = 0; i < json.length; i++) {
                    strs += "<li onclick='setInputQuery(this)' style='margin-left:10px;margin-top:5px;color:white;height:28px;line-height:28px;text-align:left;text-indent:8px;font-size:14px;overflow:hidden;' class='" + json[i].id + "'>" + json[i].name + " </li>";
                }
            }
            $("#showKakouQuery").html(strs);

        }
        //卡口模糊查询的时候，点击下面卡口给文本框赋值
        function setInputQuery(li) {
            $("#kakouQuery").val(""); $("#kakouIdQuery").val("");
            $("#kakouQuery").val(li.innerText);
            $("#kakouIdQuery").val(li.className);
            AlarmportSetting.SetSession("1");
            $.post("../Passcar/Getjson.ashx", "", function (data) {
                zNodes = eval(data);
                if (zNodes != null) {
                    $.fn.zTree.init($("#treeDemoQuery"), settingQuery, zNodes);
                }
            });
        }
        function returnKakouQuery() {
            $("#menuContentQuery").css("display", "block");
            $("#selectKakouQuery").css("display", "none");
        }
    </script>
    <!--查询流量报警配置卡口选择插件Js结束-->

    <!--新增流量报警配置卡口选择插件Js开始-->
    <script type="text/javascript">
        ////选中
        var setSelect = function (kakouNames) {
            Window1.hide();
            $.post("../Passcar/Getjson.ashx", "", function (data) {
                zNodes = eval(data);
                if (zNodes != null) {
                    $.fn.zTree.init($("#treeDemo"), setting, zNodes);
                }
            });
            $("#kakou").val(kakouNames);
        };
        //check: {
        //        enable: true,
        //        chkboxType: { "Y": "ps", "N": "ps" }
        //},
        var setting = {

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
                //onCheck: onCheck
                onClick: onClick
            }
        };

        function beforeClick(treeId, treeNode) {
            //var zTree = $.fn.zTree.getZTreeObj("treeDemo");
            //zTree.checkNode(treeNode, !treeNode.checked, null, true);
            //return false;
            var check = (treeNode && !treeNode.isParent);
            if (!check) alert("只能选择卡口...");
            return check;
        }
        var xianshi = null;
        //选择卡口复选框时触发
        function onClick(e, treeId, treeNode) {
            try {
                var zTree = $.fn.zTree.getZTreeObj(treeId);
                //nodes = zTree.getCheckedNodes(true);
                nodes = zTree.getSelectedNodes();
                var v = "";
                var ids = "";
                nodes.sort(function compare(a, b) { return a.id - b.id; });
                for (var i = 0, l = nodes.length; i < l; i++) {
                    //if (nodes[i].isParent == true) {//把部门去除
                    //    continue;
                    //}
                    v += nodes[i].name + ",";
                    ids += nodes[i].id + ",";
                }
                if (v.length > 0) v = v.substring(0, v.length - 1);
                if (ids.length > 0) ids = ids.substring(0, ids.length - 1);
                if (treeId == "treeDemo") {
                    var cityObj = $("#kakou");
                    cityObj.attr("value", v);
                    $("#kakouId").val(ids);
                    AlarmportSetting.StoreKakouDirectionDataBind(ids);
                }
                else {
                    var cityObj = $("#kakouQuery");
                    cityObj.attr("value", v);
                    //alert(ids);
                    $("#kakouIdQuery").val(ids);
                }
                $("#menuContent").css("display", "none");
                $("#selectKakou").css("display", "none");

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
                $("#menuContent").css({ right: "0px", top: (cityOffset.top) + cityObj.outerHeight() + "px" }).slideDown("fast");

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
            AlarmportSetting.ClearKakou();
        }
        var zNodes = null;

        document.onclick = function () {

            //$("#menuContent").hide();

        }
        function showSelect(event) {
            var code = event.keyCode;
            if (code == 13 || code == 32) {
                AlarmportSetting.GetKakou("0");
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
                strs += "<li style='margin-top:150px;margin-left:120px;'> 当前没查询到无数据 </li>";
            } else {
                for (var i = 0; i < json.length; i++) {
                    strs += "<li onclick='setInput(this)' style='margin-left:10px;margin-top:5px;color:white;height:28px;line-height:28px;text-align:left;text-indent:8px;font-size:14px;overflow:hidden;' class='" + json[i].id + "'>" + json[i].name + " </li>";
                }
            }
            $("#showKakou").html(strs);

        }
        //给页面赋值
        function setMainValue(kakou, kkid) {
            $("#kakou").val(kakou);
            $("#kakouId").val(kkid);
            $.post("../Passcar/Getjson.ashx", "", function (data) {
                zNodes = eval(data);
                if (zNodes != null) {
                    $.fn.zTree.init($("#treeDemo"), setting, zNodes);
                }
            });
        }
        //卡口模糊查询的时候，点击下面卡口给文本框赋值
        function setInput(li) {
            $("#kakou").val(""); $("#kakouId").val("");
            $("#kakou").val(li.innerText);
            $("#kakouId").val(li.className);
            AlarmportSetting.SetSession("0");
            $.post("../Passcar/Getjson.ashx", "", function (data) {
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
    <!--新增流量报警配置卡口选择插件Js结束-->
</head>
<%--<body onload="inputExcel()">--%>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="AlarmportSetting" />

        <ext:Hidden ID="Hidden1" runat="server" />
        <ext:Hidden ID="Hidden2" runat="server" />
        <ext:Hidden ID="realCount" runat="server" />
        <ext:Hidden ID="curpage" runat="server" />
        <ext:Hidden ID="allPage" runat="server" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Store ID="StoreAlarmportType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreDescribe" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreRecipient" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <%--上方--%>
                <ext:FormPanel ID="Panel1" Region="North" runat="server" Title="" Collapsible="false" Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server">
                            <Items>
                                <ext:Panel ID="Panel2" runat="server">
                                    <Content>
                                        <div style="width: 300px; height: 30px;">
                                            <span style="margin-top: 5px; font-size: 15px; margin-left: 1px; margin-right: 1px; float: left;">卡口名称：</span>
                                            <input id="kakouQuery" onkeyup="showSelectQuery(event)" runat="server" type="text" value="" onclick="showMenuQuery(event);" />
                                            <input onclick="showMenuQuery(event);" id="kakouXialaQuery" type="button"></input>
                                            <%--  <input runat="server" type="button" id="htmlBtn" />--%>
                                            <input id="kakouIdQuery" runat="server" hidden="hidden" />
                                        </div>
                                    </Content>
                                </ext:Panel>
                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("AlarmportSetting2","配置人：") %>' StyleSpec="margin-left:8px;">
                                </ext:Label>
                                <ext:TextField ID="TxtplateId1" runat="server" Width="150" EmptyText='<%# GetLangStr("Txt2","请输入配置人") %>'>
                                    <Listeners>
                                        <Change Fn="changeUpper" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("AlarmportSetting4","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" Timeout="60000">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("AlarmportSetting5","重置") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButRefresh" runat="server" Icon="DriveGo" Text='<%# GetLangStr("AlarmportSetting6","刷新") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButRefreshClick" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <%--中间--%>
                <ext:FormPanel ID="FormPanel2" Region="Center" runat="server" Layout="FitLayout"
                    AutoScroll="true">
                    <Items>
                        <ext:GridPanel ID="GridAlarmport" runat="server" StripeRows="true"
                            AutoScroll="true">
                            <TopBar>
                                <ext:Toolbar runat="server" Layout="Container">
                                    <Items>
                                        <ext:Toolbar runat="server">
                                            <Items>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10" />
                                                <ext:Button ID="ButFisrt" runat="server" Text="首页" Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutFisrt" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButLast" runat="server" Style="margin-left: 10px;" Icon="ControlRewindBlue" Text="上一页" Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutLast" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButNext" runat="server" Style="margin-left: 10px;" Icon="ControlFastforwardBlue" Text="下一页"
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
                                                <ext:Label ID="lblTitle" runat="server" Text="当前是第" StyleSpec=" margin-left:10px;" />
                                                <ext:Label ID="lblCurpage" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label2" runat="server" Text="页,共有" />
                                                <ext:Label ID="lblAllpage" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label9" runat="server" Text="页,共有" />
                                                <ext:Label ID="lblRealcount" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label12" runat="server" Text="条记录" />
                                                <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                                                <ext:Button ID="ButExcel" runat="server" Text='<%# GetLangStr("AlarmportSetting7","导出Excel") %>'
                                                    Icon="PageExcel">
                                                    <DirectEvents>
                                                        <Click OnEvent="ToExcel">
                                                            <%--  <EventMask ShowMask="true" Msg="正在导出" />--%>
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>

                            <Store>
                                <ext:Store ID="StoreAlarmport" runat="server" IgnoreExtraFields="false" OnRefreshData="MyData_Refresh">
                                    <AutoLoadParams>
                                        <ext:Parameter Name="start" Value="={0}" />
                                        <ext:Parameter Name="limit" Value="={15}" />
                                    </AutoLoadParams>
                                    <UpdateProxy>
                                        <ext:HttpWriteProxy Method="GET" Url="AlarmportSetting.aspx">
                                        </ext:HttpWriteProxy>
                                    </UpdateProxy>
                                    <Reader>
                                        <ext:JsonReader>
                                            <Fields>
                                                <ext:RecordField Name="col0" Type="String" />
                                                <ext:RecordField Name="col1" Type="String" />
                                                <ext:RecordField Name="col2" Type="String" />
                                                <ext:RecordField Name="col3" Type="String" />
                                                <ext:RecordField Name="col4" Type="String" />
                                                <ext:RecordField Name="col5" Type="String" />
                                                <ext:RecordField Name="col6" Type="String" />
                                                <ext:RecordField Name="col7" Type="Date" />
                                                <ext:RecordField Name="col8" Type="String" />
                                                <ext:RecordField Name="col9" Type="String" />
                                                <ext:RecordField Name="col10" Type="String" />
                                                <ext:RecordField Name="col11" Type="String" />
                                                <%--<ext:RecordField Name="col12" Type="String" />--%>
                                                <%--<ext:RecordField Name="col13" Type="String" />
                                                <ext:RecordField Name="col14" Type="String" />           --%>
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn></ext:RowNumbererColumn>
                                    <ext:Column Header='<%# GetLangStr("AlarmportSetting99","编号") %>' AutoDataBind="true" DataIndex="col0" Width="0" Hidden="true" />
                                    <ext:Column Header='<%# GetLangStr("AlarmportSetting60","卡口名称") %>' AutoDataBind="true" DataIndex="col8" Width="200" />
                                    <ext:Column Header='<%# GetLangStr("AlarmportSetting98","卡口方向") %>' AutoDataBind="true" DataIndex="col11" Width="80" />
                                    <ext:Column Header='<%# GetLangStr("AlarmportSetting61","开始时间") %>' AutoDataBind="true" DataIndex="col2" Width="60" />
                                    <ext:Column Header='<%# GetLangStr("AlarmportSetting62","结束时间") %>' AutoDataBind="true" DataIndex="col3" Width="60" />
                                    <ext:Column Header='<%# GetLangStr("AlarmportSetting63","统计周期") %>' AutoDataBind="true" DataIndex="col4" Width="80" />
                                    <ext:Column Header='<%# GetLangStr("AlarmportSetting64","报警阈值(%)") %>' AutoDataBind="true" DataIndex="col5" Width="80" />
                                    <ext:Column Header='<%# GetLangStr("AlarmportSetting65","配置人") %>' AutoDataBind="true" DataIndex="col9" Width="80" />
                                    <ext:DateColumn Header='<%# GetLangStr("AlarmportSetting66","配置时间") %>' AutoDataBind="true" DataIndex="col7" Width="120" Format="yyyy-MM-dd HH:mm:ss" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    <DirectEvents>
                                        <RowSelect OnEvent="SelectPort" Buffer="250">
                                            <ExtraParams>
                                                <ext:Parameter Name="sdata" Value="record.data" Mode="Raw" />
                                            </ExtraParams>
                                        </RowSelect>
                                    </DirectEvents>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <View>
                                <ext:GridView ID="GroupingView1" runat="server" ForceFit="true">
                                    <GetRowClass Fn="getRowClass" />
                                </ext:GridView>
                            </View>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>

                <%--右边--%>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Frame="true" Collapsible="false"
                    Width="320" Icon="Table" DefaultAnchor="100%">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:TabStrip ID="TabStrip1" runat="server">
                                    <Items>
                                        <ext:TabStripItem ActionItemID="pnlAmply" Title='<%# GetLangStr("AlarmportSetting8","车辆信息") %>' AutoDataBind="true" />
                                        <ext:TabStripItem ActionItemID="pnlImport" Title='<%# GetLangStr("AlarmportSetting9","批量录入") %>' AutoDataBind="true" Hidden="true" />
                                    </Items>
                                    <DirectEvents>
                                        <TabChange OnEvent="Unnamed_Event"></TabChange>
                                    </DirectEvents>
                                </ext:TabStrip>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:FormPanel ID="pnlAmply" runat="server" Header="false" Title='<%# GetLangStr("AlarmportSetting10","车辆信息") %>' DefaultAnchor="100%"
                            Padding="5">
                            <Items>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("AlarmportSetting11","编号") %>' ID="BH" Width="280" ReadOnly="true" Hidden="true" />
                                <ext:Panel runat="server">
                                    <Content>
                                        <div style="width: 487px; height: 30px;">
                                            <span style="margin-top: 5px; font-size: 15px; margin-left: 1px; margin-right: 29px; float: left;">卡口名称：</span>
                                            <input id="kakou" onkeyup="showSelect(event)" runat="server" type="text" style="width: 173px" value="" onclick="showMenu(event);" />
                                            <input onclick="showMenu(event);" id="kakouXiala" type="button"></input>
                                            <input id="kakouId" runat="server" hidden="hidden" />
                                        </div>
                                    </Content>
                                </ext:Panel>
                                <ext:ComboBox ID="CmbKakouDirection" runat="server" Editable="false" DisplayField="col1" FieldLabel='<%# GetLangStr("AlarmportSetting98","卡口方向") %>'
                                    ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("AlarmportSetting95","请选择卡口方向...") %>'
                                    SelectOnFocus="true" Width="120">
                                    <Store>
                                        <ext:Store ID="StoreKakouDirection" runat="server">
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
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:TimeField runat="server" FieldLabel='<%# GetLangStr("AlarmportSetting72","开始时间") %>' ID="KSSD" Width="280" Increment="1" Format="H:mm" />
                                <ext:TimeField runat="server" FieldLabel='<%# GetLangStr("AlarmportSetting73","结束时间") %>' ID="JSSD" Width="280" Increment="1" Format="H:mm" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("AlarmportSetting74","统计周期(s)") %>' ID="TJZQ" Width="280" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("AlarmportSetting75","报警阈值(%)") %>' ID="BJFZ" Width="280" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("AlarmportSetting76","配置人") %>' ID="KKPZR" Width="280" ReadOnly="true" Hidden="true" />
                                <ext:DateField runat="server" FieldLabel='<%# GetLangStr("AlarmportSetting77","配置时间") %>' ID="GXSJ" Width="280" ReadOnly="true" Format="yyyy-MM-dd HH:mm:ss" Hidden="true" />
                                <ext:Button ID="Button1" runat="server" Text='<%# GetLangStr("AlarmportSetting96","保存并增加时间段") %>' Icon="TableSave">
                                    <Listeners>
                                        <Click Handler="AlarmportSetting.UpdateAddData()" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                            <TopBar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:Button ID="ButAdd" runat="server" Text='<%# GetLangStr("AlarmportSetting16","增加") %>' Icon="Add" ToolTip='<%# GetLangStr("AlarmportSetting97","增加") %>'>
                                            <Listeners>
                                                <Click Handler="AlarmportSetting.InfoSave()" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="Button4" runat="server" Text='<%# GetLangStr("AlarmportSetting17","保存") %>' Icon="TableSave">
                                            <Listeners>
                                                <Click Handler="AlarmportSetting.UpdateData()" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="ButDelete" runat="server" Text='<%# GetLangStr("AlarmportSetting18","删除") %>' Icon="Delete">
                                            <Listeners>
                                                <Click Handler="AlarmportSetting.DoConfirm()" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:FormPanel>

                        <ext:FormPanel ID="pnlImport" runat="server" Padding="5" DefaultAnchor="100%" Width="320">
                            <Items>
                                <ext:FormPanel ID="bujupnlImport"
                                    runat="server" Header="false" Title='<%# GetLangStr("AlarmportSetting19","车辆信息") %>' DefaultAnchor="100%"
                                    Padding="-1" LabelAlign="left">
                                    <Items>

                                        <ext:FileUploadField ID="ExcelFile" runat="server" EmptyText='<%# GetLangStr("AlarmportSetting20","选择Excel文件") %>' FieldLabel="选择Excel文件" LabelStyle="margin-top:3px;"
                                            ButtonText="" Icon="TableAdd" Width="100" Height="30" />

                                        <%--       <ext:ProgressBar ID="Progress1" runat="server" Width="300" />--%>
                                    </Items>

                                    <Buttons>
                                        <ext:Button ID="ButDownload" runat="server" Text='<%# GetLangStr("AlarmportSetting21","模板下载") %>' Icon="DiskDownload">
                                            <Listeners>
                                                <Click Handler="AlarmportSetting.Download()" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="ButSave" runat="server" Text='<%# GetLangStr("AlarmportSetting22","保存") %>' Icon="TableSave">
                                            <DirectEvents>
                                                <Click OnEvent="StartLongAction">
                                                    <EventMask ShowMask="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Buttons>
                                </ext:FormPanel>
                                <%-- <ext:ProgressBar ID="Progress1" runat="server" Width="300" />--%>
                            </Items>
                        </ext:FormPanel>

                        <%--  去掉白色的进度条框--%>
                        <ext:ProgressBar ID="Progress1" runat="server" Width="300" />
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>

        <ext:TaskManager ID="TaskManager1" runat="server">
            <Tasks>
                <ext:Task TaskID="SaveExcelData" Interval="1000" AutoRun="false"
                    OnStart="#{ButSave}.setDisabled(true);"
                    OnStop="#{ButSave}.setDisabled(false);">
                    <DirectEvents>
                        <Update OnEvent="RefreshProgress" />
                    </DirectEvents>
                </ext:Task>
            </Tasks>
        </ext:TaskManager>
        <!-- 新增卡口配置显示卡口下拉框开始-->
        <div>
            <div id="menuContent" class="menuContent" style="display: none; position: absolute; z-index: 999; width: 389px; height: 377px;">
                <!--overflow-y: auto; overflow-x: hidden; -->
                <div style="position: relative; margin-top: 0px; width: 100%; height: 90%; overflow-y: auto; overflow-x: hidden;" class="kkselectStyle">
                    <ul id="treeDemo" class="ztree" style="margin-top: 0px; width: 100%; height: 90%;">
                    </ul>
                </div>
                <div style="position: relative; bottom: 0; height: 5%; padding-bottom: 10px; border-radius: 0px 0px 15px 15px;" class="kkselectStyle">
                    <input type="button" value="清除" class="func_btn" onclick="clearMenu()" style="margin-left: 100px;" />

                    <input type="button" value="关闭" class="func_btn" onclick="hideMenu()" />
                </div>
            </div>
            <div id="selectKakou" style="display: none; position: absolute; z-index: 999; width: 389px; height: 377px; overflow-y: auto; overflow-x: hidden;">
                <div style="position: relative; margin-top: 0px; width: 100%; height: 90%; overflow-y: auto; overflow-x: hidden;" class="kkselectStyle">
                    <ul id="showKakou" style="margin-top: 0px; width: 100%; height: 90%;">
                    </ul>
                </div>

                <div style="position: relative; bottom: 0; height: 5%; padding-bottom: 10px; border-radius: 0px 0px 15px 15px;" class="kkselectStyle">
                    <input type="button" class="func_btn" value="返回目录" onclick="returnKakou()" style="margin-left: 100px;" />

                    <input type="button" class="func_btn" value="关闭" onclick="hideMenuSelect()" />
                </div>
            </div>
        </div>
        <!-- 新增卡口配置显示卡口下拉框结束-->

        <!-- 查询卡口配置显示卡口下拉框开始-->
        <div>
            <div id="menuContentQuery" class="menuContent" style="display: none; position: absolute; z-index: 999; width: 389px; height: 377px;">
                <!--overflow-y: auto; overflow-x: hidden; -->
                <div style="position: relative; margin-top: 0px; width: 100%; height: 90%; overflow-y: auto; overflow-x: hidden;" class="kkselectStyle">
                    <ul id="treeDemoQuery" class="ztree" style="margin-top: 0px; width: 100%; height: 90%;">
                    </ul>
                </div>
                <div style="position: relative; bottom: 0; height: 5%; padding-bottom: 10px; border-radius: 0px 0px 15px 15px;" class="kkselectStyle">
                    <input type="button" value="清除" class="func_btn" onclick="clearMenuQuery()" style="margin-left: 100px;" />

                    <input type="button" value="关闭" class="func_btn" onclick="hideMenuQuery()" />
                </div>
            </div>
            <div id="selectKakouQuery" style="display: none; position: absolute; z-index: 999; width: 389px; height: 377px; overflow-y: auto; overflow-x: hidden;">
                <div style="position: relative; margin-top: 0px; width: 100%; height: 90%; overflow-y: auto; overflow-x: hidden;" class="kkselectStyle">
                    <ul id="showKakouQuery" style="margin-top: 0px; width: 100%; height: 90%;">
                    </ul>
                </div>

                <div style="position: relative; bottom: 0; height: 5%; padding-bottom: 10px; border-radius: 0px 0px 15px 15px;" class="kkselectStyle">
                    <input type="button" class="func_btn" value="返回目录" onclick="returnKakouQuery()" style="margin-left: 100px;" />

                    <input type="button" class="func_btn" value="关闭" onclick="hideMenuSelectQuery()" />
                </div>
            </div>
        </div>
        <!-- 查询卡口配置显示卡口下拉框结束-->
    </form>
</body>
</html>