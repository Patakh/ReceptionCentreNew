
var ws = null;
function connect() {
    ws = new WebSocket("wss://192.168.34.9:8889");
    ws.onopen = function () {
        //alert("About to send data");
        ws.send("89604195432"); // I WANT TO SEND THIS MESSAGE TO THE SERVER!!!!!!!!
        //alert("Message sent!");
    };

    ws.onmessage = function (evt) {
        //alert("About to receive data");
        var received_msg = evt.data;
        console.log("Message received = " + received_msg);
    };
    ws.onclose = function () {
        // websocket is closed.
        console.log("Connection is closed...");
    };
};
var i = 0;


// unblock when ajax activity stops
$(document).ajaxStart(function () {
    $.blockUI({ message: '<div class="fa fa-spin fa-spinner icon-lg"></div>', overlayCSS: { backgroundColor: "#FFF", cursor: "wait" }, css: { border: 0, padding: 0, backgroundColor: "none" } });
}).ajaxStop(function () {
    $.unblockUI();
});

var resizefunc = [];
$('.navbar-toggle').on('click', function (event) {
    $(this).toggleClass('open');
    $('#navigation').slideToggle(400);
    $('.cart, .search').removeClass('open');
});

$('.navigation-menu>li').slice(-1).addClass('last-elements');

$('.navigation-menu li.has-submenu a[href="#"]').on('click', function (e) {
    if ($(window).width() < 992) {
        e.preventDefault();
        $(this).parent('li').toggleClass('open').find('.submenu:first').toggleClass('open');
    }
});


// событие клика по веб-документу
$(document).mouseup(function (e) {

    var bar = $("#alertBar"); // Панель уведомлений
    var btn = $("#alertBtn"); // Кнопка уведомлений

    // Проверяем, находится ли курсор на панели или на кнопке уведомлений, если нет, то закрываем панель
    if (!bar.is(e.target)
        && bar.has(e.target).length === 0
        && !btn.is(e.target)
        && btn.has(e.target).length === 0) {
        bar.removeClass('slideInRight').addClass('slideOutRight');
        setTimeout(function () {
            bar.hide().removeClass('slideOutRight');
            btn.closest('li').removeClass('open');
        }, 200);
    }
    else {
        $('.navbar .active').removeClass('active');
        btn.closest('li').addClass('open');
    }
});

$(document).on('click', '[data-toggle="animation"]', function (event) {
    //event.stopPropagation();
    var button = $(this);
    var target = $(button.data('target'));
    var animationIn = button.data('animation');
    var animationOut = animationIn.replace('In', 'Out').replace('Up', 'Down').replace('Down', 'Up');
    var state = target.hasClass(animationIn);
    if (state) {
        target.removeClass(animationIn).addClass(animationOut);
        setTimeout(function () {
            target.hide().removeClass(animationOut);
            button.closest('li').removeClass('open');
        }, 200);
    } else {
        target.addClass(animationIn).show();
        button.closest('li').addClass('open');
    }
});
$('#delete-dublication-call').click(function () {
    $.ajax({
        url: 'System/DeleteDublicationCall',
        type: 'POST',
        success: function (data) {
            location.reload();
        }
    });
});

