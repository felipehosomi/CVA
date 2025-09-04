
$(document).ready(function () {
    SetDataTable();

    $(document).on('change', '#BPLIdCode', function () {
        callBPLID();
    });

    $(document).on('blur', '#apontamentoData', function () {
        callBPLID();
    });

    $(document).on('change', '#ContratoCode', function () {
        clearItens();
        RefreshServico();
    });

    $(document).on('change', '#ServicoCode', function () {
        RefreshErroSend();
    });

    /*TERCEIROS*/
    $(document).on('change', '#Add-BPCardCode', function () {
        GetTerceiroCliente('add');
    });

    $(document).on('change', '#Edit-BPCardCode', function () {
        GetTerceiroCliente('edit');
    });

    $(document).on('blur', '#QtyRef', function () {
        CalculatePercent('force');
    });
    /*TERCEIROS*/

    /*ITENS*/
    $(document).on('change', '#Add-InsumoCode', function () {
        ItemOnHand('changeAdd');
    });

    $(document).on('change', '#ItemCodeList', function () {
        ItemOnHand('change');
    });

    $(document).on('change', '#Add-Qty', function () {
        CalculateUsedOnHand('changeAdd');
    });
    /*ITENS*/

    $(document).resize(function () {
        table.columns.adjust();
    });

});

$(function () {
    // document ready
    SetChosen();
});

function RefreshErroSend() {
    clearBPs();
    AutoRemoveBtnTerceiros();
    RefreshContrTer();
}

function RefreshContrTer() {
    RefreshContratoQty();
    RefreshTerceiros(false);
}

function RefreshContrato() {

    var bplId = $("#BPLIdCode").val().trim();
    var bplIdDesc = $("#BPLIdCode option:selected").text();
    var apontamentoData = $("#apontamentoData").val().trim();

    if ((bplId !== null && bplId !== '') && (apontamentoData !== null && apontamentoData !== '')) {

        loadingOn();

        $.getJSON(
            "/Apontamento/CallOpenOrders?ordersBPLId=" + bplId + "&ordersDate=" + apontamentoData,
            {},
            function (data) {

                clearContrato();
                RefreshChosen();

                if (data.length === 0) {
                    new PNotify({
                        title: 'Alerta',
                        text: "Contrato não encontrado para a filial " + bplIdDesc,
                        type: 'error',
                        styling: 'bootstrap3'
                    });
                    loadingOff();
                    return;
                }
                
                $.each(data, function (index, selectList) {
                    $('#ContratoCode').append($("<option/>", {
                        value: selectList.Value,
                        text: selectList.Text
                    }));
                });

                RefreshChosen();
                loadingOff();
            });

    }
    else {
        clearAll();
    }
}

function RefreshCliente() {

    if ($('#BPLIdCode').find('option').length > 0) {

        var bplId = $("#BPLIdCode").val().trim();
        var bplIdDesc = $("#BPLIdCode option:selected").text();

        if (bplId !== null && bplId !== '') {

            loadingOn();

            $.getJSON(
                "/Apontamento/CallCliente?clienteBPLID=" + bplId,
                {},
                function (data) {
                    
                    clearCliente();
                    RefreshChosen();

                    if (data.length === 0) {
                        new PNotify({
                            title: 'Alerta',
                            text: "Não encontrado para o cliente para filial " + bplIdDesc,
                            type: 'error',
                            styling: 'bootstrap3'
                        });
                        loadingOff();
                        return;
                    }
                    else {
                        
                        $('#ClienteCode').val(data.CardCode);
                        $('#ClienteName').val(data.CardName);
                        $('#txtClienteName').val(data.CardCode + ' - ' + data.CardName);
                    }

                    RefreshChosen();
                    loadingOff();
                });
        }
        else {
            clearCliente();
            CalculatePercent('');
        }

    }
}

function RefreshTerceiros(alert) {

    if ($('#ContratoCode').find('option').length > 0) {

        
        var contratoId = $("#ContratoCode").val().trim();
        var servicoId = $("#ServicoCode").val().trim();
        
        if ((contratoId !== null && contratoId !== '') && (servicoId !== null && servicoId !== '')) {

            var bplId = $("#BPLIdCode").val().trim();
            var turnoId = $("#ContratoCode").val().trim();
            var apontamentoData = $("#apontamentoData").val().trim();

            loadingOn();

            clearBPSolo();

            $.getJSON(
                "/Apontamento/CallTerceiros?tBPLID=" + bplId + "&tData=" + apontamentoData + "&tTurno=" + turnoId + "&tServico=" + servicoId,
                {},
                function (data) {

                    clearBPSolo();
                    RefreshChosen();

                    if (data.Code !== null) {

                        $.each(data.CVA_APTO_TERCEIROS1Collection, function (index, list) {

                            var countBPList = $("#countBPList").val();

                            $("#dtTerceiros > tbody").append(
                                '<tr id="bp-' + countBPList + '">'
                                + '    <td>' + list.U_CARDCODE + '</td>'
                                + '    <td>' + list.U_CARDNAME + '</td>'
                                + '    <td>' + list.U_QTYAPT + '</td>'
                                + '    <td style="text-align: center">'
                                + '        <input id="BPs_' + countBPList + '__BPType" name="BPs[' + countBPList + '].BPType" type="hidden" value="0">'
                                + '        <input id="BPs_' + countBPList + '__BPCardCode" name="BPs[' + countBPList + '].BPCardCode" type="hidden" value="' + list.U_CARDCODE + '">'
                                + '        <input id="BPs_' + countBPList + '__BPCardName" name="BPs[' + countBPList + '].BPCardName" type="hidden" value="' + list.U_CARDNAME + '">'
                                + '        <input data-val="true" data-val-number="The field Quantidade must be a number." data-val-required="The Quantidade field is required." id="BPs_' + countBPList + '__BPQtyRefeicao" name="BPs[' + countBPList + '].BPQtyRefeicao" type="hidden" value="' + list.U_QTYAPT + '">'
                                + '        <input data-val="true" data-val-required="The Delete field is required." id="BPs_' + countBPList + '__Remove" name="BPs[' + countBPList + '].Remove" type="hidden" value="False">'
                                + '    </td>'
                                + '</tr>');

                            $("#countBPList").val(parseInt(countBPList) + 1);

                        });

                        if (data.U_QTYREF !== null && data.U_QTYREF > 0) {
                            $("#QtyRef").val(data.U_QTYREF);
                        }

                        $("#IdApontamento").val(data.Code);

                        if (alert === false) {
                            new PNotify({
                                title: 'Apontamento de terceiros',
                                text: 'Apontamento já foi realizado pelo usuário: ' + data.U_USERPORTAL,
                                type: 'success',
                                styling: 'bootstrap3'
                            });
                        }
                        
                    }

                    table.columns.adjust();

                    //$("#collapseContrato").collapse('show');
                    //RefreshContratoQty();

                    //
                    AutoRemoveBtnTerceiros();
                    //

                    RefreshChosen();
                    //loadingOff();

                    ///
                    RefreshContratoInfo(alert);
                });
        }
        else {
            clearServico();
            clearBPs();
        }

    }

}

function RefreshServico() {

    if ($('#ContratoCode').find('option').length > 0) {

        var ContratoCode = $("#ContratoCode").val().trim();
        var ContratoDesc = $("#ContratoCode option:selected").text();

        if (ContratoCode !== null && ContratoCode !== '') {

            loadingOn();

            $.getJSON(
                "/Apontamento/CallGetServico?servicoTurno=" + ContratoCode,
                {},
                function (data) {
                    
                    $('#ServicoCode').empty();
                    RefreshChosen();

                    if (data.length === 0) {
                        new PNotify({
                            title: 'Alerta',
                            text: "Não foi encontrado serviço para a ordem de serviço " + ContratoDesc,
                            type: 'error',
                            styling: 'bootstrap3'
                        });
                        loadingOff();
                        //callBPLID();                        
                        return;
                    }
                    else {

                        if (data !== null) {

                            $.each(data, function (index, selectList) {
                                $('#ServicoCode').append($("<option/>", {
                                    value: selectList.Value,
                                    text: selectList.Text
                                }));
                            });


                            $("#collapseFilter").collapse('hide');
                            $("#collapseTerceiros").collapse('show');
                        }

                        RefreshChosen();
                        loadingOff();
                    }
                });
        }
        else {
            $('#ServicoCode').empty();
            clearServico();
            RefreshChosen();
        }

    }
}

function RefreshContratoQty() {

    if ($('#ServicoCode').find('option').length > 0) {

        var servicoId = $("#ServicoCode").val().trim();

        if (servicoId !== null && servicoId !== '') {
            var bplId = $("#BPLIdCode").val().trim();
            var turnoId = $("#ContratoCode").val().trim();
            var apontamentoData = $("#apontamentoData").val().trim();
            var servicoIdDesc = $("#ServicoCode option:selected").text();
            
            loadingOn();

            $.getJSON(
                "/Apontamento/CallGetQtyAsync?qtySERVICO=" + servicoId + "&qtyTURNO=" + turnoId + "&date=" + apontamentoData + "&qtyBPLID=" + bplId,
                {},
                function (data) {

                    RefreshChosen();
                    if (data.length === 0) {
                        new PNotify({
                            title: 'Alerta',
                            text: "Não encontrado informações parar o serviço " + servicoIdDesc,
                            type: 'error',
                            styling: 'bootstrap3'
                        });
                        loadingOff();
                        return;
                    }
                    else {

                        if (data.Qty !== null && data.Qty > 0) {
                            
                            $("#QtyPlan").val(data.Qty);
                            CalculatePercent('force');
                        }
                    }

                    RefreshChosen();
                    loadingOff();
                });
        }
        else {
            clearServico();
        }
    }
}

