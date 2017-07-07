VNMAP = {
    markerList: ['故宫,116.39677,39.91870', '重庆,106.56068,29.60262', '上海,121.46147,31.26095'], //'延庆县,116.00591,40.35372'
    PI: 3.14159265358979323846,
    boxOffset: null,
    map: null, //地图主对象
    minimap: null,
    lines: null, //线
    ParentPage: null, //线
    Layers: null, //图层
    POI: false, //自定义标注开关
    DDPOI: { FLAG: false, TYPE: "", DATA: {} }, //地点标注开关
    GPSLayer: null, //GPS图层
    PointLayer: null, //点图层
    TFMLayer: null, //流量图层
    RoundLayer: null, //圆形区域图层
    Round: null, //圆行矢量对象
    CustomMarkers: null, //自定义标注
    TextLayer: null, //圆形区域图层
    GPSMarkers: null, //GPS标注
    markers: null, //路口全局变量
    markLayer: null,  //全部显示图层
    LocationMarkers: null, //路口全局变量
    AlarmLayer: null, //路口全局变量
    isMeasure: false,
    isRemoveLayer: false,
    MeasurePoint: new Array(), //测量对象point
    MarkLayers: new Array(), //测量对象point
    MeasureMouseInfoDiv: null, //测量对象跟随鼠标的信息框
    tfm_vectors: null,
    //获取当前可视区域中心点经纬度坐标
    getCenter: function () {
        var lonLat = this.map.getCenter();
        return this.ParsePxToLonLat(lonLat);
    },

    //将经纬度转换成墨卡托坐标
    ParseLonLatToPx: function (x, y) {
        x = parseFloat(x);
        y = parseFloat(y);
        var LonLat = {
            lon: x * 20037508.34 / 180,
            lat: (Math.log(Math.tan((90 + y) * this.PI / 360)) / (this.PI / 180)) * 20037508.34 / 180
        };
        return LonLat;
    },

    //将墨卡托坐标转换成经纬度
    ParsePxToLonLat: function (lonLat) {

        if (this.map.displayProjection) {
            return lonLat.transform(this.map.getProjectionObject(), this.map.displayProjection);
        }
    },
    //矢量地图
    getM: function (bounds) {

        bounds = this.adjustBounds(bounds);
        var zoom = this.map.getZoom();


        var lon = ((bounds.left).toFixed(2) / 20037508.34) * 180.0;
        var lat = ((bounds.top).toFixed(2) / 20037508.34) * 180.0;
        lat = 57.295779513082323 * ((2.0 * Math.atan(Math.exp((lat * 3.1415926535897931) / 180.0))) - 1.5707963267948966);

        var lonlat = {
            lon: lon,
            lat: lat
        }
        var MinLatitude = -85.05112878;
        var MaxLatitude = 85.05112878;
        var MinLongitude = -180;
        var MaxLongitude = 180;
        var latitude = Math.min(Math.max(lonlat.lat, MinLatitude), MaxLatitude);
        var x = (Math.min(Math.max(lonlat.lon, MinLongitude), MaxLongitude) + 180.0) / 360.0;
        var sinLatitude = Math.sin((latitude * 3.1415926535897931) / 180.0);
        var y = 0.5 - (Math.log((1.0 + sinLatitude) / (1.0 - sinLatitude)) / 12.566370614359173);
        var mapSize = 0x100;
        x = Math.min(Math.max((((x * mapSize) * Math.pow(2.0, zoom)) + 0.5), 0.0), (mapSize * Math.pow(2.0, zoom)) - 1.0);
        y = Math.min(Math.max((((y * mapSize) * Math.pow(2.0, zoom)) + 0.5), 0.0), (mapSize * Math.pow(2.0, zoom)) - 1.0);
        x = Math.floor((x / 256.0));
        y = Math.floor((y / 256.0));
        var obj = "m" + "/" + zoom + "/" + "m" + "_" + zoom + "_" + x + "_" + y + "." + "png";
        var t = mapserver + obj;
        VNMAP.HideLayer(zoom);
        return t;
    },
    HideLayer: function (zoom) {

        if (this.TextLayer != null) {
            if (zoom > 12) {
                this.isRemoveLayer = false;
                this.map.addLayer(this.TextLayer);
            }
            else {
                if (!this.isRemoveLayer) {
                    this.isRemoveLayer = true;
                    this.map.removeLayer(this.TextLayer);
                }

            }
        }
    },
    //卫星地图
    getS: function (bounds) {
        bounds = this.adjustBounds(bounds);
        var zoom = this.map.getZoom();

        var lon = ((bounds.left).toFixed(2) / 20037508.34) * 180.0;
        var lat = ((bounds.top).toFixed(2) / 20037508.34) * 180.0;
        lat = 57.295779513082323 * ((2.0 * Math.atan(Math.exp((lat * 3.1415926535897931) / 180.0))) - 1.5707963267948966);

        var lonlat = {
            lon: lon,
            lat: lat
        }
        var MinLatitude = -85.05112878;
        var MaxLatitude = 85.05112878;
        var MinLongitude = -180;
        var MaxLongitude = 180;
        var latitude = Math.min(Math.max(lonlat.lat, MinLatitude), MaxLatitude);
        var x = (Math.min(Math.max(lonlat.lon, MinLongitude), MaxLongitude) + 180.0) / 360.0;
        var sinLatitude = Math.sin((latitude * 3.1415926535897931) / 180.0);
        var y = 0.5 - (Math.log((1.0 + sinLatitude) / (1.0 - sinLatitude)) / 12.566370614359173);
        var mapSize = 0x100;
        x = Math.min(Math.max((((x * mapSize) * Math.pow(2.0, zoom)) + 0.5), 0.0), (mapSize * Math.pow(2.0, zoom)) - 1.0);
        y = Math.min(Math.max((((y * mapSize) * Math.pow(2.0, zoom)) + 0.5), 0.0), (mapSize * Math.pow(2.0, zoom)) - 1.0);
        x = Math.floor((x / 256.0));
        y = Math.floor((y / 256.0));
        var obj = "s" + "/" + zoom + "/" + "s" + "_" + zoom + "_" + x + "_" + y + "." + "jpg";
        return t;
    },
    MiniMapInit: function () {
        var lonlat = this.ParseLonLatToPx(center.x, center.y);
        this.minimap = new OpenLayers.Map("minimap",
        {
            controls: [
            //new OpenLayers.Control.Navigation({ 'zoomWheelEnabled': false }),
                new OpenLayers.Control.MouseDefaults({
                    defaultMouseUp: function (e) {
                        if (!OpenLayers.Event.isLeftClick(e)) {
                            return;
                        }
                        if (this.zoomBox) {
                            this.zoomBoxEnd(e);
                        }
                        else {
                            if (this.performedDrag) {
                                this.map.setCenter(this.map.center);
                            }
                        }
                        document.onselectstart = null;
                        this.mouseDragStart = null;
                        var xy = this.map.getCenter();
                        xy = VNMAP.ParsePxToLonLat(xy);
                        VNMAP.GotoXY(xy.lon, xy.lat, VNMAP.map.getZoom());
                    },
                    defaultWheelUp: function (e) {
                        return false
                    },
                    defaultWheelDown: function (e) {
                        return false
                    },
                    defaultDblClick: function (e) {
                        var xy = this.map.getLonLatFromViewPortPx(e.xy);
                        xy = VNMAP.ParsePxToLonLat(xy);
                        VNMAP.GotoXY(xy.lon, xy.lat, VNMAP.map.getZoom());
                        var lonlat = this.map.getLonLatFromViewPortPx(e.xy);
                        VNMAP.miniGotoXY(lonlat, VNMAP.minimap.getZoom());
                    },
                    defaultClick: function (e) {
                        return false
                    }
                })
            ],
            projection: new OpenLayers.Projection("EPSG:900913"),
            displayProjection: new OpenLayers.Projection("EPSG:4326"),
            units: "m",
            maxResolution: 156543.0339,
            numZoomLevels: 15,
            panMethod: OpenLayers.Easing.Expo.easeOut,
            panDuration: 30,
            maxExtent: new OpenLayers.Bounds(-20037508.34, -20037508.34, 20037508.34, 20037508.34)
        });
        M = new OpenLayers.Layer.TMS("矢量地图", "", { type: 'png', getURL: this.getM, isBaseLayer: true, transitionEffect: "resize", layers: "basic" });
        var Layers = [M];
        this.minimap.addLayers(Layers); //添加到层图
        //设置中心
        this.minimap.setCenter(new OpenLayers.LonLat(lonlat.lon, lonlat.lat), 4);
    },
    //初始化地图
    MapInit: function () {
        var lonlat = this.ParseLonLatToPx(center.x, center.y);
        var PanZoomBar = new OpenLayers.Control.PanZoomBar({ zoomWorldIcon: true, slideFactor: 200 });
        this.map = new OpenLayers.Map("map", {
            controls: [
     		new OpenLayers.Control.MouseDefaults({
     		    defaultClick: function (e) {
     		        if (VNMAP.POI) {
     		            VNMAP.showCustomMarker(e);
     		        }
     		        if (VNMAP.DDPOI.FLAG) {
     		            VNMAP.SaveMarker(e);
     		        }


     		    }
     		}),
            //new OpenLayers.Control.MousePosition(),
            new OpenLayers.Control.ScaleLine(),
     		new OpenLayers.Control.LayerSwitcher(),
            new OpenLayers.Control.TouchNavigation({
                dragPanOptions: {
                    enableKinetic: true
                },
                defaultDblClick: function (e) {
                    var xy = this.map.getLonLatFromViewPortPx(e.xy);
                    var xy = VNMAP.ParsePxToLonLat(xy);
                    VNMAP.GotoXY(xy.lon, xy.lat, this.map.zoom + 1);
                    var lonlat = this.map.getLonLatFromViewPortPx(e.xy);
                    VNMAP.miniGotoXY(lonlat, VNMAP.minimap.zoom + 1);
                }
            }),
            PanZoomBar
        ],
            projection: new OpenLayers.Projection("EPSG:900913"),
            displayProjection: new OpenLayers.Projection("EPSG:4326"),
            units: "m",
            maxResolution: 156543.0339,
            numZoomLevels: 19,
            panMethod: OpenLayers.Easing.Expo.easeOut,
            panDuration: 30,
            maxExtent: new OpenLayers.Bounds(-20037508.34, -20037508.34, 20037508.34, 20037508.34)
        });
        PanZoomBar.div.onmousemove = function () {
            PanZoomBar.showButtons("block");
        };
        this.map.layerContainerDiv.onmousedown = function () {
            PanZoomBar.showButtons("none");
        };
        /*
        点、线、面样式
        */
        var sketchSymbolizers = {
            "Point":
            {
                pointRadius: 4,
                graphicName: "square",
                fillColor: "white",
                fillOpacity: 1,
                strokeWidth: 1,
                strokeOpacity: 1,
                strokeColor: "#6688CC"
            },
            "Line":
            {
                strokeWidth: 3,
                strokeOpacity: 0.5,
                strokeColor: "#6688CC",
                strokeDashstyle: "solid"
            },
            "Polygon":
            {
                strokeWidth: 2,
                strokeOpacity: 1,
                strokeColor: "#6688CC",
                fillColor: "white",
                fillOpacity: 0.3
            }
        };

        /*测距-画线*/
        var style = new OpenLayers.Style();
        style.addRules([
                new OpenLayers.Rule({ symbolizer: sketchSymbolizers })
            ]);
        var styleMap = new OpenLayers.StyleMap({ "default": style });
        measureControls = {
            line: new OpenLayers.Control.Measure(
                    OpenLayers.Handler.Path,
                    {
                        title: '测距',
                        measureComplete: function (event) {
                            var p = event.components[event.components.length - 1];
                            var html = "<div style='border:1px solid #6688CC;font-size:12px;background-color:white;width:auto;height:auto;text-align:center'><font style='color:red'>终点</font>，共：" + $("#Measure").attr("m") + "</div>";
                            var layer = new OpenLayers.Layer.Markers("终点");
                            VNMAP.MeasurePoint.push(layer);
                            VNMAP.TipInfo(p, html, layer);
                            $("#Measure").html("单击选择起点,<b>Esc</b>&nbsp;键或单击\"平移\"按钮退出");
                        },
                        persist: true,
                        handlerOptions:
                        {
                            layerOptions: { styleMap: styleMap }
                        }
                    }
                ),
            polygon: new OpenLayers.Control.Measure(
                    OpenLayers.Handler.Polygon,
                     {
                         measureComplete: function (event) {
                             var p = event.components[0].components[event.components[0].components.length - 2]; //event.components[0].components
                             var html = "<div style='border:1px solid #6688CC;font-size:12px;background-color:white;width:auto;height:auto;text-align:center'>共：" + $("#Measure").attr("m") + "</div>";
                             var layer = new OpenLayers.Layer.Markers("测面积");
                             VNMAP.MeasurePoint.push(layer);
                             VNMAP.TipInfo(p, html, layer);
                             $("#Measure").html("单击图层画面,<b>Esc</b>&nbsp;键或单击\"平移\"按钮退出");
                         },
                         persist: true,
                         handlerOptions:
                        {
                            layerOptions: { styleMap: styleMap }
                        }
                     }
                )
        };
        var control;
        for (var key in measureControls) {
            control = measureControls[key];
            control.events.on({
                "measure": this.handleMeasurements,
                "measurepartial": this.handleMeasurements
            });
            this.map.addControl(control);
        };
        this.toggleImmediate(true);

        S = new OpenLayers.Layer.TMS("卫星影像", "", { type: 'jpg', getURL: this.getS, isBaseLayer: true, transitionEffect: "resize" });
        M = new OpenLayers.Layer.TMS("矢量地图", "", { type: 'png', getURL: this.getM, isBaseLayer: true, transitionEffect: "resize", layers: "basic" });
        this.Layers = [M];
        if (sLayer) {
            this.Layers = [M, S];
        }
        this.map.addLayers(this.Layers); //添加到层图
        //设置中心
        this.map.setCenter(new OpenLayers.LonLat(lonlat.lon, lonlat.lat), center.zoom);
        var element = document.createElement("a");
        element.style.textDecoration = "none";
        element.style.position = "absolute";
        element.style.width = "70px";
        element.style.zIndex = "999";
        element.style.backgroundColor = "transparent";
        element.style.bottom = "0px";
        element.style.right = "20px";
        //element.innerHTML = "<img title='平光信息技术有限公司' src='img/number/pg_q.png' />"; //OpenLayers.i18n("©2011 VNMAP&Version 1.5.1");
        element.href = "#";
        this.map.div.appendChild(element);
        /*
        是否加载鹰眼
        */
        if (MiniMap) {
            //<div id="minimap" style="float:right;z-index:1000;position:relative;width: 200px; height: 150px;border:5px solid #000000;top:500px;"></div>
            var border = document.createElement("div");
            border.id = "border";
            border.style.position = "absolute";
            border.style.width = "208px";
            border.style.height = "158px";
            border.style.zIndex = "999";
            border.style.border = "1px solid #6688CC"; //#6688CC
            border.style.backgroundColor = "#DFE8F6";
            border.style.bottom = "-1px";
            border.style.right = "0px";
            border.style.display = "none";
            var icon = document.createElement("img");
            icon.src = "img/up.png";
            icon.style.position = "absolute";
            icon.style.zIndex = "999";
            icon.style.cursor = "pointer";
            icon.style.backgroundColor = "#DFE8F6";
            icon.style.bottom = "0px";
            icon.style.right = "0px";
            var f = false;
            icon.onclick = function () {
                if (f) {
                    $("#border").hide(300);
                    this.src = "img/up.png";
                    this.title = "打开鹰眼";
                    this.style.bottom = "0px";
                    this.style.right = "0px";
                    $(element).animate({ right: "20px" }, 300);
                    f = false;
                }
                else {
                    $("#border").show(300);
                    this.src = "img/down.png";
                    this.title = "关闭鹰眼";
                    this.style.bottom = "3px";
                    this.style.right = "4px";
                    $(element).animate({ right: "220px" }, 300);
                    f = true;
                }
            }

            var minidiv = document.createElement("div");
            minidiv.id = "minimap";
            minidiv.style.position = "absolute";
            minidiv.style.width = "200px";
            minidiv.style.height = "150px";
            minidiv.style.zIndex = "999";
            minidiv.style.border = "1px solid #6688CC";
            minidiv.style.left = "3px";
            minidiv.style.top = "3px";
            border.appendChild(minidiv);
            this.map.div.appendChild(border);
            this.map.div.appendChild(icon);
            VNMAP.MiniMapInit();
        }

        this.MarkLayers = new Array();
        //        VNMAP.TFM("red", "1000000");
        //        VNMAP.TFM("lime", "1000001");



    },
    /*
    根据经纬度移动到指定位置和指定级别
    */
    GotoXY: function (x, y, z) {
        var lonlat = this.ParseLonLatToPx(x, y); //经纬度转墨卡托坐标
        lonlat = new OpenLayers.LonLat(lonlat.lon, lonlat.lat); //坐标转换
        z = parseFloat(z);
        this.map.panTo(lonlat);
        VNMAP.SelectPoint(x, y);
        this.miniGotoXY(lonlat, VNMAP.minimap.getZoom());
        if (z) {
            setTimeout(function () {
                z = parseInt(z);
                VNMAP.map.zoomTo(z);
            }, 1000);
        }
    },
    //移动鹰眼
    miniGotoXY: function (lonlat, z) {
        //var lonlat = this.map.getLonLatFromPixel(lonlat);
        z = parseFloat(z);
        this.minimap.panTo(lonlat);
        if (z) {
            setTimeout(function () {
                z = parseInt(z);
                VNMAP.minimap.zoomTo(z);
            }, 1000);
        }
    },
    /*
    n:名称,
    w:点宽度,
    h:点高度,
    lonlat:坐标{lon,lat,x,y}
    */


    addPoint: function (n, w, h, lon, lat, b) {
        var PoiLonLat = VNMAP.ParseLonLatToPx(lon, lat); //经纬度转地图坐标
        markers = new OpenLayers.Layer.Markers(n);
        var size = new OpenLayers.Size(23, 25);  //标点大小         
        var offset = new OpenLayers.Pixel(-(size.w / 2), -size.h);
        var icon = new OpenLayers.Icon('img/marker2.png', size, offset, null);  //创建图标           
        markerDJ = new OpenLayers.Marker(new OpenLayers.LonLat(PoiLonLat.lon, PoiLonLat.lat), icon, null); //标点
        markerDJ.icon.imageDiv.className = "trigger";
        markerDJ.icon.imageDiv.x = lon;
        markerDJ.icon.imageDiv.y = lat;
        markerDJ.icon.imageDiv.name = n;
        markerDJ.icon.imageDiv.b = b;
        markerDJ.icon.imageDiv.style.cursor = "pointer";
        markers.addMarker(markerDJ); //投下标点
        VNMAP.map.addLayer(markers);
        this.map.setLayerZIndex(markers, -1);
        markerDJ.icon.imageDiv.onclick = function () {
            VNMAP.GotoXY(this.x, this.y, VNMAP.map.getZoom());
            var id = this.id;
            var imgDiv = this;
            var lonLat = VNMAP.map.getCenter();
            var ViewPortPx = VNMAP.map.getViewPortPxFromLonLat(lonLat);
            $("#" + id).colorbox({ onComplete: function () {

                $("#cboxBottomCenter").css({ width: $("#cboxBottomCenter")[0].offsetWidth });
            }, modal: false, parentDiv: imgDiv, opacity: 0.0, overlayClose: false, top: -132, left: -93, html: "<div style='margin-top:10px;margin-left:10px'>地点名称：" + this.name + "<br/><br/>地点描述：" + this.b + "</div>", innerHeight: 80, innerWidth: 200, close: "<span onclick=\"javascript:isClose=true\">关闭</span>"
            });
        };
        return markers;
    },
    addSpanPoint: function (lon, lat) {
        var PoiLonLat = VNMAP.ParseLonLatToPx(lon, lat); //经纬度转地图坐标
        if (this.CustomMarkers == null) {
            this.CustomMarkers = new OpenLayers.Layer.Markers("临时点");
        }
        var size = new OpenLayers.Size(20, 34);  //标点大小         
        var offset = new OpenLayers.Pixel(-(size.w / 2), -size.h);
        var icon = new OpenLayers.Icon('img/number/tb2.gif', size, offset, null);  //创建图标    


        markerDJ = new OpenLayers.Marker(new OpenLayers.LonLat(PoiLonLat.lon, PoiLonLat.lat), icon, null); //标点
        this.CustomMarkers.addMarker(markerDJ); //投下标点
        VNMAP.map.addLayer(this.CustomMarkers);
        return markerDJ;
    },
    addSpanPoint123: function (lon, lat, count) {
        var PoiLonLat = VNMAP.ParseLonLatToPx(lon, lat); //经纬度转地图坐标
        if (this.CustomMarkers == null) {
            this.CustomMarkers = new OpenLayers.Layer.Markers("临时点");
        }
        var size = new OpenLayers.Size(20, 34);  //标点大小         
        var offset = new OpenLayers.Pixel(-(size.w / 2), -size.h);
        var icon = new OpenLayers.Icon('img/number/tb' + count + '.gif', size, offset, null);  //创建图标    


        markerDJ = new OpenLayers.Marker(new OpenLayers.LonLat(PoiLonLat.lon, PoiLonLat.lat), icon, null); //标点
        this.CustomMarkers.addMarker(markerDJ); //投下标点
        VNMAP.map.addLayer(this.CustomMarkers);
        return markerDJ;
    },
    addSpanPointGps: function (lon, lat, count) {
        var PoiLonLat = VNMAP.ParseLonLatToPx(lon, lat); //经纬度转地图坐标
        if (this.GPSMarkers == null) {
            this.GPSMarkers = new OpenLayers.Layer.Markers("轨迹回放");
        }
        var size = new OpenLayers.Size(20, 34);  //标点大小         
        var offset = new OpenLayers.Pixel(-(size.w / 2), -size.h);
        var icon = new OpenLayers.Icon('img/number/tb' + count + '.gif', size, offset, null);  //创建图标    


        markerDJ = new OpenLayers.Marker(new OpenLayers.LonLat(PoiLonLat.lon, PoiLonLat.lat), icon, null); //标点
        this.GPSMarkers.addMarker(markerDJ); //投下标点
        VNMAP.map.addLayer(this.GPSMarkers);
        return markerDJ;
    },
    //添加标注
    addMarker: function (img, markers, n, lon, lat, ptype, obj) {

        var size = new OpenLayers.Size(30, 30);  //标点大小
        //var size = new OpenLayers.Size(64, 64);  //标点大小
        //var size2 = new OpenLayers.Size(37, 34); //阴影
        var offset = new OpenLayers.Pixel(-(size.w / 2), -size.h);
        var PoiLonLat = VNMAP.ParseLonLatToPx(lon, lat); //经纬度转地图坐标
        var icon = new OpenLayers.Icon(img, size, offset, null);  //创建图标
        markerDJ = new OpenLayers.Marker(new OpenLayers.LonLat(PoiLonLat.lon, PoiLonLat.lat), icon, null); //标点
        markerDJ.icon.imageDiv.className = "trigger";
        markerDJ.icon.imageDiv.x = lon;
        markerDJ.icon.imageDiv.y = lat;

        markerDJ.icon.imageDiv.name = n;
        markerDJ.icon.imageDiv.ptype = ptype;
        markerDJ.icon.imageDiv.data = obj;
        markerDJ.icon.imageDiv.style.cursor = "pointer";
        if (obj != null) {
            markerDJ.icon.imageDiv.title = obj.name;
            // VNMAP.Text(lon, lat, obj.name, offset.x, offset.y, "blue");
        }

        markers.addMarker(markerDJ); //投下标点
        markerDJ.icon.imageDiv.onclick = function () {
            if (isClose) {
                isClose = false;
                return;
            }
            this.style.cursor = "";
            imgDiv = this;
            //            if (infoBox) {
            //                infoBox.colorbox.close();
            //            }
            VNMAP.GotoXY(lon, lat, VNMAP.map.getZoom());
            switch (ptype) {
                case "COM":
                    var html = "<div style='margin-top:0px;margin-left:0px;text-align:center'><table  border='0' cellpadding='3' cellspacing='0' style='width: 100%;margin:auto'><tr><td>" + imgDiv.data.name + "</td></tr></table></div>";
                    infoBox = $.colorbox({
                        onComplete: function () {
                        }, modal: false, parentDiv: markerDJ.icon.imageDiv, opacity: 0.0, overlayClose: false, escKey: false, top: -135, left: -90, html: html, title: "", innerHeight: 60, innerWidth: 220, close: "<span onclick=\"javascript:isClose=true\">关闭</span>"
                    });
                    break;
                case "TGS":
                    // VNMAP.CreateInfoBox("TgsShow.aspx?kkid=" + imgDiv.data.id, -220, -300, 420, 375);
                    GisBrowse.ShowOpenWindow(imgDiv.data.name, "480", "410", "../Map/TgsShow.aspx?kkid=" + imgDiv.data.id);
                    break;
                case "TMS":
                    //VNMAP.CreateInfoBox("TgsShow.aspx?kkid=" + imgDiv.data.id, -220, -300, 420, 375);
                    GisBrowse.ShowOpenWindow(imgDiv.data.name, "500", "400", "../Map/TgsShow.aspx?kkid=" + imgDiv.data.id);
                    break;
                case "TPS":
                    VNMAP.CreateInfoBox("TgsShow.aspx?kkid=" + imgDiv.data.id, -220, -300, 420, 375);
                    break;
                case "CCTV":
                    var szDevpame = imgDiv.data.ip;
                    var arrmp = szDevpame.split("|");
                    var szDevUser = arrmp[3];
                    var szDevPwd = arrmp[4];
                    var szDevPort = arrmp[2];
                    var szDevIp = arrmp[0];
                    var szRecodeType = arrmp[5];
                    switch (szRecodeType) {
                        case "4":

                            GisBrowse.ShowOpenWindow(imgDiv.data.name, "500", "400", "../CCTVSingleBrowse.aspx?videoUrl=" + imgDiv.data.name + "|" + imgDiv.data.ip);
                            //VNMAP.CreateInfoBox1("CCTVSingleBrowse.aspx?videoUrl=" + imgDiv.data.ip, -200, -300, 500, 300);
                            break;
                        case "6":
                            GisBrowse.ShowOpenWindow(imgDiv.data.name, "500", "300", "../CCTVSingleBrowse.aspx?videoUrl=" + imgDiv.data.name + "|" + imgDiv.data.ip);
                            // VNMAP.CreateInfoBox1("CCTVSingleBrowse.aspx?videoUrl=" + imgDiv.data.ip, -200, -300, 500, 300);
                            break;

                        case "38":
                            GisBrowse.ShowOpenWindow(imgDiv.data.name, "500", "300", "VLCPlayer.htm?videoUrl=rtsp://admin:123456@" + szDevIp + ":554/mpeg4");
                            //VNMAP.CreateInfoBox1("VLCPlayer.htm?videoUrl=rtsp://admin:123456@" + szDevIp + ":554/mpeg4", -200, -350, 400, 300);
                            break;
                    }
                    break;
                case "UTC":
                    GisBrowse.ShowOpenWindow(imgDiv.data.name, '520', '420', "UtcShow.aspx?kkid=" + imgDiv.data.ip);
                    break;
                case "TFM":
                    GisBrowse.ShowOpenWindow(imgDiv.data.name, "420", "375", "TfmShow.aspx?kkid=" + imgDiv.data.id);
                    //VNMAP.CreateInfoBox("TfmShow.aspx?kkid=" + imgDiv.data.id, -200, -300, 410, 310);
                    break;
                case "VMS":
                    var szDevpame = imgDiv.data.ip;
                    var arrmp = szDevpame.split("|");
                    var szWidth = arrmp[1] * 1 + 50;
                    var szHeight = arrmp[2] * 1 + 60;
                    GisBrowse.ShowOpenWindow(imgDiv.data.name, szWidth, szHeight, "VmsShow.aspx?kkid=" + imgDiv.data.id);
                    //VNMAP.CreateInfoBox("VmsShow.aspx?kkid=" + imgDiv.data.id, -200, -141, 350, 300);
                    break;
                case "TES":
                    VNMAP.CreateInfoBox("TesShow.aspx?kkid=" + imgDiv.data.id, -200, -300, 415, 390);
                    break;
                case "WEA":
                    GisBrowse.ShowOpenWindow(imgDiv.data.name, "515", "440", "WeaShow.aspx?kkid=" + imgDiv.data.id);
                    //VNMAP.CreateInfoBox("WeaShow.aspx?kkid=" + imgDiv.data.id, -200, -300, 500, 400);
                    break;
                case "ZD":
                    ZDBrowse.ShowOpenWindow("交通施工信息", "515", "400", "CTJShow.aspx?id=" + imgDiv.data.id);
                    break;
                case "GZ":
                    ZDBrowse.ShowOpenWindow("交通管制信息", "600", "260", "TFCShow.aspx?id=" + imgDiv.data.id);
                    break;

            }
        }
        return [markerDJ];
    },



    SaveMarker: function (evt) {

        if (VNMAP.DDPOI.FLAG) {
            $("#Measure").remove();
            this.MeasureMouseInfoDiv = null;
            this.map.div.style.cursor = "";
            var lonlat = this.map.getLonLatFromPixel(evt.xy);
            lonlat = this.ParsePxToLonLat(lonlat);
            switch (VNMAP.DDPOI.TYPE) {

                case "ZD":
                case "GZ":
                    var res = ZDBrowse.SaveMarkInfo(lonlat.lon, lonlat.lat, VNMAP.DDPOI.TYPE, VNMAP.DDPOI.DATA.col0, "");
                    break;
                default:
                    var res = GisBrowse.SaveMarkInfo(lonlat.lon, lonlat.lat, VNMAP.DDPOI.TYPE, VNMAP.DDPOI.DATA.col0, VNMAP.DDPOI.DATA.col1);
                    break;
            }

            VNMAP.DDPOI.FLAG = false;
        }
    },
    SaveMarker2: function (img, x, y, ptype, obj) {

        VNMAP.SavaMarkType(img, x, y, ptype, obj);
    }
    ,
    SavaMarkType: function (img, x, y, ptype, obj) {

        var DeviceLayer = null;

        for (var i = 0; i < this.MarkLayers.length; i++) {
            var tmplayer = this.MarkLayers[i];

            if (tmplayer.name == VNMAP.GetMarkerType(ptype)) {
                DeviceLayer = tmplayer;
            }
        }
        if (DeviceLayer == null) {
            var layerType = VNMAP.GetMarkerType(ptype);
            DeviceLayer = new OpenLayers.Layer.Markers(layerType);
            this.MarkLayers.push(DeviceLayer);

        }
        var mark = this.addMarker(img, DeviceLayer, ptype, x, y, ptype, obj);
        this.map.setLayerZIndex(DeviceLayer, -2);
        this.map.addLayer(DeviceLayer);
        MarkLayers = [];
    },
    Clear: function () {

        for (var i = 0; i < this.MarkLayers.length; i++) {
            var tmplayer = this.MarkLayers[i];
            this.map.removeLayer(tmplayer);

        }
        this.MarkLayers = null;
        this.MarkLayers = new Array();
    },
    ClearMarkType: function (ptype) {

        for (var i = 0; i < this.MarkLayers.length; i++) {
            var tmplayer = this.MarkLayers[i];

            if (tmplayer.name == VNMAP.GetMarkerType(ptype)) {
                tmplayer.clearMarkers();

            }
        }

    },
    GetMarkerType: function (markType) {
        var stype = "自定义";
        switch (markType) {
            case "TMS":
                stype = "电子警察";
                break;
            case "TGS":
                stype = "治安卡口";
                break;
            case "TPS":
                stype = "电子测速";
                break;
            case "CCTV":
                stype = "视频监控";
                break;
            case "VMS":
                stype = "诱导大屏";
                break;
            case "UTC":
                stype = "交通信号";
                break;
            case "WEA":
                stype = "气象监测";
                break;
            case "TES":
                stype = "事件检测";
                break;
            case "TFM":
                stype = "交通流量";
                break;
            case "COM":
                stype = "自定义";
                break;
            case "CAR":
                stype = "GPS车辆";
                break;
            case "ZD":
                stype = "交通施工占道";
                break;
            case "GZ":
                stype = "交通管制信息";
                break;
        }
        return stype;
    },

    CreateInfoBox: function (url, top, left, width, height) {
        if (infoBox) {
            setTimeout(function () {
                infoBox = $.colorbox({
                    modal: false, parentDiv: imgDiv, iframe: true, opacity: 0.0, overlayClose: false, top: top, left: left, href: url, innerHeight: height, innerWidth: width, close: "<span onclick=\"javascript:isClose=true\">关闭</span>"
                });
            }, 500);
        }
        else {
            infoBox = $.colorbox({
                modal: false, parentDiv: imgDiv, iframe: true, opacity: 0.0, overlayClose: false, top: top, left: left, href: url, innerHeight: height, innerWidth: width, close: "<span onclick=\"javascript:isClose=true\">关闭</span>"
            });
        }
    },
    CreateInfoBox1: function (url, top, left, width, height) {
        if (infoBox) {
            setTimeout(function () {
                infoBox = $.colorbox({
                    modal: false, parentDiv: imgDiv, iframe: true, opacity: 0.0, overlayClose: false, top: top, left: left, href: url, innerHeight: height, innerWidth: width, close: "<a href='setup.exe'>控件下载</a>&nbsp&nbsp<span onclick=\"javascript:isClose=true\">关闭</span>"
                });
            }, 500);
        }
        else {
            infoBox = $.colorbox({
                modal: false, parentDiv: imgDiv, iframe: true, opacity: 0.0, overlayClose: false, top: top, left: left, href: url, innerHeight: height, innerWidth: width, close: "<a href='setup.exe'>控件下载</a>&nbsp&nbsp<span onclick=\"javascript:isClose=true\">关闭</span>"
            });
        }
    },
    showCustomMarker: function (evt) {

        if (this.POI) {
            $("#Measure").remove();
            this.MeasureMouseInfoDiv = null;
            this.map.div.style.cursor = "";
            var lonlat = this.map.getLonLatFromPixel(evt.xy);
            lonlat = this.ParsePxToLonLat(lonlat);
            switch (VNMAP.ParentPage) {

                case "GisBrowse":
                    GisBrowse.ShowMarkWindow(lonlat.lon, lonlat.lat);
                    break;
                case "AlarmMapBrowse":
                    AlarmMapBrowse.ShowMarkWindow(lonlat.lon, lonlat.lat);
                    break;

            }
            this.POI = false;

        }

    },
    addCustomMarker2: function (img, x, y, obj) {
        if (this.CustomMarkers == null) {
            this.CustomMarkers = new OpenLayers.Layer.Markers("自定义标注");
        }
        var mark = this.addMarker(img, this.CustomMarkers, "自定义标注", x, y, "Custom", obj);
        this.map.addLayer(this.CustomMarkers);
    },
    /*
    自定义标注
    */
    addCustomMarker: function (evt) {
        $("#Measure").remove();
        this.MeasureMouseInfoDiv = null;
        this.map.div.style.cursor = "";
        var lonlat = this.map.getLonLatFromPixel(evt.xy);
        var ViewPortPx = this.map.getViewPortPxFromLonLat(lonlat);
        if (this.CustomMarkers == null) {
            this.CustomMarkers = new OpenLayers.Layer.Markers("自定义标注");
        }
        lonlat = this.ParsePxToLonLat(lonlat);
        var m = this.addMarker('img/marker2.png', this.CustomMarkers, "自定义标注", lonlat.lon, lonlat.lat);
        this.map.addLayer(this.CustomMarkers);
    },

    /*
    移除临时点
    */
    removePoint: function (markers) {

        this.map.removeLayer(markers);
    },
    removePoint2: function (x, y, ptype) {
        var markLayer = null;
        for (var i = 0; i < this.MarkLayers.length; i++) {
            var tmplayer = this.MarkLayers[i];
            var layerType = VNMAP.GetMarkerType(ptype);
            if (tmplayer.name == layerType) {
                markLayer = tmplayer;
            }
        }
        if (markLayer != null) {
            var mark = markLayer;
            if (mark != null) {
                var i = 0;
                for (i = 0; i < mark.markers.length; i++) {

                    var point = mark.markers[i];
                    if (point.icon.imageDiv.x == x && point.icon.imageDiv.y == y) {
                        mark.removeMarker(point);
                    }
                }
            }
        }
        if (this.TextLayer != null) {
            var lola = VNMAP.ParseLonLatToPx(x, y);
            for (i = 0; i < this.TextLayer.features.length; i++) {
                var feature = this.TextLayer.features[i];
                if (feature.geometry.x == lola.lon && feature.geometry.y == lola.lat) {
                    this.TextLayer.eraseFeatures(feature);
                    this.TextLayer.removeFeatures(feature);
                }
            }

        }

    },
    /*
    测量类事件处理
    */
    handleMeasurements: function (event) {
        VNMAP.map.div.style.cursor = "url(img/ruler.cur)";
        var geometry = event.geometry;
        var units = (event.units == "km") ? "公里" : "米";
        var order = event.order;
        var measure = event.measure;
        var element = $("#Measure");
        var out = "";
        if (order == 1) {
            out += "<b>当前: " + measure.toFixed(3) + units + "</b><br />单击左键继续测距，双击结束";
            element.attr("m", measure.toFixed(3) + units);
        }
        else {
            out += "面积: " + measure.toFixed(3) + "<sup>2</" + "sup>" + units;
            element.attr("m", measure.toFixed(3) + "平方" + units);
        }
        element.html(out);
    },
    /*
    选择激活控件
    */
    toggleControl: function (v) {
        for (key in measureControls) {
            var control = measureControls[key];
            if (v == key) {
                control.activate();
            }
            else {
                control.deactivate();
            }
        }
    },
    toggleGeodesic: function (element) {
        for (key in measureControls) {
            var control = measureControls[key];
            control.geodesic = element.checked;
        }
    },
    toggleImmediate: function (bool) {
        for (key in measureControls) {
            var control = measureControls[key];
            control.setImmediate(bool);
        }
    },
    //测量信息框
    TipInfo: function (ll, html, layer) {
        var lonlat = new OpenLayers.LonLat(ll.x, ll.y);
        lonlat = VNMAP.ParsePxToLonLat(lonlat); //坐标转换
        var boxSize = new OpenLayers.Size(140, 20); //信息盒子大小
        boxOffset = new OpenLayers.Pixel(-((boxSize.w) - 144), -((boxSize.h) - 24));
        var box = new OpenLayers.InfoBox(null, boxSize, boxOffset); //信息框
        var InfoBox = new OpenLayers.Marker(new OpenLayers.LonLat(ll.x, ll.y), null, box); //标点信息盒子
        box.setHtml(html);
        layer.addMarker(InfoBox); //信息盒子
        VNMAP.map.addLayer(layer);
        box.display(true);
    },
    /*
    跟随鼠标的提示信息
    */
    MouseBox: function (msg) {
        var div = this.MeasureMouseInfoDiv;
        if (div == null) {
            div = document.createElement("div");
            div.id = "Measure";
            div.style.width = "auto";
            div.style.padding = "2px 5px 2px 5px";
            div.style.height = "auto";
            div.style.fontSize = "12px";
            div.style.backgroundColor = "white";
            div.style.border = "1px solid #6688CC";
            div.style.lineHeight = "20px";
            div.style.textAlign = "left";
            div.style.display = "block";
            div.style.position = "absolute";
            div.style.left = "0";
            div.style.top = "0";
            div.style.zIndex = "9999";
            div.style.cursor = "crosshair";
            $("#map").append(div);

            //绑定事件,跟随鼠标
            var floatX, floatY, boxX, boxY, pageX, pageY;
            var cX = $('#map').outerWidth(true);
            var cY = $('#map').outerHeight(true);
            $("#map").mousemove(function (event) {
                pageX = event.clientX + $(window).scrollLeft();
                pageY = event.clientY + $(window).scrollTop();
                boxX = $('#Measure').outerWidth(true);
                boxY = $('#Measure').outerHeight(true);
                if ((cX - event.clientX) < (boxX + 10)) {
                    floatX = pageX - boxX - 10;
                } else {
                    floatX = pageX + 10;
                }
                if ((cY - event.clientY) < (boxY + 0)) {
                    floatY = pageY - boxY - 10;
                } else {
                    floatY = pageY + 10;
                }
                $('#Measure').css({ top: floatY - 35, left: floatX });
            });
            this.MeasureMouseInfoDiv = div;
        }
        div.innerText = msg;
        $("#Measure").show();
    },
    /*
    测量类
    */
    Measure: function (type, msg) {

        if (type == undefined) {
            VNMAP.isMeasure = false;
            this.toggleControl("none");
            if (VNMAP.MeasurePoint.length > 0) {
                for (i = 0; i < VNMAP.MeasurePoint.length; i++) {
                    VNMAP.map.removeLayer(VNMAP.MeasurePoint[i]);
                };
                VNMAP.MeasurePoint = new Array();
            };
            $("#Measure").remove();
            this.MeasureMouseInfoDiv = null;
            return;
        }
        else if (msg == undefined) {
            msg = "";
        }
        else {
            VNMAP.isMeasure = true;
            VNMAP.map.div.style.cursor = "url(img/ruler.cur)";
        }

        this.toggleControl(type);
        this.MouseBox(msg);
        //绑定Esc键盘事件
        $(document).bind('keydown', function (e) {
            var key = e.keyCode;
            if (key === 27) {
                VNMAP.toggleControl("none");
                if (VNMAP.MeasurePoint.length > 0) {
                    for (i = 0; i < VNMAP.MeasurePoint.length; i++) {
                        VNMAP.map.removeLayer(VNMAP.MeasurePoint[i]);
                    };
                    VNMAP.MeasurePoint = new Array();
                };
                $("#Measure").remove();
                VNMAP.isMeasure = false;
                VNMAP.map.div.style.cursor = "url(img/openhand.cur)";
                VNMAP.MeasureMouseInfoDiv = null;
            }
        });
    },
    /*
    画圆
    */
    roundArea: function (x, y, m) {
        if (this.RoundLayer == null && this.Round == null) {
            var style_round = {
                strokeWidth: 2,
                strokeOpacity: 1,
                strokeColor: "#6688CC",
                fillColor: "blue",
                fillOpacity: 0.1,
                graphicZIndex: -9999
            };
            var layer_style = OpenLayers.Util.extend({}, OpenLayers.Feature.Vector.style['default']);
            layer_style.fillOpacity = 0.2;
            layer_style.graphicOpacity = 1;
            //            this.RoundLayer = new OpenLayers.Layer.Vector("侦查范围", { style: layer_style });
            this.RoundLayer = new OpenLayers.Layer.Vector("侦查范围", { style: style_round });
            polyOptions = { sides: 360, snapAngle: 0, radius: m ? m : 1000 };
            this.Round = new OpenLayers.Control.DrawFeature(this.RoundLayer,
                                            OpenLayers.Handler.RegularPolygon,
                                            { handlerOptions: polyOptions });
            this.map.addControl(this.Round);
            this.map.addLayer(this.RoundLayer);
            this.map.setLayerZIndex(this.RoundLayer, -2);
            this.Round.handler.layer = this.RoundLayer;
            var maploc = this.ParseLonLatToPx(x, y);
            var xy = this.map.getPixelFromLonLat(maploc);
            this.Round.handler.down({ xy: xy });
            this.RoundLayer.div.title = "侦查范围：周围" + (m ? m : 1000) + "米";
        }
    },
    setBounds: function (x, y, m) {
        this.removeRound();
        this.roundArea(x, y, m);
    },
    /*
    清除圆形图层
    */
    removeRound: function () {
        if (this.RoundLayer != null && this.Round != null) {
            this.Round.handler.clear();
            this.Round = null;
            this.map.removeLayer(this.RoundLayer);
            this.RoundLayer = null;
        }
    },

    /*
    移除GPS轨迹层
    */
    removeGPS: function () {
        if (this.GPSLayer != null) {
            this.map.removeLayer(this.GPSLayer);
            this.GPSLayer = null;
        }
        if (this.GPSMarkers != null) {
            VNMAP.removePoint(this.GPSMarkers);
            this.GPSMarkers = null;
        }

    },
    /*
    GPS轨迹回放
    */
    GPS: function (datalist) {
        this.removeGPS();
        var vectors, lineFeature; //存放线路 
        //线路样式 
        var style_GPS = {
            strokeWidth: 3,
            strokeOpacity: 0.8,
            strokeColor: "blue",
            strokeDashstyle: "solid",
            pointRadius: 1,
            pointerEvents: "visiblePainted"
        };
        //画线图层设置 
        var layer_style = OpenLayers.Util.extend({}, OpenLayers.Feature.Vector.style['default']);
        layer_style.fillOpacity = 0.2;
        layer_style.graphicOpacity = 1;

        //画线图层 
        vectors = new OpenLayers.Layer.Vector("车辆轨迹线", { style: layer_style });
        this.map.addLayer(vectors);
        this.map.setLayerZIndex(vectors, -1);
        this.GPSLayer = vectors;

        //获取坐标信息
        var x = datalist[0].split(',')[1];
        var y = datalist[0].split(',')[2];
        var lola = VNMAP.ParseLonLatToPx(x, y);
        newPoint = new OpenLayers.Geometry.Point(lola.lon, lola.lat);
        lineFeature = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.LineString(newPoint), null, style_GPS);
        vectors.addFeatures(lineFeature);

        //开始轨迹回放
        var i = 0;
        var GPS_Timer = setInterval(function () {
            if (datalist.length > i) {
                var x = datalist[i].split(',')[1];
                var y = datalist[i].split(',')[2];
                VNMAP.GotoXY(x, y, VNMAP.map.getZoom());
                var lola = VNMAP.ParseLonLatToPx(x, y);
                newPoint = new OpenLayers.Geometry.Point(lola.lon, lola.lat);
                lineFeature.geometry.addPoint(newPoint);
                vectors.drawFeature(lineFeature);
                VNMAP.addSpanPointGps(x, y, i + 1);
                i++;
            }
            else {
                clearInterval(GPS_Timer);
            }
        }, 1000);
    },
    /*
    移除GPS轨迹层
    */
    removeTFM: function () {
        if (this.TFMLayer != null) {
            this.map.removeLayer(this.TFMLayer);
            this.TFMLayer = null;
        }
        if (this.TFMMarkers != null) {
            VNMAP.removePoint(this.TFMMarkers);
            this.TFMMarkers = null;
        }

    },
    SelectPoint: function (x, y) {

        var style_blue = OpenLayers.Util.extend({}, OpenLayers.Feature.Vector.style['default']);
        style_blue.strokeColor = "blue";
        style_blue.fillColor = "blue";
        var style_green = {
            strokeColor: "#339933",
            strokeOpacity: 1,
            strokeWidth: 3,
            pointRadius: 15,
            pointerEvents: "visiblePainted"
        };
        if (this.PointLayer != null) {
            this.map.removeLayer(this.PointLayer);
            this.PointLayer = null;
        }
        if (this.PointLayer == null) {
            this.PointLayer = new OpenLayers.Layer.Vector("动态点", { style: style_blue });

        }
        var lola = VNMAP.ParseLonLatToPx(x, y);
        var point = new OpenLayers.Geometry.Point(lola.lon, lola.lat);
        var dynamicFeature = new OpenLayers.Feature.Vector(point, null, style_blue);
        this.map.addLayer(this.PointLayer);
        this.map.setLayerZIndex(this.PointLayer, -1);
        this.PointLayer.addFeatures([dynamicFeature]);
    },

    /*
    流量图层
    */
    TFM: function (color, link_id) {
        // this.removeTFM();
        var tfm_lineFeature; //存放线路 
        //线路样式 
        var style_TFM = {
            strokeWidth: 6,
            strokeOpacity: 1,
            strokeColor: color,
            strokeDashstyle: "solid",
            pointRadius: 1,
            pointerEvents: "visiblePainted"
        };
        //画线图层设置 
        var tfm_layer_style = OpenLayers.Util.extend({}, OpenLayers.Feature.Vector.style['default']);
        tfm_layer_style.fillOpacity = 0.2;
        tfm_layer_style.graphicOpacity = 1;

        //画线图层 
        if (this.tfm_vectors == null) {
            this.tfm_vectors = new OpenLayers.Layer.Vector("流量状态", { style: tfm_layer_style });
            this.map.addLayer(this.tfm_vectors);
        }
        this.map.setLayerZIndex(this.tfm_vectors, -3);
        this.TFMLayer = this.tfm_vectors;


        var xmlDoc = new ActiveXObject('Microsoft.XMLDOM');
        xmlDoc.async = false;
        try {
            xmlDoc.load("kml/" + link_id + ".kml");
        }
        catch (e) {
        }
        if (xmlDoc != null) {
            var nodes = xmlDoc.getElementsByTagName("kml/Document/Placemark/LineString/coordinates")[0].childNodes;
            for (var i = 0; i < nodes.length; i++) {
                //如果接点名为 tree 
                if (nodes(i).nodeValue != "") {
                    var str = nodes(i).nodeValue;
                    var strs = new Array(); //定义一数组
                    strs = str.split("0 "); //字符分割      
                    for (i = 0; i < strs.length - 1; i++) {
                        var xys = new Array();
                        xys = strs[i].split(",");
                        if (i == 0) {

                            var x1 = xys[0];
                            var y1 = xys[1];
                            x1 = x1 * 1 + 0.006;
                            y1 = y1 * 1 + 0.001;
                            var lola = VNMAP.ParseLonLatToPx(x1, y1);
                            newPoint = new OpenLayers.Geometry.Point(lola.lon, lola.lat);
                            tfm_lineFeature = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.LineString(newPoint), null, style_TFM);
                            this.tfm_vectors.addFeatures(tfm_lineFeature);
                        }
                        else {
                            var x2 = xys[0];
                            var y2 = xys[1];
                            x2 = x2 * 1 + 0.006;
                            y2 = y2 * 1 + 0.001;
                            var lola2 = VNMAP.ParseLonLatToPx(x2, y2);
                            newPoint = new OpenLayers.Geometry.Point(lola2.lon, lola2.lat);
                            tfm_lineFeature.geometry.addPoint(newPoint);
                            this.tfm_vectors.drawFeature(tfm_lineFeature);
                        }

                    }
                }
            }
        }
        window.setInterval(function () { VNMAP.FlashPoint() }, 1000);

    },
    TFMSTATE: function (color, strTemp) {
        var tfm_lineFeature; //存放线路 
        //线路样式 
        var style_TFM = {
            strokeWidth: 5,
            strokeOpacity: 1,
            strokeColor: color,
            strokeDashstyle: "solid",
            pointRadius: 5,
            pointerEvents: "visiblePainted"
        };
        //画线图层设置 
        var tfm_layer_style = OpenLayers.Util.extend({}, OpenLayers.Feature.Vector.style['default']);
        tfm_layer_style.fillOpacity = 0.2;
        tfm_layer_style.graphicOpacity = 1;

        //画线图层 
        if (this.tfm_vectors == null) {
            this.tfm_vectors = new OpenLayers.Layer.Vector("流量状态", { style: tfm_layer_style });
            this.map.addLayer(this.tfm_vectors);
         }
        this.map.setLayerZIndex(this.tfm_vectors, -3);
        this.TFMLayer = this.tfm_vectors;

        var str = strTemp;
        var strs = new Array(); //定义一数组
        strs = str.split("|"); //字符分割      
        for (i = 0; i < strs.length; i++) {
            var xys = new Array();
            xys = strs[i].split(",");
            if (i == 0) {

                var x1 = xys[0];
                var y1 = xys[1];
                x1 = x1 * 1 + 0.0062;
                y1 = y1 * 1 + 0.0014;
                var lola = VNMAP.ParseLonLatToPx(x1, y1);
                newPoint = new OpenLayers.Geometry.Point(lola.lon, lola.lat);

                tfm_lineFeature = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.LineString(newPoint), null, style_TFM);
                this.tfm_vectors.addFeatures(tfm_lineFeature);
            }
            else {
                var x2 = xys[0];
                var y2 = xys[1];
                x2 = x2 * 1 + 0.0062;
                y2 = y2 * 1 + 0.0014;
                var lola2 = VNMAP.ParseLonLatToPx(x2, y2);
                newPoint = new OpenLayers.Geometry.Point(lola2.lon, lola2.lat);
                tfm_lineFeature.geometry.addPoint(newPoint);
                this.tfm_vectors.drawFeature(tfm_lineFeature);
            }

        }

    },
    FlashPoint: function () {

        if (this.tfm_vectors != null) {
            for (var i = 0; i < this.tfm_vectors.features.length; i++) {
                var feature = this.tfm_vectors.features[i];
                var color = feature.style.strokeColor;
                switch (color) {
                    case "red":
                        color = "#C00000";
                        break;
                    case "#C00000":
                        color = "red";
                        break;
                    case "yellow":
                        color = "#DAFE30";
                        break;
                    case "#DAFE30":
                        color = "yellow";
                        break;
                    case "lime":
                        color = "#00C000";
                        break;
                    case "#00C000":
                        color = "lime";
                        break;
                }
                feature.style.strokeColor = color;
                this.tfm_vectors.drawFeature(feature);
                this.map.setLayerZIndex(this.tfm_vectors, -2);
            }
        }
    },
    Text: function (x, y, txt, xleft, ytop, color) {

        var renderer = OpenLayers.Util.getParameters(window.location.href).renderer;
        renderer = (renderer) ? [renderer] : OpenLayers.Layer.Vector.prototype.renderers;

        if (this.TextLayer == null) {
            this.TextLayer = new OpenLayers.Layer.Vector("文字标注", {
                styleMap: new OpenLayers.StyleMap({ 'default': {
                    strokeColor: "#00FF00",
                    strokeOpacity: 1,
                    strokeWidth: 3,
                    fillColor: "#FF5500",
                    fillOpacity: 0.5,
                    pointRadius: 0,
                    pointerEvents: "visiblePainted",
                    label: "${name}",
                    fontColor: "${favColor}",
                    fontSize: "10px",
                    fontFamily: "宋体, monospace",
                    fontWeight: "bold",
                    labelAlign: "cm",
                    labelXOffset: "${left}",
                    labelYOffset: "${top}"
                }
                }),
                renderers: renderer
            });

            this.map.addLayer(this.TextLayer);
        }

        var lola = VNMAP.ParseLonLatToPx(x, y);
        newPoint = new OpenLayers.Geometry.Point(lola.lon, lola.lat);
        var pointFeature = new OpenLayers.Feature.Vector(newPoint);
        var l = 15 + xleft;
        var t = 30 + ytop;
        pointFeature.attributes = {
            name: txt,
            favColor: color,
            align: "cm",
            left: l,
            top: t
        };
        this.isRemoveLayer = false;
        this.TextLayer.addFeatures([pointFeature]);

    },
    RemoveAlarmMarker: function (obj) {
        if (this.AlarmLayer != null) {
            this.map.removeLayer(this.AlarmLayer);
            this.AlarmLayer = null;
        }
    },
    AddAlarmMarker: function (x, y, obj) {

        if (this.AlarmLayer != null) {
            this.map.removeLayer(this.AlarmLayer);
            this.AlarmLayer = null;
        }
        var renderer = OpenLayers.Util.getParameters(window.location.href).renderer;
        renderer = (renderer) ? [renderer] : OpenLayers.Layer.Vector.prototype.renderers;

        this.AlarmLayer = new OpenLayers.Layer.Vector("报警", {
            styleMap: new OpenLayers.StyleMap({ 'default': {
                strokeColor: "#00FF00",
                strokeOpacity: 1,
                strokeWidth: 1,
                fillColor: "#FF5500",
                fillOpacity: 1,
                pointRadius: 0,
                pointerEvents: "visiblePainted",
                label: "${name}",
                graphicZIndex: 1,
                graphicWidth: 100,
                graphicHeight: 100,
                graphicXOffset: -50,
                graphicYOffset: -50,
                externalGraphic: "img/Alarm.gif",
                fontColor: "${favColor}",
                fontSize: "14px",
                fontFamily: "宋体, monospace",
                fontWeight: "bold",
                labelAlign: "cm",
                labelXOffset: "0",
                labelYOffset: "20"
            }
            }),
            renderers: renderer
        });
        this.map.addLayer(this.AlarmLayer);
        var lola = VNMAP.ParseLonLatToPx(x, y);
        newPoint = new OpenLayers.Geometry.Point(lola.lon, lola.lat);
        var pointFeature = new OpenLayers.Feature.Vector(newPoint);
        pointFeature.attributes = {
            name: obj.name,
            favColor: 'blue',
            align: "cm"
        };
        var popup;
        this.AlarmLayer.addFeatures([pointFeature]);
        VNMAP.GotoXY(x, y, VNMAP.map.getZoom());
        var selectControl = new OpenLayers.Control.SelectFeature(this.AlarmLayer,
                { onSelect: onFeatureSelect, onUnselect: onFeatureUnselect });
        function onPopupClose(evt) {
            selectControl.unselect(pointFeature);
        }
        function onFeatureSelect(feature) {
            selectedFeature = feature;

            var html = "<div style='margin-top:0px;margin-left:10px;font-size:12px'><table><tr><td>" + obj.bjyy + "</td></tr><tr><td><br/><img src=" + obj.zjwj + "  style='height: 250px; width: 300px'/></tr><tr><td>&nbsp;</td></tr></table></div>";
            popup = new OpenLayers.Popup.FramedCloud("chicken",
                        feature.geometry.getBounds().getCenterLonLat(),
                        null,
                        html,
                        null,
                        false,
                        onPopupClose);
            popup.setBackgroundColor("#ffffff");
            popup.setBorder("2px solid #d91f12");
            feature.popup = popup;
            this.map.addPopup(popup);

        }
        function onFeatureUnselect(feature) {
            this.map.removePopup(feature.popup);
            feature.popup.destroy();
            feature.popup = null;
        }
        this.map.addControl(selectControl);
        selectControl.activate();
    },
    /*
    切换城市
    */
    setCity: function (x, y, p, c, n) {
        province = p;
        city = c;
        cityName = n;
        document.getElementById("CurCity").innerHTML = cityName;
        this.GotoXY(x, y, this.map.getZoom());
        if (selectWin) {
            selectWin.hide();
        }
    }

};


