<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtherAnalysisMain.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Count.OtherAnalysisMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=GBK"/>
    <title></title>
    <link href="../Styles/tabStyle.css" rel="stylesheet" />
      <style type="text/css">
        #TabPassCarInfoCount {
            z-index: 101; position: absolute; top: 0px;left: 10px;width: 100px;
        }
        #Label1 {
            z-index: 102; position: absolute; top: -5px; left: 120px; width: 10px;font-size:20px
        }
        #TabPassCarFlowCount {
            z-index: 103; position: absolute; top: 0px;left: 125px; width: 100px;
        }
        #Label2 {
            z-index: 104; position: absolute; top: -5px; left: 235px; width: 10px;font-size:20px
        }
        #TabPassCarSpeedCount {
            z-index: 105; position: absolute; top: 0px;left: 240px; width: 100px;
        }
           #Label3 {
            z-index: 106; position: absolute; top: -5px; left: 350px; width: 10px;font-size:20px
        }
        #TabPassCarOcrCount {
            z-index: 107; position: absolute; top: 0px;left: 355px; width: 120px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:Button ID="TabPassCarInfoCount" runat="server"
                CssClass="TabButton" OnClick="TabPassCarInfoCount_Click"></asp:Button>
            <asp:Label ID="Label1" runat="server" Text="|" ></asp:Label>
            <asp:Button ID="TabPassCarFlowCount" runat="server"
                CssClass="TabButton" OnClick="TabPassCarFlowCount_Click" ></asp:Button>
             <asp:Label ID="Label2" runat="server" Text="|" ></asp:Label>
            <asp:Button ID="TabPassCarSpeedCount"   runat="server"
                CssClass="TabButton" OnClick="TabPassCarSpeedCount_Click" ></asp:Button>
         <asp:Label ID="Label3" runat="server" Text="|" ></asp:Label>
          <asp:Button ID="TabPassCarOcrCount"   runat="server"
                CssClass="TabButton" OnClick="TabPassCarOcrCount_Click" ></asp:Button>
           <iframe id="IFRAME1" style="border-right: 0px solid; border-top: 0px solid; z-index: 110; left: 11px; border-left: 0px solid; width: 99%; border-bottom: 0px solid; position: absolute; top: 20px; height: 90%"
                runat="server"></iframe>
    </div>
    </form>
</body>
</html>
