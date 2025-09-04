cvaGestao.controller('BranchController', ['$scope', '$http', function ($scope, $http) {
    $scope.TypeBranch = [{ Option: 'Sim', Value: 1 }, { Option: 'Não', Value: 2 }];

    $scope.InitValue = function (model) {
        if (model != null) {
            $http({
                method: 'GET',
                url: '/Filial/GetUf'
            }).success(function (result) {
                $scope.UfList = result;
            });
            $scope.branch = model;

            if (model.Contacts != null) {
                $scope.ContactList = [];
                $scope.ContactList = model.Contacts;
            }

            if (model.PoliticExpense != null) {
                for (var i = 0; i < model.PoliticExpense.length; i++) {
                    $scope.ExpenseSelectedList[i] = {
                        Name: model.PoliticExpense[i].Expense.Name,
                        Value: model.PoliticExpense[i].Value,
                        Id: model.PoliticExpense[i].Id
                    };
                }
            }
        }
    }

    $scope.branch = {};
    $scope.StatusList = [];
    $scope.ContactList = [];
    $scope.UfList = [];
    $scope.cepValid = false;
    $scope.ExpenseTypeList = [];
    $scope.ExpenseSelectedList = [];


    $scope.branch.Address = {};
    $scope.branch.Address.Uf = {};
    $scope.branch.PoliticExpense = {};
    $scope.branch.UseMasterPoliticExpense = 0;

    $http({
        method: 'GET',
        url: '/Filial/GetSpecificStatus'
    }).success(function (result) {
        $scope.StatusList = result;
    });

    
    $scope.AddLines = function () {
        $scope.branch.Contact.CellPhone = $('#cellphone-mask').val().replace('(', '').replace(')', '').replace('-', '').replace(' ','');
        $scope.branch.Contact.Phone = $('#phone-mask').val().replace('(', '').replace(')', '').replace('-', '').replace(' ','');
        $scope.ContactList[$scope.ContactList.length] = $scope.branch.Contact;
        $scope.branch.Contact = {};
    }

    $scope.RemoveLine = function (index) {
        $scope.ContactList.splice(index, 1);
    }

    $scope.EditContact = function (index) {
        $scope.branch.Contact = $scope.ContactList[index];
    }

    $scope.ClearForm = function () {
        $scope.branch = {};
        $scope.ContactList = [];
        $scope.ExpenseSelectedList = [];
        $("input[type=checkbox]").parent(0).removeClass("checked");
    }

    $http({
        method: 'GET',
        url: '/Filial/GetUf'
    }).success(function (result) {
        $scope.UfList = result;
    });

    $http({
        method: 'GET',
        url: '/Filial/GetExpenseTypes'
    }).success(function (result) {
        $scope.ExpenseTypeList = result;
    });

    $scope.SearchCep = function () {
        var zipCode = $('#cep-mask').val().replace('-', '');
        if (zipCode.length == 8) {
            $http({
                method: 'GET',
                url: 'http://viacep.com.br/ws/' + zipCode + '/json/'
            }).success(function (address) {
                if (address.logradouro == undefined) {
                    $scope.cepValid = true;
                    return;
                } else
                    $scope.cepValid = false;

                $scope.branch.Address.Street = address.logradouro;
                $scope.branch.Address.Block = address.bairro;
                $scope.branch.Address.City = address.localidade;
                $scope.branch.Address.Uf = {};

                for (var i = 0; i < $scope.UfList.length; i++) {
                    if ($scope.UfList[i].Uf == address.uf) {
                        $scope.branch.Address.Uf.Id = $scope.UfList[i].Id;
                        break;
                    }
                }
            });
        }
    }

    $scope.SaveBranch = function () {
        if (ValidateFields()) {
            $scope.branch.Contacts = $scope.ContactList;
            $scope.PoliticExpense = [];

            $scope.IsProcessing = true;
            if ($scope.branch.UseMasterPoliticExpense == 0) {
                for (var i = 0; i < $scope.ExpenseSelectedList.length; i++) {
                    $scope.PoliticExpense[i] = {
                        Expense: { Id: $scope.ExpenseSelectedList[i].Id },
                        Value: $scope.ExpenseSelectedList[i].Value
                    };
                }
            }

            $scope.branch.PoliticExpense = $scope.PoliticExpense;
            $http.post('/Filial/Salvar', $scope.branch)
                    .success(function (message) {
                        if (message.Success == null || message.Success == undefined) {
                            $('#message-error').text('');
                            $('#message-error').append(message.Error.Message);
                            $('#message-box-danger').addClass('open');
                        }
                        else {
                            $('#message-box-success').addClass('open');
                            $scope.branch = {};
                            $scope.branch.Address = {};
                            $scope.branch.Address.Uf = {};
                            $scope.cepValid = false;
                            $scope.ContactList = [];
                        }
                        $scope.IsProcessing = false;
                    });
        }
    }

    function ValidateFields() {
        $scope.branch.CNPJ = $('#cnpj-mask').val().replace('.', '').replace('.', '').replace('/', '').replace('-', '');
        if ($scope.branch.Name == '' || $scope.branch.Name == null) {
            noty({ text: 'Informe a Razão Social', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.branch.FantasyName == '' || $scope.branch.FantasyName == null) {
            noty({ text: 'Informe o nome fantasia', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.branch.Code == '' || $scope.branch.Code == null) {
            noty({ text: 'Informe um código para a filial', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.branch.StatusId == '' || $scope.branch.StatusId == null || $scope.branch.StatusId == undefined) {
            noty({ text: 'Selecione um status para o cadastro', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.branch.CNPJ == '' || $scope.branch.CNPJ == null) {
            noty({ text: 'Informe o Cnpj', layout: 'topRight', type: 'error' });
            return false;
        }
        if (($scope.branch.CloseOnLastDay == '' || $scope.branch.CloseOnLastDay == null) && ($scope.branch.FinishDay_Period == '' || $scope.branch.FinishDay_Period == null)) {
            noty({ text: 'Informe o dia de fechamento do período', layout: 'topRight', type: 'error' });
            return false;
        }
        return ValidateAddressFields();
    }

    function ValidateAddressFields() {
        if ($scope.branch.Address.ZipCode == '' || $scope.branch.Address.ZipCode == undefined) {
            noty({ text: 'Informe o CEP', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.branch.Address.Street == '' || $scope.branch.Address.Street == undefined) {
            noty({ text: 'Informe a rua', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.branch.Address.StreetNo == '' || $scope.branch.Address.StreetNo == undefined) {
            noty({ text: 'Informe o número da residência', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.branch.Address.Block == '' || $scope.branch.Address.Block == undefined) {
            noty({ text: 'Informe o bairro', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.branch.Address.City == '' || $scope.branch.Address.City == undefined) {
            noty({ text: 'Informe a cidade', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.branch.address != null) {
            if ($scope.branch.Address.Uf.Id == '' || $scope.branch.Address.Uf.Id == undefined) {
                noty({ text: 'Selecione o estado', layout: 'topRight', type: 'error' });
                return false;
            }
        }

        return true;
    }

    $('#branch-politicExpense').on('ifClicked', function (event) {
        var checked = $("#branch-politicExpense").parent('[class*="icheckbox"]').hasClass("checked");
        if (!checked)
            $scope.branch.UseMasterPoliticExpense = 1;
        else
            $scope.branch.UseMasterPoliticExpense = 0;
    });

    $('#branch-closeOnLastDay').on('ifClicked', function (event) {
        var checked = $("#branch-closeOnLastDay").parent('[class*="icheckbox"]').hasClass("checked");
        if (!checked) {
            $scope.branch.CloseOnLastDay = 1;
            $scope.branch.FinishDay_Period = 0;
        }
        else {
            $scope.branch.CloseOnLastDay = 0;
            $scope.branch.FinishDay_Period = 1;
        }
    });

    $scope.AddExpenseLines = function () {
        var model = {};
        var valor = $scope.branch.PoliticExpense.Value;
        var tipo = $scope.branch.PoliticExpense.Id;

        if (tipo == null || tipo == undefined || tipo == '') {
            noty({ text: 'Informe o tipo de despesa', layout: 'topRight', type: 'error' });
            return false;
        }
        if (valor == null || valor == undefined || valor == '') {
            noty({ text: 'Informe o valor', layout: 'topRight', type: 'error' });
            return false;
        }
        
        for (var i = 0; i < $scope.ExpenseTypeList.length; i++) {
            if ($scope.ExpenseTypeList[i].Id == $scope.branch.PoliticExpense.Id) {
                model.Name = $scope.ExpenseTypeList[i].Name;
                model.Id = $scope.branch.PoliticExpense.Id;
                model.Value = $scope.branch.PoliticExpense.Value;
            }
        }
        $scope.ExpenseSelectedList[$scope.ExpenseSelectedList.length] = model;
        model = {};
        $scope.branch.PoliticExpense = {};
    }

    $scope.RemoveExpenseLine = function (index) {
        $scope.ExpenseSelectedList.splice(index, 1);
    }

    $scope.EditBranch = function (id) {
        window.location.href = '/Filial/Editar?branchID=' + id;
    }
}]);