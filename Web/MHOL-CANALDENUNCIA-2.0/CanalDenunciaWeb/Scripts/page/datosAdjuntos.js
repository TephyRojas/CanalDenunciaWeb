(function () {

    $(document).on('click', 'form .prevent-default, input:submit .prevent-default, button:submit .prevent-default', function (e) {
        e.preventDefault();
    });

})();

$(document).ready(function () {
});

function Descargar(archivo) {
    const idDenuncia = $('#IdDenuncia').val();
    const ruta = `/Denuncia/Descargar?Archivo=${archivo}&IdDenuncia=${idDenuncia}`;
    window.location.assign(ruta);
}

function Eliminar(archivo) {
    const idDenuncia = $('#IdDenuncia').val();
    const ruta = `/Denuncia/EliminarDocumento?Archivo=${archivo}&IdDenuncia=${idDenuncia}`;
    window.location.assign(ruta);
}

function ConfirmarEliminar(id) {
   
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
                            var urlResponse = Microsoft.Security.Application.AntiXss.JavaScriptEncode(response.redirect);
                            window.location.href = urlResponse
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
            $(msj).text(arr[1]);          
            $(formGroup).attr(`class`, `form-group has-danger`);
            $(obj).attr(`class`, `form-control is-invalid`)
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
