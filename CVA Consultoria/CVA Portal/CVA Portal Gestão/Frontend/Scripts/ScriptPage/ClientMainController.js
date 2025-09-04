cvaGestao.controller('ClientMainController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {
    $scope.OnLoad = function () {
        CreateModel();
        CreateArrays();
        LoadArrays();
    }

    function CreateModel() {
        $scope.client = {};
    }

    function CreateArrays() {
        $scope.ClientList = [];
        $scope.StatusList = [];
    }

    function LoadArrays() {
        $http({
            method: 'GET',
            url: '/Cliente/GetSpecificStatus'
        }).success(function (result) {
            $scope.StatusList = result;
        });
    }

    $scope.Search = function () {
        ValidateFilters();
        $http({
            method: 'GET',
            url: '/Cliente/Search?name=' + $scope.client.Name
        }).success(function (result) {
            if (result.length == 0) {
                $scope.ClientList = [];
            } else {
                $scope.ClientList = result;
            }
        });
    }

    function ValidateFilters() {
        if ($scope.client.Name == null || $scope.client.Name == undefined)
            $scope.client.Name = '';
    }

    //Redireciona para a página de edição do cliente
    $scope.EditClient = function (id) {
        window.location.href = '/Cliente/Editar?clienteId=' + id;
    }

    //Faz a exportação 
    $scope.ExportarExcel = function () {
        ValidateFilters();
        window.location.href = '/Cliente/RelatorioClientes?clientCode=' + $scope.client.Code + '&name=' + $scope.client.Name + '&cnpj=' + $scope.client.CNPJ + '&status=' + $scope.client.Status + '&branch=' + 0 + '&salesChannel=' + $scope.client.SalesChannel + '&reportType=' + 1
    }
    $scope.ExportarPDF = function () {
        ValidateFilters();
        window.location.href = '/Cliente/RelatorioClientes?clientCode=' + $scope.client.Code + '&name=' + $scope.client.Name + '&cnpj=' + $scope.client.CNPJ + '&status=' + $scope.client.Status + '&branch=' + 0 + '&salesChannel=' + $scope.client.SalesChannel + '&reportType=' + 2
    }
}]);