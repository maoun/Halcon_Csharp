<%@ Control Language="C#" AutoEventWireup="true" Inherits="MyNet.Atmcs.Uscmcp.UI.UIDepartment" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
  <%--background-color: #ddecfe;            border-radius: 15px;--%>
<ext:Hidden ID="DepaertMentId" runat="server" />
<ext:Hidden ID="DepaertMentName" runat="server" />
<ext:DropDownField ID="FieldDepartment" runat="server" Editable="false" TriggerIcon="Combo" EmptyText=" 请选择所属机构...">
    <Component>
        <ext:TreePanel runat="server" Height="300" Shadow="None" ID="TreeDepartment"
            UseArrows="true" AutoScroll="true" Animate="true" EnableDD="true" ContainerScroll="true" 
            RootVisible="true"  StyleSpec="background-color:rgba(68,138,202,0.9); border-radius: 15px;" >
            <Root>
            </Root>
            <Buttons>
                <ext:Button runat="server" Text="关闭">
                    <Listeners>
                        <Click Handler="#{FieldDepartment}.collapse();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
            <Listeners>
                <Click Handler="#{DepaertMentId}.setValue(node.id);#{DepaertMentName}.setValue(node.text);this.dropDownField.setValue(node.text);#{FieldDepartment}.collapse();" />
            </Listeners>
            <DirectEvents>
                <Click OnEvent="ButResetClick" />
            </DirectEvents>
            <TopBar>
                <ext:Toolbar runat="server" Layout="FitLayout">
                    <Items>
                        <ext:TriggerField ID="TriggerFieldDept" runat="server" EnableKeyEvents="true"  StyleSpec= "border-radius: 20px;">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" />
                            </Triggers>
                            <Listeners>
                                <KeyUp  Fn="filterTree" 
                                    Buffer="500" />
                                <TriggerClick Handler="#{DepaertMentId}.setValue('');
                                    #{DepaertMentName}.setValue('');#{FieldDepartment}.setValue('');
                                    #{TriggerFieldDept}.setValue('');#{TreeDepartment}.clearFilter();
                                    #{TreeDepartment}.getRootNode().collapseChildNodes(true);
                                     #{TreeDepartment}.getRootNode().ensureVisible();
                                 #{TreeDepartment}.expandAll(); " />
                            </Listeners>
                        </ext:TriggerField>
                    </Items>
                </ext:Toolbar>
            </TopBar>
        </ext:TreePanel>
    </Component>
    <Listeners>
        <Expand Handler="this.component.getRootNode().expand(true);" Single="true" Delay="10" />
    </Listeners>
</ext:DropDownField>