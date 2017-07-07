/**
 *Description：EzServerClient配置文档设置
 */
 /*
 *Description:存放配置文档中设置的全局类
 */
if(typeof EzServerClient == "undefined" || !EzServerClient)var EzServerClient = {};
 /*
 *Description:存放配置文档中设置的全局变量
 */
EzServerClient.GlobeParams = {};


/**********************************************************************************/
/*************************以下为EzServer Client配置参数部分*************************/
/**********************************************************************************/
/**
 *参数说明：设置图图片请求地址，三维数组第二层中：[][0]指图片服务器名称;[][1])指图片服务器地址;[][2]指叠加在[][1]之下的图片服务器地址（[][2]可省略）
 *参数类型：{[][(2|3)][]String} String类型的n*(2|3)*n的三维数组
 *取值范围：无限制
 *默认值：无
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.MapSrcURL = [
	["矢量地图", ["http://171.18.18.131/PGIS_S_TileMapServer/Maps/default"]],
	["影像地图", ["http://171.18.18.131/PGIS_S_TileMapServer/Maps/Trans"]],
	["矢量叠加地图", ["http://171.18.18.131/PGIS_S_TileMapServer/Maps/Image"]]];
/*********************
 *上边变量中的[][1]和[][2]可以引用多个图片来源地址，用图片引用于集群，以减轻图片服务器负担，如EzServerClient.GlobeParams.MapSrcURL= [["矢量影像",["http://192.168.10.3/EzServer","http://192.168.10.4/EzServer"]]];
**********************/
 /**
 *参数说明：设置版权信息
 *参数类型：{String}
 *取值范围：无限制
 *默认值：""
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.Copyright = "&copy; 2010 PGIS";	

/**
 *参数说明：设置版本信息
 *参数类型：{Float}
 *取值范围：无限制
 *默认值：无
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.Version = 0.3;


/**
 *参数说明：设置全图显示时地图显示范围
 *参数类型：{[4]Float} 长度为4的Float类型数组
 *取值范围：无限制117.81445,37.43652
 *默认值：无
 *参数举例：如下所示117.06835,36.65527,118.53906,38.26464
 */
EzServerClient.GlobeParams.MapFullExtent = [106.19696, 36.05517, 106.37298, 35.97534];
//EzServerClient.GlobeParams.MapFullExtent = [13225108.33007,4493646.95507,13458090.67382,4740966.67382];

/**
 *参数说明：设置地图初始显示级别
 *参数类型：{Float}
 *取值范围：无限制
 *默认值：无
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.MapInitLevel = 15;

/**
 *参数说明：设置地图显示的最大级别
 *参数类型：{Float}
 *取值范围：[0,20]
 *默认值：无
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.MapMaxLevel = 19;

/**
 *参数说明：设置地图级别的偏移量（可为正负数：当前级别的实际级别=当前级别+EzServerClient.GlobeParams.ZoomOffset）
 *参数类型：{Float}
 *取值范围：无限制
 *默认值：0
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.ZoomOffset = 0;

/**
 *参数说明：设置来源地图图片大小（根据图片来源服务器提供图片的大小决定）
 *参数类型：{Int}
 *取值范围：128|256
 *默认值：无
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.MapUnitPixels = 256;

/**
 *参数说明：设置地图坐标系类型：经纬度坐标系为1；地方坐标时为EzServerClient.GlobeParams.MapConvertScale所设定的值
 *参数类型：{Int}
 *取值范围：无限制
 *默认值：无
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.MapCoordinateType = 1;
//EzServerClient.GlobeParams.MapCoordinateType = 114699;

/**
 *参数说明：设置地方坐标系缩放比例（根据切图时所给定的值设定此值）
 *参数类型：{Int}
 *取值范围：无限制
 *默认值：114699
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.MapConvertScale = 114699;

/**
 *参数说明：设置地方坐标系X轴偏移量（此时，根据切图时所给定的值设定此值）
 *参数类型：{Int}
 *取值范围：无限制
 *默认值：无
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.MapConvertOffsetX = 0;
/**
 *参数说明：设置地方坐标系Y轴偏移量（此时，根据切图时所给定的值设定此值）
 *参数类型：{Int}
 *取值范围：无限制
 *默认值：无
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.MapConvertOffsetY = 0;


/**
 *参数说明：设置地图图片是否叠加
 *参数类型：{Boolean}
 *取值范围：true|false
 *默认值：无
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.IsOverlay = true;

/**
 *参数说明：设置请求地图图片来源是否通过代理
 *参数类型：{Boolean}
 *取值范围：true|false
 *默认值：无
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.MapProx = false;

/**
 *参数说明：设置EzServerClient引用地址
 *参数类型：{String}
 *取值范围：无
 *默认值：无
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.EzServerClientURL = "http://171.18.18.131/PGIS_S_TileMap";

/**
 *参数说明：如果需要调用空间矢量数据服务，则需要设置以下默认EzMapService的引用地址，有关EzMapService服务的介绍请参照EzMapService的有关介绍（可选配置）
 *参数类型：{String}
 *取值范围：无
 *默认值：无
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.EzMapServiceURL = "http://171.18.18.131/PGIS_S_TileMapServer";	

/**
 *参数说明：如果需要调用地理处理服务，则需要设置以下默认EzGeographicProcessingService引用地址，有关EzGeographicProcessingService服务的介绍请参照EzGeographicProcessingService的有关介绍（可选配置）
 *参数类型：{String}
 *取值范围：无
 *默认值：无
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.EzGeoPSURL = "http://171.18.18.131:10000/EzGeographicProcessingService";	

/**
 *参数说明：设置地图比例尺级别是降序还是升序的，以及采用的是什么版本地图
 *参数类型：{Int}
 *取值范围：{0,1,2,3}
 *默认值：2
 *参数举例：如下所示
 */
