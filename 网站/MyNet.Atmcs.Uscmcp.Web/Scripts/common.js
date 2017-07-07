//打印前隐藏按钮
function beforeprint() {
    document.getElementById("btnsetting").style.display = "none";
    document.getElementById("btnpreview").style.display = "none";
    document.getElementById("btnprint").style.display = "none";
}
//打印后显示按钮
function afterprint() {
    document.getElementById("btnsetting").style.display = "";
    document.getElementById("btnpreview").style.display = "";
    document.getElementById("btnprint").style.display = "";
}

//隐藏打印按钮或者导出按钮
function InitPrintPage() {
    var opt = queryString("opt");
    if (opt == "print") {
        document.getElementById("lbtExpExcel").style.display = "none";
    }
    else if (opt == "excel") {
        document.getElementById("lbtExpExcel").style.display = "";
        beforeprint();
    }
}

//重置页面中查询控件
function ResetAllControls() {
    $(":input[@type=text]").each(
    function () {
        jQuery(this).val("");
    }
    );

    $("select").each(
    function () {
        jQuery(this)[0].selectedIndex = 0;
    }
    );
}

//得到url参数
function queryString(sParam) {
    var sBase = window.location.search
    var re = eval("/" + sParam + "=([^&]*)/")
    if (re.test(sBase)) {
        return RegExp.$1
    }
    else {
        return "";
    }
}

//打开一个最大化窗体
function OpenFullScreenPage(url) {
    var w = window.screen.availWidth;
    var h = window.screen.availHeight;
    newwin = window.open(url, "", "scrollbars");
    if (document.all) {
        newwin.moveTo(0, 0)//新窗口的坐标
        newwin.resizeTo(w, h)
    }
}

function OpenIndexPage() {
    var w = window.screen.availWidth;
    var h = window.screen.availHeight;
    newwin = window.open("index.aspx", "", "resizable:no,help:yes,scroll:yes;toolbar=yes;menubar=yes,scrollbars=yes,location=yes,status=yes");
    if (document.all) {
        newwin.moveTo(0, 0)//新窗口的坐标
        newwin.resizeTo(w, h)
    }
}

