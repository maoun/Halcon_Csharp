<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StationMain.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Station.StationMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
      <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Styles/tabStyle.css" rel="stylesheet" />
       <style type="text/css">
      
        #TabStationManager {
            z-index: 101; position: absolute; top: 0px;left: 10px;width: 100px;
        }
        #Label1 {
            z-index: 102; position: absolute; top: -5px; left: 110px; width: 10px;font-size:20px
        }
        #TabStationMark {
            z-index: 103; position: absolute; top: 0px;left: 110px; width: 100px;
        }
           #Label2 {
            z-index: 104; position: absolute; top: -5px; left: 210px; width: 10px;font-size:20px
        }
        #TabStationSpeed {
            z-index: 105; position: absolute; top: 0px;left: 212px; width: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <asp:Button ID="TabStationManager" runat="server"
                CssClass="TabButton" OnClick="TabStationManager_Click"></asp:Button>
            <asp:Label ID="Label1" runat="server" Text="|" ></asp:Label>
            <asp:Button ID="TabStationMark" runat="server"
                CssClass="TabButton" OnClick="TabStationMark_Click" ></asp:Button>
          <asp:Label ID="Label2" runat="server" Text="|" ></asp:Label>
            <asp:Button ID="TabStationSpeed" runat="server"
                CssClass="TabButton" OnClick="TabStationSpeed_Click" ></asp:Button>
           <iframe id="IFRAME1" style="border-right: 0px solid; border-top: 0px solid; z-index: 106; left: 11px; border-left: 0px solid; width: 99%; border-bottom: 0px solid; position: absolute; top: 20px; height: 90%"
                runat="server"></iframe>
    </div>
    </form>
</body>
</html>
