// JavaScript Document
$(function(){
	 var Sys = {};
	var ua = navigator.userAgent.toLowerCase();
	var s;
	(s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
	(s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
	(s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :
	(s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :
	(s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;
	if (Sys.ie <=8){
		$('body').append("<div class='ietips'><h2>您的使用的IE浏览器"+ Sys.ie+"版本比较低，为了您更好的使用体验，建议用 IE9 或者 IE9 以上进行预览网站！ <p class='anther-change'>或者其他主流浏览器进行预览网站！</p></h2><span class='close-p'>好的！知道了！</span></div><div class='u-mark'></div>");
		$('.close-p').on('click',function(){
		   $('.ietips,.u-mark').remove();
		 });
		//alert('IE浏览器版本: ' + Sys.ie); 
	};
	if (parseInt(Sys.firefox)<=15){
		$('body').append("<div class='ietips'><h2>您的火狐浏览器版本"+ Sys.firefox+"比较低，为了您更好的使用体验，建议用 Firefox16 或者 Firefox16 以上进行预览网站！<p class='anther-change'>或者其他主流浏览器进行预览网站！</p></h2><span class='close-p'>好的！知道了！</span></div><div class='u-mark'></div>");
		$('.close-p').on('click',function(){
		   $('.ietips,.u-mark').remove();
		 });
		 //alert('firefox浏览器版本: ' + Sys.firefox); 
	};
	if (parseInt(Sys.chrome) <=15){
		$('body').append("<div class='ietips'><h2>您的Chrome浏览器版本"+ Sys.chrome+"比较低，为了您更好的使用体验，建议用 Chrome16 或者 Chrome16 以上进行预览网站！<p class='anther-change'>或者其他主流浏览器进行预览网站！</p></h2><span class='close-p'>好的！知道了！</span></div><div class='u-mark'></div>");
		$('.close-p').on('click',function(){
		   $('.ietips,.u-mark').remove();
		 });
		//alert('Chrome浏览器版本: ' + Sys.chrome);
	}; 
	if (parseInt(Sys.opera) <=10){
		$('body').append("<div class='ietips'><h2>您的Oprea浏览器版本"+ Sys.opera+"比较低，为了您更好的使用体验，建议用 Opera11 或者 Opera11 以上进行预览网站！<p class='anther-change'>或者其他主流浏览器进行预览网站！</p></h2><span class='close-p'>好的！知道了！</span></div><div class='u-mark'></div>");
		$('.close-p').on('click',function(){
		   $('.ietips,.u-mark').remove();
		 });
		//alert('Opera浏览器版本: ' + Sys.opera);
	}; 
	if (parseInt(Sys.safari) <=3) {
		$('body').append("<div class='ietips'><h2>您的Safari浏览器版本"+ Sys.safari+"比较低，为了您更好的使用体验，建议用 Safari4 或者 Safari4 以上进行预览网站！<p class='anther-change'>或者其他主流浏览器进行预览网站！</p></h2><span class='close-p'>好的！知道了！</span></div><div class='u-mark'></div>");
		$('.close-p').on('click',function(){
		   $('.ietips,.u-mark').remove();
		 });
		//alert('Safari浏览器版本: ' + Sys.safari);
	};
	var clickBtn = $('.lessIE-btn');
	var clickBtnOpen = $('.suport-btn');
	var clickBtnOpen02 = $('.suport-btn02');
	//browser set	
});
