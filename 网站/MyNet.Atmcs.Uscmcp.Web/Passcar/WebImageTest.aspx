<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebImageTest.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Passcar.WebImageTest" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=GBK"/>
    <title></title>
     <script type="text/javascript">
         function Show() {
             $("#ImgFile-file").click();
         }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="WebImageTest" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="RowLayout" Cls="new-layout">
            <Items>
                        <ext:Button ID="butcut" runat="server" Height="50" Text="截图" >
                            <DirectEvents>
                                <Click OnEvent="cutclick"  />
                            </DirectEvents>
                        </ext:Button>
                <ext:Panel runat="server" Layout="ColumnLayout">
                    <Items>
                                 <ext:Panel ID="pnlData" runat="server" Title="地图浏览"  AutoRender="true" Region="Center"
                Header="false" Height="800" Width="1000"  IconCls="temp/map.png" Cls="map-bg">
                <Items>
                        <ext:Image 
                            ID="b" 
                            runat="server"
                            Resizable="true" 
                            ImageUrl="../Map/temp/map.png">
                            <ResizeConfig runat="server" PreserveRatio="true" HandlesSummary="e se s"/>
                            
                        </ext:Image> 
                    </Items>
            </ext:Panel>
                          <ext:Panel ID="Panel1" runat="server" Title="地图浏览"  AutoRender="true" Region="Center"
                Header="false" Height="180" Width="300"  Layout="RowLayout" IconCls="temp/map.png" Cls="map-bg">
                <Items>
                        <ext:Image 
                            ID="Image2" 
                            runat="server"
                            Resizable="true" 
                            ImageUrl="../Map/temp/map.png">
                            <ResizeConfig runat="server" Height="180" PreserveRatio="true" HandlesSummary="e se s"/>
                            
                        </ext:Image> 
                    <ext:Panel runat="server" RowHeight="1" />
                    </Items>
            </ext:Panel>
                            
                        
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
