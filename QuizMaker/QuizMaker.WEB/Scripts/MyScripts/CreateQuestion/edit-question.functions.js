var EditQuestion = (function () {

    return {
        editTest: function (model) {
            if (model != null) {
                $("#Value_QuestionText").val(model.Question_text);
                $("#Value_question_id").val(model.Id);

                if (model.Answers.length == 1) {
                   $("#input_field").prop("checked", true);
                    $("#input_field").change();
                    $("#Value_CorrectAnswer").val(model.Answers[0].Text);
                } else {
                    $("#multiple_answers").prop("checked", true);
                    $("#multiple_answers").change();
                    for (var i = 0; i < model.Answers.length; i++) {
                        Container.addAnswerToList(model.Answers[i].Text);
                    }
                }
                $("#Value_points").val(model.Points);
                this.editButton();
            }
        },
        editPoll: function (model) {
            if (model != null) {
                $("#Value_QuestionText").val(model.Question_text);
                $("#Value_question_id").val(model.Id);

                    for (var i = 0; i < model.Answers.length; i++) {
                        Container.addAnswerToList(model.Answers[i].Text);
                    }
                this.editButton();
            }
        },
        editSurvey: function (model) {
            if (model != null) {
                $("#Value_QuestionText").val(model.Question_text);
                $("#Value_question_id").val(model.Id);

                if (model.Answers.length == 1) {
                    $("#input_field").prop("checked", true);
                    $("#input_field").change();
                    $("#Value_CorrectAnswer").val(model.Answers[0].Text);
                } else {
                    $("#multiple_answers").prop("checked", true);
                    $("#multiple_answers").change();
                    for (var i = 0; i < model.Answers.length; i++) {
                        Container.addAnswerToList(model.Answers[i].Text);
                    }
                }
                this.editButton();
            }
        },
        editButton: function () {
            var submit = document.getElementById("Submit_btn");
            submit.classList.add("hide");
            var save = document.getElementById("Edit_btn");
            save.classList.remove("hide");
            var discard = document.getElementById("Discard_btn");
            discard.classList.remove("hide");
            
            //var button_box = document.getElementById("button_box");
            //var save = document.createElement('button');
            //save.classList.add("btn", "btn-success", "no-border");
            //save.innerHTML = '<i class="icon-save bigger-110"></i> ' + "Save";
            //save.id = "Edit_btn";
            //save.onclick = function () {
            //   // alert("sss");
            //    var question_model = question.SubmitEditForm();
            //    // console.log(model);
            //    if (question_model != null) {
            //        $.ajax({
            //            url: '/Question/Edit',
            //            type: 'post',
            //            dataType: 'json',
            //            data: question_model,
            //            success: function (response) {
            //                if (response.success) {
            //                    console.log(response.qmodel);
            //                    question.Reset();
            //                    Success(" Question successfuly edited!");
            //                    var question_model = JSON.stringify(response.qmodel);
            //                    var model = JSON.parse(question_model);
            //                    EditQuestion.displayChangedQuestion(model);
            //                } else {
            //                    Failed();
            //                    //window.location.href = response.responseUrl;
            //                }
            //                //
            //            }
            //        });
            //    }

                

            //};
            //button_box.appendChild(save);
        },
        submitButton: function () {
            var submit = document.getElementById("Submit_btn");
            submit.classList.remove("hide");
            var save = document.getElementById("Edit_btn");
            save.classList.add("hide");
            var discard = document.getElementById("Discard_btn");
            discard.classList.add("hide");
        },
        displayChangedQuestion: function (question, isOwner) {
            var ID = "" + question.Id;
            var boxx = document.getElementById(ID);

            while (boxx.firstChild) {
                boxx.removeChild(boxx.firstChild);
            }

            var dl = document.createElement('dl');
            dl.className = "dl-horizontal";

            var pull_right = document.createElement('i');
            pull_right.className = "pull-right";
            if (isOwner) {
            var a = document.createElement('button');
            a.classList.add("btn", "btn-danger", "btn-sm", "pull-right", "no-border");
            a.setAttribute('data-id', question.Id);
            a.onclick = function () {
                //console.log(this.getAttribute('data-id'));
                DeleteFunctionn(this.getAttribute('data-id'));
            }
            var ii = document.createElement('i');
            ii.classList.add("icon-trash", "icon-only", "bigger-130");
            a.appendChild(ii);
            pull_right.appendChild(a);
        }
            var aa = document.createElement('button');
            aa.classList.add("btn", "btn-primary", "btn-sm", "pull-right", "no-border");
            aa.setAttribute('data-id', question.Id);
            aa.onclick = function () {
                //console.log(this.getAttribute('data-id'));
                EditFunctionn(this.getAttribute('data-id'));
            }
            

            
            var aii = document.createElement('i');
            aii.classList.add("icon-edit", "icon-only", "bigger-130");
            aa.appendChild(aii);
            pull_right.appendChild(aa);
            boxx.appendChild(pull_right);

            for (var someKey in question) {
                if (question.hasOwnProperty(someKey)) {
                    console.log(someKey);
                    if (question[someKey] != null) {
                        if (someKey === "Question_number" || someKey === "Type" || someKey === "Id" || someKey === "ReadOnly") {
                            //do nothing
                        } else {
                            if (someKey === "Points") {
                                //alert(question["Type"]);
                                if (question["Type"] == "test") {
                                    var prop = question[someKey];
                                    var dt = document.createElement('dt');
                                    dt.innerText = someKey + ":";
                                    dl.appendChild(dt);

                                    var dd = document.createElement('dd');
                                    dd.innerText = prop;
                                    dl.appendChild(dd);
                                }
                            } else {
                                var prop = question[someKey];
                                var dt = document.createElement('dt');
                                if (someKey === "Question_text") {
                                    dt.innerText = "Text:";
                                } else {
                                    dt.innerText = someKey + ":";
                                }
                                dl.appendChild(dt);
                                if (prop instanceof Array) {

                                    if (prop.length < 2 && someKey == "Answers") {
                                        var dd = document.createElement('dd');
                                        var ins = document.createElement('ins');
                                        var italic = document.createElement('i');
                                        if (question["Type"] == "test") {
                                            italic.innerHTML = "&nbsp; &nbsp; &nbsp;" + prop[0].Text + " &nbsp; &nbsp; &nbsp;";
                                        } else {
                                            italic.innerHTML = "&nbsp; &nbsp; &nbsp;  User input &nbsp; &nbsp; &nbsp;";
                                        }
                                        ins.appendChild(italic);
                                        dd.appendChild(ins);
                                        //console.log(prop);
                                        dl.appendChild(dd);
                                    } else {
                                        dl.appendChild(document.createElement('br'));
                                        for (var i = 0; i < prop.length; i++) {
                                            var dd = document.createElement('dd');
                                            //console.log(prop[i]);
                                            var ico = document.createElement('i');
                                            if (question["Type"] == "test") {
                                                if (prop[i].IsCorrect) {
                                                    ico.className = "icon-check";
                                                } else {
                                                    ico.className = "icon-check-empty";
                                                }
                                            } else {
                                                ico.className = "icon-check-empty";
                                            }
                                            //alert(ico.className);
                                            var span = document.createElement('span');
                                            span.innerHTML = "&nbsp;" + prop[i].Text;
                                            dd.appendChild(ico);
                                            dd.appendChild(span);
                                            dl.appendChild(dd);
                                        }
                                    }
                                } else {

                                    var dd = document.createElement('dd');

                                    dd.innerText = prop;
                                    console.log(prop);
                                    dl.appendChild(dd);


                                }
                            }

                        }

                    }

                }
            }
            boxx.appendChild(dl);
        },
        disable: function (id) {
            var box = document.getElementById(id);
            var button_box = box.querySelector(".pull-right");

            NodeList.prototype.forEach = Array.prototype.forEach
            var children = button_box.childNodes;
            children.forEach(function (item) {
              //  console.log(item.classList);
                item.disabled = true;
              //  console.log(item.disabled);
            });
        },
        enable: function (id) {
            var box = document.getElementById(id);
            var button_box = box.querySelector(".pull-right");

            NodeList.prototype.forEach = Array.prototype.forEach
            var children = button_box.childNodes;
            children.forEach(function (item) {
                
                item.disabled = false;
            });
        }
    }
})();