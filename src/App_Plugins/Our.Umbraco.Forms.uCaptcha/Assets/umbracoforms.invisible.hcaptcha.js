var submittedFormId = null;

//hCaptcha callback
function onSubmit(token) {
    //Get form container with the id set earlier
    var frm = $("#" + submittedFormId);
    var form = frm.find('form').first();
    //Check we have the correct form by comparing response token
    if (form.length > 0 && form.find(".h-captcha-response").val(token)) {
        //Set hCaptcha field to true
        form.find(".u-captcha-bool").val("true");
        //Submit the form
        form.submit();
    }
}

function validate() {
    //trigger hCaptcha
    hcaptcha.execute();
}

//Remove umbraco forms click event
$(".umbraco-forms-form input[type=submit]").not(".cancel").off('click');

//Replace with hCaptcha trigger
$(".umbraco-forms-form input[type=submit]").not(".cancel").click(function (evt) {
    evt.preventDefault();
    var self = $(this);
    var frm = self.closest("form");
    //Validate the form as per usual Umbraco forms way
    frm.validate();
    if (frm.valid()) {
        //Set form id for easy form finding on call back
        submittedFormId = frm.parent(".umbraco-forms-form").attr('id');
        //Start hCaptcha process
        validate();
        //Disable submit button
        self.attr("disabled", "disabled");
    }
});