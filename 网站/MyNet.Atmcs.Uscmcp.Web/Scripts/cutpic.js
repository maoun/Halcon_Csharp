var $ = function (id) {
    return "string" == typeof id ? document.getElementById(id) : id;
};

var isIE = (document.all) ? true : false;

function addEventHandler(oTarget, sEventType, fnHandler) {
	if (oTarget.addEventListener) {
		oTarget.addEventListener(sEventType, fnHandler, false);
	} else if (oTarget.attachEvent) {
		oTarget.attachEvent("on" + sEventType, fnHandler);
	} else {
		oTarget["on" + sEventType] = fnHandler;
	}
};

function removeEventHandler(oTarget, sEventType, fnHandler) {
    if (oTarget.removeEventListener) {
        oTarget.removeEventListener(sEventType, fnHandler, false);
    } else if (oTarget.detachEvent) {
        oTarget.detachEvent("on" + sEventType, fnHandler);
    } else { 
        oTarget["on" + sEventType] = null;
    }
};


var Class = {
  create: function() {
    return function() {
      this.initialize.apply(this, arguments);
    }
  }
}

Object.extend = function(destination, source) {
    for (var property in source) {
        destination[property] = source[property];
    }
    return destination;
}

//拖放程序
var Drag = Class.create();
Drag.prototype = {
  //拖放对象,触发对象
  initialize: function(obj, drag, options) {
    var oThis = this;
	
	this._obj = $(obj);//拖放对象
	this.Drag = $(drag);//触发对象
	this._x = this._y = 0;//记录鼠标相对拖放对象的位置
	//事件对象(用于移除事件)
	this._fM = function(e){ oThis.Move(window.event || e); }
	this._fS = function(){ oThis.Stop(); }
	
	this.SetOptions(options);
	
	this.Limit = !!this.options.Limit;
	this.mxLeft = parseInt(this.options.mxLeft);
	this.mxRight = parseInt(this.options.mxRight);
	this.mxTop = parseInt(this.options.mxTop);
	this.mxBottom = parseInt(this.options.mxBottom);
	
	this.onMove = this.options.onMove;
	
	this._obj.style.position = "absolute";
	addEventHandler(this.Drag, "mousedown", function(e){ oThis.Start(window.event || e); });
  },
  //设置默认属性
  SetOptions: function(options) {
    this.options = {//默认值
		Limit:		false,//是否设置限制(为true时下面参数有用,可以是负数)
		mxLeft:		0,//左边限制
		mxRight:	0,//右边限制
		mxTop:		0,//上边限制
		mxBottom:	0,//下边限制
		onMove:		function(){}//移动时执行
    };
    Object.extend(this.options, options || {});
  },
  //准备拖动
  Start: function(oEvent) {
	//记录鼠标相对拖放对象的位置
	this._x = oEvent.clientX - this._obj.offsetLeft;
	this._y = oEvent.clientY - this._obj.offsetTop;
	//mousemove时移动 mouseup时停止
	addEventHandler(document, "mousemove", this._fM);
	addEventHandler(document, "mouseup", this._fS);
	//使鼠标移到窗口外也能释放
	if(isIE){
		addEventHandler(this.Drag, "losecapture", this._fS);
		this.Drag.setCapture();
	}else{
		addEventHandler(window, "blur", this._fS);
	}
  },
  //拖动
  Move: function(oEvent) {
	//清除选择(ie设置捕获后默认带这个)
	window.getSelection && window.getSelection().removeAllRanges();
	//当前鼠标位置减去相对拖放对象的位置得到offset位置
	var iLeft = oEvent.clientX - this._x, iTop = oEvent.clientY - this._y;	
	//设置范围限制
	if(this.Limit){
		//获取超出长度
		var iRight = iLeft + this._obj.offsetWidth - this.mxRight, iBottom = iTop + this._obj.offsetHeight - this.mxBottom;
		//这里是先设置右边下边再设置左边上边,可能会不准确
		if(iRight > 0) iLeft -= iRight;
		if(iBottom > 0) iTop -= iBottom;
		if(this.mxLeft > iLeft) iLeft = this.mxLeft;
		if(this.mxTop > iTop) iTop = this.mxTop;
	}
	//设置位置
	this._obj.style.left = iLeft + "px";
	this._obj.style.top = iTop + "px";	
	//附加程序
	this.onMove();
  },
  //停止拖动
  Stop: function() {
	//移除事件
	removeEventHandler(document, "mousemove", this._fM);
	removeEventHandler(document, "mouseup", this._fS);
	if(isIE){
		removeEventHandler(this.Drag, "losecapture", this._fS);
		this.Drag.releaseCapture();
	}else{
		removeEventHandler(window, "blur", this._fS);
	}
  }
};