$(document).ready(function () {
    $('.select2').select2();
    $('.select2-nosearch').select2({ minimumResultsForSearch: Infinity });
    $('.select2-inmodal').select2({
        dropdownParent: $(".modal-wrapper")
    });
    $('.select2-nosearch-inmodal').select2({
        dropdownParent: $(".modal-wrapper"),
        minimumResultsForSearch: Infinity
    });
    $('.collaborator-placeholder').select2({
        placeholder: 'Сотрудник'
    });

    // popover'ы с поддержкой html разметки.
    $(function () {
        $('[data-popover]').popover({
            container: 'body',
            trigger: 'hover',
            placement: 'bottom',
            html: true
        });
    });

    connection.on("ReceiveNotification", function (user, message) {
        console.log(user + " says " + message);
    });

    connection.on("sendComment", function (message) {
        Lobibox.notify('info', {
            title: 'Уведомление!',
            size: 'normal',
            position: 'top right',
            msg: message,
            soundPath: '~/plugins/lobibox/sounds/',
            sound: true,
            delay: false,
        });

        //$.Notification.autoHideFalseNotify('custom', 'right top false', 'Новое уведомление!', message);
        console.log("Получено уведомление");
    });

    // входящий звонок
    connection.on("incomingCall", function (message) {
        showIncomingCallAlert(message);
        console.log("Звонок");
    });

    connection.on("ReturnCallId", function (message) {
        $('#jitsi_call_id').val(message); // Присвоение Id-звонка
        console.log(message, "ИД звонка от звонилки");
    });

    connection.on("callback", function (message) {
        CallBackCountRefresh();
        CallBackRefresh();
    });

    connection.on("get_online_users", function (result) {
        console.log(Object.keys(result));
        var connectionId = connection.hub.Id;
        console.log(connectionId);
    })
     
    if (document.visibilityState == "visible") {
        console.log("первый вызов _hubStart");
        _hubStart();
    }

    function _hubStart() {
        connection.start().then(function () {
            connection.invoke("sendNotification", "Hello from client");

        }).catch(function (err) {
            return console.log(err.toString());
        });
    }
     
    document.addEventListener('visibilitychange', handleVisibilityChange, false);

    function handleVisibilityChange() {
        if (document.visibilityState == "visible") {
            //connection.onreconnecting();
            _hubStart(); 
        } else {
            if (connection.state == 'Connected') {
                connection.stop();
            } 
        }
    }

    $.ajax({
        async: true,
        url:'../Socket/SocketJitsi' ,
        type: 'POST',
        success: function (data) {
            console.log('SocketJitsi', data);
        }
    });
    $.ajax({
        async: true,
        url: '../Common/GetEmails',
        type: 'POST',
        success: function (data) {
            $('#menu_input_email').text(data);
        }
    });

    $.ajax({
        async: true,
        url: '../Common/GetNotifications',
        type: 'POST',
        success: function (data) {
            $('#menu_input_notification').text(data);
        }
    });

});
// Callback
$('#callBackHeadBtn').on('click', function (e) {
    CallBackRefresh();
});
function CallBackCustomer(Id, fio, tel) {
    $.ajax({
        type: "POST",
        url: "/Appeal/_CallBackPhone",
        data: { CallbackId: Id, CustomerFio: fio, tel: tel }, // обратный звонок type 2
        success: function (result) {
            var socket = new WebSocket("wss://localhost:8889");
            socket.onopen = function () {
                socket.send(result);
            };
            socket.onmessage = function (event) {
                console.log("Ответ", event.data);
            }
        },
        error: function (event) {
            console.log(event);
        }
    });
}
function CallBackClose(Id) {
    swal({
        title: "Закрыть заявку?",
        text: "Заявка будет закрыта",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Да",
        cancelButtonText: "Нет"
    }).then(function () {
        $.ajax({
            type: 'POST',
            url: 'CallBack/CallBackClose',
            data: { Id: Id },
            success: function (data) {
                if (data !== null) {
                    swal({
                        title: "Готово!",
                        text: "Заявка закрыта",
                        type: "success",
                        confirmButtonColor: "#73c482",
                        confirmButtonText: "Закрыть"
                    });
                }
                else {
                    swal({
                        title: "Ошибка!",
                        text: "Не удалось закрыть заявку",
                        type: "error",
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Закрыть"
                    });
                }
            }
        });
    }).catch(swal.noop);
    // Отключить закрытые dropdown при клике внутри самого dropdown
    function navSearch(search, $target) {
        if (search.length != 0) {
            if (search.length >= 3) {
                $.ajax({
                    url: "/Home/Search",
                    type: "GET",
                    data: { search: search },
                    success: function (data) {
                        if (data.length > 0) {
                            var resultList = '<div class="table-responsive"> <table id="searchAppealTable" class="table cases-table table-hover table-vertical-middle mb-0"><tbody>';
                            data.forEach(function (e) {
                                resultList += '<tr data-appeal-number="' + e.NumberAppeal + '">';
                                resultList += '<td class="text-left">';
                                resultList += '<span class="font-12 text-nowrap">' + e.ApplicantName + '</span>';
                                resultList += '<b class="d-block my-1 lh-12">' + e.TextAppeal + '</b>';
                                resultList += '<p class="mb-0 font-12">';
                                resultList += '<span>#' + e.NumberAppeal + '</span> от <span class="m-r-5">' + moment(e.DateAdd).format('DD.MM.YYYY') + '</span>';
                                resultList += '<span class="label label-default">' + e.StatusName + '</span>';
                                resultList += '</td>';
                                resultList += '</tr>';
                            });
                            resultList += '</tbody></table></div></div>';
                            $target.html(resultList);
                        }
                        else {
                            $target.html('<div class="alert-default" style="padding:.75rem 1.25rem;"> <span style="color: #00545c;"><strong><i class="icon-info"></i></strong> Нет данных </span> </div>');
                        }
                        $('#searchResults').addClass('show');
                    },
                    beforeSend: function (b) {
                        var em = '<div class="alert-default" style="padding:.75rem 1.25rem;"> <span style="color: #00545c;"><strong><i class="icon-info"></i></strong> Идет поиск ... </span> </div>';
                        $target.html(em);
                    }
                });
            } else {
                $target.html('<div class="alert-default" style="padding:.75rem 1.25rem;"> <span style="color: #00545c;"><strong><i class="icon-info"></i></strong> Введите 3 или более символа </span> </div>');
                $('#searchResults').addClass('show');
            }
        }
        return false;
    };

    $(document).on({
        keyup: debounce(function () {
            var $input = $('#navSearchString'),
                $target = $('#searchResults');
            navSearch($input.val(), $target);
        }, 500),
        click: function () {
            var $input = $('#navSearchString'),
                $target = $('#searchResults');
            navSearch($input.val(), $target);
        }
    }, '#navSearchString, #appSearchBtn');

    $(window).click(function () {
        $('.navbar-search-results').removeClass('show');
    });

    $(document).on('click', '#searchResults tr', function () {
        var url = '';
        var number = $(this).data('appealNumber');
        //url = '/Appeal/AppealInfo?number=' + number;
        //location.href = url;
        $.ajax({
            url: '../Appeal/AppealInfo',
            data: { number: number, modal: true },
            type: 'POST',
            success: function (data) {
                $('#newModal').modal('show').html(data);
            },
        })
    });

    // Добавляем класс active к кнопкам в шаге Заявитель
    $('.list-group-item-action').click(function () {
        $('.list-group-item-action').removeClass('active');
        $(this).addClass('active');
    });

    // Добавляем возможность выбора строки в таблице
    $('.route-table tr:not(.thead-row)').click(function () {
        if ($(this).is('.active')) {
            $(this).removeClass('active').removeClass('unfocused');
            $(this).siblings('tr:not(.thead-row)').removeClass('active').removeClass('unfocused');
        } else {
            $(this).siblings('tr:not(.thead-row)').removeClass('active').addClass('unfocused');
            $(this).removeClass('unfocused').addClass('active');
        }
    });

    // Добавляем возможность множественного выбора строк в таблице
    $('.route-table-multiple tr:not(.thead-row)').click(function () {
        if ($(this).is('.active')) {
            $(this).removeClass('active').addClass('unfocused');
            if (!$(this).siblings('tr:not(.thear-row)').hasClass('active')) {
                $(this).removeClass('unfocused').siblings('tr:not(.thead-row)').removeClass('unfocused');
            }
        } else {
            $(this).siblings('tr:not(.thead-row):not(.active)').removeClass('active').addClass('unfocused');
            $(this).removeClass('unfocused').addClass('active');
        }
    });
}

