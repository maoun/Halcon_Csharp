var zDialog = null;
var zDialogForUpdate = null;
var MC_table = "GIS371000000000.MAP_CORRECT";
var MC_userid="admin";
var MC_map = _MapApp;
var MC_add_coords ="";
var MC_que_markers=new Array();
var MC_add_markers=new Array();
function dialog_show(){
	if(zDialog==null){
		zDialog = new Dialog();
		zDialog.Width = 300;
		zDialog.Height = 200;
		zDialog.Title = "地图纠错";
		zDialog.MessageTitle = "欢迎使用地图纠错功能";
		zDialog.OKEvent = mc_add_summit;//点击确定后调用的方法
		zDialog.CancelEvent= quitDialog;
		zDialog.Drag=true;//允许拖拽
		this.Modal = false;//没有遮罩层
		var html = '';
	    html += '<div align="center"> ';
	    html += '   <table border="0" width="'+zDialog.Width+'px" height="'+zDialog.Height+'px" cellpadding="0" cellspacing="0" align="center"><tr>';
	    html += '       <td align="right" width="70px">';
	    html += '       	地点名称：</td><td>';
	    html += '       	<input id="placename" type="text" style="width:200px;"/>';
	    html += '   	</td></tr>';
	    html += '       <tr><td align="right">';
	    html += '       	选择地点：</td><td>';
	    html += '       	<input type="hidden" id="coords1"/><input type="hidden" id="coords2"/><a href="#" style="padding-left:10px;" onclick="mc_add_selectPoint();">拾取位置</a>';
	    html += '   	</td></tr>';
	  	html += '       <tr><td align="right" >';
	    html += '       	错误信息：</td><td>';
	    html += '       	<textarea id="errorinfo" rows="8" cols="30"></textarea>';
	    html += '   	</td></tr></table>';
	    html += '</div>';
		zDialog.InnerHtml=html;
	}
	zDialog.show();
}
function mc_add_selectPoint() {
	MC_add_coords =""
	mc_clearMarkers();
    _MapApp.changeDragMode('drawPoint',document.getElementById("coords1"),document.getElementById("coords2"),mc_add_callback);
}
function mc_add_callback(str){
	MC_add_coords = str;
	var _pPoint = str.split(",");
	var _icon = new Icon();
	_icon.width = 32;
	_icon.height = 32;
	_icon.topOffset = 16;
	_icon.leftOffset= 8;
	var _title = new Title("地点名称",12,2,"宋体","#000000","#e4f2fc","#00B2FF","1");
	var _marker= new Marker(new Point(_pPoint[0],_pPoint[1]),_icon);
	_marker.div.style.filter="progid:DXImageTransform.Microsoft.AlphaImageLoader(src='mapCorrect/img/mc-point.png')";
	_marker.div.style.border="0px";
	_MapApp.addOverlay(_marker);
	MC_add_markers.push(_marker);
}
function mc_add_summit(){
	if (MC_add_coords == '') {
		alert('请先在地图上确定该地点的位置，再提交信息');
		return;
	}
	var placename = document.getElementById("placename").value;
	if (placename == '') {
		alert('请输入地点名称，再提交信息');
		return;
	}
	var errorinfo = document.getElementById("errorinfo").value;
	var sel_type = "point";
	var today=new Date();
   	var summit_date=today.getYear()+"-"+(today.getMonth()+1)+"-"+today.getDate()+" "+today.getHours()+":"+today.getMinutes()+":"+today.getSeconds();
	var status = "未处理";
	var pEdit = new EditObject("add", "POINT", MC_table);
	pEdit.addField("PLACENAME", placename);
	pEdit.addField("ERRORINFO", errorinfo);
	//pEdit.addField("SEL_TYPE", sel_type);
	pEdit.addField("USERID", MC_userid);
	//pEdit.addField("SUMMIT_DATE", summit_date);
	pEdit.addField("STATUS", status);
	pEdit.addField("shape", MC_add_coords);
	_MapApp.edit(pEdit,function(response){
		if (response){
			alert("纠错信息入库成功！");
			document.getElementById("placename").value='';
			document.getElementById("errorinfo").value='';
		}else{ 
			alert("纠错信息入库失败！");}
	});
	MC_add_coords="";
	//mc_clearMarkers();
}
function quitDialog(){
	zDialog.close();
	mc_clearMarkers();
	_MapApp.changeDragMode("pan");
}
function quitUpdateDialog(){
	zDialogForUpdate.close();
	mc_clearMarkers();
	_MapApp.changeDragMode("pan");
}
function mc_clearMarkers(){
	for(var i=0;i<MC_add_markers.length;i++){
		_MapApp.removeOverlay(MC_add_markers[i]);
	}
	MC_add_markers=[];
}

