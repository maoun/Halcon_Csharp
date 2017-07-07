//var mapserver = "http://192.168.2.100/WEBGIS/"; //地图服务器地址 "http://www.jinyueyan.com/TMS"; //"http://192.168.2.200:9090/WMS";
var mapserver = "http://127.0.0.1:90/beijing/beijing/";
//var center = { x: 111.980951, y: 43.645524, zoom: 9 }; //中心点设置 x：经度 , y:纬度 , zoom:显示层级
//var center = { x: 116.39677, y: 39.91870, zoom: 12}; //北京市
var center = { x: 116.58182052004, y: 39.89482189899, zoom: 12 }; // 北京京通快速路
var province = "beijing";
var city = "beijing";
var cityName = "北京市";
var sLayer = true; //是否加载卫星地图
var MiniMap = true; //是否加载地图鹰眼
var isClose = false;
var infoBox;
var TEMP;
//城市列表
var cityList = [
    '<a href="#beijing" onclick=\'VNMAP.setCity(116.39677,39.91870,"beijing","beijing","北京市")\'>北京市</a>'
//'<a href="#liupanshui" onclick=\'VNMAP.setCity(104.818455,26.578668,"guizhou","liupanshui","六盘水市")\'>六盘水市</a>',
// '<a href="#erlian" onclick=\'VNMAP.setCity(111.980951,43.645524,"neimeng","erlian","二连浩特市")\'>二连浩特市</a>'
 ];