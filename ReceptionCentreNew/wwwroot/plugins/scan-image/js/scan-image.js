var fileSend = [];
var params;
var cid;
var onSuccessFunc;
var urlDeleteImage = "";
var urlSaveImage = "";
var urlGetFtp = "";
var index = 1;
var indexFileName = 1;
var url = "http://localhost:9624/signalr";
var string_byte;
var uploadFTPResultLoad = false;

$(document).on('click', '.add-picture-block', function () {
    $('.add-picture-block').html('<div class="add-scan-picture loading-scan"><i class="ion-load-a ion-loading-a "></i><br /><span>Идет сканирование...</span></div>');
    scan();
});

$(document).on('click', '.modal-footer #saveScan', function () {
    $('#saveScan').prop('disabled', true);

    $.connection.scanAppHub.url = url;
    var scanHub = $.connection.scanHub;
    var errorUpload = [];
    var $res = $('.case-services-documents-list2').find('.r-tabs-state-active');

    $.ajax({
        url: urlGetFtp,
        type: "GET",
        async: false,
        success: function (ftpModelData) {
            // Start the connection.
            scanHub.client.closeConnection = function () {
                $.connection.scanAppHub.stop();
            }

            if (!uploadFTPResultLoad) {
                console.log('upload');
                scanHub.client.uploadFTPResult = function (result, fileId, index) {
                    if (result) {
                        $('.picture-block[data-index="' + index + '"] .scan-picture').prepend('<div class="success-result"><i class="ion-checkmark-round"></i></div>');
                        delete fileSend[index];
                    } else {
                        $.ajax({
                            url: urlDeleteImage,
                            type: "POST",
                            async: false,
                            data: { serviceFileId: fileId },
                            success: function (data) {
                                errorUpload.push(index);
                                $('.picture-block[data-index="' + index + '"] .scan-picture').prepend('<div class="error-result"><i class="ion-alert-circled"></i></div>');
                                delete fileSend[index];
                            },
                            error: function (data) {
                            }
                        });

                    }
                    if ((index + 1) == fileSend.length) {
                        $.connection.scanAppHub.stop();
                        onSuccessFunc();
                        if (errorUpload.length == 0) {
                            $('#modal_container').removeClass('is-active');
                            swal({
                                title: "Готово!",
                                text: "Документы успешно добавились",
                                type: "success",
                                confirmButtonColor: "#73c482",
                                confirmButtonText: "Закрыть",
                            });
                        }
                        else {
                            $('.modal-card-body .error-message').html('Не удалось сохранить ' + errorUpload.length + ' файл(ов).');
                        }

                    }
                }
                uploadFTPResultLoad = true;
            }
            

            $.connection.scanAppHub.start().done(function () {
                fileSend.forEach(function (element, index, array) {
                    $.ajax({
                        url: urlSaveImage,
                        type: "POST",
                        async: false,
                        data: { data_services_table_id: params.data_services_table_id, data_services_info_id: params.data_services_info_id, file_name: $('.picture-block[data-index="' + index + '"] input.scan-picture-name').val(), file_size: element.length },
                        success: function (data) {
                            scanHub.server.uploadFTP(ftpModelData.ftpServer, ftpModelData.ftpLogin, ftpModelData.ftpPass, ftpModelData.ftpFolder, params.data_services_info_id, data.id, index, element);
                        },
                        error: function (data) {
                            errorUpload.push(index);
                            $('.picture-block[data-index="' + index + '"] .scan-picture').prepend('<div class="error-result"><i class="ion-alert-circled"></i></div>');
                            delete fileSend[index];
                        }
                    });
                });
            }).fail(function () {
                $('span.error-connect-scanapp').remove();
                document.getElementById('pictureBlock').insertAdjacentHTML('beforeBegin', '<span class="error-connect-scanapp">Не удалось соединиться с программой для сканирования.</span>');
                $('#saveScan').prop('disabled', false);
            });
        }
    });
});

