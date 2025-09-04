cvaGestao.controller('NoteAdministratorController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {
    $scope.OnLoad = function () {
        $scope.TotalHours = '00:00';
        CreateArrays();
        LoadArrays();
    }

    function CreateArrays() {
        $scope.CollaboratorList = [];
        $scope.ClientList = [];
        $scope.ProjectList = [];
        $scope.NotesList = [];
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
        if (ValidateFields()) {
            $http({
                method: 'GET',
                url: '/Apontamento/Filtrar?user=' + $scope.Colaborador + '&initialDate=' + $filter('date')($scope.De, 'mediumDate') + '&finishDate=' + $filter('date')($scope.Ate, 'mediumDate') + '&projectId=' + $scope.Projeto + '&statusID=0' + '&clientID=' + $scope.Cliente
            }).success(function (result) {
                if (result.length == 0) {
                    $scope.TotalHours = '0:00';
                    $scope.NotesList = null;
                } else {
                    $scope.TotalHours = result[0].TotalHours;
                    $scope.NotesList = result;
                }
            });
        }
    }

    function ValidateFields() {
        if (($("#initialDate").attr('class').indexOf('invalid') != -1) || ($("#finishDate").attr('class').indexOf('invalid') != -1)) {
            noty({ text: 'Informe uma data válida ou deixe o campo em branco', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Colaborador == undefined)
            $scope.Colaborador = 0;
        if ($scope.Projeto == undefined)
            $scope.Projeto = 0;
        if ($scope.Cliente == undefined)
            $scope.Cliente = 0;
        if ($scope.De == undefined)
            $scope.De = null;
        if ($scope.Ate == undefined)
            $scope.Ate = null;

        return true;
    }
    
    $scope.ExportarExcel = function () {
        window.location.href = '/Apontamento/RelatorioApontamento?user=' + $scope.Colaborador + '&initialDate=' + $filter('date')($scope.De, 'mediumDate') + '&finishDate=' + $filter('date')($scope.Ate, 'mediumDate') + '&projectId=' + $scope.Projeto + '&statusID=0' + '&type=Excel' + '&clientID=' + $scope.Cliente + '&interno=false';
    }

    $scope.ExportarPDF = function () {
        window.location.href = '/Apontamento/RelatorioApontamento?user=' + $scope.Colaborador + '&initialDate=' + $filter('date')($scope.De, 'mediumDate') + '&finishDate=' + $filter('date')($scope.Ate, 'mediumDate') + '&projectId=' + $scope.Projeto + '&statusID=0' + '&type=PDF' + '&clientID=' + $scope.Cliente + '&interno=false';
    }
}]);