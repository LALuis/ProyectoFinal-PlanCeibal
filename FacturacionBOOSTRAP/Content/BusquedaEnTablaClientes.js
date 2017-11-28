$(document).ready(function () {
    $("#miBusqueda").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#miSeccion tr").each(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});