
var QuestionModel = function () {
    this.strategy = "";
    this.Display = function () {
        Container.add("Question", 'input', "Value_QuestionText");
    };

};

QuestionModel.prototype = {
    setStrategy: function (type) {
        if (type == "test") {
            this.strategy = new Test();
        } else if (type == "survey") {
            this.strategy = new Survey;
        } else if (type == "poll") {
            this.strategy = new Poll();
        } else {
            this.strategy = new NULLObject();
        }
    },

    DisplayCreateModel: function () {
        return this.strategy.DisplayCreateModel();
    },
    Validate: function () {
        return this.strategy.Validate();
    },
    SubmitForm: function () {
        return this.strategy.SubmitForm();
    },
    Reset: function () {
        return this.strategy.Reset();
    },
    DisplayResults: function (lista, text) {
        return this.strategy.DisplayResults(lista, text);
    },
    DisplayViewModel: function (name, id) {
        return this.strategy.DisplayViewModel(name , id);
    },
    DisplayStats: function (lista, name) {
        return this.strategy.DisplayStats(lista, name);
    },
    GetResults: function (model) {
        return this.strategy.GetResults(model);
    },
    GetStats: function (id) {
        return this.strategy.GetStats(id);
    },
    Edit: function (id, team) {
        if (team) {
            var item = {
                Id: id,
                Value: true
            }
            $.ajax({
                url: '/Question/ReadOnly',
                type: 'post',
                dataType: 'json',
                data: item,
                success: function (response) {

                }
            });
        }
        var type = $("#Value_quiz_type").val();
        if (Container.isEmpty()) {

            run(type);
        } else {

            Container.show();
        }
            EditQuestion.disable(id);
            return this.strategy.Edit(id);
    },
    SubmitEditForm: function () {
        return this.strategy.SubmitEditForm();
    }
};

