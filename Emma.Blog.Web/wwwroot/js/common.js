var api_domain = "http://localhost:21222";
$.ajaxSetup({
    dataType: 'json',
    xhrFields: {
        withCredentials: true
    }
});

$(document).ajaxSend(function(e, xhr, settings) {
    settings.url = api_domain + settings.url;
});
