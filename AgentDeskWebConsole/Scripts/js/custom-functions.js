function fnValidateEmail(email) {
    if (!email || typeof email !== 'string') return false;
    const regex = /^\w+([-+.'']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    return regex.test(email.trim());
}

function toggleLoading(show) {
    $('#loading-element').toggle(show);
}
function resetPagination() {
    $('#Pagination_Skip').val(0);
    $('#Pagination_TotalRecord').val(0);
}

function fnHandleAjaxRequest({ url, method = 'GET', dataType = 'html', data = null, onSuccess, onError })
{
    $.ajax({
        url: url,
        method: method,
        dataType: dataType,
        data: data,
        beforeSend: function () {
            $('#loading-element').show();
        },
        success: function (response) {
            toggleLoading(false);
            if (typeof onSuccess === 'function') {
                onSuccess(response);
            }
        },
        error: function () {
            toggleLoading(false);
            if (typeof onError === 'function') {
                onError();
            }
        },
        complete: function () {
            $('#loading-element').hide();
        }
    });
}

function resetPagination() {
    $('#Pagination_Skip').val(0);
    $('#Pagination_TotalRecord').val(0);
}

