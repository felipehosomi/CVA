cvaGestao.controller('OportunittyMainController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {
    //Métodos de iniciação da tela
    CreateModel();
    CreateArrays();

    function CreateModel() {
        $scope.Filtro = {};
    }
    function CreateArrays() {
        $scope.ClientList = [];

        //Carrega o campo Clientes
        $http({
            method: 'GET',
            url: '/Cliente/LoadCombo'
        }).success(function (clients) {
            $scope.ClientList = clients;
            document.getElementById('load-00').hidden = true;
        });
    }

    
    $scope.Search = function () {
        if ($scope.Filtro == undefined || $scope.Filtro.Codigo == null)
            $scope.Filtro.Codigo = '';

        if ($scope.Filtro == undefined || $scope.Filtro.Cliente == null)
            $scope.Filtro.Cliente = 0;


        $http({
            method: 'GET',
            url: '/Oportunidade/Search?Codigo=' + $scope.Filtro.Codigo + '&IdCliente=' + $scope.Filtro.Cliente
        }).success(function (result) {
            if (result.length == 0) {
                $scope.TableList = null;
            } else {
                $scope.TableList = result;
            }
        });
    }

    //Redireciona para a página de edição do colaborador
    $scope.EditOportunitty = function (id) {
        window.location.href = '/Oportunidade/Editar?Id=' + id;
    }

    //Faz a exportação para Excel
    $scope.ExportarExcel = function () {
        window.location.href = '/Oportunidade/GerarRelatorio?Codigo=' + $scope.Filtro.Codigo + '&IdCliente=' + $scope.Filtro.Cliente + '&Tipo=' + 1
    }

    //Faz a exportação para PDF
    $scope.ExportarPDF = function () {
        window.location.href = '/Oportunidade/GerarRelatorio?Codigo=' + $scope.Filtro.Codigo + '&IdCliente=' + $scope.Filtro.Cliente + '&Tipo=' + 2
    }
}]);