//Functions for loading data in
function checkApplyOrOkClicked(button) {
    if (button.includes("OK")) {
        $("#CloseFormOnSubmit").val("Y");
    }
    else {
        $("#CloseFormOnSubmit").val("N");
    }
}

function attachListFunctions(
    listButtonClass,
    objectIDField
) {
    if (!objectIDField) {
        return true;
    }
    else if ($("#" + objectIDField).length > 0) {
        $("." + listButtonClass).click(function (event) {
            //Set active object ID - input field must exist
            var objectID = $(this).data("id");
            var formTitle = $(this).data("loading-text");
            $("#" + objectIDField).val(objectID);

            if (formTitle) {
                $("#FormTitleID").val(formTitle);
            }
        });
    }
    else {
        doErrorModal("Object ID is invalid", "#" + objectIDField + " does not exist");
    }

    //$(".dataList th").click(function (event) {
    //    //For showing filter popup
    //    event.preventDefault();
    //    //Get field and value
    //    var fld = $(this).find("a").attr("target");

    //    if (fld != null) {
    //        var order = $(this).find("a").attr("href").trim();
    //        var curSort = $("#SortQuery").val();

    //        if (order !== "") {
    //            $("#SortQuery").val(curSort + '!' + fld + ',' + order);
    //        }
    //        else {
    //            $("#SortQuery").val(curSort + '!' + fld);
    //        }

    //        $("#doSearch").submit();
    //    }
    //});

    $(".dataList td").click(function (event) {
        showFilterAndSort(this);
    });
    $(".dataList td").contextmenu(function (event) {
        showFilterAndSort(this);
    });
}

function showFilterAndSort(elem) {
    event.preventDefault();

    //Hide any other tooltips
    $(".dataList").find('td').popover('hide');

    //Get field and value
    var fld = $(elem).find("a").attr("target");

    if (fld != null) {
        var val = $(elem).find("a").html().trim();
        //Store values in popup
        $("#FilterCol").val(fld);
        $("#FilterVal").val(val);

        $(elem).popover({
            trigger: 'manual',
            html: true,
            title: function () {
                return "Filter and Sort";
            },
            content: function () {
                return $("#FilterPanel").html();
            }
        }).popover('show');

        attachFilterFunctions();
    }
}

function attachFilterFunctions() {
    $(".FilterButton").click(function (event) {
        event.preventDefault();

        var col = $("#FilterCol").val();
        var comp = $(this).attr("aria-label");
        var val = $("#FilterVal").val();
        var curFilter = $("#FilterQuery").val();

        //Handle checkboxes
        if (val.indexOf('checked="checked"') > -1) {
            val = "true";
        }
        else if (val.indexOf('check-box') > -1) {
            val = "false";
        }

        //Replace commas with |
        val = val.replace(/[,]/gm, "|");

        //Replace spaces with _
        val = val.replace(/[ ]/gm, "_");

        //Remove pence
        val = val.replace(".00", "");

        //Store values in popup
        $("#FilterComp").val(comp);

        //Clear search
        if (comp === "X") {
            $("#FilterQuery").val("");
        }
        //If blank value filtered on
        else if (!val) {
            $("#FilterQuery").val(curFilter + '!' + col + ',NULL');
        }
        else {
            $("#FilterQuery").val(curFilter + '!' + col + ',' + comp + ',' + val);
        }
        
        $("#doSearch").submit();
    });

    $(".FilterSearch").keypress(function (e) {
        if (e.which === 13) {
            event.preventDefault();

            var col = $("#FilterCol").val();
            var comp = "EQ";
            var val = $(this).val();
            var curFilter = $("#FilterQuery").val();

            //Replace commas with |
            val = val.replace(/[,]/gm, "|");

            //Replace spaces with _
            val = val.replace(/[ ]/gm, "_");

            //Remove pence
            val = val.replace(".00", "");

            //If value contains % or * then switch to a like
            val = val.replace(/[*]/gm, "%");
            if (val.indexOf("%") > -1) {
                comp = "LK";
            }

            $("#FilterQuery").val(curFilter + '!' + col + ',' + comp + ',' + val);
            $("#doSearch").submit();
        }

        
    });
}

$(".ClearSortButton").click(function (event) {
    event.preventDefault();

    $("#SortQuery").val("");
    $("#doSearch").submit();
});

$(".ClearCurrentFiltersButton").click(function (event) {
    event.preventDefault();

    $("#FilterQuery").val("");
    $("#doSearch").submit();
});

