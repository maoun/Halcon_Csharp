<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UIDepartment.ascx.cs" Inherits="MyNet.Atmcs.Uscmcp.UI.UIDepartment" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<script type="text/javascript">
    var filterTree = function (el, e) {
        var tree = TreeDepartment,
            text = this.getRawValue();

        tree.clearFilter();

        if (Ext.isEmpty(text, false)) {
            return;
        }

        if (e.getKey() === Ext.EventObject.ESC) {
            clearFilter();
        } else {
            var re = new RegExp(".*" + text + ".*", "i");

            tree.filterBy(function (node) {
                return re.test(node.text);
            });
        }
    };

    var clearFilter = function () {
        var field = TriggerFieldDept,
            tree = TreeDepartment;

        field.setValue("");
        tree.clearFilter();
        tree.getRootNode().collapseChildNodes(true);
        tree.getRootNode().ensureVisible();
        //tree.expandAll();
    };
</script>
<ext:hidden id="DepaertMentId" runat="server" />
<ext:hidden id="DepaertMentName" runat="server" />

<ext:dropdownfield id="FieldDepartment" runat="server" editable="false" triggericon="Combo" emptytext=" 请选择所属机构..."  AllowBlank="false">
    <Component>
        <ext:TreePanel runat="server" Height="300" Shadow="None" ID="TreeDepartment"
            UseArrows="true" AutoScroll="true" Animate="true" EnableDD="true" ContainerScroll="true"
            RootVisible="true">
            <Root>
            </Root>
            <Buttons>
                <ext:Button runat="server" Text="关闭中">
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
                        <ext:TriggerField ID="TriggerFieldDept" runat="server" EnableKeyEvents="true">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" />
                            </Triggers>
                            <Listeners>
                                <KeyUp Fn="filterTree" Buffer="250" />
                                <TriggerClick Handler="clearFilter();" />
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
</ext:dropdownfield>