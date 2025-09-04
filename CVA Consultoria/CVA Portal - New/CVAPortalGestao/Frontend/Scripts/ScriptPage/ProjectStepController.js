cvaGestao.controller('ProjectStepController', ['$scope', '$http', function ($scope, $http) {

    $scope.IsProjectOrOportunitty = [];
    $scope.IsProjectOrOportunitty[0] = { Id: 0, Name: 'Oportunidade' };
    $scope.IsProjectOrOportunitty[1] = { Id: 1, Name: 'Projeto' };

    $scope.InitValue = function (model) {
        $scope.projectStep = model;
    }

    $scope.projectStep = {};
    $scope.projectStep.Status = {};
    $scope.StatusList = [];
    $scope.PercentList = [];

    $http({
        method: 'GET',
        url: '/FaseProjeto/GetSpecificStatus'
    }).success(function (status) {
        $scope.StatusList = status;
    });

    $scope.EditProjectStep = function (id) {
        window.location.href = '/FaseProjeto/Editar?id=' + id;
    }

    $scope.ClearForm = function () {
        $scope.projectStep = {};
    }

    $('#oportunitty-cancel').on('ifClicked', function (event) {
        var checked = $("#oportunitty-iscancel").parent('[class*="icheckbox"]').hasClass("checked");
        if (!checked)
            $scope.projectStep.IsCancel = 1;
        else
            $scope.projectStep.IsCancel = 0;
    });

    $scope.SaveProjectStep = function () {
        if (ValidateFields()) {
            $scope.IsProcessing = true;
            $http.post('/FaseProjeto/Salvar', $scope.projectStep)
                    .success(function (message) {
                        if (message.Success == null || message.Success == undefined) {
                            $('#message-error').text('');
                            $('#message-error').append(message.Error.Message);
                            $('#message-box-danger').addClass('open');
                        }
                        else {
                            $('#message-box-success').addClass('open');
                            $scope.projectStep = {};
                        }
                        $scope.IsProcessing = false;
                    });
        }
    }

    function ValidateFields() {
        if ($scope.projectStep.Name == '' || $scope.projectStep.Name == undefined) {
            noty({ text: 'Informe o nome da fase', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.projectStep.Code == '' || $scope.projectStep.Code == undefined) {
            noty({ text: 'Informe o código da fase do projeto', layout: 'topRight', type: 'error' });
            return false;
        }          

        if ($scope.projectStep.Id == null) {
            if ($scope.projectStep.IsProjectStep == undefined) {
                noty({ text: 'Selecione a utilização do cadastro', layout: 'topRight', type: 'error' });
                return false;
            }
        }
        
        return true;
    }
}]);