$("#LoadingModal").modal("show");

//Enable popovers
$(function () {
    $("[data-toggle=popover]").popover();
});

//Attach events to buttons
$(".SaveCustomerButton").click(function (event) {
    $("#CustomerFormFields").submit();
    checkApplyOrOkClicked($(this).text());
});

//Submit form on button press
$("#DeleteCustomerButton").click(function (event) {
    $("#CustomerFormFields").submit();
});

//Clear current customer ID when new button pressed
$(".NewCustomerButton").click(function (event) {
    $("#CustomerID").val("");
});

$(".AuditCustomerButton").click(function (event) {
    $("#AuditRecordID").val($("#CustomerID").val());
    $("#AuditTable").val("Customer");
});

$(".SaveAddressButton").click(function (event) {
    $("#AddressFormFields").submit();
    checkApplyOrOkClicked($(this).text());
});

$(".DeleteAddressButton").click(function (event) {
    $("#AddressFormFields").submit();
});

$(".NewAddressButton").click(function (event) {
    $("#AddressID").val("");
});

$(".AuditAddressButton").click(function (event) {
    $("#AuditRecordID").val($("#AddressID").val());
    $("#AuditTable").val("Address");
});

$(".SaveNoteButton").click(function (event) {
    $("#NoteFormFields").submit();
    checkApplyOrOkClicked($(this).text());
});

$(".DeleteNoteButton").click(function (event) {
    $("#NoteFormFields").submit();
});

$(".NewNoteButton").click(function (event) {
    $("#NoteID").val("");
});

$(".AuditNoteButton").click(function (event) {
    $("#AuditRecordID").val($("#NoteID").val());
    $("#AuditTable").val("Note");
});

//Load data in when model is displayed
$("#customerModal").on("shown.bs.modal", function () {
    var customerID = $("#CustomerID").val();
    var formTitle = $("#FormTitleID").val();

    if (formTitle === "") {
        formTitle = "New Customer";
    }

    $("#customerModalLabel").find(".title").html(formTitle);

    //Set form title back to blank to default to new recond functionality
    $("#FormTitleID").val("");

    loadInputForm("CustomerDetails", "CustomerListArea", "Customers", customerID, customerID, "CustomerForm", "CustomerFormFields", "CustomerList", getSearchParams(), "OpenCustomerButton", "3", "asc", "CustomerID", true, "customerModal");
    loadList("AddressListArea", "Addresses", "Index", customerID, "AddressList", "", "OpenAddressButton", "4", "asc", "AddressID");
    loadList("NoteListArea", "Notes", "Index", customerID, "NoteList", "", "OpenNoteButton", "3", "desc", "NoteID");

    showCustomerAlerts(customerID);
});

$("#customerModal").on("hidden.bs.modal", function () {
    $("#CustomerTabs a:first").tab("show");
    let loadingAnim = $("#LoadingHTML").html();
    $("#CustomerDetails").html(loadingAnim);
});

$("#deleteCustomerModal").on("shown.bs.modal", function () {
    var customerID = $("#CustomerID").val();

    loadDeleteForm("DeleteCustomerDetails", "CustomerListArea", "Customers", customerID, customerID, "CustomerForm", "CustomerFormFields", "CustomerList", getSearchParams(), "OpenCustomerButton", "3", "asc", "CustomerID", true, "customerModal");
});

$("#deleteCustomerModal").on("hidden.bs.modal", function () {
    let loadingAnim = $("#LoadingHTML").html();
    $("#DeleteCustomerDetails").html(loadingAnim);
});

$("#addressModal").on("shown.bs.modal", function () {
    var customerID = $("#CustomerID").val();
    var addressID = $("#AddressID").val();
    var formTitle = $("#FormTitleID").val();

    //If customer is blank then perform a save first
    if (customerID === "") {
        checkApplyOrOkClicked("Apply");
        $("#CustomerFormFields").submit();
        showAddressDeferred();
    }
    else {
        showAddressModal(customerID, addressID, formTitle);
    }
});

