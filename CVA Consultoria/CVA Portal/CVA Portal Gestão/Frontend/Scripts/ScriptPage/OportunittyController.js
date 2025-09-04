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
        $scope.Oportunidade.Pricing = {};
        $scope.Oportunidade.Pricing.ItensPricing = [];

        $scope.Oportunidade.ValorOportunidade = 0.0;
        $scope.Oportunidade.CustoOrcado = 0.0;
        $scope.Oportunidade.HorasOrcadas = 0.0;
        $scope.Oportunidade.IngressoLiquido = 0.0;
        $scope.Oportunidade.RiscoGerenciavel = 0.0;
        $scope.Oportunidade.IngressoTotal = 0.0;
        $scope.Oportunidade.Status = {};
        $scope.Oportunidade.ProjectStep = {};
        $scope.Pricing.PorcentagemBackoffice = 15;
        $scope.Pricing.PorcentagemRisco = 10;
        $scope.Pricing.PorcentagemMargem = 20;
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
            if (model.DataPrevista != undefined)
                model.DataPrevista = new Date(Date.parse(model.DataPrevista));
            $scope.Oportunidade = model;
            $scope.Oportunidade.ContatoComercial.Id = parseInt(model.ContatoComercial.Id);

            if (model.Pricing != null)
                $scope.Pricing = model.Pricing;
            else
                $scope.Pricing = {};

            if (model.Pricing.ItensPricing != null)
                $scope.ItensPricing = model.Pricing.ItensPricing;
            else
                $scope.ItensPricing = [];

            $scope.StepsSelectedList = model.Fases;

            $scope.ObservationSelectedList = model.Detalhes;

            $scope.Codigo = $scope.Oportunidade.Codigo + ' - ' + $scope.Oportunidade.Tag + ' ' + $scope.Oportunidade.Nome;

            $scope.ClientSelected();
            $scope.Oportunidade.Status.Id = model.Status.Id.toString();
        }
    }


    /*--Geral--*/

    $scope.GenerateCode = function () {
        if ($scope.Oportunidade.Id == null || $scope.Oportunidade.Id == undefined) {
            $http({
                method: 'GET',
                url: '/Projeto/Generate_Number?id=' + $scope.Oportunidade.TipoProjeto.Id
            }).success(function (result) {
                $scope.Oportunidade.Codigo = result;
                $scope.GenerateName();
            });
        }
    }

    $scope.GenerateName = function () {
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

    $http({
        method: 'GET',
        url: '/TiposProjeto/Get_All'
    }).success(function (result) {
        $scope.TiposProjeto = result;
        document.getElementById('load-02').hidden = true;
    });


    //Carrega o campo Cliente
    $http({
        method: 'GET',
        url: '/Cliente/LoadCombo'
    }).success(function (clients) {
        $scope.ClientList = clients;
        document.getElementById('loading02').hidden = true;
        $scope.ClientSelected();
    });

    $http({
        method: 'GET',
        url: '/Especialidade/Get_All'
    }).success(function (result) {
        $scope.Especialidades = result;
        document.getElementById('load-06').hidden = true;
    });

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
    //$scope.AddPricingItens = function () {
    //    if ($scope.Especialidade == undefined || $scope.Especialidade.Id == null || $scope.Especialidade.Id == '') {
    //        noty({ text: 'Selecione uma especialidade', layout: 'topRight', type: 'error' });
    //        return false;
    //    }
    //    if ($scope.EspecialidadeValor == undefined || $scope.EspecialidadeValor == null || $scope.EspecialidadeValor == '') {
    //        noty({ text: 'Informe o valor', layout: 'topRight', type: 'error' });
    //        return false;
    //    }
    //    if ($scope.EspecialidadeHoras == undefined || $scope.EspecialidadeHoras == null || $scope.EspecialidadeHoras == '') {
    //        noty({ text: 'Informe o total de horas', layout: 'topRight', type: 'error' });
    //        return false;
    //    }

    //    var line = {};

    //    // -- Recursos -- //
    //    line.Especialidade = {};
    //    line.Especialidade.Id = $scope.Especialidade.Id;

    //    for (var i = 0; i < $scope.Especialidades.length; i++) {
    //        if ($scope.Especialidades[i].Id == $scope.Especialidade.Id) {
    //            line.Especialidade.Name = $scope.Especialidades[i].Name;
    //            break;
    //        }
    //    }
    //    line.EspecialidadeValor = $scope.FormatValue($scope.EspecialidadeValor);
    //    line.EspecialidadeHoras = $scope.FormatValue($scope.EspecialidadeHoras);
    //    line.EspecialidadeCusto = $scope.FormatValue(line.EspecialidadeValor * line.EspecialidadeHoras);

    //    // -- Porcentagens -- //
    //    line.ValorBackoffice = $scope.FormatValue(line.EspecialidadeCusto * ($scope.FormatValue($scope.Pricing.PorcentagemBackoffice) / 100));
    //    line.ValorRisco = $scope.FormatValue(line.EspecialidadeCusto * ($scope.FormatValue($scope.Pricing.PorcentagemRisco) / 100));
    //    line.ValorMargem = $scope.FormatValue(line.EspecialidadeCusto * ($scope.FormatValue($scope.Pricing.PorcentagemMargem) / 100));
    //    line.ValorComissao = $scope.FormatValue((line.EspecialidadeCusto + line.ValorBackoffice + line.ValorRisco + line.ValorMargem) * ($scope.FormatValue($scope.Pricing.PorcentagemComissao) / 100));

    //    // -- Despesas -- //
    //    line.HotelDiarias = $scope.HotelDiarias;
    //    line.HotelValor = $scope.HotelValor;
    //    line.KmTrechos = $scope.KmTrechos;
    //    line.KmDistancia = $scope.KmDistancia;
    //    line.KmValor = $scope.KmValor;
    //    line.AlimentacaoDias = $scope.AlimentacaoDias;
    //    line.AlimentacaoValor = $scope.AlimentacaoValor;
    //    line.DeslocamentoHoras = $scope.DeslocamentoHoras;
    //    line.DeslocamentoValor = $scope.DeslocamentoValor;
    //    line.AereoTrechos = $scope.AereoTrechos;
    //    line.AereoValor = $scope.AereoValor;
    //    line.HotelTotal = $scope.TotalHotel;
    //    line.KmTotal = $scope.TotalQuilometragem;
    //    line.AlimentacaoTotal = $scope.TotalAlimentacao;
    //    line.DeslocamentoTotal = $scope.TotalDeslocamento;
    //    line.AereoTotal = $scope.TotalAereo;

    //    // -- Totais -- // 
    //    line.RecursoSubTotal = $scope.FormatValue(line.EspecialidadeCusto + line.ValorBackoffice + line.ValorRisco + line.ValorMargem + line.ValorComissao);
    //    line.RecursoDespesas = $scope.FormatValue(line.HotelTotal + line.KmTotal + line.AlimentacaoTotal + line.DeslocamentoTotal + line.AereoTotal);
    //    line.RecursoValorComDespesas = $scope.FormatValue(line.RecursoSubTotal + line.RecursoDespesas);
    //  //  line.ValorImposto = $scope.FormatValue(line.RecursoValorComDespesas * ($scope.FormatValue($scope.Pricing.PorcentagemImposto) / 100));
    //    line.ValorImposto = $scope.FormatValue($scope.FormatValue(line.RecursoValorComDespesas / ($scope.FormatValue(1 - ($scope.Pricing.PorcentagemImposto / 100)))) - $scope.FormatValue(line.RecursoValorComDespesas));

    //    line.RecursoValorComImpostos = $scope.FormatValue(line.RecursoValorComDespesas + line.ValorImposto);
    //    line.RecursoValorHorasSemDespesa = $scope.FormatValue((line.RecursoSubTotal + line.ValorImposto) / line.EspecialidadeHoras);
    //    line.RecursoValorHorasComDespesa = $scope.FormatValue(line.RecursoValorComImpostos / line.EspecialidadeHoras);


    //    var index = $scope.ItensPricing.length;

    //    if ($scope.SpecialtyLine == -1)
    //        $scope.ItensPricing[index] = line;

    //    else
    //        $scope.ItensPricing[$scope.SpecialtyLine] = line;

    //    $scope.SpecialtyLine = -1;
    //    $scope.Especialidade = {};
    //    $scope.EspecialidadeHoras = "";
    //    $scope.EspecialidadeValor = "";
    //    $scope.Calculate_OpportunityValues();
    //}

    $scope.EditPricingItens = function (index) {
        $scope.SpecialtyLine = index;
        $scope.Especialidade = {};
        $scope.Especialidade.Id = $scope.ItensPricing[index].Especialidade.Id;

        $scope.EspecialidadeValor = $scope.ItensPricing[index].EspecialidadeValor;
        $scope.EspecialidadeHoras = $scope.ItensPricing[index].EspecialidadeHoras;
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
            $scope.ItensPricing[i].RecursoValorHorasSemDespesa = $scope.FormatValue(($scope.ItensPricing[i].RecursoSubTotal + $scope.ItensPricing[i].ValorImposto) / $scope.ItensPricing[i].EspecialidadeHoras);
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

    //Carrega o campo Responsável
    $http({
        method: 'GET',
        url: '/Oportunidade/GetProjectManagers'
    }).success(function (projectManagers) {
        $scope.projectManagerList = projectManagers;
     //   document.getElementById('loading04').hidden = true;
    });

    //Carrega os campos Vendedor e Técnico
    $http({
        method: 'GET',
        url: '/Colaborador/LoadCombo'
    }).success(function (collaborators) {
        $scope.CollaboratorList = collaborators;
        document.getElementById('loading05').hidden = true;
        document.getElementById('loading06').hidden = true;
    });




    

    //Carrega os contatos
    $scope.ClientSelected = function () {
        for (var i = 0; i < $scope.ClientList.length; i++) {
            if ($scope.ClientList[i].Id == $scope.Oportunidade.Cliente.Id) {
                $scope.Oportunidade.Tag = '[' + $scope.ClientList[i].Tag + ']';
                $scope.GenerateName();
                break;
            }
        }

       

        $http({
            method: 'GET',
            url: '/Oportunidade/GetContactsFromClient?id=' + $scope.Oportunidade.Cliente.Id
        }).success(function (result) {
            $scope.ContactClientList = result;
        });



    }

    //Carrega as Fases do Projeto
    $http({
        method: 'GET',
        url: '/Oportunidade/GetSteps'
    }).success(function (projectSteps) {
        $scope.projectStepsList = projectSteps;
    });

    //Carrega o campo Temperatura
    $http({
        method: 'GET',
        url: '/Oportunidade/GetPercents  '
    }).success(function (percents) {
        $scope.PercentList = percents;
    });

    //Métodos para adicionar, editar e remover Fases do projeto
    $scope.AddStepLine = function () {
        if ($scope.Oportunidade.Steps.ProjectStep.Id == '' || $scope.Oportunidade.Steps.ProjectStep.Id == null) {
            noty({ text: 'Selecione a etapa atual do projeto', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Oportunidade.Steps.Description == '' || $scope.Oportunidade.Steps.Description == null) {
            noty({ text: 'Informe uma descrição para a etapa', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Oportunidade.Steps.DateConclusion == '' || $scope.Oportunidade.Steps.DateConclusion == null) {
            noty({ text: 'Informe a data prevista de conclusão da etapa', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Oportunidade.Steps.DateInit == '' || $scope.Oportunidade.Steps.DateInit == null) {
            noty({ text: 'Informe a data de início da etapa', layout: 'topRight', type: 'error' });
            return false;
        }

        var obj = {};
        obj.ProjectStep = {};
        for (var i = 0; i < $scope.projectStepsList.length; i++) {
            if ($scope.projectStepsList[i].Id == $scope.Oportunidade.Steps.ProjectStep.Id) {
                obj.ProjectStep.Id = $scope.Oportunidade.Steps.ProjectStep.Id;
                obj.ProjectStep.Name = $scope.projectStepsList[i].Nome;
                //$scope.Oportunidade.Steps.ProjectStep.Name = $scope.projectStepsList[i].Name;
                break;
            }
        }
        obj.Description = $scope.Oportunidade.Steps.Description;
        obj.DateConclusion = $scope.Oportunidade.Steps.DateConclusion;
        obj.DateInit = $scope.Oportunidade.Steps.DateInit;

        $scope.StepsSelectedList[$scope.StepsSelectedList.length] = obj;
        obj = {};
        $scope.Oportunidade.Steps = {};
    }

    $scope.EditStepLine = function (index) {
        $scope.Oportunidade.Steps = $scope.StepsSelectedList[index];
        //$scope.Oportunidade.Steps.ProjectStep = {};
        $scope.Oportunidade.Steps.ProjectStep.Id = $scope.StepsSelectedList[index].ProjectStep;
        $scope.Oportunidade.Steps.DateInit = new Date($scope.StepsSelectedList[index].DateInit);
        $scope.Oportunidade.Steps.DateConclusion = new Date($scope.StepsSelectedList[index].DateConclusion);
    }
    $scope.RemoveStepList = function (index) {
        $scope.StepsSelectedList.splice(index, 1);
    }


    $scope.PercentSelected = function () {
        for (var i = 0; i < $scope.PercentList.length; i++) {
            if ($scope.Oportunidade.Percent.Id == $scope.PercentList[i].Id) {
                $scope.VisualPercent = $scope.PercentList[i].Percent;
                $scope.Oportunidade.Financial.BusinessValue = parseFloat($scope.Oportunidade.Financial.OportunittyValue.replace('.', ',')).toFixed(2) * $scope.PercentList[i].Percent / 100;
                break;
            }
        }
    }

    $scope.OportunidadeValue = function () {
        for (var i = 0; i < $scope.PercentList.length; i++) {
            if ($scope.Oportunidade.Percent.Id == $scope.PercentList[i].Id) {
                $scope.Oportunidade.Financial.BusinessValue = parseFloat($scope.Oportunidade.Financial.OportunittyValue.replace('.', '')).toFixed(2) * $scope.PercentList[i].Percent / 100;
                break;
            }
        }
    }

    //Métodos para adicionar e remover Detalhes do projeto
    $scope.AddDetailsLine = function () {
        //if ($scope.Oportunidade.Observations.Date == '' || $scope.Oportunidade.Observations.Date == null) {
        //    noty({ text: 'Informe a data da observação', layout: 'topRight', type: 'error' });
        //    return false;
        //}
        if ($scope.Oportunidade.Observations.Observation == '' || $scope.Oportunidade.Observations.Observation == null) {
            noty({ text: 'Informe uma descrição para a observação', layout: 'topRight', type: 'error' });
            return false;
        }
        var obj = {};
        //obj.Date = $scope.Oportunidade.Observations.Date;
        obj.Data = new Date();
        obj.Observation = $scope.Oportunidade.Observations.Observation;
        obj.Colaborador = $scope.UserName;
        $scope.ObservationSelectedList[$scope.ObservationSelectedList.length] = obj;

        obj = {};
        $scope.Oportunidade.Observations = {};
    }
    $scope.RemoveDetailsLine = function (index) {
        $scope.ObservationSelectedList.splice(index, 1);
    }

    


    $scope.SaveOportunitty = function () {
        if (ValidateModel()) {
            $scope.Oportunidade.Pricing = $scope.Pricing;
            $scope.Oportunidade.Pricing.ItensPricing = $scope.ItensPricing;
            $scope.Oportunidade.Detalhes = {};
            $scope.Oportunidade.Detalhes = $scope.ObservationSelectedList
            $scope.Oportunidade.Fases = {};
            $scope.Oportunidade.Fases = $scope.StepsSelectedList;



            $scope.projectStepsList
            $scope.IsProcessing = true;
            $http.post('/Oportunidade/Salvar', $scope.Oportunidade)
                .success(function (message) {
                    if (message.Success == null || message.Success == undefined) {
                        $('#message-error').text('');
                        $('#message-error').append(message.Error.Message);
                        $('#message-box-danger').addClass('open');
                    }
                    else {
                        $scope.successMessage = 'Oportunidade criada com sucesso!';
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
        //if ($scope.Oportunidade.ContatoComercial == undefined || $scope.Oportunidade.ContatoComercial.Id == null) {
        //    noty({ text: 'Selecione o contato comercial', layout: 'topRight', type: 'error' });
        //    return false;
        //}
        //if ($scope.Oportunidade.Responsavel.Id == '' || $scope.Oportunidade.Responsavel.Id == null) {
        //    noty({ text: 'Selecione o responsável', layout: 'topRight', type: 'error' });
        //    return false;
        //}
        //if ($scope.Oportunidade.Vendedor.Id == '' || $scope.Oportunidade.Vendedor.Id == null) {
        //    noty({ text: 'Selecione o vendedor', layout: 'topRight', type: 'error' });
        //    return false;
        //}
        //if ($scope.Oportunidade.Tecnico.Id == '' || $scope.Oportunidade.Tecnico.Id == null) {
        //    noty({ text: 'Selecione o especialista técnico', layout: 'topRight', type: 'error' });
        //    return false;
        //}
        return true;
    }

    function CreateModel() {
        $scope.Oportunidade = {};
        $scope.Oportunidade.Financial = {};
        $scope.Oportunidade.Financial.Temperature = '';
        $scope.Oportunidade.Cliente = {};
        $scope.Oportunidade.Contact = {};
        $scope.Oportunidade.Contact.ProjectManager = {};
        $scope.Oportunidade.Contact.Vendor = {};
        $scope.Oportunidade.Contact.Technical = {};
        $scope.Oportunidade.Steps = {};
        $scope.StepsSelectedList = [];
        $scope.ObservationSelectedList = [];
        $scope.PercentList = [];
        $scope.successMessage = '';
        $scope.Pricing = {};
    }

    function CreateArrays() {
        $scope.TiposProjeto = [];
        $scope.ClientList = [];
        $scope.ContactClientList = [];
        $scope.projectManagerList = [];
        $scope.ContactList = [];
        $scope.CollaboratorList = [];
        $scope.TemperatureList = [];
        $scope.projectStepsList;
        $scope.StepsSelectedList = [];
        $scope.VisualPercent = 0;
        $scope.ObservationSelectedList = [];
        $scope.Especialidades = [];
        $scope.ItensPricing = [];
        $scope.SpecialtyLine = -1;
        $scope.TotalHotel = 0.0;
        $scope.TotalQuilometragem = 0.0;
        $scope.TotalAlimentacao = 0.0;
        $scope.TotalDeslocamento = 0.0;
        $scope.TotalAereo = 0.0;
    }

    $scope.ConvertToProject = function () {
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
                }
                else {
                    $('#message-success').text('');
                    $scope.successMessage = 'Projeto criado com sucesso';
                    $('#message-success').append('Projeto criado com sucesso!');
                    $('#message-box-success').addClass('open');
                    CreateModel();
                }
            });
    }
}]);