//缩放程序
var Resize = Class.create();
Resize.prototype = {
  //缩放对象
  initialize: function(obj, options) {
    var oThis = this;
	
	this._obj = $(obj);//缩放对象
	this._right = this._down = this._left = this._up = 0;//坐标参数
	this._scale = 1;//比例参数
	this._touch = null;//当前触发对象
	
	//用currentStyle（ff用getComputedStyle）取得最终样式
	var _style = this._obj.currentStyle || document.defaultView.getComputedStyle(this._obj, null);
	this._xBorder = parseInt(_style.borderLeftWidth) + parseInt(_style.borderRightWidth);
	this._yBorder = parseInt(_style.borderTopWidth) + parseInt(_style.borderBottomWidth);
	
	//事件对象(用于移除事件)
	this._fR = function(e){ oThis.Resize(e); }
	this._fS = function(){ oThis.Stop(); }
	
	this.SetOptions(options);
	
	this.Limit = !!this.options.Limit;
	this.mxLeft = parseInt(this.options.mxLeft);
	this.mxRight = parseInt(this.options.mxRight);
	this.mxTop = parseInt(this.options.mxTop);
	this.mxBottom = parseInt(this.options.mxBottom);
	
	this.MinWidth = parseInt(this.options.MinWidth);
	this.MinHeight = parseInt(this.options.MinHeight);
	this.Scale = !!this.options.Scale;
	this.onResize = this.options.onResize;
	
	this._obj.style.position = "absolute";
  },
  //设置默认属性
  SetOptions: function(options) {
    this.options = {//默认值
		Limit:		false,//是否设置限制(为true时下面mx参数有用)
		mxLeft:		0,//左边限制
		mxRight:	0,//右边限制
		mxTop:		0,//上边限制
		mxBottom:	0,//下边限制
		MinWidth:	50,//最小宽度
		MinHeight:	50,//最小高度
		Scale:		false,//是否按比例缩放
		onResize:	function(){}//缩放时执行
    };
    Object.extend(this.options, options || {});
  },
  //设置触发对象
  Set: function(resize, side) {
	var oThis = this, resize = $(resize), _fun, _cursor;
	if(!resize) return;
	//根据方向设置 _fun是缩放时执行的程序 _cursor是鼠标样式
	switch (side.toLowerCase()) {
	case "up" :
		_fun = function(e){ if(oThis.Scale){ oThis.scaleUpRight(e); }else{ oThis.SetUp(e); } };
		_cursor = "n-resize";
		break;
	case "down" :
		_fun = function(e){ if(oThis.Scale){ oThis.scaleDownRight(e); }else{ oThis.SetDown(e); } };
		_cursor = "n-resize";
		break;
	case "left" :
		_fun = function(e){ if(oThis.Scale){ oThis.scaleLeftUp(e); }else{ oThis.SetLeft(e); } };
		_cursor = "e-resize";
		break;
	case "right" :
		_fun = function(e){ if(oThis.Scale){ oThis.scaleRightDown(e); }else{ oThis.SetRight(e); } };
		_cursor = "e-resize";
		break;
	case "left-up" :
		_fun = function(e){ if(oThis.Scale){ oThis.scaleLeftUp(e); }else{ oThis.SetLeft(e); oThis.SetUp(e); } };
		_cursor = "nw-resize";
		break;
	case "right-up" :
		_fun = function(e){ if(oThis.Scale){ oThis.scaleRightUp(e); }else{ oThis.SetRight(e); oThis.SetUp(e); } };
		_cursor = "ne-resize";
		break;
	case "left-down" :
		_fun = function(e){ if(oThis.Scale){ oThis.scaleLeftDown(e); }else{ oThis.SetLeft(e); oThis.SetDown(e); } };
		_cursor = "ne-resize";
		break;
	case "right-down" :
	default :
		_fun = function(e){ if(oThis.Scale){ oThis.scaleRightDown(e); }else{ oThis.SetRight(e); oThis.SetDown(e); } };
		_cursor = "nw-resize";
	}
	//设置触发对象
	resize.style.cursor = _cursor;
	addEventHandler(resize, "mousedown", function(e){ oThis._fun = _fun; oThis._touch = resize; oThis.Start(window.event || e); });
  },
  //准备缩放
  Start: function(oEvent, o) {	
	//防止冒泡
	if(isIE){ oEvent.cancelBubble = true; } else { oEvent.stopPropagation(); }
	//计算样式初始值
	this.style_width = this._obj.offsetWidth;
	this.style_height = this._obj.offsetHeight;
	this.style_left = this._obj.offsetLeft;
	this.style_top = this._obj.offsetTop;
	//设置缩放比例
	if(this.Scale){ this._scale = this.style_width / this.style_height; }
	//计算当前边的对应另一条边的坐标 例如右边缩放时需要左边界坐标
	this._left = oEvent.clientX - this.style_width;
	this._right = oEvent.clientX + this.style_width;
	this._up = oEvent.clientY - this.style_height;
	this._down = oEvent.clientY + this.style_height;
	//如果有范围 先计算好范围内最大宽度和高度
	if(this.Limit){
		this._mxRight = this.mxRight - this._obj.offsetLeft;
		this._mxDown = this.mxBottom - this._obj.offsetTop;
		this._mxLeft = this.mxLeft + this.style_width + this._obj.offsetLeft;
		this._mxUp = this.mxTop + this.style_height + this._obj.offsetTop;
	}
	//mousemove时缩放 mouseup时停止
	addEventHandler(document, "mousemove", this._fR);
	addEventHandler(document, "mouseup", this._fS);
	
	//使鼠标移到窗口外也能释放
	if(isIE){
		addEventHandler(this._touch, "losecapture", this._fS);
		this._touch.setCapture();
	}else{
		addEventHandler(window, "blur", this._fS);
	}
  },  
  //缩放
  Resize: function(e) {
	//没有触发对象的话返回
	if(!this._touch) return;
	//清除选择(ie设置捕获后默认带这个)
	window.getSelection && window.getSelection().removeAllRanges();
	//执行缩放程序
	this._fun(window.event || e);
	//设置样式
	//因为计算用的offset是把边框算进去的所以要减去
	this._obj.style.width = this.style_width - this._xBorder + "px";
	this._obj.style.height = this.style_height - this._yBorder + "px";
	this._obj.style.top = this.style_top + "px";
	this._obj.style.left = this.style_left + "px";	
	//附加程序
	this.onResize();
  },
  //普通缩放
  //右边
  SetRight: function(oEvent) {
	//当前坐标位置减去左边的坐标等于当前宽度
	this.repairRight(oEvent.clientX - this._left);
  },
  //下边
  SetDown: function(oEvent) {
	this.repairDown(oEvent.clientY - this._up);
  },
  //左边
  SetLeft: function(oEvent) {
	//右边的坐标减去当前坐标位置等于当前宽度
	this.repairLeft(this._right - oEvent.clientX);
  },
  //上边
  SetUp: function(oEvent) {
	this.repairUp(this._down - oEvent.clientY);
  },
  //比例缩放
  //比例缩放右下
  scaleRightDown: function(oEvent) {
	//先计算宽度然后按比例计算高度最后根据确定的高度计算宽度（宽度优先）
	this.SetRight(oEvent);
	this.repairDown(parseInt(this.style_width / this._scale));
	this.repairRight(parseInt(this.style_height * this._scale));
  },
  //比例缩放左下
  scaleLeftDown: function(oEvent) {
	this.SetLeft(oEvent);
	this.repairDown(parseInt(this.style_width / this._scale));
	this.repairLeft(parseInt(this.style_height * this._scale));
  },
  //比例缩放右上
  scaleRightUp: function(oEvent) {
	this.SetRight(oEvent);
	this.repairUp(parseInt(this.style_width / this._scale));
	this.repairRight(parseInt(this.style_height * this._scale));
  },
  //比例缩放左上
  scaleLeftUp: function(oEvent) {
	this.SetLeft(oEvent);
	this.repairUp(parseInt(this.style_width / this._scale));
	this.repairLeft(parseInt(this.style_height * this._scale));
  },
  //这里是高度优先用于上下两边(体验更好)
  //比例缩放下右
  scaleDownRight: function(oEvent) {
	this.SetDown(oEvent);
	this.repairRight(parseInt(this.style_height * this._scale));
	this.repairDown(parseInt(this.style_width / this._scale));
  },
  //比例缩放上右
  scaleUpRight: function(oEvent) {
	this.SetUp(oEvent);
	this.repairRight(parseInt(this.style_height * this._scale));
	this.repairUp(parseInt(this.style_width / this._scale));
  },
  //修正程序
  //修正右边
  repairRight: function(iWidth) {
	//右边和下边只要设置宽度和高度就行
	//当少于最少宽度
	if (iWidth < this.MinWidth){ iWidth = this.MinWidth; }
	//当超过当前设定的最大宽度
	if(this.Limit && iWidth > this._mxRight){ iWidth = this._mxRight; }
	//修改样式
	this.style_width = iWidth;
  },
  //修正下边
  repairDown: function(iHeight) {
	if (iHeight < this.MinHeight){ iHeight = this.MinHeight; }
	if(this.Limit && iHeight > this._mxDown){ iHeight = this._mxDown; }
	this.style_height = iHeight;
  },
  //修正左边
  repairLeft: function(iWidth) {
	//左边和上边比较麻烦 因为还要计算left和top
	//当少于最少宽度
	if (iWidth < this.MinWidth){ iWidth = this.MinWidth; }
	//当超过当前设定的最大宽度
	else if(this.Limit && iWidth > this._mxLeft){ iWidth = this._mxLeft; }
	//修改样式
	this.style_width = iWidth;
	this.style_left = this._obj.offsetLeft + this._obj.offsetWidth - iWidth;
  },
  //修正上边
  repairUp: function(iHeight) {
	if(iHeight < this.MinHeight){ iHeight = this.MinHeight; }
	else if(this.Limit && iHeight > this._mxUp){ iHeight = this._mxUp; }
	this.style_height = iHeight;
	this.style_top = this._obj.offsetTop + this._obj.offsetHeight - iHeight;
  },
  //停止缩放
  Stop: function() {
	//移除事件
	removeEventHandler(document, "mousemove", this._fR);
	removeEventHandler(document, "mouseup", this._fS);
	if(isIE){
		removeEventHandler(this._touch, "losecapture", this._fS);
		this._touch.releaseCapture();
	}else{
		removeEventHandler(window, "blur", this._fS);
	}
	this._touch = null;
  }
};


