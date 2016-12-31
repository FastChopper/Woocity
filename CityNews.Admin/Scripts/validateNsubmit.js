function vNs(selector, rules, messages, url, callback) {
    $(function () {
        $(selector).validate({
            rules: rules,
            messages: messages,
            submitHandler: function (form) {
                var ele = {};
                $(form).find('input[type!=radio][type!=checkbox],textarea').each(function () {
                    var elename = $(this).attr('name')
                    if (elename && elename != name) {
                        if ($(this).val() != '') {
                            ele[elename] = $(this).val();
                        }
                    }
                });

                $(form).find('input[type=radio]:checked').each(function () {
                    var elename = $(this).attr('name')
                    if (elename && elename != name) {
                        if ($(this).val() != '') {
                            ele[elename] = $(this).val();
                        }
                    }
                });

                $(form).find('input[type=checkbox]:checked').each(function () {
                    var elename = $(this).attr('name')
                    if (elename && elename != name) {
                        if ($(this).val() != '') {
                            if (ele[elename]) {
                                ele[elename] += ',' + $(this).val();
                            }
                            else {
                                ele[elename] = $(this).val();
                            }
                        }
                    }
                });

                $(form).find('select').each(function () {
                    var elename = $(this).attr('name')
                    if (elename && elename != name) {
                        var sel = $(this).find(':selected');
                        if (sel.length > 0) {
                            var selStr = '';
                            sel.each(function () {
                                if (selStr != '') {
                                    selStr += ',';
                                }
                                selStr += $(this).val();
                            })
                            ele[elename] = selStr;
                        }
                    }
                });
                
                
                $.post(url, ele, callback, 'json');
            },
            errorClass: 'text-danger'
            //errorElement: 'div'
        });
    });
}

