cvaGestao.controller('ProjectTypeMainController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {

    $scope.OnLoad = function () {
        CreateArrays();
        LoadArrays();
    }

    $scope.Novo = function () {
        window.location.href = '/TiposProjeto/Cadastrar';
    }

    $scope.Editar = function (id) {
        window.location.href = '/TiposProjeto/Editar?id=' + id;
    }

    //$scope.Remover = function (id, index) {
    //    $http({
    //        method: 'GET',
    //        url: '/TiposProjeto/Remove?id=' + id
    //    }).success(function (result) {
    //        if (result.Success == null || result.Success == undefined) {
    //            $('#message-error').text('');
    //            $('#message-error').append(result.Error.Message);
    //            $('#message-box-danger').addClass('open');
    //        }
    //        else {
    //            $scope.TiposProjeto.splice(index, 1);
    //        }
    //    });
    //}

    function CreateArrays() {
        $scope.TiposProjeto = [];
    }

    function LoadArrays() {
        $http({
            method: 'GET',
            url: '/TiposProjeto/Get_All'
        }).success(function (result) {
            $scope.TiposProjeto = result;
        });
    }

}]);