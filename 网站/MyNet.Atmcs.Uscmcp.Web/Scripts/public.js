function webInit() {
    var agr = Array.prototype.slice.call(arguments)
    for (var i = 0; i < agr.length; i++) {
        switch (agr[i]) {
            case "Menulist":
                Menulist();
                break;
        }
    }
}

/*左侧菜单animate*/

window.onload = function () {
    alert(1233)
}
function Menulist() {
    //插入显示器
    $.fn.ckSlide = function (opts) {
        opts = $.extend({}, $.fn.ckSlide.opts, opts);
        var screena = $(this);
        var freq = opts.frequency;
        if (freq == 1) {
            screena.append("<div class='dropzone1'>1</div>");
        } else { } if (freq == 2) {
            screena.append("<div class='dropzone1'>1</div><div class='dropzone2'>2</div>");
        } else { } if (freq == 3) {
            screena.append("<div class='dropzone1'>1</div><div class='dropzone2'>2</div><div class='dropzone3'>3</div>");
        } else { } if (freq == 4) {
            screena.append("<div class='dropzone1'>1</div><div class='dropzone2'>2</div><div class='dropzone3'>3</div><div class='dropzone4'>4</div>");
        } else { } if (freq == 5) {
            screena.append("<div class='dropzone1'>1</div><div class='dropzone2'>2</div><div class='dropzone3'>3</div><div class='dropzone4'>4</div><div class='dropzone5'>5</div>");
        } else { } if (freq == 6) {
            screena.append("<div class='dropzone1'>1</div><div class='dropzone2'>2</div><div class='dropzone3'>3</div><div class='dropzone4'>4</div><div class='dropzone5'>5</div><div class='dropzone6'>6</div>");
        } else { } if (freq == 7) {
            screena.append("<div class='dropzone1'>1</div><div class='dropzone2'>2</div><div class='dropzone3'>3</div><div class='dropzone4'>4</div><div class='dropzone5'>5</div><div class='dropzone6'>6</div><div class='dropzone7'>7</div>");
        } else { }
    }

    //跳转URL
    $.fn.Jumpurl = function (opts) {
        opts = $.extend({}, $.fn.Jumpurl.opts, opts);
        var d = $(this).dad({ fre: opts.frer });
        var target1 = $('.dropzone1');
        var target2 = $('.dropzone2');
        var target3 = $('.dropzone3');
        var target4 = $('.dropzone4');
        var target5 = $('.dropzone5');
        var target6 = $('.dropzone6');
        var target7 = $('.dropzone7');
        d.addDropzone(target1, function (e) {
            var oDivWs = window.screen.width;
            var oDivHs = window.screen.height;
            var url = e.find('span').attr("dir");

            $(".dads-children-clone").remove();
            window.open(url, "_blank", "toolbar=yes, location=yes, directories=no, status=no, menubar=yes, scrollbars=yes, resizable=yes,copyhistory=yes,left=" + oDivWs + "px,top=0,width=" + oDivWs + ",height=" + oDivHs + "")
        });
        d.addDropzone(target2, function (e) {
            var oDivWs = window.screen.width;
            var oDivHs = window.screen.height;
            var url = e.find('span').attr("dir");

            $(".dads-children-clone").remove();
            window.open(url, "_blank", "toolbar=yes, location=yes, directories=no, status=no, menubar=yes, scrollbars=yes, resizable=yes, copyhistory=yes,left=" + oDivWs * 2 + "px,top=0,width=" + oDivWs + ",height=" + oDivHs + "")
        });
        d.addDropzone(target3, function (e) {
            var oDivWs = window.screen.width;
            var oDivHs = window.screen.height;
            var url = e.find('span').attr("dir");

            $(".dads-children-clone").remove();
            window.open(url, "_blank", "toolbar=yes, location=yes, directories=no, status=no, menubar=yes, scrollbars=yes, resizable=yes, copyhistory=yes,left=" + oDivWs * 3 + "px,top=0,width=" + oDivWs + ",height=" + oDivHs + "")
        });
        d.addDropzone(target4, function (e) {
            var oDivWs = window.screen.width;
            var oDivHs = window.screen.height;
            var url = e.find('span').attr("dir");

            $(".dads-children-clone").remove();
            window.open(url, "_blank", "toolbar=yes, location=yes, directories=no, status=no, menubar=yes, scrollbars=yes, resizable=yes, copyhistory=yes,left=" + oDivWs * 4 + "px,top=0,width=" + oDivWs + ",height=" + oDivHs + "")
        });
        d.addDropzone(target5, function (e) {
            var oDivWs = window.screen.width;
            var oDivHs = window.screen.height;
            var url = e.find('span').attr("dir");

            $(".dads-children-clone").remove();
            window.open(url, "_blank", "toolbar=yes, location=yes, directories=no, status=no, menubar=yes, scrollbars=yes, resizable=yes, copyhistory=yes,left=" + oDivWs * 5 + "px,top=0,width=" + oDivWs + ",height=" + oDivHs + "")
        });
        d.addDropzone(target6, function (e) {
            var oDivWs = window.screen.width;
            var oDivHs = window.screen.height;
            var url = e.find('span').attr("dir");

            $(".dads-children-clone").remove();
            window.open(url, "_blank", "toolbar=yes, location=yes, directories=no, status=no, menubar=yes, scrollbars=yes, resizable=yes, copyhistory=yes,left=" + oDivHs * 6 + "px,top=0,width=" + oDivWs + ",height=" + oDivHs + "")
        });
        d.addDropzone(target7, function (e) {
            var oDivWs = window.screen.width;
            var oDivHs = window.screen.height;
            var url = e.find('span').attr("dir");

            $(".dads-children-clone").remove();
            window.open(url, "_blank", "toolbar=yes, location=yes, directories=no, status=no, menubar=yes, scrollbars=yes, resizable=yes, copyhistory=yes,left=" + oDivHs * 7 + "px,top=0,width=" + oDivWs + ",height=" + oDivHs + "")
        });
    }
}

