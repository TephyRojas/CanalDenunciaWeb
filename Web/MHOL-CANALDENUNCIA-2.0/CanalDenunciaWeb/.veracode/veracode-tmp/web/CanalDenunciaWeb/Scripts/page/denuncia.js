(function () {

    $(document).on('click', 'form .prevent-default, input:submit .prevent-default, button:submit .prevent-default', function (e) {
        e.preventDefault();
    });

   

})();

$(document).ready(function () {

   
    /* fecha */
    $('input[name="filtroFecha"]').daterangepicker({
        timePicker: false,
        locale: {
            "format": 'DD/MM/YYYY',
            "separator": " - ",
            "applyLabel": "Seleccionar",
            "cancelLabel": "Cancelar",
            "fromLabel": "Desde",
            "toLabel": "Hasta",
            "customRangeLabel": "Custom",
            "weekLabel": "W",
            "daysOfWeek": [
                "Do",
                "Lu",
                "Ma",
                "Mi",
                "Ju",
                "Vi",
                "Sa"
            ],
            "monthNames": [
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Setiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            ],
            "firstDay": 1,
        }
    });
    $('input[name="filtroFecha"]').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
    });
  
});

function AbrirModal(id) {
    const url = BASE_PATH + 'Denuncia/DatosPersonales?id=' + id;
    $.ajax({
        url: url,
        data: {},
        contentType: false,
        processData: false,
        type: 'GET',
        success: function (response) {
            if (response.respuesta === undefined) {
                $('#DatosContainer').html(response);
                $('#Modal').modal('show');
            } else {
                swal.fire({
                    position: 'top-end',
                    title: 'Error',
                    text: response.errorMessage,
                    icon: 'error',
                    showConfirmButton: false,
                    timer: 2000
                })
            }
        },
        error: function (response, b, c) {

            swal.fire({
                position: 'top-end',
                title: 'Error',
                text: "Ha ocurrido un error inesperado, intente nuevamente. Si el error persiste comuníquese con el administrador",
                icon: 'error',
                showConfirmButton: false,
                timer: 2000
            })
        }
    });

};


function AbrirModalAnonimo() {
    const url = BASE_PATH + 'Login/NuevaDenunciaAnonimo';
    $.ajax({
        url: url,
        data: {},
        contentType: false,
        processData: false,
        type: 'GET',
        success: function (response) {
            if (response.respuesta === undefined) {                
                //var Response = Microsoft.Security.Application.AntiXss.JavaScriptEncode(response.redirect);
                $('#CorreoContainer').html(response);
                $('#ModalAnonimo').modal('show');
            } else {
                swal.fire({
                    position: 'top-end',
                    title: 'Error',
                    text: response.errorMessage,
                    icon: 'error',
                    showConfirmButton: false,
                    timer: 2000
                })
            }
        },
        error: function (response, b, c) {

            swal.fire({
                position: 'top-end',
                title: 'Error',
                text: "Ha ocurrido un error inesperado, intente nuevamente. Si el error persiste comuníquese con el administrador",
                icon: 'error',
                showConfirmButton: false,
                timer: 2000
            })
        }
    });

};

function Filtrar() {
    const denuncia = $('#filtroDenuncia').val();
    const fecha = $('#filtroFecha').val();
    const fechaO = $('#filtroFechaO').val() == null ? "" : $('#filtroFechaO').val();
    const sede = $('#filtroSede').val();
    const departamento = $('#filtroDepartamento').val();
    const tipoDelito = $('#filtroTipoDelito').val();
    const estado = $('#filtroEstado').val();
    const ruta = `/Denuncia/IndexFiltro?Denuncia=${denuncia}&FechaDenuncia=${fecha}&FechaOcurrencia=${fechaO}&IdSede=${sede}&IdDepartamento=${departamento}&IdTipoDelito=${tipoDelito}&IdEstado=${estado}`;
    window.location.assign(ruta);
}

