function remover(controller, id) {
    $.ajax({
        url: '/' + controller + '/Delete',
        type: 'GET',
        dataType: 'json',
        data: {
            id: id
        },
        contentType: 'application/json; charset=utf-8',
        success: function (message) {
            $('#modal-delete-' + id).modal('toggle');
            $('.modal-backdrop').css("visibility", "hidden");

            if (message == null || message == "") {
                new PNotify({
                    delay: 3000,
                    title: 'Sucesso',
                    text: "Registro removido!",
                    type: 'notice',
                    styling: 'bootstrap3'
                });
                $('#tr-' + id).remove();
            }
            else {
                new PNotify({
                    delay: 3000,
                    title: 'Alerta',
                    text: "Erro ao remover: " + message, 
                    type: 'error',
                    styling: 'bootstrap3'
                });
            }
        },
        error: function () {
            alert("error");
        }
    });
}