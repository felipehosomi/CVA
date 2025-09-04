$(document).ready(function () {
    SetDataTable();

    $(document).on('change', '#BPLIdCode', function () {
        clearContrato();
        clearItens();
        RefreshContrato();
    });

    $(document).on('change', '#ContratoCode', function () {
        clearContrato();
        clearItens();
        RefreshContratoInfo();
    });
    

});

function RefreshContrato() {

    var bplId = $("#BPLIdCode").val().trim();
    var bplIdDesc = $("#BPLIdCode option:selected").text();

    if (bplId !== null && bplId !== '') {
        $.getJSON(
            "/Apontamento/CallGetContrato?BPLId=" + bplId,
            {},
            function (data) {

                $('#ContratoCode').empty();
                if (data.length === 0) {
                    new PNotify({
                        title: 'Alerta',
                        text: "Contrato não encontrado para a filial " + bplIdDesc,
                        type: 'error',
                        styling: 'bootstrap3'
                    });
                    return;
                }

                $.each(data, function (index, selectList) {
                    $('#ContratoCode').append($("<option/>", {
                        value: selectList.Value,
                        text: selectList.Text
                    }));
                });
            });
    }
    else {
        clearAll();
    }
    

}

function RefreshContratoInfo() {
    
    if ($('#ContratoCode').find('option').length > 0) {

        var contratoId = $("#ContratoCode").val().trim();
        var contratoIdDesc = $("#ContratoCode option:selected").text();

        $.getJSON(
            "/Apontamento/CallGetInfo?contratoId=" + contratoId,
            {},
            function (data) {

                clearContrato();

                if (data.length === 0) {
                    new PNotify({
                        title: 'Alerta',
                        text: "Não encontrado para o contrato " + contratoIdDesc,
                        type: 'error',
                        styling: 'bootstrap3'
                    });
                    return;
                }
                else {

                    if (data.U_CVA_ID_CLIENTE !== null) {
                        $('#ClienteCode').val(data.U_CVA_ID_CLIENTE);
                        $('#ClienteName').val(data.U_CVA_ID_CLIENTE + ' - ' + data.U_CVA_DES_CLIENTE);
                        $('#txtClienteName').val($('#ClienteName').val());
                    }
                    
                    clearItens();
                }
            });
    }

}

function Search() {

    if(!checkItens()) return;

}

function checkItens() {
    
    var txtBPLIdCode = $("#BPLIdCode").val();
    var txtContratoCode = $("#ContratoCode").val();
    var txtClienteCode = $("#ClienteCode").val();
    var txtcontratoDe = $("#contratoDe").val();
    var txtcontratoAte = $("#contratoAte").val();

    if (txtBPLIdCode === "" || txtBPLIdCode === null) {
        new PNotify({
            title: 'Alerta',
            text: "Informe o código da filial",
            type: 'error',
            styling: 'bootstrap3'
        });
        return false;
    }

    if (txtContratoCode === "" || txtContratoCode === null) {
        new PNotify({
            title: 'Alerta',
            text: "Informe o código do contrato",
            type: 'error',
            styling: 'bootstrap3'
        });
        return false;
    }

    if (txtClienteCode === "" || txtClienteCode === null) {
        new PNotify({
            title: 'Alerta',
            text: "Informe um contrato/cliente válido",
            type: 'error',
            styling: 'bootstrap3'
        });
        return false;
    }

    if (txtcontratoDe === "" || txtcontratoDe === null) {
        new PNotify({
            title: 'Alerta',
            text: "Informe uma data inicial",
            type: 'error',
            styling: 'bootstrap3'
        });
        return false;
    }

    if (txtcontratoAte === "" || txtcontratoAte === null) {
        new PNotify({
            title: 'Alerta',
            text: "Informe uma data final",
            type: 'error',
            styling: 'bootstrap3'
        });
        return false;
    }

    return true;
}

/*****************************************/
function CalculatePercent() {
    var countList = $("#countList").val();
    var comensaisDia = $("#ComensaisDia").val();
    var qtyRefeicoes = $("#Qty").val();

    if (comensaisDia !== null && comensaisDia > 0) {
        for (var i = 0; i < countList; i++) {
            var percentItem = $("#Itens_" + i + "__U_CVA_PERCENT").val();
            var calc = (comensaisDia * percentItem) / 100;
            $("#Itens_" + i + "__QtyPlanejado").val(calc);
            $("#QtyPlanejado-" + i).text(calc);

            if (qtyRefeicoes !== null && qtyRefeicoes > 0) {
                var calcQty = (qtyRefeicoes * percentItem) / 100;
                $("#Itens_" + i + "__QtyConsumido").val(calcQty);
            }
        }
    }
}

function SetDataTable() {
    $('#dtInsumo').DataTable({
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
}

function clearAll() {
    clearItens();
    clearContrato();
}

function clearItens() {
    $("#countList").val(0);
    $("#dtInsumo > tbody").empty();
}

function clearContrato() {
    $('#txtClienteName').val("");
    $('#ClienteCode').val("");
    $('#ClienteName').val("");
    
    
}
/*****************************************/