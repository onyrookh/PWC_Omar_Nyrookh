var ajaxCallBack = {
    callServerMethod: function (url, parameters, onSuccessFunc, onFailFunc, elementsToBlock, onSessionTimeOut) {

        $.ajax({
            type: "POST",
            url: url,
            data: parameters,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: onSuccessFunc,
            error: function (request, status, error) {
                //var err = eval("(" + request.responseText + ")");

                if (request.status == 409 && rami == 0) {
                    rami = 1;

                    if (globalFunctions.getAppLanguage() == 1) {
                        localizedText = 'You logged in from a different location';
                    } else if (globalFunctions.getAppLanguage() == 4) {
                        localizedText = 'Vous vous êtes connecté depuis un autre endroit';
                    } else if (globalFunctions.getAppLanguage() == 2) {
                        localizedText = ' قمت بالدخول من مكان آخر';
                    } else if (globalFunctions.getAppLanguage() == 3) {
                        localizedText = 'Farklı bir konumdan giriş yaptınız';
                    }

                    globalFunctions.showConcurrentDialog(localizedText);
                }
                else if (!globalFunctions.checkSessionExpiration(request.status, onSessionTimeOut)) {
                    return onFailFunc();
                }
            },
            beforeSend: function (jqXHR, settings) {
                if (elementsToBlock != null && elementsToBlock != 'undefined') {
                    globalFunctions.toggleBlockUI(elementsToBlock, true);
                }
            },
            complete: function (jqXHR, textStatus) {
                if (elementsToBlock != null && elementsToBlock != 'undefined') {
                    globalFunctions.toggleBlockUI(elementsToBlock, false);
                }
            }
        });
    },
    callServerMethodGet: function (url, onSuccessFunc, onFailFunc, elementsToBlock, onSessionTimeOut) {

        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: onSuccessFunc,
            error: function (request, status, error) {
                //var err = eval("(" + request.responseText + ")");
                if (request.status == 409 && rami == 0) {
                    rami = 1;

                    if (globalFunctions.getAppLanguage() == 1) {
                        localizedText = 'You logged in from a different location';
                    } else if (globalFunctions.getAppLanguage() == 4) {
                        localizedText = 'Vous vous êtes connecté depuis un autre endroit';
                    } else if (globalFunctions.getAppLanguage() == 2) {
                        localizedText = ' قمت بالدخول من مكان آخر';
                    } else if (globalFunctions.getAppLanguage() == 3) {
                        localizedText = 'Farklı bir konumdan giriş yaptınız';
                    }

                    globalFunctions.showConcurrentDialog(localizedText);
                }
                else if (!globalFunctions.checkSessionExpiration(request.status, onSessionTimeOut)) {
                    return onFailFunc();
                }
            },
            beforeSend: function (jqXHR, settings) {
                if (elementsToBlock != null && elementsToBlock != 'undefined') {
                    globalFunctions.toggleBlockUI(elementsToBlock, true);
                }
            },
            complete: function (jqXHR, textStatus) {
                if (elementsToBlock != null && elementsToBlock != 'undefined') {
                    globalFunctions.toggleBlockUI(elementsToBlock, false);
                }
            }
        });
    },

    callServerMethodForm: function (url, formData, onSuccessFunc, onFailFunc, elementsToBlock, onSessionTimeOut) {

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: onSuccessFunc,
            error: function (request, status, error) {
                if (request.status == 409 && rami == 0) {
                    rami = 1;

                    if (globalFunctions.getAppLanguage() == 1) {
                        localizedText = 'You logged in from a different location';
                    } else if (globalFunctions.getAppLanguage() == 4) {
                        localizedText = 'Vous vous êtes connecté depuis un autre endroit';
                    } else if (globalFunctions.getAppLanguage() == 2) {
                        localizedText = ' قمت بالدخول من مكان آخر';
                    } else if (globalFunctions.getAppLanguage() == 3) {
                        localizedText = 'Farklı bir konumdan giriş yaptınız';
                    }

                    globalFunctions.showConcurrentDialog(localizedText);
                }
                else if (!globalFunctions.checkSessionExpiration(request.status, onSessionTimeOut)) {
                    return onFailFunc();
                }
            },
            beforeSend: function (jqXHR, settings) {
                if (elementsToBlock != null && elementsToBlock != 'undefined') {
                    globalFunctions.toggleBlockUI(elementsToBlock, true);
                }
            },
            complete: function (jqXHR, textStatus) {
                if (elementsToBlock != null && elementsToBlock != 'undefined') {
                    globalFunctions.toggleBlockUI(elementsToBlock, false);
                }
            }
        });
    },

    prepareJsonObjectParam: function (paramName, paramValue) {
        return paramName + ':' + JSON.stringify(paramValue);
    },
    prepareJsonParamList: function (paramsArray, appendAccid) {
        if (appendAccid != false) {
            if (paramsArray == null) {
                paramsArray = new Array();
            }
            paramsArray.push(ajaxCallBack.prepareJsonObjectParam('accID', globalFunctions.getAccountID()));
        }

        jParams = '{';
        if (paramsArray.length > 0) {
            for (i = 0; i < paramsArray.length; i++) {
                if (i != paramsArray.length - 1) {
                    jParams += paramsArray[i] + ',';
                }
                else {
                    jParams += paramsArray[i];
                }
            }
        }
        jParams += '}';
        return jParams;
    }
};

