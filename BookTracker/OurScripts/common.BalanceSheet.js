$(document).ready(function () {
    var otable3 = $('#assetsTotals').DataTable({

        "Dom": "t<'row'<'col-md-12'p >> ",
        "ajax": {
            "url": '/FinancialStatements/TotalAssets',
            "type": "POST",
            "datatype": "json",
            "data": "data"
        },

        "columns": [

            {
                "data": null, render: function (data) {
                    return "";
                }, "name": "total", "autoWidth": true
            },
           
            {
                "data": "sumTotal",
                "render": $.fn.dataTable.render.number(',', '.', 2),
                "name": "", "autoWidth": true
            },
            {
                "data": null, render: function (data) {
                    return "";
                }, "name": "total", "autoWidth": false
            },

        ],
        rowCallback: function (row, data) {
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2, '$').display;
       
          
            $('td:nth-child(3)', row).html(numFormat(data.sumTotal));
            $('td:nth-child(3)', row).css('text-align', 'right');
            $('td:nth-child(3)', row).css('width', '10%');
            $('td:nth-child(3)', row).css('text-undeline', 'double');
        
        },
        "searching": false,
        "paging": false,
        "info": false,
        "sort": false,


    });
	var otable = $('#currentassetsTable').DataTable({

		"Dom": "t<'row'<'col-md-12'p >> ",
		"ajax": {
            "url": '/FinancialStatements/CurrentAssets',
			"type": "POST",
			"datatype": "json",
			"data": "data"
		},

		"columns": [

            { "data": "acctName", "name": "acctName", "autoWidth": true },           
            {
            	"data": "balance",
            	"render": $.fn.dataTable.render.number(',', '.', 2),
            	"name": "balance", "autoWidth": false
            },
            {
                "data": null, render: function (data) {
                    return "";
                }, "name": "total", "autoWidth": false
            },

		],

        rowCallback: function (row, data, DisplayIndex) {
               
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2).display;

            if (DisplayIndex == 0) {
                $('td:nth-child(2)', row).html('$' + numFormat(data.balance));

            }
            if (data.balance < 0) {
                $('td:nth-child(2)', row).html('(' + numFormat(data.balance)*-1  + ')');
            }
         
            $('td:nth-child(2)', row).css('text-align', 'right');  
            $('td:nth-child(2)', row).css('width', '10%');  
            $('td:nth-child(1)', row).css('padding-left', '25px');  
          

            
           
            
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

          
            $(api.column(0).footer()).html('Total Current Assets');

            $(api.column(2).footer()).html(numFormat(debtTotal));
            $(api.column(2).footer()).css('text-align', 'right');
            $(api.column(0).footer()).css('text-size', '1rem');
            $(api.column(0).footer()).css('font-weight', '700');
            $(api.column(0).footer()).css('padding-left', '15px');
            $(api.column(1).footer()).css('border-top', 'solid');
            //$(api.column(1).footer()).css('border-bottom', 'double');
         
            $(api.column(2).footer()).css('width', '20%');
        },
		"searching": false,
		"paging": false,
		"info": false,
		"sort": false,


    });
 
	var otable2 = $('#longtermassetsTable').DataTable({

		"Dom": "t<'row'<'col-md-12'p >> ",
        "ajax": {
            "url": '/FinancialStatements/LongTermAssets',
            "type": "POST",
            "datatype": "json",
            "data": "data"
        },

        "columns": [

            { "data": "acctName", "name": "acctName", "autoWidth": true },
            {
                "data": "balance",
                "render": $.fn.dataTable.render.number(',', '.', 2),
                "name": "balance", "autoWidth": true
            },
            {
                "data": null, render: function (data) {
                    return "";
                }, "name": "total", "autoWidth": false
            },

        ],
        rowCallback: function (row, data, DisplayIndex) {
            $('td:nth-child(2)', row).css('text-align', 'right');
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2).display;

            if (DisplayIndex == 0) {
                $('td:nth-child(2)', row).html('$' + numFormat(data.balance));

            }
            if (data.balance < 0) {
                $('td:nth-child(2)', row).html('(' + numFormat(data.balance*-1) +')');
            }
            $('td:nth-child(1)', row).css('padding-left', '25px');  
            $('td:nth-child(2)', row).css('width', '10%'); 
         
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
            
                
            $(api.column(0).footer()).html('Property Plant & Equipment, Net');

            $(api.column(2).footer()).html(numFormat(debtTotal));
            $(api.column(2).footer()).css('text-align', 'right');
            $(api.column(0).footer()).css('text-size', '1rem');
            $(api.column(0).footer()).css('font-weight', '700');
            $(api.column(0).footer()).css('padding-left', '15px');
            $(api.column(1).footer()).css('border-top', 'solid');
            //$(api.column(1).footer()).css('border-bottom', 'double');

            $(api.column(2).footer()).css('width', '20%');
           // $(api.column(1).footer()).css('border-bottom', 'double');


        },
        "searching": false,
        "paging": false,
        "info": false,
        "sort": false,


    });
  
	var otable4 = $('#equityTable').DataTable({

        "Dom": "t<'row'<'col-md-12'p >> ",
        "ajax": {
            "url": '/FinancialStatements/CurrentLiabilites', 
            "type": "POST",
            "datatype": "json",
            "data": "data"
        },

        "columns": [

            { "data": "acctName", "name": "acctName", "autoWidth": true },
            {
                "data": "balance",
                "render": $.fn.dataTable.render.number(',', '.', 2),
                "name": "balance", "autoWidth": true
            },
            {
                "data": null, render: function (data) {
                    return "";
                }, "name": "total", "autoWidth": false
            },
        ],

        rowCallback: function (row, data, DisplayIndex) {
            $('td:nth-child(2)', row).css('text-align', 'right');
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2).display;

            if (DisplayIndex == 0) {
                $('td:nth-child(2)', row).html('$' + numFormat(data.balance));

            }

            if (data.balance < 0) {
                $('td:nth-child(2)', row).html('(' + numFormat(data.balance) * -1 + ')');
            }

            $('td:nth-child(1)', row).css('padding-left', '25px');  
            $('td:nth-child(2)', row).css('width', '10%');
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


            $(api.column(0).footer()).html('Total Liabilities');
            $(api.column(2).footer()).html(numFormat(debtTotal));
            $(api.column(2).footer()).css('text-align', 'right');
            $(api.column(0).footer()).css('text-size', '1rem');
            $(api.column(0).footer()).css('font-weight', '700');
            $(api.column(0).footer()).css('padding-left', '15px');
            $(api.column(1).footer()).css('border-top', 'solid');
            //$(api.column(1).footer()).css('border-bottom', 'double');

            $(api.column(2).footer()).css('width', '20%');
           



        },
        "searching": false,
        "paging": false,
        "info": false,
        "sort": false,

	});
	var otable5 = $('#liabilityTable').DataTable({


        "Dom": "t<'row'<'col-md-12'p >> ",
        "ajax": {
            "url": '/FinancialStatements/OwnersEquity',
            "type": "POST",
            "datatype": "json",
            "data": "data"
        },

        "columns": [

            { "data": "acctName", "name": "acctName", "autoWidth": true },
           
            {
                "data": null, render: function (data) {
                    return "";
                }, "name": "total", "autoWidth": false
            },
             {
                "data": "balance",
                "render": $.fn.dataTable.render.number(',', '.', 2),
                "name": "balance", "autoWidth": true
            },

        ],

        rowCallback: function (row, data, DisplayIndex) {
            $('td:nth-child(3)', row).css('text-align', 'right');
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2).display;

            if (DisplayIndex == 0) {
                $('td:nth-child(3)', row).html('$' + numFormat(data.balance));

            }
            if (data.balance < 0) {
                $('td:nth-child(3)', row).html('(' + numFormat(data.balance) * -1 + ')');
            }
            $('td:nth-child(1)', row).css('padding-left', '25px'); 
            $('td:nth-child(3)', row).css('width', '10%');



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
                .column(2)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);


            $(api.column(0).footer()).html("Total Stockholder's Equity");

            $(api.column(2).footer()).html(numFormat(debtTotal));
            $(api.column(2).footer()).css('text-align', 'right');
            $(api.column(0).footer()).css('text-size', '1rem');
            $(api.column(0).footer()).css('font-weight', '700');
            $(api.column(0).footer()).css('padding-left', '15px');
            $(api.column(2).footer()).css('border-top', 'solid');
            //$(api.column(1).footer()).css('border-bottom', 'double');

          
          

        },
        "searching": false,
        "paging": false,
        "info": false,
        "sort": false,
      

	});
    var otable6 = $('#liabilitiesTotals').DataTable({

        "Dom": "t<'row'<'col-md-12'p >> ",
        "ajax": {
            "url": '/FinancialStatements/TotalLiabilites',
            "type": "POST",
            "datatype": "json",
            "data": "data"
        },

        "columns": [

            {
                "data": null, render: function (data) {
                    return "";
                }, "name": "total", "autoWidth": true
            },
            {
                "data": null,
                render: function (data) {
                    return "";
                },
                "name": "", "autoWidth": true
            },
            {
                "data": "sumTotal",
                "render": $.fn.dataTable.render.number(',', '.', 2),
                "name": "", "autoWidth": true
            },

        ],
        rowCallback: function (row, data) {
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2, '$').display;


            $('td:nth-child(2)', row).html(numFormat(data.sumTotal));
            $('td:nth-child(2)', row).css('text-align', 'right');
            $('td:nth-child(2)', row).css('width', '20%');
            $('td:nth-child(2)', row).css('text-undeline', 'double');
        },
     
        "searching": false,
        "paging": false,
        "info": false,
        "sort": false,

    });

});
