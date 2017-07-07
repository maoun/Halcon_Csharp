<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportantMain.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Important.ImportantMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  
    <title></title>
     <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
   <%-- <link href="../Styles/tabStyle.css" rel="stylesheet" />--%>
    <style type="text/css">
         .TabButton {
            border-top-style: none;
            border-right-style: none;
            border-left-style: none;
            border-bottom-style: none;
            color: blue;
            font-weight: bold;
            font-size:15px;
            background-color:transparent;/*背景为透明的*/
        }    
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="width: 100%;">
                    <asp:Button ID="TabRule" Style="z-index: 101; position: absolute; top: 0px; width: 100px; left: 10px;" runat="server"
                        CssClass="TabButton" OnClick="TabRule_Click"></asp:Button>
                    <asp:Label ID="Label1" runat="server" Text="|" Style="z-index: 102; position: absolute; top: -5px; left: 117px; width: 10px; font-size: 20px"></asp:Label>
                    <asp:Button ID="TabArea" Style="z-index: 103; position: absolute; top: 0px; left: 120px; width: 100px;" runat="server"
                        CssClass="TabButton" OnClick="TabArea_Click"></asp:Button>
                    <iframe id="IFRAME1" style="border-right: 0px solid; border-top: 0px solid; z-index: 104; border-left: 0px solid; width: 99%; border-bottom: 0px solid; position: absolute; top: 20px; height: 90%;"
                        runat="server"></iframe>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>