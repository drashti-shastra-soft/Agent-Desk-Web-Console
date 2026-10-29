$(document).ready(function () {

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

    $('#EmailAddress').on('input', function () {
        let val = $(this).val();

        // Allow only valid email characters
        val = val.replace(/[^a-zA-Z0-9@._\-]/g, '');

        // Prevent double @ symbols
        const parts = val.split('@');
        if (parts.length > 2) {
            val = parts[0] + '@' + parts[1];
        }

        $(this).val(val);
    });

    $('#Code').mask('AAAAA');

    $('#Name').on('input', function () {
        this.value = this.value.replace(/[^a-zA-Z\s]/g, '');
    });

    $('#ContactPerson').on('input', function () {
        this.value = this.value.replace(/[^a-zA-Z\s]/g, '');
    });

    $('#Line1').on('input', function () {
        this.value = this.value.replace(/[^a-zA-Z0-9\s,.-]/g, '');
    });

    $('#Line2').on('input', function () {
        this.value = this.value.replace(/[^a-zA-Z0-9\s,.-]/g, '');
    });

    $('#City').on('input', function () {
        this.value = this.value.replace(/[^a-zA-Z\s]/g, '');
    });

    $('#State').on('input', function () {
        this.value = this.value.replace(/[^a-zA-Z\s]/g, '');
    });

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

    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    var name = $.trim($('#Name').val());
    var code = $.trim($('#Code').val());
    var line1 = $.trim($('#Line1').val());
    var city = $.trim($('#City').val());
    var state = $.trim($('#State').val());
    var emailAddress = $.trim($('#EmailAddress').val());
    var contactPerson = $.trim($('#ContactPerson').val());
    var mobileNumber = $.trim($('#MobileNumber').val().replace(/\s+/g, ''));


    // Name
    if (name == null || name == "" || name == undefined) {
        $("#Name").next("span").text("Name is required!");
        isValid = false;
    } else {
        $('#Name').next('span').text("");
    }

    //code
    if (code == null || code == "" || code == undefined) {
        $("#Code").next("span").text("Code is required!");
        isValid = false;
    } else {
        $('#Code').next('span').text("");
    }

    // Line1
    if (line1 == null || line1 == "" || line1 == undefined) {
        $('#Line1').next('span').text('Address Line 1 is required!');
        isValid = false;
    } else {
        $('#Line1').next('span').text('');
    }

    // City
    if (city == null || city == "" || city == undefined) {
        $('#City').next('span').text('City is required!');
        isValid = false;
    } else {
        $('#City').next('span').text('');
    }

    // State
    if (state == null || state == "" || state == undefined) {
        $('#State').next('span').text('State is required!');
        isValid = false;
    } else {
        $('#State').next('span').text('');
    }

    // Email Address
    if (emailAddress == null || emailAddress == '' || emailAddress == undefined) {
        $('#EmailAddress').next('span').text('Email Address is required!');
        isValid = false;
    } else if (emailAddress != null && emailAddress != '' && emailAddress != undefined) {
        if (!emailRegex.test(emailAddress)) {
            $('#EmailAddress').next('span').text('Email Address is invalid!');
            isValid = false;
        } else {
            $('#EmailAddress').next('span').text('');
        }
    } else {
        $('#EmailAddress').next('span').text('');
    }

    // Contact Person
    if (contactPerson == null || contactPerson == "" || contactPerson == undefined) {
        $('#ContactPerson').next('span').text('Contact Person is required!');
        isValid = false;
    } else {
        $('#ContactPerson').next('span').text('');
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