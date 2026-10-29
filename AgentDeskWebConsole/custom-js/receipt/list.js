$(function () {
    $(document).off('click', '.add-receipt').on('click', '.add-receipt', fnCreate);

    $(document).off('click', '.icon-edit').on('click', '.icon-edit', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnGet(id);
    });

    $(document).off('click', '.icon-pdf').on('click', '.icon-pdf', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnGetPdf(id);
    });

    $(document).off('click', '.icon-delete').on('click', '.icon-delete', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnDelete(id);
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
    })

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
        url: '/Receipt/Create',
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-receipt').modal('show');
        },
        onError: function () {
            showToastr("Failed to load form.");
        }
    });
}

function fnGet(id) {
    fnHandleAjaxRequest({
        url: `/Receipt/Get/${id}`,
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-receipt').modal('show');
        }
    });
}

function fnGetPdf(id) {
    fnHandleAjaxRequest({
        url: `/Receipt/GetPdf/${id}`,
        onSuccess: async function (resultData) {

            // Parse the JSON string into an object
            var parsed = (typeof resultData === "string") ? JSON.parse(resultData) : resultData;

            // Build data URI
            var pdfDataUri = "data:application/pdf;base64," + parsed.Base64String;

            // Convert to Blob using fetch
            const response = await fetch(pdfDataUri);
            const blob = await response.blob();

            // Create object URL
            const blobUrl = URL.createObjectURL(blob);

            // Open in new tab
            window.open(blobUrl, "_blank");
        }
    });
}

function fnDelete(id) {
    fnHandleAjaxRequest({
        url: `/Receipt/Delete/${id}`,
        dataType: 'json',
        onSuccess: function (response) {
            if (response) {
                showToastr(response.Type, response.Message);
                $('#frmList').submit();
            }
        },
        onError: function () {
            showToast("Failed to delete receipt.");
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