//============= Опрос ==============
var initQuiz = function (template) {

    let quizList = template.querySelector('.quiz-list');
    let quizLinks = quizList.querySelectorAll('a');
    function showQuizList() {
        quizList.classList.toggle('d-none');
        quizContainer.classList.toggle('d-none');
    };

    quizLinks.forEach(function (it) {
        it.addEventListener('click', function (evt) {
            evt.preventDefault();
            showQuizList();
        });
    });

    function showSlide(n) {
        slides[currentSlide].classList.remove('active-slide');
        slides[n].classList.add('active-slide');
        currentSlide = n;

        if (currentSlide === 0) {
            returnButton.classList.remove('d-none');
            previousButton.classList.add('d-none');
        } else {
            returnButton.classList.add('d-none');
            previousButton.classList.remove('d-none');
        }

        if (currentSlide === slides.length - 1) {
            nextButton.classList.add('d-none');
            submitButton.classList.remove('d-none');
        } else {
            nextButton.classList.remove('d-none');
            submitButton.classList.add('d-none');
        }
    };

    function showNextSlide() {
        showSlide(currentSlide + 1);
    };

    function showPreviousSlide() {
        showSlide(currentSlide - 1);
    };

    function showResults() {
        let returnBtn = document.createElement('button');
        returnBtn.classList.add('btn', 'btn-purple', 'waves-effect', 'waves-light');
        returnBtn.textContent = 'Вернуться к списку';
        returnBtn.addEventListener('click', function (evt) {
            evt.preventDefault();
            showSlide(0);
            quizList.classList.toggle('d-none');
            resultsContainer.innerHTML = '';
        });

        let resultText = document.createElement('h4');
        resultText.textContent = 'Результат опроса сохранен.';
        resultText.classList.add('mb-4', 'text-success');

        quizContainer.classList.add('d-none');
        resultsContainer.appendChild(resultText);
        resultsContainer.appendChild(returnBtn);
        resultsContainer.classList.remove('d-none');
    };

    let quizContainer = template.querySelector('.quiz-container');
    let quiz = quizContainer.querySelector('.quiz');
    let submitButton = quizContainer.querySelector('.btn-submit');
    let resultsContainer = template.querySelector('.results');

    let returnButton = quizContainer.querySelector('.btn-return');
    let previousButton = quizContainer.querySelector('.btn-previous');
    let nextButton = quizContainer.querySelector('.btn-next');
    let slides = quizContainer.querySelectorAll('.slide');
    let currentSlide = 0;

    showSlide(0);

    // on submit, show results
    returnButton.addEventListener('click', showQuizList);
    submitButton.addEventListener('click', showResults);
    previousButton.addEventListener('click', showPreviousSlide);
    nextButton.addEventListener('click', showNextSlide);
};

