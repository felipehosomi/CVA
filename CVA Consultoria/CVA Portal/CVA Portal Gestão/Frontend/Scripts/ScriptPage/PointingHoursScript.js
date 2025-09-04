$('.line-description').hide();
$('#panel-filtro').addClass('panel-toggled');
$('#span-filtro').removeClass().addClass('fa fa-angle-up');

$('body').on('click', 'tr.tr-principal td.clickable', function () {
    $(this).parent().next('tr.line-description').slideToggle();
});

$('body').on('click', 'tr .add-row', function () {
    var $tr = $(this).parent().parent();
    var $clone = $tr.clone().css('background-color', 'rgba(199, 199, 199, 0.34)');

    $tr.after($clone);
});

$('body').on('focusout', '.hora-entrada', function () {
    if ($(this).val().length <= 4) {
        $(this).val(ajustarInput($(this).val()));
    }

    var horaInicial = $(this).val();
    var horaFinal = $(this).parent().parent().find('.hora-saida').val();
    var intervalo = $(this).parent().parent().find('.hora-intervalo').val();

    $(this).parent().parent().find('.total-horas').val(diferencaHoras(horaInicial, horaFinal, intervalo));
});

$('body').on('focusout', '.hora-intervalo', function () {
    if ($(this).val().length <= 4) {
        $(this).val(ajustarInput($(this).val()));
    }

    var horaInicial = $(this).parent().parent().find('.hora-entrada').val();
    var horaFinal = $(this).parent().parent().find('.hora-saida').val();
    var intervalo = $(this).val();

    $(this).parent().parent().find('.total-horas').val(diferencaHoras(horaInicial, horaFinal, intervalo));
});

$('body').on('focusout', '.hora-saida', function () {
    if ($(this).val().length <= 4) {
        $(this).val(ajustarInput($(this).val()));
    }

    var horaInicial = $(this).parent().parent().find('.hora-entrada').val();
    var horaFinal = $(this).val();
    var intervalo = $(this).parent().parent().find('.hora-intervalo').val();

    $(this).parent().parent().find('.total-horas').val(diferencaHoras(horaInicial, horaFinal, intervalo));
});

function diferencaHoras(horaInicial, horaFinal, intervalo) {
    try {
        if (!CompareHours(horaInicial, horaFinal)) {
            aux = horaFinal;
            horaFinal = horaInicial;
            horaInicial = aux;
        }

        hIni = horaInicial.split(':');
        hFim = horaFinal.split(':');
        var interval = intervalo.split(':');

        horasTotal = parseInt(hFim[0], 10) - parseInt(hIni[0], 10) - parseInt(interval[0], 10);
        minutosTotal = parseInt(hFim[1], 10) - parseInt(hIni[1], 10) - parseInt(interval[1], 10);

        if (minutosTotal < 0) {
            minutosTotal += 60;
            horasTotal -= 1;
        }
        horaFinal = completaZeroEsquerda(horasTotal) + ":" + completaZeroEsquerda(minutosTotal);
        if (horaFinal == "NaN:NaN")
            return '00:00';
        return horaFinal;
    } catch (e) {
        return '00:00';
    }
}

function CompareHours(horaInicial, horaFinal) {
    horaIni = horaInicial.split(':');
    horaFim = horaFinal.split(':');

    hIni = parseInt(horaIni[0], 10);
    hFim = parseInt(horaFim[0], 10);
    if (hIni != hFim)
        return hIni < hFim;

    mIni = parseInt(horaIni[1], 10);
    mFim = parseInt(horaFim[1], 10);
    if (mIni != mFim)
        return mIni < mFim;
}

function preencheuHoraCompleta(horario) {
    var hora = horario.replace(/[^0-9:]/g, '');
    return hora.length == 5;
}

function isHoraValida(horario) {
    var regex = new RegExp("^([0-1][0-9]|[2][0-3]):([0-5][0-9])$");
    return regex.exec(horario) != null;
}

function possuiValor(valor) {
    return valor != undefined && valor != '';
}

function completaZeroEsquerda(numero) {
    return (numero < 10 ? "0" + numero : numero);
}

function ajustarInput(str) {
    var adicionar = 4 - str.length;
    for (var i = 0; i < adicionar; i++)
        str = '0' + str;

    return str = str.slice(0, 2) + ':' + str.slice(-2);
}