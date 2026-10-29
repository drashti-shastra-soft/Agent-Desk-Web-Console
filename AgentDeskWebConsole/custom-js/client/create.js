$(document).ready(function () {

    $('#PhoneNumber').mask('+91 00000 00000', {
        translation: {
            '0': { pattern: /[0-9]/ }
        },
        onKeyPress: function (val, e, field, options) {
            if (!val.startsWith('+91')) {
                field.val('+91 ' + val.replace(/[^\d]/g, '').substring(0, 10).replace(/(\d{5})(\d{0,5})/, '$1 $2'));
            }
        }
    });


    $('#firstName').on('input', function () {
        this.value = this.value.replace(/[^a-zA-Z\s]/g, '');
    });

    $('#lastName').on('input', function () {
        this.value = this.value.replace(/[^a-zA-Z\s]/g, '');
    });

    $('#reservationdate').datetimepicker({
        format: 'DD-MM-YYYY',
        defaultDate: moment()
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

            showToastr(notificationType, notificationMsg)
            $('.modal').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
            $('#frmList').submit();
        }
    }
});

function fnValidation() {
    var isValid = true;

    var firstName = $.trim($("#FirstName").val());
    var lastName = $.trim($("#LastName").val());
    var phoneNumber = $.trim($('#PhoneNumber').val().replace(/\s+/g, ''));
    var gender = $.trim($("#Gender").val());
    var groupId = $.trim($("#GroupId").val());


    //First Name
    if (firstName == null || firstName == "" || firstName == undefined) {
        $("#FirstName").next("span").text("First Name is required!");
        isValid = false;
    } else {
        $("#FirstName").next("span").text("");
    }

    //Last Name
    if (lastName == null || lastName == "" || lastName == undefined) {
        $("#LastName").next("span").text("Last Name is required!");
        isValid = false;
    } else {
        $("#LastName").next("span").text("");
    }


    // Phone Number
    if (phoneNumber == null || phoneNumber == "" || phoneNumber == undefined) {
        $('#PhoneNumber').next('span').text('Phone Number is required!');
        isValid = false;
    } else if (!/^\+91\d{10}$/.test(phoneNumber)) {
        $('#PhoneNumber').next('span').text('Enter valid 10-digit Mobile Number with +91 prefix!');
        isValid = false;
    } else {
        $('#PhoneNumber').next('span').text('');
    }

    // Gender
    if (gender == null || gender == "" || gender == undefined || gender == "Select Gender") {
        $("#Gender").next("span").text("Gender is required!");
        isValid = false;
    } else {
        $("#Gender").next("span").text("");
    }

    // Group
    if (groupId == null || groupId == "" || groupId == undefined || groupId == "0") {
        $('#GroupId').next('span').text('Group is required!');
        isValid = false;
    } else {
        $('#GroupId').next('span').text('');
    }

    return isValid;
}