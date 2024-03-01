/**
* Theme: Ubold Admin Template
* Author: Coderthemes
* Component: Full-Calendar
* 
*/




!function ($) {
    "use strict";

    var CalendarApp = function () {
        this.$body = $("body")
        this.$modal = $('#event-modal'),
        this.$event = ('#external-events div.external-event'),
        this.$calendar = $('#calendar'),
        this.$saveCategoryBtn = $('.save-category'),
        this.$categoryForm = $('#add-category form'),
        this.$extEvents = $('#external-events'),
        this.$calendarObj = null
    };


    /* on drop */
    CalendarApp.prototype.onDrop = function (eventObj, date) {
        var $this = this;
        // retrieve the dropped element's stored Event Object
        var originalEventObject = eventObj.data('eventObject');
        var $categoryClass = eventObj.attr('data-class');
        // we need to copy it, so that multiple events don't have a reference to the same object
        var copiedEventObject = $.extend({}, originalEventObject);
        // assign it the date that was reported
        copiedEventObject.start = date;
        if ($categoryClass)
            copiedEventObject['className'] = [$categoryClass];
        // render the event on the calendar
        $this.$calendar.fullCalendar('renderEvent', copiedEventObject, true);
        // is the "remove after drop" checkbox checked?
        if ($('#drop-remove').is(':checked')) {
            // if so, remove the element from the "Draggable Events" list
            eventObj.remove();
        }
    },
    /* on click on event */
    CalendarApp.prototype.onEventClick = function (calEvent, jsEvent, view) {
        var $this = this;
        var form = $("<form></form>");
        form.append("<div class='row'></div>");
        form.find(".row")
            .append("<div class='col-md-12'><div class='form-group'><label class='control-label'>Заголовок</label><input class='form-control' placeholder='Введите заголовок' type='text' name='title' value='" + calEvent.title + "'/></div></div>")
            .append("<div class='col-md-12'><div class='form-group'><label class='control-label'>Текст напоминания</label><textarea class='form-control' placeholder='Введите текст напоминания' name='text'>" + calEvent.text + "</textarea></div></div>");
        $this.$modal.modal({
            backdrop: 'static'
        });
        $this.$modal.find('.delete-event').show().end().find('.modal-body').empty().prepend(form).end().find('.delete-event').unbind('click').click(function () {
            $.ajax({
                type: 'POST',
                async: false,
                url: '/Home/SubmitReminderDelete',
                data: { dataReminderId: calEvent.id },
                success: function (data) {
                    $this.$calendarObj.fullCalendar('removeEvents', function (ev) {
                        return (ev._id == calEvent._id);
                    });
                    $this.$modal.modal('hide');
                }
            });
            
        });
        $this.$modal.find('.save-event').unbind('click').click(function () {
            form.submit();
        });
        $this.$modal.find('form').on('submit', function () {
            var title = form.find("input[name='title']").val();
            var text = form.find("textarea[name='text']").val();
            $.ajax({
                type: 'POST',
                async: false,
                url: '/Home/SubmitReminderSave',
                data: { id: calEvent.id, header: title, text: text, start_date: calEvent.start.format('DD.MM.YYYY'), end_date: calEvent.end.format('DD.MM.YYYY') },
                success: function (data) {
                    calEvent.title = title;
                    calEvent.text = text;
                    $this.$calendarObj.fullCalendar('updateEvent', calEvent);
                    $this.$modal.modal('hide');
                }
            });
            
            return false;
        });
    },
    /* on select */
    CalendarApp.prototype.onSelect = function (start, end, allDay) {
        var $this = this;
        $this.$modal.modal({
            backdrop: 'static'
        });
        var form = $("<form></form>");
        form.append("<div class='row'></div>");
        form.find(".row")
            .append("<div class='col-md-12'><div class='form-group'><label class='control-label'>Заголовок</label><input class='form-control' placeholder='Введите заголовок' type='text' name='title'/></div></div>")
            .append("<div class='col-md-12'><div class='form-group'><label class='control-label'>Текст напоминания</label><textarea class='form-control' placeholder='Введите текст напоминания' name='text'></textarea></div></div>");

        $this.$modal.find('.delete-event').hide().end().find('.save-event').show().end().find('.modal-body').empty().prepend(form).end().find('.save-event').unbind('click').click(function () {
            form.submit();
        });
        $this.$modal.find('form').on('submit', function () {
            var title = form.find("input[name='title']").val();
            var text = form.find("textarea[name='text']").val();
            var beginning = form.find("input[name='beginning']").val();
            var ending = form.find("input[name='ending']").val();
            if (title !== null && title.length != 0) {
                $.ajax({
                    type: 'POST',
                    async: false,
                    url: '/Home/SubmitReminderSave',
                    data: { header: title, text: text, start_date: start.format('DD.MM.YYYY'), end_date: end.format('DD.MM.YYYY') },
                    success: function (data) {
                        $this.$calendarObj.fullCalendar('renderEvent', {
                            id: data,
                            title: title,
                            text: text,
                            start: start,
                            end: end,
                            allDay: false,
                        }, true);
                        $this.$modal.modal('hide');
                    }
                });
                
            }
            else {
                alert('Поле напоминания не должно быть пустым');
            }
            return false;

        });
        $this.$calendarObj.fullCalendar('unselect');
    },
    CalendarApp.prototype.enableDrag = function () {
        //init events
        $(this.$event).each(function () {
            // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
            // it doesn't need to have a start or end
            var eventObject = {
                title: $.trim($(this).text()) // use the element's text as the event title
            };
            // store the Event Object in the DOM element so we can get to it later
            $(this).data('eventObject', eventObject);
            // make the event draggable using jQuery UI
            $(this).draggable({
                zIndex: 999,
                revert: true,      // will cause the event to go back to its
                revertDuration: 0  //  original position after the drag
            });
        });
    }
    /* Initializing */
    CalendarApp.prototype.init = function () {
        this.enableDrag();
        /*  Initialize the calendar  */
        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();
        var form = '';
        var today = new Date($.now());

        var defaultEvents = null;

        $.ajax({
            type: 'POST',
            async: false,
            url: '/Home/GetAllRemindersJson',
            success: function (data) {
                defaultEvents = data;
            }
        });

        var $this = this;
        $this.$calendarObj = $this.$calendar.fullCalendar({
            slotDuration: '00:15:00', /* If we want to split day time each 15minutes */
            minTime: '08:00:00',
            maxTime: '19:00:00',
            defaultView: 'month',
            handleWindowResize: true,
            height: $(window).height() - 250,
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },
            events: defaultEvents,
            editable: true,
            droppable: true, // this allows things to be dropped onto the calendar !!!
            eventLimit: true, // allow "more" link when too many events
            selectable: true,
            drop: function (date) { $this.onDrop($(this), date); },
            select: function (start, end, allDay) { $this.onSelect(start, end, allDay); },
            eventClick: function (calEvent, jsEvent, view) { $this.onEventClick(calEvent, jsEvent, view); },
            monthNames: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
            monthNamesShort: ['Янв.', 'Фев.', 'Март', 'Апр.', 'Май', 'Июнь', 'Июль', 'Авг.', 'Сент.', 'Окт.', 'Ноя.', 'Дек.'],
            dayNames: ["Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота"],
            dayNamesShort: ["ВС", "ПН", "ВТ", "СР", "ЧТ", "ПТ", "СБ"],
            buttonText: {
                today: "Сегодня",
                month: "Месяц",
                week: "Неделя",
                day: "День"
            },
            displayEventTime: false
        });
    },

    //init CalendarApp
    $.CalendarApp = new CalendarApp, $.CalendarApp.Constructor = CalendarApp

}(window.jQuery),

//initializing CalendarApp
function ($) {
    "use strict";
    $.CalendarApp.init()
}(window.jQuery);
