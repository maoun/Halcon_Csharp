<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PassCarInfoQueryMain.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.PassCarInfoQueryMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <title></title>
    <link href="../Styles/tabStyle.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="TabInfo" Style="z-index: 101; position: absolute; top: 0px; width: 100px; left: 10px;" runat="server"
                CssClass="TabButton" OnClick="TabInfo_Click"></asp:Button>
            <asp:Label ID="Label1" runat="server" Text="|" Style="z-index: 102; position: absolute; top: -5px; left: 100px; width: 10px; font-size: 20px"></asp:Label>
            <asp:Button ID="TabImg" Style="z-index: 103; position: absolute; top: 0px; left: 93px; width: 100px;" runat="server"
                CssClass="TabButton" OnClick="TabImg_Click"></asp:Button>
            <iframe id="IFRAME1" style="border-right: 0px solid; border-top: 0px solid; z-index: 104; left: 11px; border-left: 0px solid; width: 99%; border-bottom: 0px solid; position: absolute; top: 20px; height: 96%"
                runat="server"></iframe>
        </div>
    </form>
</body>
</html>