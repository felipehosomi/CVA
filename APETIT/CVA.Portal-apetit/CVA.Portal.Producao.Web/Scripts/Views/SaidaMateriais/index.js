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

    if (bplId !== null && bplId !== "") {

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

                RefreshChosen();
                loadingOff();
            });

        if ($('#BPLIdCode').find('option').length > 0) {

            loadingOn();
            $.getJSON(
                "/SaidaMateriais/CallGetInsumo?BPLId=" + bplId,
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
                            text: selectList.Value + ' | ' + selectList.Text
                        }));
                    });

                    RefreshChosen();
                    loadingOff();
                });
        }
    }
    else {
        $('#txtClienteName').empty();
        RefreshChosen();
    }
}


function checkItens(typeCheck) {

    var txtInsumo = "";
    var txtQty = 0;
    if (typeCheck === "add") {
        txtInsumo = $("#Add-InsumoCode").val();
        txtQty = $("#Add-Qty").val();
    }
    else {
        txtInsumo = $("#Edit-InsumoCode").val();
        txtQty = $("#Edit-Qty").val();
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

    return true;
}

function AddClick() {

    if (!checkItens('add')) return;

    var txtInsumo = $("#Add-InsumoCode").val();
    var txtQty = $("#Add-Qty").val();
    var txtInsumoDesc = $("#Add-InsumoCode option:selected").text().split('|')[1];

    var countList = $("#countList").val();

    if (countList === "0") {
        $("#dtInsumo > tbody > tr").hide();
    }

    $("#dtInsumo").append(
        '<tr id="tr-' + countList + '">' 
        +'    <td>' + txtInsumo + '</td>'
        + '    <td>' + txtInsumoDesc + '</td>'
        + '    <td>' + txtQty + '</td>'
        + '    <td style="text-align: center">'
        + '        <input id="Itens_' + countList + '__InsumoCode" name="Itens[' + countList + '].InsumoCode" type="hidden" value="' + txtInsumo + '">'
        + '        <input id="Itens_' + countList + '__InsumoName" name="Itens[' + countList + '].InsumoName" type="hidden" value="' + txtInsumoDesc + '">'
        + '        <input data-val="true" data-val-number="The field Quantidade must be a number." data-val-required="The Quantidade field is required." id="Itens_' + countList + '__Qty" name="Itens[' + countList + '].Qty" type="hidden" value="' + txtQty + '">'
        + '        <input data-val="true" data-val-required="The Delete field is required." id="Itens_' + countList + '__Delete" name="Itens[' + countList + '].Delete" type="hidden" value="False">'
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
    clear('add');
    $("#modal-novo").modal('hide');
    loadingOff();
}

function EditClick() {

    if (!checkItens('edit')) return;

    var txtInsumo = $("#Edit-InsumoCode").val();
    var txtQty = $("#Edit-Qty").val();
    var txtInsumoDesc = $("#Edit-InsumoCode option:selected").text().split('|')[1];

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
        + '    <td style="text-align: center">'
        + '        <input id="Itens_' + countList + '__InsumoCode" name="Itens[' + countList + '].InsumoCode" type="hidden" value="' + txtInsumo + '">'
        + '        <input id="Itens_' + countList + '__InsumoName" name="Itens[' + countList + '].InsumoName" type="hidden" value="' + txtInsumoDesc + '">'
        + '        <input data-val="true" data-val-number="The field Quantidade must be a number." data-val-required="The Quantidade field is required." id="Itens_' + countList + '__Qty" name="Itens[' + countList + '].Qty" type="hidden" value="' + txtQty + '">'
        + '        <input data-val="true" data-val-required="The Delete field is required." id="Itens_' + countList + '__Delete" name="Itens[' + countList + '].Delete" type="hidden" value="False">'
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
    table=$('#dtInsumo').DataTable({
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
    if (typeClear === "add") {
        $('#Add-InsumoCode option').first().prop('selected', true);
        $("#Add-Qty").val(1);
    }
    else {
        $('#Edit-InsumoCode option').first().prop('selected', true);
        $("#Edit-Qty").val(1);
    }
}



function modalItem(typeModel,id)
{

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


function ItemOnHand() 
{   
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