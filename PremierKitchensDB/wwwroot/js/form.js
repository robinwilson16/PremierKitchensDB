//Help with IE compatibility for .load and .get
$.ajaxSetup({ cache: false });

//Fix column header widths on jquery dataTables
$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    //$($.fn.dataTable.tables(true)).DataTable().columns.adjust();
    $($.fn.dataTable.tables(true)).DataTable().scroller.measure();
});

function noNulls(input) {
    if (input != null) {
        return input;
    }
    else {
        return "";
    }
}

function BRs(input) {
    if (input != null) {
        return input + "<br />";
    }
    else {
        return "";
    }
}

function getInitials(forename, surname) {
    let initials = "";

    if (forename.trim().length >= 1) {
        initials += forename.trim().substr(0, 1);
    }

    if (surname.trim().length >= 1) {
        initials += surname.trim().substr(0, 1);
    }

    return initials;
}

function hexColourFromName(forename, surname) {
    let hex = "";

    if (forename.trim().length >= 1) {
        hex += forename.trim().charCodeAt(0).toString(16);
    }
    else {
        hex += "00";
    }

    if (surname.trim().length >= 2) {
        hex += surname.trim().charCodeAt(0).toString(16);
        hex += surname.trim().charCodeAt(1).toString(16);
    }
    else if (surname.trim().length >= 1) {
        hex += surname.trim().charCodeAt(0).toString(16);
        hex += surname.trim().charCodeAt(0).toString(16);
    }
    else {
        hex += "0000";
    }

    return "#" + hex;
}

function hashCode(str) { // java String#hashCode
    var hash = 0;
    for (var i = 0; i < str.length; i++) {
        hash = str.charCodeAt(i) + ((hash << 5) - hash);
    }
    return hash;
}

function intToRGB(i) {
    var c = (i & 0x00FFFFFF)
        .toString(16)
        .toUpperCase();

    return "00000".substring(0, 6 - c.length) + c;
}

var stringToColour = function (str) {
    var hash = 0;
    for (var i = 0; i < str.length; i++) {
        hash = str.charCodeAt(i) + ((hash << 5) - hash);
    }
    var colour = '#';
    for (i = 0; i < 3; i++) {
        var value = (hash >> (i * 8)) & 0xFF;
        colour += ('00' + value.toString(16)).substr(-2);
    }
    return colour;
};

//Functions for loading data in
function checkApplyOrOkClicked(button) {
    if (button.includes("OK")) {
        $("#CloseFormOnSubmit").val("Y");
    }
    else {
        $("#CloseFormOnSubmit").val("N");
    }
}

function attachListFunctions (
    listButtonClass,
    objectIDField
) {
    //Attach after table has finished loading
    //$("#LoadingModal").modal("hide");

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

    $(".dataList td").click(function (event) {
        var rowIndex = $(this).parent().parent().children().index($(this).parent());
        var colIndex = $(this).index();

        showFilterAndSort(this, colIndex);
    });
    $(".dataList td").contextmenu(function (event) {
        var rowIndex = $(this).parent().parent().children().index($(this).parent());
        var colIndex = $(this).index();
        showFilterAndSort(this, colIndex);
    });
}

function showFilterAndSort(elem, colIndex) {
    event.preventDefault();

    //Hide any other tooltips
    $(".dataList").find("td").popover("hide");

    //Get field and value
    var fld = $(elem).find("a").attr("target");

    if (fld != null) {
        var val = $(elem).find("a").html().trim();
        var dataTable = $(elem).find("a").attr("aria-label");

        //Store values in popup
        $("#FilterCol").val(fld);
        $("#FilterColNum").val(colIndex);
        $("#FilterVal").val(val);
        $("#FilterTable").val(dataTable);

        //Get value of current filter
        var curFilter = $("#FilterQuery").val();

        //Hide clear filter button if no filter applied
        if (curFilter === '') {
            $(".ClearFilterControls").hide();
        }
        else {
            $(".ClearFilterControls").show();
        }

        //If not number then hide less then and more than controls
        var re = new RegExp("^(?!0)£?[0-9.,]{1,}$");

        if (re.test(val) || isDate(val)) {
            $(".NumberControls").show();
            $(".TextControls .InfoNumber").show();
            $(".TextControls .InfoText").hide();
        }
        else {
            $(".NumberControls").hide();
            $(".TextControls .InfoNumber").hide();
            $(".TextControls .InfoText").show();
        }

        //If checkboxes hide text input
        if (val.indexOf('check-box') > -1) {
            $(".TextControls").hide();
        }
        else {
            $(".TextControls").show();
        }

        $(elem).popover({
            trigger: 'manual',
            html: true,
            sanitize: false,
            title: function () {
                return "Filter and Sort <span class=\"close\">&times;</span>";
            },
            content: function () {
                return $("#FilterPanel").html();
            }
        }).popover('show');

        $(elem).on('shown.bs.popover', function (e) {
            //Functionality for close button
            var curPopover = $('#' + $(e.target).attr('aria-describedby'));

            curPopover.find('.close').click(function () {
                $(elem).popover("hide");
            });

            attachFilterFunctions(colIndex);
        });  
    }
}

