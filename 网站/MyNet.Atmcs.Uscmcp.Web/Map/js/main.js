var x1;
var y1;
var store;
var myMask;
var TEMP;
var infoBox;
var isClose = false;
var xjInfo = null;
var loadMask = null;

var name = null;
var txt_cphm = null;
var btn = null;
var keyword = null;
var btn_pos = null;
var btn_point = null;
var btn_clear = null;
var c_grid = null;
var c_store = null;
var c_bbar = null;
var c_kssj = null;
var c_jssj = null;
var c_clzl = null;
var d_grid = null;
var d_store = null;
var d_bbar = null;
var e_grid = null;
var e_store = null;
var e_bbar = null;
var ptype;
var o_del = "system";
var box = null;
loadMask = new Ext.LoadMask(Ext.getBody(), { msg: '正在加载地图...', madal: false });
var selectWin = null;
function updateXjState(obj) {
    obj.disabled = true;
    v = Common.$("xjzt").innerHTML;
    Common.$("xjzt").innerHTML = "<span><img src='../App_Themes/Default/Frame/loading.gif' />正在更新状态...</span>";
    setTimeout(function () {
        Common.$("xjzt").innerHTML = v;
        obj.disabled = false;
    }, 3000);
};
function showXiangJiInfo(d) {
    var content = '<table style="height:100%"><tr><td>相机名称：' + d[0] + '</td></tr><tr><td>相机IP：' + d[1] + '</td></tr></tr><tr><td>相机型号：' + d[2] + '</td></tr><tr><td>相机状态：<span id="xjzt">' + d[3] + '</span> &nbsp;&nbsp;<a href="javascript:void(0);" onclick="updateXjState(this);">更新状态</a></td></tr><tr><td>监控方向：' + d[4] + '</td></tr><tr><td><a href="#">查看实时视频</a></td></tr></table>';
    if (!xjInfo) {
        xjInfo = new Ext.Window({
            layout: 'fit',
            maximizable: false,
            width: 270,
            draggable: true,
            resizable: false,
            modal: true,
            border: false,
            html: '<div style="padding-left:15px;padding-top:10px;padding-bottom:10px;line-height:25px" id="content">' + content + '</div>',
            autoDestroy: true,
            height: 220,
            closeAction: 'hide',
            plain: false
        });
    } else {
        Common.$("content").innerHTML = content;
    }
    xjInfo.setTitle(d[0] + "&nbsp;的详细信息");
    xjInfo.doLayout();
    xjInfo.show();
    xjInfo.center();
    Ext.Window.superclass.onDestroy.call(this);
};
function showSelectWin(title, width, height) {
    if (!selectWin) {
        selectWin = new Ext.Window({
            layout: 'fit',
            title: title,
            maximizable: false,
            width: 400,
            draggable: true,
            resizable: false,
            modal: true,
            border: false,
            html: '<div style="padding:20px 20px 20px 20px;">' + cityList + '</div>',
            autoDestroy: true,
            autoHeight: true,
            closeAction: 'hide',
            plain: false
        });
    }
    selectWin.setTitle(title);
    selectWin.doLayout();
    selectWin.show();
    selectWin.center();
    Ext.Window.superclass.onDestroy.call(this);
};
function CallWebService(method, params, onSuccess, onFailure) {
    Ext.Ajax.request({
        async: true,    // 异步模式
        headers: { 'Content-Type': 'application/json; charset=utf-8' },
        type: "POST",   // 发送数据
        url: method,    // WebService 地址和方法
        jsonData: params,   // 方法参数
        success: onSuccess, // 执行成功
        failure: onFailure  //执行失败
    });
};

