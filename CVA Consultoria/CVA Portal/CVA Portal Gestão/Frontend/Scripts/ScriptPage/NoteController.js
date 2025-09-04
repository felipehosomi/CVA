cvaGestao.controller('NoteController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {
    $scope.OnLoad = function () {
        CreateModel();
        CreateArrays();
        LoadArrays();
    }

    function CreateModel() {
        $scope.note = {};

        $scope.note.Id = 0;
        $scope.note.Date = new Date();
        $scope.note.Project = {};
        $scope.note.Project.TipoProjeto = {};
        $scope.note.InitHour = '08:30';
        $scope.note.IntervalHour = '01:00';
        $scope.note.FinishHour = '17:30';
        $scope.note.Specialty = {};


        var model = {};
        if ($scope.note != null) {
            model.IndicatedHours = $scope.note.IndicatedHours;
            model.ProvidedHours = $scope.note.ProvidedHours;
            model.StatusPDA = $scope.note.StatusPDA;
        }



        $scope.note.IndicatedHours = model.IndicatedHours;
        $scope.note.ProvidedHours = model.ProvidedHours;
        $scope.note.StatusPDA = model.StatusPDA;
    }


    function CreateArrays() {
        $scope.ProjectList = [];
        $scope.StepList = [];
        $scope.AMSList = [];
        $scope.SpecialtyList = [];
        $scope.NotesList = [];
        $scope.UsaAMS = 0;
    }

    function LoadArrays() {
        $http({
            method: 'GET',
            url: '/Apontamento/GetProjects'
        }).success(function (result) {
            $scope.ProjectList = result;
        });

        $http({
            method: 'GET',
            url: '/Colaborador/Get_Specialties'
        }).success(function (result) {
            $scope.SpecialtyList = result;
        });
    }



    //Carrega o combo 'N° Chamado'
    $scope.LoadTickets = function () {
        $http({
            method: 'GET',
            url: '/Apontamento/Get_ProjectSteps?projectId=' + $scope.note.Project.Id,
        }).success(function (result) {
            $scope.StepList = result;
        });

        for (var i = 0; i < $scope.ProjectList.length; i++) {
            if ($scope.note.Project.Id == $scope.ProjectList[i].Id) {
                $scope.UsaAMS = $scope.ProjectList[i].TipoProjeto.AMS;
                break;
            }
        }


        // Carrega chamados de acordo com o projeto selecionado
        $http({
            method: 'GET',
            url: '/Apontamento/GetTicketByProject?projectId=' + $scope.note.Project.Id,
        }).success(function (result) {
            $scope.AMSList = result;
        });


        //if ($scope.UsaAMS == 1) {
        //    // Carrega chamados de acordo com o projeto selecionado
        //    $http({
        //        method: 'GET',
        //        url: '/Apontamento/GetTicketByProject?projectId=' + $scope.note.Project.Id,
        //    }).success(function (result) {
        //        $scope.AMSList = result;
        //    });
        //}
        //else  {
        //    $scope.AMSList = [];
        //}
    }



    //---------  Métodos de CRUD de apontamentos -----------
    //Salvar apontamento
    $scope.SaveNote = function () {
        if (ValidateFields()) {
            $scope.note.InitHour = $("#init-hour").val();
            $scope.note.IntervalHour = $("#interval-hour").val();
            $scope.note.FinishHour = $("#finish-hour").val();
            $http.post('/Apontamento/Salvar', $scope.note).success(function (message) {
                if (message.Success == null || message.Success == undefined) {
                    $('#message-error').text('');
                    $('#message-error').append(message.Error.Message);
                    $('#message-box-danger').addClass('open');
                }
                else {
                    $('#message-box-success').addClass('open');
                    Wait();
                    $scope.AtualizarGrid();
                    $scope.AMSList = [];
                    $scope.note = {};
                }
            });
        }
    }

    //Atualiza lista de apontamentos
    $scope.AtualizarGrid = function () {
        $http({
            method: 'GET',
            url: '/Apontamento/GetApontamentos'
        }).success(function (apontamentos) {
            $scope.NotesList = apontamentos;
            $scope.note.ProvidedHours = apontamentos[0].ProvidedHours;
            $scope.note.IndicatedHours = apontamentos[0].IndicatedHours;
            $scope.note.StatusPDA = apontamentos[0].StatusPDA;
            
        });
    }

    //Editar apontamento
    $scope.EditNote = function (note) {
        var resultado = angular.copy(note)
        resultado.Date = new Date(resultado.Date);
        //São adicionadas 2 horas pois por algum motivo o javascript reduz 2 horas, alterando a data de apontamentos feitos entre 00:00 e 02:00
        resultado.Date.setHours(resultado.Date.getHours() + 2);
        if (resultado.InitHour != null)
            resultado.InitHour = resultado.InitHour.split('T')[1];
        if (resultado.IntervalHour != null)
            resultado.IntervalHour = resultado.IntervalHour.split('T')[1];
        if (resultado.FinishHour != null)
            resultado.FinishHour = resultado.FinishHour.split('T')[1];
        $scope.note = resultado;

        $('#tab-first').addClass('active');
        $('#tab-first-li').addClass('active');
        $('#tab-three-li').removeClass('active');
        $('#tab-tree').removeClass('active');
        $('#tab-firt').attr('aria-expended', 'true');
        $('#tab-tree').attr('aria-expended', 'false');

        $http({
            method: 'GET',
            url: '/Apontamento/Get_ProjectSteps?projectId=' + $scope.note.Project.Id,
        }).success(function (result) {
            $scope.StepList = result;

            $scope.note.Step = resultado.Step;
            $scope.note.Step.Id = resultado.Step.Id;
        });


        for (var i = 0; i < $scope.ProjectList.length; i++) {
            if ($scope.note.Project.Id == $scope.ProjectList[i].Id) {
                $scope.UsaAMS = $scope.ProjectList[i].TipoProjeto.AMS;
                break;
            }
        }

        if ($scope.UsaAMS == 1) {
            // Carrega chamados de acordo com o projeto selecionado
            $http({
                method: 'GET',
                url: '/Apontamento/GetTicketByProject?projectId=' + $scope.note.Project.Id,
            }).success(function (result) {
                $scope.AMSList = result;
                console.log(resultado);
                console.log(resultado.Ticket.Id);
                for (var i = 0; i < result.length; i++) {
                    if (result[i].Id == resultado.Ticket.Id) {

                        $scope.note.Ticket.Id = result[i].Id;
                        $scope.note.Ticket.Code = resultado.Ticket.Id;
                        break;
                    }
                }
            });
        }
        else {
            $scope.AMSList = [];
        }

    }
    //Remover apontamento
    $scope.RemoveNote = function (id) {
        $http({
            method: 'GET',
            url: '/Apontamento/Remove?id=' + id
        }).success(function (message) {
            if (message.Success == null || message.Success == undefined) {
                $('#message-error').text('');
                $('#message-error').append(message.Error.Message);
                $('#message-box-danger').addClass('open');
            }
            else {
                noty({ text: 'Apontamento removido', layout: 'topRight', type: 'success' });

                Wait();
                $scope.AtualizarGrid();
            }
        });
    }
    //Carregar tabela de Apontamentos para consulta
    $http({
        method: 'GET',
        url: '/Apontamento/GetApontamentos'
    }).success(function (apontamentos) {
        $scope.NotesList = apontamentos;
        $scope.note.ProvidedHours = apontamentos[0].ProvidedHours;
        $scope.note.IndicatedHours = apontamentos[0].IndicatedHours;
        $scope.note.StatusPDA = apontamentos[0].StatusPDA;


        

        //var planejado = 8;

        //var dateArray = [];
        //var date = new Date();
        //var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        //var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        //var totalLine = 0;

        //var totalHoras = [];
        //for (var i = 0; i < apontamento.UserNotes.length; i++) {
        //    totalHoras[i] = {
        //        horas: ConvertToSeconds(apontamento.UserNotes[i].TotalLine + ':00'),
        //        data: apontamento.UserNotes[i].Date
        //    };
        //}

        //for (var i = 1; i <= lastDay.getDate(); i++) {
        //    for (var a = 0; a < apontamento.UserNotes.length; a++) {
        //        var dataSplit = firstDay.toLocaleDateString().split('/');
        //        if (i == (new Date(Date.parse(apontamento.UserNotes[a].Date)).getDate())) {
        //            totalLine = totalLine + ConvertToSeconds(apontamento.UserNotes[a].TotalLine + ':00');
        //            dateArray[dateArray.length] = {
        //                y: dataSplit[2] + '-' + dataSplit[1] + '-' + dataSplit[0],
        //                a: ConvertToHours(totalLine),
        //                b: planejado
        //            };
        //        } else {
        //            dateArray[dateArray.length] = {
        //                y: dataSplit[2] + '-' + dataSplit[1] + '-' + dataSplit[0],
        //                a: ConvertToHours(totalLine),
        //                b: planejado
        //            };
        //        }
        //    }
        //    firstDay.setDate(firstDay.getDate() + 1);
        //    planejado = (168 / lastDay.getDate()) + planejado;
        //}

    });

    function Wait() {
        var counter = 0
            , start = new Date().getTime()
            , end = 0;
        while (counter < 1500) {
            end = new Date().getTime();
            counter = end - start;
        }
    }

    //Realiza a validação do apontamento a ser salvo
    function ValidateFields() {
        var today = new Date();
        if ($scope.note.Date == null || $scope.note.Date == '') {
            noty({ text: 'Informe a data do apontamento', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.note.Project.Id == '' || $scope.note.Project.Id == null || $scope.note.Project == null) {
            noty({ text: 'Informe o projeto', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.note.Step == undefined || $scope.note.Step.Id == null || $scope.note.Step == null) {
            noty({ text: 'Informe a fase', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.note.InitHour == '' || $scope.note.InitHour == null) {
            noty({ text: 'Informe a hora inicial', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.note.FinishHour == '' || $scope.note.FinishHour == null) {
            noty({ text: 'Informe a hora final', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.note.Requester == null) {
            noty({ text: 'Informe o solicitante', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.note.Specialty.Id == '' || $scope.note.Specialty.Id == null || $scope.note.Specialty == null) {
            noty({ text: 'Informe a especialidade', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.note.Description == '' || $scope.note.Description == null) {
            noty({ text: 'Informe a descrição', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.UsaAMS == 1 && (($scope.note.Ticket == undefined) || ($scope.note.Ticket.Code == '' || $scope.note.Ticket.Code == null))) {
            noty({ text: 'Informe o n° do chamado', layout: 'topRight', type: 'error' });
            return false;
        }

        var Entrada = $scope.note.InitHour.split(':');
        var horaEntrada = parseInt(Entrada[0]);
        var minutoEntrada = parseInt(Entrada[1]);

        var Saida = $scope.note.FinishHour.split(':');
        var horaSaida = parseInt(Saida[0]);
        var minutoSaida = parseInt(Saida[1]);

        if ($scope.note.IntervalHour != null) {
            var Intervalo = $scope.note.IntervalHour.split(':');
            var horaIntervalo = parseInt(Intervalo[0]);
            var minutoIntervalo = parseInt(Intervalo[1]);
            if ((horaEntrada >= horaSaida) && (minutoEntrada >= minutoSaida)) {
                noty({ text: 'Hora de saída inferior a hora de entrada', layout: 'topRight', type: 'error' });
                return false;
            }
            if (((horaEntrada * 60) + minutoEntrada + (horaIntervalo * 60) + minutoIntervalo) >= (horaSaida * 60 + minutoSaida)) {
                noty({ text: 'Tempo de intervalo inválido', layout: 'topRight', type: 'error' });
                return false;
            }
        }
        return true;
    }


    //--------- Funções auxiliares -------------------------
    function StringContains(string, it) {
        return string.indexOf(it) != -1;
    }
    function ConvertToHours(TotalSeconds) {
        var hours = parseInt(TotalSeconds / 3600) % 24;
        var minutes = parseInt(TotalSeconds / 60) % 60;
        var seconds = TotalSeconds % 60;

        var result = (hours < 10 ? "0" + hours : hours) + ":" + (minutes < 10 ? "0" + minutes : minutes) + ":" + (seconds < 10 ? "0" + seconds : seconds);
        return result;
    }
    function ConvertToSeconds(hour) {
        if (hour.length == 8) {
            var a = hour.split(':');
            var seconds = (+a[0]) * 60 * 60 + (+a[1]) * 60 + (+a[2]);
            return seconds;
        } else {
            hour = hour + ':00';

            var a = hour.split(':');
            var seconds = (+a[0]) * 60 * 60 + (+a[1]) * 60 + (+a[2]);
            return seconds;
        }
        return 0;
    }
}]);