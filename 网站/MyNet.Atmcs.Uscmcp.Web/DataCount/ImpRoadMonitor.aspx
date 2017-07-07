<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpRoadMonitor.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.ImpRoadMonitor" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>重点道路分析图</title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
        <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../Map/css/index.css" />
    <link rel="stylesheet" type="text/css" href="../Map/css/jquery.dad.css" />
    <link rel="stylesheet" type="text/css" href="../Map/css/custom.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/jquery.mCustomScrollbar.css" />
     <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.10.4.min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.mousewheel.min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.mCustomScrollbar.js" charset="UTF-8"></script>
    <style type="text/css">
        .progress-bar li {
            height: 50px;
        }

        .progress-bar .pro-bar {
            position: relative;
            padding-top: 2px !important;
        }

            .progress-bar .pro-bar .pro-bg {
                position: relative;
                height: 20px;
                border-radius: 16px;
                width: 95%;
                overflow: hidden;
                display: block;
                background-color: #b5c6db;
            }

                .progress-bar .pro-bar .pro-bg .pro-step {
                    position: absolute;
                    top: 0;
                    left: 0;
                    width: 0;
                    height: 20px;
                    border-radius: 16px;
                    background-color: red;
                    display: block;
                }

        .progress-bar .pro-persent {
            padding-left: 10px;
        }

        .yellow-bg, .new-layout .orange-bg, .new-layout .green-bg, .new-layout .blue-bg {
            color: #fff !important;
            -webkit-transition: all 0.4s linear;
            -o-transition: all 0.4s linear;
            -moz-transition: all 0.4s linear;
            -ms-transition: all 0.4s linear;
            transition: all 0.4s linear;
        }

        .yellow-bg {
            background-color: #fbad04 !important;
        }

        .orange-bg {
            background-color: #fb5004 !important;
        }

        .green-bg {
            background-color: #0be41f !important;
        }

        .blue-bg {
            background-color: #1b5ed8 !important;
        }

        .write-bg {
            background-color: #fff !important;
        }

        .word-nowrap {
            white-space: nowrap !important;
        }

        .yellow-color {
            color: #fb5004;
        }

        .x-grid3-row {
            height: 38px;
        }
    </style>
    <script type="text/javascript" language="javascript" src="../Map/js/jquery-1.7.min.js"></script>
    <script type="text/javascript" language="javascript" src="../Map/js/jquery.dad.min.js"></script>
    <script type="text/javascript" language="javascript" src="../Map/js/Qquery1.91-min.js"></script>
    <script type="text/javascript" language="javascript" src="../Map/js/jquery.nicescroll.js"></script>
    <script type="text/javascript">
        var DataState = function (value) {
            var color = "green-bg"
            if (value < 30) {
                color = "green-bg";
            }
            if (value > 30) {
                color = "yellow-bg";
            }
            if (value > 70) {
                color = "orange_bg";
            }
            return " <ul class=\"progress-bar\"><div class=\"pro-bar\"><span class=\"pro-bg\"><i class=\"pro-step " + color + "\" style=\"width:" + value + "%;\"></i></span></div></ul>";
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="ImpRoadMonitor" />
        <div class="containerbox1">
            <ext:Viewport ID="Viewport1" runat="server" Layout="FitLayout">
                <Items>
                    <ext:GridPanel ID="GridFlow" runat="server" StripeRows="true" Title='<%# GetLangStr("ImpRoadMonitor1","重点道路分析图")%>' Layout="FitLayout" AutoScroll="true" MinColumnWidth="110">
                        <Store>
                            <ext:Store ID="StoreFlow" runat="server">
                                <AutoLoadParams>
                                    <ext:Parameter Name="start" Value="={0}" />
                                    <ext:Parameter Name="limit" Value="={15}" />
                                </AutoLoadParams>
                                <UpdateProxy>
                                    <ext:HttpWriteProxy Method="GET" Url="ImpRoadMonitor.aspx">
                                    </ext:HttpWriteProxy>
                                </UpdateProxy>
                                <Reader>
                                    <ext:JsonReader>
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
                                            <ext:RecordField Name="col20" />
                                            <ext:RecordField Name="col21" />
                                            <ext:RecordField Name="col22" />
                                            <ext:RecordField Name="dlid" />
                                            <ext:RecordField Name="dlmc" />
                                            <ext:RecordField Name="zs" />
                                            <ext:RecordField Name="gwbl" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <TopBar>
                            <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="15" StoreID="StoreFlow">
                            </ext:PagingToolbar>
                        </TopBar>
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                                <ext:Column Header='<%# GetLangStr("ImpRoadMonitor2","道路名称")%>'  AutoDataBind="true" Width="300" Fixed="true" DataIndex="dlmc" Resizable="false">
                                </ext:Column>
                                <ext:Column Header='<%# GetLangStr("ImpRoadMonitor3","图例")%>'  AutoDataBind="true" Width="250" Align="Center" Sortable="true" EmptyGroupText="0" DataIndex="gwbl">
                                    <Renderer Fn="DataState" />
                                </ext:Column>
                                <ext:Column Header='<%# GetLangStr("ImpRoadMonitor4","过车总数")%>'  AutoDataBind="true"  Width="240" Align="Center" Sortable="true" EmptyGroupText="0" DataIndex="zs">
                                </ext:Column>
                                <ext:Column Header='<%#  GetLangStr("ImpRoadMonitor5","历史均值")%>'  AutoDataBind="true"  Width="220" Align="Center" EmptyGroupText="0" Sortable="true" DataIndex="zs">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server" SingleSelect="true"></ext:RowSelectionModel>
                        </SelectionModel>
                        <View>
                            <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                            </ext:GridView>
                        </View>
                    </ext:GridPanel>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>