//图片切割
var ImgCropper = Class.create();
ImgCropper.prototype = {
  //容器对象,拖放缩放对象,图片地址,宽度,高度
  initialize: function(container, drag, url, width, height, options) {
	var oThis = this;
	
	//容器对象
	this.Container = $(container);
	this.Container.style.position = "relative";
	this.Container.style.overflow = "hidden";
	
	//拖放对象
	this.Drag = $(drag);
	this.Drag.style.cursor = "move";
	this.Drag.style.zIndex = 200;
	if(isIE){
		//设置overflow解决ie6的渲染问题(缩放时填充对象高度的问题)
		this.Drag.style.overflow = "hidden";
		//ie下用一个透明的层填充拖放对象 不填充的话onmousedown会失效(未知原因)
		(function(style){
			style.width = style.height = "100%"; style.backgroundColor = "#fff"; style.filter = "alpha(opacity:0)";
		})(this.Drag.appendChild(document.createElement("div")).style)
	}
	
	this._pic = this.Container.appendChild(document.createElement("img"));//图片对象
	this._cropper = this.Container.appendChild(document.createElement("img"));//切割对象
	this._pic.style.position = this._cropper.style.position = "absolute";
	this._pic.style.top = this._pic.style.left = this._cropper.style.top = this._cropper.style.left = "0";//对齐
	this._cropper.style.zIndex = 100;
	this._cropper.onload = function(){ oThis.SetPos(); }
	
	this.Url = url;//图片地址
	this.Width = parseInt(width);//宽度
	this.Height = parseInt(height);//高度
	
	this.SetOptions(options);
	
	this.Opacity = parseInt(this.options.Opacity);
	this.dragTop = parseInt(this.options.dragTop);
	this.dragLeft = parseInt(this.options.dragLeft);
	this.dragWidth = parseInt(this.options.dragWidth);
	this.dragHeight = parseInt(this.options.dragHeight);
	
	//设置预览对象
	this.View = $(this.options.View) || null;//预览对象
	this.viewWidth = parseInt(this.options.viewWidth);
	this.viewHeight = parseInt(this.options.viewHeight);
	this._view = null;//预览图片对象
	if(this.View){
		this.View.style.position = "relative";
		this.View.style.overflow = "hidden";
		this._view = this.View.appendChild(document.createElement("img"));
		this._view.style.position = "absolute";
	}
	
	this.Scale = parseInt(this.options.Scale);
	
	//设置拖放
	this._drag = new Drag(this.Drag, this.Drag, { Limit: true, onMove: function(){ oThis.SetPos(); } });
	//设置缩放
	this._resize = this.GetResize();
	
	this.Init();
  },
  //设置默认属性
  SetOptions: function(options) {
    this.options = {//默认值
		Opacity:	50,//透明度(0到100)	
		//拖放位置和宽高
		dragTop:	0,
		dragLeft:	0,
		dragWidth:	200,
		dragHeight:	200,
		//缩放触发对象
		Right:		"",
		Left:		"",
		Up:			"",
		Down:		"",
		RightDown:	"",
		LeftDown:	"",
		RightUp:	"",
		LeftUp:		"",
		Scale:		false,//是否按比例缩放
		//预览对象设置
		View:	"",//预览对象
		viewWidth:	200,//预览宽度
		viewHeight:	200//预览高度
    };
    Object.extend(this.options, options || {});
  },
  //初始化对象
  Init: function() {
	var oThis = this;
	
	//设置容器
	this.Container.style.width = this.Width + "px";
	this.Container.style.height = this.Height + "px";
	
	//设置拖放对象
	this.Drag.style.top = this.dragTop + "px";
	this.Drag.style.left = this.dragLeft + "px";
	this.Drag.style.width = this.dragWidth + "px";
	this.Drag.style.height = this.dragHeight + "px";
	
	//设置切割对象
	this._pic.src = this._cropper.src = this.Url;
	this._pic.style.width = this._cropper.style.width = this.Width + "px";
	this._pic.style.height = this._cropper.style.height = this.Height + "px";
	if(isIE){
		this._pic.style.filter = "alpha(opacity:" + this.Opacity + ")";
	} else {
		this._pic.style.opacity = this.Opacity / 100;
	}
	
	//设置预览对象
	if(this.View){ this._view.src = this.Url; }
	
	//设置拖放
	this._drag.mxRight = this.Width; this._drag.mxBottom = this.Height;
	//设置缩放
	if(this._resize){ this._resize.mxRight = this.Width; this._resize.mxBottom = this.Height; this._resize.Scale = this.Scale; }
  },
  //设置获取缩放对象
  GetResize: function() {
	var op = this.options;
	//有触发对象时才设置
	if(op.RightDown || op.LeftDown || op.RightUp || op.LeftUp || op.Right || op.Left || op.Up || op.Down ){
		var oThis = this, _resize = new Resize(this.Drag, { Limit: true, onResize: function(){ oThis.SetPos(); } });
		
		//设置缩放触发对象
		if(op.RightDown){ _resize.Set(op.RightDown, "right-down"); }
		if(op.LeftDown){ _resize.Set(op.LeftDown, "left-down"); }
		
		if(op.RightUp){ _resize.Set(op.RightUp, "right-up"); }
		if(op.LeftUp){ _resize.Set(op.LeftUp, "left-up"); }
		
		if(op.Right){ _resize.Set(op.Right, "right"); }
		if(op.Left){ _resize.Set(op.Left, "left"); }
		
		if(op.Up){ _resize.Set(op.Up, "up"); }
		if(op.Down){ _resize.Set(op.Down, "down"); }
		
		return _resize;
	} else { return null; }
  },  
  //设置切割
  SetPos: function() {
	var o = this.Drag;
	//按拖放对象的参数进行切割
	this._cropper.style.clip = "rect(" + o.offsetTop + "px " + (o.offsetLeft + o.offsetWidth) + "px " + (o.offsetTop + o.offsetHeight) + "px " + o.offsetLeft + "px)";
	//this is xlad added (start)  this can move to the current page
	document.getElementById("L").value=o.offsetLeft;
	document.getElementById("T").value=o.offsetTop;
	document.getElementById("W").value=o.offsetWidth;
	document.getElementById("H").value=o.offsetHeight;
	//this is xland added (end)
	//切割预览
	if(this.View) this.PreView();
  },  
  //切割预览
  PreView: function() {
	//按比例设置宽度和高度
	var o = this.Drag, h = this.viewWidth, w = h * o.offsetWidth / o.offsetHeight;
	if(w > this.viewHeight){ w = this.viewHeight; h = w * o.offsetHeight / o.offsetWidth; }
	//获取对应比例尺寸
	var scale = h / o.offsetHeight, ph = this.Height * scale, pw = this.Width * scale, pt = o.offsetTop * scale, pl = o.offsetLeft * scale, styleView = this._view.style;
	//设置样式
	styleView.width = pw + "px"; styleView.height = ph + "px";
	styleView.top = - pt + "px "; styleView.left = - pl + "px";
	//切割预览图
	styleView.clip = "rect(" + pt + "px " + (pl + w) + "px " + (pt + h) + "px " + pl + "px)";
  }
}