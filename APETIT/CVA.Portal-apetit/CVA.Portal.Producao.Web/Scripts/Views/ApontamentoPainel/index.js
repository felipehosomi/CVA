$(document).ready(function () {
    SetDataTable();

    $(document).on('change', '#BPLIdCode', function () {
        clearCliente();
        clearItens();
        RefreshCliente();
    });
    
});

$(function () {
    // document ready
    SetChosen();
});

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

                    loadingOff();

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

                    loadingOff();
                    RefreshChosen();
                });
        }
        else {
            clearCliente();
            CalculatePercent();
        }

    }
}


function Search() {

    if (!checkItens()) return;

    var bplId = $("#BPLIdCode").val().trim();
    var bplIdDesc = $("#BPLIdCode option:selected").text();
    var dateDe = $("#contratoDe").val().trim();
    var dateAte = $("#contratoAte").val().trim();

    loadingOn();

    $.getJSON(
        "/ApontamentoPainel/CallGetPainel?pnBplId=" + bplId + "&dateDe=" + dateDe + "&dateAte=" + dateAte,
        {},
        function (data) {

            RefreshChosen();

            if (data.length === 0) {
                new PNotify({
                    title: 'Alerta',
                    text: "Não encontrado informações para a filial " + bplIdDesc,
                    type: 'error',
                    styling: 'bootstrap3'
                });
                loadingOff();
                return;
            }
            else {

                clearItens();
                $.each(data, function (index, list) {
                    var countList = $("#countList").val();

                    $("#dtInsumo > tbody").append(
                        '<tr id="tr-' + countList + '">'
                        + '    <td>' + JavascriptDate(list.DT) + '</td>'
                        + '    <td>' + list.DAYOFWEEKDESC + '</td>'
                        + '    <td>' + list.SERVICONAME  + '</td>'
                        + '    <td>' + list.TURNONAME + '</td>'
                        + '    <td>' + list.QTYPL.toFixed(2) + '</td>'
                        + '    <td>' + list.QTYAP.toFixed(2) + '</td>'
                        + '    <td>' + list.DIFF.toFixed(2) + '</td>'
                        + '    <td style ="background-color:' + CalculateDiff(list.DIFF) + '">' + '</td>'
                        + '    <td style="text-align: center;"><button type="button" class="btn-default" onclick="goToApontamento(' + list.DT + ',' + list.SERVICOCODE + ',' + list.TURNOCODE + ')"><span class="fa fa-exchange fa-right"></span></button></td>'
                        + '</tr>');

                    $("#countList").val(parseInt(countList) + 1);
                });

            }

            loadingOff();
            RefreshChosen();
        });

}

function checkItens() {
    
    var txtBPLIdCode = $("#BPLIdCode").val();
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

function refresh() {
    window.location.reload(true);
}

function CalculateDiff(diff) {

    switch (true) {
        case (diff === 0):
            return "rgba(0, 128, 0, 0.6);";
        case (diff > 0):
            return "rgba(0, 0, 255, 0.7);";
        case (diff < 0):
            return "rgba(255, 0, 0, 0.7);";
    }
    
}

function JavascriptDate(value) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    // get date in dd/MM/yyyy format
    return (dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear());
}

function JavascriptDateURL(value) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    // get date in dd/MM/yyyy format
    return (dt.getFullYear() + "-" + (dt.getMonth() + 1) + "-" + dt.getDate());
}

function goToApontamento(date, servico, ordem) {
    var bplId = $("#BPLIdCode").val().trim();
    var txtClienteCode = $("#ClienteCode").val().trim();
    var txtClienteName = $("#txtClienteName").val().trim();
    window.location.replace("./Apontamento/Painel/" + bplId + '|' + txtClienteCode + '|' + txtClienteName + '|' + JavascriptDateURL(date) + '|' + servico + '|' + ordem);
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

    RefreshChosen();
}

function clearCliente() {
    $('#txtClienteName').val("");
    $('#ClienteCode').val("");
    $('#ClienteName').val("");

    RefreshChosen();
}
/*****************************************/

