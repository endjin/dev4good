$(document).ready(function () {
    var theDiv = $("div #div_submit_buttons");
    var listOfForms = theDiv.data("forms");
    if (listOfForms != null && listOfForms.length > 0) {
        theDiv.empty();
        for (x in listOfForms) {
            var button = $(document.createElement("div"));
            button.addClass("submit-link");
            button.append("Continue with form " + listOfForms[x]);
            button.data("formId", listOfForms[x]);
            theDiv.append(button);
        }
    }

    $("div.submit-link").click(function (input) {
        var formId = $(input.currentTarget).data("formId");
        var appFormIdField = $("#input_applicationFormId")[0];
        appFormIdField.value = formId;
        var contactDetailsForm = $("#form_contactDetails")[0];
        contactDetailsForm.submit();
    });

})
