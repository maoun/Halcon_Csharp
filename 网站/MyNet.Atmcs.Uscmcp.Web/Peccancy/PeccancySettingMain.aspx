<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeccancySettingMain.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Peccancy.PeccancySettingMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=GBK"/>
    <title></title>
    <link href="../Styles/tabStyle.css" rel="stylesheet" />
    <style type="text/css">
      
        #TabPeccancyTypeSetting {
            z-index: 101; position: absolute; top: 0px;left: 10px;width: 130px;
        }
        #Label1 {
            z-index: 102; position: absolute; top:-5px; left: 150px; width: 10px;font-size:20px
        }
        #TabTmsStationManager {
            z-index: 103; position: absolute; top: 0px;left: 155px; width: 100px;
        }
        #Label2 {
            z-index: 104; position: absolute; top: -5px; left: 265px; width: 10px;font-size:20px
        }
        #TabPeccancyType {
            z-index: 105; position: absolute; top: 0px;left: 270px; width: 100px;
        }
           #Label3 {
            z-index: 106; position: absolute; top: -5px; left: 380px; width: 10px;font-size:20px
        }
        #TabTmsUserLocation {
            z-index: 107; position: absolute; top: 0px;left: 385px; width: 100px;
        }
         #Label4 {
            z-index: 106; position: absolute; top: -5px; left: 492px; width: 10px;font-size:20px
        }
        #TabTmsCheckLess {
            z-index: 107; position: absolute; top: 0px;left: 496px; width: 89px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:Button ID="TabPeccancyTypeSetting" runat="server"
                CssClass="TabButton" OnClick="TabPeccancyTypeSetting_Click"></asp:Button>
            <asp:Label ID="Label1" runat="server" Text="|" ></asp:Label>
            <asp:Button ID="TabTmsStationManager" runat="server"
                CssClass="TabButton" OnClick="TabTmsStationManager_Click" ></asp:Button>
             <asp:Label ID="Label2" runat="server" Text="|" ></asp:Label>
            <asp:Button ID="TabPeccancyType"   runat="server"
                CssClass="TabButton" OnClick="TabPeccancyType_Click" ></asp:Button>
          <asp:Label ID="Label3" runat="server" Text="|" ></asp:Label>
            <asp:Button ID="TabTmsUserLocation"   runat="server"
                CssClass="TabButton" OnClick="TabTmsUserLocation_Click" ></asp:Button>
                  <asp:Label ID="Label4" runat="server" Text="|" ></asp:Label>
            <asp:Button ID="TabTmsCheckLess"   runat="server"
                CssClass="TabButton" OnClick="TabTmsCheckLess_Click" ></asp:Button>
           <iframe id="IFRAME1" style="border-right: 0px solid; border-top: 0px solid; z-index: 108; left: 11px; border-left: 0px solid; width: 99%; border-bottom: 0px solid; position: absolute; top: 20px; height: 90%"
                runat="server"></iframe>
    </div>
    </form>
</body>
</html>
