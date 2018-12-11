var StatsContainer = (function () {
    var container = document.getElementById("stats");
    return {
        showTest: function (lista, name) {
            var main = document.getElementById("wmain");
            if (lista.length === 0) main.innerHTML = "Standings are empty";
            var header = document.getElementById("results_header");
            header.innerText = name;
            
            var test_ul = document.getElementById("results");

            for (var k = 0; k < lista.length; k++) {
                var num = k + 1;
                var points = lista[k].Points;
                var username = lista[k].Username;

                var box = document.createElement('li');
                box.classList.add("list-group-item", "col-sm-12");


                var i = document.createElement('i');
                i.classList.add("pull-left");
                i.innerText = num;
                var ii = document.createElement('i');
                ii.classList.add("col-sm-9");
                ii.innerText = username;

                box.appendChild(i);
                box.appendChild(ii);

                var lbl = document.createElement('label');
                lbl.classList.add("label", "label-lg", "arrowed-in-right", "arrowed", "pull-right", "label-success");
                var strong = document.createElement('strong');
                strong.innerHTML = points;
                lbl.appendChild(strong);
                box.appendChild(lbl);
                test_ul.appendChild(box);
            }
            var cont = document.getElementById("stats_container");
            cont.classList.remove("hide");
        },
        showSurvey: function (lista, name) {
            console.log(lista);
            var wizard = document.getElementById("wizard-div");
            wizard.classList.remove("hide");
            var step_container = document.getElementById("step-container");
            for (var k = 0; k < lista.length; k++) {
                var num = k +1;
                var id = "step" + num;
                var answers = lista[k].Answers;
                var text = lista[k].Question_text;

                var wsteps = document.getElementById("wizard_steps");
                var step = document.createElement('li');
                step.dataset.target = "#step" + num;
                var span = document.createElement('span');
                span.className = "step";
                span.innerText = num;
                step.appendChild(span);
                wsteps.appendChild(step);

                var box = document.createElement('div');
                box.classList.add("step-pane", "question_step");
                box.id = id;
                var div = document.createElement('div');
                div.className = "center";
                var qtext = document.createElement('h4');
                qtext.classList.add("blue", "lighter");
                qtext.innerText = text;
                var hr = document.createElement('hr');
                hr.className = "hr-double";
                div.appendChild(qtext);
                div.appendChild(hr);

                var ul = document.createElement('ul');
                for (var j = 0; j < answers.length; j++) {
                    var li = document.createElement('li');
                    li.classList.add("list-group-item", "col-sm-6", "col-sm-offset-3");
                    var i = document.createElement('i');
                    i.className = "col-sm-9";
                    i.innerText = answers[j].Text;
                    li.appendChild(i);

                    var lab = document.createElement('label');
                    lab.classList.add("label", "label-lg", "arrowed-in-right", "arrowed", "pull-right", "label-inverse");
                    var s = document.createElement('strong');
                    s.innerText = answers[j].Number;
                    lab.appendChild(s);
                    li.appendChild(lab);
                    ul.appendChild(li);
                }


                box.appendChild(div);
                box.appendChild(ul);
                step_container.appendChild(box);
            }
            var wizard = document.getElementById("wizard-div");
            wizard.classList.remove("hide");

            $('[data-rel=tooltip]').tooltip();
            $(".select2").css('width', '200px').select2({ allowClear: true })
                .on('change', function () {
                    $(this).closest('form').validate().element($(this));
                });


            var $validation = true;
            $('#fuelux-wizard').ace_wizard().on('change', function (e, info) {
            }).on('finished', function (e) {
                alert("End of survey.");
            }).on('stepclick', function (e) {
                //return false;//prevent clicking on steps
            });

        },
        showPoll: function (lista, text) {
            //console.log(lista);
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
                progress.style.width = x + "%";
                div.appendChild(progress);

                li.appendChild(clearfix);
                li.appendChild(div);
                box.appendChild(li);
            }
            var cont = document.getElementById("stats_container");
            cont.classList.remove("hide");
        },
        removeAll: function () {
            while (container.firstChild) {
                container.removeChild(container.firstChild);
            }
        }
    }
})();