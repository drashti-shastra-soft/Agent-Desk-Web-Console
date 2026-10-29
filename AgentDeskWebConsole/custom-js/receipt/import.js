$(document).ready(function () {
    $("#importFile").change(function () {

        var fileUpload = $("#importFile").get(0);
        var files = fileUpload.files;

        if (files.length === 0) return;

        var fileData = new FormData();
        fileData.append("file", files[0]);
        fnHandleAjaxRequest({
            url: "/Receipt/Import",
            method: "POST",
            dataType: "json", // assuming JSON response
            contentType: false,
            processData: false,
            data: fileData,
            onSuccess: function (resultData) {
                var $tbody = $("#tblImportInvoice tbody");
                $tbody.empty();
                if (resultData) {
                    var validationCount = 0;

                    $.each(resultData, function (index, item) {
                        var errorHtml = "";
                        if (item.Validation && item.Validation.length > 0) {
                            errorHtml = `<ol>`;
                            $.each(item.Validation, function (i, msg) {
                                errorHtml += `<li>${msg.replace(/"/g, '&quot;')}</li>`;
                            });
                            errorHtml += `</ol>`;
                            validationCount = item.Validation.length;
                        }
                        var row = `
                                <tr id="tr_${index}" class="main-row">
                                    <td>${item.No}</td>
                                    <td>${item.ClientName}</td>
                                    <td>${item.PolicyNumber}</td>
                                    <td>${item.Plan}</td>
                                    <td>${item.DueDate}</td>
                                    <td>${item.RiskDate}</td>
                                    <td>${item.CBO}</td>
                                    <td>${item.AdjustmentDate}</td>
                                    <td>${item.Premium}</td>
                                    <td>${item.Commission}</td>
                                </tr>
                            `;
                        $tbody.append(row);
                        
                    });

                    if (parseInt(validationCount) == 0) {
                        $('#btn-upload').removeClass('btn-default').addClass('btn-success');
                    }
                }
            },
            onError: function () {
                showToastr("Failed to import recipt!");
            }
        });

    });



});