var initPopovers = function () {
    $(function () {
        $('.case-text').popover({
            trigger: 'hover',
            container: '.call-modal',
            placement: 'bottom'
        });
    });
};

// Модальное окно набора номера
var callModal = new Custombox.modal({
    content: {
        effect: 'push',
        animateFrom: 'left',
        animateTo: 'left',
        fullscreen: true,
        target: '#call_modal'
    }
});

const callModalContent = document.querySelector('#call_modal .custom-modal-text');

var onCallModalBtnClick = function (e, d) {
    let callModalTemplate = document.querySelector('#call-modal-template').content.cloneNode(true);

    let dialpadTemplate = document.querySelector('#dialpad-template').content.cloneNode(true);
    let callInfoTemplate = document.querySelector('#call-info-template').content.cloneNode(true);
    let colTopCenter = callModalTemplate.querySelector('.top-center');
    let colBottomRight = callModalTemplate.querySelector('#call-survey');
    let callModalCols = callModalTemplate.querySelectorAll('.call-modal-col');

    let dialpad = dialpadTemplate.querySelector('.dialpad')
    console.log(e);
    if (e.length <= 12) {
        dialpadTemplate.querySelector('#call_number').value = e;
    }
    let callNumber = dialpadTemplate.querySelector('#call_number');
    let backspaceBtn = dialpadTemplate.querySelector('#btn_backspace');
    let callBtn = dialpadTemplate.querySelector('#btnCall');

    var onCallNumberInput = function (evt) {
        evt.target.style.opacity = 1;

        if (callNumber.value) {
            backspaceBtn.classList.remove('d-none');
        } else {
            backspaceBtn.classList.add('d-none');
        }
    };

    var onDialpadClick = function (evt) {
        evt.preventDefault();

        if (callNumber.value.length < 11 && evt.target.tagName.toLowerCase() === 'button') {
            callNumber.value += evt.target.textContent.replace(/[\n\r]+|[\s]{2,}/g, ' ').trim();
        }

        if (callNumber.value) {
            backspaceBtn.classList.remove('d-none');
        }
    };

    var onBackspaceBtnClick = function (evt) {
        evt.preventDefault();
        let number = callNumber.value;
        number = number.slice(0, -1);
        callNumber.value = number;

        if (!callNumber.value) {
            backspaceBtn.classList.add('d-none');
        }
    };

    var onCallBtnClick = function () {
        const card = colTopCenter.querySelector('.card');
        if (callNumber.value) {
            colTopCenter.replaceChild(callInfoTemplate, card);
            $('#call_modal h2').text(callNumber.value);
            //==========Звонок==========
            $.ajax({
                type: "POST",
                url: "/Appeal/_CallPhone",
                data: { PhoneNumber: callNumber.value, NumberAppeal: d },
                success: function (result) {
                    //var socket = new WebSocket("ws://192.168.35.190:8886");
                    console.log('звонок' + callNumber.value);
                    var socket = new WebSocket("wss://localhost:8889");
                    socket.onopen = function () {
                        socket.send(result);
                    };
                },
                error: function (event) {
                    $.Notification.notify('error', 'right top', 'Ошибка!', 'Произошла ошибка при попытке осуществления звонка.');
                    console.log(event);
                }
            });
            //==========Инфо о пользователе================                  
            $.ajax({
                async: true,
                url: '/Call/UserInfo',
                data: { PhoneNumber: callNumber.value },
                type: 'POST',
                success: function (data) {
                    $('#call-user-info').html(data);
                }
            });
            //==========Новое обращение================
            $.ajax({
                async: true,
                url: '/Call/NewAppeal',
                data: { PhoneNumber: callNumber.value },
                type: 'POST',
                success: function (data) {
                    $('#call-new-appeal').html(data);
                }
            });
            //==========Комментарии по делу если звонят из обращения================                  
            $.ajax({
                async: true,
                url: '/Call/CallCommentts',
                data: { NumberAppeal: d },
                type: 'POST',
                success: function (data) {
                    $('#call-appeal-commentts').html(data);
                }
            });
            //==========Последние обращения================
            $.ajax({
                async: true,
                url: '/Call/LastAppeals',
                type: 'POST',
                data: { PhoneNumber: callNumber.value },
                success: function (data) {
                    $('#call-last-appeals').html(data);
                }
            });
            //==========Частые вопросы================
            $.ajax({
                async: true,
                url: '/Call/Questions',
                type: 'POST',
                success: function (data) {
                    $('#call-questions').html(data);
                }
            });
            //==========Консультация================
            $.ajax({
                async: true,
                url: '/Api/GetHashtag',
                type: 'POST',
                success: function (data) {
                    $('#call-consultations').html(data);
                }
            });

            callModalCols.forEach(function (it) {
                it.classList.add('show');
            });
        } else {
            colTopCenter.classList.add('animated', 'shake')
            setTimeout(function () {
                colTopCenter.classList.remove('animated', 'shake');
            }, 1200);
        }
    };
    //==========Опрос================
    $.ajax({
        async: false,
        url: '/Call/Survey',
        type: 'POST',
        success: function (data) {
            callModalTemplate.querySelector('#call-survey').innerHTML = data;
        }
    });

    dialpad.addEventListener('click', onDialpadClick);
    backspaceBtn.addEventListener('click', onBackspaceBtnClick);
    callNumber.addEventListener('input', onCallNumberInput);
    callBtn.addEventListener('click', onCallBtnClick);

    initPopovers();
    initQuiz(callModalTemplate);

    colTopCenter.appendChild(dialpadTemplate);
    callModalContent.appendChild(callModalTemplate);
    callModal.open();
};