function mc_query(){
	for(var i = 0; i<MC_que_markers.length;i++){
		_MapApp.removeOverlay(MC_que_markers[i]);
	}
	MC_que_markers = [];
	_MapApp.clearOverlays();
	var _subFields="OBJECTID:1;PLACENAME:地点名称;ERRORINFO:错误信息;STATUS:纠错状态;SUMMIT_DATE:提交时间";
	var pQuery=new QueryObject();
	pQuery.queryType=6;
	pQuery.tableName=MC_table;
	pQuery.where = " userid  = '"+MC_userid+"' ";
	pQuery.dispField="PLACENAME";
	pQuery.featurelimit = 1000;
	pQuery.beginrecord = 1;
	pQuery.addSubFields(_subFields);
	var img="<img title='正在读去数据...' src='mapCorrect/img/mc-loader.gif' width='32' heigh='32' style='padding:20px;margin:100px' valign='middle'/>";
	leftSlip.msg.innerHTML = img;
	leftSlip.slipOut();
	_MapApp.clearLayers(); 
	_MapApp.query(pQuery,ParseQueryResultForMapCorrect);
}

function ParseQueryResultForMapCorrect(){
	var results = _MapApp.getQueryResult();
	var MC_List = new MC_PageList(results,leftSlip.msg);
	MC_List.creatContent();
}

//分页开始
function MC_PageList(results, wrapper) {
	this.totalCount = results[0].maxRecord<0?0:results[0].maxRecord;
	this.dataset = results[0].features;
	this.wrapper = wrapper;
	this.offset = 10;
	this.page = 1;
	this.tpages = 0;
	this.wrapper.innerHTML = "";
}
MC_PageList.prototype = {
	creatContent : function() {
		var _dataset = this.dataset;
		var page = this.page;
		var lastIndex = this.totalCount;
		var startIndex = (page-1)* this.offset<0?0:(page-1)* this.offset;
		var endIndex = page*this.offset>lastIndex?lastIndex:page*this.offset;
		this.tpages = this.getTotalPage();
		var content = "";
		content +="<div style='width:100%'>"
				+ "	<table width='200px' border='0' cellpadding='2' cellspacing='3' align='center' bordercolorlight='#FF0000'><tr>"
			//	+ "		<td colSpan='2' style='color:blue;font-size: 13px;'>我的纠错:</td></tr><tr>"
				+ "		<td style='font-size: 12px;' colSpan='2'>"
				+ "			<b>共搜索到<font color=red>"+this.totalCount+"</font>项结果&nbsp;当前"+(startIndex+1)+"~"+endIndex+"</b></td>"
				+ "	</tr>";
		for (var i=startIndex;i<endIndex;i++) {
			var feature = _dataset[i];
			if (!feature) {
				break;
			}
			var objid = feature.getFieldValue('OBJECTID');
			var placename = feature.getFieldValue('PLACENAME');
			var einfo = feature.getFieldValue('ERRORINFO');
			var sdate = feature.getFieldValue('SUMMIT_DATE');
			
			var _pPoint = new Point(feature.point.x,feature.point.y);
			var _icon = new Icon();
			_icon.width = 32;
			_icon.height = 32;
			_icon.topOffset = -(_icon.height/2);
			_icon.leftOffset=0;
			var _title= new Title(feature.dispname, 12, 2, "宋体", "blue", "#e4f2fc", "#00B2FF", "1");
			var _marker= new Marker(_pPoint,_icon,_title);
			_marker.div.style.filter="progid:DXImageTransform.Microsoft.AlphaImageLoader(src='mapCorrect/img/mc-point.png')";
			_marker.div.style.border="0px";
			_MapApp.addOverlay(_marker);
			MC_que_markers.push(_marker);
			
			var strMsg="<table width='200px' border='0' cellpadding='2' cellspacing='0' align='center' bordercolorlight='#FF0000'>" ;
			strMsg +=	"<tr><td nowrap='nowrap' width='20%' align='right'>地点名称：</td><td style='font-weight: bold;'>"+
						"		<font color='blue'>"+placename+"</font>" +
						"	</td></tr><tr>" +
						"	<td nowrap='nowrap' width='20%' align='right'>错误信息：</td><td style='font-weight: bold;'>"+
						"		<font color='blue'>"+einfo+"</font>" +
						"	</td></tr><tr>" +
						"	<td nowrap='nowrap' width='20%' align='right'>提交时间：</td><td style='font-weight: bold;'>"+
						"		<font color='blue'>"+sdate+"</font>" +
						"	</td></tr><tr>" +
						"	<td nowrap='nowrap' width='20%' align='right'></td><td align='center'>"+
						"		<a href='javascript:void(0);' style = 'CURSOR: hand;' onclick='mc_delete(\""+objid+"\",\""+i+"\");'><font color='red'>删除信息</font</a>" +
						"		<a href='javascript:void(0);' style = 'CURSOR: hand;' onclick='mc_update(\""+objid+"\",\""+i+"\",\""+placename+"\",\""+einfo+"\",\""+feature.point.x+"\",\""+feature.point.y+"\");'><font color='red'>修改信息</font</a>" +
						"	</td>" +
						"</tr>" 		
			strMsg +="</table>"
			//var detialInfo = feature.toHTML();
			mc_addListenerForObj(_marker,strMsg);
			var span = "";
			span += "<tr>"
					+ "<td style='font-size: 12px;' align='right' width='20%'>"
					+ (i + 1)
					+ ".</td>"
					+ "<td width='85%'>";
			span += "<a style='CURSOR: hand' style='font-size: 12;' onclick='mc_locateObj(\""
					+ i
					+ "\");'>"
					+ placename
					+ "</a></td></tr>";
			content += span;
		}
		content += "</table></div>";
		this.wrapper.innerHTML=content;
		this.createPager();
	},
	createPager : function() {
		var pager = document.createElement("div");
		pager.className = "pager";
		var prev = document.createElement("span");
		prev["onclick"] = this.pageGo(-1);
		prev.style.cursor = "pointer";
		prev.title = "\u4e0a\u4e00\u9875";
		prev.innerHTML = "<img id='MC_pre' src='mapCorrect/img/mc-previous.gif'>";
		var page = document.createElement("span");
		//page.innerHTML = "&nbsp;[" + this.page + "/" + this.tpages + "]&nbsp;";
		page.innerHTML = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
		var next = document.createElement("span");
		next["onclick"] = this.pageGo(1);
		next.innerHTML = "<img id='MC_next' src='mapCorrect/img/mc-next.gif'>";
		next.style.cursor = "pointer";
		next.title = "\u4e0b\u4e00\u9875";
		pager.appendChild(prev);
		pager.appendChild(page);
		pager.appendChild(next);
		this.wrapper.appendChild(pager);
	},
	pageGo : function(dir) {
		var me = this;
		return function() {
			var newPage = me.page + dir;
			if (newPage >= 1 && newPage <= me.tpages) {
				_MapApp.clear();
				me.page = me.page + dir;
				me.creatContent();
			}
		};
	},
	getTotalPage : function() {
		var len = this.totalCount;
		if (len <= this.offset) {
			return 1;
		} else {
			return Math.floor((len + this.offset - 1) / this.offset);
		}
	}
};
//分页结束

