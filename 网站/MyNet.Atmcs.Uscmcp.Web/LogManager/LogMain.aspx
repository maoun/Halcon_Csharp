<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogMain.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.LogManager.LogMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=GBK" />
    <link href="../Styles/tabStyle.css" rel="stylesheet" />
    <style type="text/css">
        #TabUser {
            z-index: 101;
            position: absolute;
            top: 0px;
            left: 10px;
            width: 100px;
        }

        #Label1 {
            z-index: 102;
            position: absolute;
            top: -5px;
            left: 120px;
            width: 10px;
            font-size: 20px;
        }

        #TabPriv {
            z-index: 103;
            position: absolute;
            top: 0px;
            left: 125px;
            width: 100px;
        }

        #Label2 {
            z-index: 104;
            position: absolute;
            top: -5px;
            left: 235px;
            width: 10px;
            font-size: 20px;
        }

        #TabRole {
            z-index: 105;
            position: absolute;
            top: 0px;
            left: 240px;
            width: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 100%; height: 100%">
            <asp:Button ID="TabUser" runat="server"
                CssClass="TabButton" OnClick="TabUser_Click"></asp:Button>
            <asp:Label ID="Label1" runat="server" Text="|"></asp:Label>
            <asp:Button ID="TabPriv" runat="server"
                CssClass="TabButton" OnClick="TabPriv_Click"></asp:Button>
            <asp:Label ID="Label2" runat="server" Text="|"></asp:Label>
            <asp:Button ID="TabRole" runat="server"
                CssClass="TabButton" OnClick="TabRole_Click"></asp:Button>
            <iframe id="IFRAME1" style="border-right: 0px solid; border-top: 0px solid; z-index: 106; left: 11px; border-left: 0px solid; width: 99%; border-bottom: 0px solid; position: absolute; top: 20px; height: 95%"
                runat="server"></iframe>
        </div>
    </form>
</body>
</html>