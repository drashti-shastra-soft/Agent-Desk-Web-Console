$(function () {
    $(document).off('click', '.add-client').on('click', '.add-client', fnCreate);

    //$(document).off('click', '.icon-edit').on('click', '.icon-edit', function (e) {
    //    e.preventDefault();
    //    const id = $(this).data('id');
    //    if (id) fnGet(id);
    //});
    $(document).off('click', '.icon-edit').on('click', '.icon-edit', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnGetClientMetadata(id);
    });

    $(document).off('click', '.icon-delete').on('click', '.icon-delete', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnDelete(id);
    });

    // Address
    $(document).off('click', '.icon-address').on('click', '.icon-address', function (e) {
        e.preventDefault();
        const clientId = $(this).data('id');
        if (clientId) fnGetAddress(clientId);
    });

    $(document).off('click', '.icon-document').on('click', '.icon-document', function (e) {
        e.preventDefault();
        var clientId = $(this).data('id');
        if (!clientId) return;
        window.location.href = '/Policy/Index/' + clientId;
    });

    // Reset pagination and submit the form on Enter key in search fields
    $(document).off('keyup', '.search-text').on('keyup', '.search-text', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            resetPagination();
            $('#frmList').trigger('submit');
        }
    });
    // Submit form when page size filter changes
    $(document).off('change', '.search-dropdown').on('change', '.search-dropdown', function () {
        $('#frmList').trigger('submit');
    });

    $(document).off('change', '.search-pagination').on('change', '.search-pagination', function () {
        $('#frmList').trigger('submit');
    });

    $(document).off('click', '.navigation').on('click', '.navigation', function (e) {
        e.preventDefault();
        var direction = $(this).data('direction').trim();
        if (direction != null && direction != undefined && direction != '') {
            triggerPageNavigation(direction);
        }
        $('#frmList').trigger('submit');
    });
});

function fnCreate() {
    fnHandleAjaxRequest({
        url: '/Client/Create',
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-client').modal('show');
        },
        onError: function () {
            showToastr("Failed to load form.");
        }
    });
}

function fnGetAddress(id) {
    $('#loading-element').show();
    $.ajax({
        url: `/Client/GetAddress/${id}`,
        type: "Get",
        dataType: "html",
        success: function (responseData) {
            $("#divAddress").empty().html(responseData);
            $("#modal-address").modal('show');
            $('#loading-element').hide();
        },
        error: function (xhr, status, error) {
            $('#loading-element').hide();
        }
    });
}


//function fnGet(id) {
//    fnHandleAjaxRequest({
//        url: `/Client/Get/${id}`,
//        onSuccess: function (html) {
//            $('#divCreate').html(html);
//            $('#modal-client').modal('show');
//        }
//    });
//}

function fnGetClientMetadata(id) {
    fnHandleAjaxRequest({
        url: `/Client/GetClientMetadata/${id}`,
        dataType: 'json', // 👈 This ensures automatic parsing
        onSuccess: function (response) {
            if (response.success && response.redirectUrl) {
                window.location.href = response.redirectUrl;
            }
        }
    });
}

function fnDelete(id) {
    fnHandleAjaxRequest({
        url: `/Client/Delete/${id}`,
        dataType: 'json',
        onSuccess: function (response) {
            if (response) {
                showToastr(response.NotificationType, response.NotificationMsg);
                $('#frmList').submit();
            }
        },
        onError: function () {
            showToastr("Failed to delete client.");
        }
    });
}

function triggerPageNavigation(btn) {
    const config = window.paginationConfig;
    const currentSkip = parseInt($('#Pagination_Skip').val()) || 0;

    let newSkip = currentSkip;

    switch (btn) {
        case "First":
            if (config.currentPage > 1) newSkip = 0;
            break;

        case "Next":
            if (config.currentPage < config.totalPage) newSkip += config.pageSize;
            break;

        case "Previous":
            if (currentSkip >= config.pageSize) newSkip -= config.pageSize;
            break;

        case "Last":
            if (config.currentPage < config.totalPage) newSkip = (config.totalPage - 1) * config.pageSize;
            break;

        default:
            return;
    }

    if (newSkip !== currentSkip || btn === "First") {
        $('#Pagination_Skip').val(newSkip);
        $('#frmList').submit();
    }


}