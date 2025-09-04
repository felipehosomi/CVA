function setMenuClassSession() {
    var cssClass = $("#menu-left").attr("class");
    $.ajax
   ({
       url: '/Home/SetMenuClass',
       data: { "cssClass": cssClass },
       success: function (retorno) {
           
       }
   });
}