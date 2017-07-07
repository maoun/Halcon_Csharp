function $get(id) {
    return document.getElementById(id);
}

//下面代码 截取自 QQ MAP 部分代码 略有改动 : Begin
var changedStr = '_' + 'changed';

function getAccessors(mvcObject) {
    return mvcObject.accessors_ || (mvcObject.accessors_ = {});
};
function getBindings(mvcObject) {
    return mvcObject.bindings_ || (mvcObject.bindings_ = {});
};
function triggerChanged(mvcObj, key) {
    var change = key + changedStr;
    mvcObj[change] ? mvcObj[change]() : mvcObj.changed(key);
    //_abb.trigger(mvcObj, key.toLowerCase() + changedStr);
};
easeOutCubic = function (t, b, c, d) {
    return c * (Math.pow(t / d - 1, 3) + 1) + b
};
function qAnimationExec(anim) {
    return function () {
        var currentFrame = anim.get('current');
        var duration = anim.get('duration');
        var fps = anim.get('fps');
        currentFrame > duration && (currentFrame = duration);
        anim.frame(currentFrame);
        if (currentFrame >= duration) {
            anim.stop();
            return
        }
        anim.set('current', currentFrame + 1 / fps)
    }
};

function animEffect(options) {
    this.initialize(options);
};
animEffect.prototype = {
    initialize: function (options) {
        options = options || {};
        options['duration'] = options['duration'] || 0;
        options['fps'] = options['fps'] || 40;
        this.setValues(options)
    },
    prepare: function () { },
    start: function (noEvent) {
        this.prepare();
        this.stop(true);
        var fps = this.get('fps');
        this.set('current', 1 / fps);
        //! noEvent && _abb.trigger(this, 'start');
        this.set('status', 1);
        this.timer_ = window.setInterval(qAnimationExec(this), 1000 / fps)
    },
    stop: function (noEvent) {
        this.timer_ && window.clearInterval(this.timer_);
        this.timer_ = null;
        //! noEvent && _abb.trigger(this, 'end');
        this.set('status', 0);
        this.set('current', -1)
    },
    getStatus: function () {
        return this.get('status')
    },
    frame: function (currentFrame) {
        var begins = this.get('begins');
        var ends = this.get('ends');
        var method = this.get('method');
        var duration = this.get('duration');
        var values = [];
        var steps = [];
        for (var i = 0, len = begins.length; i < len; ++i) {
            var delta = ends[i] - begins[i], value, step;
            if (duration != 0) {
                value = method(currentFrame, begins[i], delta, duration);
                var lastFrame = currentFrame - 1 / this.get('fps');
                lastFrame < 0 && (lastFrame = 0);
                step = value - method(lastFrame, begins[i], delta, duration)
            } else {
                value = ends[i];
                step = delta
            }
            values.push(parseInt(value, 10));
            steps.push(parseInt(step, 10))
        }
        this.qFxFrame(values, steps)
    },
    qFxFrame: function (values, steps) {
        var callback = this.get('callback');
        if (callback) {
            callback(values, steps)
        }
    },
    //----------------------------------------------------------------------------------
    get: function (key) {
        var accessor = getAccessors(this)[key];
        if (accessor) {
            var target = accessor['target'];
            var targetKey = accessor['key'];
            var getterName = 'get' + _aaf.capitalInitial(targetKey);
            return target[getterName] ? target[getterName]() : target.get(targetKey)
        } else {
            return this[key]
        }
    },

    set: function (key, value) {
        var accessor = getAccessors(this)[key];
        if (accessor) {
            var target = accessor['target'];
            var targetKey = accessor['key'];
            var setterName = 'set' + _aaf.capitalInitial(targetKey);
            target[setterName] ? target[setterName](value) : target.set(targetKey, value)
        } else {
            this.__checker__ = this.__checker__ || {};
            var checker = this.__checker__[key];
            if (checker && (!checker(value))) {
                throw Error("Invalid value for property <" + (key + (">: " + value)))
            }
            if (this[key] !== value) {
                this[key] = value;
                triggerChanged(this, key)
            }
        }
    },
    setValues: function (values) {
        for (var key in values) {
            var value = values[key];
            var setterName = 'set' + getAccessors(key);
            this[setterName] ? this[setterName](value) : this.set(key, value)
        }
    },
    //----------------------------------------------------------------------------------
    changed: function (key) { }
};
//截取自 QQ MAP : End

