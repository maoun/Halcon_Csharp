function getLodop(oOBJECT, oEMBED) {
    /**************************
    本函数根据浏览器类型决定采用哪个对象作为控件实例：
    IE系列、IE内核系列的浏览器采用oOBJECT，
    其它浏览器(Firefox系列、Chrome系列、Opera系列、Safari系列等)采用oEMBED。
    **************************/
    var strHtml1 = "<br><font color='#FF00FF'>打印控件未安装!点击这里<a href='../Tools/ActivX/install_lodop.exe'>执行安装</a>,安装后请刷新页面或重新进入。</font>";
    var strHtml2 = "<br><font color='#FF00FF'>打印控件需要升级!点击这里<a href='../Tools/ActivX/install_lodop.exe'>执行升级</a>,升级后请重新进入。</font>";
    var strHtml3 = "<br><br><font color='#FF00FF'>注意：<br>1：如曾安装过Lodop旧版附件npActiveXPLugin,请在【工具】->【附加组件】->【扩展】中先卸它;<br>2：如果浏览器表现出停滞不动等异常，建议关闭其“plugin-container”(网上搜关闭方法)功能;</font>";
    var LODOP = oEMBED;
    try {
        if (navigator.appVersion.indexOf("MSIE") >= 0) { LODOP = oOBJECT; };
        if ((LODOP == null) || (typeof (LODOP.VERSION) == "undefined")) {
            if (navigator.userAgent.indexOf('Firefox') >= 0)
                download();
            if (navigator.appVersion.indexOf("MSIE") >= 0) {
                download();
            } else {
                download();
            }
            return LODOP;
        } else if (LODOP.VERSION < "6.0.3.1") {
            if (navigator.appVersion.indexOf("MSIE") >= 0) {
                download();
            } else {
                download();
            }
            return LODOP;
        };
        return LODOP;
    } catch (err) {
        Ext.Msg.alert("请重新安装打印控件");
        return LODOP;
    }
}

function download() {
    Ext.MessageBox.show({
        title: '打印控件安装?',
        msg: '本机尚未安装打印控件,是否下载安装？',
        buttons: Ext.MessageBox.YESNO,
        fn: function (btn) {
            if (btn == "yes") {
                window.location = "../Tools/ActivX/install_lodop.exe";
            }
        },
        animEl: 'Install',
        icon: Ext.MessageBox.QUESTION
    });
}

var OBJECT = '<div>';
OBJECT += '<object id="LODOP" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">';
OBJECT += '    <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0" pluginspage="install_lodop.exe"></embed>';
OBJECT += '</object>';
OBJECT += '</div>';
function CheckIsInstall() {
    try {
        //document.getElementById("PRINT").innerHTML = OBJECT;
        var LODOP = getLodop(document.getElementById('LODOP'), document.getElementById('LODOP_EM'));
        if ((LODOP != null) && (typeof (LODOP.VERSION) != "undefined")) {
            return true;
        }
    } catch (err) {
        alert("Error:本机未安装打印控件!");
        return false;
    }
}

