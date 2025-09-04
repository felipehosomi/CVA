$(document).ready(function () {
    SetDataTable();

    $("#btnFiltro").click(function () {
        $("#btnFiltro").button('loading');

        var dataDe = $("#tbxDataDe").val();
        var dataAte = $("#tbxDataAte").val();
        var status = $("#ddlStatus").val();
        var pedido = $("#tbxPedido").val();
        var op = $("#tbxOP").val();
        var codigoItem = $("#tbxCodigoItem").val();

        var divOPChild = document.getElementById("divOPChild");
        var divOP = document.getElementById("divOP");
        divOP.removeChild(divOPChild);
        $.ajax
        ({
            url: '/InspecaoOP/TabelaOPs',
            data: {
                "dataDe": dataDe,
                "dataAte": dataAte,
                "status": status,
                "pedido": pedido,
                "op": op,
                "codigoItem": codigoItem
            },
            success: function (retorno) {
                $("#divOP").html(retorno);
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

    $(document).on('blur', '#tbxNrOP', function () {
        var nrOP = $("#tbxNrOP").val().trim();
        var cbxEtapa = $("#cbxEtapa");

        $.getJSON(
               "/InspecaoOP/GetByOP",
               { nrOP: nrOP },
               function (data) {
                   cbxEtapa.empty();
                   if (data.length == 0) {
                        new PNotify({
                       title: 'Alerta',
                           text: "OP ou etapa(s) não encontrados",
                               type: 'error',
                                   styling: 'bootstrap3'
                               });
                       return;
                   }

                   $.each(data, function (index, selectList) {
                       cbxEtapa.append($("<option/>", {
                           value: selectList.Value,
                           text: selectList.Text
                       }));
                   });
               });
    });

    $(document).on('click', '#btnNovo', function () {
        var nrOP = $("#tbxNrOP").val().trim();
        var codEtapa = $("#cbxEtapa").val();

        if (nrOP == "") {
            new PNotify({
                title: 'Alerta',
                text: "Informe o Nr. da OP",
                type: 'error',
                styling: 'bootstrap3'
            });
            return;
        }

        if (codEtapa == "") {
            new PNotify({
                title: 'Alerta',
                text: "Informe a etapa",
                type: 'error',
                styling: 'bootstrap3'
            });
            return;
        }

        window.location.href = '/FichaInspecao/CreateManual?nrOP=' + nrOP + '&codEtapaOP=' + codEtapa;
    });
});

function SetDataTable() {
    $('#tblOP').DataTable({
        info: false,
        paging: true,
        searching: false,
        ordering: true,
        pagingType: "numbers",
        "lengthChange": false,
    });
}