$(document).ready(function () {
    SetDataTable();

    $(document).on('change', '#BPLIdCode', function () {

        $("#dtInsumo tbody tr[id^=\"tr-\"]").each(function () {
            $("#modal-delete-Id").val($(this).attr('id').replace('tr-', ''));
            removeTb();
        });

        RefreshSelect();
    });

    $(document).on('change', '#Add-InsumoCode', function () {
        ItemOnHand();
    });

    $(document).on('change', '#Add-Qty', function () {
        CalculateUsedOnHand();
    });

    $(document).resize(function () {
        table.columns.adjust();
    });
});

$(function () {
    // document ready
    SetChosen();
});

///
var zero = 0.00;
///

function RefreshSelect() {

    var bplId = $("#BPLIdCode").val().trim();
    var bplIdDesc = $("#BPLIdCode option:selected").text();

    loadingOn();

    $.getJSON(
        "/Apontamento/CallCliente?clienteBPLID=" + bplId,
        {},
        function (data) {

            clearCliente();

            if (data.length === 0) {
                new PNotify({
                    title: 'Alerta',
                    text: "Cliente não encontrado para a filial " + bplIdDesc,
                    type: 'error',
                    styling: 'bootstrap3'
                });
                loadingOff();
                return;
            }

            $('#ClienteCode').val(data.CardCode);
            $('#ClienteName').val(data.CardName);
            $('#txtClienteName').val(data.CardCode + ' - ' + data.CardName);

            loadingOff();
            RefreshChosen();
        });

    if ($('#BPLIdCode').find('option').length > 0) {

        loadingOn();

        $.getJSON(
            "/ReposicaoInsumos/CallGetInsumo?BPLId=" + bplId,
            {},
            function (data) {

                $('#Add-InsumoCode').empty();
                $('#Edit-InsumoCode').empty();
                RefreshChosen();

                if (data.length === 0) {
                    new PNotify({
                        title: 'Alerta',
                        text: "Insumos não encontrado para a filial " + bplIdDesc,
                        type: 'error',
                        styling: 'bootstrap3'
                    });
                    loadingOff();
                    return;
                }

                $.each(data, function (index, selectList) {
                    $('#Add-InsumoCode').append($("<option/>", {
                        value: selectList.Value,
                        text: selectList.Value + ' | ' + selectList.Text
                    }));

                    $('#Edit-InsumoCode').append($("<option/>", {
                        value: selectList.Value,
                        text: selectList.Value + ' |' + selectList.Text
                    }));
                });

                loadingOff();
                RefreshChosen();
            });
    }

}


function checkItens(typeCheck) {

    var txtInsumo = "";
    var txtQty = 0;
    var txtDt = "";

    if (typeCheck === "add") {
        txtInsumo = $("#Add-InsumoCode").val();
        txtQty = $("#Add-Qty").val();
        txtDt = $("#Add-DtNecessidade").val();
    }
    else {
        txtInsumo = $("#Edit-InsumoCode").val();
        txtQty = $("#Edit-Qty").val();
        txtDt = $("#Edit-DtNecessidade").val();
    }

    if (txtInsumo === "" || txtInsumo === null) {
        new PNotify({
            title: 'Alerta',
            text: "Informe o código do Insumo",
            type: 'error',
            styling: 'bootstrap3'
        });
        loadingOff();
        return false;
    }

    if (txtQty === "" || txtQty === "0" || txtQty === null) {
        new PNotify({
            title: 'Alerta',
            text: "Informe uma quantidade",
            type: 'error',
            styling: 'bootstrap3'
        });
        loadingOff();
        return false;
    }

    if (txtDt === "" || txtDt === null) {
        new PNotify({
            title: 'Alerta',
            text: "Informe o Data da Necessidade",
            type: 'error',
            styling: 'bootstrap3'
        });
        loadingOff();
        return false;
    }
    
    return true;
}

