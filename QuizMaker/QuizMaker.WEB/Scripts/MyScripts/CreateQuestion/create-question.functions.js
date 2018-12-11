var Container = (function () {
    var uid = 0;
    var box = document.getElementById("forma");
    var container = document.createElement('div');
    var chose_box = document.getElementById("chose");
    var isSurvey = false;
    var isTest = false;
    var select = document.createElement('select');
    var button_box = document.createElement('div');
    var is_button = false;

    return {
        addQuestion: function (question) {
            //alert(question.Id);
            var list = document.getElementById("question-list");

            var boxx = document.createElement('div');
            boxx.id = question.Id;
            boxx.classList.add("clearfix", "form-actions");
            var dl = document.createElement('dl');
            dl.className = "dl-horizontal";

            var pull_right = document.createElement('i');
            pull_right.className = "pull-right";
            var a = document.createElement('button');
            a.classList.add("btn", "btn-danger", "btn-sm", "no-border");
            a.setAttribute('data-id', question.Id);
            a.onclick = function () {
                //console.log(this.getAttribute('data-id'));
                DeleteFunctionn(this.getAttribute('data-id'));
            }

            var aa = document.createElement('button');
            aa.classList.add("btn", "btn-primary", "btn-sm", "no-border");
            aa.setAttribute('data-id', question.Id);
            aa.onclick = function () {
                //console.log(this.getAttribute('data-id'));
                EditFunctionn(this.getAttribute('data-id'));
            }


            var ii = document.createElement('i');
            ii.classList.add("icon-trash", "icon-only", "bigger-130");
            a.appendChild(ii);
            pull_right.appendChild(a);
            var aii = document.createElement('i');
            aii.classList.add("icon-edit", "icon-only", "bigger-130");
            aa.appendChild(aii);
            pull_right.appendChild(aa);
            boxx.appendChild(pull_right);
            
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
            boxx.appendChild(dl);
            list.appendChild(boxx);
            list.appendChild(document.createElement('hr'));
        },
        addButton: function (name, type, id) {

                is_button = true;
                var clearfix = document.createElement('div');
                clearfix.classList.add("clearfix", "form-actions");
                
               // var element = document.createElement('div');
                //button_box.classList.add("col-sm-offset-2", "col-xs-6");
               // button_box.id = "button_box";
                var labela = document.createElement('label');
                labela.classList.add("col-sm-3", "col-xs-6", "control-label", "no-padding-right");
                labela.innerText = " ";
                clearfix.appendChild(labela);
                var col = document.createElement('div');
                col.classList.add("col-sm-6", "col-xs-12");
                col.id = "button_box";

                var submit = document.createElement(type);
                submit.classList.add("btn", "btn-inverse", "no-border");
                submit.innerHTML = '<i class="icon-ok bigger-110"></i> ' + name;
                submit.id = id;
                //var i = document.createElement('label');
                //i.innerHTML = "&nbsp; &nbsp; &nbsp;";
                
                col.appendChild(submit);
                button_box.appendChild(col);
                clearfix.appendChild(button_box);
                container.appendChild(clearfix);    
        },
        addEditButton: function (name, id) {

            //var buton_box = document.getElementById("button_box");
            var save = document.createElement('button');
            save.classList.add("btn", "btn-grey", "no-border", "hide");
            save.innerHTML = '<i class="icon-save bigger-110"></i> ' + "Save";
            save.id = "Edit_btn";
            var discard = document.createElement('button');
            discard.classList.add("btn", "btn-default", "no-border", "hide");
            discard.innerHTML = '<i class="icon-remove bigger-110"></i>';
            discard.id = "Discard_btn";
            button_box.appendChild(save);
            button_box.appendChild(discard);
        },
        add: function (name, type, id) {
            var element = document.createElement('div');
            element.className = "form-group";
            var labela = document.createElement('label');
            labela.classList.add("col-sm-3", "control-label", "no-padding-right");
            labela.innerText = name;
            element.appendChild(labela);
            var col = document.createElement('div');
            col.classList.add("col-sm-6", "col-xs-12");
          
            if (type == 'input') {
                
                var input = document.createElement('textarea');
                input.classList.add("form-control", "autosize-transition");
               
                input.id = id;
                col.appendChild(input);
                
            } else {
                var input = document.createElement(type);
                input.classList.add("form-control");
                input.id = id;
                col.appendChild(input);
            }


            element.appendChild(col);
            var space = document.createElement('div');
            space.className = 'space-4';
            container.appendChild(space);
            container.appendChild(element);
        },
        addMulti: function () {

            var x = document.createElement("INPUT");
            x.setAttribute("type", "radio");
            x.setAttribute("name", "group");
            x.setAttribute("id", "input_field");
            x.classList.add("col-sm-1", "no-padding-right");
            x.onchange = function () {
                if (x.checked == true) {
                    Container.blank_field();
                }
            };
            var first_el = document.createElement('div');
            first_el.className = "form-group";
            var lab = document.createElement('label');
            lab.classList.add("col-sm-3", "control-label", "no-padding-right");
            lab.innerText = "Chose:";
            first_el.appendChild(lab);
            var f_col = document.createElement('div');
            f_col.classList.add("col-sm-6", "col-xs-12");
            f_col.appendChild(x);
            var f_labela = document.createElement('label');
            f_labela.classList.add("col-sm-3", "no-padding-left");
            f_labela.innerText = "Blank field";
            f_col.appendChild(f_labela);
            var xx = document.createElement("INPUT");
            xx.setAttribute("type", "radio");
           
            xx.setAttribute("name", "group");
            xx.setAttribute("id", "multiple_answers");
            xx.classList.add("col-sm-1", "no-padding-right");
            xx.onchange = function () {
                if (xx.checked == true) {
                    Container.multiple_answers();

                }
            };
            f_col.appendChild(xx);
            var s_labela = document.createElement('label');
            s_labela.classList.add("col-sm-3", "no-padding-left");
            s_labela.innerText = "Multiple Answers";
            f_col.appendChild(s_labela);
            first_el.appendChild(f_col);

            container.appendChild(first_el);

            var chose = document.createElement('div');
            chose.id = "chose";
            container.appendChild(chose);
            chose_box = chose;

        },
        addCheckBoxes: function (isCheckBox) {
            if (isCheckBox) {
                
                if (!$('#list').hasClass('checked-list-box')) {
                    $('#list').addClass("checked-list-box");
                }
                    
            } else {
                if ($('#list').hasClass('checked-list-box'))
                    $('#list').removeClass("checked-list-box");
                }
        },
        disableInput: function (check) {
            isSurvey = check;
        },
        addOnlyMultipleAnswers: function () {
            var chose = document.createElement('div');
            chose.id = "chose";
            container.appendChild(chose);
            chose_box = chose;
            this.multiple_answers();
        },
        multiple_answers: function () {
            if (chose_box != null) {
                while (chose_box.firstChild) {
                    chose_box.removeChild(chose_box.firstChild);
                }
                

                var element = document.createElement('div');
                element.className = "form-group";
                var labela = document.createElement('label');
                labela.classList.add("col-sm-3", "control-label", "no-padding-right");
                labela.innerText = name;
                element.appendChild(labela);
                var col = document.createElement('div');
                col.classList.add("col-sm-6", "col-xs-12");
                var input = document.createElement('input');
                input.classList.add("form-control");
                input.setAttribute("maxlength", "150");
                input.id = "in";
                col.appendChild(input);
                element.appendChild(col);
                chose_box.appendChild(element);

                var p_col = document.createElement('div');
                p_col.classList.add("col-sm-6", "col-xs-12");
                var p_element = document.createElement('div');
                p_element.className = "form-group";
                var buton = document.createElement('button');
                buton.classList.add("btn", "btn-inverse", "col-sm-3","col-xs-6", "no-border");
                buton.innerText = "Add";
                buton.id = "add";
                buton.onclick = function () {
                    var x = document.getElementById("list");
                    var option = document.createElement("li");
                  //  option.id = "ul" + uid;
                  //  uid++;
                    var el = $("#in").val();
                    //   option.innerText = el;
                    var optionVal = document.createElement('span');
                    optionVal.className = "Value_option";
                    optionVal.innerText = el;
                    option.className = "list-group-item";
                    var del = document.createElement('button');
                    del.classList.add("btn", "btn-danger", "btn-xs", "no-border");
                    del.innerHTML = 'Del'
                    del.onclick = function () {
                        this.parentElement.remove();
                    };

                    var space = document.createElement('label');
                    space.innerHTML = "&nbsp;&nbsp;&nbsp; &nbsp;   ";
                    option.appendChild(optionVal);
                    option.appendChild(space);
                    option.appendChild(del);
                    if (checkIfExist2("list", el)) {
                        alert("Element: \"" + el + "\". Already exist!");
                    } else {
                        if (el == "" || el == " ") {
                            alert("Input field is empty!");
                        } else {
                            //alert(checkIfExist("list", el));
                            x.appendChild(option);
                        }

                    }
                    refresh();
                };
                var p_labela = document.createElement('labela');
                p_labela.classList.add("col-xs-5", "control-label", "no-padding-right");
                p_labela.innerText = " ";
                p_element.appendChild(p_labela);
                p_col.appendChild(buton);
                p_element.appendChild(p_col);
                chose_box.appendChild(p_element);

                var pp_col = document.createElement('div');
                pp_col.classList.add("col-sm-6", "col-xs-12");
                var pp_element = document.createElement('div');
                pp_element.className = "form-group";
                var well = document.createElement('div');
                well.className = "well";
                var ul = document.createElement('ul');
                ul.classList.add("list-group");
                ul.id = "list";
                well.appendChild(ul);
                pp_col.appendChild(well);

                var pp_labela = document.createElement('labela');
                pp_labela.classList.add("col-sm-3", "control-label", "no-padding-right");
                pp_labela.id = "c_answer";
                pp_labela.innerText = ' ';
                pp_element.appendChild(pp_labela);
                pp_element.appendChild(pp_col);
                chose_box.appendChild(pp_element);
                if (isTest) {
                    this.addCheckBoxes(true);
                }
            }
        },
        blank_field: function () {
            while (chose_box.firstChild) {
                chose_box.removeChild(chose_box.firstChild);
            }

            var element = document.createElement('div');
            element.className = "form-group";
            var labela = document.createElement('label');
            labela.classList.add("col-sm-3", "control-label", "no-padding-right");
            labela.innerText = "Answers";
            element.appendChild(labela);
            var col = document.createElement('div');
            col.classList.add("col-sm-6", "col-xs-12");
            var input = document.createElement('input');
            input.classList.add("form-control");
            input.id = "Value_CorrectAnswer";
            input.disabled = isSurvey;
            col.appendChild(input);

            element.appendChild(col);
            var space = document.createElement('div');
            space.className = 'space-4';
            chose_box.appendChild(space);
            chose_box.appendChild(element);
        },
        addPointField: function () {
            var element = document.createElement('div');
            element.className = "form-group";
            var labela = document.createElement('label');
            labela.classList.add("col-sm-3", "control-label", "no-padding-right");
            labela.innerText = "Points:";
            element.appendChild(labela);
            var col = document.createElement('div');
            col.classList.add("col-sm-6", "col-xs-12");
            var input = document.createElement('input');
            input.type = "text";
            input.classList.add("input-mini");
            input.id = "Value_points";
                col.appendChild(input);

            element.appendChild(col);
            var space = document.createElement('div');
            space.className = 'space-4';
            container.appendChild(space);
            container.appendChild(element);

            $('#Value_points').ace_spinner({ value: 0, min: 0, max: 100, step: 10, on_sides: true, icon_up: 'icon-plus smaller-75', icon_down: 'icon-minus smaller-75', btn_up_class: 'btn-inverse', btn_down_class: 'btn-inverse' });
        },
        addError: function (msg) {
            var error_box = document.getElementById("error_panel");
            var element = document.createElement('div');
            element.classList.add("alert", "alert-danger");
            var buton = document.createElement('button');
            buton.type = "button";
            buton.className = "close";
            buton.dataset.dismiss = "alert";
            var i = document.createElement('i');
            i.className = "icon-remove";
            buton.appendChild(i);
            element.appendChild(buton);
            var strong = document.createElement('strong');
            var ii = document.createElement('i');
            ii.className = "icon-remove";
            strong.appendChild(ii);
            strong.innerText = "Failed!";
            element.appendChild(strong);
            var span = document.createElement('span');
            span.innerText = msg;
            element.appendChild(span);
            error_box.appendChild(element);
        },
        removeErrors: function () {
           
            var error_box = document.getElementById("error_panel");
            while (error_box.firstChild) {
                error_box.removeChild(error_box.firstChild);
            }
        },
        setTest: function (test) {
            isTest = test;
        },
        addAnswerToList: function (el) {
            var x = document.getElementById("list");
            var option = document.createElement("li");
            var optionVal = document.createElement('span');
            optionVal.className = "Value_option";
            optionVal.innerText = el;
            option.classList.add("list-group-item");
            var del = document.createElement('button');
            del.classList.add("btn", "btn-danger", "btn-xs", "no-border");
            del.innerHTML = 'Del'
            del.onclick = function () {
                this.parentElement.remove();
            };

            var space = document.createElement('label');
            space.innerHTML = "&nbsp;&nbsp;&nbsp; &nbsp;   ";
            option.appendChild(optionVal);
            option.appendChild(space);
            option.appendChild(del);
            if (checkIfExist2("list", el)) {
                alert("Element: \"" + el + "\". Already exist!");
            } else {
                if (el == "" || el == " ") {
                    alert("Input field is empty!");
                } else {
                    //alert(checkIfExist("list", el));
                    x.appendChild(option);
                }

            }
            refresh();
        },
        show: function () {
            //console.log(container);
            box.appendChild(container);
        },
        isEmpty: function () {
            console.log(container.childNodes.length);
            if (container.childNodes.length > 0)
                return false;
            else
                return true;
        }
    }
})();

