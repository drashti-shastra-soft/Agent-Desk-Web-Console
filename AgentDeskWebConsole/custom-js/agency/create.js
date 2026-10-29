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

    $('#MobileNumber').mask('+91 00000 00000', {
        translation: {
            '0': { pattern: /[0-9]/ }
        },
        onKeyPress: function (val, e, field, options) {
            if (!val.startsWith('+91')) {
                field.val('+91 ' + val.replace(/[^\d]/g, '').substring(0, 10).replace(/(\d{5})(\d{0,5})/, '$1 $2'));
            }
        }
    });

    $('#Code').mask('AAAAA');

    $('#Name').on('input', function () {
        this.value = this.value.replace(/[^a-zA-Z\s]/g, '');
    });
});

function fnValidation() {
    var isValid = true;
    var code = $.trim($('#Code').val());
    var name = $.trim($('#Name').val());
    var emailAddress = $.trim($('#EmailAddress').val());
    var comapanyName = $.trim($('#CompanyName').val());
    var mobileNumber = $.trim($('#MobileNumber').val().replace(/\s+/g, ''));


    //code
    if (code == null || code == "" || code == undefined) {
        $("#Code").next("span").text("Code is required!");
        isValid = false;
    } else {
        $('#Code').next('span').text("");
    }

    // Name
    if (name == null || name == "" || name == undefined) {
        $("#Name").next("span").text("Name is required!");
        isValid = false;
    } else {
        $('#Name').next('span').text("");
    }

    // Email Address
    if (emailAddress == null || emailAddress == "" || emailAddress == undefined) {
        $("#EmailAddress").next("span").text("Email Address is required!");
        isValid = false;
    } else {
        $('#EmailAddress').next('span').text("");
    }

    // Comapany Name
    if (comapanyName == null || comapanyName == "" || comapanyName == undefined) {
        $("#CompanyName").next("span").text("Company Name is required!");
        isValid = false;
    } else {
        $('#CompanyName').next('span').text("");
    }

    // Mobile Number
    if (mobileNumber == null || mobileNumber == "" || mobileNumber == undefined) {
        $('#MobileNumber').next('span').text('Mobile Number is required!');
        isValid = false;
    } else if (!/^\+91\d{10}$/.test(mobileNumber)) {
        $('#MobileNumber').next('span').text('Enter valid 10-digit Mobile Number with +91 prefix!');
        isValid = false;
    } else {
        $('#MobileNumber').next('span').text('');
    }

    return isValid;
}