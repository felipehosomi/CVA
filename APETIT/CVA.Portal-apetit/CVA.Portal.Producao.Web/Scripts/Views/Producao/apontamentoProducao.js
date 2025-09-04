function apontamentoModal(nrOP, codEtapa) {
    var divFilho = document.getElementById("divEPFilho");
    var divPai = document.getElementById("divEPPai");
    divPai.removeChild(divFilho);
    $.ajax
    ({
        url: '/Producao/ModalEstruturaProducao',
        data: { "nrOP": nrOP, "codEtapa": codEtapa },
        success: function (retorno) {
            $("#divEPPai").html(retorno);
        }
    });
}

function calcularQtdeSaida() {
    if (!$('#tbxQtdeProd').is('[readonly]')) {
        var qtdeOriginal = parseFloat($("#hdnQtdeOP").val());
        var qtdeProd = parseFloat($("#tbxQtdeProd").val());

        if (qtdeProd >= qtdeOriginal) {
            $("#tbxQtdeProd").val(qtdeOriginal);
            qtdeProd = qtdeOriginal;
            $("#cbxConcluir").prop("checked", true);
            new PNotify({
                delay: 5000,
                title: 'Alerta',
                text: "Quantidade não pode ser superior a quantidade liberada " + qtdeOriginal,
                type: 'error',
                styling: 'bootstrap3'
            });
        }
        else {
            $("#cbxConcluir").prop("checked", false);
        }

        var qtdeLinhas = $("#hdnQtdeLinhas").val();
        for (var i = 0; i < qtdeLinhas; i++) {
            var qtdeBase = $("#hdnQtdeBase" + i).val();
            $("#tbxQtde" + i).val(qtdeProd * qtdeBase);
        }
    }
}

function apontamentoSalvar() {
    $.ajax
   ({
       url: '/FichaProduto/Obrigatorio',
       data: {
           "docEntryOP": $("#hdnOP").val(),
           "codItem": $("#hdnItem").val(),
           "codEtapa": $("#hdnEtapa").val(),
           "quantidade": parseFloat($("#tbxQtdeProd").val()),
       },
       success: function (retorno) {
           if (retorno == "0") {
               $("#btnApontamento").button('loading');
               $.ajax
               ({
                   url: '/Producao/Apontamento',
                   type: 'post',
                   data: $("#divEPPai :input").serialize(),
                   success: function (message) {
                       if (message == null || message == "") {
                           location.reload();
                       }
                       else {
                           $("#btnApontamento").button('reset');
                           new PNotify({
                               delay: 5000,
                               title: 'Alerta',
                               text: "Erro ao efetuar apontamento: " + message,
                               type: 'error',
                               styling: 'bootstrap3'
                           });
                       }
                   }
               });
           } else {
               fichaInspecaoModal();
               $("#modal-ficha").modal("show");
           }
       }
   });
}