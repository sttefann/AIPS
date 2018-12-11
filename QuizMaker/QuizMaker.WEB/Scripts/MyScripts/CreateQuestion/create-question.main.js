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
                        Success(" Question successfuly added!");
                        var question_model = JSON.stringify(response.model);
                        var model = JSON.parse(question_model);
                        Container.addQuestion(model);
                    } else {
                        Failed();
                        window.location.href = response.responseUrl;
                    }
                    //
                }
            });
        }
    });
    $('#Edit_btn').click(function () {
        var question_model = question.SubmitEditForm();
        // console.log(model);
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
                        EditQuestion.displayChangedQuestion(model, true);
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
            }
        });
        //signalr send to all to change readonly
        
    });

    refresh();
}

jQuery(function ($) {
    $("#btnEditQuiz").click(function () {
        $("#btnEditQuiz").prop('disabled', true);
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
                    $("#btnEditQuiz").prop('disabled', false);
                }
            });
        }
    }); 
});

