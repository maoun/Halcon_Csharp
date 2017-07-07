var ImageZoom = function(image, viewer, options) {
	this._initialize( image, viewer, options );
	this._initMode( this.options.mode );
	this._oninit();
	this._initLoad();
};

ImageZoom.prototype = {
  _initialize: function(image, viewer, options) {
	this._image = $$(image); 
	this._zoom = document.createElement("img"); 
	this._viewer = $$(viewer); 
	this._viewerWidth = 0; 
	this._viewerHeight = 0; 
	this._preload = new Image(); 
	this._rect = null;
	this._repairLeft = 0;
	this._repairTop = 0;
	this._rangeWidth = 0;
	this._rangeHeight = 0;
	this._timer = null;
	this._loaded = false;
	this._substitute = false;
	
	var opt = this._setOptions(options);
	
	this._scale = opt.scale;
	this._max = opt.max;
	this._min = opt.min;
	this._originPic = opt.originPic;
	this._zoomPic = opt.zoomPic;
	this._rangeWidth = opt.rangeWidth;
	this._rangeHeight = opt.rangeHeight;
	
	this.delay = opt.delay;
	this.autoHide = opt.autoHide;
	this.mouse = opt.mouse;
	this.rate = opt.rate;
	
	this.onLoad = opt.onLoad;
	this.onStart = opt.onStart;
	this.onMove = opt.onMove;
	this.onEnd = opt.onEnd;
	
	var oThis = this, END = function(){ oThis._end(); };
	this._END = function(){ oThis._timer = setTimeout( END, oThis.delay ); };
	this._START = $$F.bindAsEventListener( this._start, this );
	this._MOVE = $$F.bindAsEventListener( this._move, this );
	this._MOUSE = $$F.bindAsEventListener( this._mouse, this );
	this._OUT = $$F.bindAsEventListener( function(e){
			if ( !e.relatedTarget ) this._END();
		}, this );
  },
  _setOptions: function(options) {
    this.options = {
		mode:		"simple",
		scale:		0,
		max:		10,
		min:		1.5,
		originPic:	"",
		zoomPic:	"",
		rangeWidth:	0,
		rangeHeight:0,
		delay:		100,
		autoHide: false, 
		mouse:		true,
		rate:		.5,
		onLoad:		$$.emptyFunction,
		onStart:	$$.emptyFunction,
		onMove:		$$.emptyFunction,
		onEnd:		$$.emptyFunction
    };
    return $$.extend(this.options, options || {});
  },
  _initMode: function(mode) {
	mode = $$.extend({
		options:{},
		init:	$$.emptyFunction,
		load:	$$.emptyFunction,
		start:	$$.emptyFunction,
		end:	$$.emptyFunction,
		move:	$$.emptyFunction,
		dispose:$$.emptyFunction
    }, (ImageZoom._MODE || {})[ mode.toLowerCase() ] || {} );
	this.options = $$.extend( mode.options, this.options );
	this._oninit = mode.init;
	this._onload = mode.load;
	this._onstart = mode.start;
	this._onend = mode.end;
	this._onmove = mode.move;
	this._ondispose = mode.dispose;
  },
  _initLoad: function() {
	var image = this._image, originPic = this._originPic,
		useOrigin = !this._zoomPic && this._scale,
		loadImage = $$F.bind( useOrigin ? this._loadOriginImage : this._loadImage, this );
	if ( this.autoHide ) { this._viewer.style.display = "none"; }
	if ( originPic && originPic != image.src ) {
		image.onload = loadImage;
		image.src = originPic;
	} else if ( image.src ) {
		if ( !image.complete ) {
			image.onload = loadImage;
		} else {
			loadImage();
		}
	} else {
		return;
	}
	if ( !useOrigin ) {
		var preload = this._preload, zoomPic = this._zoomPic || image.src,
			loadPreload = $$F.bind( this._loadPreload, this );
		if ( zoomPic != preload.src ) {
			preload.onload = loadPreload;
			preload.src = zoomPic;
		} else {
			if ( !preload.complete ) {
				preload.onload = loadPreload;
			} else {
				this._loadPreload();
			}
		}
	}
  },
  _loadOriginImage: function() {
	this._image.onload = null;
	this._zoom.src = this._image.src;
	this._initLoaded();
  },
  _loadImage: function() {
	this._image.onload = null;
	if ( this._loaded ) {
		this._initLoaded();
	} else {
		this._loaded = true;
		if ( this._scale ) {
			this._substitute = true;
			this._zoom.src = this._image.src;
			this._initLoaded();
		}
	}
  },
  _loadPreload: function() {
	this._preload.onload = null;
	this._zoom.src = this._preload.src;
	if ( this._loaded ) {
		if ( !this._substitute ) { this._initLoaded(); }
	} else {
		this._loaded = true;
	}
  },
_initLoaded: function (src) {
	this._initSize();
	this._initViewer();
	this._initData();
	this._onload();
	this.onLoad();
	this.start();
  },
  _initSize: function() {
	var zoom = this._zoom, image = this._image, scale = this._scale;
	if ( !scale ) { scale = this._preload.width / image.width; }
	this._scale = scale = Math.min( Math.max( this._min, scale ), this._max );
	zoom.width = Math.ceil( image.width * scale );
	zoom.height = Math.ceil( image.height * scale );
  },
  _initViewer: function() {
	var zoom = this._zoom, viewer = this._viewer;
	var styles = { padding: 0, overflow: "hidden" }, p = $$D.getStyle( viewer, "position" );
	if ( p != "relative" && p != "absolute" ){ styles.position = "relative"; };
	$$D.setStyle( viewer, styles );
	zoom.style.position = "absolute";
	if ( !$$D.contains( viewer, zoom ) ){ viewer.appendChild( zoom ); }
  },
  _initData: function() {
	var zoom = this._zoom, image = this._image, viewer = this._viewer,
		scale = this._scale, rangeWidth = this._rangeWidth, rangeHeight = this._rangeHeight;
	this._rect = $$D.rect( image );
	this._repairLeft = image.clientLeft + parseInt($$D.getStyle( image, "padding-left" ));
	this._repairTop = image.clientTop + parseInt($$D.getStyle( image, "padding-top" ));
	if ( rangeWidth > 0 && rangeHeight > 0 ) {
		rangeWidth = Math.ceil( rangeWidth );
		rangeHeight = Math.ceil( rangeHeight );
		this._viewerWidth = Math.ceil( rangeWidth * scale );
		this._viewerHeight = Math.ceil( rangeHeight * scale );
		$$D.setStyle( viewer, {
			width: this._viewerWidth + "px",
			height: this._viewerHeight + "px"
		});
	} else {
		var styles;
		if ( !viewer.clientWidth ) {
			var style = viewer.style;
			styles = {
				display: style.display,
				position: style.position,
				visibility: style.visibility
			};
			$$D.setStyle( viewer, {
				display: "block", position: "absolute", visibility: "hidden"
			});
		}
		this._viewerWidth = viewer.clientWidth;
		this._viewerHeight = viewer.clientHeight;
		if ( styles ) { $$D.setStyle( viewer, styles ); }
		
		rangeWidth = Math.ceil( this._viewerWidth / scale );
		rangeHeight = Math.ceil( this._viewerHeight / scale );
	}
	this._rangeWidth = rangeWidth;
	this._rangeHeight = rangeHeight;
  },

  _start: function() {
	clearTimeout( this._timer );
	var viewer = this._viewer, image = this._image, scale = this._scale;
	viewer.style.display = "block";
	this._onstart();
	this.onStart();
	$$E.removeEvent( image, "mouseover", this._START );
	$$E.removeEvent( image, "mousemove", this._START );
	$$E.addEvent( document, "mousemove", this._MOVE );
	$$E.addEvent( document, "mouseout", this._OUT );
	this.mouse && $$E.addEvent( document, $$B.firefox ? "DOMMouseScroll" : "mousewheel", this._MOUSE );
  },
  _move: function(e) {
	clearTimeout( this._timer );
	var x = e.pageX, y = e.pageY, rect = this._rect;
	if ( x < rect.left || x > rect.right || y < rect.top || y > rect.bottom ) {
		this._END();
	} else {
		var zoom = this._zoom,
		pos = this._repair(x - rect.left - this._repairLeft,y - rect.top - this._repairTop	);
		this._onmove( e, pos );
		zoom.style.left = pos.left + "px";
		zoom.style.top = pos.top + "px";
		this.onMove();
	}
  },
  _repair: function(x, y) {
	var scale = this._scale, zoom = this._zoom,
		viewerWidth = this._viewerWidth,
		viewerHeight = this._viewerHeight;
	x = Math.ceil( viewerWidth / 2 - x * scale );
	y = Math.ceil( viewerHeight / 2 - y * scale );
	x = Math.min( Math.max( x, viewerWidth - zoom.width ), 0 );
	y = Math.min( Math.max( y, viewerHeight - zoom.height ), 0 );
	return { left: x, top: y };
  },
  _end: function() {
	this._onend();
	this.onEnd();
	if ( this.autoHide ) { this._viewer.style.display = "none"; }
	this.stop();
	this.start();
  },
  _mouse: function(e) {
	this._scale += ( e.wheelDelta ? e.wheelDelta / (-120) : (e.detail || 0) / 3 ) * this.rate;
	
	var opt = this.options;
	this._rangeWidth = opt.rangeWidth;
	this._rangeHeight = opt.rangeHeight;
	
	this._initSize();
	this._initData();
	this._move(e);
	e.preventDefault();
  },
  start: function() {
	if ( this._viewerWidth && this._viewerHeight ) {
		var image = this._image, START = this._START;
		$$E.addEvent( image, "mouseover", START );
		$$E.addEvent( image, "mousemove", START );
	}
  },
  stop: function() {
	clearTimeout( this._timer );
	$$E.removeEvent( this._image, "mouseover", this._START );
	$$E.removeEvent( this._image, "mousemove", this._START );
	$$E.removeEvent( document, "mousemove", this._MOVE );
	$$E.removeEvent( document, "mouseout", this._OUT );
	$$E.removeEvent( document, $$B.firefox ? "DOMMouseScroll" : "mousewheel", this._MOUSE );
  },
  reset: function(options) {
	this.stop();
	
	var viewer = this._viewer, zoom = this._zoom;
	if ( $$D.contains( viewer, zoom ) ) { viewer.removeChild( zoom ); }
	
	var opt = $$.extend( this.options, options || {} );
	this._scale = opt.scale;
	this._max = opt.max;
	this._min = opt.min;
	this._originPic = opt.originPic;
	this._zoomPic = opt.zoomPic;
	this._rangeWidth = opt.rangeWidth;
	this._rangeHeight = opt.rangeHeight;
	
	this._loaded = this._substitute = false;
	this._rect = null;
	this._repairLeft = this._repairTop = 
	this._viewerWidth = this._viewerHeight = 0;
	
	this._initLoad();
  },
  dispose: function() {
	this._ondispose();
	this.stop();
	if ( $$D.contains( this._viewer, this._zoom ) ) {
		this._viewer.removeChild( this._zoom );
	}
	this._image.onload = this._preload.onload =
		this._image = this._preload = this._zoom = this._viewer =
		this.onLoad = this.onStart = this.onMove = this.onEnd =
		this._START = this._MOVE = this._END = this._OUT = null
  }
}