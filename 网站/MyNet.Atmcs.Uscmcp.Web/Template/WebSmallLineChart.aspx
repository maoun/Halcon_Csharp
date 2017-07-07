<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebSmallLineChart.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.WebSmallLineChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <script src="../build/dist/echarts.js" charset="UTF-8"></script>
    <script type="text/javascript">
        require.config({
            paths: {
                echarts: '../build/dist'
            }
        });
        require(
            [
                'echarts',
                'echarts/chart/line' // 使用柱状图就加载bar模块，按需加载
            ],
            function (ec) {
                // 基于准备好的dom，初始化echarts图表
                var myChart = ec.init(document.getElementById('main'));

                var option = {
                    tooltip: {
                        // trigger: 'axis'
                    },
                    toolbox: {
                        show: true
                    },
                    calculable: false,
                    xAxis: [
                        {
                            type: 'category',
                            boundaryGap: false,
                            data: ['1:00', '2:00', '3:00', '4:00', '5:00', '6:00', '7:00', '8:00', '9:00', '10:00', '11:00', '12:00', '13:00', '14:00', '15:00', '16:00', '17:00', '18:00', '19:00', '20:00', '21:00', '22:00', '23:00', '24:00'],
                        }
                    ],
                    yAxis: [
                         {
                             type: 'category',
                             axisLine: { onZero: false },
                             axisLabel: {
                                 formatter: '{value} K'
                             },
                             boundaryGap: false,
                             data: ['0', '10', '20', '30', '40', '50', '60', '70', '80', '90', '100'],
                         }

                    ],
                    series: [
                        {
                            type: 'line',
                            smooth: true,
                            symbol: 'emptyCircle',
                            symbolSize: 4,
                            /*itemStyle: {normal: {areaStyle: {color: '#78c9ec',type: 'default'},lineStyle:{color: '#000',type: 'type',width: '1'},label:{show:true,textStyle:{color:'#3a535b'}}}},*/
                            itemStyle: { normal: { areaStyle: { color: '#b3ea5a', type: 'default' }, lineStyle: { borderColor: "green", color: '#000', type: 'type', width: '1' }, label: { show: true, textStyle: { color: '#3a535b' } } } },
                            data: [0, 10, 10, 20, 30, 40, 50, 60, 70, 80, 90, 70, 60, 70, 60, 50, 50, 40, 30, 20, 20, 10, 10, 0]
                        }
                    ]
                };

                // 为echarts对象加载数据
                myChart.setOption(option);
            }
        );
    </script>
</head>
<body style="overflow-x: hidden; overflow-y: hidden">
    <form id="form1" runat="server">
        <div id="main" style="height: 200px"></div>
    </form>
</body>
</html>
