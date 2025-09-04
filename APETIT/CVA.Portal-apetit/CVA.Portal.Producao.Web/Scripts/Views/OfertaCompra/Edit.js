function duplicateLine(id) {
    $.ajax({
        url: this.href,
        cache: false,
        success: function () {
            var row = document.getElementById(id);
            var table = document.getElementById("tableItens");
            var clone = row.cloneNode(true);

            clone.id = table.rows.length - 1;
            clone.innerHTML = clone.innerHTML.split('(' + id + ')').join('(' + clone.id + ')').split('[' + id + ']').join('[' + clone.id + ']').split('_' + id + '__').join('_' + clone.id + '__').split('type="hidden" value="' + id + '">').join('type="hidden" value="' + clone.id + '">');
            table.appendChild(clone);

            $("#Itens_" + clone.id + "__newRegister").val(true);

            $(".priceMask").inputmask('decimal', {
                'alias': 'numeric',
                'groupSeparator': ',',
                'autoGroup': true,
                'digits': 2,
                'radixPoint': ".",
                'digitsOptional': false,
                'allowMinus': false,
                'prefix': 'R$ ',
                'placeholder': ''
            });
        }
    });

    return false;
}

function calculaTotais(index) {
    var vlr = document.getElementById("Itens_" + index + "__Price").value.replace("R$ ", "").split(",").join("");
    var qtde = document.getElementById("Itens_" + index + "__Quantity").value;

    document.getElementById("Itens_" + index + "__ValorTotal").value = vlr * qtde;
}

function sendEmail(tipoEmail) {
    if (tipoEmail == 5) {
        $.ajax(
        {
            type: 'GET',
            url: '/OfertaCompra/SendEmail?tipoEmail=' + tipoEmail + '&sCotacaoCompra=' + document.getElementById("DocNum").value + '&sNomeContato=' + document.getElementById("CompradorNome").value + '&sEmailContato=' + document.getElementById("CompradorEmail").value +
                '&sObsRevisao=' + document.getElementById("ObsRevisao").value,
            dataType: 'html',
            cache: false,
            async: true,
            success: function (data) {
                location.reload();
            }
        });
    } else {
        $.ajax(
        {
            type: 'GET',
            url: '/OfertaCompra/SendEmail?tipoEmail=' + tipoEmail + '&sCotacaoCompra=' + document.getElementById("DocNum").value + '&sNomeContato=' + document.getElementById("FornecedorNome").value + '&sEmailContato=' + document.getElementById("FornecedorEmail").value +
                '&sObsRevisao=' + document.getElementById("ObsRevisao").value,
            dataType: 'html',
            cache: false,
            async: true,
            success: function (data) {
                location.reload();
            }
        });
    }   
}

$(".priceMask").inputmask('decimal', {
    'alias': 'numeric',
    'groupSeparator': ',',
    'autoGroup': true,
    'digits': 2,
    'radixPoint': ".",
    'digitsOptional': false,
    'allowMinus': false,
    'prefix': 'R$ ',
    'placeholder': ''
});