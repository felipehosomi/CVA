$(document).ready(function () {
    var tipo = $("#ddlTipo").val();
    if (tipo == "G") {
        $("#divItem").css("display", "none");
        $("#divGrupo").css("display", "block");
    } else if (tipo == "I") {
        $("#divItem").css("display", "block");
        $("#divGrupo").css("display", "none");
    }

    $("#ddlTipo").change(function () {
        var tipo = $("#ddlTipo").val();
        if (tipo == "G") {
            $("#divItem").css("display", "none");
            $("#divGrupo").css("display", "block");
        } else {
            $("#divItem").css("display", "block");
            $("#divGrupo").css("display", "none");
        } 
    });
});