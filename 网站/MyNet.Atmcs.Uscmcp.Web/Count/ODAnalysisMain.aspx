<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ODAnalysisMain.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.ODAnalysisMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <title></title>
    <link href="../Styles/tabStyle.css" rel="stylesheet" />
    <style type="text/css">
        #TabIllegalPlateType {
            z-index: 101;
            position: absolute;
            top: 0px;
            left: 10px;
            width: 165px;
        }

        #Label1 {
            z-index: 102;
            position: absolute;
            top: 2px;
            left: 79px;
            width: 10px;
            font-size: 20px;
        }

        #TabIllegalVehicleType {
            z-index: 103;
            position: absolute;
            top: 0px;
            left: 178px;
            width: 150px;
        }

     

        #TabIllegalVehicleProperty {
            z-index: 105;
            position: absolute;
            top: 0px;
            left: 323px;
            width: 165px;
        }

    </style>

</head>
<body>
    <form id="form1" runat="server">
                <div>
                    <asp:Button ID="TabOD" runat="server"
                        CssClass="TabButton" OnClick="TabOD_Click"></asp:Button>
                    <asp:Label ID="Label1" runat="server" Text="|"></asp:Label>
                    <asp:Button ID="TabODConfig" runat="server"
                        CssClass="TabButton" OnClick="TabODConfig_Click"></asp:Button>
                    <iframe id="IFRAME1" style="border-right: 0px solid; border-top: 0px solid; z-index: 110; left: 11px; border-left: 0px solid; width: 99%; border-bottom: 0px solid; position: absolute; top: 30px; height: 90%"
                        runat="server"></iframe>
                </div>
    </form>
</body>
</html>