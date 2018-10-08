﻿//Enable popovers
$(function () {
    $("[data-toggle=popover]").popover();
});

//Load in functionality for customer list
loadList("CustomerListArea", "Customers", "", "CustomerList", getSearchParams(), "OpenCustomerButton", "CustomerID");

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
    //Save customer first to generate customer ID
    $("#CustomerFormFields").submit();
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
    //Save customer first to generate customer ID
    $("#CustomerFormFields").submit();
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

    loadInputForm("CustomerDetails", "CustomerListArea", "Customers", customerID, "", "CustomerForm", "CustomerFormFields", "CustomerList", getSearchParams(), "OpenCustomerButton", "CustomerID", true, "customerModal");
    loadList("AddressListArea", "Addresses", customerID, "AddressList", "", "OpenAddressButton", "AddressID");
    loadList("NoteListArea", "Notes", customerID, "NoteList", "", "OpenNoteButton", "NoteID");
});

$("#customerModal").on("hidden.bs.modal", function () {
    $("#CustomerDetails").html($("#LoadingHTML").html());
    $("#AddressListArea").html($("#LoadingHTML").html());
    $("#NoteListArea").html($("#LoadingHTML").html());

    $("#CustomerTabs a:first").tab("show");
});

$("#deleteCustomerModal").on("shown.bs.modal", function () {
    var customerID = $("#CustomerID").val();

    loadDeleteForm("DeleteCustomerDetails", "CustomerListArea", "Customers", customerID, "", "CustomerForm", "CustomerFormFields", "CustomerList", getSearchParams(), "OpenCustomerButton", "CustomerID", true, "customerModal");
});

$("#deleteCustomerModal").on("hidden.bs.modal", function () {
    $("#DeleteCustomerDetails").html($("#LoadingHTML").html());
});

$("#addressModal").on("shown.bs.modal", function () {
    var customerID = $("#CustomerID").val();
    var addressID = $("#AddressID").val();
    var formTitle = $("#FormTitleID").val();

    if (formTitle === "") {
        formTitle = "New Address";
    }

    $("#addressModalLabel").find(".title").html(formTitle);

    //Set form title back to blank to default to new recond functionality
    $("#FormTitleID").val("");

    loadInputForm("AddressDetails", "AddressListArea", "Addresses", addressID, customerID, "AddressForm", "AddressFormFields", "AddressList", "", "OpenAddressButton", "AddressID", true, "addressModal");
});

$("#addressModal").on("hidden.bs.modal", function () {
    $("#AddressDetails").html($("#LoadingHTML").html());
});

$("#deleteAddressModal").on("shown.bs.modal", function () {
    var customerID = $("#CustomerID").val();
    var addressID = $("#AddressID").val();

    loadDeleteForm("DeleteAddressDetails", "AddressListArea", "Addresses", addressID, customerID, "AddressForm", "AddressFormFields", "AddressList", "", "OpenAddressButton", "AddressID", true, "addressModal");
});

$("#deleteAddressModal").on("hidden.bs.modal", function () {
    $("#DeleteAddressDetails").html($("#LoadingHTML").html());
});

$("#noteModal").on("shown.bs.modal", function () {
    var customerID = $("#CustomerID").val();
    var noteID = $("#NoteID").val();
    var formTitle = $("#FormTitleID").val();

    if (formTitle === "") {
        formTitle = "New Note";
    }

    $("#noteModalLabel").find(".title").html(formTitle);

    //Set form title back to blank to default to new recond functionality
    $("#FormTitleID").val("");

    loadInputForm("NoteDetails", "NoteListArea", "Notes", noteID, customerID, "NoteForm", "NoteFormFields", "NoteList", "", "OpenNoteButton", "NoteID", true, "noteModal");
});

$("#noteModal").on("hidden.bs.modal", function () {
    $("#NoteDetails").html($("#LoadingHTML").html());
});

$("#deleteNoteModal").on("shown.bs.modal", function () {
    var customerID = $("#CustomerID").val();
    var noteID = $("#NoteID").val();

    loadDeleteForm("DeleteNoteDetails", "NoteListArea", "Notes", noteID, customerID, "NoteForm", "NoteFormFields", "NoteList", "", "OpenNoteButton", "NoteID", true, "noteModal");
});

$("#deleteNoteModal").on("hidden.bs.modal", function () {
    $("#DeleteNoteDetails").html($("#LoadingHTML").html());
});

$("#auditModal").on("shown.bs.modal", function () {
    var AuditRecordID = $("#AuditRecordID").val();
    var AuditTable = $("#AuditTable").val();

    loadList("AuditListArea", "AuditTrails", AuditTable + "/" + AuditRecordID, "AuditTrailsList", "", "OpenAuditButton", "");
});

$("#auditModal").on("hidden.bs.modal", function () {
    $("#AuditListArea").html($("#LoadingHTML").html());
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