function OpenCenterScreenModalPage(url, width, height) {
    var left = (screen.width - width) / 2;
    var top = (screen.height - height) / 2;
    window.showModalDialog(url, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px;dialogLeft:" + left + "px;dialogTop:" + top + "px;status:no;help:no; scroll:yes;resizable:no;");
}

function OpenPage(url, width, height) {
    var left = (screen.width - width) / 2;
    var top = (screen.height - height) / 2;
    window.open(url, "", "top=" + top + ",left=" + left + ",width=" + width + ",height=" + height + ",status:no,help:no,scroll:yes,resizable:no");
}

function switchTag(tag, content) {
    var count = document.getElementById("ultitle").getElementsByTagName("li").length;
    for (i = 1; i <= count; i++) {
        if ("content" + i == content) {
            document.getElementById(tag).getElementsByTagName("a")[0].className = "selectli1";
            document.getElementById(tag).getElementsByTagName("a")[0].getElementsByTagName("span")[0].className = "selectspan1";
            document.getElementById(content).className = "content";
        }
        else {
            document.getElementById("content" + i).className = "hidecontent";
            document.getElementById("tag" + i).getElementsByTagName("a")[0].className = "";
            document.getElementById("tag" + i).getElementsByTagName("a")[0].getElementsByTagName("span")[0].className = "";
        }
    }
}
var imgObj;
function checkImg(theURL, winName) {
    // 对象是否已创建
    if (typeof (imgObj) == "object") {
        // 是否已取得了图像的高度和宽度
        if ((imgObj.width != 0) && (imgObj.height != 0))
            // 根据取得的图像高度和宽度设置弹出窗口的高度与宽度，并打开该窗口
            // 其中的增量 20 和 30 是设置的窗口边框与图片间的间隔量
            OpenFullSizeWindow(theURL, winName, ",width=" + (imgObj.width + 20) + ",height=" + (imgObj.height + 30) + ",left=" + (screen.availWidth - imgObj.width) / 2 + ",top=" + (screen.availHeight - imgObj.height) / 2);
        else
            // 因为通过 Image 对象动态装载图片，不可能立即得到图片的宽度和高度，所以每隔100毫秒重复调用检查
            setTimeout("checkImg('" + theURL + "','" + winName + "')", 100)
    }
}

function OpenFullSizeWindow(theURL, winName, features) {
    var aNewWin, sBaseCmd;
    // 弹出窗口外观参数
    sBaseCmd = "toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=no,";
    // 调用是否来自 checkImg
    if (features == null || features == "") {
        // 创建图像对象
        imgObj = new Image();
        // 设置图像源
        imgObj.src = theURL;
        // 开始获取图像大小
        checkImg(theURL, winName)
    }
    else {
        // 打开窗口
        aNewWin = window.open(theURL, winName, sBaseCmd + features);
        // 聚焦窗口
        aNewWin.focus();
    }
}

function OnTreeNodeChecked() {
    var ele = window.event.srcElement;
    if (ele.type == 'checkbox') {
        var childrenDivID = ele.id.replace('CheckBox', 'Nodes');
        var div = document.getElementById(childrenDivID);
        if (div != null) {
            var checkBoxs = div.getElementsByTagName('INPUT');
            for (var i = 0; i < checkBoxs.length; i++) {
                checkBoxs[i].checked = ele.checked;
            }
        }
    }
}

function CtoH(obj) {
    var str = obj.value;
    var result = "";
    for (var i = 0; i < str.length; i++) {
        if (str.charCodeAt(i) == 12288) {
            result += String.fromCharCode(str.charCodeAt(i) - 12256);
            continue;
        }
        if (str.charCodeAt(i) > 65280 && str.charCodeAt(i) < 65375)
            result += String.fromCharCode(str.charCodeAt(i) - 65248);
        else
            result += String.fromCharCode(str.charCodeAt(i));
    }
    obj.value = result;
}

function ReturnPage() {
    window.dialogArguments.location.href = window.dialogArguments.location.href;
    self.close();
}
function ReturnNotDialogPage() {
    window.opener.location.href = window.opener.location.href;
    self.close();
}

function ReturnPreLocation() {
}

function FormatDateTime(date, format) {
    var o =
    {
        "M+": date.getMonth() + 1, //month
        "d+": date.getDate(),    //day
        "h+": date.getHours(),   //hour
        "m+": date.getMinutes(), //minute
        "s+": date.getSeconds(), //second
        "q+": Math.floor((date.getMonth() + 3) / 3),  //quarter
        "S": date.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
    }
    return format;
}

function OpenPicPage(url) {
    var ImageView = window.open('../Common/ImageVideo.html?imgUrl=' + url, 'ImageView', 'toolbar=no,location=no,menubar=no,scrollbars=no,resizable=yes,width=1240,height=700');
    ImageView.focus();
}
function OpenBigImg(url) {
    var ImageView = window.open('../Common/ImageGetBig.html?imgUrl=' + url);
    ImageView.focus();
}
function OpenPrintPageV(printxml) {
    var PrintView = window.open('../Common/DataTablePrintVertical.aspx?printxml=' + printxml, 'PrintView', 'toolbar=no,location=no,menubar=no,scrollbars=no,resizable=yes,width=1240,height=700');
    PrintView.focus();
}
function OpenPrintPageH(printxml) {
    var PrintView = window.open('../Common/DataTablePrintHorizontal.aspx?printxml=' + printxml, 'PrintView', 'toolbar=no,location=no,menubar=no,scrollbars=no,resizable=yes,width=1240,height=700');
    PrintView.focus();
}
function OpenPrintChartH(printxml) {
    var PrintView = window.open('../Common/ChartPrintHorizontal.aspx?printxml=' + printxml, 'PrintView', 'toolbar=no,location=no,menubar=no,scrollbars=no,resizable=yes,width=1240,height=700');
    PrintView.focus();
}
function OpenPrintChartV(printxml) {
    var PrintView = window.open('../Common/ChartPrintVertical.aspx?printxml=' + printxml, 'PrintView', 'toolbar=no,location=no,menubar=no,scrollbars=no,resizable=yes,width=1240,height=700');
    PrintView.focus();
}
function OpenPicModelPage(url) {
    var left = (screen.width - width) / 2;
    var top = (screen.height - height) / 2;
    var width = 1024;
    var height = 700;
    var u = '../Common/ImageVideo.html?imgUrl=' + url;
    window.showModalDialog(u, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px;dialogLeft:" + left + "px;dialogTop:" + top + "px;status:no;help:no; scroll:yes;resizable:yes;");
}
function OpenCheckModelPage() {
    var left = 0;
    var top = 0;
    var width = window.screen.availWidth;
    var height = window.screen.availHeight;
    var u = '../Peccancy/PeccancySingleCheck.aspx';
    window.showModalDialog(u, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px;dialogLeft:" + left + "px;dialogTop:" + top + "px;status:no;help:no; scroll:yes;resizable:yes;");
}
function OpenPeccancyOperateLog() {
    var left = 0;
    var top = 0;
    var width = window.screen.availWidth;
    var height = window.screen.availHeight;
    var u = '../Count/PeccancyOperateLog.aspx';
    window.showModalDialog(u, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px;dialogLeft:" + left + "px;dialogTop:" + top + "px;status:no;help:no; scroll:yes;resizable:yes;");
}
function OpenDevices() {
    var left = (window.screen.availWidth - 1200) / 2;
    var top = (window.screen.availHeight - 600) / 2;
    var width = 1200;//window.screen.availWidth;
    var height = 600; //window.screen.availHeight;
    var u = '/Template/WebDeviceShow.aspx';
    window.showModalDialog(u, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px;dialogLeft:" + left + "px;dialogTop:" + top + "px;status:no;help:no; scroll:yes;resizable:yes;");
}
function OpenRepeatCheckModelPage() {
    var left = 0;
    var top = 0;
    var width = window.screen.availWidth;
    var height = window.screen.availHeight;
    var u = '../Peccancy/PeccancySingleRepeatCheck.aspx';
    window.showModalDialog(u, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px;dialogLeft:" + left + "px;dialogTop:" + top + "px;status:no;help:no; scroll:yes;resizable:yes;");
}
function OpenAreaCheckModelPage() {
    var left = 0;
    var top = 0;
    var width = window.screen.availWidth;
    var height = window.screen.availHeight;
    var u = 'PeccancyAreaChecking.aspx';
    window.showModalDialog(u, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px;dialogLeft:" + left + "px;dialogTop:" + top + "px;status:no;help:no; scroll:yes;resizable:yes;");
}
function CloseWindow() {
    if (event.clientX <= 0 || event.clientY < 0) {
        ReturnPage();
    }
}
function CloseNotDialogWindow() {
    if (event.clientX <= 0 || event.clientY < 0) {
        ReturnNotDialogPage();
    }
}

/*
功能：保存cookies函数
参数：name，cookie名字；value，值
*/
function SetCookie(name, value) {
    var Days = 60;   //cookie 将被保存两个月
    var exp = new Date();  //获得当前时间
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);  //换成毫秒
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}
/*
功能：获取cookies函数
参数：name，cookie名字
*/
function getCookie(name) {
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (arr != null)
        return unescape(arr[2]);
    return null;
}
/*
功能：删除cookies函数
参数：name，cookie名字
*/

function delCookie(name) {
    var exp = new Date();  //当前时间
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null) document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}

function OpenQueryPage(url) {
    var left = (window.window.outerWidth - width) / 2;
    var top = (window.window.outerHeight - height) / 2;
    var width = window.screen.availWidth;
    var height = window.screen.availHeight;

    window.showModalDialog(url, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px;dialogLeft:" + left + "px;dialogTop:" + top + "px;status:no;help:no;location:no; scroll:yes;resizable:yes;");
}

function OpenPDFPage(url) {
    var left = (window.window.outerWidth - width) / 2;
    var top = (window.window.outerHeight - height) / 2;
    var width = window.screen.availWidth * 0.8;
    var height = window.screen.availHeight;
    window.showModalDialog(url, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px;dialogLeft:" + left + "px;dialogTop:" + top + "px;Status:no;help:no;location:no;scroll:yes;resizable:no;");
}

function OpenPassAnalysisPage(url) {
    var left = (window.window.outerWidth - width) / 2;
    var top = (window.window.outerHeight - height) / 2;
    var width = 1000;
    var height = 800;
    window.showModalDialog(url, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px;dialogLeft:" + left + "px;dialogTop:" + top + "px;Status:no;help:no;location:no;scroll:yes;resizable:no;");
}

function OpenImgCutPage(imgpath, imgwidth, imgheight) {
    var left = (window.window.outerWidth - width) / 2;
    var top = (window.window.outerHeight - height) / 2;
    var width = window.screen.availWidth;
    var height = window.screen.availHeight;
    var url = '../Common/ImageCut.aspx?imgpath=' + imgpath + '&imgheight=' + imgheight + '&imgwidth=' + imgwidth;
    window.showModalDialog(url, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px;dialogLeft:" + left + "px;dialogTop:" + top + "px;status:no;help:no; scroll:yes;resizable:yes;");
}

function OpenScreenPage(url) {
    var left = (window.window.outerWidth - width) / 2;
    var top = (window.window.outerHeight - height) / 2;
    var width = 600;
    var height = 300;
    window.showModalDialog(url, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px;dialogLeft:" + left + "px;dialogTop:" + top + "px;Status:no;help:no;location:no;scroll:yes;resizable:no;");
}