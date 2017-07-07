<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FisrtIntoCity.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Passcar.FisrtIntoCity" %>

<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>初次入城</title>
    <%--样式--%>
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <link rel="Stylesheet" type="text/css" href="../Styles/hphm/autohphm.css" />
    <style type="text/css">
        .ext-gen114 {
            font-weight: bold;
        }

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
        /*号牌控件背景*/
        #WindowEditor1_Panel1 {
            background: white;
        }

        #FormPanel1-xcollapsed {
            display: none !important;
        }

        #WindowEditor1_Panel1 .x-btn {
            border-radius: 0px;
            border: none;
        }

        #WindowEditor1_Panel1 button {
            height: 24px;
        }

        .ext-gen193 {
            border-radius: 8px;
        }

        .fis {
            display: inline-block;
            float: left;
            width: 33.3333%;
            height: 220px;
        }
    </style>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <%--<script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8">></script>
    <!--图片放大开始-->

    <script type="text/javascript" src="../Scripts/Zoom/jquery.photo.gallery.js"></script>
    <!--图片放大结束-->
    <link rel="stylesheet" href="../Css/showphotostyle.css" type="text/css" />
    <script type="text/javascript" src="../Scripts/common.js" charset="UTF-8"></script>
    <script src="../Scripts/showphoto.js" type="text/javascript" charset="UTF-8">></script>
    <link rel="Stylesheet" href="../Styles/datetime.css" type="text/css" />
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
        var change = function (obj, value) {
            obj.setValue(value.toUpperCase());
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
    </script>
    <%--详细信息--%>
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
        function ShowImage(image1, image2, image3, palteid, platetype) {
            document.getElementById("zjwj1").src = image1;
            document.getElementById("zjwj2").src = image2;
            document.getElementById("zjwj3").src = image3;
            ChangeBackColor("divplateId", platetype, palteid);

        }
        function clearTime(start, end) {

            CmbPlateType.triggers[0].hide();

        }
    </script>

    <script type="text/javascript">
        var IMGDIR = '../images/sets';
        var attackevasive = '0';
        var gid = 0;
        var fid = parseInt('0');
        var tid = parseInt('0');
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridPanel2.view.findRowIndex(this.triggerElement),
                cellIndex = GridPanel2.view.findCellIndex(this.triggerElement),
                record = StorePeccancy.getAt(rowIndex),
                fieldName = GridPanel2.getColumnModel().getDataIndex(cellIndex),
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
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="FisrtIntoCity" />
        <div id="append_parent">
        </div>
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
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Hidden ID="realCount" runat="server" />
        <ext:Hidden ID="allPage" runat="server" />
        <ext:Hidden ID="curpage" runat="server" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="RowLayout" Cls="new-layout">
            <Items>
                <ext:Panel ID="paneltime" Height="40" Padding="5" Layout="ColumnLayout" runat="server">
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Panel runat="server" Height="40" Border="false" Width="500">
                                    <Content>
                                        <div id="selectDate" style="margin-top: 5px">
                                            <span class="laydate-span" style="margin-left: 12px; height: 24px;"><%#GetLangStr("FisrtIntoCity32","查询时间：") %></span>
                                            <li runat="server" class="laydate-icon" id="start" style="width: 160px; margin-left: 16px; height: 22px;"></li>
                                        </div>
                                        <div style="margin-top: 5px">
                                            <span class="laydate-span" style="margin-left: 12px; height: 24px;">--</span>
                                            <li runat="server" class="laydate-icon" id="end" style="width: 160px; margin-left: 16px; height: 22px;"></li>
                                        </div>
                                    </Content>
                                </ext:Panel>

                                <ext:Label ID="Label2" runat="server" Text='<%# GetLangStr("FisrtIntoCity1","号牌种类：") %>' ColumnWidth=".5" StyleSpec=" margin-left:5px;  float: left; height: 24px; line-height: 24px!important; text-align: center">
                                </ext:Label>
                                <ext:ComboBox ID="CmbPlateType" runat="server" Editable="false" DisplayField="col1" StoreID="StorePlateType"
                                    ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("FisrtIntoCity2","请选择...") %>'
                                    SelectOnFocus="true" Width="130">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("FisrtIntoCity3","清除选中") %>' AutoDataBind="true" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Label Text='<%# GetLangStr("FisrtIntoCity4","号牌号码:") %>' Width="100" runat="server" Height="30" StyleSpec="margin-left:10px;" />
                                <ext:Panel ID="Panel1" runat="server" Border="false" Height="24">
                                    <Content>
                                        <veh:VehicleHead ID="cboplate" runat="server" />
                                    </Content>
                                </ext:Panel>
                                <ext:Panel runat="server" Layout="ColumnLayout" Height="30">
                                    <Items>
                                        <ext:TextField ID="txtplate" runat="server" Hidden="false" MaxLength="6" Width="150" Height="30" EmptyText='<%# GetLangStr("FisrtIntoCity5","六位号牌号码") %>'>
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
                                        <ext:Checkbox runat="server" ID="cktype" Checked="false" BoxLabel='<%# GetLangStr("FisrtIntoCity6","模糊查询") %>' StyleSpec="margin-left:0px;">
                                            <DirectEvents>
                                                <Check OnEvent="changtype" />
                                            </DirectEvents>
                                        </ext:Checkbox>
                                    </Items>
                                </ext:Panel>
                                <ext:Button ID="ButQuery" runat="server" Text='<%# GetLangStr("FisrtIntoCity7","查询") %>' Icon="ControlPlayBlue" StyleSpec="margin-left:10px;">
                                    <DirectEvents>
                                        <Click OnEvent="ButQuery_Event">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                    </Items>
                </ext:Panel>
                <ext:Panel runat="server" RowHeight=".8" HideBorders="true" ColumnWidth="1" ID="PanelFisrt" Layout="FitLayout">
                    <Items>
                        <ext:FormPanel runat="server" ID="FormPanelFisrt" Layout="FitLayout">

                            <%--分页头部--%>
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server" Layout="container" Flat="false">
                                    <Items>
                                        <ext:Toolbar runat="server">
                                            <Items>

                                                <%--  <ext:Label ID="LabNum" runat="server" Html="<font>&nbsp;&nbsp;当前0页,共0页</font>">
                                                </ext:Label>--%>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10" />

                                                <ext:Button ID="ButFisrt" runat="server" Text='<%# GetLangStr("FisrtIntoCity8","首页") %>'>
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutFisrt">
                                                            <EventMask ShowMask="true" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>

                                                <ext:Button ID="ButLast" runat="server" Icon="ControlRewindBlue" StyleSpec="margin-left:10px;" Text='<%# GetLangStr("FisrtIntoCity9","上一页") %>'>
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutLast">
                                                            <EventMask ShowMask="true" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButNext" runat="server" Icon="ControlFastforwardBlue" StyleSpec="margin-left:10px;" Text='<%# GetLangStr("FisrtIntoCity10","下一页") %>'>
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutNext">
                                                            <EventMask ShowMask="true" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>

                                                <ext:Button ID="ButEnd" runat="server" Text='<%# GetLangStr("FisrtIntoCity11","尾页") %>' StyleSpec="margin-left:10px;">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutEnd">
                                                            <EventMask ShowMask="true" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Label ID="lblTitle" runat="server" Text='<%# GetLangStr("FisrtIntoCity12","查询结果：当前是第") %>' StyleSpec="margin-left:10px;fonsi-size:25px;" />
                                                <ext:Label ID="lblCurpage" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("FisrtIntoCity13","页,共有") %>' />
                                                <ext:Label ID="lblAllpage" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label9" runat="server" Text='<%# GetLangStr("FisrtIntoCity13","页,共有") %>' StyleSpec="font-weight:bolder;" />
                                                <ext:Label ID="lblRealcount" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label12" runat="server" Text='<%# GetLangStr("FisrtIntoCity15","条记录") %>' />
                                                <ext:ToolbarFill runat="server">
                                                </ext:ToolbarFill>
                                            </Items>
                                        </ext:Toolbar>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <%-- 显示数据--%>
                            <Items>
                                <ext:GridPanel ID="GridPanel2" runat="server" StripeRows="true" Header="false" BodyStyle="height:100%;width:100%" Collapsible="true" TrackMouseOver="true">
                                    <Store>
                                        <ext:Store ID="StorePeccancy" runat="server" OnRefreshData="MyData_Refresh">
                                            <AutoLoadParams>
                                                <ext:Parameter Name="start" Value="={0}" />
                                                <ext:Parameter Name="limit" Value="={15}" />
                                            </AutoLoadParams>
                                            <UpdateProxy>
                                                <ext:HttpWriteProxy Method="GET" Url="FisrtIntoCity.aspx">
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
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel runat="server">
                                        <Columns>
                                            <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                                            <ext:Column Header='<%# GetLangStr("FisrtIntoCity16","详情") %>'   AutoDataBind="true"   DataIndex="col0" Align="Center" Hidden="true" />
                                            <ext:Column Header='<%# GetLangStr("FisrtIntoCity17","卡口名称") %>' AutoDataBind="true"  DataIndex="col2" Align="Left" />
                                            <ext:Column Header='<%# GetLangStr("FisrtIntoCity18","号牌号码") %>'  AutoDataBind="true" DataIndex="col3" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("FisrtIntoCity19","号牌种类") %>' AutoDataBind ="true"  DataIndex="col5" Align="Center" />
                                            <ext:DateColumn Header='<%# GetLangStr("FisrtIntoCity20","过往时间") %>' AutoDataBind ="true" DataIndex="col6" Format="yyyy-MM-dd HH:mm:ss" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("FisrtIntoCity21","行车方向") %>' AutoDataBind="true" DataIndex="col8" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("FisrtIntoCity22","车道") %>'  AutoDataBind="true" DataIndex="col9" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("FisrtIntoCity23","车辆速度") %>' AutoDataBind="true"  DataIndex="col10" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("FisrtIntoCity24","数据来源") %>' AutoDataBind="true"  DataIndex="col13" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("FisrtIntoCity25","记录类型") %>' AutoDataBind="true"  DataIndex="col15" Align="Center" />
                                        </Columns>
                                    </ColumnModel>
                                    <%--点击触发事件--%>
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
                                        <Command Handler="PeccancyQuery.VideoShow(command, record.data.col26);" />
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
                                            Target="={GridPanel2.getView().mainBody}"
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
                </ext:Panel>

                <%--详细信息--%>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Frame="true"
                    Width="0" Height="0" Icon="Images" DefaultAnchor="100%" Collapsible="true"
                    AutoScroll="true" Collapsed="true">
                    <Content>
                        <div class="photoblock-many">
                            <center>
                                <div id="divplateId" style="width: 100%; font-size: 30pt; font-family: 微软雅黑; color: white; background-color: blue;">
                                </div>
                                <div class="container" style="width: 100%; height: 220px">
                                    <div class="fis">
                                        <img id="zjwj1" style="cursor: pointer" onclick="$.openPhotoGallery(this);"  class="photo"
                                            src="../images/NoImage.png" alt='<%# GetLangStr("FisrtIntoCity26","车辆图片(图片点击滚轮缩放)") %>' width="100%" height="220" />
                                    </div>

                                    <div class="fis">
                                        <img id="zjwj2" style="cursor: pointer" onclick="$.openPhotoGallery(this);"  class="photo"
                                            src="../images/NoImage.png" alt='<%# GetLangStr("FisrtIntoCity26","车辆图片(图片点击滚轮缩放)") %>' width="100%" height="220" />
                                    </div>

                                    <div class="fis">
                                        <img id="zjwj3" style="cursor: pointer" onclick="$.openPhotoGallery(this);"  class="photo"
                                            src="../images/NoImage.png" alt='<%# GetLangStr("FisrtIntoCity26","车辆图片(图片点击滚轮缩放)") %>' width="100%" height="220" />
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
        istoday: false,
        choose: function (datas) {
            end.min = datas; //开始日选好后，重置结束日的最小日期
            end.start = datas //将结束日的初始值设定为开始日
            $("#end").click();//开始时间选中后，自动弹出结束时间
            var tt = $("#start").html();
            FisrtIntoCity.GetDateTime(true, tt);
            //alert(tt);
        }
    };
    var end = {
        elem: '#end',
        format: 'YYYY-MM-DD hh:mm:ss',
        min: laydate.now(),
        max: '2099-06-16 23:59:59',
        istime: true,
        istoday: false,
        choose: function (datas) {
            start.max = datas; //结束日选好后，重置开始日的最大日期
            var tt = $("#end").html();
            FisrtIntoCity.GetDateTime(false, tt);
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
                // $(this).removeClass("import").next().remove();
            }

        })
    })
</script>