
$(document).ready(function () {
    //manager table
    $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {
            var min = $('#min').datepicker("getDate");
            var max = $('#max').datepicker("getDate");
            var startDate = new Date(data[1]);
            if (min == null && max == null) { return true; }
            if (min == null && startDate <= max) { return true; }
            if (max == null && startDate >= min) { return true; }
            if (startDate <= max && startDate >= min) { return true; }
            return false;
        }
    );
    $("#min").datepicker({ onSelect: function () { otable.draw(); }, changeMonth: true, changeYear: true });
    $("#max").datepicker({ onSelect: function () { otable.draw(); }, changeMonth: true, changeYear: true });
    var otable = $('#journalTable').DataTable({
      
        "processing": false,

        "ajax": {
            "url": '/Journal/LoadData',
            "type": "POST",
            "datatype": "json",
            "data": "data"
        },

        "columns": [


            //index 0
            {
                "data": "journalID", "render": function (data) {

                    return "<a href='#' class='btn btn-sm btn-outline-primary' onclick='showDetails(" + data + ")'></i>View Details</a>";

                },
                "name": "journalID",
                retrieve: true,
                paging: false,
                sort: false,

            },
            {
                "data": "datePrepared",
                "render": function (d) {
                    return moment(d).format("MM/DD/YYYY");
                },
                "name": "datePrepared", "autoWidth": true
            },//index 1
            { "data": "journalType", "name": "journalType", "autoWidth": true },
            { "data": "comments", "name": "journalType", "autoWidth": true },//index 1
            { "data": "preparorID", "name": "preparorID", "autoWidth": true },
            {
                "data": "totalCredits",
                "render": $.fn.dataTable.render.number(',', '.', 2, '$'),
                "name": "totalCredits", "autoWidth": true
            },
            {
                "data": "totalDebits",
                "render": $.fn.dataTable.render.number(',', '.', 2, '$'),
                "name": "totalDebits", "autoWidth": true
            },
            { "data": "transStatus", "name": "transStatus", "autoWidth": true },
            {
                data: null,
                paging: false,
                sort: false
            }



        ],
        "columnDefs": [
            { className: "text-right", "targets": [5, 6] }

        ],
        "rowCallback": function (row, data) {
            
            if (data.transStatus == "Pending Approval") {
                if (data.journalType == "Closing") {
                    $('td:nth-child(9)', row).html("<a href='#' class='btn btn-block btn-success'  onclick='approveClosingJournal(" + data.journalID + ")'>Approve</a> <a href='#' class='btn btn-block  btn-danger'  onclick='rejectJournal(" + data.journalID + ")' >Reject</a>");

                }
                else {
                    $('td:nth-child(9)', row).html("<a href='#' class='btn btn-block btn-success'  onclick='approveJournal(" + data.journalID + ")'>Approve</a> <a href='#' class='btn btn-block  btn-danger'  onclick='rejectJournal(" + data.journalID + ")' >Reject</a>");

                }
                $('td:nth-child(8)', row).css('font-weight', 'bold');
                $('td:nth-child(8)', row).css('background-color', '#F19748');
            }
            else if (data.transStatus == "Approved") {
                $('td:nth-child(9)', row).html("");
                $('td:nth-child(9)', row).css('font-weight', 'bold');
                $('td:nth-child(9)', row).css('background-color', '#40EF1D');
            }
            else if (data.transStatus == "Suspended") {
                $('td:nth-child(9)', row).html("");
                $('td:nth-child(9)', row).css('font-weight', 'bold');
                $('td:nth-child(9)', row).css('background-color', '#F7EC56');
            }
            else if (data.transStatus == "Rejected") {
                $('td:nth-child(9)', row).html("");
                $('td:nth-child(9)', row).css('font-weight', 'bold');
                $('td:nth-child(9)', row).css('background-color', '##F44F4F');
            }

            

        },
        order: ([7,'dsc']),
        
        
    });

    //shared functions
    $('#table-filter').on('change', function () {
        otable.search(this.value).draw();
    });
    $('#table-filter1').on('change', function () {
        otable.search(this.value).draw();
    });

    $('#min, #max').change(function () {
        otable.draw();
    });

   

});



function approveJournal(journalID) {
    $("#ConfirmApprove").modal('show');
    $("#journalID").val(journalID);

}

