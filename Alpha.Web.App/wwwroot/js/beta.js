
$('[data-toggle="collapse"]').on('click', function () {
    var $this = $(this),
        $parent = typeof $this.data('parent') !== 'undefined' ? $($this.data('parent')) : undefined;
    if ($parent === undefined) { /* Just toggle my  */
        $this.find('.fa').toggleClass('fa-minus fa-plus');
        return true;
    }

    /* Open element will be close if parent !== undefined */
    var currentIcon = $this.find('.fa');
    currentIcon.toggleClass('fa-minus fa-plus');
    $parent.find('.fa').not(currentIcon).removeClass('fa-plus').addClass('fa-minus');

});





// Edit a comment // 
$(document).on("click", "input[id^='editC_']", function (event) {

    var clickedId = event.target.id;
    var commentId = clickedId.split("_")[1];
    var articleId = clickedId.split("_")[2];
    var targetId = "#cardId_" + commentId;
    var copyOfComment = $(targetId);
    var test = copyOfComment.clone().appendTo(jQuery.validator.format("#collapse_{0}", commentId)).addClass("hidden");

    var text = $(targetId)[0].innerText;
    $(targetId).replaceWith(jQuery.validator.format("<textarea id='textarea_{0}_{1}'>", commentId, articleId) + text + "</textarea>");
    var copyOfEditButton = $("#" + clickedId);
    var cancelBtn = $("#" + clickedId).replaceWith(jQuery.validator.format("<input id='cancelEdit_{0}_{1}'type='submit' class='submitLink' value='cancel'/>", commentId, articleId));

    $(jQuery.validator.format("#commentId_{0}", commentId)).children(':eq(1)').after(jQuery.validator.format("<span><input id='saveC_{0}_{1}' type='submit' class='submitLink' value='save' /></span>", commentId, articleId));
});

// Cancel editing //
$(document).on("click", "input[id^='cancelEdit_']", function (event) {
    var clickedId = event.target.id;
    var commentId = clickedId.split("_")[1];
    var articleId = clickedId.split("_")[2];
    var targetId = "#cardId_" + commentId;
    $("#textarea_" + jQuery.validator.format("{0}_{1}", commentId, articleId)).replaceWith($("#cardId_" + commentId).removeClass("hidden"));

    $("#" + clickedId).replaceWith(jQuery.validator.format("<input id='editC_{0}_{1}' type='submit' class='submitLink' value='edit'/>", commentId, articleId));
    $(jQuery.validator.format("#saveC_{0}_{1}", commentId, articleId)).remove();
});




$("a[id^='replyC_']").on("click", function (event) {
    var nodeId = event.target.id;
    var commentId = nodeId.split("_")[1];
    var articleId = nodeId.split("_")[2];
    var commentReplyId = "#replyComment_" + commentId.toString();
    var commentTextId = "#commentText_" + commentId;
    var formId = "form_" + commentId;
    var valueOftextarea = $(commentTextId).val();
    var appendReplyNode = true;
    if (typeof valueOftextarea != "undefined") {
        if (valueOftextarea.length >= 0) {
            appendReplyNode = false;
        }
    }
    if (appendReplyNode) {
        $(commentReplyId).append(
            $('<form />', { method: 'post', action: '/Comment/Save', id: formId }).append(
                $('<div/>', { class: 'form-group' }).append(
                    $('<textarea />',
                        {
                            class: 'form-control',
                            id: 'commentText_' + commentId,
                            name: 'Dsc',
                            placeholder: 'your comment',
                            type: 'text',
                            row: '3'
                        }),
                    $('<input/>', { type: 'hidden', name: 'CurrentArticleId', id: 'CurrentArticleId', value: articleId }),
                    $('<input/>', { type: 'hidden', name: 'CurrentParentId', id: 'CurrentParentId', value: commentId }),
                    $('<input />', { id: 'submitComment_' + commentId, type: 'submit', value: 'send', class: 'btn btn-secondary' })
                )
            ));
    }

});


/* ************* Save editing *************  */
$(document).on("click", "input[id^='saveC_']", function (event) {
    event.preventDefault();
    var clickedId = event.target.id;
    var commentId = clickedId.split("_")[1];
    var articleId = clickedId.split("_")[2];
    var comment = $("#" + jQuery.validator.format("textarea_{0}_{1}", commentId, articleId)).val();
    var formData = new FormData();
    formData.append("Dsc", comment);
    formData.append("ArticleId", articleId);
    formData.append("CommentId", commentId);

    var data = {
        "CommentId": commentId,
        "ArticleId": articleId,
        "Dsc": comment
    };
    var dataJSON = JSON.stringify(data);
    $.ajax({
        url: "/Comment/EditComment",
        type: "POST",
        datatype: "JSON",
        data: dataJSON,
        contentType: "application/json; charset=utf-8",
        processData: false,
        success: function (data, textStatus, jqXhr) {
            $("#textarea_" + jQuery.validator.format("{0}_{1}", commentId, articleId)).replaceWith($("#cardId_" + commentId).removeClass("hidden"));
            $("#cardId_" + commentId)[0].innerText = comment;
            $("#" + clickedId).replaceWith(jQuery.validator.format("<input id='editC_{0}_{1}' type='submit' value='edit' class='submitLink'/>", commentId, articleId));
            $(jQuery.validator.format("#cancelEdit_{0}_{1}", commentId, articleId)).remove();
        },
        failure: function (response) {
            alert(response);
        }
    });
});

/* ************* Save editing *************  */
$(document).on("click", "input[id^='deleteC_']", function (event) {
    event.preventDefault();
    var clickedId = event.target.id;
    var commentId = clickedId.split("_")[1];
    var articleId = clickedId.split("_")[2];
    var data = {
        "CommentId": commentId,
        "ArticleId": articleId
    };
    var dataJSON = JSON.stringify(data);
    $.ajax({
        url: "/Comment/DeleteComment",
        type: "POST",
        datatype: "JSON",
        data: dataJSON,
        contentType: "application/json; charset=utf-8",
        processData: false,
        success: function (data, textStatus, jqXhr) {
            $("#commentNode_" + commentId).remove();
            $("#collapse_" + commentId).remove();
        },
        failure: function (response) {
            alert(response);
        }
    });
});




$(document).on("click", "input[id^='submitComment_']", function (event) {
    event.preventDefault();
    var submitCommentId = event.target.id;
    var id = submitCommentId.split("_")[1];
    var commentTextId = jQuery.validator.format('#commentText_{0}', id);
    var formData = new FormData();
    formData.append("Dsc", $(commentTextId).val());
    formData.append("ArticleId", $("#CurrentArticleId").val());
    formData.append("ParentId", $("#CurrentParentId").val());
    $.ajax({
        url: "/Comment/Save",
        //beforeSend: function (xhr) {
        //    xhr.setRequestHeader("XSRF-TOKEN",
        //        $('input:hidden[name="__RequestVerificationToken"]').val());
        //},
        type: "POST",
        datatype: "JSON",
        data: formData,
        contentType: false,
        processData: false,
        success: function (data, textStatus, jqXhr) {
            var nodeId = data;
            var parentId = event.target.id.split("_")[1];
            var collapseId = jQuery.validator.format('#cardId_{0}', parentId);
            var appendTo = collapseId + "~ul";
            if ($(appendTo).length) {
                var ee = $(appendTo).prepend("<li>Saved Successfully!</li>");
            }
            else {
                $(collapseId).append("<ul><li>Saved Successfully!</li></ul>");
            }

            //            document.location.reload();
        },
        failure: function (response) {
            alert(response);
        }
    });

});