function AddClick() {

    if (!checkItens('add')) return;
    
    var txtInsumoDesc = $("#Add-InsumoCode option:selected").text().split('|')[1];

    var txtInsumo = $("#Add-InsumoCode").val();
    var txtQty = $("#Add-Qty").val();
    var txtDt = $("#Add-DtNecessidade").val();

    var fullDate = new Date(txtDt + ' 00:00');
    var twoDigitMonth = ((fullDate.getMonth().length + 1) === 1) ? (fullDate.getMonth() + 1) : '0' + (fullDate.getMonth() + 1);
    var txtDtShow = fullDate.getDate() + "/" + twoDigitMonth + "/" + fullDate.getFullYear();
     
    
    var countList = $("#countList").val();

    if (countList === "0") {
        $("#dtInsumo > tbody > tr").hide();
    }

    $("#dtInsumo").append(
        '<tr id="tr-' + countList + '">'
        + '    <td>' + txtInsumo + '</td>'
        + '    <td>' + txtInsumoDesc + '</td>'
        + '    <td>' + txtQty + '</td>'
        + '    <td>' + txtDtShow + '</td>'
        + '    <td style="text-align: center">'
        + '        <input id="Itens_' + countList + '__InsumoCode" name="Itens[' + countList + '].InsumoCode" type="hidden" value="' + txtInsumo + '">'
        + '        <input id="Itens_' + countList + '__InsumoName" name="Itens[' + countList + '].InsumoName" type="hidden" value="' + txtInsumoDesc + '">'
        + '        <input id="Itens_' + countList + '__Qty" name="Itens[' + countList + '].Qty" type="hidden" value="' + txtQty + '">'
        + '        <input id="Itens_' + countList + '__DtNecessidade" name="Itens[' + countList + '].DtNecessidade" type="hidden" value="' + txtDt + '">'
        + '        <input id="Itens_' + countList + '__Delete" name="Itens[' + countList + '].Delete" type="hidden" value="False">'
        + '        <button type="button" class="btn btn-default btn-small no-margin" onclick="modalItem(\'edit\',' + countList + ')">'
        + '            <span class="fa fa-pencil"></span>'
        + '        </button>'
        + '        <button type="button" class="btn btn-default btn-small no-margin" onclick="modalItem(\'delete\',' + countList + ')">'
        + '            <span class="fa fa-trash"></span>'
        + '        </button>'
        + '    </td>'
        + '</tr>');

    $("#countList").val(parseInt(countList) + 1);
    clear('add');
    $("#modal-novo").modal('hide');
    loadingOff();
}

function EditClick() {

    if (!checkItens('edit')) return;

    var txtInsumoDesc = $("#Edit-InsumoCode option:selected").text().split('|')[1];

    var txtInsumo = $("#Edit-InsumoCode").val();
    var txtQty = $("#Edit-Qty").val();
    var txtDt = $("#Edit-DtNecessidade").val();

    var fullDate = new Date(txtDt + ' 00:00');
    var twoDigitMonth = ((fullDate.getMonth().length + 1) === 1) ? (fullDate.getMonth() + 1) : '0' + (fullDate.getMonth() + 1);
    var txtDtShow = fullDate.getDate() + "/" + twoDigitMonth + "/" + fullDate.getFullYear();


    var countList = $("#countList").val();

    if (countList === "0") {
        $("#dtInsumo > tbody > tr").hide();
    }

    $("#modal-delete-Id").val($("#modal-edit-Id").val());
    removeTb();

    $("#dtInsumo").append(
        '<tr id="tr-' + countList + '">'
        + '    <td>' + txtInsumo + '</td>'
        + '    <td>' + txtInsumoDesc + '</td>'
        + '    <td>' + txtQty + '</td>'
        + '    <td>' + txtDtShow + '</td>'
        + '    <td style="text-align: center">'
        + '        <input id="Itens_' + countList + '__InsumoCode" name="Itens[' + countList + '].InsumoCode" type="hidden" value="' + txtInsumo + '">'
        + '        <input id="Itens_' + countList + '__InsumoName" name="Itens[' + countList + '].InsumoName" type="hidden" value="' + txtInsumoDesc + '">'
        + '        <input id="Itens_' + countList + '__Qty" name="Itens[' + countList + '].Qty" type="hidden" value="' + txtQty + '">'
        + '        <input id="Itens_' + countList + '__DtNecessidade" name="Itens[' + countList + '].DtNecessidade" type="hidden" value="' + txtDt + '">'
        + '        <input id="Itens_' + countList + '__Delete" name="Itens[' + countList + '].Delete" type="hidden" value="False">'
        + '        <button type="button" class="btn btn-default btn-small no-margin" onclick="modalItem(\'edit\',' + countList + ')">'
        + '            <span class="fa fa-pencil"></span>'
        + '        </button>'
        + '        <button type="button" class="btn btn-default btn-small no-margin" onclick="modalItem(\'delete\',' + countList + ')">'
        + '            <span class="fa fa-trash"></span>'
        + '        </button>'
        + '    </td>'
        + '</tr>');
    table.columns.adjust();

    $("#countList").val(parseInt(countList) + 1);
    clear('edit');
    $("#modal-edit").modal('hide');
    loadingOff();
}

