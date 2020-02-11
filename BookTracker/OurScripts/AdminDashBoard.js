$(document).ready(function () {
    new total();
    new active();
    new inactive();
    new admin();
    new accountant();
    new manager();
    new currentRatio();
    new roa();
    new roe();
    new npm
    new tato();
    new fato();
    new quickRatio();

});

function total() {
    $.ajax({
        type: "GET",
        url: '/Admin/TotalUsers',
        datatype: "Json",
        success: function (data) {
            // alert(data);
            $("#tuText").val(data);
        }


    });
};
function active() {
    $.ajax({
        type: "GET",
        url: '/Admin/ActiveUsers',
        datatype: "Json",
        success: function (data) {
            // alert(data);
            $("#auText").val(data);
        }


    });
};
function inactive() {
    $.ajax({
        type: "GET",
        url: '/Admin/InActiveUsers',
        datatype: "Json",
        success: function (data) {
            // alert(data);
            $("#iuText").val(data);
        }


    });
};
function admin() {
    $.ajax({
        type: "GET",
        url: '/Admin/AdminUsers',
        datatype: "Json",
        success: function (data) {
            // alert(data);
            $("#adText").val(data);
        }


    });
};
function accountant() {
    $.ajax({
        type: "GET",
        url: '/Admin/AccountantUsers',
        datatype: "Json",
        success: function (data) {
            // alert(data);
            $("#acText").val(data);
        }


    });
};

function manager() {
    $.ajax({
        type: "GET",
        url: '/Admin/ManagerUsers',
        datatype: "Json",
        success: function (data) {
            // alert(data);
            $("#mnText").val(data);
        }


    });
};


function currentRatio() {
    $.ajax({
        type: "GET",
        url: '/FinancialStatements/CurrentRatio',
        datatype: "Json",
        success: function (data) {
            // alert(data);

            $("#currentRatioText").val(data + '%');
            if (data > 0 && data <= 50) {
                $('#crBox').css('background-color', 'red');
            }
            else if (data > 50 && data < 100) {
                $('#crBox').css('background-color', 'orange');
            }
            else
                $('#crBox').css('background-color', 'green');

        }


    });
};
function roa() {
    $.ajax({
        type: "GET",
        url: '/FinancialStatements/returnOnAssets',
        datatype: "Json",
        success: function (data) {
            // alert(data);

            $("#roaText").val(data + '%');
            if (data < 0) {
                $('#roaBox').css('background-color', 'red');
            }
            else {
                $('#roaBox').css('background-color', 'green');
            }


        }
    });
};
function roe() {
    $.ajax({
        type: "GET",
        url: '/FinancialStatements/returnOnEquity',
        datatype: "Json",
        success: function (data) {
            // alert(data);

            $("#roeText").val(data + '%');
            if (data < 0) {
                $('#roeBox').css('background-color', 'red');
            }
            else {
                $('#roeBox').css('background-color', 'green');
            }


        }
    });
};
function quickRatio() {
    $.ajax({
        type: "GET",
        url: '/FinancialStatements/QuickRatio',
        datatype: "Json",
        success: function (data) {
            // alert(data);

            $("#qcRatioText").val(data + '%');
            if (data < 0) {
                $('#quBox').css('background-color', 'red');
            }
            else
                $('#quBox').css('background-color', 'green');

        }


    });
};
function npm() {
    $.ajax({
        type: "GET",
        url: '/FinancialStatements/npmRatio',
        datatype: "Json",
        success: function (data) {
            // alert(data);

            $("#npmText").val(data + '%');
            if (data < 0) {
                $('#npmBox').css('background-color', 'red');
            }
            else if (data > 0)
                $('#npmBox').css('background-color', 'green');
            else
                $('#npmBox').css('background-color', 'Background');

        }


    });
};
function fato() {
    $.ajax({
        type: "GET",
        url: '/FinancialStatements/faTO',
        datatype: "Json",
        success: function (data) {
            // alert(data);

            $("#fatText").val(data + '%');
            if (data < 0) {
                $('#fatBox').css('background-color', 'red');
            }
            else if (data > 0)
                $('#fatBox').css('background-color', 'green');
            else
                $('#fatBox').css('background-color', 'Background');

        }


    });
};
function tato() {
    $.ajax({
        type: "GET",
        url: '/FinancialStatements/taTO',
        datatype: "Json",
        success: function (data) {
            // alert(data);

            $("#tatText").val(data + '%');
            if (data < 0) {
                $('#tatBox').css('background-color', 'red');
            }
            else if (data > 0)
                $('#tatBox').css('background-color', 'green');
            else
                $('#tatBox').css('background-color', 'Background');

        }


    });
};