function scan() {
    $('#saveScan').prop('disabled', true);
    $.connection.scanAppHub.url = url;
    var scanHub = $.connection.scanHub;
    $('.error-connect-scanapp').remove();
    scanHub.client.closeConnection = function () {
        $.connection.scanAppHub.stop();
        $('.loading-scan').parent().html('<div class="add-scan-picture"><i class="ion-ios-plus-outline"></i></div>');
    }
    $.connection.scanAppHub.reconnecting(function () {
        $.connection.scanAppHub.stop();
        $('.loading-scan').parent().html('<div class="add-scan-picture"><i class="ion-ios-plus-outline"></i></div>');
    });
    scanHub.client.scanImage = function (file) {
        $('.add-picture-block').remove();
        var picBlock = document.createElement("div");
        var scanPicDiv = document.createElement("div");
        var removePicI = document.createElement("i");
        removePicI.className = "ion-close-circled";
        picBlock.setAttribute("data-index", index);
        picBlock.className = "picture-block";
        scanPicDiv.className = "scan-picture";
        var picImg = document.createElement("img");
        var inputName = document.createElement("input");
        inputName.className = "scan-picture-name";
        inputName.setAttribute("type", "text");
        if ($('#docTemplate').val() == "") {
            inputName.value = 'scan_' + indexFileName++;
        }
        else {
            inputName.value = $('#docTemplate option:selected').text() + "_" + indexFileName++;
        }

        picImg.setAttribute("src", "data:image/jpeg;base64," + file);

        scanPicDiv.appendChild(picImg);
        picBlock.appendChild(removePicI);
        picBlock.appendChild(inputName);
        picBlock.appendChild(scanPicDiv);
        document.getElementById('pictureBlock').appendChild(picBlock);

        document.getElementById('pictureBlock').insertAdjacentHTML('beforeend', '<div class="add-picture-block"><div class="add-scan-picture"><i class="ion-ios7-plus-outline"></i></div></div>');
        fileSend[index++] = file;
        $('#saveScan').prop('disabled', false);

    };

    // Start the connection.
    $.connection.scanAppHub.start().done(function () {
        scanHub.server.scan('scan', $('#message').val());
    }).fail(function () {
        $('.loading-scan').parent().html('<div class="add-scan-picture"><i class="ion-ios7-plus-outline"></i></div>');
        document.getElementById('pictureBlock').insertAdjacentHTML('beforeBegin', '<span class="error-connect-scanapp">Не удалось соединиться с программой для сканирования.</span>');
    });
}

