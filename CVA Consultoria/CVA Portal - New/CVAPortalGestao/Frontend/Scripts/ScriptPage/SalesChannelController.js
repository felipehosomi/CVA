cvaGestao.controller('SalesChannelController', ['$scope', '$http', function ($scope, $http) {

    $scope.LoadInit = function (salesChannel) {
        $scope.saleschannel = salesChannel;
        $scope.saleschannel.Status = {};
        $scope.BranchListSelected = salesChannel.Branchs;
    }

    $scope.salesChannel = {};
    $scope.StatusList = [];
    $scope.BranchList = [];
    $scope.BranchListSelected = [];

    $http({
        method: 'GET',
        url: '/CanalVenda/GetSpecificStatus'
    }).success(function (status) {
        $scope.StatusList = status;
    });

    $scope.LoadDescription = function () {
        for (var i = 0; i < $scope.BranchList.length; i++) {
            if ($scope.BranchList[i].Id == $scope.saleschannel.Branch.Id)
            {
                $scope.saleschannel.Branch.Description = $scope.BranchList[i].Description;
                return;
            }
        }
        $scope.saleschannel.Branch.Description = '';
    }

    $scope.AddLines = function () {
        for (var i = 0; i < $scope.BranchList.length; i++) {
            if ($scope.BranchList[i].Id == $scope.saleschannel.Branch.Id) {
                $scope.saleschannel.Branch.Name = $scope.BranchList[i].Name;
            }
        }

        $scope.BranchListSelected[$scope.BranchListSelected.length] = $scope.saleschannel.Branch;
        $scope.saleschannel.Branch = {};
    }

    $scope.RemoveLines = function (index) {
        $scope.BranchListSelected.splice(index, 1);
    }

    $scope.SaveSalesChannel = function () {
        if (ValidateFields()) {

            $scope.IsProcessing = true;
            $scope.saleschannel.Branchs = $scope.BranchListSelected;
            $scope.saleschannel.Status = {};
            $scope.saleschannel.Status.Id = $scope.saleschannel.StatusId;
            $http.post('/CanalVenda/Salvar', $scope.saleschannel)
                    .success(function (message) {
                        if (message.Success == null || message.Success == undefined) {
                            $('#message-error').text('');
                            $('#message-error').append(message.Error.Message);
                            $('#message-box-danger').addClass('open');
                        }
                        else {
                            $('#message-box-success').addClass('open');
                            $scope.saleschannel = {};
                            $scope.BranchListSelected = [];
                        }
                        $scope.IsProcessing = false;
                    });
        }
    }

    $scope.EditSalesChannel = function (id) {
        window.location.href = '/CanalVenda/Editar?id=' + id;
    }

    $scope.ClearForm = function () {
        $scope.saleschannel = {};
        $scope.BranchListSelected = [];
    }
    function ValidateFields() {
        if ($scope.saleschannel.Name == '' || $scope.saleschannel.Name == null) {
            noty({ text: 'Informe o nome do canal de vendas', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.saleschannel.StatusId == '' || $scope.saleschannel.StatusId == null) {
            noty({ text: 'Selecione o status do cadastro', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }
}]);