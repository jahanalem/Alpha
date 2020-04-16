function onSignIn(googleUser) {
    var profile = googleUser.getBasicProfile();
    console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
    console.log('Name: ' + profile.getName());
    console.log('Image URL: ' + profile.getImageUrl());
    console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
}

function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

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

// convert utc time to local time
function convertUTCDateToLocalDate(date) {
    var newDate = new Date(date.getTime() - date.getTimezoneOffset() * 60 * 1000);
    return newDate;
}

$(document).ready(function () {
    $("#searchForm").submit(function (event) {
        var searchTerm = $("#searchBox").val();
        if (searchTerm.trim().length > 0) {
            return;
        }

        alert("You must enter some characters.");
        event.preventDefault();
    });
});

$(document).ready(function () {

    var key = $(".mytime").each(function (i, obj) {
        var element = $(this); // <div> or <span> element. 
        var utc = element.attr("utc"); // "2018-12-28T02:36:13.6774675Z"
        var d = new Date(utc);

        var l = convertUTCDateToLocalDate(d).toLocaleString();//d.toLocaleString(); // Runs client side, so will be client's local time!
        element.text(l);

    });
});


/* <<<<<<<<<<<<<<<<<<<  Edit   >>>>>>>>>>>>>>>>>>> */
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

/* <<<<<<<<<<<<<<<<<<<  Delete   >>>>>>>>>>>>>>>>>>> */
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


/* <<<<<<<<<<<<<<<<<<<  Submit   >>>>>>>>>>>>>>>>>>> */
$(document).on("click", "input[id^='submitComment_']", function (event) {
    event.preventDefault();
    var submitCommentId = event.target.id;
    var id = submitCommentId.split("_")[1];
    var commentTextId = jQuery.validator.format('#commentText_{0}', id);
    var formData = new FormData();
    var newMessage = $(commentTextId).val();
    var currentArticleId = $("#CurrentArticleId").val();
    formData.append("Dsc", newMessage);
    formData.append("ArticleId", currentArticleId);
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
            var newCommentId = data.id;
            var user = data.user;
            var parentId = event.target.id.split("_")[1];
            var collapseId = jQuery.validator.format('#collapse_{0}', parentId);

            $("#form_" + parentId).remove();
            var appendTo = collapseId + "~ul";
            if ($(appendTo).length) {
                var ee = $(appendTo).prepend("<li>Saved Successfully!</li>");
            }
            else {
                var newCommentTemplate = '<div class="media mt-3 w-100" id="commentNode_{{ID}}">' +
                    '<a class="pr-0" href = "#" >' +
                    '<img class="mr-3" src="/images/comment1.png" alt="x">' +
                    '</a>' +
                    '<div class="media-body w-100">' +
                    '<h5 class="mt-0 mb-1">{{USER}}</h5>' +
                    '<div id="collapse_{{ID}}" class="">' +
                    '<div id="cardId_{{ID}}" class="card">' +
                    '<p>{{MESSAGE}}</p>' +
                    '</div>' +
                    '<div class="comment-meta" id="commentId_{{ID}}">' +
                    '<span><input id="deleteC_{{ID}}_{{ARTICLEID}}" type="submit" class="submitLink" value="delete"></span>' +
                    '<span><input id="editC_{{ID}}_{{ARTICLEID}}" type="submit" class="submitLink" value="edit"></span>' +
                    '<span>' +
                    '<a id="replyC_{{ID}}_{{ARTICLEID}}" class="" role="button" data-toggle="collapse" href="#replyComment_{{ID}}" aria-expanded="false" aria-controls="collapseExample">reply</a>' +
                    '</span>' +
                    '<div id="replyComment_{{ID}}" class="collapse"></div>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '</div>';
                var newComment = newCommentTemplate.replace(/{{USER}}/g, user)
                    .replace(/{{ID}}/g, newCommentId)
                    .replace(/{{ARTICLEID}}/g, currentArticleId)
                    .replace(/{{MESSAGE}}/g, newMessage);
                $(collapseId).append(newComment);
            }
        },
        failure: function (response) {
            alert(response);
        }
    });

});

/* <<<<<<<<<<<<<<<<<<<  Reply   >>>>>>>>>>>>>>>>>>> */