Ext.onReady(function () {
    loadMask.show();
    var tree;
    var I = setInterval(function () {
        if (document.readyState == "complete") {
            setTimeout(function () {
                loadMask.hide();
                clearInterval(I);
            }, 100);
        }
    }, 1000);
    var html = '<div id="map" style="background-image:url(OpenLayers-2.11-rc3/img/bg.png);width: 100%; height: 100%"></div>';
 
    var left = new Ext.Panel({
        region: 'center',
        border: false,
        split: true,
        width: 1000,
        html: html,
        tbar: new Ext.Toolbar({
            items: [
                "当前城市：<span id='CurCity'>" + cityName + "</span>&nbsp;<a href='#city' onclick=\"javascript:showSelectWin('选择城市',400,300);\">切换城市</a>&nbsp;&nbsp;<a href='#reload' onclick='javascript:window.location.reload();'>重载地图</a>",
                '->',
                { id: 1, height: '16px', handler: OnToolbarClick, width: '22px', text: '平移' }, //text: '<div class="map_toolbar_btn"  title="平移" style="background-image: url(img/toolbar/pan_off.png);"></div>' },
                {id: 2, height: '16px', handler: OnToolbarClick, width: '22px', text: '中心点' }, //text: '<div class="map_toolbar_btn"  title="获取当前中心点坐标" style="background-image: url(img/toolbar/center.png);"></div>' },
                {id: 3, height: '16px', handler: OnToolbarClick, width: '22px', text: '测距' }, //text: '<div class="map_toolbar_btn"  title="测距" style="background-image: url(img/toolbar/ruler.png);"></div>' },
            //{ id: 4, height: '16px', handler: OnToolbarClick, width: '22px', text: '<div class="map_toolbar_btn"  title="锁定当前范围" style="background-image: url(img/toolbar/move_feature_off.png);"></div>' },
                {id: 5, height: '16px', handler: OnToolbarClick, width: '22px', text: '测面积' }, // text: '<div class="map_toolbar_btn"  title="测面积" style="background-image: url(img/toolbar/draw_polygon_off.png);"></div>' },
                {id: 6, height: '16px', handler: OnToolbarClick, width: '22px', text: '标注'}//text: '<div class="map_toolbar_btn"  title="标点" style="background-image: url(img/toolbar/draw_point_off.png);"></div>' },
            //{ id: 7, height: '16px', handler: OnToolbarClick, width: '22px', text: '<div class="map_toolbar_btn"  title="拉框放大,快捷键：摁住Shift，再拖动鼠标" style="background-image: url(img/toolbar/zoomIn.png);"></div>' },
            //{ id: 8, height: '16px', handler: OnToolbarClick, width: '22px', text: '<div class="map_toolbar_btn"  title="拉框缩小,快捷键：摁住Alt，再拖动鼠标" style="background-image: url(img/toolbar/zoomOut.png);"></div>' }
                , '-'
            ]
        })
    });

    var div = null;
    function OnToolbarClick(item) {
        switch (item.id) {
            case 1: //平移
                VNMAP.Measure();
                break;
            case 2: //
                var lonlat = VNMAP.getCenter();
                parent.Ext.Msg.alert("当前中心点", lonlat.lon + "," + lonlat.lat);
                break;
            case 3: //测距
                var msg = "单击选择起点,<b>Esc</b>&nbsp;键或单击\"平移\"按钮退出";
                VNMAP.Measure("line", msg);
                break;
            case 4:
                VNMAP.removeRound();
                break;
            case 5: //测面积
                var msg = "单击图层画面,<b>Esc</b>&nbsp;键或单击\"平移\"按钮退出";
                VNMAP.Measure("polygon", msg);
                break;
            case 6:
                VNMAP.POI = true;
                VNMAP.MouseBox("单击地图投下标点");
                break;
            case 7: //拉框放大
                VNMAP.map.BoxZoomIn = true;
                break;
            case 8: //拉框缩小
                VNMAP.map.BoxZoomOut = true;
                break;
            case 9:
                break;
        };
    };

    function loadTreeData(tree) {
        CallWebService("../ShiPinJianKong/ShiPinChaKan.asmx/GetTreeForMap", {}, function (request, options) {
            tree.getRootNode().removeAll(true);
            var data = Ext.util.JSON.decode(request.responseText);
            tree.getRootNode().appendChild(data.d);
            tree.getRootNode().expandChildNodes(true);
            onSuccessed(data);
        }, function (request, options) {
            Ext.MessageBox.show({
                title: '错误',
                msg: '连接数据库失败!<br />请检查本地与服务器网络是否通畅。',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR
            });
        });
    };
    /*
    根据分辨率得到treePanel高度
    */
    function getPanelHeight() {
        //debugger
        var h = 0;
        switch (this.window.screen.height) {
            case 720:
                h = 340;
                break;
            case 768:
                h = 390;
                break;
            case 800:
                h = 420;
                break;
            case 900:
                h = 520;
                break;
            default:
                h = 390;
                break;
        }
        return h;
    };

    tree = new Ext.tree.TreePanel({
        autoScroll: true,
        rootVisible: false, // 是否隐藏根节点
        animate: false,
        border: false,
        autoWidth: true,
        minSize: 200,
        height: getPanelHeight(),
        region: 'center',
        id: 'tree',
        containerScroll: true,
        listeners: {
            //监听单击事件
            "click": function (treepanel) {
                cellclickFn(treepanel);
            }
        },
        root: new Ext.tree.AsyncTreeNode({
            id: "000",
            text: "二连浩特",
            expanded: true,
            leaf: false
        })
    });
    tree.addListener('contextmenu', rightClickFn, this);
    function onSuccessed(result) {
        store = new Ext.data.Store({
            proxy: new Ext.data.HttpProxy({ url: "../DataStore/VNMAP/DiDianXinXi_LieBiao_Store.aspx" }),
            baseParams: { LKMC: '', SFLK: '' },
            reader: new Ext.data.JsonReader({
                root: "data",
                totalProperty: "totalCount"
            }, ['TYPE', 'INFO', 'DDBH', 'DDMC', 'SFBZ', 'XXZB', 'YYZB', 'BZ']
            )
        });
        store.load({ params: {
            start: 0,
            limit: 600
        }
        });

        var DDMC;
        store.addListener('load', StroeLoad, this);
        var IsInit = true;
        function StroeLoad(s, columnIndex) {
            if (IsInit) {
                var count = store.getCount();
                if (count > 0) {
                    markers = new OpenLayers.Layer.Markers("地点信息");
                    for (i = 0; i < count; i++) {
                        var _ddbh = store.getAt(i).get("DDBH");
                        var x = store.getAt(i).get("XXZB");
                        var y = store.getAt(i).get("YYZB");
                        var name = store.getAt(i).get("DDMC");
                        var bz = store.getAt(i).get("BZ");
                        var type = store.getAt(i).get("TYPE");
                        if (x != "0") {
                            VNMAP.addMarker('img/marker2.png', markers, name, x, y, { ddbh: _ddbh, bz: bz });
                            markerDJ.icon.imageDiv.title = name;

                            if (type == 'xj') {
                                markerDJ.icon.url = "img/marker5.png";
                            } else if (type == 'lk') {
                                markerDJ.icon.url = "img/marker3.png";
                            };
                            markerDJ.icon.imageDiv.onclick = function () {
                                if (isClose) {
                                    isClose = false;
                                    return;
                                }
                                this.style.cursor = "";
                                imgDiv = this;
                                if (infoBox) {
                                    infoBox.colorbox.close();
                                }
                                VNMAP.GotoXY(this.x, this.y, VNMAP.map.getZoom());
                                //如果是相机
                                if (imgDiv.innerHTML.indexOf('marker5.png') > 0) {
                                    setTimeout(function () {
                                        VNMAP.map.pan(0, -80);
                                    }, 500);
                                    var ip;
                                    var dlm;
                                    var mm;
                                    var dk;
                                    for (index = 0; index < store.data.items.length; index++) {
                                        if (store.data.items[index].data.TYPE == 'xj' & store.data.items[index].data.DDBH == imgDiv.ddbh) {
                                            ip = imgDiv.ddbh;
                                            //admin$12345$8000
                                            var temp = store.data.items[index].data.INFO.split('$');
                                            if (temp.length == 3) {
                                                var dlm = temp[0];
                                                var mm = temp[1];
                                                var dk = temp[2];
                                            }
                                            break;
                                        }
                                    }
                                    if (infoBox) {
                                        setTimeout(function () {
                                            infoBox = $.colorbox({
                                                modal: false, parentDiv: imgDiv, iframe: true, opacity: 0.0, overlayClose: false, top: -295, left: -143, href: '../VN-MAP-OL/ShiShiJianKong/ShiPinXianShi.htm?IP=' + ip + '&User=' + dlm + '&Pwd=' + mm + '&Port=' + dk + '', title: imgDiv.name, innerHeight: 243, innerWidth: 800, close: "<span onclick=\"javascript:isClose=true\">关闭</span>"
                                            });
                                        }, 500);
                                    } else {
                                        infoBox = $.colorbox({
                                            modal: false, parentDiv: imgDiv, iframe: true, opacity: 0.0, overlayClose: false, top: -295, left: -143, href: '../VN-MAP-OL/ShiShiJianKong/ShiPinXianShi.htm?IP=' + ip + '&User=' + dlm + '&Pwd=' + mm + '&Port=' + dk + '', title: imgDiv.name, innerHeight: 243, innerWidth: 800, close: "<span onclick=\"javascript:isClose=true\">关闭</span>"
                                        });
                                    }
                                }
                                //如果是路口
                                else if (imgDiv.innerHTML.indexOf('marker3.png') > 0) {
                                    //debugger
                                    if (infoBox) {
                                        setTimeout(function () {

                                            infoBox = $.colorbox({
                                                modal: false, parentDiv: imgDiv, iframe: true, opacity: 0.0, overlayClose: false, top: -180, left: -141, href: '../VN-MAP-OL/LuKouXiangJi.aspx?lkbh=' + imgDiv.ddbh + '&lkmc=' + encodeURI(imgDiv.name) + '', innerHeight: 130, innerWidth: 322, close: "<span onclick=\"javascript:isClose=true\">关闭</span>"
                                            });
                                        }, 500);
                                    } else {
                                        infoBox = $.colorbox({
                                            modal: false, parentDiv: imgDiv, iframe: true, opacity: 0.0, overlayClose: false, top: -180, left: -141, href: '../VN-MAP-OL/LuKouXiangJi.aspx?lkbh=' + imgDiv.ddbh + '&lkmc=' + encodeURI(imgDiv.name) + '', innerHeight: 130, innerWidth: 322, close: "<span onclick=\"javascript:isClose=true\">关闭</span>"
                                        });
                                    }
                                }
                            };
                        }
                    };
                    VNMAP.map.addLayer(markers);
                    IsInit = false;
                }
            }
        };
    };
    //设备标注右键菜单事件
    var rightClick = new Ext.menu.Menu({
        id: 'rightClickCont',
        items: [{
            scope: this,
            icon: "../App_Themes/Default/Frame/add.gif",
            id: 'rMenu1',
            text: '标注此地点',
            disabled: false,
            handler: menu1
        }, {
            icon: "../App_Themes/Default/ToolBar/Delete.gif",
            id: 'rMenu2',
            text: '删除地点标注',
            disabled: true,
            handler: menu2
        }]
    });
    //右键菜单事件处理函数,设备标注
    function menu1(e) {
        VNMAP.DDPOI.FLAG = true;
        VNMAP.DDPOI.DATA.DDMC = DDMC;
        VNMAP.DDPOI.DATA.DDBH = DDBH;
        VNMAP.DDPOI.DATA.STORE = store;
        VNMAP.DDPOI.DATA.callback = function () { loadTreeData(tree) };
        VNMAP.MouseBox("单击地图投下标注");
    };
    //右键菜单事件处理函数,删除设备标注
    function menu2() {
        if (confirm("确定删除" + DDMC + "的标注信息?")) {

            if (DDBH.length <= 4) {
                var jsonObj = { XXZB: "0", YYZB: "0", DDBH: DDBH };
                VNMAP.SaveMarker(jsonObj, function () { store.reload(); loadTreeData(tree); }, onFailt);
            }
            else {
                var jsonObj = { XXZB: "0", YYZB: "0", DDBH: DDBH };
                VNMAP.SaveMarkers(jsonObj, function () { store.reload(); loadTreeData(tree); }, onFailt);
            }
            //VNMAP.CustomMarkers.removeMarker(m[i]);

        }
    };
    var onFailt = function (error) {
        alert(error.statusText);
    };
    function rightClickFn(node, event) {

        event.preventDefault();
        var xxzb;
        var yyzb;
        if (node.id.indexOf('|') > 0) {
            var list = node.id.split('|');
            DDMC = list[1]; // gridCtxRecord.get("DDMC");
            DDBH = list[0]; //gridCtxRecord.get("DDBH");
            xxzb = list[2];
            yyzb = list[3];
        }
        else {
            //东向西$192.168.1.110$admin$12345$8000$0.000000$0.000000
            var list = node.id.split('$');
            DDMC = list[0]; // gridCtxRecord.get("DDMC");
            DDBH = list[1]; //gridCtxRecord.get("DDBH");
            xxzb = list[5];
            yyzb = list[6];
        }

        if (xxzb != '0') {
            rightClick.items.items[0].setText("√地点已标注");
            rightClick.items.items[0].setDisabled(true);
            rightClick.items.items[1].setDisabled(false);
        } else {
            rightClick.items.items[0].setText("标注此地点");
            rightClick.items.items[0].setDisabled(false);
            rightClick.items.items[1].setDisabled(true);
        }
        rightClick.showAt(event.getXY());
        //alert();
    };
    function cellclickFn(treepanel) {
        var list; // treepanel.id;
        var xxzb;
        var yyzb;
        if (treepanel.id.indexOf('|') > 0) {
            list = treepanel.id.split('|');
            var x = list[2];
            var y = list[3];
        }
        else {
            list = treepanel.id.split('$');
            var x = list[5];
            var y = list[6];
        }
        if (x != '0') {
            VNMAP.GotoXY(x, y, 15);
        }
    };

    function activateHandler3(tab) {
        if (e_grid == null) {
            //  debugger
            e_store = new Ext.data.Store({
                proxy: new Ext.data.HttpProxy({ url: "../DataStore/VNMAP/getcarspos/ZiDingYi.aspx" }),
                reader: new Ext.data.JsonReader({
                    root: "data",
                    totalProperty: "totalCount"
                }, ['CODE', 'NAME', 'LONLAT', 'ZOOM', 'DESCRIPTION', 'KEYWORD', 'ICON']
                    )
            });

            e_bbar = new Ext.PagingToolbar({
                pageSize: 15,
                store: e_store,
                displayInfo: true,
                displayMsg: '{0}-{1}',
                emptyMsg: "没有数据"
            });
            e_grid = new Ext.grid.GridPanel({
                store: e_store,
                loadMask: new Ext.LoadMask(Ext.get("point"), { msg: '正在加载数据...', store: e_store }),
                columns: [
                { header: 'code', align: "center", width: 0, sortable: false, hidden: true, dataIndex: 'CODE' },
                { header: '地点名称', align: "center", width: 225, sortable: false, dataIndex: 'NAME' },
                { header: '坐标', align: "center", width: 0, sortable: false, hidden: true, dataIndex: 'LONLAT' },
                { header: 'zoom', align: "center", width: 0, sortable: false, hidden: true, dataIndex: 'ZOOM' },
                { header: 'description', align: "center", width: 0, sortable: false, hidden: true, dataIndex: 'DESCRIPTION' },
                { header: 'keyword', align: "center", width: 0, sortable: false, hidden: true, dataIndex: 'KEYWORD' },
                { header: 'icon', align: "center", width: 0, sortable: false, hidden: true, dataIndex: 'ICON' }
            ],
                bbar: e_bbar,
                width: 230,
                height: 380
            });
            e_store.load();
            function rightClickFn(grid, rowIndex, e) {
                var gridCtxRecord = e_store.getAt(rowIndex);
                this.rowIndex = rowIndex;
                code = gridCtxRecord.get("CODE");
                name = gridCtxRecord.get("NAME");
                lonlat = gridCtxRecord.get("LONLAT");
                b = gridCtxRecord.get("DESCRIPTION");
                x = lonlat.split(",")[0];
                y = lonlat.split(",")[1];
                e.preventDefault();
                contentMenu.showAt(e.getXY());


            };
            function rowclickFns(grid, rowindex, e) {

                var recordtoedit = grid.getSelectionModel().getSelected();
                var lonlat = recordtoedit.get("LONLAT");
                var name = recordtoedit.get("NAME");
                var b = recordtoedit.get("DESCRIPTION");
                x = lonlat.split(",")[0];
                y = lonlat.split(",")[1];
                VNMAP.GotoXY(x, y, VNMAP.map.getZoom());
//                if (markers != null) {
//                    VNMAP.removePoint(markers);
//                }

                markers = VNMAP.addPoint(name, 0, 0, x, y, b);
            };
            e_grid.addListener('rowcontextmenu', rightClickFn, this);
            e_grid.addListener('rowdblclick', rowclickFns);
            e_grid.render('e_grid');
        }
    };
    //右键菜单
    var contentMenu = new Ext.menu.Menu({
        id: 'rightClickCont',
        items: [{
            scope: this,
            icon: "../App_Themes/Default/Frame/delete.gif",
            id: 'rMenu1',
            text: '删除此标点',
            disabled: false,
            handler: function () {
                parent.Ext.MessageBox.show({
                    title: '撤销?',
                    msg: '确定要删除此标注?',
                    buttons: Ext.MessageBox.YESNO,
                    fn: function (e) {
                        if (e == "yes") {
                            CallWebService("XiangJiWeb.asmx/deledata", { sjid: code }, function (request, opts) { e_store.load(); }, function (reqeust, opts) {
                                alert(reqeust.responseText);
                            });
                        }
                    },
                    animEl: 'cancelAudit',
                    icon: Ext.MessageBox.QUESTION
                });
            }
        }, {
            scope: this,
            icon: "../App_Themes/Default/Frame/add.gif",
            id: 'rMenu2',
            text: '定位到此标点',
            disabled: false,
            handler: function () {
                VNMAP.GotoXY(x, y, VNMAP.map.getZoom());
                if (markers != null) {
                    VNMAP.removePoint(markers);
                }
                markers = VNMAP.addPoint(name, 0, 0, x, y, b);
            }
        }]
    });
    function activateHandler2(tab) {
        if (keyword == null) {
            keyword = ITMS.TextBox.CreateTextBox(false, 150, 25, '请输入关键字', 'keyword');
            btn_point = new Ext.Button({
                renderTo: 'pointd',
                text: "　搜&nbsp;&nbsp;索",
                height: 21,
                style: 'margin-right:80px',
                width: 10,
                disabled: false
            });

            btn_point.on("click", function () {
                if (trim(keyword.getValue()) == "") {

                    alert("请输入地点名称!");
                    return;
                }
                var name = keyword.getValue();
                d_store.load({ params: { name: name, start: 0, limit: 15} });
            });
        }
        if (d_grid == null) {
            d_store = new Ext.data.Store({
                proxy: new Ext.data.HttpProxy({ url: "../DataStore/VNMAP/getcarspos/DiMing.aspx" }),
                baseParams: { name: keyword.getValue() },
                reader: new Ext.data.JsonReader({
                    root: "data",
                    totalProperty: "totalCount"
                }, ['CODE', 'NAME', 'LONLAT', 'ZOOM', 'DESCRIPTION', 'KEYWORD', 'ICON']
                    )
            });

            d_bbar = new Ext.PagingToolbar({
                pageSize: 15,
                store: d_store,
                displayInfo: true,
                displayMsg: '{0}-{1}',
                emptyMsg: "没有数据"
            });
            d_grid = new Ext.grid.GridPanel({
                store: d_store,
                loadMask: new Ext.LoadMask(Ext.get("search"), { msg: '正在加载数据...', store: d_store }),
                columns: [
                { header: 'code', align: "center", width: 155, sortable: false, hidden: true, dataIndex: 'CODE' },
                { header: '地点名称', align: "center", width: 225, sortable: false, dataIndex: 'NAME' },
                { header: '坐标', align: "center", width: 100, sortable: false, hidden: true, dataIndex: 'LONLAT' },
                { header: 'zoom', align: "center", width: 155, sortable: false, hidden: true, dataIndex: 'ZOOM' },
                { header: 'description', align: "center", width: 65, sortable: false, hidden: true, dataIndex: 'DESCRIPTION' },
                { header: 'keyword', align: "center", width: 65, sortable: false, hidden: true, dataIndex: 'KEYWORD' },
                { header: 'icon', align: "center", width: 65, sortable: false, hidden: true, dataIndex: 'ICON' }
            ],
                bbar: d_bbar,
                width: 230,
                height: 380
            });
            function d_bbar_beforechange(pagingToolbar, event) {
                var index = event.start;
                d_store.load({ params: { name: keyword.getValue(), start: index, limit: event.limit} });
                return false;
            };
            function rightClickFn(grid, rowIndex, e) {
                var gridCtxRecord = d_store.getAt(rowIndex);
                this.rowIndex = rowIndex;
                code = gridCtxRecord.get("CODE");
                lonlat = gridCtxRecord.get("LONLAT");
                x = lonlat.split(",")[0];
                y = lonlat.split(",")[1];
                e.preventDefault();
                contentMenu1.showAt(e.getXY());
            };
            function rowclickFn(grid, rowindex, e) {
                var recordtoedit = grid.getSelectionModel().getSelected();
                var lonlat = recordtoedit.get("LONLAT");
                var name = recordtoedit.get("NAME");
                var b = recordtoedit.get("DESCRIPTION");
                var description = recordtoedit.get("DESCRIPTION");
                x = lonlat.split(",")[0];
                y = lonlat.split(",")[1];
                VNMAP.GotoXY(x, y, VNMAP.map.getZoom());
                //                if (markers != null) {
                //                    VNMAP.removePoint(markers);
                //                }



                markers = VNMAP.addPoint(name, 0, 0, x, y, b);
            };
            d_bbar.addListener("beforechange", d_bbar_beforechange);
            d_grid.addListener('rowclick', rowclickFn);
            d_grid.addListener('rowcontextmenu', rightClickFn, this);

            //右键菜单
            var contentMenu1 = new Ext.menu.Menu({
                id: 'rightClickCont',
                items: [{
                    scope: this,
                    icon: "../App_Themes/Default/Frame/delete.gif",
                    id: 'rMenu1',
                    text: '删除此标点',
                    disabled: false,
                    handler: function () {
                        parent.Ext.MessageBox.show({
                            title: '撤销?',
                            msg: '确定要删除此标注?',
                            buttons: Ext.MessageBox.YESNO,
                            fn: function (e) {
                                if (e == "yes") {
                                    CallWebService("XiangJiWeb.asmx/deledata", { sjid: code }, function (request, opts) { d_store.load(); }, function (reqeust, opts) {
                                        alert(reqeust.responseText);
                                    });
                                }
                            },
                            animEl: 'cancelAudit',
                            icon: Ext.MessageBox.QUESTION
                        });
                    }
                }, {
                    scope: this,
                    icon: "../App_Themes/Default/Frame/add.gif",
                    id: 'rMenu2',
                    text: '定位到此标点',
                    disabled: false,
                    handler: function () {
                        VNMAP.GotoXY(x, y, VNMAP.map.getZoom());
                        if (markers != null) {
                            VNMAP.removePoint(markers);
                        }
                        markers = VNMAP.addPoint(name, 0, 0, x, y);
                    }
                }]
            });
            d_grid.render('d_grid');
        }
    };
    function activateHandler(tab) {

    };
    function activateHandler1(tab) {
        if (txt_cphm == null) {
            txt_cphm = ITMS.TextBox.CreateTextBox(false, 150, 21, '请输入车牌号', 'cphm');
            btn = new Ext.Button({
                renderTo: 'btn',
                //iconCls: 's',
                text: "查　询",
                height: 21
            });
            btn_pos = new Ext.Button({
                renderTo: 'btn_pos',
                //iconCls: 's',
                text: "轨迹回放",
                height: 21,
                disabled: true
            });
            btn_clear = new Ext.Button({
                renderTo: 'btn_clear',
                text: '清除图层',
                height: 21,
                handler: function () {
                    VNMAP.removeGPS();
                    VNMAP.removeRound();
                    VNMAP.removePoint(VNMAP.CustomMarkers);
                    //VNMAP.CustomMarkers.removeMarker(TEMP);

                    btn_clear.setDisabled(true);
                    VNMAP.map.addLayer(markers);
                },
                disabled: true
            });
            btn.on("click", function () {

                if (trim(txt_cphm.getValue()) == "") {
                    alert("请输入号牌号码!");
                    return;
                }

                var cp = encodeURI(txt_cphm.getValue());
                var ks = Ext.getDom("kssj").value;
                var js = Ext.getDom("jssj").value;
                var zl = c_clzl.getValue();
                c_store.reload({
                    params: {
                        CPHM: cp,
                        KSSJ: ks,
                        JSSJ: js,
                        CLZL: zl,
                        start: 0,
                        limit: 15
                    }
                });
                btn_pos.setDisabled(true);
                btn.setText("查询中...");
                btn.setIconClass("ing");
                btn.setDisabled(true);
                setTimeout(function () {
                    if (c_store.getCount() > 0) {
                        btn_pos.setDisabled(false);
                    }
                    btn.setText("查　询");
                    btn.setDisabled(false);
                    btn.setIconClass("");
                }, 2500);
            });
            btn_pos.on("click", function () {
                if (markers != null) {
                    VNMAP.removePoint(markers);
                }
                var pos_data = [];
                var count = c_store.getCount();
                for (i = 0; i < count; i++) {
                    var n = c_store.getAt(i).get("DDMC");
                    var x = c_store.getAt(i).get("XXZB");
                    var y = c_store.getAt(i).get("YYZB");
                    var coun = i + 1;
                    if (x != "0") {

                        pos_data.push(n + "," + x + "," + y);
                        VNMAP.addSpanPoint123(x, y, coun);
                    }

                };


                VNMAP.removeRound();
                VNMAP.GPS(pos_data);
                btn_clear.setDisabled(false);
            });
        };
        if (c_kssj == null) {
            c_kssj = ITMS.DatetimePicker.CreateDateTimeField("", 150, 24, "kssj")
            c_jssj = ITMS.DatetimePicker.CreateDateTimeField("", 150, 24, "jssj");
        };
        if (c_clzl == null) {
            var data = [["01", "小型汽车"], ["02", "大型汽车"], ["03", "其他"]]
            var clzl_store = new Ext.data.ArrayStore({
                fields: ['Value', 'Text'],
                data: data
            });
            c_clzl = ITMS.Combox.BindCombox(clzl_store, 'Text', 'Value', 150, 24, 'clzl');
        };
        if (c_grid == null) {
            c_store = new Ext.data.Store({
                proxy: new Ext.data.HttpProxy({ url: "../DataStore/VNMAP/getcarspos/Default.aspx" }),
                baseParams: { CPHM: txt_cphm.getValue(), KSSJ: '', JSSJ: '', CLZL: '' },
                reader: new Ext.data.JsonReader({
                    root: "data",
                    totalProperty: "totalCount"
                }, ['SJID', 'CPHM', 'DDMC', 'JGSJ', 'XXZB', 'YYZB', 'FJMC', 'ROWNUM']
                    )
            });
            c_bbar = new Ext.PagingToolbar({
                pageSize: 100,
                store: c_store,
                displayInfo: true,
                displayMsg: '{2}条',
                emptyMsg: "0条"
            });
            c_grid = new Ext.grid.GridPanel({
                store: c_store,
                loadMask: new Ext.LoadMask(Ext.get("gps"), { msg: '正在加载数据...', store: c_store }),
                columns: [
                { id: 'ID', header: 'ID', align: "left", width: 10, hidden: true, sortable: false, dataIndex: 'SJID' },
                { header: '号牌号码', align: "center", width: 85, sortable: false, dataIndex: 'CPHM' },
                { header: '经过地点', align: "center", width: 155, sortable: false, dataIndex: 'DDMC' },
                { header: '经过时间', align: "center", width: 155, sortable: false, dataIndex: 'JGSJ' },
                { header: 'X坐标', align: "center", width: 65, sortable: false, hidden: true, dataIndex: 'XXZB' },
                { header: 'Y坐标', align: "center", width: 65, sortable: false, hidden: true, dataIndex: 'YYZB' },
                { header: 'FJMC', align: "center", width: 65, sortable: false, hidden: true, dataIndex: 'FJMC' },
                 { header: 'ROWNUM', align: "center", width: 65, sortable: false, hidden: true, dataIndex: 'ROWNUM' }
            ],
                bbar: c_bbar,
                width: 230,
                height: 565
            });

            function c_bbar_beforechange(pagingToolbar, event) {
                var index = event.start;
                c_store.load({ params: { CPHM: txt_cphm.getValue(), start: index, limit: event.limit} });
                return false;
            };
            function c_cellclickFn(grid, rowIndex, columnIndex, e) {//轨迹点击路口事件
                var record = c_grid.getSelectionModel().getSelected();
                var x = record.get("XXZB");
                var y = record.get("YYZB");
                var count = record.get("ROWNUM");
                VNMAP.addSpanPoint123(x, y, count);
                GPSPopup.BindEvents();
            };
            c_bbar.addListener("beforechange", c_bbar_beforechange);
            c_grid.addListener('rowclick', c_cellclickFn, this);
        };
        c_grid.render('c_grid');
    };
    var view = new Ext.Viewport({
        layout: "border",
        items: [
            left,
            {
                border: false,
                xtype: "panel",
                title: ' ',
                region: "east",
                width: 230,
                id: 'mapEast',
                collapsible: true,
                margins: '0 0 0 2',
                items: [{
                    xtype: 'tabpanel',
                    activeTab: 0,
                    enableTabScroll: true,
                    items: [
                            { title: "地点信息", listeners: { activate: activateHandler }, border: false, xtype: 'panel', height: getPanelHeight(), autoWidth: true, layout: "fit", items: [tree] },
                            { title: "车辆追踪", listeners: { activate: activateHandler1 }, html: '<div id="gps" style="line-height:22px;height:570px;width:220px"><table style="font-size:12px"><tr><td width="50%">开始日期：</td><td colspan="2"><input type="text" id="kssj" /></td></tr><tr><td>结束日期：</td><td colspan="2"><input type="text" id="jssj" /></td></tr><tr><td>车辆种类：</td><td colspan="2"><span id="clzl" /></td></tr><tr><td>号牌号码：</td><td colspan="2"><span id="cphm" /></td></tr><tr><td><span id="btn_pos" /></td><td ><span id="btn" /></td><td><span id="btn_clear" /></td></tr></table><table><tr><tr><td colspan="2"><span id="result"></span></td></tr><tr><td colspan="2"><div id="c_grid"></div></td></tr></table></div>' },
                            { title: "搜索地名", listeners: { activate: activateHandler2 }, html: '<div id="search" style="height:' + getPanelHeight() + 'px"><table style="font-size:12px;"><tr><td height="30px" width="100px" ><span type="text" id="keyword" /></td><td><span id="pointd" /></td></tr><tr><td colspan="2"><div id="d_grid"></div></td></tr></table></div>' },
                            { title: "自定义标注", listeners: { activate: activateHandler3 }, html: '<div id="point" style="height:' + getPanelHeight() + 'px"><table style="font-size:12px"><tr><td><div id="e_grid"></div></td></tr></table></div>' }
                    ]
                }]
            }]
    });
    loadTreeData(tree);
    VNMAP.MapInit();
    document.oncontextmenu = function () { return true; };
});
var GPSPopup = {
    show: function (parentDiv, h, boxH, bbar, data) {
        p = $.colorbox({
            modal: false,
            parentDiv: parentDiv,
            opacity: 0.0,
            overlayClose: false,
            top: -h,
            left: -132,
            html: "<div style='margin-top:10px;margin-left:10px'>号牌号码：" + data.cphm + "<br />" + data.label + "经过地点：" + data.ddmc + "<br />" + data.label + "经过时间：" + data.jgsj + "<br /><a href='javascript:void(0);' onclick=\"window.showModalDialog('tupian.aspx?url=" + data.fjmc + "','Derek','dialogWidth:410px;dialogHeight:310px;help:no;center:yes;resizable:no;status:no;scrolling:no');\">查看图片</a><br />" + bbar + "</div>", innerHeight: boxH, innerWidth: 300, close: '<span onclick="VNMAP.removeRound();">关闭</span>'
        });
        $(document).bind('keydown', function (e) {
            var key = e.keyCode;
            if (key === 27) {
                VNMAP.removeRound();
            }
        });
        return p;
    },
    resize: function (size) {
        box.colorbox.resize({ innerHeight: size.height });
    },
    PreviousP: function () {
        c_grid.getSelectionModel().selectPrevious();
        this.BindEvents();
    },
    NextP: function () {
        c_grid.getSelectionModel().selectNext();
        this.BindEvents();
    },
    BindEvents: function () {
        if (box) {
            box.colorbox.close();
        }
        var last, first = false;
        var label = "";
        var record = c_grid.getSelectionModel().getSelected();

        if (record.get("XXZB") != 0) {
            var x = record.get("XXZB");
            var y = record.get("YYZB");
            var cphm = record.get("CPHM");
            var name = record.get("DDMC");
            var jgsj = record.get("JGSJ");
            var fjmc = record.get("FJMC");
            var count = record.get("ROWNUM");
            VNMAP.GotoXY(x, y, 15);
            TEMP= VNMAP.addSpanPoint123(x, y, count);//点击后判断图标
            //TEMP = VNMAP.addSpanPoint(x, y);
            var bbar = "";
            PreHas = c_grid.getSelectionModel().hasPrevious();
            NextHas = c_grid.getSelectionModel().hasNext();
            var h = (!PreHas && !NextHas) ? 150 : (PreHas && NextHas) ? 195 : 180;
            var boxH = (!PreHas && !NextHas) ? 102 : (PreHas && NextHas) ? 150 : 132;
            if (PreHas) {
                var previous_ddmc = c_grid.store.data.items[c_grid.getSelectionModel().lastActive - 1].data.DDMC;
                bbar += "<a href='javascript:void(0);' onclick=\"GPSPopup.PreviousP();VNMAP.removeRound();\">上一个经过点：" + previous_ddmc + "</a><br /><br />";
            } else {
                first = true;
                label = "最早";
            }
            if (NextHas) {
                var next_ddmc = c_grid.store.data.items[c_grid.getSelectionModel().lastActive + 1].data.DDMC;
                bbar += "<a href='javascript:void(0);' onclick=\"GPSPopup.NextP()\">下一个经过点：" + next_ddmc + "</a>";
            } else {
                last = true;
                label = "最后";
                h = 226;
                boxH = 160;
                bbar += "<a href='javascript:void(0);' onclick=\"VNMAP.roundArea(" + x + ", " + y + ");GPSPopup.resize({height:180});javascript:Common.$('bounds').style.display='block';\">显示侦查范围</a>";
                bbar += "<br /><br /><div id='bounds' style='display:none'>周围：<a href='javascript:void(0);' onclick=\"VNMAP.setBounds(" + x + "," + y + ",500)\">500m</a>&nbsp;&nbsp;<a href='javascript:void(0);' onclick=\"VNMAP.setBounds(" + x + "," + y + ",1000)\">1000m</a>&nbsp;&nbsp;<a href='javascript:void(0);' onclick=\"VNMAP.setBounds(" + x + "," + y + ",2000)\">2000m</a>&nbsp;&nbsp;<a href='javascript:void(0);' onclick=\"VNMAP.setBounds(" + x + "," + y + ",3000)\">3000m</a>&nbsp;&nbsp;<a href='javascript:void(0);' onclick=\"VNMAP.setBounds(" + x + "," + y + ",5000)\">5000m</a></div>";
            }
            setTimeout(function () {
                box = GPSPopup.show(TEMP.icon.imageDiv, h, boxH, bbar, { cphm: cphm, ddmc: name, jgsj: jgsj, label: label, fjmc: fjmc });
            }, 600);
        }
    }
};