<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviceMain.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Station.DeviceMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=GBK"/>
    <title></title>
    <link href="../Styles/tabStyle.css" rel="stylesheet" />
     <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
        <style type="text/css">
      
        #TabDeviceStatus {
            z-index: 101; position: absolute; top: 0px;left: 10px;width: 100px;
        }
        #Label1 {
            z-index: 102; position: absolute; top: -5px; left: 120px; width: 10px;font-size:20px
        }
        #TabDeviceManager {
            z-index: 103; position: absolute; top: 0px;left: 125px; width: 100px;
        }
         #Label2 {
            z-index: 104; position: absolute; top: -5px; left: 235px; width: 10px;font-size:20px
        }
        #TabDeviceOperation {
            z-index: 105; position: absolute; top: 0px;left: 240px; width: 100px;
        }
         #Label3{
            z-index: 106; position: absolute; top: -5px; left: 350px; width: 10px;font-size:20px
        }
        #TabDeviceStatistics {
            z-index: 107; position: absolute; top: 0px;left: 355px; width: 100px;
        }
          #Label4 {
            z-index: 108; position: absolute; top: -5px; left: 460px; width: 10px;font-size:20px
        }
        #TabServerManager {
            z-index: 109; position: absolute; top: 0px;left: 465px; width: 120px;
        }
    </style>
</head>
<body>
    
    <form id="form1" runat="server">
    <div>
       <asp:Button ID="TabDeviceStatus" runat="server"
                CssClass="TabButton" OnClick="TabDeviceStatus_Click"></asp:Button>
            <asp:Label ID="Label1" runat="server" Text="|" ></asp:Label>
            <asp:Button ID="TabDeviceManager" runat="server"
                CssClass="TabButton" OnClick="TabDeviceManager_Click" ></asp:Button>
            <asp:Label ID="Label2" runat="server" Text="|" ></asp:Label>
         <asp:Button ID="TabDeviceOperation" runat="server"
                CssClass="TabButton" OnClick="TabDeviceOperation_Click"></asp:Button>
          <asp:Label ID="Label3" runat="server" Text="|" ></asp:Label>
          <asp:Button ID="TabDeviceStatistics" runat="server"
                CssClass="TabButton" OnClick="TabDeviceStatistics_Click"></asp:Button>
            <asp:Label ID="Label4" runat="server" Text="|" ></asp:Label>
          <asp:Button ID="TabServerManager" runat="server"
                CssClass="TabButton" OnClick="TabServerManager_Click"></asp:Button>
           <iframe id="IFRAME1" style="border-right: 0px solid; border-top: 0px solid; z-index: 110; left: 11px; border-left: 0px solid; width: 99%; border-bottom: 0px solid; position: absolute; top: 20px; height: 90%"
                runat="server"></iframe>
    </div>
    </form>
</body>
</html>