$(document).on("click", "a[id^='replyC_']", function (event) {
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

//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////

function togglePasswordVisibility() {
    var x = document.getElementById("Password");
    var y = document.getElementById("ConfirmPassword");
    if (x.type === "password") {
        x.type = "text";
        y.type = "text;";
    } else {
        x.type = "password";
        y.type = "password";
    }
}

$('#signupForm').validate(); //https://johnnycode.com/2014/03/27/using-jquery-validate-plugin-html5-data-attribute-rules/
//$('#contact-form').validate();

// Make Highlight Active Item In Menu
$(document).ready(function () {
    var lnk0 = decodeURIComponent(location.pathname);
    $('a[href="' + lnk0 + '"]').parents('li').addClass('active');

    $("#navbarResponsive ul").on('click', 'li', function (e) {
        $("#navbarResponsive ul li.active").removeClass('active');
    });
    var lnk = decodeURIComponent(location.pathname);
    $('a[href="' + lnk + '"]').parents('li').addClass('active');
});

// CONTACT FORM
$(document).ready(function () {
    document.getElementById("submitContactForm").disabled = true;
    var form = document.getElementById("contact-form");
    "change keydown keyup".split(" ").forEach(function (e) {
        form.addEventListener(e, () => {
            document.getElementById("submitContactForm").disabled = !form.checkValidity();
            var valid = $("#contact-form").validate().checkForm();
            if (valid && validateEmail($("#Email").val())) {
                $('#submitContactForm').prop('disabled', false);
                $('#submitContactForm').removeClass('isDisabled');
            } else {
                $('#submitContactForm').prop('disabled', 'disabled');
                $('#submitContactForm').addClass('isDisabled');
            }
        });
    });

    $('#contact-form').validate({
        validClass: "success",
        errorClass: "danger",

        //...your validation rules come here,
        invalidHandler: function (event, validator) {
            // 'this' refers to the form

            var errors = validator.numberOfInvalids();
            var messageText;
            var alertBox;
            if (errors) {
                messageText = errors === 1
                    ? 'You missed 1 field. It has been highlighted'
                    : 'You missed ' + errors + ' fields. They have been highlighted';
                alertBox = '<div class="alert alert-danger ' +
                    ' alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' +
                    messageText +
                    '</div>';
                $("div.messages").html(alertBox);
                $("div.messages").show();
            } else {

                $("div.messages").hide();

            }
        },

        submitHandler: function (form) {
            //e.preventDefault();
            //var $form = $(this);
            //if (!$form.valid()) return false;
            form.submitContactForm.disabled = true;

            //var formAction = $(this).attr("action");

            var firstNumber = $("#FirstNumber").val();
            var secondNumber = $("#SecondNumber").val();
            var captchaResult = $("#Result").val();
            if (parseInt(captchaResult) !== (parseInt(firstNumber) + parseInt(secondNumber))) {
                var messageText = 'The sum is wrong';
                var alertBox = '<div class="alert alert-danger ' +
                    ' alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' +
                    messageText +
                    '</div>';
                $("div.messages").html(alertBox);
                $("div.messages").show();
                return;
            }


            var firstName = $("#FirstName").val();
            var lastName = $("#LastName").val();
            var email = $("#Email").val();
            var title = $("#Title").val();
            var description = $("#Description").val();


            var formData = new FormData();
            formData.append("FirstName", firstName);
            formData.append("LastName", lastName);
            formData.append("Email", email);
            formData.append("Title", title);
            formData.append("Description", description);
            formData.append("FirstNumber", firstNumber);
            formData.append("SecondNumber", secondNumber);
            formData.append("Result", captchaResult);

            $.ajax({
                type: form.method,
                url: form.action,
                data: formData,
                processData: false,
                contentType: false
            }).done(function (result) {
                console.log(result);
                if (result.status === "success") {
                    var messageAlert = 'alert-' + result.type;
                    var messageText = result.message;

                    var alertBox = '<div class="alert ' +
                        messageAlert +
                        ' alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' +
                        messageText +
                        '</div>';

                    if (messageAlert && messageText) {
                        // inject the alert to .messages div in our form
                        $('#contact-form').find('.messages').html(alertBox);
                        // empty the form
                        $('#contact-form')[0].reset();
                    }
                } else {
                    alert(result.message);
                }
            });
        }
    });
});