function rejectJournal(journalID) {
    $("#ConfirmReject").modal('show');
    $("#journalID").val(journalID);

}
function suspendJournal(journalID) {
    $("#ConfirmSuspend").modal('show');
    $("#journalID").val(journalID);

}
function approveClosingJournal(journalID) {
    $("#ConfirmCloseApproval").modal('show');
    $("#journalID").val(journalID);

}

function closeAccounts() {
    //$("#closeModal").modal('show');
    $.ajax({
        type: "POST",
        url: "/Manager/LoadClosingData",
        success: function (result) {
            //$("#accountsTable").api().ajax.reload();
            alert("Closing Transactions Successfully Made Successfully");
            //$("#ConfirmApprove").modal('hide');
            window.location.href = "/Manager/Journal";

        }
    });
    
   
}

function approvalJournal() {

    var journalID = $("#journalID").val();

    $.ajax({
        type: "POST",
        url: "/Manager/ApproveJournal?journalID=" + journalID,
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //$("#accountsTable").api().ajax.reload();
            //alert("Journal #" + journalID + " Successfully Approved");
            $("#ConfirmApprove").modal('hide');
            window.location.href = "/Manager/Journal";

        }
    });

}

function closeJournalAccounts() {

    var journalID = $("#journalID").val();

    $.ajax({
        type: "POST",
        url: "/Manager/ApproveClosingJournal?journalID=" + journalID,
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //$("#accountsTable").api().ajax.reload();
           // alert("Journal #" + journalID + " Successfully Approved");
            $("#ConfirmApprove").modal('hide');
            window.location.href = "/Manager/Journal";

        }
    });

}
function rejectionJournal() {
    var journalID = parseInt($("#journalID").val());
    var reason = $("#reason").val();

    //alert(journalID + reason);

    $.ajax({
        type: "POST",
        url: "/Manager/RejectJournal",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ journalID: journalID, reason: reason }),
        //dataType: JSON,
        success: function (result) {
            //$("#accountsTable").api().ajax.reload();
           // alert("Journal #" + journalID + " Successfully Rejected");
            $("#ConfirmReject").modal('hide');
            window.location.href = "/Manager/Journal";

        }
    });

}

function suspensionJournal() {
    var journalID = parseInt($("#journalID").val());
    var reason = $("#reasonSus").val();

    alert(journalID + reason);

    $.ajax({
        type: "POST",
        url: "/Manager/SuspendJournal",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ journalID: journalID, reason: reason }),
        //dataType: JSON,
        success: function (result) {
            //$("#accountsTable").api().ajax.reload();
           // alert("Journal #" + journalID + " Successfully Suspended");
            $("#ConfirmSuspend").modal('hide');
            window.location.href = "/Manager/Journal";

        }
    });

}




//common functions to be pasted here

//shared common functions   
//Pop up of transaction details for a specific journal entry
function showDetails(journal) {

    $("#transModal").modal();
    // var url = "/Accountant/LoadTransaction?journalId=" + data;
    // otable.bDestroy();


    var transDebTable = $('#transDebTable').DataTable({
        processing: false,


        "ajax": {

            "url": "/Journal/LoadTransactionDeb?journalId=" + journal,
            "type": "GET",
            "datatype": "json",
            "data": "data"
        },
        "destroy": true,
        "searching": false,
        "paging": false,
        "info": false,
        "sort": false,


        "columns": [
            { "data": "acctName", "name": "acctName" },//index 1
            { "data": "transactionType", "name": "transactionType" },//index 2
            {
                "data": "transactionAmount",
                "render": $.fn.dataTable.render.number(',', '.', 2, '$ '),
                "name": "transactionAmount",

            },

        ],

        "columnDefs": [
            { className: "text-right", "targets": [2] },
            { className: "text-left", "targets": [0] },
            { className: "text-center", "targets": [1] },
        ]


    });
    var transCredTable = $('#transCredTable').DataTable({
        processing: false,

        "ajax": {
            "url": "/Journal/LoadTransactionCred?journalId=" + journal,
            "type": "GET",
            "datatype": "json",
            "data": "data"
        },

        "destroy": true,
        "searching": false,
        "paging": false,
        "info": false,
        "sort": false,


        "columns": [

            { "data": "acctName", "name": "acctName" },//index 1
            { "data": "transactionType", "name": "transactionType" },//index 2
            {
                "data": "transactionAmount",
                "render": $.fn.dataTable.render.number(',', '.', 2, '$ '),
                "name": "transactionAmount",

            },

        ],

        "columnDefs": [
            { className: "text-right", "targets": [2] },
            { className: "text-right", "targets": [0, 1] }
        ]


    });
    var filesTable = $('#filesTable').DataTable({
        "ajax": {

            "url": "/Blob/JournalFiles?journalId=" + journal,
            "type": "GET",
            "datatype": "json",
            "data": "data"
        },
        "language": { "emptyTable":"There are no files!"},
        "destroy": true,
        "searching": false,
        "paging": false,
        "info": false,
        "sort": false,

        "rowID": "ActualFileName",

        "columns": [
            {
                "data": "FileNameWithoutExt",
                "name": "FileNameWithoutExt",
                "autoWidth": true
            },
            {
                "data": "ActualFileName",
                "render": function (data, type, row, meta) {
                    if (type === 'display') {
                        data = '<a href="https://booktrackerfiles.blob.core.windows.net/journal/' + journal + '/' + data + '" download>Download &#xf019;</a>'
                    }
                    return data;
                }
            },
        ],


        "columnDefs": [
            { className: "text-right", "targets": 1 }
        ]
    })

    //transTable.destroy();

}

