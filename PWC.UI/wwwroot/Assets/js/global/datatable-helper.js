; var DataTableHelper = (function () {
    var elements = {

    };
    var variables = {

    };
    var helpers = {
        InitDataTable: function (elementDt, sAjaxSource, fnServerData, columnDefs, columns, selectStyle, responsive) {
            return elementDt.DataTable({
                responsive: responsive || (responsive == null || responsive == undefined),
                serverSide: true,
                bFilter: false,
                bSortCellsTop: true,
                order: [[1, "asc"]],
                pagingType: "full_numbers",
                sAjaxSource: sAjaxSource,
                fnServerData: fnServerData,
                columnDefs: columnDefs,
                columns: columns,
                select: selectStyle,
                scrollX: true,
                language: {
                    infoFiltered: ""
                }
            });
        }
    };
    return {
        InitDataTable: function (elementDt, sAjaxSource, fnServerData, columnDefs, columns, selectStyle, responsive) {
            return helpers.InitDataTable(elementDt, sAjaxSource, fnServerData, columnDefs, columns, selectStyle, responsive);
        }
    }
})();