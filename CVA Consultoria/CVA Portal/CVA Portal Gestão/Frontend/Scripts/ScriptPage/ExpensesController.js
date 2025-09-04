cvaGestao.controller('ExpensesController', ['$scope', '$http', '$filter', '$window', function ($scope, $http, $filter, $window) {
    /*----Iniciais----*/
    $scope.OnLoad = function () {
        CreateModel();
        CreateArrays();
        LoadArrays();
    }

    function CreateModel() {
        $scope.expense = {};
        $scope.fd = new FormData();
    }

    function CreateArrays() {
        $scope.ProjectList = []
        $scope.ExpensesList = [];
        $scope.ExpenseTypeList = [];
    }

    function LoadArrays() {
        $http({
            method: 'GET',
            url: '/Despesa/GetProjects'
        }).success(function (result) {
            $scope.ProjectList = result;
        });

        $http({
            method: 'GET',
            url: '/Despesa/Get'
        }).success(function (result) {
            $scope.ExpensesList = result;
        });

    }
    /*-------------------------------------*/

    /*-----CRUD-----*/
    $scope.Salvar = function (expense) {
        if (ValidateModel()) {
            $scope.IsProcessing = true;
            
            $scope.fd.set("Expense", angular.toJson($scope.expense));

            $http.post("/Despesa/Save", $scope.fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            }).success(function (message) {
                if (message.Success == null || message.Success == undefined) {
                    $('#message-error').text('');
                    $('#message-error').append(message.Error.Message);
                    $('#message-box-danger').addClass('open');
                }
                else {
                    $('#message-success').text('');
                    $('#message-success').append(message.Success.Message);
                    $('#message-box-success').addClass('open');
                    $scope.AtualizarGrid();
                    $scope.expense = {};
                }
                $scope.IsProcessing = false;
            });;
        }
    }

    $scope.Editar = function (expense) {
        var resultado = angular.copy(expense)
        resultado.Data = new Date(resultado.Data);
  
        $scope.expense = resultado;
        $scope.expense.Data = resultado.Data;
        $scope.ProjectSelected();
        $scope.expense.TipoDespesa.Id = resultado.TipoDespesa.Id;

        $('#tab-first').addClass('active');
        $('#tab-first-li').addClass('active');
        $('#tab-tree').removeClass('active');
        $('#tab-three-li').removeClass('active');
        $('#tab-firt').attr('aria-expended', 'true');
        $('#tab-tree').attr('aria-expended', 'false');
    }

    $scope.Remover = function (id, index) {
        $http({
            method: 'GET',
            url: '/Despesa/Remove?id=' + id
        }).success(function (message) {
            if (message.Success == null || message.Success == undefined) {
                $('#message-error').text('');
                $('#message-error').append(message.Error.Message);
                $('#message-box-danger').addClass('open');
            }
            else {      
                $('#message-success').text('');
                $('#message-success').append(message.Success.Message);
                $('#message-box-success').addClass('open');
                $scope.AtualizarGrid();
            }
        });
    }

    $scope.AtualizarGrid = function () {
        $http({
            method: 'GET',
            url: '/Despesa/Get'
        }).success(function (result) {
            $scope.ExpensesList = result;
        });
    }

    function ValidateModel() {
        if ($scope.expense.Projeto.Id == '' || $scope.expense.Projeto.Id == 0 || $scope.expense.Projeto == undefined) {
            noty({ text: 'Obrigatório informar o projeto', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.expense.Data == '' || $scope.expense.Data == null || $scope.expense.Data == undefined) {
            noty({ text: 'Obrigatório informar a data', layout: 'topRight', type: 'error' });
            return false;
        }
        var today = new Date();
        if ($scope.expense.Data > today) {
            noty({ text: 'Impossível apontar despesa com data futura', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.expense.TipoDespesa.Id == '' || $scope.expense.TipoDespesa.Id == null || $scope.expense.TipoDespesa.Id == undefined) {
            noty({ text: 'Obrigatório informar o tipo de despesa', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.expense.Descricao == '' || $scope.expense.Descricao == null || $scope.expense.Descricao == undefined) {
            noty({ text: 'Obrigatório informar a descrição', layout: 'topRight', type: 'error' });
            return false;
        }
        if (($scope.expense.ValorDespesa == '' || $scope.expense.ValorDespesa == null) && $('#valor-reembolso').hidden == false) {
            noty({ text: 'Obrigatório informar o valor para reembolso', layout: 'topRight', type: 'error' });
            return false;
        }
        if (($scope.expense.Quilometragem == '' || $scope.expense.Quilometragem == null) && $('#quilometragem').hidden == false) {
            noty({ text: 'Obrigatório informar o valor para reembolso', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.expense.NumNota == '' || $scope.expense.NumNota == null) {
            noty({ text: 'Obrigatório informar o N° da nota fiscal', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.expense.ValorNota == '' || $scope.expense.ValorNota == null) {
            noty({ text: 'Obrigatório informar o valor total da nota fiscal', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }
    /*-------------------------------------*/


    $scope.ProjectSelected = function () {
        $http({
            method: 'GET',
            url: '/Despesa/GetExpenseByProject?projectId=' + $scope.expense.Projeto.Id
        }).success(function (result) {
            if (result == 'null')
                return;
            $scope.ExpenseTypeList = result;
        });
    }


    $scope.UploadFile = function (files) {
        $scope.fd.set("file", files[0]);
    };

    $scope.GetAttachedFile = function () {
        var fileURL = 'GetAttachedFile?date=' + $filter('date')($scope.expense.Data, 'mediumDate') + '&projectId=' + $scope.expense.Projeto.Id + '&fileName=' + $scope.expense.File;
        $window.open(fileURL, '_blank', '');
    }




    //------- [Métodos para calcular valor total do reembolso] ---------------
    $scope.TipoAlterado = function () {
        for (var i = 0; i < $scope.ExpenseTypeList.length; i++) {
            if ($scope.ExpenseTypeList[i].Expense.Id == $scope.expense.TipoDespesa.Id) {
                if ($scope.ExpenseTypeList[i].Expense.UnitMeter == 1)//KM
                {
                    $('#quilometragem').show();
                    $('#valor-reembolso').hide();
                    $scope.CalculaKm();
                    break;
                } else {
                    $('#quilometragem').hide();
                    $('#valor-reembolso').show();
                    $scope.CalculaTotal();
                    break;
                }
            } else {
                $('#valor-nf').show();
                $('#quilometragem').hide();
                $('#valor-reembolso').show();
            }
        }
    }
    $scope.CalculaTotal = function () {
        if ($scope.expense.ValorDespesa != null) {
            var valor = $scope.expense.ValorDespesa.replace(',', '.');
            for (var i = 0; i < $scope.ExpenseTypeList.length; i++) {
                if ($scope.ExpenseTypeList[i].Expense.Id == $scope.expense.TipoDespesa.Id) {
                    if (valor < (parseFloat($scope.ExpenseTypeList[i].Value))) {
                        $scope.expense.ValorReembolso = valor;
                    }
                    else
                        $scope.expense.ValorReembolso = $scope.ExpenseTypeList[i].Value;
                }
            }
        }
    }
    $scope.CalculaKm = function () {
        for (var i = 0; i < $scope.ExpenseTypeList.length; i++) {
            if ($scope.ExpenseTypeList[i].Expense.Id == $scope.expense.TipoDespesa.Id) {

                var valor = parseFloat($scope.ExpenseTypeList[i].Value.replace(',', '.'));
                var km = parseFloat($scope.expense.Quilometragem.replace(',', '.'));

                $scope.expense.ValorReembolso = (km * valor);
            }
        }
    }
}]);