<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportantRule.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.ImportantRule" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%# GetLangStr("ImportantRule1","重点车辆监管设置") %></title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../../../../resources/css/examples.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>
    <script type="text/javascript">
        var CountrySelector = {
            add: function (source, destination) {
                source = source || GQueryKkmc;
                destination = destination || GBindKkmc;

                if (source.hasSelection()) {
                    var records = source.selModel.getSelections();
                    source.deleteSelected();
                    destination.store.add(records);

                    //ImportantRule.AddBindKkmc(arrayObj1);
                }
            },
            remove: function (source, destination) {
                this.add(destination, source);
                //var records = destination.selModel.getSelections();
                //for (var i = 0; i < records.length; i++) {
                //    arrayObj2[i] = GBindKkmc.getStore().data.items[i].data.col1;
                //}
                //ImportantRule.DelteBindKkmc(arrayObj2);
            },
            save: function () {
                var arrayObj3 = new Array();
                var j = GBindKkmc.getStore().data.length;
                for (i = 0; i < j; i++) {
                    arrayObj3[i] = GBindKkmc.getStore().data.items[i].data.col1;
                }
                ImportantRule.insertMessage(arrayObj3);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="ImportantRule" />
            <ext:Viewport runat="server">
                <Items>
                    <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true" FitHeight="true">
                        <Columns>
                            <ext:LayoutColumn ColumnWidth="0.4">
                                <ext:GridPanel ID="GQueryKkmc" runat="server">
                                    <TopBar>
                                        <ext:Toolbar ID="Toolbar1" runat="server" Layout="ColumnLayout">
                                            <Items>
                                                <ext:TextField ID="TxtStationName" FieldLabel='<%# GetLangStr("ImportantRule2","卡口名称") %>' runat="server" ColumnWidth="0.7" />
                                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" ColumnWidth="0.1" Text='<%# GetLangStr("ImportantRule3","查询") %>'>
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutQueryClick">
                                                            <EventMask ShowMask="true" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Label ID="Label4" runat="server" ColumnWidth="0.2" />
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <Store>
                                        <ext:Store
                                            ID="Skkmc"
                                            runat="server">
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
                                    <ColumnModel runat="server" ID="ColumnModel1">
                                        <Columns>
                                            <ext:Column Header='<%# GetLangStr("ImportantRule4","道路编号") %>' AutoDataBind="true" DataIndex="col1" Width="100" Align="Center" Hidden="true" />
                                            <ext:Column Header='<%# GetLangStr("ImportantRule5","卡口名称") %>' AutoDataBind="true" DataIndex="col2" Width="200" Align="Center" />
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:CheckboxSelectionModel runat="server" />
                                    </SelectionModel>
                                    <View>
                                        <ext:GridView ID="GridView2" runat="server" ForceFit="true">
                                        </ext:GridView>
                                    </View>
                                </ext:GridPanel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn ColumnWidth="0.2">
                                <ext:Panel runat="server" Layout="RowLayout" HideBorders="true">
                                    <Items>
                                        <ext:Panel runat="server" RowHeight=".42" />
                                        <ext:Button ID="BAdd" runat="server" Text='<%# GetLangStr("ImportantRule6","添加") %>' RowHeight=".05">
                                            <Listeners>
                                                <Click Handler="CountrySelector.add();" />
                                            </Listeners>
                                            <ToolTips>
                                                <ext:ToolTip ID="ToolTip1" runat="server" Title='<%# GetLangStr("ImportantRule7","添加") %>' Html='<%# GetLangStr("ImportantRule8","添加选择的卡口") %>' />
                                            </ToolTips>
                                        </ext:Button>
                                        <ext:Button ID="BDelete" runat="server" Text='<%# GetLangStr("ImportantRule9","删除") %>' RowHeight=".05">
                                            <Listeners>
                                                <Click Handler="CountrySelector.remove(GQueryKkmc, GBindKkmc);" />
                                            </Listeners>
                                            <ToolTips>
                                                <ext:ToolTip ID="ToolTip3" runat="server" Title='<%# GetLangStr("ImportantRule10","移除") %>' Html='<%# GetLangStr("ImportantRule11","移除选中的限行地点") %>' />
                                            </ToolTips>
                                        </ext:Button>
                                        <ext:Button ID="BSave" runat="server" Text='<%# GetLangStr("ImportantRule12","保存") %>' RowHeight=".05">
                                            <Listeners>
                                                <Click Handler="CountrySelector.save();" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Panel runat="server" RowHeight=".42" />
                                    </Items>
                                </ext:Panel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn ColumnWidth="0.4">
                                <ext:GridPanel ID="GBindKkmc" Title='<%# GetLangStr("ImportantRule13","限行地点") %>' runat="server">
                                    <Store>
                                        <ext:Store
                                            ID="Skkmc2"
                                            runat="server">
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
                                    <ColumnModel runat="server" ID="ColumnModel2">
                                        <Columns>
                                            <ext:Column Header='<%# GetLangStr("ImportantRule14","道路编号") %>' AutoDataBind="true" DataIndex="col1" Width="100" Align="Center" Hidden="true" />
                                            <ext:Column Header='<%# GetLangStr("ImportantRule15","卡口名称") %>' AutoDataBind="true" DataIndex="col2" Width="200" Align="Center" />
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:CheckboxSelectionModel runat="server" />
                                    </SelectionModel>
                                    <View>
                                        <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                                        </ext:GridView>
                                    </View>
                                </ext:GridPanel>
                            </ext:LayoutColumn>
                        </Columns>
                    </ext:ColumnLayout>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>