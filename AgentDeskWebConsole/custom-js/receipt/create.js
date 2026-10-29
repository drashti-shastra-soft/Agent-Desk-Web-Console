$(document).ready(function () {


    //$('#PremiumAmount, #CDCharge, #TotalAmount, #LateFee').on('input', function () {
    //    let value = $(this).val().replace(/,/g, '').replace(/\D/g, '');
    //    if (value) {
    //        let lastThree = value.substring(value.length - 3);
    //        let otherNumbers = value.substring(0, value.length - 3);
    //        if (otherNumbers !== '')
    //            lastThree = ',' + lastThree;
    //        value = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
    //    }
    //    $(this).val(value);
    //});

    $('#PremiumAmount').mask("#,##0.00", { reverse: true });
    $('#LateFee').mask("#,##0.00", { reverse: true });
    $('#CDCharge').mask("#,##0.00", { reverse: true });
    $('#TotalAmount').mask("#,##,##,##,##,##,##0.00", { reverse: true });


    function calculateTotal() {

        var premium = parseFloat($('#PremiumAmount').val()) || 0;
        var lateFee = parseFloat($('#LateFee').val()) || 0;
        var cdCharges = parseFloat($('#CDCharge').val()) || 0;

        var total = premium + lateFee + cdCharges;

        $('#TotalAmount').val(total.toFixed(2));
    }

    $(document).on('input', '#LateFee, #CDCharge', function () {
        calculateTotal();
    });

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });


    $(document).off('blur', '#PolicyNumber').on('blur', '#PolicyNumber', function () {

        var policyNumber = $.trim($(this).val());

        if (policyNumber) {
            $('#loading-element').show();
            $.ajax({
                url: '/Receipt/GetPolicyDetails',
                type: 'GET',
                data: { policyNumber: policyNumber },
                success: function (response) {
                    if (response && response.success) {
                        $('#PolicyId').val(response.policyId);
                        $('#ClientId').val(response.clientId);
                        $('#ClientName').val(response.name);
                        $('#PremiumAmount').val(response.premiumAmount);

                        calculateTotal();

                    } else {
                        showToastr("error", "Policy not found!");
                        $('#PolicyId').val('');
                        $('#ClientId').val('');
                        $('#ClientName').val('');
                        $('#PremiumAmount').val('');
                    }
                },
                error: function () {
                    showToastr("error", "Error fetching policy details!");
                },
                complete: function () {
                    $('#loading-element').hide();
                }
            });

        } else {

            $('#ClientName').val('');
            $('#PremiumAmount').val('');
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

    var date = $.trim($('#Date').val());
    var policyNumber = $.trim($('#PolicyNumber').val());
    var totalAmount = $.trim($('#TotalAmount').val());

    // Date
    if (date == null || date == "" || date == undefined) {
        $("#Date").next("span").text("Date is required!");
        isValid = false;
    } else {
        $('#Date').next('span').text("");
    }

    //Policy Number
    if (policyNumber == null || policyNumber == "" || policyNumber == undefined) {
        $("#PolicyNumber").next("span").text("Policy Number is required!");
        isValid = false;
    } else {
        $('#PolicyNumber').next('span').text("");
    }

    //Total Amount
    if (totalAmount == null || totalAmount == "" || totalAmount == undefined) {
        $("#TotalAmount").next("span").text("Total Amount is required!");
        isValid = false;
    } else {
        $('#TotalAmount').next('span').text("");
    }

    return isValid;
}