//init list for adding transactions 
var list = [];
//data.length() = 0;
var debSum = 0;
var credSum = 0;

function addNewOrder() {


    $('.datepicker').datepicker(); //Initialise any date pickers
    $("#newTransModal").modal();
    $("#date").datepicker('setDate', new Date());



    //$("#transJournDD").focus();

    var error_message = "Please Enter Journal Type";

    $("#accountDebDD").attr("disabled", "disabled");
    $("#debAmount").attr("disabled", "disabled");
    $("#addMoreDeb").attr("disabled", "disabled");
    $("#accountCredDD").attr("disabled", "disabled");
    $("#addMoreCred").attr("disabled", "disabled");
    $("#creditAmount").attr("disabled", "disabled");
    $("#attach").attr("disabled", "disabled");
    $("#saveOrder").attr("disabled", "disabled");
    $("#comments").attr("disabled", "disabled");
    $("#resetForm").attr("disabled", "disabled");

    $('#debAmount').change(function () {
        var amount = parseFloat($('#debAmount').val());
        if (amount == 0) {
            $("#addMoreDeb").attr("disabled", "disabled");
            alert("Can Not Enter Zero Amounts!");
        }
        else {
            $("#addMoreDeb").removeAttr("disabled");
        }
    });
    $('#creditAmount').change(function () {
        var amount = parseFloat($('#creditAmount').val());
        if (amount == 0) {
            $("#addMoreCred").attr("disabled", "disabled");
            alert("Can Not Enter Zero Amounts!");
        }
        else {
            $("#addMoreCred").removeAttr("disabled");
        }
    });


    $("#transJournDD").blur(function () {
        if ($(this).val() != '') {
            $("#accountDebDD").removeAttr("disabled");

            $("#debAmount").removeAttr("disabled");


            $("#Error1").html("");
        }
        else {
            $("#accountDebDD").attr("disabled", "disabled");
            $("#debAmount").attr("disabled", "disabled");
            $("#addMoreDeb").attr("disabled", "disabled");
            $("#accountCredDD").attr("disabled", "disabled");
            $("#addMoreCred").attr("disabled", "disabled");
            $("#creditAmount").attr("disabled", "disabled");
            $("#attach").attr("disabled", "disabled");
            $("#comments").attr("disabled", "disabled");
            $("#Error1").append(error_message);
        }
    });
    /*$("#debAmount").blur(function () {
        if ($(this).val() != '') {
            $("#accountCredDD").removeAttr("disabled");
            $("#addMoreCred").removeAttr("disabled");
            $("#creditAmount").removeAttr("disabled");
            $("#Error1").html("");
        }
        else {
 
            $("#accountCredDD").attr("disabled", "disabled");
            $("#addMoreCred").attr("disabled", "disabled");
            $("#creditAmount").attr("disabled", "disabled");
            $("#attach").attr("disabled", "disabled");
            $("#Error1").append(error_message);
        }
    });*/
}


