<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebAlarmQuery.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.WebAlarmQuery" %>

<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>报警信息查询</title>
    <meta name="GENERATOR" content="MSHTML 8.00.7600.16853" />
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" href="../Css/chooser.css" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <link rel="stylesheet" href="../Css/showphotostyle.css" type="text/css" />
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
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8"></script>
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="UTF-8"></script>
    <script src="../Scripts/showphoto.js" language="JavaScript" type="text/javascript" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>
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
            return '<img class="imgEdit" ext:qtip="查看详细信息" style="cursor:pointer;" src="../images/button/vcard_edit.png" />';
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
    </script>
    <script type="text/javascript">
        function ShowImage(image1, image2, image3, palteid, platetype) {
            document.getElementById("zjwj1").src = image1;
            document.getElementById("zjwj2").src = image2;
            //document.getElementById("zjwj3").src = image3;
            ChangeBackColor("divplateId", platetype, palteid);
            ButCsv.Disabled = false;
            ButExcel.Disabled = false;
            ButXml.Disabled = false;
            ButPrint.Disabled = false;

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
                                        <ext:Panel runat="server" BodyBorder="false">
                                            <Content>
                                                <div runat="server" id="selectDate" style="width: 490px">
                                                    <span style="float: left; margin-left: 0px; height: 24px; line-height: 24px!important; text-align: center">&nbsp;&nbsp;查询时间</span><li runat="server" class="laydate-icon" id="start" style="width: 155px; margin-left: 16px; float: left; list-style: none; cursor: pointer; height: 22px; line-height: 22px!important"></li>
                                                </div>
                                                <div>
                                                    <span style="float: left; margin-left: 20px; height: 24px; line-height: 24px!important; text-align: center">--</span><li runat="server" class="laydate-icon" id="end" style="width: 157px; margin-left: 16px; float: left; list-style: none; cursor: pointer; height: 22px; line-height: 22px!important"></li>
                                                </div>
                                            </Content>
                                        </ext:Panel>
                                        <ext:Label ID="Label3" runat="server" Html="<font >车牌号牌：</font>">
                                        </ext:Label>
                                        <ext:Panel ID="Panel4" runat="server" Height="29">
                                            <Content>
                                                <veh:VehicleHead ID="vehicleHead" runat="server" />
                                            </Content>
                                        </ext:Panel>
                                        <ext:TextField ID="TxtplateId" runat="server" Width="112"  EmptyText="六位号牌号码" >
                                            <Listeners>
                                                <Change Fn="change" />
                                            </Listeners>
                                        </ext:TextField>
                                        <ext:Label ID="Label1" runat="server" Html="<font >&nbsp;&nbsp;号牌种类：</font>">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbPlateType" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText="请选择..."
                                            SelectOnFocus="true" Width="130">
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
                                            </Store> <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.triggers[0].show();" />
                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                    </Listeners>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar6" runat="server" Flat="false">
                                    <Items>
                                        <ext:Label ID="Label4" runat="server" Html="<font >&nbsp;&nbsp;卡口列表：</font>">
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
                                                        <ext:Button runat="server" Text="清除">
                                                            <Listeners>
                                                                <Click Handler="clearSelect(TreeStation,FieldStation);" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:Button runat="server" Text="关闭">
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
                                        </ext:DropDownField>
                                        <ext:Label ID="Label5" runat="server" Html="<font >&nbsp;&nbsp;报警类型：</font>">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbAlarmType" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText="选择报警类型..."
                                            SelectOnFocus="true" Width="160" Enabled="false"  Selectable="false">
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
                                        </ext:ComboBox>       
                                       <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                                        <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text="查询" StyleSpec=" margin-left: 10px;">
                                            <DirectEvents>
                                                <Click OnEvent="TbutQueryClick" Timeout="60000">
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
                <%--中间--%>
                <ext:FormPanel ID="FormPanel2" Region="Center" runat="server" Title="查询结果" Layout="FitLayout"
                    AutoScroll="true">
                    <TopBar>
                        <%--分页--%>
                        <ext:Toolbar ID="Toolbar2" runat="server" Flat="false">
                            <Items>
                                <ext:Label ID="LabNum" runat="server" Html="<font >&nbsp;&nbsp;当前0页,共0页</font>">
                                </ext:Label>
                                <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10" />
                                <ext:Button ID="ButFisrt" runat="server" Icon="ControlStartBlue" Text="首页" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="TbutFisrt" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButLast" runat="server" Icon="ControlRewindBlue" Text="上一页" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="TbutLast" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButNext" runat="server" Icon="ControlFastforwardBlue" Text="下一页"
                                    Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="TbutNext" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButEnd" runat="server" Icon="ControlEndBlue" Text="尾页" Disabled="true">
                                    <DirectEvents>
                                        <Click OnEvent="TbutEnd" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <%--数据展示--%>
                        <ext:GridPanel ID="GridAlarmInfo" runat="server" StripeRows="true" MinColumnWidth="150px" BodyStyle="height:100%;width:100%">
                                 <TopBar>
                                <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StoreAlarmInfo">
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
                                            <SelectedItem Value="15" />
                                            <Listeners>
                                                <Select Handler="#{PagingToolbar1}.pageSize = parseInt(this.getValue()); #{PagingToolbar1}.doLoad();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:PagingToolbar>
                            </TopBar>
                              <Store>
                                <ext:Store ID="StoreAlarmInfo" runat="server" OnRefreshData="MyData_Refresh">
                                    <AutoLoadParams>
                                        <ext:Parameter Name="start" Value="={0}" />
                                        <ext:Parameter Name="limit" Value="={15}" />
                                    </AutoLoadParams>
                                    <UpdateProxy>
                                        <ext:HttpWriteProxy Method="GET" Url="AlarmInfoQuery.aspx">
                                        </ext:HttpWriteProxy>
                                    </UpdateProxy>
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
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column ColumnID="Details" Header="详细" Width="50" Align="Center" Fixed="true"
                                        MenuDisabled="true" Resizable="false">
                                        <Renderer Fn="DataAmply" />
                                    </ext:Column>
                                    <ext:Column Header="记录编号" Width="100" Sortable="true" DataIndex="col0" Hidden="true" />
                                    <ext:Column Header="报警卡口" Width="150" Sortable="true" DataIndex="col9">
                                    </ext:Column>
                                    <ext:Column Header="号牌号码" Width="75" Sortable="true" DataIndex="col3">
                                    </ext:Column>
                                    <ext:Column Header="号牌种类" Width="80" Sortable="true" DataIndex="col2">
                                    </ext:Column>
                                    <ext:DateColumn Header="报警时间" DataIndex="col4" Width="120" Format="yyyy-MM-dd HH:m:ss" />
                                    <ext:Column Header="报警类型" Width="150" Sortable="true" DataIndex="col6">
                                    </ext:Column>
                                    <ext:Column Header="报警原因" Width="200" Sortable="true" DataIndex="col7">
                                    </ext:Column>
                                    <ext:Column Header="处理状态" Width="80" Sortable="true" DataIndex="col18">
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
                                <ext:GridView ID="GroupingView1" runat="server" ForceFit="true" >
                                  
                                </ext:GridView>
                            </View>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>
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
                                        <img id="zjwj1" style="cursor: pointer" onclick="zoom(this,false);" class="photo"
                                            src="../images/NoImage.png" alt="车辆图片(图片点击滚轮缩放)" width="100%" height="220" />
                                    </div>

                                    <div class="fis">
                                        <img id="zjwj2" style="cursor: pointer" onclick="zoom(this,false);" class="photo"
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
            WebAlarmQuery.GetDateTime(true, tt);
            
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
            WebAlarmQuery.GetDateTime(false, tt);
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
          //  $(this).removeClass("import").next().remove();
        }

    })
</script>