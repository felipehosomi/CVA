cvaGestao.controller('UserController', ['$scope', '$http', function ($scope, $http) {
    $scope.OnLoad = function (model) {
        CreateArrays();
        LoadArrays();
        CreateModel(model);
    }

    function CreateModel(model) {
        $scope.user = {};
        $scope.user.Status = {};
        $scope.user.Collaborator = {};
        $scope.user.Profile = {};

        if (model != null) {
            $scope.user = model;
            $scope.user.Status = model.Status;
            $scope.user.Collaborator = model.Collaborator;
            $scope.user.Profile = model.Profile;
        }
    }

    function CreateArrays() {
        $scope.StatusList = [];
        $scope.CollaboratorList = [];
        $scope.ProfileList = [];
    }

    function LoadArrays() {
        $http({
            method: 'GET',
            url: '/Usuario/GetSpecificStatus'
        }).success(function (status) {
            $scope.StatusList = status;
        });

        $http({
            method: 'GET',
            url: '/Colaborador/Get_NotUser'
        }).success(function (collaborator) {
            $scope.CollaboratorList = collaborator;
        });

        $http({
            method: 'GET',
            url: '/Usuario/GetProfile'
        }).success(function (profile) {
            $scope.ProfileList = profile;
        });
    }

    $scope.Save = function () {
        if (ValidateModel()) {
            $scope.IsProcessing = true;
            $http.post('/Usuario/Save', $scope.user)
                .success(function (message) {
                    if (message.Success == null || message.Success == undefined) {
                        $('#message-error').text('');
                        $('#message-error').append(message.Error.Message);
                        $('#message-box-danger').addClass('open');
                    }
                    else {
                        $('#message-box-success').addClass('open');

                        $scope.CollaboratorSelectedList = [];
                        $scope.ProfileSelectedList = [];
                    }
                    $scope.IsProcessing = false;
                });
        }
    }

    function ValidateModel() {
        if ($scope.user.Name == '' || $scope.user.Name == null) {
            noty({ text: 'Preencha o campo "Nome"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.user.Email == '' || $scope.user.Email == null) {
            noty({ text: 'Preencha o campo "Email"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.user.Status == undefined || $scope.user.Status.Id == null || $scope.user.Status.Id == '') {
            noty({ text: 'Preencha o campo "Status"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.user.Collaborator == undefined || $scope.user.Collaborator.Id == null || $scope.user.Collaborator.Id == '') {
            noty({ text: 'Preencha o campo "Colaborador"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.user.Profile == undefined || $scope.user.Profile.Id == null || $scope.user.Profile.Id == '') {
            noty({ text: 'Preencha o campo "Perfil"', layout: 'topRight', type: 'error' });
            return false;
        }

        return true;
    }

    $scope.EditUser = function (id) {
        window.location.href = '/Usuario/Editar?userID=' + id;
    }
}]);