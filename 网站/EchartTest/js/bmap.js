BMAP = {
    overlays: new Array(),
    MarkLayers: new Array(),
    AlarmLayers: new Array(),
    CircleLayers: new Array(),
    LineLayers: new Array(),
    LineTempLayers: new Array(),
    drawingManager: null,
    POI: false, //自定义标注开关
    POIData: null, //
    GPSLayer: null, //GPS图层
    map: null,
    myDis: null,
    PolyLine: null,
    MapInit: function () {
        
        map = new BMap.Map("map_canvas");
        var location = new BMap.Point(center.x, center.y);
        map.centerAndZoom(location, center.zoom);
        map.addControl(new BMap.NavigationControl());
        map.addControl(new BMap.OverviewMapControl());
        map.addControl(new BMap.MapTypeControl({
            mapTypes: [BMAP_NORMAL_MAP, BMAP_HYBRID_MAP]
        }));
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
        drawingManager.addEventListener('overlaycomplete', this.overlaycomplete);
        map.centerAndZoom(location, center.zoom);
    },
    getCenter: function () {
        var lonLat = map.getCenter();
        return lonLat;
    },
    GotoCenter: function () {
        var location = new BMap.Point(center.x, center.y);
        map.centerAndZoom(location, center.zoom);
    },
    GotoXY: function (x, y) {
        var location = new BMap.Point(x, y);
        map.centerAndZoom(location, map.zoomLevel);
    },
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

                case "Freduent":
                    FrequentOverCar.AddPosPoints(e.overlay.getPath());
                    break;
                    
                case "AddEventPoint":
                    debugger;
                    CarStrandQuery.AddPosPoints(e.overlay.getPosition());
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

       heatmapOverlay.hide();
    },
    Clear: function () {
        for (var i = 0; i < this.MarkLayers.length; i++) {
            var tmplayer = this.MarkLayers[i];
            map.removeOverlay(tmplayer);

        }
        this.MarkLayers = null;
        this.MarkLayers = new Array();
    },
    SaveMarker: function (markerdata) {

        BMAP.POIData = markerdata;
        drawingManager.setDrawingMode(BMAP_DRAWING_MARKER);
        drawingManager.open();
    },
   
    SaveAreaMarker: function (markerdata) {

        BMAP.POIData = markerdata;
        drawingManager.setDrawingMode(BMAP_DRAWING_POLYGON);
        drawingManager.open();


    },
    ClearLine: function () {
        debugger;
        for (var i = 0; i < this.LineLayers.length; i++) {
            var tmplayer = this.LineLayers[i];
            map.removeOverlay(tmplayer);

        }
        this.LineLayers = null;
        this.LineLayers = new Array();
    },
    ClearCircle: function () {
        for (var i = 0; i < this.CircleLayers.length; i++) {
            var tmplayer = this.CircleLayers[i];
            map.removeOverlay(tmplayer);

        }
        this.CircleLayers = null;
        this.CircleLayers = new Array();
    },
    ClearTempLine: function () {
        for (var i = 0; i < BMAP.LineTempLayers.length; i++) {
            var tmplayer = BMAP.LineTempLayers[i];
            map.removeOverlay(tmplayer);

        }
        BMAP.LineTempLayers = null;
        BMAP.LineTempLayers = new Array();
    },
    RemoveAlarmMarker: function () {
        for (var i = 0; i < this.AlarmLayers.length; i++) {
            var tmplayer = this.AlarmLayers[i];
            map.removeOverlay(tmplayer);
        }
        this.AlarmLayers = null;
        this.AlarmLayers = new Array();
    },
    DistanceTool: function () {
        myDis = new BMapLib.DistanceTool(map);
        myDis.open();
    },
    CalculateArea: function () {

        BMAP.POIData = { Operate: 'CalculateArea' };
        BMAP.ClearCircle();
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
        debugger;
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
    },
    addMarkerbs: function (img, lon, lat, stitle, obj,content) {
        debugger;
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
        debugger;
        var content = "<div style='text-align:center;'><h4 style='margin:0 0 0 0;padding:0.1em 0'>" + stitle + "</h4>";
        content = content + "<iframe frameborder=0 width=520 height=420 marginheight=0 marginwidth=0 scrolling=no src='UtcShow.aspx?kkid=" + '' + "'></iframe></div>";
        map.addOverlay(marker);
        this.MarkLayers.push(marker);
        var infoWindow = new BMap.InfoWindow(content);
        marker.addEventListener("click", function () {
            this.openInfoWindow(infoWindow);
        });
    },
    addMarkerHtml: function (img, lon, lat, stitle, obj,html) {
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
            enableDragging: true,
            raiseOnDrag: false,
            icon: new BMap.Icon(img, new BMap.Size(32, 32)),
            title: title
        });
        debugger;
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
    addMarkerGPSlabel: function (img, lon, lat, title, lable) {
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
        map.addOverlay(marker);
        this.MarkLayers.push(marker);
        var content = "<div style='text-align:center;'><h4 style='margin:0 0 0 0;padding:0.1em 0'>" + stitle + "</h4>";
        switch (obj.type.toUpperCase()) {
            case "TGS":
            case "TMS":
                content = content + "<iframe frameborder=0 width=500 height=400 marginheight=0 marginwidth=0 scrolling=no src='TgsShow.aspx?kkid=" + obj.id + "'></iframe></div>";
                break;
            case "UTC":
                content = content + "<iframe frameborder=0 width=520 height=420 marginheight=0 marginwidth=0 scrolling=no src='UtcShow.aspx?kkid=" + obj.para + "'></iframe></div>";
                break;
            case "VMS":
                var szDevpame = obj.para;
                var arrmp = szDevpame.split("|");
                var szWidth = arrmp[1] * 1;
                var szHeight = arrmp[2] * 1;
                content = content + "<iframe frameborder=0 width=" + szWidth + " height=" + szHeight + " marginheight=0 marginwidth=0 scrolling=no src='VmsShow.aspx?kkid=" + obj.id + "'></iframe></div>";
                break;
            case "CCTV":
                content = content + "<iframe frameborder=0 width=500 height=400 marginheight=0 marginwidth=0 scrolling=no src='CCTVSingleBrowse.aspx?videoUrl=" + stitle + "|" + obj.para + "'></iframe></div>";
                break;
            case "WEA":
                content = content + "<iframe frameborder=0 width=515 height=440 marginheight=0 marginwidth=0 scrolling=no src='WeaShow.aspx?kkid=" + obj.id + "'></iframe></div>";
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
        var infoWindow = new BMap.InfoWindow(content, {
            offset: new BMap.Size(0, -20),
            enableMessage: false,
            enableAutoPan: false
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
        map.addOverlay(marker);
        this.MarkLayers.push(marker);
        marker.addEventListener("click", function () {
            this.openInfoWindow(infoWindow);
        });
        marker.addEventListener('dragend', function (e) {
            if (confirm("确定要保存新的位置吗？")) {
                MarkerManager.UpdateMarkInfo(e.point.lng, e.point.lat, obj.type, obj.id);
            }
        });
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
            x1 = x1 * 1 ;
            y1 = y1 * 1 ;
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
            x1 = x1 * 1 ;
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
        debugger;
        var infoWindow = new BMap.InfoWindow(content);
        var location = new BMap.Point(xpoint, ypoint);
        map.openInfoWindow(infoWindow, location);
    },
    addPolyline2: function (color, strTemp, name) {
        debugger;
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


