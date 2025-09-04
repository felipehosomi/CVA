$(document).ready(function () {
    SetDataTable();

    $(document).on('change', '#BPLIdCode', function () {
        BplSelect();
    });

});

$(function () {
    // document ready
    SetChosen();
});

function BplSelect() {

    var bplId = $("#BPLIdCode").val().trim();
    var bplIdDesc = $("#BPLIdCode option:selected").text();

    if (bplId !== null && bplId !== "") {

        $("#dtInsumo > tbody > tr").hide();

        $.getJSON(
            "/Apontamento/CallCliente?clienteBPLID=" + bplId,
            {},
            function (data) {

                loadingOff();

                clearCliente();

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

                    ContratoSelect();
                }

                loadingOff();
            });

    }
    else {
        $('#ContratoCode').empty();
        $("#dtInsumo > tbody > tr").hide();
        RefreshChosen();
    }
}

function ContratoSelect() {

    var bplId = $("#BPLIdCode").val().trim();
    var bplIdDesc = $("#BPLIdCode option:selected").text();

    if (bplId !== null && bplId !== ""){

        $.getJSON(
            "/SaidaMateriais/CallReportPosicaoEstoque?RptBPLId=" + bplId,
            {},
            function (data) {

                //Clear
                $("#dtInsumo > tbody > tr").hide();
                RefreshChosen();
                if (data.length === 0) {
                    new PNotify({
                        title: 'Alerta',
                        text: "Não foi encontrado os itens para filial " + bplIdDesc,
                        type: 'error',
                        styling: 'bootstrap3'
                    });
                    return;
                }

                $.each(data, function (index, row) {

                    $("#dtInsumo").append(
                        '<tr>'
                        + '    <td>' + row.InsumoCode + '</td>'
                        + '    <td>' + row.InsumoName + '</td>'
                        + '    <td>' + row.Qty + '</td>'
                        + '    <td> R$ ' + row.Custo + '</td>'
                        + '    <td> R$ ' + formatMoney((row.Qty * row.Custo), 2, ".", ",") + '</td>'
                        + '</tr>');
                });

                RefreshChosen();
            });
    }
    else {
        $("#dtInsumo > tbody > tr").hide();
        RefreshChosen();
    }
}

function formatMoney(amount, decimalCount, decimal, thousands) {
    try {
        decimalCount = Math.abs(decimalCount);
        decimalCount = isNaN(decimalCount) ? 2 : decimalCount;

        const negativeSign = amount < 0 ? "-" : "";

        let i = parseInt(amount = Math.abs(Number(amount) || 0).toFixed(decimalCount)).toString();
        let j = (i.length > 3) ? i.length % 3 : 0;

        return negativeSign + (j ? i.substr(0, j) + thousands : '') + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousands) + (decimalCount ? decimal + Math.abs(amount - i).toFixed(decimalCount).slice(2) : "");
    } catch (e) {
        console.log(e)
    }
};

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

function clearCliente() {
    $('#ClienteCode').val("");
    $('#ClienteName').val("");
    $('#txtClienteName').val("");
}

function refresh() {
    window.location.reload(true);
}


