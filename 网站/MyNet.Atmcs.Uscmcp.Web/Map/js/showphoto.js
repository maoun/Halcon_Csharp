/*
[I5A6] IA Inc
$RCSfile: common.js -- menu.js  --common.js ,v $
$Revision: 1.103 $
$Date: 2008/07/30 09:16:52 $
*/


//common.js ------------------start 2008-12-26----------------

//process language is en-us or zh-cn 

var zoomwheel = "Mouse Wheel zoom picture";
var newWindowPic = "In the new window opens";
var resizePic = "Actual size";
var closePic = "Close";
if (window.navigator.systemLanguage == "zh-CN") {
    zoomwheel = "";
    newWindowPic = "在新窗口打开";
    resizePic = "实际大小";
    closePic = "关闭";
}

var lang = new Array();
var userAgent = navigator.userAgent.toLowerCase();
var is_opera = userAgent.indexOf('opera') != -1 && opera.version();
var is_moz = (navigator.product == 'Gecko') && userAgent.substr(userAgent.indexOf('firefox') + 8, 3);
var is_ie = (userAgent.indexOf('msie') != -1 && !is_opera) && userAgent.substr(userAgent.indexOf('msie') + 5, 3);

function $i5a6(id) {
    return document.getElementById(id);
}

/*
Array.prototype.push = function(value) {
this[this.length] = value;
return this.length;
}
*/
if (typeof Array.prototype.push === 'undefined') {
    Array.prototype.push = function (value) {
        this[this.length] = value;
        return this.length;
    }
}


function checkall(form, prefix, checkall) {
    var checkall = checkall ? checkall : 'chkall';
    for (var i = 0; i < form.elements.length; i++) {
        var e = form.elements[i];
        if (e.name && e.name != checkall && (!prefix || (prefix && e.name.match(prefix)))) {
            e.checked = form.elements[checkall].checked;
        }
    }
}

function doane(event) {
    e = event ? event : window.event;
    if (is_ie) {
        e.returnValue = false;
        e.cancelBubble = true;
    } else if (e) {
        e.stopPropagation();
        e.preventDefault();
    }
}

function fetchCheckbox(cbn) {
    return $i5a6(cbn) && $i5a6(cbn).checked == true ? 1 : 0;
}

function getcookie(name) {
    var cookie_start = document.cookie.indexOf(name);
    var cookie_end = document.cookie.indexOf(";", cookie_start);
    return cookie_start == -1 ? '' : unescape(document.cookie.substring(cookie_start + name.length + 1, (cookie_end > cookie_start ? cookie_end : document.cookie.length)));
}

