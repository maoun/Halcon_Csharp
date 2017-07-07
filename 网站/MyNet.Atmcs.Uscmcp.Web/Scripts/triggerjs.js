function Expand(){ 
	//自建题库列表模式 鼠标经过
	$(".Selftkbox .sel_box3_1 ul.Listlist li,.Selftkbox .sel_box3_3 ul.Listlist li").hover(function () {
      $(this).find(".showFunction").stop(true,true).show();
    });
	$(".Selftkbox .sel_box3_1 ul.Listlist li,.Selftkbox .sel_box3_3 ul.Listlist li").mouseleave(function(){
      $(this).find(".showFunction").stop(true,true).hide();
     });
}


$(document).ready(function () {
	//取消发布弹窗口
	$(".forli1 .Cancel").click(function () {
		$(".Cancel_Popup").fadeIn();
	});
	//清楚首页最近任务最后一条任务下划线
	$("dl.Taskbox table").find("tr:last").addClass("Clearline"); 
	//关闭弹出层
	$(".close_btn").click(function () {
		$(this).parent().fadeOut();
		$(".Maskbox").fadeOut();
		$(".Maskbox").css("z-index","9998");
	});
//	$(".Release,.Cancel").click(function () {
//		$(this).parent().parent().fadeOut();
//		$(".Maskbox").fadeOut();
//	});
	
	
	//任务延期 帮助
	$("#Help1").hover(function () {
      $(this).find("p").toggle();
    });
	//弹出 发布任务 发布内容
	$("#Release_Popup ul.Formbox li.forli .Contentlist ul.FolderUl li.FileUl_li,#Release_Popup ul.Formbox li.forli .Contentlist ul.FolderUl li .showtit,#Release_Popup ul.Formbox li.forli .Contentlist ul.FolderUl li .showlist p").hover(function () {
      $(this).find("a.del").toggle();
    });
	//自建题库列表模式 鼠标经过
	$(".Selftkbox .sel_box3_1 ul.Listlist li,.Selftkbox .sel_box3_3 ul.Listlist li").hover(function () {
      $(this).find(".showFunction").stop(true,true).show();
    });
	$(".Selftkbox .sel_box3_1 ul.Listlist li,.Selftkbox .sel_box3_3 ul.Listlist li").mouseleave(function(){
      $(this).find(".showFunction").stop(true,true).hide();
     });
	//弹出 发布任务 发布内容 文件夹列表弹出
	$("#Release_Popup ul.Formbox li.forli .Contentlist ul.FolderUl li .showtit,#Release_Popup ul.Formbox li.forli .Contentlist ul.FolderUl li em").click(function () {
		$(this).siblings(".showlist").toggle();
		$(this).siblings("em").toggleClass("Bounce");
		$(this).toggleClass("Bounce");
		$(this).parent().siblings("li").find(".showlist").hide();
		$(this).parent().siblings("li").find("em").removeClass("Bounce");
		$('.scroll-pane').jScrollPane({maintainPosition:true,stickToBottom:true,});
	});
   //选择习题
	var _obi= $(".Preview_Popup");
	var _wid= _obi.width();
	var _hid= _obi.height();
	var _lwid=$(window).width()/2 - _wid/2
	var _lhid=$(window).height()/2 - _hid/2
	$("#Preview").click(function () {
		$(".Preview_Popup").css({"left":_lwid,"top":_lhid}).fadeIn();
		$(".Maskbox").fadeIn()
	});
	//添加班级和删除班级
	$(".yj_ul li ul li h3").live('click',function(){
		var grade=$(this).parent().parent().siblings("h3").text();
		var item=$(this).text();
		$(".classlist").append("<li><span>" + grade + item + "</span><a></a></li>");
    })
	$(".classlist li a").live('click',function(){
		$(this).parent().remove();
	});
	//新建习题 挑战录音
	$(".Rechallenge_Big span a.b_Sound_Big").live('click',function(){
		$(this).toggleClass("b_Sound_Big_Start");
	});
	//发布任务弹窗
	var _obi3= $("#Release_Popup");
	var _wid3= _obi3.width();
	var _hid3= _obi3.height();
	var _lwid3=$(window).width()/2 - _wid3/2
	var _lhid3=$(window).height()/2 - _hid3/2
	$("#Choice,#Choice_A").click(function () {
		$("#Release_Popup").css({"left":_lwid3,"top":_lhid3}).fadeIn();
	    $('.scroll-pane').jScrollPane({maintainPosition:true,stickToBottom:true,});
		$(".Maskbox").fadeIn()
	});
	var _obi1= $(".Success_Popup");
	var _wid1= _obi1.width();
	var _hid1= _obi1.height();
	var _lwid1=$(window).width()/2 - _wid1/2
	var _lhid1=$(window).height()/2 - _hid1/2
	$("#ReleaseA").click(function () {
		$(".Success_Popup").css({"left":_lwid1,"top":_lhid1}).fadeIn();
		$(this).parents("#Release_Popup").hide();
		$(".Maskbox").fadeIn()
	});
	$("#ReleaseB").click(function () {
		$("#Release_Popup").hide();
		$(this).parents(".Cancel_Popup").hide();
		$(".Maskbox").fadeOut()
		$(".Maskbox").css("z-index","9998");
	});
	var _obi2= $(".Cancel_Popup");
	var _wid2= _obi2.width();
	var _hid2= _obi2.height();
	var _lwid2=$(window).width()/2 - _wid2/2
	var _lhid2=$(window).height()/2 - _hid2/2
	$("#CancelA").click(function () {
		$(".Cancel_Popup").css({"left":_lwid2,"top":_lhid2}).fadeIn();
		$(".Maskbox").css("z-index","9999");
		//$(this).parents("#Release_Popup").hide();
	});
	$("#CancelB,.close_can").click(function () {
		$(this).parents(".Cancel_Popup").hide();
		$(".Maskbox").css("z-index","9998");
	});
	//移动到弹窗
	var _obi4= $("#Moveto_Popup");
	var _wid4= _obi4.width();
	var _hid4= _obi4.height();
	var _lwid4=$(window).width()/2 - _wid4/2
	var _lhid4=$(window).height()/2 - _hid4/2
	$("#funboxa2,#funboxa2_A").click(function () {
		$("#Moveto_Popup").css({"left":_lwid4,"top":_lhid4}).fadeIn();
		$(".Maskbox").fadeIn()
	   $('.scroll-pane').jScrollPane({maintainPosition:true,stickToBottom:true,});
	});
	$("#CancelC").click(function () {
		$(this).parents("#Moveto_Popup").fadeOut();
		$(".Maskbox").fadeOut()
	});
	var _obi5= $("#RenameSuccess");
	var _wid5= _obi5.width();
	var _hid5= _obi5.height();
	var _lwid5=$(window).width()/2 - _wid5/2
	var _lhid5=$(window).height()/2 - _hid5/2
	$("#ReleaseC").click(function () {
		$("#RenameSuccess").css({"left":_lwid5,"top":_lhid5}).fadeIn();
		$(this).parents("#Moveto_Popup").fadeOut();
		$(".Maskbox").fadeOut()
		function changeTime()
         {
		  $("#RenameSuccess").fadeOut();
	     }
		setInterval(changeTime,1000);
	});
	//训练题库题目弹出层
	var _obi6= $(".Topicbox_Popup");
	var _wid6= _obi6.width();
	var _hid6= _obi6.height();
	var _lwid6=$(window).width()/2 - _wid6/2
	var _lhid6=$(window).height()/2 - _hid6/2
	$("dl dd ul.Listlist li .Booklist p a").click(function () {
		$(".Topicbox_Popup").css({"left":_lwid6,"top":_lhid6}).fadeToggle();
		//$(this).toggleClass("active");
		sliderB(sliderB_1);
		$(".Maskbox").toggle()
	   $('.scroll-pane').jScrollPane({maintainPosition:true,stickToBottom:true,});
	});
	//头部习题框弹出
	$(".header .l_nav .nav .Exercisesbox").hover(function () {
		timer = setTimeout(function(){
          $(".Exercisesbox").addClass("E_After");
     	  $(".ExerciPopup").show();
	      $('.scroll-pane').jScrollPane({maintainPosition:true,stickToBottom:true,});
		  //$('#SelectAll1 input[type=checkbox]').prop("checked",false).siblings("label").removeClass("focus");
        },500);
      },function(){
          $(".Exercisesbox").removeClass("E_After");
     	  $(".ExerciPopup").hide();
          clearTimeout(timer);
      });
//      $(this).toggleClass("E_After");
//	  $(this).find(".ExerciPopup").toggle();
//	  $('.scroll-pane').jScrollPane({maintainPosition:true,stickToBottom:true,});
//    });
	//习题框展开收起
	$(".Selftkbox .sel_box3_2 dl dd ul.Listlist li .showtit").click(function () {
      $(this).siblings(".Booklist").slideToggle();
	  $(this).find("a").toggleClass("ret");
	  $(this).parents().siblings("li").find(".Booklist").slideUp();
	  $(this).parents().siblings("dl").find(".Booklist").slideUp();
	  $(this).parents().siblings(".sel_box3_2").find(".Booklist").slideUp();
	  
	  $(this).parents().siblings("li").find(".showtit a").removeClass("ret");
	  $(this).parents().siblings("dl").find(".showtit a").removeClass("ret");
	  $(this).parents().siblings(".sel_box3_2").find(".showtit a").removeClass("ret");
    });
	//训练题库展开收起
	$(".Selftkbox .sel_box3_3 dl dd ul.Listlist li .showtit").click(function () {
      $(this).siblings(".Booklist").slideToggle();
	  $(this).find("a").toggleClass("ret");
	  $(this).parents().siblings("li").find(".Booklist").slideUp();
	  $(this).parents().siblings("li").find(".showtit a").removeClass("ret");
    });
	//排序图标切换
	$(".SortJS").click(function () {
	  $(this).toggleClass("Sort");
    });
	//统计分析播放暂停
	$(".PlayJS").click(function () {
	  var txt = $(this).text();
	  $(this).toggleClass("Stop");
	  if(txt=='播放录音'){
			$(this).text('停止播放');
		} else if(txt=='停止播放'){
			$(this).text('播放录音');
	   }
    });
	
	//任务管理删除弹出层
	var _obi7= $(".Delete_Popup");
	var _wid7= _obi7.width();
	var _hid7= _obi7.height();
	var _lwid7=$(window).width()/2 - _wid7/2
	var _lhid7=$(window).height()/2 - _hid7/2
	$("#Delete").click(function () {
		$(".Delete_Popup").css({"left":_lwid7,"top":_lhid7}).fadeToggle();
		//$(this).toggleClass("active");
		$(".Maskbox").toggle()
	});

});



