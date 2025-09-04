cvaGestao.controller('ExpenseAdministratorController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {
    $scope.OnLoad = function () {
        CreateModel();
        CreateArrays();
        LoadArrays();
    }

    function CreateModel() {

    }

    function CreateArrays() {
        $scope.ProjectList = [];
        $scope.CollaboratorList = [];
        $scope.ClientList = [];
        $scope.TableList = [];
    }

    function LoadArrays() {
        $http({
            method: 'GET',
            url: '/Colaborador/LoadCombo'
        }).success(function (result) {
            $scope.CollaboratorList = result;
            document.getElementById('load-00').hidden = true;
        });

        $http({
            method: 'GET',
            url: '/Cliente/LoadCombo'
        }).success(function (result) {
            $scope.ClientList = result;
            document.getElementById('load-01').hidden = true;
        });

        $http({
            method: 'GET',
            url: '/Projeto/LoadCombo'
        }).success(function (result) {
            $scope.ProjectList = result;
            document.getElementById('load-02').hidden = true;
        });
    }

    $scope.Search = function () {
        if (($("#initialDate").attr('class').indexOf('invalid') != -1) || ($("#finishDate").attr('class').indexOf('invalid') != -1)) {
            noty({ text: 'Informe uma data válida ou deixe o campo em branco', layout: 'topRight', type: 'error' });
        }
        else {
            ValidaFiltros();
            $http({
                method: 'GET',
                url: '/Despesa/Filtrar?col=' + $scope.Colaborador + '&cli=' + $scope.Cliente + '&prj=' + $scope.Projeto + '&de=' + $filter('date')($scope.De, 'mediumDate') + '&ate=' + $filter('date')($scope.Ate, 'mediumDate') + '&interno=false'
            }).success(function (result) {
                if (result.length > 0) {
                    $scope.TableList = result;
                    $scope.TotalExpense = 0.0;
                    for (var i = 0; i < result.length; i++) {
                        $scope.TotalExpense = $scope.TotalExpense + Number(result[i].ValorReembolso.replace(',', '.'));
                        $scope.TotalExpense = parseFloat($scope.TotalExpense.toFixed(2));
                    }
                }
                else {
                    $scope.TableList = [];
                    $scope.TotalExpense = 0.0;
                }
            });
        }
    }

    function ValidaFiltros() {
        if ($scope.Colaborador == undefined || $scope.Colaborador == null)
            $scope.Colaborador = 0;

        if ($scope.Cliente == undefined || $scope.Cliente == null)
            $scope.Cliente = 0;

        if ($scope.Projeto == undefined || $scope.Projeto == null)
            $scope.Projeto = 0;

        if ($scope.De == undefined)
            $scope.De = null;

        if ($scope.Ate == undefined)
            $scope.Ate = null;

        if ($scope.De != null && $scope.Ate != null) {
            if ($scope.De > $scope.Ate)
                $scope.De = $scope.expense.FinishDate;
        }
    }

    // Métodos de exportação
    $scope.ExportarExcel = function () {
        window.location.href = '/Despesa/Extrair_Relatorio?col=' + $scope.Colaborador + '&cli=' + $scope.Cliente + '&prj=' + $scope.Projeto + '&de=' + $filter('date')($scope.De, 'mediumDate') + '&ate=' + $filter('date')($scope.Ate, 'mediumDate') + '&tipo=Excel' + '&interno=false';
    }
    $scope.ExportarPDF = function () {
        window.location.href = '/Despesa/Extrair_Relatorio?col=' + $scope.Colaborador + '&cli=' + $scope.Cliente + '&prj=' + $scope.Projeto + '&de=' + $filter('date')($scope.De, 'mediumDate') + '&ate=' + $filter('date')($scope.Ate, 'mediumDate') + '&tipo=PDF' + '&interno=false';
    }
}]);