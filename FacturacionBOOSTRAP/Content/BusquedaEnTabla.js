$(document).ready(function () {
    $("#miEntrada").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#miTabla tr").each(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});