function checkIfExist(id, val) {
    var result = false;
    var list = document.getElementById(id).getElementsByTagName("li");
    var textContentProp = "innerText" in document.body ? "innerText" : "textContent";
    for (i = 0; i < list.length; ++i) {

        if (list[i][textContentProp] == val) {
            console.log(list[i][textContentProp]);
            result = true;
        }
    }
    return result;
};
function checkIfExist2(id, val) {
    var result = false;
    var node;
    var list = document.getElementById(id);
    if (list != null) {
        NodeList.prototype.forEach = Array.prototype.forEach
        var children = list.childNodes;
        children.forEach(function (item) {
            for (var i = 0; i < item.childNodes.length; i++) {
                if (item.childNodes[i].className == "Value_option") {
                    node = item.childNodes[i];
                    break;
                }
            }
            if (node.innerText == val) {
                result = true;
            }
        });
    }
    return result;
};

function refresh() {
    $('.list-group.checked-list-box .list-group-item').each(function () {

        // Settings
        var $widget = $(this),
            $checkbox = $('<input type="checkbox" class="hidden" />'),
            color = ($widget.data('color') ? $widget.data('color') : "primary"),
            style = ($widget.data('style') == "button" ? "btn-" : "list-group-item-"),
            settings = {
                on: {
                    icon: 'glyphicon glyphicon-check'
                },
                off: {
                    icon: 'glyphicon glyphicon-unchecked'
                }
            };

        $widget.css('cursor', 'pointer')
        $widget.append($checkbox);

        // Event Handlers
        $widget.on('click', function () {
            $checkbox.prop('checked', !$checkbox.is(':checked'));
            $checkbox.triggerHandler('change');
            updateDisplay();
        });
        $checkbox.on('change', function () {
            updateDisplay();
        });


        // Actions
        function updateDisplay() {
            var isChecked = $checkbox.is(':checked');

            // Set the button's state
            $widget.data('state', (isChecked) ? "on" : "off");

            // Set the button's icon
            $widget.find('.state-icon')
                .removeClass()
                .addClass('state-icon ' + settings[$widget.data('state')].icon);

            // Update the button's color
            if (isChecked) {
                $widget.addClass(style + color + ' active');
            } else {
                $widget.removeClass(style + color + ' active');
            }
        }

        // Initialization
        function init() {

            if ($widget.data('checked') == true) {
                $checkbox.prop('checked', !$checkbox.is(':checked'));
            }

            updateDisplay();

            // Inject the icon if applicable
            if ($widget.find('.state-icon').length == 0) {
                $widget.prepend('<span class="state-icon ' + settings[$widget.data('state')].icon + '"></span>');
            }
        }
        init();
    });

    $('#get-checked-data').on('click', function (event) {
        event.preventDefault();
        var checkedItems = {}, counter = 0;
        $("#check-list-box li.active").each(function (idx, li) {
            checkedItems[counter] = $(li).text();
            counter++;
        });
        $('#display-json').html(JSON.stringify(checkedItems, null, '\t'));
    });


};
