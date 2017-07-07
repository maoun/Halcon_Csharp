<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Department.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Department" %>

<%@ Register Assembly="netchartdir" Namespace="ChartDirector" TagPrefix="chart" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>组织结构管理</title>
    <meta http-equiv="Content-Type" content="text/html;charset=GBK" />
    <link href="../Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <style type="text/css">
        /*.x-grid3-header-offset {
            width: 100% !important;
        }*/
        
        /*.x-treegrid-root-table {
            width: 100% !important;
        }*/
        /*
        #ext-gen103 {
            width: 100% !important;
        }

        #ext-gen104 {
            width: 100% !important;
        }

        #ext-gen105 {
            width: 100% !important;
        }

            #ext-gen105 div {
                width: 100% !important;
            }

                #ext-gen105 div table {
                    width: 100% !important;
                }

        .x-treegrid-node-ct-table {
            width: 100% !important;
        }

        #ext-gen107 {
            width: 100% !important;
        }*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:Hidden ID="CurrentSystemId" runat="server" />
        <ext:Hidden ID="newId" runat="server"></ext:Hidden>
        <ext:Hidden ID="nowId" runat="server"></ext:Hidden>
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="Department" />
        <ext:Store ID="Storedepart" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreClass" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreSystem" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="ViewPort1" runat="server" Layout="border">
            <Items>
                <%--中间--%>
                <ext:FormPanel ID="PanelNavigate" runat="server" Region="Center" Layout="FitLayout">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server" Visible="false">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Visible="false" Text='<%# GetLangStr("Department1","系统类型") %>' StyleSpec="margin-left: 10px; ">
                                </ext:Label>
                                <ext:ComboBox runat="server" ID="CmbSystem" StoreID="StoreSystem" Visible="false"
                                    Width="250" Editable="false" DisplayField="col1" ValueField="col0" Mode="Local"
                                    TriggerAction="All" EmptyText='<%# GetLangStr("Department2","选择系统名称") %>'>
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("Department3","清除选中") %>' />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                    <DirectEvents>
                                        <Select OnEvent="cmbSystem_Select" Buffer="250">
                                            <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{TreeGrid1}" />
                                        </Select>
                                    </DirectEvents>
                                </ext:ComboBox>
                                <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                                <ext:Button ID="Button4" runat="server" Text='<%# GetLangStr("Department4","收起") %>' Icon="SectionCollapsed">
                                    <Listeners>
                                        <Click Handler="#{TreeGrid1}.collapseAll();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="Button5" runat="server" Text='<%# GetLangStr("Department5","展开") %>' Icon="SectionExpanded">
                                    <Listeners>
                                        <Click Handler="#{TreeGrid1}.expandAll();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:TreeGrid ID="TreeGrid1" runat="server" Title='<%# GetLangStr("Department6","机构关系") %>' Icon="House" NoLeafIcon="true" ColumnWidth="1"
                            EnableDD="true" UseArrows="true" Animate="true" ColumnResize="true" Layout="FitLayout">
                            <TopBar>
                            </TopBar>
                            <Columns>
                                <ext:TreeGridColumn Header='<%# GetLangStr("Department7","机构名称") %>' AutoDataBind="true" Width="430" DataIndex="col0" />
                                <%--<ext:TreeGridColumn Header="工作科室" AutoDataBind="true" Width="200" DataIndex="col1" />--%>
                                <ext:TreeGridColumn Header='<%# GetLangStr("Department8","负责人") %>' AutoDataBind="true" Width="200" DataIndex="col1" />
                                <ext:TreeGridColumn Header='<%# GetLangStr("Department9","负责人电话") %>' AutoDataBind="true" Width="200" DataIndex="col2" />
                            </Columns>
                            <Listeners>
                                <Click Handler="Department.ShowInfo(node.id)" />
                            </Listeners>
                        </ext:TreeGrid>
                    </Items>
                </ext:FormPanel>
                <%--右边--%>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Frame="true"
                    Title='<%# GetLangStr("Department10","详细信息") %>' Width="300" Icon="Table" DefaultAnchor="100%" MonitorValid="true">
                    <Items>
                        <ext:Panel ID="Panel9" Header="true" runat="server" Title='<%# GetLangStr("Department11","机构信息") %>' DefaultAnchor="100%">
                            <Items>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("Department12","记录编号") %>' ID="TxtId" Width="270" Disabled="true" Visible="false" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("Department13","机构编号") %>' Disabled="true"   ID="txtDepartId" AllowBlank="false"
                                    Vtype="alphanum" VtypeText='<%# GetLangStr("Department14","只能输入数字和编号") %>' Width="270" MaxLength="12" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("Department15","机构名称") %>' ID="txtDepartName" AllowBlank="false"
                                    Width="270" />
                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("Department16","机构级别") %>' ID="CmbClass" StoreID="StoreClass"
                                    Editable="false" DisplayField="CODEDESC" ValueField="CODE" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("Department17","选择机构级别") %>' Width="270" AllowBlank="false">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("Department18","清除选中") %>' />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();Department.GetDepart()" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>

                                <ext:ComboBox runat="server" FieldLabel='<%# GetLangStr("Department19","所属机构") %>' ID="CmbDepart" StoreID="Storedepart"
                                    Editable="false" DisplayField="col2" ValueField="col1" Mode="Local" TriggerAction="All"
                                    EmptyText='<%# GetLangStr("Department20","选择所属机构") %>' Width="270" AllowBlank="false">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("Department20","清除选中") %>' />
                                    </Triggers>
                                    <Listeners>
                                        <Select Handler="this.triggers[0].show();" />
                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("Department21","工作内容") %>' ID="TxtWorkContent" Width="270" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("Department22","工作地址") %>' ID="TxtWorkAddress" Width="270" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("Department23","负责人") %>' ID="TxtManage" Width="270" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("Department24","负责人电话") %>' ID="TxtManagemoble" Width="270" MaxLength="11" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("Department25","负责人手机") %>' ID="TxtManagePhone" Width="270" MaxLength="11" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("Department26","办公电话1") %>' ID="TxtOfficePhone" Width="270" MaxLength="11" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("Department27","办公电话2") %>' ID="TxtOfficePhone2" Width="270" MaxLength="11" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("Department28","办公电话3") %>' ID="TxtOfficePhone3" Width="270" MaxLength="11" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("Department29","传真号码") %>' ID="txtOfficefax" Width="270" MaxLength="11" />
                                <ext:TextField runat="server" FieldLabel='<%# GetLangStr("Department30","邮政编码") %>' ID="txtPostcode" Width="270" MaxLength="6" />
                            </Items>
                        </ext:Panel>
                    </Items>
                    <TopBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <ext:Button ID="ButAdd" runat="server" Text='<%# GetLangStr("Department31","增加") %>' Icon="Add" ToolTip='<%# GetLangStr("Department32","增加") %>'>
                                    <Listeners>
                                        <Click Handler="Department.InfoSave()" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButUpdate" runat="server" Text='<%# GetLangStr("Department32","保存") %>' Icon="TableSave">
                                    <Listeners>
                                        <Click Handler="Department.UpdateData()" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButDelete" runat="server" Text='<%# GetLangStr("Department56","删除") %>' Icon="Delete">
                                    <Listeners>
                                        <Click Handler="Department.DoConfirm()" />
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