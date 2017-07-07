<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Index" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>交通综合管理信息平台</title>
      <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <script type="text/javascript">
        function MenuItemClick(menuItem) {
            var myUrl = menuItem.title;
            var strs = new Array(); //定义一数组
            strs = myUrl.split("|"); //字符分割
            CenterPanel.autoLoad.url = strs[1];
            CenterPanel.reload();
            CenterPanel.setTitle(String.format("{0}", strs[0] + " -> " + menuItem.text));
            // filter: PROGID:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr='white', EndColorStr='steelblue');

        }
        function MenuItemClickByUrl(myUrl, menutext) {
            var strs = new Array(); //定义一数组
            strs = myUrl.split("|"); //字符分割
            CenterPanel.autoLoad.url = strs[1];
            CenterPanel.reload();
            CenterPanel.setTitle(String.format("{0}", strs[0] + " -> " + menutext));
        }

        function EditTheme(menuItem) {
            Index.GetThemeUrl(menuItem.group, {
                success: function (result) {
                    Ext.net.ResourceMgr.setTheme(result);
                    if (CenterPanel.getBody().Ext) {
                        CenterPanel.getBody().Ext.net.ResourceMgr.setTheme(result, menuItem.group.toLowerCase());
                    }
                }
            });
        }
    </script>
    <script type="text/javascript">
        // 屏蔽backspace
        document.onkeydown = function (e) {
            var code;
            if (!e) { var e = window.event; }
            if (e.keyCode) { code = e.keyCode; }
            else if (e.which) { code = e.which; }
            //BackSpace 8;
            if ((event.keyCode == 8) && ((event.srcElement.type != "text" && event.srcElement.type != "textarea" && event.srcElement.type != "password") || event.srcElement.readOnly == true)) {
                event.keyCode = 0;
                event.returnValue = false;
            }
            return true;
        };
    </script>
</head>
<body>
    <form id="Form1" runat="server">
        <div id="append_parent" />
        <ext:ResourceManager ID="ResourceManager1" runat="server" IDMode="Explicit" DirectMethodNamespace="Index" />
        <ext:Hidden ID="CurrentSelectMenu" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="SystemName" runat="server">
        </ext:Hidden>
        <ext:Viewport ID="Viewport1" runat="server" Layout="border">
            <Items>
                <ext:Panel ID="PanelMain" runat="server" Title="" Region="North" Height="89" Padding="0"
                    Collapsible="false">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server" Height="50">
                            <Items>
                                <ext:Image ID="Image1" runat="server" Pressed="true" Height="40" Width="100" ImageUrl="./images/login/logo.png">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTipLogo" runat="server" Title="" />
                                    </ToolTips>
                                </ext:Image>
                                <ext:Label ID="lblSystemName" runat="server" Pressed="true" Height="40" Width="200">
                                </ext:Label>
                                <ext:ToolbarFill />
                                <ext:Label ID="lblUserName" runat="server" Text="" LabelAlign="Left" />
                                <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="LinkExit" runat="server" Text="退出" LabelAlign="Left" Width="40">
                                    <DirectEvents>
                                        <Click OnEvent="LinkExit_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="LinkAbout" runat="server" Text="关于" LabelAlign="Left" Width="40">
                                    <DirectEvents>
                                        <Click OnEvent="LinkAbout_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="LinkPassword" runat="server" Text="密码修改" LabelAlign="Left" Width="40">
                                    <DirectEvents>
                                        <Click OnEvent="LinkPassword_Click" />
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="Button1" runat="server" Text="皮肤">
                                    <Menu>
                                        <ext:Menu ID="Menu1" runat="server">
                                            <Items>
                                                <ext:CheckMenuItem ID="CheckMenuItem1" runat="server" Text="默认风格" Icon="BulletBlue"
                                                    Group="Default" />
                                                <ext:CheckMenuItem ID="CheckMenuItem2" runat="server" Text="铂金灰" Icon="BulletWhite"
                                                    Group="Gray" />
                                                <ext:CheckMenuItem ID="CheckMenuItem3" runat="server" Text="简约时尚" Icon="BulletBlack"
                                                    Group="Access" />
                                                <ext:CheckMenuItem ID="CheckMenuItem4" runat="server" Text="自然石头" Icon="BulletGreen"
                                                    Group="Slate" />
                                            </Items>
                                            <Listeners>
                                                <ItemClick Fn="EditTheme" />
                                            </Listeners>
                                        </ext:Menu>
                                    </Menu>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:Panel ID="Panel6" runat="server" Height="70" Padding="0" Header="false" Border="false">
                            <TopBar>
                                <ext:Toolbar ID="ToolbarSystem" runat="server" Height="40">
                                    <Items>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:Panel>
                    </Items>
                </ext:Panel>
                <ext:FormPanel ID="PanelNavigate" runat="server" Title="工作导航" Region="West" Width="150"
                    MinWidth="150" MaxWidth="200" Collapsible="true" Layout="Fit">
                </ext:FormPanel>
                <ext:Panel ID="CenterPanel" runat="server" Region="Center" Title="系统主界面">
                    <AutoLoad Url="Blank.aspx" Mode="IFrame" ShowMask="true" />
                </ext:Panel>
                <ext:Panel ID="PanCopyright" runat="server" Title="版权" Region="South" Height="20"
                    Padding="0" Html="">
                </ext:Panel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
