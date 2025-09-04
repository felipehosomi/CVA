cvaGestao.controller('ProjectMainController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {

    //Cria o modelo de oportunidade
    $scope.project = {};
    $scope.project.Contact = {};
    $scope.project.Contact.ProjectManager = {};
    $scope.project.Financial = {};

    //Cria listas para armazenar os objetos carregados do banco
    function CreateArrays() {
        $scope.ClientList = [];
        $scope.projectManagerList = [];
    }

    //Carrega o campo Clientes
    $http({
        method: 'GET',
        url: '/Cliente/LoadCombo'
    }).success(function (clients) {
        $scope.ClientList = clients;
        document.getElementById('loading00').hidden = true;
    });

    //Carrega a tabela de Projeto conforme os filtros informados
    $scope.Search = function () {
        ValidateFilters(); 
        $http({
            method: 'GET',
            url: '/Projeto/Filter_Projects?clientId=' + $scope.project.ClientId + '&code=' + $scope.project.Code
        }).success(function (result) {
            if (result.length == 0) {
                $scope.TableList = null;
            } else {
                $scope.TableList = result;
            }
        });
    }

    function ValidateFilters() {
        if ($scope.project.ClientId == null || $scope.project.ClientId == undefined) {
            $scope.project.Client = {};
            $scope.project.ClientId = 0;
        }

        if ($scope.project.Code == null || $scope.project.Code == undefined)
            $scope.project.Code = '';
    }

    //Redireciona para a página de edição da Projeto
    $scope.EditProject = function (id) {
        window.location.href = '/Projeto/Get?id=' + id;
    }


    //Gerar relatório completo do projeto
    $scope.RelatorioProjeto = function (id) {
        window.location.href = '/Projeto/RelatorioProjeto?id=' + id
    } 

    //Faz a exportação para Excel
    $scope.ExportarExcel = function () {
        window.location.href = '/Projeto/ExtrairRelatorio?clientId=' + $scope.project.ClientId + '&code=' + $scope.project.Code + '&reportType=' + 1
    }

    //Faz a exportação para PDF
    $scope.ExportarPDF = function () {
        window.location.href = '/Projeto/ExtrairRelatorio?clientId=' + $scope.project.ClientId + '&code=' + $scope.project.Code + '&reportType=' + 2
    }
}]);