$("#addMoreDeb").click(function (e) {
    //data.length('0');
    $("#accountCredDD").removeAttr("disabled");
    // $("#addMoreCred").removeAttr("disabled");
    $("#creditAmount").removeAttr("disabled");
    $("#resetForm").removeAttr("disabled");


    /*  fileDropZone.options.url = "/Attachments/UploadFiles";
      fileDropZone.options.paramName = "name";
      fileDropZone.processQueue();*/

    e.preventDefault();


    var
        acctName = $("#accountDebDD option:selected").text(),
        acctNo = $("#accountDebDD option:selected").val(),
        transactionAmount = $("#debAmount").val(),
        transactionType = '2',
        detailsTableBody = $("#detailsTable tbody");

    //hide selected from dropdowns
    $("#accountDebDD option[value=" + acctNo + "]").hide();
    $("#accountCredDD option[value=" + acctNo + "]").hide();

    var transaction = {

        transactionAmount,
        transactionType,
        acctNo
    }

    debSum += parseFloat(transactionAmount);
    $("#sumDebs").text('$ ' + debSum.toFixed(2));
    calcTotal();
    //alert(debSum.toFixed(2));
    list.push(transaction);
    // var debItem = '<tr><td>' + acctNo + '</td><td>' + '' + '</td><td>' + '$ ' + parseFloat(transactionAmount) + '.00' + '</td><td>' + '</td><td><a data-itemId="0" href="#" class="deleteItem">Remove</a></td></tr>';
    var debItem = '<tr><td>' + acctName + '</td><td>' + acctNo + '</td><td  style="text-align: right">' + '$ ' + parseFloat(transactionAmount) + '.00' + '</td><td>' + '' + '</td><td>' + '</td><td><a data-itemId="' + acctNo + '" transactionType="2" href="#" class="deleteItem btn btn-outline-danger">Remove</a></td></tr>';
    //$("#accountCredDD option[value='foo']").remove();
    detailsTableBody.append(debItem);
    clearItem();




});
//Function to check if credit account and deb account is not wokring

$("#addMoreCred").click(function (e) {
    $("#attach").removeAttr("disabled");
    $("#comments").removeAttr("disabled");

    e.preventDefault();



    var
        acctName = $("#accountCredDD option:selected").text(),
        acctNo = $("#accountCredDD option:selected").val(),
        transactionAmount = $("#creditAmount").val(),
        transactionType = '1',
        //transStatusID
        detailsTableBody = $("#detailsTable tbody");


    //hide selected from dropdowns
    $("#accountDebDD option[value=" + acctNo + "]").hide();
    $("#accountCredDD option[value=" + acctNo + "]").hide();

    var transaction = {

        transactionAmount,
        transactionType,
        acctNo
    };

    credSum += parseFloat(transactionAmount);
    $("#sumCred").text('$ ' + credSum.toFixed(2));
    list.push(transaction);
    calcTotal();

    var creditItem = '<tr><td style="text-align: right">' + acctName + '</td><td style="text-align: right">' + acctNo + '</td><td>' + '' + '</td><td  style="text-align: right">' + '$ ' + parseFloat(transactionAmount) + '.00' + '</td><td>' + '</td><td><a data-itemId="' + acctNo + '" transactionType="1" href="#" class="deleteItem btn btn-outline-danger">Remove</a></td></tr>';
    detailsTableBody.append(creditItem);
    clearItem();



});

function clearItem() {
    $("#accountCredDD").val('');
    $("#creditAmount").val('');
    $("#debAmount").val('');
    $("#accountDebDD").val('');

}


$(document).on('click', 'a.deleteItem', function (e) {

    e.preventDefault();

    var $self = $(this).attr('data-itemId');

    if ($(this).attr('transactionType') == 1) {
        var transactionAmount = $(this).attr('creditAmount');
        credSum = credSum - parseFloat(transactionAmount);

        if (credSum.toFixed(2) > 0) {
            $("#sumCred").text('$ ' + credSum.toFixed(2));
        }
        else {
            credSum = 0;
            $("#sumCred").text('$ ' + credSum.toFixed(2));
        }
    }

    if ($(this).attr('transactionType') == 2) {
        var transactionAmount = $(this).attr('debAmount');
        debSum = debSum - parseFloat(transactionAmount);

        if (debSum.toFixed(2) > 0) {
            $("#sumDebs").text('$ ' + debSum.toFixed(2));
        }
        else {
            debSum = 0;
            $("#sumDebs").text('$ ' + debSum.toFixed(2));
        }
    }




    calcTotal();

    list = $.grep(list, function (item) {
        return item.acctNo !== $self;
    });

    //show items in dropdowns
    $("#accountDebDD option[value=" + $self + "]").show();
    $("#accountCredDD option[value=" + $self + "]").show();

    if ($(this).attr('data-itemId') == $self) {
        $(this).parents('tr').css("background-color", "#ff6347").fadeOut(800, function () {
            $(this).remove();
        });
    }
});


