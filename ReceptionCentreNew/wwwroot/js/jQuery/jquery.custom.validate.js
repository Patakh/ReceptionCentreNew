$.validator.methods.range = function (value, element, param) {
    var globalizedValue = value.replace(",", ".");
    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
}

$.validator.methods.number = function (value, element) {
    element.value = value.replace(".", ",");
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
}
$.validator.addMethod('date',
            function (value, element) {
                var dateReg = /^\d{2}([./-])\d{2}\1\d{4}$/;
                var dateReg2 = /^(\d{2}).(\d{2}).(\d{4}) (\d{2}):(\d{2})$/;
                return this.optional(element) || value.match(dateReg) || value.match(dateReg2);
            },
            "");


$('.phone-number').inputmask({
    mask: '+7(999) 999-9999', clearIncomplete: true
});

$('.date').inputmask({
    mask: '99.99.9999', clearIncomplete: true
});


$('input[data-type=phone-number]').inputmask({
    mask: '+7(999) 999-9999', clearIncomplete: true
});

$('input[data-type=date]').inputmask({
    mask: '99.99.9999', clearIncomplete: true
});

//$('input[data-type=dateTime]').inputmask({
//    //mask: '99.99.9999 99:99', clearIncomplete: true
//});