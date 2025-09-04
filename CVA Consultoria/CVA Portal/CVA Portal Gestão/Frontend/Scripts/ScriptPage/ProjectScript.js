/*________________________ProjectScript.js______________________________
|                                                                       |
| Author: Giuliano Costa                                                |
| Date: 24/03/2016                                                      |
| Protected by: CVA Consultoria                                         |
|________________________ProjectScript.js_______________________________*/

/************************************************************************
| Method Name: LoadStatus                                               |
| Author: Giuliano Costa                                                |
| Creation Date: 24/03/2016 14:20                                       |
| Description:                                                          |
| Load the select box with specific status                              |
*************************************************************************/
$.getJSON("/Projeto/GetSpecificStatus", null, function (data) {
    $('#project-status option').remove();
    $('#project-status').append('<option value="0" selected="selected">Selecione</option>');

    for (var i = 0; i < data.length; i++) {
        $('#project-status').append('<option value="' + data[i].Id + '">' + data[i].Status + '</option>');
    }
});


/************************************************************************
| Method Name: SaveProject                                              |
| Author: Giuliano Costa                                                |
| Creation Date: 24/03/2016 14:20                                       |
| Description:                                                          |
| Send form data for controller using ajax                              |
*************************************************************************/
$('body').on('click', '#save-project', function () {
    var project = {};
});

/************************************************************************
| Method Name: ClearForm                                                |
| Author: Giuliano Costa                                                |
| Creation Date: 24/03/2016 14:20                                       |
| Description:                                                          |
| clear form                                                            |
*************************************************************************/
$('body').on('click', '#screen-clean', function () {
    $('.project').val('');
});
