$(document).ready(function () {
    var theDiv = $("div #div_submit_buttons");
    theDiv.empty();
    var button = $(document.createElement("div"));
    button.addClass("submit-link");
    button.append("Continue with form 1");
    button.data("formId", 1);
    theDiv.append(button);

    button = $(document.createElement("div"));
    button.addClass("submit-link");
    button.append("Continue with form 2");
    button.data("formId", 2);
    theDiv.append(button);



    $("div.submit-link").click(function (input) {
        alert("you requested to continue with form " + $(input.currentTarget).data("formId"));
    });

})
