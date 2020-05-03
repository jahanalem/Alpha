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
    selector: 'textarea#summaryId',
    max_chars: 255, // max. allowed chars
    plugins: "paste",
    setup: function (ed) {
        var allowedKeys = [8, 37, 38, 39, 40, 46]; // backspace, delete and cursor keys
        ed.on('keydown', function (e) {
            tinymce.triggerSave();
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




tinymce.init({
    selector: 'textarea#descId',
    plugins: 'print preview paste importcss searchreplace autolink autosave save directionality code visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern noneditable help charmap quickbars emoticons',
    imagetools_cors_hosts: ['picsum.photos'],
    menubar: 'file edit view insert format tools table help',
    toolbar: 'undo redo | bold italic underline strikethrough | fontselect fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media template link anchor codesample | ltr rtl',
    toolbar_sticky: true,
    autosave_ask_before_unload: true,
    autosave_interval: "30s",
    autosave_prefix: "{path}{query}-{id}-",
    autosave_restore_when_empty: false,
    autosave_retention: "2m",
    image_advtab: true,
    content_css: '//www.tiny.cloud/css/codepen.min.css',
    link_list: [
        { title: 'My page 1', value: 'http://www.tinymce.com' },
        { title: 'My page 2', value: 'http://www.moxiecode.com' }
    ],
    image_list: [
        { title: 'My page 1', value: 'http://www.tinymce.com' },
        { title: 'My page 2', value: 'http://www.moxiecode.com' }
    ],
    image_class_list: [
        { title: 'None', value: '' },
        { title: 'Some class', value: 'class-name' }
    ],
    importcss_append: true,
    height: 400,
    file_picker_callback: function (callback, value, meta) {
        /* Provide file and text for the link dialog */
        if (meta.filetype === 'file') {
            callback('https://www.google.com/logos/google.jpg', { text: 'My text' });
        }

        /* Provide image and alt text for the image dialog */
        if (meta.filetype === 'image') {
            callback('https://www.google.com/logos/google.jpg', { alt: 'My alt text' });
        }

        /* Provide alternative source and posted for the media dialog */
        if (meta.filetype === 'media') {
            callback('movie.mp4', { source2: 'alt.ogg', poster: 'https://www.google.com/logos/google.jpg' });
        }
    },
    templates: [
        { title: 'New Table', description: 'creates a new table', content: '<div class="mceTmpl"><table width="98%%"  border="0" cellspacing="0" cellpadding="0"><tr><th scope="col"> </th><th scope="col"> </th></tr><tr><td> </td><td> </td></tr></table></div>' },
        { title: 'Starting my story', description: 'A cure for writers block', content: 'Once upon a time...' },
        { title: 'New list with dates', description: 'New List with dates', content: '<div class="mceTmpl"><span class="cdate">cdate</span><br /><span class="mdate">mdate</span><h2>My List</h2><ul><li></li><li></li></ul></div>' }
    ],
    template_cdate_format: '[Date Created (CDATE): %m/%d/%Y : %H:%M:%S]',
    template_mdate_format: '[Date Modified (MDATE): %m/%d/%Y : %H:%M:%S]',
    height: 600,
    image_caption: true,
    quickbars_selection_toolbar: 'bold italic | quicklink h2 h3 blockquote quickimage quicktable',
    noneditable_noneditable_class: "mceNonEditable",
    toolbar_mode: 'sliding',
    contextmenu: "link image imagetools table",

    setup: function (editor) {
        editor.on('change', function () {
            tinymce.triggerSave();
        });
    }
});



//tinymce.init({ selector: '#summaryId', max_chars:"4" });

//tinymce.init(
//    {
//        selector: '#descId',
//        setup: function (editor) {
//            editor.on('change', function () {
//                tinymce.triggerSave();
//            });
//        }
//    }
//);


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






//$(document).ready(function (e) {

//    $("#usersDataTable").DataTable({
//        "processing": true, // for show progress bar
//        "serverSide": true, // for process server side
//        "filter": true, // this is for disable filter (search box)
//        "orderMulti": false, // for disable multiple column at once
//        "ajax": {
//            "url": "/Admin/Users/LoadData",
//            "type": "POST",
//            "datatype": "json"
//        },
//        "columnDefs":
//            [{
//                "targets": [0],
//                "visible": false,
//                "searchable": false
//            }],
//        "columns": [
//            { "data": "Id", "name": "Id", "autoWidth": true },
//            { "data": "UserName", "name": "UserName", "autoWidth": true },
//            { "data": "Email", "name": "Email", "autoWidth": true },
//            { "data": "RoleNames", "name": "RoleNames", "autoWidth": true },
//            {
//                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Admin/Users/Edit/' + full.Id + '">Edit</a>'; }
//            },
//            {
//                data: null, render: function (data, type, row) {
//                    return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.Id + "'); >Delete</a>";
//                }
//            }
//        ]

//    });
//});


//function DeleteData(Id) {
//    if (confirm("Are you sure you want to delete ...?")) {
//        Delete(Id);


//    }
//    else {
//        return false;
//    }
//}


//function Delete(Id) {
//    var url = "/admin/Users/Delete";
//    $.post(url, { ID: Id }, function (data) {
//        if (data) {
//            oTable = $('#usersDataTable').DataTable();
//            oTable.draw();
//        }
//        else {
//            alert("Something Went Wrong!");
//        }
//    });
//}



$(document).ready(function () {

    var key = $(".mytime").each(function (i, obj) {
        var element = $(this); // <div> or <span> element. 
        var utc = element.attr("utc"); // "2018-12-28T02:36:13.6774675Z"
        var d = new Date(utc);

        var l = convertUTCDateToLocalDate(d).toLocaleString();//d.toLocaleString(); // Runs client side, so will be client's local time!
        element.text(l);

    });
});

$(document).ready(function () {
    $("#sidebar ul").on('click', 'li', function (e) {
        //var url = window.location.href;
        //e.preventDefault();
        $("#sidebar ul li.active").removeClass('active');
        //$(this).addClass('active');
        //return this.href = url;
        //$(this).show();
    });
    var lnk = decodeURIComponent(location.pathname);
    $('a[href="' + lnk + '"]').parents('li').addClass('active');
});