function loadList(
    loadIntoDivID,
    relativeURL,
    listObjectID,
    listToRefresh,
    listQueryParams,
    listButtonClass,
    listSortCol,
    listSortOrder,
    objectIDField,
) {
    var dataToLoad;

    if (!listSortCol) {
        listSortCol = "1";
    }
    if (!listSortOrder) {
        listSortOrder = "asc";
    }

    if (listObjectID.length > 0) {
        dataToLoad = "/" + relativeURL + "/Index/" + listObjectID + listQueryParams + " #" + listToRefresh;
    }
    else {
        dataToLoad = "/" + relativeURL + "/Index/" + listQueryParams + " #" + listToRefresh;
    }

    $("#" + loadIntoDivID).load(dataToLoad, function (responseText, textStatus, req) {
        if (textStatus === "error") {
            doErrorModal("Error Loading " + dataToLoad, "The list at " + dataToLoad + " returned a server error and could not be loaded");
        }
        else {
            attachListFunctions(
                listButtonClass,
                objectIDField
            );
            console.log(dataToLoad + " Loaded");
            //Need jquery datatables plugin
            var table = $(".dataList").DataTable();
            table
                .order([listSortCol, listSortOrder])
                .draw();

            //Show any alerts from the remote page
            var alerts = $(".Alerts", responseText).html();
            if (alerts) {
                alerts = alerts.trim();
            }
            if (alerts) {
                var alertList = alerts.split("|");
                var alertHtml = "";

                for (var i = 0; i < alertList.length; i++) {
                    alertHtml += "<p>" + alertList[i] + "</p>";
                    //Do something
                }

                doModal("Customer Alerts", alertHtml);
            }
        }
    });
}

function loadInputForm(
    loadFormIntoDivID,
    loadListIntoDivID,
    relativeURL,
    objectID,
    parentObjectID,
    formToLoad,
    formToSubmit,
    listToRefresh,
    listQueryParams,
    listButtonClass,
    listSortCol,
    listSortOrder,
    objectIDField,
    closeModalOnSuccess,
    modelToClose
) {
    var dataToLoad;
    if (objectID.length > 0) {
        dataToLoad = "/" + relativeURL + "/Edit/" + objectID + " #" + formToLoad;
    }
    else if (parentObjectID.length > 0) {
        dataToLoad = "/" + relativeURL + "/Create/" + parentObjectID + " #" + formToLoad;
    }
    else {
        dataToLoad = "/" + relativeURL + "/Create/ #" + formToLoad;
    }

    $("#" + loadFormIntoDivID).load(dataToLoad, function (responseText, textStatus, req) {
        if (textStatus === "error") {
            doErrorModal("Error Loading Form " + formToLoad, "The form at " + dataToLoad + " returned a server error and could not be loaded");
        }
        else {
            attachSubmitInputForm(
                loadFormIntoDivID,
                loadListIntoDivID,
                relativeURL,
                objectID,
                parentObjectID,
                formToSubmit,
                listToRefresh,
                listQueryParams,
                listButtonClass,
                listSortCol,
                listSortOrder,
                objectIDField,
                closeModalOnSuccess,
                modelToClose
            );
            console.log(dataToLoad + " Loaded");
        }
    });
}

function loadDeleteForm(
    loadFormIntoDivID,
    loadListIntoDivID,
    relativeURL,
    objectID,
    parentObjectID,
    formToLoad,
    formToSubmit,
    listToRefresh,
    listQueryParams,
    listButtonClass,
    listSortCol,
    listSortOrder,
    objectIDField,
    closeModalOnSuccess,
    modelToClose
) {
    var dataToLoad;
    if (objectID.length > 0) {
        dataToLoad = "/" + relativeURL + "/Delete/" + objectID + " #" + formToLoad;
        $("#" + loadFormIntoDivID).load(dataToLoad, function (responseText, textStatus, req) {
            if (textStatus === "error") {
                doErrorModal("Error Loading Form " + formToLoad, "The form at " + dataToLoad + " returned a server error and could not be loaded");
            }
            else {
                attachSubmitDeleteForm(
                    loadFormIntoDivID,
                    loadListIntoDivID,
                    relativeURL,
                    objectID,
                    parentObjectID,
                    formToSubmit,
                    listToRefresh,
                    listQueryParams,
                    listButtonClass,
                    listSortCol,
                    listSortOrder,
                    objectIDField,
                    closeModalOnSuccess,
                    modelToClose
                );
                console.log(dataToLoad + " Loaded");
            }
        });
    }
    else {
        doErrorModal("Object ID not valid", "Object ID does not have a valid value");
    }
}

