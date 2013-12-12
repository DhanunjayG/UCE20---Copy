/*
* Fancyform - jQuery Plugin
* Simple and fancy form styling alternative
*
* Examples and documentation at: http://www.lutrasoft.nl/jQuery/fancyform/
* 
* Copyright (c) 2010-2011 - Lutrasoft
* 
* Version: 1.3.0 (14/01/2013)
* Requires: jQuery v1.3.2+ 
*
* Dual licensed under the MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
*/
(function ($) {
    $.simpleEllipsis = function (totalTxt, maxchars) {
        var text,
	        i;

        if (totalTxt.length < maxchars) { return totalTxt; }

        for (i = 0; i < maxchars; i++) {
            text = totalTxt.substring(0, i + 1) + "...";
        }

        return text;
    }

    $.isTouchDevice = function () {
        return !!('ontouchstart' in window);
    };

    $.fn.extend({
        /*
        Get the caret on an textarea
        */
        caret: function (start, end) {
            var elem = this[0];

            if (elem) {
                // get caret range
                if (typeof start == "undefined") {
                    if (elem.selectionStart) {
                        start = elem.selectionStart;
                        end = elem.selectionEnd;
                    }
                    // <= IE 8
                    else if (document.selection) {
                        var val = this.val();
                        this.focus();

                        var r = document.selection.createRange();
                        if (r == null) {
                            return { start: 0, end: e.value.length, length: 0 }
                        }

                        var re = elem.createTextRange();
                        var rc = re.duplicate();
                        re.moveToBookmark(r.getBookmark());
                        rc.setEndPoint('EndToStart', re);

                        // IE counts a line (not \n or \r) as 1 extra character
                        return { start: rc.text.length - (rc.text.split("\n").length + 1) + 2, end: rc.text.length + r.text.length - (rc.text.split("\n").length + 1) + 2, length: r.text.length, text: r.text };
                    }
                }
                // set caret range
                else {
                    var val = this.val();

                    if (typeof start != "number") start = -1;
                    if (typeof end != "number") end = -1;
                    if (start < 0) start = 0;
                    if (end > val.length) end = val.length;
                    if (end < start) end = start;
                    if (start > end) start = end;

                    elem.focus();

                    if (elem.selectionStart) {
                        elem.selectionStart = start;
                        elem.selectionEnd = end;
                    }
                    else if (document.selection) {
                        var range = elem.createTextRange();
                        range.collapse(true);
                        range.moveStart("character", start);
                        range.moveEnd("character", end - start);
                        range.select();
                    }
                }

                return { start: start, end: end };
            }
        },
        insertAtCaret: function (myValue) {
            return this.each(function (i) {
                if (document.selection) {
                    //For browsers like Internet Explorer
                    this.focus();
                    sel = document.selection.createRange();
                    sel.text = myValue;
                    this.focus();
                }
                else if (this.selectionStart || this.selectionStart == '0') {
                    //For browsers like Firefox and Webkit based
                    var startPos = this.selectionStart;
                    var endPos = this.selectionEnd;
                    var scrollTop = this.scrollTop;
                    this.value = this.value.substring(0, startPos) + myValue + this.value.substring(endPos, this.value.length);
                    this.focus();
                    this.selectionStart = startPos + myValue.length;
                    this.selectionEnd = startPos + myValue.length;
                    this.scrollTop = scrollTop;
                } else {
                    this.value += myValue;
                    this.focus();
                }
            })
        },

       
        /*
        Replace select with list
        =========================
        HTML will look like
        <ul>
        <li><span>Selected value</span>
        <ul>
        <li data-settings='{"alwaysvisible" : true}'><span>Option</span></li>
        <li><span>Option</span></li>
        </ul>
        </li>
        </ul>
        */
        transformSelect: function (opts) {
            var defaults = {
                dropDownClass: "transformSelect",
                showFirstItemInDrop: true,

                acceptManualInput: false,
                useManualInputAsFilter: false,

                subTemplate: function (option) {
                    if ($(this)[0].type == "select-multiple") {

                        return "<span><input type='checkbox' value='" + $(option).val() + "' " + ($(option).is(":selected") ? "checked='checked'" : "") + " name='" + $(this).attr("name").replace("_backup", "") + "' />" + $(option).text() + "</span>";
                    }
                    else {
                        return "<span>" + $(option).text() + "</span>";
                    }
                },
                initValue: function () { return $(this).text(); },
                valueTemplate: function () { return $(this).text(); },

                ellipsisLength: null,
                addDropdownToBody: false
            };

            var options = $(this).data("settings");

            var method = {
                init: function () {
                    // Hide mezelf
					
                    $(this).hide();
					
                    // Generate HTML
                    var selectedIndex = -1,
                        selectedOption = null,
                        _this = this;
					
                    if ($(this).find("option:selected").length > 0 && $(this)[0].type != "select-multiple") {
                        selectedOption = $(this).find("option:selected");
                        selectedIndex = $(this).find("option").index(selectedOption);
                    }
                    else {
                        selectedIndex = 0;
                        selectedOption = $(this).find("option:first");
                    }

                    // Maak een ul aan
                    var ul = "<ul class='" + options.dropDownClass + " trans-element'>";
                    ul += "<li>"

                    if (options.acceptManualInput && !$.isTouchDevice()) {
                        var value = $(this).data("value") ? $(this).data("value") : options.initValue.call(selectedOption);
                        ul += "<ins></ins><input type='text' name='" + $(this).attr("name").replace("_backup", "") + "' value='" + value + "' />";

                        // Save old select
                        if ($(this).attr("name").indexOf("_backup") == -1) {
                            $(this).attr("name", $(this).attr("name") + "_backup");
                        }
                    }
                    else {
                        if (options.ellipsisLength) {
                            ul += "<span title=\"" + selectedOption.text() + "\">" + $.simpleEllipsis(options.initValue.call(selectedOption), options.ellipsisLength) + "</span>";
                        }
                        else {
                            ul += "<span>" + options.initValue.call(selectedOption) + "</span>";
                        }
                    }

                    ul += "<ul style='display: none;'>";

                    $(this).children().each(function (i) {
                        if (i == 0 && options.showFirstItemInDrop == false) {
                            // Don't do anything when you don't wanna show the first element
                        }
                        else {
                            var child = "";
                            switch ($(this).get(0).tagName.toUpperCase()) {
                                case "OPTION":
                                    ul += method.getLIOptionChild.call(_this, this);
                                    break;
                                case "OPTGROUP":
                                    ul += method.getLIOptgroupChildren.call(_this, this);
                                    break;
                            }
                        }
                    });

                    ul += "</ul>";
                    ul += "</li>";
                    ul += "</ul>";

                    $(this).after(ul);

                    // Bind handlers
                    var $ul = $(this).next("ul");

                    if ($(this).is(":disabled")) {
                        method.disabled.call(this, true);
                    }

                    if ($(this)[0].type == "select-multiple" && !$.isTouchDevice()) {
                        if ($(this).attr("name") && $(this).attr("name").indexOf("_backup") == -1) {
                            $(this).attr("name", $(this).attr("name") + "_backup");
                        }
                        $ul.find("ul li:not(.group)").click(method.selectCheckbox);
                    }
                    else {
                        $ul.find("ul li:not(.group)").click(method.selectNewValue);

                        $ul.find("input").click(method.openDrop)
                    				.keydown(function (e) {
                    				    var ar = [9, 13]; // Tab or enter
                    				    if ($.inArray(e.which, ar) != -1)
                    				        method.closeAllDropdowns();
                    				})
                                    .prev("ins")
                                    .click(method.openDrop);
                    }

                    if (options.useManualInputAsFilter) {
                        $ul.find("input").keyup(method.filterByInput);
                    }

                    $ul.find("span").eq(0).click(method.openDrop);

                    // Set data if we use addDropdownToBody option
                    $ul.find("ul:first").data("trans-element", $ul).addClass("transformSelectDropdown");
                    $ul.data("trans-element-drop", $ul.find("ul:first"));

                    if (options.addDropdownToBody) {
                        $ul.find("ul:first").appendTo("body");
                    }

                    // Check if there is already an event
                    $("html").unbind("click.transformSelect").bind("click.transformSelect", method.closeDropDowns)                    // Bind hotkeys

                    if ($.hotkeys && !$("body").data("trans-element-select")) {
                        $("body").data("trans-element-select", true);

                        $(document)
                            .bind("keydown.up", function (e) {
                                var ul = $(".trans-focused");
                                // Only enable on trans-element without input
                                if (!ul.length || ul.find("input").length) return false;
                                var select = ul.prevAll("select").first();

                                var selectedIndex = select[0].selectedIndex - 1
                                if (selectedIndex < 0) {
                                    selectedIndex = select.find("option").length - 1;
                                }

                                method.selectIndex.call(select, selectedIndex);

                                return false;
                            })
                            .bind("keydown.down", function (e) {
                                var ul = $(".trans-focused");
                                // Only enable on trans-element without input
                                if (!ul.length || ul.find("input").length) return false;
                                var select = ul.prevAll("select").first();

                                var selectedIndex = select[0].selectedIndex + 1
                                if (selectedIndex > select.find("option").length - 1) {
                                    selectedIndex = 0;
                                }

                                method.selectIndex.call(select, selectedIndex);
                                return false;
                            });
                    }

                    // Gebruik native selects
                    if ($.isTouchDevice()) {
                        $(this)
                            .appendTo($ul.find("li:first"))
                            .show()
                            .css({
                                opacity: 0,
                                position: "absolute",
                                width: "100%",
                                height: "100%",
                                left: 0,
                                top: 0
                            });
                        $ul.find("li:first").css({
                            position: "relative"
                        });
                        $(this).change(method.mobileChange);
                    }
                },
                getUL: function () {
                    if ($.isTouchDevice()) {
                        return $(this).closest("ul");
                    }
                    else {
                        return $(this).next(".trans-element:first")
                    }
                },
                getSelect: function ($ul) {
                    if ($.isTouchDevice()) {
                        return $ul.find("select");
                    }
                    else {
                        return $ul.prevAll("select").eq(0);
                    }
                },
                disabled: function (disabled) {
                    var ul = method.getUL.call(this);
                    if (disabled) {
                        ul.addClass("disabled");
                    }
                    else {
                        ul.removeClass("disabled");
                    }
                },
                repaint: function () {
                    var ul = method.getUL.call(this),
                        dropdown = ul.data("trans-element-drop");

                    dropdown.remove();
                    ul.remove();

                    method.init.call(this);
                },
                filterByInput: function () {
                    var val = $(this).val().toLowerCase(),
                        ul = $(this).closest("ul"),
                        drop = ul.data("trans-element-drop"),
                        li = drop.find("li");

                    if (val == "") {
                        li.show();
                    }
                    else {
                        li.each(function () {
                            if ($(this).data("settings").alwaysvisible === true) {
                                $(this).show();
                            }
                            else {
                                $(this)[$(this).text().toLowerCase().indexOf(val) == -1 ? "hide" : "show"]();
                            }
                        });
                    }
                },
                selectIndex: function (index) {
                    var select = $(this),
                        ul = method.getUL.call(this),
                        drop = ul.data("trans-element-drop");

                    try {
                        drop.find("li").filter(function () {
                            return $(this).text() == select.find("option").eq(index).text();
                        }).first().trigger("click");
                    }
                    catch (err) { }
                },
                selectValue: function (value) {
                    var select = $(this),
                        ul = method.getUL.call(this),
                        drop = ul.data("trans-element-drop");

                    if (value == null) {
                        //Selecteer waarde zonder value attribuut
                        method.selectIndex.call(this, select.find("option:not([value])").index());
                    }
                    else {
                        method.selectIndex.call(this, select.find("option[value='" + value + "']").index());
                    }
                },
                /*
                *	GET option child
                */
                getLIOptionChild: function (option) {
                    var settings = $(option).attr("data-settings");
                    if (settings && settings != "") {
                        settings = "data-settings='" + settings + "'";
                    }
                    if ($(option).hasClass('hideMe')) {
                        settings = settings + " class='hideMe'";
                    }
                    return "<li " + settings + ">" + options.subTemplate.call(this, $(option)) + "</li>";
                },
                /*
                *	GET optgroup children
                */
                getLIOptgroupChildren: function (group) {
                    var li = "<li class='group'>",
                        _this = this;
                    li += "<span>" + $(group).attr("label") + "</span>";
                    li += "<ul>";

                    $(group).find("option").each(function () {
                        li += method.getLIOptionChild.call(_this, this);
                    });

                    li += "</ul>";
                    li += "</li>";

                    return li;
                },
                /*
                *	Select a new value
                */
                selectNewValue: function () {
                    var index = 0;
                    if ($(this).closest(".group").length != 0) {
                        var group = $(this).closest(".group")
                        index = $(this).closest(".transformSelectDropdown").find("li").index($(this)) - group.prevAll(".group").length - 1;
                    }
                    else {
                        index = $(this).parent().find("li").index($(this));
                        if (options.showFirstItemInDrop == false) {
                            index += 1;
                        }
                        index -= $(this).prevAll(".group").length;
                    }

                    var $drop = $(this).closest(".transformSelectDropdown"),
						$ul = $drop.data("trans-element"),
                        select = method.getSelect($ul);

                    select[0].selectedIndex = index;

                    // If it has an input, there is no span used for value holding
                    if ($ul.find("input").length > 0) {
                        $ul.find("input").val(options.valueTemplate.call($(this)));
                    }
                    else {
                        if (options.ellipsisLength) {
                            $ul
					            .find("span")
                                .eq(0)
					            .html($.simpleEllipsis(options.valueTemplate.call(select.find("option:selected")), options.ellipsisLength));
                        }
                        else {
                            $ul
					            .find("span")
                                .eq(0)
					            .html(options.valueTemplate.call(select.find("option:selected")));
                        }
                    }

                    method.closeAllDropdowns();

                    // Trigger onchange
                    select.trigger("change");

                    $(".trans-element").removeClass("trans-focused");
                    $ul.addClass("trans-focused");

                    // Update validator
                    if ($.fn.validate) {
                        select.valid();
                    }
                },
                mobileChange: function () {
                    var select = $(this),
                        $ul = method.getUL.call(this);

                    if (options.ellipsisLength) {
                        $ul
					            .find("span")
                                .eq(0)
					            .html($.simpleEllipsis(options.valueTemplate.call(select.find("option:selected")), options.ellipsisLength));
                    }
                    else {
                        $ul
					            .find("span")
                                .eq(0)
					            .html(options.valueTemplate.call(select.find("option:selected")));
                    }
                },
                selectCheckbox: function (e) {
                    var t = $(this).closest("li");
                    if ($(e.target).is("li")) {
                        t = $(this);
                    }
                    else if ($(e.target).is(":checkbox")) {
                        return true;
                    }

                    var checkbox = $(this).find(":checkbox");
                    if (checkbox.is(":checked")) {
                        checkbox.removeAttr("checked");
                    }
                    else {
                        checkbox.attr("checked", "checked");
                    }

                    if (checkbox.data("transformCheckbox.initialized") === true) {
                        checkbox.transformCheckbox("setImage");
                    }

                    checkox.change();
                },
                /*
                *	Open clicked dropdown
                *		and Close all others
                */
                openDrop: function () {
                    var UL = $(this).closest(".trans-element"),
                        childUL = UL.data("trans-element-drop"),
						childLI = $(this).parent();

                    if (UL.hasClass("disabled")) {
                        return false;
                    }

                    // Close on second click
                    if (childLI.hasClass("open") && !$(this).is("input")) {
                        method.closeAllDropdowns();
                    }
                    // Open on first click
                    else {
                        childLI
							.css({ 'z-index': 1200 })
							.addClass("open");

                        childUL.css({ 'z-index': 1200 }).show();

                        method.hideAllOtherDropdowns.call(this);
                    }

                    if (options.addDropdownToBody) {
                        childUL.css({
                            position: "absolute",
                            top: childLI.offset().top + childLI.outerHeight(),
                            left: childLI.offset().left
                        });
                    }
                },
                /*
                *	Hide all elements except this element
                */
                hideAllOtherDropdowns: function () {
                    // Hide elements with the same class
                    var allElements = $("body").find("*");
                    var elIndex = allElements.index($(this).parent());

                    $("body").find("ul.trans-element:not(.ignore)").each(function () {
                        var childUL = $(this).data("trans-element-drop");

                        if (elIndex - 1 != allElements.index($(this))) {
                            childUL
                                   .hide()
                                   .css({ 'z-index': 0 })
                                        .parent()
                                        .css({ 'z-index': 0 })
                                        .removeClass("open");
                        }
                    });
                },
                /*
                *	Close all dropdowns
                */
                closeDropDowns: function (e) {
                    if ($(e.target).closest(".trans-element").length == 0) {
                        method.closeAllDropdowns();
                    }
                },
                closeAllDropdowns: function () {
                    $("ul.trans-element:not(.ignore)").each(function () {
                        var childUL = $(this).data("trans-element-drop");
                        childUL.hide();
                        $(this).find("li").eq(0).removeClass("open")
                    }).removeClass("trans-focused");
                }
            }

            if (typeof (opts) == "string") {
                method[opts].apply(this, Array.prototype.slice.call(arguments, 1))
                return this;
            }
            return this.each(function () {
                // Is already initialized
                if ($(this).data("transformSelect.initialized") === true) {
                    return this;
                }

                options = $.extend(defaults, opts);
                $(this).data("settings", options);

                // set initialized
                $(this).data("transformSelect.initialized", true);

                // Call init functions
                method.init.call(this);
                return this;
				
            });
			
			
			
			
			
			
			
			
        },
        /*
        Transform a input:file to your own layout
        ============================================
        Basic CSS:
        <style>
        .customInput {
        display: inline-block;
        font-size: 12px;
        }
		
        .customInput .inputPath {
        width: 150px;
        padding: 4px;
        display: inline-block;
        border: 1px solid #ABADB3;
        background-color: #FFF;
        overflow: hidden;
        vertical-align: bottom;
        white-space: nowrap;
        -o-text-overflow: ellipsis;
        text-overflow:    ellipsis;
        }
		
        .customInput .inputButton {
        display: inline-block;
        padding: 4px;
        border: 1px solid #ABADB3;
        background-color: #CCC;
        vertical-align: bottom;
        }        </style>
        */
        transformFile: function (options) {
            var method = {
                file: function (fn, cssClass) {
                    return this.each(function () {
                        var el = $(this);
                        var holder = $('<div></div>').appendTo(el).css({
                            position: 'absolute',
                            overflow: 'hidden',
                            '-moz-opacity': '0',
                            filter: 'alpha(opacity: 0)',
                            opacity: '0',
                            zoom: '1',
                            width: el.outerWidth() + 'px',
                            height: el.outerHeight() + 'px',
                            'z-index': 1
                        });

                        var wid = 0;

                        var inp;

                        var addInput = function () {
                            var current = inp = holder.html('<input ' + (window.FormData ? 'multiple ' : '') + 'type="file" style="border:none; position:absolute">').find('input');

                            wid = wid || current.width();

                            current.change(function () {
                                current.unbind('change');

                                addInput();
                                fn(current[0]);
                            });
                        };
                        var position = function (e) {
                            holder.offset(el.offset());
                            if (e) {
                                inp.offset({ left: e.pageX - wid + 25, top: e.pageY - 10 });
                                addMouseOver();
                            }
                        };

                        var addMouseOver = function () {
                            el.addClass(cssClass + 'MouseOver');
                        };

                        var removeMouseOver = function () {
                            el.removeClass(cssClass + 'MouseOver');
                        };

                        addInput();

                        el.mouseover(position);
                        el.mousemove(position);
                        el.mouseout(removeMouseOver);
                        position();
                    });
                }
            };

            return this.each(function (i) {
                // Is already initialized
                if ($(this).data("transformFile.initialized") === true) {
                    return this;
                }

                // set initialized
                $(this).data("transformFile.initialized", true);

                // 
                var el = $(this);
                var id = null;
                var name = el.attr('name');
                var cssClass = (!options ? 'customInput' : (options.cssClass ? options.cssClass : 'customInput'));
                var label = (!options ? 'Browse...' : (options.label ? options.label : 'Browse...'));

                el.hide();

                if (!el.attr('id')) { el.attr('id', 'custom_input_file_' + (new Date().getTime()) + Math.floor(Math.random() * 100000)); }
                id = el.attr('id');

                el.after('<span id="' + id + '_custom_input" class="' + cssClass + '"><span class="inputPath" id="' + id + '_custom_input_path">&nbsp;</span><span class="inputButton">' + label + '</span></span>');

                method.file.call($('#' + id + '_custom_input'), function (inp) {
                    inp.id = id;
                    inp.name = name;
                    $('#' + id).replaceWith(inp);
                    $('#' + id).removeAttr('style').hide();
                    $('#' + id + '_custom_input_path').html($('#' + id).val().replace(/\\/g, '/').replace(/.*\//, ''));
                }, cssClass);

                return this;
            });

        }
        
    });

})(jQuery);