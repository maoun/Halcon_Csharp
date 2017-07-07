<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImgCarQuery.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.ImgCarQuery" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Src="../UIDepartment.ascx" TagName="UIDepartment" TagPrefix="dpart" %>
<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>以图搜车</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="../Map/css/custom.css" />
    <link rel="stylesheet" type="text/css" href="../Style/customMap.css" />
    <style type="text/css">
        .custom {
            filter: PROGID:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr='white', EndColorStr='steelblue') PROGID:DXImageTransform.Microsoft.DropShadow(Color='white', OffX=0, OffY=0,Positive=1);
            /*cursor: hand;*/
            cursor: pointer;
            padding-top: 0px;
        }

        .textarea {
            font-size: 20px;
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
                margin: 5px 0px 0px 5px;
                width: 130px;
                height: 130px;
            }

            div.item-wrap h6 {
                font-size: 14px;
                color: #3A4B5B;
                font-family: tahoma,arial,san-serif;
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
                    background: transparent url(images/group-expand-sprite.gif) no-repeat 3px -47px;
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

        .label {
            font: bold 11px tahoma,arial,sans-serif;
            width: 300px;
            height: 15px;
            padding: 5px 0;
            border: 1px dotted #99bbe8;
            color: #15428b;
            cursor: default;
            margin: 10px;
            background: #dfe8f6;
            text-align: center;
            margin-left: 0px;
        }

        .bgimage {
            background: url(../images/SelectImgBg ) no-repeat left top;
        }

        #panelTop {
            background: url(../Images/top/top.png);
            position: fixed;
            width: 88px;
            height: 84px;
            right: 20px;
            top: 40%;
            margin-top: -17px;
            cursor: pointer;
            z-index: 9999;
            display: none;
        }
    </style>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>
    <script language="javascript" src="../Scripts/common.js" type="text/javascript" charset="UTF-8"></script>
    <script language="javascript" type="text/javascript">
        var change = function (obj, value) {
            obj.setValue(value.toUpperCase());
        };
    </script>
    <script type="text/javascript">
        var selectionChanged = function (dv, nodes) {
            if (nodes.length > 0) {
                var id = nodes[0].id;

                var title = nodes[0].title;
                var text = nodes[0].innerText;
                var url = nodes[0].attributes["url"].value;
                url = url.replace("**", "&");
                ImgCarQuery.ImgClick(url);
            }
        }

        var viewClick = function (dv, e) {
            var group = e.getTarget("h2", 3, true);
            if (group) {
                group.up("div").toggleClass("collapsed");
            }
        }
    </script>
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
        function Show() {
            $("#ImgFile-file").click();
        }
    </script>
    <script type="text/javascript">
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="ImgCarQuery" />
        <ext:Hidden ID="Copyright" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="SystemName" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="hiddenCsys" runat="server"></ext:Hidden>
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
        <ext:Viewport ID="Viewport1" runat="server" Layout="RowLayout" HideBorders="true">
            <Items>
                <ext:Panel runat="server" Layout="ColumnLayout" HideBorders="true" ID="panelQuery" RowHeight="1" ColumnWidth="1" Region="Center">
                    <Items>
                        <ext:Panel runat="server" ColumnWidth=".01" HideBorders="true" />
                        <ext:Panel runat="server" ColumnWidth=".98" HideBorders="true" Layout="RowLayout">
                            <Items>
                                <ext:Panel runat="server" HideBorders="true" Height="10"></ext:Panel>
                                <%--时间--%>
                                <ext:Panel runat="server" Layout="ColumnLayout" Height="26" HideBorders="true" StyleSpec="margin-top: 10px">
                                    <Items>
                                        <ext:Panel runat="server" Height="40" HideBorders="true" ColumnWidth=".25" Layout="ContainerLayout">
                                            <Content>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 35%" align="center"><span style="float: left; height: 24px; line-height: 24px!important; text-align: center">查询时间： </span></td>
                                                        <td style="width: 65%">
                                                            <li class="laydate-icon" id="start" runat="server" style="width: 93%; float: left; list-style: none; cursor: pointer; height: 22px; line-height: 22px!important"></li>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Content>
                                        </ext:Panel>
                                        <ext:Panel runat="server" Height="40" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                            <Content>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 35%" align="center"><span style="height: 24px; line-height: 24px!important; text-align: center">--</span>
                                                        </td>
                                                        <td style="width: 65%">
                                                            <li class="laydate-icon" id="end" runat="server" style="width: 93%; float: left; list-style: none; cursor: pointer; height: 22px; line-height: 22px!important"></li>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Content>
                                        </ext:Panel>
                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                            <Items>
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                            <Items>
                                            </Items>
                                        </ext:Panel>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel runat="server" HideBorders="true" Height="20"></ext:Panel>
                                <%--查询条件--%>
                                <ext:Panel runat="server" Layout="ColumnLayout" Height="240" HideBorders="true">
                                    <Items>
                                        <ext:Panel runat="server" Layout="RowLayout" ColumnWidth=".3" HideBorders="true" Cls="bgimage">
                                            <Items>
                                                <ext:Image runat="server" ImageUrl="../image/SelectImg.png" ID="imgShow" ColumnWidth="1" RowHeight=".9">
                                                    <Listeners>
                                                        <Click Fn="Show" />
                                                    </Listeners>
                                                </ext:Image>
                                                <ext:FileUploadField ID="ImgFile" runat="server" ButtonOnly="true" RowHeight=".1" ButtonText="选择" Hidden="true">
                                                    <DirectEvents>
                                                        <FileSelected OnEvent="FileUploadSelect" IsUpload="true">
                                                            <EventMask ShowMask="true" Msg="上传中" />
                                                        </FileSelected>
                                                    </DirectEvents>
                                                </ext:FileUploadField>
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" Layout="ColumnLayout" HideBorders="true" ColumnWidth=".01" />
                                        <ext:Panel runat="server" Layout="RowLayout" ColumnWidth=".69" HideBorders="true">
                                            <Items>
                                                <ext:Panel runat="server" Layout="ColumnLayout" HideBorders="true" Height="25" />
                                                <ext:Panel runat="server" Layout="ColumnLayout" HideBorders="true" Height="26">
                                                    <Items>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".33" Layout="ColumnLayout">
                                                            <Items>
                                                                <ext:Label ID="Label3" runat="server" Text="号牌号码： " ColumnWidth=".35" StyleSpec="margin-left:10px;  float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                                </ext:Label>
                                                                <ext:Panel ID="Panel4" runat="server" Height="24" ColumnWidth=".15" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                                    <Content>
                                                                        <veh:VehicleHead ID="vehicleHead" runat="server" />
                                                                    </Content>
                                                                </ext:Panel>
                                                                <ext:TextField ID="TxtplateId" runat="server" ColumnWidth=".5" Disabled="true"  EmptyText="六位号牌号码" >
                                                                    <Listeners>
                                                                        <Change Fn="change" />
                                                                    </Listeners>
                                                                </ext:TextField>
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".33" Layout="ColumnLayout">
                                                            <Items>
                                                                <ext:Label ID="Label1" runat="server" Text="号牌种类： " ColumnWidth=".35" StyleSpec="margin-left:10px;float: left; height: 24px; line-height: 24px!important; text-align: center">
                                                                </ext:Label>
                                                                <ext:ComboBox ID="CmbPlateType" runat="server" Editable="false" DisplayField="col1" StoreID="StorePlateType"
                                                                    ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                                                    SelectOnFocus="true" ColumnWidth=".65" Disabled="true">
                                                                    <Triggers>
                                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                                                    </Triggers>
                                                                    <Listeners>
                                                                        <Select Handler="this.triggers[0].show();" />
                                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                                    </Listeners>
                                                                </ext:ComboBox>
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".33" />
                                                        <ext:Panel runat="server" ColumnWidth=".01" />
                                                    </Items>
                                                </ext:Panel>
                                                <ext:Panel ID="Panel6" runat="server" HideBorders="true" Layout="ColumnLayout" Height="30" />
                                                <ext:Panel runat="server" Layout="ColumnLayout" Height="26" HideBorders="true">
                                                    <Items>
                                                        <ext:Panel ID="Panel12" runat="server" HideBorders="true" Layout="ColumnLayout" ColumnWidth=".33" Height="24">
                                                            <Items>
                                                                <ext:Label ID="Label8" runat="server" Text="车辆品牌： " ColumnWidth=".35" StyleSpec="margin-left:10px; float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                                </ext:Label>
                                                                <ext:TextField ID="txtClpp" runat="server" ColumnWidth=".65" Height="24" Disabled="true" />
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:Panel ID="Panel1" runat="server" HideBorders="true" Layout="ColumnLayout" ColumnWidth=".33" Height="24">
                                                            <Items>
                                                                <ext:Label ID="Label5" runat="server" Text="车辆型号： " ColumnWidth=".35" StyleSpec="margin-left:10px; float:left;  height: 24px; line-height: 24px!important;text-align: center">
                                                                </ext:Label>
                                                                <ext:TextField ID="TFClzpp" runat="server" ColumnWidth=".65" Height="24" Disabled="true" />
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:Panel ID="Panel7" runat="server" HideBorders="true" Layout="ColumnLayout" ColumnWidth=".33" Height="24">
                                                            <Items>
                                                                <ext:Label ID="Label4" runat="server" Text="车身颜色： " ColumnWidth=".35" StyleSpec="margin-left:10px; float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                                </ext:Label>
                                                                <ext:TextField ID="txtcsys" runat="server" ColumnWidth=".65" Height="24" Disabled="true" />
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".01" Layout="ColumnLayout" />
                                                    </Items>
                                                </ext:Panel>
                                                <%--<ext:Panel ID="Panel3" runat="server" HideBorders="true" Layout="ColumnLayout" Height="30" />
                                                <ext:Panel ID="Panel2" runat="server" HideBorders="true" Layout="ColumnLayout" Height="26">
                                                    <Items>

                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".33" />
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".33" />
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".01" Layout="ColumnLayout" />
                                                        <%--<ext:Panel runat="server" HideBorders="true" ColumnWidth=".02" />
                                                        <ext:CheckboxGroup ID="CheckboxGroup1" runat="server" ColumnsNumber="5">
                                                            <Items>
                                                                <ext:Checkbox ID="cboxNjb" runat="server" BoxLabel="年检标" Width="70" Disabled="true"></ext:Checkbox>
                                                                <ext:Checkbox ID="cboxZjh" runat="server" BoxLabel="纸巾盒" Width="70" Disabled="true"></ext:Checkbox>
                                                                <ext:Checkbox ID="cboxZyb" runat="server" BoxLabel="遮阳板" Width="70" Disabled="true"></ext:Checkbox>
                                                                <ext:Checkbox ID="cboxDz" runat="server" BoxLabel="吊坠" Width="70" Disabled="true"></ext:Checkbox>
                                                                <ext:Checkbox ID="cboxBj" runat="server" BoxLabel="摆件" Width="70" Disabled="true"></ext:Checkbox>
                                                            </Items>
                                                        </ext:CheckboxGroup>
                                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".1" Layout="ColumnLayout" />--%>
                                                <%--    </Items>
                                                </ext:Panel>--%>
                                            </Items>
                                        </ext:Panel>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel ID="DashBoardPanel" runat="server" RowHeight=".6" AutoScroll="true">
                                    <Items>
                                        <ext:CenterLayout runat="server">
                                            <Items>
                                                <ext:DataView ID="Dashboard" runat="server" StoreID="StoreQuery" ColumnWidth="1" SingleSelect="true"
                                                    ItemSelector="div.item-wrap" AutoHeight="true" EmptyText="没有显示">
                                                    <Template ID="Template1" runat="server">
                                                        <Html>
                                                            <div id="items-ct">
								        <tpl for=".">
									        <div class="group-header">
										        <dl>
											        <tpl for="Items">
												        <div id="{Id}" class="item-wrap" title ="{Title}" url="{Uri}">
													        <img src="{Icon}" />
													        <div>
														        <H6>{Title}</H6>
													        </div>
												        </div>
											        </tpl>
											        <div style="clear:left"></div>
										         </dl>
									        </div>
								        </tpl>
							        </div>
                                                        </Html>
                                                    </Template>
                                                    <Listeners>
                                                        <SelectionChange Fn="selectionChanged" />
                                                        <ContainerClick Fn="viewClick" />
                                                    </Listeners>
                                                </ext:DataView>
                                            </Items>
                                        </ext:CenterLayout>
                                    </Items>
                                </ext:Panel>
                            </Items>
                        </ext:Panel>
                        <ext:Panel runat="server" ColumnWidth=".01" HideBorders="true" />
                    </Items>
                </ext:Panel>
                <%--底部--%>
                <ext:Panel runat="server" ID="panelMain" ColumnWidth="1" Region="South">
                    <%--回到顶部--%>
                    <Content>
                        <div id="panelTop">
                        </div>
                    </Content>
                    <AutoLoad Mode="IFrame" ShowMask="true">
                    </AutoLoad>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
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
            ImgCarQuery.GetDateTime(true, tt);
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
            ImgCarQuery.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
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
</html>