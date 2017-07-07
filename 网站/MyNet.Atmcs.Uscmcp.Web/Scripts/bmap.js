BMAP = {
    overlays: new Array(),
    MarkLayers: new Array(),// 点图层
    AlarmLayers: new Array(), //报警图层
    CircleLayers: new Array(),// 圆圈图层
    LineLayers: new Array(),// 线图层
    LineTempLayers: new Array(),// 线临时图层
    drawingManager: null, // 绘制工具类
    POI: false, //自定义标注开关
    POIData: null, //
    GPSLayer: null, //GPS图层
    map: null, // 地图对象
    myDis: null,
    PolyLine: null,
    markerClusterer: null,
    JHmarkers: null,
    // 地图初始化
    MapInit: function () {
        map = new BMap.Map("map_canvas", { minZoom: 8, maxZoom: 19 }); // 创建map实例
        var location = new BMap.Point(center.x, center.y); // 获得中心点

        // map.centerAndZoom(location, center.zoom);// 初始化地图 设置中心点  及地图显示级别

        // 添加默认缩放平移控件
        map.addControl(new BMap.NavigationControl({
            offset: new BMap.Size(15, 70),
        }));
        //添加默认缩略地图控件
        map.addControl(new BMap.OverviewMapControl());
        //设置地图类型控件  地图 超分蓝
        map.addControl(new BMap.MapTypeControl({
            mapTypes: [BMAP_NORMAL_MAP, BMAP_HYBRID_MAP],
            offset: new BMap.Size(5, 40),
            anchor: BMAP_ANCHOR_TOP_LEFT
        }));
        // 添加 比例尺
        map.addControl(new BMap.ScaleControl({
            offset: new BMap.Size(5, 5),
            anchor: BMAP_ANCHOR_BOTTOM_LEFT
        }));

        var styleOptions = {
            strokeColor: "blue",
            fillColor: "blue",
            strokeWeight: 4,
            strokeOpacity: 0.6,
            fillOpacity: 0.2,
            strokeStyle: 'solid' //solid or dashed
        };
        // 初始化鼠标绘制工具
        drawingManager = new BMapLib.DrawingManager(map, {
            isOpen: false,
            enableDrawingTool: false,
            enableCalculate: true,
            drawingToolOptions: {
                anchor: BMAP_ANCHOR_TOP_LEFT, //位置
                offset: new BMap.Size(80, 9), //偏离值
                scale: 0.5
            },
            circleOptions: styleOptions,
            polylineOptions: styleOptions,
            polygonOptions: styleOptions,
            rectangleOptions: styleOptions
        });
        JHmarkers = [];
        markerClusterer = new BMapLib.MarkerClusterer(map, {});
        drawingManager.addEventListener('overlaycomplete', this.overlaycomplete);
        map.centerAndZoom(location, center.zoom);
        map.enableScrollWheelZoom();
    },

    // 获得地图中心点
    getCenter: function () {
        var lonLat = map.getCenter();
        return lonLat;
    },
    // 缩放到中心点
    GotoCenter: function () {
        var location = new BMap.Point(center.x, center.y);
        map.centerAndZoom(location, center.zoom);
    },
    // 缩放到指定位置
    GotoXY: function (x, y) {
        var location = new BMap.Point(x, y);
        map.centerAndZoom(location, map.zoomLevel);
    },
    // 缩放到指定位置 指定层级
    GotoXYZ: function (x, y, z) {
        var location = new BMap.Point(x, y);
        map.centerAndZoom(location, z);
    },

    overlaycomplete: function (e) {
        var result = "";
        result += e.drawingMode + ":";
        if (e.drawingMode == BMAP_DRAWING_MARKER) {
            map.removeOverlay(e.overlay);
            switch (BMAP.POIData.type) {
                case "AddEventPoint":
                    CarStrandQuery.AddPosPoints(e.overlay.getPosition().lng, e.overlay.getPosition().lat);
                    break;
                case "COM":
                    MarkerManager.ShowMarkWindow(e.overlay.getPosition().lng, e.overlay.getPosition().lat);
                    break;
                case "YJZJ":
                case "YJZB":
                case "YJWZ":
                    RrmMarkManager.SaveMarkInfo(e.overlay.getPosition().lng, e.overlay.getPosition().lat, BMAP.POIData.type, BMAP.POIData.id, BMAP.POIData.name);
                    break;
                case "AddPostPoint":
                    PostMapBrowse.AddPostPoints(e.overlay.getPosition(), "Point");
                    break;
                case "UpdatePostPoint":
                    PostMapBrowse.UpdataPoints(e.overlay.getPosition(), "Point");
                    break;
                case "EVENT":
                    AlarmMapBrowse.MarkPointSave(e.overlay.getPosition().lng, e.overlay.getPosition().lat, BMAP.POIData.id, BMAP.POIData.msg);
                    break;
                case "AddHotPoint":
                    HotMainPage.AddPostPoints(e.overlay.getPosition());
                    break;
                default:
                    MarkerManager.SaveMarkInfo(e.overlay.getPosition().lng, e.overlay.getPosition().lat, BMAP.POIData.type, BMAP.POIData.id, BMAP.POIData.name);
                    break;
            }
            drawingManager.close();
        }
        if (e.drawingMode == BMAP_DRAWING_CIRCLE) {
            BMAP.CircleLayers.push(e.overlay);
            drawingManager.close();
        }
        if (e.drawingMode == BMAP_DRAWING_POLYLINE || e.drawingMode == BMAP_DRAWING_POLYGON || e.drawingMode == BMAP_DRAWING_RECTANGLE) {
            BMAP.CircleLayers.push(e.overlay);
            drawingManager.disableCalculate();
            drawingManager.close();
            switch (BMAP.POIData.Operate) {
                case "AddRoadLine":
                    GisRoadBrowse.AddRoadPoints(e.overlay.getPath());
                    break;
                case "AddRoadSegLine":
                    GisRoadLinkBrowse.AddRoadPoints(e.overlay.getPath());
                    break;
                case "UpdateRoadLine":
                    GisRoadBrowse.UpdataPoints(e.overlay.getPath());
                    break;
                case "UpdateRoadSegLine":
                    GisRoadLinkBrowse.UpdataPoints(e.overlay.getPath());
                    break;
                case "SEVBrowse":
                    SEVBrowse.SaveInfo(e.overlay.points);
                    break;
                case "AddPostArea":
                    PostMapBrowse.AddPostPoints(e.overlay.getPath(), "Area");
                    break;
                case "AddPostLine":
                    PostMapBrowse.AddPostPoints(e.overlay.getPath(), "Line");
                    break;
                case "UpdatePostLine":
                    PostMapBrowse.UpdataPoints(e.overlay.getPath(), "Line");
                    break;
                case "UpdatePostArea":
                    PostMapBrowse.UpdataPoints(e.overlay.getPath(), "Area");
                    break
                case "AddHotPoint":
                    HotMainPage.UpdataPoints(e.overlay.getPath());
                    break
                case "CarStanQuery":
                    CarStanQuery.AddPosPoints(e.overlay.getPath());
                    break;
                case "Dispatched":
                    Dispatched.AddPosPoints(e.overlay.getPath());
                    break;
                case "MapStation":
                    MapStation.AddPosPoints(e.overlay.getPath());
                    break;
                case "Freduent":
                    FrequentOverCar.AddPosPoints(e.overlay.getPath());
                    break;
            }
        }
    },

    OpenHeatmap: function (points) {
        heatmapOverlay = new BMapLib.HeatmapOverlay({ "radius": 20 });
        map.addOverlay(heatmapOverlay);
        heatmapOverlay.setDataSet({ data: points, max: 100 });
        heatmapOverlay.show();
    },

    CloseHeatmap: function () {
        if (heatmapOverlay != null)
            heatmapOverlay.hide();
    },

    Clear: function () {
        for (var i = 0; i < this.MarkLayers.length; i++) {
            var tmplayer = this.MarkLayers[i];
            map.removeOverlay(tmplayer);
        }
        this.MarkLayers = null;
        this.MarkLayers = new Array();

        JHmarkers = null;
        JHmarkers = [];

        markerClusterer.clearMarkers()
        map.clearOverlays();
    },
    SaveMarker: function (markerdata) {
        BMAP.POIData = markerdata;
        drawingManager.close();
        drawingManager.setDrawingMode(BMAP_DRAWING_MARKER);
        drawingManager.open();
    },

    SaveAreaMarker: function (markerdata) {
        BMAP.POIData = markerdata;
        drawingManager.close();
        drawingManager.disableCalculate();
        drawingManager.setDrawingMode(BMAP_DRAWING_POLYGON);
        drawingManager.open();
    },
    // 清理Label
    ClearLabel: function () {
        var Overlays = map.getOverlays();
        for (var i = 0; i < Overlays.length; i++) {
            if (Overlays[i].domElement.nodeName == "LABEL") {
                map.removeOverlay(Overlays[i]);
            }
        }
    },
    // 清理线图层
    ClearLine: function () {
        for (var i = 0; i < this.LineLayers.length; i++) {
            var tmplayer = this.LineLayers[i];
            map.removeOverlay(tmplayer);
        }
        this.LineLayers = null;
        this.LineLayers = new Array();
    },
    // 清理圆圈图层
    ClearCircle: function () {
        for (var i = 0; i < this.CircleLayers.length; i++) {
            var tmplayer = this.CircleLayers[i];
            map.removeOverlay(tmplayer);
        }
        this.CircleLayers = null;
        this.CircleLayers = new Array();
    },
    // 清理临时线图层
    ClearTempLine: function () {
        for (var i = 0; i < BMAP.LineTempLayers.length; i++) {
            var tmplayer = BMAP.LineTempLayers[i];
            map.removeOverlay(tmplayer);
        }
        BMAP.LineTempLayers = null;
        BMAP.LineTempLayers = new Array();
    },
    // 清理报警临时图层
    RemoveAlarmMarker: function () {
        for (var i = 0; i < this.AlarmLayers.length; i++) {
            var tmplayer = this.AlarmLayers[i];
            map.removeOverlay(tmplayer);
        }
        this.AlarmLayers = null;
        this.AlarmLayers = new Array();
    },
    // 开启测距
    DistanceTool: function () {
        //myDis = new BMapLib.DistanceTool(map);
        //myDis.open();

        BMAP.POIData = { Operate: 'CalculateArea' };
        //BMAP.ClearCircle();
        drawingManager.enableCalculate();
        drawingManager.setDrawingMode(BMAP_DRAWING_POLYLINE);
        drawingManager.open();
    },
    // 打开测量面积
    CalculateArea: function () {
        BMAP.POIData = { Operate: 'CalculateArea' };
        //BMAP.ClearCircle();
        drawingManager.enableCalculate();
        drawingManager.setDrawingMode(BMAP_DRAWING_POLYGON);
        drawingManager.open();
    },
    AddRoadPoint: function (markerdata) {
        BMAP.POIData = markerdata;
        drawingManager.disableCalculate();
        drawingManager.setDrawingMode(BMAP_DRAWING_POLYLINE);
        drawingManager.open();
    },
    removeMarker: function (x, y) {
        var markLayer = null;
        for (var i = 0; i < this.MarkLayers.length; i++) {
            var mark = this.MarkLayers[i];
            if (mark.point.lng == x && mark.point.lat == y) {
                map.removeOverlay(mark);
            }
        }
    },
    addMarker: function (img, lon, lat, stitle, obj) {
        var locat = new BMap.Point(lon, lat);
        var marker = new BMap.Marker(locat, {
            enableMassClear: true,
            enableDragging: false,
            raiseOnDrag: false,
            icon: new BMap.Icon(img, new BMap.Size(32, 32)),
            title: stitle,
            data: obj
        });
        map.addOverlay(marker);
        this.MarkLayers.push(marker);
    },
    addMarkerbs: function (img, lon, lat, stitle, obj, content) {
        var locat = new BMap.Point(lon, lat);
        var marker = new BMap.Marker(locat, {
            enableMassClear: true,
            enableDragging: true,
            raiseOnDrag: false,
            icon: new BMap.Icon(img, new BMap.Size(32, 32)),
            title: stitle,
            data: obj
        });
        map.addOverlay(marker);
        this.MarkLayers.push(marker);

        var infoWindow = new BMap.InfoWindow(content);
        marker.addEventListener("click", function () {
            this.openInfoWindow(infoWindow);
        });
    },
    addMarkerDev: function (img, lon, lat, stitle, obj) {
        var locat = new BMap.Point(lon, lat);
        var marker = new BMap.Marker(locat, {
            enableMassClear: true,
            enableDragging: true,
            raiseOnDrag: false,
            icon: new BMap.Icon(img, new BMap.Size(32, 32)),
            title: stitle,
            data: obj
        });

        var content = "<div style='text-align:center;'><h4 style='margin:0 0 0 0;padding:0.1em 0'>" + stitle + "</h4>";
        content = content + "<iframe frameborder=0 width=520 height=420 marginheight=0 marginwidth=0 scrolling=no src='UtcShow.aspx?kkid=" + '' + "'></iframe></div>";
        map.addOverlay(marker);
        this.MarkLayers.push(marker);
        var infoWindow = new BMap.InfoWindow(content);
        marker.addEventListener("click", function () {
            this.openInfoWindow(infoWindow);
        });
    },
    addMarkerHtml: function (img, lon, lat, stitle, obj, html) {
        var locat = new BMap.Point(lon, lat);
        var marker = new BMap.Marker(locat, {
            enableMassClear: true,
            enableDragging: true,
            raiseOnDrag: false,
            icon: new BMap.Icon(img, new BMap.Size(32, 32))
        });
        map.addOverlay(marker);
        this.MarkLayers.push(marker);
        var infoWindow = new BMap.InfoWindow(html);
        marker.addEventListener("click", function () {
            this.openInfoWindow(infoWindow);
        });
    },
    addPolygonMarker: function (strTemp) {
        var str = strTemp;
        var strs = new Array(); //定义一数组
        var points = [];
        strs = str.split("|"); //字符分割
        for (i = 0; i < strs.length; i++) {
            if (strs[i] != "") {
                var xys = new Array();
                xys = strs[i].split(",");
                var x1 = xys[0];
                var y1 = xys[1];
                var point = new BMap.Point(x1, y1);
                points.push(point);
            }
        }
        var polygon2 = new BMap.Polygon(points, { strokeColor: "red", strokeWeight: 4, strokeOpacity: 0.7, fillOpacity: 0.1, fillColor: "red" });
        map.addOverlay(polygon2);
        this.CircleLayers.push(polygon2);
    },
    addMarkerlabel: function (img, lon, lat, title) {
        var locat = new BMap.Point(lon, lat);
        var marker = new BMap.Marker(locat, {
            enableMassClear: true,
            enableDragging: false,
            raiseOnDrag: false,
            icon: new BMap.Icon(img, new BMap.Size(32, 32)),
            title: title
        });
        map.addOverlay(marker);
        var label = new BMap.Label(title, {
            position: locat,
            enableMassClear: false,
            offset: new BMap.Size(-19, -19)
        });
        label.setStyle({
            borderColor: "#808080",
            color: "#333",
            padding: "1px 3px 1px 3px",
            borderRadius: "3px",
            backgroundColor: "#F4F4F4"
        });
        marker.setLabel(label);
        this.MarkLayers.push(marker);
    },
    addMarkerGPSlabel: function (img, lon, lat, title, lable, time) {
        var locat = new BMap.Point(lon, lat);
        var marker = new BMap.Marker(locat, {
            enableMassClear: true,
            enableDragging: true,
            raiseOnDrag: false,
            icon: new BMap.Icon(img, new BMap.Size(32, 32)),
            title: title
        });
        map.addOverlay(marker);
        var label = new BMap.Label(lable, {
            position: locat,
            enableMassClear: false,
            offset: new BMap.Size(-19, -19)
        });
        label.setStyle({
            borderColor: "#808080",
            color: "#ffffff",
            padding: "0px 0px 0px 0px",
            borderRadius: "1px",
            backgroundColor: "#0000ff"
        });
        marker.setLabel(label);
        this.MarkLayers.push(marker);
    },
    addmarkerstation: function (enableDrag, img, lon, lat, stitle, time, obj) {
        var locat = new BMap.Point(lon, lat);
        var marker = new BMap.Marker(locat, {
            enableMassClear: true,
            enableDragging: enableDrag,
            raiseOnDrag: false,
            icon: new BMap.Icon(img, new BMap.Size(32, 32)),
            title: stitle,
            data: obj
        });
        var content = "<div style='text-align:center;'>";
        var w = 600;
        var h = 400;
        switch (obj.type.toUpperCase()) {
            case "STATION":
                content = content + "<iframe frameborder=0 width=600 height=400 marginheight=0 marginwidth=0 scrolling=no src='TgsShowStation.aspx?id=" + obj.id + "&type=station&datetime=" + time + "'></iframe></div>";
                break;
            case "ROAD":
                content = content + "<iframe frameborder=0 width=600 height=400 marginheight=0 marginwidth=0 scrolling=no src='TgsShowStation.aspx?id=" + obj.id + "&type=road&datetime=" + time + "'></iframe></div>";
                break;
            case "IMAGE":
                content = content + "<iframe frameborder=0 width=450 height=300 marginheight=0 marginwidth=0 scrolling=no src='ShowImage.aspx?hphm=" + obj.hphm + "&gcsj=" + obj.gcsj + "&kkmc=" + stitle + "&url=" + time + "&clwz=" + obj.clwz + "'></iframe></div>"
                w = 450;
                h = 300;
                break;
        };
        var Window3 = new BMapLib.SearchInfoWindow(map, content, {
            title: stitle, //标题
            width: w, //宽度
            height: h, //高度
            panel: "panel", //检索结果面板
            enableAutoPan: true, //自动平移
            searchTypes: [
            ]
        });
        Window3.open(marker);
    },
    // 往点图层上添加对象
    addMarkerContent: function (enableDrag, img, lon, lat, stitle, obj) {
        var locat = new BMap.Point(lon, lat);
        var marker = new BMap.Marker(locat, {
            enableMassClear: true,
            enableDragging: enableDrag,
            raiseOnDrag: false,
            icon: new BMap.Icon(img, new BMap.Size(32, 32)),
            title: stitle,
            data: obj
        });

        //map.addOverlay(marker);
        //this.MarkLayers.push(marker);

        var content = "<div style='text-align:center;'>";
        switch (obj.type.toUpperCase()) {
            case "TGS":
            case "TCS":
            case "TMS":
            case "VGS":
                content = content + "<iframe frameborder=0 width=500 height=400 marginheight=0 marginwidth=0 scrolling=no src='../Map/TgsShow.aspx?kkid=" + obj.id + "'></iframe></div>";
                break;
            case "UTC":
                content = content + "<iframe frameborder=0 width=520 height=420 marginheight=0 marginwidth=0 scrolling=no src='../Map/UtcShow.aspx?kkid=" + obj.para + "'></iframe></div>";
                break;
            case "VMS":
                var szDevpame = obj.para;
                var arrmp = szDevpame.split("|");
                var szWidth = arrmp[1] * 1;
                var szHeight = arrmp[2] * 1;
                content = content + "<iframe frameborder=0 width=" + szWidth + " height=" + szHeight + " marginheight=0 marginwidth=0 scrolling=no src='../Map/VmsShow.aspx?kkid=" + obj.id + "'></iframe></div>";
                break;
            case "CCTV":
                content = content + "<iframe frameborder=0 width=500 height=400 marginheight=0 marginwidth=0 scrolling=no src='../Map/CCTVSingleBrowse.aspx?videoUrl=" + stitle + "|" + obj.para + "'></iframe></div>";
                break;
            case "WEA":
                content = content + "<iframe frameborder=0 width=515 height=440 marginheight=0 marginwidth=0 scrolling=no src='../Map/WeaShow.aspx?kkid=" + obj.id + "'></iframe></div>";
                break;
            case "TES":
                content = content + "<iframe frameborder=0 width=500 height=400 marginheight=0 marginwidth=0 scrolling=no src='TesShow.aspx?kkid=" + obj.id + "'></iframe></div>";
                break;
            case "TFM":
                content = content + "<iframe frameborder=0 width=420 height=375 marginheight=0 marginwidth=0 scrolling=no src='TfmShow.aspx?kkid=" + obj.id + "'></iframe></div>";
                break;
            case "ZD":
                content = content + "<iframe frameborder=0 width=515 height=360 marginheight=0 marginwidth=0 scrolling=no src='CTJShow.aspx?id=" + obj.id + "'></iframe></div>";
                break;
            case "GZ":
                content = content + "<iframe frameborder=0 width=600 height=220 marginheight=0 marginwidth=0 scrolling=no src='TFCShow.aspx?id=" + obj.id + "'></iframe></div>";
                break;
            case "USER":
                content = content + "<iframe frameborder=0 width=300 height=120 marginheight=0 marginwidth=0 scrolling=no src='GpsShow.aspx?id=" + obj.id + "&&type=user&&lx=" + obj.lx + "'></iframe></div>";
                break;
            case "CAR":
                content = content + "<iframe frameborder=0 width=300 height=120 marginheight=0 marginwidth=0 scrolling=no src='GpsShow.aspx?id=" + obj.id + "&&type=car&&lx=" + obj.lx + "'></iframe></div>";
                break;
        }
        // 去除老弹窗
        //var infoWindow = new BMap.InfoWindow(content, {
        //    offset: new BMap.Size(0, -20),
        //    enableMessage: false,
        //    enableAutoPan: true
        //});
        //infoWindow.addEventListener("close", function (e) {
        //    alert(e.type);
        //});
        //设定新样式
        var searchInfoWindow3 = new BMapLib.SearchInfoWindow(map, content, {
            title: stitle, //标题
            width: 500, //宽度
            height: 400, //高度
            panel: "panel", //检索结果面板
            enableAutoPan: true, //自动平移
            searchTypes: [
            ]
        });
        //添加关闭事件
        searchInfoWindow3.addEventListener("close", function (e) {
            //alert(e.type );
            //debugger;
            try { QuipmentSpecial.CloseMq(); }
            catch (err)
            { }
        });

        var label = new BMap.Label(stitle, {
            position: locat,
            enableMassClear: false,
            offset: new BMap.Size(-19, -19)
        });
        label.setStyle({
            borderColor: "#808080",
            color: "#0000ff",
            padding: "1px 3px 1px 3px",
            borderRadius: "3px",
            backgroundColor: "#F4F4F4"
        });
        marker.setLabel(label);
        //增加点
        //map.addOverlay(marker);
        //this.MarkLayers.push(marker);
        JHmarkers.push(marker);
        //移除老绑定事件
        //marker.addEventListener("click", function () {
        //    this.openInfoWindow(infoWindow);
        //});
        // 添加新的点击事件
        marker.addEventListener("click", function () {
            searchInfoWindow3.open(marker);
        });
        marker.addEventListener('dragend', function (e) {
            if (confirm("确定要保存新的位置吗？")) {
                MarkerManager.UpdateMarkInfo(e.point.lng, e.point.lat, obj.type, obj.id);
            }
        });
    },

    // 添加聚合显示
    ShowMarkerClusterer: function () {
        markerClusterer.addMarkers(JHmarkers);
    },

    addMarkerAlarm: function (img, lon, lat, stitle, obj, isShow) {
        var locat = new BMap.Point(lon, lat);
        var marker = new BMap.Marker(locat, {
            enableMassClear: true,
            enableDragging: false,
            raiseOnDrag: false,
            icon: new BMap.Icon(img, new BMap.Size(16, 16)),
            title: stitle,
            data: obj
        });
        map.addOverlay(marker);
        this.AlarmLayers.push(marker);
        var content = "<div style='margin-top:0px;margin-left:10px;font-size:12px'><table><tr><td>" + obj.bjyy + "</td></tr><tr><td><br/></tr><tr><td>&nbsp;</td></tr></table></div>";
        var infoWindow = new BMap.InfoWindow(content, {
            offset: new BMap.Size(0, -20),
            enableMessage: false,
            enableAutoPan: false
        });
        var label = new BMap.Label(obj.bjyy, {
            position: locat,
            enableMassClear: false,
            offset: new BMap.Size(-40, -40)
        });
        label.setStyle({
            borderColor: "#808080",
            color: "#ff0000",
            padding: "1px 3px 1px 3px",
            borderRadius: "3px",
            backgroundColor: "#F4F4F4"
        });
        marker.setLabel(label);
        map.addOverlay(marker);
        this.AlarmLayers.push(marker);
        marker.addEventListener("click", function () {
            this.openInfoWindow(infoWindow, map.getCenter());
        });
        if (isShow) {
            marker.openInfoWindow(infoWindow, map.getCenter());
        }
    },
    addCircleEvent: function (lon, lat, r) {
        var locat = new BMap.Point(lon, lat);
        var circle = new BMap.Circle(locat, r, { strokeColor: "red", strokeWeight: 4, strokeOpacity: 0.7, fillOpacity: 0.1, fillColor: "red" });

        var label = new BMap.Label("方圆" + r + "米", {
            position: locat,
            enableMassClear: false,
            offset: new BMap.Size(-19, 100)
        });
        label.setStyle({
            borderColor: "#808080",
            color: "#000000",
            padding: "1px 3px 1px 3px",
            borderRadius: "3px",
            backgroundColor: "#F4F4F4"
        });
        map.addOverlay(circle);
        map.addOverlay(label);
        this.CircleLayers.push(circle);
        this.CircleLayers.push(label);
    },
    addCircle: function (lon, lat, title) {
        var locat = new BMap.Point(lon, lat);
        var circle = new BMap.Circle(locat, 2000, { strokeColor: "red", strokeWeight: 4, strokeOpacity: 0.7, fillOpacity: 0.1, fillColor: "red" });

        var label = new BMap.Label("方圆2000米", {
            position: locat,
            enableMassClear: false,
            offset: new BMap.Size(-19, 100)
        });
        label.setStyle({
            borderColor: "#808080",
            color: "#000000",
            padding: "1px 3px 1px 3px",
            borderRadius: "3px",
            backgroundColor: "#F4F4F4"
        });
        map.addOverlay(circle);
        map.addOverlay(label);
        this.CircleLayers.push(circle);
        this.CircleLayers.push(label);
    },
    addPolyline: function (color, strTemp, name) {
        var str = strTemp;
        var strs = new Array(); //定义一数组
        var points = [];
        strs = str.split("|"); //字符分割
        for (i = 0; i < strs.length; i++) {
            var xys = new Array();
            xys = strs[i].split(",");
            var x1 = xys[0];
            var y1 = xys[1];
            x1 = x1 * 1;
            y1 = y1 * 1;
            var point = new BMap.Point(x1, y1);
            points.push(point);
        }
        var polyline = new BMap.Polyline(points, { strokeColor: color, strokeWeight: 6, strokeOpacity: 0.8, fillOpacity: 0.01, fillColor: "red" });
        map.addOverlay(polyline);
        polyline.addEventListener("mouseover", function (e) {
            var count = e.currentTarget.points.length;
            var label = new BMap.Label(name, {
                position: e.point,
                enableMassClear: false,
                offset: new BMap.Size(-19, -19)
            });
            label.setStyle({
                borderColor: "#808080",
                color: "#000000",
                padding: "1px 3px 1px 3px",
                borderRadius: "3px",
                backgroundColor: "#F4F4F4"
            });
            PolyLine = new BMap.Polyline(e.currentTarget.points, { strokeColor: "000000", strokeWeight: 6, strokeOpacity: 0.3, fillOpacity: 0.01, fillColor: "red" });
            map.addOverlay(PolyLine);
            map.addOverlay(label);
            BMAP.LineTempLayers.push(PolyLine);
            BMAP.LineTempLayers.push(label);
        });
        polyline.addEventListener("mouseout", function (e) {
            BMAP.ClearTempLine();
        });
        this.LineLayers.push(polyline);
    },
    addRoadline: function (color, strTemp, name, segid) {
        var str = strTemp;
        var strs = new Array(); //定义一数组
        var points = [];
        strs = str.split("|"); //字符分割
        for (i = 0; i < strs.length; i++) {
            var xys = new Array();
            xys = strs[i].split(",");
            var x1 = xys[0];
            var y1 = xys[1];
            x1 = x1 * 1;
            y1 = y1 * 1;
            var point = new BMap.Point(x1, y1);
            points.push(point);
        }
        var linewidth = map.zoomLevel / 3;
        var polyline = new BMap.Polyline(points, { strokeColor: color, strokeWeight: linewidth, strokeOpacity: 0.8, fillOpacity: 0.01, fillColor: "red" });
        map.addOverlay(polyline);

        polyline.addEventListener("click", function (e) {
            var content = "<div style='text-align:center;'><h4 style='margin:0 0 0 0;padding:0.1em 0'>" + name + "</h4>";
            content = content + "<iframe frameborder=0 width=400 height=320 marginheight=0 marginwidth=0 scrolling=no src='TfmShow.aspx?kkid=" + segid + "&x=" + e.point.lng + "&y=" + e.point.lat + "'></iframe></div>";
            var infoWindow = new BMap.InfoWindow(content);
            map.openInfoWindow(infoWindow, e.point);
        });
        polyline.addEventListener("mouseover", function (e) {
            var count = e.currentTarget.points.length;
            var label = new BMap.Label(name, {
                position: e.point,
                enableMassClear: false,
                offset: new BMap.Size(-19, -19)
            });
            label.setStyle({
                borderColor: "#808080",
                color: "#000000",
                padding: "1px 3px 1px 3px",
                borderRadius: "3px",
                backgroundColor: "#F4F4F4"
            });
            map.addOverlay(label);
            BMAP.LineTempLayers.push(label);
        });

        polyline.addEventListener("mouseout", function (e) {
            BMAP.ClearTempLine();
        });
        this.LineLayers.push(polyline);
    },
    openWindow: function (content, xpoint, ypoint) {
        var infoWindow = new BMap.InfoWindow(content);
        var location = new BMap.Point(xpoint, ypoint);
        map.openInfoWindow(infoWindow, location);
    },
    addPolyline2: function (color, strTemp, name) {
        var str = strTemp;
        var strs = new Array(); //定义一数组
        var points = [];
        strs = str.split("|"); //字符分割
        for (i = 0; i < strs.length; i++) {
            var xys = new Array();
            xys = strs[i].split(",");
            var x1 = xys[0];
            var y1 = xys[1];
            x1 = x1 * 1;
            y1 = y1 * 1;
            var point = new BMap.Point(x1, y1);
            points.push(point);
        }
        var linewidth = map.zoomLevel / 3;
        var polyline = new BMap.Polyline(points, { strokeColor: color, strokeWeight: linewidth, strokeOpacity: 0.8, fillOpacity: 0.01, fillColor: "blue" });
        map.addOverlay(polyline);
        polyline.addEventListener("mouseover", function (e) {
            var count = e.currentTarget.points.length;
            var label = new BMap.Label(name, {
                position: e.point,
                enableMassClear: false,
                offset: new BMap.Size(-19, -19)
            });
            label.setStyle({
                borderColor: "#808080",
                color: "#000000",
                padding: "1px 3px 1px 3px",
                borderRadius: "3px",
                backgroundColor: "#F4F4F4"
            });
            PolyLine = new BMap.Polyline(e.currentTarget.points, { strokeColor: "000000", strokeWeight: 5, strokeOpacity: 0.3, fillOpacity: 0.01, fillColor: "blue" });
            map.addOverlay(PolyLine);
            map.addOverlay(label);
            BMAP.LineTempLayers.push(PolyLine);
            BMAP.LineTempLayers.push(label);
        });
        polyline.addEventListener("mouseout", function (e) {
            BMAP.ClearTempLine();
        });
        this.LineLayers.push(polyline);
    },
    addPolyline3: function (color, strTemp, name, sleep) {
        var str = strTemp;
        var strs = new Array(); //定义一数组
        var points = [];
        strs = str.split("|"); //字符分割
        for (i = 0; i < strs.length; i++) {
            var xys = new Array();
            xys = strs[i].split(",");
            var x1 = xys[0];
            var y1 = xys[1];
            x1 = x1 * 1;
            y1 = y1 * 1;
            var point = new BMap.Point(x1, y1);
            points.push(point);
        }
        var linewidth = map.zoomLevel / 3;
        var polyline = new BMap.Polyline(points, { strokeColor: color, strokeWeight: linewidth, strokeOpacity: 0.8, fillOpacity: 0.01, fillColor: "blue" });
        map.addOverlay(polyline);
        polyline.addEventListener("mouseover", function (e) {
            var count = e.currentTarget.points.length;
            var label = new BMap.Label(name, {
                position: e.point,
                enableMassClear: false,
                offset: new BMap.Size(-19, -19)
            });
            label.setStyle({
                borderColor: "#808080",
                color: "#000000",
                padding: "1px 3px 1px 3px",
                borderRadius: "3px",
                backgroundColor: "#F4F4F4"
            });
            PolyLine = new BMap.Polyline(e.currentTarget.points, { strokeColor: "000000", strokeWeight: 5, strokeOpacity: 0.3, fillOpacity: 0.01, fillColor: "blue" });
            map.addOverlay(PolyLine);
            map.addOverlay(label);
            BMAP.LineTempLayers.push(PolyLine);
            BMAP.LineTempLayers.push(label);
        });
        polyline.addEventListener("mouseout", function (e) {
            BMAP.ClearTempLine();
        });
        this.LineLayers.push(polyline);
    }
};