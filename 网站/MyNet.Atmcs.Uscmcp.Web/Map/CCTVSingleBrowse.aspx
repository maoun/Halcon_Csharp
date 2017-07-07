<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CCTVSingleBrowse.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.CCTVSingleBrowse" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>视频监控浏览</title>
       <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <link href="Css/chooser.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        function SetUrl(videoUrl) {

            var obj = thisMovie("AtmcsWebActiveX");
            var str = window.location.href;
            var str1 = str.substring(7);
            var ipaddress = "";
            var parastr = str1.indexOf("/")
            if (parastr > 0) {

                ipaddress = str.substring(7, parastr + 7);

            }
            var arrmp = videoUrl.split("|");
            var szTitle = arrmp[0];
            var szIp = arrmp[1];
            var szPort = arrmp[2];
            var szChl = arrmp[3];;
            var szUser = arrmp[4];
            var szPwd = arrmp[5];
            var szEncoderType = arrmp[6];
            var url = "http://" + ipaddress + "/DataService/DataService.asmx";
            obj.SetServiceUrl(url);
            obj.SetPlay(szTitle, szIp, szPort, szChl, szUser, szPwd, szEncoderType);
            obj.SetFormName("SINGLEMAIN");

        }
        //       function SetUrl(videoUrl) {
        //
        //           var obj = thisMovie("AtmcsWebActiveX");
        //           var str = window.location.href;
        //           var str1 = str.substring(7);
        //           var ipaddress = "";
        //           var parastr = str1.indexOf("/")
        //           if (parastr > 0)
        //            {

        //               ipaddress = str.substring(7, parastr + 7);

        //           }

        //           var arrmp = videoUrl.split("|");
        //           var szTitle ="123";
        //           var szIp = "10.54.138.204";
        //           var szPort = "37777";
        //           var szChl = "0"; ;
        //           var szUser = "admin";
        //           var szPwd = "admin";
        //           var szEncoderType = "4";
        //           var url = "http://10.54.138.201/DataService/DataService.asmx";
        //           obj.SetServiceUrl(url);
        //           obj.SetPlay(szTitle, szIp, szPort, szChl, szUser, szPwd, szEncoderType);
        //           obj.SetFormName("SINGLEMAIN");

        //       }
        function thisMovie(movieName) {
            var app = navigator.appName;
            var verStr = navigator.appVersion;
            if (app.indexOf('Netscape') != -1) {
                return window[movieName];
            }
            else if (app.indexOf('Microsoft') != -1) {

                return document.getElementById(movieName);
            }
        }
    </script>
    <style type="text/css">
          #cboplate_Panel1 table{
            border-radius:0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate></ContentTemplate>
        </asp:UpdatePanel>
        <div style="position: absolute; z-index: auto; top: 0px; left: 0px; width: 100%; height: 100%;">
            <object id="AtmcsWebActiveX" classid="clsid:956F226D-0AEB-45f9-9DBC-7815C5FD0F70" style="position: absolute; z-index: auto; top: 0px; left: 0px; width: 100%; height: 100%;"></object>
        </div>
    </form>
</body>
</html>
