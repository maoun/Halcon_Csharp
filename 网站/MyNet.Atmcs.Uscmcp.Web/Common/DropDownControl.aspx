<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DropDownControl.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.DropDownControl" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
   "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DropDownField Overview - Ext.NET Examples</title>
     <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <script type="text/javascript">
        //清理选中
        var clearSelect = function (tree, field) {
            var ids = field.getValue();
            if (ids.length > 0) {
                try {
                    tree.setChecked({ ids: ids, silent: false });
                } catch (e) {
                }
            }
        };

        var getPersonInfos = function (tree) {
            var msg = "", id = "";
            selNodes = tree.getChecked();
            Ext.each(selNodes, function (node) {
                if (msg.length > 1) {
                    msg += ",";
                    id += ",";
                }
                msg += node.text;
                id += "'" + node.id + "'";
            });
            hidJh.value = id;
            return msg;
        };
        var clearSelect = function (tree) {
            var selNodes = tree.getChecked();
            for (var i = 0; i < selNodes.length; i++) {
                selNodes[i].attributes.checked = false;
            }
        };
        var syncValue = function (value) {
            var tree = this.component;
            if (tree.rendered) {
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
                } catch (e) {

                }

            }
        };
    </script>
</head>
<body>
    <form runat="server">
        <ext:ResourceManager runat="server" />
        <ext:Hidden runat="server" ID="hidJh"></ext:Hidden>
        <ext:Hidden runat="server" ID="hidXm"></ext:Hidden>
        <ext:DropDownField ID="FieldPerson" runat="server" Editable="false" Width="400" TriggerIcon="SimpleArrowDown" EmptyText="请选择警员...">
            <Triggers>
                <ext:FieldTrigger Icon="Clear" />
            </Triggers>
            <Listeners>
                <TriggerClick Handler="#{FieldPerson}.setValue('','');#{hidJh}.setValue('');clearSelect(#{TreePerson})" />
            </Listeners>
            <Component>
                <ext:TreePanel ID="TreePerson"
                    runat="server"
                    Icon="Accept"
                    Height="500"
                    Shadow="None"
                    UseArrows="true"
                    AutoScroll="true"
                    Animate="true"
                    EnableDD="true"
                    ContainerScroll="true"
                    RootVisible="true">
                    <Root>
                        <ext:TreeNode>
                            <Nodes>
                                <ext:TreeNode Text="To Do" Icon="Folder">
                                    <Nodes>
                                        <ext:TreeNode Text="Go jogging" Leaf="true" NodeID="1" Checked="False" />
                                        <ext:TreeNode Text="Take a nap" Leaf="true" NodeID="2" Checked="False" />
                                        <ext:TreeNode Text="Clean house" Leaf="true" NodeID="3" Checked="False" />
                                    </Nodes>
                                </ext:TreeNode>

                                <ext:TreeNode Text="Grocery List" Icon="Folder">
                                    <Nodes>
                                        <ext:TreeNode Text="Bananas" Leaf="true" Checked="False" />
                                        <ext:TreeNode Text="Milk" Leaf="true" Checked="False" />
                                        <ext:TreeNode Text="Cereal" Leaf="true" Checked="False" />

                                        <ext:TreeNode Text="Energy foods" Icon="Folder">
                                            <Nodes>
                                                <ext:TreeNode Text="Coffee" Leaf="true" Checked="False" />
                                                <ext:TreeNode Text="Red Bull" Leaf="true" Checked="False" />
                                            </Nodes>
                                        </ext:TreeNode>
                                    </Nodes>
                                </ext:TreeNode>

                                <ext:TreeNode Text="Kitchen Remodel" Icon="Folder">
                                    <Nodes>
                                        <ext:TreeNode Text="Finish the budget" Leaf="true" Checked="False" />
                                        <ext:TreeNode Text="Call contractors" Leaf="true" Checked="False" />
                                        <ext:TreeNode Text="Choose design" Leaf="true" Checked="False" />
                                    </Nodes>
                                </ext:TreeNode>
                            </Nodes>
                        </ext:TreeNode>
                    </Root>
                    <Buttons>
                        <ext:Button runat="server" Text="清除">
                            <Listeners>
                                <Click Handler="clearSelect(TreePerson,FieldPerson);" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button runat="server" Text="关闭">
                            <Listeners>
                                <Click Handler="#{FieldPerson}.collapse();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <CheckChange Handler="this.dropDownField.setValue(getPersonInfos(this), false);" />
                    </Listeners>
                </ext:TreePanel>
            </Component>
            <Listeners>
                <Expand Handler="this.component.getRootNode().expand(false);" Single="true" Delay="10" />
            </Listeners>
            <SyncValue Fn="syncValue" />
        </ext:DropDownField>
        <ext:Button runat="server" Text="显示选中value">
            <Listeners>
                <Click Handler="alert(#{hidJh}.value)" />
            </Listeners>
        </ext:Button>
    </form>
</body>
</html>