//表单文字消失
$(function() {
  $('.input_text').each(function(){
    $(this).data( "txt", $.trim($(this).val()) );
	$(this).css("color","#d8d8d8");
  })
  .focus(function(){
    if ( $.trim($(this).val()) === $(this).data("txt") ) {
    $(this).val("");
	//$(this).css({"color":"#4a4a4a","border":"1px solid #fbb612"});
	$(this).css({"color":"#4a4a4a"});
   }
 })
 .blur(function(){
    if ( $.trim($(this).val()) === "" && !$(this).hasClass("once") ) {
    $(this).val( $(this).data("txt") );
	$(this).css({"color":"#d8d8d8","border":"1px solid #aeaeae"});
    }
   });
});
$(function() {
  $('.input_text_s').each(function(){
    $(this).data( "txt", $.trim($(this).val()) );
	$(this).css("color","#d8d8d8");
  })
  .focus(function(){
    if ( $.trim($(this).val()) === $(this).data("txt") ) {
    $(this).val("");
	//$(this).css({"color":"#4a4a4a","border":"1px solid #fbb612"});
	$(this).css({"color":"#4a4a4a"});
   }
 })
 .blur(function(){
    if ( $.trim($(this).val()) === "" && !$(this).hasClass("once") ) {
    $(this).val( $(this).data("txt") );
	$(this).css({"color":"#d8d8d8","border":"1px"});
    }
   });
});