function thumbImg(obj) {
    var zw = obj.width;
    var zh = obj.height;
    if (is_ie && zw == 0 && zh == 0) {
        var matches
        re = /width=(["']?)(\d+)(\1)/i
        matches = re.exec(obj.outerHTML);
        zw = matches[2];
        re = /height=(["']?)(\d+)(\1)/i
        matches = re.exec(obj.outerHTML);
        zh = matches[2];
    }
    obj.resized = true;
    obj.style.width = zw + 'px';
    obj.style.height = 'auto';
    if (obj.offsetHeight > zh) {
        obj.style.height = zh + 'px';
        obj.style.width = 'auto';
    }
    if (is_ie) {
        var imgid = 'img_' + Math.random();
        obj.id = imgid;
        setTimeout('try {if ($i5a6(\'' + imgid + '\').offsetHeight > ' + zh + ') {$i5a6(\'' + imgid + '\').style.height = \'' + zh + 'px\';$i5a6(\'' + imgid + '\').style.width = \'auto\';}} catch(e){}', 1000);
    }
    obj.onload = null;
}

function imgzoom(obj) { }

function in_array(needle, haystack) {
    if (typeof needle == 'string' || typeof needle == 'number') {
        for (var i in haystack) {
            if (haystack[i] == needle) {
                return true;
            }
        }
    }
    return false;
}

function setcopy(text, alertmsg) {
    if (is_ie) {
        clipboardData.setData('Text', text);
        alert(alertmsg);
    } else if (prompt('Press Ctrl+C Copy to Clipboard', text)) {
        alert(alertmsg);
    }
}

function isUndefined(variable) {
    return typeof variable == 'undefined' ? true : false;
}

function mb_strlen(str) {
    var len = 0;
    for (var i = 0; i < str.length; i++) {
        len += str.charCodeAt(i) < 0 || str.charCodeAt(i) > 255 ? (charset == 'utf-8' ? 3 : 2) : 1;
    }
    return len;
}

function setcookie(cookieName, cookieValue, seconds, path, domain, secure) {
    var expires = new Date();
    expires.setTime(expires.getTime() + seconds);
    document.cookie = escape(cookieName) + '=' + escape(cookieValue)
		+ (expires ? '; expires=' + expires.toGMTString() : '')
		+ (path ? '; path=' + path : '/')
		+ (domain ? '; domain=' + domain : '')
		+ (secure ? '; secure' : '');
}

function strlen(str) {
    return (is_ie && str.indexOf('\n') != -1) ? str.replace(/\r?\n/g, '_').length : str.length;
}

function updatestring(str1, str2, clear) {
    str2 = '_' + str2 + '_';
    return clear ? str1.replace(str2, '') : (str1.indexOf(str2) == -1 ? str1 + str2 : str1);
}

function toggle_collapse(objname, noimg) {
    var obj = $i5a6(objname);
    obj.style.display = obj.style.display == '' ? 'none' : '';
    if (!noimg) {
        var img = $i5a6(objname + '_img');
        img.src = img.src.indexOf('_yes.gif') == -1 ? img.src.replace(/_no\.gif/, '_yes\.gif') : img.src.replace(/_yes\.gif/, '_no\.gif')
    }
    var collapsed = getcookie('discuz_collapse');
    collapsed = updatestring(collapsed, objname, !obj.style.display);
    setcookie('discuz_collapse', collapsed, (collapsed ? 86400 * 30 : -(86400 * 30 * 1000)));
}

function trim(str) {
    return (str + '').replace(/(\s+)$/g, '').replace(/^\s+/g, '');
}

function updateseccode() {
    type = seccodedata[2];
    var rand = Math.random();
    if (type == 2) {
        $i5a6('seccodeimage').innerHTML = '<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0" width="' + seccodedata[0] + '" height="' + seccodedata[1] + '" align="middle">'
			+ '<param name="allowScriptAccess" value="sameDomain" /><param name="movie" value="seccode.php?update=' + rand + '" /><param name="quality" value="high" /><param name="wmode" value="transparent" /><param name="bgcolor" value="#ffffff" />'
			+ '<embed src="seccode.php?update=' + rand + '" quality="high" wmode="transparent" bgcolor="#ffffff" width="' + seccodedata[0] + '" height="' + seccodedata[1] + '" align="middle" allowScriptAccess="sameDomain" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" /></object>';
    } else {
        $i5a6('seccodeimage').innerHTML = '<img id="seccode" onclick="updateseccode()" width="' + seccodedata[0] + '" height="' + seccodedata[1] + '" src="seccode.php?update=' + rand + '" class="absmiddle" alt="" />';
    }
}

function updatesecqaa() {
    var x = new Ajax();
    x.get('ajax.php?action=updatesecqaa&inajax=1', function (s) {
        $i5a6('secquestion').innerHTML = s;
    });
}

function _attachEvent(obj, evt, func) {
    if (obj.addEventListener) {
        obj.addEventListener(evt, func, false);
    } else if (obj.attachEvent) {
        obj.attachEvent("on" + evt, func);
    }
}
//menu.js ------------------end 2008-12-26----------------

var jsmenu = new Array();
var ctrlobjclassName;
jsmenu['active'] = new Array();
jsmenu['timer'] = new Array();
jsmenu['iframe'] = new Array();

function initCtrl(ctrlobj, click, duration, timeout, layer) {
    if (ctrlobj && !ctrlobj.initialized) {
        ctrlobj.initialized = true;
        ctrlobj.unselectable = true;

        ctrlobj.outfunc = typeof ctrlobj.onmouseout == 'function' ? ctrlobj.onmouseout : null;
        ctrlobj.onmouseout = function () {
            if (this.outfunc) this.outfunc();
            if (duration < 3) jsmenu['timer'][ctrlobj.id] = setTimeout('hideMenu(' + layer + ')', timeout);
        }

        if (click && duration) {
            ctrlobj.clickfunc = typeof ctrlobj.onclick == 'function' ? ctrlobj.onclick : null;
            ctrlobj.onclick = function (e) {
                doane(e);
                if (jsmenu['active'][layer] == null || jsmenu['active'][layer].ctrlkey != this.id) {
                    if (this.clickfunc) this.clickfunc();
                    else showMenu(this.id, true);
                } else {
                    hideMenu(layer);
                }
            }
        }

        ctrlobj.overfunc = typeof ctrlobj.onmouseover == 'function' ? ctrlobj.onmouseover : null;
        ctrlobj.onmouseover = function (e) {
            doane(e);
            if (this.overfunc) this.overfunc();
            if (click) {
                clearTimeout(jsmenu['timer'][this.id]);
            } else {
                for (var id in jsmenu['timer']) {
                    if (jsmenu['timer'][id]) clearTimeout(jsmenu['timer'][id]);
                }
            }
        }
    }
}

function initMenu(ctrlid, menuobj, duration, timeout, layer) {
    if (menuobj && !menuobj.initialized) {
        menuobj.initialized = true;
        menuobj.ctrlkey = ctrlid;
        menuobj.onclick = ebygum;
        menuobj.style.position = 'absolute';
        if (duration < 3) {
            if (duration > 1) {
                menuobj.onmouseover = function () {
                    clearTimeout(jsmenu['timer'][ctrlid]);
                }
            }
            if (duration != 1) {
                menuobj.onmouseout = function () {
                    jsmenu['timer'][ctrlid] = setTimeout('hideMenu(' + layer + ')', timeout);
                }
            }
        }
        menuobj.style.zIndex = 50;
        if (is_ie) {
            menuobj.style.filter += "progid:DXImageTransform.Microsoft.shadow(direction=135,color=#CCCCCC,strength=2)";
        }
    }
}

function showMenu(ctrlid, click, offset, duration, timeout, layer, showid, maxh) {
    e = window.event ? window.event : showMenu.caller.arguments[0];
    var ctrlobj = $i5a6(ctrlid);
    if (!ctrlobj) return;
    if (isUndefined(click)) click = false;
    if (isUndefined(offset)) offset = 0;
    if (isUndefined(duration)) duration = 2;
    if (isUndefined(timeout)) timeout = 500;
    if (isUndefined(layer)) layer = 0;
    if (isUndefined(showid)) showid = ctrlid;
    var showobj = $i5a6(showid);
    var menuobj = $i5a6(showid + '_menu');
    if (!showobj || !menuobj) return;
    if (isUndefined(maxh)) maxh = 400;

    hideMenu(layer);

    for (var id in jsmenu['timer']) {
        if (jsmenu['timer'][id]) clearTimeout(jsmenu['timer'][id]);
    }

    initCtrl(ctrlobj, click, duration, timeout, layer);
    ctrlobjclassName = ctrlobj.className;
    ctrlobj.className += ' hover';
    initMenu(ctrlid, menuobj, duration, timeout, layer);

    menuobj.style.display = '';
    if (!is_opera) {
        menuobj.style.clip = 'rect(auto, auto, auto, auto)';
    }

    setMenuPosition(showid, offset);

    if (is_ie && is_ie < 7) {
        if (!jsmenu['iframe'][layer]) {
            var iframe = document.createElement('iframe');
            iframe.style.display = 'none';
            iframe.style.position = 'absolute';
            iframe.style.filter = 'progid:DXImageTransform.Microsoft.Alpha(style=0,opacity=0)';
            //$i5a6('append_parent') ? $i5a6('append_parent').appendChild(iframe) : menuobj.parentNode.appendChild(iframe);
            jsmenu['iframe'][layer] = iframe;
        }
        jsmenu['iframe'][layer].style.top = menuobj.style.top;
        jsmenu['iframe'][layer].style.left = menuobj.style.left;
        jsmenu['iframe'][layer].style.width = menuobj.w;
        jsmenu['iframe'][layer].style.height = menuobj.h;
        jsmenu['iframe'][layer].style.display = 'block';
    }

    if (maxh && menuobj.scrollHeight > maxh) {
        menuobj.style.height = maxh + 'px';
        if (is_opera) {
            menuobj.style.overflow = 'auto';
        } else {
            menuobj.style.overflowY = 'auto';
        }
    }

    if (!duration) {
        setTimeout('hideMenu(' + layer + ')', timeout);
    }

    jsmenu['active'][layer] = menuobj;
}

function setMenuPosition(showid, offset) {
    var showobj = $i5a6(showid);
    var menuobj = $i5a6(showid + '_menu');
    if (isUndefined(offset)) offset = 0;
    if (showobj) {
        showobj.pos = fetchOffset(showobj);
        showobj.X = showobj.pos['left'];
        showobj.Y = showobj.pos['top'];
        showobj.w = showobj.offsetWidth;
        showobj.h = showobj.offsetHeight;
        menuobj.w = menuobj.offsetWidth;
        menuobj.h = menuobj.offsetHeight;
        menuobj.style.left = (showobj.X + menuobj.w > document.body.clientWidth) && (showobj.X + showobj.w - menuobj.w >= 0) ? showobj.X + showobj.w - menuobj.w + 'px' : showobj.X + 'px';
        menuobj.style.top = offset == 1 ? showobj.Y + 'px' : (offset == 2 || ((showobj.Y + showobj.h + menuobj.h > document.documentElement.scrollTop + document.documentElement.clientHeight) && (showobj.Y - menuobj.h >= 0)) ? (showobj.Y - menuobj.h) + 'px' : showobj.Y + showobj.h + 'px');
        if (menuobj.style.clip && !is_opera) {
            menuobj.style.clip = 'rect(auto, auto, auto, auto)';
        }
    }
}

function hideMenu(layer) {
    if (isUndefined(layer)) layer = 0;
    if (jsmenu['active'][layer]) {
        try {
            $i5a6(jsmenu['active'][layer].ctrlkey).className = ctrlobjclassName;
        } catch (e) { }
        clearTimeout(jsmenu['timer'][jsmenu['active'][layer].ctrlkey]);
        jsmenu['active'][layer].style.display = 'none';
        if (is_ie && is_ie < 7 && jsmenu['iframe'][layer]) {
            jsmenu['iframe'][layer].style.display = 'none';
        }
        jsmenu['active'][layer] = null;
    }
}

function fetchOffset(obj) {
    var left_offset = obj.offsetLeft;
    var top_offset = obj.offsetTop;
    while ((obj = obj.offsetParent) != null) {
        left_offset += obj.offsetLeft;
        top_offset += obj.offsetTop;
    }
    return { 'left': left_offset, 'top': top_offset };
}

function ebygum(eventobj) {
    if (!eventobj || is_ie) {
        window.event.cancelBubble = true;
        return window.event;
    } else {
        if (eventobj.target.type == 'submit') {
            eventobj.target.form.submit();
        }
        eventobj.stopPropagation();
        return eventobj;
    }
}

function menuoption_onclick_function(e) {
    this.clickfunc();
    hideMenu();
}

function menuoption_onclick_link(e) {
    choose(e, this);
}

function menuoption_onmouseover(e) {
    this.className = 'popupmenu_highlight';
}

function menuoption_onmouseout(e) {
    this.className = 'popupmenu_option';
}

function choose(e, obj) {
    var links = obj.getElementsByTagName('a');
    if (links[0]) {
        if (is_ie) {
            links[0].click();
            window.event.cancelBubble = true;
        } else {
            if (e.shiftKey) {
                window.open(links[0].href);
                e.stopPropagation();
                e.preventDefault();
            } else {
                window.location = links[0].href;
                e.stopPropagation();
                e.preventDefault();
            }
        }
        hideMenu();
    }
}

//menu.js ------------------start 2008-12-26----------------


//menu.js ------------------end 2008-12-26----------------



//photo.js ------------------start 2008-12-26----------------
var msgwidth = 0;
function attachimg(obj, action) {
    if (action == 'load') {
        if (is_ie && is_ie < 7) {
            var objinfo = fetchOffset(obj);
            msgwidth = document.body.clientWidth - objinfo['left'] - 20;
        } else {
            if (!msgwidth) {
                var re = /postcontent|message/i;
                var testobj = obj;
                while ((testobj = testobj.parentNode) != null) {
                    var matches = re.exec(testobj.className);
                    if (matches != null) {
                        msgwidth = testobj.clientWidth - 20;
                        break;
                    }
                }
                if (msgwidth < 1) {
                    msgwidth = window.screen.width;
                }
            }
        }
        if (obj.width > msgwidth) {
            obj.resized = true;
            obj.width = msgwidth;
            obj.style.cursor = 'pointer';
        } else {
            obj.onclick = null;
        }
    } else if (action == 'mouseover') {
        if (obj.resized) {
            obj.style.cursor = 'pointer';
        }
    }
}

function attachimginfo(obj, infoobj, show, event) {
    objinfo = fetchOffset(obj);
    if (show) {
        $i5a6(infoobj).style.left = objinfo['left'] + 'px';
        $i5a6(infoobj).style.top = obj.offsetHeight < 40 ? (objinfo['top'] + obj.offsetHeight) + 'px' : objinfo['top'] + 'px';
        $i5a6(infoobj).style.display = '';
    } else {
        if (is_ie) {
            $i5a6(infoobj).style.display = 'none';
            return;
        } else {
            var mousex = document.body.scrollLeft + event.clientX;
            var mousey = document.documentElement.scrollTop + event.clientY;
            if (mousex < objinfo['left'] || mousex > objinfo['left'] + objinfo['width'] || mousey < objinfo['top'] || mousey > objinfo['top'] + objinfo['height']) {
                $i5a6(infoobj).style.display = 'none';
            }
        }
    }
}

function copycode(obj) {
    if (is_ie && obj.style.display != 'none') {
        var rng = document.body.createTextRange();
        rng.moveToElementText(obj);
        rng.scrollIntoView();
        rng.select();
        rng.execCommand("Copy");
        rng.collapse(false);
    }
}

function signature(obj) {
    if (obj.style.maxHeightIE != '') {
        var height = (obj.scrollHeight > parseInt(obj.style.maxHeightIE)) ? obj.style.maxHeightIE : obj.scrollHeight;
        if (obj.innerHTML.indexOf('<IMG ') == -1) {
            obj.style.maxHeightIE = '';
        }
        return height;
    }
}

function fastreply(subject, postnum) {
    if ($i5a6('postform')) {

        $i5a6('postform').subject.value = subject.replace(/#/, $i5a6(postnum).innerHTML.replace(/<[\/\!]*?[^<>]*?>/ig, ''));
        $i5a6('postform').message.focus();
    }
}

function tagshow(event) {
    var obj = is_ie ? event.srcElement : event.target;
    obj.id = !obj.id ? 'tag_' + Math.random() : obj.id;
    ajaxmenu(event, obj.id, 0, '', 1, 3, 0);
    obj.onclick = null;
}

var zoomobj = Array(); var zoomadjust; var zoomstatus = 1;

function zoom(obj, zimg) {
    if (!zoomstatus) {
        window.open(zimg, '', '');
        return;
    }
    if (!zimg) {
        zimg = obj.src;
    }
    if (!$i5a6('zoomimglayer_bg')) {
        div = document.createElement('div'); div.id = 'zoomimglayer_bg';
        div.style.position = 'absolute';
        div.style.left = div.style.top = '0px';
        div.style.width = '100%';
        div.style.height = document.body.scrollHeight + 'px';
        div.style.backgroundColor = '#000';
        div.style.display = 'none';
        div.style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=80,finishOpacity=100,style=0)';
        div.style.opacity = 0.8;
        $i5a6('append_parent').appendChild(div);
        div = document.createElement('div'); div.id = 'zoomimglayer';
        div.style.position = 'absolute';
        div.className = 'popupmenu_popup';
        div.style.padding = 0;
        $i5a6('append_parent').appendChild(div);
    }
    zoomobj['srcinfo'] = fetchOffset(obj);
    zoomobj['srcobj'] = obj;
    zoomobj['zimg'] = zimg;
    $i5a6('zoomimglayer').style.display = '';
    $i5a6('zoomimglayer').style.left = zoomobj['srcinfo']['left'] + 'px';
    $i5a6('zoomimglayer').style.top = zoomobj['srcinfo']['top'] + 'px';
    $i5a6('zoomimglayer').style.width = zoomobj['srcobj'].width + 'px';
    $i5a6('zoomimglayer').style.height = zoomobj['srcobj'].height + 'px';
    $i5a6('zoomimglayer').style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=40,finishOpacity=100,style=0)';
    $i5a6('zoomimglayer').style.opacity = 0.4;
    $i5a6('zoomimglayer').style.zIndex = 999;
    $i5a6('zoomimglayer').innerHTML = '<table width="100%" height="100%" cellspacing="0" cellpadding="0"><tr><td align="center" valign="middle"><img src="' + IMGDIR + '/loading.gif"></td></tr></table><div style="position:absolute;top:-100000px;visibility:hidden"><img onload="zoomimgresize(this)" src="' + zoomobj['zimg'] + '"></div>';
}

var zoomdragstart = new Array();
var zoomclick = 0;
function zoomdrag(e, op) {
    if (op == 1) {
        zoomclick = 1;
        zoomdragstart = is_ie ? [event.clientX, event.clientY] : [e.clientX, e.clientY];
        zoomdragstart[2] = parseInt($i5a6('zoomimglayer').style.left);
        zoomdragstart[3] = parseInt($i5a6('zoomimglayer').style.top);
        doane(e);
    } else if (op == 2 && zoomdragstart[0]) {
        zoomclick = 0;
        var zoomdragnow = is_ie ? [event.clientX, event.clientY] : [e.clientX, e.clientY];
        $i5a6('zoomimglayer').style.left = (zoomdragstart[2] + zoomdragnow[0] - zoomdragstart[0]) + 'px';
        $i5a6('zoomimglayer').style.top = (zoomdragstart[3] + zoomdragnow[1] - zoomdragstart[1]) + 'px';
        doane(e);
    } else if (op == 3) {
        if (zoomclick) zoomclose();
        zoomdragstart = [];
        doane(e);
    }
}

function zoomST(c) {
    if ($i5a6('zoomimglayer').style.display == '') {
        $i5a6('zoomimglayer').style.left = (parseInt($i5a6('zoomimglayer').style.left) + zoomobj['x']) + 'px';
        $i5a6('zoomimglayer').style.top = (parseInt($i5a6('zoomimglayer').style.top) + zoomobj['y']) + 'px';
        $i5a6('zoomimglayer').style.width = (parseInt($i5a6('zoomimglayer').style.width) + zoomobj['w']) + 'px';
        $i5a6('zoomimglayer').style.height = (parseInt($i5a6('zoomimglayer').style.height) + zoomobj['h']) + 'px';
        var opacity = c * 20;
        $i5a6('zoomimglayer').style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=' + opacity + ',finishOpacity=100,style=0)';
        $i5a6('zoomimglayer').style.opacity = opacity / 100;
        c++;
        if (c <= 5) {
            setTimeout('zoomST(' + c + ')', 5);
        } else {
            zoomadjust = 1;
            $i5a6('zoomimglayer').style.filter = '';
            $i5a6('zoomimglayer_bg').style.display = '';
            $i5a6('zoomimglayer').innerHTML = '<table cellspacing="0" cellpadding="2"><tr><td style="text-align: right;FONT-FAMILY: Verdana,Helvetica,Arial,sans-serif;font-size=12px">' + zoomwheel + '&nbsp;&nbsp;  <a href="' + zoomobj['zimg'] + '" target="_blank"><img src="' + IMGDIR + '/newwindow.gif" border="0" style="vertical-align: middle" title="' + newWindowPic + '" /></a> <a href="###" onclick="zoomimgadjust(event, 1)"><img src="' + IMGDIR + '/resize.gif" border="0" style="vertical-align: middle" title="' + resizePic + '" /></a> <a href="###" onclick="zoomclose()"><img style="vertical-align: middle" src="' + IMGDIR + '/close.gif" title="' + closePic + '" /></a>&nbsp;</td></tr><tr><td align="center" id="zoomimgbox"><img id="zoomimg" style="cursor: move; margin: 5px;" src="' + zoomobj['zimg'] + '" width="' + $i5a6('zoomimglayer').style.width + '" height="' + $i5a6('zoomimglayer').style.height + '"></td></tr></table>';
            $i5a6('zoomimglayer').style.overflow = 'visible';
            $i5a6('zoomimglayer').style.width = $i5a6('zoomimglayer').style.height = 'auto';
            if (is_ie) {
                $i5a6('zoomimglayer').onmousewheel = zoomimgadjust;
            } else {
                $i5a6('zoomimglayer').addEventListener("DOMMouseScroll", zoomimgadjust, false);
            }
            $i5a6('zoomimgbox').onmousedown = function (event) { try { zoomdrag(event, 1); } catch (e) { } };
            $i5a6('zoomimgbox').onmousemove = function (event) { try { zoomdrag(event, 2); } catch (e) { } };
            $i5a6('zoomimgbox').onmouseup = function (event) { try { zoomdrag(event, 3); } catch (e) { } };
        }
    }
}

function zoomimgresize(obj) {
    zoomobj['zimginfo'] = [obj.width, obj.height];
    var r = obj.width / obj.height;
    var w = document.body.clientWidth * 0.95;
    w = obj.width > w ? w : obj.width;
    var h = w / r;
    var clientHeight = document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight;
    var scrollTop = document.body.scrollTop ? document.body.scrollTop : document.documentElement.scrollTop;
    if (h > clientHeight) {
        h = clientHeight;
        w = h * r;
    }
    var l = (document.body.clientWidth - w) / 2;
    var t = h < clientHeight ? (clientHeight - h) / 2 : 0;
    t += +scrollTop;
    zoomobj['x'] = (l - zoomobj['srcinfo']['left']) / 5;
    zoomobj['y'] = (t - zoomobj['srcinfo']['top']) / 5;
    zoomobj['w'] = (w - zoomobj['srcobj'].width) / 5;
    zoomobj['h'] = (h - zoomobj['srcobj'].height) / 5;
    $i5a6('zoomimglayer').style.filter = '';
    $i5a6('zoomimglayer').innerHTML = '';
    setTimeout('zoomST(1)', 5);
}

function zoomimgadjust(e, a) {
    if (!a) {
        if (!e) e = window.event;
        if (e.altKey || e.shiftKey || e.ctrlKey) return;
        var l = parseInt($i5a6('zoomimglayer').style.left);
        var t = parseInt($i5a6('zoomimglayer').style.top);
        if (e.wheelDelta <= 0 || e.detail > 0) {
            if ($i5a6('zoomimg').width <= 200 || $i5a6('zoomimg').height <= 200) {
                doane(e); return;
            }
            $i5a6('zoomimg').width -= zoomobj['zimginfo'][0] / 10;
            $i5a6('zoomimg').height -= zoomobj['zimginfo'][1] / 10;
            l += zoomobj['zimginfo'][0] / 20;
            t += zoomobj['zimginfo'][1] / 20;
        } else {
            if ($i5a6('zoomimg').width >= zoomobj['zimginfo'][0]) {
                zoomimgadjust(e, 1); return;
            }
            $i5a6('zoomimg').width += zoomobj['zimginfo'][0] / 10;
            $i5a6('zoomimg').height += zoomobj['zimginfo'][1] / 10;
            l -= zoomobj['zimginfo'][0] / 20;
            t -= zoomobj['zimginfo'][1] / 20;
        }
    } else {
        var clientHeight = document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight;
        var scrollTop = document.body.scrollTop ? document.body.scrollTop : document.documentElement.scrollTop;
        $i5a6('zoomimg').width = zoomobj['zimginfo'][0]; $i5a6('zoomimg').height = zoomobj['zimginfo'][1];
        var l = (document.body.clientWidth - $i5a6('zoomimg').clientWidth) / 2; l = l > 0 ? l : 0;
        var t = (clientHeight - $i5a6('zoomimg').clientHeight) / 2 + scrollTop; t = t > 0 ? t : 0;
    }
    $i5a6('zoomimglayer').style.left = l + 'px';
    $i5a6('zoomimglayer').style.top = t + 'px';
    $i5a6('zoomimglayer_bg').style.height = t + $i5a6('zoomimglayer').clientHeight > $i5a6('zoomimglayer_bg').clientHeight ? (t + $i5a6('zoomimglayer').clientHeight) + 'px' : $i5a6('zoomimglayer_bg').style.height;
    doane(e);
}

function zoomclose() {
    $i5a6('zoomimglayer').innerHTML = '';
    $i5a6('zoomimglayer').style.display = 'none';
    $i5a6('zoomimglayer_bg').style.display = 'none';
}

function videoPlay(vid, vtime, tid, pid) {
    ajaxget('api/video.php?action=updatevideoinfo&vid=' + vid + '&vtime=' + vtime + '&tid=' + tid + '&pid=' + pid, '');
}

//photo.js ------------------end 2008-12-26----------------
function showpotonohref() { return false; }