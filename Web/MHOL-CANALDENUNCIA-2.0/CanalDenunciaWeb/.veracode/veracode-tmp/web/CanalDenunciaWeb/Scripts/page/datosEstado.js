(function () {

    $(document).on('click', 'form .prevent-default, input:submit .prevent-default, button:submit .prevent-default', function (e) {
        e.preventDefault();
    });

})();

$(document).ready(function () {
    //$('[data-toggle="tooltip"]').tooltip();
    //$('[data-toggle="popover"]').popover()


  
});

function Filtrar() {
    const Nombre = $('#filtroNombre').val();
    const sede = $('#filtroSede').val();
    const departamento = $('#filtroDepartamento').val();
    const denuncia = $('#IdDenuncia').val();
    const ruta = `/Denuncia/Involucrados?idDenuncia=${denuncia}&nombre=${Nombre}&idSede=${sede}&idDepartamento=${departamento}`;
    console.log(ruta);
    window.location.assign(ruta);
}

$('#btnCambiarEstado').click(function (e) {
    e.preventDefault();

    var $form = $("#frmDenunciaEstado");
    var url = BASE_PATH + 'Denuncia/GuardarEstado';
    var formdata = false;
    if (window.FormData) {
        formdata = new FormData($form[0]);
    }

    swal.fire({
        title: '¿Est&aacute;s seguro de Guardar el registro?',
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
                data: formdata ? formdata : $form.serializeObject(),
                contentType: false,
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

                    } else {

                        $('#validationSummary').show();

                        displaySummaryErrors(response.errorSumary);
                        displayErrors(response.errorControl);

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
        }
    });
});

function ConfirmarEliminar(id) {
    const idDenuncia = $("#IdDenuncia").val();
    swal.fire({
        title: '¿Est&aacute;s seguro de Eliminar el registro?',
        text: '',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí',
        cancelButtonText: 'Cancelar'
    }).then(function (result) {
        if (result.isConfirmed) {
            const url = BASE_PATH + `Denuncia/Delete?id=${id}&idDenuncia=${idDenuncia}`;

            $.ajax({
                url: url,
                data: {},
                contentType: false,
                processData: false,
                type: 'GET',
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

                    } else {
                        $('#validationSummary').show();

                        displaySummaryErrors(response.errorSumary);
                        displayErrors(response.errorControl);

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
        }
    });
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
            var comment = HttpUtility.JavaScriptStringEncode(arr[1]);
            $(msj).text(comment);
            $(formGroup).attr(`class`, `form-group has-danger`);
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
        let errMsj = System.web.HttpUtility.HtmlEncode(errorMessage);
        $ul.append('<li>' + errMsj + '</li>');
    });
}