//单击显示气泡框方法
function mc_addListenerForObj(obj,message){
	obj.addListener("click",function (){obj.openInfoWindowHtml(message);});
}
function mc_locateObj(num){
	MC_que_markers[num].onclick();
	var point = MC_que_markers[num].getPoint();
	_MapApp.centerAtLatLng(point);
}
function mc_delete(objid,num){
	if (!confirm('确认删除此信息?'))
        return;
    if (objid === null)
        return;
	var pEdit=new EditObject("del","POINT",MC_table);
    pEdit.where="OBJECTID='"+objid+"'";
    _MapApp.edit(pEdit,function(result){
    	if (result) {
    		MC_que_markers[num].closeInfoWindowHtml();
            alert("删除成功！");
           	mc_query();
        } else
            alert("删除失败！");
    });

}
var UPDATE_OBJID = "";
function mc_update(objid,num,placename,einfo,x,y){
	UPDATE_OBJID = objid;
    if (objid === null)
        return;
    if(zDialogForUpdate==null){
		zDialogForUpdate = new Dialog();
		zDialogForUpdate.Width = 300;
		zDialogForUpdate.Height = 200;
		zDialogForUpdate.Title = "地图纠错";
		zDialogForUpdate.MessageTitle = "修改纠错信息";
		zDialogForUpdate.OKEvent = mc_update_summit;//点击确定后调用的方法
		zDialogForUpdate.CancelEvent= quitUpdateDialog;
		zDialogForUpdate.Drag=true;//允许拖拽
		this.Modal = false;//没有遮罩层
	}
    var html = '';
    html += '<div align="center"> ';
    html += '   <table border="0" width="'+zDialogForUpdate.Width+'px" height="'+zDialogForUpdate.Height+'px" cellpadding="0" cellspacing="0" align="center"><tr>';
    html += '       <td align="right" width="70px">';
    html += '       	地点名称：</td><td>';
    html += '       	<input id="placename" type="text" value="'+placename+'" style="width:200px;"/>';
    html += '   	</td></tr>';
    html += '       <tr><td align="right">';
    html += '       	选择地点：</td><td>';
    html += '       	<input type="hidden" id="coords1"/><input type="hidden" id="coords2"/><a href="#" style="padding-left:10px;" onclick="mc_add_selectPoint();">重新拾点</a>';
    html += '   	</td></tr>';
  	html += '       <tr><td align="right" >';
    html += '       	错误信息：</td><td>';
    html += '       	<textarea id="errorinfo" rows="8" cols="30">'+einfo+'</textarea>';
    html += '   	</td></tr></table>';
    html += '</div>';
	zDialogForUpdate.InnerHtml=html;
	zDialogForUpdate.show();
	MC_add_coords = x+","+y;
}

