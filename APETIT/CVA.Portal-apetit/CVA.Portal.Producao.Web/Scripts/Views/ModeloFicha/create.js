function adicionaLinha() {
    var linha = document.getElementById("tblFicha").getElementsByTagName("tr").length;
    var seq = (linha - 1);

    $.ajax
        ({
            url: '/ModeloFicha/ModeloFichaItem',
            data: { "linha": $("#tbxID" + seq).val() },
            success: function (data) {
                $("#tr-" + seq).after(data);
                // Altera o nome dos campos para funcionar o bind ao salvar
                $("#hdnCode" + linha).attr("name", "ItemList[" + seq + "].Code");
                $("#hdnCodFicha" + linha).attr("name", "ItemList[" + seq + "].CodFicha");
                $("#hdnDeleted" + linha).attr("name", "ItemList[" + seq + "].Deleted");

                $("#tbxID" + linha).attr("name", "ItemList[" + seq + "].ID");
                $("#ddlEspecificacao" + linha).attr("name", "ItemList[" + seq + "].CodEspec");
                $("#tbxVlrNominal" + linha).attr("name", "ItemList[" + seq + "].VlrNominal");
                $("#tbxPadraoDe" + linha).attr("name", "ItemList[" + seq + "].PadraoDe");
                $("#tbxPadraoAte" + linha).attr("name", "ItemList[" + seq + "].PadraoAte");
                $("#tbxAnalise" + linha).attr("name", "ItemList[" + seq + "].Analise");
                $("#tbxObservacao" + linha).attr("name", "ItemList[" + seq + "].Observacao");
                $("#tbxMetodo" + linha).attr("name", "ItemList[" + seq + "].Metodo");
                $("#tbxAmostragem" + linha).attr("name", "ItemList[" + seq + "].Amostragem");
                $("#tbxLink" + linha).attr("name", "ItemList[" + seq + "].Link");
            }
        });
};

function removeLinha(linha) {
    $("#tr-" + linha).attr("style", "display:none");
    $("#hdnDeleted" + linha).val(1);
}

function setTipo(linha) {
    
    var codEpec = $("#ddlEspecificacao" + linha).val();
    if (codEpec != null && codEpec != "") {
        $.ajax
        ({
            url: '/ModeloFicha/GetTipoCampoEspecificacao',
            data: { "codEspec": codEpec },
            success: function (retorno) {
                if (retorno == "N") {
                    $("#tbxPadraoDe" + linha).attr("type", "number");
                    $("#tbxPadraoAte" + linha).attr("type", "number");
                    $("#tbxVlrNominal" + linha).attr("type", "number");
                    //$("#tbxVlrNominal" + linha).val("");
                } else if (retorno == "D") {
                    $("#tbxPadraoDe" + linha).attr("type", "date");
                    $("#tbxPadraoAte" + linha).attr("type", "date");
                    $("#tbxVlrNominal" + linha).attr("type", "date");
                    //$("#tbxVlrNominal" + linha).val("");
                } else {
                    $("#tbxPadraoDe" + linha).attr("type", "text");
                    $("#tbxPadraoAte" + linha).attr("type", "text");
                    $("#tbxVlrNominal" + linha).attr("type", "text");
                    //$("#tbxPadraoDe" + linha).val("");
                    //$("#tbxPadraoAte" + linha).val("");
                }
            }
        });
    }
}