ImageZoom._MODE = {
    //跟随
    "follow": {
        methods: {
            init: function () {
                this._stylesFollow = null; //备份样式
                this._repairFollowLeft = 0; //修正坐标left
                this._repairFollowTop = 0; //修正坐标top
            },
            load: function () {
                var viewer = this._viewer, style = viewer.style, styles;
                this._stylesFollow = {
                    left: style.left, top: style.top, position: style.position
                };
                viewer.style.position = "absolute";
                //获取修正参数
                if (!viewer.offsetWidth) {//隐藏
                    styles = { display: style.display, visibility: style.visibility };
                    $$D.setStyle(viewer, { display: "block", visibility: "hidden" });
                }
                //修正中心位置
                this._repairFollowLeft = viewer.offsetWidth / 2;
                this._repairFollowTop = viewer.offsetHeight / 2;
                //修正offsetParent位置
                if (!/BODY|HTML/.test(viewer.offsetParent.nodeName)) {
                    var parent = viewer.offsetParent, rect = $$D.rect(parent);
                    this._repairFollowLeft += rect.left + parent.clientLeft;
                    this._repairFollowTop += rect.top + parent.clientTop;
                }
                if (styles) { $$D.setStyle(viewer, styles); }
            },
            repair: function (e, pos) {
                var zoom = this._zoom,
					viewerWidth = this._viewerWidth,
					viewerHeight = this._viewerHeight;
                pos.left = (viewerWidth / 2 - pos.left) * (viewerWidth / zoom.width - 1);
                pos.top = (viewerHeight / 2 - pos.top) * (viewerHeight / zoom.height - 1);
            },
            move: function (e) {
                var style = this._viewer.style;
                style.left = e.pageX - this._repairFollowLeft + "px";
                style.top = e.pageY - this._repairFollowTop + "px";
            },
            dispose: function () {
                $$D.setStyle(this._viewer, this._stylesFollow);
            }
        }
    },
    //拖柄
    "handle": {
        options: {//默认值
            handle: ""//拖柄对象
        },
        methods: {
            init: function () {
                var handle = $$(this.options.handle);
                if (!handle) {//没有定义的话用复制显示框代替
                    var body = document.body;
                    handle = body.insertBefore(this._viewer.cloneNode(false), body.childNodes[0]);
                    handle.id = "";
                    handle["_createbyhandle"] = true; //生成标识用于移除
                }
                $$D.setStyle(handle, { padding: 0, margin: 0, display: "none" });

                this._handle = handle;
                this._repairHandleLeft = 0; //修正坐标left
                this._repairHandleTop = 0; //修正坐标top
            },
            load: function () {
                var handle = this._handle, rect = this._rect;
                $$D.setStyle(handle, {
                    position: "absolute",
                    width: this._rangeWidth + "px",
                    height: this._rangeHeight + "px",
                    display: "block",
                    visibility: "hidden"
                });
                //获取修正参数
                this._repairHandleLeft = rect.left + this._repairLeft - handle.clientLeft;
                this._repairHandleTop = rect.top + this._repairTop - handle.clientTop;
                //修正offsetParent位置
                if (!/BODY|HTML/.test(handle.offsetParent.nodeName)) {
                    var parent = handle.offsetParent, rect = $$D.rect(parent);
                    this._repairHandleLeft -= rect.left + parent.clientLeft;
                    this._repairHandleTop -= rect.top + parent.clientTop;
                }
                //隐藏
                $$D.setStyle(handle, { display: "none", visibility: "visible" });
            },
            start: function () {
                this._handle.style.display = "block";
            },
            move: function (e, x, y) {
                var style = this._handle.style, scale = this._scale;
                style.left = Math.ceil(this._repairHandleLeft - x / scale) + "px";
                style.top = Math.ceil(this._repairHandleTop - y / scale) + "px";
            },
            end: function () {
                this._handle.style.display = "none";
            },
            dispose: function () {
                if ("_createbyhandle" in this._handle) { document.body.removeChild(this._handle); }
                this._handle = null;
            }
        }
    },
    //切割
    "cropper": {
        options: {//默认值
            opacity: .5//透明度
        },
        methods: {
            init: function () {
                var body = document.body,
					cropper = body.insertBefore(document.createElement("img"), body.childNodes[0]);
                cropper.style.display = "none";

                this._cropper = cropper;
                this.opacity = this.options.opacity;
            },
            load: function () {
                var cropper = this._cropper, image = this._image, rect = this._rect;
                cropper.src = image.src;
                cropper.width = image.width;
                cropper.height = image.height;
                $$D.setStyle(cropper, {
                    position: "absolute",
                    left: rect.left + this._repairLeft + "px",
                    top: rect.top + this._repairTop + "px"
                });
            },
            start: function () {
                this._cropper.style.display = "block";
                $$D.setStyle(this._image, "opacity", this.opacity);
            },
            move: function (e, x, y) {
                var w = this._rangeWidth, h = this._rangeHeight, scale = this._scale;
                x = Math.ceil(-x / scale); y = Math.ceil(-y / scale);
                this._cropper.style.clip = "rect(" + y + "px " + (x + w) + "px " + (y + h) + "px " + x + "px)";
            },
            end: function () {
                $$D.setStyle(this._image, "opacity", 1);
                this._cropper.style.display = "none";
            },
            dispose: function () {
                document.body.removeChild(this._cropper);
                this._cropper = null;
            }
        }
    }
}

ImageZoom.prototype._initialize = (function () {
    var init = ImageZoom.prototype._initialize,
		mode = ImageZoom._MODE,
		modes = {
		    "follow": [mode.follow],
		    "handle": [mode.handle],
		    "cropper": [mode.cropper],
		    "handle-cropper": [mode.handle, mode.cropper]
		};
    return function () {
        var options = arguments[2];
        if (options && options.mode && modes[options.mode]) {
            $$A.forEach(modes[options.mode], function (mode) {
                //扩展options
                $$.extend(options, mode.options, false);
                //扩展钩子
                $$A.forEach(mode.methods, function (method, name) {
                    $$CE.addEvent(this, name, method);
                }, this);
            }, this);
        }
        init.apply(this, arguments);
    }
})();