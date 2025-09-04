cvaGestao.controller('CalendarController', ['$scope', '$http', function ($scope, $http) {

    $scope.InitValue = function (model) {
        if (model != null) {
            $http({
                method: 'GET',
                url: '/Calendario/GetSpecificStatus'
            }).success(function (result) {
                $scope.StatusList = result;
            });

            model.InitialDate = new Date(model.InitialDate);
            model.FinishDate = new Date(model.FinishDate);

            $scope.calendar = model;

            if (model.Holidays != null) {
                $scope.HolidaysList = [];
                $scope.HolidaysList = model.Holidays;
            }
        }
    }

    $scope.calendar = {};
    $scope.calendar.Status = {};
    LoadInitialDates();
    $scope.StatusList = [];
    $scope.calendar.StatusSelectedDescription = '';
    $scope.HolidaysList = [];

    $http({
        method: 'GET',
        url: '/Calendario/GetSpecificStatus'
    }).success(function (result) {
        $scope.StatusList = result;
    });

    $scope.ClearForm = function () {
        $scope.calendar = {};
        LoadInitialDates();
        $scope.HolidaysList = [];
    }

    $scope.StatusSelected = function () {
        for (var i = 0; i < $scope.StatusList.length; i++) {
            if ($scope.StatusList[i].Id == $scope.calendar.StatusId) {
                $scope.calendar.StatusSelectedDescription = $scope.StatusList[i].Status;
                break;
            } else {
                $scope.calendar.StatusSelectedDescription = '';
            }
        }
    }

    $scope.AddLines = function () {
        if (ValideDate()) {
            $scope.calendar.Holiday.Date = convertDate($scope.calendar.Holiday.Date);
            $scope.HolidaysList[$scope.HolidaysList.length] = $scope.calendar.Holiday;
            $scope.calendar.Holiday = {};
        }
    }

    $scope.RemoveLine = function (index) {
        $scope.HolidaysList.splice(index, 1);
    }

    $scope.SaveCalendar = function () {
        if (ValidateFields()) {
            $scope.IsProcessing = true;
            $scope.calendar.Holidays = $scope.HolidaysList;
            $http.post('/Calendario/Salvar', $scope.calendar)
                    .success(function (message) {
                        if (message.Success == null || message.Success == undefined) {
                            $('#message-error').text('');
                            $('#message-error').append(message.Error.Message);
                            $('#message-box-danger').addClass('open');
                        }
                        else {
                            $('#message-box-success').addClass('open');
                            $scope.calendar = {};
                            LoadInitialDates();
                            $scope.HolidaysList = [];
                        }
                        $scope.IsProcessing = false;
                    });
        }
    }

    $scope.EditCalendar = function (id) {
        window.location.href = '/Calendario/Editar?calendarID=' + id;
    }

    function ValidateFields() {
        if ($scope.calendar.Name == '' || $scope.calendar.Name == undefined) {
            noty({ text: 'Informe o nome do calendário', layout: 'topRight', type: 'error' });
            return false;
        }
        console.log($scope.calendar.Status.Id);
        if ($scope.calendar.Status.Id == '' || $scope.calendar.Status.Id == null || $scope.calendar.Status.Id == undefined) {
            noty({ text: 'Selecione o status do calendário', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.calendar.InitialDate == '' || $scope.calendar.InitialDate == null || $scope.calendar.InitialDate == undefined) {
            noty({ text: 'Informe a data inicial', layout: 'topRight', type: 'error' });
            return false;
        }
        if ($scope.calendar.FinishDate == '' || $scope.calendar.FinishDate == null || $scope.calendar.FinishDate == undefined) {
            noty({ text: 'Informe a data final', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }


    function LoadInitialDates(){
        $scope.calendar.InitialDate = new Date();
        $scope.calendar.FinishDate = new Date(new Date().getFullYear(), 11, 31);
    }

    function ValideDate() {
        if ($scope.calendar.InitialDate > $scope.calendar.Holiday.Date || $scope.calendar.FinishDate < $scope.calendar.Holiday.Date) {
            noty({ text: 'O feriado deve estar dentro do ano corrente do calendário', layout: 'topRight', type: 'error' });
            return false;
        }
        return true;
    }

    function convertDate(inputFormat) {
        function pad(s) { return (s < 10) ? '0' + s : s; }
        var d = new Date(inputFormat);
        return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('/');
    }
}]);