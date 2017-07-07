<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComplexQuery.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.ComplexQuery" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>车辆综合信息</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <script type="text/javascript" language="javascript" src="../Scripts/common.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8"></script>
    <script type="text/javascript" language="javascript">
        function ShowPage(type) {
            ComplexQuery.ShowEvent(type);
        }
        var change = function (obj, value) {
            obj.setValue(value.toUpperCase());
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="ComplexQuery" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="ColumnLayout" HideBorders="true">
            <Items>
                <ext:Panel runat="server" ColumnWidth=".03" HideBorders="true" />
                <ext:Panel runat="server" ColumnWidth=".96" HideBorders="true" Layout="RowLayout">
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Label ID="Label13" runat="server" Text='号牌号码：' StyleSpec="margin-left: 10px;float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                </ext:Label>
                                <ext:Panel ID="Panel2" runat="server" Height="30">
                                    <Content>
                                        <veh:VehicleHead ID="WindowEditor1" runat="server" />
                                    </Content>
                                </ext:Panel>
                                <ext:TextField ID="TxtplateId" runat="server" Width="102"  EmptyText="六位号牌号码" >
                                    <Listeners>
                                        <Change Fn="change " />
                                    </Listeners>
                                </ext:TextField>
                                <ext:Label ID="Label14" runat="server" Text='号牌种类：' StyleSpec="margin-left: 10px;float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                </ext:Label>
                                <ext:ComboBox ID="CmbPlateType" runat="server" Editable="false" DisplayField="col1"
                                    ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='请选择...'
                                    SelectOnFocus="true" Width="120">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
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
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text="查询" StyleSpec="margin-left: 20px;">
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" Timeout="60000">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Style="margin-left: 20px" Icon="ControlBlank" Text="重置" Hidden="true">
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>

                                    <ext:Button ID="btnJsr"  Style="margin-left: 20px" runat="server" Text="驾驶人关联分析" Hidden="true" Icon="Group">
                                    <Listeners>
                                        <Click Handler="ShowPage('1')" />
                                    </Listeners>
                                </ext:Button>
                              
                                <ext:Button ID="btnRcgl" Style="margin-left: 20px" runat="server" Text="人车关联分析"  Icon="Car">
                                    <Listeners>
                                        <Click Handler="ShowPage('2')" />
                                    </Listeners>
                                </ext:Button>
                               
                                <ext:Button ID="btnWfcx" Style="margin-left: 20px" runat="server" Text="违法关联查询" Icon="ApplicationDouble">
                                    <Listeners>
                                        <Click Handler="ShowPage('3')" />
                                    </Listeners>
                                </ext:Button>
                              
                                <ext:Button ID="btnLjd" Style="margin-left: 20px" runat="server" Text="落脚点" Icon="UserHome">
                                    <Listeners>
                                        <Click Handler="ShowPage('4')" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:Panel runat="server" Height="150" Layout="RowLayout" HideBorders="true" Title="车驾管信息">
                            <Items>
                                <ext:Panel runat="server" HideBorders="true" Height="10"></ext:Panel>
                                <ext:Panel runat="server" HideBorders="true" Layout="ColumnLayout" Height="26">
                                    <Items>
                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                            <Items>
                                                <ext:Label ID="Label2" runat="server" Text="车辆品牌：" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                </ext:Label>
                                                <ext:TextField ID="txtClpp" runat="server" ColumnWidth=".65" Disabled="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                            <Items>
                                                <ext:Label ID="Label4" runat="server" Text="车身颜色：" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                </ext:Label>
                                                <ext:TextField ID="txtCsys" runat="server" ColumnWidth=".65" Disabled="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                            <Items>
                                                <ext:Label ID="Label5" runat="server" Text="号牌种类：" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                </ext:Label>
                                                <ext:TextField ID="txtCllx" runat="server" ColumnWidth=".65" Disabled="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                            <Items>
                                                <ext:Label ID="Label6" runat="server" Text="使用性质：" ColumnWidth=".35" StyleSpec="float: left; height: 24px; line-height: 24px!important; text-align: center">
                                                </ext:Label>
                                                <ext:TextField ID="txtSyxz" runat="server" ColumnWidth=".65" Disabled="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel runat="server" HideBorders="true" Height="10"></ext:Panel>
                                <ext:Panel runat="server" HideBorders="true" Layout="ColumnLayout" Height="26">
                                    <Items>
                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                            <Items>
                                                <ext:Label ID="Label7" runat="server" Text="车辆状态：" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                </ext:Label>
                                                <ext:TextField ID="txtClzt" runat="server" ColumnWidth=".65" Disabled="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                            <Items>
                                                <ext:Label ID="Label8" runat="server" Text="发证机关：" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                </ext:Label>
                                                <ext:TextField ID="txtFzjg" runat="server" ColumnWidth=".65" Disabled="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                            <Items>
                                                <ext:Label ID="Label9" runat="server" Text="车主姓名：" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                </ext:Label>
                                                <ext:TextField ID="txtSyr" runat="server" ColumnWidth=".65" Disabled="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                            <Items>
                                                <ext:Label ID="Label10" runat="server" Text="联系电话：" ColumnWidth=".35" StyleSpec="float: left; height: 24px; line-height: 24px!important; text-align: center">
                                                </ext:Label>
                                                <ext:TextField ID="txtLxdh" runat="server" ColumnWidth=".65" Disabled="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel runat="server" HideBorders="true" Height="10"></ext:Panel>
                                <ext:Panel runat="server" HideBorders="true" Layout="ColumnLayout" Height="26">
                                    <Items>
                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".25" Layout="ColumnLayout">
                                            <Items>
                                                <ext:Label ID="Label11" runat="server" Text="有效期止：" ColumnWidth=".35" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center">
                                                </ext:Label>
                                                <ext:TextField ID="txtYxqz" runat="server" ColumnWidth=".65" Disabled="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                        <ext:Panel runat="server" HideBorders="true" ColumnWidth=".75" Layout="ColumnLayout">
                                            <Items>
                                                <ext:Label ID="Label12" runat="server" Text="家庭住址：" ColumnWidth=".117" StyleSpec="float: left;  height: 24px; line-height: 24px!important; text-align: center" />
                                                <ext:TextField ID="txtXxdz" runat="server" ColumnWidth=".883" Disabled="true" />
                                            </Items>
                                        </ext:Panel>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel runat="server" HideBorders="true" Height="10"></ext:Panel>
                            </Items>
                        </ext:Panel>
                        <ext:Panel runat="server" HideBorders="true" Height="20"></ext:Panel>
                        <ext:Panel runat="server" RowHeight=".7" HideBorders="true" Layout="RowLayout" ID="panelChart">
                            <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
                        </ext:Panel>
                      
                        <ext:Panel runat="server" HideBorders="true" Height="20"></ext:Panel>
                    </Items>
                </ext:Panel>
                <ext:Panel runat="server" ColumnWidth=".01" HideBorders="true" />
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>