function RefreshContratoInfo(alert) {
    
    if ($('#ContratoCode').find('option').length > 0) {

        var contratoId = $("#ContratoCode").val().trim();
        if ((contratoId !== null && contratoId !== '') && (servicoId !== null && servicoId !== '')) {

            var contratoIdDesc = $("#ContratoCode option:selected").text();
            var servicoId = $("#ServicoCode").val().trim();

            //loadingOn();

            $.getJSON(
                "/Apontamento/CallGetInfo?contratoId=" + contratoId + "&servicoId=" + servicoId,
                {},
                function (data) {

                    clearItens();
                    RefreshChosen();

                    if (data.length === 0) {
                        if (alert === false) {
                            new PNotify({
                                title: 'Alerta',
                                text: "Não encontrado os itens para o contrato " + contratoIdDesc,
                                type: 'error',
                                styling: 'bootstrap3'
                            });
                        }
                        
                        loadingOff();
                        return;
                    }
                    else {

                        var doc;
                        $.each(data, function (index, list) {

                            var countList = $("#countList").val();

                            if (doc === undefined) {
                                doc = list.DocEntry;
                                $("#dtInsumo > tbody").append(
                                    '<tr id="tr-' + doc + '-">'
                                    + '    <td colspan="11" style="border: 1px dashed #00000080;background-color: #cccccc75;">' + list.ProdName + ' <b>|</b> Ordem de Produção Nº - ' + list.DocNum + ' <b>|</b> Quantidade Planejada - ' + list.QtyPlanejadoOrdemProducao + '<button type="button" class="btn-primary pull-right" onclick="SubmitOrdem(' + doc + ')" style="width: 150px;"><span class="fa fa-plus fa-right"></span> Salvar Apontamento</button></td > '
                                    + '    <td style="text-align: center;border: 1px dashed #00000080;background-color: #cccccc75;"><button type="button" class="btn-dark pull-right" onclick="ClickModelAdd(' + doc + ')" style="width: 100px;"><span class="fa fa-cart-plus fa-right"></span> Adicionar</button></td > '
                                    + '</tr>');
                            }
                            else {
                                if (doc !== list.DocEntry) {
                                    doc = list.DocEntry;
                                    $("#dtInsumo > tbody").append(
                                        '<tr id="tr-' + doc + '--">'
                                        + '    <td colspan="11"></td > '
                                        + '</tr>');
                                    $("#dtInsumo > tbody").append(
                                        '<tr id="tr-' + doc + '-">'
                                        + '    <td colspan="11" style="border: 1px dashed #00000080;background-color: #cccccc75;">' + list.ProdName + ' <b>|</b> Ordem de Produção Nº - ' + list.DocNum + ' <b>|</b> Quantidade Planejada - ' + list.QtyPlanejadoOrdemProducao + '<button type="button" class="btn-primary pull-right" onclick="SubmitOrdem(' + doc + ')" style="width: 150px;"><span class="fa fa-plus fa-right"></span> Salvar Apontamento</button></td > '
                                        + '    <td style="text-align: center;border: 1px dashed #00000080;background-color: #cccccc75;"><button type="button" class="btn-dark pull-right" onclick="ClickModelAdd(' + doc + ')" style="width: 100px;"><span class="fa fa-cart-plus fa-right"></span> Adicionar</button></td > '
                                        + '</tr>');
                                }
                            }

                            var qtyEfetivaRound = list.QtyEfetiva.toFixed(5);
                            var qtyUtilizadaRound = list.QtyUtilizada.toFixed(5);
                            var qtyDifRound = list.QtyDif.toFixed(5);
                            var qtySaldoRound = list.QtySaldo.toFixed(5);
                            var roundHTML = '0.01';

                            if (list.Und === 'UN' || list.Und === 'PC') {
                                roundHTML = '1.00'; 
                                qtyEfetivaRound = Math.round(qtyEfetivaRound).toFixed(5);
                                qtyUtilizadaRound = Math.round(qtyUtilizadaRound).toFixed(5);
                                qtyDifRound = Math.round(qtyDifRound).toFixed(5);
                                qtySaldoRound = Math.round(qtySaldoRound).toFixed(5);
                            }

                            //if (("#QtyPlan").Val() == 0) {
                            //    list.QtyEfetiva = list.QtyPlanejado;
                            //    qtyEfetivaRound = list.QtyPlanejado;
                            //    qtyUtilizadaRound = list.QtyPlanejado;
                            //}

                            $("#dtInsumo > tbody").append(
                                '<tr id="tr-' + doc + '-' + countList + '">'
                                + '    <td>'
                                + '    <input type="hidden" id="Itens_' + countList + '__DocEntry" name="Itens[' + countList + '].DocEntry" value="' + list.DocEntry + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__LineNum" name="Itens[' + countList + '].LineNum" value="' + list.LineNum + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__ProdName" name="Itens[' + countList + '].ProdName" value="' + list.ProdName + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__Tipo" name="Itens[' + countList + '].Tipo" value="' + list.Tipo + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__ItemCode" name="Itens[' + countList + '].ItemCode" value="' + list.ItemCode + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__ItemName" name="Itens[' + countList + '].ItemName" value="' + list.ItemName + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__QtyPlanejado" name="Itens[' + countList + '].QtyPlanejado" value="' + list.QtyPlanejado + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__Und" name="Itens[' + countList + '].Und" value="' + list.Und + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__QtyEfetiva" name="Itens[' + countList + '].QtyEfetiva" value="' + list.QtyEfetiva + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__QtyDif" name="Itens[' + countList + '].QtyDif" value="' + list.QtyDif + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__QtySaldo" name="Itens[' + countList + '].QtySaldo" value="' + list.QtySaldo + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__QtySaldoHidden" name="Itens[' + countList + '].QtySaldoHidden" value="' + list.QtySaldoHidden + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__ItemCodeChange" name="Itens[' + countList + '].ItemCodeChange" value="">'
                                + '    <input type="hidden" id="Itens_' + countList + '__ItemNameChange" name="Itens[' + countList + '].ItemNameChange" value="">'
                                + '    <input type="hidden" id="Itens_' + countList + '__Delete" name="Itens[' + countList + '].Delete" value="False">'
                                + '    <label id="Tipo-' + countList + '">' + list.Tipo + '</label></td> '
                                + '    <td>' + list.ItemCode + '</td > '
                                + '    <td>' + list.ItemName + '</td > '
                                + '    <td style="text-align: center;"><label id="QtyPlanejado-' + countList + '">' + list.QtyPlanejado.toFixed(5) + '</label></td>'
                                + '    <td style="text-align: center;">' + list.Und + '</td > '
                                + '    <td style="text-align: center;"><label id="QtyEfetiva-' + countList + '">' + qtyEfetivaRound + '</label></td>'
                                + '    <td><input class="form-control text-box single-line" data-val="true" data-val-number="O campo Quantidade deve ser um número." data-val-required="O campo Quantidade é obrigatório." id="Itens_' + countList + '__QtyUtilizada" name="Itens[' + countList + '].QtyUtilizada" type="number" min="0" step="' + roundHTML+'" value="' + qtyUtilizadaRound + '" style="height: 23px;width: 100%;" onchange="CalculatePercent(\'\')"></td>'
                                + '    <td style="text-align: center;"><label id="QtyDif-' + countList + '">' + qtyDifRound + '</label></td>'
                                + '    <td style="text-align: center;"><label id="QtySaldo-' + countList + '">' + qtySaldoRound + '</label></td>'
                                + '    <td><select id="Itens_' + countList + '__Motivo" name="Itens[' + countList + '].Motivo" style="width: 100%;" type="number"><option value="">' + list.MotivoList + '</select></td>'
                                + '    <td><input class="text-box single-line" id="Itens_' + countList + '__Justificativa" name="Itens[' + countList + '].Justificativa" type="text" style="width: 100%;" value="" maxlength="50"></td>'
                                + '    <td style="text-align: center;"><button id="Btn_' + countList + '" type="button" style="width: 100px;" class="btn-default pull-right" onclick="changeShow(' + doc + ',' + countList + ')"><span class="fa fa-exchange fa-right"></span> Trocar</button></td > '
                                + '</tr>');

                            $("#countList").val(parseInt(countList) + 1);

                        });
                        table.columns.adjust();

                        CalculatePercent('force');
                        $("#collapseContrato").collapse('show');
                        RefreshChosen();
                        loadingOff();
                    }
                });


            $.getJSON(
                "/Apontamento/CallGetInfoFechado?contratoId=" + contratoId + "&servicoId=" + servicoId,
                {},
                function (data) {

                    clearItensFechado();
                    RefreshChosen();

                    if (data.length === 0) {
                      
                    }
                    else {

                        var doc2;
                        var countList = 0;
                        $.each(data, function (index, list) {
                            if (doc2 === undefined) {
                                doc2 = list.DocEntry;
                                $("#dtInsumoFechado > tbody").append(
                                    '<tr id="tr-' + doc2 + '-">'
                                    + '    <td colspan="11" style="border: 1px dashed #00000080;background-color: #cccccc75;">' + list.ProdName + ' <b>|</b> Ordem de Produção Nº - ' + list.DocNum + ' <b>|</b> Quantidade Planejada - ' + list.QtyPlanejadoOrdemProducao + ' </td > '
                                    + '    <td style="text-align: center;border: 1px dashed #00000080;background-color: #cccccc75;"></td > '
                                    + '</tr>');
                            }
                            else {
                                if (doc2 !== list.DocEntry) {
                                    doc2 = list.DocEntry;
                                    $("#dtInsumoFechado > tbody").append(
                                        '<tr>'
                                        + '    <td colspan="11"></td > '
                                        + '</tr>');
                                    $("#dtInsumoFechado > tbody").append(
                                        '<tr >'
                                        + '    <td colspan="11" style="border: 1px dashed #00000080;background-color: #cccccc75;">' + list.ProdName + ' <b>|</b> Ordem de Produção Nº - ' + list.DocNum + ' <b>|</b> Quantidade Planejada - ' + list.QtyPlanejadoOrdemProducao + '</td > '
                                        + '    <td style="text-align: center;border: 1px dashed #00000080;background-color: #cccccc75;"> </td > '
                                        + '</tr>');
                                }
                            }

                            var qtyEfetivaRound = list.QtyEfetiva.toFixed(5);
                            var qtyUtilizadaRound = list.QtyUtilizada.toFixed(5);
                            var qtyDifRound = list.QtyDif.toFixed(5);
                            var qtySaldoRound = list.QtySaldo.toFixed(5);
                            var roundHTML = '0.01';
                            var motivo = list.Motivo;
                            var justificativa = list.Justificativa;

                            if (list.Und === 'UN' || list.Und === 'PC') {
                                roundHTML = '1.00';
                                qtyEfetivaRound = Math.round(qtyEfetivaRound).toFixed(5);
                                qtyUtilizadaRound = Math.round(qtyUtilizadaRound).toFixed(5);
                                qtyDifRound = Math.round(qtyDifRound).toFixed(5);
                                qtySaldoRound = Math.round(qtySaldoRound).toFixed(5);
                            }

                            //if (("#QtyPlan").Val() == 0) {
                            //    list.QtyEfetiva = list.QtyPlanejado;
                            //    qtyEfetivaRound = list.QtyPlanejado;
                            //    qtyUtilizadaRound = list.QtyPlanejado;
                            //}

                            $("#dtInsumoFechado > tbody").append(
                                '<tr>'
                                + '    <td>'
                                + '    <label >' + list.Tipo + '</label></td> '
                                + '    <td>' + list.ItemCode + '</td > '
                                + '    <td>' + list.ItemName + '</td > '
                                + '    <td style="text-align: center;"><label ">' + list.QtyPlanejado.toFixed(5) + '</label></td>'
                                + '    <td style="text-align: center;">' + list.Und + '</td > '
                                + '    <td style="text-align: center;"><label >' + qtyEfetivaRound + '</label></td>'
                                + '    <td> ' + qtyUtilizadaRound + '</td>'
                                + '    <td style="text-align: center;"><label > </label></td>'
                                + '    <td style="text-align: center;"><label >' + qtySaldoRound + '</label></td>'
                                + '    <td> ' + motivo + '</td>'
                                + '    <td> ' + justificativa + '</td > '
                                + '</tr>');

                            countList++;

                            //$("#countList").val(parseInt(countList) + 1);

                        });
                        table.columns.adjust();

                        CalculatePercent('force');
                        $("#collapseContrato").collapse('show');
                        RefreshChosen();
                        loadingOff();
                    }
                });

        }
        else {
            RefreshChosen();
            loadingOff();
        }

    } else {
        RefreshChosen();
        loadingOff();
    }

}