function refresh() {
    window.location.reload(true);
}

var table;
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
}


function clear(typeClear) {
    var fullDate = new Date();
    //convert month to 2 digits
    var twoDigitMonth = ((fullDate.getMonth().length + 1) === 1) ? (fullDate.getMonth() + 1) : '0' + (fullDate.getMonth() + 1);

    var currentDate = fullDate.getFullYear() + "-" + twoDigitMonth + "-" + fullDate.getDate();

    if (typeClear === "add") {
        $('#Add-InsumoCode option').first().prop('selected', true);
        $("#Add-Qty").val(1);
        $("#Add-DtNecessidade").val(currentDate);

        $("#lblQtySaidaTxt").text(zero);
        $("#lblQtyTotalTxt").text(zero);
        $("#lblQtyEstoqueTxt").text(zero);
    }
    else {
        $('#Edit-InsumoCode option').first().prop('selected', true);
        $("#Edit-Qty").val(1);
        $("#Edit-DtNecessidade").val(currentDate);
    }
}



function modalItem(typeModel, id) {

    if (typeModel !== null && typeModel !== "") {

        if (id !== null && id !== "") {

            if (typeModel === "delete") {
                $("#modal-delete-Id").val(id);
                $("#modal-delete").modal('show');
            }
            else if (typeModel === "edit") {
                $("#modal-edit-Id").val(id);
                $("#Edit-InsumoCode").val($("#Itens_" + id + "__InsumoCode").val());
                $("#Edit-Qty").val($("#Itens_" + id + "__Qty").val());
                $("#Edit-DtNecessidade").val($("#Itens_" + id + "__DtNecessidade").val());
                $("#modal-edit").modal('show');
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

function removeTb() {
    var id = $("#modal-delete-Id").val();

    if (id !== null && id !== "") {
        $('#Itens_' + id + '__Delete').val("True");
        $('#tr-' + id).closest("tr").fadeOut(100);
        $('.modal-backdrop').css("visibility", "hidden");
        $("#modal-delete-Id").val("");
        $("#modal-delete").modal('hide');
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

function ItemOnHand() {
    var bplId = $("#BPLIdCode").val().trim();
    var bplIdDesc = $("#BPLIdCode option:selected").text();
    var txtInsumo = $("#Add-InsumoCode").val();

    if ((bplId !== null && bplId !== "") && (txtInsumo !== null && txtInsumo !== "")) {
        loadingOn();

        $.getJSON(
            "/SaidaMateriais/CallGetInsumoOnHand?itemCode=" + txtInsumo + "&bplId=" + bplId,
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
                    $("#lblQtyEstoqueTxt").text(data.OnHand.toFixed(2));
                    CalculateUsedOnHand();
                }

                if (data.AvgPrice !== null) {
                    $("#lblCustoTxt").text(data.AvgPrice.toFixed(2));
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

function CalculateUsedOnHand() {
    var txtQty = parseFloat($("#Add-Qty").val());
    $("#lblQtySaidaTxt").text(txtQty.toFixed(2));

    var qtyEstoque = parseFloat($("#lblQtyEstoqueTxt").text());
    if (qtyEstoque > 0) {
        var total = qtyEstoque - txtQty;
        $("#lblQtyTotalTxt").text(total.toFixed(2));
    }
    else {
        $("#lblQtyTotalTxt").text(zero);
    }

    loadingOff();
}


function ClickModelAdd() {

    if ($('#BPLIdCode').find('option').length > 0) {

        var bplId = $("#BPLIdCode").val().trim();
        if (bplId !== null && bplId !== '') {

            $("#lblQtySaidaTxt").text(zero);
            $("#lblQtyTotalTxt").text(zero);
            $("#lblQtyEstoqueTxt").text(zero);
            $("#lblCustoTxt").text(zero);

            $("#modal-novo").modal('show');
            ItemOnHand();
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

function clearCliente() {
    $('#ClienteCode').val("");
    $('#ClienteName').val("");
    $('#txtClienteName').val("");
    RefreshChosen();
}