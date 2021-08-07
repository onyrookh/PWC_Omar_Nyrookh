; var ReviewThesisList = (function () {

    var elements = {
        tabulatorThesisList: $("#tabulator-thesis-list"),
        btnTabulatorSearch: $("#btnTabulatorSearch"),
        txtFilterTitle: $("#txtFilterTitle"),
        txtFilterAuthor: $("#txtFilterAuthor"),
        dllFilterStatus: $("#dllFilterStatus"),
        mainContainer: $("#main_container")
    };

    var variables = {
        table: null
    };

    var resources = {
        actionURLs: {
            GetComplaintUrl: '/api/ComplaintApi/GetComplaintList',
        },
        messages: {
            LoadingMsg: "Loading...",
        }
    };

    var initializer = {
        init: function () {
            initializer.initControls();
            initializer.initEventHandlers();
        },
        initControls: function () {
            var langs = {
                "en": {
                    "ajax": {
                        "loading": localizedText.TabulatorText.Loading,
                        "error": localizedText.TabulatorText.Error,
                    },
                    "pagination": {
                        "first": localizedText.TabulatorText.First,
                        "first_title": localizedText.TabulatorText.FirstPageToolTip,
                        "last": localizedText.TabulatorText.Last,
                        "last_title": localizedText.TabulatorText.LastPageToolTip,
                        "prev": localizedText.TabulatorText.Previous,
                        "prev_title": localizedText.TabulatorText.PreviousPageToolTip,
                        "next": localizedText.TabulatorText.Next,
                        "next_title": localizedText.TabulatorText.NextPageToolTip,
                        "page_size": localizedText.TabulatorText.PageSize,
                    },
                }
            };

            variables.table = new Tabulator(elements.tabulatorThesisList.selector, {
                ajaxLoaderLoading: $("<div style='display:inline-block;'><img src='/Assets/images/loader.gif' /></div>")[0],
                layout: "fitColumns",
                langs: langs,
                pagination: "remote",
                ajaxURL: resources.actionURLs.GetComplaintUrl,
                ajaxConfig: "POST",
                ajaxContentType: "json",
                placeholder: localizedText.NoDataFound,
                ajaxSorting: true,
                paginationSize: 10,
                paginationSizeSelector: [10, 20, 50, 100],
                paginationDataSent: {
                    "page": "PageNumber",
                    "size": "PageSize",
                    "sorters": "FieldSort",
                },
                paginationDataReceived: {
                    "last_page": "totalPages", //change last_page parameter name to "max_pages"
                    "current_page": "pageNumber",
                },
                columns: [
                    { title: "Complaint ID", field: "complaintID", width: 150, headerSort: false},
                    { title: "Message", headerTooltip: "Message", field: "message", formatter: "textarea", headerSort: false},
                    { title: "Username", field: "username", headerTooltip: "Username", headerSort: false, width: 120},
                    { title: "Status", headerTooltip: "Status", field: "statusName", width: 120, headerSort: false},
                    {
                        title: "Edit", headerTooltip: "Edit", cssClass: 'text-center', headerSort: false, width: localizedText.ThesisListColumns.ReviewButtonWidth, formatter: function (cell, formatterParams, onRendered) {
                            return "<a href='/Complaint/AddEdit/" + cell.getData().complaintID + "' class='btn-success btn btn-sm' id='btnReview'> Edit </a>"
                        },
                    }
                ],
                initialSort: [
                    { column: "complaintID", dir: "desc" }
                ],
                ajaxParams: { ThesisTitle: helpers.getTitleFilter(), AuthorName: helpers.getAuthorNameFilter(), Status: helpers.getStatusFilter() },
                ajaxRequesting: function (url, params) {
                    params.ThesisTitle = helpers.getTitleFilter();
                    params.AuthorName = helpers.getAuthorNameFilter();
                    params.Status = helpers.getStatusFilter();
                    return true; // return false if you would to stop the request
                },
                ajaxResponse: function (url, params, response) {
                    //url - the URL of the request
                    //params - the parameters passed with the request
                    //response - the JSON object returned in the body of the response.
                    var TableData = {
                        data: [],
                        pageNumber: 1,
                        pageSize: 5,
                        totalPages: 1,
                        totalRows: 0
                    };

                    if (response.status.isSuccess)
                        TableData = response.data;

                    return TableData; //return the tableData property of a response json object
                },
                ajaxError: function (xhr, textStatus, errorThrown) {
                    //xhr - the XHR object
                    //textStatus - error type
                    //errorThrown - text portion of the HTTP status
                    globalFunctions.checkSessionExpiration(xhr.status, null)
                },
            });
            variables.table.setLocale("en");
        },
     
    };


    var helpers = {
        getTitleFilter: function () {
            return elements.txtFilterTitle.val();
        },
        getAuthorNameFilter: function () {
            return elements.txtFilterAuthor.val();
        },
        getStatusFilter: function () {
            return elements.dllFilterStatus.val();
        },
        showDeleteThesisConfirmationMessage: function (thesisID) {
            bootbox.confirm({
                message: localizedText.ThesisListColumns.DeleteThesisConfirmationMessage,
                buttons: {
                    confirm: {
                        label: localizedText.ThesisListColumns.OkButton,
                        className: 'btn-danger'
                    },
                    cancel: {
                        label: localizedText.ThesisListColumns.CancelButton,
                        className: 'btn-default'
                    }
                },
                callback: function (result) {
                    if (result == true) {
                        helpers.deleteThesis(thesisID);
                    }
                }
            });
        },
       
    };


    return {
        init: function () {
            initializer.init();
        },
        afterLoadInit: function () {
            initializer.afterLoadInit();
        }
    }

})();

$(document).ready(function () {
    ReviewThesisList.init();
});