function mc_update_summit(){
	var placename = document.getElementById("placename").value;
	if (placename == '') {
		alert('请输入地点名称，再提交信息');
		return;
	}
	var errorinfo = document.getElementById("errorinfo").value;
	var today=new Date();
   	var summit_date=today.getYear()+"-"+(today.getMonth()+1)+"-"+today.getDate()+" "+today.getHours()+":"+today.getMinutes()+":"+today.getSeconds();
	var pEdit=new EditObject("update","POINT",MC_table);
	pEdit.addField("PLACENAME", placename);
	pEdit.addField("ERRORINFO", errorinfo);
	pEdit.addField("USERID", MC_userid);
	pEdit.addField("SUMMIT_DATE", summit_date);
	pEdit.addField("STATUS", "已修改");
	if (MC_add_coords != '') {
		pEdit.addField("shape", MC_add_coords);
	}
    pEdit.where="OBJECTID='"+UPDATE_OBJID+"'";
    _MapApp.edit(pEdit,function(result){
    	if (result) {
            alert("修改成功！");
           	mc_query();
        } else
            alert("修改失败！");
    });
	MC_add_coords="";
	quitUpdateDialog();
	//mc_clearMarkers();
}


function showNotOnlineGps(){
	var _subFields = "GPSID:电台编号;CALLNO:呼号;CARNO:号牌号码;";
	var pQuery = new QueryObject();
	pQuery.queryType=6;
	pQuery.tableName = "pgisapp.t_gps_info";
	pQuery.where = " realtime<sysdate-4/24 ";
	pQuery.dispField="callno";
	pQuery.featurelimit = 10000;
	pQuery.beginrecord = 1;
	pQuery.addSubFields(_subFields);
	_MapApp.clearLayers(); 
	_MapApp.query(pQuery,getQueryResultForNotOnlineGps);
}

function getQueryResultForNotOnlineGps(){
	var results = _MapApp.getQueryResult();
	var features = results[0].features;
	alert(features.length);
	for (var i=0; i<features.length; i++) {
			var feature = features[i];
			if (!feature) {
				break;
			}
			var gpsid = feature.getFieldValue('GPSID');
			var callno = feature.getFieldValue('CALLNO');
			var carno = feature.getFieldValue('CARNO');
			//var cartype = feature.getFieldValue('CARTYPE');
			//var ssdw = feature.getFieldValue('REMARK1');
			
			var _pPoint = new Point(feature.point.x,feature.point.y);
			var _icon = new Icon();
			_icon.width = 32;
			_icon.height = 32;
			_icon.topOffset = -(_icon.height/2);
			_icon.leftOffset=0;
			var _title= new Title(feature.dispname, 12, 2, "宋体", "blue", "#e4f2fc", "#00B2FF", "1");
			var _marker= new Marker(_pPoint,_icon,_title);
			_marker.div.style.filter="progid:DXImageTransform.Microsoft.AlphaImageLoader(src='mapCorrect/img/mc-point.png')";
			_marker.div.style.border="0px";
			_MapApp.addOverlay(_marker);
/*
			MC_que_markers.push(_marker);
			
			var strMsg="<table width='200px' border='0' cellpadding='2' cellspacing='0' align='center' bordercolorlight='#FF0000'>" ;
			strMsg +=	"<tr><td nowrap='nowrap' width='20%' align='right'>电台编号：</td><td style='font-weight: bold;'>"+
						"		<font color='blue'>"+gpsid+"</font>" +
						"	</td></tr><tr>" +
						"	<td nowrap='nowrap' width='20%' align='right'>呼  号：</td><td style='font-weight: bold;'>"+
						"		<font color='blue'>"+callno+"</font>" +
						"	</td></tr><tr>" +
						"	<td nowrap='nowrap' width='20%' align='right'>号牌号码：</td><td style='font-weight: bold;'>"+
						"		<font color='blue'>"+carno+"</font>" +
						"	</td></tr><tr>" +
						"	<td nowrap='nowrap' width='20%' align='right'>警种类型：</td><td style='font-weight: bold;'>"+
						"		<font color='blue'>"+cartype+"</font>" +
						"	</td></tr><tr>" +
						"	<td nowrap='nowrap' width='20%' align='right'>所属单位：</td><td style='font-weight: bold;'>"+
						"		<font color='blue'>"+ssdw+"</font>" +
						"	</td></tr>"; 
			strMsg +="</table>"
			mc_addListenerForObj(_marker,strMsg);
			*/
		}
}


