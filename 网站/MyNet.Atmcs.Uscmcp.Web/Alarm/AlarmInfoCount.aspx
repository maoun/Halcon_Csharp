<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmInfoCount.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.AlarmInfoCount" %>

<%@ Register Assembly="netchartdir" Namespace="ChartDirector" TagPrefix="chart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%#GetLangStr("AlarmInfoCount25", "报警车辆统计") %></title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="Stylesheet" href="../Styles/datetime.css" type="text/css" />
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <%--<script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>--%>
    <script language="javascript" src="../Common/Print/LodopFuncs.js" type="text/javascript" charset="UTF-8"></script>
    <!--卡口选择插件引用开始-->
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js"></script>
    <link href="../KakouSelect/css/zTreeStyle/zTreeStyleAlarm.css" rel="stylesheet" />
    <script type="text/javascript" src="../KakouSelect/js/jquery.ztree.core.js"></script>
    <script type="text/javascript" src="../KakouSelect/js/jquery.ztree.excheck.js"></script>
    <!--卡口选择插件引用结束-->
    <script type="text/javascript">
        var getTasks = function (tree) {
            var msg = [], msgn = [],
           selNodes = tree.getChecked();
            msg.push("");
            msgn.push("")
            Ext.each(selNodes, function (node) {
                if (msg.length > 1) {
                    msg.push(",");
                    msgn.push(",");
                }
                msg.push(node.text);
                msgn.push(node.id);
            });
            msg.push("");
            msgn.push("");
            GridData.setValue(msgn.join(""));
            return msg.join("");
        };
        var getTasks = function (tree, node) {
            if (node.attributes.checked) {
                node.expand();
            }
            else {
                node.collapse();
            }
            var nodevalue = "";
            var nodetext = "";
            for (var i = 0; i < node.childNodes.length; i++) {

                node.childNodes[i].getUI().toggleCheck(node.attributes.checked);

            }
            selNodes = tree.getChecked();
            for (var i = 0; i < selNodes.length; i++) { //从节点中取出子节点依次遍历
                if (i != selNodes.length - 1) {
                    nd = selNodes[i];
                    if (nd.attributes.iconCls == "icon-arrownsew") {
                        nodevalue += nd.id + ",";
                        nodetext += nd.text + ",";
                    }
                }
                else {

                    nd = selNodes[i];
                    if (nd.attributes.iconCls == "icon-arrownsew") {
                        nodevalue += nd.id;
                        nodetext += nd.text;
                    }
                }
            }
            GridData.setValue(nodevalue);
            return nodetext;
        };
    </script>
    <script type="text/javascript">
        function ClearSelect() {

            TreePanel1.dropDownField.setValue("", false)
            GridData.setValue("");
            selNodes = TreePanel1.getChecked();
            Ext.each(selNodes, function (node) {
                TreePanel1.clearChecked();
            });
        }
    </script>
    <script type="text/javascript">
        var template = '<img src="../images/state/bfb.gif"  width="{0}" height="10"  />'
        var template2 = '<span>{0}</span>';

        var change = function (value) {
            return String.format(template, value * 2);
        };
        var pctChange = function (value) {
            return String.format(template2, value + "%");
        };
    </script>
    <script language="javascript" type="text/javascript">
        function Preview(title, ctime) {
            CreatePrintPage(title, ctime);
            LODOP.PREVIEW();
        };
        function PreviewChart(ctime) {
            CreatePrintChart(ctime);
            LODOP.PREVIEW();
        };
        function Setup() {
            CreatePrintPage();
            LODOP.PRINT_SETUP();
        };
        function DirPrint() {
            CreatePrintPage();
            LODOP.PRINT();
        };
        function Design() {
            CreatePrintPage();
            LODOP.PRINT_DESIGN();
        };
        function CreatePrintPage(title, ctime) {
            LODOP.PRINT_INIT('<%#GetLangStr("AlarmInfoCount27", "报表打印") %>');
            LODOP.SET_PRINT_TEXT_STYLE(1, "宋体", 10, 1, 0, 0, 1);
            LODOP.SET_PRINT_STYLE("FontSize", 16);
            LODOP.SET_PRINT_STYLE("Bold", 1);
            LODOP.ADD_PRINT_TEXT(20, 180, 400, 39, title);
            LODOP.SET_PRINT_STYLE("FontSize", 12);
            LODOP.ADD_PRINT_TEXT(50, 200, 400, 39, ctime);
            var strBodyStyle = "<style>table { border: 1 solid #f0f0f0 }</style>";
            var strFormHtml = strBodyStyle + "<body>" + document.getElementById("GridCount").outerHTML + "</body>";
            LODOP.ADD_PRINT_HTM(70, 20, 730, 1000, strFormHtml);

        };
        function CreatePrintChart(ctime) {
            LODOP.PRINT_INIT('<%#GetLangStr("AlarmInfoCount27", "报表打印") %>');
            LODOP.SET_PRINT_TEXT_STYLE(1, "宋体", 10, 1, 0, 0, 1);
            LODOP.SET_PRINT_STYLE("FontSize", 12);
            LODOP.SET_PRINT_STYLE("Bold", 1);
            LODOP.ADD_PRINT_TEXT(30, 200, 400, 39, ctime);
            LODOP.ADD_PRINT_HTM(70, 100, 730, 1000, document.getElementById("WebChartViewer1").outerHTML);
        };
    </script>

    <%--卡口列表js--%>
    <script type="text/javascript" language="javascript">
        function clearTime(start, end) {

            document.getElementById("start").innerText = start;
            document.getElementById("end").innerText = end;
            CmbCountType.triggers[0].hide();

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
            AlarmInfoCount.ClearKakou();
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
                AlarmInfoCount.GetKakou();
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
                strs += "<li style='margin-top:150px;margin-left:120px;'> <%# GetLangStr("AlarmInfoCount26","当前没查询到数据") %></li>";
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
        AlarmInfoCount.SetSession();
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
    //鼠标悬浮在行上有提示信息（展示的时候）
    var showTip = function () {
        var rowIndex = GridCount.view.findRowIndex(this.triggerElement),
            cellIndex = GridCount.view.findCellIndex(this.triggerElement),
            record = StoreCount.getAt(rowIndex),
            fieldName = GridCount.getColumnModel().getDataIndex(cellIndex),
            data = record.get(fieldName);
        this.body.dom.innerHTML = data;
    };
    </script>
    <!--卡口选择插件Js结束-->
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="AlarmInfoCount" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Viewport ID="Viewport2" runat="server" Layout="border">
            <Items>
                <%--上方--%>
                <ext:FormPanel ID="Panel1" Region="North" runat="server"
                    Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server" Layout="Container">
                            <Items>
                                <ext:Toolbar ID="Toolbar2" runat="server">
                                    <Items>
                                        <ext:Panel runat="server" BodyBorder="false">
                                            <Content>
                                                <%-- <div runat="server" id="selectDate" style="width: 475px">
                                                    <span class="laydate-span" style="margin-left: 0px; height: 24px;">&nbsp;&nbsp;<%# GetLangStr("AlarmInfoCount1","查询时间：") %></span>
                                                    <li runat="server" class="laydate-icon" id="start" style="width: 150px; margin-left: 16px; height: 22px;"></li>
                                                </div>
                                                <div>
                                                    <span class="laydate-span" style="margin-left: 20px; height: 24px;">--</span>
                                                    <li runat="server" class="laydate-icon" id="end" style="width: 150px; margin-left: 16px; height: 22px;"></li>
                                                </div>--%>
                                                <table style="width: 400px">
                                                    <tr>
                                                        <td style="width: 50px">
                                                            <span class="laydate-span" style="height: 30px; font-size: 15px; margin-left: 12px; margin-right: 2px; margin-top: 5px;"><%# GetLangStr("AlarmInfoCount1","查询时间：") %></span></td>
                                                        <td style="width: 150px">
                                                            <li class="laydate-icon" id="start" runat="server" style="width: 150px; height: 25px; margin-left: 5px;"></li>
                                                        </td>
                                                        <td style="width: 20px;"><span class="laydate-span" style="height: 30px; margin-left: 5px; margin-right: 5px">--</span>
                                                        </td>
                                                        <td style="width: 150px">
                                                            <li class="laydate-icon" id="end" runat="server" style="width: 150px; height: 25px;"></li>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Content>
                                        </ext:Panel>
                                        <ext:Label ID="lblStartTime" runat="server" Text='<%# GetLangStr("AlarmInfoCount2","统计类型：") %>' StyleSpec="margin-left:10px;">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbCountType" runat="server" Width="100">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("AlarmInfoCount3","清除选中") %>' />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                            </Listeners>
                                            <Items>
                                                <ext:ListItem Text='<%# GetLangStr("AlarmInfoCount4","报警类型") %>' AutoDataBind="true" Value="bjlxms" />
                                                <ext:ListItem Text='<%# GetLangStr("AlarmInfoCount5","处理状态") %>' AutoDataBind="true" Value="clbjms" />
                                                <ext:ListItem Text='<%# GetLangStr("AlarmInfoCount6","号牌种类") %>' AutoDataBind="true" Value="hpzlms" />
                                            </Items>
                                        </ext:ComboBox>
                                        <%--  <ext:Label ID="Label4" runat="server" Text='<%# GetLangStr("AlarmInfoCount6","卡口名称：") %>' StyleSpec="margin-left:10px;">
                                        </ext:Label>
                                        <ext:DropDownField ID="FieldStation" runat="server"
                                            Editable="false" Width="400px" TriggerIcon="SimpleArrowDown" Mode="ValueText">
                                            <Component>
                                                <ext:TreePanel runat="server" Height="400" Shadow="None" ID="TreeStation"
                                                    UseArrows="true" AutoScroll="true" Animate="true" EnableDD="true" ContainerScroll="true" RootVisible="true"
                                                    StyleSpec="background-color: rgba(68,138,202,0.9); border-radius: 20px;">
                                                    <Root>
                                                    </Root>
                                                    <Buttons>
                                                        <ext:Button runat="server" Text='<%# GetLangStr("AlarmInfoCount7","清除") %>'>
                                                            <Listeners>
                                                                <Click Handler="clearSelect(TreeStation,FieldStation);" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:Button runat="server" Text='<%# GetLangStr("AlarmInfoCount8","关闭") %>'>
                                                            <Listeners>
                                                                <Click Handler="#{FieldStation}.collapse();" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Buttons>
                                                    <Listeners>
                                                        <CheckChange Handler="this.dropDownField.setValue(getValues(this), getText(this), false);" />
                                                    </Listeners>
                                                    <SelectionModel>
                                                        <ext:MultiSelectionModel runat="server" />
                                                    </SelectionModel>
                                                </ext:TreePanel>
                                            </Component>
                                            <Listeners>
                                                <Expand Handler="this.component.getRootNode().expand(false);" Single="true" Delay="20" />
                                            </Listeners>
                                            <SyncValue Fn="syncValue" />
                                        </ext:DropDownField>--%>
                                        <ext:Panel runat="server">
                                            <Content>
                                                <div style="width: 480px; height: 30px;">
                                                    <span style="margin-top: 5px; font-size: 15px; margin-left: 12px; margin-right: 1px; float: left;"><%# GetLangStr("AlarmInfoCount7","卡口名称：") %></span>
                                                    <input id="kakou" onkeyup="showSelect(event)" runat="server" type="text" value="" onclick="showMenu(event);" />
                                                    <input onclick="showMenu(event);" id="kakouXiala" type="button"></input>
                                                    <%--  <input runat="server" type="button" id="htmlBtn" />--%>
                                                    <input id="kakouId" runat="server" hidden="hidden" />
                                                </div>
                                            </Content>
                                        </ext:Panel>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                        <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("AlarmInfoCount8","查询") %>'>
                                            <DirectEvents>
                                                <Click OnEvent="TbutQueryClick" Timeout="60000">
                                                    <EventMask ShowMask="true" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("AlarmInfoCount9","重置") %>'>
                                            <DirectEvents>
                                                <Click OnEvent="ButResetClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                        <ext:Button ID="ButPie" runat="server" Icon="ChartPie" ToolTip='<%# GetLangStr("AlarmInfoCount10","显示图表") %>' Hidden="true">
                                            <DirectEvents>
                                                <Click OnEvent="ButPie_Click">
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButPrint" runat="server" Icon="Printer" ToolTip='<%# GetLangStr("AlarmInfoCount11","打印统计报表") %>' Hidden="true">
                                            <DirectEvents>
                                                <Click OnEvent="ButPrint_Click">
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButPrintChart" runat="server" Icon="Report" ToolTip='<%# GetLangStr("AlarmInfoCount12","打印统计图表") %>' Hidden="true">
                                            <DirectEvents>
                                                <Click OnEvent="ButPrintChart_Click">
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <%--中间--%>
                <ext:Panel ID="Panel2" runat="server" Region="Center" DefaultBorder="false">
                    <Items>
                        <ext:BorderLayout ID="BorderLayout1" runat="server">
                            <Center>
                                <ext:Panel ID="Panel3" runat="server" DefaultBorder="false" Title='<%# GetLangStr("AlarmInfoCount13","列表统计") %>' AutoScroll="true">
                                    <Items>
                                        <ext:GridPanel ID="GridCount" runat="server" StripeRows="true" Header="false" Collapsible="true"
                                            AutoHeight="true">
                                            <Store>
                                                <ext:Store ID="StoreCount" runat="server" GroupField="col2">
                                                    <Reader>
                                                        <ext:JsonReader>
                                                            <Fields>
                                                                <ext:RecordField Name="col0" />
                                                                <ext:RecordField Name="col1" />
                                                                <ext:RecordField Name="col2" />
                                                                <ext:RecordField Name="col3" />
                                                                <ext:RecordField Name="col4" />
                                                                <ext:RecordField Name="col5" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                            <ColumnModel ID="ColumnModel1" runat="server">
                                                <Columns>
                                                    <ext:RowNumbererColumn Width="40" Align="Center"></ext:RowNumbererColumn>
                                                    <ext:Column Header='<%# GetLangStr("AlarmInfoCount14","行驶方向") %>' AutoDataBind="true" DataIndex="col2" Width="180" />
                                                    <ext:GroupingSummaryColumn DataIndex="col3" Align="Center" ColumnID="counttype" Header='<%# GetLangStr("AlarmInfoCount15","统计类型") %>' AutoDataBind="true" Width="130"
                                                        Hideable="false" SummaryType="Count">
                                                        <SummaryRenderer Handler="return ((value === 0 || value > 1) ? '(' + value +'项)' : '(1项)');" />
                                                    </ext:GroupingSummaryColumn>
                                                    <ext:GroupingSummaryColumn Width="80" Align="Center" Header='<%# GetLangStr("AlarmInfoCount16","统计数量") %>' AutoDataBind="true" DataIndex="col4" SummaryType="Sum">
                                                        <Renderer Handler="return value" />
                                                        <SummaryRenderer Handler="return '合计：' + value" />
                                                    </ext:GroupingSummaryColumn>
                                                    <ext:Column Header='<%# GetLangStr("AlarmInfoCount17","百分比") %>' Align="Center" AutoDataBind="true" DataIndex="col5" Width="80">
                                                        <Renderer Fn="pctChange" />
                                                    </ext:Column>
                                                    <ext:Column Header='<%# GetLangStr("AlarmInfoCount18","图例") %>' AutoDataBind="true" DataIndex="col5" Width="200">
                                                        <Renderer Fn="change" />
                                                    </ext:Column>
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                                    <DirectEvents>
                                                        <RowSelect OnEvent="ShowDetails" Buffer="250">
                                                            <ExtraParams>
                                                                <ext:Parameter Name="data" Value="record.data" Mode="Raw" />
                                                            </ExtraParams>
                                                        </RowSelect>
                                                    </DirectEvents>
                                                </ext:RowSelectionModel>
                                            </SelectionModel>
                                            <View>
                                                <ext:GroupingView ID="GroupingView1" runat="server" ForceFit="true" MarkDirty="false"
                                                    ShowGroupName="false" EnableNoGroups="true" HideGroupedColumn="true" GroupByText='<%# GetLangStr("AlarmInfoCount19","用该列进行分组") %>'
                                                    ShowGroupsText='<%# GetLangStr("AlarmInfoCount20","显示分组") %>' />
                                            </View>
                                            <Plugins>
                                                <ext:GroupingSummary ID="GroupingSummary1" runat="server">
                                                </ext:GroupingSummary>
                                            </Plugins>
                                            <ToolTips>
                                                <ext:ToolTip
                                                    ID="RowTip"
                                                    runat="server"
                                                    Target="={GridCount.getView().mainBody}"
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
                            </Center>
                        </ext:BorderLayout>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
        <ext:Window ID="winShow" runat="server" Width="600px" Height="400px" Hidden="true">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Header="false" AutoRender="true">
                    <Content>
                        <center>
                            <chart:WebChartViewer ID="WebChartViewer1" runat="server" />
                        </center>
                    </Content>
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
                    <input type="button" value='<%# GetLangStr("AlarmInfoCount21","清除") %>' class="func_btn" onclick="clearMenu()" style="margin-left: 100px;" />

                    <input type="button" value='<%# GetLangStr("AlarmInfoCount22","关闭") %>' class="func_btn" onclick="hideMenu()" />
                </div>
            </div>
            <div id="selectKakou" style="display: none; position: absolute; z-index: 999; width: 389px; height: 377px; overflow-y: auto; overflow-x: hidden;">
                <div style="position: relative; margin-top: 0px; width: 100%; height: 90%; overflow-y: auto; overflow-x: hidden;" class="kkselectStyle">
                    <ul id="showKakou" style="margin-top: 0px; width: 100%; height: 90%;">
                    </ul>
                </div>

                <div style="position: relative; bottom: 0; height: 5%; padding-bottom: 10px; border-radius: 0px 0px 15px 15px;" class="kkselectStyle">
                    <input type="button" class="func_btn" value='<%# GetLangStr("AlarmInfoCount23","返回目录") %>' onclick="returnKakou()" style="margin-left: 100px;" />

                    <input type="button" class="func_btn" value='<%# GetLangStr("AlarmInfoCount24","关闭") %>' onclick="hideMenuSelect()" />
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
            AlarmInfoCount.GetDateTime(true, tt);
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
            AlarmInfoCount.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>