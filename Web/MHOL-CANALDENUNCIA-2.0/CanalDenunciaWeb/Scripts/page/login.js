(function () {

    $(document).on('click', 'form .prevent-default, input:submit .prevent-default, button:submit .prevent-default', function (e) {
        e.preventDefault();
    });

    //$('[data-toggle="tooltip"]').tooltip();
    //$('[data-toggle="popover"]').popover()

})();


$('#btnLogin').click(function (e) {
        e.preventDefault();

        var $form = $("#formLogin");
        var url = BASE_PATH + 'Login/Validar';
        var formdata = false;
        if (window.FormData) {
            formdata = new FormData($form[0]);
        }

        $.ajax({
            url: url,
            data: formdata ? formdata : $form.serializeObject(),
            contentType: false,
            processData: false,
            type: 'POST',
            success: function (response) {
                if (response.respuesta == undefined) {

                    $('#RolContainer').html(response);
                    $('#Modal').modal('show');

                }
                else if (response.respuesta){
                    $('#validationSummary').hide();
                    $('.form-group').attr("class", "form-group");

                    displayErrors(response.errorControl);



                    Swal.fire({
                        title: 'Éxito',
                        text: response.errorMessage,
                        icon: 'success',
                        showCancelButton: false,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Entrar al Sitio'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.href = response.redirect;
                        }
                    })
                
                }
                else {

                    $('#validationSummary').show();

                    displaySummaryErrors(response.errorSumary);
                    displayErrors(response.errorControl);

                    Swal.fire(
                        'Error',
                        response.errorMessage,
                        'error'
                    );
                }
            },
            error: function (response, b, c) {
                Swal.fire(
                    'Error',
                    response,
                    'error'
                );
            }
        });

});

$('#btnGuardarCorreo').click(function (e) {
    e.preventDefault();

    var $form = $("#frmCorreo");
    var url = BASE_PATH + 'Login/GuardarCorreo';
    var formdata = false;
    if (window.FormData) {
        formdata = new FormData($form[0]);
    }

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

                Swal.fire({
                    title: 'Éxito',
                    text: response.errorMessage,
                    icon: 'success',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Realizar denuncia'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = response.redirect;
                    }
                })

            }
            else {

                $('#validationSummary').show();

                displaySummaryErrors(response.errorSumary);
                displayErrors(response.errorControl);

                Swal.fire(
                    'Error',
                    response.errorMessage,
                    'error'
                );
            }
        },
        error: function (response, b, c) {
            Swal.fire(
                'Error',
                response,
                'error'
            );
        }
    });

});

function EntrarAnonimo() {

    const ruta = "/denuncia/datosInvolucrados";
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
