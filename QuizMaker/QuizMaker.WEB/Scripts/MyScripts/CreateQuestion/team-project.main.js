jQuery(function ($) {
    window.startHub().done(function () {
        var quiz_id = $("#Value_quiz_id").val();
        hub.server.addToGroup(quiz_id);
    })

    $("#ReadOnly").change(function () {
        
        //$(this).prop('disabled', true);
        //console.log("disabled");
        var quiz_id = $("#Value_quiz_id").val();
        var val = $(this).prop('checked');
        var dis = document.getElementById("ReadOnly");
        dis.disabled = true;

        //var box = document.getElementById("forma");
        //var owners_bxo = document.getElementById("owners_box");
        //if (owners_bxo != null)
        //    owners_bxo.remove();
        //if (box != null)
        //    box.remove();
        if (val) {
            window.startHub().done(function () {
                hub.server.disable(quiz_id);
            })
        } else {
            window.startHub().done(function () {
                hub.server.enable(quiz_id);
            })
        }
        var item = {
            Id: quiz_id,
            Value: val
        };
        $.ajax({
            url: '/Group/ReadOnly',
            type: 'post',
            dataType: 'json',
            data: item,
            success: function (response) {
                if (response.success) {
                    Success(" Quiz's read-only atribute is successfuly changed to " + response.val + "!");
                    $("#ReadOnly").prop('checked', response.val);
                }
                else {
                    Failed("Something is wrong. Refresh the page");
                    $("#ReadOnly").prop('checked', !response.val);
                }
                $("#ReadOnly").prop('disabled', false);
            }
        });

            
    });

    $("#user_addbtn").click(function () {
        var quiz_id = $("#Value_quiz_id").val();
        var userId = $("#UserList").val();
        var username = $("#UserList").find(":selected").text();
        var user = {
            User_Id: userId,
            Quiz_Id: quiz_id,
            Username: username
        };
        $.ajax({
            url: '/Group/AddUser',
            type: 'post',
            dataType: 'json',
            data: user,
            success: function (response) {
                if (response.success) {
                    window.startHub().done(function () {
                        hub.server.broadcast(response.user.Value, response.quiz.Value, response.quiz.Id);
                    })
                    UserContainer.add(response.user.Value/*, response.user.Id*/);
                }
                else {
                    Failed("Something is wrong. Refresh the page");
                }
            }
        });
    });
    $("#btnEditQuiz").click(function () {
        var validate = true;
        var quiz_id = $("#Value_quiz_id").val();
        var category = $("#Quiz_Category_Id").val();
        var name = $("#Quiz_Name").val();
        var quiz = {
            Name: name,
            Category_Id: category
        };


        if (validate) { 
            $.ajax({
                url: '/Quiz/Edit/' + quiz_id,
                type: 'post',
                dataType: 'json',
                data: quiz,
                success: function (response) {
                    if (response.success) {
                        Success(" Changes are successfuly saved!");

                    }
                    else {
                        Failed("Something is wrong. Refresh the page");
                    }
                }
            });
        }
    });
});


window.startHub();
hub.client.message = function (question_model, quiz_id) {
   // console.log(question_model);
    var model = JSON.parse(question_model);
    //console.log(model);
    item = {
        Id: quiz_id,
        Value: ""
    };
    $.ajax({
        url: '/Quiz/IsOwner',
        type: 'post',
        dataType: 'json',
        data: item,
        success: function (response) {
            QuestionContainer.addQuestion(model, response.IsOwner);
        }
    });
}
hub.client.remove = function (id) {
    QuestionContainer.removeQuestion(id);
}
hub.client.disable = function () {
    var box = document.getElementById("forma");
    while (box.firstChild) {
        box.removeChild(box.firstChild);
    }
    DisableFunctionn();
    var arr = document.querySelectorAll("i.pull-right");
    for (var i = 0; i < arr.length; i++) {
        NodeList.prototype.forEach = Array.prototype.forEach
        var children = arr[i].childNodes;
        children.forEach(function (item) {
            //  console.log(item.classList);
            item.disabled = true;
            //  console.log(item.disabled);
        });
    }
}
hub.client.enable = function () {
    //window.location.reload(true);
    var type = $("#Value_quiz_type").val();
    if (Container.isEmpty()) {
        run(type);
    } else {
        Container.show();
    }
    
    //
    var arr = document.querySelectorAll("i.pull-right");
   
    for (var i = 0; i < arr.length; i++) {
        NodeList.prototype.forEach = Array.prototype.forEach
        var children = arr[i].childNodes;
        children.forEach(function (item) {
            //  console.log(item.classList);
            item.disabled = false;
            //  console.log(item.disabled);
        });
    }
}
hub.client.readOnly = function (questionId, disable) {

    if (disable) {
        EditQuestion.disable(questionId);
    } else {
        EditQuestion.enable(questionId);
    }
    
}
hub.client.displayChanges = function (question_model, quiz_id) {
    var model = JSON.parse(question_model);
    item = {
        Id: quiz_id,
        Value: ""
    };
    $.ajax({
        url: '/Quiz/IsOwner',
        type: 'post',
        dataType: 'json',
        data: item,
        success: function (response) {
            EditQuestion.displayChangedQuestion(model, response.IsOwner);
        }
    });
}

