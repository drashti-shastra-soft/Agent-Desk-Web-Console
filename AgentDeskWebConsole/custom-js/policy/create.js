$(document).ready(function () {

    togglePolicyFields();

    $("#PolicyTypeId").change(function () {
        togglePolicyFields();
    });


    $(".btn-submit").click(function () {

        $("#SumAssured").val($("#SumAssured").val().replace(/,/g, ''));
        $("#PremiumAmount").val($("#PremiumAmount").val().replace(/,/g, ''));

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

    $('#PolicyTypeId').change(function () {
        var policyTypeId = $(this).val()
        if (policyTypeId) {
            fnBindPolicyStatus(0, policyTypeId)
        } else {
            const $policyStatus = $('#PolicyStatusId');
            $policyStatus.empty();
            $policyStatus.append($('<option>', {
                value: '',
                text: 'Select Policy Status'
            }));
        }
    });


    $('#SumAssured').mask("#,##,##,##,##,##,##0.00", { reverse: true });
    $('#PremiumAmount').mask("#,##0.00", { reverse: true });

    $('#PolicyNumber').blur(function () {
        var policyNumber = $.trim($('#PolicyNumber').val());
        if (policyNumber == null || policyNumber == "" || policyNumber == undefined) {
            $("#PolicyNumber").next("span").text("Policy Number is required!");
        }
        else {
            CheckPolicyNumberExist(policyNumber);
        }
    });

    $('#PolicyNumber').mask('000000000', {
        translation: {
            '0': { pattern: /[0-9]/ }
        }
    });

    $(function () {
        $('#fupdate').datetimepicker({
            format: 'DD-MM-YYYY',
            defaultDate: moment()
        });

        $('#commercement').datetimepicker({
            format: 'DD-MM-YYYY',
            defaultDate: moment()
        });

        $('#maturity').datetimepicker({
            format: 'DD-MM-YYYY',
            defaultDate: moment()
        });

        $('#startdate').datetimepicker({
            format: 'DD-MM-YYYY',
            defaultDate: moment()
        });

        $('#endDate').datetimepicker({
            format: 'DD-MM-YYYY',
            defaultDate: moment()
        });
    });

    
    function togglePolicyFields() {

        var policyType = $("#PolicyTypeId").val();

        if (policyType == "1") {

            // Life Insurance
            $("#lifeDateFields").show();
            $("#vehicleDateFields").hide();

            $("#sumAssuredDiv").show();
            $("#idvDiv").hide();

            // Show all Pay Modes
            $("#PayMode option").show();
            $("#PayMode").val("");
        }
        else {

            // Health, Property, Vehicle
            $("#lifeDateFields").hide();
            $("#vehicleDateFields").show();

            // Vehicle
            if (policyType == "4") {
                $("#sumAssuredDiv").hide();
                $("#idvDiv").show();
            }
            else {
                $("#sumAssuredDiv").show();
                $("#idvDiv").hide();
            }

            // Only Yearly Pay Mode
            $("#PayMode option").hide();
            $("#PayMode option").filter(function () {
                return $(this).text().trim() == "Yearly";
            }).show().prop("selected", true);
        }
    }
});

function fnValidation() {

    var isValid = true;
    var policyTypeId = $.trim($('#PolicyTypeId').val());
    var policyNumber = $.trim($('#PolicyNumber').val());
    var sumAssured = $.trim($('#SumAssured').val());
    var policyStatusId = $.trim($('#PolicyStatusId').val());
    var fupDate = $.trim($('#FUPDate').val());
    var commercementDate = $.trim($('#CommercementDate').val());
    var payMode = $.trim($('#PayMode').val());
    var maturityDate = $.trim($('#MaturityDate').val());

    // Policy Type
    if (policyTypeId == null || policyTypeId == "" || policyTypeId == undefined) {
        $("#PolicyTypeId").next("span").text("Policy Type is required!");
        isValid = false;
    } else {
        $('#PolicyTypeId').next('span').text("");
    }

    //Policy Status
    if (policyStatusId == null || policyStatusId == "" || policyStatusId == undefined) {
        $("#PolicyStatusId").next("span").text("Policy Status is required!");
        isValid = false;
    } else {
        $('#PolicyStatusId').next('span').text("");
    }

    //Policy Number
    if (policyNumber == null || policyNumber == "" || policyNumber == undefined) {
        $("#PolicyNumber").next("span").text("Policy Number is required!");
        isValid = false;
    }
    else if (policyNumber.length != 9) {
        $("#PolicyNumber").next("span").text("Policy Number must be exactly 9 digits!");
        isValid = false;
    }
    else {
        $("#PolicyNumber").next("span").text("");
    }

    //Pay Mode
    if (payMode == null || payMode == "" || payMode == undefined) {
        $("#PayMode").next("span").text("Pay Mode is required!");
        isValid = false;
    } else {
        $('#PayMode').next('span').text("");
    }

    //Sum Assured
    if (sumAssured == null || sumAssured == "" || sumAssured == undefined || sumAssured == 0) {
        $("#SumAssured").next("span").text("Sum Assured is required!");
        isValid = false;
    } else {
        $('#SumAssured').next('span').text("");
    }

    //Commercement Date
    if (commercementDate == null || commercementDate == "" || commercementDate == undefined) {
        $('#CommercementDate').next('span').text('Commercement Date is required!');
        isValid = false;
    } else {
        $('#CommercementDate').next('span').text('');
    }

    //FUP Date
    if (fupDate == null || fupDate == "" || fupDate == undefined) {
        $('#FUPDate').next('span').text('FUP Date is required!');
        isValid = false;
    } else {
        $('#FUPDate').next('span').text('');
    }

    // Maturity Date
    if (maturityDate == null || maturityDate == "" || maturityDate == undefined) {
        $('#MaturityDate').next('span').text('Maturity Date is required!');
        isValid = false;
    } else {
        $('#MaturityDate').next('span').text('');
    }



    return isValid;
}

function fnBindPolicyStatus(policyStatusId, policyTypeId) {
    fnHandleAjaxRequest({
        url: `/Policy/GetPolicyStatusBy?policyTypeId=${policyTypeId}`,
        dataType: 'json',
        onSuccess: function (resultData) {
            if (resultData && resultData.PolicyStatus) {
                const $policyStatus = $('#PolicyStatusId');
                $policyStatus.empty();
                $policyStatus.append($('<option>', {
                    value: '',
                    text: 'Select Policy Status'
                }));
                resultData.PolicyStatus.forEach(function (policyStatusId) {
                    const intValue = parseInt(policyStatusId.Value, 10);
                    $policyStatus.append($('<option>', {
                        value: policyStatusId.Value,
                        text: policyStatusId.Text
                    }));
                });

                if (policyStatusId) {
                    $policyStatus.val(policyStatusId);
                }
            }
        }
    });
}

function CheckPolicyNumberExist(policyNumber) {
    var mPolicy = {};
    mPolicy.Id = parseInt($('#Id').val());
    mPolicy.PolicyNumber = policyNumber;
    fnHandleAjaxRequest({
        url: `/Policy/CheckPolicyNumberExist`,
        method: 'POST',
        dataType: 'json',
        data: mPolicy,

        onSuccess: function (response) {
            if (response) {
                if (response.StatusCode === "Ok") {
                    if (response.IsExist === true) {
                        $('.btn-submit').removeClass('btn-submit').addClass('disabled-link');
                        $("#PolicyNumber").next("span").text("This policy number already exists.");
                    }
                    else {
                        $('.disabled-link').removeClass('disabled-link').addClass('btn-submit');
                        $("#PolicyNumber").next("span").text("");
                    }
                }
            }
        },

        onError: function () {
            showToastr("Failed to check duplicate policy!");
        }
    });
}

$(document).ready(function () {
    document.querySelectorAll('input[type="date"]').forEach(input => {
        if (input.value) {
            let parts = input.value.split("-");
            input.value = `${parts[2]}-${parts[1]}-${parts[0]}`;
        }
    });
});