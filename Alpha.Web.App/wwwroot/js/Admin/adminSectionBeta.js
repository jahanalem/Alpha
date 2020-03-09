function convertUTCDateToLocalDate(date) {
    var newDate = new Date(date.getTime() - date.getTimezoneOffset() * 60 * 1000);
    return newDate;
}


tinymce.init({ selector: 'textarea' });


$(document).ready(function () {
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar, #content').toggleClass('active');
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
});






$(document).ready(function (e) {

    $("#usersDataTable").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Admin/Users/LoadData",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs":
        [{
            "targets": [0],
            "visible": false,
            "searchable": false
        }],
        "columns": [
            { "data": "Id", "name": "Id", "autoWidth": true },
            { "data": "UserName", "name": "UserName", "autoWidth": true },
            { "data": "Email", "name": "Email", "autoWidth": true },
            { "data": "RoleNames", "name": "RoleNames", "autoWidth": true },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Admin/Users/Edit/' + full.Id + '">Edit</a>'; }
            },
            {
                data: null, render: function(data, type, row) {
                     return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.Id + "'); >Delete</a>";
                }
            }
        ]

    });
});


function DeleteData(Id) {
    if (confirm("Are you sure you want to delete ...?")) {
        Delete(Id);


    }
    else {
        return false;
    }
}


function Delete(Id) {
    var url = "/admin/Users/Delete";
    $.post(url, { ID: Id }, function (data) {
        if (data) {
            oTable = $('#usersDataTable').DataTable();
            oTable.draw();
        }
        else {
            alert("Something Went Wrong!");
        }
    });
}



$(document).ready(function () {

    var key = $(".mytime").each(function (i, obj) {
        var element = $(this); // <div> or <span> element. 
        var utc = element.attr("utc"); // "2018-12-28T02:36:13.6774675Z"
        var d = new Date(utc);
        
        var l = convertUTCDateToLocalDate(d).toLocaleString();//d.toLocaleString(); // Runs client side, so will be client's local time!
        element.text(l);

    });
});