/*获得焦点*/
function Focus() {
  $('.textarea1,.textarea2').focus(function(){
    $(this).css({"border":"1px solid #3d88a8"});
 })
 .blur(function(){
    $(this).css({"border":"1px solid #fff"});
	$("#textB .textarea1").blur(function() { 
         var newtxt = $(this).val(); 
		   //判断文本是否为空 
		   if(newtxt == ''){
			 $(this).next().remove();
			 $(this).remove();
             $('.scroll-pane').jScrollPane({maintainPosition:true,stickToBottom:true,});
		   }
		 });
   });
   
};


//判断表单字数
function check(){ 
 var key=document.getElementById('xtname'); 
 if(key.value.length>16){ 
 //alert("关键字长度不能小于2!");
 $("#xtname").addClass("Baocuo").siblings(".Errorbox").show();
 return false; 
 } 
} 


$(function(){
	//下拉菜单
	function selectCL(obj,callback){
		if(callback!=undefined)obj.data("callback",callback);//添加回调函数
		if(obj.data("init")==true){return}
		obj.data("init",true);
		//alert(obj.find(".selectCL_list>li[selected]").length)
		obj.find(".selectCL_list>li").each(function(index, element) {
			if($(element).attr("selected")!=undefined){
				var value=""
				if ($(element).attr("value")!=undefined)value=$(element).attr("value");//这种会出问题，jquery取不到全值
				if ($(element).attr("data-value")!=undefined)value=$(element).attr("data-value");
				$(element).parents(".selectCL:first").find("input").val(value);
				$(element).parents(".selectCL:first").find("p").text($(element).text())
			}
		});
	    obj.find('li').last().css("border-bottom",0);
		obj.find(".selectCL_list").bind("click",function(event){
			if($(event.target).parent().hasClass("selectCL_list")){
				$(event.target).parent().prev().html($(event.target).html());
				$(event.target).parent().slideUp(300);
				var value="";
				if ($(event.target).attr("value")!=undefined)value=$(event.target).attr("value");//这种会出问题，jquery取不到全值
				if ($(event.target).attr("data-value")!=undefined)value=$(event.target).attr("data-value");
				$(event.target).parents(".selectCL:first").find("input").val(value);
				$(event.target).parents(".selectCL:first").find("input").trigger("change");
				$(event.target).parents(".selectCL:first").find("p").removeClass("error");
				if(obj.data("callback")!=undefined)obj.data("callback").call(event.target);
				
			}
		})
		obj.find('.selectCL_list').bind("mouseover",function(event){
			if($(event.target).parent().hasClass("selectCL_list")){$(event.target).addClass('hover');}
		}).bind("mouseout",function(event){
			if($(event.target).parent().hasClass("selectCL_list")){$(event.target).removeClass('hover');}
		})
	    obj.find('p').click(function() {
			if($(this).parent().find(".selectCL_list").children().length==0)return;
	    	if($(this).hasClass('visited')){
		        $(this).removeClass('visited');
		        var ulH = obj.find("ul").height();
		        $(this).next().slideUp(300);
	    	}
	    	else{
		        $(this).addClass('visited');
		        var ulH = obj.find("ul").height();
		        $(this).next().slideDown(300);
		    }
	        //ulH >= 170 ? obj.find("li").css("padding-right","20px") : obj.find("li").css("padding-right","10px");
	    });
	    
	    $(document.body).click(function(e) {
	        if (e.target != obj.find('p').get(0) && e.target != obj.find('img').get(0)) {
	            obj.find('ul').slideUp(300);
	            obj.find('p').removeClass('visited');
	        };
	    })
	}
	
	//排序图标切换
	$(".selectCL_list .CustomButton").click(function () {
	  $(".Custombox").show();
    });
	$(".selectCL_list li.li1").click(function () {
	  $(".Custombox").hide();
    });
	
	
	window.selectCL=selectCL;//变成全局变量
	//新初始化 下拉框
	var choose = $('.selectCL');
    choose.each(function(i) {
    	selectCL($(this));
    });
	
	
	
	
	//多级下拉菜单
	function selectDL(obj,callback){
		if(callback!=undefined)obj.data("callback",callback);//添加回调函数
		if(obj.data("init")==true){return}
		obj.data("init",true);
		//alert(obj.find(".selectCL_list>li[selected]").length)
		obj.find(".selectDL_list>li").each(function(index, element) {
			if($(element).attr("selected")!=undefined){
				var value=""
				if ($(element).attr("value")!=undefined)value=$(element).attr("value");//这种会出问题，jquery取不到全值
				if ($(element).attr("data-value")!=undefined)value=$(element).attr("data-value");
				$(element).parents(".selectDL:first").find("input").val(value);
				$(element).parents(".selectDL:first").find("p").text($(element).text())
			}
		});
	    obj.find('li').last().css("border-bottom",0);
		obj.find(".selectDL_list").bind("click",function(event){
			if($(event.target).parent().parent().parent().hasClass("selectDL_list")){
				$(event.target).parent().parent().parent().prev().html($(event.target).parent().parent().find(".selectDL_span").text()+$(event.target).html());
				$(event.target).parent().parent().parent().prev().attr("title",$(event.target).parent().parent().find(".selectDL_span").text()+$(event.target).html());
				//alert($(event.target).parent().parent().find(".selectDL_span").text());
				$(event.target).parent().parent().parent().slideUp(300);
				var value="";
				if ($(event.target).attr("value")!=undefined)value=$(event.target).attr("value");//这种会出问题，jquery取不到全值
				if ($(event.target).attr("data-value")!=undefined)value=$(event.target).attr("data-value");
				$(event.target).parents(".selectDL:first").find("input").val(value);
				$(event.target).parents(".selectDL:first").find("input").trigger("change");
				$(event.target).parents(".selectDL:first").find("p").removeClass("error");
				if(obj.data("callback")!=undefined)obj.data("callback").call(event.target);
			}
		})
	    obj.find('p').click(function() {
			if($(this).parent().find(".selectDL_list").children().length==0)return;
	    	if($(this).hasClass('visited')){
		        $(this).removeClass('visited');
		        //var ulH = obj.find("ul").height();
		        $(this).next().slideUp(300);
	    	}
	    	else{
		        $(this).addClass('visited');
		        //var ulH = obj.find("ul").height();
		        $(this).next().slideDown(300);
		    }
	        //ulH >= 170 ? obj.find("li").css("padding-right","20px") : obj.find("li").css("padding-right","10px");
	    });
	    obj.find('.selectDL_list .selectDL_li').hover(function() {
			$(this).find("ul").toggle();
			$(this).toggleClass("hover");
	    });
	    
	    $(document.body).click(function(e) {
	        if (e.target != obj.find('p').get(0) && e.target != obj.find('img').get(0)) {
	            obj.find('ul').slideUp(300);
	            obj.find('p').removeClass('visited');
	        };
	    })
	}
	window.selectDL=selectDL;//变成全局变量
	//新初始化 下拉框
	var choose = $('.selectDL');
    choose.each(function(i) {
    	selectDL($(this));
    });


});



