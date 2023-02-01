var submittedFormId = null;

// Turnstile callback
function onSubmit(token) {
    // Get form container with the id set earlier
    var frm = document.getElementById(submittedFormId);
    if (frm) {
        var form = frm.querySelector('form');
        // Check we have the correct form by comparing response token
        if (form && form.querySelector("[id^='g-captcha-response']")) {
            form.querySelector("[id^='g-captcha-response']").value = token;
            // Set Turnstile field to true
            form.querySelector(".u-captcha-bool").value = "true";
            // Submit the form
            form.submit();
        }
    }
}

function validate() {
    // trigger Turnstile
    grecaptcha.execute();
}

// Remove umbraco forms click event
var submitButtons = document.querySelectorAll(".umbraco-forms-form input[type=submit]");
submitButtons.forEach(function (button) {
    if (!button.classList.contains("cancel")) {
        button.removeEventListener("click", function () { });
    }
});

// Replace with Turnstile trigger
submitButtons.forEach(function (button) {
    if (!button.classList.contains("cancel")) {
        button.addEventListener("click", function (evt) {
            evt.preventDefault();
            var frm = button.closest("form");
            // Validate the form as per usual Umbraco forms way
            frm.validate();
            if (frm.valid()) {
                // Set form id for easy form finding on call back
                submittedFormId = frm.parentNode.id;
                // Start Turnstile process
                validate();
                // Disable submit button
                button.setAttribute("disabled", "disabled");
            }
        });
    }
});