var globalFunctions = {
    showAlert: function (container, msg, type, autoHide) {
        autoHide = autoHide == null || autoHide == 'undefined' ? false : autoHide;
        var msgDiv = $('#' + container);
        msgDiv.html('');
        msgDiv.append('<div></div>');
        msgDiv = $('#' + container + ' div');
        msgDiv.append('<button type="button" class="close" data-dismiss="alert">&times;</button>');
        msgDiv.append(msg);

        msgDiv.removeClass();
        msgDiv.addClass('alert fade show');
        switch (type) {
            case 1:
                msgDiv.addClass('alert-success');
                break;
            case 2:
                msgDiv.addClass('alert-danger');
                break;
            case 3:
                msgDiv.addClass('alert-info');
        }
        msgDiv.parent().show();
        if (autoHide) {
            msgDiv.fadeIn('fast').delay(2000).fadeOut('fast');
        }
    },
    showConcurrentDialog: function (errorMessage) {
        if (errorMessage != null && errorMessage != 'undefined') {
            bootbox.dialog({
                message: errorMessage,
                title: "",
                buttons: {
                    success: {
                        label: globalFunctions.getAppLanguage() == 2 ? "موافق" : globalFunctions.getAppLanguage() == 4 ? "Fermer" : globalFunctions.getAppLanguage() == 3 ? "Kapatmak" : "Okay",
                        className: "btn-info",
                        callback: function () {
                            globalFunctions.goToURL('/Account/Logout');
                        }
                    }
                }
            });
        }
    },
    showErrorDialog: function (error) {
        if (error != null && error != 'undefined') {
            bootbox.dialog({
                message: error.errorMessage,
                title: "",
                buttons: {
                    success: {
                        label: localizedText.Ok,
                        className: "btn-info",
                        callback: function () {
                            if (error.ID == globalEnums.ServerErrorCode.InvalidAuthentication) {
                                globalFunctions.goToURL(window.location.href);
                            }
                        }
                    }
                }
            });
        }
    },
    showDialog: function (error, title, buttons, closeButton) {
        if (error != null && error != 'undefined') {
            bootbox.dialog({
                message: error.errorMessage,
                title: title,
                buttons: buttons,
                closeButton: closeButton,
            });
        }
    },
    toggleBlockUI: function (elementsToBlock, block) {
        if (elementsToBlock != null && elementsToBlock != 'undefined') {
            block == null || block == 'undefined' ? true : block;

            if (block) {
                elementsToBlock.each(function () {
                    $(this).block({
                        message: null,
                        overlayCSS: {
                            backgroundColor: '#727F8B',
                            opacity: 0.6,
                        }
                    });
                });
            }
            else {
                elementsToBlock.each(function () {
                    $(this).unblock();
                });
            }
        }
        return false;
    },
    getParameterByName: function (name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    },
    goToURL: function (url) {
        if (url != 'undefined' && url != null) {
            window.location.href = url;
        }
        return false;
    },
    validateSpecialChars: function (txt) {
        var isValid = true;
        var usernameRegex = /^([a-zA-Z0-9.]+@){0,1}([a-zA-Z0-9.])+$/;
        if (txt.match(usernameRegex) == null) {
            isValid = false;
        }
        return isValid;
    },
    parseQueryString: function (query) {
        var parts = query.split('&');
        var params = {};
        for (var i = 0, ii = parts.length; i < parts.length; ++i) {
            var param = parts[i].split('=');
            var key = param[0];
            var value = param.length > 1 ? param[1] : null;
            params[decodeURIComponent(key)] = decodeURIComponent(value);
        }
        return params;
    },
    validateEmail: function (email) {
        var isValid = true;
        var emailRegex = /^[a-zA-Z0-9(%:=;,'#*&$~!/^)_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/;
        if (email != null) {
            if (email.match(emailRegex) == null) {
                isValid = false;
            }
        }
        else {
            isValid = false;
        }
        return isValid;
    },
    checkSessionExpiration: function (errID, onSessionTimeOut) {

        var isExpired = false;
        if (errID != null && errID == globalEnums.ServerErrorCode.InvalidAuthurization) {
            if (onSessionTimeOut != null) {
                isExpired = true;
                onSessionTimeOut();
            }
            else {

                globalFunctions.handleSessionExpiration();
            }
        }
        return isExpired;
    },
    handleSessionExpiration: function () {
        $('#SeesionEndModal').modal('show');
        //location.reload();

    },
    getAccountID: function () {
        var hdnAccID = $('#hdnAccID');
        var accID = 0;
        if (hdnAccID != 'undefined' && hdnAccID.val() && hdnAccID.val().length > 0) {
            accID = hdnAccID.val();
        }
        return accID;
    },
    getDateString: function (date, format) {
        if (date != null) {
            switch (format) {
                case 'dmy':
                    return globalFunctions.formatDatePart(date.getDate()) + '/' + globalFunctions.formatDatePart(date.getMonth() + 1) + '/' + date.getFullYear() + " " + date.getHours() + ":" + date.getMinutes() + ":" + date.getMilliseconds();
                case 'mdy':
                    return globalFunctions.formatDatePart(date.getMonth() + 1) + '/' + globalFunctions.formatDatePart(date.getDate()) + '/' + date.getFullYear() + " " + date.getHours() + ":" + date.getMinutes() + ":" + date.getMilliseconds();
                default:
                    return globalFunctions.formatDatePart(date.getMonth() + 1) + '/' + globalFunctions.formatDatePart(date.getDate()) + '/' + date.getFullYear() + " " + date.getHours() + ":" + date.getMinutes() + ":" + date.getMilliseconds();
            }
        }
        else {
            return null;
        }
    },
    formatDatePart: function (part) {
        if (part < 10) {
            return '0' + part;
        }
        return part;
    },
    getAppLanguage: function () {
        return $('#hdnAppLang').val();
    },
    formatAMPM: function (date) {
        var hours = date.getHours();
        var ampm = hours >= 12 ? ' PM' : ' AM';
        return ampm.toString();
    },
    showErrorsMessages: function (errorMsg) {
        globalFunctions.showErrorDialog({ errorMessage: errorMsg });
    },
    setInfiniteScrollButton: function (btnLoadMore) {
        $(window).scroll(function () {
            var MaxAutoLoad = 5;
            if (globalFunctions.isScrolledIntoView(btnLoadMore) && btnLoadMore.attr("data-enable") == 1 && btnLoadMore.attr("data-auto-load") < MaxAutoLoad) {
                var btnAutoLoadCount = parseInt(btnLoadMore.attr("data-auto-load"));
                btnAutoLoadCount++;
                btnLoadMore.attr("data-auto-load", btnAutoLoadCount);
                btnLoadMore.attr('data-enable', '0');
                btnLoadMore.text(btnLoadMore.attr('data-loading-label'));
                return btnLoadMore.click();
            }
        });
    },
    getBrowserType: function () {
        var type = globalEnums.BrowserType.Chrome;
        if (navigator.appVersion.indexOf("MSIE ") != -1)
            type = globalEnums.BrowserType.IE;
        else
            type = globalEnums.BrowserType.Chrome;
        return type;
    },
    getParameterByName: function (name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    },
    isScrolledIntoView: function isScrolledIntoView(elem) {
        var docViewTop = $(window).scrollTop();
        var docViewBottom = docViewTop + $(window).height();

        var elemTop = $(elem).offset().top;
        var elemBottom = elemTop + $(elem).height();

        return ((elemBottom <= docViewBottom) && (elemTop >= docViewTop));
    },
    updateTooltip: function (selector) {
        selector.tooltipster({
            theme: ['tooltipster-borderless']
        });
    },
    CloseWindow: function () {
        window.close();
    },
    showMessageDialog: function (message, title) {
        bootbox.dialog({
            message: message,
            title: title,
            buttons: {
                success: {
                    label: localizedText.Ok,
                    className: "btn-info"
                }
            }
        });
    },
};

var jsonHelper = {
    parseJsonDate: function (jsonDate) {
        if (jsonDate != null && jsonDate != '') {
            return new Date(parseInt(jsonDate.replace(/(^.*\()|([+-].*$)/g, '')));
        }
        else {
            return null;
        }
    },

    parseJsonDateToString: function (jsonDate, format) {
        var date = jsonHelper.parseJsonDate(jsonDate);
        return globalFunctions.getDateString(date, format);

    }
};


//Polyfill Math.trunc if not supported
if (!Math.trunc) {
    Math.trunc = function (v) {
        v = +v;
        return (v - v % 1) || (!isFinite(v) || v === 0 ? v : v < 0 ? -0 : 0);
    };
}