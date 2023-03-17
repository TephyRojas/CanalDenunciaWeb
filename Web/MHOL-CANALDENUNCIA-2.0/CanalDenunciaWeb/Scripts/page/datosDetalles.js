(function () {

    $(document).on('click', 'form .prevent-default, input:submit .prevent-default, button:submit .prevent-default', function (e) {
        e.preventDefault();
    });

})();

$(document).ready(function () {
});

function Ver(id) {
    const url = BASE_PATH + 'Denuncia/MostrarObservacion?id=' + id;
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

$("#IdTipoDelito").change(function () {
    const id = $(this).val();
    const url = BASE_PATH + 'Denuncia/GetDetalleByIdTipoDelito?idTipoDelito=' + id;
    if (id == '0') {
        $('#DescripcionDelito').empty();
        return false;
    }

    $.ajax({
        url: url,
        data: {},
        contentType: false,
        processData: false,
        type: 'GET',
        success: function (response) {
            if (response.respuesta === undefined) {
                let texto = "";               
                $(response).each(function (index, value) {
                    texto += `${ value.Descripcion }`
                });
               
                $('#DescripcionDelito').text(texto);
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
});

$('#btnFinalizar').click(function (e) {
    e.preventDefault();
    $(this).prop("disabled", true);
    // add spinner to button
    $(this).html(
        `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Guardando...`
    );
    $('#btnCancelar').prop("disabled", true);
    const $form = $("#frmDenunciaDetalles");
    const url = BASE_PATH + 'Denuncia/GuardarDetalles';
    let formdata = false;

    if (window.FormData) {
        formdata = new FormData($form[0]);
    }

    swal.fire({
        title: '¿Est&aacute;s seguro de Finalizar el registro?',
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
                        $(this).prop("disabled", false);
                        // add spinner to button
                        $(this).html(
                            `<a id="btnFinalizar" name="btnFinalizar" href="~/denuncia/Index" class="btn btn-success btn-sm" data-toggle="tooltip" data-placement="bottom" title="Finalizar"><i class="fas fa-chevron-circle-right"></i>&nbsp;Enviar y Finalizar</a>`
                        );
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
                        $('#btnFinalizar').prop("disabled", false);
                        // add spinner to button
                        $('#btnFinalizar').html(
                            `<a id="btnFinalizar" name="btnFinalizar" href="~/denuncia/Index" class="btn btn-success btn-sm" data-toggle="tooltip" data-placement="bottom" title="Finalizar"><i class="fas fa-chevron-circle-right"></i>&nbsp;Enviar y Finalizar</a>`
                        );
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
        else {
            $('#btnFinalizar').prop("disabled", false);
            // add spinner to button
            $('#btnFinalizar').html(
                `<a id="btnFinalizar" name="btnFinalizar" href="~/denuncia/Index" class="btn btn-success btn-sm" data-toggle="tooltip" data-placement="bottom" title="Finalizar"><i class="fas fa-chevron-circle-right"></i>&nbsp;Enviar y Finalizar</a>`
            );
            $('#btnCancelar').prop("disabled", false);
        }
    });
});
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
