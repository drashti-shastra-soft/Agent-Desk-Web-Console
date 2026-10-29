$(document).ready(function () {

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });

    var notificationType = $.trim($('#NotificationType').val());
    var notificationMsg = $.trim($('#NotificationMsg').val());

    if (notificationType != null && notificationType != undefined && notificationType != '') {
        if (notificationMsg != null && notificationMsg != undefined && notificationMsg != '') {
            showToastr(notificationType, notificationMsg);
            $('.modal').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
            $('#frmList').submit();
        }
    }

});

function fnValidation() {
    var isValid = true;

    var name = $.trim($('#Name').val());
    var age = $.trim($('#Age').val());
    var relation = $.trim($('#Relation').val());


    //age
    if (name == null || name == "" || name == undefined) {
        $("#Name").next("span").text("Name is required!");
        isValid = false;
    } else {
        $('#Name').next('span').text("");
    }

    //age
    if (age == null || age == "" || age == undefined) {
        $("#Age").next("span").text("Age is required!");
        isValid = false;
    } else {
        $('#Age').next('span').text("");
    }

    // Relation
    if (relation == null || relation == "" || relation == undefined) {
        $("#Relation").next("span").text("Relation is required!");
        isValid = false;
    } else {
        $('#Relation').next('span').text("");
    }

    return isValid;
}