$(function(){
    $("#tab-btns>a").click(function(){
        $("#part_action,#detail_action,#sponsor,.hintLayer").hide();
        var index = $(this).index();
        $("#tab-btns>a").removeClass("curTab");
        $(this).addClass("curTab");
        if(index == 0 ){
            $("#part_action").show();
        }else if(index == 1){
            $("#sponsor").show();
        }
    });
    $(document).on("click","#part_action .per",function(){
        $("#part_action,#detail_action,#sponsor,.hintLayer").hide();
        $("#detail_action").show();
        var $id = $(this).find('img[data-id]').attr('data-id');
        $.ajax({
            type:"get",
            url:"http://city.hd.jstv.com/CityNewsApi/GetActityByID?id="+$id,
            dataType:'json',
            success:function (data) {
                console.log(data)
                // $('.')
                $('.detail_wrap').find('img').attr('src',data.Data[0].FilePath);
                $('.detail_wrap').find('img').attr('data-id',data.Data[0].ID);
                $('.detail_wrap h1').html(data.Data[0].Name);
                $('.detail_wrap dl:first dd p').html(data.Data[0].Contents)
                $('.detail_wrap dl:eq(1) dd p').html(data.Data[0].Arrange)
                $('.detail_wrap dl:eq(2) dd p').html(data.Data[0].AddTime +"至" +data.Data[0].EndTime)
                $('.detail_wrap dl:eq(3) dd p').html(data.Data[0].PersonCount)

            }
        })
    });
    $("#file").on("change",function(){
       var files =  $("#file").get(0).files;
        for(var i=0;i<files.length;i++){
           if(files[0].type != "image/jpeg" && files[0].type != "image/png"){
               // alert("您上传的"+files[i].name+"文件格式不对！");
           }else{
               var fileReader  = new FileReader();
               fileReader.readAsDataURL(files[0]);
               fileReader.onloadend = function(event){
                   var oImg = document.createElement("img");
                    oImg.src = event.target.result;
                    $("#upload_imgs").append(oImg);
                   // oImg.onload=function(){
                   //    alert($(this).width());
                   //     alert($(this).height());
                   //     alert($(this).width()/$(this).height());
                   // }
               }
           }
        }

    });
    var orupload;
    var $imgurl = [];
    var wHeight = $(window).height();
    $('#clipArea').height(wHeight - 50);
    //document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);
    $("#clipArea").photoClip({
        width: 200,
        height: 200,
        file: "#post_file",
        view: "#picker",
        ok: "#clipBtn",
        loadStart: function () {
            console.log("照片读取中");
        },
        loadComplete: function () {
            console.log("照片读取完成");
        },
        clipFinish: function (dataURL) {
            console.log(dataURL.split(","));
            var $filename = $('#post_file').val();
            orupload=$filename;
            $json = {
                Path: dataURL.split(",")[1],
                FileName: $filename,
            }
            $.ajax({
                type: "post",
                url: "http://city.hd.jstv.com/CityNewsApi/Uploader",
                data: $json,
               
                success: function (data) {
                    console.log(data)
                    if (data.Success == true) {
//                            $imgurl.push(data.Url);
//                            $imgname.push(data.FileName)
                        var $imgjson = {
                            Path:data.Url,
                            FileName:data.FileName,
                            PathMin:"1"
                        };
                        $imgurl.push($imgjson)
                    }else{
                        alert("失败")
                    }
                }

            })
        }
    });
    $.ajax({
        type: 'get',
        url: "http://city.hd.jstv.com/CityNewsApi/GetActity",
        datatype: "json",
        success: function (data) {
            console.log(data);

            for (var i = 0; i < data.Data.length; i++) {
                console.log(data.Data.leader)
                var $div = "<div class='per'><div class='per_wrap'><div class='per-fuck'><div class='per-fuck-o'><p>" + data.Data[i].Name + "</p><div class='per-fuck-p'><span><img src='images/User-Profile.png' alt=''>活动发起人&nbsp;" + data.Data[i].Leader + "</span><span><img src='images/Shape-33.png' alt=''>" + data.Data[i].StartTime.substring(5) + "&nbsp;至&nbsp;" + data.Data[i].EndTime.substring(5) + "</span></div></div><img data-id='" + data.Data[i].ID + "' src='" + data.Data[i].FilePath + "' alt=''></div></div></div>"
                $("#part_action").append($div)
            }

        }
    })
    $('#post_file').change(function () {
        $('.htmleaf-container').css('visibility', "visible")
    });
    $('#clipBtn').click(function () {
        $('.htmleaf-container').css({"visibility": "hidden"});
        $('#picker input').css("display", "none")
    })

    $('input[name=time]:eq(3)').change(function () {
        $('input[name=time]:eq(0)').val(" ")
    });
    $('input[name=time]:eq(4)').change(function () {
        $('input[name=time]:eq(1)').val(" ")
    });
    $('input[name=time]:eq(5)').change(function () {
        $('input[name=time]:eq(2)').val(" ")
    });
    function hclose() {
        setTimeout(function () {
            $('.hintLayer').hide();
        }, 3000)
    }

    $('#signup').click(function () {
        var re = /^1[34578]\d{9}$/;
        var $ID = $('.detail_wrap').find('img').attr('data-id');
        var $phone = $('.detail_wrap input:first').val();
        var $name = $('.detail_wrap input:eq(1)').val();
        if (!re.test($phone)) {
            alert("请检查手机号码是否正确")
            return false;
        } else if ($name == "") {
            alert("请检查姓名");
            return false;
        } else {
            var info = {
                Id: $ID,
                Phone: $phone,
                Name: $name
            }
            $.ajax({
                type: 'post',
                url: 'http://city.hd.jstv.com/CityNewsApi/ActitySign?id=' + $ID,
                data: info,
                dataType: 'json',
                success: function (data) {
                    console.log(data);
                    if (data.Success == true) {
                        $('#submitLayer').show();
                        hclose()
                    } else {
                        $('.hintLayer-lose').show();
                        hclose();
                    }
                }
            })

        }
    });
    $('#submit').click(function () {
        var re = /^1[34578]\d{9}$/;

        var $name = $('.item_group input:first').val();
        var $person = $('.item_group input:eq(1)').val()
        var $phone = $('.item_group input:eq(2)').val()
        var $contents = $('.item_group textarea:first').val();
        var $Arrange = $('.item_group textarea:eq(1)').val();
        var $startTime = $('.input-p input:eq(3)').val();
        var $endTime = $('.input-p input:eq(4)').val();
        var $signendTime = $('.input-p input:eq(5)').val();
//            console.log(url)
        if ($name == "") {
            alert("请检查活动名称");
            return false
        } else if ($person == "") {
            alert("请检查活动发起人");
            return false
        }else if (!re.test($phone) ) {
            alert("请检查活动发起人手机号码");
            return false
        } else if ($contents == "") {
            alert("请检查活动详情");
            return false
        } else if ($Arrange == "") {
            alert("请检查活动安排");
            return false
        } else if ($startTime == "") {
            alert("请检查开始时间");
            return false
        } else if ($endTime == "") {
            alert("请检查结束时间");
            return false
        } else if ($signendTime == "") {
            alert("请检查报名截止时间");
            return false
        } else if (orupload.length == 0) {
            alert("请检查是否上传图片");
            return false;
        }
        var $data = {
            Name: $name,
            Leader: $person,
            Contents: $contents,
            Phone:$phone,
            SignEndTime: $signendTime,
            EndTime: $endTime,
            StartTime: $startTime,
            Arrange: $Arrange,
            FilePath:$imgurl
        };
        $.ajax({
            type: "post",
            url: "http://city.hd.jstv.com/CityNewsApi/ActitiesList ",
            data: $data,
            success: function (data) {
                console.log(data);
                if (data.Success == true) {
                    $('#sponsorLayer').show();
                    setTimeout(function() {
                        $('#sponsorLayer').hide();
                        location.reload();
                    }, 3000);
                   
                }
            }
        })
    })
})