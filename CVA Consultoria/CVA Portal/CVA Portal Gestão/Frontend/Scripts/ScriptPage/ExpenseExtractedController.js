cvaGestao.controller('ExpenseExtractedController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {
    $scope.OnLoad = function () {
        CreateModel();
        CreateArrays();
        LoadArrays();
    }

    function CreateModel() {
        $scope.expense = {};
        $scope.TotalExpense = 0.0;
    }

    function CreateArrays() {
        $scope.ProjectList = [];
        $scope.ClientList = [];
        $scope.TableList = [];
    }

    function LoadArrays() {
        $http({
            method: 'GET',
            url: '/Cliente/LoadCombo'
        }).success(function (result) {
            $scope.ClientList = result;
            document.getElementById('load-00').hidden = true;
        });

        $http({
            method: 'GET',
            url: '/Projeto/LoadCombo'
        }).success(function (result) {
            $scope.ProjectList = result;
            document.getElementById('load-01').hidden = true;
        });
    }


    $scope.Search = function () {
        if (($("#initialDate").attr('class').indexOf('invalid') != -1) || ($("#finishDate").attr('class').indexOf('invalid') != -1)) {
            noty({ text: 'Informe uma data válida ou deixe o campo em branco', layout: 'topRight', type: 'error' });
        }
        else {
            ValidateModel();
            $http({
                method: 'GET',
                url: '/Despesa/Filtrar?col=' + 0 + '&cli=' + $scope.expense.ClientId + '&prj=' + $scope.expense.Project.Id + '&de=' + $filter('date')($scope.expense.InitialDate, 'mediumDate') + '&ate=' + $filter('date')($scope.expense.FinishDate, 'mediumDate') + '&interno=true'
            }).success(function (result) {
                if (result.length > 0) {
                    $scope.TotalExpense = 0.0;
                    for (var i = 0; i < result.length; i++) {
                        var valorReembolso = result[i].ValorReembolso.replace(',', '.');
                        $scope.TotalExpense = Number($scope.TotalExpense) + Number(valorReembolso);
                    }
                    $scope.TotalExpense = parseFloat($scope.TotalExpense.toFixed(2));
                    $scope.TableList = result;
                }
                
                else {
                    $scope.TableList = [];
                    $scope.TotalExpense = 0.0;
                }
            });
        }
    }
    
    function ValidateModel() {
        if ($scope.expense.ClientId == undefined)
            $scope.expense.ClientId = 0;

        if ($scope.expense.Project == null)
            $scope.expense.Project = {};
        if ($scope.expense.Project.Id == undefined)
            $scope.expense.Project.Id = 0;

        if ($scope.expense.InitialDate == undefined)
            $scope.expense.InitialDate = null;
        if ($scope.expense.FinishDate == undefined)
            $scope.expense.FinishDate = null;

        if ($scope.expense.FinishDate != null && $scope.expense.InitialDate != null) {
            if ($scope.expense.InitialDate > $scope.expense.FinishDate)
                $scope.expense.InitialDate = $scope.expense.FinishDate;
        }
    }

    // Métodos de exportação
    $scope.ExportarExcel = function () {
        window.location.href = '/Despesa/Extrair_Relatorio?col=' + 0 + '&cli=' + $scope.expense.ClientId + '&prj=' + $scope.expense.Project.Id + '&de=' + $filter('date')($scope.expense.InitialDate, 'mediumDate') + '&ate=' + $filter('date')($scope.expense.FinishDate, 'mediumDate') + '&tipo=Excel' + '&interno=true';
    }
    $scope.ExportarPDF = function () {
        window.location.href = '/Despesa/Extrair_Relatorio?col=' + 0 + '&cli=' + $scope.expense.ClientId + '&prj=' + $scope.expense.Project.Id + '&de=' + $filter('date')($scope.expense.InitialDate, 'mediumDate') + '&ate=' + $filter('date')($scope.expense.FinishDate, 'mediumDate') + '&tipo=PDF' + '&interno=true';
    }
}]);    