var myAnim = null;
function startSlideMove(pixel, divBlock, duration) {
    var pixelCurrent = [divBlock.offsetLeft, divBlock.offsetTop];
    var deltaX = pixelCurrent[0] - pixel[0];
    var deltaY = pixelCurrent[1] - pixel[1];

    if (!myAnim) {
        var options = {
            'callback': function (values, deltas) {
                //debugger
                //console.log(values);
                $get(VNMAP.map.layerContainerDiv.id).style.left = values[0] + 'px';
                $get(VNMAP.map.layerContainerDiv.id).style.top = values[1] + 'px';
            }
        };

        myAnim = new animEffect(options);
    } else {
        myAnim.stop()
    }

    var SPEED = 100;
    var dur = Math.sqrt(deltaX * deltaX + deltaY * deltaY) / SPEED;
    dur < 0.2 && (dur = 0.2);
    dur > 0.5 && (dur = 0.5);
    if (duration != 0) {
        duration = duration || dur;
    }

    var fxMethod = easeOutCubic;

    myAnim.set('begins', pixelCurrent);
    myAnim.set('ends', pixel);
    myAnim.set('duration', duration);
    myAnim.set('method', fxMethod);
    myAnim.set('fps', 40);
    myAnim.start();
}

//By CrossYou
mousedrag = {
    'drags': [],
    'init': function (id) {
        //debugger
        var o = document.getElementById(id);
        o.onmousedown = mousedrag.starIt;
    },
    'starIt': function (e) {
        //重置Event对象
        e = mousedrag.setEv(e);
        if (myAnim) myAnim.stop();
        t = this;
        timer = new Date().getTime();

        // 注册mousemove事件到document对象
        document.onmousemove = mousedrag.dragIt;
        // 注册mouseup事件到document对象
        document.onmouseup = mousedrag.endIt;

        page = {
            x: 0,
            y: 0
        };
        //获取对象的offset
        offset = {
            left: t.offsetLeft,
            top: t.offsetTop
        }
        //获取鼠标的页面坐标,并存放到page对象
        page.x = e.pageX; //pageX和pageY只在FF下有效
        page.y = e.pageY;
    },
    'dragIt': function (e) {
        //重置Event对象
        e = mousedrag.setEv(e);

        var left = e.pageX - page.x + offset.left;
        var top = e.pageY - page.y + offset.top;

        deltaX = left - parseInt(t.style.left);
        deltaY = top - parseInt(t.style.top);

        t.style.left = left + "px";
        t.style.top = top + "px";

        var tmp = new Date().getTime();
        if (tmp != null) {
            disTime = tmp - timer;
            timer = tmp;
            disX = -deltaX;
            disY = -deltaY
        }
    },
    'endIt': function (e) {
        //重置Event对象
        e = mousedrag.setEv(e);
        var tmp = new Date().getTime();
        var fDisTime = tmp - timer;
        if (fDisTime > 100) {
            disX = 0;
            disY = 0
        }
        disTime = fDisTime < 5 ? disTime : fDisTime;
        if (disTime == 0 && (disX != 0 || disY != 0)) {
            disTime = 1
        }
        if (disTime) {
            var tx = -(120 / disTime) * disX;
            var ty = -(120 / disTime) * disY;
            var pixel = [parseInt(t.style.left) + tx, parseInt(t.style.top) + ty];
            //console.log(disX);
            startSlideMove(pixel, t, 0.6);
        }
        disX = 0;
        disY = 0;
        disTime = 0;
        timer = null

        document.onmousemove = null;
        document.onmouseup = null;
    },
    'setEv': function (e) {
        var e = e || window.event;
        if (typeof e.pageX == 'undefined') {
            e.pageX = e.clientX + document.documentElement.scrollLeft;
            e.pageY = e.clientY + document.documentElement.scrollTop;
        }
        return e;
    }
};