var zero = parseFloat(0).toFixed(5);
function CalculatePercent(type) {
    var countList = $("#countList").val();
    var comensaisDia = $("#QtyPlan").val();
    var qtyRefeicoes = $("#QtyRef").val();

    if ((comensaisDia !== null && comensaisDia > 0) && (qtyRefeicoes !== null && qtyRefeicoes > 0)) {
        qtyRefeicoes = parseFloat(qtyRefeicoes);
        comensaisDia = parseFloat(comensaisDia);

        var percentItem = qtyRefeicoes / comensaisDia;
        for (var i = 0; i < countList; i++) {
            var valTipo = $("#Itens_" + i + "__Tipo").val();
            var valUnd = $("#Itens_" + i + "__Und").val();
            var valPlan = parseFloat($("#Itens_" + i + "__QtyPlanejado").val());
            var calc = (valPlan * percentItem).toFixed(5);

            //if (valUnd === "UN" || valUnd === "PC")
              //  calc = Math.round(calc);

            $("#Itens_" + i + "__QtyEfetiva").val(calc);
            $("#QtyEfetiva-" + i).text(calc);

            var valUtil = $("#Itens_" + i + "__QtyUtilizada").val();
            if ((valUtil === 0 && type !== '') || type === 'force') {
                $("#Itens_" + i + "__QtyUtilizada").val(calc);
                valUtil = calc;
            }

            var valSaldo = $("#Itens_" + i + "__QtySaldoHidden").val();

            if (valTipo === "[-]") {
                valUtil = -valUtil;

                if (valUnd === "UN" || valUnd === "PC")
                    valUtil = Math.round(valUtil);
                

                $("#Itens_" + i + "__QtyDif").val(valUtil.toFixed(5));
                $("#QtyDif-" + i).text(valUtil.toFixed(5));
                valUtil = 0;
                $("#Itens_" + i + "__QtyUtilizada").val(valUtil.toFixed(5));
            }

            if (valUtil !== null && valUtil > 0) {
                valUtil = parseFloat(valUtil);
                valSaldo = parseFloat(valSaldo);

                var result = calc - valUtil;
                var resultSaldo = valSaldo; //- valUtil;

                if (valUnd === "UN" || valUnd === "PC") {
                    valUtil = Math.round(valUtil);
                    valSaldo = Math.round(valSaldo);
                    result = Math.round(result);
                    resultSaldo = Math.round(resultSaldo);
                }
                   
                if (valTipo === "Substituto") {
                    $("#Itens_" + i + "__QtyDif").val(valUtil.toFixed(5));
                    $("#QtyDif-" + i).text(valUtil.toFixed(5));
                }
                else {
                    $("#Itens_" + i + "__QtyDif").val(result.toFixed(5));
                    $("#QtyDif-" + i).text(result.toFixed(5));
                }
                
                $("#Itens_" + i + "__QtySaldo").val(resultSaldo.toFixed(5));
                $("#QtySaldo-" + i).text(resultSaldo.toFixed(5));
            }
            else {

                var saldo = parseFloat($("#Itens_" + i + "__QtySaldoHidden").val()).toFixed(5);

                if (valUnd === "UN" || valUnd === "PC") {
                    calc = Math.round(calc).toFixed(5);
                    saldo = Math.round(saldo).toFixed(5);
                }                   

                if (valTipo === "[-]") {
                    calc = -calc;
                    $("#Itens_" + i + "__QtyDif").val(calc);
                    $("#QtyDif-" + i).text(calc);
                }
                else {
                    //$("#Itens_" + i + "__QtyDif").val(zero);
                    //$("#QtyDif-" + i).text(zero);
                    $("#Itens_" + i + "__QtyDif").val(calc);
                    $("#QtyDif-" + i).text(calc);
                }

                $("#Itens_" + i + "__Saldo").val(saldo);
                $("#QtySaldo-" + i).text(saldo);
            }
        }
    } else {
        for (var i = 0; i < countList; i++) {
            var percentual = 1;

            var valPlan = parseFloat($("#Itens_" + i + "__QtyPlanejado").val());

            if (($("#Itens_" + i + "__QtyUtilizada").val() * 100 / valPlan) > 0 && ($("#Itens_" + i + "__QtyUtilizada").val() * 100 / valPlan) == 100) {
                percentual = ($("#Itens_" + i + "__QtyUtilizada").val() * 100 / valPlan) / 100;
            }

            var valTipo = $("#Itens_" + i + "__Tipo").val();
            var valUnd = $("#Itens_" + i + "__Und").val();
            var calc = (valPlan * percentual).toFixed(5);

            //if (valUnd === "UN" || valUnd === "PC")
            //  calc = Math.round(calc);

            $("#Itens_" + i + "__QtyEfetiva").val(calc);
            $("#QtyEfetiva-" + i).text(calc);

            var valUtil = $("#Itens_" + i + "__QtyUtilizada").val();
            if ((valUtil === 0 && type !== '') || type === 'force') {
                $("#Itens_" + i + "__QtyUtilizada").val(calc);
                valUtil = calc;
            }

            var valSaldo = $("#Itens_" + i + "__QtySaldoHidden").val();

            if (valTipo === "[-]") {
                valUtil = -valUtil;

                if (valUnd === "UN" || valUnd === "PC")
                    valUtil = Math.round(valUtil);


                $("#Itens_" + i + "__QtyDif").val(valUtil.toFixed(5));
                $("#QtyDif-" + i).text(valUtil.toFixed(5));
                valUtil = 0;
                $("#Itens_" + i + "__QtyUtilizada").val(valUtil.toFixed(5));
            }

            if (valUtil !== null && valUtil > 0) {
                valUtil = parseFloat(valUtil);
                valSaldo = parseFloat(valSaldo);

                var result = calc - valUtil;
                var resultSaldo = valSaldo; //- valUtil;

                if (valUnd === "UN" || valUnd === "PC") {
                    valUtil = Math.round(valUtil);
                    valSaldo = Math.round(valSaldo);
                    result = Math.round(result);
                    resultSaldo = Math.round(resultSaldo);
                }

                if (valTipo === "Substituto") {
                    $("#Itens_" + i + "__QtyDif").val(valUtil.toFixed(5));
                    $("#QtyDif-" + i).text(valUtil.toFixed(5));
                }
                else {
                    $("#Itens_" + i + "__QtyDif").val(result.toFixed(5));
                    $("#QtyDif-" + i).text(result.toFixed(5));
                }

                $("#Itens_" + i + "__QtySaldo").val(resultSaldo.toFixed(5));
                $("#QtySaldo-" + i).text(resultSaldo.toFixed(5));
            }
            else {

                var saldo = parseFloat($("#Itens_" + i + "__QtySaldoHidden").val()).toFixed(5);

                if (valUnd === "UN" || valUnd === "PC") {
                    calc = Math.round(calc).toFixed(5);
                    saldo = Math.round(saldo).toFixed(5);
                }

                if (valTipo === "[-]") {
                    calc = -calc;
                    $("#Itens_" + i + "__QtyDif").val(calc);
                    $("#QtyDif-" + i).text(calc);
                }
                else {
                    //$("#Itens_" + i + "__QtyDif").val(zero);
                    //$("#QtyDif-" + i).text(zero);
                    $("#Itens_" + i + "__QtyDif").val(calc);
                    $("#QtyDif-" + i).text(calc);
                }

                $("#Itens_" + i + "__Saldo").val(saldo);
                $("#QtySaldo-" + i).text(saldo);
            }
        }
    }
    //else {
        
    //    for (var a = 0; a < countList; a++) {
    //        $("#Itens_" + a + "__QtyUtilizada").val(zero);

    //        $("#Itens_" + a + "__QtyEfetiva").val(zero);
    //        $("#QtyEfetiva-" + a).text(zero);

    //        $("#Itens_" + a + "__QtyDif").val(zero);
    //        $("#QtyDif-" + a).text(zero);

    //        var saldo1 = parseFloat($("#Itens_" + a + "__QtySaldoHidden").val()).toFixed(5);

    //        if (valUnd === "UN" || valUnd === "PC") 
    //            saldo1 = Math.round(saldo1).toFixed(5);

    //        $("#Itens_" + a + "__Saldo").val(saldo1);
    //        $("#QtySaldo-" + a).text(saldo1);
    //    }
    //}

    table.columns.adjust();
}

