<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImageCut.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.ImageCut" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <title></title>
    <style type="text/css">
        #rRightDown, #rLeftDown, #rLeftUp, #rRightUp, #rRight, #rLeft, #rUp, #rDown {
            position: absolute;
            background: #F00;
            width: 5px;
            height: 5px;
            z-index: 500;
            font-size: 0;
        }

        #dragDiv {
            border: 1px solid #000000;
        }
    </style>
    <script src="../Scripts/cutpic.js" type="text/javascript" charset="UTF-8"></script>
    <script type="text/javascript" language="javascript">
        function UnloadConfirm() {
            window.dialogArguments.document.getElementById('T').value = document.getElementById('T').value;
            window.dialogArguments.document.getElementById('L').value = document.getElementById('L').value;
            window.dialogArguments.document.getElementById('W').value = document.getElementById('W').value;
            window.dialogArguments.document.getElementById('H').value = document.getElementById('H').value;
            window.dialogArguments.document.getElementById('imgCut').src = document.getElementById('hidUrl').value;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:Hidden runat="server" ID="hidUrl"></ext:Hidden>
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="ImageCut" />
        <ext:Toolbar runat="server">
            <Items>
                <ext:Button runat="server" ID="btnCut" Text="截取" Icon="ImageEdit">
                    <DirectEvents>
                        <Click OnEvent="Cut_Event"></Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server" ID="btnColse" Text="关闭" Icon="Cancel">
                    <Listeners>
                        <Click Handler="window.close();" />
                    </Listeners>
                </ext:Button>
                <ext:Label runat="server" ID="labl"></ext:Label>
                <ext:Panel runat="server" Layout="ContainerLayout">
                    <Content>
                        <div>
                            <input id="T" value="0" runat="server" />
                            <input id="L" value="0" runat="server" />
                            <input id="W" runat="server" />
                            <input id="H" runat="server" />
                        </div>
                    </Content>
                </ext:Panel>
            </Items>
        </ext:Toolbar>
        <div id="bgDiv" style="width: 400px; height: 500px;">
            <div id="dragDiv">
                <div id="rRightDown" style="right: 0; bottom: 0;"></div>
                <div id="rLeftDown" style="left: 0; bottom: 0;"></div>
                <div id="rRightUp" style="right: 0; top: 0;"></div>
                <div id="rLeftUp" style="left: 0; top: 0;"></div>
                <div id="rRight" style="right: 0; top: 50%;"></div>
                <div id="rLeft" style="left: 0; top: 50%;"></div>
                <div id="rUp" style="top: 0; left: 50%;"></div>
                <div id="rDown" style="bottom: 0; left: 50%;"></div>
            </div>
        </div>
    </form>
</body>

<script type="text/javascript" language="javascript">
    window.onunload = UnloadConfirm; //关闭后的事件
</script>
</html>