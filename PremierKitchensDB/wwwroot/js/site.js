$(".DatabaseSelector").change(function (event) {
    var systemDatabase = $(this).val();
    setCookie("SystemDatabase", systemDatabase, 14);
});

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; path=/; " + expires;
}

function formatMoney(num, rnd, symb, decimalSep, thousSep) {
    rnd = isNaN(rnd = Math.abs(rnd)) ? 2 : rnd;
    symb = symb === undefined ? "." : symb;
    decimalSep = decimalSep === undefined ? "." : decimalSep;
    thousSep = thousSep === undefined ? "," : thousSep;

    var s = num < 0 ? "-" : "";
    var i = String(parseInt(num = Math.abs(Number(num) || 0).toFixed(rnd)));
    var j = (j = i.length) > 3 ? j % 3 : 0;

    return s + symb + (j ? i.substr(0, j) + thousSep : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousSep) + (rnd ? decimalSep + Math.abs(num - i).toFixed(rnd).slice(2) : "");
}

function doModal(title, text) {
    console.log(text);

    html = '<div id="dynamicModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="confirm-modal" aria-hidden="true">';
    html += '<div class="modal-dialog">';
    html += '<div class="modal-content">';
    html += '<div class="modal-header">';
    html += '<h5 class="modal-title" id="customerModalLabel"><i class="fas fa-info-circle"></i> ' + title + '</h5>';
    html += '<button type="button" class="close" data-dismiss="modal" aria-label="Close">';
    html += '<span aria-hidden="true">&times;</span>';
    html += '</button>';
    html += '</div>';
    html += '<div class="modal-body">';
    html += '<p>' + text + '</p>';
    html += '</div>';
    html += '<div class="modal-footer">';
    html += '<span class="btn btn-primary" data-dismiss="modal">Close</span>';
    html += '</div>';  // content
    html += '</div>';  // dialog
    html += '</div>';  // footer
    html += '</div>';  // modalWindow
    $('body').append(html);
    $("#dynamicModal").modal();
    $("#dynamicModal").modal('show');

    $('#dynamicModal').on('hidden.bs.modal', function (e) {
        $(this).remove();
    });
}

function doErrorModal(title, text) {
    console.log(text);

    html = '<div id="dynamicModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="confirm-modal" aria-hidden="true">';
    html += '<div class="modal-dialog">';
    html += '<div class="modal-content">';
    html += '<div class="modal-header">';
    html += '<h5 class="modal-title" id="customerModalLabel"><i class="fas fa-exclamation-triangle"></i> ' + title + '</h5>';
    html += '<button type="button" class="close" data-dismiss="modal" aria-label="Close">';
    html += '<span aria-hidden="true">&times;</span>';
    html += '</button>';
    html += '</div>';
    html += '<div class="modal-body">';
    html += '<p>An unexpected error has occurred which could indicate a defect with the system</p>';
    html += '<div class="alert alert-danger" role="alert">';
    html += '<i class="fas fa-bug"></i> ' + text;
    html += '</div>';
    html += '</div>';
    html += '<div class="modal-footer">';
    html += '<span class="btn btn-primary" data-dismiss="modal">Close</span>';
    html += '</div>';  // content
    html += '</div>';  // dialog
    html += '</div>';  // footer
    html += '</div>';  // modalWindow
    $('body').append(html);
    $("#dynamicModal").modal();
    $("#dynamicModal").modal('show');

    $('#dynamicModal').on('hidden.bs.modal', function (e) {
        $(this).remove();
    });

    var audio = new Audio("/sounds/error.wav");
    audio.play();
}

function doCrashModal(error) {
    var stackError = $(error.responseText).find(".stackerror").html() || "Unknown error";
    var stackTrace = $(error.responseText).find(".rawExceptionStackTrace").html() || "";
    console.log(stackTrace);

    html = '<div id="dynamicModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="confirm-modal" aria-hidden="true">';
    html += '<div class="modal-dialog">';
    html += '<div class="modal-content">';
    html += '<div class="modal-header">';
    html += '<h5 class="modal-title" id="customerModalLabel"><i class="fas fa-exclamation-triangle"></i> An application error has occurred</h5>';
    html += '<button type="button" class="close" data-dismiss="modal" aria-label="Close">';
    html += '<span aria-hidden="true">&times;</span>';
    html += '</button>';
    html += '</div>';
    html += '<div class="modal-body">';
    html += '<p>An unexpected error has occurred which could indicate a defect with the system</p>';
    html += '<div class="alert alert-danger" role="alert">';
    html += '<i class="fas fa-bug"></i> ' + stackError;
    html += '</div>';
    html += '<div class="pre-scrollable small">';
    html += '<p><code>' + stackTrace + '</code></p>';
    html += '</div>';
    html += '</div>';
    html += '<div class="modal-footer">';
    html += '<span class="btn btn-primary" data-dismiss="modal">Close</span>';
    html += '</div>';  // content
    html += '</div>';  // dialog
    html += '</div>';  // footer
    html += '</div>';  // modalWindow
    $('body').append(html);
    $("#dynamicModal").modal();
    $("#dynamicModal").modal('show');

    $('#dynamicModal').on('hidden.bs.modal', function (e) {
        $(this).remove();
    });

    var audio = new Audio("/sounds/error.wav");
    audio.play();
}