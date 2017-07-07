<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VehicleAnalysisMain.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Count.VehicleAnalysisMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <link href="../Styles/tabStyle.css" rel="stylesheet" />
      <style type="text/css">
        #TabCllx {
            z-index: 101; position: absolute; top: 0px;left: 10px;width: 110px;
        }
        #Label1 {
            z-index: 102; position: absolute; top: -5px; left: 120px; width: 10px;font-size:20px
        }
        #TabHpzl {
            z-index: 103; position: absolute; top: 0px;left: 125px; width: 100px;
        }
        #Label2 {
            z-index: 104; position: absolute; top: -5px; left: 235px; width: 10px;font-size:20px
        }
        #TabSyxz {
            z-index: 105; position: absolute; top: 0px;left: 240px; width: 100px;
        }
           #Label3 {
            z-index: 106; position: absolute; top: -5px; left: 350px; width: 10px;font-size:20px
        }
        #TabSyq {
            z-index: 107; position: absolute; top: 0px;left: 350px; width: 100px;
        }
            #Label4 {
            z-index: 108; position: absolute; top: -5px; left: 450px; width: 10px;font-size:20px
        }
        #TabWnj {
            z-index: 109; position: absolute; top: 0px;left: 455px; width: 120px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="TabCllx" runat="server"
                CssClass="TabButton" OnClick="TabCllx_Click"></asp:Button>
            <asp:Label ID="Label1" runat="server" Text="|" ></asp:Label>
            <asp:Button ID="TabHpzl" runat="server"
                CssClass="TabButton" OnClick="TabHpzl_Click" ></asp:Button>
             <asp:Label ID="Label2" runat="server" Text="|" ></asp:Label>
            <asp:Button ID="TabSyxz"   runat="server"
                CssClass="TabButton" OnClick="TabSyxz_Click" ></asp:Button>
         <asp:Label ID="Label3" runat="server" Text="|" ></asp:Label>
          <asp:Button ID="TabSyq"   runat="server"
                CssClass="TabButton" OnClick="TabSyq_Click" ></asp:Button>
         <asp:Label ID="Label4" runat="server" Text="|" ></asp:Label>
          <asp:Button ID="TabWnj"   runat="server"
                CssClass="TabButton"  OnClick="TabWnj_Click"></asp:Button>
           <iframe id="IFRAME1" style="border-right: 0px solid; border-top: 0px solid; z-index: 110; left: 11px; border-left: 0px solid; width: 99%; border-bottom: 0px solid; position: absolute; top: 20px; height: 90%"
                runat="server"></iframe>
    </div>
    </form>
</body>
</html>
