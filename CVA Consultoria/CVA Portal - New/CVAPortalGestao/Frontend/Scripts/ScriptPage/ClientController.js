cvaGestao.controller('ClientController', ['$scope', '$http', function ($scope, $http) {
    $scope.LoadInit = function (model) {
        CreateModel();
        CreateArrays();
        LoadArrays();

        if (model != null) {
            $scope.client = model;

            console.log(model);

            $scope.client.Status = {};
            $scope.client.Status.Id = model.Status.Id;
            var expenses = [];
            if (model.PoliticExpense != null) {
                for (var i = 0; i < model.PoliticExpense.length; i++) {
                    expenses[i] = {
                        Name: model.PoliticExpense[i].Expense.Name,
                        Id: model.PoliticExpense[i].Expense.Id,
                        Value: model.PoliticExpense[i].Value
                    };
                }
            }

            $scope.ContactList = model.Contact;
            $scope.AddressList = model.Addresses;
            $scope.ExpenseSelectedList = expenses;
        }
    }

    function CreateModel() {
        $scope.client = {};
        $scope.client.Contact = {};
        $scope.client.Address = {};
        $scope.client.Address.Uf = {};
        $scope.client.Branch = {};
        $scope.client.LocalPoliticExpense = 0;
        $scope.client.Status = {};
        $scope.Contato = {};
        $scope.cepValid = false;
    }

    function CreateArrays() {
        $scope.ExpenseTypeList = [];
        $scope.ExpenseSelectedList = [];
        $scope.StatusList = [];
        $scope.UfList = [];
        $scope.ContactList = [];
        $scope.AddressList = [];

        $scope.ContactLine = -1;
    }

    function LoadArrays() {
        $http({
            method: 'GET',
            url: '/Cliente/GetSpecificStatus'
        }).success(function (result) {
            $scope.StatusList = result;
        });

     

        $http({
            method: 'GET',
            url: '/Cliente/GetUf'
        }).success(function (uf) {
            $scope.UfList = uf;
        });

        $http({
            method: 'GET',
            url: '/Cliente/GetExpenseTypes'
        }).success(function (result) {
            $scope.ExpenseTypeList = result;
        });
    }

    /*--- Contatos ---*/
    $scope.AddContactLines = function () {
        if ($scope.Contato.Nome == '' || $scope.Contato.Nome == null) {
            noty({ text: 'Informe o nome do contato', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Contato.Fone == '' || $scope.Contato.Fone == null) {
            noty({ text: 'Informe o telefone do contato', layout: 'topRight', type: 'error' });
            return false;
        }

        var index = $scope.ContactList.length;
        var obj = {};

        obj.Name = $scope.Contato.Nome;
        obj.Email = $scope.Contato.Email;
        obj.Phone = $scope.RemoveFormation($scope.Contato.Fone, false, false);
        obj.CellPhone = $scope.RemoveFormation($scope.Contato.Celular, false, false);

        if ($scope.ContactLine == -1)
            $scope.ContactList[index] = obj;
        else
            $scope.ContactList[$scope.ContactLine] = obj;

        $scope.ContactLine = -1;
        $scope.Contato = {};
    }

    $scope.EditContactLine = function (index) {
        $scope.Contato.Nome = $scope.ContactList[index].Name;
        $scope.Contato.Email = $scope.ContactList[index].Email;
        $scope.Contato.Fone = $scope.ContactList[index].Phone;
        $scope.Contato.Celular = $scope.ContactList[index].CellPhone;
        $scope.ContactLine = index;
    }

    $scope.RemoveContactLine = function (index) {
        $scope.ContactList.splice(index, 1);
    }


    /*--- Endereços ---*/
    $scope.AddAddressLines = function () {
        if (ValidateAddress()) {
            $scope.AddressList[$scope.AddressList.length] = $scope.client.Address;
            $scope.client.Address = {};
            $scope.client.Address.Uf = {};
        }
    }

    $scope.RemoveAddressLine = function (index) {
        $scope.AddressList.splice(index, 1);
    }

    function ValidateAddress() {
        if ($scope.client.Address == null) {
            noty({ text: 'Informe ao menos um endereço', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.client.Address.Street == '' || $scope.client.Address.Street == null) {
            noty({ text: 'Insira a rua', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.client.Address.StreetNo == '' || $scope.client.Address.StreetNo == null) {
            noty({ text: 'Insira o número do endereço', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.client.Address.Block == '' || $scope.client.Address.Block == null) {
            noty({ text: 'Insira o bairro', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.client.Address.City == '' || $scope.client.Address.City == null) {
            noty({ text: 'Insira a cidade', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.client.Address.Uf.Id == '' || $scope.client.Address.Uf.Id == null) {
            noty({ text: 'Selecione o estado', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.client.Address.ZipCode == '' || $scope.client.Address.ZipCode == null) {
            noty({ text: 'Insira o CEP', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }




    // Salva o cliente
    $scope.SaveClient = function () {
        if (ValidateFields()) {
            $scope.client.CNPJ = $scope.RemoveFormation($scope.client.CNPJ, true, false);
            $scope.client.Contact = $scope.ContactList;
            $scope.client.Addresses = $scope.AddressList;

            $scope.PoliticExpense = [];

            if ($scope.ExpenseSelectedList.length > 0) {
                if ($scope.client.LocalPoliticExpense == undefined || $scope.client.LocalPoliticExpense == 0) {
                    for (var i = 0; i < $scope.ExpenseSelectedList.length; i++) {
                        $scope.PoliticExpense[i] = {
                            Expense: { Id: $scope.ExpenseSelectedList[i].Id },
                            Value: $scope.ExpenseSelectedList[i].Value.replace(',', '.')
                        };
                    }
                }
            }


            $scope.client.PoliticExpense = $scope.PoliticExpense;

            $scope.IsProcessing = true;
            $http.post('/Cliente/Salvar', $scope.client)
                .success(function (message) {
                    if (message.Success == null || message.Success == undefined) {
                        $('#message-error').text('');
                        $('#message-error').append(message.Error.Message);
                        $('#message-box-danger').addClass('open');
                    }
                    else {
                        $('#message-box-success').addClass('open');
                        $scope.client = {};
                        $scope.ContactList = [];
                        $scope.AddressList = [];
                    }
                    $scope.IsProcessing = false;
                });
        }
    }

    // Remove a formatação dos campos
    $scope.RemoveFormation = function (value, isCnpj, isMoney) {
        if (isCnpj) {
            var cnpj = value.replace('.', '').replace(',', '').replace('/', '').replace('-', '');
            return cnpj;
        }

        //if (isMoney) {
        //    var valor = value.replace(',', '.');
        //    return valor;
        //}
        var ph = value.replace('(', '').replace(')', '').replace('-', '').replace(' ', '');
        return ph;
    }

    // Realiza a validação dos campos
    function ValidateFields() {
        if ($scope.client.Name == '' || $scope.client.Name == null) {
            noty({ text: 'Informe a razão social', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.client.FantasyName == '' || $scope.client.FantasyName == null) {
            noty({ text: 'Informe o nome fantasia da empresa', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.client.Status.Id == '' || $scope.client.Status.Id == null) {
            noty({ text: 'Selecione o status do cliente', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.client.CNPJ == '' || $scope.client.CNPJ == null) {
            noty({ text: 'Informe o CNPJ', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.ContactList.length == 0) {
            noty({ text: 'Informe ao menos um contato', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.AddressList.length == 0) {
            noty({ text: 'Informe ao menos um endereço', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.client.Tag.length >  18) {
            noty({ text: 'Campo TAG não pode exceder o tamanho máximo de 18 caractéres', layout: 'topRight', type: 'error' });
            return false;
        }
        if (!$("#client-politicExpense").is(":checked") && $scope.ExpenseSelectedList.length == 0) {
            noty({ text: 'Informe qual é a política de despesa para este cliente', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }

    // Realiza a busca de informações conforme o CEP digitado
    $scope.SearchCep = function () {
        var zipCode = $scope.client.Address.ZipCode;
        zipCode = zipCode.replace('.', '').replace('-', '');
        if (zipCode.length == 8) {
            $http({
                method: 'GET',
                url: 'http://viacep.com.br/ws/' + zipCode + '/json/'
            }).success(function (address) {
                if (address.logradouro == undefined) {
                    $scope.cepValid = true;
                    $scope.client.Address.Street = '';
                    $scope.client.Address.Block = '';
                    $scope.client.Address.City = '';
                    $scope.client.Address.Uf = '';
                    return;
                } else {
                    $scope.cepValid = false;

                    $scope.client.Address.Street = address.logradouro;
                    $scope.client.Address.Block = address.bairro;
                    $scope.client.Address.City = address.localidade;

                    for (var i = 0; i < $scope.UfList.length; i++) {
                        if ($scope.UfList[i].Uf == address.uf) {
                            $scope.client.Address.Uf = $scope.UfList[i];
                            break;
                        }
                    }
                }
            });
        }
    }

   


    $scope.AddExpenseLines = function () {
        if ($scope.client.PoliticExpense == undefined || $scope.client.PoliticExpense.Id == null || $scope.client.PoliticExpense.Id == '') {
            noty({ text: 'Informe o tipo de despesa', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.client.PoliticExpense == undefined || $scope.client.PoliticExpense.Coast == null || $scope.client.PoliticExpense.Coast == '') {
            noty({ text: 'Informe o valor máximo de reembolso', layout: 'topRight', type: 'error' });
            return false;
        }

        if ($scope.client.PoliticExpense == undefined || $scope.client.PoliticExpense.Coast == null || $scope.client.PoliticExpense.Coast == '') {
            noty({ text: 'Informe o valor máximo de reembolso', layout: 'topRight', type: 'error' });
            return false;
        }

        for (var i = 0; i < $scope.ExpenseSelectedList.length; i++) {
            if ($scope.client.PoliticExpense.Id == $scope.ExpenseSelectedList[i].Id) {
                noty({ text: 'Este tipo de despesa já foi inserido', layout: 'topRight', type: 'error' });
                return false;
            }
        }

        var model = {};
        for (var i = 0; i < $scope.ExpenseTypeList.length; i++) {
            if ($scope.ExpenseTypeList[i].Id == $scope.client.PoliticExpense.Id) {
                model.Name = $scope.ExpenseTypeList[i].Name;
                model.Id = $scope.client.PoliticExpense.Id;
                var valor = $scope.client.PoliticExpense.Coast;
                model.Value = $scope.RemoveFormation(valor, false, true);
            }
        }

        $scope.ExpenseSelectedList[$scope.ExpenseSelectedList.length] = model;
        model = {};
        $scope.client.PoliticExpense = {};
    }

    $scope.RemoveExpenseLine = function (index) {
        $scope.ExpenseSelectedList.splice(index, 1);
    }

    $scope.EditClient = function (id) {
        window.location.href = '/Cliente/Editar?clienteId=' + id;
    }

    $('#client-politicExpense').on('ifClicked', function (event) {
        var checked = $("#client-politicExpense").parent('[class*="icheckbox"]').hasClass("checked");
        if (!checked) {
            $("#bt_AddExpense").attr("disabled", "disabled");
            $scope.client.LocalPoliticExpense = 1;
        }
        else {
            $("#bt_AddExpense").removeAttr("disabled");
            $scope.client.LocalPoliticExpense = 0;
        }
    });
}]);