function attachSubmitInputForm(
    loadFormIntoDivID,
    loadListIntoDivID,
    relativeURL,
    objectID,
    parentObjectID,
    formToSubmit,
    listToRefresh,
    listQueryParams,
    listButtonClass,
    listSortCol,
    listSortOrder,
    objectIDField,
    closeModalOnSuccess,
    modelToClose
) {
    var form = $("#" + formToSubmit);
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);

    extraSubmitFormFunctions();

    form.submit(function (event) {
        event.preventDefault();

        //If existing item then update
        if (objectID.length > 0) {
            //If no unobtrusive validation errors
            if (form.valid()) {
                $.ajax({
                    type: "POST",
                    url: "/" + relativeURL + "/Edit/" + objectID,
                    data: form.serialize(),
                    success: function (data) {
                        if (closeModalOnSuccess === true) {
                            if ($("#CloseFormOnSubmit").val() === "Y") {
                                $("#" + modelToClose).modal("hide");
                            }
                            var audio = new Audio("/sounds/confirm.wav");
                            audio.play();
                        }
                        loadList(
                            loadListIntoDivID,
                            relativeURL,
                            parentObjectID,
                            listToRefresh,
                            listQueryParams,
                            listButtonClass,
                            listSortCol,
                            listSortOrder,
                            objectIDField
                        );
                    },
                    error: function (error) {
                        doCrashModal(error);
                    }
                });
            }
        }
        else {
            //If no unobtrusive validation errors
            if (form.valid()) {
                $.ajax({
                    type: "POST",
                    url: "/" + relativeURL + "/Create",
                    data: form.serialize(),
                    success: function (data) {
                        var hasClosedModal = false;
                        if (closeModalOnSuccess === true) {
                            if ($("#CloseFormOnSubmit").val() === "Y") {
                                hasClosedModal = true;
                                $("#" + modelToClose).modal("hide");
                            }
                            var audio = new Audio("/sounds/confirm.wav");
                            audio.play();
                        }

                        //Now object created must switch to edit mode
                        if (!hasClosedModal) {
                            $("#" + objectIDField).val(data.objectID);
                            $("#" + modelToClose).trigger("shown.bs.modal");
                        }

                        loadList(
                            loadListIntoDivID,
                            relativeURL,
                            parentObjectID,
                            listToRefresh,
                            listQueryParams,
                            listButtonClass,
                            listSortCol,
                            listSortOrder,
                            objectIDField
                        );
                    },
                    error: function (error) {
                        doCrashModal(error);
                    }
                });
            }
        }
    });

    return form.valid();
}

function attachSubmitDeleteForm(
    loadFormIntoDivID,
    loadListIntoDivID,
    relativeURL,
    objectID,
    parentObjectID,
    formToSubmit,
    listToRefresh,
    listQueryParams,
    listButtonClass,
    listSortCol,
    listSortOrder,
    objectIDField,
    closeModalOnSuccess,
    modelToClose
) {
    var form = $("#" + formToSubmit);
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);

    form.submit(function (event) {
        event.preventDefault();

        if (objectID.length > 0) {
            //If no unobtrusive validation errors
            if (form.valid()) {
                $.ajax({
                    type: "POST",
                    url: "/" + relativeURL + "/Delete/" + objectID,
                    data: form.serialize(),
                    success: function (data) {
                        if (closeModalOnSuccess === true) {
                            $("#" + modelToClose).modal("hide");
                        }
                        loadList(
                            loadListIntoDivID,
                            relativeURL,
                            parentObjectID,
                            listToRefresh,
                            listQueryParams,
                            listButtonClass,
                            listSortCol,
                            listSortOrder,
                            objectIDField
                        );
                    },
                    error: function (error) {
                        doCrashModal(error);
                    }
                });
            }
        }
        else {
            doErrorModal("Object ID not valid", "Object ID does not have a valid value");
        }
    });

    return form.valid();
}

function getSearchParams(searchString) {
    //Can pass in existing search parameters

    if (!searchString) {
        searchString = "";
    }

    let searchParams = new URLSearchParams(window.location.search);

    if (searchParams.has('sort')) {
        let sort = searchParams.get('sort');

        if (searchString === "") {
            searchString += "?sort=" + sort;
        }
        else {
            searchString += "&sort=" + sort;
        }
    }

    if (searchParams.has('search')) {
        let search = searchParams.get('search');

        if (searchString === "") {
            searchString += "?search=" + search;
        }
        else {
            searchString += "&search=" + search;
        }
    }

    return searchString;
}

function extraSubmitFormFunctions() {

    $('.ProfileIconAnimated').css({
        "margin-left": "-100px",
        "borderSpacing": "-90"
    });

    $('.ProfileIconAnimated').animate({
        borderSpacing: 90,
        marginLeft: "0px"
    },
    {
        step: function (now, fx) {
            $(this).css('transform', 'rotate(' + now + 'deg)');
        },
        duration: 500
    }, 'linear');
}