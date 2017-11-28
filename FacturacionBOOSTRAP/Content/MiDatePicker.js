/*Formato del datepicker con fecha que usamos en Uruguay*/
$.fn.datepicker.defaults.format = "dd/mm/yyyy";
$('.datepicker').datepicker({
    startDate: '-3d'
});

/*Activacion del DatePicker*/
$(document).ready(function () {
    $('#fecha').datepicker();
});