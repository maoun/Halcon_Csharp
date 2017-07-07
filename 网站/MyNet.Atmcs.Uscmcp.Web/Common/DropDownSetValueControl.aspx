<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DropDownSetValueControl.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.DropDownSetValueControl" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
   "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <title></title>

    <script type="text/javascript" language="javascript">
        //清理选中
        var clearSelect = function (tree, field) {
            var ids = field.getValue();
            if (ids.length > 0) {
                try {
                    //设置 取消勾选
                    tree.setChecked({ ids: ids, silent: false });
                } catch (e) {
                }
            }
            //tree.getRootNode().collapseChildNodes(true);
        };
        // 获得选中value
        var getValues = function (tree) {
            var msg = [],
                selNodes = tree.getChecked();
            Ext.each(selNodes, function (node) {
                msg.push(node.id);
            });
            return msg.join(",");
        };
        // 获得选中text
        var getText = function (tree) {
            var msg = [],
                selNodes = tree.getChecked();
            Ext.each(selNodes, function (node) {
                msg.push(node.text);
            });
            return msg.join(",");
        };

        var syncValue = function (value) {
            var tree = this.component;
            if (tree.rendered) {
                if (value) {
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
                    } catch (e) { }
                }
            }
        };

        var filterTree = function (el, e) {
            var tree = TreeStation,
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
            var field = TriggerFieldStation,
                tree = TreeStation;
            field.setValue("");
            tree.clearFilter();
            //  tree.getRootNode().collapseChildNodes(true);
            tree.getRootNode().ensureVisible();
        };
    </script>
</head>
<body>

    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="KkidControl" />
        <ext:DropDownField ID="FieldStation" runat="server"
            Editable="false" Width="600" TriggerIcon="SimpleArrowDown" Mode="ValueText">
            <Component>
                <ext:TreePanel runat="server" Height="500" Shadow="None" ID="TreeStation"
                    UseArrows="true" AutoScroll="true" Animate="true" EnableDD="true" ContainerScroll="true" RootVisible="true"
                    StyleSpec="background-color: #ddecfe; border-radius: 20px;">
                    <Root>
                    </Root>
                    <Buttons>
                        <ext:Button runat="server" Text="清除">
                            <Listeners>
                                <Click Handler="clearSelect(TreeStation,FieldStation);" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button runat="server" Text="关闭">
                            <Listeners>
                                <Click Handler="#{FieldStation}.collapse();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <CheckChange Handler="this.dropDownField.setValue(getValues(this), getText(this), false);" />
                    </Listeners>
                    <SelectionModel>
                        <ext:MultiSelectionModel runat="server" />
                    </SelectionModel>
                    <TopBar>
                        <ext:Toolbar runat="server" Layout="FitLayout">
                            <Items>
                                <ext:TriggerField ID="TriggerFieldStation" runat="server" EnableKeyEvents="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" />
                                    </Triggers>
                                    <Listeners>
                                        <KeyUp Fn="filterTree" Buffer="400" />
                                        <TriggerClick Handler="clearFilter();" />
                                    </Listeners>
                                </ext:TriggerField>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:TreePanel>
            </Component>
            <Listeners>
                <Expand Handler="this.component.getRootNode().expand(false);" Single="true" Delay="20" />
            </Listeners>
            <SyncValue Fn="syncValue" />
        </ext:DropDownField>
        <ext:Button ID="btnValue" runat="server" Text="显示选中信息">
            <DirectEvents>
                <Click OnEvent="btnValue_Event"></Click>
            </DirectEvents>
        </ext:Button>
    </form>
</body>
</html>