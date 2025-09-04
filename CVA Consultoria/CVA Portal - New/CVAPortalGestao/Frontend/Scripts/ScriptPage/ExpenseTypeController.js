cvaGestao.controller('ExpenseTypeController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {

    $scope.LoadInit = function (Model) {
        $scope.expenseType = Model;
    }

    $scope.expenseType = {};
    $scope.StatusList = [];
    $scope.UnitMeterList = [];

    $http({
        method: 'GET',
        url: '/TipoDespesa/GetSpecificStatus'
    }).success(function (status) {
        $scope.StatusList = status;
    });
    $http({
        method: 'GET',
        url: '/TipoDespesa/GetUnitMeters'
    }).success(function (unitMeter) {
        $scope.UnitMeterList = unitMeter;
    });

    $scope.Edit = function (id) {
        window.location.href = '/TipoDespesa/Editar?id=' + id;
    }

    $scope.Save = function () {
        if (ValidateFields()) {
            $scope.IsProcessing = true;
            $scope.expenseType.Status = {};
            $scope.expenseType.Status.Id = $scope.expenseType.StatusId;


            $http.post('/TipoDespesa/Salvar', $scope.expenseType)
                    .success(function (message) {
                        if (message.Success == null || message.Success == undefined) {
                            $('#message-error').text('');
                            $('#message-error').append(message.Error.Message);
                            $('#message-box-danger').addClass('open');
                        }
                        else {
                            $('#message-box-success').addClass('open');
                            $scope.expenseType = {};
                        }
                        $scope.IsProcessing = false;
                    });
        }
    }

    function ValidateFields() {
        if ($scope.expenseType.Name == '' || $scope.expenseType.Name == null) {
            noty({ text: 'Informe um nome para o tipo de despesa', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.expenseType.StatusId == '' || $scope.expenseType.StatusId == null) {
            noty({ text: 'Selecione o Status do registro', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }
}]);