function Exportar() {
    const denuncia = $('#filtroDenuncia').val();
    const fecha = $('#filtroFecha').val();
    const fechaO = $('#filtroFechaO').val() == null ? "" : $('#filtroFechaO').val();
    const sede = $('#filtroSede').val();
    const departamento = $('#filtroDepartamento').val();
    const tipoDelito = $('#filtroTipoDelito').val();
    const estado = $('#filtroEstado').val();
    const ruta = `/Denuncia/Exportar?Denuncia=${denuncia}&FechaDenuncia=${fecha}&FechaOcurrencia=${fechaO}&IdSede=${sede}&IdDepartamento=${departamento}&IdTipoDelito=${tipoDelito}&IdEstado=${estado}`;
    window.location.assign(ruta);
}

function Buscar() {
    const denuncia = $('#Denuncia').val();
    const ruta = `/login/BuscarDenuncia?Denuncia=${denuncia}`;
    window.location.assign(ruta);
}

$('#btnBuscar').click(function (e) {
    e.preventDefault();
    const url = BASE_PATH + 'login/BuscarDenuncia';
    let datosFormulario = {
        Denuncia: $("#Denuncia").val(),
    }
    
    Denuncia: $("#Denuncia").val();
    swal.fire({
        title: '¿Est&aacute;s seguro de buscar la denuncia?',
        text: '',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí',
        cancelButtonText: 'Cancelar'
    }).then(function (result) {
        if (result.isConfirmed) {

            $.ajax({
                url: url,
                data: JSON.stringify(datosFormulario),
                contentType: "application/json; charset=utf-8",
                processData: false,
                type: 'POST',
                success: function (response) {
                    if (response.respuesta) {

                        $('#validationSummary').hide();
                        $('.form-group').attr("class", "form-group");

                        displayErrors(response.errorControl);

                        swal.fire({
                            position: 'top-end',
                            title: 'Éxito',
                            text: response.errorMessage,
                            icon: 'success',
                            showCancelButton: false,
                            confirmButtonColor: '#3085d6',
                            cancelButtonColor: '#d33',
                            confirmButtonText: 'Ok',
                            showConfirmButton: false,
                            timer: 2000
                        }).then(function () {
                            window.location.href = response.redirect;
                        })

                    }
                    else {

                        $('#validationSummary').show();

                        displaySummaryErrors(response.errorSumary);
                        displayErrors(response.errorControl);

                        swal.fire({
                            position: 'top-end',
                            title: 'Errorsss',
                            text: response.errorMessage,
                            icon: 'error',
                            showConfirmButton: false,
                            timer: 2000
                        })
                    }
                },
                error: function (response, b, c) {

                    swal.fire({
                        position: 'top-end',
                        title: 'Error',
                        text: "Ha ocurrido un error inesperado, intente nuevamente. Si el error persiste comuníquese con el administrador",
                        icon: 'error',
                        showConfirmButton: false,
                        timer: 2000
                    })
                }
            });
        }
    });


});

function AbrirDenuncia(id) {
    const ruta = `/Denuncia/DatosInvolucrados?idDenuncia=${id}`;
    window.location.assign(ruta);
}

function AbrirInvolucrados(id) {

    const ruta = "/denuncia/AgregarInvolucrados?idDenuncia=" + id;
    window.location.assign(ruta);
}

/*=======================*/
/* FUNCIONES INAMOVIBLES */
/*=======================*/

function displayErrors(errors) {
    $.each(errors, function (idx, control) {
        var arr = control.split('|');
        var msj = '#msj' + arr[0];
        var obj = '#' + arr[0];
        var formGroup = '#frm' + arr[0];

        if (arr[1] != "NOERROR") {
            $(msj).text(arr[1]);
            $(formGroup).attr("class", "form-group has-danger");
            $(obj).attr("class", "form-control is-invalid")
        }
        else {
            $(obj).attr("class", "form-control")
            $(formGroup).attr("class", "form-group");
        }
    });
}

function displaySummaryErrors(errors) {
    var $ul = $('div.validation-summary-valid.text-danger > ul');
    $ul.empty();
    $.each(errors, function (idx, errorMessage) {
        $ul.append('<li>' + errorMessage + '</li>');
    });
}
