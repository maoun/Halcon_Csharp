<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Login" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <title>登录页面</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Styles/login.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-1.8.0.js" charset="utf-8"></script>
    <script type="text/javascript" language="javascript" src="../Scripts/common.js" charset="utf-8"></script>
    <script type="text/javascript">
        //document.onkeydown = function (e) {
        //    var code;
        //    if (!e) { var e = window.event; }
        //    if (e.keyCode) { code = e.keyCode; }
        //    else if (e.which) { code = e.which; }
        //    //BackSpace 8;
        //    if ((event.keyCode == 8) && ((event.srcElement.type != "text" && event.srcElement.type != "textarea" && event.srcElement.type != "password") || event.srcElement.readOnly == true)) {
        //        event.keyCode = 0;
        //        event.returnValue = false;
        //    }
        //    if (event.keyCode == 13) { //网页内按下回车触发
        //        Login();
        //        return false;
        //    }
        //    return true;
        //};
    </script>
    <%--登录事件--%>
    <script type="text/javascript" language="javascript">
        function Login() {
            var username = document.getElementById("txtUsername").value;
            var pwd = document.getElementById("txtPwd").value;
            var keep = '0';
            if (username == null || username == undefined || username == '') {
                alert('<%# GetLangStr("Login3","请输入用户名")%>'); return;
            }
            if (pwd == null || pwd == undefined || pwd == '') {
                alert("请输入密码"); return;
            }
            if (document.getElementById("cbkKeep").checked) { keep = '1'; }
            if (window.screen.width == 1920) {
                Login.SetScreen("1");//大屏幕
            } else if (window.screen.width == 1600) {
                Login.SetScreen("2");//中屏幕
            } else if (window.screen.width <= 1366) {
                Login.SetScreen("3");//小屏幕
            } else {
                Login.SetScreen("3");//小屏幕
            }
            Login.UserLogin(username, pwd, keep);

        }
        function Check(str) {
            document.getElementById("txtPwd").value = str;
            document.getElementById("cbkKeep").checked = true;
        }
        function NoCheck() {
            document.getElementById("cbkKeep").checked = false;
        }
    </script>
    <%--基于jquery的回车事件 兼容IE与火狐--%>
    <script type="text/javascript">
        document.onkeydown = function (e) {
            var theEvent = window.event || e;
            var code = theEvent.keyCode || theEvent.which;
            if (code == 13) {
                $("#btnLogin").click();
            }
        }
    </script>
    <script type="text/JavaScript">
        function fullscreen() {
            var width = window.screen.availWidth;
            var height = window.screen.availHeight;
            window.moveTo(0, 0);
            window.resizeTo(width, height);
        }
        function ReSize() {

        }
    </script>
</head>
<body onload="fullscreen()" onresize="ReSize();">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="Login" />
    <div class="box" style="position: absolute; width: 100%; height: 100%; z-index: -1">
        <img style="position: fixed;" src="../Images/login/bg.png" height="100%" width="100%" alt="#" />
        <div class="left"></div>
        <div class="center">
            <div class="yuan">
                <img src="../Images/login/yuan.png" height="470" alt="#" />
            </div>
            <div class="logo">
                <img src="../Images/login/logomain.png" width="103" height="110" alt="#" />
            </div>
            <div class="title"><%# GetLangStr("Login1","智慧车行云应用系统") %></div>
            <input class="Username" type="text" placeholder="用户名" id="txtUsername" runat="server" />
            <input class="Pwd" type="password" placeholder="密&nbsp;&nbsp;&nbsp;码" id="txtPwd" runat="server" />
            <div class="rember">
                <input type="checkbox" style="margin-right: 3px" runat="server" id="cbkKeep" />记住用户名和密码
            </div>
            <input class="txt3" type="button" value="<%#GetLangStr("Login2","登录") %>" id="btnLogin" onclick="Login();" />
            <div class="foot-sub">
                <center>
                    <p>北京尚易德科技有限公司版权所有</p>
                    <span>Copuright©2016 sunnsingtech.com</span></center>
            </div>
            <div class="foot">
                地址：北京市石景山区阜石路165号华录大厦8层 邮编：100043 电话：400-1616-123 传真：010-52281009 京ICP备11037434号-2
            </div>
        </div>
        <div class="right"></div>
    </div>
</body>
</html>