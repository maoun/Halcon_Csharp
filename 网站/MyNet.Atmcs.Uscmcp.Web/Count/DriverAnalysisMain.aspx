<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DriverAnalysisMain.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Count.DriverAnalysisMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=GBK"/>
    <title></title>
    <link href="../Styles/tabStyle.css" rel="stylesheet" />
      <style type="text/css">     
        #TabDriveAgeAnalysis {
            z-index: 101; position: absolute; top: 0px;left: 10px;width: 100px;
        }
        #Label1 {
            z-index: 102; position: absolute; top: -5px; left: 100px; width: 10px;font-size:20px
        }
        #TabQuarterDrivingFrom {
            z-index: 103; position: absolute; top: 0px;left: 95px; width: 100px;
        }
        #Label2 {
            z-index: 104; position: absolute; top: -5px; left: 185px; width: 10px;font-size:20px
        }
        #TabQuarterDrivingAge {
            z-index: 105; position: absolute; top: 0px;left: 180px; width: 100px;
        }
           #Label3 {
            z-index: 106; position: absolute; top: -5px; left: 270px; width: 10px;font-size:20px
        }
        #TabRemoteDriver {
            z-index: 107; position: absolute; top: 0px;left: 275px; width: 120px;
        }
            #Label4 {
            z-index: 108; position: absolute; top: -5px; left: 397px; width: 10px;font-size:20px
        }
        #TabCarlienseAnalysis {
            z-index: 109; position: absolute; top: 0px;left: 395px; width: 120px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
      <asp:Button ID="TabDriveAgeAnalysis" runat="server"
                CssClass="TabButton" OnClick="TabDriveAgeAnalysis_Click"></asp:Button>
            <asp:Label ID="Label1" runat="server" Text="|" ></asp:Label>
            <asp:Button ID="TabQuarterDrivingFrom" runat="server"
                CssClass="TabButton" OnClick="TabQuarterDrivingFrom_Click" ></asp:Button>
             <asp:Label ID="Label2" runat="server" Text="|" ></asp:Label>
            <asp:Button ID="TabQuarterDrivingAge"   runat="server"
                CssClass="TabButton" OnClick="TabQuarterDrivingAge_Click" ></asp:Button>
         <asp:Label ID="Label3" runat="server" Text="|" ></asp:Label>
          <asp:Button ID="TabRemoteDriver"   runat="server"
                CssClass="TabButton" OnClick="TabRemoteDriver_Click" ></asp:Button>
         <asp:Label ID="Label4" runat="server" Text="|" ></asp:Label>
          <asp:Button ID="TabCarlienseAnalysis"   runat="server"
                CssClass="TabButton"  OnClick="TabCarlienseAnalysis_Click"></asp:Button>
           <iframe id="IFRAME1" style="border-right: 0px solid; border-top: 0px solid; z-index: 110; left: 11px; border-left: 0px solid; width: 99%; border-bottom: 0px solid; position: absolute; top: 20px; height: 90%"
                runat="server"></iframe>
    </form>
</body>
</html>