// Открыть модальное окне при клике на кнопку Вызова в шапке
const callModalBtn = document.querySelector('#btnCallModal');
callModalBtn.addEventListener('click', onCallModalBtnClick);

//=========== Входящий звонок ============//
const incomingCallModalContent = document.querySelector('#incoming-call_modal .custom-modal-text');

var incomingCallModal = new Custombox.modal({
    content: {
        effect: 'push',
        animateFrom: 'left',
        animateTo: 'left',
        fullscreen: true,
        target: '#incoming-call_modal'
    }
});

var showIncomingCallAlert = function (e) {
    console.log(e);
    console.log(e.length);
    var n = "";
    if (e.length <= 12) {
        n = e;
    }
    console.log(n);
    //swal({
    //    imageUrl: '/Content/img/user_image.jpg',
    //  imageClass: 'rounded-circle',
    //  imageWidth: 100,
    //  imageHeight: 100,
    //  title: n,
    //  text: 'Входящий звонок',
    //  showCancelButton: true,
    //  confirmButtonText: '<i class="md md-call"></i>',
    //  confirmButtonClass: 'btn btn-success mr-3 waves-effect waves-light',
    //  cancelButtonText: '<i class="md md-call-end"></i>',
    //  cancelButtonClass: 'btn btn-danger waves-effect waves-light',
    //}).then((result) => {
    //    if (result) {          
    let incomingCallModalTemplate = document.querySelector('#call-modal-template').content.cloneNode(true);
    let colTopCenter = incomingCallModalTemplate.querySelector('.top-center');
    let callInfoTemplate = document.querySelector('#call-info-template').content.cloneNode(true);
    let callModalCols = incomingCallModalTemplate.querySelectorAll('.call-modal-col');

    var callNumber = n; //$('.swal2-title').text();
    console.log(callNumber);
    callInfoTemplate.querySelector('#call-phone-number').innerText = callNumber;

    //==========Инфо о пользователе================                  
    $.ajax({
        async: true,
        url: '/Call/UserInfo',
        data: { PhoneNumber: callNumber },
        type: 'POST',
        beforeSend: function () {
            $('#call-user-info').html("... ждите");
        },
        success: function (data) {
            $('#call-user-info').html(data);
        }
    });
    //==========Новое обращение================
    $.ajax({
        async: true,
        url: '/Call/NewAppeal',
        data: { PhoneNumber: callNumber },
        type: 'POST',
        success: function (data) {
            $('#call-new-appeal').html(data);
        }
    });
    //==========Последние обращения================
    $.ajax({
        async: true,
        url: '/Call/LastAppeals',
        type: 'POST',
        data: { PhoneNumber: callNumber },
        success: function (data) {
            $('#call-last-appeals').html(data);
        }
    });
    //==========Частые вопросы================
    $.ajax({
        async: true,
        url: '/Call/Questions',
        type: 'POST',
        success: function (data) {
            $('#call-questions').html(data);
        }
    });
    //==========Консультация================
    $.ajax({
        async: true,
        url: '/Api/GetHashtag',
        type: 'POST',
        success: function (data) {
            $('#call-consultations').html(data);
        }
    });

    //==========Опрос================
    $.ajax({
        async: false,
        url: '/Call/Survey',
        type: 'POST',
        success: function (data) {
            incomingCallModalTemplate.querySelector('#call-survey').innerHTML = data;
        }
    });

    $.unblockUI();
    initPopovers();
    initQuiz(incomingCallModalTemplate);
    colTopCenter.appendChild(callInfoTemplate);
    callModalCols.forEach(function (it) {
        it.classList.add('show');
    });
    incomingCallModalContent.appendChild(incomingCallModalTemplate);
    incomingCallModal.open();
};

