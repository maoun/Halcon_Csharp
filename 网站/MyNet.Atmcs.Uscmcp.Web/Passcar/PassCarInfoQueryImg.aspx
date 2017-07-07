<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PassCarInfoQueryImg.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.PassCarInfoQueryImg" %>

<%@ Register Src="../UIDepartment.ascx" TagName="UIDepartment" TagPrefix="dpart" %>
<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>过往车辆查询</title>
    <meta name="GENERATOR" content="MSHTML 8.00.7600.16853" />
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" href="../Css/chooser.css" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <link rel="stylesheet" href="../Css/showphotostyle.css" type="text/css" />
    <link href="../Styles/tabStyle.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../Map/css/custom.css" />
    <link rel="stylesheet" type="text/css" href="../Style/customMap.css" />
    <link rel="Stylesheet" href="../Styles/datetime.css" type="text/css" />
    <link href="../Styles/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../Styles/Clzpp/carPicker.css" />
    <link rel="Stylesheet" type="text/css" href="../Styles/hphm/autohphm.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/Clzpp/base.css" />
    <script type="text/javascript" src="../Scripts/Clzpp/carData.js"></script>
    <script type="text/javascript" src="../Scripts/Clzpp/carPicker.js"></script>
    <!--卡口选择插件引用开始-->
    <%--   <script type="text/javascript" src="../KakouSelect/js/jquery-1.4.4.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js"></script>
    <link href="../KakouSelect/css/demo.css" rel="stylesheet" />
    <link href="../KakouSelect/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" />
    <script type="text/javascript" src="../KakouSelect/js/jquery.ztree.core.js"></script>
    <script type="text/javascript" src="../KakouSelect/js/jquery.ztree.excheck.js"></script>
    <!--卡口选择插件引用结束-->
    <!--图片放大开始-->
    <script type="text/javascript" src="../Scripts/Zoom/jquery.photo.gallery.js"></script>
    <!--图片放大结束-->
    <style type="text/css">
        body {
            font-size: 14px;
        }

        div.item-wrap {
            float: left;
            border: 1px solid transparent;
            margin: 5px 25px 5px 25px;
            width: 160px;
            cursor: pointer;
            height: 160px;
            text-align: center;
        }

            div.item-wrap img {
                margin: 0px 0px 0px 5px;
                width: 130px;
                height: 110px;
            }

            div.item-wrap h6 {
                font-size: 16px;
                color: #3A4B5B;
                font-family: Microsoft YaHei,tahoma,arial,san-serif;
            }

        .items-view .x-view-over {
            border: solid 1px silver;
        }

        #items-ct {
            padding: 0px 30px 24px 30px;
        }

            #items-ct h2 {
                border-bottom: 2px solid #3A4B5B;
                cursor: pointer;
            }

                #items-ct h2 div {
                    background: transparent url(../images/group-expand-sprite.gif) no-repeat 3px -47px;
                    padding: 4px 4px 4px 17px;
                    font-family: tahoma,arial,san-serif;
                    font-size: 12px;
                    color: #3A4B5B;
                }

            #items-ct .collapsed h2 div {
                background-position: 3px 3px;
            }

            #items-ct dl {
                margin-left: 2px;
            }

            #items-ct .collapsed dl {
                display: none;
            }

        #CheckboxGroup1 .x-column {
            width: 70px !important;
        }

        #Panel9 {
            visibility: hidden;
            height: 0px;
        }

        #panelTop {
            background: url(../Images/top/top.png);
            position: fixed;
            width: 88px;
            height: 84px;
            right: 20px;
            top: 50%;
            margin-top: -17px;
            cursor: pointer;
            z-index: 9999;
            display: none;
        }

        #vehicleHead_Panel1 .x-btn {
            border-radius: 0px;
            border: none;
        }

        #vehicleHead_Panel1 button {
            height: 24px;
        }

        #vehicleHead_Panel1 #ext-gen210 {
            margin-top: -2px;
        }
    </style>
    <style type="text/css">
        .images-view .x-panel-body {
            font: 11px Arial, Helvetica, sans-serif;
        }

        .images-view .thumb {
            background: #dddddd;
            padding: 3px;
        }

            .images-view .thumb img {
                height: 150px;
                /*width: 200px;*/
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
            background: #FC5004;
            padding: 4px;
        }

        .images-view .x-view-selected {
            background: #FC5004;
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
            padding-left: 10px;
            margin: 5px;
        }

        .fis {
            display: inline-block;
            float: left;
            width: 33.33333%;
            height: 220px;
        }
    </style>
    <%-- <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>--%>
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="UTF-8"></script>
    <script language="JavaScript" src="../Scripts/showphoto.js" type="text/javascript" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.rotate.min.js" charset="UTF-8"></script>
    <script src="../Scripts/common.js" type="text/javascript" charset="UTF-8"></script>
    <script src="../Scripts/jquery-ui-1.10.4.min.js" type="text/javascript" charset="UTF-8"></script>
    <script src="../Scripts/jquery.mousewheel.min.js" type="text/javascript" charset="UTF-8"></script>
    <script src="../Scripts/jquery.mCustomScrollbar.js" type="text/javascript" charset="UTF-8"></script>
    <script type='text/javascript'>

        (function ($) {
            $(window).load(function () {
                $(".content").mCustomScrollbar();
            });
        })(jQuery);

        var change = function (obj, value) {
            obj.setValue(value.toUpperCase());
        };

        $(function () {
            $("body").delegate("#TxtplateId", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#CmbPlateType").click();
                }
            })

            $("body").delegate("#ext-gen229 .x-combo-list-item", "click", function (event) {
                $("#CmbClzpp").click();
            })
        })

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

        var selectionChanged = function (dv, nodes) {
            if (nodes.length > 0) {
                var id = nodes[0].id;
                var title = nodes[0].title;
                var text = nodes[0].innerText;
                var url = nodes[0].attributes["url"].value;
                url = url.replace("**", "&");
                PasscarAllQuery.ImgClick(id, url);
            }
        }
        var viewClick = function (dv, e) {
            var group = e.getTarget("h2", 3, true);
            if (group) {
                group.up("div").toggleClass("collapsed");
            }
        }
    </script>
    <script type="text/javascript" language="javascript">
        function clearTime(start, end) {
            document.getElementById("start").innerText = start;
            document.getElementById("end").innerText = end;
            CmbPlateType.triggers[0].hide();
            CmbCsys.triggers[0].hide();
            CmbXsfx.triggers[0].hide();
            document.getElementById("ClppChoice").innerText = "";
        }

        function directclear() {

            clearSelect(TreeStation, FieldStation);
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
        var getValues = function (tree, node, checked) {
            var nodeid = "";
            var nodeBumenid = "";
            var cnodes = node.childNodes;
            for (var i = 0; i < cnodes.length; i++) {
                cnodes[i].attributes.checked = checked;

            }
            selNodes = tree.getChecked();
            if (selNodes.length > 0) {
                for (var i = 0; i < selNodes.length; i++) { //从节点中取出子节点依次遍历

                    nd = selNodes[i];
                    if (nd.attributes.qtip == "Bumen") {
                        nodeBumenid += "Bumen" + nd.id + ",";
                    }
                    if (nd.attributes.qtip == "Kakou") {
                        nodeid += nd.id + ",";
                    }
                }
                nodeid = nodeBumenid + nodeid;
                nodeid = nodeid.substr(0, nodeid.length - 1);
            }

            return nodeid;

        };

        // 获得选中text
        var getText = function (tree, node, checked) {
            var nodetext = "";
            var cnodes = node.childNodes;
            for (var i = 0; i < cnodes.length; i++) {
                cnodes[i].attributes.checked = checked;

            }
            selNodes = tree.getChecked();
            if (selNodes.length > 0) {
                for (var i = 0; i < selNodes.length; i++) { //从节点中取出子节点依次遍历

                    nd = selNodes[i];
                    if (nd.attributes.qtip == "Bumen") {

                    }
                    if (nd.attributes.qtip == "Kakou") {
                        nodetext += nd.text + ",";
                    }
                }

                nodetext = nodetext.substr(0, nodetext.length - 1);
            }

            return nodetext;
        };
        var SetJGValue = function (tree, node, checked) {
            var idlist = getValues(tree, node, checked);
            var namelist = getText(tree, node, checked);
            FieldStation.setValue(idlist, namelist, false);
        };
        ////选中
        //var setSelect = function (nodeid, nodeName) {
        //    Window1.hide();
        //    var ids = nodeid;
        //    if (ids.length > 0) {
        //        try {
        //            //设置 取消勾选
        //            //TreeStation.setChecked({ ids: ids, silent: true });
        //            FieldStation.setValue(nodeid, nodeName);
        //            //TreeStation.getNodeById(nodeid).setChecked(true);
        //        } catch (e) {
        //        }
        //    }
        //    //tree.getRootNode().collapseChildNodes(true);
        //};
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
        var AllowInputNumberGsd = function () {
            txtGsd.setValue(txtGsd.getValue().replace(/[^\d\d]/g, "")); //只能输入数字
        }
        var AllowInputNumberDsd = function () {
            txtDsd.setValue(txtDsd.getValue().replace(/[^\d\d]/g, "")); //只能输入数字
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
    <script type="text/javascript">
        /* 卡口模糊查询*/
        var filterTree = function (el, e) {
            var tree = TreeStation,
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
            var field = TriggerFieldDept,
                tree = TreeStation;

            field.setValue("");
            tree.clearFilter();
            tree.getRootNode().collapseChildNodes(true);
            tree.getRootNode().ensureVisible();
            //tree.expandAll();
        };
    </script>
    <script type="text/javascript">
        var IMGDIR = '../images/sets';
        var attackevasive = '0';
        var gid = 0;
        var fid = parseInt('0');
        var tid = parseInt('0');
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
            PassCarInfoQueryImg.ClearKakou();
        }
        var zNodes = null;

        $(document).ready(function () {
            $.post("Getjson.ashx", "", function (data) {
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
                PassCarInfoQueryImg.GetKakou();
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
        //卡口模糊查询的时候，点击下面卡口给文本框赋值
        function setInput(li) {
            $("#kakou").val(""); $("#kakouId").val("");
            $("#kakou").val(li.innerText);
            $("#kakouId").val(li.className);
            PassCarInfoQueryImg.SetSession();
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
        function showWindow() {
            Window1.show();
        }
    </script>
    <!--卡口选择插件Js结束-->
</head>
<body>
    <form id="form1" runat="server">
        <div id="append_parent">
        </div>
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PassCarInfoQueryImg">

            <%--  <CustomDirectEvents>
                <ext:DirectEvent Target="htmlBtn" OnEvent="GetWindow">
                </ext:DirectEvent>
            </CustomDirectEvents>--%>
        </ext:ResourceManager>
        <ext:Store runat="server" ID="Store1">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="hphm" />
                        <ext:RecordField Name="hpzlms" />
                        <ext:RecordField Name="gwsj" />
                        <ext:RecordField Name="kkmc" />
                        <ext:RecordField Name="zjwj1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
            <AutoLoadParams>
                <ext:Parameter Name="start" Value="0" Mode="Raw" />
                <ext:Parameter Name="limit" Value="18" Mode="Raw" />
            </AutoLoadParams>
        </ext:Store>
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

        <ext:Store ID="StoreCsys" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" Type="String" />
                        <ext:RecordField Name="col1" Type="String" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreCjjg" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" Type="String" />
                        <ext:RecordField Name="col1" Type="String" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreXsfx" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" Type="String" />
                        <ext:RecordField Name="col1" Type="String" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreQuery" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Title" />
                        <ext:RecordField Name="Items" IsComplex="true" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="storekk" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Hidden ID="realCount" runat="server" />
        <ext:Hidden ID="curpage" runat="server" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="RowLayout" HideBorders="true">
            <Items>
                <ext:Panel runat="server" Layout="ColumnLayout" HideBorders="true" ID="panelQuery" RowHeight="1" ColumnWidth="1" Region="Center">
                    <Items>
                        <ext:Panel runat="server" ColumnWidth=".01" HideBorders="true" />
                        <ext:Panel runat="server" ColumnWidth=".98" HideBorders="true" Layout="RowLayout">
                            <Items>
                                <ext:Toolbar runat="server" Layout="ContainerLayout">
                                    <Items>
                                        <ext:Toolbar runat="server">
                                            <Items>
                                                <%--时间、号牌等条件--%>
                                                <ext:Panel runat="server" Height="40">
                                                    <Content>
                                                        <table style="width: 400px">
                                                            <tr>
                                                                <td style="width: 50px">
                                                                    <span class="laydate-span" style="height: 30px; font-size: 15px; margin-left: 12px; margin-right: 2px; margin-top: 5px;"><%# GetLangStr("PassCarInfoQueryImg42","查询时间：") %></span></td>
                                                                <td style="width: 150px">
                                                                    <li class="laydate-icon" id="start" runat="server" style="width: 150px; height: 25px; margin-left: 5px;"></li>
                                                                </td>
                                                                <td style="width: 20px;"><span class="laydate-span" style="height: 30px; margin-left: 16px; margin-right: 16px">--</span>
                                                                </td>
                                                                <td style="width: 150px">
                                                                    <li class="laydate-icon" id="end" runat="server" style="width: 150px; height: 25px;"></li>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </Content>
                                                </ext:Panel>
                                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("PassCarInfoQueryImg1","号牌种类：") %>' StyleSpec=" margin-top:10px; margin-left:45px; float: left; height: 30px; line-height: 30px!important; text-align: center;">
                                                </ext:Label>
                                                <ext:ComboBox Height="30" ID="CmbPlateType" runat="server" Editable="false" DisplayField="col1" StoreID="StorePlateType"
                                                    ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PassCarInfoQueryImg2","请选择...") %>'
                                                    SelectOnFocus="true" Width="150">
                                                    <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PassCarInfoQueryImg3","清除选中") %>' AutoDataBind="true" />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.triggers[0].show();" />
                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                    </Listeners>
                                                </ext:ComboBox>
                                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("PassCarInfoQueryImg4","号牌号码：") %>' StyleSpec="margin-top:10px;margin-left:40px;   float: left;  height: 30px; line-height: 30px!important; text-align: center">
                                                </ext:Label>
                                                <ext:Panel ID="Panel4" runat="server" Height="30" StyleSpec="float: left;  height: 30px; line-height: 30px!important; text-align: center">
                                                    <Content>
                                                        <veh:VehicleHead ID="vehicleHead" runat="server" />
                                                    </Content>
                                                </ext:Panel>
                                                <ext:Panel runat="server" Layout="ColumnLayout" Height="30">
                                                    <Items>
                                                        <ext:TextField ID="TxtplateId" runat="server" Hidden="false" MaxLength="6" Width="150" Height="30" EmptyText='<%# GetLangStr("PassCarInfoQueryImg5","六位号牌号码") %>'>
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
                                                <ext:Panel runat="server" StyleSpec="margin-left:0px;">
                                                    <Items>
                                                        <ext:Checkbox runat="server" ID="cktype" Checked="false" BoxLabel='<%# GetLangStr("PassCarInfoQueryImg6","模糊查询") %>' StyleSpec="margin-left:0px;">
                                                            <DirectEvents>
                                                                <Check OnEvent="changtype" />
                                                            </DirectEvents>
                                                        </ext:Checkbox>
                                                    </Items>
                                                </ext:Panel>
                                            </Items>
                                        </ext:Toolbar>
                                        <ext:Toolbar runat="server">
                                            <Items>
                                                <%--<ext:Panel runat="server" Layout="ColumnLayout" Height="40" HideBorders="true">
                                                    <Items>
                                                        <ext:Label ID="Label11" runat="server" Text='<%# GetLangStr("PasscarAllQuery5","卡口列表：") %>' StyleSpec="margin-top:5px;margin-left:13px;margin-right:11px; float: left; height: 30px; line-height: 30px!important; " />
                                                        <ext:DropDownField ID="FieldStation" runat="server"
                                                            Editable="false" TriggerIcon="SimpleArrowDown" Mode="ValueText" Width="390">
                                                            <Component>
                                                                <ext:TreePanel runat="server" Height="400" Shadow="None" ID="TreeStation"
                                                                    UseArrows="true" AutoScroll="true" Animate="true" EnableDD="true" ContainerScroll="true" RootVisible="true"
                                                                    StyleSpec="background-color: rgba(68,138,202,0.9); border-radius: 15px;">
                                                                    <Root>
                                                                    </Root>
                                                                    <Buttons>
                                                                        <ext:Button runat="server" Text='<%# GetLangStr("PasscarAllQuery6","清除") %>'>
                                                                            <Listeners>
                                                                                <Click Handler="clearSelect(TreeStation,FieldStation);" />
                                                                            </Listeners>
                                                                        </ext:Button>
                                                                        <ext:Button runat="server" Text='<%# GetLangStr("PasscarAllQuery7","关闭") %>'>
                                                                            <Listeners>
                                                                                <Click Handler="#{FieldStation}.collapse();" />
                                                                            </Listeners>
                                                                        </ext:Button>
                                                                    </Buttons>
                                                                    <Listeners>
                                                                        <CheckChange Handler="SetJGValue(this,node,checked);" />
                                                                    </Listeners>
                                                                    <SelectionModel>
                                                                        <ext:MultiSelectionModel runat="server" />
                                                                    </SelectionModel>
                                                                    <TopBar>
                                                                        <ext:Toolbar runat="server" Layout="FitLayout">
                                                                            <Items>
                                                                                <ext:TriggerField ID="TriggerFieldDept" runat="server" EnableKeyEvents="true" StyleSpec=" border-radius:15px;">
                                                                                    <Triggers>
                                                                                        <ext:FieldTrigger Icon="Clear" />
                                                                                    </Triggers>
                                                                                    <Listeners>
                                                                                        <KeyUp Fn="filterTree" Buffer="250" />
                                                                                        <TriggerClick Handler="clearFilter();" />
                                                                                    </Listeners>
                                                                                </ext:TriggerField>
                                                                            </Items>
                                                                        </ext:Toolbar>
                                                                    </TopBar>
                                                                </ext:TreePanel>
                                                            </Component>
                                                            <Listeners>
                                                                <Expand Handler="this.component.getRootNode().expand(false);" Single="true" Delay="20" />
                                                            </Listeners>
                                                            <SyncValue Fn="syncValue" />
                                                        </ext:DropDownField>

                                                        <ext:Panel runat="server">
                                                            <Content>

                                                                <input runat="server" type="button" id="htmlBtn" style="cursor: pointer; border-bottom: 1px solid #B8C5E1; border-right: 1px solid #B8C5E1; border-top: 1px solid #B8C5E1; border-left: 1px solid #B8C5E1; background: rgba(0, 0, 0, 0) url('../Images/Mapimg/baidumap.png') no-repeat scroll -13px -10px / 50px 48px; width: 24px; height: 28px" />
                                                            </Content>
                                                        </ext:Panel>
                                                    </Items>
                                                </ext:Panel>--%>
                                                <ext:Panel runat="server">
                                                    <Content>
                                                        <div style="width: 512px; height: 40px;">
                                                            <span style="margin-top: 5px; font-size: 15px; margin-left: 13px; margin-right: 10px; float: left;"><%# GetLangStr("PassCarInfoQueryImg43","卡口列表：") %></span>
                                                            <input id="kakou" onkeyup="showSelect(event)" runat="server" type="text" value="" onclick="showMenu(event);" />
                                                            <input onclick="showMenu(event);" id="kakouXiala" type="button"></input>
                                                            <input runat="server" type="button" id="htmlBtn" onclick="showWindow()" />
                                                            <input id="kakouId" runat="server" hidden="hidden" />
                                                        </div>
                                                    </Content>
                                                </ext:Panel>

                                                <ext:Label ID="Label5" runat="server" Text='<%# GetLangStr("PassCarInfoQueryImg7","车身颜色：") %>' StyleSpec="margin-top:5px;margin-left:23px;margin-right:0px;float: left;  height: 30px; line-height: 30px!important; text-align: center"></ext:Label>
                                                <ext:ComboBox ID="CmbCsys" runat="server" Editable="false" DisplayField="col1"
                                                    ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("PassCarInfoQueryImg8","选择车身颜色...") %>'
                                                    SelectOnFocus="true" Width="150" StoreID="StoreCsys">
                                                    <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PassCarInfoQueryImg3","清除选中") %>' AutoDataBind="true" />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.triggers[0].show();" />
                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                    </Listeners>
                                                </ext:ComboBox>
                                                <ext:Button ID="Button1" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("PassCarInfoQueryImg9","查询")%>' StyleSpec="margin-left:40px;">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutQueryClick" Timeout="60000">
                                                            <EventMask ShowMask="true" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="Button2" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("PassCarInfoQueryImg10","重置")%>'>
                                                    <DirectEvents>
                                                        <Click OnEvent="ButResetClick" />
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </Items>
                                </ext:Toolbar>
                                <%--高级查询--%>
                                <ext:Panel runat="server" HideBorders="true" Height="24">
                                    <Content>
                                        <li id="morelist"><a><span class="act" style="color: #fc5004; cursor: pointer; -moz-user-select: none; -webkit-user-select: none"><%# GetLangStr("PassCarInfoQueryImg11","高级查询") %></span></a><img class="act" alt="" id="transform" src="../Image/jiantou-down.png" style="margin-left: 10px; cursor: pointer"></img></li>
                                    </Content>
                                </ext:Panel>
                                <ext:Panel runat="server" HideBorders="true" Height="10"></ext:Panel>
                                <%--更多查询条件--%>
                                <ext:Panel ID="Panel9" runat="server" Layout="RowLayout" HideBorders="true" Height="40">
                                    <Items>
                                        <ext:Toolbar runat="server">
                                            <Items>
                                                <ext:Panel ID="Panel8" runat="server" HideBorders="true" Layout="ColumnLayout" Height="30">
                                                    <Items>
                                                        <ext:Panel runat="server">
                                                            <Content>
                                                                <input type="hidden" runat="server" id="clpp" value="" />
                                                                <input type="hidden" runat="server" id="clzpp" value="" />
                                                                <div style="margin-top: 5px;">
                                                                    <span style="margin-left: 15px; margin-right: 2px;"><%# GetLangStr("PassCarInfoQueryImg83","车辆品牌：") %></span>
                                                                </div>
                                                            </Content>
                                                        </ext:Panel>
                                                        <ext:Panel runat="server">

                                                            <Content>

                                                                <input type="text" runat="server" id="ClppChoice" style="width: 170px; height: 26px; " />
                                                            </Content>
                                                        </ext:Panel>
                                                    </Items>
                                                </ext:Panel>
                                                <ext:Label ID="Label9" runat="server" Text='<%# GetLangStr("PassCarInfoQueryImg12","行驶方向：") %>' StyleSpec="margin-top:10px;margin-left:10px;float: left;  height: 30px; line-height:30px!important; text-align: center">
                                                </ext:Label>
                                                <ext:ComboBox ID="CmbXsfx" runat="server" Editable="false" DisplayField="col1"
                                                    ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%#GetLangStr("PassCarInfoQueryImg13","选择行驶方向...") %>'
                                                    SelectOnFocus="true" StoreID="StoreXsfx" Width="132">
                                                    <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("PassCarInfoQueryImg3","清除选中") %>' AutoDataBind="true" />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.triggers[0].show();" />
                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                    </Listeners>
                                                </ext:ComboBox>

                                                <ext:Label ID="Label6" runat="server" Text='<%# GetLangStr("PassCarInfoQueryImg14","车速(km/h)：") %>' Style="margin-left: 35px;" />
                                                <ext:TextField ID="txtDsd" EnableKeyEvents="true" runat="server" Height="30" StyleSpec="margin-left:0px;" Width="69">
                                                    <Listeners>
                                                        <KeyUp Fn="AllowInputNumberDsd" />
                                                    </Listeners>
                                                </ext:TextField>
                                                <ext:Label ID="Label12" runat="server" Text="--" StyleSpec="text-align:center" />
                                                <ext:TextField ID="txtGsd" EnableKeyEvents="true" runat="server" Height="30" Width="69">

                                                    <Listeners>
                                                        <KeyUp Fn="AllowInputNumberGsd" />
                                                    </Listeners>
                                                </ext:TextField>

                                                <ext:Label ID="Label10" runat="server" Text='<%# GetLangStr("PassCarInfoQueryImg14","车道编号：") %>' StyleSpec="margin-top:13px;margin-left:42px;margin-right:2px;float: left;  height: 30px; line-height: 30px!important; text-align: center">
                                                </ext:Label>
                                                <ext:TextField ID="txtXscd" runat="server" Cls=" heigth:24" Width="150" />
                                            </Items>
                                        </ext:Toolbar>
                                    </Items>
                                </ext:Panel>
                                <%-- 查询--%>
                                <ext:Panel runat="server" RowHeight=".6" HideBorders="true" ColumnWidth="1" Layout="FitLayout">

                                    <Items>
                                        <%--中部--%>
                                        <ext:FormPanel ID="FormPanel2" Region="Center" runat="server" Title='<%# GetLangStr("PassCarInfoQueryImg15","查询结果") %>' Layout="FitLayout" AutoScroll="true">
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar2" runat="server" Layout="Container" Flat="false">
                                                    <Items>
                                                        <ext:Toolbar runat="server">
                                                            <Items>
                                                                <ext:Label ID="LabNum" runat="server" Text='<%# GetLangStr("PassCarInfoQueryImg16","当前0页,共0页") %>'>
                                                                </ext:Label>
                                                                <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10" />
                                                                <ext:Button ID="ButLast" runat="server" Icon="ControlRewindBlue" Text='<%# GetLangStr("PassCarInfoQueryImg17","上一页") %>' Disabled="true">
                                                                    <DirectEvents>
                                                                        <Click OnEvent="TbutLast" />
                                                                    </DirectEvents>
                                                                </ext:Button>
                                                                <ext:Button ID="ButNext" runat="server" Icon="ControlFastforwardBlue" Text='<%# GetLangStr("PassCarInfoQueryImg18","下一页") %>'
                                                                    Disabled="true">
                                                                    <DirectEvents>
                                                                        <Click OnEvent="TbutNext" />
                                                                    </DirectEvents>
                                                                </ext:Button>
                                                                <ext:ToolbarFill runat="server">
                                                                </ext:ToolbarFill>
                                                            </Items>
                                                        </ext:Toolbar>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <Items>

                                                <ext:Panel
                                                    runat="server"
                                                    ID="ImagePanel"
                                                    Cls="images-view"
                                                    Frame="true"
                                                    AutoHeight="true"
                                                    AutoWidth="true"
                                                    Layout="Fit">
                                                    <%-- <TopBar>
                                                        <ext:PagingToolbar runat="server" StoreID="Store1" PageSize="8" HideRefresh="true" />
                                                    </TopBar>--%>
                                                    <Items>
                                                        <ext:DataView
                                                            runat="server" StoreID="Store1" AutoHeight="true" MultiSelect="true"
                                                            OverClass="x-view-over" ItemSelector="div.thumb-wrap" EmptyText="No images to display">
                                                            <Template runat="server">
                                                                <Html>
                                                                    <tpl for=".">
								<div class="thumb-wrap" id="{name}">
									<div class="thumb"><img src="{zjwj1}" title="{hphm}  "  onclick="$.openPhotoGalleryImg(this);"  class="photo" /></div>
                                    <div>
														<H6>通过卡口：{kkmc}</H6>
													</div>
                                    <div>
														<H6>过往时间：{gwsj}</H6>
													</div>
                                    <div>
														<H6>号牌：{hpzlms}-{hphm}</H6>
													</div>
								</div>
							</tpl>
                                                                    <div class="x-clear"></div>
                                                                </Html>
                                                            </Template>

                                                            <%--  <Listeners>
                                                                <SelectionChange Fn="selectionChanged" />
                                                            </Listeners>--%>
                                                        </ext:DataView>
                                                    </Items>
                                                </ext:Panel>
                                            </Items>
                                        </ext:FormPanel>
                                    </Items>
                                </ext:Panel>
                            </Items>
                        </ext:Panel>
                        <ext:Panel runat="server" ColumnWidth=".01" HideBorders="true" />
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
        <ext:Window runat="server" ID="Window1" Title='<%# GetLangStr("PassCarInfoQueryImg19","卡口选择") %>' Hidden="true" Maximizable="true"
            Modal="true" Width="1000" Height="600" ButtonAlign="Center">
            <AutoLoad
                Url="../Map/MapStation.aspx"
                Mode="IFrame"
                ShowMask="true"
                MaskMsg='<%# GetLangStr("PassCarInfoQueryImg20","加载中...") %>' />
            <%-- <Buttons>
                <ext:Button runat="server" Text="保存">
                    <Listeners>
                        <Click Handler="OnEvl.hidemap();" />
                    </Listeners>
                </ext:Button>
            </Buttons>--%>
        </ext:Window>

        <!-- 显示卡口下拉框开始-->
        <div>
            <div id="menuContent" class="menuContent" style="display: none; position: absolute; z-index: 999; width: 389px; height: 377px;">
                <!--overflow-y: auto; overflow-x: hidden; -->
                <div style="position: relative; margin-top: 0px; width: 100%; height: 90%; overflow-y: auto; overflow-x: hidden;" class="kkselectStyle">
                    <ul id="treeDemo" class="ztree" style="margin-top: 0px; width: 100%; height: 90%;">
                    </ul>
                </div>
                <div style="position: relative; bottom: 0; height: 5%; padding-bottom: 10px;border-radius: 0px 0px 15px 15px;" class="kkselectStyle">
                    <input type="button" value='<%# GetLangStr("PassCarInfoQueryImg21","清除") %>' class="func_btn" onclick="clearMenu()" style="margin-left: 100px;" />

                    <input type="button" value='<%# GetLangStr("PassCarInfoQueryImg22","关闭") %>' class="func_btn" onclick="hideMenu()" />
                </div>
            </div>
            <div id="selectKakou" style="display: none; position: absolute; z-index: 999; width: 389px; height: 377px; overflow-y: auto; overflow-x: hidden;">
                <div style="position: relative; margin-top: 0px; width: 100%; height: 90%; overflow-y: auto; overflow-x: hidden; " class="kkselectStyle">
                    <ul id="showKakou" style="margin-top: 0px; width: 100%; height: 90%;">
                    </ul>
                </div>

                <div style="position: relative; bottom: 0; height: 5%; padding-bottom: 10px; border-radius: 0px 0px 15px 15px;" class="kkselectStyle">
                    <input type="button" class="func_btn" value='<%# GetLangStr("PassCarInfoQueryImg23","返回目录") %>' onclick="returnKakou()" style="margin-left: 100px;" />

                    <input type="button" class="func_btn" value='<%# GetLangStr("PassCarInfoQueryImg22","关闭") %>' onclick="hideMenuSelect()" />
                </div>
            </div>
        </div>
        <!-- 显示卡口下拉框结束-->
    </form>
</body>
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
            //  $(this).removeClass("import").next().remove();
        }

    })
</script>
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

            PassCarInfoQueryImg.GetDateTime(true, tt);

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
            PassCarInfoQueryImg.GetDateTime(false, tt);
        }

    };
    laydate(start);
    laydate(end);
</script>
<script type="text/javascript">

    $("#morelist .act").click(function () {

        if ($("#Panel9").css("visibility") == "hidden") {
            $("#transform").rotate({ animateTo: 180 });
            $("#Panel9").css("visibility", "visible");
            $("#Panel9").css("height", "40px");
            $("#ctl68").css("margin-top", "0px")
        }
        else {
            $("#Panel9").css("visibility", "hidden");
            $("#Panel9").css("height", "0px");
            $("#transform").rotate({ animateTo: 0 });
            $("#ctl68").css("margin-top", "0px")
        }
    })
    //车辆品牌和子品牌联动
    $("body").delegate("#ext-gen204 div", "click", function () {
        $("#CmbClzpp").click();
    })
</script>
<script type="text/javascript">
    //点击图标回到顶部.
    $("#panelTop").click(function () {

        $("#panelQuery").animate({ marginTop: 0 });
        $("#panelTop").css("display", "none");
    })
</script>
<script type="text/javascript">
    //综合查询图标移入移出加阴影.
    $("body").delegate("#Dashboard .item-wrap", "hover", function () {

        $(this).addClass("active").css("background", "rgba(144,144,144, 0.3)");

    })
    $("body").delegate("#Dashboard .item-wrap", "mouseleave", function () {

        $("#Dashboard .active").removeClass("active").css("background", "rgba(144,144,144, 0)");
    })
</script>
<script type="text/javascript">
    //移到回到顶部图片，变图片.
    $("#panelTop").mousemove(function () {
        $(this).css("background", "url(../Images/top/top_1.png)")
    })
    $("#panelTop").mouseleave(function () {
        $(this).css("background", "url(../Images/top/top.png)")
    })
</script>
<script type="text/javascript">
    //选择车辆子品牌
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