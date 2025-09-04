cvaGestao.controller('CollaboratorController', ['$scope', '$http', function ($scope, $http) {

    $scope.OnLoad = function (model) {
        $scope.collaborator = {};
        $scope.collaborator.Endereco = {};
        $scope.collaborator.Endereco.Uf = {};
        $scope.collaborator.Especialidades = [];
        $('#collaborator-CNPJ').prop('disabled', true);

        CreateArrays();
        LoadArrays();


        if (model != null) {
            $scope.collaborator = model;
            $scope.collaborator.Status = {};
            $scope.collaborator.Status = model.Status;
            $scope.collaborator.EmissaoRG = new Date(model.EmissaoRG);
            $scope.collaborator.DataNascimento = new Date(model.DataNascimento);
            $scope.collaborator.ValidadePassaporte = new Date(model.ValidadePassaporte);

            $scope.collaborator.Endereco = model.Endereco;
            $scope.Especialidades = model.Especialidades;

            if ($scope.collaborator.GerenciaProjetos == 1)
                document.getElementById("collaborator-gp").checked = true;
            else
                document.getElementById("collaborator-gp").checked = false;
        }
    }

    function CreateArrays() {
        $scope.SpecialtyLine = -1;
        $scope.Especialidade = {};
        $scope.StatusList = [];
        $scope.UfList = [];
        $scope.MaritalList = [];
        $scope.GenreList = [];
        $scope.SpecialtyList = [];
        $scope.Especialidades = [];
        $scope.TypeList = [];
    }

    function LoadArrays() {
        $http({
            method: 'GET',
            url: '/Colaborador/GetTypes'
        }).success(function (result) {
            $scope.TypeList = result;
        });

        $http({
            method: 'GET',
            url: '/Colaborador/GetSpecificStatus'
        }).success(function (result) {
            $scope.StatusList = result;
        });

        $http({
            method: 'GET',
            url: '/Colaborador/GetEstadoCivil'
        }).success(function (result) {
            $scope.MaritalList = result;
        });

        $http({
            method: 'GET',
            url: '/Colaborador/GetGeneros'
        }).success(function (result) {
            $scope.GenreList = result;
        });

        $http({
            method: 'GET',
            url: '/Colaborador/GetUf'
        }).success(function (result) {
            $scope.UfList = result;
        });

        $http({
            method: 'GET',
            url: '/Especialidade/Get_All'
        }).success(function (result) {
            $scope.SpecialtyList = result;
        });
    }


    $scope.Save = function () {
        if ($scope.ValidateModel()) {
            $scope.collaborator.Especialidades = $scope.Especialidades;
            $scope.collaborator.Status = {};
            $scope.collaborator.Status.Id = $scope.collaborator.StatusId;
            if ($scope.collaborator.Telefone != null || $scope.collaborator.Telefone != undefined)
                $scope.collaborator.Telefone = $scope.RemoveFormation($scope.collaborator.Telefone, false, false);
            $scope.collaborator.Celular = $scope.RemoveFormation($scope.collaborator.Celular, false, false);
            if (document.getElementById("collaborator-gp").checked == true)
                $scope.collaborator.GerenciaProjetos = 1;
            else
                $scope.collaborator.GerenciaProjetos = 0;
            if ($scope.collaborator.Id == null)
                $http.post('/Colaborador/Insert', $scope.collaborator)
                    .success(function (message) {
                        if (message.Success == null || message.Success == undefined) {
                            $('#message-error').text('');
                            $('#message-error').append(message.Error.Message);
                            $('#message-box-danger').addClass('open');
                        }
                        else {
                            $('#message-success').text('');
                            $('#message-success').append(message.Success.Message);
                            $('#message-box-success').addClass('open');
                            $scope.collaborator = {};
                            $scope.Especialidades = [];
                        }
                    });
            else
                $http.post('/Colaborador/Update', $scope.collaborator)
                    .success(function (message) {
                        if (message.Success == null || message.Success == undefined) {
                            $('#message-error').text('');
                            $('#message-error').append(message.Error.Message);
                            $('#message-box-danger').addClass('open');
                        }
                        else {
                            $('#message-success').text('');
                            $('#message-success').append(message.Success.Message);
                            $('#message-box-success').addClass('open');
                            $scope.collaborator = {};
                            $scope.Especialidades = [];
                        }
                    });
        }
    }

    $scope.ValidateModel = function () {
        if ($scope.collaborator == undefined) {
            noty({ text: 'Obrigatório o preenchimento do formulário', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.Nome == null || $scope.collaborator.Nome == '') {
            noty({ text: 'Informe o nome do colaborador', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.StatusId == null || $scope.collaborator.StatusId == '') {
            noty({ text: 'Selecione um status para o cadastro', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.DataNascimento == null || $scope.collaborator.DataNascimento == '') {
            noty({ text: 'Informe a data de nascimento', layout: 'topRight', type: 'error' });
            return false;
        }
        if ((new Date().getFullYear() - $scope.collaborator.DataNascimento.getFullYear()) < 18) {
            noty({ text: 'A idade precisa ser igual ou superior a 18 anos', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.Email == '' || $scope.collaborator.Email == null) {
            noty({ text: 'Informe o e-mail do colaborador', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.EstadoCivil == undefined || $scope.collaborator.EstadoCivil == '') {
            noty({ text: 'Informe o estado civíl', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.Genero == undefined || $scope.collaborator.Genero == '') {
            noty({ text: 'Informe o gênero', layout: 'topRight', type: 'error' });
            $('#collaborator-genre').focus();
            return false;
        }
        if ($scope.collaborator.Celular == '' || $scope.collaborator.Celular == undefined) {
            noty({ text: 'Informe o telefone celular', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.RG == '' || $scope.collaborator.RG == undefined) {
            noty({ text: 'Informe o RG', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.CPF == '' || $scope.collaborator.CPF == undefined) {
            noty({ text: 'Informe o CPF', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.Tipo == null || $scope.collaborator.Tipo.Id == null || $scope.collaborator.Tipo.Id == '') {
            noty({ text: 'Selecione o tipo do colaborador', layout: 'topRight', type: 'error' });
            return false;
        }
        if (!$('#collaborator-CNPJ').prop('disabled') && ($scope.collaborator.CNPJ == undefined || $scope.collaborator.CNPJ == '' || $scope.collaborator.CNPJ == null)) {
            noty({ text: 'Informe o CNPJ', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.OrgaoEmissor == '' || $scope.collaborator.OrgaoEmissor == undefined) {
            noty({ text: 'Informe o orgão emissor', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.Nacionalidade == '' || $scope.collaborator.Nacionalidade == undefined) {
            noty({ text: 'Informe a nacionalidade', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.Naturalidade == '' || $scope.collaborator.Naturalidade == undefined) {
            noty({ text: 'Informe a naturalidade', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.Endereco.ZipCode == '' || $scope.collaborator.Endereco.ZipCode == undefined) {
            noty({ text: 'Informe o CEP', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.Endereco.Street == '' || $scope.collaborator.Endereco.Street == undefined) {
            noty({ text: 'Informe a rua', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.Endereco.StreetNo == '' || $scope.collaborator.Endereco.StreetNo == undefined) {
            noty({ text: 'Informe o número da residência', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.Endereco.Block == '' || $scope.collaborator.Endereco.Block == undefined) {
            noty({ text: 'Informe o bairro', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.Endereco.City == '' || $scope.collaborator.Endereco.City == undefined) {
            noty({ text: 'Informe a cidade', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.collaborator.Endereco.Uf_Id == null || $scope.collaborator.Endereco.Uf_Id == '') {
            noty({ text: 'Selecione o estado', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Especialidades.length == 0) {
            noty({ text: 'Selecione uma especialidade', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }


    /*--Especialidades--*/
    $scope.AddSpecialty = function () {
        if ($scope.Especialidade.Id == null || $scope.Especialidade.Id == '') {
            noty({ text: 'Informe a especialidade', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.Especialidade.Value == null || $scope.Especialidade.Value == '') {
            noty({ text: 'Informe o valor pago por hora', layout: 'topRight', type: 'error' });
            return false;
        }

        var line = {};

        for (var i = 0; i < $scope.SpecialtyList.length; i++) {
            if ($scope.SpecialtyList[i].Id == $scope.Especialidade.Id) {

                line.Id = $scope.Especialidade.Id;
                line.Name = $scope.SpecialtyList[i].Name;
                line.Value = $scope.Especialidade.Value;
                break;
            }
        }

        var index = $scope.Especialidades.length;

        if ($scope.SpecialtyLine == -1)
            $scope.Especialidades[index] = line;
        else
            $scope.Especialidades[$scope.SpecialtyLine] = line;

        $scope.SpecialtyLine = -1;
        line = {};
        $scope.Especialidade = {};
        document.getElementById("Especialidade").removeAttribute("disabled");
    }

    $scope.EditSpecialty = function (index) {
        $scope.SpecialtyLine = index;
        $scope.Especialidade = {};
        $scope.Especialidade.Id = $scope.Especialidades[index].Id;
        $scope.Especialidade.Value = $scope.Especialidades[index].Value;
        document.getElementById("Especialidade").setAttribute("disabled", "disabled");
    }

    $scope.RemoveSpecialty = function (index) {

        if ($scope.collaborator.Id != undefined || $scope.collaborator.Id != null) {
            //inputando o id do colaborador no usuario pois não foi possivel passar multiplos parametros via post
            $scope.Especialidades[index].User = $scope.collaborator.Id;

            $http.post('/Colaborador/Remove_Specialty', $scope.Especialidades[index]).success(function (result) {
                if (result.Success == null || result.Success == undefined) {
                    $('#message-error').text('');
                    $('#message-error').append(result.Error.Message);
                    $('#message-box-danger').addClass('open');
                }
                else {
                    $scope.Especialidades.splice(index, 1);
                }
            });
        }
        else
            $scope.Especialidades.splice(index, 1);
    }

    $scope.SpecialtySelected = function () {
        for (var i = 0; i < $scope.SpecialtyList.length; i++) {
            if ($scope.SpecialtyList[i].Id == $scope.Especialidade.Id) {

                $scope.Especialidade.Value = $scope.SpecialtyList[i].Value;
                break;
            }
        }
    }


    /*--Auxiliares--*/
    $('#collaborator-gp').on('ifClicked', function (event) {

        var checked = $("#collaborator-gp").hasClass("checked");
        if (!checked)
            $scope.collaborator.GerenciaProjetos = 1;
        else
            $scope.collaborator.GerenciaProjetos = 0;
    });

    $scope.UploadFile = function () {
        $("#btnImportar").button('loading');
        var formData = new FormData();
        var totalFiles = document.getElementById("tbxExcel").files.length;
        for (var i = 0; i < totalFiles; i++) {
            var file = document.getElementById("tbxExcel").files[i];
            formData.append("FileUpload", file);
        }

        $.ajax({
            url: '/Colaborador/UploadExcel',
            type: 'POST',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (retorno) {
                if (retorno == "") {
                    $scope.ImportarDadosColaborador();
                }
                else {
                    noty({ text: retorno, layout: 'topRight', type: 'error' });
                }
            }
        });
        $("#btnImportar").button('reset');
    };

    $scope.ImportarDadosColaborador = function () {
        $http({
            method: 'GET',
            url: '/Colaborador/ImportarDadosColaborador'
        }).success(function (result) {
            $scope.collaborator.StatusId = 1;
            $scope.collaborator.Nome = result.Nome;
            $scope.collaborator.DataNascimento = new Date(result.DataNascimento);
            $scope.collaborator.Genero = result.Genero;
            $scope.collaborator.EstadoCivil = result.EstadoCivil;
            $scope.collaborator.Telefone = result.Telefone;
            $scope.collaborator.Celular = result.Celular;
            $scope.collaborator.Email = result.Email;

            $scope.collaborator.Endereco = {};
            $scope.collaborator.Endereco.ZipCode = result.Endereco.ZipCode.replace('.', '').replace('-', '');

            $scope.collaborator.CPF = result.CPF;
            $scope.collaborator.RG = result.RG;
            $scope.collaborator.OrgaoEmissor = result.OrgaoEmissor;
            $scope.collaborator.ValidadePassaporte = new Date(result.ValidadePassaporte);
            $scope.collaborator.Tipo = result.Tipo;
            $scope.collaborator.CNPJ = result.CNPJ;
            $scope.collaborator.EmissaoRG = new Date(result.EmissaoRG);
            $scope.collaborator.Passaporte = result.Passaporte;
            $scope.collaborator.Naturalidade = result.Naturalidade;
            $scope.collaborator.Nacionalidade = result.Nacionalidade;
            for (var i = 0; i < result.Especialidades.length; i++) {
                for (var j = 0; j < $scope.SpecialtyList.length; j++) {
                    if (result.Especialidades[i].Name == $scope.SpecialtyList[j].Name) {
                        result.Especialidades[i].Value = $scope.SpecialtyList[j].Value;
                        break;
                    }
                }
            }
            $scope.Especialidades = result.Especialidades;

            $scope.SearchCep();
            $('#modal-importar').modal('hide');
        });
    };

    $scope.SearchCep = function () {
        if ($scope.collaborator.Endereco.ZipCode.length == 8) {
            $http({
                method: 'GET',
                url: 'http://viacep.com.br/ws/' + $scope.collaborator.Endereco.ZipCode + '/json/'
            }).success(function (result) {
                if (result.logradouro == undefined) {
                    return;
                }

                $scope.collaborator.Endereco.Street = result.logradouro;
                $scope.collaborator.Endereco.Block = result.bairro;
                $scope.collaborator.Endereco.City = result.localidade;

                for (var i = 0; i < $scope.UfList.length; i++) {
                    if ($scope.UfList[i].Uf == result.uf) {
                        $scope.collaborator.Endereco.Uf_Id = $scope.UfList[i].Id;
                        break;
                    }
                }
            });
        }
    }

    $scope.VerifyType = function () {
        if ($scope.collaborator.Tipo.Id != null) {
            for (var i = 0; i < $scope.TypeList.length; i++) {
                if ($scope.TypeList[i].Id == $scope.collaborator.Tipo.Id) {
                    if ($scope.TypeList[i].CnpjRequired == 1) {
                        $('#collaborator-CNPJ').prop('disabled', false);
                        $scope.collaborator.Tipo.CnpjRequired = 1;
                        break;
                    } else {
                        $('#collaborator-CNPJ').prop('disabled', true);
                        $scope.collaborator.Tipo.CnpjRequired = 0;
                        break;
                    }
                }
            }
        } else
            $('#collaborator-CNPJ').prop('disabled', true);
    }
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



    $scope.EditCollaborator = function (id) {
        window.location.href = '/Colaborador/Editar?colaboradorId=' + id;
    }
}]);