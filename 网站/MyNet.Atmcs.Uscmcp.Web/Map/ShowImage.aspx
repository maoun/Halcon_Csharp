<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowImage.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Map.ShowImage" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=9" />
    <meta charset="utf-8" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta http-equiv="expires" content="0" />
    <style>
        #canvas {
            border: 1px solid #ccc;
        }
    </style>
    <script type="text/javascript">

        function OpenPic(url) {
            var ImageView = window.open('../Common/ImageVideo.html?imgUrl=' + url, 'ImageView', 'toolbar=no,location=no,menubar=no,scrollbars=no,resizable=yes,width=1240,height=700');
            ImageView.focus();
        };
        function OpenPicPage() {
            ShowImage.OpenImage();
        };
    </script>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="ShowImage" />
    <form id="form1" runat="server">
        <ext:Hidden runat="server" ID="urlimg" />
        <ext:FormPanel runat="server" ID="extForm">
            <Items>
                <ext:Label runat="server" ID="lblPassInfo" Text="" StyleSpec="font-size: 16px; font-weight:bold"></ext:Label>
            </Items>
        </ext:FormPanel>
        <div title="s12345" style="width: 25%; height: 546px; position: relative; right: 0; left: 0%; z-index: 98;">

            <%--  <div id="main" style="position: relative; overflow: hidden; width: 200px; height: 300px">--%>
            <div style="float: left;">
                <canvas id="imgurl_1" title="点击查看详细信息" onclick="OpenPicPage();"></canvas>
            </div>

            <div>
                <canvas id="imgurl_2" title="特征信息"></canvas>
            </div>
        </div>
    </form>
</body>
<script type="text/javascript">
    function getRootPath() {
        //获取当前网址，
        var curWwwPath = window.document.location.href;
        //获取主机地址之后的目录，
        var pathName = window.document.location.pathname;
        var pos = curWwwPath.indexOf(pathName);
        //获取主机地址，
        var localhostPaht = curWwwPath.substring(0, pos);
        //获取带"/"的项目名，
        var projectName = pathName.substring(0, pathName.substr(1).indexOf('/') + 1);
        return (localhostPaht + projectName);
    };
    /**
*row,代表的是表格中的选中的那一行数据，img1代表的是image对象1；ctx_1代表的是那个canvas中的画2D图的函数
*/
    function canvas_tu(imgUrls, clwx) {

        var img_1 = document.getElementById('imgurl_1');
        var ctx_1 = img_1.getContext('2d');
        var img_2 = document.getElementById('imgurl_2');
        var ctx_2 = img_2.getContext('2d');
        if (typeof (imgUrls) != "undefined" && imgUrls != "-" && imgUrls != "") {
            var img1 = imgUrls;
            var img = new Image();
            img.src = img1;
            if (typeof (clwx) != "undefined" && clwx != "-" && clwx != "") {
                var leg = clwx.split(",");
                img.onload = function () {
                    img_1.width = img.width;
                    img_1.height = img.height;
                    ctx_1.drawImage(img, 0, 0);
                    ctx_1.beginPath();
                    ctx_1.strokeStyle = '#067709';
                    ctx_1.lineWidth = 20;
                    ctx_1.strokeRect(parseInt(leg[0]), parseInt(leg[1]), parseInt(leg[2]), parseInt(leg[3]));
                    ctx_1.closePath();
                    img_2.width = leg[2];
                    img_2.height = leg[3];
                    ctx_2.drawImage(img, leg[0], leg[1], leg[2], leg[3], 0, 0, leg[2], leg[3]);
                    ctx_2.save();
                }

            } else {
                img.onload = function () {
                    img_1.width = img.width;
                    img_1.height = img.height;
                    ctx_1.drawImage(img, 0, 0);
                }
                var img1 = new Image();
                img1.src = "/pages/imgs/zanwu_image.png";

                img1.onload = function () {
                    img_2.width = img1.width;
                    img_2.height = img1.height;
                    ctx_2.drawImage(img1, 0, 0);
                }
            }

        } else {
            var img1_0 = new Image();
            img1_0.src = "/pages/imgs/zanwu_image.png";
            img1_0.onload = function () {
                img_1.width = img1_0.width;
                img_1.height = img1_0.height;
                ctx_1.drawImage(img1_0, 0, 0);
                img_2.width = img1_0.width;
                img_2.height = img1_0.height;
                ctx_2.drawImage(img1_0, 0, 0);
            }

        }
        document.getElementById("imgurl_1").style.width = "300px";
        document.getElementById("imgurl_2").style.width = "150px";
        document.getElementById("imgurl_1").style.height = "300px";
        document.getElementById("imgurl_2").style.height = "150px";
    };
    //canvas_tu("http://192.168.1.249:8001/capture/2016/01/01/12/100000010700/100000010700_其它_无牌_20160101125620_4_03_0_0_0.JPG", "100,100,100,100");
</script>
</html>