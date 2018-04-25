var api_domain = "http://localhost:21222";
//var api_domain = "http://api.songlin.net.cn"
$.ajaxSetup({
    dataType: 'json',
    xhrFields: {
        withCredentials: true
    }
});

$(document).ajaxSend(function(e, xhr, settings) {
    settings.url = api_domain + settings.url;
    xhr.setRequestHeader('Authorization', 'Bearer ' + getCookie('token'));
});

function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}

function setCookie(c_name, value, expire) {
    var exdate = new Date()
    exdate.setDate(exdate.getTime() + expire * 1000)
    document.cookie = c_name + "=" + escape(value) + ";path=/;" +
        ((expire == null) ? "" : ";expires=" + exdate.toGMTString())
}