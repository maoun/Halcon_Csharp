//Copyright 2012 Advanced Software Engineering Ltd
ase_0="2.0.0";ase_1=navigator.userAgent.toLowerCase();ase_2=(ase_1.indexOf('gecko')!=-1&&ase_1.indexOf('safari')==-1);ase_3=(ase_1.indexOf('konqueror')!=-1);ase_4=(ase_1.indexOf('chrome')!=-1);ase_5=(ase_1.indexOf('safari')!=-1)&&!ase_4;ase_6=(ase_1.indexOf('opera')!=-1);ase_7=(ase_1.indexOf('msie')!=-1&&!ase_6&&(ase_1.indexOf('webtv')==-1));function ase_8(){return(new RegExp("msie ([0-9]{1,}[\.0-9]{0,})").exec(ase_1)!=null)?parseFloat(RegExp.$1):6.0;}
function ase_9(id){return document.getElementById(id);}
function ase_a(a,b){if(a.indexOf)return a.indexOf(b);for(var i=0;i<a.length;++i){if(a[i]==b)return i;}
return-1;}
function ase_b(s){return s.replace(/^\s+|\s+$/g,'');}
function ase_c(ld){var d=document;var de=d.documentElement;return((de&&de[ld])||d.body[ld]);}
function ase_d(e,lh,ld,li,lj){if(e&&(typeof(e[li])!='undefined'))return e[li];if(window.event)return window.event[lh]+ase_c(ld);if(e)return e[lh]+window[lj];else return null;}
function ase_e(lk){return ase_7?document.body[lk]+document.documentElement[lk]:0 }
function ase_f(e){return ase_d(e,"clientX","scrollLeft","pageX","scrollX")-ase_e("clientLeft");}
function ase_g(e){return ase_d(e,"clientY","scrollTop","pageY","scrollY")-ase_e("clientTop");}
function ase_h(e){if(ase_7&&window.event)return window.event.button;else return(e.which==3)?2:e.which;}
function ase_i(ll,lm){return ll?ll[lm]+ase_i(ll.offsetParent,lm):0;}
function ase_j(ll,lm){if((!ase_6)&&ll&&(ll!=document.body)&&(ll!=document.documentElement))return ll[lm]+ase_j(ll.parentNode,lm);else return 0;}
function ase_k(ll){if(ll.getBoundingClientRect)return ll.getBoundingClientRect().left+ase_c("scrollLeft")-ase_e("clientLeft");else return ase_i(ll,"offsetLeft")-ase_j(ll,"scrollLeft")+(ll.offsetWidth-ll.clientWidth)/2;}
function ase_l(ll){if(ll.getBoundingClientRect)return ll.getBoundingClientRect().top+ase_c("scrollTop")-ase_e("clientTop");else return ase_i(ll,"offsetTop")-ase_j(ll,"scrollTop")+(ll.offsetHeight-ll.clientHeight)/2;}
function ase_m(ll,ln,lo,lp){if(ll.addEventListener)ll.addEventListener(ln,lo,lp);else ll.attachEvent("on"+ln,lo);}
function ase_n(ll,ln,lo,lp){if(ll.removeEventListener)ll.removeEventListener(ln,lo,lp);else ll.detachEvent("on"+ln,lo);}
function ase_o(lq,lr){return lq+((lq.indexOf('?')!=-1)?'&':'?')+lr;}
function ase_p(ls,lt,lu){var re=new RegExp(lt,'g');return ls.replace(re,lu);}
function ase_q(lw){var lx=document.scripts;if(((!lx)||(!lx.length))&&document.getElementsByTagName)lx=document.getElementsByTagName("script");if(lx){for(var i=0;i<lx.length;++i){var ly=lx[i].src;if(!ly)continue;var lz=ly.indexOf(lw);if(lz!=-1)return ly.substring(0,lz);}
}
return "";}
function ase_r(c){if((((c>>24)&0xff)==0xff)&&((c&0xffffff)==0))return null;var l11=(c&0x00ffffff).toString(16);if(l11.length<6)l11="000000".substring(l11.length)+l11;return "#"+l11;}
function ase_s(l21,l31,l41){var l51=l21.length-1;var l61=0;while(l61<=l51){var l71=Math.floor((l61+l51)/2);var l81=l41(l21[l71],l31);if(l81>0){l51=l71-1;}else if(l81<0){l61=l71+1;}else{return l71;}
}
return~l61;}
function ase_t(l91,la1,lb1){var lc1=l91.indexOf(la1);var ld1=l91.indexOf(lb1);if((lc1<0)||(ld1<=lc1))return '';else return l91.substring(lc1+la1.length,ld1);}
function ase_u(l91,le1){var lz=l91.indexOf(le1);return(lz>=0)?l91.substring(0,lz):l91;}
function ase_v(l91,le1){var lz=l91.indexOf(le1);return(lz>=0)?l91.substring(lz+1,l91.length):"";}
function ase_w(v){return ase_p(ase_p(v,'&','&amp;'),'"','&#34;');}
function ase_x(){if(typeof XMLHttpRequest!='undefined')return new XMLHttpRequest();
/*@cc_on
@if(@_jscript_version>=5)
try{return new ActiveXObject("Msxml2.XMLHTTP");}catch(e){}
try{return new ActiveXObject("Microsoft.XMLHTTP");}catch(e){}
@end
@*/
}
function ase_y(lq,lg1,lh1){var r=ase_x();if(r){r.onreadystatechange=function(){if(r.readyState==4){var status=-9999;eval("try { status = r.status; } catch(e) {}");if(status==-9999)return;if((r.status==200)||(r.status==304))lg1(r.responseText);else if(lh1)lh1(r.status,r.responseText);window.setTimeout(function(){r.onreadystatechange=function(){};r.abort();},1);}
}
if((lq.length<1000)||(ase_6&&!r.setRequestHeader)){r.open('GET',lq,true);r.send(null);}
else {r.open('POST',ase_u(lq,"?"),true);r.setRequestHeader("Content-Type","application/x-www-form-urlencoded");r.send(ase_v(lq,"?"));}
}
return r;}
function _jcv(v){this.lr=v.id;v.li2=v.useMap;this.lv1=v.style.cursor;this.lx1();this.lo={};var lk1=v.id+"_JsChartViewerState";this.l02=ase_9(lk1);if(!this.l02){var p=v.parentNode||v.parentElement;var s=this.l02=document.createElement("HIDDEN");s.id=s.name=lk1;s.value=this.lg1();p.insertBefore(s,v);}
else {this.decodeState(this.l02.value);this.lq4();}
this.ly1();if(!ase_7)this.l71(this.l22());if(this.ln)this.partialUpdate();}
_jcvp=_jcv.prototype;_jcv.l82=function(ln1){var lo1=window.cdjcv_path;if(typeof lo1=="undefined")lo1=ase_q("cdjcv.js");else if((lo1.length>0)&&("/=".indexOf(lo1.charAt(lo1.length-1))==-1))lo1+='/';return lo1+ln1;}
_jcv.addEventListener=ase_m;_jcv.removeEventListener=ase_n;_jcv.Horizontal=0;_jcv.Vertical=1;_jcv.HorizontalVertical=2;_jcv.Default=0;_jcv.Scroll=2;_jcv.ZoomIn=3;_jcv.ZoomOut=4;_jcv.BottomLeft=1;_jcv.Bottom=2;_jcv.BottomCenter=2;_jcv.BottomRight=3;_jcv.Left=4;_jcv.Center=5;_jcv.Right=6;_jcv.TopLeft=7;_jcv.Top=8;_jcv.TopCenter=8;_jcv.TopRight=9;_jcv.Transparent=0xff000000;_jcv.msgContainer='<div style="font:bold 8pt Verdana;padding:3px 8px 3px 8px;border:1px solid #000000;background-color:#FFCCCC;color:#000000">%msg</div>';_jcv.okButton='<center>[<a href="javascript:%closeScript"> OK </a>]</center>';_jcv.xButton='[<a href="javascript:%closeScript"> X </a>]';_jcv.shortErrorMsg='Error %errCode accessing server'+_jcv.okButton;_jcv.serverErrorMsg=_jcv.xButton+'<div style="font:bold 15pt Arial;">Error %errCode accessing server</div><hr>%errMsg';_jcv.updatingMsg='<div style="padding:0px 8px 0px 6px;background-color:#FFFFCC;color:#000000;border:1px solid #000000"><table><tr><td><img src="'+_jcv.l82('wait.gif')+'"></td><td style="font:bold 8pt Verdana;">Updating</td></tr></table></div>';_jcv.lp1=new Array("l0","l1","l2","l3","l4","l5","l6","l7","l8","l9","la","lb","lc","ld","le","lf","lg","lh","li","lj","lk","ll","lm","ln","lo","lp","lq","grabCursor");_jcv.get=function(id){var imgObj=ase_9(id);if(!imgObj)return null;if(!imgObj._jcv)imgObj._jcv=new _jcv(imgObj);return imgObj._jcv;}
_jcvp.getId=function(){return this.lr;}
_jcvp.lz1=function(){return ase_9(this.lr);}
_jcvp.lx1=function(){this.lz1().l41=function(e,id){var l11;l11=this._jcv["onImg"+id](e);if(this["_jcvOn"+id+"Chain"])l11=this["_jcvOn"+id+"Chain"](e);return l11;};this.lz1()._jcvOnMouseMoveChain=this.lz1().onmousemove;this.lz1()._jcvOnMouseUpChain=this.lz1().onmouseup;this.lz1()._jcvOnMouseDownChain=this.lz1().onmousedown;this.lz1()._jcvOnMouseOutChain=this.lz1().onmouseout;var lq1=this.lr;this.lz1().onmousemove=function(e){return ase_9(lq1).l41(e,"MouseMove");}
this.lz1().onmousedown=function(e){return ase_9(lq1).l41(e,"MouseDown");}
this.lz1().onmouseup=function(e){return ase_9(lq1).l41(e,"MouseUp");}
this.lz1().onmouseout=function(e){return ase_9(lq1).l41(e,"MouseOut");}
}
_jcvp.lw2=function(x){return x-ase_k(this.lz1());}
_jcvp.lx2=function(y){return y-ase_l(this.lz1());}
_jcvp.lv2=function(w){return w;}
_jcvp.lu2=function(h){return h;}
_jcvp.ls2=function(x){return x+ase_k(this.lz1());}
_jcvp.lt2=function(y){return y+ase_l(this.lz1());}
_jcvp.lr2=function(w){return w;}
_jcvp.lq2=function(h){return h;}
_jcvp.setCustomAttr=function(k,v){this.lo[k]=v;this.lk2();}
_jcvp.getCustomAttr=function(k){return this.lo[k];}
_jcvp.getValueAtViewPort=function(id,lw1,lx1){var ly1=parseFloat(this.getCustomAttr(id+"_min"));var lz1=parseFloat(this.getCustomAttr(id+"_max"));if(!lx1)return ly1+(lz1-ly1)*lw1;else return ly1*Math.pow(lz1/ly1,lw1);}
_jcvp.getViewPortAtValue=function(id,l02,lx1){var ly1=parseFloat(this.getCustomAttr(id+"_min"));var lz1=parseFloat(this.getCustomAttr(id+"_max"));if(!lx1)return(l02-ly1)/(lz1-ly1);else return Math.log(l02/ly1)/Math.log(lz1/ly1);}
_jcvp.l4=0;_jcvp.l5=0;_jcvp.l6=1;_jcvp.l7=1;_jcvp.setViewPortLeft=function(x){this.l4=x;this.lk2();}
_jcvp.getViewPortLeft=function(){return this.l4;}
_jcvp.setViewPortTop=function(y){this.l5=y;this.lk2();}
_jcvp.getViewPortTop=function(){return this.l5;}
_jcvp.setViewPortWidth=function(w){this.l6=w;this.lk2();}
_jcvp.getViewPortWidth=function(){return this.l6;}
_jcvp.setViewPortHeight=function(h){this.l7=h;this.lk2();}
_jcvp.getViewPortHeight=function(){return this.l7;}
_jcvp.l0=-1;_jcvp.l1=-1;_jcvp.l2=-1;_jcvp.l3=-1;_jcvp.lp4=false;_jcvp.lq4=function(){if((!this.lp4)&&(this.l2>0))return;var l12=1E308;var l22=l12;var l32=-l12;var l42=l32;for(var i=0;i<this.getChartCount();++i){var c=this.getChart(i);var p=c.getPlotArea();l12=Math.min(p.getLeftX()+c.getAbsOffsetX(),l12);l22=Math.min(p.getTopY()+c.getAbsOffsetY(),l22);l32=Math.max(p.getRightX()+c.getAbsOffsetX(),l32);l42=Math.max(p.getBottomY()+c.getAbsOffsetY(),l42);}
var l52=(l12<=l32);this.l0=l52?l12:-1;this.l1=l52?l22:-1;this.l2=l52?l32-l12:-1;this.l3=l52?l42-l22:-1;this.lp4=l52;}
_jcvp.l61=function(x,y){x=this.lw2(x);y=this.lx2(y);return(this.l0<=x)&&(x<=this.l0+this.l2)&&(this.l1<=y)&&(y<=this.l1+this.l3);}
_jcvp.msgBox=function(l62,l72){var m=this.l81;if(!m&&l62){var d=document;{m=d.createElement("DIV");m.style.position='absolute';m.style.visibility='hidden';d.body.appendChild(m);}
if(m)this.l81=m;}
if(m){window.clearTimeout(m.l91);var s=m.style;if(l62){if(l72)m.l91=window.setTimeout(function(){s.visibility='hidden';},Math.abs(l72));if(l72<0)l62+=_jcv.okButton;if(l62.substring(0,4).toLowerCase()!="<div")l62=ase_p(_jcv.msgContainer,'%msg',l62);var l92="_jcv.get('"+this.lr+"').msgBox();";m.innerHTML=ase_p(l62,'%closeScript',l92);s.visibility='visible';s.left=this.ls2(Math.max(0,this.l0+(this.l2-m.offsetWidth)/2))+"px";s.top=this.lt2(Math.max(0,this.l1+(this.l3-m.offsetHeight)/2))+"px";}
else {s.visibility='hidden';}
}
}
_jcvp.l8=2;_jcvp.l9="#000000";_jcvp.setSelectionBorderWidth=function(w){this.l8=w;this.lk2();}
_jcvp.getSelectionBorderWidth=function(){return this.l8;}
_jcvp.setSelectionBorderColor=function(c){this.l9=c;this.lk2();}
_jcvp.getSelectionBorderColor=function(){return this.l9;}
_jcvp.l14=function(id){return ase_9(this.getId()+"_"+id);}
_jcvp.lc2=function(id){id=this.getId()+"_"+id;var l11=ase_9(id);if(!l11){var d=document;{l11=d.createElement("DIV");l11.id=id;var s=l11.style;s.position="absolute";s.visibility="hidden";s.backgroundColor="#000000";s.width="1px";s.height="1px";s.cursor=this.lz1().style.cursor;d.body.appendChild(l11);if(l11.clientHeight!=1)l11.innerHTML="<img width='1px' height='1px' />";}
if(!this.li4)this.li4=[];this.li4[this.li4.length]=id;}
return l11;}
_jcvp.lq1=function(x,y,la2,lb2){var lc2=this.lc2("leftLine").style;var ld2=this.lc2("rightLine").style;var le2=this.lc2("topLine").style;var lf2=this.lc2("bottomLine").style;lc2.left=le2.left=lf2.left=x+"px";lc2.top=ld2.top=le2.top=y+"px";le2.width=lf2.width=la2+"px";lf2.top=(y+lb2-this.l8+1)+"px";lc2.height=ld2.height=lb2+"px";ld2.left=(x+la2-this.l8+1)+"px";lc2.width=ld2.width=le2.height=lf2.height=this.l8+"px";lc2.backgroundColor=ld2.backgroundColor=le2.backgroundColor=lf2.backgroundColor=this.l9;}
_jcvp.lr1=function(b){var lg2=["leftLine","rightLine","topLine","bottomLine"];for(var i=0;i<lg2.length;++i){var ll=this.l14(lg2[i]);if(ll)ll.style.visibility=b?"visible":"hidden";}
}
_jcvp.la=_jcv.Default;_jcvp.lb=_jcv.Horizontal;_jcvp.lc=_jcv.Horizontal;_jcvp.ld=2;_jcvp.le=0.5;_jcvp.lf=0.01;_jcvp.lg=1;_jcvp.lh=0.01;_jcvp.li=1;_jcvp.getMouseUsage=function(){return this.la;}
_jcvp.setMouseUsage=function(lh2){this.la=lh2;this.lk2();}
_jcvp.getScrollDirection=function(){return this.lb;}
_jcvp.setScrollDirection=function(li2){this.lb=li2;this.lk2();}
_jcvp.getZoomDirection=function(){return this.lc;}
_jcvp.setZoomDirection=function(li2){this.lc=li2;this.lk2();}
_jcvp.getZoomInRatio=function(){return this.ld;}
_jcvp.setZoomInRatio=function(lw1){if(lw1>0)this.ld=lw1;this.lk2();}
_jcvp.getZoomOutRatio=function(){return this.le;}
_jcvp.setZoomOutRatio=function(lw1){if(lw1>0)this.le=lw1;this.lk2();}
_jcvp.getZoomInWidthLimit=function(){return this.lf;}
_jcvp.setZoomInWidthLimit=function(lw1){this.lf=lw1;this.lk2();}
_jcvp.getZoomOutWidthLimit=function(){return this.lg;}
_jcvp.setZoomOutWidthLimit=function(lw1){this.lg=lw1;this.lk2();}
_jcvp.getZoomInHeightLimit=function(){return this.lh;}
_jcvp.setZoomInHeightLimit=function(lw1){this.lh=lw1;this.lk2();}
_jcvp.getZoomOutHeightLimit=function(){return this.li;}
_jcvp.setZoomOutHeightLimit=function(lw1){this.li=lw1;this.lk2();}
_jcvp.lh1=function(){return((this.lc!=_jcv.Vertical)&&(this.l6>this.lf))||((this.lc!=_jcv.Horizontal)&&(this.l7>this.lh));}
_jcvp.li1=function(){return((this.lc!=_jcv.Vertical)&&(this.l6<this.lg))||((this.lc!=_jcv.Horizontal)&&(this.l7<this.li));}
_jcvp.ly2=-1;_jcvp.lz2=-1;_jcvp.lj=5;_jcvp.getMinimumDrag=function(){return this.lj;}
_jcvp.setMinimumDrag=function(lj2){this.lj=lj2;this.lk2();}
_jcvp.la1=function(e,d){var lk2=Math.abs(ase_f(e)-this.ly2);var ll2=Math.abs(ase_g(e)-this.lz2);switch(d){case _jcv.Horizontal:return lk2>=this.lj;case _jcv.Vertical:return ll2>=this.lj;default:return(lk2>=this.lj)||(ll2>=this.lj);}
}
_jcvp.onImgMouseDown=function(e){if(this.l24){window.clearTimeout(this.l94);this.l24=false;}
this.lm3("chartmousedown",e);if(this.ls1)return;if(this.l61(ase_f(e),ase_g(e))&&(ase_h(e)==1)){if(e&&e.preventDefault&&(this.la!=_jcv.Default))e.preventDefault();this.lj2(true);this.ls(e);}
}
_jcvp.onImgMouseMove=function(e){if(this.l24){window.clearTimeout(this.l94);this.l24=false;}
this.lw(e);var lm2=this.li3;this.li3=this.l61(ase_f(e),ase_g(e));if(this.li3)this.lu(e);else if(lm2)this.l11(e);if(this.ls1)return;this.lf3=this.l72||this.li3;if(this.lf3&&this.l72){if((this.la!=_jcv.Default)&&this.lz1().useMap)this.lz1().useMap=null;this.lt(e);}
this.l71(this.l51(e));return this.la==_jcv.Default;}
_jcvp.onImgMouseUp=function(e){this.lm3("chartmouseup",e);if(this.ls1)return;if(this.l72&&(ase_h(e)==1)){this.lj2(false);this.lv(e);}
if(!this.ls1)this.l71(this.l51(e));}
_jcvp.onImgMouseOut=function(e){if(!this.l24){var id=this.lr;var x=this.lw2(ase_f(e));var y=this.lx2(ase_g(e));this.l94=window.setTimeout(function(){_jcv.get(id).ly(e,x,y)},1);this.l24=true;}
}
_jcvp.ly=function(e,x,y){if(this.li3){this.li3=false;this.l11(e);}
this.lx(e);}
_jcvp.lj2=function(b){var imgObj=this.lz1();if(b){if(((this.la==_jcv.ZoomIn)||(this.la==_jcv.ZoomOut))&&imgObj.useMap)imgObj.useMap=null;if(imgObj.setCapture)imgObj.setCapture();else {if(!window._jcvOnMouseUpChain)window._jcvOnMouseUpChain=window.onmouseup;if(!window._jcvOnMouseMoveChain)window._jcvOnMouseMoveChain=window.onmousemove;window.onmouseup=imgObj.onmouseup;window.onmousemove=imgObj.onmousemove;}
}
else {if(imgObj.useMap!=imgObj.li2)imgObj.useMap=imgObj.li2;if(imgObj.setCapture)imgObj.releaseCapture();else {window.onmouseup=window._jcvOnMouseUpChain;window.onmousemove=window._jcvOnMouseMoveChain;window._jcvOnMouseUpChain=null;window._jcvOnMouseMoveChain=null;}
}
this.l72=b;}
_jcvp.setZoomInCursor=function(ln2){this.lk=ln2;this.lk2();}
_jcvp.getZoomInCursor=function(){return this.lk;}
_jcvp.setZoomOutCursor=function(ln2){this.ll=ln2;this.lk2();}
_jcvp.getZoomOutCursor=function(){return this.ll;}
_jcvp.setNoZoomCursor=function(ln2){this.lq=ln2;this.lk2();}
_jcvp.getNoZoomCursor=function(){return this.lq;}
_jcvp.setScrollCursor=function(ln2){this.lm=ln2;this.lk2();}
_jcvp.getScrollCursor=function(){return this.lm;}
_jcvp.lj4=function(c,x,y){var lq=_jcv.l82(c);if(ase_7)return "url('"+lq+"'), auto";else return "url('"+lq+"') "+x+" "+y+", url('"+lq+"'), auto";}
_jcvp.l22=function(){switch(this.la){case _jcv.ZoomIn:if(this.lh1()){if(this.lk)return this.lk;else return ase_2?"-moz-zoom-in":this.lj4('zoomin.cur',15,15);}
else {if(this.lq)return this.lq;else return this.lj4('nozoom.cur',15,15);}
case _jcv.ZoomOut:if(this.li1()){if(this.ll)return this.ll;else return ase_2?"-moz-zoom-out":this.lj4('zoomout.cur',15,15);}
else {if(this.lq)return this.lq;else return this.lj4('nozoom.cur',15,15);}
default:return "";}
}
_jcvp.l51=function(e){if(this.ls1)return "wait";if(this.l62){if(this.lm)return this.lm;if(!e&&!window.event)return "";switch(this.lb){case _jcv.Horizontal:return(ase_f(e)>=this.ly2)?"e-resize":"w-resize";case _jcv.Vertical:return(ase_g(e)>=this.lz2)?"s-resize":"n-resize";default:return "move";}
}
if(this.lf3)return this.l22();else return "";}
_jcvp.l71=function(lo2){if(lo2!=this.lv1){this.lv1=lo2;this.lz1().style.cursor=new String(lo2);if(this.li4){for(var i=0;i<this.li4.length;++i){var ll=ase_9(this.li4[i]);if(ll)ll.style.cursor=new String(lo2);}
}
}
}
_jcvp.lm3=function(lp2,e){this.lp3=this.lw2(ase_f(e));this.lq3=this.lx2(ase_g(e));this.applyHandlers(lp2,e);}
_jcvp.getChartMouseX=function(){return this.lp3;}
_jcvp.getChartMouseY=function(){return this.lq3;}
_jcvp.getPlotAreaMouseX=function(){var l11=this.lr3;return(typeof l11=="undefined")?this.l0+this.l2:l11;}
_jcvp.getPlotAreaMouseY=function(){var l11=this.ls3;return(typeof l11=="undefined")?this.l1+this.l3:l11;}
_jcvp.lb4=16;_jcvp.lc4=16;_jcvp.ld4=16;_jcvp.la4=16;_jcvp.setPlotAreaMouseMargin=function(lq2,lr2,ls2,lt2){if(arguments.length==1)lr2=ls2=lt2=lq2;this.lb4=lq2;this.lc4=lr2;this.ld4=ls2;this.la4=lt2;}
_jcvp.setPlotAreaMouseMargin2=_jcvp.setPlotAreaMouseMargin;_jcvp.lw=function(e){this.lm3("mousemovechart",e);var l22=this.l1-this.ld4;var l42=this.l1+this.l3+this.la4;var l12=this.l0-this.lb4;var l32=this.l0+this.l2+this.lc4;var x=this.getChartMouseX();var y=this.getChartMouseY();var lu2=(y>=l22)&&(y<=l42);var lv2=(x>=l12)&&(x<=l32);if(lu2&&lv2)this.lz(e);else this.l01(e);}
_jcvp.lx=function(e){this.l01(e);this.applyHandlers("mouseoutchart",e);}
_jcvp.lz=function(e){this.lg3=true;this.lr3=Math.max(this.l0,Math.min(this.lp3,this.l0+this.l2));this.ls3=Math.max(this.l1,Math.min(this.lq3,this.l1+this.l3));this.applyHandlers("mousemoveplotarea",e);}
_jcvp.l01=function(e){if(this.lg3){this.lg3=false;this.applyHandlers("mouseoutplotarea",e);}
}
_jcvp.isMouseOnPlotArea=function(){return this.lg3;}
_jcvp.l74=function(id){var l11=this.l14(id);if(!l11){l11=this.lc2(id);if(!this.l23)this.l23=[];this.l23.push(id);this.l04(l11);}
return l11;}
_jcvp.l04=function(ll){ll.onmousedown=this.lz1().onmousedown;ll.onmousemove=this.lz1().onmousemove;ll.onmouseout=this.lz1().onmouseout;ll.onmouseover=this.lz1().onmousemove;}
_jcvp.drawHLine=function(id,y,x1,x2,ly2){if(x1>x2){var lz2=x1;x1=x2;x2=lz2;}
return this.la3(id,y,x1,x2,ly2);}
_jcvp.la3=function(id,y,x1,x2,ly2){var l03=x2-x1+1;var ll=this.l74(id);var s=ll.style;if((l03<=0)||!ly2)s.visibility="hidden";else {s.width=this.lr2(l03)+"px";s.height="0px";s.borderTop=ly2;s.backgroundColor="";s.left=this.ls2(x1)+"px";s.top=(this.lt2(y)-Math.floor(ll.clientTop/2))+"px";s.visibility="visible";}
return ll;}
_jcvp.drawVLine=function(id,x,y1,y2,ly2){if(y1>y2){var lz2=y1;y1=y2;y2=lz2;}
return this.lb3(id,x,y1,y2,ly2);}
_jcvp.lb3=function(id,x,y1,y2,ly2){var l03=y2-y1+1;var ll=this.l74(id);var s=ll.style;if((l03<=0)||!ly2)s.visibility="hidden";else {s.width="0px";s.height=this.lq2(l03)+"px";s.borderLeft=ly2;s.backgroundColor="";s.left=(this.ls2(x)-Math.floor(ll.clientLeft/2))+"px";s.top=this.lt2(y1)+"px";s.visibility="visible";}
return ll;}
_jcvp.showCrossHair=function(x,y,ly2,lq2,lr2,ls2,lt2,l33){if(!ly2)ly2="1px dotted black";if(!lq2)lq2=0;if(!lr2)lr2=0;if(!ls2)ls2=0;if(!lt2)lt2=0;if(typeof l33=="undefined")l33=5;var l43=this.l0+1-lq2;var l53=l43+this.l2-1+lr2;var l63=this.l1+1-ls2;var l73=l63+this.l3-1+lt2;x=Math.max(this.l0,Math.min(this.l0+this.l2,x));y=Math.max(this.l1,Math.min(this.l1+this.l3,y));if(l33){this.la3("hLeftLine",y,l43,Math.min(l53,x-l33),ly2);this.la3("hRightLine",y,Math.max(l43,x+l33),l53,ly2);this.lb3("vTopLine",x,l63,Math.min(l73,y-l33),ly2);this.lb3("vBottomLine",x,Math.max(l63,y+l33),l73,ly2);}
else {this.la3("hLeftLine",y,l43,l53,ly2);this.la3("hRightLine",0,0,0,null);this.lb3("vTopLine",x,l63,l73,ly2);this.lb3("vBottomLine",0,0,0,null);}
}
_jcvp.hideCrossHair=function(){var lg2=["hLeftLine","hRightLine","vTopLine","vBottomLine"];for(var i=0;i<lg2.length;++i)this.hideObj(lg2[i]);}
_jcvp.le4=function(x,la2,l83,l93){if(l83)return x-Math.floor(la2/2);else if(l93)return x-la2;else return x;}
_jcv.l44=function(la3){return(la3==_jcv.Top)||(la3==_jcv.Center)||(la3==_jcv.Bottom);}
_jcv.l54=function(la3){return(la3==_jcv.TopRight)||(la3==_jcv.Right)||(la3==_jcv.BottomRight);}
_jcv.l64=function(la3){return(la3==_jcv.Left)||(la3==_jcv.Center)||(la3==_jcv.Right);}
_jcv.l34=function(la3){return(la3==_jcv.Bottom)||(la3==_jcv.BottomLeft)||(la3==_jcv.BottomRight);}
_jcv.setStyle=function(s){var lb3=s.split(';');for(var i=0;i<lb3.length;++i){var av=ase_b(lb3[i]);var lz=av.indexOf(":");if(lz==-1)continue;var ld3=ase_b(av.substring(0,lz)).split('-');for(var j=1;j<ld3.length;++j)ld3[j]=ld3[j].charAt(0).toUpperCase()+ld3[j].substring(1);var a=ld3.join('');if(a)this.style[a]=ase_b(av.substring(lz+1));}
}
_jcvp.showTextBox=function(id,x,y,la3,l91,lf3){this.removeAutoHide(id);var lg3=this.l14(id);var ll=this.l74(id);if(!lg3){ll.setStyle=_jcv.setStyle;ll.setStyle("background-color:;width:;height:");}
if(lf3)ll.setStyle(lf3);if(l91!="[unchanged]")ll.innerHTML=l91;var s=ll.style;s.visibility="visible";s.left=this.ls2(this.le4(x,ll.offsetWidth,_jcv.l44(la3),_jcv.l54(la3)))+"px";s.top=this.lt2(this.le4(y,ll.offsetHeight,_jcv.l64(la3),_jcv.l34(la3)))+"px";return ll;}
_jcvp.hideObj=function(id,lh3){if((!lh3)&&(id.toLowerCase()=="all")){var li3=this.l23;if(li3){for(var i=0;i<li3.length;++i)this.hideObj(li3[i],1);}
}
else if(id.toLowerCase()=="crosshair"){this.hideCrossHair();}
else {var ll=this.l14(id);if(ll)ll.style.visibility="hidden";}
}
_jcvp.setAutoHide=function(id,lj3){if(id.toLowerCase()=="all")id="all";lj3=lj3.toLowerCase();if(!this.ln3)this.ln3={};if(!this.ln3[lj3])this.ln3[lj3]={};this.ln3[lj3][id]=1;}
_jcvp.removeAutoHide=function(id,lj3){if(this.ln3){var lk3=this.ln3[lj3];if(lk3)delete lk3[id];}
}
_jcvp.ll3=function(lj3){if(this.ln3){var lk3=this.ln3[lj3];if(lk3){if(lk3["all"])this.hideObj("all");else {for(var id in lk3)this.hideObj(id);}
this.ln3[lj3]={};}
}
}
_jcv.lg4=function(a,id){if((!a)||(!a.length))return null;if(!id)id=0;var ll3=typeof id=="number";var l11=ll3?a[id]:_jcv.lh4(a,id);if(!l11)l11=ll3?_jcv.lh4(a,id):a[id];return l11;}
_jcv.lh4=function(a,id){for(var i=0;i<a.length;++i){var ll=a[i];if(ll&&(ll.id==id))return ll;}
return null;}
_jcv.l83=["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"];_jcv.l93=["Sun","Mon","Tue","Wed","Thu","Fri","Sat"];_jcv.l73=["am","pm"];_jcv.ly3=function(s,c,n){return(s.indexOf(c)!=-1)?s.replace(new RegExp(c+c,'g'),(n<10)?"0"+n:n).replace(new RegExp(c,'g'),n):s;}
_jcv.ln4=function(s,c,r){return(s.indexOf(c)!=-1)?s.replace(new RegExp(c,'g'),r):s;}
_jcvp.htmlRect=function(w,h,ln3,lo3){lo3=lo3?"border:"+lo3+";":"";ln3=ln3?"background-color:"+ln3+";":"";var lq=(ase_7&&(ase_8()<8))?_jcv.l82('spacer.gif'):'data:image/gif;base64,R0lGODlhAQABAID/AMDAwAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==';var l11="<img width='"+w+"px' height='"+h+"px' src='"+lq+"' style='"+lo3+ln3+"'";if(ase_5||ase_4){var lp3=["onmousedown","onmousemove","onmouseout"];for(var i=0;i<lp3.length;++i)l11+=" "+lp3[i]+"='document.getElementById(\""+this.getId()+"\")."+lp3[i]+"(event);'";}
return l11+" />";}
_jcvp.getChart=function(id){var lq3=window[this.lr+"_chartModel"];if(!lq3)return null;var lr3=(lq3.charts||id);if(lr3)lq3=_jcv.lg4(lq3.charts,id);if(!lq3)return null;if(!lq3.obj)lq3.obj=lr3?new _jcvxyc(lq3.x,lq3.y,lq3.c,this):new _jcvxyc(0,0,lq3,this);return lq3.obj;}
_jcvp.getChartByZ=_jcvp.getChart;_jcvp.getChartCount=function(id){var l11=0;while(this.getChart(l11))++l11;return l11;}
_jcv.lo3=function(a,b){if(a==null)return(b==null)?0:-1;else return(b==null)?1:(a-b);}
function _jcvxyc(ls3,lt3,lq3,lu3){this.offsetX=ls3;this.offsetY=lt3;this.data=lq3;this.lo4=lu3;}
_jcvxycp=_jcvxyc.prototype;_jcvxycp.getAbsOffsetX=function(){return this.offsetX;}
_jcvxycp.getAbsOffsetY=function(){return this.offsetY;}
_jcvxycp.getPlotArea=function(){if(!this.pa)this.pa=new _jcvpa(this.data.plotarea);return this.pa;}
_jcvxycp.getLayer=function(i){if(!this.lk3){this.lk3={};var lw3=null;for(var j=0;j<this.data.layers.length;++j){var lx3=this.data.layers[j];this.lk3[lx3.id]=j;}
}
var ly3=this.lk3[i];return this.l84((ly3==null)?null:this.data.layers[ly3]);}
_jcvxycp.getLayerByZ=function(i){return this.l84(_jcv.lg4(this.data.layers,i));}
_jcvxycp.l84=function(lq3){if(!lq3)return null;if(!lq3.obj)lq3.obj=new _jcvl(this,lq3);return lq3.obj;}
_jcvxycp.getLayerCount=function(){var lz3=this.data.layers;return lz3?lz3.length:0;}
_jcvxycp.getNearestXValue=function(l04){var l11=null;var l14=null;var l24=this.xAxis();var lw3=null;for(var i=0;lw3=this.getLayer(i);++i){var l34=lw3.getNearestXValue(l04);if(l34==null)continue;var l44=Math.abs(l24.lc3(l34)-l04);if((l14==null)||(l14>l44)){l11=l34;l14=l44;}
}
return l11;}
_jcvxycp.getAxis=function(i){var lq3=_jcv.lg4(this.data.axes,i);if(!lq3)return null;if(!lq3.obj)lq3.obj=new _jcva(this,lq3);return lq3.obj;}
_jcvxycp.xAxis=function(){return this.getAxis(0);}
_jcvxycp.xAxis2=function(){return this.getAxis(1);}
_jcvxycp.yAxis=function(){return this.getAxis(2);}
_jcvxycp.yAxis2=function(){return this.getAxis(3);}
_jcvxycp.getXCoor=function(v){return this.xAxis().ld3(v);}
_jcvxycp.getYCoor=function(v,l24){if(!l24)l24=this.yAxis();return l24.ld3(v);}
_jcvxycp.getXValue=function(l04){return this.xAxis().le3(l04);}
_jcvxycp.getYValue=function(l54,l24){if(!l24)l24=this.yAxis();return l24.le3(l54);}
_jcvxycp.setNumberFormat=function(l64,l74,l84){l64=l64||"~";l74=l74||"~";l84=l84||"~";this.data.l63=l64.substring(0,1)+l74.substring(0,1)+l84.substring(0,1);}
_jcvxycp.setMonthNames=function(l94){this.data.l43=l94;}
_jcvxycp.setWeekDayNames=function(l94){this.data.l53=l94;}
_jcvxycp.setAMPM=function(am,pm){this.data.l33=[am,pm];}
_jcvxycp.lx3=function(lc4,d){if(d==null)return;if(!d.getMonth)d=_jcv.NTime(d);var ld4=this.data.l43||_jcv.l83;var le4=this.data.l53||_jcv.l93;var lf4=this.data.l33||_jcv.l73;var f=lc4.replace(/mmm/g,'#p').replace(/M/g,'#M').replace(/w/g,'#w').replace(/a/g,'#a');for(var i=0;(i<4)&&(f.indexOf('y')!=-1);++i)f=f.replace(new RegExp("yyyy".substring(0,4-i),'g'),(i!=3)?new String(d.getFullYear()).substring(i,4):d.getFullYear());f=_jcv.ly3(f,'m',d.getMonth()+1);f=_jcv.ly3(f,'d',d.getDate());f=_jcv.ly3(f,'h',(f.indexOf('a')!=-1)?(d.getHours()+11)%12+1:d.getHours());f=_jcv.ly3(f,'n',d.getMinutes());f=_jcv.ly3(f,'s',d.getSeconds());for(var j=0;(j<3)&&(f.indexOf('f')!=-1);++j)f=f.replace(new RegExp("fff".substring(0,3-j),'g'),new String(d.getMilliseconds()+1000).substring(1,4-j));for(var k=0;(k<3)&&(f.indexOf('#M')!=-1);++k)f=f.replace(new RegExp("#M#M#M".substring(0,6-2*k),'g'),ld4[d.getMonth()].toUpperCase().substring(0,3-k));f=_jcv.ln4(f,"#p",ld4[d.getMonth()]);f=_jcv.ln4(f,"#w",le4[d.getDay()]);f=_jcv.ln4(f,"#a",lf4[(d.getHours()>=12)?1:0]);return f;}
_jcvxycp.lf4=function(c,lh4){if(!c||(c=='?'))return lh4;else return(c=='~')?"":c;}
_jcvxycp.lz3=function(lc4,d){if(d==null)return null;var li4=this.data.l63||"~.-";var re=(/^([eEgGpP]?)(\d*)(.?)(.?)(.?)(.?)/).exec(lc4);var lj4=re[1];var lk4=re[2]?parseInt(re[2]):-1;var l64=this.lf4(re[3],li4.charAt(0));var l74=this.lf4(re[4],li4.charAt(1));var l84=this.lf4(re[5],li4.charAt(2));var ll4=this.lf4(re[6],"");var lm4=false;var ln4=0;var lo4=d<0;if(lo4)d=-d;if(lj4!=""){ln4=(d>0)?Math.floor(Math.log(d)/Math.LN10):0;if((lj4=='g')||(lj4=='G')){lm4=true;var lp4=(lk4<0)?4:lk4;if((ln4>=lp4)||(ln4<-3)){lj4=(lj4=='g')?'e':'E';--lk4;}
else {if(ln4>=0)lk4=lp4-1-ln4;else lk4=Math.max(4,lk4);}
}
if((lj4=='e')||(lj4=='E')){d/=Math.pow(10,ln4);if(lk4<0)lk4=3;}
if((lj4=='p')||(lj4=='P')){lm4=true;if(lk4<0)lk4=3;lk4=Math.max(0,Math.min(lk4,lk4-ln4));}
}
var lq4=Math.floor(d);var lr4=d-lq4;if(lr4>0.999999999999){lr4=0;++lq4;}
else if(lr4<0.000000000001){lr4=0;}
else {if((lk4<0)&&(lq4>=100000000*lr4))lk4=0;if(lk4>=0)d+=Math.pow(10,-lk4)*0.5;else d+=0.0000005;if(lq4!=Math.floor(d)){lq4=Math.floor(d);lr4=0;}
}
if((lq4==10)&&((lj4=='e')||(lj4=='E'))){++ln4;lq4=1;lr4/=10;}
if((lq4==0)&&(lr4==0))lo4=false;var ls4=ll4+lq4;for(var lt4=ls4.length-3;lt4>((0!=ll4)?0:1);lt4-=3)ls4=ls4.substring(0,lt4)+l64+ls4.substring(lt4);if(((lk4>0)&&(!lm4||(lr4>0)))||((lk4<0)&&(lr4>0))){var lu4=(lk4>0)?lk4:6;var lv4=Math.round(Math.pow(10,lu4));var lw4=new String(Math.round(lr4*lv4)%lv4);while(lw4.length<lu4)lw4='0'+lw4;if((lk4<0)||lm4)lw4=lw4.replace(/0*$/,'');if(lw4.length>0)ls4+=(l74==""?'.':l74)+lw4;}
if(lo4&&ls4.match(/[1-9]/))ls4=l84+ls4;if((lj4=='e')||(lj4=='E'))ls4+=lj4+(((ln4>=0)&&!lm4)?'+':'')+ln4;return ls4;}
_jcvxycp.lw3=function(lc4,d){if(!lc4)lc4="";var lx4=lc4?lc4.charAt(0):'\0';if(("eEgGpP".indexOf(lx4)==-1)&&(((lx4>='A')&&(lx4<='Z'))||((lx4>='a')&&(lx4<='z'))))return this.lx3(lc4,d);else return this.lz3(lc4,d);}
_jcvxycp.formatValue=function(l02,lc4){if((!lc4)||(lc4.indexOf('{')==-1))return this.lw3(lc4,l02);else return lc4.replace(new RegExp("[{]value(?:|([^}]*))?[}]",'g'),this.lw3("$1",l02));}
function _jcvpa(lq3){this.data=lq3;}
_jcvpap=_jcvpa.prototype;_jcvpap.getLeftX=function(){return this.data.x;}
_jcvpap.getTopY=function(){return this.data.y;}
_jcvpap.getWidth=function(){return this.data.w;}
_jcvpap.getHeight=function(){return this.data.h;}
_jcvpap.getRightX=function(){return this.getLeftX()+this.getWidth();}
_jcvpap.getBottomY=function(){return this.getTopY()+this.getHeight();}
function _jcva(lz4,lq3){this.lm4=lz4;this.data=lq3;}
_jcvap=_jcva.prototype;_jcvap.getX=function(){return this.data.x;}
_jcvap.getY=function(){return this.data.y;}
_jcvap.getLength=function(){return this.data.l;}
_jcvap.getAlignment=function(){return this.data.dir;}
_jcvap.le3=function(l15){if(l15==null)return null;var lw1=(l15-this.data.minC)/(this.data.maxC-this.data.minC);if(this.data.st!=4)return lw1*(this.data.maxV-this.data.minV)+this.data.minV;else return Math.pow(this.data.maxV/this.data.minV,lw1)*this.data.minV;}
_jcvap.lc3=function(l02){if(l02==null)return null;if(l02.getFullYear)l02=_jcv.CTime(l02);if(this.data.st!=4)return(l02-this.data.minV)/(this.data.maxV-this.data.minV)*(this.data.maxC-this.data.minC)+this.data.minC;else return Math.log(l02/this.data.minV)/Math.log(this.data.maxV/this.data.minV)*(this.data.maxC-this.data.minC)+this.data.minC;}
_jcvap.ld3=function(l02){var l11=this.lc3(l02);return(l11==null)?null:Math.round(l11);}
_jcvap.getMinValue=function(){return this.data.minV;}
_jcvap.getMaxValue=function(){return this.data.maxV;}
_jcvap.getLabel=function(v){var l25=this.data.labels[v];return l25?l25[0]:null;}
_jcvap.getFormattedLabel=function(v,lc4){var l35=this.data.labels[v];if(l35){if((v=l35[1])==null)return l35[0];}
if(!lc4&&(v!=null)&&(v>=56770934400)&&(v<=69393715200))lc4=(v%86400==0)?"mmm dd, yyyy":"mmm dd, yyyy hh:nn:ss";return this.lm4.formatValue(v,lc4);}
function _jcvl(lz4,lx3){this.lm4=lz4;this.lj3=lx3;}
_jcvlp=_jcvl.prototype;_jcvlp.getDataSet=function(i){var lq3=this.lj3.dataSets[i];if(!lq3)return null;if(!lq3.obj){lq3.i=i;lq3.obj=new _jcvd(this,lq3);}
return lq3.obj;}
_jcvlp.getDataSetById=function(id){var l55=this.lj3.dataSets;for(var i=0;i<l55.length;++i){if(l55[i].id==id)return this.getDataSet(i);}
return null;}
_jcvlp.getDataSetByZ=function(i){return this.getDataSet(this.lj3.dataSets.length-i-1);}
_jcvlp.getDataSetCount=function(){var l55=this.lj3.dataSets;return l55?l55.length:0;}
_jcvlp.getXPosition=function(i){if(this.lj3.xValues)return this.lj3.xValues[i];if((this.lj3.minX!=null)&&(this.lj3.maxX!=null)){if(this.lj3.dss==1)return(i==0)?(this.lj3.maxX+this.lj3.minX)/2:null;else return((this.lj3.maxX-this.lj3.minX)*i)/(this.lj3.dss-1)+this.lj3.minX;}
return i;}
_jcvlp.getXCoor=function(){if(!this.lj3.ll4){var l65=0;for(var j=0;j<this.lj3.dataSets.length;++j)l65=Math.max(l65,this.lj3.dataSets[j].data.length);var l24=this.lm4.xAxis();var ly1=Math.min(l24.getMinValue(),l24.getMaxValue());var lz1=Math.max(l24.getMinValue(),l24.getMaxValue());var l75=[];for(var i=0;i<l65;++i){var l85=this.getXPosition(i);if((l85==null)||(l85>lz1)||(l85<ly1))l75[i]=null;else l75[i]=l24.lc3(l85);}
this.lj3.ll4=l75;}
return this.lj3.ll4;}
_jcvlp.getNearestXIndex=function(lt){var l75=this.getXCoor();if((lt==null)||!l75||!l75.length)return null;var l95=this.lj3.lk4;if(l75!=this.lj3.lt3){l95=[];for(var i=0;i<l75.length;++i)l95[i]=i;l95.sort(function(a,b){return _jcv.lo3(l75[a],l75[b]);});this.lj3.lk4=l95;this.lj3.lt3=l75;}
var l11=ase_s(l95,lt,function(a,b){return _jcv.lo3(l75[a],b);});if(l11>=0)return l95[l11];l11=~l11;if(l11>=l95.length)return l95[l95.length-1];if(l11==0)return l95[0];var la5=l95[l11-1];var lb5=l95[l11];if((l75[la5]==null)||(lt-l75[la5]>l75[lb5]-lt))return lb5;else return la5;}
_jcvlp.getNearestXValue=function(lt){var l11=this.getNearestXIndex(lt);return(l11!=null)?this.getXPosition(l11):null;}
_jcvlp.getXIndexOf=function(l85,lc5){if(!lc5)lc5=0;var l11=this.getNearestXIndex(this.lm4.xAxis().lc3(l85));return((l11==null)||(Math.abs(this.getXPosition(l11)-l85)>lc5))?null:l11;}
function _jcvd(lz4,lq3){this.lm4=lz4;this.data=lq3;}
_jcvdp=_jcvd.prototype;_jcvdp.getDataName=function(){return this.data.id;}
_jcvdp.getDataSetNo=function(){return this.data.i;}
_jcvdp.getDataGroup=function(){return this.data.g;}
_jcvdp.getDataColor=function(){return ase_r(this.data.color);}
_jcvdp.getUseYAxis=function(){return this.lm4.lm4.getAxis(this.data.axis);}
_jcvdp.getValue=function(i){return((i>=0)&&(i<this.data.data.length))?this.data.data[i]:null;}
_jcvdp.getPosition=function(i){var l02=this.getValue(i);if(l02==null)return null;var le5=this.getDataSetNo();var lf5=this.getDataGroup();switch(this.lm4.lj3.dcm){case 1:for(var j=le5-1;(j>=0)&&(this.lm4.getDataSet(j).getDataGroup()==lf5);--j){var v=this.lm4.getDataSet(j).getValue(i);if((v!=null)&&((l02>=0)^(v<0)))l02+=v;}
return l02;case 4:l02=Math.abs(l02);var lg5=1;for(var k=le5-1;(k>=0)&&(this.lm4.getDataSet(k).getDataGroup()==lf5);--k){var v=this.lm4.getDataSet(k).getValue(i);if(v!=null){++lg5;l02+=Math.abs(v);}
}
var lh5=l02;var li5=lg5;for(var n=le5+1;(n<this.lm4.getDataSetCount())&&(this.lm4.getDataSet(n).getDataGroup()==lf5);++n){var v=this.lm4.getDataSet(n).getValue(i);if(v!=null){++li5;lh5+=Math.abs(v);}
}
return(lh5<=0)?(lg5*100/li5):(l02*100/lh5);default:return l02;}
}
_jcvdp.getValueCount=function(){return this.data.data.length;}
_jcv.NTime=function(dt){var t=Math.floor(dt);var ms=Math.min(999,Math.floor(Math.round((dt-t)*100000)/100));var s=t%60;t=(t-s)/60;var n=t%60;t=(t-n)/60;var h=t%24;t=(t-h)/24;var y=Math.floor(t/365.2425)+1;t-=_jcv.lv3(y);if(_jcv.lh3(y)){if(t>=366){++y;t-=366;}
}
else {if(t>=365){++y;t-=365;}
}
for(var m=1;m<12;++m){if(t<_jcv.lu3(y,m+1))break;}
var d=t-_jcv.lu3(y,m)+1;return new Date(y,m-1,d,h,n,s,ms);}
_jcv.lh3=function(y){return((y%400==0)||((y%4==0)&&(y%100!=0)));}
_jcv.lv3=function(y){--y;return y*365+Math.floor(y/4)-Math.floor(y/100)+Math.floor(y/400);}
_jcv.l13=[0,31,59,90,120,151,181,212,243,273,304,334];_jcv.lu3=function(lm5,ln5){var l11=_jcv.l13[ln5-1];if((ln5>2)&&_jcv.lh3(lm5))++l11;return l11;}
_jcv.CTime=function(d){return _jcv.chartTime(d.getFullYear(),d.getMonth()+1,d.getDate(),d.getHours(),d.getMinutes(),d.getSeconds()+d.getMilliseconds()/1000.0);}
_jcv.chartTime=function(y,m,d,h,n,s){return(_jcv.lv3(y)+_jcv.lu3(y,m)+d-1)*86400+(h||0)*3600+(n||0)*60+(s||0);}
_jcvp.ls=function(e){this.ly2=ase_f(e);this.lz2=ase_g(e);}
_jcvp.lt=function(e){var eX=ase_f(e);var eY=ase_g(e);if(this.la==_jcv.ZoomIn){var d=this.lc;var lq5=this.lh1()&&this.la1(e,d);if(lq5){var ly1=Math.min(eX,this.ly2);var lr5=Math.min(eY,this.lz2);var lk2=Math.abs(eX-this.ly2);var ll2=Math.abs(eY-this.lz2);switch(d){case _jcv.Horizontal:this.lq1(ly1,this.lt2(this.l1),lk2,this.lq2(this.l3));break;case _jcv.Vertical:this.lq1(this.ls2(this.l0),lr5,this.lr2(this.l2),ll2);break;default:this.lq1(ly1,lr5,lk2,ll2);break;}
}
this.lr1(lq5);}
else if(this.la==_jcv.Scroll){var d=this.lb;if(this.l62||this.la1(e,d)){this.l62=true;var ls3=(d==_jcv.Vertical)?0:(eX-this.ly2);var lt3=(d==_jcv.Horizontal)?0:(eY-this.lz2);if((ls3<0)&&(this.l4+this.l6-this.l6*this.lv2(ls3)/this.l2>1))ls3=Math.min(0,(this.l4+this.l6-1)*this.l2/this.l6);if((ls3>0)&&(this.l6*this.lv2(ls3)/this.l2>this.l4))ls3=Math.max(0,this.l4*this.l2/this.l6);if((lt3<0)&&(this.l5+this.l7-this.l7*this.lu2(lt3)/this.l3>1))lt3=Math.min(0,(this.l5+this.l7-1)*this.l3/this.l7);if((lt3>0)&&(this.l7*this.lv2(lt3)/this.l3>this.l5))lt3=Math.max(0,this.l5*this.l3/this.l7);this.lq1(this.ls2(this.l0)+ls3,this.lt2(this.l1)+lt3,this.lr2(this.l2),this.lq2(this.l3));this.lr1(true);}
}
}
_jcvp.lv=function(e){this.lr1(false);switch(this.la){case _jcv.ZoomIn:if(this.lh1()){if(this.la1(e,this.lc))this.lk1(e);else this.lm1(e,this.ld);}
break;case _jcv.ZoomOut:if(this.li1())this.lm1(e,this.le);break;default:if(this.l62)this.ll1(e);break;}
this.l62=false;}
_jcvp.lu=function(e){}
_jcvp.l11=function(e){}
_jcvp.lm1=function(e,ls5){var eX=ase_f(e);var eY=ase_g(e);var lt5=this.l6/ls5;var lu5=this.l7/ls5;this.ld1(this.lc,(this.lw2(eX)-this.l0)*this.l6/this.l2-lt5/2,lt5,(this.lx2(eY)-this.l1)*this.l7/this.l3-lu5/2,lu5);}
_jcvp.ll1=function(e){var eX=ase_f(e);var eY=ase_g(e);this.ld1(this.lb,this.l6*this.lv2(this.ly2-eX)/this.l2,this.l6,this.l7*this.lu2(this.lz2-eY)/this.l3,this.l7);}
_jcvp.lk1=function(e){var eX=ase_f(e);var eY=ase_g(e);var lt5=this.l6*this.lv2(Math.abs(this.ly2-eX))/this.l2;var lu5=this.l7*this.lu2(Math.abs(this.lz2-eY))/this.l3;this.ld1(this.lc,this.l6*(this.lw2(Math.min(this.ly2,eX))-this.l0)/this.l2,lt5,this.l7*(this.lx2(Math.min(this.lz2,eY))-this.l1)/this.l3,lu5);}
_jcvp.ld1=function(d,lv5,lw5,lx5,ly5){var lz5=this.l4;var l06=this.l5;var lt5=this.l6;var lu5=this.l7;if((((lw5<this.l6)&&(this.l6<this.lf))||(d==_jcv.Vertical))&&(((ly5<this.l7)&&(this.l7<this.lh))||(d==_jcv.Horizontal)))return;if(d!=_jcv.Vertical){if(lw5!=this.l6){lt5=Math.max(this.lf,Math.min(lw5,this.lg));lv5-=(lt5-lw5)/2;}
lz5=Math.max(0,Math.min(this.l4+lv5,1-lt5));}
if(d!=_jcv.Horizontal){if(ly5!=this.l7){lu5=Math.max(this.lh,Math.min(ly5,this.li));lx5-=(lu5-ly5)/2;}
l06=Math.max(0,Math.min(this.l5+lx5,1-lu5));}
if((lz5!=this.l4)||(l06!=this.l5)||(lt5!=this.l6)||(lu5!=this.l7)){this.ln2=this.l4;this.lo2=this.l5;this.lp2=this.l6;this.lm2=this.l7;this.l4=lz5;this.l5=l06;this.l6=lt5;this.l7=lu5;this.raiseViewPortChangedEvent();}
}
_jcvp.raiseViewPortChangedEvent=function(){this.lp=1;this.lk2();this.applyHandlers("viewportchanged");if(this.onViewPortChanged&&(this.lu1("viewportchanged").length==0))this.onViewPortChanged();this.lp=0;}
_jcvp.lu1=function(lj3){var id=(lj3+"events").toLowerCase();if(!this[id])this[id]=[];return this[id];}
_jcvp.attachHandler=function(lj3,f){if(lj3 instanceof Array){var l11=[];for(var i=0;i<lj3.length;++i)l11[i]=this.attachHandler(lj3[i],f);return l11;}
else {var a=this.lu1(lj3);a[a.length]=f;return lj3+":"+(a.length-1);}
}
_jcvp.detachHandler=function(lp2){if(lp2 instanceof Array){for(var i=0;i<lp2.length;++i)this.detachHandler(lp2[i]);}
else {var ab=lp2.split(':');var a=this.lu1(ab[0]);a[parseInt(ab[1])]=null;}
}
_jcvp.applyHandlers=function(lj3,l26){this.ll3(lj3);var l11=false;var a=this.lu1(lj3);for(var i=0;i<a.length;++i){this.l03=a[i];if(this.l03!=null)l11|=this.l03(l26);}
this.l03=null;return l11;}
_jcvp.partialUpdate=function(){if(this.ls1)return;_jcv.lj1(this.lz1());this.applyHandlers("preupdate");this.ln=1;this.lk2();var l36=this.updatingMsg;if(typeof l36=="undefined")l36=_jcv.updatingMsg;if(l36&&(l36!="none"))this.msgBox(l36);var lq=ase_o(ase_9(this.lr+"_callBackURL").value,"cdPartialUpdate="+this.lr+"&cdCacheDefeat="+(new Date().getTime())+"&"+this.l02.name+"="+ase_p(escape(this.l02.value),'\\+','%2B'));var lu3=this;this.ls1=true;ase_y(lq,function(t){lu3.la2(t)},function(l46,l56){lu3.l31(l46,l56);});}
_jcvp.la2=function(l91){var l66=ase_t(l91,"<!--CD_SCRIPT "," CD_SCRIPT-->");if(l66){var l76=ase_t(l91,"<!--CD_MAP "," CD_MAP-->");var imgObj=this.lz1();var imgBuffer=(this.lc1=(this.doubleBuffering?new Image():null))||imgObj;if(imgObj.useMap)imgObj.useMap=null;imgObj.loadImageMap=function(){window.setTimeout(function(){_jcv.putMap(imgObj,l76);},100);};imgBuffer.onload=function(){imgObj._jcv.onPartialLoad(true);}
imgBuffer.onerror=imgBuffer.onabort=function(l62){imgObj._jcv.l31(999,"Error loading image '"+this.src+"'["+l62+"]");}
var l96=window.onerror;window.onerror=function(l62){imgObj._jcv.l31(801,"Error interpretating partial update result ["+l62+"] <div style='margin:20px;background:#dddddd'><xmp>"+l66+"</xmp></div>")};eval(l66);window.onerror=l96;if(ase_2)this.lk2();}
else this.l31(800,"Partial update returns invalid data <div style='margin:20px;background:#dddddd'><xmp>"+l91+"</xmp></div>");}
_jcvp.l21=function(la6){var imgObj=this.lz1();var imgBuffer=this.lc1||imgObj;if(imgBuffer)imgBuffer.onerror=imgBuffer.onabort=imgBuffer.onload='';imgObj.onUpdateCompleted='';this.ls1=false;this.l71(this.l51());if(la6){this.ln2=this.l4;this.lm2=this.l7;this.lo2=this.l5;this.lp2=this.l6;if(imgObj!=imgBuffer){imgObj.src=imgBuffer.src;imgObj.style.width=imgBuffer.style.width;imgObj.style.height=imgBuffer.style.height;}
imgObj.loadImageMap();}
else {imgObj.useMap=imgObj.li2;if(this.lp2||this.lm2){this.l4=this.ln2;this.l7=this.lm2;this.l5=this.lo2;this.l6=this.lp2;this.lk2();}
}
imgObj.loadImageMap='';}
_jcvp.onPartialLoad=function(la6){if(this.lz1().onUpdateCompleted)this.lz1().onUpdateCompleted();else this.msgBox();this.l21(la6);window[this.getId()+"_chartModel"]=window[this.getId()+"_pending_chartModel"];this.lq4();this.applyHandlers("postupdate");}
_jcvp.l31=function(l46,l56){this.l21(false);this.msgBox();this.errCode=l46;this.errMsg=l56;if(!this.applyHandlers("updateerror")){var lb6=this.serverErrorMsg;if(!lb6)lb6=_jcv.serverErrorMsg;if(lb6&&(lb6!="none"))this.msgBox(ase_p(ase_p(lb6,'%errCode',l46),'%errMsg',l56));}
this.errCode=null;this.errMsg=null;}
_jcvp.streamUpdate=function(lc6){var ld6=new Date().getTime();if(!lc6)lc6=60;var ls4=this.lb2;if(ls4){if(lc6*1000>=ld6-ls4.lb1)return false;ls4.src=null;ls4.onerror=ls4.onabort=ls4.onload=null;}
if(!this.l92)this.l92=this.lz1().src;this.lb2=ls4=new Image();ls4.lb1=ld6;var lu3=this;ls4.onload=function(){var imgObj=lu3.lz1();if(imgObj.useMap)imgObj.useMap=null;var b=lu3.lb2;if(imgObj!=b)imgObj.src=b.src;b.onabort();}
ls4.onerror=ls4.onabort=function(){var b=lu3.lb2;if(b)b.onload=b.onabort=b.onerror=null;lu3.lb2=null;}
ls4.src=ase_o(this.l92,"cdDirectStream="+this.lr+"&cdCacheDefeat="+ld6);return true;}
_jcvp.lf1=function(a,v){return a+((typeof v!="number")?"**":"*")+v;}
_jcvp.le1=function(av){var lz=av.indexOf("*");if(lz==-1)return null;var a=av.substring(0,lz);var v=av.substring(lz+1,av.length);if(v.charAt(0)=="*")v=v.substring(1,v.length);else v=parseFloat(v);return{"attr":a,"value":v};}
_jcvp.lg1=function(){var l11="";for(var i=0;i<_jcv.lp1.length;++i){var a=_jcv.lp1[i];var v=null;if((a=="lo")&&this.lo){for(var lm in this.lo)v=((v==null)?"":v+"\x1f")+this.lf1(lm,this.lo[lm]);}
else v=this[a];if((typeof v!="undefined")&&(null!=v))l11+=(l11?"\x1e":"")+this.lf1(i,v);}
return l11;}
_jcvp.decodeState=function(s){var le6=s.split("\x1e");for(var i=0;i<le6.length;++i){var av=this.le1(le6[i]);if(!av)continue;var a=_jcv.lp1[parseInt(av.attr)];if(a=="lo"){var lf6=av.value.split("\x1f");for(var i2=0;i2<lf6.length;++i2){var lh6=this.le1(lf6[i2]);if(!lh6)continue;this.lo[lh6.attr]=lh6.value;}
}
else this[a]=av.value;}
this.lp=0;}
_jcvp.lk2=function(){if(this.l02)this.l02.value=this.lg1();}
_jcvp.ly1=function(){var imgObj=this.lz1();var m=_jcv.lt1(imgObj);if(m){m.onmousedown=imgObj.onmousedown;m.onmousemove=imgObj.onmousemove;m.onmouseout=imgObj.onmouseout;}
}
_jcv.lt1=function(imgObj){var li6=imgObj.li2;if(!li6)li6=imgObj.useMap;if(!li6)return null;var lz=li6.indexOf('#');if(lz>=0)li6=li6.substring(lz+1);return ase_9(li6);}
_jcv.loadMap=function(imgObj,lq){if(!imgObj.li2)imgObj.li2=imgObj.useMap;_jcv.lj1(imgObj);imgObj.l12=ase_y(lq,function(t){_jcv.putMap(imgObj,ase_t(t,"<!--CD_MAP "," CD_MAP-->"));},function(l46,l56){_jcv.onLoadMapError(l46,l56);}
);}
_jcv.loadPendingMap=function(){if(!window._jcvPendingMap)return;for(var a in window._jcvPendingMap){var ll=ase_9(a);if(ll){var lq=window._jcvPendingMap[a];window._jcvPendingMap[a]=null;if(lq)_jcv.loadMap(ll,lq);}
}
}
_jcv.lj1=function(imgObj){if(imgObj.l12){imgObj.l12.abort();imgObj.l12=null;}
}
_jcv.onLoadMapError=function(l46,l56){}
_jcv.putMap=function(imgObj,lj6){var m=_jcv.lt1(imgObj);if(!m&&lj6){var li6='map_'+imgObj.id;imgObj.useMap=imgObj.li2='#'+li6;var d=document;m=d.createElement("MAP");m.id=m.name=li6;d.body.appendChild(m);if(imgObj._jcv)imgObj._jcv.ly1();}
if(m){m.innerHTML=lj6;if(imgObj.useMap!=imgObj.li2)imgObj.useMap=imgObj.li2;}
imgObj.l12=null;}
_jcv.canSupportPartialUpdate=function(){return(window.XMLHttpRequest||ase_x());}
_jcv.getVersion=function(){return ase_0;}
JsChartViewer=_jcv;_jcv.loadPendingMap();