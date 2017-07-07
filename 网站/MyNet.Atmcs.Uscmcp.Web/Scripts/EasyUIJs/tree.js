var people;
function dianjifuzhi(data){
	var title = $(data).attr("class");
		var value = $(data).text();
		window.people.val(value);
		window.people.attr("title",title);
		$("#tree-select").hide();

		event=event||window.event;    
	    event.stopPropagation();  
}
function qiuchu(){
	window.people.val("");
	  var roots = $('#tree').tree('getRoots');  
      for(var i=0;i<roots.length;i++){  
          var node=$('#tree').tree('find',roots[i].id);  
          $('#tree').tree('uncheck', node.target);  
      } 
}
//关闭
function guanbi(){
	$("#tree-select").hide();
}
//返回目录
function fanhuimulu(){
	$("#sousuo_nav").hide();
	$("#tree_nav").show();
	$("#FieldStation").val("");
}
function tree_keyup(id){
		$("#sousuo_mohu").empty();
		var search_content = $(id).val(); //得到搜索的文件 
		if(search_content != ''){ 
			$("#sousuo_mohu").append("<ul id='ss_list'>");
		    var roots=$('#tree').tree('getRoots'); //得到tree顶级node 
		    var children;
		    for(var i =0;i<roots.length;i++){
		    	 children = $('#tree').tree('getChildren',roots[i].target);
		    	 if(children){
		    	 	for(j = 0;j<children.length;j++){
		    	 		 if($('#tree').tree('isLeaf',children[j].target)){
		    	 		 	if(children[j].text.indexOf(search_content) >= 0){
		    	 		 		
		    	 		 		$("#ss_list").append("<li class='"+children[j].attributes.kkid+"' onclick='dianjifuzhi(this)'>"+children[j].text+"</li>");
		    	 		 	} 
		    	 		 }
		    	 	}
		    	 }
		    }
			$("#sousuo_mohu").append("</ul>");
		}
		if($("#sousuo_nav").css("display") == "block"){
			if($("#ss_list").text() == ""){
				$("#sousuo_mohu").empty();
				$("#sousuo_mohu").append("<div style='clear:both;width:100%;height:60px;margin-top:80px;line-height:60px;text-align:center;'>没有符合当前条件的数据</div>");
			}
		}
		$("#tree_nav").hide();
		$("#sousuo_nav").show();
}
/**
	date:2016年10月18日 18:38:15
	author:yangpf
	description:当前对象的点击获取tree目录信息
*/
function tree_people(id,url){
	var node = "<div id='tree-select'>"+
			"<div id='tree_nav' class='tree_nav'>"
			+"<div id='tree_tt' class='container'>"
			+"<ul id='tree'  checkbox='true'></ul>"
			+"</div>"
			+"<ul class='function' id='func_1'>"
	    					+"<li><a id='clear_tree' class='func_btn'  onclick='qiuchu()'>清除</a><a id='close_tree' onclick='guanbi()' class='func_btn'>关闭</a></li>"
	    				+"</ul>"
    				+"</div>"
    				+"<div id='sousuo_nav' class='sousuo_nav'>"
    					+"<div id='sousuo_mohu'></div>"
    					+"<ul class='function' id='func_2'>"
	    					+"<li><a id='fanhui_tree' class='func_btn' onclick='fanhuimulu()'>返回目录</a><a id='close_sousuo' onclick='guanbi()' class='func_btn'>关闭</a></li>"
	    				+"</ul>"
    				+"</div>"
    			+"</div>";
    window.people = $(id);
    if($(id).parent().find("div[id='tree-select']").length > 0){
    }else{
		$(id).parent().append(node);
    }
    
    /**
	*   date:2016-10-18 19:03:00
	*	author:yangpf
	*   description:获取卡点名称和部门的tree目录信息
	*/
	$('#tree').tree({    
	    url:url,
	    method:'post', 
	    cascadeCheck:true,
	    onLoadSuccess:function(node,data){
	    	/*if(data.length > 0){
	    	    var n = $('ul[id="tree"]').tree("getRoot");
	    	    var temp = $('#tree').tree("getChildren",n.children[0].target);
	    	    for(var i=0;i<temp.length;i++){		
	    	    	if(i<10){
						var node = $('#tree').tree('find',parseInt(temp[0].id+"0"+(i+1)));
						if(node!= null){
							$('#tree').tree('check',node.target);
						}else{
							continue;
						}
	    	    	}else{
	    	    		break;
	    	    	}
	    	    }
          	}*/
	    },
	    onCheck:function(node,checked){
	    	var nodes = $('#tree').tree('getChecked');
	    	if(nodes.length >10){
	    		$("#tree").tree("uncheck",node.target);
	    	}else{
				checkTreeData1(nodes);
			}
	    }
	}); 
	$("#tree-select").get(0).style.left=$(id).offset().left;
		$("#tree-select").get(0).style.top=$(id).offset().top+24;
		$("#tree-select").show();
		$("#tree_nav").show();
		$("#sousuo_nav").hide();
    $("div[id='tree-select']").fadeIn();
	
}
function checkTreeData1(nodes){
	var s = '';
	var user_id = '';
	for(var i=0; i<nodes.length; i++){
		var text = window.people.val();
		if(typeof(nodes[i].attributes) != "undefined" && nodes[i].attributes.kkid != ""){
			if (s != ''){
				s += ","+nodes[i].text;
			}else{
				s = nodes[i].text;
			}
			if (user_id != ''){
				user_id += ","+nodes[i].attributes.kkid;
			}else{
				user_id = nodes[i].attributes.kkid;
			}
		}
	}
    window.people.val(s);
	window.people.attr("title",user_id);
}