function adaptiveHeight(a, baserows, maxrows) {
  var po =  parseInt(a.css('padding-top')) + parseInt(a.css('padding-bottom'));
  var baseLineHeight = parseInt(a.css('line-height'));
  var baseHeight = baseLineHeight * baserows;
  a.height(baseHeight);
  var scrollval = a[0].scrollHeight;
  if (scrollval - po >= baseLineHeight * maxrows) {
    scrollval = baseHeight + baseLineHeight * (maxrows-baserows) + po;
  }
  a.height(scrollval - po);
  $('.scroll-pane').jScrollPane({maintainPosition:true,stickToBottom:true,});
};

function adaptiveTextarea(sel, baserows, maxrows, callback){
  sel.bind('input propertychange', function(e) {
    adaptiveHeight($(this), baserows, maxrows);
    if(callback) callback(e);
    });
  adaptiveHeight(sel, baserows, maxrows);
};



function getTextForTest() {
	// alert(1);
	
	//initTextInfo();
	// $("#qb_description").val('');
	// var content = '';
	$("#textB .jspPane").html('');
		$("#textB .jspPane").append('<textarea name="" cols="" rows="" class="textareabox textarea1">dfdfdfasfsdafsdafsaf</textarea><div class="divline"></div>');
        $('.scroll-pane').jScrollPane({maintainPosition:true,stickToBottom:true,});
	 $("#textA").remove();
	 $("#textB").show();
	 isTextok = true;
 	 Focus();
     adaptiveTextarea($('.textarea1'), 1, 3000);

}