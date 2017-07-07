<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UIStation.ascx.cs" Inherits="MyNet.Atmcs.Uscmcp.UI.UIStation" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

      <ext:DropDownField ID="FieldStation" runat="server"
            Editable="false" Width="600" TriggerIcon="SimpleArrowDown" Mode="ValueText">
            <Component>
                <ext:TreePanel runat="server" Height="500" Shadow="None" ID="TreeStation"
                    UseArrows="true" AutoScroll="true" Animate="true" EnableDD="true" ContainerScroll="true" RootVisible="true"
                    StyleSpec="background-color: rgba(68,138,202,0.9); border-radius: 20px;">
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
                </ext:TreePanel>
            </Component>
            <Listeners>
                <Expand Handler="this.component.getRootNode().expand(false);" Single="true" Delay="20" />
            </Listeners>
            <SyncValue Fn="syncValue" />
        </ext:DropDownField>