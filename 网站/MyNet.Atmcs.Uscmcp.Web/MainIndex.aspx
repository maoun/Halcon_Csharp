<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainIndex.aspx.cs" Inherits="MyNet.Atmcs.Web.MainIndex" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Import Namespace="MyNet.Atmcs.Uscmcp.Bll" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>立体化社会治安防控系统</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Styles/index.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/base.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Styles/jquery.dad.css" />
    <link rel="stylesheet" href="../Styles/perfect-scrollbar.css" />
    <link href="../Styles/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ex-panel-background {
            background-color: rgba(255, 255, 255, 0.2);
            -webkit-transition: all 0.3s linear;
            -o-transition: all 0.3s linear;
            -moz-transition: all 0.3s linear;
            -ms-transition: all 0.3s linear;
            transition: all 0.3s linear;
        }

            .ex-panel-background:hover, .ex-panel-background.active {
                background: rgba(251, 80, 4, 1) !important;
                background: #fb5004;
                width: 130px !important;
                height: 60px;
                display: block;
                line-height: 60px;
                text-align: center;
            }

            .ex-panel-background span {
                color: white;
                font-family: 楷体;
                font-size: 18px;
            }

        .ex-panel-morelist {
            position: relative;
            top: 90px;
            left: 0px;
        }

        .ex-panel {
            border-style: solid;
            border-color: transparent;
            border-width: 0;
        }

        .ex-center-background {
            background: transparent;
        }

        .ext-top-background {
            background: transparent;
        }

        .ex-center-PanelCenter {
            position: relative;
        }

        #PanelLeft .ex-panel-background span.avtivees {
            background: rgba(251, 80, 4, 1) !important;
            filter: Alpha(opacity=100);
            background: #fb5004;
        }

        #ulmoremenu li.active span {
            color: #fb5004 !important;
        }

        #seletMenu {
            background-color: rgba(242,243,243,0.1);
            background-image: url(../Images/left/menuico0101.png);
            background-repeat: no-repeat;
            background-position: 92px 16px;
        }

            #seletMenu span {
                font-size: 16px;
                line-height: 50px;
                color: #274047;
                margin-left: 10px;
            }

            #seletMenu.active {
                background-color: #fc5004;
                background-image: url(../Images/left/menuico0101_1.png);
                background-repeat: no-repeat;
                background-position: 92px 16px;
            }

                #seletMenu.active span {
                    color: white;
                }

        #PanelLeftMenu li img.active + span {
            color: #fc5004 !important;
        }

        .custom {
            background-color: gray;
            /*border: solid 5px gray;*/
        }

            .custom .x-grouptabs-corner {
                background-image: none;
            }

            .custom ul.x-grouptabs-strip li.x-grouptabs-strip-active {
                background: #DBDBDB;
                border: none !important;
            }

            .custom ul.x-grouptabs-strip li.x-grouptabs-main {
                border: solid 1px white;
            }

            .custom li.x-grouptabs-strip-active ul.x-grouptabs-sub li.x-grouptabs-strip-active {
                background-color: white;
            }
        /*个人中心css*/
        #person ul li:first-child {
            border-top: 0;
        }

        #person ul li {
            width: 165px;
            height: 37.5px;
            border-top: 1px solid silver;
            line-height: 37.5px;
            text-align: center;
            cursor: pointer;
        }

            #person ul li a {
                text-decoration: none;
            }

            #person ul li:hover {
                background: #fc5004;
            }

                #person ul li:hover a {
                    color: white;
                }

        #person {
            display: none;
            width: 165px;
            height: 266px;
            position: absolute;
            top: 90px;
            right: 50px;
        }

        #li3 {
            display: none;
            width: 610px;
            height: 370px;
            position: absolute;
            top: 200px;
            left: 50%;
            margin-left: -305px;
        }

        .top {
            width: 610px;
            height: 40px;
            background: rgba(226,240,249,0.95);
            line-height: 40px;
            text-align: center;
            font-size: 16px;
        }

        .container {
            width: 610px;
            height: 329px;
            border-top: 1px solid silver;
            background: rgba(226,240,249,0.95);
        }

        .tableUserInfo {
            margin: auto;
            margin-top: 15px;
            border-collapse: separate;
            border-spacing: 10px;
            text-align: left;
        }

        .tableUserInfo2 {
            margin: auto;
            margin-top: 15px;
            border-collapse: separate;
            border-spacing: 10px;
        }

        .tableDownLoad {
            border-collapse: separate;
            border-spacing: 2px;
        }

            .tableDownLoad th {
                font-size: 18px;
                color: gray;
                padding-top: 8px;
                padding-bottom: 15px;
            }

            .tableDownLoad td {
                color: blue;
            }

        .tableUserInfo td {
            font-size: 16px;
        }

        .tableUserInfo2 td {
            font-size: 16px;
        }

        .title {
            position: absolute;
            top: 17px;
            left: 270px;
            font-size: 16px;
            font-weight: 500;
            color: black;
        }

        .backImage {
            width: 185px;
            height: 110px;
            float: left;
            margin: 4px;
            border: 1px solid gray;
        }
        /*左侧菜单矩形展示*/
        .x-btn {
            cursor: pointer;
            white-space: nowrap;
            border-radius: 0px !important;
            border: 0px !important;
        }

        #PanelHead {
            overflow: visible !important;
        }

        #PanelLeft_Content {
            width: 100%;
            height: 100%;
        }
    </style>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.10.4.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.mousewheel.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.mCustomScrollbar.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.dad.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/public.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/common.js" charset="utf-8"></script>
    <!--[if lt IE 10]>
     <script type="text/javascript" src="js/PIE.js"></script>
     <![endif]-->
    <script type='text/javascript'>
        (function ($) {
            $(window).load(function () {
                $(".content").mCustomScrollbar();
            });
        })(jQuery);
    </script>
    <script type="text/javascript">
        var personalClick = function () { document.getElementById("Search").click(); }
    </script>

    <script type="text/javascript">
        function MenuItemClick(url) {
            //左侧选择模板点击回收事件
            $("#PanelLeftMenu").addClass('active').animate({ "left": "-258px" });
            $("#seletMenu").removeClass('active').animate({ "left": "0px" });
            if (url.indexOf("Template") >= 0) {
            }
            if (url.indexOf("../") < 0) {
                url = document.getElementById("JavaText").value + url;
            }
            CenterPanel.autoLoad.url = url;
            CenterPanel.reload();
        }
    </script>
    <script type="text/javascript">
        function MainmenuItemClick(id) {
            if (id != '0101') {
                $("#TemplatePanel").css("display", "none");
            }
            else {
                $("#TemplatePanel").css("display", "block");
            }
            MainIndex.MainMenuItemClick(id);
            $("#nav .item").click(function () {
                if (!$(this).hasClass("activees")) {
                    $("#nav .item.activees").removeClass("activees");
                    $(this).addClass("activees");
                    $("#ulmoremenu li.active").removeClass("active");
                }
            })
        }
    </script>
    <%--更多点击事件--%>
    <script type="text/javascript">
        $(function () {
            $("#nav").each(function () {
                $(this).find(".item").click(function () {
                    $(".morelist").css("display", "none");
                    $("#CenterPanel").css("margin-top", "0px");
                })
            })
        })
    </script>
    <%--个人中心--%>
    <script type="text/javascript">
        var selectedSrc = "";
        var usedSrc = "";
        window.onload = function getBodyImageUrl() {
            var back = $('body').css('backgroundImage');
            var i = back.replace("//", "").indexOf("/");
            usedSrc = ".." + back.replace("//", "").substring(i).replace("\")", "");
        }
        //修改密码
        function pwdUpadte() {
            MainIndex.AddWindowModify();
        }
        //用户信息
        function userInfo() {
            MainIndex.userInfoShow();
        }
        //控件下载
        function downLoad() {
            MainIndex.downLoadShow();
        }
        //背景图片修改
        function changeBack() {
            MainIndex.backImageShow();
        }
        //关于我们
        function aboutMe() {
            MainIndex.aboutMeShow();
        }
        //用户手册
        function userManual() {
            OpenWPDFPage("../Common/UserGuide.aspx");
        }
        function ScreenDetect() {
            OpenScreenPage("../Common/CSBrowse.aspx?funcid=screen");
        }
        //选中图片事件
        function getImageSrc(id, src) {
            selectedSrc = src;
            $("#divImages img").css("border", "1px solid gray");
            $("#" + id).css("border", "1px solid red");
            $("body").css("background-image", "url(" + src + ")");
        }
        //保存用户背景图片
        function urlSave() {
            if (selectedSrc == "") {
                //alert("请选择要保存的图片");
                WindowChangeBack.hide();
            }
            else {
                MainIndex.imageSave(selectedSrc);
                usedSrc = selectedSrc;
                selectedSrc = "";
            }
        }
        //背景设置页取消
        function Cancel() {
            selectedSrc = "";
            MainIndex.windowCancel();
            usedSrc = hidDefaultBack.value;
            if (usedSrc != "") {
                $("body").css("background-image", "url(" + usedSrc + ")");
            }
        }
        // 还原默认背景页
        function SetDefaultBackGround() {
            usedSrc = hidDefaultBack.value;
            if (usedSrc != "") {
                $("body").css("background-image", "url(" + usedSrc + ")");
            }
            selectedSrc = "";
        }
        //删除背景
        function Delete() {
            if (selectedSrc == "") {
                alert("请选择要删除的图片");
            }
            else {
                MainIndex.windowDelete(selectedSrc);
                if (usedSrc != "") {
                    $("body").css("background-image", "url(" + usedSrc + ")");
                }
            }
        }
        function exit() {
            MainIndex.Exit();
        }
        function addImage(imgSrc) {
            var divImage = document.getElementById("divImages");
            var imgNode = document.createElement('img');
            imgNode.setAttribute('src', imgSrc);
            imgNode.setAttribute('class', 'backImage');
            imgNode.onclick = "getImageSrc(id,src)";
            divImage.appendChild(imgNode);
        }
        function OpenWPDFPage(url) {
            var left = (window.window.outerWidth - width) / 2;
            var top = (window.window.outerHeight - height) / 2;
            var width = window.screen.availWidth * 0.8;
            var height = window.screen.availHeight;
            window.showModalDialog(url, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px;dialogLeft:" + left + "px;dialogTop:" + top + "px;Status:no;help:no;location:no;scroll:yes;resizable:no;");
        }
    </script>
</head>
<body runat="server" id="bd">
    <ext:ResourceManager runat="server" ID="ResourceManager1" DirectMethodNamespace="MainIndex" />
    <form id="form1" style="position: relative; top: 0px!important" runat="server">
        <input id="JavaText" type="text" style="display: none" runat="server" />
        <ext:Hidden ID="hiddenContentCode" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="CurrentSelectMenu" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="hidScreenNum" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="hidScreen1" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="hidScreen2" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="hidScreen3" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="hidScreen4" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="hidScreen5" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="hidScreen6" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="hidScreen7" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="hidScreen8" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="hidScreen9" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="hidDefaultBack" runat="server">
        </ext:Hidden>
        <ext:Viewport runat="server" Layout="border" ID="viewportMain">
            <Items>
                <%--上方--%>
                <ext:Panel runat="server" Region="North" ID="PanelHead" StyleSpec="height:90px">
                    <Content>
                        <div class="screenlist" style="width: 100%; height: 160px; background: rgba(0, 0, 0, 0.15)"></div>
                        <div id="personalCenter" runat="server" class="header">
                            <div class="logo">
                                <a href="#">
                                    <img src="Image/logo.png" alt=""></a>
                            </div>
                            <ul class="nav dad-container" runat="server" id="nav">
                            </ul>
                            <div runat="server" id="personlCenter" class="member"></div>
                        </div>
                    </Content>
                </ext:Panel>
                <ext:Panel runat="server" Cls="ex-panel-morelist">
                    <Content>
                        <div class="morelist">
                            <ul runat='server' id="ulmoremenu">
                            </ul>
                        </div>
                    </Content>
                </ext:Panel>

                <%--左侧--%>
                <ext:FormPanel ID="PanelLeft" runat="server" Region="West" Layout="FitLayout" Width="130" Split="false" Margins="0px 0 0 0">
                    <Content>
                        <div runat="server" id="TemplatePanel" style="display: block">
                        </div>
                    </Content>
                </ext:FormPanel>
                <%--中间--%>
                <ext:Panel runat="server" Region="Center" Cls="ex-center-PanelCenter">
                    <Items>
                        <ext:BorderLayout runat="server">
                            <Center>
                                <ext:Panel ID="CenterPanel"
                                    runat="server"
                                    Closable="true"
                                    Border="false"
                                    Padding="6" Cls="ex-center-background">
                                    <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
                                    <Content>
                                        <%--点击个人中心展开--%>
                                        <div id="person">
                                            <ul style="width: 165px; height: 304px; background: rgba(249, 250, 253,0.8)">
                                                <li onclick="userInfo()"><a href="#"><%# GetLangStr("AlarmInfoCount1","用户信息") %></a></li>
                                                <li onclick="pwdUpadte()"><a href="#"><%# GetLangStr("AlarmInfoCount2","修改密码") %></a></li>
                                                <li onclick="exit()"><a href="#"><%# GetLangStr("AlarmInfoCount3","注销登录") %></a></li>
                                                <li onclick="ScreenDetect()"><a href="#"><%# GetLangStr("AlarmInfoCount4","检测屏幕") %></a></li>
                                                <li onclick="downLoad()"><a href="#"><%# GetLangStr("AlarmInfoCount5","下载控件") %></a></li>
                                                <li onclick="changeBack()"><a href="#"><%# GetLangStr("AlarmInfoCount6","更换背景") %></a></li>
                                                <li onclick="aboutMe()"><a href="#"><%# GetLangStr("AlarmInfoCount7","关于我们") %></a></li>
                                                <li onclick="userManual()"><a href="#"><%# GetLangStr("AlarmInfoCount8","用户手册") %></a></li>
                                            </ul>
                                        </div>
                                    </Content>
                                </ext:Panel>
                            </Center>
                        </ext:BorderLayout>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
        <%--用户信息--%>
        <ext:Window ID="PanelUserInfo" Modal="true" runat="server" Hidden="true" Height="369px"
            Width="610px" Title="<p class='title'>用户信息</p>" Resizable="false">
            <Content>
                <hr style="height: 1px; border: none; border-top: 1px solid silver;" />
            </Content>
        </ext:Window>
        <%--控件下载--%>
        <ext:Window ID="PanelDowload" Modal="true" runat="server" Hidden="true" Height="369px"
            Width="610px" Title="<p class='title'>控件下载</p>" Resizable="false">
            <Content>
                <hr style="height: 1px; border: none; border-top: 1px solid silver;" />
                <div runat="server" id="tableDownload" style="overflow: auto">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td style="height: 50px; color: white; font-size: 20px;" valign="middle" align="center">
                                <a href="ActiveX/dotnetfx4.0.exe">运行环境.net4.0下载</a>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 50px; color: white; font-size: 20px;" valign="middle" align="center">
                                <a href="ActiveX/UscmcpWebActiveX.exe">控件下载</a>
                            </td>
                        </tr>
                    </table>
                </div>
            </Content>
        </ext:Window>
        <%--更换背景--%>
        <ext:Window ID="WindowChangeBack" runat="server" Hidden="true" Height="369px"
            Width="610px" Title="<p class='title'>更换背景</p>" Resizable="false">
            <Items>
                <ext:Panel ID="PanelChangeBack" runat="server" Width="610">
                    <Content>
                        <hr style="height: 1px; border: none; border-top: 1px solid silver;" />
                        <div runat="server" id="divBackImage" style="padding-left: 5px; height: 240px; overflow: auto">
                            <div id="upload" style="width: 185px; height: 110px; float: left; margin: 4px">
                                <ext:Image runat="server" ImageUrl="../images/persons/noname-2.png" ID="imgShow" Width="185px" Height="110px">
                                    <Listeners>
                                        <Click Fn="Show" />
                                    </Listeners>
                                </ext:Image>
                                <ext:FileUploadField ID="ImgFile" runat="server" ButtonOnly="true" ButtonText="选择" Hidden="true">
                                    <DirectEvents>
                                        <FileSelected OnEvent="FileUploadSelect">
                                        </FileSelected>
                                    </DirectEvents>
                                </ext:FileUploadField>
                            </div>
                            <div runat="server" id="divImages"></div>
                        </div>
                        <div style="text-align: center; clear: left">
                            <br />
                            <input type="button" style="width: 120px; height: 30px; border-style: none; margin-right: 20px; background-image: url(images/persons/noname-1.png)" value="确定" onclick="urlSave()" />
                            <input id="cancle" type="button" style="width: 120px; height: 30px; border-style: none; margin-right: 20px; background-image: url(images/persons/noname-1.png)" value="取消" onclick="Cancel()" />
                            <input id="delete" type="button" style="width: 120px; height: 30px; border-style: none; margin-right: 20px; background-image: url(images/persons/noname-1.png)" value="删除" onclick="Delete()" />
                        </div>
                    </Content>
                </ext:Panel>
            </Items>
        </ext:Window>
        <%--关于我们--%>
        <ext:Window ID="Window3" Modal="true" runat="server" Hidden="true" Height="375px"
            Width="610px" Title="<p class='title'>关于我们</p>" Resizable="false">
            <Content>
                <hr style="height: 1px; border: none; border-top: 1px solid silver;" />
                <div runat="server" id="copanyIntroduce" style="text-align: center">
                </div>
            </Content>
        </ext:Window>
    </form>
</body>

<script type="text/javascript">
    function Show() {
        $("#ImgFile-file").click();
    }
</script>
<script type="text/javascript">
    /*点击显示更多*/
    $("#morelist").click(function () {

        if ($(".morelist").css("display") == 'block') {
            $("#CenterPanel").css("margin-top", "0px")

        } else {

            $("#CenterPanel").css("margin-top", "0px")
        }
        $(".morelist").fadeToggle("100");

    });
</script>
<script type="text/javascript">
    $("#nav").each(function () {
        $(this).find(".item").click(function () {
            /*点击头部右侧内容推送*/
            $("#CenterPanel").each(function () {
                $("#CenterPanel").css("margin-left", "100%")
            })
            $("#CenterPanel").stop(true).animate({ "margin-left": '0px' }, 500)
        })
    })
    $("#ulmoremenu").each(function () {
        $(this).find("span").click(function () {
            /*点击更多右侧内容推送*/
            $("#CenterPanel").each(function () {
                $("#CenterPanel").css("margin-left", "100%")
            })
            $("#CenterPanel").stop(true).animate({ "margin-left": '0px' }, 500)
        })
    })
</script>
<%--点击效果--%>
<script type="text/javascript" language="javascript">
    $(function () {

        $("#ulmoremenu li").click(function () {
            //判断当前选中元素是有有active
            if (!$(this).hasClass("active")) {
                //判断带ICON图标的导航菜单有无activees
                if ($("#nav .item.activees").length == 1) {

                    var a = $("#nav .item.activees").children("span").css("background-position");
                    var x = parseInt(a.substring(0, a.indexOf(" ")));
                    var y = parseInt(a.substring(a.indexOf(" "), a.length)) + 70;
                    $("#nav .item.activees").removeClass("activees").children("span").css("background-position", x + "px " + y + "px");
                }
                //判断点击更多后下发菜单是否有active
                if ($("#ulmoremenu li.active").length == 1) {
                    $("#ulmoremenu li.active").removeClass("active").children("span").css("color", "#000");
                }
                $(this).addClass("active").children("span").css("color", "#fb5004");;
                ////
            }
        })
        $("#ulmoremenu li").mousemove(function () {
            if (!$(this).hasClass("active")) {
                $(this).addClass("activeenter").children("span").css("color", "#fb5004")
                $("#ulmoremenu .activeenter").mouseleave(function () {
                    //鼠标离开时  获取移入class 执行离开事件
                    if ($("#ulmoremenu .activeenter").length == 1) {
                        $(this).removeClass("activeenter").children("span").css("color", "#000")
                    }
                })
            }
        })
    })
</script>
<script language="javascript" type="text/javascript">
    jQuery(function ($) {
        $("body").delegate("#PanelLeft .ex-panel-background", "mouseenter", function () {
            if (!$(this).hasClass("active")) {

                var oPosition = -$(this).children("span").attr("name") - 60;
                var oPositionV = "0 " + oPosition + "px";

                var oPs = -$(this).children("span").attr("name");
                var oPos = "0 " + oPs + "px";
                $(this).stop(true, true).addClass("activeenter").animate({ width: '130px' }, "fast").find("a").css({ "padding-left": "8px", "font-size": "18px", "line-height": "60px", "text-decoration": "none", "color": "white", "background-position": oPositionV, "margin-left": "66px", "text-indent": "-130px" });

                $(this).click(function () {
                    $(this).removeClass("activeenter");

                    var oPs = -$("#PanelLeft .ex-panel-background.active").children("span").attr("name");
                    var oPos = "0 " + oPs + "px";

                    $("#PanelLeft .ex-panel-background.active").removeClass("active").stop(true, true).animate({ width: '66px' }).children("span").css("width", "66px").find("a").css({ width: '66px', "height": "60px", "line-height": "60px", "font-size": "0px", "display": "block", "background-position": oPos, "margin-left": "0px", "padding-left": "0px" });

                    var oPosition = -$(this).children("span").attr("name") - 60;
                    var oPositionV = "0 " + oPosition + "px";

                    var oPs = -$(this).children("span").attr("name");
                    var oPos = "0 " + oPs + "px";
                    $(this).stop(true, true).addClass("active").animate({ width: '130px' }, "fast").find("a").css({ "padding-left": "8px", "font-size": "18px", "text-decoration": "none", "line-height": "60px", "color": "white", "background-position": oPositionV, "margin-left": "66px", "text-indent": "-130px" });

                })

                $("#PanelLeft .activeenter").mouseleave(function () {
                    if ($("#PanelLeft .activeenter").length == 1) {

                        $($(this)).stop(true, true).removeClass("activeenter").animate({ width: '66px' }).find("a").css({ width: '66px', "height": "60px", "font-size": "0px", "display": "block", "background-position": oPos, "margin-left": "0px", "padding-left": "0px" });
                    }
                })
            }
        })
    })
</script>
<%--个人中心操作--%>
<script type="text/javascript">
    //个人中心点击显示隐藏
    $(".member").click(function () {
        var oDiv = $("#person");
        if (oDiv.css("display") == "none") {
            oDiv.css("display", "block");
        }
        else {
            oDiv.css("display", "none");
        }
    })
    //点击li展开li对应的页面
    $("#person li").click(function () {
        $("#person li").each(function () {
            var oLi = "li" + $(this).index();
            $("#" + oLi).css("display", "none");
        })
        $("#person").css("display", "none");
        var oLi = "li" + $(this).index();
        $("#" + oLi).css("display", "block");
    })
</script>

<script type="text/javascript">
    window.onload = function () {
        var screen = $("#hidScreenNum").val();
        if (screen >= 2) {
            webInit("Menulist");
            $(".nav").Jumpurl({ frer: screen });
        }
    }
</script>
</html>