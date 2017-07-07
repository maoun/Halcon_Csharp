<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Blank.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Blank" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <link type="text/css" href="../Style/style.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ext:ResourceManager ID="ResourceManager1" runat="server" IDMode="Explicit" />
            <ext:FormPanel ID="Panel2" runat="server" Header="false" DefaultAnchor="100%">
                <Content>
                    <div>
                        <center>
                            <span>无权访问该页面</span>
                        </center>
                    </div>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Label ID="label1" runat="server">
                            </ext:Label>
                        </Items>
                    </ext:Toolbar>
                </Content>
            </ext:FormPanel>
        </div>
    </form>
</body>
</html>