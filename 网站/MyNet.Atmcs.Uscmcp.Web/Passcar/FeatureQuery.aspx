<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FeatureQuery.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.FeatureQuery" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>特征查询</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="../Map/css/custom.css" />
    <link rel="stylesheet" type="text/css" href="../Style/customMap.css" />
    <style type="text/css">
        .custom {
            filter: PROGID:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr='white', EndColorStr='steelblue') PROGID:DXImageTransform.Microsoft.DropShadow(Color='white', OffX=0, OffY=0,Positive=1);
            cursor: hand;
            padding-top: 0px;
        }

        .textarea {
            font-size: 20px;
        }
    </style>
    <style type="text/css">
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
    </style>
    <style type="text/css">
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

        #imgCut {
            width: auto !important;
            height: auto !important;
            max-width: 473px;
            max-height: 310px;
        }
    </style>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/common.js" charset="UTF-8"></script>
    <script type="text/javascript">
        function Show() {
            $("#ImgFile-file").click();
        }
        function CutImg() {
            FeatureQuery.ShowCutPanel();
            OpenImgCutPage(document.getElementById("hidimgpath").value, document.getElementById("hidimgwidth").value, document.getElementById("hidimghight").value);
        }
    </script>
    <script type="text/javascript">
        var resize = function (image, factor) {
            if (!factor || !image.complete) {
                return;
            }
            var orgSize = image.getOriginalSize();
            factor = parseFloat(factor);
            image.setSize(parseInt(orgSize.width * factor), parseInt(orgSize.height * factor));
        }

        var newFactor = function (combo, dir) {
            var index = combo.getSelectedIndex(),
                count = combo.store.getCount();
            index += dir;
            if (index >= 0 && index < count) {
                combo.setValueAndFireSelect(combo.store.getAt(index).get(combo.valueField));
            }
        }
    </script>

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
<ext:Store ID="storekk" runat="server">
    <Reader>
        <ext:JsonReader>
            <Fields>
                <ext:RecordField Name="STATION_NAME" />
                <ext:RecordField Name="STATION_ID" />
            </Fields>
        </ext:JsonReader>
    </Reader>
