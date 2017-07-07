<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OneLisenceMulCar.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Passcar.OneLisenceMulCar" %>

<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>套牌</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" href="../Css/chooser.css" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <link rel="stylesheet" href="../Css/showphotostyle.css" type="text/css" />
    <link rel="Stylesheet" href="../Styles/datetime.css" type="text/css" />
    <style type="text/css">
        body .ui-right-wrap .x-grid3-body .x-grid3-td-numberer {
            background-image: none !important;
            background-image: none;
        }
    </style>
    <style type="text/css">
        body, html {
            font-family: Arial,Verdana;
            font-size: 13px;
            margin: 0;
            overflow: hidden;
        }

        #map_canvas {
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            position: absolute;
        }

        #cboplate_Panel1 {
            background: white;
        }

            #cboplate_Panel1 .x-btn {
                border-radius: 0px;
                border: none;
            }

            #cboplate_Panel1 button {
                height: 24px;
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
                height: 210px;
                width: 252px;
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
            padding-left: 20px;
            margin: 10px;
        }

        .fis {
            display: inline-block;
            float: left;
            width: 33.33333%;
            height: 220px;
        }
    </style>
    <script type="text/javascript" language="javascript" src="../Scripts/common.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8"></script>
    <script type="text/javascript" src="js/Qquery1.91-min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="js/jquery.nicescroll.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <%--<script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>--%>
    <!--图片放大开始-->
    <script type="text/javascript" src="../Scripts/Zoom/jquery.photo.gallery.js"></script>
    <!--图片放大结束-->
    <script type="text/javascript">

        var change = function (obj, value) {
            obj.setValue(value.toUpperCase());
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:Hidden ID="curpage" runat="server" />
        <ext:Hidden ID="totalpage" runat="server" />

        <ext:Hidden ID="realCountImg" runat="server" />
        <ext:Hidden ID="curpage1" runat="server" />
        <ext:Hidden ID="allPage" runat="server" />
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="OneLisenceMulCar" />
        <ext:Store runat="server" ID="Store1">
            <Reader>
                <ext:JsonReader>
                    <Fields>

                        <ext:RecordField Name="hphm" />
                        <ext:RecordField Name="hpzlname" />
                        <ext:RecordField Name="gwsj1" />
                        <ext:RecordField Name="kkidname1" />
                        <ext:RecordField Name="zjwj1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
            <AutoLoadParams>
                <ext:Parameter Name="start" Value="0" Mode="Raw" />
                <ext:Parameter Name="limit" Value="10" Mode="Raw" />
            </AutoLoadParams>
        </ext:Store>
        <ext:Viewport ID="Viewport1" runat="server" Layout="RowLayout" Cls="new-layout">
            <Items>
                <ext:Panel runat="server" Hidden="true" Border="false" Height="30" Layout="ColumnLayout">
                    <Items>
                        <ext:Panel runat="server" Border="false" ColumnWidth="1" />
                        <ext:Button runat="server" Text="导出excel" />
                        <ext:Label runat="server" Text="|" />
                        <ext:Button runat="server" Text="导出word" />
                        <ext:Label runat="server" Text="|" />
                        <ext:Button runat="server" Text="打印" />
                        <ext:Label runat="server" Text="" Width="50" />
                    </Items>
                </ext:Panel>
                <%--查询条件--%>
                <ext:Panel runat="server" Border="false" Height="30" Padding="10" Layout="ColumnLayout">
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Panel runat="server" Height="30" StyleSpec="margin-right:10px;" AutoDataBind="true">
                                    <Content>
                                        <table style="width: 400px">
                                            <tr>
                                                <td style="width: 50px">
                                                    <span class="laydate-span" style="height: 30px; font-size: 15px; margin-left: 12px; margin-right: 2px; margin-top: 5px;"><%# GetLangStr("OneLisenceMulCar41","查询时间:") %>  </span> </td>
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
                                <ext:Label FieldLabel='<%# GetLangStr("OneLisenceMulCar1","号牌号码") %>' AutoDataBind="true"  Width="70" runat="server" LabelStyle="margin-left: 10px" />
                                <ext:Panel ID="Panel4" runat="server" Border="false" Height="26" StyleSpec="margin-left:-30px;">
                                    <Content>
                                        <veh:VehicleHead ID="cboplate" runat="server" />
                                    </Content>
                                </ext:Panel>
                                <ext:TextField ID="txtplate" Width="175" runat="server">
                                    <Listeners>
                                        <Change Fn="change" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:Button runat="server" Width="100" Text='<%# GetLangStr("OneLisenceMulCar2","查询") %>' AutoDataBind="true"  Icon="ControlPlayBlue">
                                    <DirectEvents>
                                        <Click OnEvent="ButQueryClick">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:Panel>
                <ext:Panel Layout="ColumnLayout" Border="true" RowHeight="1" runat="server">
                    <Items>
                        <ext:Panel runat="server" Layout="RowLayout" Width="350">
                            <TopBar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:Button ID="ButFirst" runat="server" AutoDataBind="true" Text='<%# GetLangStr("OneLisenceMulCar3","首页") %>'> 
                                            <DirectEvents>
                                                <Click OnEvent="TbutFisrt" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButLast" runat="server" AutoDataBind="true"    Text='<%# GetLangStr("OneLisenceMulCar4","上一页") %>'>
                                            <DirectEvents>
                                                <Click OnEvent="TbutLast" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButNext" runat="server"  AutoDataBind="true"   Text='<%# GetLangStr("OneLisenceMulCar5","下一页") %>'>
                                            <DirectEvents>
                                                <Click OnEvent="TbutNext" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButEnd" runat="server"  AutoDataBind="true"   Text='<%# GetLangStr("OneLisenceMulCar6","尾页") %>'>
                                            <DirectEvents>
                                                <Click OnEvent="TbutEnd" />
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Items>
                                <ext:GridPanel ID="GridRoadManager" runat="server" AutoDataBind="true" Title='<%# GetLangStr("OneLisenceMulCar7","当前0页,共0页") %>' StripeRows="true" HideBorders="false" BodyBorder="true"
                                    AutoHeight="false" AutoExpandColumn="hphm" RowHeight="1" HideCollapseTool="true" Width="350" AutoScroll="true">
                                    <Store>
                                        <ext:Store ID="StoreInfo" runat="server">
                                            <Reader>
                                                <ext:JsonReader IDProperty="col1">
                                                    <Fields>
                                                        <ext:RecordField Name="hphm" />
                                                        <ext:RecordField Name="hpzlname" />
                                                        <ext:RecordField Name="tpcs" />
                                                        <ext:RecordField Name="hpzl" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <ext:RowNumbererColumn Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("OneLisenceMulCar1","号牌号码") %>'  AutoDataBind="true"   DataIndex="hphm" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("OneLisenceMulCar9","号牌种类") %>'  AutoDataBind="true"    DataIndex="hpzlname" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("OneLisenceMulCar10","套牌次数") %>' AutoDataBind="true"   DataIndex="tpcs" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("OneLisenceMulCar11","hpzl") %>'     AutoDataBind="true"   Hidden="true" DataIndex="hpzl" Align="Center" />
                                        </Columns> 
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel runat="server">
                                            <Listeners>
                                                <RowSelect Handler="OneLisenceMulCar.SelectRow(record.data.hphm,record.data.hpzl)" />
                                            </Listeners>
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                </ext:GridPanel>
                            </Items>
                        </ext:Panel>

                        <ext:Panel runat="server" ColumnWidth="1" Border="false" Layout="RowLayout">
                            <Items>
                                <ext:Panel
                                    runat="server"
                                    ID="ImagePanel"
                                    Cls="images-view"
                                    Frame="true"
                                    AutoHeight="true"
                                    AutoWidth="true"
                                    Layout="Fit">
                                    <TopBar>
                                        <%-- <ext:PagingToolbar runat="server" StoreID="Store1" PageSize="5" ID="PagesizeScreen" HideRefresh="true" />--%>
                                        <ext:Toolbar runat="server">
                                            <Items>
                                                <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10" />

                                                <ext:Button ID="Button1" runat="server" Icon="ControlRewindBlue" Style="margin-left: 10px" Text="" Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutLast1" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="Button2" runat="server" Icon="ControlFastforwardBlue" Style="margin-left: 10px" Text=""
                                                    Disabled="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutNext1" />
                                                    </DirectEvents>
                                                </ext:Button>

                                                <ext:Label ID="lblTitle" runat="server" Text='<%# GetLangStr("OneLisenceMulCar12","查询结果：当前是第") %>' StyleSpec=" margin-left:10px;" />
                                                <ext:Label ID="lblCurpage" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label13" runat="server" Text='<%# GetLangStr("OneLisenceMulCar13","页,共有") %>' />
                                                <ext:Label ID="lblAllpage" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label14" runat="server" Text='<%# GetLangStr("OneLisenceMulCar13","页,共有") %>' />
                                                <ext:Label ID="lblRealcount" runat="server" Text="" Cls="pageNumLabel" />
                                                <ext:Label ID="Label15" runat="server" Text='<%# GetLangStr("OneLisenceMulCar15","条记录") %>' />
                                                <ext:ToolbarFill runat="server">
                                                </ext:ToolbarFill>
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <Items>
                                        <ext:DataView
                                            runat="server" StoreID="Store1" AutoHeight="true" MultiSelect="true"
                                            OverClass="x-view-over" ItemSelector="div.thumb-wrap" EmptyText="No images to display">
                                            <Template runat="server">
                                                <Html>
                                                    <tpl for=".">
								<div class="thumb-wrap" id="{name}">
									<div class="thumb"><img src="{zjwj1}" title="{hphm}  " onclick="$.openPhotoGalleryImg(this);" class="photo" /></div>
                                    <div>
														<H3>通过卡口:{kkidname1}</H3>
													</div>
                                    <div>
														<H3>过往时间:{gwsj1}</H3>
													</div>
                                    <div>
														<H3>号牌:{hpzlname}-{hphm}</H3>
													</div>
								</div>
							</tpl>
                                                    <div class="x-clear"></div>
                                                </Html>
                                            </Template>
                                        </ext:DataView>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel runat="server" Layout="ColumnLayout" Border="false" Height="150" ColumnWidth="1">
                                    <Items>
                                        <ext:Panel runat="server" Width="100" Layout="RowLayout">
                                            <Items>
                                                <ext:Panel runat="server" RowHeight=".4"></ext:Panel>
                                                <ext:Button runat="server" Width="100" Text='<%# GetLangStr("OneLisenceMulCar16","加入套牌库") %>' AutoDataBind="true" >
                                                    <DirectEvents>
                                                        <Click OnEvent="AddTp" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Panel runat="server" RowHeight=".2"></ext:Panel>
                                                <ext:Button runat="server" Width="100" Text='<%# GetLangStr("OneLisenceMulCar17","地图") %>' AutoDataBind="true">
                                                    <DirectEvents>
                                                        <Click OnEvent="MapShow" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Panel runat="server" RowHeight=".4"></ext:Panel>
                                            </Items>
                                        </ext:Panel>

                                        <ext:Panel runat="server" Title='<%# GetLangStr("OneLisenceMulCar18","车驾管信息") %>' AutoDataBind="true" ColumnWidth="1" Layout="RowLayout">
                                            <Items>
                                                <ext:Panel runat="server" Layout="ColumnLayout" RowHeight=".3">
                                                    <Items>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                                            <Items>
                                                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("OneLisenceMulCar19","车牌号牌：") %>' AutoDataBind="true" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                                </ext:Label>
                                                                <ext:TextField ID="txtHphm" runat="server" ColumnWidth=".65" Disabled="true">
                                                                </ext:TextField>
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                                            <Items>
                                                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("OneLisenceMulCar20","号牌种类：") %>' AutoDataBind="true" ColumnWidth=".35" StyleSpec="float: left; height: 24px; line-height: 24px!important; text-align: center">
                                                                </ext:Label>
                                                                <ext:TextField ID="txtHpzl" runat="server" ColumnWidth=".65" Disabled="true">
                                                                </ext:TextField>
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                                            <Items>
                                                                <ext:Label ID="Label2" runat="server" Text='<%# GetLangStr("OneLisenceMulCar19","车辆品牌：") %>' AutoDataBind="true" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                                </ext:Label>
                                                                <ext:TextField ID="txtClpp" runat="server" ColumnWidth=".65" Disabled="true">
                                                                </ext:TextField>
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                                            <Items>
                                                                <ext:Label ID="Label4" runat="server" Text='<%# GetLangStr("OneLisenceMulCar21","车身颜色：") %>' AutoDataBind="true" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                                </ext:Label>
                                                                <ext:TextField ID="txtCsys" runat="server" ColumnWidth=".65" Disabled="true">
                                                                </ext:TextField>
                                                            </Items>
                                                        </ext:Panel>
                                                    </Items>
                                                </ext:Panel>
                                                <ext:Panel runat="server" Layout="ColumnLayout" RowHeight=".3">
                                                    <Items>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                                            <Items>
                                                                <ext:Label ID="Label5" runat="server" Text='<%# GetLangStr("OneLisenceMulCar19","车牌号牌：") %>' AutoDataBind="true" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                                </ext:Label>
                                                                <ext:TextField ID="txtCllx" runat="server" ColumnWidth=".65" Disabled="true">
                                                                </ext:TextField>
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                                            <Items>
                                                                <ext:Label ID="Label6" runat="server" Text='<%# GetLangStr("OneLisenceMulCar22","使用性质：") %>' ColumnWidth=".35" StyleSpec="float: left; height: 24px; line-height: 24px!important; text-align: center">
                                                                </ext:Label>
                                                                <ext:TextField ID="txtSyxz" runat="server" ColumnWidth=".65" Disabled="true">
                                                                </ext:TextField>
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                                            <Items>
                                                                <ext:Label ID="Label7" runat="server" Text='<%# GetLangStr("OneLisenceMulCar23","车辆状态：") %>' AutoDataBind="true" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                                </ext:Label>
                                                                <ext:TextField ID="txtClzt" runat="server" ColumnWidth=".65" Disabled="true">
                                                                </ext:TextField>
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                                            <Items>
                                                                <ext:Label ID="Label8" runat="server" Text='<%# GetLangStr("OneLisenceMulCar24","发证机关：") %>' AutoDataBind="true" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                                </ext:Label>
                                                                <ext:TextField ID="txtFzjg" runat="server" ColumnWidth=".65" Disabled="true">
                                                                </ext:TextField>
                                                            </Items>
                                                        </ext:Panel>
                                                    </Items>
                                                </ext:Panel>
                                                <ext:Panel runat="server" Layout="ColumnLayout" RowHeight=".3">
                                                    <Items>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                                            <Items>
                                                                <ext:Label ID="Label9" runat="server" Text='<%# GetLangStr("OneLisenceMulCar25","车主姓名：") %>' AutoDataBind="true" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                                </ext:Label>
                                                                <ext:TextField ID="txtSyr" runat="server" ColumnWidth=".65" Disabled="true">
                                                                </ext:TextField>
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                                            <Items>
                                                                <ext:Label ID="Label10" runat="server" Text='<%# GetLangStr("OneLisenceMulCar26","联系电话：" ) %>' AutoDataBind="true" ColumnWidth=".35" StyleSpec="float: left; height: 24px; line-height: 24px!important; text-align: center">
                                                                </ext:Label>
                                                                <ext:TextField ID="txtLxdh" runat="server" ColumnWidth=".65" Disabled="true">
                                                                </ext:TextField>
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                                            <Items>
                                                                <ext:Label ID="Label11" runat="server" Text='<%# GetLangStr("OneLisenceMulCar27","有效期止：") %>' AutoDataBind="true" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                                </ext:Label>
                                                                <ext:TextField ID="txtYxqz" runat="server" ColumnWidth=".65" Disabled="true">
                                                                </ext:TextField>
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                                            <Items>
                                                                <ext:Label ID="Label12" runat="server" Text='<%# GetLangStr("OneLisenceMulCar28","家庭住址：") %>' AutoDataBind="true" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center" />
                                                                <ext:TextField ID="txtXxdz" runat="server" ColumnWidth=".65" Disabled="true" />
                                                            </Items>
                                                        </ext:Panel>
                                                    </Items>
                                                </ext:Panel>
                                                <ext:Panel runat="server" RowHeight="1" />
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" RowHeight=".3" HideBorders="true" Layout="RowLayout" ID="panelChart">
                                            <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
                                        </ext:Panel>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel runat="server" Width="15" />
                            </Items>
                        </ext:Panel>
                    </Items>
                </ext:Panel>
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
            OneLisenceMulCar.GetDateTime(true, tt);
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
            OneLisenceMulCar.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>