//$.fn.dataTable.ext.search.push (
//    function (settings, data, dataIndex, row, counter) {
//        if (settings.sTableId === "CustomerList") {
//            return filterDataTable(data);
//        }
//        else {
//            return true;
//        }
//    }
//);

//function filterDataTable(data) {
//    //Filter the table using jQuery Datatables
//    let filterQuery = $("#FilterQuery").val();

//    //Remove leading ! from string
//    filterQuery = filterQuery.replace(/^\!+|\!+$/g, '');

//    let filters = filterQuery.split("!");
//    if (filterQuery !== "" && filters.length > 0) {
//        for (let filter of filters) {
//            let colID = filter.substring(0, filter.indexOf(","));
//            let comp = filter.substring(filter.indexOf(",") + 1, filter.lastIndexOf(","));
//            let searchVal = filter.substring(filter.lastIndexOf(",") + 1, filter.length);
//            let curVal = data[colID];

//            if (curVal === searchVal) {
//                return true;
//            }
//            else {
//                return false;
//            }
//        }

//        //Can also search this way - not used:
//        //listData
//        //    .column(3)
//        //    .search("Cu")
//        //    .draw();
//    }
//    else {
//        //No filter applied
//        return true;
//    }
//}

function attachFilterFunctions(colIndex) {
    $(".FilterButton").click(function (event) {
        event.preventDefault();

        var col = $("#FilterCol").val();
        var comp = $(this).attr("aria-label");
        var val = $("#FilterVal").val();
        var tbl = $("#FilterTable").val();
        var curFilter = $("#FilterQuery").val();

        performFilteredSearch(colIndex, col, comp, val, tbl, curFilter);
    });

    $(".DoFilterSearch").click(function (event) {
        event.preventDefault();

        var col = $("#FilterCol").val();
        var comp = $(this).attr("aria-label");
        var val = $(this).parent().parent().parent().find(".FilterSearch").val();
        var tbl = $("#FilterTable").val();
        var curFilter = $("#FilterQuery").val();

        performFilteredSearch(colIndex, col, comp, val, tbl, curFilter);
    });

    $(".FilterSearch").keypress(function (e) {
        if (e.which === 13) {
            event.preventDefault();

            var col = $("#FilterCol").val();
            var comp = "EQ";
            var val = $(this).val();
            var tbl = $("#FilterTable").val();
            var curFilter = $("#FilterQuery").val();

            performFilteredSearch(colIndex, col, comp, val, tbl, curFilter);
        }
    });

    $(".SortButton").click(function (event) {
        event.preventDefault();

        var col = $("#FilterCol").val();
        var tbl = $("#FilterTable").val();
        var sortOrder = $(this).attr("aria-label");
        var searchParams = $("#FilterQuery").val();
        var curSort = $("#SortQuery").val();

        $("#SortQuery").val(curSort + '!' + col + ',' + sortOrder);

        //Hide tooltip
        var table = $("#" + tbl);
        var tableSrc = table.attr("aria-label");

        table.find("td").popover("hide");

        sortOrder = sortOrder.toLowerCase();

        var listData = table.DataTable();

        //Use SQL backend to sort list
        listData.ajax.url("/" + tableSrc + "/?handler=Json&search=" + searchParams + "&sort=" + curSort).load(null, false);

        //Use Data Tables frontend instead
        //listData
        //    .order([colIndex, sortOrder])
        //    .draw();

        $(".SortApplied").show();
    });
}

