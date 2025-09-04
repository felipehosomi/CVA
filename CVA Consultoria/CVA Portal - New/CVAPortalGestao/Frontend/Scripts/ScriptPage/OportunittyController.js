cvaGestao.controller('OportunittyController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {
    $scope.OnLoad = function (model) {
        $http({
            method: 'GET',
            url: '/Oportunidade/GetName'
        }).success(function (result) {
            $scope.UserName = result;
        });

        CreateModel();
        CreateArrays();
        LoadArrays();


        $scope.Oportunidade.ValorOportunidade = 0.0;
        $scope.Oportunidade.CustoOrcado = 0.0;
        $scope.Oportunidade.HorasOrcadas = 0.0;
        $scope.Oportunidade.IngressoLiquido = 0.0;
        $scope.Oportunidade.RiscoGerenciavel = 0.0;
        $scope.Oportunidade.IngressoTotal = 0.0;
        $scope.Oportunidade.Status = {};
        $scope.Oportunidade.ProjectStep = {};
        $scope.Pricing.PorcentagemBackoffice = 18;
        $scope.Pricing.PorcentagemRisco = 10;
        $scope.Pricing.PorcentagemMargem = 30;
        $scope.Pricing.PorcentagemComissao = 2;
        $scope.Pricing.PorcentagemImposto = 18.33;
        $scope.HotelDiarias = 0;
        $scope.HotelValor = 150;
        $scope.KmTrechos = 0;
        $scope.KmDistancia = 0;
        $scope.KmValor = 0.67;
        $scope.AlimentacaoDias = 0;
        $scope.AlimentacaoValor = 45;
        $scope.DeslocamentoHoras = 0;
        $scope.DeslocamentoValor = 0;
        $scope.AereoTrechos = 0;
        $scope.AereoValor = 0;

        //Editando oportunidade
        if (model != null) {
            model.DataPrevista = new Date(Date.parse(model.DataPrevista));


            $scope.Oportunidade = model;
            $scope.Oportunidade.Status.Id = model.Status.Id.toString();


            if (model.Pricing != null) {
                $scope.Pricing = model.Pricing;
                $scope.Pricing.PorcentagemBackoffice = 18.00;
                $scope.Pricing.PorcentagemRisco = 10.00;
                $scope.Pricing.PorcentagemMargem = 30.00;
                $scope.Pricing.PorcentagemComissao = 2.00;
                $scope.Pricing.PorcentagemImposto = 18.33;
            }
            
            if (model.Pricing.Itens != null)
                $scope.ItensPricing = model.Pricing.Itens;

            $scope.Codigo = $scope.Oportunidade.Codigo + ' - ' + $scope.Oportunidade.Tag + ' ' + $scope.Oportunidade.Nome;
        }
    }

    function CreateModel() {
        $scope.Oportunidade = {};

        $scope.Oportunidade.Status = {};
        $scope.Oportunidade.Cliente = {};
        $scope.Oportunidade.Temperatura = {};
        $scope.Oportunidade.TipoProjeto = {};
        $scope.Oportunidade.Vendedor = {};
        $scope.Oportunidade.Pricing = {};
        $scope.Oportunidade.Pricing.Itens = [];

        $scope.Pricing = {};
    }

    function CreateArrays() {
        $scope.TiposProjeto = [];
        $scope.Clientes = [];
        $scope.Temperaturas = [];
        $scope.Vendedores = [];
        $scope.Especialidades = [];
        $scope.ItensPricing = [];


        $scope.SpecialtyLine = -1;
        $scope.TotalHotel = 0.0;
        $scope.TotalQuilometragem = 0.0;
        $scope.TotalAlimentacao = 0.0;
        $scope.TotalDeslocamento = 0.0;
        $scope.TotalAereo = 0.0;
    }

    function LoadArrays() {
        $http({
            method: 'GET',
            url: '/TiposProjeto/Get_All'
        }).success(function (result) {
            $scope.TiposProjeto = result;
            document.getElementById('load-01').hidden = true;
        });

        $http({
            method: 'GET',
            url: '/Cliente/LoadCombo'
        }).success(function (clients) {
            $scope.Clientes = clients;
            document.getElementById('load-02').hidden = true;
            $scope.ClientSelected();
        });

        $http({
            method: 'GET',
            url: '/Colaborador/LoadCombo'
        }).success(function (collaborators) {
            $scope.Vendedores = collaborators;
            document.getElementById('load-03').hidden = true;
        });

        $http({
            method: 'GET',
            url: '/Especialidade/Get_All'
        }).success(function (result) {
            $scope.Especialidades = result;
            document.getElementById('load-04').hidden = true;
        });


        $http({
            method: 'GET',
            url: '/Oportunidade/GetPercents  '
        }).success(function (percents) {
            $scope.Temperaturas = percents;
        });
    }


    $scope.ClientSelected = function () {
        if ($scope.Clientes != undefined && $scope.Clientes != null)
            for (var i = 0; i < $scope.Clientes.length; i++) {
                if ($scope.Clientes[i].Id == $scope.Oportunidade.Cliente.Id) {
                    $scope.Oportunidade.Tag = '[' + $scope.Clientes[i].Tag + ']';
                    $scope.GenerateName();
                    break;
                }
            }
    }
    /*--Geral--*/

    $scope.GenerateCode = function () {
        if ($scope.Oportunidade.Id == null || $scope.Oportunidade.Id == undefined) {
            $http({
                method: 'GET',
                url: '/Oportunidade/Generate_NewCode?id=' + $scope.Oportunidade.TipoProjeto.Id
            }).success(function (result) {
                $scope.Oportunidade.Codigo = result;
                $scope.GenerateName();
            });
        }
    }

    $scope.GenerateName = function () {
        if ($scope.Oportunidade.Nome == undefined)
            $scope.Oportunidade.Nome = '';

        if ($scope.Oportunidade.Codigo == undefined && $scope.Oportunidade.Tag == undefined) {
            $scope.Codigo = $scope.Oportunidade.Nome;
            return false;
        }

        else if ($scope.Oportunidade.Codigo == undefined) {
            $scope.Codigo = ' - ' + $scope.Oportunidade.Tag + ' ' + $scope.Oportunidade.Nome;
            return false;
        }

        else if ($scope.Oportunidade.Tag == undefined) {
            $scope.Codigo = $scope.Oportunidade.Codigo + ' - ' + $scope.Oportunidade.Nome;
            return false;
        }

        else {
            $scope.Codigo = $scope.Oportunidade.Codigo + ' - ' + $scope.Oportunidade.Tag + ' ' + $scope.Oportunidade.Nome;
            return false;
        }
    }

    $scope.FillSpecialtyValue = function (id) {
        for (var i = 0; i < $scope.Especialidades.length; i++) {
            if ($scope.Especialidades[i].Id == id) {
                $scope.EspecialidadeValor = $scope.Especialidades[i].Value;
                $scope.DeslocamentoValor = $scope.Especialidades[i].Value;
                break;
            }
        }
        $scope.CalculateExpenses();
    }

    $scope.AddPricingItens = function () {
        if ($scope.Especialidade == undefined || $scope.Especialidade.Id == null || $scope.Especialidade.Id == '') {
            noty({ text: 'Selecione uma especialidade', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.EspecialidadeValor == undefined || $scope.EspecialidadeValor == null || $scope.EspecialidadeValor == '') {
            noty({ text: 'Informe o valor', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.EspecialidadeHoras == undefined || $scope.EspecialidadeHoras == null || $scope.EspecialidadeHoras == '') {
            noty({ text: 'Informe o total de horas', layout: 'topRight', type: 'error' });
            return false;
        }

        $scope.CalculateExpenses();
        var line = {};

        // -- Recursos -- //
        line.Especialidade = {};
        line.Especialidade.Id = $scope.Especialidade.Id;

        for (var i = 0; i < $scope.Especialidades.length; i++) {
            if ($scope.Especialidades[i].Id == $scope.Especialidade.Id) {
                line.Especialidade.Name = $scope.Especialidades[i].Name;
                break;
            }
        }
        line.EspecialidadeValor = $scope.FormatValue($scope.EspecialidadeValor);
        line.EspecialidadeHoras = $scope.FormatValue($scope.EspecialidadeHoras);
        line.EspecialidadeCusto = $scope.FormatValue(line.EspecialidadeValor * line.EspecialidadeHoras);


        // -- Despesas -- //
        line.HotelDiarias = $scope.HotelDiarias;
        line.HotelValor = $scope.HotelValor;
        line.KmTrechos = $scope.KmTrechos;
        line.KmDistancia = $scope.KmDistancia;
        line.KmValor = $scope.KmValor;
        line.AlimentacaoDias = $scope.AlimentacaoDias;
        line.AlimentacaoValor = $scope.AlimentacaoValor;
        line.DeslocamentoHoras = $scope.DeslocamentoHoras;
        line.DeslocamentoValor = $scope.DeslocamentoValor;
        line.AereoTrechos = $scope.AereoTrechos;
        line.AereoValor = $scope.AereoValor;
        line.HotelTotal = $scope.TotalHotel;
        line.KmTotal = $scope.TotalQuilometragem;
        line.AlimentacaoTotal = $scope.TotalAlimentacao;
        line.DeslocamentoTotal = $scope.TotalDeslocamento;
        line.AereoTotal = $scope.TotalAereo;
        line.RecursoDespesas = $scope.FormatValue(line.HotelTotal + line.KmTotal + line.AlimentacaoTotal + line.DeslocamentoTotal + line.AereoTotal);



        // -- Porcentagens -- //
        line.ValorBackoffice = $scope.FormatValue(line.EspecialidadeCusto * ($scope.FormatValue($scope.Pricing.PorcentagemBackoffice) / 100));
        line.ValorRisco = $scope.FormatValue(line.EspecialidadeCusto * ($scope.FormatValue($scope.Pricing.PorcentagemRisco) / 100));
        line.ValorMargem = $scope.FormatValue(line.EspecialidadeCusto * ($scope.FormatValue($scope.Pricing.PorcentagemMargem) / 100));
        line.ValorComissao = $scope.FormatValue((line.EspecialidadeCusto + line.ValorBackoffice + line.ValorRisco + line.ValorMargem) * ($scope.FormatValue($scope.Pricing.PorcentagemComissao) / 100));



        // -- Totais -- // 
        line.RecursoSubTotal = $scope.FormatValue(line.EspecialidadeCusto + line.ValorBackoffice + line.ValorRisco + line.ValorMargem + line.ValorComissao);
        line.RecursoValorComDespesas = $scope.FormatValue(line.RecursoSubTotal + line.RecursoDespesas);
        line.RecursoValorComImpostos = $scope.FormatValue($scope.FormatValue(line.RecursoValorComDespesas / (1 - ($scope.FormatValue($scope.Pricing.PorcentagemImposto) / 100))));

        line.ValorImposto = line.RecursoValorComImpostos - line.RecursoValorComDespesas;

        line.RecursoValorHorasSemDespesa = $scope.FormatValue(line.RecursoValorComDespesas / line.EspecialidadeHoras);
        line.RecursoValorHorasComDespesa = $scope.FormatValue(line.RecursoValorComImpostos / line.EspecialidadeHoras);

        var index = $scope.ItensPricing.length;

        if ($scope.SpecialtyLine == -1)
            $scope.ItensPricing[index] = line;

        else
            $scope.ItensPricing[$scope.SpecialtyLine] = line;

        $scope.SpecialtyLine = -1;
        $scope.Especialidade = {};
        $scope.EspecialidadeHoras = "";
        $scope.EspecialidadeValor = "";
        $scope.Calculate_OpportunityValues();
    }

    $scope.EditPricingItens = function (index) {
        $scope.SpecialtyLine = index;
        $scope.Especialidade = {};
        $scope.Especialidade.Id = $scope.ItensPricing[index].Especialidade.Id;

        $scope.EspecialidadeValor = $scope.ItensPricing[index].EspecialidadeValor;
        $scope.EspecialidadeHoras = $scope.ItensPricing[index].EspecialidadeHoras;


        $scope.HotelDiarias = $scope.ItensPricing[index].HotelDiarias;
        $scope.HotelValor = $scope.ItensPricing[index].HotelValor;
        $scope.TotalHotel = ($scope.HotelDiarias * $scope.HotelValor);
  
        $scope.KmTrechos = $scope.ItensPricing[index].KmTrechos;
        $scope.KmDistancia = $scope.ItensPricing[index].KmDistancia;
        $scope.KmValor = $scope.ItensPricing[index].KmValor;
        $scope.TotalQuilometragem = ($scope.KmTrechos * $scope.KmDistancia * $scope.KmValor);
  
        $scope.AlimentacaoDias = $scope.ItensPricing[index].AlimentacaoDias;
        $scope.AlimentacaoValor = $scope.ItensPricing[index].AlimentacaoValor;
        $scope.TotalAlimentacao = ($scope.AlimentacaoDias * $scope.AlimentacaoValor);
  
        $scope.DeslocamentoHoras = $scope.ItensPricing[index].DeslocamentoHoras;
        $scope.DeslocamentoValor = $scope.ItensPricing[index].DeslocamentoValor;
        $scope.TotalDeslocamento = ($scope.DeslocamentoHoras * $scope.DeslocamentoValor);
 
        $scope.AereoTrechos = $scope.ItensPricing[index].AereoTrechos;
        $scope.AereoValor = $scope.ItensPricing[index].AereoValor;
        $scope.TotalAereo = ($scope.AereoTrechos * $scope.AereoValor);


        noty({ text: 'Ajuste os valores do recurso', layout: 'topRight', type: 'warning' });
    }

    $scope.RemovePricingItens = function (index) {
        $scope.ItensPricing.splice(index, 1);
        $scope.Calculate_OpportunityValues();
    }

    $scope.Calculate_OpportunityValues = function () {
        //Seta os valores para início do cálculo
        $scope.Oportunidade.ValorOportunidade = 0.0;
        $scope.Oportunidade.CustoOrcado = 0.0;
        $scope.Oportunidade.HorasOrcadas = 0.0;
        $scope.Oportunidade.IngressoLiquido = 0.0;
        $scope.Oportunidade.RiscoGerenciavel = 0.0;
        $scope.Oportunidade.IngressoTotal = 0.0;
        $scope.IngressoLiquidoPorcentagem = 0;
        $scope.RiscoGerenciavelPorcentagem = 0;
        $scope.IngressoTotalPorcentagem = 0;
        $scope.TotalComDespesas = 0;
        //Soma valores para cada recurso inserido
        for (var i = 0; i < $scope.ItensPricing.length; i++) {
            $scope.ItensPricing[i].RecursoSubTotal = $scope.FormatValue($scope.ItensPricing[i].EspecialidadeCusto + $scope.ItensPricing[i].ValorBackoffice + $scope.ItensPricing[i].ValorRisco + $scope.ItensPricing[i].ValorMargem + $scope.ItensPricing[i].ValorComissao);
            $scope.ItensPricing[i].RecursoDespesas = $scope.FormatValue($scope.ItensPricing[i].HotelTotal + $scope.ItensPricing[i].KmTotal + $scope.ItensPricing[i].AlimentacaoTotal + $scope.ItensPricing[i].DeslocamentoTotal + $scope.ItensPricing[i].AereoTotal);
            $scope.ItensPricing[i].RecursoValorComDespesas = $scope.FormatValue($scope.ItensPricing[i].RecursoSubTotal + $scope.ItensPricing[i].RecursoDespesas);
            $scope.ItensPricing[i].RecursoValorComImpostos = $scope.FormatValue($scope.ItensPricing[i].RecursoValorComDespesas / (1 - ($scope.FormatValue($scope.Pricing.PorcentagemImposto) / 100)));
            $scope.ItensPricing[i].RecursoValorHorasSemDespesa = $scope.FormatValue(($scope.ItensPricing[i].RecursoSubTotal / (1 - ($scope.FormatValue($scope.Pricing.PorcentagemImposto) / 100))) / $scope.ItensPricing[i].EspecialidadeHoras);
            $scope.ItensPricing[i].RecursoValorHorasComDespesa = $scope.FormatValue($scope.ItensPricing[i].RecursoValorComImpostos / $scope.ItensPricing[i].EspecialidadeHoras);

            $scope.ItensPricing[i].ValorImposto = $scope.FormatValue($scope.FormatValue($scope.ItensPricing[i].RecursoValorComImpostos - $scope.ItensPricing[i].RecursoValorComDespesas));


            //Soma os valores dos recursos
            $scope.Oportunidade.CustoOrcado += $scope.ItensPricing[i].EspecialidadeCusto + $scope.ItensPricing[i].RecursoDespesas;
            $scope.Oportunidade.HorasOrcadas += $scope.ItensPricing[i].EspecialidadeHoras;
            $scope.Oportunidade.IngressoLiquido += $scope.ItensPricing[i].ValorBackoffice + $scope.ItensPricing[i].ValorMargem;
            $scope.Oportunidade.RiscoGerenciavel += $scope.ItensPricing[i].ValorRisco;
            $scope.Oportunidade.IngressoTotal += $scope.ItensPricing[i].ValorBackoffice + $scope.ItensPricing[i].ValorMargem + $scope.ItensPricing[i].ValorRisco;
            $scope.Oportunidade.ValorOportunidade += $scope.ItensPricing[i].RecursoValorComImpostos;

            //Arredonda o valor somado
            $scope.Oportunidade.ValorOportunidade = $scope.FormatValue($scope.Oportunidade.ValorOportunidade);
            $scope.Oportunidade.CustoOrcado = $scope.FormatValue($scope.Oportunidade.CustoOrcado);
            $scope.Oportunidade.HorasOrcadas = $scope.FormatValue($scope.Oportunidade.HorasOrcadas);
            $scope.Oportunidade.IngressoLiquido = $scope.FormatValue($scope.Oportunidade.IngressoLiquido);
            $scope.Oportunidade.RiscoGerenciavel = $scope.FormatValue($scope.Oportunidade.RiscoGerenciavel);


            $scope.TotalComDespesas += $scope.ItensPricing[i].RecursoValorComDespesas;


            $scope.IngressoLiquidoPorcentagem = $scope.FormatValue(($scope.Oportunidade.IngressoLiquido / $scope.TotalComDespesas) * 100);
            $scope.RiscoGerenciavelPorcentagem = $scope.FormatValue(($scope.Oportunidade.RiscoGerenciavel / $scope.TotalComDespesas) * 100);
            $scope.IngressoTotalPorcentagem = $scope.FormatValue(($scope.Oportunidade.IngressoTotal / $scope.TotalComDespesas) * 100);

        }
    }

    $scope.Calculate_Percentages = function () {
        for (var i = 0; i < $scope.ItensPricing.length; i++) {
            $scope.ItensPricing[i].ValorBackoffice = $scope.FormatValue($scope.ItensPricing[i].EspecialidadeCusto * ($scope.FormatValue($scope.Pricing.PorcentagemBackoffice) / 100));
            $scope.ItensPricing[i].ValorRisco = $scope.FormatValue($scope.ItensPricing[i].EspecialidadeCusto * ($scope.FormatValue($scope.Pricing.PorcentagemRisco) / 100));
            $scope.ItensPricing[i].ValorMargem = $scope.FormatValue($scope.ItensPricing[i].EspecialidadeCusto * ($scope.FormatValue($scope.Pricing.PorcentagemMargem) / 100));
            $scope.ItensPricing[i].ValorComissao = $scope.FormatValue(($scope.ItensPricing[i].EspecialidadeCusto + $scope.ItensPricing[i].ValorBackoffice + $scope.ItensPricing[i].ValorRisco + $scope.ItensPricing[i].ValorMargem) * ($scope.FormatValue($scope.Pricing.PorcentagemComissao) / 100));
            $scope.ItensPricing[i].ValorImposto = $scope.FormatValue($scope.FormatValue($scope.ItensPricing[i].RecursoValorComDespesas / ($scope.FormatValue(1 - ($scope.FormatValue($scope.Pricing.PorcentagemImposto)) / 100))));
        }
        $scope.Calculate_OpportunityValues();
    }

    $scope.CalculateExpenses = function () {
        $scope.TotalHotel = $scope.FormatValue($scope.HotelDiarias) * $scope.FormatValue($scope.HotelValor);
        $scope.TotalQuilometragem = $scope.FormatValue($scope.FormatValue($scope.KmTrechos) * ($scope.FormatValue($scope.KmDistancia) * $scope.FormatValue($scope.KmValor)));
        $scope.TotalAlimentacao = $scope.FormatValue($scope.AlimentacaoDias) * $scope.FormatValue($scope.AlimentacaoValor);
        $scope.TotalDeslocamento = $scope.FormatValue($scope.DeslocamentoHoras) * $scope.FormatValue($scope.DeslocamentoValor);
        $scope.TotalAereo = $scope.FormatValue($scope.AereoTrechos) * $scope.FormatValue($scope.AereoValor);
    }



    $scope.FormatValue = function (value) {
        if (value != 0 && value != undefined) {
            value = value.toString();
            var ph = value.replace(',', '.');
            var number = Number(ph);
            number = parseFloat(number.toFixed(2));
            return number;
        }
        else {
            var number = Number(value);
            number = parseFloat(number.toFixed(2));
            return number;
        }
    }



    $scope.SaveOportunitty = function () {
        if (ValidateModel()) {
            console.log($scope.Pricing);
            $scope.Oportunidade.Pricing = $scope.Pricing;
            $scope.Oportunidade.Pricing.Itens = $scope.ItensPricing;

            $scope.IsProcessing = true;
            $http.post('/Oportunidade/Salvar', $scope.Oportunidade)
                .success(function (message) {
                    if (message.Success == null || message.Success == undefined) {
                        $('#message-error').text('');
                        $('#message-error').append(message.Error.Message);
                        $('#message-box-danger').addClass('open');
                    }
                    else {
                        $('#message-success').text('');
                        $('#message-success').append('Oportunidade criada com sucesso!');
                        $('#message-box-success').addClass('open');
                        $scope.Oportunidade.Id = message.Success.Code;
                    }
                    $scope.IsProcessing = false;
                });
        }

    }

    function ValidateModel() {
        if ($scope.Oportunidade.Convertida != 0 && $scope.Oportunidade.Convertida != null) {
            noty({ text: 'Esta oportunidade já foi convertida para projeto', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Oportunidade.Codigo == '' || $scope.Oportunidade.Codigo == null) {
            noty({ text: 'Informe o código da oportunidade', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Oportunidade.Nome == '' || $scope.Oportunidade.Nome == null) {
            noty({ text: 'Informe um nome para a oportunidade', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Oportunidade.Cliente.Id == '' || $scope.Oportunidade.Cliente.Id == null) {
            noty({ text: 'Selecione o cliente da oportunidade', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Oportunidade.Temperatura.Id == '' || $scope.Oportunidade.Temperatura.Id == null) {
            noty({ text: 'Selecione a temperatura da oportunidade', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Oportunidade.Status.Id == '' || $scope.Oportunidade.Status.Id == null) {
            noty({ text: 'Selecione o status do registro', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Oportunidade.TipoProjeto.Id == '' || $scope.Oportunidade.TipoProjeto.Id == null) {
            noty({ text: 'Selecione o tipo de projeto', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Oportunidade.DataPrevista == '' || $scope.Oportunidade.DataPrevista == null) {
            noty({ text: 'Informe a data de previsão para início do projeto', layout: 'topRight', type: 'error' });
            return false;
        }


        $scope.Pricing.PorcentagemBackoffice = $scope.FormatValue($scope.Pricing.PorcentagemBackoffice);
        $scope.Pricing.PorcentagemRisco = $scope.FormatValue($scope.Pricing.PorcentagemRisco);
        $scope.Pricing.PorcentagemMargem = $scope.FormatValue($scope.Pricing.PorcentagemMargem);
        $scope.Pricing.PorcentagemComissao = $scope.FormatValue($scope.Pricing.PorcentagemComissao);
        $scope.Pricing.PorcentagemImposto = $scope.FormatValue($scope.Pricing.PorcentagemImposto);

        return true;
    }



    $scope.ConvertToProject = function () {
        $scope.IsProcessing = true;
        if ($scope.Oportunidade.Id == 0 || $scope.Oportunidade.Id == null) {
            noty({ text: 'Por favor, salve a oportunidade antes de converte-la em projeto', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Oportunidade.Convertida != 0 && $scope.Oportunidade.Convertida != null) {
            noty({ text: 'Esta oportunidade já foi convertida para projeto', layout: 'topRight', type: 'error' });
            return false;
        }
        $http.post('/Oportunidade/CopyToProject', $scope.Oportunidade)
            .success(function (message) {
                if (message.Success == null || message.Success == undefined) {
                    $('#message-error').text('');
                    $('#message-error').append(message.Error.Message);
                    $('#message-box-danger').addClass('open');
                    $scope.IsProcessing = false;
                }
                else {
                    $('#message-success').text('');
                    $('#message-success').append('Projeto criado com sucesso!');
                    $('#message-box-success').addClass('open');
                }
            });
    }
}]);