<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VideoTemplate.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.VideoTemplate" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
   

    <script type="text/javascript">

        function ShowWindow(panel, type) {
            ButData.setValue(panel);
            Window1.autoLoad.url = "SelectPage.aspx?type=" + type;
            Window1.reload();
            Window1.show();
        }
      
    </script>
</head>
<body class="content">
    <form id="formVedio" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="VideoTemplate" />
        <ext:Hidden ID="ButData" runat="server" />
        <ext:Viewport runat="server" Layout="Fit">
            <Items>
                <ext:Panel runat="server" Padding="5" Border="false" ID="panelVideo">
                  <AutoLoad Mode="IFrame"></AutoLoad>
                </ext:Panel>
            </Items>
        </ext:Viewport>

        <ext:Window ID="Window1" runat="server" Title="选择显示的页" Icon="Application" Height="700px" Width="1024" Padding="5" Modal="true" Hidden="true">
            <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
            <TopBar>
                <ext:Toolbar ID="Toolbar5" runat="server">
                    <Items>
                        <ext:Label runat="server" Text="设为首页："></ext:Label>
                        <ext:RadioGroup
                            ID="RadioGroupTemplate"
                            runat="server"
                            ColumnsNumber="4" Width="440">
                            <Items>
                                <ext:Radio ID="radData" runat="server" BoxLabel="数据类模板" InputValue="DataTemplate" Checked="true" Width="100px" />
                                <ext:Radio ID="radVideo" runat="server" BoxLabel="视频类模板" InputValue="VideoTemplate" Width="100px" />
                                <ext:Radio ID="radList" runat="server" BoxLabel="列表类模板" InputValue="ListTemplate" Width="100px" />
                                <ext:Radio ID="radUser" runat="server" BoxLabel="自定义类模板" InputValue="UserTemplate" Width="120px" />
                            </Items>
                            <DirectEvents>
                                <Change OnEvent="TemplateGroup_Event"></Change>
                            </DirectEvents>
                        </ext:RadioGroup>
                        <ext:ToolbarFill></ext:ToolbarFill>
                        <ext:Button runat="server" ID="OkBtn" Text="确定" Icon="Accept">
                            <DirectEvents>
                                <Click OnEvent="ButOKClick" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="CancelBtn" Text="取消" Icon="Cancel">
                            <Listeners>
                                <Click Handler="Window1.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
        </ext:Window>
    </form>
</body>
</html>