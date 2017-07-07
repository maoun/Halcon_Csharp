<%@ Control Language="C#" AutoEventWireup="true" Inherits="MyNet.Atmcs.Uscmcp.UI.VehicleHead" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<ext:DropDownField ID="Field2" runat="server" Editable="false" Width="45">
    <Component>
        <ext:Panel ID="Panel1" runat="server" Width="200" Height="60" StyleSpec="background-color:#ddecfe;">
            <TopBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <ext:TableLayout ID="TableLayout1" runat="server" Columns="6">
                            <Cells>
                                <ext:Cell>
                                    <ext:Button ID="Button333" runat="server" Text="云" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button111" runat="server" Text="晋" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button222" runat="server" Text="冀" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>

                                <ext:Cell>
                                    <ext:Button ID="Button44" runat="server" Text="豫" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button55" runat="server" Text="京" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button66" runat="server" Text="津" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button77" runat="server" Text="沪" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button88" runat="server" Text="湘" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button99" runat="server" Text="蒙" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button10" runat="server" Text="宁" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button11" runat="server" Text="甘" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button12" runat="server" Text="陕" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button13" runat="server" Text="辽" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button14" runat="server" Text="皖" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button15" runat="server" Text="鄂" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button16" runat="server" Text="青" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button17" runat="server" Text="川" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button18" runat="server" Text="渝" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button19" runat="server" Text="闽" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button20" runat="server" Text="粤" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button21" runat="server" Text="浙" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button22" runat="server" Text="赣" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button23" runat="server" Text="苏" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button24" runat="server" Text="吉" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button25" runat="server" Text="黑" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button26" runat="server" Text="新" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button27" runat="server" Text="鲁" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button28" runat="server" Text="贵" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button29" runat="server" Text="桂" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                                <ext:Cell>
                                    <ext:Button ID="Button30" runat="server" Text="琼" Flat="true" StyleSpec="border-radius: 0px; border: none;">
                                        <Listeners>
                                            <Click Handler="#{Field2}.setValue(this.text);" />
                                        </Listeners>
                                    </ext:Button>
                                </ext:Cell>
                            </Cells>
                        </ext:TableLayout>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <BottomBar>
                <ext:Toolbar ID="Toolbar2" runat="server">
                    <Items>
                        <ext:Button ID="Button40" runat="server" Text="清空" StyleSpec="border-radius: 0px; border: none;">
                            <Listeners>
                                <Click Handler="#{Field2}.setValue('');" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </BottomBar>
        </ext:Panel>
    </Component>
</ext:DropDownField>