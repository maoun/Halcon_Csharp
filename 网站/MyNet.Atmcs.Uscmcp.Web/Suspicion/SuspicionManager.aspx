<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuspicionManager.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.SuspicionManager" %>

<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<%@ Register Src="../UIDepartment.ascx" TagName="UIDepartment" TagPrefix="dpart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%# GetLangStr("SuspicionManager65","车辆布控管理") %></title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <link rel="Stylesheet" type="text/css" href="../Styles/hphm/autohphm.css" />
    <style type="text/css">
        #FieldPerson {
            width: 148px;
        }
    </style>
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/UIContorl.js" charset="utf-8"></script>
    <script type="text/javascript">
        var template = '<span style="color:{0};">{1}</span>';
        var change = function (value) {
            return String.format(template, (value == '<%# GetLangStr("SuspicionManager66","已布控") %>') ? "green" : "red", value);
        };
        var changetime = function (value) {
            var mydate = Ext.util.Format.date(new Date(), 'Y-m-d H:i:s');
            return String.format(template, (value > mydate) ? "green" : "red", value);
        };
        var saveData = function () {
            GridData.setValue(Ext.encode(GridSuspicion.getRowsValues(false)));
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

    <script type="text/javascript">
        //清除号牌简称
        function clearHpjc() {
            document.getElementById("WindowEditor1_Field2").innerText = "";
            CmbPlateType.triggers[0].hide();
            CmbQueryMdlx.triggers[0].hide();

        }
        //清理选中
        function clearSelect() {
            var ids = FieldPerson.getValue();
            if (ids.length > 0) {
                try {
                    TreePerson.setChecked({ ids: ids, silent: false });
                } catch (e) {
                }
            }
        }
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

        function inputExcel() {
            try {
                var excelText = document.getElementById("ExcelFile");
                excelText.setAttribute("size", "16");
            } catch (e) {

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
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridSuspicion.view.findRowIndex(this.triggerElement),
                cellIndex = GridSuspicion.view.findCellIndex(this.triggerElement),
                record = StoreSuspicion.getAt(rowIndex),
                fieldName = GridSuspicion.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);
            if (fieldName == "col8" || fieldName == "col17") {

                data = data.toString().substring(0, 10) + " " + data.toString().substring(11, 19);
            }
            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body onload="inputExcel()">
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="SuspicionManager" />

        <ext:Hidden ID="Hidden1" runat="server" />
        <ext:Hidden ID="Hidden2" runat="server" />
        <ext:Hidden ID="realCount" runat="server" />
        <ext:Hidden ID="curpage" runat="server" />
        <ext:Hidden ID="allPage" runat="server" />

        <ext:Hidden ID="GridData" runat="server" />
        <ext:Store ID="StorePlateType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreColor" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreMdlx" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreBdbj" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
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
                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("SuspicionManager1","号牌号码：") %>' StyleSpec="margin-left:8px;">
                                </ext:Label>
                                <ext:Panel ID="Panel4" runat="server" Height="29">
                                    <Content>
                                        <veh:VehicleHead ID="WindowEditor1" runat="server" />
                                    </Content>
                                </ext:Panel>
                                <ext:TextField ID="TxtplateId" runat="server" Width="90" EmptyText='<%# GetLangStr("SuspicionManager2","六位号牌号码：") %>' MaxLength="6">
                                    <Listeners>
                                        <Change Fn="changeUpper" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("SuspicionManager3","号牌种类：") %>' StyleSpec="margin-left:10px;">
                                </ext:Label>
                                <ext:ComboBox ID="CmbPlateType" runat="server" Editable="false" StoreID="StorePlateType"
                                    DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                    EmptyText='<%# GetLangStr("SuspicionManager4","请选择...") %>' SelectOnFocus="true" Width="123">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("SuspicionManager5","清除选中:") %>' />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Label ID="Label5" runat="server" Text='<%# GetLangStr("SuspicionManager6","布控类型：") %>' StyleSpec="margin-left:10px;">
                                </ext:Label>
                                <ext:ComboBox ID="CmbQueryMdlx" runat="server" Editable="false" StoreID="StoreMdlx"
                                    DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                    EmptyText='<%# GetLangStr("SuspicionManager7","选择布控类型...") %>' SelectOnFocus="true" Width="123">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("SuspicionManager5","清除选中:") %>' />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("SuspicionManager8","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" Timeout="60000">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("SuspicionManager9","重置") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButRefresh" runat="server" Icon="DriveGo" Text='<%# GetLangStr("SuspicionManager10","刷新") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButRefreshClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButDispatched" runat="server" Icon="DriveGo" Text='<%# GetLangStr("SuspicionManager11","一键布控") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButDispatchedClick">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <%--中间--%>
                <ext:FormPanel ID="FormPanel2" Region="Center" runat="server" Layout="FitLayout">
                    <Items>
                        <ext:GridPanel ID="GridSuspicion" runat="server" StripeRows="true"
                            AutoScroll="true">
                            <TopBar>
                                <ext:Toolbar runat="server" Layout="Container">
                                    <Items>
                                        <ext:Toolbar runat="server">
                                            <Items>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10" />
                                                <ext:Button ID="ButFisrt" runat="server" Text='<%# GetLangStr("SuspicionManager12","首页") %>' Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutFisrt" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButLast" runat="server" Style="margin-left: 10px;" Icon="ControlRewindBlue" Text='<%# GetLangStr("SuspicionManager13","上一页") %>' Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutLast" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButNext" runat="server" Style="margin-left: 10px;" Icon="ControlFastforwardBlue" Text='<%# GetLangStr("SuspicionManager14","下一页") %>'
                                                    Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutNext" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButEnd" runat="server" Style="margin-left: 10px;" Text='<%# GetLangStr("SuspicionManager15","尾页") %>' Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutEnd" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Label ID="lblTitle" runat="server" Text='<%# GetLangStr("SuspicionManager16","查询结果：当前是第") %>' StyleSpec=" margin-left:10px;" />
                                                <ext:Label ID="lblCurpage" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label2" runat="server" Text='<%# GetLangStr("SuspicionManager17","页,共有") %>' />
                                                <ext:Label ID="lblAllpage" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label9" runat="server" Text='<%# GetLangStr("SuspicionManager17","页,共有") %>' />
                                                <ext:Label ID="lblRealcount" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label12" runat="server" Text='<%# GetLangStr("SuspicionManager64","条记录") %>' />
                                                <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                                                <ext:Button ID="ButExcel" runat="server" Text='<%# GetLangStr("SuspicionManager18","导出Excel") %>'
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
                                <ext:Store ID="StoreSuspicion" runat="server" IgnoreExtraFields="false" OnRefreshData="MyData_Refresh">
                                    <AutoLoadParams>
                                        <ext:Parameter Name="start" Value="={0}" />
                                        <ext:Parameter Name="limit" Value="={15}" />
                                    </AutoLoadParams>
                                    <UpdateProxy>
                                        <ext:HttpWriteProxy Method="GET" Url="SuspicionManager.aspx">
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
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40" Align="Center"></ext:RowNumbererColumn>
                                    <ext:Column Header='<%# GetLangStr("SuspicionManager19","布控编号") %>' AutoDataBind="true" DataIndex="col0" Width="0" Hidden="true" />
                                    <ext:Column Header='<%# GetLangStr("SuspicionManager20","号牌号码") %>' AutoDataBind="true" DataIndex="col1" Width="80" />
                                    <ext:Column Header='<%# GetLangStr("SuspicionManager21","号牌种类") %>' AutoDataBind="true" DataIndex="col3" Width="110" />
                                    <ext:Column Header='<%# GetLangStr("SuspicionManager22","车身颜色") %>' AutoDataBind="true" DataIndex="col4" Width="80" />
                                    <ext:Column Header='<%# GetLangStr("SuspicionManager23","车辆品牌") %>' AutoDataBind="true" DataIndex="col5" Width="80" />
                                    <ext:Column Header='<%# GetLangStr("SuspicionManager24","布控类型") %>' AutoDataBind="true" DataIndex="col7" Width="80" />
                                    <ext:Column Header='<%# GetLangStr("SuspicionManager25","布控标识") %>' AutoDataBind="true" DataIndex="col13" Width="70">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("SuspicionManager26","有效时间") %>' AutoDataBind="true" DataIndex="col8" Width="70">
                                        <Renderer Fn="changetime" />
                                    </ext:Column>
                                    <ext:Column Header='<%# GetLangStr("SuspicionManager27","布控联系人") %>' AutoDataBind="true" DataIndex="col14" Width="70" />
                                    <ext:Column Header='<%# GetLangStr("SuspicionManager28","联系电话") %>' AutoDataBind="true" DataIndex="col15" Width="70" />
                                    <ext:Column Header='<%# GetLangStr("SuspicionManager29","模糊布控") %>' AutoDataBind="true" DataIndex="col20" Width="0" Hidden="true">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:DateColumn Header='<%# GetLangStr("SuspicionManager30","更新时间") %>' AutoDataBind="true" DataIndex="col17" Width="120" Format="yyyy-MM-dd HH:mm:ss" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    <DirectEvents>
                                        <RowSelect OnEvent="SelectSuspicion" Buffer="250">
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
                            <ToolTips>
                                <ext:ToolTip
                                    ID="RowTip"
                                    runat="server"
                                    Target="={GridSuspicion.getView().mainBody}"
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

                <%--右边--%>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Frame="true" Collapsible="false"
                    Width="320" Icon="Table" DefaultAnchor="100%">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:TabStrip ID="TabStrip1" runat="server">
                                    <Items>
                                        <ext:TabStripItem ActionItemID="pnlAmply" Title='<%# GetLangStr("SuspicionManager31","布控车辆信息") %>' AutoDataBind="true" />
                                        <ext:TabStripItem ActionItemID="pnlImport" Title='<%# GetLangStr("SuspicionManager32","批量录入") %>' AutoDataBind="true" Hidden="true" />
                                    </Items>
                                    <DirectEvents>
                                        <TabChange OnEvent="Unnamed_Event"></TabChange>
                                    </DirectEvents>
                                </ext:TabStrip>
                                <ext:Checkbox runat="server" ID="ChkLike" Checked="false" Enabled="false" BoxLabel='<%# GetLangStr("SuspicionManager33","模糊布控") %>' StyleSpec="margin-left:10px;">
                                    <DirectEvents>
                                        <Check OnEvent="changtype" />
                                    </DirectEvents>
                                </ext:Checkbox>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:FormPanel ID="pnlAmply" runat="server" Header="false" Title='<%# GetLangStr("SuspicionManager34","布控车辆信息") %>' DefaultAnchor="100%" Padding="5">
                            <Items>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("SuspicionManager35","比对编号") %>' ID="txtID" Width="280" ReadOnly="true" Hidden="true" />
                                <ext:Panel ID="CompositeField2" runat="server" Width="280" Layout="ColumnLayout" Style="margin-bottom: 5px">
                                    <Items>
                                        <ext:Label runat="server" Text='<%# GetLangStr("SuspicionManager36","号牌号码：") %>' StyleSpec="margin-left:10px;" Style="margin-top: 5px; margin-right: 15px;" />
                                        <ext:Panel ID="Panel3" runat="server">
                                            <Content>
                                                <veh:VehicleHead ID="VehicleHead" runat="server" />
                                            </Content>
                                        </ext:Panel>
                                        <ext:TextField ID="txtHphm" runat="server" Width="152" AllowBlank="false" StyleSpec="margin-left: 2px">
                                            <Listeners>
                                                <Change Fn="changeUpper" />
                                            </Listeners>
                                        </ext:TextField>
                                        <ext:Panel runat="server" ID="pnhphm" Hidden="true" Width="140" Height="20" Layout="ColumnLayout">
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
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("SuspicionManager37","号牌种类") %>' ID="cmbHpzl" StoreID="StorePlateType"
                                    Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("SuspicionManager38","选择号牌种类") %>' Width="280" AllowBlank="false">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("SuspicionManager5","清除选中") %>' />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("SuspicionManager39","车身颜色") %>' ID="cmbCsys" StoreID="StoreColor"
                                    Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("SuspicionManager40","选择车身颜色") %>' Width="280">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("SuspicionManager5","清除选中") %>' />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("SuspicionManager41","车辆品牌") %>' ID="txtClpp" Width="280" />
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("SuspicionManager42","布控类型") %>' ID="cmbMdlx" StoreID="StoreMdlx" Editable="false"
                                    DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All" EmptyText='<%# GetLangStr("SuspicionManager43","选择布控类型") %>'
                                    Width="280" AllowBlank="false">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <%--<ext:CompositeField ID="CompositeField1" runat="server" FieldLabel='<%# GetLangStr("SuspicionManager36","有效时间") %>' Width="280" DefaultAnchor="100%"  Layout="RowLayout">
                                    <Items>--%>
                                <ext:DateField ID="DateYxsj" runat="server" Vtype="daterange" Format="yyyy-MM-dd HH-mm-ss" AllowBlank="false" ColumnWidth="1" FieldLabel='<%# GetLangStr("SuspicionManager45","有效时间") %>'>
                                </ext:DateField>
                                <%--  <ext:TimeField ID="TimeYxsj" runat="server" Increment="1" Width="50" /> --%>
                                <%--    </Items>
                                </ext:CompositeField>--%>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("SuspicionManager46","布控原因") %>' ID="txtSjyy" Width="280" />
                                <%--ext:Panel ID="Panel2" runat="server" Height="32" Layout="ContainerLayout" Border="false">
                                    <Content>
                                        <dpart:UIDepartment ID="uiDepartment" runat="server" FieldLabel='<%# GetLangStr("SuspicionManager38","数据来源") %>' Width="300" ListWidth="300" />
                                    </Content>
                                </%ext:Panel--%>
                                <ext:Panel ID="Panel5" runat="server" Height="32" Layout="ContainerLayout" Border="false">
                                    <Items>
                                        <ext:DropDownField ID="FieldPerson" runat="server" Editable="false" FieldLabel='<%# GetLangStr("SuspicionManager47","报警接收人") %>'
                                            Width="300" TriggerIcon="SimpleArrowDown" Mode="ValueText">
                                            <Component>
                                                <ext:TreePanel ID="TreePerson"
                                                    runat="server" Height="300" Width="300" Shadow="None" UseArrows="true" AutoScroll="true"
                                                    Animate="true" EnableDD="true" ContainerScroll="true"
                                                    RootVisible="true" StyleSpec="background-color: rgba(68,138,202,0.9); border-radius: 20px;">
                                                    <Buttons>
                                                        <ext:Button runat="server" Text='<%# GetLangStr("SuspicionManager48","清除") %>' ID="btnClear">
                                                            <Listeners>
                                                                <Click Handler="clearSelect()" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:Button runat="server" Text='<%# GetLangStr("SuspicionManager49","关闭") %>'>
                                                            <Listeners>
                                                                <Click Handler="#{FieldPerson}.collapse();" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Buttons>
                                                    <Listeners>
                                                        <CheckChange Handler="this.dropDownField.setValue(getValues(this), getText(this), false);" />
                                                    </Listeners>
                                                </ext:TreePanel>
                                            </Component>
                                            <Listeners>
                                                <Expand Handler="this.component.getRootNode().expand(false);" Single="true" Delay="20" />
                                            </Listeners>
                                            <SyncValue Fn="syncValue" />
                                        </ext:DropDownField>
                                    </Items>
                                </ext:Panel>
                                <ext:ComboBox ID="cmbbdbj" FieldLabel='<%# GetLangStr("SuspicionManager50","布控标识") %>' runat="server" StoreID="StoreBdbj" Editable="false"
                                    DisplayField="col1" ValueField="col0" Mode="Local" EmptyText='<%# GetLangStr("SuspicionManager51","选择比对或者撤销") %>' Width="280"
                                    AllowBlank="false">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("SuspicionManager5","清除选中") %>' />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("SuspicionManager52","布控联系人") %>' ID="Txtbkry" AllowBlank="false" Width="280" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("SuspicionManager53","联系电话") %>' ID="Txtlxdh" AllowBlank="false" Width="280" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("SuspicionManager54","备注信息") %>' ID="TxtBz" Width="280" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("SuspicionManager55","更新时间") %>' ID="TxtGxsj" Width="280" ReadOnly="true" Hidden="true" />
                            </Items>
                            <TopBar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:Button ID="ButAdd" runat="server" Text='<%# GetLangStr("SuspicionManager56","增加") %>' Icon="Add" ToolTip='<%# GetLangStr("SuspicionManager57","增加") %>'>
                                            <Listeners>
                                                <Click Handler="SuspicionManager.InfoSave()" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="Button4" runat="server" Text='<%# GetLangStr("SuspicionManager58","保存") %>' Icon="TableSave">
                                            <Listeners>
                                                <Click Handler="SuspicionManager.UpdateData()" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="ButDelete" runat="server" Text='<%# GetLangStr("SuspicionManager59","删除") %>' Icon="Delete">
                                            <Listeners>
                                                <Click Handler="SuspicionManager.DoConfirm()" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:FormPanel>

                        <ext:FormPanel ID="pnlImport" runat="server" Padding="5" DefaultAnchor="100%" Width="320">
                            <Items>
                                <ext:FormPanel ID="bujupnlImport"
                                    runat="server" Header="false" Title='<%# GetLangStr("SuspicionManager60","车辆信息") %>' DefaultAnchor="100%"
                                    Padding="-1" LabelAlign="left">
                                    <Items>

                                        <ext:FileUploadField ID="ExcelFile" runat="server" EmptyText='<%# GetLangStr("SuspicionManager61","选择Excel文件") %>' FieldLabel='<%# GetLangStr("SuspicionManager61","选择Excel文件") %>' LabelStyle="margin-top:3px;"
                                            ButtonText="" Icon="TableAdd" Width="100" Height="30" />

                                        <%--       <ext:ProgressBar ID="Progress1" runat="server" Width="300" />--%>
                                    </Items>

                                    <Buttons>
                                        <ext:Button ID="ButDownload" runat="server" Text='<%# GetLangStr("SuspicionManager62","模板下载") %>' Icon="DiskDownload">
                                            <Listeners>
                                                <Click Handler="SuspicionManager.Download()" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="ButSave" runat="server" Text='<%# GetLangStr("SuspicionManager63","保存") %>' Icon="TableSave">
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
    </form>
</body>
</html>