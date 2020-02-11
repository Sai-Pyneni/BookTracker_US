$(document).ready(function () {
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

    var otable = $('#eventTable').DataTable({

        "Dom": "t<'row'<'col-md-12'p >> ",
        "ajax": {
            "url": '/AcMnEventLog/LoadData',
            "type": "POST",
            "datatype": "json",
            "data": "data"
        },

        "columns": [

            { "data": "eventID", "name": "acctNo", "autoWidth": true },//index 1
            {
                "data": "eventDate",
                "render": function (d) {
                    return moment(d).format("MM/DD/YYYY");
                },
                "name": "acctName", "autoWidth": true
            },
            {
                "data": "fromPage",
              
                "name": "totalDebits", "autoWidth": true
            },
            {
                "data": "toPage",
               
                "name": "totalCredits", "autoWidth": true
            },
            {
                "data": "journalID",

                "name": "totalCredits", "autoWidth": true
            },
            {
                "data": "userID",

                "name": "totalCredits", "autoWidth": true
            },
            {
                "data": "userID2",

                "name": "totalCredits", "autoWidth": true
            },
        ],            

     


    });



});