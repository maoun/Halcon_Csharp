<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="EchartTest.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
<link rel="stylesheet" type="text/css" href="../Style/customMap.css" />
    <script src="js/common.js"></script>
<style type="text/css">
    body, html
    {
        font-family: Arial,Verdana;
        font-size: 13px;
        margin: 0;
        overflow: hidden;
    }
    #map_canvas
    {
        left: 0;
        top: 0;
        width: 100%; 
        height: 100%;
        position: absolute;
    }
</style>
    <script type="text/javascript">
        function Dianji() {
            OpenBigImg('http://192.168.1.244:8080/imageServer/image/1/20160816090425276[1]_1.jpg');
        }
     
    </script>
<script type="text/javascript" src="js/bmapFile.js"></script>
<script type="text/javascript" src ="js/BMapExt/BMapExt.js"></script>
<script type="text/javascript" src="js/bmap.js"></script>
<script type="text/javascript" src="js/Heatmap_min.js" ></script>
<script type="text/javascript" src="build/dist/echarts.js"></script>
 <script type="text/javascript" src="js/jquery-1.7.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
     <div style="width: 100%; height: 100%; border: 1px solid gray" id="map_canvas"> </div>
        <button onclick="Dianji();">点击</button>
    </form>
</body>
</html>
