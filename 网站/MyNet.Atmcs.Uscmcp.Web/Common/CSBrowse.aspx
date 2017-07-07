<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CSBrowse.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.CSBrowse" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>浏览</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="../Css/chooser.css" />
    <script language="javascript" type="text/javascript" charset="utf-8" src="../Scripts/common.js"></script>
    <script type="text/javascript">
        function SetUrl(userid, funcid) {
            var obj = thisMovie("UscmcpWebActiveX");
            var str = window.location.href;
            var str1 = str.substring(7)
            var ipaddress = "";
            var parastr = str1.indexOf("/")
            if (parastr > 0) {
                ipaddress = str.substring(7, parastr + 7);
            }
            var url = "http://" + ipaddress + "/DataService/DataService.asmx";
            obj.SetServiceUrl(url);
            obj.SetUserId(userid, funcid);
        }
        function thisMovie(movieName) {
            return document.getElementById(movieName);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate></ContentTemplate>
        </asp:UpdatePanel>
        <div class="" style="width: 100%; height: 300px; z-index: 800;">
            <object id="UscmcpWebActiveX" classid="clsid:956F226D-0AEB-45f9-9DBC-7815C5FD0F70" style="position: absolute; z-index: -888; top: 0px; left: -2px; width: 100%; height: 100%;">
            </object>
        </div>
    </form>
</body>
</html>