function performFilteredSearch(colIndex, col, comp, val, tbl, curFilter) {
    $("#LoadingModal").modal("show").on('shown.bs.modal', function () {

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

        //If value contains % or * then switch to a like
        val = val.replace(/[*]/gm, "%");
        if (val.indexOf("%") > -1) {
            comp = "LK";
        }
        //If value contains < or > then switch to a greater than or less than
        if (val.indexOf("<=") > -1) {
            comp = "LTE";
        }
        else if (val.indexOf(">=") > -1) {
            comp = "GTE";
        }
        else if (val.indexOf("<") > -1) {
            comp = "LT";
        }
        else if (val.indexOf(">") > -1) {
            comp = "GT";
        }
        //Now remove <, >, = from the value
        val = val.replace("<=", "");
        val = val.replace(">=", "");
        val = val.replace("<", "");
        val = val.replace(">", "");

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
        var searchParams = $("#FilterQuery").val();
        var curSort = $("#SortQuery").val();

        //Hide tooltip
        var table = $("#" + tbl);
        var tableSrc = table.attr("aria-label");

        table.find("td").popover("hide");

        var listData = table.DataTable();
        //Use SQL backend to filter list
        listData.ajax.url("/" + tableSrc + "/?handler=Json&search=" + searchParams + "&sort=" + curSort).load(null, false);

        //Use DataTables frontend to filter list - use SQL instead due to performance issues over several executions of.load
        //listData.draw();

        $(".FilterApplied").show();

        $("#LoadingModal").modal("hide");
        $("#LoadingModal").unbind('shown.bs.modal');
    });
}

$(".ClearSortButton").click(function (event) {
    event.preventDefault();

    var tbl = $(this).attr("aria-label");
    var table = $("#" + tbl);
    var tableSrc = table.attr("aria-label");

    $("#SortQuery").val("");

    var searchParams = $("#FilterQuery").val();
    var curSort = "";

    var listData = table.DataTable();
    listData.ajax.url("/" + tableSrc + "/?handler=Json&search=" + searchParams + "&sort=" + curSort).load(null, false);

    //listData.order.neutral().draw();

    $(".SortApplied").hide();
});

$(".ClearCurrentFiltersButton").click(function (event) {
    event.preventDefault();

    var tbl = $(this).attr("aria-label");
    var table = $("#" + tbl);
    var tableSrc = table.attr("aria-label");

    $("#FilterQuery").val("");

    var searchParams = "";
    var curSort = $("#SortQuery").val();

    var listData = table.DataTable();
    listData.ajax.url("/" + tableSrc + "/?handler=Json&search=" + searchParams + "&sort=" + curSort).load(null, false);

    $(".FilterApplied").hide();
});

function loadList(
    loadIntoDivID,
    relativeURL,
    pageID,
    listObjectID,
    listToRefresh,
    listQueryParams,
    listButtonClass,
    listSortCol,
    listSortOrder,
    objectIDField
) {
    var dataToLoad;

    if (!pageID) {
        pageID = "Index";
    }

    if (!listSortCol) {
        listSortCol = "1";
    }
    if (!listSortOrder) {
        listSortOrder = "asc";
    }

    if (listObjectID.length > 0) {
        dataToLoad = "/" + relativeURL + "/" + pageID + "/" + listObjectID + listQueryParams;
    }
    else {
        dataToLoad = "/" + relativeURL + "/" + pageID + "/" + listQueryParams;
    }

    var listData;

    listData = $("#" + listToRefresh).DataTable();

    var listSrc;
    if (listObjectID.length > 0) {
        listSrc = "/" + relativeURL + "/" + listObjectID + "/?handler=Json&search=" + listQueryParams;
    }
    else {
        listSrc = "/" + relativeURL + "/?handler=Json&search=" + listQueryParams;
    }

    listData.ajax.url(listSrc).load(null, false);

    console.log(listSrc + " Loaded");
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
        dataToLoad = "/" + relativeURL + "/Create/";
    }

    $.get(dataToLoad, function (data) {

    })
        .then(data => {
            var formData = $(data).find("#" + formToLoad);
            $("#" + loadFormIntoDivID).html(formData);

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
        })
        .fail(function () {
            let title = `Error Loading Form ${formToLoad}`;
            let content = `The form at ${dataToLoad} returned a server error and could not be loaded`;

            doErrorModal(title, content);
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

        $.get(dataToLoad, function (data) {

        })
            .then(data => {
                var formData = $(data).find("#" + formToLoad);
                $("#" + loadFormIntoDivID).html(formData);

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
            })
            .fail(function () {
                let title = `Error Loading Form ${formToLoad}`;
                let content = `The form at ${dataToLoad} returned a server error and could not be loaded`;

                doErrorModal(title, content);
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
                            "Index",
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
                            "Index",
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

    //return form.valid();
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
                            "Index",
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

function isDate(dateVal) {
    var d = new Date(dateVal);
    return d.toString() === 'Invalid Date' ? false : true;
}