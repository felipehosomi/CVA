cvaGestao.controller('ProjectResourcesController', ['$scope', '$http', function ($scope, $http) {
    $scope.ProjectList = [];
    $scope.CollaboratorList = [];
    $scope.ResourcesList = [];
    $scope.project = {};
    $scope.project.Collaborators = [];
    $scope.collaborators = {};

    $http({
        method: 'GET',
        url: '/RecursosProjeto/GetProjetos'
    }).success(function (projetos) {
        $scope.ProjectList = projetos;
        document.getElementById('loading00').hidden = true;
    });

    $http({
        method: 'GET',
        url: '/RecursosProjeto/GetColaboradores?projectId=0'
    }).success(function (collaborator) {
        $scope.CollaboratorList = collaborator;
        document.getElementById('loading01').hidden = true;
    });

    $scope.CollaboratorSelected = function () {
        if ($scope.project.Id == null || $scope.project.Id == '' || $scope.project.Id == undefined) {
            $scope.ResourcesList = [];
            $http({
                method: 'GET',
                url: '/RecursosProjeto/GetProjetoPorColaborador?colaboradorId=' + $scope.collaborators.Id
            }).success(function (projetos) {
                if (projetos.length == 0 || projetos == null)
                    return;
                else {
                    var model = [];
                    for (var i = 0; i < projetos.length; i++) {
                        for (var f = 0; f < $scope.CollaboratorList.length; f++) {
                            if ($scope.CollaboratorList[f].Id == $scope.collaborators.Id) {//////////////
                                model = { Id: $scope.CollaboratorList[f].Id, Name: $scope.CollaboratorList[f].Name, ProjectCode: projetos[i].Code, ProjectId: projetos[i].Id };
                                $scope.ResourcesList.push(model);
                            }
                        }
                    }
                }
            });
        }
    }

    $scope.ProjectSelected = function () {
        $scope.ResourcesList = [];
        //if ($scope.collaborators.Id == null || $scope.collaborators.Id == '' || $scope.collaborators.Id == undefined) {
            if ($scope.project.Id == null || $scope.project.Id == '' || $scope.project.Id == undefined) {
                $http({
                    method: 'GET',
                    url: '/RecursosProjeto/GetColaboradores?projectId=0'
                }).success(function (collaborator) {
                    $scope.CollaboratorList = collaborator;
                });
            }
            else {
                $http({
                    method: 'GET',
                    url: '/RecursosProjeto/GetColaboradores?projectId=' + $scope.project.Id
                }).success(function (colaboradores) {
                    $scope.CollaboratorList = colaboradores;
                });

                var projectCode = {};
                for (var i = 0; i < $scope.ProjectList.length; i++) {
                    if ($scope.ProjectList[i].Id == $scope.project.Id) {
                        projectCode = $scope.ProjectList[i].Code;
                        break;
                    }
                }

                $http({
                    method: 'GET',
                    url: '/RecursosProjeto/GetColaboradoresPorProjeto?projectId=' + $scope.project.Id
                }).success(function (colaboradores) {
                    var col = colaboradores;
                    if (col.length == 0 || col == null)
                        return;
                    else {
                        $scope.ResourcesList = [];
                        var model = [];
                        for (var i = 0; i < colaboradores.length; i++) {
                            model = { Id: colaboradores[i].Id, Name: colaboradores[i].Name, ProjectCode: projectCode, ProjectId: $scope.project.Id };
                            $scope.ResourcesList.push(model);
                        }
                    }
                });
            }
        //}
    }

    $scope.AddLines = function () {
        if ($scope.project.Id == null || $scope.collaborators == null) {
            noty({ text: 'Informe um projeto e/ou um colaborador', layout: 'topRight', type: 'error' });
            return false;
        }
        var projectCode = {};
        for (var i = 0; i < $scope.ProjectList.length; i++) {
            if ($scope.ProjectList[i].Id == $scope.project.Id) {
                projectCode = $scope.ProjectList[i].Code;
                break;
            }
        }

        model = {};
        for (var i = 0; i < $scope.CollaboratorList.length; i++) {
            if ($scope.CollaboratorList[i].Id == $scope.collaborators.Id) {
                model = {
                    Id: $scope.CollaboratorList[i].Id,
                    Name: $scope.CollaboratorList[i].Name,
                    ProjectCode: projectCode,
                    ProjectId: $scope.project.Id
                };
            }
        }
        $scope.ResourcesList[$scope.ResourcesList.length] = model;
    }

    $scope.ClearForm = function () {
        $scope.ResourcesList = [];
        $scope.project = {};
        $scope.collaborators = {};
    }

    $scope.RemoveLine = function (index) {
        $scope.ResourcesList.splice(index, 1);
    }

    //$scope.RemoveLine = function (index) {
    //    var model = {
    //        id: $scope.ResourcesList[index].ProjectId,
    //        Collaborators: [
    //            { Id: $scope.ResourcesList[index].Id }
    //        ]
    //    };
    //    $http.post('/RecursosProjeto/Inactive', model)
    //              .success(function (message) {
    //                  if (message.Success == null || message.Success == undefined) {
    //                      noty({ text: 'Não foi possível remover o colaborador do projeto', layout: 'topRight', type: 'error' });
    //                      return false;
    //                  }
    //                  else {
    //                      noty({ text: 'Recurso removido do projeto', layout: 'topRight', type: 'success' });
    //                  }
    //              });

    //    $scope.ResourcesList.splice(index, 1);
    //    $scope.project = {};
    //    $scope.collaborators = {};
    //}

    $scope.Save = function () {

        $scope.IsProcessing = true;
        $scope.project.Collaborators = [];
        for (var i = 0; i < $scope.ResourcesList.length; i++) {
            $scope.project.Collaborators[$scope.project.Collaborators.length] = $scope.ResourcesList[i];
        }
        
        $http.post('/RecursosProjeto/Salvar', $scope.project)
                   .success(function (message) {
                       if (message.Success == null || message.Success == undefined) {
                           $('#message-error').text('');
                           $('#message-error').append(message.Error.Message);
                           $('#message-box-danger').addClass('open');
                       }
                       else {
                           $('#message-box-success').addClass('open');
                           $scope.ResourcesList = [];
                           $scope.project = {};
                           $scope.collaborators = {};
                       }
                       $scope.IsProcessing = false;
                   });

    }
}]);