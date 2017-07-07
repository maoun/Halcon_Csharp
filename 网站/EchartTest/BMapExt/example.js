(function () {
    require.config({
        paths: {
            echarts: 'js'
        },
        packages: [
            {
                name: 'BMap',
                location: './',
                main: 'main'
            }
        ]
    });

    require(
    [
        'echarts',
        'BMap',
        'echarts/chart/map'
    ],
    function (echarts, BMapExtension) {
        $('#map_canvas').css({
            height:$('body').height(),
            width: $('body').width()
        });

        // 初始化地图
        var BMapExt = new BMapExtension($('#map_canvas')[0], BMap, echarts, {
            enableMapClick: false
        });
        var map = BMapExt.getMap();
        var container = BMapExt.getEchartsContainer();

        var startPoint = {
            x: 104.114129,
            y: 37.550339
        };

        var point = new BMap.Point(startPoint.x, startPoint.y);
        map.centerAndZoom(point, 5);
        map.enableScrollWheelZoom(true);
         //地图自定义样式
    
       option = {
            color: ['gold','aqua','lime'],
            title : {
                text: '城市OD分析结果',
                subtext: '城市OD分析结果',
                x:'center',
                textStyle : {
                    color: '#fff'
                }
            },
            tooltip : {
                trigger: 'item',
                formatter: function (v) {
                    return v[1].replace(':', ' > ');
                }
            },
            legend: {
                orient: 'vertical',
                x:'left',
                data: ['德州', '济南', '滨州'],
                selectedMode: 'single',
                selected:{
                    '济南': false,
                    '滨州': false
                },
                textStyle : {
                    color: '#fff'
                }
            },
            toolbox: {
                show : true,
                orient : 'vertical',
                x: 'right',
                y: 'center',
                feature : {
                    mark : {show: true},
                    dataView : {show: true, readOnly: false},
                    restore : {show: true},
                    saveAsImage : {show: true}
                }
            },
            dataRange: {
                min : 0,
                max : 100,
                range: {
                    start: 10,
                    end: 90
                },
                x: 'right',
                calculable : true,
                color: ['#ff3333', 'orange', 'yellow','lime','aqua'],
                textStyle:{
                    color:'#fff'
                }
            },
            series : [
                {
                    name: '德州',
                    type:'map',
                    mapType: 'none',
                    data: [],
                    geoCoord: {
 
                        '德州': [116.6858,37.2107],
                        '枣庄': [117.323, 34.8926],
                        '济南': [117.1582, 36.8701],
                        '济宁': [116.8286, 35.3375],
                        '淄博': [118.0371, 36.6064],
                        '滨州': [117.8174, 37.4963],
                        '潍坊': [119.0918, 36.524],
                        '烟台': [120.7397, 37.5128]
                    },

                    markLine : {
                        smooth:true,
                        effect : {
                            show: true,
                            scaleSize: 1,
                            period: 30,
                            color: '#fff',
                            shadowBlur: 10
                        },
                        itemStyle : {
                            normal: {
                                borderWidth:2,
                                lineStyle: {
                                    type: 'solid',
                                    shadowBlur: 10
                                }
                            }
                        },
                        data: [
                   
                            [{ name: '枣庄', value: 95 }, { name: '德州' }],
                            [{ name: '济南', value: 90 }, { name: '德州' }],
                            [{ name: '济宁', value: 80 }, { name: '德州' }],
                            [{ name: '淄博', value: 70 }, { name: '德州' }],
                            [{ name: '滨州', value: 60 }, { name: '德州' }],
                            [{ name: '潍坊', value: 40 }, { name: '德州' }],
                            [{ name: '烟台', value: 30 }, { name: '德州' }]
                        ]
                    },
                    markPoint: {
                        symbol: 'emptyCircle',
                        symbolSize: function (v) {
                            return 1 + v / 10
                        },
                        effect: {
                            show: true,
                            shadowBlur: 0
                        },
                        itemStyle: {
                            normal: {
                                label: { show: false }
                            }
                        },
                        data : [
                            { name: '枣庄', value: 95 },
                            { name: '济南', value: 90 },
                            { name: '济宁', value: 80 },
                            { name: '淄博', value: 70 },
                            { name: '滨州', value: 60 },
                            { name: '潍坊', value: 50 },
                            { name: '德州', value: 90 },
                            { name: '烟台', value: 40 }
                        
                        ]
                    }

                },
                {
                    name: '济南',
                    type:'map',
                    mapType: 'none',
                    data:[],
                    markLine : {
                        smooth:true,
                        effect : {
                            show: true,
                            scaleSize: 1,
                            period: 30,
                            color: '#fff',
                            shadowBlur: 10
                        },
                        itemStyle : {
                            normal: {
                                borderWidth:1,
                                lineStyle: {
                                    type: 'solid',
                                    shadowBlur: 10
                                }
                            }
                        },
                        data : [
                            [{ name: '济南' }, { name: '烟台', value: 60 }],
                            [{ name: '济南' }, { name: '淄博', value: 50 }],
                            [{ name: '济南' }, { name: '滨州', value: 40 }],
                            [{ name: '济南' }, { name: '济宁', value: 30 }],
                            [{ name: '济南' }, { name: '德州', value: 20 }],
                            [{ name: '济南' }, { name: '枣庄', value: 10 }]
                        ]
                    },
                    markPoint : {
                        symbol:'emptyCircle',
                        symbolSize : function (v){
                            return 10 + v/10
                        },
                        effect : {
                            show: true,
                            shadowBlur : 0
                        },
                        itemStyle:{
                            normal:{
                                label:{show:false}
                            }
                        },
                        data : [
                            { name: '烟台', value: 60 },
                            { name: '淄博', value: 50 },
                            { name: '滨州', value: 40 },
                            { name: '济宁', value: 30 },
                            { name: '德州', value: 20 },
                            { name: '枣庄', value: 10 }
                        ]
                    }
                },
                {
                    name: '滨州',
                    type:'map',
                    mapType: 'none',
                    data:[],
                    markLine : {
                        smooth:true,
                        effect : {
                            show: true,
                            scaleSize: 1,
                            period: 30,
                            color: '#fff',
                            shadowBlur: 10
                        },
                        itemStyle : {
                            normal: {
                                borderWidth:1,
                                lineStyle: {
                                    type: 'solid',
                                    shadowBlur: 10
                                }
                            }
                        },
                        data : [
                            [{ name: '滨州' }, { name: '枣庄', value: 95 }],
                            [{ name: '滨州' }, { name: '德州', value: 90 }],
                            [{ name: '滨州' }, { name: '济宁', value: 80 }],
                            [{ name: '滨州' }, { name: '淄博', value: 70 }],
                            [{ name: '滨州' }, { name: '烟台', value: 60 }],
                            [{ name: '滨州' }, { name: '济南', value: 50 }]
                        ]
                    },
                    markPoint : {
                        symbol:'emptyCircle',
                        symbolSize : function (v){
                            return 10 + v/10
                        },
                        effect : {
                            show: true,
                            shadowBlur : 0
                        },
                        itemStyle:{
                            normal:{
                                label:{show:false}
                            }
                        },
                        data : [
                            { name: '枣庄', value: 95 },
                            { name: '德州', value: 90 },
                            { name: '济宁', value: 80 },
                            { name: '淄博', value: 70 },
                            { name: '烟台', value: 60 },
                            { name: '济南', value: 50 }
                        ]
                    }
                }
            
            ]
        };

        var myChart = BMapExt.initECharts(container);
        window.onresize = myChart.onresize;
        BMapExt.setOption(option);
    }
);
})();