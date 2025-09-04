cvaGestao.controller('MessageController', ['$scope', '$http', function ($scope, $http) {

//------ Método inicial da tela --------------------------------------------------------
    $scope.InitValue = function (model) {

        var user = {};
        var profile = {};
        $scope.message = {};
        $scope.UserList = [];
        $scope.StatusList = [];
        $scope.ProfileList = [];
        $scope.message.User = [];
        $scope.message.Profile = [];
        $scope.UserSelectedList = [];
        $scope.ProfileSelectedList = [];

        //---- Caso seja uma edição de mensagem
        if (model != null) {

            $scope.message = model;
            $scope.message.User = model.User;
            $scope.message.ProfileId = model.ProfileId;
            $scope.message.Code = model.Code;
            $scope.message.Message = model.Message;
            $scope.message.StatusId = model.StatusId;

            model.InitialDate = new Date(model.InitialDate);
            model.FinishDate = new Date(model.FinishDate);
            //São adicionadas 4 horas pois por algum motivo o javascript reduz 4 horas, alterando as datas da mensagem
            model.InitialDate.setHours(model.InitialDate.getHours() + 4);
            model.FinishDate.setHours(model.FinishDate.getHours() + 4);
            $scope.UserSelectedList = model.User;
            for (var i = 0; i < $scope.UserSelectedList.length; i++)
                $scope.UserSelectedList[i].Tipo = 'Usuário';
            $scope.ProfileSelectedList = model.Profile;
            for (var i = 0; i < $scope.ProfileSelectedList.length; i++)
                $scope.ProfileSelectedList[i].Tipo = 'Perfil/Grupo';
        }
    }

    $http({
        method: 'GET',
        url: '/Mensagem/GetSpecificStatus'
    }).success(function (result) {
        $scope.StatusList = result;
    });

    $http({
        method: 'GET',
        url: '/Mensagem/GetUsers'
    }).success(function (result) {
        $scope.UserList = result;
        document.getElementById('loading00').hidden = true;
    });

    $http({
        method: 'GET',
        url: '/Mensagem/GetProfiles'
    }).success(function (result) {
        $scope.ProfileList = result;
        document.getElementById('loading01').hidden = true;
    });

    $scope.SaveMessage = function () {
        if ($scope.ValidateFields()) {
            $scope.IsProcessing = true;
            $scope.message.User = $scope.UserSelectedList;
            $scope.message.Profile = $scope.ProfileSelectedList;

            $http.post('/Mensagem/SalvarMensagem', $scope.message)
                    .success(function (message) {
                        if (message.Success == null || message.Success == undefined) {
                            $('#message-error').text('');
                            $('#message-error').append(message.Error.Message);
                            $('#message-box-danger').addClass('open');
                        }
                        else {
                            $('#message-success').text('');
                            $('#message-success').append('Oportunidade criada com sucesso!');
                            $('#message-box-success').addClass('open');
                            $scope.message = {};
                            $scope.UserSelectedList = [];
                            $scope.ProfileSelectedList = [];
                        }
                        $scope.IsProcessing = false;
                    });
        }
    }

    $scope.AddLines = function () {
        if ($scope.message.User == null) {
            for (var i = 0; i < $scope.ProfileList.length; i++) {
                if ($scope.message.ProfileId == $scope.ProfileList[i].Id) {
                    var profile = {};
                    profile.Id = $scope.message.ProfileId;
                    profile.Name = $scope.ProfileList[i].Name;
                    $scope.message.Profile = [];
                    $scope.message.Profile[$scope.message.Profile.length] = profile;

                    profile.Tipo = 'Perfil/Grupo';
                    $scope.ProfileSelectedList[$scope.ProfileSelectedList.length] = profile;

                    profile = {};
                    break;
                }
            }
        } else {
            for (var i = 0; i < $scope.UserList.length; i++) {
                if ($scope.message.User == $scope.UserList[i].Id) {
                    var user = {};
                    user.Id = $scope.message.User;
                    user.Name = $scope.UserList[i].Name;
                    $scope.message.User = [];
                    $scope.message.User[$scope.message.User.length] = user;

                    user.Tipo = 'Usuário';
                    $scope.UserSelectedList[$scope.UserSelectedList.length] = user;
                    user = {};
                    break;
                }
            }
        }
        $scope.message.User = null;
        $scope.message.ProfileId = null;
    }

    $scope.RemoveLine = function (index, tipo) {

        if (tipo == 1) {
            for (var i = 0; i < $scope.message.User.length; i++) {
                var result = $scope.UserSelectedList[index];
                if ($scope.message.User[i].Id == result.Id) {
                    $scope.message.User.splice(i, 1);
                }
            }
        } else {
            for (var i = 0; i < $scope.message.Profile.length; i++) {
                var result = $scope.ProfileSelectedList[index];
                if ($scope.message.Profile[i].Id == result.Id) {
                    $scope.message.Profile.splice(i, 1);
                }
            }
        }
    }
    $scope.ValidateFields = function () {
        if ($scope.message == null) {
            noty({ text: 'Informe o código da mensagem', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.message.Code == '' || $scope.message.Code == null) {
            noty({ text: 'Informe o código da mensagem', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.message.StatusId == '' || $scope.message.StatusId == null) {
            noty({ text: 'Informe o status do registro', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.message.InitialDate == '' || $scope.message.InitialDate == null) {
            noty({ text: 'Informe a data de envio da mensagem', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.message.FinishDate == '' || $scope.message.FinishDate == null) {
            noty({ text: 'Informe a data de encerramento da mensagem', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.message.Message == '' || $scope.message.Message == null) {
            noty({ text: 'INforme a mensagem para envio', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }

    $scope.ClearForm = function () {
        $scope.message = {};
        $scope.UserSelectedList = [];
        $scope.ProfileSelectedList = [];
    }
}]);