function CheckDay() {

    var apontamentoData = $("#apontamentoData").val().trim();

    if (apontamentoData !== null && apontamentoData !== '') {

        loadingOn();

        $.getJSON(
            "/Apontamento/CallCheckDay?date=" + apontamentoData,
            {},
            function (data) {
                
                if (data.length === 0) {
                    new PNotify({
                        title: 'Alerta',
                        text: "Não encontrado informações parar o serviço " + servicoIdDesc,
                        type: 'error',
                        styling: 'bootstrap3'
                    });
                    loadingOff();
                    return;
                }
                else {

                    if (data !== null) {
                        alert(data);
                    }
                }

                loadingOff();
            });
    }
}

function SubmitOrdem(id) {

    $("#OrderCode").val(id);
    var retCheck = SubmitCheckOrdem(id);

    if (retCheck === true) {

        loadingOn();

        $.post("Apontamento/Create"
            , $("#idForm").serialize()
            ,function (data) {
                if (data !== null && data.Sucess === true) {

                    new PNotify({
                        title: 'Sucesso',
                        text: data.Message,
                        type: 'success',
                        styling: 'bootstrap3'
                    });
                    loadingOff();
                    $('[id^="tr-' + id + '"]').hide();

                    RefreshTerceiros(true);
                }
                else {

                    new PNotify({
                        title: 'Alerta',
                        text: data.Message,
                        type: 'error',
                        styling: 'bootstrap3'
                    });
                    loadingOff();

                    //RefreshErroSend();
                }
            });
    }
}

function SubmitCheckOrdem(id) {
    var countList = $("#countList").val();
    var countBPList = $("#countBPList").val();
    var checkAllZero = false;
    //Verificar se há data anterior sem apontamento para o serviço, ou seja, se há dia do calendário sem apontamento do serviço selecionado.Se existir, não permitir ir adiante, exibir uma mensagem de alerta de “Falta de Apontamentos de dias anteriores “e solicitar o código do serviço novamente.
    //BLOQUEAR APONTAMENTO SEM APONTAMENTO DOS DIAS ANTERIORES ENCERRADAS

    //OK
    //Bloquear apontamentos com mais de 72h da sua data
    //var dtApontamento = $("#apontamentoData").val();
    //if (dtApontamento !== null && dtApontamento !== "") {
    //    var calcDays = daysBetweenDates(dtApontamento);

    //    if (isNaN(calcDays)) {
    //        new PNotify({
    //            title: 'Alerta',
    //            text: "houve uma falha na captação da data. favor tentar novamente.",
    //            type: 'error',
    //            styling: 'bootstrap3'
    //        });
    //        return false;
    //    }

    //    if (calcDays > 3) {
    //        new PNotify({
    //            title: 'Alerta',
    //            text: "Bloquear apontamentos com mais de 72hs!",
    //            type: 'error',
    //            styling: 'bootstrap3'
    //        });
    //        return false;
    //    }
        
    //}
    //else {
    //    new PNotify({
    //        title: 'Alerta',
    //        text: "Favor selecionar uma data válida!",
    //        type: 'error',
    //        styling: 'bootstrap3'
    //    });
    //    return false;
    //}

    if (countList <= 0) {
        new PNotify({
            title: 'Alerta',
            text: "Não se pode enviar a solicitação sem itens.",
            type: 'error',
            styling: 'bootstrap3'
        });
        return false;
    }

    if (countBPList <= 0) {
        new PNotify({
            title: 'Alerta',
            text: "Não se pode ser realizado o apontamento sem a indicação de total de refeições.",
            type: 'error',
            styling: 'bootstrap3'
        });
        return false;
    }

    for (var i = 0; i < countList; i++) {
        var valDocEntry = $("#Itens_" + i + "__DocEntry").val();
        var valDelete = $("#Itens_" + i + "__Delete").val();
        

        if (valDocEntry !== id.toString()) continue;
        //var valTipo = $("#Itens_" + i + "__Tipo").val();
        var valItem = $("#Itens_" + i + "__ItemCode").val();
        var valItemName = $("#Itens_" + i + "__ItemName").val();
        var valQtyUtilizada = parseFloat($("#Itens_" + i + "__QtyUtilizada").val());
        var valQtyEfetiva = parseFloat($("#Itens_" + i + "__QtyEfetiva").val());
        //Qtd de Refeições=zero (0)


        //Qtd de Utilizadas=zero (0)
        //if (valQtyUtilizada > 0 && checkAllZero === false)
        //    checkAllZero = true;

        

        //Diferença > 0 && Tipo = Substituto


        //Substituto deve ter justificativa
        var valTipo = $("#Itens_" + i + "__Tipo").val();
        var valDif = parseFloat($("#Itens_" + i + "__QtyDif").val());
        if ((valTipo === "Substituto" || (valTipo === "[+]" && valDelete === "False")) || (valQtyUtilizada === 0 || Object.is(NaN, valQtyUtilizada) || valQtyUtilizada != valQtyEfetiva)) {

            var valMotivo = $("#Itens_" + i + "__Motivo").val();
            if (valMotivo === null || valMotivo === "") {
                new PNotify({
                    title: 'Alerta',
                    text: "Item: " + valItem + ' - ' + valItemName + ' obrigatório ter um motivo!',
                    type: 'error',
                    styling: 'bootstrap3'
                });
                return false;
            }

            var valJustificativa = $("#Itens_" + i + "__Justificativa").val();
            if (valJustificativa === null || valJustificativa === "") {
                new PNotify({
                    title: 'Alerta',
                    text: "Item: " + valItem + ' - ' + valItemName + ' obrigatório ter uma justificativa!',
                    type: 'error',
                    styling: 'bootstrap3'
                });
                return false;
            }
        }


        if (valTipo !== "[-]") {
            //Saldo Negativo
            var valSaldo = parseFloat($("#Itens_" + i + "__QtySaldo").val());
            if (valSaldo < 0) {

                new PNotify({
                    title: 'Alerta',
                    text: "Item: " + valItem + ' - '+ valItemName + ' com saldo negativo!',
                    type: 'error',
                    styling: 'bootstrap3'
                });
                return false;
            }
        }
        
    }

    //if (checkAllZero === false) {
    //    new PNotify({
    //        title: 'Alerta',
    //        text: "Todos as quantidades utilizadas desta OP estão vazias!",
    //        type: 'error',
    //        styling: 'bootstrap3'
    //    });
    //    return false;
    //}

    return true;

}

