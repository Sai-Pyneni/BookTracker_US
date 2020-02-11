$(document).ready(function () {

    var otable = $('#incomeTable').DataTable({

        "Dom": "t<'row'<'col-md-12'p >> ",
        "ajax": {
            "url": '/Accountant/IncomeStatementRevenues',
            "type": "POST",
            "datatype": "json",
            "data": "data"
        },

        "columns": [

            { "data": "acctName", "name": "acctName", "autoWidth": true },
            {
                "data": "totalCredits",
                "render": $.fn.dataTable.render.number(',', '.', 2, '$'),
                "name": "totalCredits", "autoWidth": false
            },

        ],

        rowCallback: function (row, data) {
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2).display;

           
            $('td:nth-child(2)', row).html(numFormat(data.totalDebits));
            //$('td:nth-child(4)', row).html('$' + (data.totalCredits = data.totalDebits - data.totalDebits) + '.00');
            data.totalCredits == data.totalDebits;
            $('td:nth-child(2)', row).css('text-align', 'right');
            $('td:nth-child(1)', row).css('text-align', 'left');
            $('td:nth-child(1)', row).css('padding-left', '25px');  
            $('td:nth-child(2)', row).css('width', '25px');
        },
        footerCallback: function (row, data) {
            var api = this.api(), data;
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2, '$').display;
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            var debtTotal = api
                .column(1)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);


            $(api.column(0).footer()).html('Total Revenues');
            $(api.column(1).footer()).html(numFormat(debtTotal));
            $(api.column(0).footer()).css('text-align', 'left');
       
            $(api.column(1).footer()).css('border-bottom', 'solid');
            $(api.column(1).footer()).css('width', '10%');
            $(api.column(1).footer()).css('text-align', 'right');
            /* $(api.column(3).footer()).css('text-decoration', 'underline');
             $(api.column(3).footer()).css('text-decoration-style', 'double');
             $(api.column(2).footer()).css('text-decoration', 'underline');
             $(api.column(2).footer()).css('text-decoration-style', 'double'); */

        },
        "searching": false,
        "paging": false,
        "info": false,
        "sort": false,


    });
    var otable2 = $('#incomeTableExpenses').DataTable({

        "Dom": "t<'row'<'col-md-12'p >> ",
        "ajax": {
            "url": '/Accountant/IncomeStatementExpense',
            "type": "POST",
            "datatype": "json",
            "data": "data"
        },

        "columns": [

            { "data": "acctName", "name": "acctName", "autoWidth": true },
            {
                "data": "totalDebits",
                "render": $.fn.dataTable.render.number(',', '.', 2, '$'),
                "name": "totalDebits", "autoWidth": false
            },

        ],

        rowCallback: function (row, data, DisplayIndex) {
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2).display;

            $('td:nth-child(2)', row).html(numFormat(data.totalDebits));
            //$('td:nth-child(4)', row).html('$' + (data.totalCredits = data.totalDebits - data.totalDebits) + '.00');
            data.totalCredits == data.totalDebits;
            $('td:nth-child(2)', row).css('text-align', 'right');
            $('td:nth-child(1)', row).css('text-align', 'left');
            $('td:nth-child(2)', row).css('width', '25px');
            $('td:nth-child(1)', row).css('padding-left', '25px');  
            if (DisplayIndex == 0) {
                $('td:nth-child(2)', row).html('$' + numFormat(data.totalDebits));
            }


        },
        footerCallback: function (row, data) {
            var api = this.api(), data;
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2, '$').display;
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            var debtTotal = api
                .column(1)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);


            $(api.column(0).footer()).html('Total Expenses');
            $(api.column(1).footer()).html(numFormat(debtTotal));
            $(api.column(0).footer()).css('text-align', 'left');
          
            $(api.column(1).footer()).css('border-bottom', 'solid');
            $(api.column(1).footer()).css('text-align', 'right');
            $(api.column(1).footer()).css('width', '10%');
            /* $(api.column(3).footer()).css('text-decoration', 'underline');
             $(api.column(3).footer()).css('text-decoration-style', 'double');
             $(api.column(2).footer()).css('text-decoration', 'underline');
             $(api.column(2).footer()).css('text-decoration-style', 'double'); */

        },
        "searching": false,
        "paging": false,
        "info": false,
        "sort": false,

    });  
    var otable3 = $('#income').DataTable({

        "Dom": "t<'row'<'col-md-12'p >> ",
        "ajax": {
            "url": '/Accountant/IncomeStatementTotals',
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
                "name": "total", "autoWidth": false
            },

        ],
        rowCallback: function (row, data) {
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2, '$').display;
            if (data.total > 0) {
                $('td:nth-child(1)', row).html('Net Income');
                $('td:nth-child(2)', row).html(numFormat(data.total));
                //$('td:nth-child(4)', row).html('$' + (data.totalCredits = data.totalDebits - data.totalDebits) + '.00');
                $('td:nth-child(1)', row).css('text-align', 'left');
                $('td:nth-child(2)', row).css('text-align', 'right');
                $('td:nth-child(2)', row).css('border-top', 'solid');
                $('td:nth-child(2)', row).css('border-bottom', 'double');
                $('td:nth-child(2)', row).css('border-bottom-width', '10px');
                $('td:nth-child(2)', row).css('width', '50px');
                $('td:nth-child(2)', row).css('font-weight', '700');
                $('td:nth-child(1)', row).css('font-weight', '700');
            }
            else {
                $('td:nth-child(1)', row).html('Net Loss');
                $('td:nth-child(2)', row).html('('+numFormat(data.total)+')');
                //$('td:nth-child(4)', row).html('$' + (data.totalCredits = data.totalDebits - data.totalDebits) + '.00');
                $('td:nth-child(1)', row).css('text-align', 'left');
                $('td:nth-child(2)', row).css('text-align', 'right');
                $('td:nth-child(2)', row).css('border-top', 'solid');
                $('td:nth-child(2)', row).css('border-bottom', 'double');
                $('td:nth-child(2)', row).css('border-bottom-width', '10px');
                $('td:nth-child(2)', row).css('width', '50px');
                $('td:nth-child(2)', row).css('font-weight', '700');
                $('td:nth-child(1)', row).css('font-weight', '700');
            }
            
       
          


        },
     
        "searching": false,
        "paging": false,
        "info": false,
        "sort": false,

    });

   
});


   