function showAddressDeferred() {
    //Wait until we have customer ID
    var customerID = $("#CustomerID").val();
    var addressID = $("#AddressID").val();
    var formTitle = $("#FormTitleID").val();

    if (customerID === "") {
        window.setTimeout(showAddressDeferred, 100);
    }
    else {
        showAddressModal(customerID, addressID, formTitle);
    }
}

function showAddressModal(customerID, addressID, formTitle) {
    if (formTitle === "") {
        formTitle = "New Address";
    }

    $("#addressModalLabel").find(".title").html(formTitle);

    //Set form title back to blank to default to new recond functionality
    $("#FormTitleID").val("");

    loadInputForm("AddressDetails", "AddressListArea", "Addresses", addressID, customerID, "AddressForm", "AddressFormFields", "AddressList", "", "OpenAddressButton", "4", "asc", "AddressID", true, "addressModal");
}

$("#addressModal").on("hidden.bs.modal", function () {
    let loadingAnim = $("#LoadingHTML").html();
    $("#AddressDetails").html(loadingAnim);
});

$("#deleteAddressModal").on("shown.bs.modal", function () {
    var customerID = $("#CustomerID").val();
    var addressID = $("#AddressID").val();

    loadDeleteForm("DeleteAddressDetails", "AddressListArea", "Addresses", addressID, customerID, "AddressForm", "AddressFormFields", "AddressList", "", "OpenAddressButton", "4", "asc", "AddressID", true, "addressModal");
});

$("#deleteAddressModal").on("hidden.bs.modal", function () {
    let loadingAnim = $("#LoadingHTML").html();
    $("#DeleteAddressDetails").html(loadingAnim);
});

$("#noteModal").on("shown.bs.modal", function () {
    var customerID = $("#CustomerID").val();
    var noteID = $("#NoteID").val();
    var formTitle = $("#FormTitleID").val();

    //If customer is blank then perform a save first
    if (customerID === "") {
        checkApplyOrOkClicked("Apply");
        $("#CustomerFormFields").submit();
        showNoteDeferred();
    }
    else {
        showNoteModal(customerID, noteID, formTitle);
    }
});

function showNoteDeferred() {
    //Wait until we have customer ID
    var customerID = $("#CustomerID").val();
    var noteID = $("#NoteID").val();
    var formTitle = $("#FormTitleID").val();

    if (customerID === "") {
        window.setTimeout(showNoteDeferred, 100);
    }
    else {
        showNoteModal(customerID, noteID, formTitle);
    }
}

function showNoteModal(customerID, noteID, formTitle) {
    if (formTitle === "") {
        formTitle = "New Note";
    }

    $("#noteModalLabel").find(".title").html(formTitle);

    //Set form title back to blank to default to new recond functionality
    $("#FormTitleID").val("");

    loadInputForm("NoteDetails", "NoteListArea", "Notes", noteID, customerID, "NoteForm", "NoteFormFields", "NoteList", "", "OpenNoteButton", "3", "desc", "NoteID", true, "noteModal");
}

$("#noteModal").on("hidden.bs.modal", function () {
    let loadingAnim = $("#LoadingHTML").html();
    $("#NoteDetails").html(loadingAnim);
});

$("#deleteNoteModal").on("shown.bs.modal", function () {
    var customerID = $("#CustomerID").val();
    var noteID = $("#NoteID").val();

    loadDeleteForm("DeleteNoteDetails", "NoteListArea", "Notes", noteID, customerID, "NoteForm", "NoteFormFields", "NoteList", "", "OpenNoteButton", "3", "desc", "NoteID", true, "noteModal");
});

$("#deleteNoteModal").on("hidden.bs.modal", function () {
    let loadingAnim = $("#LoadingHTML").html();
    $("#DeleteNoteDetails").html(loadingAnim);
});

$("#auditModal").on("shown.bs.modal", function () {
    var AuditRecordID = $("#AuditRecordID").val();
    var AuditTable = $("#AuditTable").val();

    loadList("AuditListArea", "AuditTrails", "Index", AuditTable + "/" + AuditRecordID, "AuditList", "", "OpenAuditButton", "1", "desc", "");
});