// Очищаем модальное окно
document.addEventListener('custombox:overlay:close', function (evt) {
    while (callModalContent.firstChild) {
        callModalContent.removeChild(callModalContent.firstChild);
    }
    while (incomingCallModalContent.firstChild) {
        incomingCallModalContent.removeChild(incomingCallModalContent.firstChild);
    }
});

$(document).ready(function () {
    $('#btnCallAlert').click(function () { showIncomingCallAlert('89999999999') });
    //$('footer').hover(showIncomingCallAlert);
});

$(document).on('click', '[data-pic]', function (event) {
    event.preventDefault();
    event.stopPropagation();
    var button = $(this);
    $('button.current').removeClass('current');
    button.addClass('current');
    string_byte = null;
    var fileId = button.data('pic');
    var options = button.data('picOption');
    var type = button.data('type');
    _type_ext = type.toLowerCase();

    $.ajax({
        url: '../Common/GetFile',
        type: 'POST',
        async: false,
        data: { id: fileId },
        success: function (data) {
            string_byte = data;
            _type_ext = type.toLowerCase();
            $.fancybox.close();
            if (type.toLowerCase() == '.pdf') {
                $.fancybox.open(
                    {
                        type: 'iframe',
                        src: 'data:application/pdf;base64,' + data,
                        opts: {
                            caption: "<div class='fs-12'>" +
                                "<strong>Имя</strong>: " + options['name'] + "</br>" +
                                "<strong>Размер</strong>: " + Math.round(options['size'] / 1042).toFixed(0) + " кБ</br>" +
                                "<strong>Тип</strong>: " + options['type'] + "</br>" +
                                "<strong>Сотрудник</strong>: " + options['fio'] + "</br>" +
                                "<strong>Дата</strong>: " + options['date'] + "</br></div>"
                        }
                    });
            }
            else {
                $.fancybox.open(
                    {
                        src: 'data:image/png;base64,' + data,
                        opts: {
                            caption: "<div class='fs-12'>" +
                                "<strong>Имя</strong>: " + options['name'] + "</br>" +
                                "<strong>Размер</strong>: " + Math.round(options['size'] / 1042).toFixed(0) + " кБ</br>" +
                                "<strong>Тип</strong>: " + options['type'] + "</br>" +
                                "<strong>Сотрудник</strong>: " + options['fio'] + "</br>" +
                                "<strong>Дата</strong>: " + options['date'] + "</br></div>"
                        }
                    });
            }
        }
    });
});