var Poll = function () {
    this.DisplayCreateModel = function () {
        Container.addOnlyMultipleAnswers();
        Container.disableInput(true);
        Container.addCheckBoxes(false);
        Container.addButton("Submit and finish", "button", "Submit_btn");
        Container.addEditButton("Save", "Edit_btn");
        Container.show();
    }
    this.SubmitForm = function () {
        var list = document.getElementById("list");

        var posible_answ = [];

            NodeList.prototype.forEach = Array.prototype.forEach
            var children = list.childNodes;
            children.forEach(function (item) {
                posible_answ.push(item.childNodes[0].innerText);
            });
       

        var text = $("#Value_QuestionText").val();
        var quizID = parseInt($("#Value_quiz_id").val());
       

        var question_model = {
            Quiz_Id: quizID,
            Question_text: text,
            PossibleAnswers: posible_answ,
            Question_number: 1
        };
        validated = this.Validate(question_model);

        //console.log(question_model);
        //console.log(validated);
        //validated = false;
        if (validated) {
            return question_model;
        } else {
            return null;
        }
    }
    this.Validate = function (model) {
        var result = true;
        if (model.Question_text == "") {
            Container.addError("Question text is required!");
            result = false;
        }
        if (model.PossibleAnswers == null) {
            Container.addError("Possible answers must be more then two.");
            result = false;
        }else if (model.PossibleAnswers.length < 2) {
            Container.addError("Possible answers must be more then two.");
            result = false;
        }
        if (!result)
            setTimeout(Container.removeErrors, 2000);
        return result;
    }
    this.Reset = function () {
        var box = document.getElementById("forma");
        while (box.firstChild) {
            box.removeChild(box.firstChild);
        }
        //var list = document.getElementById("list");
        //while (list.firstChild) {
        //    list.removeChild(list.firstChild);
        //}
        //$("#in").val('');
        //$("#Value_QuestionText").val('');
    }
    this.DisplayResults = function (lista, text) {
        var p_div = document.getElementById('points_div');
        p_div.remove();
        var header = document.getElementById('results_header');
        header.innerText = text;
        var sum = 0;
        for (var i = 0; i < lista.length; i++) {
            sum += lista[i].Votes;
        }
        var box = document.getElementById("results");
        for (var i = 0; i < lista.length; i++) {
            var li = document.createElement('li');
            var clearfix = document.createElement('div');
            clearfix.className = "clearfix";
            var option = document.createElement('span');
            option.innerHTML = lista[i].PossibleAnswer;
            clearfix.appendChild(option);
            var votes = document.createElement('span');
            votes.className = "pull-right";
            votes.innerHTML = lista[i].Votes;
            clearfix.appendChild(votes);
            var div = document.createElement('div');
            div.classList.add("progress", "progress-mini");
            var progress = document.createElement('div');
            progress.classList.add("progress-bar");
            if (lista[i].MyAnswer) {
                progress.classList.add("progress-bar-danger");
            }
            var x = (lista[i].Votes / sum) * 100 >> 0;
            progress.style.width = x+"%";
            div.appendChild(progress);
           
            li.appendChild(clearfix);
            li.appendChild(div);
            box.appendChild(li);
        }
    }
    this.DisplayViewModel = function (name, id) {
        QuizContainer.showPoll(name, id);
    }
    this.DisplayStats = function (lista, name) {
        StatsContainer.showPoll(lista, name);
    }
    this.GetResults = function (model) {
        var that = this;
        $.ajax({
            url: '/Quiz/PollResult',
            type: 'post',
            dataType: 'json',
            data: model,
            success: function (response) {
                if (response.success) {
                    that.DisplayResults(response.results, response.text);
                } else {
                    Failed();
                }
                //
            }
        });
    }
    this.GetStats = function (id) {
        var that = this;
        $.ajax({
            url: '/Quiz/PollStats/' + id,
            type: 'get',
            dataType: 'json',
            success: function (response) {
                if (response.results.Standings == null)
                    that = new NULLObject();
                //console.log(response.results);
                that.DisplayStats(response.results.Standings, response.results.Name);
            },
            error: function (req, textStatus, errorThrown) {
                //this is going to happen when you send something different from a 200 OK HTTP
                // alert('Ooops, something happened: ' + textStatus + ' ' + errorThrown);
            }
        });
    }
    this.Edit = function (id) {
        $.ajax({
            url: '/Question/Edit/' + id,
            type: 'get',
            dataType: 'json',
            success: function (response) {
                        var list = document.getElementById("list");
                while (list.firstChild) {
                    list.removeChild(list.firstChild);
                }
                EditQuestion.editPoll(response.model);
            },
            error: function (req, textStatus, errorThrown) {
                //this is going to happen when you send something different from a 200 OK HTTP
                // alert('Ooops, something happened: ' + textStatus + ' ' + errorThrown);
            }
        });
    }
    this.SubmitEditForm = function () {
        var list = document.getElementById("list");

        var posible_answ = [];

        NodeList.prototype.forEach = Array.prototype.forEach
        var children = list.childNodes;
        children.forEach(function (item) {
            posible_answ.push(item.childNodes[0].innerText);
        });


        var text = $("#Value_QuestionText").val();
        var ID = parseInt($("#Value_question_id").val());


        var question_model = {
            Id: ID,
            Question_text: text,
            PossibleAnswers: posible_answ,
            Question_number: 1
        };
        validated = this.Validate(question_model);

        //console.log(question_model);
        //console.log(validated);
        //validated = false;
        if (validated) {
            return question_model;
        } else {
            return null;
        }
    }
};

