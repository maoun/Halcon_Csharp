<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeccancyType.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.PeccancyType" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>违法类型管理</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <script type="text/javascript">
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridPecType.view.findRowIndex(this.triggerElement),
                cellIndex = GridPecType.view.findCellIndex(this.triggerElement),
                record = StorePecType.getAt(rowIndex),
                fieldName = GridPecType.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PeccancyType" />
        <ext:Hidden ID="realCount" runat="server" />
        <ext:Hidden ID="curpage" runat="server" />
        <ext:Hidden ID="allPage" runat="server" />
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
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <%--上方--%>
                <ext:FormPanel ID="Panel1" Region="North" runat="server" Height="40" Layout="ContainerLayout">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("PeccancyType1","违法代码：") %>' StyleSpec="margin-left: 10px; ">
                                </ext:Label>
                                <ext:TextField ID="TxtPecCode" runat="server" Width="100" />
                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("PeccancyType2","违法行为：") %>' StyleSpec="margin-left: 10px; ">
                                </ext:Label>
                                <ext:TextField ID="TxtPecType" runat="server" Width="100" />
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("PeccancyType3","查询") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" Timeout="60000">
                                            <EventMask ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("PeccancyType4","重置") %>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <%--中部--%>
                <ext:FormPanel ID="FormPanel2" Region="Center" runat="server" Layout="FitLayout" AutoScroll="true">
                    <TopBar>
                        <ext:Toolbar runat="server" Layout="Container">
                            <Items>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10" />
                                        <ext:Button ID="ButFisrt" runat="server" StyleSpec="margin-left:10px;" Text='<%# GetLangStr("PeccancyType18","首页") %>' Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="TbutFisrt" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButLast" runat="server" StyleSpec="margin-left:10px;" Icon="ControlRewindBlue" Text='<%# GetLangStr("PeccancyType19","上一页") %>' Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="TbutLast" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButNext" runat="server" StyleSpec="margin-left:10px;" Icon="ControlFastforwardBlue" Text='<%# GetLangStr("PeccancyType23","下一页") %>'
                                            Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="TbutNext" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="ButEnd" runat="server" StyleSpec="margin-left:10px;" Text='<%# GetLangStr("PeccancyType35","尾页") %>' Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="TbutEnd" />
                                            </DirectEvents>
                                        </ext:Button>
                                        <ext:Label ID="lblTitle" runat="server" Text='<%# GetLangStr("PeccancyType36","查询结果：当前是第") %>' StyleSpec="margin-left:10px;" />
                                        <ext:Label ID="lblCurpage" runat="server" Text="" Cls="pageNumLabel" />
                                        <ext:Label ID="Label2" runat="server" Text='<%# GetLangStr("PeccancyType37","页,共有") %>' />
                                        <ext:Label ID="lblAllpage" runat="server" Text="" Cls="pageNumLabel" />
                                        <ext:Label ID="Label9" runat="server" Text='<%# GetLangStr("PeccancyType37","页,共有") %>'  StyleSpec="font-weight:bolder;" />
                                        <ext:Label ID="lblRealcount" runat="server" Text="" Cls="pageNumLabel" />
                                        <ext:Label ID="Label12" runat="server" Text='<%# GetLangStr("PeccancyType38","条记录") %>'  />
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridPecType" runat="server" StripeRows="true" TrackMouseOver="true">
                            <Store>
                                <ext:Store ID="StorePecType" runat="server" OnRefreshData="MyData_Refresh">
                                    <AutoLoadParams>
                                        <ext:Parameter Name="start" Value="={0}" />
                                        <ext:Parameter Name="limit" Value="={15}" />
                                    </AutoLoadParams>
                                    <UpdateProxy>
                                        <ext:HttpWriteProxy Method="GET" Url="PeccancyType.aspx">
                                        </ext:HttpWriteProxy>
                                    </UpdateProxy>
                                    <Reader>
                                        <ext:JsonReader IDProperty="col0">
                                            <Fields>
                                                <ext:RecordField Name="col0" Type="String" />
                                                <ext:RecordField Name="col1" Type="String" />
                                                <ext:RecordField Name="col2" Type="String" />
                                                <ext:RecordField Name="col3" Type="String" />
                                                <ext:RecordField Name="col4" Type="String" />
                                                <ext:RecordField Name="col5" Type="String" />
                                                <ext:RecordField Name="col6" Type="String" />
                                                <ext:RecordField Name="col7" Type="String" />
                                                <ext:RecordField Name="col8" Type="String" />
                                                <ext:RecordField Name="col9" Type="String" />
                                                <ext:RecordField Name="col10" Type="String" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                                    <ext:Column Header='<%# GetLangStr("PeccancyType7","违法代码") %>' AutoDataBind="true" DataIndex="col1" Width="60" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("PeccancyType8","违法行为") %>' AutoDataBind="true" DataIndex="col3" Width="180" />
                                    <ext:Column Header='<%# GetLangStr("PeccancyType9","处罚依据") %>' AutoDataBind="true" DataIndex="col4" Width="220" />
                                    <ext:Column Header='<%# GetLangStr("PeccancyType10","违法扣分") %>' AutoDataBind="true" DataIndex="col5" Width="80" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("PeccancyType11","罚款金额") %>' AutoDataBind="true" DataIndex="col6" Width="80" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("PeccancyType12","是否使用") %>' AutoDataBind="true" DataIndex="col10" Width="80" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    <DirectEvents>
                                        <RowSelect OnEvent="SelectPecType" Buffer="250">
                                            <ExtraParams>
                                                <ext:Parameter Name="sdata" Value="record.data" Mode="Raw" />
                                            </ExtraParams>
                                        </RowSelect>
                                    </DirectEvents>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <View>
                                <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                                </ext:GridView>
                            </View>
                            <ToolTips>
                                <ext:ToolTip
                                    ID="RowTip"
                                    runat="server"
                                    Target="={GridPecType.getView().mainBody}"
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
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Frame="true"
                    Title='<%# GetLangStr("PeccancyType13","详细信息") %>' AutoDataBind="true" Width="400" Icon="Table" DefaultAnchor="100%" MonitorValid="true">
                    <Items>
                        <ext:Panel ID="Panel9" runat="server" DefaultAnchor="100%" Title='<%# GetLangStr("PeccancyType5","违法类型") %>' AutoDataBind="true" Padding="5"
                            Header="true" AutoScroll="true">
                            <Items>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancyType15","记录编号") %>' ID="TxtID" Width="370" Disabled="true" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("PeccancyType1","违法代码") %>' ID="Txtwfbh" Width="370" AllowBlank="false"
                                    Vtype="alphanum" VtypeText='<%# GetLangStr("PeccancyType17","只能输入数字和字母") %>' />
                                <ext:TextArea runat="server" FieldLabel='<%# GetLangStr("PeccancyType2","违法行为") %>' ID="Txtwfxw" Width="370" Height="50"
                                    AllowBlank="false" />
                                <ext:TextArea runat="server" FieldLabel='<%# GetLangStr("PeccancyType9","处罚依据") %>' ID="Txtcfyj" Width="370" Height="50" />
                                <ext:TextArea runat="server" FieldLabel='<%# GetLangStr("PeccancyType22","违法全称") %>' ID="Txtwfjc" Width="370" Height="50"
                                    AllowBlank="false" />
                                <ext:NumberField runat="server" FieldLabel='<%#GetLangStr("PeccancyType10","违法扣分") %>' ID="Txtwfkf" Width="370" MinValue="0"
                                    MaxValue="12" AllowBlank="false" />
                                <ext:NumberField runat="server" FieldLabel='<%# GetLangStr("PeccancyType11","罚款金额") %>' ID="Txtfkje" Width="370" MinValue="0"
                                    MaxValue="3000" AllowBlank="false" />
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("PeccancyType12","是否使用") %>' ID="cmbUse" StoreID="StoreUseType"
                                    Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                    Width="370" />
                            </Items>
                        </ext:Panel>
                    </Items>
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button ID="ButAdd" runat="server" Text='<%# GetLangStr("PeccancyType24","增加") %>' Icon="TableAdd">
                                    <Listeners>
                                        <Click Handler="PeccancyType.InsertPecType()" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButUpdate" runat="server" Text='<%# GetLangStr("PeccancyType25","保存") %>' Icon="TableSave">
                                    <Listeners>
                                        <Click Handler="PeccancyType.UpdatePecType()" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButDelete" runat="server" Text='<%# GetLangStr("PeccancyType26","删除") %>' Icon="TableDelete">
                                    <Listeners>
                                        <Click Handler="PeccancyType.DeletePecType()" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Listeners>
                        <ClientValidation Handler="ButUpdate.setDisabled(!valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>