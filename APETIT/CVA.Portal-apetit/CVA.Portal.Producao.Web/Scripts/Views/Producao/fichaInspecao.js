function fichaInspecaoModal() {
    $.ajax
        ({
            url: '/FichaInspecao/GetModal',
            type: 'post',
            data: $("#divEPPai :input").serialize(),
            success: function (retorno) {
                if (retorno != null && retorno != "") {
                    var divFilho = document.getElementById("divQAFilho");
                    var divPai = document.getElementById("divQAPai");
                    divPai.removeChild(divFilho);
                    $("#divQAPai").html(retorno);
                }
            }
        });
}

function fichaInspecaoSalvar() {
    $("#btnQASalvar").button('loading');

    var permiteParcial;
    $.ajax
        ({
            url: '/Config/PermiteParcial',
            type: 'get',
            success: function (retorno) {
                permiteParcial = parseInt(retorno);
                $.ajax
                    ({
                        url: '/FichaInspecao/CreateModal',
                        type: 'post',
                        data: $("#divQAPai :input").serialize(),
                        success: function (message) {
                            if (message == null || message == "") {
                                if (permiteParcial == 0 || $("#hdnQtdeSeq").val() >= $("#hdnSeqParcial").val()) {
                                    fichaInspecaoFechar();
                                }
                                else {
                                    new PNotify({
                                        delay: 5000,
                                        title: 'Sucesso',
                                        text: "Apontamento " + $("#hdnSeqParcial").val() + ' de ' + $("#hdnQtdeSeq").val() + ' efetuado com sucesso!',
                                        type: 'success',
                                        styling: 'bootstrap3'
                                    });

                                    $.ajax
                                        ({
                                            url: '/FichaInspecao/GoNext',
                                            type: 'post',
                                            data: $("#divQAPai :input").serialize(),
                                            success: function (retorno) {
                                                var divFilho = document.getElementById("divQAFilho");
                                                var divPai = document.getElementById("divQAPai");
                                                divPai.removeChild(divFilho);
                                                $("#divQAPai").html(retorno);
                                            }
                                        });
                                }
                            }
                            else {
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
            }
        });
    $("#btnQASalvar").button('reset');
}


function fichaInspecaoFechar() {
    $('#modal-ficha').modal('hide');

}