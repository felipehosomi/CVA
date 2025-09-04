cvaGestao.controller('NotePeriodController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {

    $scope.OnLoad = function () {
        CreateArrays();
        LoadArrays();

        $scope.Periodo = {};

        $('#inputYear').val(new Date().getFullYear());
    }

    $scope.OnLoadNew = function () {
        $scope.Periodo = {};
        $scope.Periodo.Year = new Date().getFullYear();
        $scope.Periodo.Month = (new Date().getMonth() + 1).toString();
    }

    $scope.Novo = function () {
        window.location.href = '/Periodo/Cadastrar';
    }

    $scope.Salvar = function () {
        if ($scope.Periodo.Year == undefined || $scope.Periodo.Year == null || $scope.Periodo.Year == '') {
            noty({ text: 'Obrigatório informar o ano', layout: 'topRight', type: 'error' });
            return false;
        }

        if ($scope.Periodo.Month == undefined || $scope.Periodo.Month == null || $scope.Periodo.Month == '') {
            noty({ text: 'Obrigatório informar o mês', layout: 'topRight', type: 'error' });
            return false;
        }

        $http({
            method: 'POST',
            url: '/Periodo/Incluir',
            data: $scope.Periodo
        }).then(function successCallback(message) {
            $('#message-box-success').addClass('open');
            window.location.href = '/Periodo/Periodo';
        }, function errorCallback(response) {
            $('#message-error').text('');
            $('#message-error').append(response.statusText);
            $('#message-box-danger').addClass('open');
        });
    }

    $scope.Excluir = function (period) {
        $http({
            method: 'POST',
            url: '/Periodo/Excluir',
            data: period
        }).then(function successCallback(message) {
            $('#message-box-success').addClass('open');
            window.location.href = '/Periodo/Periodo';
        }, function errorCallback(response) {
            $('#message-error').text('');
            $('#message-error').append(response.statusText);
            $('#message-box-danger').addClass('open');
        });
    }

    function CreateArrays() {
        $scope.NotePeriods = [];
    }

    function LoadArrays() {
        $http({
            method: 'GET',
            url: '/Periodo/GetAllPeriods'
        }).success(function (result) {
            $scope.NotePeriods = result;
        });
    }
}]);