$("#customerHistoryModal").on("shown.bs.modal", function () {
    var AuditTable = "Customer";
    var UserID = $("#UserID").val();

    loadList("CustomerHistoryListArea", "AuditTrails", "History", "History/" + AuditTable + "/" + UserID, "CustomerHistoryList", "", "OpenCustomerButton", "1", "desc", "CustomerID");
});

$(".CustomerQuickSearch").keypress(function (event) {
    if (event.which === 13) {
        var val = $(this).val();

        doQuickSearch(val);

        return false;
    }
});

$(".CustomerQuickSearchButton").click(function (event) {
    val = $(".CustomerQuickSearch").val();

    doQuickSearch(val);
});

function doQuickSearch(val) {
    $("#FilterQuery").val("C.Surname,LK," + val + "!~C.Forename,LK," + val + "!~C.CustomerID,LK," + val);
    $("#doSearch").submit();
}

function showCustomerAlerts(customerID) {
    var alertsToLoad;

    if (customerID.length > 0) {
        alertsToLoad = "/Notes/" + customerID + "/?handler=Json&alertOnly=true";
    }
    else {
        alertsToLoad = "/Notes/?handler=Json&alertOnly=true";
    }

    $.get(alertsToLoad, function (data) {

    })
        .then(data => {
            //Show any alerts from the remote page
            var alertHtml = "";
            for (var key in data) {
                if (data[key].noteText !== null) {
                    alertHtml += "<p>" + data[key].noteText + "</p>";
                }
            }

            if (alertHtml !== "") {
                doModal("Customer Alerts", alertHtml);
            }

            console.log(alertsToLoad + " Loaded");
        })
        .fail(function () {
            let title = `Error Loading ${alertsToLoad}`;
            let content = `The list at ${alertsToLoad} returned a server error and could not be loaded`;

            doErrorModal(title, content);
    });
}

