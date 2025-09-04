cvaGestao.controller('ProfileController', ['$scope', '$http', function ($scope, $http) {

    $scope.InitValue = function (model) {
        $scope.profile = model;
        $scope.profile.Status = {};
        $scope.profile.Status = model.Status;
        $scope.UserViewSelectedList = model.UserView;
    }

    $scope.profile = {};
    $scope.StatusList = [];
    $scope.ViewList = [];
    $scope.UserViewSelectedList = [];

    $http({
        method: 'GET',
        url: '/Perfil/GetSpecificStatus'
    }).success(function (result) {
        $scope.StatusList = result;
    });

    $http({
        method: 'GET',
        url: '/Perfil/GetViews'
    }).success(function (result) {
        $scope.ViewList = result;
        if ($scope.UserViewSelectedList.length > 0) {
            for (var b = 0; b < $scope.UserViewSelectedList.length; b++) {
                for (var i = 0; i < $scope.ViewList.length; i++) {
                    if ($scope.ViewList[i].Id === $scope.UserViewSelectedList[b].Id) {
                        $scope.ViewList.splice(i, 1);
                    }
                }
            }
        }
    });

    $scope.Save = function () {
        $scope.profile.UserView = $scope.UserViewSelectedList;
        if ($scope.ValidateFields()) {
            $scope.IsProcessing = true;
            $http.post('/Perfil/Salvar', $scope.profile)
                .success(function (message) {
                    if (message.Success == null || message.Success == undefined) {
                        $('#message-error').text('');
                        $('#message-error').append(message.Error.Message);
                        $('#message-box-danger').addClass('open');
                    }
                    else {
                        $('#message-box-success').addClass('open');
                        $scope.profile = {};
                        $scope.UserViewSelectedList = [];
                    }
                    $scope.IsProcessing = false;
                });
        }
    }

    $scope.ValidateFields = function() {
        if ($scope.profile == null) {
            noty({ text: 'Obrigatório o preenchimento do formulário', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.profile.Name == null || $scope.profile.Name == '') {
            noty({ text: 'Preencher o nome do perfil', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.profile.Description == null || $scope.profile.Description == '') {
            noty({ text: 'Preencher a descrição do perfil', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.profile.Status.Id == null || $scope.profile.Status.Id == '') {
            noty({ text: 'Selecione o status do registro', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }

    $scope.UserViewSelected = function () {
        for (var i = 0; i < $scope.ViewList.length; i++) {
            if ($scope.ViewList[i].Id == $scope.profile.UserView.Id) {
                $scope.profile.UserView.Description = $scope.ViewList[i].Description;
                break;
            }
        }
    }

    $scope.AddLines = function () {
        var model = {};
        for (var i = 0; i < $scope.ViewList.length; i++) {
            if ($scope.ViewList[i].Id == $scope.profile.UserView.Id) {
                model.Name = $scope.ViewList[i].Name;
                model.Id = $scope.ViewList[i].Id;
                model.Description = $scope.ViewList[i].Description;
                $scope.ViewList.splice(i, 1);
                break;
            }
        }
        
        $scope.UserViewSelectedList[$scope.UserViewSelectedList.length] = model;
        $scope.profile.UserView = {};
    }

    $scope.RemoveLine = function (index) {
        var model = $scope.UserViewSelectedList[index];
        $scope.ViewList[$scope.ViewList.length] = model;
        $scope.UserViewSelectedList.splice(index, 1);
    }

    $scope.EditProfile = function (id) {
        window.location.href = '/Perfil/Editar?profileID=' + id;
    }

    $scope.ClearForm = function () {
        $scope.profile = {};
        $scope.UserViewSelectedList = [];
    }
}]);