<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DpcPeccancyQuery.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.DpcPeccancyQuery" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>违法车辆查询</title>
    <meta name="GENERATOR" content="MSHTML 8.00.7600.16853" />
    <meta http-equiv="Content-Type" content="text/html; charset=GBK"/>
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
            width: 50%;
            height: 220px;
        }
        #FormPanel1-xcollapsed{
            display:none!important;
        }
    </style>
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="UTF-8"></script>
    <script language="JavaScript" src="../Scripts/showphoto.js" type="text/javascript" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8"></script>
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
</head>
<body>
    <form id="form1" runat="server">
        <div id="append_parent"></div>
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PeccancyQuery" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Viewport ID="Viewport2" runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="Panel1" Region="North" runat="server" Title="查询条件" Collapsible="true"
                    Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server" Layout="Container">
                            <Items>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:Label ID="lblStartTime" runat="server" Html="<font >&nbsp;&nbsp;开始时间：</font>">
                                        </ext:Label>
                                        <ext:DateField ID="DateStartTime" runat="server" Vtype="daterange">
                                            <Listeners>
                                                <Render Handler="this.endDateField = '#{DateEndTime}'" />
                                            </Listeners>
                                        </ext:DateField>
                                        <ext:TimeField ID="TimeStart" runat="server" Increment="1" Width="61" />
                                        <ext:Label ID="lblEndTime" runat="server" Html="<font >&nbsp;&nbsp;结束时间：</font>">
                                        </ext:Label>
                                        <ext:DateField ID="DateEndTime" runat="server" Vtype="daterange">
                                            <Listeners>
                                                <Render Handler="this.startDateField = '#{DateStartTime}'" />
                                            </Listeners>
                                        </ext:DateField>
                                        <ext:TimeField ID="TimeEnd" runat="server" Increment="1" Width="61" />
                                        <ext:Label ID="Label3" runat="server" Html="<font >&nbsp;&nbsp;车牌号牌：</font>">
                                        </ext:Label>
                                        <ext:Panel ID="Panel2" runat="server" Height="28">
                                            <Content>
                                                <veh:VehicleHead ID="WindowEditor1" runat="server" />
                                            </Content>
                                        </ext:Panel>
                                        <ext:TextField ID="TxtplateId" runat="server" Width="102"  EmptyText="六位号牌号码" >
                                            <Listeners>
                                                <Change Fn="changeUpper " />
                                            </Listeners>
                                        </ext:TextField>
                                        <ext:Label ID="Label1" runat="server" Html="<font >&nbsp;&nbsp;号牌种类：</font>">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbPlateType" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText="请选择..."
                                            SelectOnFocus="true" Width="123">
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
                                        <ext:Checkbox ID="ChkLike" runat="server" BoxLabel="模糊查询" />
                                        <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
                                        <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text="查询">
                                            <DirectEvents>
                                                <Click OnEvent="TbutQueryClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text="重置">
                                            <DirectEvents>
                                                <Click OnEvent="ButResetClick" />
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="Toolbar6" runat="server" Flat="false">
                                    <Items>
                                        <ext:Label ID="Label4" runat="server" Html="<font >&nbsp;&nbsp;违法地点：</font>">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbLocation" runat="server" Editable="false" DisplayField="col2"
                                            Width="160" ValueField="col1" TypeAhead="true" Mode="Local" ForceSelection="true"
                                            EmptyText="选择违法地点..." SelectOnFocus="true">
                                            <Store>
                                                <ext:Store ID="StoreLocation" runat="server">
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
                                        <ext:Label ID="Label5" runat="server" Html="<font >&nbsp;&nbsp;违法行为：</font>">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbPecType" runat="server" Editable="false" DisplayField="col2"
                                            ValueField="col1" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText="选择违法行为..."
                                            SelectOnFocus="true" Width="260">
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
                                        <ext:Label ID="Label7" runat="server" Html="<font >&nbsp;&nbsp;抓拍用户：</font>">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbCaptureUser" runat="server" Editable="false" DisplayField="col2"
                                            ValueField="col1" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText="选择抓拍用户..."
                                            SelectOnFocus="true" Width="123">
                                            <Store>
                                                <ext:Store ID="StoreCaptureUser" runat="server">
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
                                        <ext:Label ID="Label6" runat="server" Html="<font >&nbsp;&nbsp;处理状态：</font>">
                                        </ext:Label>
                                        <ext:ComboBox ID="CmbDealType" runat="server" Editable="false" DisplayField="col1"
                                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText="选择处理状态..."
                                            SelectOnFocus="true" Width="100">
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
                                        <ext:ComboBox ID="CmbQueryNum" runat="server" Editable="false" DisplayField="col1"
                                            Width="60" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                            EmptyText="选择..." SelectOnFocus="true">
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
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <ext:FormPanel ID="FormPanel2" Region="Center" runat="server" Title="查询结果" Collapsible="true" AutoScroll="true">
                    <Items>
                        <ext:GridPanel ID="GridPanel1" runat="server" StripeRows="true" Header="false" AutoHeight="true" Collapsible="true">
                            <Store>
                                <ext:Store ID="StorePeccancy" runat="server" OnRefreshData="MyData_Refresh">
                                    <AutoLoadParams>
                                        <ext:Parameter Name="start" Value="={0}" />
                                        <ext:Parameter Name="limit" Value="={15}" />
                                    </AutoLoadParams>
                                    <UpdateProxy>
                                        <ext:HttpWriteProxy Method="GET" Url="PeccancyQuery.aspx">
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
                                                <ext:RecordField Name="col6" Type="Date" />
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

                                    <ext:Column Header="违法地点" Width="120" Sortable="true" DataIndex="col8">
                                    </ext:Column>
                                    <ext:Column Header="号牌号码" Width="75" Sortable="true" DataIndex="col3">
                                    </ext:Column>
                                    <ext:Column Header="号牌种类" Width="100" Sortable="true" DataIndex="col2">
                                    </ext:Column>
                                    <ext:DateColumn Header="违法时间" DataIndex="col6" Width="120" Format="yyyy-MM-dd HH:mm:ss" />
                                    <ext:Column Header="违法行为" Width="250" Sortable="true" DataIndex="col5">
                                    </ext:Column>
                                    <ext:Column Header="速度/限速" Width="80" Sortable="true" DataIndex="col12">
                                    </ext:Column>
                                    <ext:Column Header="处理进程" Width="100" Sortable="true" DataIndex="col20"></ext:Column>
                                    <ext:Column Header="抓拍用户" Width="80" Sortable="true" DataIndex="col29" />
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
                            <BottomBar>
                                <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StorePeccancy">
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
                                      <%--  <ext:Button ID="ButXml" runat="server" Text="导出XML" AutoPostBack="true" OnClick="ToXml"
                                            Icon="PageCode">
                                        </ext:Button>--%>
                                        <ext:Button ID="ButExcel" runat="server" Text="导出Excel" AutoPostBack="true" OnClick="ToExcel"
                                            Icon="PageExcel">
                                        </ext:Button>
                                       <%-- <ext:Button ID="ButCsv" runat="server" Text="导出CSV" AutoPostBack="true" OnClick="ToCsv"
                                            Icon="PageAttach">
                                        </ext:Button>
                                        <ext:Button ID="ButPrint" runat="server" Icon="Printer" Text="打印">
                                            <DirectEvents>
                                                <Click OnEvent="ButPrintClick" />
                                            </DirectEvents>
                                        </ext:Button>--%>
                                    </Items>
                                </ext:PagingToolbar>
                            </BottomBar>
                            <Listeners>
                                <CellClick Fn="cellClick" />
                            </Listeners>
                            <DirectEvents>
                                <CellClick OnEvent="ShowDetails" Failure="Ext.MessageBox.alert('加载失败', '提示');">
                                    <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="={#{GridPanel1}.body}" />
                                    <ExtraParams>
                                        <ext:Parameter Name="data" Value="params[0].getStore().getAt(params[1]).data" Mode="Raw" />
                                    </ExtraParams>
                                </CellClick>
                            </DirectEvents>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Frame="true"
                    Title="详细信息" Width="0" height="0" Icon="Images" DefaultAnchor="100%" Collapsible="true"
                    AutoScroll="true" Collapsed="true">
                    <Content>
                        <div class="photoblock-many">
                           
                                <div id="divplateId" style="width: 100%; font-size: 30pt; font-family: 微软雅黑; color: white; background-color: blue;">
                                   
                                </div>
                               <div class="container" style="width:100%;height:220px">
                              
                                <div class="fis">
                                    <img id="zjwj1" style="cursor: pointer" onclick="zoom(this,false);" class="photo"
                                        src="../images/NoImage.png" alt="车辆图片(图片点击滚轮缩放)" width="100%" height="220" ondblclick="OpenPicPage(this.src);" />
                                </div>
                             
                                <div class="fis">
                                    <img id="zjwj2" style="cursor: pointer" onclick="zoom(this,false);" class="photo"
                                        src="../images/NoImage.png" alt="车辆图片(图片点击滚轮缩放)" width="100%" height="220" ondblclick="OpenPicPage(this.src);" />
                                </div>
                               
                                <div class="fis">
                                    <img id="zjwj3" style="cursor: pointer" onclick="zoom(this,false);" class="photo"
                                        src="../images/NoImage.png" alt="车辆图片(图片点击滚轮缩放)" width="100%" height="220" ondblclick="OpenPicPage(this.src);" />
                                </div>
                               </div>
                        </div>
                    </Content>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>

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
                  //  $(this).removeClass("import").next().remove();
                }

            })
        })
</script>
</html>