$(function () {
    $.extend($.fn.dataTable.defaults, {
        language: {
            processing: '<div class="col text-center LoadingArea"><i class="fas fa-spinner fa-spin"></i></div>'
        }
    });

    var searchParams = $("#FilterQuery").val();

    CustomerListDT = $('#CustomerList').DataTable({
        dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rt<"row"<"col-md text-right"B>><"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
        buttons: [
            {
                extend: 'colvis',
                text: '<i class="fas fa-columns"></i> Columns'
            },
            {
                extend: 'copyHtml5',
                text: '<i class="far fa-copy"></i> Copy',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'excelHtml5',
                text: '<i class="far fa-file-excel"></i> Excel',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'csvHtml5',
                text: '<i class="fas fa-file-csv"></i> CSV',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'pdfHtml5',
                text: '<i class="far fa-file-pdf"></i> PDF',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i> Print',
                exportOptions: {
                    columns: ':visible'
                }
            }
        ],
        processing: true,
        serverSide: false,
        colReorder: true,
        deferRender: true,
        scroller: true,
        scrollY: 420,
        ajax: { url: "/Customers/?handler=Json&search=" + searchParams, dataSrc: "" },
        columns: [
            {
                data: {
                    _: "customerID",
                    sort: "customerID",
                    filter: "customerID",
                    display: cusOpenCustomer
                }
            },
            {
                data: {
                    _: "customerID",
                    sort: "customerID",
                    filter: "customerID",
                    display: cusCustomerPhoto
                }
            },
            {
                data: {
                    _: "customerID",
                    sort: "customerID",
                    filter: "customerID",
                    display: cusCustomerID
                }
            },
            {
                data: {
                    _: "surname",
                    sort: "surname",
                    filter: "surname",
                    display: cusSurname
                }
            },
            {
                data: {
                    _: "forename",
                    sort: "forename",
                    filter: "forename",
                    display: cusForename
                }
            },
            {
                data: {
                    _: "title",
                    sort: "title",
                    filter: "title",
                    display: cusTitle
                }
            },
            {
                data: {
                    _: "address",
                    sort: "address",
                    filter: "address",
                    display: cusAddress
                }
            },
            {
                data: {
                    _: "postCode",
                    sort: "postCode",
                    filter: "postCode",
                    display: cusPostCode
                }
            },
            {
                data: {
                    _: "email",
                    sort: "email",
                    filter: "email",
                    display: cusEmail
                },
                visible: false
            },
            {
                data: {
                    _: "mobilePhone",
                    sort: "mobilePhone",
                    filter: "mobilePhone",
                    display: cusMobilePhone
                },
                visible: false
            },
            {
                data: {
                    _: "workPhone",
                    sort: "workPhone",
                    filter: "workPhone",
                    display: cusWorkPhone
                },
                visible: false
            },
            {
                data: {
                    _: "canBeContacted",
                    sort: "canBeContacted",
                    filter: "canBeContacted",
                    display: cusCanBeContacted
                }
            },
            {
                data: {
                    _: "areas",
                    sort: "areas",
                    filter: "areas",
                    display: cusAreas
                }
            },
            {
                data: {
                    _: "orderValue",
                    sort: "orderValue",
                    filter: "orderValue",
                    display: cusOrderValue
                }
            },
            {
                data: {
                    _: "hasOutstandingRemedialWork",
                    sort: "hasOutstandingRemedialWork",
                    filter: "hasOutstandingRemedialWork",
                    display: cusHasOutstandingRemedialWork
                }
            },
            {
                data: {
                    _: "showroomName",
                    sort: "showroomName",
                    filter: "showroomName",
                    display: cusShowroomName
                }
            },
            {
                data: {
                    _: "dateOfEnquiry",
                    sort: "dateOfEnquiry",
                    filter: "dateOfEnquiry",
                    display: cusDateOfEnquiry
                }
            }
        ],
        //order: [[3, "asc"], [4, "asc"], [2, "asc"]],
        order: [],
        drawCallback: function (settings, json) {
            attachListFunctions(
                "OpenCustomerButton",
                "CustomerID"
            );
        }
    });

    AddressListDT = $('#AddressList').DataTable({
        dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rt<"row"<"col-md text-right"B>><"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
        buttons: [
            {
                extend: 'colvis',
                text: '<i class="fas fa-columns"></i> Columns'
            },
            {
                extend: 'copyHtml5',
                text: '<i class="far fa-copy"></i> Copy',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'excelHtml5',
                text: '<i class="far fa-file-excel"></i> Excel',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'csvHtml5',
                text: '<i class="fas fa-file-csv"></i> CSV',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'pdfHtml5',
                text: '<i class="far fa-file-pdf"></i> PDF',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i> Print',
                exportOptions: {
                    columns: ':visible'
                }
            }
        ],
        processing: true,
        serverSide: false,
        colReorder: true,
        deferRender: true,
        scroller: true,
        scrollY: 300,
        ajax: { url: "/Addresses/0/?handler=Json", dataSrc: "" },
        columns: [
            {
                data: {
                    _: "addressID",
                    sort: "addressID",
                    filter: "addressID",
                    display: adrOpenAddress
                }
            },
            {
                data: {
                    _: "address1",
                    sort: "address1",
                    filter: "address1",
                    display: adrAddress
                }
            },
            {
                data: {
                    _: "postcodeOut",
                    sort: "postcodeOut",
                    filter: "postcodeOut",
                    display: adrPostCode
                }
            },
            {
                data: "homePhone"
            },
            {
                data: {
                    _: "dateFrom",
                    sort: "dateFrom",
                    filter: "dateFrom",
                    display: adrDateFrom
                }
            },
            {
                data: {
                    _: "dateTo",
                    sort: "dateTo",
                    filter: "dateTo",
                    display: adrDateTo
                }
            },
            {
                data: {
                    _: "isPrimary",
                    sort: "isPrimary",
                    filter: "isPrimary",
                    display: adrIsPrimary
                }
            },
            {
                data: "addressType.addressTypeName"
            }
        ],
        //order: [[3, "asc"], [4, "asc"], [2, "asc"]],
        order: [],
        drawCallback: function (settings, json) {
            attachListFunctions(
                "OpenAddressButton",
                "AddressID"
            );
        }
    });

    NoteListDT = $('#NoteList').DataTable({
        dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rt<"row"<"col-md text-right"B>><"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
        buttons: [
            {
                extend: 'colvis',
                text: '<i class="fas fa-columns"></i> Columns'
            },
            {
                extend: 'copyHtml5',
                text: '<i class="far fa-copy"></i> Copy',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'excelHtml5',
                text: '<i class="far fa-file-excel"></i> Excel',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'csvHtml5',
                text: '<i class="fas fa-file-csv"></i> CSV',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'pdfHtml5',
                text: '<i class="far fa-file-pdf"></i> PDF',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i> Print',
                exportOptions: {
                    columns: ':visible'
                }
            }
        ],
        processing: true,
        serverSide: false,
        colReorder: true,
        deferRender: true,
        scroller: true,
        scrollY: 300,
        ajax: { url: "/Notes/0/?handler=Json", dataSrc: "" },
        columns: [
            {
                data: {
                    _: "noteID",
                    sort: "noteID",
                    filter: "noteID",
                    display: notOpenNote
                }
            },
            {
                data: "noteText"
            },
            {
                data: {
                    _: "isAlert",
                    sort: "isAlert",
                    filter: "isAlert",
                    display: notIsAlert
                }
            },
            {
                data: {
                    _: "createdDate",
                    sort: "createdDate",
                    filter: "createdDate",
                    display: notCreatedDate
                }
            }
        ],
        //order: [[1, "asc"], [2, "asc"]],
        order: [],
        drawCallback: function (settings, json) {
            attachListFunctions(
                "OpenNoteButton",
                "NoteID"
            );
        }
    });

    AuditListDT = $('#AuditList').DataTable({
        dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rt<"row"<"col-md text-right"B>><"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
        buttons: [
            {
                extend: 'colvis',
                text: '<i class="fas fa-columns"></i> Columns'
            },
            {
                extend: 'copyHtml5',
                text: '<i class="far fa-copy"></i> Copy',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'excelHtml5',
                text: '<i class="far fa-file-excel"></i> Excel',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'csvHtml5',
                text: '<i class="fas fa-file-csv"></i> CSV',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'pdfHtml5',
                text: '<i class="far fa-file-pdf"></i> PDF',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i> Print',
                exportOptions: {
                    columns: ':visible'
                }
            }
        ],
        processing: true,
        serverSide: false,
        colReorder: true,
        deferRender: true,
        scroller: true,
        scrollY: 300,
        ajax: { url: "/AuditTrails/none/?handler=Json", dataSrc: "" },
        columns: [
            {
                data: "changeInfo"
            },
            {
                data: {
                    _: "updatedDate",
                    sort: "updatedDate",
                    filter: "updatedDate",
                    display: audUpdatedDate
                }
            },
            {
                data: {
                    _: "applicationUserUpdatedBy.forename",
                    sort: "applicationUserUpdatedBy.forename",
                    filter: "applicationUserUpdatedBy.forename",
                    display: audUpdatedBy
                }
            }
        ],
        //order: [[1, "desc"]]
        order: []
    });

    CustomerHistoryListDT = $('#CustomerHistoryList').DataTable({
        dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rt<"row"<"col-md text-right"B>><"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
        buttons: [
            {
                extend: 'colvis',
                text: '<i class="fas fa-columns"></i> Columns'
            },
            {
                extend: 'copyHtml5',
                text: '<i class="far fa-copy"></i> Copy',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'excelHtml5',
                text: '<i class="far fa-file-excel"></i> Excel',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'csvHtml5',
                text: '<i class="fas fa-file-csv"></i> CSV',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'pdfHtml5',
                text: '<i class="far fa-file-pdf"></i> PDF',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i> Print',
                exportOptions: {
                    columns: ':visible'
                }
            }
        ],
        processing: true,
        serverSide: false,
        colReorder: true,
        deferRender: true,
        scroller: true,
        scrollY: 300,
        ajax: { url: "/AuditTrails/History/Customer/none/?handler=Json", dataSrc: "" },
        columns: [
            {
                data: {
                    _: "auditTrailID",
                    sort: "auditTrailID",
                    filter: "auditTrailID",
                    display: hisOpenCustomer
                }
            },
            {
                data: "changeInfo"
            },
            {
                data: {
                    _: "updatedDate",
                    sort: "updatedDate",
                    filter: "updatedDate",
                    display: hisUpdatedDate
                }
            }
        ],
        //order: [[1, "desc"]],
        order: [],
        drawCallback: function (settings, json) {
            attachListFunctions(
                "OpenCustomerButton",
                "CustomerID"
            );
        }
    });
});

