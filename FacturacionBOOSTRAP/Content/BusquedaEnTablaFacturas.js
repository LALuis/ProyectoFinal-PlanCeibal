$(document).ready(function () {
    $("#miPalabra").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#miTablaF tr").each(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});