var Survey = function () {
    this.DisplayCreateModel = function () {
        Container.addMulti();
        Container.addButton("Submit", "button", "Submit_btn");
        Container.addEditButton("Save", "Edit_btn");
        Container.show();
        Container.disableInput(true);
        Container.addCheckBoxes(false);

    }
    this.SubmitForm = function () {
        //var validated = this.Validate();
        var list = document.getElementById("list");

        var posible_answ = [];

        if ($("#multiple_answers").is(':checked')) {
            NodeList.prototype.forEach = Array.prototype.forEach
            var children = list.childNodes;
            children.forEach(function (item) {
                //console.log(item.childNodes[0].innerText);
                posible_answ.push(item.childNodes[0].innerText);
            });
        } else if ($("#input_field").is(':checked')) {
            posible_answ = null;
        } else {
            Failed("You must chose between a,b,c.. answers or answer in input field!");
        }

        var text = $("#Value_QuestionText").val();
        var quizID = parseInt($("#Value_quiz_id").val());
        var num = parseInt($("#Value_question_num").val()) + 1;

        var question_model = {
            Quiz_Id: quizID,
            Question_text: text,
            PossibleAnswers: posible_answ
        };
        
        validated = this.Validate(question_model);
        if (validated) {
            return question_model;
        } else {
            return null;
        }
                    //$.ajax({
            //    url: '/Question/Create',
            //    type: 'post',
            //    dataType: 'json',
            //    data: question_model,
            //    success: function (response) {
            //        window.location.href = response.responseUrl;
            //    }
            //});
    }
    this.Validate = function (model) {
        var result = true;
        if (model.Question_text == "") {
            Container.addError("Question text is required!");
            result = false;
        }
        if (model.PossibleAnswers !== null) {
            if (model.PossibleAnswers.length < 2) {
                Container.addError("Possible answers must be more then two.");
                result = false;
            }
        }
        if(!result)
            setTimeout(Container.removeErrors, 2000);
        return result;
    }
    this.Reset = function () {
        var list = document.getElementById("list");
        if ($("#multiple_answers").is(':checked')) {
            while (list.firstChild) {
                list.removeChild(list.firstChild);
            }
            $("#in").val('');
        }
        $("#Value_QuestionText").val('');


    }
    this.DisplayResults = function (lista, text) {
        var p_div = document.getElementById('points_div');
        p_div.remove();
        var box = document.getElementById("results");
        var h = document.createElement('h3');
        h.classList.add("green");
        h.innerText = lista;
        box.appendChild(h);
    }
    this.DisplayViewModel = function (name, id) {
        QuizContainer.showSurvey(name, id);
    }
    this.DisplayStats = function (lista, name) {
            StatsContainer.showSurvey(lista, name);
    }
    this.GetResults = function (model) {
        var that = this;
        $.ajax({
            url: '/Quiz/SurveyResult',
            type: 'post',
            dataType: 'json',
            data: model,
            success: function (response) {
                if (response.success) {
                    that.DisplayResults(response.results, response.text);
                } else {
                    Failed();
                }
                //
            }
        });
    }
    this.GetStats = function (id) {
        var that = this;
        $.ajax({
            url: '/Quiz/SurveyStats/' + id,
            type: 'get',
            dataType: 'json',
            success: function (response) {
                if (response.results.Standings == null)
                    that = new NULLObject();
                that.DisplayStats(response.results.Standings, response.results.Name);
            },
            error: function (req, textStatus, errorThrown) {
                //this is going to happen when you send something different from a 200 OK HTTP
                // alert('Ooops, something happened: ' + textStatus + ' ' + errorThrown);
            }
        });
    }
    this.Edit = function (id) {
        $.ajax({
            url: '/Question/Edit/' + id,
            type: 'get',
            dataType: 'json',
            success: function (response) {
                EditQuestion.editSurvey(response.model);
            },
            error: function (req, textStatus, errorThrown) {
                //this is going to happen when you send something different from a 200 OK HTTP
                // alert('Ooops, something happened: ' + textStatus + ' ' + errorThrown);
            }
        });
    }
    this.SubmitEditForm = function () {
        //var validated = this.Validate();
        var list = document.getElementById("list");

        var posible_answ = [];

        if ($("#multiple_answers").is(':checked')) {
            NodeList.prototype.forEach = Array.prototype.forEach
            var children = list.childNodes;
            children.forEach(function (item) {
                //console.log(item.childNodes[0].innerText);
                posible_answ.push(item.childNodes[0].innerText);
            });
        } else if ($("#input_field").is(':checked')) {
            posible_answ = null;
        } else {
            Failed("You must chose between a,b,c.. answers or answer in input field!");
        }

        var text = $("#Value_QuestionText").val();
        var ID = parseInt($("#Value_question_id").val());
        var num = parseInt($("#Value_question_num").val()) + 1;

        var question_model = {
            Id: ID,
            Question_text: text,
            PossibleAnswers: posible_answ
        };

        validated = this.Validate(question_model);
        //console.log(question_model);
        //validated = false;
        if (validated) {
            return question_model;
        } else {
            return null;
        }
    }
};