function cusOpenCustomer(data, type, dataToSet) {
    return `<button type="button" class="btn btn-secondary OpenCustomerButton" data-toggle="modal" data-id="${data.customerID}" data-target="#customerModal" data-loading-text="Record for ${data.forename} ${data.surname}">
                        <i class="fas fa-external-link-alt"></i>
                    </button>`;
}

function cusCustomerPhoto(data, type, dataToSet) {
    return `<div class="ProfileIconSmall" style="background-color: ${stringToColour(data.forename + data.surname)};">
                        ${getInitials(data.forename, data.surname)}
                    </div>`;
}

function cusCustomerID(data, type, dataToSet) {
    return `<a tabindex="0" role="button" href="#" target="C.CustomerID" aria-label="CustomerList">
                        ${data.customerID}
                    </a>`;
}

function cusSurname(data, type, dataToSet) {
    return `<a tabindex="0" role="button" href="#" target="C.Surname" aria-label="CustomerList">
                        ${noNulls(data.surname)}
                    </a>`;
}

function cusForename(data, type, dataToSet) {
    return `<a tabindex="0" role="button" href="#" target="C.Forename" aria-label="CustomerList">
                        ${noNulls(data.forename)}
                    </a>`;
}

function cusTitle(data, type, dataToSet) {
    return `<a tabindex="0" role="button" href="#" target="C.Title" aria-label="CustomerList">
                        ${noNulls(data.title)}
                    </a>`;
}