function DisableFunctionn() {
    //override dialog's title function to allow for HTML titles
    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || '&nbsp;'
            if (("title_html" in this.options) && this.options.title_html == true)
                title.html($title);
            else title.text($title);
        }
    }));

    $("#dialog-disable").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4 class='smaller'></h4></div>",
        title_html: true,
        buttons: []
    });
}


function DeleteFunctionn(id) {
    var url = '/Group/Delete/' + id;
    //alert(id);
    //override dialog's title function to allow for HTML titles
    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || '&nbsp;'
            if (("title_html" in this.options) && this.options.title_html == true)
                title.html($title);
            else title.text($title);
        }
    }));
    var quizId = $("#Value_quiz_id").val();
    // var success = Request.Url.Segments.Last();

    $("#dialog-confirm").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4 class='smaller'>Delete this question</h4></div>",
        title_html: true,
        buttons: [
            {
                html: "<i class='icon-trash bigger-110'></i>&nbsp; Delete",
                "class": "btn btn-danger btn-xs",
                click: function () {
                    $.ajax({
                        url: '/Group/Delete/' + id,
                        type: 'get',
                        dataType: 'json',
                        success: function (response) {
                            console.log(response.success);
                            if (response.success) {
                                Success(" Question successfully deleted.");
                                QuestionContainer.removeQuestion(id);
                                window.startHub().done(function () {
                                    hub.server.removeQuestion(id, quizId);
                                })
                            }
                            else {
                                Failed(" Something is wrong. Refresh the page");
                            }
                        },
                        error: function (req, textStatus, errorThrown) {
                            //this is going to happen when you send something different from a 200 OK HTTP
                            // alert('Ooops, something happened: ' + textStatus + ' ' + errorThrown);
                        }
                    });
                    $(this).dialog("close");
                }
            }
            ,
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp; Cancel",
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}


var question = new QuestionModel();
function run(type) {
    question.setStrategy(type);

    question.Display();
    question.DisplayCreateModel();
    $('#Submit_btn').click(function () {
        var question_model = question.SubmitForm();
        if (question_model != null) {
            $.ajax({
                url: '/Question/Create',
                type: 'post',
                dataType: 'json',
                data: question_model,
                success: function (response) {
                    if (response.success) {
                        question.Reset();
                        var model = JSON.stringify(response.model);
                        window.startHub().done(function () {
                            hub.server.sendQuestion(model, question_model.Quiz_Id);
                        })
                       
                    } else {
                        Failed(" Question saving failed. Please try again!");
                    }
                }
            });
        }
    });
    $('#Edit_btn').click(function () {
        var question_model = question.SubmitEditForm();
        console.log(question_model);
        if (question_model != null) {
            $.ajax({
                url: '/Question/Edit',
                type: 'post',
                dataType: 'json',
                data: question_model,
                success: function (response) {
                    if (response.success) {
                        //console.log(response.qmodel);
                        EditQuestion.submitButton();
                        question.Reset();
                        Success(" Question successfuly edited!");
                        var question_model = JSON.stringify(response.qmodel);
                        var model = JSON.parse(question_model);
                        EditQuestion.displayChangedQuestion(model);
                        window.startHub().done(function () {
                            console.log("u ajaxu");
                            console.log(model);
                            var quiz_id = parseInt($("#Value_quiz_id").val());
                            hub.server.sendChanges(question_model, quiz_id);
                            hub.server.readOnly(model.Id, false, quiz_id);
                        })

                    } else {
                        Failed();
                        //window.location.href = response.responseUrl;
                    }
                    //
                }
            });

        }
    });
    $('#Discard_btn').click(function () {
        var ID = parseInt($("#Value_question_id").val());
        var item = {
            Id: ID,
            Value: false
        }
        //ajax call to change readonly
        $.ajax({
            url: '/Question/ReadOnly',
            type: 'post',
            dataType: 'json',
            data: item,
            success: function (response) {

                EditQuestion.submitButton();
                question.Reset();
                EditQuestion.enable(response.Id);
                window.startHub().done(function () {
                    var quiz_id = $("#Value_quiz_id").val();
                    hub.server.readOnly(response.Id, false, quiz_id);
                })
            }
        });
        //signalr send to all to change readonly

    });
    refresh();
}
function EditFunctionn(id) {
    question.Edit(id, true);
    //signalr
    window.startHub().done(function () {
        var quiz_id = $("#Value_quiz_id").val();
        hub.server.readOnly(id, true, quiz_id);
    })
}

var _wasPageCleanedUp = false;
function pageCleanup() {
    if (!_wasPageCleanedUp) {
        var ID = parseInt($("#Value_question_id").val());
        var item = {
            Id: ID,
            Value: false
        }
        $.ajax({
            url: '/Question/ReadOnly',
            type: 'post',
            dataType: 'json',
            async: false,
            data: item,
            success: function (response) {
                _wasPageCleanedUp = true;
            }
        });
    }
}


$(window).on('beforeunload', function () {
    //this will work only for Chrome
    pageCleanup();
});

$(window).on("unload", function () {
    //this will work for other browsers
    pageCleanup();
});