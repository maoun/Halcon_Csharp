<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TGSUserManager.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.TGSUserManager" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户信息管理</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <script type="text/javascript">
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridUser.view.findRowIndex(this.triggerElement),
                cellIndex = GridUser.view.findCellIndex(this.triggerElement),
                record = StoreUser.getAt(rowIndex),
                fieldName = GridUser.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);
            if (fieldName == "col28") {
                data = data.toString().substring(0, 10) + " " + data.toString().substring(11, 19);
            }
            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="UserManager" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Hidden ID="RadioRoleId" runat="server" />
        <ext:Hidden ID="hidNowUsername" runat="server" />
        <ext:Store ID="StoreSex" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreRole" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreIdtype" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreDepart" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="DEPARTID" />
                        <ext:RecordField Name="DEPARTNAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <ext:FormPanel ID="Panel1" Region="North" runat="server" Height="40">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar4" runat="server">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Text='<%# GetLangStr("TGSUserManager1","用户编号：") %>' StyleSpec="margin-left: 10px;">
                                </ext:Label>
                                <ext:TextField ID="TxtUserId" runat="server" Width="100" />
                                <ext:Label ID="Label1" runat="server" Text='<%# GetLangStr("TGSUserManager2","用户名：") %>' StyleSpec="margin-left: 10px;">
                                </ext:Label>
                                <ext:TextField ID="TxtUserName" runat="server" Width="100" />
                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("TGSUserManager3","查询")%>'>
                                    <DirectEvents>
                                        <Click OnEvent="TbutQueryClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("TGSUserManager4","重置")%>'>
                                    <DirectEvents>
                                        <Click OnEvent="ButResetClick" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarFill />
                                <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
                                <ext:Button ID="ButUserAdd" runat="server" Text='<%# GetLangStr("TGSUserManager5","注册新用户")%>' Icon="UserAdd">
                                    <DirectEvents>
                                        <Click OnEvent="ButUserAdd_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButUserModify" runat="server" Text='<%# GetLangStr("TGSUserManager6","修改密码")%>' Icon="UserEdit">
                                    <DirectEvents>
                                        <Click OnEvent="ButUserModify_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="ButUserDelete" runat="server" Text='<%# GetLangStr("TGSUserManager7","删除用户")%>' Icon="UserDelete">
                                    <DirectEvents>
                                        <Click OnEvent="ButUserDelete_Click" />
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
                <ext:Panel Region="Center" runat="server" Layout="FitLayout">
                    <Items>
                        <ext:GridPanel ID="GridUser" StripeRows="true" runat="server" >
                            <Store>
                                <ext:Store ID="StoreUser" runat="server" OnRefreshData="MyData_Refresh">
                                    <Reader>
                                        <ext:JsonReader IDProperty="col1">
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
                                                <ext:RecordField Name="col25" Type="String" />
                                                <ext:RecordField Name="col26" Type="String" />
                                                <ext:RecordField Name="col27" Type="String" />
                                                <ext:RecordField Name="col28" Type="String" />
                                                <ext:RecordField Name="col29" Type="String" />
                                                <ext:RecordField Name="col30" Type="String" />
                                                <ext:RecordField Name="col31" Type="String" />
                                                <ext:RecordField Name="col32" Type="String" />
                                                <ext:RecordField Name="col33" Type="String" />
                                                <ext:RecordField Name="col34" Type="String" />
                                                <ext:RecordField Name="col35" Type="String" />
                                                <ext:RecordField Name="col36" Type="String" />
                                                <ext:RecordField Name="col37" Type="String" />
                                                <ext:RecordField Name="col38" Type="String" />
                                                <ext:RecordField Name="col39" Type="String" />
                                                 <ext:RecordField Name="col40" Type="String" />
                                                <ext:RecordField Name="col41" Type="String" />
                                                <ext:RecordField Name="col42" Type="String" />
                                                <ext:RecordField Name="col43" Type="String" />
                                                <ext:RecordField Name="col44" Type="String" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40" />
                                    <ext:Column Header='<%# GetLangStr("TGSUserManager9","用户编号")%>' AutoDataBind="true" DataIndex="col1" Width="80" Align="Center" Hidden="true" />
                                    <ext:Column Header='<%# GetLangStr("TGSUserManager10","用户名")%>' AutoDataBind="true" DataIndex="col27" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("TGSUserManager11","姓名")%>' AutoDataBind="true" DataIndex="col2" Width="80" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("TGSUserManager12","警号")%>' AutoDataBind="true" DataIndex="col21" Width="80" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("TGSUserManager13","所属机构")%>' AutoDataBind="true" DataIndex="col29" Width="180" />
                                    <ext:DateColumn Header='<%# GetLangStr("TGSUserManager14","注册时间")%>' AutoDataBind="true" DataIndex="col28" Width="150" Format="yyyy-MM-dd HH:mm:ss" Align="Center" />
                                     <ext:Column Header='<%# GetLangStr("TGSUserManager37","用户角色")%>' AutoDataBind="true" DataIndex="col43" Width="80" Align="Center"  />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    <DirectEvents>
                                        <RowSelect OnEvent="SelectUser" Buffer="250">
                                            <ExtraParams>
                                                <ext:Parameter Name="sdata" Value="record.data" Mode="Raw" />
                                            </ExtraParams>
                                        </RowSelect>
                                    </DirectEvents>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <TopBar>
                                <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1"
                                    runat="server" PageSize="15" StoreID="StoreUser">
                                </ext:PagingToolbar>
                            </TopBar>
                            <View>
                                <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                                </ext:GridView>
                            </View>
                            <ToolTips>
                                <ext:ToolTip
                                    ID="RowTip"
                                    runat="server"
                                    Target="={GridUser.getView().mainBody}"
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
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Frame="true"
                    Title='<%# GetLangStr("PeccancyQuery16","详细信息")%>' Width="410" Icon="User" AutoScroll="true">
                    <Items>
                        <ext:Panel ID="Panel2" runat="server" Title='<%# GetLangStr("TGSUserManager17","所属角色")%>' Padding="5"
                            Header="true" Width="380">
                            <Items>
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("TGSUserManager18","所属角色")%>' ID="cmbRole" StoreID="StoreRole"
                                    Editable="false" DisplayField="col2" ValueField="col1" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("TGSUserManager19","选择所属角色")%>' Width="370" AllowBlank="false" />
                            </Items>
                        </ext:Panel>
                        <ext:Panel ID="Panel19" runat="server" Title='<%# GetLangStr("TGSUserManager20","用户信息")%>' Padding="5" Header="true" Height="450" Width="380">
                            <Items>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("TGSUserManager21","用户编号") %>' ID="TxtID" Width="370" Disabled="true" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("TGSUserManager22","姓名") %>' ID="TxtName" Width="370" AllowBlank="false" />
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("TGSUserManager23","性别") %>' ID="CmbSex" StoreID="StoreSex" Editable="false"
                                    DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All" EmptyText='<%# GetLangStr("TGSUserManager24","选择性别")%>'
                                    Width="370" AllowBlank="false" />
                                <ext:DateField runat="server" FieldLabel='<%# GetLangStr("TGSUserManager25","出生年月") %>' ID="DateBirday" Vtype="daterange" Format="yyyy-MM-dd"
                                    Width="370">
                                </ext:DateField>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("TGSUserManager26","警号") %>' ID="TxtPolice" Width="370" AllowBlank="false" />
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("TGSUserManager27","证件类型") %>' ID="CmbIdType" StoreID="StoreIdType"
                                    Editable="false" DisplayField="col1" ValueField="col0" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("TGSUserManager28","选择证件类型")%>' Width="370" />
                                <%-- 前面都有这个属性：StyleSpec="margin-left: 10px;"--%>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("TGSUserManager29","证件号码")%>' ID="TxtIdNo" Width="370" />

                             
                           <ext:TextField runat="server" FieldLabel='<%# GetLangStr("TGSUserManager30","联系地址")%>' ID="TxtAddress" Width="370" />
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("TGSUserManager31","所属机构")%>' ID="CmbDerpart" StoreID="StoreDepart"
                                    Editable="false" DisplayField="DEPARTNAME" ValueField="DEPARTID" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("TGSUserManager32","选择所属机构")%>' Width="370" AllowBlank="false" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("TGSUserManager33","移动手机")%>' ID="TxtMobilePhone" Width="370" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("TGSUserManager34","固定电话")%>' ID="TxtPhone" Width="370" />
                                <ext:TextArea runat="server" FieldLabel='<%# GetLangStr("TGSUserManager35","备注信息")%>' ID="TxtBz" Width="370" Height="30" />
                            </Items>
                        </ext:Panel>
                    </Items>
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button ID="ButUpdate" runat="server" Text='<%# GetLangStr("TGSUserManager36","保存")%>' Icon="TableSave">
                                    <Listeners>
                                        <Click Handler="UserManager.UpdatePerson()" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>