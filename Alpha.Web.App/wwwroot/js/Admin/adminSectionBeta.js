function convertUTCDateToLocalDate(date) {
    var newDate = new Date(date.getTime() - date.getTimezoneOffset() * 60 * 1000);
    return newDate;
}



function tinymce_updateCharCounter(el, len) {
    $('#' + el.id).prev().find('.char_count').text(len + '/' + el.settings.max_chars);
}

function tinymce_getContentLength() {
    return tinymce.get(tinymce.activeEditor.id).contentDocument.body.innerText.length;
}

tinymce.init({
    selector: '#summaryId',
    max_chars: 255, // max. allowed chars
    plugins: "paste",
    setup: function (ed) {
        var allowedKeys = [8, 37, 38, 39, 40, 46]; // backspace, delete and cursor keys
        ed.on('keydown', function (e) {
            if (allowedKeys.indexOf(e.keyCode) != -1) return true;
            if (tinymce_getContentLength() + 1 > this.settings.max_chars) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
            return true;
        });
        ed.on('keyup', function (e) {
            tinymce_updateCharCounter(this, tinymce_getContentLength());
        });
    },
    init_instance_callback: function () { // initialize counter div
        $('#' + this.id).prev().append('<div class="char_count" style="text-align:left; color:grey;"></div>');
        tinymce_updateCharCounter(this, tinymce_getContentLength());
    },
    paste_preprocess: function (plugin, args) {
        var editor = tinymce.get(tinymce.activeEditor.id);
        var len = editor.contentDocument.body.innerHTML.length;
        var textLen = args.content.length;// $(args.content).text();
        if (len + textLen > editor.settings.max_chars) {
            alert('Pasting this exceeds the maximum allowed number of ' + editor.settings.max_chars + ' characters.');
            args.content = '';
        } else {
            //tinymce_updateCharCounter(editor, len + text.length);
        }
    }
});



//tinymce.init({ selector: '#summaryId', max_chars:"4" });

tinymce.init({ selector: '#descId' });


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