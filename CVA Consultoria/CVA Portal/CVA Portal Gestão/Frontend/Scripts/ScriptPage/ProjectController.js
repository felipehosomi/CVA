cvaGestao.controller('ProjectController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {
    $scope.OnLoad = function (model) {

        CreateArrays();
        LoadArrays();

        $scope.ItensPricing = [];
        $scope.Projeto = {};
        $scope.Projeto.Cliente = {};
        $scope.Projeto.TipoProjeto = {};
        $scope.Projeto.Membros = [];
        $scope.Projeto.Recursos = [];
        $scope.Projeto.Fases = [];
        $scope.Projeto.Itens = [];
        $scope.Projeto.Pricing = {};
        $scope.Projeto.Pricing.ItensPricing = [];
        $scope.Projeto.ChangeRequests = [];
        $scope.Projeto.Gerente = {};

        var stu = model.Status.Id;
        //Editando
        if (model != null) {
            $scope.Projeto = model;

            $scope.Projeto.Status = {};
            $scope.Projeto.Status.Id = stu.toString();
            $scope.Projeto.Cliente = model.Cliente;
            $scope.Projeto.DataInicial = new Date(model.DataInicial);
            $scope.Projeto.DataPrevista = new Date(model.DataPrevista);
            $scope.Projeto.TipoProjeto = model.TipoProjeto;
            $scope.Pricing = model.Pricing;
            $scope.ItensPricing = model.Pricing.ItensPricing;
            $scope.Membros = model.Membros;
            $scope.Recursos = model.Recursos;
            $scope.FasesProjeto = model.Fases;
            $scope.Projeto.ChangeRequests = model.ChangeRequests;
            $scope.RegrasEspecialidades = model.SpecialtyRules;
            $scope.RecursosFase = model.Itens;
            $scope.Projeto.Gerente = model.Gerente;
            $scope.StatusReports = model.StatusReport;


            $scope.Codigo = $scope.Projeto.Codigo + ' - ' + $scope.Projeto.Tag + ' ' + $scope.Projeto.Nome;

            $scope.GetChangeRequests();
            //$scope.CalculateCost();
        }
    }

    $scope.Save = function () {
        //aqui
        LoadModel();
        LoadPricingModel();
        if (ValidateModel()) {
            console.log($scope.Projeto);
            $http.post('/Projeto/Save', $scope.Projeto).success(function (result) {
                if (result.Success == undefined || result.Success == null) {
                    $('#message-error').text('');
                    $('#message-error').append(result.Error.Message);
                    $('#message-box-danger').addClass('open');
                }
                else {
                    $('#message-success').text('');
                    $('#message-success').append(result.Success.Message);
                    $('#message-box-success').addClass('open');
                }
            });
        }
    }

    /*--Geral--*/
    $scope.GenerateCode = function () {
        if ($scope.Projeto.Id == null || $scope.Projeto.Id == undefined) {
            $http({
                method: 'GET',
                url: '/Projeto/Generate_Number?id=' + $scope.Projeto.TipoProjeto.Id
            }).success(function (result) {
                $scope.Projeto.Codigo = result;
                $scope.GenerateName();
            });
        }
    }

    $scope.GenerateTag = function () {
        for (var i = 0; i < $scope.Clientes.length; i++) {
            if ($scope.Clientes[i].Id == $scope.Projeto.Cliente.Id) {
                $scope.Projeto.Tag = '[' + $scope.Clientes[i].Tag + ']';
                $scope.GenerateName();
                break;
            }
        }
    }

    $scope.GenerateName = function () {
        if ($scope.Projeto.Codigo == undefined && $scope.Projeto.Tag == undefined) {
            $scope.Codigo = $scope.Projeto.Nome;
            return false;
        }

        else if ($scope.Projeto.Codigo == undefined) {
            $scope.Codigo = ' - ' + $scope.Projeto.Tag + ' ' + $scope.Projeto.Nome;
            return false;
        }

        else if ($scope.Projeto.Tag == undefined) {
            $scope.Codigo = $scope.Projeto.Codigo + ' - ' + $scope.Projeto.Nome;
            return false;
        }

        else {
            $scope.Codigo = $scope.Projeto.Codigo + ' - ' + $scope.Projeto.Tag + ' ' + $scope.Projeto.Nome;
            return false;
        }
    }

    /*---Membros---*/
    $scope.AddMember = function () {
        if ($scope.Membro == undefined || $scope.Membro.Nome == null || $scope.Membro.Nome == '') {
            noty({ text: 'Obrigatório informar o nome do membro', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Membro == undefined || $scope.Membro.Telefone == null || $scope.Membro.Telefone == '') {
            noty({ text: 'Obrigatório informar o telefone do membro', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Membro == undefined || $scope.Membro.Email == null || $scope.Membro.Email == '') {
            noty({ text: 'Obrigatório informar o email do membro', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Membro == undefined || $scope.Membro.Departamento == null || $scope.Membro.Departamento == '') {
            noty({ text: 'Obrigatório informar o departamento do membro', layout: 'topRight', type: 'error' });
            return false;
        }

        var model = {};
        model.Nome = $scope.Membro.Nome;
        model.Telefone = $scope.Membro.Telefone;
        model.Email = $scope.Membro.Email;
        model.Departamento = $scope.Membro.Departamento;

        if ($scope.MemberLine == -1) {
            var i = $scope.Membros.length;
            $scope.Membros[i] = model;
        }

        else {
            var i = $scope.MemberLine;
            $scope.Membros[i] = model;
        }

        model = {};
        $scope.Membro = {};
        $scope.MemberLine = -1;
    }

    $scope.EditMember = function (index) {
        $scope.MemberLine = index;
        $scope.Membro = {};
        $scope.Membro.Nome = $scope.Membros[index].Nome;
        $scope.Membro.Telefone = $scope.Membros[index].Telefone;
        $scope.Membro.Email = $scope.Membros[index].Email;
        $scope.Membro.Departamento = $scope.Membros[index].Departamento;
    }

    $scope.RemoveMember = function (index) {

        $scope.Membros.splice(index, 1);
    }


    /*---Fases---*/
    $scope.AddProjectStep = function () {
        if ($scope.Fase == undefined || $scope.Fase.Id == null || $scope.Fase.Id == '') {
            noty({ text: 'Obrigatório informar a fase', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Fase == undefined || $scope.Fase.DataInicio == null || $scope.Fase.DataInicio == '') {
            noty({ text: 'Obrigatório informar a data inicial da fase', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Fase == undefined || $scope.Fase.DataPrevista == null || $scope.Fase.DataPrevista == '') {
            noty({ text: 'Obrigatório informar a data prevista da fase', layout: 'topRight', type: 'error' });
            return false;
        }

        if ($scope.Fase == undefined || $scope.Fase.Concluido == null || $scope.Fase.Concluido == '') {
            noty({ text: 'Obrigatório informar a porcentagem concluída da fase', layout: 'topRight', type: 'error' });
            return false;
        }

        if ($scope.StepLine == -1)
            for (var i = 0; i < $scope.FasesProjeto.length; i++) {
                if ($scope.Fase.Id == $scope.FasesProjeto[i].StepId) {
                    noty({ text: 'Este projeto já possui uma fase deste tipo', layout: 'topRight', type: 'error' });
                    return false;
                }
            }

        var line = {};
        line.StepId = $scope.Fase.Id;
        for (var i = 0; i < $scope.Fases.length; i++) {
            if ($scope.Fases[i].Id == line.StepId) {
                line.Nome = $scope.Fases[i].Nome;
                break;
            }
        }

        line.DataInicio = $scope.Fase.DataInicio;
        line.DataPrevista = $scope.Fase.DataPrevista;
        line.CustoOrcado = $scope.Fase.CustoOrcado;
        line.CustoReal = $scope.Fase.CustoReal;
        line.HorasOrcadas = $scope.Fase.HorasOrcadas;
        line.HorasConsumidas = $scope.Fase.HorasConsumidas;
        line.Concluido = $scope.Fase.Concluido;

        if ($scope.StepLine == -1) {
            line.CustoOrcado = '0';
            line.CustoReal = '0';
            line.HorasOrcadas = '0';
            line.HorasConsumidas = '0';
            var i = $scope.FasesProjeto.length;
            $scope.FasesProjeto[i] = line;
        }

        else {
            var i = $scope.StepLine;
            $scope.FasesProjeto[i] = line;
        }

        line = {};
        $scope.Fase = {};
        $scope.StepLine = -1;
    }

    $scope.EditProjectStep = function (index) {
        $scope.StepLine = index;
        $scope.Fase = {};
        $scope.Fase.Id = $scope.FasesProjeto[index].StepId;
        $scope.Fase.Nome = $scope.FasesProjeto[index].Nome;
        $scope.Fase.DataInicio = new Date($scope.FasesProjeto[index].DataInicio);
        $scope.Fase.DataPrevista = new Date($scope.FasesProjeto[index].DataPrevista);
        $scope.Fase.CustoOrcado = $scope.FasesProjeto[index].CustoOrcado;
        $scope.Fase.CustoReal = $scope.FasesProjeto[index].CustoReal;
        $scope.Fase.HorasOrcadas = $scope.FasesProjeto[index].HorasOrcadas;
        $scope.Fase.HorasConsumidas = $scope.FasesProjeto[index].HorasConsumidas;
        $scope.Fase.Concluido = $scope.FasesProjeto[index].Concluido;
    }

    $scope.RemoveProjectStep = function (index) {

        if ($scope.Projeto.Id != undefined || $scope.Projeto.Id != null) {

            $scope.FasesProjeto[index].ProjectId = $scope.Projeto.Id;

            $http.post('/Projeto/Remove_Step', $scope.FasesProjeto[index]).success(function (result) {
                if (result.Success == null || result.Success == undefined) {
                    $('#message-error').text('');
                    $('#message-error').append(result.Error.Message);
                    $('#message-box-danger').addClass('open');
                }
                else {
                    $scope.FasesProjeto.splice(index, 1);
                }
            });
        }
        else
            $scope.FasesProjeto.splice(index, 1);
    }

    /*---Recursos Fase---*/
    $scope.AddStepResource = function () {
        if ($scope.FaseProjeto == undefined || $scope.FaseProjeto == null || $scope.FaseProjeto == '') {
            noty({ text: 'Obrigatório informar a fase', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Especialidade == undefined || $scope.Especialidade == null || $scope.Especialidade == '') {
            noty({ text: 'Obrigatório informar a especialidade', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Colaborador == undefined || $scope.Colaborador == null || $scope.Colaborador == '') {
            noty({ text: 'Obrigatório informar o colaborador', layout: 'topRight', type: 'error' });
            return false;
        }

        if ($scope.Horas == undefined || $scope.Horas == null || $scope.Horas == '') {
            noty({ text: 'Obrigatório informar uma quantidade de horas', layout: 'topRight', type: 'error' });
            return false;
        }

        var line = {};

        line.Fase = {};
        for (var i = 0; i < $scope.FasesProjeto.length; i++) {
            if ($scope.FasesProjeto[i].StepId == $scope.FaseProjeto) {
                line.Fase.StepId = $scope.FasesProjeto[i].StepId;
                line.Fase.Nome = $scope.FasesProjeto[i].Nome;
                break;
            }
        }

        line.Especialidade = {};
        for (var i = 0; i < $scope.Especialidades2.length; i++) {
            if ($scope.Especialidades2[i].Id == $scope.Especialidade) {
                line.Especialidade.Id = $scope.Especialidades2[i].Id;
                line.Especialidade.Name = $scope.Especialidades2[i].Name;
                break;
            }
        }

        line.Colaborador = {};
        for (var i = 0; i < $scope.Colaboradores2.length; i++) {
            if ($scope.Colaboradores2[i].Id == $scope.Colaborador) {
                line.Colaborador.Id = $scope.Colaboradores2[i].Id;
                line.Colaborador.Nome = $scope.Colaboradores2[i].Nome;
                break;
            }
        }

        if ($scope.Valor == null)
            line.CustoOrcado = '0';
        else
            line.CustoOrcado = $scope.FormatValue($scope.Valor);
        line.HorasOrcadas = $scope.Horas;
        line.HorasConsumidas = '0';


        if ($scope.StepResourceLine == -1) {
            var i = $scope.RecursosFase.length;
            $scope.RecursosFase[i] = line;
        }

        else {
            var i = $scope.StepResourceLine;
            $scope.RecursosFase[i] = line;
        }

        line = {};
        $scope.Horas = ""
        $scope.Valor = "";
        $scope.StepResourceLine = -1;
        $scope.CalculateStepPrice();
    }

    $scope.EditStepResource = function (index) {
        $scope.StepResourceLine = index;
        $scope.FaseProjeto = $scope.RecursosFase[index].Fase.StepId;
        $scope.Valor = $scope.RecursosFase[index].CustoOrcado;
        $scope.Horas = $scope.RecursosFase[index].HorasOrcadas;
    }

    $scope.RemoveStepResource = function (index) {
        $scope.RecursosFase.splice(index, 1);
        $scope.CalculateStepPrice();
    }

    $scope.Get_CollaboratorBySpecialty = function (id) {
        $http({
            method: 'GET',
            url: '/Colaborador/Get_CollaboratorBySpecialty?id=' + id
        }).success(function (result) {
            $scope.Colaboradores2 = result;
        });
    }

    $scope.CalculateStepPrice = function () {
        var custo = 0.0;
        var horas = 0.0;
        for (var i = 0; i < $scope.FasesProjeto.length; i++) {
            for (var j = 0; j < $scope.RecursosFase.length; j++) {
                if ($scope.RecursosFase[j].Fase.StepId == $scope.FasesProjeto[i].StepId) {
                    custo = custo + ($scope.RecursosFase[j].CustoOrcado * $scope.RecursosFase[j].HorasOrcadas);
                    horas = horas + $scope.FormatValue($scope.RecursosFase[j].HorasOrcadas);
                }
            }
            $scope.FasesProjeto[i].CustoOrcado = custo;
            $scope.FasesProjeto[i].HorasOrcadas = horas;

            custo = 0.0;
            horas = 0.0;
        }
    }
    /*---Recursos---*/
    $scope.AddResource = function () {
        if ($scope.Recurso == undefined || $scope.Recurso.Id == null || $scope.Recurso.Id == '') {
            noty({ text: 'Obrigatório informar o recurso', layout: 'topRight', type: 'error' });
            return false;
        }

        var model = {};
        model.Id = $scope.Recurso.Id;
        for (var i = 0; i < $scope.Colaboradores.length; i++) {
            if ($scope.Colaboradores[i].Id == model.Id) {
                model.Nome = $scope.Colaboradores[i].Nome;
                break;
            }
        }

        if ($scope.ResourceLine == -1) {
            var i = $scope.Recursos.length;
            $scope.Recursos[i] = model;
        }

        else {
            var i = $scope.ResourceLine;
            $scope.Recursos[i] = model;
        }

        model = {};
        $scope.Recurso = {};
        $scope.ResourceLine = -1;
    }

    $scope.RemoveResource = function (index) {
        $scope.Recursos.splice(index, 1);
    }

    $scope.EditResourceSpecialties = function (index) {
        $('#tabs li:eq(1) a').tab('show')
        $scope.ColaboradorId = $scope.Recursos[index].Id;
        $scope.ColaboradorNome = $scope.Recursos[index].Nome;

        $http({
            method: 'GET',
            url: '/Colaborador/GetSpecialtiesForCollaborator?id=' + $scope.Recursos[index].Id
        }).success(function (result) {
            $scope.ColaboradorEspecialidades = result;
            document.getElementById('load-06').hidden = true;
        });
    }

    $scope.SelectedCol = function () {
        $http({
            method: 'GET',
            url: '/Colaborador/GetSpecialtiesForCollaborator?id=' + $scope.EspecialidadeCol
        }).success(function (result) {
            $scope.ColaboradorEspecialidades = result;
            document.getElementById('load-06').hidden = true;
        });
    }

    $scope.AddSpecialtyRule = function () {
        var model = {};

        if ($scope.ColaboradorEspecialidade == undefined || $scope.ColaboradorEspecialidade.Id == null || $scope.ColaboradorEspecialidade.Id == '') {
            noty({ text: 'Obrigatório informar a especialidade', layout: 'topRight', type: 'error' });
            return false;
        }

        var valorPago = $scope.ColaboradorEspecialidade.Value;
        if (valorPago == undefined || valorPago == null || valorPago <= 0) {
            noty({ text: 'Obrigatório informar o valor', layout: 'topRight', type: 'error' });
            return false;
        }

        for (var i = 0; i < $scope.RegrasEspecialidades.length; i++) {
            if (($scope.RegrasEspecialidades[i].SpecialtyId == $scope.ColaboradorEspecialidade.Id) && ($scope.RegrasEspecialidades[i].CollaboratorId == $scope.ColaboradorId)) {
                noty({ text: 'Já existe uma regra cadastrada para esta especialidade deste colaborador', layout: 'topRight', type: 'error' });
                return false;
            }
        }

        for (var i = 0; i < $scope.ColaboradorEspecialidades.length; i++) {
            if ($scope.ColaboradorEspecialidades[i].Id == $scope.ColaboradorEspecialidade.Id) {

                model.CollaboratorId = $scope.ColaboradorId;
                model.CollaboratorName = $scope.ColaboradorNome;
                model.SpecialtyId = $scope.ColaboradorEspecialidades[i].Id;
                model.Value = $scope.RemoveFormation(valorPago);
                model.SpecialtyName = $scope.ColaboradorEspecialidades[i].Name;
                model.Description = $scope.ColaboradorEspecialidades[i].Description;

                var j = $scope.RegrasEspecialidades.length;
                $scope.RegrasEspecialidades[j] = model;
                break;
            }
        }
        $scope.ColaboradorEspecialidade.Value = "";
        $scope.ColaboradorNome = "";
        $scope.ColaboradorEspecialidades = [];
    }

    $scope.RemoveSpecialtyRule = function (index) {
        $scope.RegrasEspecialidades.splice(index, 1);
    }



    /*---Pricing---*/
    //$scope.AddPricing = function () {
    //    LoadPricingModel();
    //    if (ValidatePricingModel()) {
    //        if ($scope.Pricing.Id == undefined || $scope.Pricing.Id == null) {
    //            $http.post('/Pricing/Insert', $scope.Pricing).success(function (result) {
    //                if (result.Success == null || result.Success == undefined) {
    //                    $('#message-error').text('');
    //                    $('#message-error').append(result.Error.Message);
    //                    $('#message-box-danger').addClass('open');
    //                }
    //                else {
    //                    $('#message-success').text('');
    //                    $('#message-success').append(result.Success.Message);
    //                    $('#message-box-success').addClass('open');
    //                }
    //            });
    //        }
    //        else {
    //            $http.post('/Pricing/Update', $scope.Pricing)
    //                .success(function (result) {
    //                    if (result.Success == null || result.Success == undefined) {
    //                        $('#message-error').text('');
    //                        $('#message-error').append(result.Error.Message);
    //                        $('#message-box-danger').addClass('open');
    //                    }
    //                    else {
    //                        $('#message-success').text('');
    //                        $('#message-success').append(result.Success.Message);
    //                        $('#message-box-success').addClass('open');
    //                    }
    //                });
    //        }
    //    }
    //}
    $scope.AddPricingItens = function () {
        if ($scope.Especialidade == undefined || $scope.Especialidade.Id == null || $scope.Especialidade.Id == '') {
            noty({ text: 'Selecione uma especialidade', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Especialidade.Valor == undefined || $scope.Especialidade.Valor == null || $scope.Especialidade.Valor == '') {
            noty({ text: 'Informe o valor', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Especialidade.Horas == undefined || $scope.Especialidade.Horas == null || $scope.Especialidade.Horas == '') {
            noty({ text: 'Informe o total de horas', layout: 'topRight', type: 'error' });
            return false;
        }

        var line = {};
        line.Especialidade = {};
        line.Especialidade.Id = $scope.Especialidade.Id;

        for (var i = 0; i < $scope.Recursos.length; i++) {
            if ($scope.Recursos[i].Id == $scope.EspecialidadeCol) {
                line.Colaborador = $scope.Recursos[i].Nome;
                break;
            }
        }

        for (var i = 0; i < $scope.ColaboradorEspecialidades.length; i++) {
            if ($scope.ColaboradorEspecialidades[i].Id == $scope.Especialidade.Id) {
                line.Especialidade.Name = $scope.ColaboradorEspecialidades[i].Name;
                break;
            }
        }
        line.Valor = $scope.FormatValue($scope.RemoveFormation($scope.Especialidade.Valor));
        line.Horas = $scope.FormatValue($scope.RemoveFormation($scope.Especialidade.Horas));
        line.Custo = $scope.FormatValue(line.Valor * line.Horas);
        line.ValorBackoffice = $scope.FormatValue(($scope.RemoveFormation($scope.Projeto.Pricing.PorcentagemBackoffice) * line.Custo) / 100);
        line.ValorRisco = $scope.FormatValue(($scope.RemoveFormation($scope.Projeto.Pricing.PorcentagemRisco) * line.Custo) / 100);
        line.ValorMargem = $scope.FormatValue(($scope.RemoveFormation($scope.Projeto.Pricing.PorcentagemMargem) * line.Custo) / 100);
        line.ValorComissao = $scope.FormatValue(($scope.RemoveFormation($scope.Projeto.Pricing.PorcentagemComissao) * line.Custo) / 100);
        line.Subtotal = $scope.FormatValue(line.Cost + line.ValorBackoffice + line.ValorRisco + line.ValorMargem + line.ValorComissao);
        line.Expenses = $scope.FormatValue(0);
        line.SubTotalExpenses = $scope.FormatValue(line.Subtotal + line.Expenses);
        line.SubTotalExpensesTaxes = $scope.FormatValue(line.SubTotalExpenses + (line.SubTotalExpenses * $scope.RemoveFormation($scope.Projeto.Pricing.PorcentagemImposto)));


        var index = $scope.ItensPricing.length;

        if ($scope.SpecialtyLine == -1)
            $scope.ItensPricing[index] = line;

        else
            $scope.ItensPricing[$scope.SpecialtyLine] = line;

        var totalHoras = 0;
        var totalCusto = 0;

        for (var i = 0; i < $scope.ItensPricing.length; i++) {
            totalHoras += $scope.ItensPricing[i].Horas * 1;
            totalCusto += $scope.ItensPricing[i].Custo;
        }
        $scope.TotalHoras = $scope.FormatValue(totalHoras);
        $scope.TotalCusto = $scope.FormatValue(totalCusto);

        $scope.SpecialtyLine = -1;

        $scope.Specialty = [];
        $scope.Especialidade.Valor = "";
        $scope.Especialidade.Horas = "";

        $scope.CalculateCost();
    }

    $scope.EditPricingItens = function (index) {
        $scope.SpecialtyLine = index;
        $scope.ColaboradorEspecialidades.Id = $scope.ItensPricing[index].Especialidade.Id;
        $scope.Especialidade = {};
        $scope.Especialidade.Valor = $scope.ItensPricing[index].Valor;
        $scope.Especialidade.Horas = $scope.ItensPricing[index].Horas;
    }

    $scope.RemovePricingItens = function (index) {
        $scope.ItensPricing.splice(index, 1);
        $scope.CalculateCost();
    }

    $scope.CalculateCost = function () {
        var totalHoras = 0;
        var totalCusto = 0;

        for (var i = 0; i < $scope.ItensPricing.length; i++) {
            totalHoras += $scope.ItensPricing[i].Horas * 1;
            totalCusto += $scope.ItensPricing[i].Custo * 1;
        }
        $scope.TotalHoras = $scope.FormatValue(totalHoras);
        $scope.TotalCusto = $scope.FormatValue(totalCusto);
        $scope.Projeto.CustoOrcado = $scope.TotalCusto;
        $scope.Projeto.HorasOrcadas = $scope.TotalHoras;

        $scope.CalculatePricing();
    }

    $scope.CalculatePricing = function () {
        ValidatePercentage();

        var subTotal = 0;
        var subTotalExpensesTaxes = 0;
        var ingressoLiquido = 0;
        var riscoGerenciavel = 0;
        var ingressoTotal = 0;
        var totalProjeto = 0;

        for (var i = 0; i < $scope.ItensPricing.length; i++) {

            $scope.ItensPricing[i].Custo = $scope.FormatValue($scope.ItensPricing[i].Valor * $scope.ItensPricing[i].Horas);
            $scope.ItensPricing[i].ValorBackoffice = $scope.FormatValue(($scope.RemoveFormation($scope.Pricing.PorcentagemBackoffice) * $scope.ItensPricing[i].Custo) / 100);
            $scope.ItensPricing[i].ValorRisco = $scope.FormatValue(($scope.RemoveFormation($scope.Pricing.PorcentagemRisco) * $scope.ItensPricing[i].Custo) / 100);
            $scope.ItensPricing[i].ValorMargem = $scope.FormatValue(($scope.RemoveFormation($scope.Pricing.PorcentagemMargem) * $scope.ItensPricing[i].Custo) / 100);
            $scope.ItensPricing[i].ValorComissao = $scope.FormatValue(($scope.RemoveFormation($scope.Pricing.PorcentagemComissao) * $scope.ItensPricing[i].Custo) / 100);
            $scope.ItensPricing[i].Subtotal = $scope.FormatValue($scope.ItensPricing[i].Custo + $scope.ItensPricing[i].ValorBackoffice + $scope.ItensPricing[i].ValorRisco + $scope.ItensPricing[i].ValorMargem + $scope.ItensPricing[i].ValorComissao);
            $scope.ItensPricing[i].Expenses = $scope.FormatValue(0);
            $scope.ItensPricing[i].SubTotalExpenses = $scope.FormatValue($scope.ItensPricing[i].Subtotal + $scope.ItensPricing[i].Expenses);
            $scope.ItensPricing[i].SubTotalExpensesTaxes = $scope.FormatValue(($scope.ItensPricing[i].SubTotalExpenses + $scope.ItensPricing[i].SubTotalExpenses * ($scope.RemoveFormation($scope.Pricing.PorcentagemImposto) / 100)));
            subTotal += $scope.FormatValue($scope.ItensPricing[i].Subtotal);
            ingressoLiquido += $scope.FormatValue($scope.ItensPricing[i].ValorBackoffice + $scope.ItensPricing[i].ValorMargem);
            riscoGerenciavel += $scope.FormatValue($scope.ItensPricing[i].ValorRisco);
            ingressoTotal += $scope.FormatValue($scope.ItensPricing[i].ValorBackoffice + $scope.ItensPricing[i].ValorMargem + $scope.ItensPricing[i].ValorRisco);
            totalProjeto += $scope.FormatValue($scope.ItensPricing[i].SubTotalExpensesTaxes);
        }

        $scope.SubTotal = $scope.FormatValue(subTotal);
        $scope.Projeto.IngressoLiquido = $scope.FormatValue(ingressoLiquido);
        $scope.Projeto.RiscoGerenciavel = $scope.FormatValue(riscoGerenciavel);
        $scope.Projeto.IngressoTotal = $scope.FormatValue(ingressoTotal);
        $scope.Projeto.ValorProjeto = $scope.FormatValue(totalProjeto);
    }

    $scope.FormatValue = function (valor) {
        valor = valor.replace(',', '.');
        valor = Number(valor);
        valor = parseFloat(valor.toFixed(2));

        return valor;
    }

    $scope.RemoveFormation = function (value) {
        if (value != 0 && value != undefined) {
            value = value.toString();
            var ph = value.replace('.', ',');
            return ph;
        }
        else
            return value;
    }

    /*---Change Requests---*/
    $scope.SaveChangeRequest = function (type) {
        if (type == 1) {
            if ($scope.ChangeRequest.Situacao == "Aprovada") {
                noty({ text: 'Não é possível editar uma Change Request que já tenha sido aprovada.', layout: 'topRight', type: 'error' });
                return false;
            }

            if (ValidateChangeRequest()) {
                var changeRequest = LoadChangeRequest(1);

                $http.post('/ChangeRequest/Save', changeRequest).success(function (result) {
                    if (result.Success == null || result.Success == undefined) {
                        $('#message-error').text('');
                        $('#message-error').append(result.Error.Message);
                        $('#message-box-danger').addClass('open');
                    }
                    else {
                        $('#message-success').text('');
                        $('#message-success').append(result.Success.Message);
                        $('#message-box-success').addClass('open');
                    }
                });

                $scope.ChangeRequest = {};
                $scope.ChangeRequestRecursosSolicitados = [];
                $scope.GetChangeRequests();
            }
        }

        if (type == 2) {
            var changeRequest = LoadChangeRequest(2);

            $http.post('/ChangeRequest/Save', changeRequest).success(function (result) {
                if (result.Success == null || result.Success == undefined) {
                    $('#message-error').text('');
                    $('#message-error').append(result.Error.Message);
                    $('#message-box-danger').addClass('open');
                }
                else {
                    $('#message-success').text('');
                    $('#message-success').append(result.Success.Message);
                    $('#message-box-success').addClass('open');
                }
            });

            $scope.ChangeRequest2 = {};
            $scope.ChangeRequestRecursosSolicitados2 = [];
            $scope.GetChangeRequests();
        }
    }

    $scope.NewChangeRequest = function () {
        $('#CR').show();
        $scope.ChangeRequest = {};
        $scope.ChangeRequest.Id = "";
        $scope.ChangeRequest.Codigo = "";
        $scope.ChangeRequest.Versao = "";
        $scope.ChangeRequest.Autor = "";
        $scope.ChangeRequest.Situacao = "Aguardando";
        $scope.ChangeRequest.GPI = "";
        $scope.ChangeRequest.GPE = "";
        $scope.ChangeRequest.Departamento = "";
        $scope.ChangeRequest.Processo = "";
        $scope.ChangeRequest.Descricao = "";
        $scope.ChangeRequest.Motivos = "";
        $scope.ChangeRequest.Recomendacoes = "";
        $scope.ChangeRequest.ImpactosPositivos = "";
        $scope.ChangeRequest.ImpactosNegativos = "";
        $scope.ChangeRequestRecursosSolicitados = [];
    }

    $scope.AddChangeRequestResource = function () {
        if ($scope.ChangeRequest == undefined || $scope.ChangeRequest.RecursoFase == null) {
            noty({ text: 'Preencha o campo "Fase"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest == undefined || $scope.ChangeRequest.RecursoEspecialidade == null) {
            noty({ text: 'Preencha o campo "Especialidade"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest == undefined || $scope.ChangeRequest.RecursoHorasSolicitadas == null || $scope.ChangeRequest.RecursoHorasSolicitadas == '') {
            noty({ text: 'Preencha o campo "Horas Solicitadas"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest == undefined || $scope.ChangeRequest.RecursoSolicitante == null || $scope.ChangeRequest.RecursoSolicitante == '') {
            noty({ text: 'Preencha o campo "Solicitante"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest == undefined || $scope.ChangeRequest.RecursoNecessidade == null || $scope.ChangeRequest.RecursoNecessidade == '') {
            noty({ text: 'Preencha o campo "Necessidade"', layout: 'topRight', type: 'error' });
            return false;
        }

        var line = {};

        line.RecursoFase = $scope.ChangeRequest.RecursoFase;
        for (var i = 0; i < $scope.FasesProjeto.length; i++) {
            if (line.RecursoFase == $scope.FasesProjeto[i].StepId) {
                line.RecursoFaseNome = $scope.FasesProjeto[i].Nome;
                break;
            }
        }

        line.RecursoEspecialidade = $scope.ChangeRequest.RecursoEspecialidade;
        for (var i = 0; i < $scope.Especialidades2.length; i++) {
            if (line.RecursoEspecialidade == $scope.Especialidades2[i].Id) {
                line.RecursoEspecialidadeNome = $scope.Especialidades2[i].Name;
                break;
            }
        }

        line.RecursoHorasSolicitadas = $scope.ChangeRequest.RecursoHorasSolicitadas;
        line.RecursoSolicitante = $scope.ChangeRequest.RecursoSolicitante;
        line.RecursoNecessidade = $scope.ChangeRequest.RecursoNecessidade;


        if ($scope.ChangeRequestResourceLine == -1) {
            var index = $scope.ChangeRequestRecursosSolicitados.length;
            $scope.ChangeRequestRecursosSolicitados[index] = line;
        }
        else {
            var index = $scope.ChangeRequestResourceLine;
            $scope.ChangeRequestRecursosSolicitados[index] = line;
        }

        line = null;
        $scope.ChangeRequest.RecursoFase = "";
        $scope.ChangeRequest.RecursoEspecialidade = "";
        $scope.ChangeRequest.RecursoHorasSolicitadas = "";
        $scope.ChangeRequest.RecursoSolicitante = "";
        $scope.ChangeRequest.RecursoNecessidade = "";
        $scope.ChangeRequestResourceLine = -1;
    }

    $scope.EditChangeRequestResource = function (index) {
        $scope.ChangeRequestResourceLine = index;

        $scope.ChangeRequest.RecursoFase = $scope.ChangeRequestRecursosSolicitados[index].RecursoFase;
        $scope.ChangeRequest.RecursoEspecialidade = $scope.ChangeRequestRecursosSolicitados[index].RecursoEspecialidade;
        $scope.ChangeRequest.RecursoHorasSolicitadas = $scope.ChangeRequestRecursosSolicitados[index].RecursoHorasSolicitadas;
        $scope.ChangeRequest.RecursoSolicitante = $scope.ChangeRequestRecursosSolicitados[index].RecursoSolicitante;
        $scope.ChangeRequest.RecursoNecessidade = $scope.ChangeRequestRecursosSolicitados[index].RecursoNecessidade;
    }

    $scope.RemoveChangeRequestResource = function (index) {
        $scope.ChangeRequestRecursosSolicitados.splice(index, 1);
    }

    $scope.ShowChangeRequest = function () {
        $('#CR').show();
        $http({
            method: 'GET',
            url: '/ChangeRequest/Get?id=' + $scope.ChangeRequest.Id
        }).success(function (result) {

            $scope.ChangeRequest = {};
            $scope.ChangeRequest.Id = result.Id;
            $scope.ChangeRequest.Codigo = result.Codigo;
            $scope.ChangeRequest.Versao = result.Versao;
            $scope.ChangeRequest.Autor = result.Autor;
            $scope.ChangeRequest.Situacao = result.Situacao;
            $scope.ChangeRequest.GPI = result.GPI;
            $scope.ChangeRequest.GPE = result.GPE;
            $scope.ChangeRequest.Departamento = result.Departamento;
            $scope.ChangeRequest.Processo = result.Processo;
            $scope.ChangeRequest.Descricao = result.Descricao;
            $scope.ChangeRequest.Motivos = result.Motivos;
            $scope.ChangeRequest.Recomendacoes = result.Recomendacoes;
            $scope.ChangeRequest.ImpactosPositivos = result.ImpactosPositivos;
            $scope.ChangeRequest.ImpactosNegativos = result.ImpactosNegativos;
            $scope.ChangeRequestRecursosSolicitados = result.RecursosSolicitados;
        });
    }

    $scope.ShowChangeRequest2 = function () {
        $('#CR2').show();
        $http({
            method: 'GET',
            url: '/ChangeRequest/Get?id=' + $scope.ChangeRequest2.Id
        }).success(function (result) {

            $scope.ChangeRequest2 = {};
            $scope.ChangeRequest2.Id = result.Id;
            $scope.ChangeRequest2.Codigo = result.Codigo;
            $scope.ChangeRequest2.Versao = result.Versao;
            $scope.ChangeRequest2.Autor = result.Autor;
            $scope.ChangeRequest2.Situacao = result.Situacao;
            $scope.ChangeRequest2.GPI = result.GPI;
            $scope.ChangeRequest2.GPE = result.GPE;
            $scope.ChangeRequest2.Departamento = result.Departamento;
            $scope.ChangeRequest2.Processo = result.Processo;
            $scope.ChangeRequest2.Descricao = result.Descricao;
            $scope.ChangeRequest2.Motivos = result.Motivos;
            $scope.ChangeRequest2.Recomendacoes = result.Recomendacoes;
            $scope.ChangeRequest2.ImpactosPositivos = result.ImpactosPositivos;
            $scope.ChangeRequest2.ImpactosNegativos = result.ImpactosNegativos;
            $scope.ChangeRequestRecursosSolicitados2 = result.RecursosSolicitados;
        });
    }

    function ValidateChangeRequest() {
        if ($scope.ChangeRequest.Codigo == null || $scope.ChangeRequest.Codigo == '') {
            noty({ text: 'Preencha o campo "Código"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest.Versao == null || $scope.ChangeRequest.Versao == '') {
            noty({ text: 'Preencha o campo "Versão"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest.Autor == null || $scope.ChangeRequest.Autor == '') {
            noty({ text: 'Preencha o campo "Autor"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest.Situacao == null || $scope.ChangeRequest.Situacao == '') {
            noty({ text: 'Preencha o campo "Status"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest.GPI == null || $scope.ChangeRequest.GPI == '') {
            noty({ text: 'Preencha o campo "Gerente de Projeto (CVA)"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest.GPE == null || $scope.ChangeRequest.GPE == '') {
            noty({ text: 'Preencha o campo "Gerente de Projeto (Cliente)"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest.Departamento == null || $scope.ChangeRequest.Departamento == '') {
            noty({ text: 'Preencha o campo "Departamento"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest.Processo == null || $scope.ChangeRequest.Processo == '') {
            noty({ text: 'Preencha o campo "Processo"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest.Descricao == null || $scope.ChangeRequest.Descricao == '') {
            noty({ text: 'Preencha o campo "Descrição"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest.Motivos == null || $scope.ChangeRequest.Motivos == '') {
            noty({ text: 'Preencha o campo "Motivos"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest.Recomendacoes == null || $scope.ChangeRequest.Recomendacoes == '') {
            noty({ text: 'Preencha o campo "Recomendações"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest.ImpactosPositivos == null || $scope.ChangeRequest.ImpactosPositivos == '') {
            noty({ text: 'Preencha o campo "Impactos Positivos"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequest.ImpactosNegativos == null || $scope.ChangeRequest.ImpactosNegativos == '') {
            noty({ text: 'Preencha o campo "Impactos caso não aprovada"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ChangeRequestRecursosSolicitados == null || $scope.ChangeRequestRecursosSolicitados.length <= 0) {
            noty({ text: 'Insira pelo menos um "Recurso Solicitado"', layout: 'topRight', type: 'error' });
            return false;
        }

        return true;
    }

    function LoadChangeRequest(type) {
        var changeRequest = {};
        if (type == 1) {
            changeRequest.Id = $scope.ChangeRequest.Id;
            changeRequest.Projeto = {};
            changeRequest.Projeto.Id = $scope.Projeto.Id;
            changeRequest.Codigo = $scope.ChangeRequest.Codigo;
            changeRequest.Versao = $scope.ChangeRequest.Versao;
            changeRequest.Autor = $scope.ChangeRequest.Autor;
            changeRequest.Situacao = "Aguardando";
            changeRequest.GPI = $scope.ChangeRequest.GPI;
            changeRequest.GPE = $scope.ChangeRequest.GPE;
            changeRequest.Departamento = $scope.ChangeRequest.Departamento;
            changeRequest.Processo = $scope.ChangeRequest.Processo;
            changeRequest.Descricao = $scope.ChangeRequest.Descricao;
            changeRequest.Motivos = $scope.ChangeRequest.Motivos;
            changeRequest.Recomendacoes = $scope.ChangeRequest.Recomendacoes;
            changeRequest.ImpactosPositivos = $scope.ChangeRequest.ImpactosPositivos;
            changeRequest.ImpactosNegativos = $scope.ChangeRequest.ImpactosNegativos;
            changeRequest.RecursosSolicitados = $scope.ChangeRequestRecursosSolicitados;
        }

        if (type == 2) {
            changeRequest.Id = $scope.ChangeRequest2.Id;
            changeRequest.Projeto = {};
            changeRequest.Projeto.Id = $scope.Projeto.Id;
            changeRequest.Codigo = $scope.ChangeRequest2.Codigo;
            changeRequest.Versao = $scope.ChangeRequest2.Versao;
            changeRequest.Autor = $scope.ChangeRequest2.Autor;
            changeRequest.Situacao = $scope.ChangeRequest2.Situacao;
            changeRequest.GPI = $scope.ChangeRequest2.GPI;
            changeRequest.GPE = $scope.ChangeRequest2.GPE;
            changeRequest.Departamento = $scope.ChangeRequest2.Departamento;
            changeRequest.Processo = $scope.ChangeRequest2.Processo;
            changeRequest.Descricao = $scope.ChangeRequest2.Descricao;
            changeRequest.Motivos = $scope.ChangeRequest2.Motivos;
            changeRequest.Recomendacoes = $scope.ChangeRequest2.Recomendacoes;
            changeRequest.ImpactosPositivos = $scope.ChangeRequest2.ImpactosPositivos;
            changeRequest.ImpactosNegativos = $scope.ChangeRequest2.ImpactosNegativos;
            changeRequest.RecursosSolicitados = $scope.ChangeRequestRecursosSolicitados2;
        }

        return changeRequest;
    }

    /*---Status Report---*/
    $scope.AddStatusReport = function () {
        if ($scope.StatusReport == undefined || $scope.StatusReport == null || $scope.StatusReport == '') {
            noty({ text: 'Preencha o Status Report', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.StatusReport.Data == undefined || $scope.StatusReport.Data == null || $scope.StatusReport.Data == '') {
            noty({ text: 'Preencha o campo "Data"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.StatusReport.Descricao == undefined || $scope.StatusReport.Descricao == null || $scope.StatusReport.Descricao == '') {
            noty({ text: 'Preencha o campo "Descrição"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.StatusReport.PontosAtencao == undefined || $scope.StatusReport.PontosAtencao == null || $scope.StatusReport.PontosAtencao == '') {
            noty({ text: 'Preencha o campo "Pontos de Atenção"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.StatusReport.PlanoDeAcao == undefined || $scope.StatusReport.PlanoDeAcao == null || $scope.StatusReport.PlanoDeAcao == '') {
            noty({ text: 'Preencha o campo "Plano de Ação"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.StatusReport.Conquistas == undefined || $scope.StatusReport.Conquistas == null || $scope.StatusReport.Conquistas == '') {
            noty({ text: 'Preencha o campo "Conquistas"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.StatusReport.ProximosPassos == undefined || $scope.StatusReport.ProximosPassos == null || $scope.StatusReport.ProximosPassos == '') {
            noty({ text: 'Preencha o campo "Próximos Passos"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.StatusReport.Data < $scope.Projeto.DataInicial) {
            noty({ text: 'Não é possível gerar Status Report com data anterior ao início do projeto', layout: 'topRight', type: 'error' });
            return false;
        }

        var model = {};
        model.Projeto = {};
        model.Projeto.Id = $scope.Projeto.Id;
        model.Data = $scope.StatusReport.Data;
        model.Descricao = $scope.StatusReport.Descricao;
        model.PontosAtencao = $scope.StatusReport.PontosAtencao;
        model.PlanoDeAcao = $scope.StatusReport.PlanoDeAcao;
        model.Conquistas = $scope.StatusReport.Conquistas;
        model.ProximosPassos = $scope.StatusReport.ProximosPassos;
        model.Fases = $scope.FasesProjeto;

        $http.post('/StatusReport/Save', model).success(function (result) {
            if (result.Success == null || result.Success == undefined) {
                $('#message-error').text('');
                $('#message-error').append(result.Error.Message);
                $('#message-box-danger').addClass('open');
            }
            else {
                $('#message-success').text('');
                $('#message-success').append(result.Success.Message);
                $('#message-box-success').addClass('open');
            }
        });
        $scope.Get_StatusReport();
        $scope.StatusReport = {};
    }

    $scope.Get_StatusReport = function () {
        $http({
            method: 'GET',
            url: '/StatusReport/Get_All?id=' + $scope.Projeto.Id
        }).success(function (result) {
            $scope.StatusReports = result;
        });
    }

    $scope.ViewStatusReport = function (index) {
        $scope.StatusReport = {};
        // $scope.StatusReport.Data = $scope.StatusReports[index].Data;
        $scope.StatusReport.HorasOrcadas = $scope.StatusReports[index].HorasOrcadas;
        $scope.StatusReport.HorasConsumidas = $scope.StatusReports[index].HorasConsumidas;
        $scope.StatusReport.Descricao = $scope.StatusReports[index].Descricao;
        $scope.StatusReport.PontosAtencao = $scope.StatusReports[index].PontosAtencao;
        $scope.StatusReport.PlanoDeAcao = $scope.StatusReports[index].PlanoDeAcao;
        $scope.StatusReport.Conquistas = $scope.StatusReports[index].Conquistas;
        $scope.StatusReport.ProximosPassos = $scope.StatusReports[index].ProximosPassos;
    }



    function LoadModel() {
        $scope.Projeto.Membros = $scope.Membros;
        $scope.Projeto.Recursos = $scope.Recursos;
        $scope.Projeto.Fases = $scope.FasesProjeto;
        //$scope.Projeto.Pricing.ItensPricing = $scope.ItensPricing;
        $scope.Projeto.SpecialtyRules = $scope.RegrasEspecialidades;
        $scope.Projeto.Itens = $scope.RecursosFase;
        $scope.Projeto.StatusReport = $scope.StatusReports;
        $scope.Projeto.ChangeRequests = [];
    }

    function LoadPricingModel() {
        //if ($scope.Projeto.Id == undefined && $scope.Projeto.Id == null) {
        //    $scope.Projeto.Pricing = {};
        //    $scope.Projeto.Pricing = $scope.Pricing;
        //    $scope.Pricing.ItensPricing = $scope.ItensPricing;
        //}
        //else {
        $scope.Projeto.Pricing = {};
        $scope.Projeto.Pricing.Projeto = {};
        $scope.Projeto.Pricing.Projeto.Id = $scope.Projeto.Id;
        $scope.Projeto.Pricing = $scope.Pricing;
        $scope.Projeto.Pricing.ItensPricing = $scope.ItensPricing;
        //}
    }

    function ValidatePricingModel() {
        if ($scope.Pricing == null) {
            noty({ text: 'Preencha o formulário', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Pricing.Nome == undefined || $scope.Pricing.Nome == null || $scope.Pricing.Nome == '') {
            noty({ text: 'Obrigatório informar o nome da Pricing', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Pricing.Solicitante == undefined || $scope.Pricing.Solicitante == null || $scope.Pricing.Solicitante == '') {
            noty({ text: 'Obrigatório informar o solicitante da Pricing', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Pricing.Data == undefined || $scope.Pricing.Data == null || $scope.Pricing.Data == '') {
            noty({ text: 'Obrigatório informar a data da Pricing', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ItensPricing.length == 0) {
            noty({ text: 'Obrigatório informar ao menos uma especialidade da Pricing', layout: 'topRight', type: 'error' });
            return false;
        }

        return true;
    }


    function ValidatePercentage() {
        if ($scope.Pricing.PorcentagemBackoffice == undefined || $scope.Pricing.PorcentagemBackoffice == '')
            $scope.Pricing.PorcentagemBackoffice = 0;
        if ($scope.Pricing.PorcentagemRisco == undefined || $scope.Pricing.PorcentagemRisco == '')
            $scope.Pricing.PorcentagemRisco = 0;
        if ($scope.Pricing.PorcentagemMargem == undefined || $scope.Pricing.PorcentagemMargem == '')
            $scope.Pricing.PorcentagemMargem = 0;
        if ($scope.Pricing.PorcentagemComissao == undefined || $scope.Pricing.PorcentagemComissao == '')
            $scope.Pricing.PorcentagemComissao = 0;
        if ($scope.Pricing.PorcentagemImposto == undefined || $scope.Pricing.PorcentagemImposto == '')
            $scope.Pricing.PorcentagemImposto = 0;
    }


    function ValidateModel() {
        if ($scope.Projeto == null) {
            noty({ text: 'Preencha o formulário', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Projeto.Codigo == undefined || $scope.Projeto.Codigo == null || $scope.Projeto.Codigo == '') {
            noty({ text: 'Obrigatório informar o código', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Projeto.Nome == undefined || $scope.Projeto.Nome == null || $scope.Projeto.Nome == '') {
            noty({ text: 'Obrigatório informar o nome', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Projeto.DataInicial == undefined || $scope.Projeto.DataInicial == null || $scope.Projeto.DataInicial == '') {
            noty({ text: 'Obrigatório informar a data inicial', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Projeto.DataPrevista == undefined || $scope.Projeto.DataPrevista == null || $scope.Projeto.DataPrevista == '') {
            noty({ text: 'Obrigatório informar a data prevista', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Projeto.Cliente.Id == undefined || $scope.Projeto.Cliente.Id == null) {
            noty({ text: 'Obrigatório informar o cliente', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Projeto.TipoProjeto.Id == undefined || $scope.Projeto.TipoProjeto.Id == null) {
            noty({ text: 'Obrigatório informar o tipo de projeto', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Projeto.Status.Id == undefined || $scope.Projeto.Status.Id == null) {
            noty({ text: 'Obrigatório informar o status', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Projeto.ResponsavelDespesa == undefined || $scope.Projeto.ResponsavelDespesa == null || $scope.Projeto.ResponsavelDespesa == '') {
            noty({ text: 'Obrigatório informar a política de despesa', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Projeto.Gerente == undefined || $scope.Projeto.Gerente.Id == null) {
            noty({ text: 'Obrigatório informar o gerente', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Projeto.Descricao == undefined || $scope.Projeto.Descricao == null || $scope.Projeto.Descricao == '') {
            noty({ text: 'Obrigatório informar a descrição', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Projeto.Recursos == undefined || $scope.Projeto.Recursos.length == 0) {
            noty({ text: 'Obrigatório informar ao menos um recurso do projeto', layout: 'topRight', type: 'error' });
            return false;
        }

        if ($scope.Projeto.Fases == undefined || $scope.Projeto.Fases.length == 0) {
            noty({ text: 'Obrigatório informar ao menos uma fase do projeto', layout: 'topRight', type: 'error' });
            return false;
        }

        return true;
    }

    function CreateArrays() {
        $scope.Especialidades2 = [];
        $scope.Colaboradores2 = [];
        $scope.StepResourceLine = -1
        $scope.PricingLine = -1;
        $scope.MemberLine = -1;
        $scope.StepLine = -1;
        $scope.ResourceLine = -1;
        $scope.SpecialtyLine = -1;
        $scope.ChangeRequestResourceLine = -1;
        $scope.Clientes = [];
        $scope.Colaboradores = [];
        $scope.Fases = [];
        $scope.Membros = [];
        $scope.Recursos = [];
        $scope.Servicos = [];
        $scope.TiposProjeto = [];
        $scope.ColaboradorEspecialidades = [];
        $scope.RegrasEspecialidades = [];
        $scope.Fases = [];
        $scope.FasesProjeto = [];
        $scope.Pricings = [];
        $scope.ItensPricing = [];
        $scope.Pricing = {};
        $scope.RecursosFase = [];
        $scope.StatusReports = [];
        $scope.Gerentes = [];
        $scope.ChangeRequests = [];
        $scope.ChangeRequestRecursosSolicitados = [];
    }

    function LoadArrays() {
        $scope.GetClients();
        $scope.GetProjectTypes();
        $scope.GetCollaborators();
        $scope.GetSteps();
        $scope.GetSpecialties();
        $scope.GetManagers();
    }


    //Métodos para carregamento de combos

    $scope.GetClients = function () {
        $http({
            method: 'GET',
            url: '/Cliente/LoadCombo'
        }).success(function (result) {
            $scope.Clientes = result;
            document.getElementById('load-01').hidden = true;
        });
    }

    $scope.GetProjectTypes = function () {
        $http({
            method: 'GET',
            url: '/TiposProjeto/Get_All'
        }).success(function (result) {
            $scope.TiposProjeto = result;
            document.getElementById('load-02').hidden = true;
        });
    }

    $scope.GetCollaborators = function () {
        $http({
            method: 'GET',
            url: '/Colaborador/LoadCombo'
        }).success(function (result) {
            $scope.Colaboradores = result;
            document.getElementById('load-03').hidden = true;
        });
    }

    $scope.GetSpecialties = function () {
        $http({
            method: 'GET',
            url: '/Especialidade/Get_All'
        }).success(function (result) {
            $scope.Especialidades2 = result;

        });
    }



    $scope.GetSteps = function () {
        $http({
            method: 'GET',
            url: '/Projeto/GetSteps'
        }).success(function (result) {
            $scope.Fases = result;
            document.getElementById('load-05').hidden = true;
        });
    }

    $scope.GetPricingInfo = function (id, index) {
        $http({
            method: 'GET',
            url: '/Pricing/Get?id=' + $scope.Pricing.Id
        }).success(function (result) {
            $scope.Pricing = result;
            $scope.Pricing.Data = new Date(result.Data);
            $scope.ItensPricing = result.ItensPricing;
            $scope.PricingLine = index;
            $scope.CalculateCost();
            $scope.CalculatePricing();
        });
    }

    $scope.GetManagers = function () {
        $http({
            method: 'GET',
            url: '/Colaborador/Get_Managers'
        }).success(function (result) {
            $scope.Gerentes = result;
            document.getElementById('load-06').hidden = true;
        });
    }

    $scope.GetChangeRequests = function () {
        $http({
            method: 'GET',
            url: '/ChangeRequest/Get_for_Project?id=' + $scope.Projeto.Id
        }).success(function (result) {
            $scope.ChangeRequests = result;
        });
    }

    $scope.Print = function (id) {
        window.location.href = '/Projeto/StatusReportParcial?id=' + id
    }
}]);