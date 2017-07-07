// exten

$(document).ready(function() {
	UiCheck();//自定义check
	UiRadio();//自定义checkIMG
		
});


function UiCheck(){//自定义check
	var CheckDom = $('.ui-chek');
	    CheckDom.on('click',function(){
			if($(this).hasClass('ui-chek-choose')){
				$(this).removeClass('ui-chek-choose');
			}else{
				$(this).addClass('ui-chek-choose');
			};
		});
};

function UiRadio() {//自定义check
    alert('qqqq');
	var CheckDom = $('.check-img');
	    CheckDom.on('click',function(){
			if($(this).hasClass('check-img-active')){
				CheckDom.removeClass('check-img-active');
				
				$(this).parent().removeClass('yellow-color');
			}else{
				CheckDom.removeClass('check-img-active');
				$(this).addClass('check-img-active');
				$('.title-check').removeClass('yellow-color');
				$(this).parent().addClass('yellow-color');
				
			};
		});
};

function scrollH(){//右侧新闻滚动高度计算
	var scrollElement = $('.scroll-wrap');
	var sPosTop = scrollElement.offset().top;
	var windowH = $(window).innerHeight();
	var MapRightH = $('.map-up-right').innerHeight();
	scrollElement.height(MapRightH-sPosTop-20);
}