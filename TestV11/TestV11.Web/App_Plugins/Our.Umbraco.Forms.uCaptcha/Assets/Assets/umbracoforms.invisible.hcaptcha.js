var submittedFormId = null;

// hCaptcha callback
function onSubmit(token) {
    // Get form container with the id set earlier
    var frm = document.getElementById(submittedFormId);
    if (frm) {
        var form = frm.querySelector('form');
        // Check we have the correct form by comparing response token
        if (form && form.querySelector("[id^='h-captcha-response']")) {
            form.querySelector("[id^='h-captcha-response']").value = token;
            // Set hCaptcha field to true
            form.querySelector(".u-captcha-bool").value = "true";
            // Submit the form
            form.submit();
        }
    }
}

function validate() {
    // trigger hCaptcha
    hcaptcha.execute();
}

// Remove umbraco forms click event
var submitButtons = document.querySelectorAll(".umbraco-forms-form input[type=submit]");
submitButtons.forEach(function (button) {
    if (!button.classList.contains("cancel")) {
        button.removeEventListener("click", function () { });
    }
});

// Replace with hCaptcha trigger
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
                // Start hCaptcha process
                validate();
                // Disable submit button
                button.setAttribute("disabled", "disabled");
            }
        });
    }
});