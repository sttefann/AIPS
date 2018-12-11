var UserContainer = (function () {

    return {
        add: function (name) {
            var x = document.getElementById("user_list");
            if (x != null) {
                var option = document.createElement("li");
                //  option.id = "ul" + uid;
                //  uid++;
                var el = name;
                //   option.innerText = el;
                var optionVal = document.createElement('span');
                optionVal.className = "Value_option";
                optionVal.innerText = el;
                option.className = "list-group-item";
                var del = document.createElement('button');
                del.classList.add("btn", "btn-danger", "btn-xs", "no-border", "pull-right");
                del.innerHTML = 'Del'
                del.onclick = function () {


                    var node;
                    var item = this.parentNode;
                    for (var i = 0; i < item.childNodes.length; i++) {
                        if (item.childNodes[i].className == "Value_option") {
                            node = item.childNodes[i];
                            break;
                        }
                    }
                    var username = node.innerText;
                    var quizid = $("#Value_quiz_id").val();

                    var user = {
                        Value: username,
                        Id: quizid
                    };
                    $.ajax({
                        url: '/Group/RemoveUser',
                        type: 'post',
                        dataType: 'json',
                        data: user,
                        success: function (response) {
                            console.log(response.success);
                            if (response.success) {
                                window.startHub().done(function () {
                                    hub.server.removeFromGroup(quizid, username);
                                })
                                node.parentNode.remove();
                            }
                            else {
                                Failed("Something is wrong. Refresh the page");
                            }
                        }
                    });
                };
                var point = document.createElement('span');
                point.classList.add("icon-angle-right", "bigger-110");
                var space = document.createElement('label');
                space.innerHTML = "&nbsp;&nbsp;&nbsp; &nbsp;";
                var space1 = document.createElement('label');
                space1.innerHTML = "&nbsp;&nbsp;";
                option.appendChild(point);
                option.appendChild(space1);
                option.appendChild(optionVal);
                option.appendChild(space);
                option.appendChild(del);
                if (checkIfExist2("user_list", el)) {
                    alert("Element: \"" + el + "\". Already exist!");
                } else {
                    if (el == "" || el == " ") {
                        alert("Input field is empty!");
                    } else {
                        //alert(checkIfExist("list", el));
                        x.appendChild(option);
                    }

                }
            }
           
        },
        addNotification: function (quiz_id, name) {
            var notifi = document.getElementById("notification");
            if (!$("#notify-bell").hasClass('red')) {
                $("#notify-bell").addClass('icon-animated-bell');
                $("#notify-bell").addClass('red');
            }
                

            var link = document.createElement('a');
            link.href = "/Group/TeamProject/" + quiz_id;
            link.onclick = function () {
                $("#notify-bell").removeClass('red');
                $("#notify-bell").removeClass('icon-animated-bell');
                window.location.href = this.href;
            };
            var div = document.createElement('div');
            div.className = "clearfix";
            var text = document.createElement('span');
            text.className = "pull-left";
            text.innerHTML = name + ", new team project!";
            div.appendChild(text);
            var icon = document.createElement('span');
            icon.classList.add("pull-right", "icon-group");
            div.appendChild(icon);
            link.appendChild(div);
            var li = document.createElement('li');
            li.appendChild(link);
            notifi.appendChild(li);
        }
    }
})();
var QuestionContainer = (function () {
    return {
        addQuestion: function (question, isOwner) {
            //alert(question.Id);
            var list = document.getElementById("question-list");

            var box = document.createElement('div');
            box.id = question.Id;
            box.classList.add("clearfix", "form-actions");
            var dl = document.createElement('dl');
            dl.className = "dl-horizontal";
            var pull_right = document.createElement('i');
            pull_right.className = "pull-right";
            if (isOwner) {
                var a = document.createElement('button');
                a.classList.add("btn", "btn-danger", "btn-sm", "no-border");
                a.setAttribute('data-id', question.Id); 
                a.onclick = function () {
                    //console.log(this.getAttribute('data-id'));
                    DeleteFunctionn(this.getAttribute('data-id'));
                }
                if (question.ReadOnly) {
                    a.disabled = true;
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
            if (question.ReadOnly) {
                aa.disabled = true;
            }
            var aii = document.createElement('i');
            aii.classList.add("icon-edit", "icon-only", "bigger-130");
            aa.appendChild(aii);
            pull_right.appendChild(aa);

            box.appendChild(pull_right);
            /*
            {
            key1: {
                    text: " ",
                    value: ""
                   },
            key2: {
                    text: " ",
                    value: {
                            val1,
                            val2,
                            val3
                            }
                    }
            }
*/
            for (var someKey in question) {
                if (question.hasOwnProperty(someKey)) {
                    //console.log(someKey);
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
            box.appendChild(dl);
            list.appendChild(box);
            list.appendChild(document.createElement('hr'));
        },
        removeQuestion: function (id) {
            //console.log(id);
            var element = document.getElementById(id);
            element.remove();
        }
    }
})();

