var QuizContainer = (function () {
    var container = document.getElementById('quiz_container');
    return {
        showTest: function (name, id) {
            var box = document.createElement('div');
            box.classList.add("col-sm-2", "center");
            box.style.height = "100px";

            var button = document.createElement('a');
            button.classList.add("btn", "btn-app", "btn-light", "btn-xs", "radius-4");
            button.href = "/Quiz/Play/" + id;
            var i = document.createElement('i');
            i.classList.add("icon-cog", "bigger-160");
            var ii = document.createElement('i');
            ii.innerText = "Test";
            button.appendChild(i);
            button.appendChild(ii);

            var lbl = document.createElement('label');
            lbl.classList.add("center", "col-sm-12");
            lbl.innerText = name;
            box.appendChild(button);
            box.appendChild(lbl);
            container.appendChild(box);
        },
        showSurvey: function (name, id) {
            var box = document.createElement('div');
            box.classList.add("col-sm-2", "center");
            box.style.height = "100px";
            var button = document.createElement('a');
            button.classList.add("btn", "btn-app", "btn-light", "btn-xs", "radius-4");
            button.href = "/Quiz/Play/" + id;
            var i = document.createElement('i');
            i.classList.add("icon-edit", "bigger-160");
            var ii = document.createElement('i');
            ii.innerText = "Survey";
            button.appendChild(i);
            button.appendChild(ii);
            var lbl = document.createElement('label');
            lbl.classList.add("center", "col-sm-12");
            lbl.innerText = name;
            box.appendChild(button);
            box.appendChild(lbl);
            container.appendChild(box);
        },
        showPoll: function (name, id) {
            var box = document.createElement('div');
            box.classList.add("col-sm-2","center");
            box.style.height = "100px";
            var button = document.createElement('a');
            button.classList.add("btn", "btn-app", "btn-light", "btn-xs", "radius-4");
            button.href = "/Quiz/Play/" + id;

            var i = document.createElement('i');
            i.classList.add("glyphicon", "glyphicon-stats", "bigger-160");
            var ii = document.createElement('i');
            ii.innerText = "Poll";
            button.appendChild(i);
            button.appendChild(ii);
            var lbl = document.createElement('label');
            lbl.classList.add("center", "col-sm-12");
            lbl.innerText = name;
            box.appendChild(button);
            box.appendChild(lbl);
            container.appendChild(box);
        },
        removeAll: function () {
            while (container.firstChild) {
                container.removeChild(container.firstChild);
            }
        }
    }
})();