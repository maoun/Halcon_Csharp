<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllAnalysisMain.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Count.AllAnalysisMain" %>

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
            top: -5px;
            left: 180px;
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

        #Label2 {
            z-index: 104;
            position: absolute;
            top: -5px;
            left: 320px;
            width: 10px;
            font-size: 20px;
        }

        #TabIllegalVehicleProperty {
            z-index: 105;
            position: absolute;
            top: 0px;
            left: 323px;
            width: 165px;
        }

        #Label3 {
            z-index: 106;
            position: absolute;
            top: -5px;
            left: 490px;
            width: 10px;
            font-size: 20px;
        }

        #TabWfDrivingage {
            z-index: 107;
            position: absolute;
            top: 0px;
            left: 495px;
            width: 150px;
        }

        #Label4 {
            z-index: 108;
            position: absolute;
            top: -5px;
            left: 650px;
            width: 10px;
            font-size: 20px;
        }

        #TabWfDrivingExperience {
            z-index: 109;
            position: absolute;
            top: 0px;
            left: 650px;
            width: 160px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function changecolor()
        {
            this.document.getElementById('<%= TabIllegalPlateType.ClientID %>').style.color="red";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
                <div>
                    <asp:Button ID="TabIllegalPlateType" runat="server"
                        CssClass="TabButton" OnClientClick="changecolor()" OnClick="TabIllegalPlateType_Click"></asp:Button>
                    <asp:Label ID="Label1" runat="server" Text="|"></asp:Label>
                    <asp:Button ID="TabIllegalVehicleType" runat="server"
                        CssClass="TabButton" OnClick="TabIllegalVehicleType_Click"></asp:Button>
                    <asp:Label ID="Label2" runat="server" Text="|"></asp:Label>
                    <asp:Button ID="TabIllegalVehicleProperty" runat="server"
                        CssClass="TabButton" OnClick="TabIllegalVehicleProperty_Click"></asp:Button>
                    <asp:Label ID="Label3" runat="server" Text="|"></asp:Label>
                    <asp:Button ID="TabWfDrivingage" runat="server"
                        CssClass="TabButton" OnClick="TabWfDrivingage_Click"></asp:Button>
                    <asp:Label ID="Label4" runat="server" Text="|"></asp:Label>
                    <asp:Button ID="TabWfDrivingExperience" runat="server"
                        CssClass="TabButton" OnClick="TabWfDrivingExperience_Click"></asp:Button>
                    <iframe id="IFRAME1" style="border-right: 0px solid; border-top: 0px solid; z-index: 110; left: 11px; border-left: 0px solid; width: 99%; border-bottom: 0px solid; position: absolute; top: 20px; height: 90%"
                        runat="server"></iframe>
                </div>
    </form>
</body>
</html>