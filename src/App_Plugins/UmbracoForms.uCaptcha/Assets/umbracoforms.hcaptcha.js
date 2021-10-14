//hCaptcha callback
function onSubmit(token) {
    //Find form which triggered hCaptcha
    var uf = $("form");
    uf.each(function () {
        if ($(this).find('.h-captcha-response').val(token)) {
            //Set hidden field to true if response matches
            $(this).find('.u-captcha-bool').val("true");
        }
    });
}