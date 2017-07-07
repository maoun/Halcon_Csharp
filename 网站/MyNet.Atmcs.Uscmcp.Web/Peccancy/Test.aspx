<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Test" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=GBK"/>
  <title>MultiCombo - Ext.NET Examples</title>
 
    
    <script type="text/javascript">
        var updateButtonText = function (item, checked) {
            var text = [];

            item.parentMenu.items.each(function (item) {
                if (item.checked) {
                    text.push(item.text);
                }
            });

            if (text.length == 0) {
                Button1.setText("[Select Colors]");
            } else {
                Button1.setText("[" + text.join(",") + "]");
            }
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
      <ext:ResourceManager runat="server" />
        
        <h2>Simple MultiCombo</h2>
        
        <ext:MultiCombo ID="Multi1" runat="server" Width="260" TriggerIcon="SimpleArrowDown" >
            <Items>
                <ext:ListItem Text="Item 1" Value="1" />
                <ext:ListItem Text="Item 2" Value="2" />
                <ext:ListItem Text="Item 3" Value="3" />
                <ext:ListItem Text="Item 4" Value="4" />
                <ext:ListItem Text="Item 5" Value="5" />
            </Items>
             <Triggers>
            <ext:FieldTrigger Icon="SimpleMagnify" />
        </Triggers>
            <SelectedItems>
                <ext:SelectedListItem Value="2" />
                <ext:SelectedListItem Index="4" />
            </SelectedItems>
        </ext:MultiCombo>
        <br />
         <h2>ComboBox</h2>
   
    <ext:ComboBox runat="server" TriggerIcon="SimpleArrowDown">
        <Items>
            <ext:ListItem Text="Item 1" />
            <ext:ListItem Text="Item 2" />
            <ext:ListItem Text="Item 3" />
            <ext:ListItem Text="Item 4" />
            <ext:ListItem Text="Item 5" />
        </Items>
        <Triggers>
            <ext:FieldTrigger Icon="SimpleMagnify" />
        </Triggers>
    </ext:ComboBox>
        <br />
        <h2>MultiCombo with WrapBySquareBrackets</h2>
        
        <ext:MultiCombo ID="MultiCombo1" runat="server" WrapBySquareBrackets="true" Width="260">
            <Items>
                <ext:ListItem Text="Item 1" Value="1" />
                <ext:ListItem Text="Item 2" Value="2" />
                <ext:ListItem Text="Item 3" Value="3" />
                <ext:ListItem Text="Item 4" Value="4" />
                <ext:ListItem Text="Item 5" Value="5" />
            </Items>
            
            <SelectedItems>
                <ext:SelectedListItem Value="2" />
                <ext:SelectedListItem Index="4" />
            </SelectedItems>
        </ext:MultiCombo>
        <br />
        
        <h2>MultiCombo with SelectionMode="Selection"</h2>
        
        <p>Selected items are highlighted.</p>
        
        <ext:MultiCombo ID="MultiCombo2" runat="server" SelectionMode="Selection" Width="260">
            <Items>
                <ext:ListItem Text="Item 1" Value="1" />
                <ext:ListItem Text="Item 2" Value="2" />
                <ext:ListItem Text="Item 3" Value="3" />
                <ext:ListItem Text="Item 4" Value="4" />
                <ext:ListItem Text="Item 5" Value="5" />
            </Items>
            
            <SelectedItems>
                <ext:SelectedListItem Value="2" />
                <ext:SelectedListItem Index="4" />
            </SelectedItems>
        </ext:MultiCombo>
        <br />
        
        <h2>MultiCombo with SelectionMode="All" (checkboxes and highlight)</h2>
        
        <p>Selected items are highlighted and checked.</p>
        
        <ext:MultiCombo ID="MultiCombo3" runat="server" SelectionMode="All" Width="260">
            <Items>
                <ext:ListItem Text="Item 1" Value="1" />
                <ext:ListItem Text="Item 2" Value="2" />
                <ext:ListItem Text="Item 3" Value="3" />
                <ext:ListItem Text="Item 4" Value="4" />
                <ext:ListItem Text="Item 5" Value="5" />
            </Items>
            
            <SelectedItems>
                <ext:SelectedListItem Value="2" />
                <ext:SelectedListItem Index="4" />
            </SelectedItems>
        </ext:MultiCombo>
        <br />
        
        <h2>Button with Menu to emulate MultiCombo</h2>
        
        <ext:Button ID="Button1" runat="server" Icon="Rgb" Text="[Select Colors]" Width="200">
            <Menu>
                <ext:Menu runat="server" Width="200">
                    <Defaults>
                        <ext:Parameter Name="HideOnClick" Value="false" Mode="Raw" />
                    </Defaults>
                    <Items>
                        <ext:CheckMenuItem runat="server" Text="Red" CheckHandler="updateButtonText"  />
                        <ext:CheckMenuItem runat="server" Text="Green" CheckHandler="updateButtonText" />
                        <ext:CheckMenuItem runat="server" Text="Blue" CheckHandler="updateButtonText" />
                    </Items>                    
                </ext:Menu>
            </Menu>
        </ext:Button>
    </form>
</body>
</html>

    