</ext:Store>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="FeatureQuery" />
        <ext:Hidden runat="server" ID="hidimgpath"></ext:Hidden>
        <ext:Hidden runat="server" ID="hidimghight"></ext:Hidden>
        <ext:Hidden runat="server" ID="hidimgwidth"></ext:Hidden>
        <ext:Hidden runat="server" ID="hidpagenum"></ext:Hidden>
        <ext:Store ID="Store2" runat="server">
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
                                <%--查询条件--%>
                                <ext:Panel runat="server" Layout="ColumnLayout" Height="26" HideBorders="true" StyleSpec="margin-top: 10px">
                                    <Items>
                                        <ext:Panel runat="server" Height="40" HideBorders="true" ColumnWidth=".25" Layout="ContainerLayout">
                                            <Content>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 35%" align="center"><span style="float: left; height: 24px; line-height: 24px!important; text-align: center"><%# GetLangStr("FeatureQuery1","查询时间：") %></span></td>
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
                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".1" Layout="ColumnLayout">
                                            <Items>
                                                <ext:Label ID="Label11" runat="server" Text='<%# GetLangStr("FeatureQuery2","相似度：") %>' ColumnWidth=".5" StyleSpec="margin-left:10px; float: left;height: 24px; line-height: 24px!important; " />
                                                <ext:TextField ID="txtxsd" runat="server" ColumnWidth=".5" Text="60" />
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".4" Layout="ColumnLayout">
                                            <Items>
                                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("FeatureQuery3","卡口列表：") %>' ColumnWidth=".175" StyleSpec="margin-left:10px; float: left; height: 24px; line-height: 24px!important; " />
                                                <ext:DropDownField ID="FieldStation" runat="server"
                                                    Editable="false" ColumnWidth=".825" TriggerIcon="SimpleArrowDown" Mode="ValueText">
                                                    <Component>
                                                        <ext:TreePanel runat="server" Height="400" Shadow="None" ID="TreeStation"
                                                            UseArrows="true" AutoScroll="true" Animate="true" EnableDD="true" ContainerScroll="true" RootVisible="true"
                                                            StyleSpec="background-color: rgba(68,138,202,0.9); border-radius: 20px;">
                                                            <Root>
                                                            </Root>
                                                            <Buttons>
                                                                <ext:Button runat="server" Text='<%# GetLangStr("FeatureQuery4","清除") %>'>
                                                                    <Listeners>
                                                                        <Click Handler="clearSelect(TreeStation,FieldStation);" />
                                                                    </Listeners>
                                                                </ext:Button>
                                                                <ext:Button runat="server" Text='<%# GetLangStr("FeatureQuery5","关闭") %>'>
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
                                            </Items>
                                        </ext:Panel>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel runat="server" HideBorders="true" Height="20"></ext:Panel>
                                <%--图片展示--%>
                                <ext:Panel runat="server" Layout="ColumnLayout" RowHeight=".4" HideBorders="true">
                                    <Items>
                                        <%--上传图片--%>
                                        <ext:Panel runat="server" Layout="RowLayout" ColumnWidth=".4" HideBorders="true" Cls="bgimage">
                                            <Items>
                                                <ext:Image runat="server" ImageUrl="../image/SelectImg.png" ID="imgShow" ColumnWidth="1" RowHeight=".9">
                                                    <Listeners>
                                                        <Click Fn="Show" />
                                                    </Listeners>
                                                </ext:Image>
                                                <ext:FileUploadField ID="ImgFile" runat="server" ButtonOnly="true" RowHeight=".1" ButtonText='<%# GetLangStr("FeatureQuery6","选择") %>' Hidden="true">
                                                    <DirectEvents>
                                                        <FileSelected OnEvent="FileUploadSelect">
                                                            <EventMask ShowMask="true" Msg='<%# GetLangStr("FeatureQuery7","上传中") %>' />
                                                        </FileSelected>
                                                    </DirectEvents>
                                                </ext:FileUploadField>
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" HideBorders="true" Width="20"></ext:Panel>
                                        <%--按钮--%>
                                        <ext:Panel runat="server" Width="100" Layout="RowLayout" HideBorders="true">
                                            <Items>
                                                <ext:Button ID="btnQuery" Width="90" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("FeatureQuery7","查询") %>'>
                                                    <DirectEvents>
                                                        <Click OnEvent="btnQuery_Event">
                                                            <EventMask ShowMask="true" Msg="识别中" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="btncutimg" ArrowAlign="Bottom" Width="90" Hidden="true" runat="server" Text='<%# GetLangStr("FeatureQuery8","截图") %>'>
                                                    <Listeners>
                                                        <Click Fn="CutImg" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" HideBorders="true" Width="20"></ext:Panel>
                                        <%--截图展示--%>
                                        <ext:Panel runat="server" ColumnWidth=".45" Layout="RowLayout" HideBorders="true" ID="panelCut" Hidden="true">
                                            <Items>
                                                <ext:Panel runat="server" Layout="ColumnLayout" RowHeight=".8">
                                                    <Items>
                                                        <ext:Panel runat="server" ColumnWidth=".5" Layout="RowLayout" Height="310" HideBorders="true">
                                                            <Items>
                                                                <ext:Image runat="server" ID="imgCut" ColumnWidth="1" RowHeight=".8" Width="100%" Height="100%">
                                                                </ext:Image>
                                                            </Items>
                                                        </ext:Panel>
                                                        <ext:Panel runat="server" HideBorders="true" Width="10"></ext:Panel>
                                                        <ext:Panel runat="server" ColumnWidth=".4" Layout="RowLayout" HideBorders="true">
                                                            <Content>
                                                                <div>
                                                                    <input id="T" value="0" runat="server" />
                                                                    <input id="L" value="0" runat="server" />
                                                                    <input id="W" runat="server" />
                                                                    <input id="H" runat="server" />
                                                                </div>
                                                            </Content>
                                                        </ext:Panel>
                                                    </Items>
                                                </ext:Panel>
                                            </Items>
                                            <BottomBar>
                                                <ext:Toolbar runat="server">
                                                    <Items>
                                                        <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                                                        <ext:Button ID="btnCutQuery" Width="100" runat="server" Text='<%# GetLangStr("FeatureQuery9","特征查询") %>'>
                                                            <DirectEvents>
                                                                <Click OnEvent="btnCutQuery_Event">
                                                                    <EventMask ShowMask="true" Msg='<%# GetLangStr("FeatureQuery10","识别中") %>' AutoDataBind="true" />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </BottomBar>
                                        </ext:Panel>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel runat="server" HideBorders="true" Height="10"></ext:Panel>
                                <%--过车数据展示--%>
                                <ext:Panel runat="server" Layout="ColumnLayout" RowHeight=".5" HideBorders="true" AutoScroll="true">
                                    <TopBar>
                                        <ext:Toolbar runat="server">
                                            <Items>
                                                <ext:Label ID="lblShow" runat="server"></ext:Label>
                                                <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                                                <ext:Button runat="server" Text="<" Width="100px">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnLast_Event">
                                                            <EventMask ShowMask="true" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button runat="server" Text=">" Width="100px">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnNext_Event">
                                                            <EventMask ShowMask="true" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <Items>
                                        <ext:Panel runat="server" ColumnWidth=".24" Padding="2" Border="false" Layout="RowLayout">
                                            <Items>
                                                <ext:Image ID="imgzjwj1" runat="server" ImageUrl="../images/NoImage.png" RowHeight=".7">
                                                    <Listeners>
                                                        <Click Handler="OpenPicModelPage(document.getElementById('imgzjwj1').src);" />
                                                    </Listeners>
                                                </ext:Image>
                                                <ext:Panel runat="server" Border="false" RowHeight=".3" Layout="Absolute">
                                                    <Items>
                                                        <ext:Label ID="labgwsj1" runat="server" X="100" Y="10" Text='<%# GetLangStr("FeatureQuery11","通过时间：") %>' StyleSpec="margin-left:10px;" />
                                                        <ext:Label ID="labkkid1" runat="server" X="100" Y="40" Text='<%# GetLangStr("FeatureQuery12","卡口名称：") %>' StyleSpec="margin-left:10px;" />
                                                        <ext:Label ID="labhphm1" runat="server" X="100" Y="70" Text='<%# GetLangStr("FeatureQuery13","号牌号码：") %>' StyleSpec="margin-left:10px;" />
                                                    </Items>
                                                </ext:Panel>
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" ColumnWidth=".24" Padding="2" Layout="RowLayout" Border="false">
                                            <Items>
                                                <ext:Image ID="imgzjwj2" runat="server" ImageUrl="../images/NoImage.png" RowHeight=".7">
                                                    <Listeners>
                                                        <Click Handler="OpenPicModelPage(document.getElementById('imgzjwj2').src);" />
                                                    </Listeners>
                                                </ext:Image>
                                                <ext:Panel runat="server" Border="false" RowHeight=".3" Layout="Absolute">
                                                    <Items>
                                                        <ext:Label ID="labgwsj2" runat="server" X="100" Y="10" Text='<%# GetLangStr("FeatureQuery14","通过时间：") %>' StyleSpec="margin-left:10px;" />
                                                        <ext:Label ID="labkkid2" runat="server" X="100" Y="40" Text='<%# GetLangStr("FeatureQuery15","卡口名称：") %>' StyleSpec="margin-left:10px;" />
                                                        <ext:Label ID="labhphm2" runat="server" X="100" Y="70" Text='<%# GetLangStr("FeatureQuery16","号牌号码：") %>' StyleSpec="margin-left:10px;" />
                                                    </Items>
                                                </ext:Panel>
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" ColumnWidth=".24" Padding="2" Layout="RowLayout" Border="false">
                                            <Items>
                                                <ext:Image ID="imgzjwj3" runat="server" ImageUrl="../images/NoImage.png" RowHeight=".7">
                                                    <Listeners>
                                                        <Click Handler="OpenPicModelPage(document.getElementById('imgzjwj3').src);" />
                                                    </Listeners>
                                                </ext:Image>
                                                <ext:Panel runat="server" Border="false" RowHeight=".3" Layout="Absolute">
                                                    <Items>
                                                        <ext:Label ID="labgwsj3" runat="server" X="100" Y="10" Text='<%# GetLangStr("FeatureQuery17","通过时间：") %>' StyleSpec="margin-left:10px;" />
                                                        <ext:Label ID="labkkid3" runat="server" X="100" Y="40" Text='<%# GetLangStr("FeatureQuery18","卡口名称：") %>' StyleSpec="margin-left:10px;" />
                                                        <ext:Label ID="labhphm3" runat="server" X="100" Y="70" Text='<%# GetLangStr("FeatureQuery19","号牌号码：") %>' StyleSpec="margin-left:10px;" />
                                                    </Items>
                                                </ext:Panel>
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" ColumnWidth=".24" Padding="2" Layout="RowLayout" Border="false">
                                            <Items>
                                                <ext:Image ID="imgzjwj4" runat="server" ImageUrl="../images/NoImage.png" RowHeight=".7">
                                                    <Listeners>
                                                        <Click Handler="OpenPicModelPage(document.getElementById('imgzjwj4').src);" />
                                                    </Listeners>
                                                </ext:Image>
                                                <ext:Panel runat="server" Border="false" RowHeight=".3" Layout="Absolute">
                                                    <Items>
                                                        <ext:Label ID="labgwsj4" runat="server" X="100" Y="10" Text='<%# GetLangStr("FeatureQuery20","通过时间：") %>' StyleSpec="margin-left:10px;" />
                                                        <ext:Label ID="labkkid4" runat="server" X="100" Y="40" Text='<%# GetLangStr("FeatureQuery21","卡口名称：") %>' StyleSpec="margin-left:10px;" />
                                                        <ext:Label ID="labhphm4" runat="server" X="100" Y="70" Text='<%# GetLangStr("FeatureQuery22","号牌号码：") %>' StyleSpec="margin-left:10px;" />
                                                    </Items>
                                                </ext:Panel>
                                            </Items>
                                        </ext:Panel>
                                    </Items>
                                </ext:Panel>
                            </Items>
                        </ext:Panel>
                        <ext:Panel runat="server" ColumnWidth=".01" HideBorders="true" />
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
            FeatureQuery.GetDateTime(true, tt);
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
            FeatureQuery.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>