$(document).ready(function () {
    $("#btnFiltro").click(function () {
        var data = $("#tbxData").val();
        if (data != null && data != "") {
            var tblOPs = document.getElementById("tblOPs");
            var divOPs = document.getElementById("divOPs");
            divOPs.removeChild(tblOPs);
            $.ajax
            ({
                url: '/Producao/TabelaOrdensProducao',
                data: { "data": data },
                success: function (retorno) {
                    $("#divOPs").html(retorno);
                }
            });
        }
    });

    $('input').keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            $("#btnFiltro").click();
        }
    });
});

function historico(docEntry) {
    var divFilho = document.getElementById("divLogFilho");
    var divPai = document.getElementById("divLogPai");
    divPai.removeChild(divFilho);
    $.ajax
    ({
        url: '/Producao/ModalHistorico',
        data: {
            "docEntry": docEntry,
        },
        success: function (retorno) {
            $("#divLogPai").html(retorno);
        }
    });
}
