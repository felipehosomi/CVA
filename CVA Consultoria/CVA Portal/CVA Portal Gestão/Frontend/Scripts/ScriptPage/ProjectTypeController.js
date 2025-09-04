cvaGestao.controller('ProjectTypeController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {

    $scope.OnLoad = function (Model) {
        CreateArrays();
        LoadArrays();

        $scope.TipoProjeto = {};

        if (Model != null)
            $scope.TipoProjeto = Model;
    }

    function CreateArrays() {

    }

    function LoadArrays() {

    }

    $scope.Salvar = function () {
        if (ValidateModel()) {
            if ($scope.TipoProjeto.Id == undefined || $scope.TipoProjeto.Id == null) {
                $http.post('/TiposProjeto/Insert', $scope.TipoProjeto).success(function (result) {
                    if (result.Success == null || result.Success == undefined) {
                        $('#message-error').text('');
                        $('#message-error').append(result.Error.Message);
                        $('#message-box-danger').addClass('open');
                    }
                    else {
                        $('#message-success').text('');
                        $('#message-success').append(result.Success.Message);
                        $('#message-box-success').addClass('open');
                    }
                });
            }
            else {
                $http.post('/TiposProjeto/Update', $scope.TipoProjeto)
                    .success(function (result) {
                        if (result.Success == null || result.Success == undefined) {
                            $('#message-error').text('');
                            $('#message-error').append(result.Error.Message);
                            $('#message-box-danger').addClass('open');
                        }
                        else {
                            $('#message-success').text('');
                            $('#message-success').append(result.Success.Message);
                            $('#message-box-success').addClass('open');
                        }
                    });
            }
        }
    }

    function ValidateModel() {
        if ($scope.TipoProjeto == null) {
            noty({ text: 'Preencha o formulário', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.TipoProjeto.Nome == undefined || $scope.TipoProjeto.Nome == null || $scope.TipoProjeto.Nome == '') {
            noty({ text: 'Obrigatório informar o nome', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.TipoProjeto.AMS == undefined || $scope.TipoProjeto.AMS == null) {
            noty({ text: 'Obrigatório informar se AMS', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.TipoProjeto.Equipe == undefined || $scope.TipoProjeto.Equipe == null || $scope.TipoProjeto.Equipe == '') {
            noty({ text: 'Obrigatório informar a equipe', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.TipoProjeto.Descricao == undefined || $scope.TipoProjeto.Descricao == null || $scope.TipoProjeto.Descricao == '') {
            noty({ text: 'Obrigatório informar a descrição', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }
}]);