var Test = function () {
    this.DisplayCreateModel = function () {
        Container.addMulti();
        Container.disableInput(false);
        Container.setTest(true);
        Container.show();
        Container.addPointField();
        Container.addButton("Submit", "button", "Submit_btn");
        Container.addEditButton("Save", "Edit_btn");
    }
    this.SubmitForm = function () {
        var validated = true;
        var list = document.getElementById("list");
        var validInput = true;

        var posible_answ = [];
        var correct_answ = [];

        if ($("#multiple_answers").is(':checked')) {
            NodeList.prototype.forEach = Array.prototype.forEach
            var children = list.childNodes;
            children.forEach(function (item) {
                posible_answ.push(item.childNodes[1].innerText);
                if (item.childNodes[0].classList.contains("glyphicon-check"))
                    correct_answ.push(item.childNodes[1].innerText);
            });
           
        } else if ($("#input_field").is(':checked')) {
            posible_answ = null;
            var answer = $("#Value_CorrectAnswer");
            if (answer != null) {
                correct_answ.push($("#Value_CorrectAnswer").val());
            }
        } else {
            Failed("You must chose between a,b,c.. answers or answer in input field!");
        }


        var text = $("#Value_QuestionText").val();
        var points = $("#Value_points").val();
        var quizID = parseInt($("#Value_quiz_id").val());
        var num = parseInt($("#Value_question_num").val()) + 1;
   
        var question_model = {
            Quiz_Id: quizID,
            Question_text: text,
            PossibleAnswers: posible_answ,
            CorrectAnswers: correct_answ,
            Points: points
        };
        validated = this.Validate(question_model);
        //console.log(validated);
        //console.log(question_model);
        if (validated) {
            return question_model;
        } else {
            return null;
        }
    }
    this.Validate = function (model) {
        var result = true;
        if (model.Question_text == "") {
            Container.addError("Question text is required!");
            result = false;
        }
        if (model.Points == 0) {
            Container.addError("Points field is required!");
            result = false;
        }
        if (model.CorrectAnswers.length == 1 && (model.CorrectAnswers[0] == "" || model.CorrectAnswers[0] == " ")) {
            Container.addError("Correct answers is required!");
            result = false;
        }
        if (model.CorrectAnswers.length == 0) {
            Container.addError("Correct answers is required!");
            result = false;
        }
        if (model.PossibleAnswers !== null) {
            if (model.PossibleAnswers.length < 2) {
                Container.addError("Possible answers must be more then two. Add possible answers and check the correct ones!");
                result = false;
            }
        }
        if(!result)
            setTimeout(Container.removeErrors, 2000);
        return result;
    }
    this.Reset = function () {
        var list = document.getElementById("list");
        if ($("#multiple_answers").is(':checked')) {
            while (list.firstChild) {
                list.removeChild(list.firstChild);
            }
            $("#in").val('');
        } else if ($("#input_field").is(':checked')) {
            $("#Value_CorrectAnswer").val('');
        }
        
        $("#Value_QuestionText").val('');
        $("#Value_points").val('0');

    }
    this.DisplayResults = function (lista, text) {
        var header = document.getElementById('results_header');
        header.innerText = text;
        var total = 0;
        var sum = 0;
        var box = document.getElementById("results");
        for (var i = 0; i < lista.length; i++) {
            var element = document.createElement('li');
            element.classList.add("list-group-item","col-sm-12");
            var ico = document.createElement('i');
            if (lista[i].Correct) {
                total += lista[i].Points;
                ico.classList.add("icon-ok", "green","col-sm-1");
            } else {
                ico.classList.add("icon-remove", "red","col-sm-1");
            }
            element.appendChild(ico);
            var text = document.createElement('i');
            text.classList.add("col-sm-9");
            text.innerHTML = lista[i].Text;
            element.appendChild(text);
            var label = document.createElement('label');
            label.classList.add("label", "label-lg", "arrowed-in-right", "arrowed", "pull-right");
            if (lista[i].Correct) {
                label.classList.add("label-success");
            } else {
                label.classList.add("label-danger");
            }
            
            var point = document.createElement('strong');
            point.innerHTML = lista[i].Points;
            label.appendChild(point);
            element.appendChild(label);
            box.appendChild(element);
            sum += lista[i].Points;
        }
        $("#points").text(total + "/" + sum);
    }
    this.DisplayViewModel = function (name, id) {
        QuizContainer.showTest(name, id);
    }
    this.DisplayStats = function (lista, name) {

        StatsContainer.showTest(lista, name);

    }
    this.GetResults = function (model) {
        var that = this;
        $.ajax({
            url: '/Quiz/TestResult',
            type: 'post',
            dataType: 'json',
            data: model,
            success: function (response) {
                if (response.success) {
                    that.DisplayResults(response.results, response.text);
                } else {
                    Failed();
                }
                //
            }
        });
    }
    this.GetStats = function (id) {
        var that = this;
        $.ajax({
            url: '/Quiz/TestStats/' + id,
            type: 'get',
            dataType: 'json',
            success: function (response) {
                if (response.results.Standings == null)
                    that = new NULLObject();
                that.DisplayStats(response.results.Standings, response.results.Name);
            },
            error: function (req, textStatus, errorThrown) {
                //this is going to happen when you send something different from a 200 OK HTTP
                // alert('Ooops, something happened: ' + textStatus + ' ' + errorThrown);
            }
        });
    }
    this.Edit = function (id) {
        $.ajax({
            url: '/Question/Edit/' + id,
            type: 'get',
            dataType: 'json',
            success: function (response) {
                EditQuestion.editTest(response.model);
            },
            error: function (req, textStatus, errorThrown) {
                //this is going to happen when you send something different from a 200 OK HTTP
                // alert('Ooops, something happened: ' + textStatus + ' ' + errorThrown);
            }
        });
    }
    this.SubmitEditForm = function () {
        var validated = true;
        var list = document.getElementById("list");
        var validInput = true;

        var posible_answ = [];
        var correct_answ = [];

        if ($("#multiple_answers").is(':checked')) {
            NodeList.prototype.forEach = Array.prototype.forEach
            var children = list.childNodes;
            children.forEach(function (item) {
                posible_answ.push(item.childNodes[1].innerText);
                if (item.childNodes[0].classList.contains("glyphicon-check"))
                    correct_answ.push(item.childNodes[1].innerText);
            });

        } else if ($("#input_field").is(':checked')) {
            posible_answ = null;
            var answer = $("#Value_CorrectAnswer");
            if (answer != null) {
                correct_answ.push($("#Value_CorrectAnswer").val());
            }
        } else {
            Failed("You must chose between a,b,c.. answers or answer in input field!");
        }


        var text = $("#Value_QuestionText").val();
        var points = $("#Value_points").val();
        var ID = parseInt($("#Value_question_id").val());
        var num = parseInt($("#Value_question_num").val()) + 1;

        var question_model = {
            Id: ID,
            Question_text: text,
            PossibleAnswers: posible_answ,
            CorrectAnswers: correct_answ,
            Points: points
        };
        validated = this.Validate(question_model);
        //console.log(validated);
        //console.log(question_model);
        //validated = false;
        if (validated) {
            return question_model;
        } else {
            return null;
        }
    }
};

var NULLObject = function () {
    this.DisplayCreateModel = function () {
        //Do nothing or send error message
    }
    this.SubmitForm = function () {
        return null;
    }
    this.Validate = function (model) {
        return false;
    }
    this.Reset = function () {
        //
    }
    this.DisplayResults = function (lista, text) {
        var p_div = document.getElementById('points_div');
        p_div.remove();
        var header = document.getElementById('results_header');
        header.innerText = "Results";
        var box = document.getElementById("results");
        box.innerHTML = "No results found!";

    }

    this.DisplayViewModel = function (name, id) {

    }
    this.DisplayStats = function (lista) {
        var box = document.getElementById('stats');
        var d = document.createElement('div');
        d.classList.add("center");
        var h = document.createElement('h1');
        h.innerHTML = "No statistics found.";
        d.appendChild(h);
        box.appendChild(d);
    }
    this.GetResults = function (model) {
        this.DisplayResults([], "");
    }
    this.GetStats = function (id) {
                this.DisplayStats([], "");
    }
    this.Edit = function (id) {
       
    }
    this.SubmitEditForm = function () {
        return null;
    }
};