function createModal(urlSaveImageParam, urlDeleteImageParam, urlGetFtpParam, paramsParam, onSuccessFuncParam) {
    var template = '<div class="row m-b-20">' +
                '<div class="col-md-12">' +
                      '<select id="docTemplate" class="select" name="docTemplate">' +
                          '<option value>Выберите шаблон документа</option>' +
                          '<option value="1">Заявление</option>' +
                          '<option value="2">ИНН</option>' +
                          '<option value="3">Кадастровый_паспорт</option>' +
                          '<option value="4">Паспорт</option>' +
                          '<option value="5">Пенсионное_удостоверение</option>' +
                          '<option value="6">Регистрационное_свидетельство</option>' +
                          '<option value="7">Свидетельство_о_рождении</option>' +
                          '<option value="8">Свидетельство_о_браке</option>' +
                          '<option value="9">СНИЛС</option>' +
                          '<option value="10">Справка</option>' +
                          '<option value="11">Технический_паспорт</option>' +
                          '<option value="12">Трудовая_книжка</option>' +
                          '<option value="13">Квитанция</option>' +
                          '<option value="14">Домовая_книга</option>' +
                          '<option value="15">Начисление_по_ЖКУ</option>' +
                          '<option value="16">Квитанция_об_уплате</option>' +
                          '<option value="17">Доверенность</option>' +
                          '<option value="18">Архивная_выписка</option>' +
                      '</select>' +
                  '</div>' +
              '</div>';
    fileSend = [];
    params = paramsParam;
    onSuccessFunc = onSuccessFuncParam;
    urlGetFtp = urlGetFtpParam;
    urlDeleteImage = urlDeleteImageParam;
    urlSaveImage = urlSaveImageParam;
    var modalContainer = document.getElementById('myModal');
    modalContainer.innerHTML = '<div class="modal-dialog modal-lg">' +
                                    '<div class="modal-content">' +
                                        '<div class="modal-header">' +
                                            '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>' +
                                            '<h4 class="modal-title" id="myLargeModalLabel">Сканирование</h4>' +
                                        '</div>' +
                                        '<div class="modal-body">' +
                                            template +
                                            '<div class="row">' +
                                                '<div class="col-md-12" id="pictureBlock">' +
                                                    '<div class="add-picture-block">' +
                                                        '<div class="add-scan-picture loading-scan">' +
                                                            '<i class="ion-load-a ion-loading-a "></i><br />' +
                                                            '<span>Идет сканирование...</span>' +
                                                        '</div>' +
                                                    '</div> ' +
                                                '</div> ' +
                                            '</div>' +
                                            '<span class="error-message"></span>' +
                                        '</div>' +
                                        '<div class="modal-footer">' +
                                            '<button type="button" class="btn btn-cancel waves-effect waves-light" data-dismiss="modal">' +
                                                '<i class="fa fa-times m-r-5"></i> Закрыть' +
                                            '</button>' +
                                            '<button type="button" id="saveScan" class="btn btn-success waves-effect waves-light"> Сохранить ' +
                                                '<i class="fa fa-check m-l-5"></i>' +
                                            '</button>' +
                                        '</div>' +
                                    '</div><!-- /.modal-content -->' +
                                '</div><!-- /.modal-dialog -->';
    $('#myModal').modal('show');
    $('select#docTemplate').niceSelect();
    scan();
}

$(document).on('click', '.picture-block > i', function () {
    delete fileSend[$(this).closest('.picture-block').attr('data-index')];
    $(this).closest('.picture-block').remove();
});

$(document).on('click', '.scan-picture', function () {
    $.fancybox.open({ href: $(this).find('img').attr('src'), openEffect: 'elastic', closeClick: true, wrapCSS: 'fancybox-custom' });
});

$(document).on('change', '#docTemplate', function () {
    if ($(this).val() != "") {
        $("input.scan-picture-name").each(function (i, e) { $(e).val($('#docTemplate option:selected').text() + "_" + (i + 1)) });
    }
});

function openImage(fileId, infoId, type, target, urlGetFtp) {
    $.connection.scanAppHub.url = url;
    var scanHub = $.connection.scanHub;
    $.ajax({
        url: urlGetFtp,
        type: "GET",
        async: false,
        success: function (ftpModelData) {
            scanHub.client.closeConnection = function () {
                $.connection.scanAppHub.stop();
            }
            scanHub.client.openFileFTPResult = function (result) {
                if (result != false) {
                    if (type.toLowerCase() == '.pdf') {
                        var iframe = $('<iframe>');
                        iframe.css('width', '100%');
                        iframe.css('height', '52em');
                        iframe.attr("src", "data:application/pdf;base64," + result);
                        $(target).html(iframe);
                    } else {
                        $.fancybox.open({ href: "data:image/jpeg;base64," + result, openEffect: 'elastic', closeClick: true, wrapCSS: 'fancybox-custom' });
                    }
                    string_byte = result;
                }
                else
                {
                    $(target + ' .modal-card-body').html('<div class="error-result"><i class="ion-alert-circled"></i><br /><span>Не удалось открыть документ.</span></div>');
                }
                $.connection.scanAppHub.stop();
            }

            $.connection.scanAppHub.start().done(function () {
                scanHub.server.openFileFtp(ftpModelData.ftpServer, ftpModelData.ftpLogin, ftpModelData.ftpPass, ftpModelData.ftpFolder, infoId, fileId, type.toLowerCase());
            }).fail(function () {
                $(target + ' .modal-card-body').html('<div class="error-result"><i class="ion-alert-circled"></i><br /><span>Не удалось открыть документ.</span></div>');
                console.log("Error open file");
            });
        }
    });
}
