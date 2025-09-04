cvaGestao.controller('NoteExtractedController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {

    $scope.LoadUserView = function () {
        $scope.UserView = true;
    }

    $scope.TotalHours = 0;
    $scope.ProjectList = [];
    $scope.CollaboratorList = [];
    $scope.TableList = [];
    $scope.hideCollumn = false;

    // Preenche o campo "Cliente"
    $http({
        method: 'GET',
        url: '/Cliente/LoadCombo'
    }).success(function (result) {
        $scope.ClientList = result;
        if ($scope.ClientList != null)
            document.getElementById('loading00').hidden = true;
    });

    // Preenche o campo "Projeto"
    $http({
        method: 'GET',
        url: '/Projeto/LoadCombo'
    }).success(function (result) {
        $scope.ProjectList = result;
        if ($scope.ProjectList != null)
            document.getElementById('loading01').hidden = true;
    });

    // Preenche o campo "Colaborador"
    $http({
        method: 'GET',
        url: '/Colaborador/LoadCombo'
    }).success(function (result) {
        $scope.CollaboratorList = result;
    });




    $scope.Search = function () {
        if (ValidateFields()) {
            $http({
                method: 'GET',
                url: '/Apontamento/Filtrar?user=' + 0 + '&initialDate=' + $filter('date')($scope.note.InitialDate, 'mediumDate') + '&finishDate=' + $filter('date')($scope.note.FinishDate, 'mediumDate') + '&projectId=' + $scope.note.ProjectId + '&clientID=' + $scope.note.ClientId + '&Chamado=' + $scope.Chamado + '&interno=true'
            }).success(function (result) {
                if (result.length == 0) {
                    $scope.TotalHours = '0:00';
                    $scope.TableList = null;
                } else {
                    $scope.TotalHours = result[0].TotalHours;
                    $scope.TableList = result;
                }
            });
        }
    }


    $scope.ExportarExcel = function () {
        window.location.href = '/Apontamento/RelatorioApontamento?user=' + $scope.note.Collaborator.Id + '&initialDate=' + $filter('date')($scope.note.InitialDate, 'mediumDate') + '&finishDate=' + $filter('date')($scope.note.FinishDate, 'mediumDate') + '&projectId=' + $scope.note.ProjectId + '&statusID=0' + '&type=Excel' + '&clientID=' + $scope.note.ClientId + '&interno=true';
    }
    $scope.ExportarPDF = function () {
        window.location.href = '/Apontamento/RelatorioApontamento?user=' + $scope.note.Collaborator.Id + '&initialDate=' + $filter('date')($scope.note.InitialDate, 'mediumDate') + '&finishDate=' + $filter('date')($scope.note.FinishDate, 'mediumDate') + '&projectId=' + $scope.note.ProjectId + '&statusID=0' + '&type=PDF' + '&clientID=' + $scope.note.ClientId + '&interno=true';
    }


    function createPDF(form, a4, cache_width) {
        getCanvas(form, a4, cache_width).then(function (canvas) {
            var
                img = canvas.toDataURL("image/png"),
                doc = new jsPDF({
                    unit: 'px',
                    format: 'a4'
                });
            doc.addImage(img, 'JPEG', 20, 20);
            doc.save('techumber-html-to-pdf.pdf');
            form.width(cache_width);
        });
    }

    function getCanvas(form, a4, cache_width) {
        form.width((a4[0] * 1.33333) - 80).css('max-width', 'none');
        return html2canvas(form, {
            imageTimeout: 2000,
            removeContainer: true
        });
    }

    function ValidateFields() {
        if (($("#initialDate").attr('class').indexOf('invalid') != -1) || ($("#finishDate").attr('class').indexOf('invalid') != -1)) {
            noty({ text: 'Informe uma data válida ou deixe o campo em branco', layout: 'topRight', type: 'error' });
            return false;
        }

        if ($scope.note == undefined)
            $scope.note = {};
        if ($scope.note.Collaborator == undefined)
            $scope.note.Collaborator = {};
        if ($scope.note.Collaborator.Id == undefined)
            $scope.note.Collaborator.Id = 0;
        if ($scope.note.InitialDate == undefined)
            $scope.note.InitialDate = null;
        if ($scope.note.FinishDate == undefined)
            $scope.note.FinishDate = null;
        if ($scope.note.ProjectId == undefined)
            $scope.note.ProjectId = 0;
        if ($scope.note.Client == undefined)
            $scope.note.Client = {};
        if ($scope.note.ClientId == undefined)
            $scope.note.ClientId = 0;
        if ($scope.Chamado == undefined)
            $scope.Chamado = 0;

        if ($scope.Chamado != undefined && $scope.Chamado != 0 && ($scope.note.ClientId == undefined || $scope.note.ClientId == 0)) {
            noty({ text: 'Por favor, informe o cliente', layout: 'topRight', type: 'error' });
            return false;
        }

        return true;
    }

    $scope.exportData = function () {
        var blob = new Blob([document.getElementById('exportable').innerHTML], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
        });
        saveAs(blob, "Relatorio_Apontamento.xls");
    };

    $scope.AdminNotes = function () {
        window.location.href = '/Apontamento/AdministrarApontamento';
    }
}]);