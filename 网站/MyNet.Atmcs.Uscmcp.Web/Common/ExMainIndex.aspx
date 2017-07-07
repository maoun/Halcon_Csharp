<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExMainIndex.aspx.cs" Inherits="MyNet.Atmcs.Web.ExMainIndex" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Import Namespace="MyNet.Atmcs.Uscmcp.Bll" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>弹出窗体</title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
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
    </style>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.10.4.min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.mousewheel.min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.mCustomScrollbar.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.dad.min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/common.js" charset="UTF-8"></script>
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
        function MenuItemClick(url) {
            //左侧选择模板点击回收事件
            $("#PanelLeftMenu").addClass('active').animate({ "left": "-258px" });
            $("#seletMenu").removeClass('active').animate({ "left": "0px" });
            if (url.indexOf("Template") >= 0) {
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
   
    <%--个人中心--%>
    <script type="text/javascript">
        var selectedSrc = "";
        var usedSrc = "";
        window.onload=function getBodyImageUrl()
        {
            var back = $('body').css('backgroundImage');
            var i = back.replace("//", "").indexOf("/");
            usedSrc = ".." + back.replace("//", "").substring(i).replace("\")", "");
        }
     
       
        
    </script>
</head>
<body runat="server" id="bd">
    <ext:ResourceManager runat="server" ID="ResourceManager1" DirectMethodNamespace="ExMainIndex" />
    <form id="form1" runat="server">
        <ext:Hidden ID="hiddenContentCode" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="CurrentSelectMenu" runat="server">
        </ext:Hidden>
        <ext:Viewport runat="server" Layout="border" ID="viewportMain">
            <Items>
                <%--左侧--%>
                <ext:FormPanel ID="PanelLeft" runat="server" Region="West" Layout="FitLayout" Width="130" Split="false" Margins="80px 0 0 0">
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
                                </ext:Panel>
                            </Center>
                        </ext:BorderLayout>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
      
    </form>
</body>

<script type="text/javascript">
    function Show() {
        $("#ImgFile-file").click();
    }
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
                $(this).stop(true, true).addClass("activeenter").animate({ width: '130px' }, "fast").find("a").css({ "padding-left": "8px", "font-size": "18px", "text-decoration": "none", "color": "white", "background-position": oPositionV, "margin-left": "66px", "text-indent": "-130px" });

                $(this).click(function () {
                    $(this).removeClass("activeenter");

                    var oPs = -$("#PanelLeft .ex-panel-background.active").children("span").attr("name");
                    var oPos = "0 " + oPs + "px";

                    $("#PanelLeft .ex-panel-background.active").removeClass("active").stop(true, true).animate({ width: '66px' }).children("span").css("width", "66px").find("a").css({ width: '66px', "height": "60px", "font-size": "0px", "display": "block", "background-position": oPos, "margin-left": "0px", "padding-left": "0px" });

                    var oPosition = -$(this).children("span").attr("name") - 60;
                    var oPositionV = "0 " + oPosition + "px";

                    var oPs = -$(this).children("span").attr("name");
                    var oPos = "0 " + oPs + "px";
                    $(this).stop(true, true).addClass("active").animate({ width: '130px' }, "fast").find("a").css({ "padding-left": "8px", "font-size": "18px", "text-decoration": "none", "color": "white", "background-position": oPositionV, "margin-left": "66px", "text-indent": "-130px" });

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
</html>
