<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeccancyTypeSetting.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.PeccancyTypeSetting" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>卡口违法行为配置</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <script type="text/javascript">
        var isuseRenderer = function (value) {
            if (!Ext.isEmpty(value)) {
                return value;
            }

            return value;
        };
        var pecctypeRenderer = function (value) {
            if (!Ext.isEmpty(value)) {
                return value;
            }

            return value;
        };
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridDevice.view.findRowIndex(this.triggerElement),
                cellIndex = GridDevice.view.findCellIndex(this.triggerElement),
                record = StorePecType.getAt(rowIndex),
                fieldName = GridDevice.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PeccancyTypeSetting" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Store ID="StorePeccancyType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreUseType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="ViewPort1" runat="server">
            <Items>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <Center Split="true" MarginsSummary="0 5 5 5">
                        <ext:Panel ID="pnlSouth" runat="server" Frame="true" Height="250" Layout="Fit">
                            <Items>
                                <ext:GridPanel ID="GridDevice" runat="server" Border="false" StripeRows="true">
                                    <Store>
                                        <ext:Store ID="StorePecType" runat="server" OnRefreshData="StorePeccancy_Refresh">
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
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>

                                    <Plugins>
                                        <ext:RowEditor ID="RowEditor1" runat="server" SaveText='<%# GetLangStr("PeccancyTypeSetting1","更新") %>' CancelText='<%# GetLangStr("PeccancyTypeSetting2","退出") %>'>
                                            <DirectEvents>
                                                <AfterEdit OnEvent="UpdatePeccancyData" />
                                            </DirectEvents>
                                        </ext:RowEditor>
                                    </Plugins>
                                    <View>
                                        <ext:GridView ID="GridView1" runat="server" ForceFit="true" MarkDirty="false" />
                                    </View>
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                                            <ext:Column Width="80" DataIndex="col0" Header='<%# GetLangStr("PeccancyTypeSetting3","编号") %>' AutoDataBind="true" Hidden="true" />
                                            <ext:Column Header='<%# GetLangStr("PeccancyTypeSetting4","超速比例") %>' AutoDataBind="true" DataIndex="col1" Width="60" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("PeccancyTypeSetting5","超速名称") %>' AutoDataBind="true" DataIndex="col2" Width="360" Align="Left" />
                                            <ext:Column Header='<%# GetLangStr("PeccancyTypeSetting6","车辆限速") %>' AutoDataBind="true" DataIndex="col3" Width="60" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("PeccancyTypeSetting7","号牌种类") %>' AutoDataBind="true" DataIndex="col4" Width="80" Align="Center" />
                                            <ext:Column Header='<%# GetLangStr("PeccancyTypeSetting9","违法代码") %>' AutoDataBind="true" DataIndex="col5" Width="60" Align="Center" />
                                            <ext:Column DataIndex="col6" Header='<%# GetLangStr("PeccancyTypeSetting10","违法行为") %>' AutoDataBind="true" Width="400" Align="Left">
                                                <Renderer Fn="pecctypeRenderer" />
                                                <Editor>
                                                    <ext:ComboBox ID="CmbPeccancyType" runat="server" Shadow="Drop" Mode="Local" TriggerAction="All"
                                                        ForceSelection="true" StoreID="StorePeccancyType" DisplayField="col2" ValueField="col1"
                                                        SelectOnFocus="true">
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col8" Header='<%# GetLangStr("PeccancyTypeSetting11","是否使用") %>' AutoDataBind="true" Width="80" Align="Center">
                                                <Renderer Fn="isuseRenderer" />
                                                <Editor>
                                                    <ext:ComboBox ID="CmbIsUse" runat="server" Shadow="Drop" Mode="Local" TriggerAction="All"
                                                        ForceSelection="true" StoreID="StoreUseType" DisplayField="col1" ValueField="col0"
                                                        SelectOnFocus="true">
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" SingleSelect="true" />
                                    </SelectionModel>

                                    <ToolTips>
                                        <ext:ToolTip
                                            ID="RowTip"
                                            runat="server"
                                            Target="={GridDevice.getView().mainBody}"
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
        </ext:Viewport>
    </form>
</body>
</html>