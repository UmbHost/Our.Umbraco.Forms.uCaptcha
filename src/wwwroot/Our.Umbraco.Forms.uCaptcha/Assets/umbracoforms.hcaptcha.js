// hCaptcha callback
function onSubmit(token) {
    // Find form which triggered hCaptcha
    var uf = document.querySelectorAll("form");
    uf.forEach(function (form) {
        var hCaptchaResponse = form.querySelector("[id^='h-captcha-response']");
        if (hCaptchaResponse) {
            hCaptchaResponse.value = token;
            // Set hidden field to true if response matches
            form.querySelector('.u-captcha-bool').value = "true";
        }
    });
}