$(document).ready(function () {

    var otable = $('#initialBalance').DataTable({
        //"sort": [[2, "asc"]],
        "Dom": "t<'row'<'col-md-12'p >> ",
        "ajax": {
            "url": '/Accountant/SoRE',
            "type": "POST",
            "datatype": "json",
            "data": "data",        
        },

        "columns": [    
            {
                "data": null,
                render: function (d) {
                    return '';
                }

            },
            {
                "data": "initialBalace",
                "render": $.fn.dataTable.render.number(',', '.', 2, '$'),
                "name": "initialBalace"
            },

        ],
        rowCallback: function (row, data) {
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2, '$').display;
            $('td:nth-child(1)', row).html('Beginnning Retained Earnings');
            $('td:nth-child(2)', row).html(numFormat(data.iniBal));
            $('td:nth-child(2)', row).css('text-align', 'right');
            //$('td:nth-child(4)', row).html('$' + (data.totalCredits = data.totalDebits - data.totalDebits) + '.00');
              



        },
          "searching": false,
        "paging": false,
        "info": false,
        "sort": false,


    });
    var otable2 = $('#income').DataTable({

        "Dom": "t<'row'<'col-md-12'p >> ",
        "ajax": {
            "url": '/Accountant/IncomeStatementTotalT',
            "type": "POST",
            "datatype": "json",
            "data": "data"


        },

        "columns": [

            {
                "data": null,
                render: function (d) {
                    return '';
                }

            },
            {
                "data": "total",
                "name": "total", "autoWidth": true
            },

        ],
        rowCallback: function (row, data) {
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2).display;
            $('td:nth-child(1)', row).html('Add: Net Income');
            $('td:nth-child(2)', row).html(numFormat(data.total));
            $('td:nth-child(2)', row).css('text-align', 'right');
            //$('td:nth-child(4)', row).html('$' + (data.totalCredits = data.totalDebits - data.totalDebits) + '.00');
          



        },

        "searching": false,
        "paging": false,
        "info": false,
        "sort": false,

    });
    var otable3 = $('#debits').DataTable({

        "Dom": "t<'row'<'col-md-12'p >> ",
        "ajax": {
            "url": '/Accountant/SoREDeb',
            "type": "POST",
            "datatype": "json",
            "data": "data"


        },

        "columns": [

            {
                "data": null,
                render: function (d) {
                    return '';
                }

            },
            {
                "data": "totalDebits",
                "name": "totalDebits", "autoWidth": true
            },

        ],
        rowCallback: function (row, data) {
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2).display;
            $('td:nth-child(1)', row).html('Less: Dividends');
            $('td:nth-child(2)', row).html('('+numFormat(data.totalDebits)+ ')');
            $('td:nth-child(2)', row).css('text-align', 'right');
            //$('td:nth-child(4)', row).html('$' + (data.totalCredits = data.totalDebits - data.totalDebits) + '.00');
     


        },

        "searching": false,
        "paging": false,
        "info": false,
        "sort": false,

    });
    var otable4 = $('#balance').DataTable({
        "Dom": "t<'row'<'col-md-12'p >> ",
        "ajax": {
            "url": '/Accountant/SoREBal',
            "type": "POST",
            "datatype": "json",
            "data": "data"


        },

        "columns": [

            {
                "data": null,
                render: function (d) {
                    return '';
                }

            },
            {
                "data": "balance",
                "name": "balance", "autoWidth": false
            },

        ],
        rowCallback: function (row, data) {
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2, '$').display;
            $('td:nth-child(1)', row).html('Ending Retained Earnings');
            $('td:nth-child(2)', row).html( numFormat(data.balance));
            $('td:nth-child(2)', row).css('border-bottom', 'double');
            $('td:nth-child(2)', row).css('border-top', 'solid');
            $('td:nth-child(2)', row).css('border-bottom-width', '10px');
            $('td:nth-child(2)', row).css('width', '10%');
            $('td:nth-child(2)', row).css('font-weight', '700');
            $('td:nth-child(2)', row).css('text-align', 'right');
            $('td:nth-child(1)', row).css('font-weight', '700');



        },

        "searching": false,
        "paging": false,
        "info": false,
        "sort": false,

    });

  
});

