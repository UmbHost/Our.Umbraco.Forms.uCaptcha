// Turnstile callback
function onSubmit(token) {
    // Find form which triggered Turnstile
    var uf = document.querySelectorAll("form");
    uf.forEach(function (form) {
        var turnstileResponse = form.querySelector("[name^='g-recaptcha-response']");
        if (turnstileResponse) {
            turnstileResponse.value = token;
            // Set hidden field to true if response matches
            form.querySelector('.u-captcha-bool').value = "true";
        }
    });
}