function cusAddress(data, type, dataToSet) {
    return `<a tabindex="0" role="button" href="#" target="AD.Address" aria-label="CustomerList">
                        ${noNulls(data.address)}
                    </a>`;
}

function cusPostCode(data, type, dataToSet) {
    return `<a tabindex="0" role="button" href="#" target="AD.PostCode" aria-label="CustomerList">
                        ${noNulls(data.postCode)}
                    </a>`;
}

function cusEmail(data, type, dataToSet) {
    return `<a tabindex="0" role="button" href="#" target="C.Email" aria-label="CustomerList">
                        ${noNulls(data.email)}
                    </a>`;
}

function cusMobilePhone(data, type, dataToSet) {
    return `<a tabindex="0" role="button" href="#" target="C.MobilePhone" aria-label="CustomerList">
                        ${noNulls(data.mobilePhone)}
                    </a>`;
}

function cusWorkPhone(data, type, dataToSet) {
    return `<a tabindex="0" role="button" href="#" target="C.WorkPhone" aria-label="CustomerList">
                        ${noNulls(data.workPhone)}
                    </a>`;
}

function cusCanBeContacted(data, type, dataToSet) {
    var isChecked = '';
    if (data.canBeContacted === true) {
        isChecked = 'checked ';
    }
    return `<a tabindex="0" role="button" href="#" target="C.CanBeContacted" aria-label="CustomerList">
                        <input class="check-box" disabled="disabled" type="checkbox" disabled ${isChecked} />
                    </a>`;
}

function cusAreas(data, type, dataToSet) {
    return `<a tabindex="0" role="button" href="#" target="CA.Areas" aria-label="CustomerList">
                        ${noNulls(data.areas)}
                    </a>`;
}

function cusOrderValue(data, type, dataToSet) {
    return `<a tabindex="0" role="button" href="#" target="C.OrderValue" aria-label="CustomerList">
                        ${formatMoney(data.orderValue, 0, "£")}
                    </a>`;
}

