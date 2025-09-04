cvaGestao.controller('CollaboratorMainController', ['$scope', '$http', function ($scope, $http) {
    $scope.OnLoad = function () {
        CreateArrays();
        LoadArrays();
    }
     
    function CreateArrays() {
        $scope.SectorList = [];
        $scope.StatusList = [];
    }

    function LoadArrays() {
        //Carrega o campo Status
        $http({
            method: 'GET',
            url: '/Colaborador/GetSpecificStatus'
        }).success(function (result) {
            $scope.StatusList = result;
            document.getElementById('loading02').hidden = true;
        });
    }

    $scope.New = function (id) {
        window.location.href = '/Colaborador/Cadastrar'
    }

    $scope.Edit = function (id) {
        window.location.href = '/Colaborador/Get?id=' + id;
    }

    /*--Filtra os colaboradores--*/
    $scope.Search = function () {
        ValidateFilters();
        $http({
            method: 'GET',
            url: '/Colaborador/Filter_Collaborators?nome=' + $scope.Nome + '&cpf=' + $scope.CPF + '&cnpj=' + $scope.CNPJ + '&especialidade=' + 0 + '&status=' + $scope.Status
        }).success(function (result) {
            if (result.length == 0) {
                $scope.Colaboradores = null;
            } else {
                $scope.Colaboradores = result;
            }
        });
    }
    function ValidateFilters() {
        if ($scope.Nome == undefined || $scope.Nome == null)
            $scope.Nome = '';
        if ($scope.CPF == undefined || $scope.CPF == null)
            $scope.CPF = '';
        if ($scope.CNPJ == undefined || $scope.CNPJ == null)
            $scope.CNPJ = '';
        //if ($scope.Especialidade == undefined || $scope.Especialidade == null)
        //    $scope.Especialidade = 0;
        if ($scope.Status == undefined || $scope.Status == null)
            $scope.Status = 0;
    }

    /*--Exportação--*/
    $scope.ExportarExcel = function () {
        ValidateFilters();
        window.location.href = '/Colaborador/RelatorioColaboradores?name=' + $scope.Nome + '&cpf=' + $scope.CPF + '&cnpj=' + $scope.CNPJ + '&Especialidade=' + 0 + '&status=' + $scope.Status + '&reportType=' + 1
    }
    $scope.ExportarPDF = function () {
        ValidateFilters();
        window.location.href = '/Colaborador/RelatorioColaboradores?name=' + $scope.Nome + '&cpf=' + $scope.CPF + '&cnpj=' + $scope.CNPJ + '&Especialidade=' + 0 + '&status=' + $scope.Status + '&reportType=' + 2
    }
}]);