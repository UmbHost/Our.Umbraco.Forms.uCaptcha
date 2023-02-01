// reCaptcha callback
function onSubmit(token) {
    // Get form container with the id set earlier
    var frm = document.getElementById(submittedFormId);
    if (frm) {
        // Check we have the correct form by comparing response token
        if (frm.querySelector("[id^='g-recaptcha-response']")) {
            frm.querySelector("[id^='g-recaptcha-response']").value = token;
            // Set reCaptcha field to true
            frm.querySelector(".u-captcha-bool").value = "true";
            // Submit the form
            frm.submit();
        }
    }
}

function validate() {
    // trigger reCaptcha
    grecaptcha.execute();
}

var submitButtons = document.querySelectorAll(".umbraco-forms-form input[type=submit]:not(.cancel)");
// Remove umbraco forms click event
submitButtons.forEach(function (button) {
    if (!button.classList.contains("cancel")) {
        button.removeEventListener("click", function () { });
    }
});


var submittedFormId = null;
for (let i = 0; i < submitButtons.length; i++) {
    var input = submitButtons[i];
    input.addEventListener("click", function (evt) {
        evt.preventDefault();
        var frm = $(this).closest("form");
        resetValidationMessages(frm[0]);
        frm.validate();
        if (frm.valid()) {
            // Set form id for easy form finding on call back
            submittedFormId = frm[0].id;
            // Start reCaptcha process
            validate();
            // Disable submit button
            this.setAttribute("disabled", "disabled");
        }
    }.bind(input));
}

/**
 * Resets the validation messages for a form.
 * @param {Element} formEl the element of the form.
 */
function resetValidationMessages(formEl) {
    var validationErrorMessageElements = formEl.getElementsByClassName('field-validation-error');
    for (var i = 0; i < validationErrorMessageElements.length; i++) {
        validationErrorMessageElements[i].className = 'field-validation-valid';
    }
}