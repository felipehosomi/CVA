cvaGestao.controller('ProjectController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {
    $scope.OnLoad = function (model) {
        document.getElementById('RecursoInfo').hidden = true;
        document.getElementById('Justificativa').hidden = true;


        CreateModel();
        CreateArrays();
        LoadArrays();


        /* -- LoadModel -- */
        var stu = model.Status.Id;
        $scope.Projeto = model;
        $scope.Projeto.Codigo = model.Codigo;
        $scope.Projeto.Status.Id = stu.toString();
        $scope.Projeto.DataInicial = new Date(model.DataInicial);
        $scope.Projeto.DataPrevista = new Date(model.DataPrevista);
        $scope.Projeto.Cliente = model.Cliente;
        $scope.Projeto.TipoProjeto = model.TipoProjeto;
        $scope.Projeto.Fases = model.Fases;
        $scope.Projeto.Recursos = model.Recursos;
        $scope.Projeto.Membros = model.Membros;
        $scope.Projeto.Alteracoes = model.Alteracoes;

        $scope.Codigo = $scope.Projeto.Codigo + ' - ' + $scope.Projeto.Tag + ' ' + $scope.Projeto.Nome;
    }

    function CreateModel() {
        $scope.Projeto = {};
        $scope.Projeto.Status = {};
        $scope.Projeto.Cliente = {};
        $scope.Projeto.Gerente = {};
        $scope.Projeto.TipoProjeto = {};
        $scope.Projeto.Fases = [];
        $scope.Projeto.Recursos = [];
        $scope.Projeto.Membros = [];
        $scope.Projeto.Alteracoes = [];
    }

    function CreateArrays() {
        $scope.Clientes = [];
        $scope.TiposProjeto = [];
        $scope.Gerentes = [];
        $scope.Fases = [];
        $scope.Especialidades = [];
        $scope.Colaboradores = [];
        $scope.FaseLine = -1
        $scope.RecursoLine = -1;
        $scope.MembroLine = -1;
        $scope.Alteracoes = [];
    }

    function LoadArrays() {
        $http({
            method: 'GET',
            url: '/Cliente/LoadCombo'
        }).success(function (result) {
            $scope.Clientes = result;
            document.getElementById('load-01').hidden = true;
        });

        $http({
            method: 'GET',
            url: '/TiposProjeto/Get_All'
        }).success(function (result) {
            $scope.TiposProjeto = result;
            document.getElementById('load-02').hidden = true;
        });

        $http({
            method: 'GET',
            url: '/Projeto/GetSteps'
        }).success(function (result) {
            $scope.Fases = result;
            document.getElementById('load-05').hidden = true;
        });

        $http({
            method: 'GET',
            url: '/Especialidade/Get_All'
        }).success(function (result) {
            $scope.Especialidades = result;

        });

        $http({
            method: 'GET',
            url: '/Colaborador/Get_Managers'
        }).success(function (result) {
            $scope.Gerentes = result;
            document.getElementById('load-06').hidden = true;
        });

    }

    /*---Fases---*/
    $scope.AddStep = function () {
        if ($scope.Fase == undefined || $scope.Fase.StepId == null || $scope.Fase.StepId == '') {
            noty({ text: 'Obrigatório informar a fase', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Fase == undefined || $scope.Fase.DataInicio == null || $scope.Fase.DataInicio == '') {
            noty({ text: 'Obrigatório informar a data inicial da fase', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Fase == undefined || $scope.Fase.Liberada == null || $scope.Fase.Liberada == '') {
            noty({ text: 'Obrigatório informar se a fase está liberada para apontamentos', layout: 'topRight', type: 'error' });
            return false;
        }

        if ($scope.FaseLine == -1) {
            for (var i = 0; i < $scope.Projeto.Fases.length; i++) {
                if ($scope.Fase.StepId == $scope.Projeto.Fases[i].StepId) {
                    noty({ text: 'Este projeto já possui uma fase deste tipo', layout: 'topRight', type: 'error' });
                    return false;
                }
            }
        }

        var line = {};

        line.Id = $scope.Fase.Id;
        line.StepId = $scope.Fase.StepId;
        line.Liberada = $scope.Fase.Liberada;
        for (var i = 0; i < $scope.Fases.length; i++) {
            if ($scope.Fases[i].Id == line.StepId) {
                line.Nome = $scope.Fases[i].Nome;
                break;
            }
        }

        line.DataInicio = $scope.Fase.DataInicio;
        line.DataPrevista = $scope.Fase.DataPrevista;

        if ($scope.FaseLine == -1) {
            line.Id = 0;
            line.HorasOrcadas = '0';
            line.HorasConsumidas = '0';
            var i = $scope.Projeto.Fases.length;
            $scope.Projeto.Fases[i] = line;
        }

        else {
            var i = $scope.FaseLine;
            line.HorasOrcadas = $scope.Fase.HorasOrcadas;
            line.HorasConsumidas = $scope.Fase.HorasConsumidas;
            $scope.Projeto.Fases[i] = line;
        }

        $scope.Fase = {};
        $scope.FaseLine = -1;
    }

    $scope.EditStep = function (index) {
        $scope.Fase = {};
        $scope.Fase.Id = $scope.Projeto.Fases[index].Id;
        $scope.Fase.StepId = $scope.Projeto.Fases[index].StepId;
        $scope.Fase.Nome = $scope.Projeto.Fases[index].Nome;
        $scope.Fase.DataInicio = new Date($scope.Projeto.Fases[index].DataInicio);
        $scope.Fase.DataPrevista = new Date($scope.Projeto.Fases[index].DataPrevista);
        $scope.Fase.HorasOrcadas = $scope.Projeto.Fases[index].HorasOrcadas;
        $scope.Fase.HorasConsumidas = $scope.Projeto.Fases[index].HorasConsumidas;
        $scope.Fase.Liberada = $scope.Projeto.Fases[index].Liberada;

        $scope.FaseLine = index;
    }

    $scope.RemoveStep = function (index) {
        if ($scope.Projeto.Fases[index].HorasConsumidas > 0) {
            noty({ text: 'Não é possível remover uma fase que já possua apontamentos.', layout: 'topRight', type: 'error' });
            return false;
        }
        else {
            $scope.Projeto.Fases[index].ProjectId = $scope.Projeto.Id;

            $http.post('/Projeto/Remove_Step', $scope.Projeto.Fases[index]).success(function (result) {
                if (result.Success == null || result.Success == undefined) {
                    $('#message-error').text('');
                    $('#message-error').append(result.Error.Message);
                    $('#message-box-danger').addClass('open');
                }
                else {
                    $scope.Projeto.Fases.splice(index, 1);
                }
            });
        }
    }

    /*---Recursos---*/
    $scope.AddResource = function () {
        if ($scope.Recurso.FaseId == undefined || $scope.Recurso.FaseId == null || $scope.Recurso.FaseId == '') {
            noty({ text: 'Obrigatório informar a fase', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Recurso.EspecialidadeId == undefined || $scope.Recurso.EspecialidadeId == null || $scope.Recurso.EspecialidadeId == '') {
            noty({ text: 'Obrigatório informar a especialidade', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Recurso.ColaboradorId == undefined || $scope.Recurso.ColaboradorId == null || $scope.Recurso.ColaboradoId == '') {
            noty({ text: 'Obrigatório informar o colaborador', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Colaboradores.length == 0) {
            noty({ text: 'Aguarde carregando colaborador', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Recurso.Horas == undefined || $scope.Recurso.Horas == null || $scope.Recurso.Horas == '') {
            noty({ text: 'Obrigatório informar uma quantidade de horas', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Recurso.Status == undefined || $scope.Recurso.Status.Id == null || $scope.Recurso.Status.Id == '') {
            noty({ text: 'Obrigatório informar o status', layout: 'topRight', type: 'error' });
            return false;
        }

        var line = {};
        line.Status = {};
        line.Status.Id = $scope.Recurso.Status.Id;

        if (line.Status.Id == 1)
            line.Status.Descricao = "Ativo";
        if (line.Status.Id == 2)
            line.Status.Descricao = "Inativo";

        line.Fase = {};
        for (var i = 0; i < $scope.Projeto.Fases.length; i++) {
            if ($scope.Projeto.Fases[i].StepId == $scope.Recurso.FaseId) {
                line.FaseId = $scope.Projeto.Fases[i].StepId;
                line.FaseNome = $scope.Projeto.Fases[i].Nome;
                break;
            }
        }

        line.Especialidade = {};
        for (var i = 0; i < $scope.Especialidades.length; i++) {
            if ($scope.Especialidades[i].Id == $scope.Recurso.EspecialidadeId) {
                line.EspecialidadeId = $scope.Especialidades[i].Id;
                line.EspecialidadeNome = $scope.Especialidades[i].Name;
                break;
            }
        }

        line.Colaborador = {};
        for (var i = 0; i < $scope.Colaboradores.length; i++) {
            if ($scope.Colaboradores[i].Id == $scope.Recurso.ColaboradorId) {
                line.ColaboradorId = $scope.Colaboradores[i].Id;
                line.ColaboradorNome = $scope.Colaboradores[i].Nome;
                break;
            }
        }

        line.Horas = $scope.Recurso.Horas;
        line.HorasFormat = line.Horas + ':00';

        for (var i = 0; i < $scope.Projeto.Recursos.length; i++) {
            if ($scope.Projeto.Recursos[i].ColaboradorId == $scope.Recurso.ColaboradorId) {
                line.HorasConsumidasFormat = $scope.Projeto.Recursos[i].HorasConsumidasFormat;
                break;
            }
        }

        if ($scope.RecursoLine == -1) {
            var i = $scope.Projeto.Recursos.length;
            $scope.Projeto.Recursos[i] = line;
            line.HorasConsumidas = 0;
        }

        else {

            line.HorasConsumidas = $scope.Recurso.HorasConsumidas;
            if (line.Horas < $scope.Recurso.HorasConsumidas) {
                noty({ text: 'Quantidade de horas orçada não pode ser menor que a quantidade de horas apontada', layout: 'topRight', type: 'error' });
                return false;
            }

            if ($scope.Justificativa == undefined || $scope.Justificativa == null || $scope.Justificativa == '') {
                noty({ text: 'Obrigatório justificar a edição no recurso', layout: 'topRight', type: 'error' });
                return false;
            }            


            var justificativa = {};

            justificativa = line;

            justificativa.Data = new Date();
            justificativa.Descricao = $scope.Justificativa;
            justificativa.HorasAdicionadas = Number(line.Horas) - Number($scope.Projeto.Recursos[$scope.RecursoLine].Horas);

            var i = $scope.RecursoLine;
            $scope.Projeto.Recursos[i] = line;



            var j = $scope.Projeto.Alteracoes.length;
            $scope.Projeto.Alteracoes[j] = justificativa;
        }

        $scope.Recurso = {};
        $scope.RecursoLine = -1;


        var horasFase = 0;

        for (var x = 0; x < $scope.Projeto.Recursos.length; x++) {
            if ($scope.Projeto.Recursos[x].FaseId == line.FaseId) {
                horasFase = Number(horasFase) + Number($scope.Projeto.Recursos[x].Horas);
            }
        }
        

        for (var i = 0; i < $scope.Projeto.Fases.length; i++) {
            if ($scope.Projeto.Fases[i].StepId == line.FaseId) {
                $scope.Projeto.Fases[i].HorasOrcadas = horasFase;
            }
        }

        $scope.Justificativa = '';
        document.getElementById('Justificativa').hidden = true;

        document.getElementById('Fase').enabled = true;
        document.getElementById('Especialidade').enabled = true;
        document.getElementById('Colaborador').enabled = true;
        $scope.IsProcessing = false;
    }

    
    $scope.EditResource = function (index) {

        $scope.IsProcessing = true;

        $scope.Recurso = {};
        $scope.Recurso.FaseId = $scope.Projeto.Recursos[index].FaseId;
        $scope.Recurso.Horas = $scope.Projeto.Recursos[index].Horas;
        $scope.Recurso.HorasConsumidas = $scope.Projeto.Recursos[index].HorasConsumidas;

        $scope.Recurso.EspecialidadeId = $scope.Projeto.Recursos[index].EspecialidadeId;

        $scope.Recurso.Status = {};
        $scope.Recurso.Status.Id = $scope.Projeto.Recursos[index].Status.Id.toString();
        $scope.Get_CollaboratorBySpecialty($scope.Recurso.EspecialidadeId);

        $scope.Recurso.ColaboradorId = $scope.Projeto.Recursos[index].ColaboradorId;
        $scope.RecursoLine = index;
        document.getElementById('Justificativa').hidden = false;
    }

    $scope.RemoveResource = function (index) {
        for (var i = 0; i < $scope.Projeto.Fases.length; i++) {
            if ($scope.Projeto.Fases[i].StepId == $scope.Projeto.Recursos[index].FaseId) {
                $scope.Projeto.Fases[i].HorasOrcadas = Number($scope.Projeto.Fases[i].HorasOrcadas) - Number($scope.Projeto.Recursos[index].Horas);
            }
        }

        $scope.Projeto.Recursos.splice(index, 1);
    }

    $scope.ShowInfo = function (index) {
        $scope.Alteracoes = [];

        $scope.NomeFase = $scope.Projeto.Recursos[index].FaseNome;

        for (var i = 0; i < $scope.Projeto.Alteracoes.length; i++) {
            if ($scope.Projeto.Alteracoes[i].FaseNome == $scope.NomeFase) {
                $scope.Alteracoes[$scope.Alteracoes.length] = $scope.Projeto.Alteracoes[i];
            }
        }

        document.getElementById('RecursoInfo').hidden = false;
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

        var line = {};
        line.Nome = $scope.Membro.Nome;
        line.Telefone = $scope.Membro.Telefone;
        line.Email = $scope.Membro.Email;
        line.Departamento = $scope.Membro.Departamento;

        if ($scope.MembroLine == -1) {
            var i = $scope.Projeto.Membros.length;
            $scope.Projeto.Membros[i] = line;
        }

        else {
            var i = $scope.MembroLine;
            $scope.Projeto.Membros[i] = line;
        }

        $scope.Membro = {};
        $scope.MembroLine = -1;
    }

    $scope.EditMember = function (index) {
        $scope.Membro.Nome = $scope.Projeto.Membros[index].Nome;
        $scope.Membro.Telefone = $scope.Projeto.Membros[index].Telefone;
        $scope.Membro.Email = $scope.Projeto.Membros[index].Email;
        $scope.Membro.Departamento = $scope.Projeto.Membros[index].Departamento;

        $scope.MembroLine = index;
    }

    $scope.RemoveMember = function (index) {
        $scope.Projeto.Membros.splice(index, 1);
    }


    $scope.Salvar = function () {
        if (ValidateModel()) {
            $http.post('/Projeto/Salvar', $scope.Projeto)
                .success(function (message) {
                    if (message.Success == null || message.Success == undefined) {
                        $('#message-error').text('');
                        $('#message-error').append(message.Error.Message);
                        $('#message-box-danger').addClass('open');
                    }
                    else {
                        $('#message-success').text('');
                        $('#message-success').append(message.Success.Message);
                        $('#message-box-success').addClass('open');
                    }
                });
        }
    }

    $scope.Get_CollaboratorBySpecialty = function (id) {
        document.getElementById('load-09').hidden = false;
        $http({
            method: 'GET',
            url: '/Colaborador/Get_CollaboratorBySpecialty?id=' + id
        }).success(function (result) {
            $scope.Colaboradores = result;
            document.getElementById('load-09').hidden = true;
        });
    }

    function ValidateModel() {
        if ($scope.Projeto.DataInicial == undefined || $scope.Projeto.DataInicial == null || $scope.Projeto.DataInicial == '') {
            noty({ text: 'Obrigatório informar a data inicial', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Projeto.DataPrevista == undefined || $scope.Projeto.DataPrevista == null || $scope.Projeto.DataPrevista == '') {
            noty({ text: 'Obrigatório informar a data prevista', layout: 'topRight', type: 'error' });
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
        if ($scope.Projeto.Fases == undefined || $scope.Projeto.Fases.length == 0) {
            noty({ text: 'Obrigatório informar ao menos uma fase do projeto', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Projeto.Recursos == undefined || $scope.Projeto.Recursos.length == 0) {
            noty({ text: 'Obrigatório informar ao menos um recurso do projeto', layout: 'topRight', type: 'error' });
            return false;
        }

        return true;
    }
}]);

//$scope.GetChangeRequests = function () {
//    $http({
//        method: 'GET',
//        url: '/ChangeRequest/Get_for_Project?id=' + $scope.Projeto.Id
//    }).success(function (result) {
//        $scope.ChangeRequests = result;
//    });
//}


///*---Status Report---*/
//$scope.AddStatusReport = function () {
//    if ($scope.StatusReport == undefined || $scope.StatusReport == null || $scope.StatusReport == '') {
//        noty({
//            text: 'Preencha o Status Report', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.StatusReport.Data == undefined || $scope.StatusReport.Data == null || $scope.StatusReport.Data == '') {
//        noty({
//            text: 'Preencha o campo "Data"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.StatusReport.Descricao == undefined || $scope.StatusReport.Descricao == null || $scope.StatusReport.Descricao == '') {
//        noty({
//            text: 'Preencha o campo "Descrição"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.StatusReport.PontosAtencao == undefined || $scope.StatusReport.PontosAtencao == null || $scope.StatusReport.PontosAtencao == '') {
//        noty({
//            text: 'Preencha o campo "Pontos de Atenção"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.StatusReport.PlanoDeAcao == undefined || $scope.StatusReport.PlanoDeAcao == null || $scope.StatusReport.PlanoDeAcao == '') {
//        noty({
//            text: 'Preencha o campo "Plano de Ação"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.StatusReport.Conquistas == undefined || $scope.StatusReport.Conquistas == null || $scope.StatusReport.Conquistas == '') {
//        noty({
//            text: 'Preencha o campo "Conquistas"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.StatusReport.ProximosPassos == undefined || $scope.StatusReport.ProximosPassos == null || $scope.StatusReport.ProximosPassos == '') {
//        noty({
//            text: 'Preencha o campo "Próximos Passos"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.StatusReport.Data < $scope.Projeto.DataInicial) {
//        noty({
//            text: 'Não é possível gerar Status Report com data anterior ao início do projeto', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }

//    var model = {};
//    model.Projeto = {};
//    model.Projeto.Id = $scope.Projeto.Id;
//    model.Data = $scope.StatusReport.Data;
//    model.Descricao = $scope.StatusReport.Descricao;
//    model.PontosAtencao = $scope.StatusReport.PontosAtencao;
//    model.PlanoDeAcao = $scope.StatusReport.PlanoDeAcao;
//    model.Conquistas = $scope.StatusReport.Conquistas;
//    model.ProximosPassos = $scope.StatusReport.ProximosPassos;
//    model.Fases = $scope.Projeto.Fases;

//    $http.post('/StatusReport/Save', model).success(function (result) {
//        if (result.Success == null || result.Success == undefined) {
//            $('#message-error').text('');
//            $('#message-error').append(result.Error.Message);
//            $('#message-box-danger').addClass('open');
//        }
//        else {
//            $('#message-success').text('');
//            $('#message-success').append(result.Success.Message);
//            $('#message-box-success').addClass('open');
//        }
//    });
//    $scope.Get_StatusReport();
//    $scope.StatusReport = {
//    };
//}



//$scope.ViewStatusReport = function (index) {
//    $scope.StatusReport = {
//    };
//    // $scope.StatusReport.Data = $scope.StatusReports[index].Data;
//    $scope.StatusReport.HorasOrcadas = $scope.StatusReports[index].HorasOrcadas;
//    $scope.StatusReport.HorasConsumidas = $scope.StatusReports[index].HorasConsumidas;
//    $scope.StatusReport.Descricao = $scope.StatusReports[index].Descricao;
//    $scope.StatusReport.PontosAtencao = $scope.StatusReports[index].PontosAtencao;
//    $scope.StatusReport.PlanoDeAcao = $scope.StatusReports[index].PlanoDeAcao;
//    $scope.StatusReport.Conquistas = $scope.StatusReports[index].Conquistas;
//    $scope.StatusReport.ProximosPassos = $scope.StatusReports[index].ProximosPassos;
//}


/*---Change Requests---*/
//$scope.SaveChangeRequest = function (type) {
//    if (type == 1) {
//        if ($scope.ChangeRequest.Situacao == "Aprovada") {
//            noty({
//                text: 'Não é possível editar uma Change Request que já tenha sido aprovada.', layout: 'topRight', type: 'error'
//            });
//            return false;
//        }

//        if (ValidateChangeRequest()) {
//            var changeRequest = LoadChangeRequest(1);

//            $http.post('/ChangeRequest/Save', changeRequest).success(function (result) {
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

//            $scope.ChangeRequest = {
//            };
//            $scope.ChangeRequestRecursosSolicitados = [];
//            $scope.GetChangeRequests();
//        }
//    }

//    if (type == 2) {
//        var changeRequest = LoadChangeRequest(2);

//        $http.post('/ChangeRequest/Save', changeRequest).success(function (result) {
//            if (result.Success == null || result.Success == undefined) {
//                $('#message-error').text('');
//                $('#message-error').append(result.Error.Message);
//                $('#message-box-danger').addClass('open');
//            }
//            else {
//                $('#message-success').text('');
//                $('#message-success').append(result.Success.Message);
//                $('#message-box-success').addClass('open');
//            }
//        });

//        $scope.ChangeRequest2 = {
//        };
//        $scope.ChangeRequestRecursosSolicitados2 = [];
//        $scope.GetChangeRequests();
//    }
//}

//$scope.NewChangeRequest = function () {
//    $('#CR').show();
//    $scope.ChangeRequest = {};
//    $scope.ChangeRequest.Id = "";
//    $scope.ChangeRequest.Codigo = "";
//    $scope.ChangeRequest.Versao = "";
//    $scope.ChangeRequest.Autor = "";
//    $scope.ChangeRequest.Situacao = "Aguardando";
//    $scope.ChangeRequest.GPI = "";
//    $scope.ChangeRequest.GPE = "";
//    $scope.ChangeRequest.Departamento = "";
//    $scope.ChangeRequest.Processo = "";
//    $scope.ChangeRequest.Descricao = "";
//    $scope.ChangeRequest.Motivos = "";
//    $scope.ChangeRequest.Recomendacoes = "";
//    $scope.ChangeRequest.ImpactosPositivos = "";
//    $scope.ChangeRequest.ImpactosNegativos = "";
//    $scope.ChangeRequestRecursosSolicitados = [];
//}

//$scope.AddChangeRequestResource = function () {
//    if ($scope.ChangeRequest == undefined || $scope.ChangeRequest.RecursoFase == null) {
//        noty({
//            text: 'Preencha o campo "Fase"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest == undefined || $scope.ChangeRequest.RecursoEspecialidade == null) {
//        noty({
//            text: 'Preencha o campo "Especialidade"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest == undefined || $scope.ChangeRequest.RecursoHorasSolicitadas == null || $scope.ChangeRequest.RecursoHorasSolicitadas == '') {
//        noty({
//            text: 'Preencha o campo "Horas Solicitadas"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest == undefined || $scope.ChangeRequest.RecursoSolicitante == null || $scope.ChangeRequest.RecursoSolicitante == '') {
//        noty({
//            text: 'Preencha o campo "Solicitante"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest == undefined || $scope.ChangeRequest.RecursoNecessidade == null || $scope.ChangeRequest.RecursoNecessidade == '') {
//        noty({
//            text: 'Preencha o campo "Necessidade"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }

//    var line = {
//    };

//    line.RecursoFase = $scope.ChangeRequest.RecursoFase;
//    for (var i = 0; i < $scope.Projeto.Fases.length; i++) {
//        if (line.RecursoFase == $scope.Projeto.Fases[i].StepId) {
//            line.RecursoFaseNome = $scope.Projeto.Fases[i].Nome;
//            break;
//        }
//    }

//    line.RecursoEspecialidade = $scope.ChangeRequest.RecursoEspecialidade;
//    for (var i = 0; i < $scope.Especialidades2.length; i++) {
//        if (line.RecursoEspecialidade == $scope.Especialidades2[i].Id) {
//            line.RecursoEspecialidadeNome = $scope.Especialidades2[i].Name;
//            break;
//        }
//    }

//    line.RecursoHorasSolicitadas = $scope.ChangeRequest.RecursoHorasSolicitadas;
//    line.RecursoSolicitante = $scope.ChangeRequest.RecursoSolicitante;
//    line.RecursoNecessidade = $scope.ChangeRequest.RecursoNecessidade;


//    if ($scope.ChangeRequestResourceLine == -1) {
//        var index = $scope.ChangeRequestRecursosSolicitados.length;
//        $scope.ChangeRequestRecursosSolicitados[index] = line;
//    }
//    else {
//        var index = $scope.ChangeRequestResourceLine;
//        $scope.ChangeRequestRecursosSolicitados[index] = line;
//    }

//    line = null;
//    $scope.ChangeRequest.RecursoFase = "";
//    $scope.ChangeRequest.RecursoEspecialidade = "";
//    $scope.ChangeRequest.RecursoHorasSolicitadas = "";
//    $scope.ChangeRequest.RecursoSolicitante = "";
//    $scope.ChangeRequest.RecursoNecessidade = "";
//    $scope.ChangeRequestResourceLine = -1;
//}

//$scope.EditChangeRequestResource = function (index) {
//    $scope.ChangeRequestResourceLine = index;

//    $scope.ChangeRequest.RecursoFase = $scope.ChangeRequestRecursosSolicitados[index].RecursoFase;
//    $scope.ChangeRequest.RecursoEspecialidade = $scope.ChangeRequestRecursosSolicitados[index].RecursoEspecialidade;
//    $scope.ChangeRequest.RecursoHorasSolicitadas = $scope.ChangeRequestRecursosSolicitados[index].RecursoHorasSolicitadas;
//    $scope.ChangeRequest.RecursoSolicitante = $scope.ChangeRequestRecursosSolicitados[index].RecursoSolicitante;
//    $scope.ChangeRequest.RecursoNecessidade = $scope.ChangeRequestRecursosSolicitados[index].RecursoNecessidade;
//}

//$scope.RemoveChangeRequestResource = function (index) {
//    $scope.ChangeRequestRecursosSolicitados.splice(index, 1);
//}

//$scope.ShowChangeRequest = function () {
//    $('#CR').show();
//    $http({
//        method: 'GET',
//        url: '/ChangeRequest/Get?id=' + $scope.ChangeRequest.Id
//    }).success(function (result) {

//        $scope.ChangeRequest = {
//        };
//        $scope.ChangeRequest.Id = result.Id;
//        $scope.ChangeRequest.Codigo = result.Codigo;
//        $scope.ChangeRequest.Versao = result.Versao;
//        $scope.ChangeRequest.Autor = result.Autor;
//        $scope.ChangeRequest.Situacao = result.Situacao;
//        $scope.ChangeRequest.GPI = result.GPI;
//        $scope.ChangeRequest.GPE = result.GPE;
//        $scope.ChangeRequest.Departamento = result.Departamento;
//        $scope.ChangeRequest.Processo = result.Processo;
//        $scope.ChangeRequest.Descricao = result.Descricao;
//        $scope.ChangeRequest.Motivos = result.Motivos;
//        $scope.ChangeRequest.Recomendacoes = result.Recomendacoes;
//        $scope.ChangeRequest.ImpactosPositivos = result.ImpactosPositivos;
//        $scope.ChangeRequest.ImpactosNegativos = result.ImpactosNegativos;
//        $scope.ChangeRequestRecursosSolicitados = result.RecursosSolicitados;
//    });
//}

//$scope.ShowChangeRequest2 = function () {
//    $('#CR2').show();
//    $http({
//        method: 'GET',
//        url: '/ChangeRequest/Get?id=' + $scope.ChangeRequest2.Id
//    }).success(function (result) {

//        $scope.ChangeRequest2 = {
//        };
//        $scope.ChangeRequest2.Id = result.Id;
//        $scope.ChangeRequest2.Codigo = result.Codigo;
//        $scope.ChangeRequest2.Versao = result.Versao;
//        $scope.ChangeRequest2.Autor = result.Autor;
//        $scope.ChangeRequest2.Situacao = result.Situacao;
//        $scope.ChangeRequest2.GPI = result.GPI;
//        $scope.ChangeRequest2.GPE = result.GPE;
//        $scope.ChangeRequest2.Departamento = result.Departamento;
//        $scope.ChangeRequest2.Processo = result.Processo;
//        $scope.ChangeRequest2.Descricao = result.Descricao;
//        $scope.ChangeRequest2.Motivos = result.Motivos;
//        $scope.ChangeRequest2.Recomendacoes = result.Recomendacoes;
//        $scope.ChangeRequest2.ImpactosPositivos = result.ImpactosPositivos;
//        $scope.ChangeRequest2.ImpactosNegativos = result.ImpactosNegativos;
//        $scope.ChangeRequestRecursosSolicitados2 = result.RecursosSolicitados;
//    });
//}

//function ValidateChangeRequest() {
//    if ($scope.ChangeRequest.Codigo == null || $scope.ChangeRequest.Codigo == '') {
//        noty({
//            text: 'Preencha o campo "Código"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest.Versao == null || $scope.ChangeRequest.Versao == '') {
//        noty({
//            text: 'Preencha o campo "Versão"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest.Autor == null || $scope.ChangeRequest.Autor == '') {
//        noty({
//            text: 'Preencha o campo "Autor"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest.Situacao == null || $scope.ChangeRequest.Situacao == '') {
//        noty({
//            text: 'Preencha o campo "Status"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest.GPI == null || $scope.ChangeRequest.GPI == '') {
//        noty({
//            text: 'Preencha o campo "Gerente de Projeto (CVA)"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest.GPE == null || $scope.ChangeRequest.GPE == '') {
//        noty({
//            text: 'Preencha o campo "Gerente de Projeto (Cliente)"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest.Departamento == null || $scope.ChangeRequest.Departamento == '') {
//        noty({
//            text: 'Preencha o campo "Departamento"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest.Processo == null || $scope.ChangeRequest.Processo == '') {
//        noty({
//            text: 'Preencha o campo "Processo"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest.Descricao == null || $scope.ChangeRequest.Descricao == '') {
//        noty({
//            text: 'Preencha o campo "Descrição"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest.Motivos == null || $scope.ChangeRequest.Motivos == '') {
//        noty({
//            text: 'Preencha o campo "Motivos"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest.Recomendacoes == null || $scope.ChangeRequest.Recomendacoes == '') {
//        noty({
//            text: 'Preencha o campo "Recomendações"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest.ImpactosPositivos == null || $scope.ChangeRequest.ImpactosPositivos == '') {
//        noty({
//            text: 'Preencha o campo "Impactos Positivos"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequest.ImpactosNegativos == null || $scope.ChangeRequest.ImpactosNegativos == '') {
//        noty({
//            text: 'Preencha o campo "Impactos caso não aprovada"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }
//    if ($scope.ChangeRequestRecursosSolicitados == null || $scope.ChangeRequestRecursosSolicitados.length <= 0) {
//        noty({
//            text: 'Insira pelo menos um "Recurso Solicitado"', layout: 'topRight', type: 'error'
//        });
//        return false;
//    }

//    return true;
//}

//function LoadChangeRequest(type) {
//    var changeRequest = {
//    };
//    if (type == 1) {
//        changeRequest.Id = $scope.ChangeRequest.Id;
//        changeRequest.Projeto = {
//        };
//        changeRequest.Projeto.Id = $scope.Projeto.Id;
//        changeRequest.Codigo = $scope.ChangeRequest.Codigo;
//        changeRequest.Versao = $scope.ChangeRequest.Versao;
//        changeRequest.Autor = $scope.ChangeRequest.Autor;
//        changeRequest.Situacao = "Aguardando";
//        changeRequest.GPI = $scope.ChangeRequest.GPI;
//        changeRequest.GPE = $scope.ChangeRequest.GPE;
//        changeRequest.Departamento = $scope.ChangeRequest.Departamento;
//        changeRequest.Processo = $scope.ChangeRequest.Processo;
//        changeRequest.Descricao = $scope.ChangeRequest.Descricao;
//        changeRequest.Motivos = $scope.ChangeRequest.Motivos;
//        changeRequest.Recomendacoes = $scope.ChangeRequest.Recomendacoes;
//        changeRequest.ImpactosPositivos = $scope.ChangeRequest.ImpactosPositivos;
//        changeRequest.ImpactosNegativos = $scope.ChangeRequest.ImpactosNegativos;
//        changeRequest.RecursosSolicitados = $scope.ChangeRequestRecursosSolicitados;
//    }

//    if (type == 2) {
//        changeRequest.Id = $scope.ChangeRequest2.Id;
//        changeRequest.Projeto = {
//        };
//        changeRequest.Projeto.Id = $scope.Projeto.Id;
//        changeRequest.Codigo = $scope.ChangeRequest2.Codigo;
//        changeRequest.Versao = $scope.ChangeRequest2.Versao;
//        changeRequest.Autor = $scope.ChangeRequest2.Autor;
//        changeRequest.Situacao = $scope.ChangeRequest2.Situacao;
//        changeRequest.GPI = $scope.ChangeRequest2.GPI;
//        changeRequest.GPE = $scope.ChangeRequest2.GPE;
//        changeRequest.Departamento = $scope.ChangeRequest2.Departamento;
//        changeRequest.Processo = $scope.ChangeRequest2.Processo;
//        changeRequest.Descricao = $scope.ChangeRequest2.Descricao;
//        changeRequest.Motivos = $scope.ChangeRequest2.Motivos;
//        changeRequest.Recomendacoes = $scope.ChangeRequest2.Recomendacoes;
//        changeRequest.ImpactosPositivos = $scope.ChangeRequest2.ImpactosPositivos;
//        changeRequest.ImpactosNegativos = $scope.ChangeRequest2.ImpactosNegativos;
//        changeRequest.RecursosSolicitados = $scope.ChangeRequestRecursosSolicitados2;
//    }

//    return changeRequest;
//}

