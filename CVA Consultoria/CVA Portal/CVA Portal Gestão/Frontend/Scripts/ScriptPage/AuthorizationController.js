cvaGestao.controller('AuthorizationController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {
    $scope.OnLoad = function (model) {
        CreateModel();
        CreateArrays();
        LoadArrays();
    }

    function CreateModel() {
        $scope.A = {};
        $scope.A.Data = new Date();
        $scope.B = {};
        $scope.B.Data = new Date();
        $scope.C = {};
        $scope.C.Data = new Date();
    }

    function CreateArrays() {
        $scope.DiasAutorizados = [];
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
        });
    }



    /*--- Autorização - Dias ---*/
    $scope.FiltrarDiasAutorizados = function () {
        var idCol = $scope.A.Colaborador;

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
            $http.post('/Autorizacao/AddDiaAutorizado', $scope.A).success(function (message) {
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
        if ($scope.A.Colaborador == undefined || $scope.A.Colaborador == '') {
            noty({ text: 'Informe o colaborador', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.A.De == undefined || $scope.A.De == null || $scope.A.De == '') {
            noty({ text: 'Informe a data inicial', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.A.Ate == undefined || $scope.A.Ate == null || $scope.A.Ate == '') {
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