function cusHasOutstandingRemedialWork(data, type, dataToSet) {
    var isChecked = '';
    if (data.hasOutstandingRemedialWork === true) {
        isChecked = 'checked ';
    }
    return `<a tabindex="0" role="button" href="#" target="C.HasOutstandingRemedialWork" aria-label="CustomerList">
                        <input class="check-box" disabled="disabled" type="checkbox" disabled ${isChecked} />
                    </a>`;
}

function cusShowroomName(data, type, dataToSet) {
    return `<a tabindex="0" role="button" href="#" target="S.ShowroomName" aria-label="CustomerList">
                        ${noNulls(data.showroomName)}
                    </a>`;
}

function cusDateOfEnquiry(data, type, dataToSet) {
    return `<a tabindex="0" role="button" href="#" target="C.DateOfEnquiry" aria-label="CustomerList">
                        ${moment(data.dateOfEnquiry).format('DD MMM YY')}
                    </a>`;
}

function adrOpenAddress(data, type, dataToSet) {
    return `<button type="button" class="btn btn-secondary OpenAddressButton" data-toggle="modal" data-id="${data.addressID}" data-target="#addressModal" data-loading-text="Address at ${data.address1}">
                        <i class="fas fa-external-link-alt"></i>
                    </button>`;
}

function adrAddress(data, type, dataToSet) {
    return `${BRs(data.address1)}
                    ${BRs(data.address2)}
                    ${BRs(data.address3)}
                    ${BRs(data.address4)}`;
}

function adrPostCode(data, type, dataToSet) {
    if (data.postcodeOut !== null) {
        return `${BRs(data.postcodeOut)} ${BRs(data.postcodeIn)}`;
    }
    else {
        return ``;
    }
}

function adrDateFrom(data, type, dataToSet) {
    return `${moment(data.dateFrom).format('DD MMM YY')}`;
}

function adrDateTo(data, type, dataToSet) {
    if (data.dateTo !== null) {
        return `${moment(data.dateTo).format('DD MMM YY')}`;
    }
    else {
        return ``;
    }
}

function adrIsPrimary(data, type, dataToSet) {
    var isChecked = '';
    if (data.isPrimary === true) {
        isChecked = 'checked ';
    }
    return `<input class="check-box" disabled="disabled" type="checkbox" disabled ${isChecked} />`;
}

function notOpenNote(data, type, dataToSet) {
    return `<button type="button" class="btn btn-secondary OpenNoteButton" data-toggle="modal" data-id="${data.noteID}" data-target="#noteModal" data-loading-text="Note created on ${data.createdDate}">
                        <i class="fas fa-external-link-alt"></i>
                    </button>`;
}

function notIsAlert(data, type, dataToSet) {
    var isChecked = '';
    if (data.isAlert === true) {
        isChecked = 'checked ';
    }
    return `<input class="check-box" disabled="disabled" type="checkbox" disabled ${isChecked} />`;
}

function notCreatedDate(data, type, dataToSet) {
    return `${moment(data.createdDate).format('DD MMM YY HH:MM')}`;
}

function audUpdatedDate(data, type, dataToSet) {
    return `${moment(data.updatedDate).format('DD MMM YY HH:MM')}`;
}

function audUpdatedBy(data, type, dataToSet) {
    return `${data.applicationUserUpdatedBy.forename} ${data.applicationUserUpdatedBy.surname}`;
}

function hisOpenCustomer(data, type, dataToSet) {
    return `<button type="button" class="btn btn-secondary OpenCustomerButton" data-dismiss="modal" data-toggle="modal" data-id="${data.objectID}" data-target="#customerModal" data-loading-text="Record for ${data.changeInfo.replace(" Viewed", "")}">
                <i class="fas fa-external-link-alt"></i>
            </button>`;
}

function hisUpdatedDate(data, type, dataToSet) {
    return `${moment(data.updatedDate).format('DD MMM YY HH:MM')}`;
}