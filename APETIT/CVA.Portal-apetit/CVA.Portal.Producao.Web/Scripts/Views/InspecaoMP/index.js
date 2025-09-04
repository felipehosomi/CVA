$(document).ready(function () {
    SetDataTable();

    $("#btnFiltro").click(function () {
        $("#btnFiltro").button('loading');

        var dataDe = $("#tbxDataDe").val();
        var dataAte = $("#tbxDataAte").val();
        var status = $("#ddlStatus").val();
        var nf = $("#tbxNF").val();
        var ItemCode = $("#tbxCodigoItem").val();

        var divRecChild = document.getElementById("divRecChild");
        var divRec = document.getElementById("divRec");
        divRec.removeChild(divRecChild);
        $.ajax
        ({
            url: '/InspecaoMP/TabelaRecebimentos',
            data: {
                "dataDe": dataDe,
                "dataAte": dataAte,
                "status": status,
                "nf": nf,
                "itemCode": ItemCode
            },
            success: function (retorno) {
                $("#divRec").html(retorno);
                SetDataTable();
                $("#btnFiltro").button('reset');
            }
        });
    });

    $('#tbxData').keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            $("#btnFiltro").click();
        }
    });
});

function SetDataTable() {
    $('#tblRec').DataTable({
        info: false,
        paging: true,
        searching: false,
        ordering: true,
        pagingType: "numbers",
        "lengthChange": false,
    });
}

$('#submitButton').click(function () {
    $('input:invalid').each(function () {
        var $closest = $(this).closest('.tab-pane');
        var id = $closest.attr('id');
        $('.nav a[href="#' + id + '"]').tab('show');
        return false;
    });
});