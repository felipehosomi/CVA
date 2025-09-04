cvaGestao.controller('ProjectParametersController', ['$scope', '$http', function ($scope, $http) {
    $scope.OnLoad = function () {
        CreateArrays();
        LoadArrays();
    }

    function CreateArrays() {
        $scope.ParameterLine = -1;
        $scope.Parametros = [];
    }
    function LoadArrays() {
        $http({
            method: 'GET',
            url: '/ParametrizacaoProjetos/Get_All'
        }).success(function (result) {
            $scope.Parametros = result;
        });
    }

    /*--Parâmetros--*/
    $scope.Save = function () {
        var message = {};
        for (var i = 0; i < $scope.Parametros.length; i++) {
            $http.post('/ParametrizacaoProjetos/Save', $scope.Parametros[i]).success(function (result) {
                if (result.Success == null || result.Success == undefined) {
                    $('#message-error').text('');
                    $('#message-error').append(result.Error.Message);
                    $('#message-box-danger').addClass('open');
                    return false;
                }
                else {
                    message = result.Success.Message;
                }
            });
        }
        $('#message-success').text('');
        $('#message-success').append(message);
        $('#message-box-success').addClass('open');
    }

    $scope.AddParameter = function () {
        if ($scope.ParametrosProjeto == undefined) {
            noty({ text: 'Preencha o formulário', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ParametrosProjeto.Equipe == null || $scope.ParametrosProjeto.Equipe == '') {
            noty({ text: 'Obrigatório informar a equipe responsável pelo projeto', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ParametrosProjeto.De == null || $scope.ParametrosProjeto.De == '') {
            noty({ text: 'Obrigatório preencher o campo "De"', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ParametrosProjeto.Ate == null || $scope.ParametrosProjeto.Ate == '') {
            noty({ text: 'Obrigatório preencher o campo "Até"', layout: 'topRight', type: 'error' });
            return false;
        }

        var model = {};

        model.Id = $scope.ParametrosProjeto.Id;
        model.Equipe = $scope.ParametrosProjeto.Equipe;
        model.De = $scope.ParametrosProjeto.De;
        model.Ate = $scope.ParametrosProjeto.Ate;

        if ($scope.ParameterLine == -1) {
            var i = $scope.Parametros.length;
            $scope.Parametros[i] = model;
        }

        else {
            var i = $scope.ParameterLine;
            $scope.Parametros[i] = model;
        }

        model = {};
        $scope.ParametrosProjeto = {};
        $scope.ParameterLine = -1;
    }

    $scope.EditParameter = function (index) {
        $scope.ParameterLine = index;
        $scope.ParametrosProjeto = {};

        $scope.ParametrosProjeto.Id = $scope.Parametros[index].Id;
        $scope.ParametrosProjeto.Equipe = $scope.Parametros[index].Equipe;
        $scope.ParametrosProjeto.De = $scope.Parametros[index].De;
        $scope.ParametrosProjeto.Ate = $scope.Parametros[index].Ate;
    }
}]);