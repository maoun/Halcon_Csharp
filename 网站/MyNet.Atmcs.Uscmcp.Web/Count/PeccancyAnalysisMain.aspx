<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeccancyAnalysisMain.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Count.PeccancyAnalysisMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=GBK"/>
    <title></title>
    <link href="../Styles/tabStyle.css" rel="stylesheet" />
     <style type="text/css">
        #TabWfxw {
            z-index: 101; position: absolute; top: 0px;left: 10px;width: 110px;
        }
        #Label1 {
            z-index: 102; position: absolute; top: -5px; left: 120px; width: 10px;font-size:20px
        }
        #TabPenishTypeAnalysis {
            z-index: 103; position: absolute; top: 0px;left: 120px; width: 110px;
        }
        #Label2 {
            z-index: 104; position: absolute; top: -5px; left: 230px; width: 10px;font-size:20px
        }
        #TabIllegalScoreAnalysis {
            z-index: 105; position: absolute; top: 0px;left: 235px; width: 120px;
        }
           #Label3 {
            z-index: 106; position: absolute; top: -5px; left: 360px; width: 10px;font-size:20px
        }
        #TabIllegalPeriodAnalysis {
            z-index: 107; position: absolute; top: 0px;left: 363px; width: 110px;
        }
            #Label4 {
            z-index: 108; position: absolute; top: -5px; left: 475px; width: 10px;font-size:20px
        }
        #TabWfPlateTopTen {
            z-index: 109; position: absolute; top: 0px;left: 473px; width: 180px;
        }
         #Label5 {
            z-index: 110; position: absolute; top: -5px; left: 650px; width: 10px;font-size:20px
        }
        #TabPeccancyCount {
            z-index: 111; position: absolute; top: 0px;left: 655px; width: 120px;
        }
            #Label6 {
            z-index: 110; position: absolute; top: -5px; left: 780px; width: 10px;font-size:20px
        }
        #TabPeccancyEnableCount {
            z-index: 111; position: absolute; top: 0px;left: 795px; width: 130px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:Button ID="TabWfxw" runat="server"
                CssClass="TabButton" OnClick="TabWfxw_Click"></asp:Button>
            <asp:Label ID="Label1" runat="server" Text="|" ></asp:Label>
            <asp:Button ID="TabPenishTypeAnalysis" runat="server"
                CssClass="TabButton" OnClick="TabPenishTypeAnalysis_Click" ></asp:Button>
             <asp:Label ID="Label2" runat="server" Text="|" ></asp:Label>
            <asp:Button ID="TabIllegalScoreAnalysis"   runat="server"
                CssClass="TabButton" OnClick="TabIllegalScoreAnalysis_Click" ></asp:Button>
         <asp:Label ID="Label3" runat="server" Text="|" ></asp:Label>
          <asp:Button ID="TabIllegalPeriodAnalysis"   runat="server"
                CssClass="TabButton" OnClick="TabIllegalPeriodAnalysis_Click" ></asp:Button>
         <asp:Label ID="Label4" runat="server" Text="|" ></asp:Label>
          <asp:Button ID="TabWfPlateTopTen"   runat="server"
                CssClass="TabButton"  OnClick="TabWfPlateTopTen_Click"></asp:Button>
          <asp:Label ID="Label5" runat="server" Text="|" ></asp:Label>
          <asp:Button ID="TabPeccancyCount"   runat="server"
                CssClass="TabButton"  OnClick="TabPeccancyCount_Click"></asp:Button>
          <asp:Label ID="Label6" runat="server" Text="|" ></asp:Label>
          <asp:Button ID="TabPeccancyEnableCount"   runat="server"
                CssClass="TabButton"  OnClick="TabPeccancyEnableCount_Click"></asp:Button>
           <iframe id="IFRAME1" style="border-right: 0px solid; border-top: 0px solid; z-index: 112; left: 11px; border-left: 0px solid; width: 99%; border-bottom: 0px solid; position: absolute; top: 20px; height: 90%"
                runat="server"></iframe>
    </div>
    </form>
</body>
</html>
