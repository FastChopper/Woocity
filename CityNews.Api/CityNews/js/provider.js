/**
 * Created by yukang on 2016/6/13.
 */
/*(function ($) {
 $.fn.slideDown = function (duration) {
 // get old position to restore it then
 var position = this.css('position');

 // show element if it is hidden (it is needed if display is none)
 this.show();

 // place it so it displays as usually but hidden
 this.css({
 position: 'absolute',
 visibility: 'hidden'
 });

 // get naturally height
 var height = this.height();

 // set initial css for animation
 this.css({
 position: position,
 visibility: 'visible',
 overflow: 'hidden',
 height: 0
 });

 // animate to gotten height
 this.animate({
 height: height
 }, duration);
 };
 })(Zepto);*/
$(function () {
    $("#tab-btns>a").click(function () {
        $("#provide_content,#myProvide_content,#i_want_provide,#iwantprovide_content,#checkout_result").hide();
        var index = $(this).index();
        $("#tab-btns>a").removeClass("curTab");
        $(this).addClass("curTab");
        if (index == 0) {
            $("#provide_content,#i_want_provide").show();
            $("#i_want_provide").parent("a").show();
        } else if (index == 1) {
            $("#myProvide_content").show();
        }
    });
    $("#i_want_provide").click(function () {
        $("#provide_content,#myProvide_content,#i_want_provide,#checkout_result").hide();
        $("#iwantprovide_content").show();
    });
    // $(".checkout_result_wrap dt").click(function () {
    //     $(this).siblings("dd").slideToggle(300);
    // });
    $(document).on("click",".checkout_result_wrap dt",function () {
        $(this).siblings("dd").slideToggle(300);
    });
    $("#checkout").click(function () {
        var re = /^1[34578]\d{9}$/;
        var $phone = $('.check_item_group input:first').val();
        var $id = $('.check_item_group input:eq(1)').val();
        var $data = {
            Phone: $phone,
            Id: $id
        }
        if (re.test($phone) || $id != "") {
            $.ajax({
                type: "post",
                data: $data,
                url: "http://city.hd.jstv.com/CityNewsApi/SearchBroke",
                success: function (data) {
                    console.log(data);
                    for (var i = 0; i < data.Data.length; i++) {
                        if (data.Data[i].State == 0) {
                            var $dl = $("<dl><dt><span>" + data.Data[i].ID + "</span><span>收到报料,正在等待编辑处理</span></dt></dl>")
                            $('.checkout_result_wrap').append($dl)
                        } else if (data.Data[i].State == 1) {
                            var $dlone = $("<dl><dt><span>" + data.Data[i].ID + "</span><span>分类完毕</span></dt><dd><div class='detail_wrap'><p>" + data.Data[i].IficationTime + "</p><p>" + data.Data[i].HmIficationTime + "</p><p>分类完毕</p></div></dd></dl>");
                            $('.checkout_result_wrap').append($dlone)
                        } else {
                            var $dltwo = $("<dl><dt><span>" + data.Data[i].ID + "</span><span>处理完毕</span></dt><dd><div class='detail_wrap'><p>" + data.Data[i].IficationTime + "</p><p>" + data.Data[i].HmIficationTime + "</p><p>分类完毕</p></div><div class='detail_wrap detail_wrap_a'><p>" + data.Data[i].ExplainTime + "</p><p>" + data.Data[i].HmExplainTime + "</p><p>处理完毕</p><b>" + data.Data[i].Explain + "</b></div></dd></dl>");
                            $('.checkout_result_wrap').append($dltwo)
                        }
                    }
                }
            });
            $('.checkout_result_wrap').find('dl').remove();
            $("#myProvide_content").hide();
            $("#checkout_result").show();

        } else {
            alert("请检查是否填写正确");
            return false;

        }

    });
    $("#file").on("change", function () {
        var files = $(this).get(0).files;
        for (var i = 0; i < files.length; i++) {
            if (files[i].type != "image/jpeg" && files[i].type != "image/png") {
                alert("您上传的" + files[i].name + "件格式不对！");
                return;
            } else {
                var fileReader = new FileReader();
                fileReader.readAsDataURL(files[i]);
                fileReader.onloadend = function (e) {
                    var oDiv = document.createElement("div");
                    var oImg = document.createElement("img");
                    oImg.src = e.target.result;
                    oDiv.appendChild(oImg);
                    $("#files_wrap").prepend(oDiv);
                    $("#iwantprovide_content .item_group input[name=file]").css("float", "left");
                }
            }
        }
    });
    var $windowHeight = $(window).height();
    $('.swiper-container').height($windowHeight);
    $.ajax({
        type: "get",
        url: "http://city.hd.jstv.com/CityNewsApi/brokeList",
        success: function (data) {
            console.log(data);
            for (var i = 0; i < data.Data.length; i++) {
                var $time = data.Data[i].Creatime.split("-");
                var $timezh = $time[0] + "年" + $time[1] + "月" + $time[2] + "日";
                var $bgimg, $bgimgs = $('<div class="list_first_imgs"></div>');
//                    console.log(data.Data[i].FilePathList.length);
                for (var j = 0; j < data.Data[i].FilePathList.length; j++) {
                    $bgimg = $("<li data-bg='" + data.Data[i].FilePathList[j].Path + "' style='background: url(" + data.Data[i].FilePathList[j].PathMin + ") no-repeat center center'></li>")
                    $bgimgs.append($bgimg);
                }
                console.log($bgimgs);
                var $div = $("<div class='provide_list'><div class='list_first'><h1>" + data.Data[i].Title + "</h1><div class='list_first_imgs'>"+ $bgimgs.html() +"</div><p>" + data.Data[i].Contents + "</p><span>" + $timezh + "</span></div></div>")
                $('#provide_content').append($div)
            }
        }
    })
    var $imgurl = [], $videourl = [];
    $('#post_file').change(function () {
        var $val = document.getElementById("post_file").files[0];
        console.log($val)
        var $Formdata = new FormData();
        $Formdata.append("imgs", $val);
        var Req = new XMLHttpRequest();
        Req.open("post", "http://localhost:65463/Upload");
        Req.addEventListener("load", function (data) {
            console.log(data);
            var $data = JSON.parse(data.currentTarget.responseText);

            if ($data.Success == true) {
                var $img = $("<span class='show_pic' style='background: url(" + $data.UrlMin + ") no-repeat center center;background-size: 6rem auto ;' ></span>");
                if ($('#fuckyou span').length == 5) {
                    alert('最多上传5张');
                    return false;
                }
                $('#post_file').before($img)
                    .css('float', "left")
                    .next('p').hide();
                var $datajson = {
                    Path: $data.Url,
                    PathMin: $data.UrlMin,
                    FileName: $data.FileName
                };
                $imgurl.push($datajson);
                console.log(JSON.parse(data.currentTarget.responseText).UrlMin)
            }
        });
        Req.send($Formdata);
//            $.ajax({
//                type:"post",
//                url:"http://city.hd.jstv.com/CityNewsApi/Upload ",
//                data:$Formdata,
//                success:function (data) {
//                        console.log(data)
//                }
//            })
    });
    $('#submit').click(function () {
        var re = /^1[34578]\d{9}$/;
        var $title = $('.item_group_war input:first').val();
        var $tel = $('.item_group_war input:eq(1)').val();
        var $content = $('.item_group textarea').val();
        console.log($content);
        if ($title == "") {
            alert("请填写要爆料的事件");
            return false;
        } else if (!re.test($tel)) {
            alert("请填写正确的手机号码")
            return false;
        } else if ($content == "") {
            alert("请填写爆料内容")
        }
        var $data = {
            Phone: $tel,
            Contents: $content,
            FilePath: $imgurl,
            Title: $title
        };
        $.ajax({
            type: "post",
            url: "http://city.hd.jstv.com/CityNewsApi/Broke",
            data: $data,
            success: function (data) {
                console.log(data)
                if (data.Success == true) {
                    $('#sucLayer').show();
                    $('#sucLayer span').html('您的报料ID是' + data.Id);
                } else {
                    alert(data.Msg)
                }
            }
        })
        console.log($imgurl);
    })
    $('#copyInfo').click(function () {
        $('#sucLayer').hide();
    })
    $(document).on("click",".provide_list",function () {
        if (!$(this).hasClass("provide_list_active")) {
            $(this).addClass('provide_list_active')
        } else {
            $(this).removeClass('provide_list_active')
        }
        ;
    });
    var mySwiper = new Swiper ('.swiper-container', {
        direction: 'horizontal',
        // 如果需要分页器
        /* pagination: '.swiper-pagination',
         paginationType:"fraction",
         paginationFractionRender: function (swiper, currentClassName, totalClassName) {
         return '<span class="' + currentClassName + '"></span>' +
         '/' +
         '<span class="' + totalClassName + '"></span>';
         }*/
    });
    $(document).on("click",".swiper-slide",function () {
        $(".swiper-container").css("visibility","hidden");
        $('.swiper-wrapper').empty();
        mySwiper.update();
        mySwiper.slideTo(0,500)

    })
    $(document).on("click",".list_first_imgs li",function () {
        $('.swiper-container').css("visibility","visible");
        var $index = $(this).parent().find("li").index($(this));
        console.log($index)
        var $swiperarray = [];
        var $swiperlenth = $(this).parent().find("li");
        console.log($swiperlenth)
        for(var i =0;i<$swiperlenth.length;i++){
//                var $databg= $(this).parent();
            $swiperarray.push($swiperlenth[i].attributes[0].nodeValue);
        }
        console.log($swiperarray);
        for(var j =0;j<$swiperarray.length;j++){
            var $swiperli =$("<div class='swiper-slide'><span></span><img src='"+ $swiperarray[j]+"' alt=''></div>");
            $('.swiper-wrapper').append($swiperli)
        }
        mySwiper.update();
        mySwiper.slideTo($index,0);
//            if (!$(this).hasClass("list_first_imgs_active")) {
//                $(this).addClass('list_first_imgs_active')
//            } else {
//                $(this).removeClass('list_first_imgs_active')
//            }
    })

});