function daysBetweenDates(dateCompare) {
    var now = new Date();
    var dtStr = dateCompare + 'T' + pad(now.getHours(), 2) + ':' + pad(now.getMinutes(), 2) + ':00';
    var dt = new Date(dtStr.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
    //var dt = new Date(dateCompare + 'T' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds());

    return Math.abs((new Date().getTime() - dt.getTime()) * 1.16e-8);
}

function pad(str, max) {
    str = str.toString();
    return str.length < max ? pad("0" + str, max) : str;
}

function refresh() {
    window.location.reload(true);
}

function changeShow(id, line) {
    $("#modalItemOrderId").val(id);
    $("#modalItemOrderLineId").val(line);

    var valItem = $("#Itens_" + line + "__ItemCode").val();
    var valItemName = $("#Itens_" + line + "__ItemName").val();
    var valQty = parseFloat($("#QtyPlanejado-" + line).text());
    $("#modalItemItemCode").val(valItem);
    $("#modalItemItemName").val(valItemName);
    $("#modalItemQty").val(valQty);
    $("#modelProdName").text(valItem + ' - ' + valItemName);

    var valProd = $("#Itens_" + line + "__ProdName").val();
    $("#modalItemProdName").val(valProd);
    $("#modelOrderName").text(valProd);
    //

    var tipo = $("#Itens_" + line + "__Tipo").val();
    if (tipo === 'Estrutura') {

        loadingOn();

        var bplId = $("#BPLIdCode").val().trim();

        $.getJSON(
            "/Apontamento/CallGetItensList?itemCodeList=" + valItem + "&BPLIdList=" + bplId,
            {},
            function (data) {
                
                $('#ItemCodeList').empty();
                RefreshChosen();

                if (data.length === 0) {
                    new PNotify({
                        title: 'Alerta',
                        text: "Não foi encontrado itens para subistituir " + valItem + ' - ' + valItemName,
                        type: 'error',
                        styling: 'bootstrap3'
                    });
                    loadingOff();
                    return;
                }
                else {

                    if (data !== null) {

                        $.each(data, function (index, selectList) {
                            $('#ItemCodeList').append($("<option/>", {
                                value: selectList.Value,
                                text: selectList.Text
                            }));
                        });

                        $("#modal-change").modal('show');
                    }

                    RefreshChosen();
                    loadingOff();
                }

            });
    }
    else {

        if (tipo === '[-]') {

            var itemChange = $("#Itens_" + line + "__ItemCodeChange").val();
            var itemNameChange = $("#Itens_" + line + "__ItemNameChange").val();

            new PNotify({
                title: 'Alerta',
                text: "O item já foi listado para substituir pelo item " + itemChange + ' - ' + itemNameChange,
                type: 'error',
                styling: 'bootstrap3'
            });
            return;
        }
        else {
            new PNotify({
                title: 'Alerta',
                text: "O item já foi listado para ser substituido",
                type: 'error',
                styling: 'bootstrap3'
            });
            return;
        }
    }


    
 
}

function ChangeClick() {
    var modalItemOrderId = $("#modalItemOrderId").val();
    var modalItemOrderLineId = $("#modalItemOrderLineId").val();
    var modalItemItemCode = $("#modalItemItemCode").val();
    var modalItemItemName = $("#modalItemItemName").val();
    var modalItemProdName = $("#modalItemProdName").val();
    var ItemCodeList = $("#ItemCodeList").val();
    //

    if ((modalItemOrderId !== null && modalItemOrderId !== "")
        && (modalItemOrderLineId !== null && modalItemOrderLineId !== "")
        && (modalItemItemCode !== null && modalItemItemCode !== "")
        && (modalItemItemName !== null && modalItemItemName !== "")
        && (modalItemProdName !== null && modalItemProdName !== "")) {

        if (ItemCodeList !== null && ItemCodeList !== "") {

            var countList = $("#countList").val();
            var bplId = $("#BPLIdCode").val().trim();

            loadingOn();

            $.getJSON(
                "/Apontamento/CallGetItemInfo?infoItem=" + ItemCodeList + "&infoBPLID=" + bplId,
                {},
                function (data) {
                    
                    $('#ItemCodeList').empty();
                    RefreshChosen();

                    if (data.length === 0) {
                        new PNotify({
                            title: 'Alerta',
                            text: "Não foi encontrado informação do item.",
                            type: 'error',
                            styling: 'bootstrap3'
                        });
                        loadingOff();
                        return;
                    }
                    else {

                        if (data !== null) {

                            $("#Tipo-" + modalItemOrderLineId).text('[-]');
                            $("#Itens_" + modalItemOrderLineId + "__Tipo").val('[-]');
                            $("#Itens_" + modalItemOrderLineId + "__ItemCodeChange").val(ItemCodeList);
                            $("#Itens_" + modalItemOrderLineId + "__ItemCodeChange").val(ItemCodeList);
                            $("#Itens_" + modalItemOrderLineId + "__ItemNameChange").val(modalItemItemName);
                            $("#Itens_" + modalItemOrderLineId + "__QtyUtilizada").val(zero);
                            $("#Btn_" + modalItemOrderLineId).hide();

                            var itemCodeList = data.ItemCode;
                            var itemNameList = data.ItemName;
                            var itemUndList = data.Und;
                            var itemSaldoList = data.QtySaldo;
                            var itemTipo = 'Substituto';
                            var idTR = '#tr-' + modalItemOrderId + '-' + modalItemOrderLineId;

                            $(idTR).css("text-decoration","line-through");
                            
                            var htmlTR = '<tr id="tr-' + modalItemOrderId + '-' + countList + '" style="font-weight: bold;">'
                                + '    <td>'
                                + '    <input type="hidden" id="Itens_' + countList + '__DocEntry" name="Itens[' + countList + '].DocEntry" value="' + $("#Itens_" + modalItemOrderLineId + "__DocEntry").val() + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__LineNum" name="Itens[' + countList + '].LineNum" value="' + $("#Itens_" + modalItemOrderLineId + "__LineNum").val() + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__ProdName" name="Itens[' + countList + '].ProdName" value="' + $("#Itens_" + modalItemOrderLineId + "__ProdName").val() + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__Tipo" name="Itens[' + countList + '].Tipo" value="' + itemTipo + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__ItemCode" name="Itens[' + countList + '].ItemCode" value="' + itemCodeList + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__ItemName" name="Itens[' + countList + '].ItemName" value="' + itemNameList + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__QtyPlanejado" name="Itens[' + countList + '].QtyPlanejado" value="' + $("#Itens_" + modalItemOrderLineId + "__QtyPlanejado").val() + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__Und" name="Itens[' + countList + '].Und" value="' + itemUndList + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__QtyEfetiva" name="Itens[' + countList + '].QtyEfetiva" value="' + zero + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__QtyDif" name="Itens[' + countList + '].QtyDif" value="' + zero + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__QtySaldo" name="Itens[' + countList + '].QtySaldo" value="' + itemSaldoList.toFixed(5) + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__QtySaldoHidden" name="Itens[' + countList + '].QtySaldoHidden" value="' + itemSaldoList.toFixed(5) + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__ItemCodeChange" name="Itens[' + countList + '].ItemCodeChange" value="' + modalItemItemCode + '">'
                                + '    <input type="hidden" id="Itens_' + countList + '__ItemNameChange" name="Itens[' + countList + '].ItemNameChange" value="' + modalItemItemName+'">'
                                + '    <input type="hidden" id="Itens_' + countList + '__Delete" name="Itens[' + countList + '].Delete" value="False">'

                                + '    <label id="Tipo-' + countList + '">' + itemTipo +'</label></td> '
                                + '    <td>' + itemCodeList + '</td > '
                                + '    <td>' + itemNameList + '</td > '
                                + '    <td style="text-align: center;"><label id="QtyPlanejado-' + countList + '">' + parseFloat($("#Itens_" + modalItemOrderLineId + "__QtyPlanejado").val()).toFixed(5) + '</label></td>'
                                + '    <td style="text-align: center;">' + itemUndList + '</td > '
                                + '    <td style="text-align: center;"><label id="QtyEfetiva-' + countList + '">' + zero + '</label></td>'
                                + '    <td><input class="form-control text-box single-line" data-val="true" data-val-number="O campo Quantidade deve ser um número." data-val-required="O campo Quantidade é obrigatório." id="Itens_' + countList + '__QtyUtilizada" name="Itens[' + countList + '].QtyUtilizada" type="number" min="0" step="0.1" value="' + zero + '" style="height: 23px;width: 100%;" onchange="CalculatePercent(\'\')"></td>'
                                + '    <td style="text-align: center;"><label id="QtyDif-' + countList + '">' + zero + '</label></td>'
                                + '    <td style="text-align: center;"><label id="QtySaldo-' + countList + '">' + itemSaldoList.toFixed(5) + '</label></td>'
                                + '    <td><select id="Itens_' + countList + '__Motivo" name="Itens[' + countList + '].Motivo" style="width: 100%;" type="number"><option value="">' + data.MotivoList + '</select></td>'
                                + '    <td><input class="text-box single-line" id="Itens_' + countList + '__Justificativa" name="Itens[' + countList + '].Justificativa" type="text" style="width: 100%;" value="Item substituto ao ' + modalItemItemCode+'"></td>'
                                + '    <td></td> '
                                + '</tr>';

                            $(idTR).after(htmlTR);

                            $("#countList").val(parseInt(countList) + 1);

                            CalculatePercent('force');

                            table.columns.adjust();
                            $("#modal-change").modal('hide');
                            
                        }
                        RefreshChosen();
                        loadingOff();
                    }
                });
        }
        else {
            new PNotify({
                title: 'Alerta',
                text: "Favor de preencher o item a se substituir.",
                type: 'error',
                styling: 'bootstrap3'
            });
            return;
        }
    }
    else {

        new PNotify({
            title: 'Alerta',
            text: "Não foi possivel encontrar todos os dados. favor refazer a operação.",
            type: 'error',
            styling: 'bootstrap3'
        });
        return;
    }
}

/*TERCEIROS*/
function AutoRemoveBtnTerceiros() {

    var idApontamentoCheck = $("#IdApontamento").val();
    if (idApontamentoCheck !== null && idApontamentoCheck !== '' && idApontamentoCheck > 0) {
        $("#btnTerceirosAdd").hide();
        $(".btnTerceirosExclude").hide();
    }
    else {
        $("#btnTerceirosAdd").show();
        $(".btnTerceirosExclude").show();
    }
}

function RemoveBtnTerceiros() {
    $("#btnTerceirosAdd").hide();
    $(".btnTerceirosExclude").hide();
}


function ClickModelTerceirosAdd() {

    if ($('#BPLIdCode').find('option').length > 0) {

        var bplId = $("#BPLIdCode").val().trim();
        if (bplId !== null && bplId !== '') {

            if (!SumPlanejado()) return;

            var servicoId = $("#ServicoCode").val().trim();

            if (servicoId !== null && servicoId !== '') {

                clearInputTerceiros('add');

                $("#modal-terceiros-add").modal('show');
                LoadTerceiro('add');
            }
            else {
                new PNotify({
                    title: 'Alerta',
                    text: "Favor selecionar um serviço",
                    type: 'error',
                    styling: 'bootstrap3'
                });
                loadingOff();
                return;
            }
        }
        else {
            new PNotify({
                title: 'Alerta',
                text: "Favor selecionar uma Filial",
                type: 'error',
                styling: 'bootstrap3'
            });
            loadingOff();
            return;
        }
    }
}

function modalItemTerceiros(typeModel, id) {

    if (typeModel !== null && typeModel !== "") {

        if (id !== null && id !== "") {

            if (typeModel === "delete") {
                $("#modal-terceiros-delete-Id").val(id);
                $("#modal-terceiros-delete").modal('show');
            }
            else if (typeModel === "edit") {
                $("#modal-terceiros-edit-Id").val(id);
                                
                $("#modal-terceiros-edit").modal('show');
                LoadTerceiro('edit');
            }

        }
        else {
            new PNotify({
                title: 'Alerta',
                text: "Id não encontrado",
                type: 'error',
                styling: 'bootstrap3'
            });
            loadingOff();
            return;
        }
    }
    else {
        new PNotify({
            title: 'Alerta',
            text: "Tipo do model não foi encontrado",
            type: 'error',
            styling: 'bootstrap3'
        });
        loadingOff();
        return;
    }
}

function LoadTerceiro(type) {

    if ($('#BPLIdCode').find('option').length > 0) {

        var bplId = $("#BPLIdCode").val().trim();
        var bplIdDesc = $("#BPLIdCode option:selected").text();

        if (bplId !== null && bplId !== '') {

            loadingOn();

            $.getJSON(
                "/Apontamento/CallTerceirosBP?bBPLID=" + bplId,
                {},
                function (data) {

                    clearInputTerceiros(type);
                    RefreshChosen();

                    if (data.length === 0) {
                        new PNotify({
                            title: 'Alerta',
                            text: "Não encontrado os terceiros para filial " + bplIdDesc,
                            type: 'error',
                            styling: 'bootstrap3'
                        });
                        loadingOff();
                        return;
                    }
                    else {

                        $.each(data, function (index, selectList) {

                            if (type === 'add') {
                                $('#Add-BPCardCode').append($("<option/>", {
                                    value: selectList.Value,
                                    text: selectList.Text
                                }));
                            } else if (type === 'edit') {

                                $("#Edit-BPQtyRefeicao").val($('#BPs_' + $("#modal-terceiros-edit-Id").val() + '__BPQtyRefeicao').val());

                                $('#Edit-BPCardCode').append($("<option/>", {
                                    value: selectList.Value,
                                    text: selectList.Text
                                }));
                            }
                            
                        });
                    }

                    RefreshChosen();
                    loadingOff();
                });
        }
        else {
            clearInputTerceiros(type);
        }

    }
}

function GetTerceiroCliente(type) {


    var addCardCode = '';
    if(type ==='add')
        addCardCode= $("#Add-BPCardCode").val().trim();
    else
        addCardCode = $("#Edit-BPCardCode").val().trim();

    if (addCardCode !== null && addCardCode !== '') {

        loadingOn();

        $.getJSON(
            "/Apontamento/CallTerceirosBPCode?bCardCode=" + addCardCode,
            {},
            function (data) {

                $('#Add-BPCardName').val('');
                RefreshChosen();

                if (data.length === 0) {
                    new PNotify({
                        title: 'Alerta',
                        text: "Não encontrado o nome do cliente para o código" + addCardCode,
                        type: 'error',
                        styling: 'bootstrap3'
                    });
                    loadingOff();
                    return;
                }
                else {
                    if (type === 'add')
                        $('#Add-BPCardName').val(data.CardName);
                    else
                        $('#Edit-BPCardName').val(data.CardName);
                }

                RefreshChosen();
                loadingOff();
            });
    }
    else {
        $('#Add-BPCardName').val('');
    }
}

function checkItensTerceiros(typeCheck) {

    var txtInsumo = "";
    var txtQty = 0;
    if (typeCheck === "add") {
        txtInsumo = $("#Add-BPCardCode").val();
        txtQty = $("#Add-BPQtyRefeicao").val();
    }
    else {
        txtInsumo = $("#Edit-BPCardCode").val();
        txtQty = $("#Edit-BPQtyRefeicao").val();
    }

    if (txtInsumo === "" || txtInsumo === null) {
        new PNotify({
            title: 'Alerta',
            text: "Informe o código do cliente",
            type: 'error',
            styling: 'bootstrap3'
        });
        return false;
    }

    if (txtQty === "" || txtQty === "0" || txtQty === null) {
        new PNotify({
            title: 'Alerta',
            text: "Informe uma quantidade",
            type: 'error',
            styling: 'bootstrap3'
        });
        return false;
    }

    return true;
}

function SumPlanejado() {
    var countBPList = $("#countBPList").val();
    var comensaisDia = $("#QtyPlan").val();

    //if (comensaisDia !== null && comensaisDia > 0) {
        comensaisDia = parseFloat(comensaisDia);

        var sumCount = 0;
        for (var i = 0; i < countBPList; i++) {

            var valDelete = $("#BPs_" + i + "__Remove").val();
            if (valDelete === "False") {
                var valUse = parseFloat($("#BPs_" + i + "__BPQtyRefeicao").val());
                sumCount += valUse;
            }
        }

        var qtyAdd = $("#Add-BPQtyRefeicao").val();
        if (qtyAdd !== null && qtyAdd > 0) {
            sumCount += parseFloat(qtyAdd);
        }

        var qtyEdit = $("#Edit-BPQtyRefeicao").val();
        if (qtyEdit !== null && qtyEdit > 0) {
            sumCount += parseFloat(qtyEdit);
        }

        //if (sumCount > comensaisDia) {
        //    new PNotify({
        //        title: 'Alerta',
        //        text: "Quantidade ultrapassou o limite planejado",
        //        type: 'error',
        //        styling: 'bootstrap3'
        //    });
        //    return false;
        //}
        //else {
        //    $("#QtyRef").val(sumCount);
        //    return true;
        //}

        $("#QtyRef").val(sumCount);
        return true;
    //}
    //else {
    //    new PNotify({
    //        title: 'Alerta',
    //        text: "Quantidade planejado zerado!",
    //        type: 'error',
    //        styling: 'bootstrap3'
    //    });
    //    return false;
    //}

}

function AddTerceiros() {

    if (!checkItensTerceiros('add')) return;
    if (!SumPlanejado()) return;

    var txtCode = $("#Add-BPCardCode").val().trim();
    var txtName = $("#Add-BPCardName").val().trim();
    var txtQty = $("#Add-BPQtyRefeicao").val();

    var countBPList = $("#countBPList").val();

    if (countBPList === "0") {
        $("#dtTerceiros > tbody > tr").hide();
    }

    $("#dtTerceiros").append(
        '<tr id="bp-' + countBPList + '">'
        + '    <td>' + txtCode + '</td>'
        + '    <td>' + txtName + '</td>'
        + '    <td>' + txtQty + '</td>'
        + '    <td style="text-align: center">'
        + '        <input id="BPs_' + countBPList + '__BPType" name="BPs[' + countBPList + '].BPType" type="hidden" value="1">'
        + '        <input id="BPs_' + countBPList + '__BPCardCode" name="BPs[' + countBPList + '].BPCardCode" type="hidden" value="' + txtCode + '">'
        + '        <input id="BPs_' + countBPList + '__BPCardName" name="BPs[' + countBPList + '].BPCardName" type="hidden" value="' + txtName + '">'
        + '        <input data-val="true" data-val-number="The field Quantidade must be a number." data-val-required="The Quantidade field is required." id="BPs_' + countBPList + '__BPQtyRefeicao" name="BPs[' + countBPList + '].BPQtyRefeicao" type="hidden" value="' + txtQty + '">'
        + '        <input data-val="true" data-val-required="The Delete field is required." id="BPs_' + countBPList + '__Remove" name="BPs[' + countBPList + '].Remove" type="hidden" value="False">'
        //+ '        <button type="button" class="btn btn-default btn-small no-margin" onclick="modalItemTerceiros(\'edit\',' + countBPList + ')">'
        //+ '            <span class="fa fa-pencil"></span>'
        //+ '        </button>'
        + '        <button type="button" class="btn btn-default btn-small no-margin btnTerceirosExclude" onclick="modalItemTerceiros(\'delete\',' + countBPList + ')">'
        + '            <span class="fa fa-trash"></span>'
        + '        </button>'
        + '    </td>'
        + '</tr>');

    table.columns.adjust();
    $("#countBPList").val(parseInt(countBPList) + 1);
    CalculatePercent('force');
    clearInputTerceiros('add');
    $("#modal-terceiros-add").modal('hide');

    loadingOff();
}

//function EditTerceiros() {

//    if (!checkItensTerceiros('edit')) return;

//    $("#modal-terceiros-delete-Id").val($("#modal-terceiros-edit-Id").val());
//    RemoveTerceiros();

//    if (!SumPlanejado()) return;

//    var txtCode = $("#Edit-BPCardCode").val().trim();
//    var txtName = $("#Edit-BPCardName").val().trim();
//    var txtQty = $("#Edit-BPQtyRefeicao").val();

//    var countBPList = $("#countBPList").val();

//    if (countBPList === "0") {
//        $("#dtTerceiros > tbody > tr").hide();
//    }

//    $("#dtTerceiros").append(
//        '<tr id="bp-' + countBPList + '">'
//        + '    <td>' + txtCode + '</td>'
//        + '    <td>' + txtName + '</td>'
//        + '    <td>' + txtQty + '</td>'
//        + '    <td style="text-align: center">'
//        + '        <input id="BPs_' + countBPList + '__BPType" name="BPs[' + countBPList + '].BPType" type="hidden" value="1">'
//        + '        <input id="BPs_' + countBPList + '__BPCardCode" name="BPs[' + countBPList + '].BPCardCode" type="hidden" value="' + txtCode + '">'
//        + '        <input id="BPs_' + countBPList + '__BPCardName" name="BPs[' + countBPList + '].BPCardName" type="hidden" value="' + txtName + '">'
//        + '        <input data-val="true" data-val-number="The field Quantidade must be a number." data-val-required="The Quantidade field is required." id="BPs_' + countBPList + '__BPQtyRefeicao" name="BPs[' + countBPList + '].BPQtyRefeicao" type="hidden" value="' + txtQty + '">'
//        + '        <input data-val="true" data-val-required="The Delete field is required." id="BPs_' + countBPList + '__Delete" name="BPs[' + countBPList + '].Delete" type="hidden" value="False">'
//        + '        <button type="button" class="btn btn-default btn-small no-margin" onclick="modalItemTerceiros(\'edit\',' + countBPList + ')">'
//        + '            <span class="fa fa-pencil"></span>'
//        + '        </button>'
//        + '        <button type="button" class="btn btn-default btn-small no-margin" onclick="modalItemTerceiros(\'delete\',' + countBPList + ')">'
//        + '            <span class="fa fa-trash"></span>'
//        + '        </button>'
//        + '    </td>'
//        + '</tr>');

//    table.columns.adjust();
//    $("#countBPList").val(parseInt(countBPList) + 1);
//    CalculatePercent();
//    clearInputTerceiros('edit');
//    $("#modal-terceiros-edit").modal('hide');

//    loadingOff();
//}

function RemoveTerceiros() {
    var id = $("#modal-terceiros-delete-Id").val();

    if (id !== null && id !== "") {
        $('#BPs_' + id + '__Remove').val("True");
        $('#bp-' + id).closest("tr").fadeOut(100);
        $('.modal-backdrop').css("visibility", "hidden");
        $("#modal-terceiros-delete-Id").val("");
        $("#modal-terceiros-delete").modal('hide');

        SumPlanejado();
        CalculatePercent('force');
    }
    else {

        new PNotify({
            title: 'Alerta',
            text: "Não foi encontrado o ID para cancelamento",
            type: 'error',
            styling: 'bootstrap3'
        });
        loadingOff();
        return;
    }
}
/*TERCEIROS*/

/*ITENS*/
function ClickModelAdd(id) {

    if ($('#BPLIdCode').find('option').length > 0) {

        var bplId = $("#BPLIdCode").val().trim();
        if (bplId !== null && bplId !== '') {

            $("#lblQtySaidaTxt").text(zero);
            $("#lblQtyTotalTxt").text(zero);
            $("#lblQtyEstoqueTxt").text(zero);
            $("#lblCustoTxt").text(zero);
            $("#Add-Und").val('');
            $("#Add-ItemName").val('');
            $("#Add-Id").val(id);
            $("#Add-Qty").val(1);
            
            $("#modal-create").modal('show');
            RefreshItensSelect();
            
            loadingOff();
        }
        else {
            new PNotify({
                title: 'Alerta',
                text: "Favor selecionar uma Filial",
                type: 'error',
                styling: 'bootstrap3'
            });
            return;
        }
    }
    loadingOff();
}

function RefreshItensSelect() {

    var bplId = $("#BPLIdCode").val().trim();
    var bplIdDesc = $("#BPLIdCode option:selected").text();

    if (bplId !== null && bplId !== "") {

        loadingOn();
       
        $.getJSON(
            "/Apontamento/CallGetInsumo?BPLId=" + bplId,
            {},
            function (data) {
                $('#Add-InsumoCode').empty();
                //$('#Edit-InsumoCode').empty();
                RefreshChosen();

                if (data.length === 0) {
                    new PNotify({
                        title: 'Alerta',
                        text: "Insumos não encontrados para a filial " + bplIdDesc,
                        type: 'error',
                        styling: 'bootstrap3'
                    });
                    loadingOff();
                    return;
                }

                $.each(data, function (index, selectList) {
                    $('#Add-InsumoCode').append($("<option/>", {
                        value: selectList.Value,
                        text: selectList.Text
                    }));

                    //$('#Edit-InsumoCode').append($("<option/>", {
                    //    value: selectList.Value,
                    //    text: selectList.Text
                    //}));
                });

                ItemOnHand('modal');
                RefreshChosen();
                //loadingOff();
            });
        
    }
    else {
        $('#Add-InsumoCode').empty();
        //$('#Edit-InsumoCode').empty();
        RefreshChosen();
    }
}

function ItemOnHand(type) {
    var bplId = $("#BPLIdCode").val().trim();
    var bplIdDesc = $("#BPLIdCode option:selected").text();
    var txtInsumo = "";

    if (type === 'changeAdd')
        txtInsumo = $("#Add-InsumoCode").val();
    else
        txtInsumo = $("#ItemCodeList").val();

    if ((bplId !== null && bplId !== "") && (txtInsumo !== null && txtInsumo !== "")) {

        if (type !== 'modal')
            loadingOn();

        $.getJSON(
            "/Apontamento/CallGetInsumoOnHand?itemCode=" + txtInsumo + "&bplId=" + bplId,
            {},
            function (data) {

                if (data.length === 0) {
                    new PNotify({
                        title: 'Alerta',
                        text: "Não encontrado o insumo para a filial " + bplIdDesc,
                        type: 'error',
                        styling: 'bootstrap3'
                    });
                    loadingOff();
                    return;
                }

                if (data.OnHand !== null) {

                    if (type === 'changeAdd')
                        $("#lblQtyEstoqueTxt").text(data.OnHand.toFixed(5));
                    else
                        $("#lblQtyEstoqueTxtChange").text(data.OnHand.toFixed(5));

                    CalculateUsedOnHand(data);                   
                }

                if (data.AvgPrice !== null) {
                    if (type === 'changeAdd')
                        $("#lblCustoTxt").text(data.AvgPrice.toFixed(5));
                    else
                        $("#lblCustoTxtChange").text(data.AvgPrice.toFixed(5));
                }

                if (data.Und !== null) {
                    if (type === 'changeAdd')
                        $("#Add-Und").val(data.Und);
                    else
                        $("#Add-UndChange").val(data.Und);
                }

                if (data.ItemName !== null) {

                    if (type === 'changeAdd')
                        $("#Add-ItemName").val(data.ItemName);
                    else
                        $("#Add-ItemNameChange").val(data.ItemName);
                }

                RefreshChosen();
                loadingOff();
            });
    }
    else {
        new PNotify({
            title: 'Alerta',
            text: "Filial e Insumo não foram encontrados",
            type: 'error',
            styling: 'bootstrap3'
        });
        return;
    }
}

function CalculateUsedOnHand(type) {
    var txtQty = 0;
    var qtyEstoque = 0;

    if (type === 'changeAdd') {
        txtQty = parseFloat($("#Add-Qty").val());
        $("#lblQtySaidaTxt").text(txtQty.toFixed(5));

        qtyEstoque = parseFloat($("#lblQtyEstoqueTxt").text());
    }
    else {
        txtQty = parseFloat($("#modalItemQty").val());
        $("#lblQtySaidaTxtChange").text(txtQty.toFixed(5));

        qtyEstoque = parseFloat($("#lblQtyEstoqueTxtChange").text());
    }
        
    if (qtyEstoque > 0) {
        var total = qtyEstoque - txtQty;

        if (type === 'changeAdd')
            $("#lblQtyTotalTxt").text(total.toFixed(5));
        else
            $("#lblQtyTotalTxtChange").text(total.toFixed(5));
    }
    else {
        if (type === 'changeAdd')
            $("#lblQtyTotalTxt").text(zero);
        else
            $("#lblQtyTotalTxtChange").text(zero);
    }

    loadingOff();
}

function checkItens(typeCheck) {

    var txtInsumo = "";
    var txtQty = 0;
    var txtId = 0;
    if (typeCheck === "add") {
        txtInsumo = $("#Add-InsumoCode").val();
        txtQty = $("#Add-Qty").val();
        txtId = $("#Add-Id").val();
    }
    else {
        txtInsumo = $("#Edit-InsumoCode").val();
        txtQty = $("#Edit-Qty").val();
        txtId = $("#Edit-Id").val();
    }

    if (txtInsumo === "" || txtInsumo === null) {
        new PNotify({
            title: 'Alerta',
            text: "Informe o código do Insumo",
            type: 'error',
            styling: 'bootstrap3'
        });
        return false;
    }

    if (txtQty === "" || txtQty === "0" || txtQty === null) {
        new PNotify({
            title: 'Alerta',
            text: "Informe uma quantidade",
            type: 'error',
            styling: 'bootstrap3'
        });
        return false;
    }

    if (txtId === "" || txtId === "0" || txtId === null) {
        new PNotify({
            title: 'Alerta',
            text: "Erro ao capturar o parametro de identificação da ordem de produção.",
            type: 'error',
            styling: 'bootstrap3'
        });
        return false;
    }

    return true;
}

function AddClick() {

    if (!checkItens('add')) return;

    var modalItemOrderId = $("#Add-Id").val();
    var modalItemItemCode = $("#Add-InsumoCode").val();
    var modalItemItemName = $("#Add-ItemName").val();
    var modalQtyPlanejado = $("#Add-Qty").val();
    //

    if ((modalItemOrderId !== null && modalItemOrderId !== "")
        && (modalItemItemCode !== null && modalItemItemCode !== "")
        && (modalItemItemName !== null && modalItemItemName !== "")) {

        var countList = $("#countList").val();
        var bplId = $("#BPLIdCode").val().trim();

        loadingOn();

        $.getJSON(
            "/Apontamento/CallGetItemInfo?infoItem=" + modalItemItemCode + "&infoBPLID=" + bplId,
            {},
            function (data) {
                    
                if (data.length === 0) {
                    new PNotify({
                        title: 'Alerta',
                        text: "Não foi encontrado informação do item.",
                        type: 'error',
                        styling: 'bootstrap3'
                    });
                    loadingOff();
                    return;
                }
                else {

                    if (data !== null) {

                        var itemCodeList = data.ItemCode;
                        var itemNameList = data.ItemName;
                        var itemUndList = data.Und;
                        var itemSaldoList = data.QtySaldo;
                        var itemTipo = '[+]';
                        var idTR = '#tr-' + modalItemOrderId+'-';
                        
                        var htmlTR = '<tr id="tr-' + modalItemOrderId + '-' + countList + '" style="font-weight: bold;">'
                            + '    <td>'
                            + '    <input type="hidden" id="Itens_' + countList + '__DocEntry" name="Itens[' + countList + '].DocEntry" value="' + modalItemOrderId + '">'
                            + '    <input type="hidden" id="Itens_' + countList + '__LineNum" name="Itens[' + countList + '].LineNum" value="-1">'
                            + '    <input type="hidden" id="Itens_' + countList + '__ProdName" name="Itens[' + countList + '].ProdName" value="' + itemCodeList + ' - ' + itemNameList + '">'
                            + '    <input type="hidden" id="Itens_' + countList + '__Tipo" name="Itens[' + countList + '].Tipo" value="' + itemTipo + '">'

                            //
                            //QtyPlanejadoOrdemProducao
                            //

                            + '    <input type="hidden" id="Itens_' + countList + '__ItemCode" name="Itens[' + countList + '].ItemCode" value="' + itemCodeList + '">'
                            + '    <input type="hidden" id="Itens_' + countList + '__ItemName" name="Itens[' + countList + '].ItemName" value="' + itemNameList + '">'
                            + '    <input type="hidden" id="Itens_' + countList + '__QtyPlanejado" name="Itens[' + countList + '].QtyPlanejado" value="' + modalQtyPlanejado + '">'
                            + '    <input type="hidden" id="Itens_' + countList + '__Und" name="Itens[' + countList + '].Und" value="' + itemUndList + '">'
                            + '    <input type="hidden" id="Itens_' + countList + '__QtyEfetiva" name="Itens[' + countList + '].QtyEfetiva" value="' + zero + '">'
                            + '    <input type="hidden" id="Itens_' + countList + '__QtyDif" name="Itens[' + countList + '].QtyDif" value="' + zero + '">'
                            + '    <input type="hidden" id="Itens_' + countList + '__QtySaldo" name="Itens[' + countList + '].QtySaldo" value="' + itemSaldoList.toFixed(5) + '">'
                            + '    <input type="hidden" id="Itens_' + countList + '__QtySaldoHidden" name="Itens[' + countList + '].QtySaldoHidden" value="' + itemSaldoList.toFixed(5) + '">'
                            + '    <input type="hidden" id="Itens_' + countList + '__ItemCodeChange" name="Itens[' + countList + '].ItemCodeChange" value="">'
                            + '    <input type="hidden" id="Itens_' + countList + '__ItemNameChange" name="Itens[' + countList + '].ItemNameChange" value="">'
                            + '    <input type="hidden" id="Itens_' + countList + '__Delete" name="Itens[' + countList + '].Delete" value="False">'

                            + '    <label id="Tipo-' + countList + '">' + itemTipo + '</label></td> '
                            + '    <td>' + itemCodeList + '</td > '
                            + '    <td>' + itemNameList + '</td > '
                            + '    <td style="text-align: center;"><label id="QtyPlanejado-' + countList + '">' + parseFloat(modalQtyPlanejado).toFixed(5) + '</label></td>'
                            + '    <td style="text-align: center;">' + itemUndList + '</td > '
                            + '    <td style="text-align: center;"><label id="QtyEfetiva-' + countList + '">' + zero + '</label></td>'
                            + '    <td><input class="form-control text-box single-line" data-val="true" data-val-number="O campo Quantidade deve ser um número." data-val-required="O campo Quantidade é obrigatório." id="Itens_' + countList + '__QtyUtilizada" name="Itens[' + countList + '].QtyUtilizada" type="number" min="0" step="0.1" value="' + zero + '" style="height: 23px;width: 100%;" onchange="CalculatePercent(\'\')"></td>'
                            + '    <td style="text-align: center;"><label id="QtyDif-' + countList + '">' + zero + '</label></td>'
                            + '    <td style="text-align: center;"><label id="QtySaldo-' + countList + '">' + itemSaldoList.toFixed(5) + '</label></td>'
                            + '    <td><select id="Itens_' + countList + '__Motivo" name="Itens[' + countList + '].Motivo" style="width: 100%;" type="number"><option value="">' + data.MotivoList + '</select></td>'
                            + '    <td><input class="text-box single-line" id="Itens_' + countList + '__Justificativa" name="Itens[' + countList + '].Justificativa" type="text" style="width: 100%;" value="Item adicionado"></td>'
                            + '    <td style="text-align: center;">'
                            //+ '        <button type="button" class="btn btn-default btn-small no-margin" onclick="modalItemTerceiros(\'edit\',' + countList + ')">'
                            //+ '            <span class="fa fa-pencil"></span>'
                            //+ '        </button>'
                            + '        <button type="button" class="btn btn-default btn-small no-margin" onclick="modalItem(\'delete\','+modalItemOrderId+ ',' + countList + ')">'
                            + '            <span class="fa fa-trash"></span>'
                            + '        </button>'
                            +'     </td > '
                            + '</tr>';

                        $(idTR).after(htmlTR);

                        $("#countList").val(parseInt(countList) + 1);

                        CalculatePercent('force');

                        table.columns.adjust();
                        $("#modal-create").modal('hide');

                    }
                    RefreshChosen();
                    loadingOff();
                }
            });
        
    }
    else {

        new PNotify({
            title: 'Alerta',
            text: "Não foi possivel encontrar todos os dados. favor refazer a operação.",
            type: 'error',
            styling: 'bootstrap3'
        });
        return;
    }
}

function modalItem(typeModel, id, line) {

    if (typeModel !== null && typeModel !== "") {

        if (id !== null && id !== "") {

            if (typeModel === "delete") {
                $("#modal-delete-Id").val(id);
                $("#modal-delete-IdLine").val(line);
                
                $("#modal-delete").modal('show');
            }
            else if (typeModel === "edit") {
                //$("#modal-edit-Id").val(id);
                //$("#Edit-InsumoCode").val($("#Itens_" + id + "__InsumoCode").val());
                //$("#Edit-Qty").val($("#Itens_" + id + "__Qty").val());
                //$("#modal-edit").modal('show');
            }

        }
        else {
            new PNotify({
                title: 'Alerta',
                text: "Id não encontrado",
                type: 'error',
                styling: 'bootstrap3'
            });
            loadingOff();
            return;
        }
    }
    else {
        new PNotify({
            title: 'Alerta',
            text: "Tipo do model não foi encontrado",
            type: 'error',
            styling: 'bootstrap3'
        });
        loadingOff();
        return;
    }
}

function RemoveClick() {
    var id = $("#modal-delete-Id").val();
    var idLine = $("#modal-delete-IdLine").val();

    if (id !== null && id !== "") {
        $('#Itens_' + idLine + '__Delete').val("True");
        $('#tr-' + id + '-' + idLine).closest("tr").fadeOut(100);
        $('.modal-backdrop').css("visibility", "hidden");
        $("#modal-delete-Id").val("");
        $("#modal-delete-IdLine").val("");
        $("#modal-delete").modal('hide');

        SumPlanejado();
        CalculatePercent('force');
    }
    else {

        new PNotify({
            title: 'Alerta',
            text: "Não foi encontrado o ID para cancelamento",
            type: 'error',
            styling: 'bootstrap3'
        });
        loadingOff();
        return;
    }
}
/*ITENS*/

var table;
//var tableTerceiros;
function SetDataTable() {
    table = $('#dtInsumo').DataTable({
        searching: false,
        pagingType: "numbers",
        lengthChange: false,
        paging: false,
        ordering: false,
        info: false,
        "language": {
            "emptyTable": "Sem dados disponíveis na tabela"
        }
    });

    table.columns.adjust();

    //tableTerceiros = $('#dtTerceiros').DataTable({
    //    searching: false,
    //    pagingType: "numbers",
    //    lengthChange: false,
    //    paging: false,
    //    ordering: false,
    //    info: false,
    //    "language": {
    //        "emptyTable": "Sem dados disponíveis na tabela"
    //    }
    //});

    //tableTerceiros.columns.adjust();
}

function callBPLID() {
    clearContrato();
    clearItens();
    clearItensFechado();
    clearBPsAll();
    
    RefreshContrato();
    RefreshCliente();

    AutoRemoveBtnTerceiros();
}

function clearAll() {
    clearItens();
    clearItensFechado();
    clearBPsAll();
    clearCliente();
    clearContrato();
    clearServicoAll();

    table.columns.adjust();
    RefreshChosen();
    $("#collapseFilter").collapse('show');
    $("#collapseTerceiros").collapse('hide');
    $("#collapseContrato").collapse('hide');
}

function clearItens() {
    $("#countList").val(0);
    $("#dtInsumo > tbody").empty();
}
function clearItensFechado() {
    $("#dtInsumoFechado > tbody").empty();
}

function clearBPs() {
    clearBPSolo();
    clearServico();
}

function clearBPSolo() {
    $("#IdApontamento").val(null);
    $("#countBPList").val(0);
    $("#dtTerceiros > tbody").empty();
}


function clearBPsAll() {
    $("#IdApontamento").val(null);
    $("#countBPList").val(0);
    $("#dtTerceiros > tbody").empty();

    clearServicoAll();
}

function clearCliente() {
    $('#ClienteCode').val("");
    $('#ClienteName').val("");
    $('#txtClienteName').val("");
}

function clearContrato() {
    $('#ContratoCode').empty();
}

function clearServico() {
    $("#QtyPlan").val(zero);
    $("#QtyRef").val(zero);
    $("#QtySobra").val(zero);
    $("#QtyResto").val(zero);
    CalculatePercent('force');
}

function clearServicoAll() {
    $('#ServicoCode').empty();
    clearServico();
    CalculatePercent('');
}

function clearInputTerceiros(type) {
    if (type === 'add') {
        $("#Add-BPCardCode").empty();
        $("#Add-BPCardName").val("");
        $("#Add-BPQtyRefeicao").val(0);
    }
    else {
        $("#Edit-BPCardCode").empty();
        $("#Edit-BPCardName").val("");
        $("#Edit-BPQtyRefeicao").val(0);
    }
    
}