EzServerClient.GlobeParams.ZoomLevelSequence = 2;
//EzServerClient.GlobeParams.ZoomLevelSequence = 0;// 切图等级，客户端升序，服务器端升序
//EzServerClient.GlobeParams.ZoomLevelSequence = 1;// 切图等级，客户端降序，服务器端升序
//EzServerClient.GlobeParams.ZoomLevelSequence = 2;// 切图等级，客户端降序，服务器端降序
//EzServerClient.GlobeParams.ZoomLevelSequence = 3;// 切图等级，客户端升序，服务器端降序
/**********************************************************************************/
/************************************参数配置结束***********************************/
/**********************************************************************************/


/**
 *字符集配置，默认引入"字符集为GB2312"，如果想引入"字符集为UTF-8"，将"字符集为UTF-8"打开，注销"字符集为GB2312"部分
 */
/**********************************字符集为GB2312***********************************/
//引入EzServerClient6.js脚本
document.writeln("<script type='text/javascript' charset='GB2312' src='" + EzServerClient.GlobeParams.EzServerClientURL + "/js/EzServerClient6.js'></script>");
// 引用EzServer.css样式表
document.writeln("<LINK type='text/css' charset='GB2312' rel='stylesheet' href='" + EzServerClient.GlobeParams.EzServerClientURL + "/css/EzServer.css'>");
// 如果配置了地理处理服务EzGeographicProcessingService，通过以下引入相关脚本
//document.writeln("<script type='text/javascript' charset='GB2312' src='" + EzServerClient.GlobeParams.EzGeoPSURL + "/ezgeops_js/EzGeoPS.js'></script>");

/**********************************字符集为UTF-8***********************************/
/*
// 引入EzServerClient6.js脚本
document.writeln("<script type='text/javascript' charset='UTF-8' src='" + EzServerClient.GlobeParams.EzServerClientURL + "/js_UTF-8/EzServerClient6.js'></script>");
// 引用EzServer.css样式表
document.writeln("<LINK charset='GB2312' href='" + EzServerClient.GlobeParams.EzServerClientURL + "/css/EzServer_UTF-8.css' type='text/css' rel='stylesheet'>");
// 如果配置了地理处理服务EzGeographicProcessingService，通过以下引入相关脚本
document.writeln("<script type='text/javascript' charset='GB2312' src='" + EzServerClient.GlobeParams.EzGeoPSURL + "/ezgeops_js/EzGeoPS.js'></script>");
*/