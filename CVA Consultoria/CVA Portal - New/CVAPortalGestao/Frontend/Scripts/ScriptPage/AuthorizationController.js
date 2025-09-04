cvaGestao.controller('AuthorizationController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {
    $scope.OnLoad = function (model) {
        CreateModel();
        CreateArrays();
        LoadArrays(); oad - 00    }

    function CreateModel() {
        $scope.Dia = {};
        $scope.Despesa = {};
        $scope.B = {};
        $scope.B.Data = new Date();
        $scope.C = {};
        $scope.C.Data = new Date();
    }

    function CreateArrays() {
        $scope.DiasAutorizados = [];
        $scope.DespesasAutorizados = [];
        $scope.HorasAutorizadas = [];
        $scope.LimiteHoras = [];

        $scope.CollaboratorList = [];
    }

    function LoadArrays() {
        $http({
            method: 'GET',
            url: '/Colaborador/LoadCombo'
        }).success(function (result) {
            $scope.CollaboratorList = result;
            document.getElementById('load-00').hidden = true;
            document.getElementById('load-01').hidden = true;
            document.getElementById('load-02').hidden = true;
            document.getElementById('load-04').hidden = true;
        });
    }



    /*--- Autorização - Dias ---*/
    $scope.FiltrarDiasAutorizados = function () {
        var idCol = $scope.Dia.Colaborador;

        if (idCol == undefined || idCol == null)
            idCol = 0;

        $http({
            method: 'GET',
            url: '/Autorizacao/Get_DiasAutorizados?idCol=' + idCol
        }).success(function (result) {
            $scope.DiasAutorizados = result;
        });
    }

    $scope.AddDiaAutorizado = function () {
        if (ValidaDiaAutorizado()) {
            $http({
                method: 'POST',
                url: '/Autorizacao/AddDiaAutorizado',
                data: $scope.Dia
            }).then(function successCallback(response) {
                $('#message-box-success').addClass('open');
            }, function errorCallback(response) {
                $('#message-error').text('');
                $('#message-error').append(response.statusText);
                $('#message-box-danger').addClass('open');
            });

            Wait();
            $scope.FiltrarDiasAutorizados();
        }
    }

    $scope.RemoveDiaAutorizado = function (id) {
        $http({
            method: 'GET',
            url: '/Autorizacao/RemoveDiaAutorizado?id=' + id
        }).success(function (message) {
            if (message.Success == null || message.Success == undefined) {
                $('#message-error').text('');
                $('#message-error').append(message.Error.Message);
                $('#message-box-danger').addClass('open');
            }
            else {
                $('#message-box-success').addClass('open');
            }
            Wait();
            $scope.FiltrarDiasAutorizados();
        });
    }

    function ValidaDiaAutorizado() {
        if ($scope.Dia.Colaborador == undefined || $scope.Dia.Colaborador == '') {
            noty({ text: 'Informe o colaborador', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Dia.De == undefined || $scope.Dia.De == null || $scope.Dia.De == '') {
            noty({ text: 'Informe a data inicial', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Dia.Ate == undefined || $scope.Dia.Ate == null || $scope.Dia.Ate == '') {
            noty({ text: 'Informe a data final', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }

    /*--- Autorização - Despesas ---*/
    $scope.FiltrarDespesasAutorizados = function () {
        var idCol = $scope.Despesa.Colaborador;

        if (idCol == undefined || idCol == null)
            idCol = 0;

        $http({
            method: 'GET',
            url: '/Autorizacao/Get_DespesasAutorizados?idCol=' + idCol
        }).success(function (result) {
            $scope.DespesasAutorizados = result;
        });
    }

    $scope.AddDespesaAutorizado = function () {
        if (ValidaDespesaAutorizado()) {
            $http.post('/Autorizacao/AddDespesaAutorizado', $scope.Despesa).success(function (message) {
                if (message.Success == null || message.Success == undefined) {
                    $('#message-error').text('');
                    $('#message-error').append(message.Error.Message);
                    $('#message-box-danger').addClass('open');
                }
                else {
                    $('#message-box-success').addClass('open');
                }
                Wait();
                $scope.FiltrarDespesasAutorizados();
            });
        }
    }

    $scope.RemoveDespesaAutorizado = function (id) {
        $http({
            method: 'GET',
            url: '/Autorizacao/RemoveDespesaAutorizado?id=' + id
        }).success(function (message) {
            if (message.Success == null || message.Success == undefined) {
                $('#message-error').text('');
                $('#message-error').append(message.Error.Message);
                $('#message-box-danger').addClass('open');
            }
            else {
                $('#message-box-success').addClass('open');
            }
            Wait();
            $scope.FiltrarDespesasAutorizados();
        });
    }

    function ValidaDespesaAutorizado() {
        if ($scope.Despesa.Colaborador == undefined || $scope.Despesa.Colaborador == '') {
            noty({ text: 'Informe o colaborador', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Despesa.De == undefined || $scope.Despesa.De == null || $scope.Despesa.De == '') {
            noty({ text: 'Informe a data inicial', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Despesa.Ate == undefined || $scope.Despesa.Ate == null || $scope.Despesa.Ate == '') {
            noty({ text: 'Informe a data final', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }

    /*--- Autorização - Horas ---*/
    $scope.FiltrarHorasAutorizadas = function () {
        var idCol = $scope.B.Colaborador;

        if (idCol == undefined || idCol == null)
            idCol = 0;

        $http({
            method: 'GET',
            url: '/Autorizacao/Get_HorasAutorizadas?idCol=' + idCol
        }).success(function (result) {
            $scope.HorasAutorizadas = result;
        });
    }

    $scope.AddHorasAutorizadas = function () {
        if (ValidaHorasAutorizadas()) {
            $scope.B.Horas = $("#Horas").val();
            $http.post('/Autorizacao/AddHorasAutorizadas', $scope.B).success(function (message) {
                if (message.Success == null || message.Success == undefined) {
                    $('#message-error').text('');
                    $('#message-error').append(message.Error.Message);
                    $('#message-box-danger').addClass('open');
                }
                else {
                    $('#message-box-success').addClass('open');
                }
            });
            Wait();
            $scope.FiltrarHorasAutorizadas();
        }
    }

    $scope.RemoveHorasAutorizadas = function (id) {
        $http({
            method: 'GET',
            url: '/Autorizacao/RemoveHorasAutorizadas?id=' + id
        }).success(function (message) {
            if (message.Success == null || message.Success == undefined) {
                $('#message-error').text('');
                $('#message-error').append(message.Error.Message);
                $('#message-box-danger').addClass('open');
            }
            else {
                $('#message-box-success').addClass('open');
            }
        });
        Wait();
        $scope.FiltrarHorasAutorizadas();
    }

    function ValidaHorasAutorizadas() {
        if ($scope.B.Colaborador == undefined || $scope.B.Colaborador == null) {
            noty({ text: 'Informe o colaborador', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.B.Horas == undefined || $scope.B.Horas == null || $scope.B.Horas == '') {
            noty({ text: 'Informe uma quantia de horas válida', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.B.Data == undefined || $scope.B.Data == null || $scope.B.Data == '') {
            noty({ text: 'Informe a data final', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }

    /*--- Configuração de Horas ---*/
    $scope.FiltrarLimiteHoras = function () {
        var idCol = $scope.C.Colaborador;

        if (idCol == undefined || idCol == null)
            idCol = 0;

        $http({
            method: 'GET',
            url: '/Autorizacao/Get_LimiteHoras?idCol=' + idCol
        }).success(function (result) {
            $scope.LimiteHoras = result;
        });
    }

    $scope.AddLimiteHoras = function () {
        if (ValidaLimiteHoras) {
            $scope.C.Horas = $("#Limite").val();
            $http.post('/Autorizacao/AddLimiteHoras', $scope.C).success(function (message) {
                if (message.Success == null || message.Success == undefined) {
                    $('#message-error').text('');
                    $('#message-error').append(message.Error.Message);
                    $('#message-box-danger').addClass('open');
                }
                else {
                    $('#message-box-success').addClass('open');
                }
                Wait();
                $scope.FiltrarLimiteHoras();
            });
        }
    }

    $scope.RemoveLimiteHoras = function (id) {
        $http({
            method: 'GET',
            url: '/Autorizacao/RemoveLimiteHoras?id=' + id
        }).success(function (result) {
            if (result.Error == null) {
                $('#message-box-success').addClass('open');
            }
            else {
                $('#message-error').text('');
                $('#message-error').append(message.Error.Message);
                $('#message-box-danger').addClass('open');
            }
            Wait();
            $scope.FiltrarLimiteHoras();
        });
    }

    function ValidaLimiteHoras() {
        if ($scope.C.Colaborador == undefined || $scope.C.Colaborador == null) {
            noty({ text: 'Informe o colaborador', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.C.Horas == undefined || $scope.C.Horas == null || $scope.C.Horas == '') {
            noty({ text: 'Informe uma quantia de horas válida', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }

    function Wait() {
        var counter = 0
            , start = new Date().getTime()
            , end = 0;
        while (counter < 1500) {
            end = new Date().getTime();
            counter = end - start;
        }
    }
}]);