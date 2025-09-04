$(document).ready(function () {
    $('#dataTable').DataTable({
        info: false,
        paging: true,
        searching: false,
        ordering: true,
        pagingType: "numbers",
        "language": {
            "lengthMenu": "Exibir _MENU_ registros"
        }
    });
});