$("#resetForm").click(function (e) {
    e.preventDefault();

    $("#accountDebDD").children('option').show();
    $("#accountCredDD").children('option').show();


    $("#detailsTable tr").remove();

    fileDropZone.removeAllFiles();

    list = [];
    debSum = 0;
    credSum = 0;
    $("#sumDebs").text('$ ' + debSum.toFixed(2));
    $("#sumCred").text('$ ' + credSum.toFixed(2));
    $("#accountDebDD").attr("disabled", "disabled");
    $("#debAmount").attr("disabled", "disabled");
    $("#addMoreDeb").attr("disabled", "disabled");
    $("#accountCredDD").attr("disabled", "disabled");
    $("#addMoreCred").attr("disabled", "disabled");
    $("#creditAmount").attr("disabled", "disabled");
    $("#attach").attr("disabled", "disabled");
    $("#saveOrder").attr("disabled", "disabled");
    $("#comments").attr("disabled", "disabled");

});

$("#saveOrder").click(function (e) {

    e.preventDefault();

    var orderArr = [];
    //orderArr = orderArr.push(list);
    orderArr.length = 0;



    var transactions = JSON.stringify({
        date: $("#date").val(),
        journalType: $("#transJournDD option:selected").val(),
        comments: $("#comments").val(),
        'transactions': list

    });

    //alert(transactions);

    /* $.when(saveOrder(data)).then(function (response) {
         console.log(response);
     }).fail(function (err) {
         console.log(err);
        });*/


    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: "/Journal/SaveTransactions",
        data: transactions,
        success: function (result) {//converted result to array to get journalID and message result.Data.nameofvariable
            //alert(result.Data.message); //message called from array result

            // calls uploadfiles function from attachments controllers.


            fileDropZone.options.url = "/Blob/UploadBlob?journalID=" + result.Data.journalID;
            fileDropZone.options.paramName = "name";
            fileDropZone.processQueue();
            setTimeout(function () {

                location.reload();//reload page
            }, 1000);
        },
        error: function () {
            alert("Error!")

        }
    });

});

$("#suspendTransactions").click(function (e) {

    e.preventDefault();

    var orderArr = [];
    //orderArr = orderArr.push(list);
    orderArr.length = 0;



    var transactions = JSON.stringify({
        date: $("#date").val(),
        journalType: $("#transJournDD option:selected").val(),
        comments: $("#comments").val(),
        'transactions': list

    });

    // alert(transactions);

    /* $.when(saveOrder(data)).then(function (response) {
         console.log(response);
     }).fail(function (err) {
         console.log(err);
        });*/


    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: "/Journal/SuspendTransactions",
        data: transactions,
        success: function (result) {//converted result to array to get journalID and message result.Data.nameofvariable
            //alert(result.Data.message); //message called from array result
            location.reload();

        },
        error: function () {
            alert("Error!")

        }
    });

});

function calcTotal() {

    if (credSum == debSum) {
        $("#saveOrder").removeAttr("disabled");
        $("#error").html("");
    }
    else {
        $("#saveOrder").attr("disabled", "disabled");
        $("#error").html("Debits Must Equal Credits!");

    }

}



$(document).on('click', 'a.deleteFile', function (e) {

    e.preventDefault();

    var fileName = $(this).attr("file-name");
    var journal = $(this).attr("file-journal");

    var ext = fileName.split('.').pop();
    var file = fileName.substr(0, fileName.lastIndexOf('.'))
    var tr = $(this).closest('tr');

    var msg = confirm("Are you sure you want to delete the file?");
    if (msg) {
        $.ajax({
            type: "post",
            //file, string extension, string journalID
            url: "/Blob/RemoveBlob?file=" + file + "&extension=" + ext + "&journalID=" + journal,
            success: function (response) {
                if (response == true) {
                    tr.remove();
                }
            }
        });
    }
});