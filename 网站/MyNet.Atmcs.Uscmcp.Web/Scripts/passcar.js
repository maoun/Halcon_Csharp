
        
//导出的html页面，显示图片方法
function showImageInfo(id)
{
    $("img[no="+id+"]").each(
    function(){

    window.top.frames('rightFrame').document.getElementById('table1').rows[0].cells[0].innerText=document.getElementById('Table1').rows[id+1].cells[3].innerText;
    if(this.type=='0')
    {
        window.top.frames('rightFrame').Image_1.src= this.src;
    }
    else if(this.type=='1')
    {
        window.top.frames('rightFrame').Image_2.src= this.src;
    }
    else if(this.type == '2')
    {
        window.top.frames('rightFrame').Image_3.src=this.src;
    } 
    
    
    });
}

 