//左侧点击选择模板 效果

$(function () {
    $("#PanelLeftMenu img").hover(function () {
        $(this).nextAll("span").css("color", "#fb5004");
    })
    $("#PanelLeftMenu img").mouseleave(function () {
        $(this).nextAll("span").css("color", "black");
    })

    $("#seletMenu").click(function () {
        if (!$("#seletMenu").hasClass("active")) {
            $("#PanelLeftMenu").addClass('active').animate({ "left": "-2px" })
            $("#seletMenu").addClass('active').animate({ "left": "230px" })
        }
        else {
            $("#PanelLeftMenu").addClass('active').animate({ "left": "-232px" })
            $("#seletMenu").removeClass('active').animate({ "left": "0px" })
        }
    })

    $("#PanelLeftMenu img").hover(function () {
        if (!$(this).hasClass("active")) {
            var src = $(this).attr("src");
            var src1 = src.substring(0, 28) + "_1.png";

            $(this).addClass("activeenter").attr("src", src1);
        }

        $("#PanelLeftMenu img.activeenter").mouseleave(function () {
            if ($("#PanelLeftMenu img.activeenter").length == 1) {
                var src = $(this).attr("src");
                var src1 = src.substring(0, 28) + ".png";

                $(this).removeClass("activeenter").attr("src", src1);
            }
        })
        //点击事件
        $(this).click(function () {
            //找到已有active的图
            var src2 = $("#PanelLeftMenu img.active").attr("src").substring(0, 28) + ".png";
            $("#PanelLeftMenu img.active").removeClass("active").attr("src", src2).next("span").css("color", "black");

            if (!$(this).hasClass("active")) {
                $(this).removeClass("activeenter");
                var src = $(this).attr("src");
                var src1 = src.substring(0, 28) + "_1.png";
                $(this).addClass("active").attr("src", src1);
            }
        })
    })
});