// #region Сокет для Jitsi
var jitsiObj = "";
function getJitsiData() {
    $.ajax({
        url: "/Home/GetIncomingCallData",
        type: "POST",
        beforeSend: function () {
            //console.log('начало вызова');
        },
        success: function (data) {
            jitsiObj = data;
        },
        complete: function () {

        }
    });
}

//setupWebSocket();
function setupWebSocket() {

    ws = new WebSocket('wss://localhost:7248');
    console.log(ws)
    ws.onerror = function (event) { console.log("error socket", event); };
    //ws.onerror = ev => { ev.stopImmediatePropagation(); ev.stopPropagation(); ev.preventDefault(); return false }
    ws.onopen = function (event) { console.log("open socket"); };
    ws.onmessage = function (event) {
        let jsonObj = JSON.parse(event.data)
        $.ajax({
            url: "/Home/GetIncomingCallData",
            type: "POST",
            data: jsonObj,
            beforeSend: function () {
            },
            success: function (data) {
                ws.send(data);
            },
            complete: function () {

            }
        });
    };
    ws.onclose = function () {
        setTimeout(setupWebSocket, 300);
    };
}
//#endregion

//#region CallBack
var CallBack = true;
if (CallBack) {
    CallBackCountRefresh();
    function CallBackCountRefresh() {
        try {
            $.ajax({
                url: "/CallBack/GetCallBackCount",
                type: "GET",
                success: function (data) {
                    let count = data;
                    if (count > 0) {
                        $('#callBackCount').html(count > 99 ? "99+" : count).removeClass('d-none');
                    } else {
                        //$('#callBackCount').addClass("d-none");
                    }
                },
                complete: function () {
                    setTimeout(CallBackCountRefresh, 120000);
                }
            });
        } catch (ex) {
            console.log('catch', ex);
            setTimeout(CallBackCountRefresh, 5000);
        }
    }
    function CallBackRefresh() {
        $.ajax({
            url: "/CallBack/GetCallBackView",
            type: "POST",
            beforeSend: function () {
                $('#newModal').html('<div style="-webkit-box-pack: center;justify-content : center;-webkit-box-align: center;align-items : center;display: flex;height: 100%; margin-right: -5px;"><i class="fa fa-spin fa-spinner fa-2x" ></i></div>');
            },
            success: function (data) {
                $('#newModal').modal('show').html(data);
            },
            complete: function () {

            }
        });
    }
}

$(document).on('click', '.finished-call', function () {
    var $data = $('#call-questions form').serialize();
    var phone = $('#call-phone-number').text();
    console.log(phone);
    $.ajax({
        async: true,
        url: 'Call/SaveQuestions',
        type: 'POST',
        data: $data + "&PhoneNumber=" + phone,
        success: function (data) {

        }
    });
    //$(this).addClass("d-none");
    Custombox.modal.close();
});
$(document).on('click', '#call-survey-save', function () {
    var $data = $('#call-survey form').serialize();
    console.log($data);
    var phone = $('#call-phone-number').text();
    $.ajax({
        async: true,
        url: 'Call/SaveSurveyAnswers',
        type: 'POST',
        data: $data + "&phoneNumber=" + phone,
        success: function (data) {

        }
    });
});
//#endregion