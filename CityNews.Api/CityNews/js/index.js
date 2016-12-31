/**
 * Created by yukang on 2016/6/12.
 */
$(document).ready(function(){
    var swiper = new Swiper('.swiper-container', {
        autoplay:5000,
        pagination: '.swiper-pagination'
    });
    $("#hotChannels h1").click(function(){
        var tabs = $("#hotChannels .tab");
        tabs.hide();
        tabs.eq(0).fadeIn();
    });
    $("#distance0").click(function(){
        var tabs = $("#hotChannels .tab");
        console.log(tabs.length)
        tabs.hide();
        tabs.eq(1).fadeIn();
    });
    $("#lawSet").click(function(){
        var tabs = $("#hotChannels .tab");
        tabs.hide();
        tabs.eq(2).fadeIn();
    });
});
