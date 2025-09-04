cvaGestao.controller('SpecialtyController', ['$scope', '$http', function ($scope, $http) {
    $scope.OnLoad = function (model) {
        CreateArrays();
        LoadArrays();
        CreateModel(model);
    }

    function CreateArrays() {
        $scope.StatusList = [];
        $scope.TiposEspecialidadeList = [];
    }

    function LoadArrays() {
        $http({
            method: 'GET',
            url: '/Especialidade/GetSpecificStatus'
        }).success(function (result) {
            $scope.StatusList = result;
        });

        $http({
            method: 'GET',
            url: '/Especialidade/Get_TiposEspecialidade'
        }).success(function (result) {
            $scope.TiposEspecialidadeList = result;
        });
    }

    function CreateModel(model) {
        $scope.specialty = {};
        $scope.specialty.Status = {};
        $scope.specialty.TiposEspecialidade = {};
        if (model != null) {
            $scope.specialty = model;
            $scope.specialty.TiposEspecialidade = model.TiposEspecialidade;
        }
    }

   
    $scope.SaveSpecialty = function () {
        if (ValidateFields()) {
            $scope.specialty.Status = {};
            $scope.specialty.Status.Id = $scope.specialty.StatusId;
            $scope.IsProcessing = true;
            $http.post('/Especialidade/Salvar', $scope.specialty)
                .success(function (message) {
                    if (message.Success == null || message.Success == undefined) {
                        $('#message-error').text('');
                        $('#message-error').append(message.Error.Message);
                        $('#message-box-danger').addClass('open');
                    }
                    else {
                        $('#message-box-success').addClass('open');
                        $scope.specialty = {};
                    }
                    $scope.IsProcessing = false;
                });
        }
    }

    function ValidateFields() {
        if ($scope.specialty == null) {
            noty({ text: 'Preencha o formulário!', layout: 'topRight', type: 'error' });
            return false;
        }

        if ($scope.specialty.Name == '' || $scope.specialty.Name == undefined) {
            noty({ text: 'Informe o nome da especialidade', layout: 'topRight', type: 'error' });
            return false;
        }

        if ($scope.specialty.StatusId == '' || $scope.specialty.StatusId == null || $scope.specialty.StatusId == undefined) {
            noty({ text: 'Selecione o status da especialidade', layout: 'topRight', type: 'error' });
            return false;
        }

        if ($scope.specialty.Value <= 0 || $scope.specialty.Value == null) {
            noty({ text: 'Informe um valor válido para a especialidade', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }

    $scope.EditSpecialty = function (id) {
        window.location.href = '/Especialidade/Editar?id=' + id;
    }
}]);