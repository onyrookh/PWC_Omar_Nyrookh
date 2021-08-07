; var Complaint = (function () {

    var elements = {
        btnSave: $("#btnSave"),
        txtMessage: $("#txtMessage"),
        ComplaintID: $("#ComplaintID"),
        StatusID: $("#StatusID")

    };

    var resources = {
        actionURLs: {
            AddEditComplaintUrl: '/api/ComplaintApi/AddEditComplaint',
        },
        messages: {
            LoadingMsg: "Loading...",
            PleaseFillReasonOfRejecting: null,
            PleaseSelectResult: null
        }
    };

    var initializer = {
        init: function () {
            initializer.initEventHandlers();
            debugger;
            $("#status").val(elements.StatusID.val());
        },
        initEventHandlers: function () {
            elements.btnSave.click(eventHandlers.onbtnSaveClicked);
        },
    };

    var eventHandlers = {
        onbtnSaveClicked: function () {
            debugger;
            var statusID = helpers.getSelectedStatusValue();

                var params = new Array();
                params.push(ajaxCallBack.prepareJsonObjectParam('StatusID', helpers.getSelectedStatusValue()));
                params.push(ajaxCallBack.prepareJsonObjectParam('Message', elements.txtMessage.val()));
                params.push(ajaxCallBack.prepareJsonObjectParam('ComplaintID', elements.ComplaintID.val()));
                ajaxCallBack.callServerMethod(
                    resources.actionURLs.AddEditComplaintUrl,
                    ajaxCallBack.prepareJsonParamList(params, false),
                    function (data) {
                        var oError = data;
                        if (oError != null) {
                            if (oError.isSuccess) {
                                window.location = "/Complaint/Index";
                            }
                            else {
                                globalFunctions.showErrorDialog(oError);
                            }
                        }
                    }, function () {
                        globalFunctions.showErrorsMessages("an error occurred, please try again later");
                    },$("main"));
            }
    };

    var helpers = {
        getSelectedStatusValue: function () {
            var statusID = $("select[name=status]").val();
            if (statusID != undefined && statusID != '' && statusID != null) {
                return parseInt(statusID);
            }
            return 0;
        }
    };


    return {
        init: function () {
            initializer.init();
        }
    }

})();

$(document).ready(function () {
    Complaint.init();
});
