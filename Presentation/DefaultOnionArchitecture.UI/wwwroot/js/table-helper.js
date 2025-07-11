let table;
function checkLoader(loaderTable) {
    if ($(loaderTable).closest(".dataTables_wrapper .loader").length > 0) {
        $(loaderTable).closest(".dataTables_wrapper .loader").remove();
    } else {
        $(loaderTable).closest(".dataTables_wrapper").append(`<div class="loader">
				<div class="spinner-border"></div>
		</div>`)
    }
}

function fetchData(url) {
    let lang = currentLang;
    checkLoader(tableId)
    $.ajax({
        url,
        type: 'GET',
        data: { lang },
        success: function (response) {
            if (response.result == 1) {
                checkLoader(tableId)
                initializeDataTable(response.data);
            }
        },
        error: function () {
            checkLoader(tableId)
            alert('Veriler alınamadı!');
        }
    });
}

function initializeDataTable(items) {
    let oldLangHtml;
    if ($(".currency-switcher").length > 0) {
        oldLangHtml = $(".currency-switcher .caption").html()
    }
    table = CreateDataTable(tableId, tableSettings, oldLangHtml);

    formatData(items)
}

function CreateDataTable(element, tableSettings = {}, langOldSelectHtml) {
    const {
        addButton = null,
        hasPaging = true,
        hasOrdering = true,
        colDefs = [],
        isBtn = true,
        href = null,
        columns = [],
        pageLength = 10,
        isSpecialbar = false,
    } = tableSettings;

    return $(element).DataTable({
        language: {
            lengthMenu: "_MENU_",
            search: "",
            emptyTable: "Tabloda herhangi bir veri mevcut değil",
            zeroRecords: "Tabloda herhangi bir veri bulunamadı",
            paginate: {
                first: "İlk",
                last: "Son",
                next: "Sonraki",
                previous: "Önceki"
            },
            loadingRecords: '&nbsp;',
            processing: '<div class="spinner"></div>'
        },
        columns,
        pageLength,
        orderCellsTop: true,
        processing: true,
        columnDefs: colDefs,
        lengthMenu: [[10, 30, 50, 100, 500, -1], [10, 30, 50, 100, 500, "Hepsi"]],
        order: [[0, "desc"]],
        ordering: hasOrdering,
        bDestroy: true,
        autoWidth: false,
        paging: hasPaging,
        info: false,
        initComplete: function (settings, json) {
            $('.dataTables_length select').addClass('form-select');

            const filterContainer = $(element).closest(".dataTables_wrapper").find('.dataTables_filter input').closest(".dataTables_filter");

            if (isSpecialbar) {
                let selectedHtml =` <img src="/assets/images/flags/turkey.png" alt="TR" width="32" height="17">
                            Türkçe
                            <span>TR</span>`

                if (langOldSelectHtml) {
                    selectedHtml = langOldSelectHtml
                }

                $(element).before(`<div class='special-bar'>
                <div class="currency-switcher">
                      <div class="flag-dropdown">
                        <div class="caption">
                            ${selectedHtml}
                        </div>
                        <div class="list">
                        <div class="item" data-item="1">
                            <img src="/assets/images/flags/turkey.png" alt="TR" width="32" height="17">
                            Türkçe
                            <span>TR</span>
                          </div>
                          <div class="item" data-item="2">
                            <img src="/assets/images/flags/usa.png" alt="USA" width="32" height="17">
                            İngilizce
                            <span>EN</span>
                          </div>                         
                        </div>
                      </div>
                    </div>
                </div>`)
            }

            if (isBtn) {
                if (addButton) {
                    if (addButton.id) {
                        filterContainer.append(
                            `<button class="btn btn-success data-add-btn" id='${addButton.id}'>${addButton.text}</button>`
                        );
                    } else if (addButton.modalId) {
                        filterContainer.append(
                            `<button class="btn btn-success data-add-btn" data-bs-toggle="modal" data-bs-target="${addButton.modalId}">${addButton.text}</button>`
                        );
                    }
                }
            } else if (href) {
                filterContainer.append(
                    `<a class="btn btn-success" href='${href.link}'>${href.text}</a>`
                );
            }

            $('.dataTables_filter input').addClass('form-control').attr('placeholder', 'Ara');
        }
    });
}

$("body").on("click", ".data-add-btn", function () {
    if (currentLang && $("[name='lang']").length > 0) {

        $("[name='lang']").val(currentLang)
    }
})

function removeDataTableRow(rowId, anotherTable) {
    var rowIndex = getRowIndexByDataId(rowId, anotherTable)
    if (anotherTable) {
        anotherTable.row(rowIndex).remove().draw(false);
    } else {

        table.row(rowIndex).remove().draw(false);
    }
}

function addDataTableRow(data, dataId, anotherTable) {
    if (anotherTable) {
        var rowNode = anotherTable.row
            .add(data)
            .draw()
            .node();
        anotherTable.order([0, 'desc']).draw();
    } else {
        var rowNode = table.row
            .add(data)
            .draw()
            .node();
        table.order([0, 'desc']).draw();
    }


    $(rowNode).attr('data-id', dataId);

    return rowNode;
}

let globalDataId;

function updateDataTableRow(data, dataId, anotherTable) {
    if (dataId) {

        var rowIndex = getRowIndexByDataId(dataId, anotherTable)
    } else {
        var rowIndex = getRowIndexByDataId(globalDataId, anotherTable)
    }

    if (anotherTable) {

        var rowNode = anotherTable.row(rowIndex)
            .data(data)
            .draw()
            .node();
    } else {

        var rowNode = table.row(rowIndex)
            .data(data)
            .draw()
            .node();

        table.draw()
    }
    return rowNode;
}

function getRowIndexByDataId(dataId, anotherTable) {
    var rowIndex = -1;
    if (anotherTable) {
        anotherTable.rows().every(function (index) {
            if ($(this.node()).attr('data-id') == dataId) {
                rowIndex = index;
                return false;
            }
        });
    } else {
        table.rows().every(function (index) {
            if ($(this.node()).attr('data-id') == dataId) {
                rowIndex = index;
                return false;
            }
        });
    }


    return rowIndex;
}

$(document).ready(function () {

    $.fn.dataTable.ext.type.order['date-dd-mm-yyyy-pre'] = function (d) {
        var parts = d.split('.');
        return new Date(parts[2], parts[1] - 1, parts[0]);
    };

    $.fn.dataTable.ext.type.order['time-hh-mm-pre'] = function (d) {
        var parts = d.split(':');
        return parseInt(parts[0]) * 60 + parseInt(parts[1]); 
    };

$.fn.dataTable.ext.type.order['datetime-dd-mm-yyyy-hh-mm-pre'] = function (d) {
    var datetimeParts = d.split(' ');
    var dateParts = datetimeParts[0].split('.');
    var timeParts = datetimeParts[1].split(':');
    
    return new Date(dateParts[2], dateParts[1] - 1, dateParts[0], timeParts[0], timeParts[1]);
};

    $.fn.dataTable.ext.type.order['currency-pre'] = function (data) {
        var expression = /((\(\₺))|(\₺\()/g;

        if (data.match(expression)) {
            data = '-' + data.replace(/[\₺\(\),.]/g, '');
        } else {
            data = data.replace(/[\₺\,.]/g, '');
        }
        
        return parseInt(data, 10);
    };
})