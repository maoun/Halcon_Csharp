<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserTemplate.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.UserTemplate" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
  <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <style type="text/css">
        .ToolbarStyle {
            background-color: transparent;
            border-bottom-width: 0px;
        }

            .ToolbarStyle button {
                color: transparent;
            }
    </style>
    <script type="text/javascript">

        function ShowWindow(panel, type) {
            ButData.setValue(panel);
            Window1.autoLoad.url = "SelectPage.aspx?type=" + type;
            Window1.reload();
            Window1.show();
        }
        function AutoLoadPanel(panelname, url) {
            switch (panelname) {
                case "UserTemplate":
                    UserTemplate1.autoLoad.url = url;
                    UserTemplate1.reload();
                    break;
            }
        }
    </script>
</head>
<body>
    <form runat="server">
        <ext:ResourceManager runat="server" ID="ResourceManager1" DirectMethodNamespace="UserTemplate" />
        <ext:Hidden ID="ButData" runat="server" />
        <ext:Viewport ID="ViewPort1" runat="server" Layout="FitLayout">
            <Items>
                <ext:Panel runat="server" Layout="RowLayout">
                    <Items>
                        <ext:Panel runat="server" Height="40" Border="false">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server" Cls="ToolbarStyle" Layout="Container" Flat="true">
                                    <Items>
                                        <ext:Button runat="server" Text="编辑" ID="Button2">
                                            <Listeners>
                                                <Click Handler="ShowWindow('UserTemplate','func'); " />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:Panel>
                        <ext:Panel runat="server" Border="false" ID="UserTemplate1" RowHeight="1" ColumnWidth="1" AutoScroll="true">
                            <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
                        </ext:Panel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
        <ext:Window ID="Window1" runat="server" Title="选择显示的页" Icon="Application" Height="700px" Width="1024" Padding="5" Modal="true" Hidden="true">
            <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
            <TopBar>
                <ext:Toolbar ID="Toolbar4" runat="server">
                    <Items>
                         <ext:Label runat="server" Text="设为首页："></ext:Label>
                        <ext:RadioGroup
                            ID="RadioGroupTemplate"
                            runat="server"
                            ColumnsNumber="4" Width="440" >
                            <Items>
                                <ext:Radio  ID="radData" runat="server" BoxLabel="数据类模板"  InputValue="DataTemplate" Checked="true"  Width="100px" />
                                <ext:Radio  ID="radVideo" runat="server" BoxLabel="视频类模板"  InputValue="VideoTemplate"   Width="100px"/>
                                <ext:Radio  ID="radList" runat="server" BoxLabel="列表类模板"   InputValue="ListTemplate" Width="100px" />
                                <ext:Radio  ID="radUser" runat="server" BoxLabel="自定义类模板"  InputValue="UserTemplate"   Width="120px"/>
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