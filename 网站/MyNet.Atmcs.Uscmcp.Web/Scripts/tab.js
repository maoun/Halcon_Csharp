window.onload = function () {

    alert(123)
    function webInit() {

        var agr = Array.prototype.slice.call(arguments)
        for (var i = 0; i < agr.length; i++) {
            switch (agr[i]) {
                case "Menulist":

                    Menulist();//左侧菜单
                    break;
            }
        }
    }



    /*左侧菜单animate*/

    function Menulist() {

       alert(123)
       

       /* $("#PanelLeft").find("table:first-child").stop(true, true).addClass("active").animate({ width: '150px' }); */

        $("#PanelLeft").each(function () {


            $(this).find(".ex-panel-background").hover(function () {
               
                $(this).stop(true, true).addClass("active").animate({ width: '150px' });
               
            
              

                $(this).siblings().stop(true, true).removeClass("active").css({ width: '66px' });

            });

        })

    }
    Menulist()

    




       

}
   

