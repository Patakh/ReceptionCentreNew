function removeData(action, param, func) {
    swal({
        title: "Вы уверены?",
        text: "Запись можно будет восстановить!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Да, удалить!",
        cancelButtonText: "Нет, отмена"
    }).then(function () {
        $.ajax({
            type: 'POST',
            url: action,
            data: param,
            async: false,
            success: function (data) {
                swal("Готово!", "Запись успешно удалена.", "success");
                $('#' + func).html(data);
            },
            error: function (data, textStatus, jqXHR) {
                //$.Notification.autoHideNotify('error', 'top right', 'Ошибка', data.responseText);
                swal("Ошибка!", "Не удается удалить запись.", "error");
                console.log(data.responseText);
            }
        });
    });
};

function reAttachData(action, param, _div) {
    swal({
        title: "Вы уверены?",
        text: "Запись будет откреплена от обращения!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Да, открепить!",
        cancelButtonText: "Нет, отмена"
    }).then(function () {
        $.ajax({
            type: 'POST',
            url: action,
            data: param,
            async: false,
            success: function (data) {
                swal("Готово!", "Запись успешно откреплена.", "success");
                $('#' + _div).html(data);
            },
            error: function (data, textStatus, jqXHR) {
                swal("Ошибка!", "Не удается открепить запись.", "error");
                console.log(data.responseText);
            }
        });
    });
};

function removeRecovery(paramObject) {
    var onFailure = function (data) { };
    var onSuccess = function (data) { };
    if (paramObject.onFailure != undefined)
        onFailure = paramObject.onFailure;
    if (paramObject.onSuccess != undefined)
        onSuccess = paramObject.onSuccess;

    $.ajax({
        type: 'POST',
        url: paramObject.url,
        data: paramObject.params,
        success: function (data) {
            swal({
                type: 'success',
                title: 'Готово!',
                html: 'Запись успешно восстановлена.'
            });
            onSuccess(data);
        },
        error: function (data, textStatus) {
            onFailure(data);
            $.Notification.autoHideNotify('error', 'top right', 'Ошибка', data.responseText);
        }
    });
}

function removeWithComment(paramObject) {
    var onFailure = function(data) {};
    var onSuccess = function (data) { };
    if (paramObject.onFailure != undefined)
        onFailure = paramObject.onFailure;
    if (paramObject.onSuccess != undefined)
        onSuccess = paramObject.onSuccess;

    swal({
        title: 'Причина удаления записи',
        input: 'textarea',
        showCancelButton: true,
        confirmButtonText: 'Удалить',
        cancelButtonText: 'Отмена',
        showLoaderOnConfirm: true,
        confirmButtonClass: 'btn btn-danger',
        cancelButtonClass: 'btn m-l-10',
        preConfirm: function (comment) {
            paramObject.params['comment'] = comment;
            return new Promise(function (resolve, reject) {
                if (comment === '') {
                    reject('Введите причину удаления.');
                } else {
                    $.ajax({
                        type: 'POST',
                        url: paramObject.url,
                        data: paramObject.params,
                        async: false,
                        success: function (data) {
                            onSuccess(data);
                            resolve();
                        },
                        error: function (data, textStatus) {
                            onFailure(data);
                            $.Notification.autoHideNotify('error', 'top right', 'Ошибка', data.responseText);
                            reject('Не удается удалить запись.');
                        }
                    });
                }
            });
        },
        allowOutsideClick: false
    }).then(function (comment) {
        swal({
            type: 'success',
            title: 'Готово!',
            html: 'Запись успешно удалена.'
        });
    });
}

function successAdd() {
    $.Notification.autoHideNotify('success', 'top right', 'Готово!', 'Запись успешно добавлена');
};

function successSave() { 
    $.Notification.autoHideNotify('success', 'top right', 'Готово!', 'Запись успешно сохранена');
};

function successEdit() {
    $.Notification.autoHideNotify('success', 'top right', 'Готово!', 'Запись успешно изменена'); 
};

function successDelete() {
    $.Notification.autoHideNotify('success', 'top right', 'Готово!', 'Запись успешно удалена');
};

function errorAction() {
    $.Notification.autoHideNotify('error', 'top right', 'Ошибка!', 'Повторите попытку');
}

function printAjax(url, param) {
    $('#loadingAjax').show();
    $.ajax({
        type: 'POST',
        url: url,
        data: param,
        success: function (data) {
            $(data).printThis({
                debug: false,
                importCSS: false,
                importStyle: false,
                printContainer: true,
                loadCSS: "",
                pageTitle: "",
                removeInline: false,
                printDelay: 333,
                header: null,
                formValues: true
            });
        }
    });
}

