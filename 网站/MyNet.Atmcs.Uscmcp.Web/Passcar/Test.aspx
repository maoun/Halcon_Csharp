<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Passcar.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <!--卡口选择插件引用开始-->
    <script src="../KakouSelect/js/jquery-1.4.4.min.js"></script>
    <link href="../KakouSelect/css/demo.css" rel="stylesheet" />
    <link href="../KakouSelect/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" />
    <script src="../KakouSelect/js/jquery.ztree.core.js"></script>
    <script src="../KakouSelect/js/jquery.ztree.excheck.js"></script>
    <!--卡口选择插件引用结束-->
    <!--卡口选择插件Js开始-->
    <script type="text/javascript">
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

            } catch (e) {

            }

        }
        <%-- <div id="menuContent" class="menuContent" style="display: none; position: absolute;">
                    <ul id="treeDemo" class="ztree" style="margin-top: 0px; width: 377px; height: 377px;">
                    </ul>
                    <div>

                        <input type="button" value="清除" onclick="clearMenu()" style="margin-right: 10px; margin-left: 200px;">
                        </input>
                        <input type="button" value="关闭" onclick="hideMenu()" style="margin-right: 0px;">
                        </input>
                    </div>
                </div>--%>
        //显示卡口下拉
        function showMenu() {
            var cityObj = $("#kakou");
            var cityOffset = $("#kakou").offset();
            //var divContent = "<div id='menuContent' class='menuContent' style='display: block; position: absolute;'>   <ul id='treeDemo' class='ztree' style='margin-top: 0px; width: 377px; height: 377px;'> </ul> <div> <input type='button' value='清除' onclick='clearMenu()' style='margin-right: 10px; margin-left: 200px;'> </input> <input type='button' value='关闭' onclick='hideMenu()' style='margin-right: 10px; margin-left: 200px;'> </input></input></div></div>";
            //divContent.appendTo($('body'));
            $("#menuContent").css({ left: cityOffset.left + "px", top: cityOffset.top + cityObj.outerHeight() + "px" }).slideDown("fast");

            //$("body").bind("mousedown", onBodyDown);
        }
        //隐藏卡口下拉
        function hideMenu() {
            $("#menuContent").css("display", "none");
            // $("body").unbind("mousedown", onBodyDown);
        }
        //function onBodyDown(event) {
        //    if (!(event.target.id == "menuBtn" || event.target.id == "citySel" || event.target.id == "menuContent" || $(event.target).parents("#menuContent").length > 0)) {
        //        hideMenu();
        //    }
        //}
        //清除
        function clearMenu() {
            $("#kakou").val("");

            var zTree = $.fn.zTree.getZTreeObj("treeDemo");
            zTree.checkAllNodes(false);
        }
        var zNodes = null;

        //var zNodes = [
        //	{ id: 1, pId: 0, name: "北京" },
        //	{ id: 2, pId: 0, name: "天津" },
        //	{ id: 3, pId: 0, name: "上海" },
        //	{ id: 6, pId: 0, name: "重庆" },
        //	{ id: 4, pId: 0, name: "河北省", open: true, check: true },
        //	{ id: 41, pId: 4, name: "石家庄" },
        //	{ id: 42, pId: 4, name: "保定" },
        //	{ id: 43, pId: 4, name: "邯郸" },
        //	{ id: 44, pId: 4, name: "承德" },
        //	{ id: 5, pId: 0, name: "广东省", open: true, check: true },
        //	{ id: 51, pId: 5, name: "广州" },
        //	{ id: 52, pId: 5, name: "深圳" },
        //	{ id: 53, pId: 5, name: "东莞" },
        //	{ id: 54, pId: 5, name: "佛山" },
        //	{ id: 6, pId: 0, name: "福建省", open: true, check: true },
        //	{ id: 61, pId: 6, name: "福州" },
        //	{ id: 62, pId: 6, name: "厦门" },
        //	{ id: 63, pId: 6, name: "泉州" },
        //	{ id: 64, pId: 6, name: "三明" }
        //];

        $(document).ready(function () {
            $.post("Getjson.ashx", "", function (data) {
                zNodes = eval(data);
                if (zNodes != null) {
                    $.fn.zTree.init($("#treeDemo"), setting, zNodes);
                }
            });
            $("#checkAllTrue").bind("click", { type: "checkAllTrue" }, checkNode);
            $("#checkAllFalse").bind("click", { type: "checkAllFalse" }, checkNode);
            $("#checkTruePS").bind("click", { type: "checkTruePS" }, checkNode);
            $("#checkFalsePS").bind("click", { type: "checkFalsePS" }, checkNode);
        });
        function checkNode(e) {
            var zTree = $.fn.zTree.getZTreeObj("treeDemo"),
			type = e.data.type,
			nodes = zTree.getSelectedNodes();
            if (type.indexOf("All") < 0 && nodes.length == 0) {
                alert("请先选择一个节点");
            }

            if (type == "checkAllTrue") {
                zTree.checkAllNodes(true);
            } else if (type == "checkAllFalse") {
                zTree.checkAllNodes(false);
            }
            else {
                var callbackFlag = $("#callbackTrigger").attr("checked");
                for (var i = 0, l = nodes.length; i < l; i++) {
                    if (type == "checkTrue") {
                        zTree.checkNode(nodes[i], true, false, callbackFlag);
                    } else if (type == "checkFalse") {
                        zTree.checkNode(nodes[i], false, false, callbackFlag);
                    } else if (type == "toggle") {
                        zTree.checkNode(nodes[i], null, false, callbackFlag);
                    } else if (type == "checkTruePS") {
                        zTree.checkNode(nodes[i], true, true, callbackFlag);
                    } else if (type == "checkFalsePS") {
                        zTree.checkNode(nodes[i], false, true, callbackFlag);
                    } else if (type == "togglePS") {
                        zTree.checkNode(nodes[i], null, true, callbackFlag);
                    }
                }
            }

        }

        function showDiv() {
            $("#showDiv").css("display", "block");
            $("#showDiv").css("position", "absolute");
            $("#showDiv").css("margin-top", "50px");
            $("#showDiv").css("margin-left", "100px");
        }
    </script>
    <!--卡口选择插件Js结束-->

    <!--卡口选择第二版开始-->
    <%--<link href="../tree_chajian/tree.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.8.0.js"></script>
    <script src="../tree_chajian/jquery-easyui-1.3.4/jquery.easyui.min.js"></script>
    <script src="../tree_chajian/tree.js"></script>
    <script src="../tree_chajian/jquery-easyui-1.3.4/datagrid-detailview.js"></script>
    <script src="../tree_chajian/jquery-easyui-1.3.4/datagrid-groupview.js"></script>
    <link href="../tree_chajian/jquery-easyui-1.3.4/themes/icon.css" rel="stylesheet" />
    <link href="../tree_chajian/jquery-easyui-1.3.4/themes/default/easyui.css" rel="stylesheet" />
    <script type="text/javascript">

        function checkTreeData(nodes) {
            var s = '';
            var kkid = '';
            for (var i = 0; i < nodes.length; i++) {
                var text = $("#FieldStation").val();
                if (typeof (nodes[i].attributes) != "undefined" && nodes[i].attributes.kkid != "") {
                    if (s != '') {
                        s += "," + nodes[i].text;
                    } else {
                        s = nodes[i].text;
                    }
                    if (kkid != '') {
                        kkid += "," + nodes[i].attributes.kkid;
                    } else {
                        kkid = nodes[i].attributes.kkid;
                    }
                }
            }
            $("#FieldStation").val(s);
            $("#FieldStation").attr("title", s);
            $("#kakou_station").val(kkid);
        }
        $(document).ready(function () {
            //$.post("GetJson.ashx", {  }, function (shuju) {
            //    var str=shuju
            //})
            //tree目录的加载
            $('#tt').tree({
                url: 'Getjson.ashx',
                method: 'post',
                cascadeCheck: true,
                onLoadSuccess: function (node, data) {
                    if (data.length > 0) {
                        var n = $('#tt').tree("getRoot");
                        var temp = $('#tt').tree("getChildren", n.children[0].target);
                        if (typeof (temp[0].children[0]) != "undefined") {
                            var temp1 = $('#tt').tree("getChildren", temp[0].children[0].target);
                            for (var i = 0; i < temp1.length; i++) {
                                if (i < 10) {
                                    var node = $('#tt').tree('find', parseInt(parseInt(temp1[0].id) + (i)));
                                    if (node != null) {
                                        $('#tt').tree('check', node.target);
                                    } else {
                                        continue;
                                    }
                                } else {
                                    break;
                                }
                            }
                        } else {
                            for (var i = 0; i < temp.length; i++) {
                                if (i < 10) {
                                    var node = $('#tt').tree('find', parseInt(parseInt(temp[0].id) + (i)));
                                    if (node != null) {
                                        $('#tt').tree('check', node.target);
                                    } else {
                                        continue;
                                    }
                                } else {
                                    break;
                                }
                            }
                        }

                    }
                    $(".select_btn").click();

                },
                onCheck: function (node, checked) {
                    var nodes = $('#tt').tree('getChecked');
                    /*if(nodes.length >10){
                        msg("超出规定的可选项最大值");
                        $("#tt").tree("uncheck",node.target);
                    }else{*/
                    checkTreeData(nodes);
                    //}
                }
            });

        });
        function zhanshi() {
            //var ds = document.getElementById("tree-select");
            //ds.style = "display:block";
            $("#tree-select").css("display", "block");

        }
    </script>--%>
    <!--卡口选择第二版结束-->
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Panel runat="server" StyleSpec="">
            <Content>

                <span style="font-size: 15px; margin-left: 13px; margin-right: 6px;">卡口列表：</span>
                <input id="kakou" type="text" value="" style="margin-top: 0px; width: 388px; height: 25px; background-color: #b5b8c8; border-bottom: 1px solid #B8C5E1; border-right: 1px solid #B8C5E1; border-top: 1px solid #B8C5E1; border-left: 1px solid #B8C5E1;" onclick="showMenu();" />
                <input type="button" id="checkAllTrue" value="勾选" onclick="return false;" />
                <input id="checkAllFalse" type="button" value="取消勾选" onclick="return false;" />
                <input type="button" id="checkTruePS" value="Ps勾选" onclick="return false;" />
                <input id="checkFalsePS" type="button" value="Ps取消勾选" onclick="    return false;" />
                <%-- <div id="menuContent" class="menuContent" style="display: none; position: absolute;">
                    <ul id="treeDemo" class="ztree" style="margin-top: 0px; width: 377px; height: 377px;">
                    </ul>
                    <div>

                        <input type="button" value="清除" onclick="clearMenu()" style="margin-right: 10px; margin-left: 200px;">
                        </input>
                        <input type="button" value="关闭" onclick="hideMenu()" style="margin-right: 0px;">
                        </input>
                    </div>
                </div>--%>

                <!--弹出层-->
                <input type="button" value="弹出层" onclick="showDiv()" />
                <div id="showDiv" style="width: 200px; height: 200px; display: none; background-color: red;">
                </div>
                <%--  <div class="selectName">
                    <input type="hidden" id="kakou_station" />
                    <span class="font">卡口名称:</span>
                    <span>
                        <input type="text" name="FieldStation" placeholder="请选择卡口名称" id="FieldStation" onclick="zhanshi()" /></span>
                </div>
                <div id="tree-select">
                    <div id="tree_nav" class="tree_nav">
                        <div id="tree_tt">
                            <ul id="tt"></ul>
                        </div>
                        <ul class="function" id="func_1">
                            <li><a id="clear_tree" class="func_btn">清除</a><a id="close_tree" class="func_btn">关闭</a></li>
                        </ul>
                    </div>
                    <div id="sousuo_nav" class="sousuo_nav">
                        <div id="sousuo_mohu"></div>
                        <ul class="function" id="func_2">
                            <li><a id="fanhui_tree" class="func_btn">返回目录</a><a id="close_sousuo" class="func_btn">关闭</a><>
                        </ul>
                    </div>
                </div>--%>
            </Content>
        </ext:Panel>

        <%--<ext:Panel runat="server" Height="400" Width="400" StyleSpec="background-color:red;">
        </ext:Panel>--%>
        <div id="menuContent" class="menuContent" style="display: none; position: absolute;">
            <ul id="treeDemo" class="ztree" style="margin-top: 0px; width: 377px; height: 377px;">
            </ul>
            <div>

                <input type="button" value="清除" onclick="clearMenu()" style="margin-right: 10px; margin-left: 200px;" />

                <input type="button" value="关闭" onclick="hideMenu()" style="margin-right: 0px;" />
            </div>
        </div>
    </form>
</body>
</html>