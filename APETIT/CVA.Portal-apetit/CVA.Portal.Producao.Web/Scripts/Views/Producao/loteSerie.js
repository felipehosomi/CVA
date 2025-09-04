var table;

function loteSerieModal(docEntry, lineNum, codMP, descMP, lote, serie, deposito) {
    var qtde = $("#tbxQtde" + lineNum).val();
    if (qtde == "") {
        qtde = "0";
    }

    var divFilho = document.getElementById("divLoteFilho");
    var divPai = document.getElementById("divLotePai");
    divPai.removeChild(divFilho);
    $.ajax
    ({
        url: '/Producao/ModalLoteSerie',
        data: {
            "docEntry": docEntry,
            "lineNum": lineNum,
            "codMP": codMP,
            "descMP": descMP,
            "qtde": qtde,
            "lote": lote,
            "serie": serie,
            "deposito": deposito,
        },
        success: function (retorno) {
            $("#divLotePai").html(retorno);

            $('#tblLoteSerie thead tr').clone(true).appendTo('#tblLoteSerie thead');
            $('#tblLoteSerie thead tr:eq(1) th').each(function (i) {
                var title = $(this).text();
                $(this).html('<input type="text" placeholder="Buscar ' + title + '" />');

                $('input', this).on('keyup change', function () {
                    if (table.column(i).search() !== this.value) {
                        table
                            .column(i)
                            .search(this.value)
                            .draw();
                    }
                });
            });

            table = $('#tblLoteSerie').DataTable({
                info: false,
                paging: false,
                searching: true,
                ordering: true,
                "oLanguage": {
                    "sSearch": "Buscar",
                    "sEmptyTable": "Nenhum registro encontrado"
                },
                orderCellsTop: true,
                fixedHeader: true
            });
        }
    });
}

function loteSerieSalvar() {
    table
 .search('')
 .columns().search('')
 .draw();

    $.ajax
    ({
        url: '/Producao/LoteSerie',
        type: 'post',
        data: $("#divLotePai :input").serialize(),
        success: function (message) {
            if (message == null || message == "") {
                $('#modal-lote').modal('toggle');
                $('.modal-backdrop').css("visibility", "hidden");
            }
            else {
                new PNotify({
                    title: 'Alerta',
                    text: "Erro ao salvar: " + message,
                    type: 'error',
                    styling: 'bootstrap3'
                });
            }
        }
    });
}

function loteCalcular() {
    $.ajax
    ({
        url: '/Producao/CalculaLote',
        type: 'post',
        data: $("#divLotePai :input").serialize(),
        success: function (retorno) {
            $('#tbxSelecionado').val(retorno);
        }
    });
}

function serieCalcular() {
    var selecionados = $('.cbxSerie').filter(':checked').length;
    $('#tbxSelecionado').val(selecionados);
}

function loteSerieFechar() {
    $('#modal-lote').modal('hide');
}