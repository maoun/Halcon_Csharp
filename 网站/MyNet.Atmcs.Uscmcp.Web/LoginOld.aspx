<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginOld.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.LoginOld" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>城市交通综合管控平台</title>
    <meta charset="GBK" />
    <meta name="robots" content="INDEX,FOLLOW" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <script type="text/javascript">
        function Login() {
            LoginOld.UserLogin();
        }
        function ReSize() {
            try {
                Window1.center();
            }
            catch (e) {
            }
        }
    </script>
    <script type="text/javascript">
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
            if (event.keyCode == 13) { //网页内按下回车触发
                Login();
                return false;
            }
            return true;
        };
    </script>
</head>
<body id="home" class="dehome" background="../Images/back.png" onresize="ReSize();">
    <form id="Form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="LoginOld" />
        <ext:Button ID="ButLogout" runat="server" Text="Logout" Icon="LockOpen">
            <Listeners>
                <Click Handler="#{Window1}.show();" />
            </Listeners>
        </ext:Button>
        <ext:Window ID="Window1" runat="server" Closable="false" Resizable="false" Height="590"
            Header="false" Title="" Draggable="false" Width="850" Padding="0" Layout="Form"
            BodyBorder="true" Border="true">
            <Items>
                <ext:Panel ID="Panel2" runat="server" Region="North" Title="用户登录" Header="false"
                    Border="false" Height="40" Padding="0">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Image ID="Image1" runat="server" Pressed="true" Height="40" Width="100" ImageUrl="../Images/login/logo.png">
                                    <ResizeConfig ID="ResizeConfig1" runat="server" Adjustments="0, 0" EnableViewState="False"
                                        Visible="False">
                                    </ResizeConfig>
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTipLogo" runat="server" Title="" />
                                    </ToolTips>
                                </ext:Image>
                                <ext:Label ID="lblSystemName" runat="server" Pressed="true" Height="50" Width="200">
                                </ext:Label>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:Panel>
                <ext:Panel ID="Panel3" Region="Center" runat="server" Padding="10" Height="390">
                    <Content>
                        <ext:Image ID="ImageLogo" runat="server" Width="812" Height="365" ImageUrl="../Images/sun.jpg">
                        </ext:Image>
                    </Content>
                </ext:Panel>
                <ext:Panel ID="Panel4" Region="South" runat="server" Title="用户登录" Layout="Absolute"
                    Height="90" Padding="10">
                    <Items>
                        <ext:Label ID="Label2" runat="server" X="10" Y="15" Text="用户:" />
                        <ext:TextField ID="txtUsername" runat="server" X="70" Y="10" BlankText="输入用户名......" Text="admin"
                            Width="100" AllowBlank="false" />
                        <ext:Label ID="Label1" runat="server" X="180" Y="15" Text="密码:" />
                        <ext:TextField ID="txtPassword" runat="server" X="240" Y="10" InputType="Password" Text="admin"
                            BlankText="输入密码......." Width="100" AllowBlank="false">
                            <Listeners>
                                <SpecialKey Handler="if(event.keyCode==13) Login();" />
                            </Listeners>
                        </ext:TextField>
                        <ext:Label ID="Label3" runat="server" X="350" Y="15" Text="模式:" />
                        <ext:ComboBox ID="CmbPageType" runat="server" Editable="false" EmptyText="选择模式......"
                            Width="130" X="420" Y="10">
                            <Items>
                                <ext:ListItem Text="普通模式" Value="A" />
                                <ext:ListItem Text="快捷模式" Value="B" />
                            </Items>
                        </ext:ComboBox>
                        <ext:Button ID="btnLogin" runat="server" Text="登录" Icon="Accept" X="600" Y="10" Width="60">
                            <Listeners>
                                <Click Handler="if (!#{txtUsername}.validate() || !#{txtPassword}.validate()) {Ext.Msg.alert('错误','请输入用户名和密码！'); return false; }" />
                            </Listeners>
                            <DirectEvents>
                                <Click OnEvent="btnLogin_Click">
                                    <EventMask ShowMask="true" Msg="正在验证..." MinDelay="500" />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="btnCancel" runat="server" Text="重置" Icon="Decline" X="720" Y="10"
                            Width="60">
                            <Listeners>
                                <Click Handler="#{txtPassword}.setText('');#{txtUsername}.setText('')"></Click>
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Panel>
                <ext:Panel ID="PanCopyright" runat="server" Title="<center> <img src='../Images/Login/about.png' height='12px' /> 版权所有 2012 -2016 </center>"
                    DefaultAnchor="100%" Height="40" Padding="0" Html="" />
            </Items>
        </ext:Window>
    </form>
</body>
</html>