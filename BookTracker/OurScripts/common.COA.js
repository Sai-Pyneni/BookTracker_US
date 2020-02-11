$(document).ready(function () {
    var otable = $('#accountsTable').DataTable({
        "processing": true,
        // "serverSide": true,
        "ajax": {
            "url": '/Journal/CoAView',
            "type": "POST",
            "datatype": "json",
            "data": "data"
        },
        "columns": [


            //index 0
            {
                "data": "acctNo",
                "render": function (data, type) {
                    if (type == 'display') {
                        return "<a href='#' onclick='ledgerModal(" + data + ")'>" + data + " </a>";
                    }
                    return data;
                },

                "name": "acctNo", "autoWidth": true
            },//index 1
            { "data": "acctName", "name": "acctName", "autoWidth": true },//index 1
            { "data": "catName", "name": "acctCat", "autoWidth": true },//index 2
            { "data": "acctSubCatName", "name": "acctSubCat", "autoWidth": true },//index 3
            {
                "data": "balance",
                "render": $.fn.dataTable.render.number(',', '.', 2, '$'),
                "name": "balance", "autoWidth": true
            },//index 4
            { "data": "createdBy", "name": "createdBy", "autoWidth": true },//index 5
            {
                "data": "dateCreated",
                "render": function (d) {
                    return moment(d).format("MM/DD/YYYY");
                },
                "name": "dateCreated", "autoWidth": true
            },//index 6
            { "data": "actState", "name": "acctStateID", "autoWidth": true },//index 7
            { "data": "comments", "name": "comments", "autoWidth": true },
            { "data": "termName", "name": "termName", "autoWidth": true },

            //index 8

        ],

        "columnDefs": [
            { className: "text-right", "targets": 4 },

        ],

        rowCallback: function (row, data) {
            var numFormat = $.fn.dataTable.render.number('\,', '.', 2, '$').display;
            if (data.balance < 0) {
                $('td:nth-child(5)', row).html(numFormat(data.balance * -1));
            }
                
            //$('td:nth-child(4)', row).html('$' + (data.totalCredits = data.totalDebits - data.totalDebits) + '.00');
           

        },


    });
    function sqlToJsDate(sqlDate) {
        //sqlDate in SQL DATETIME format ("yyyy-mm-dd hh:mm:ss.ms")
        var sqlDateArr1 = sqlDate.split("-");
        //format of sqlDateArr1[] = ['yyyy','mm','dd hh:mm:ms']
        var sYear = sqlDateArr1[0];
        var sMonth = (Number(sqlDateArr1[1]) - 1).toString();
        var sqlDateArr2 = sqlDateArr1[2].split(" ");
        //format of sqlDateArr2[] = ['dd', 'hh:mm:ss.ms']
        var sDay = sqlDateArr2[0];
        var sqlDateArr3 = sqlDateArr2[1].split(":");
        //format of sqlDateArr3[] = ['hh','mm','ss.ms']
        var sHour = sqlDateArr3[0];
        var sMinute = sqlDateArr3[1];
        var sqlDateArr4 = sqlDateArr3[2].split(".");
        //format of sqlDateArr4[] = ['ss','ms']
        var sSecond = sqlDateArr4[0];
        var sMillisecond = sqlDateArr4[1];

        return new Date(sYear, sMonth, sDay, sHour, sMinute, sSecond, sMillisecond);
    }
    $('#table-filter').on('change', function () {
        otable.search(this.value).draw();
    });
    $('#table-filter1').on('change', function () {
        otable.search(this.value).draw();
    });
    $('#table-filter2').on('change', function () {
        otable.search(this.value).draw();
    });


});



//function commented already out in both, copied and lefted commented out
   /* function AddAccounts(acctNo) {
        $("form")[0].reset();
        acctNo = $("#acctNo").val();
        $("#modatTitle").html("Add New Account");
        $("#acctCatDD option:selected").text("--Select Cat--");
        $("#addAccounts").modal();

    }*/
