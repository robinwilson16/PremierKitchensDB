$(".DatabaseSelector").change(function (event) {
    var systemDatabase = $(this).val();
    setCookie("SystemDatabase", systemDatabase, 14);
});

$("#AboutSystemLink").click(function (event) {
    let dataToLoad = `https://raw.githubusercontent.com/robinwilson16/PremierKitchensDB/master/README.md`;
    let title = `About Premier Kitchens DB System`;

    $.get(dataToLoad, function (data) {

    })
        .then(data => {
            var markdown = marked(data);
            let content = `
                <p>Premier Kitchens DB System &copy; Premier Kitchens</p>
                <div class="scrollable">${markdown}</div>`;

            doModal(title, content, "lg", "AboutInfo");
        })
        .fail(function () {
            let content = `Error loading content`;

            doErrorModal(title, content);
        });
    doModal(title, content);
});

$("#ChangelogLink").click(function (event) {
    let dataToLoad = `https://raw.githubusercontent.com/robinwilson16/PremierKitchensDB/master/CHANGELOG.md`;
    let title = `Changelog for Premier Kitchens DB System`;

    $.get(dataToLoad, function (data) {

    })
        .then(data => {
            var markdown = marked(data);

            doModal(title, markdown, "lg", "ChangelogInfo");
        })
        .fail(function () {
            let content = `Error loading content`;

            doErrorModal(title, content);
        });
});

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + exdays * 24 * 60 * 60 * 1000);
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