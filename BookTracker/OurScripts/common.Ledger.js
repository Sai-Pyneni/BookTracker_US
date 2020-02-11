
function ledgerModal(data) {
    $("#ledger").modal();
    $("#acctTitle").html("Account: " + data);
    var ledgerTable = $('#ledgerTable').DataTable({
        //processing: false,
        "ajax": {

            "url": "/Journal/LoadLedger?acctNo=" + data,
            "type": "POST",
            "datatype": "json",
            "data": "data"
        },
        "destroy": true,
        "searching": false,
        "paging": false,
        "info": false,
        "sort": false,
      
        "columns": [

            {
                "data": "approvalDate",
                "render": function (d) {
                    return moment(d).format("MM/DD/YYYY");
                }, "name": "approvalDate", "autoWidth": true
            },
            {
                "data": "journalID",  // this is only difference, link here, accountant no link.
                "render": function (data, type) {
                    if (type == 'display') {
                        return "<a href='#' onclick='journalModal(" + data + ")'>" + data + " </a>";
                    }
                    return data;
                },

                "name": "acctName", "autoWidth": true
            },

            {
                "data": "debits",
                "render": $.fn.dataTable.render.number(',', '.', 2, '$'),
                "name": "debits",
                "autoWidth": true,
            },
            {
                "data": "credits",
                "render": $.fn.dataTable.render.number(',', '.', 2, '$'),
                "name": "credits",
                "autoWidth": true,
            },

        ],
        "columnDefs": [
            { className: "text-right", "targets": [2, 3] },
            { className: "text-center", "targets": [0, 1] },
            { searchable: false, targets: [0, 1, 2, 3] },
            { orderable: false, targets: [0, 1, 2, 3] },
            { orderFixed: [1, ''] }

        ],

        footerCallback: function (row, data) {
            var api = this.api(), data;
            var numFormat = $.fn.dataTable.render.number(',', '.', 2, '$').display;
            var intVal = function (i) {
                return i
            };

            var debtTotal = api
                .column(2)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            var credTotal = api
                .column(3)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            $(api.column(0).footer()).html('Balance');
            $(api.column(2).footer()).html('');
            $(api.column(2).footer()).css('text-align', 'right');
            $(api.column(3).footer()).html(numFormat((debtTotal - credTotal)));

            $(api.column(3).footer()).css('text-align', 'right');

        },

    });
}

function journalModal(journal) {
    $("#journalDetails").modal();
    $("#jouralTitle").html("Journal #: " + journal);
    var journalDetails = $('#journalDetailsTable').DataTable({
        processing: false,
        "ajax": {

            "url": "/Journal/LoadJournalDetails?journalId=" + journal,
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
            {
                "data": "datePrepared",
                "render": function (d) {
                    return moment(d).format("MM/DD/YYYY");
                },
                "name": "acctName", "autoWidth": true
            },//index 1
            { "data": "journalType", "name": "transactionType", "autoWidth": true },
            { "data": "preparorID", "name": "transactionType", "autoWidth": true },
            { "data": "approverID", "name": "transactionType", "autoWidth": true },//index 2
            

        ],

        "columnDefs": [
            { className: "text-right", "targets": [2,3] }
        ]


    });
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
            { "data": "acctName", "name": "acctName", "autoWidth": true },//index 1
            { "data": "transactionType", "name": "transactionType", "autoWidth": true },//index 2
            {
                "data": "transactionAmount",
                "render": $.fn.dataTable.render.number(',', '.', 2, '$ '),
                "name": "transactionAmount",
                "autoWidth": true,
            },

        ],

        "columnDefs": [
            { className: "text-right", "targets": [2] },
            { className: "text-left", "targets": [0] },
            { className: "text-center", "targets": [1] }
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

            { "data": "acctName", "name": "acctName", "autoWidth": true },//index 1
            { "data": "transactionType", "name": "transactionType", "autoWidth": true },//index 2
            {
                "data": "transactionAmount",
                "render": $.fn.dataTable.render.number(',', '.', 2, '$ '),
                "name": "transactionAmount",
                "autoWidth": true,
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
        "language": { "emptyTable": "There are no files!" },
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
    });
}
