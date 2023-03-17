﻿(function () {

    $(document).on('click', 'form .prevent-default, input:submit .prevent-default, button:submit .prevent-default', function (e) {
        e.preventDefault();
    });

    //$('[data-toggle="tooltip"]').tooltip();
    //$('[data-toggle="popover"]').popover()

})();


$('#btnRecupera').click(function (e) {
    e.preventDefault();
   
    var $form = $("#frmRecupera");
    var url = BASE_PATH + 'Login/EnviaMailRecupera';
    var formdata = false;
    if (window.FormData) {
        formdata = new FormData($form[0]);
    }
    $(this).prop("disabled", false);
    $(this).html(
        `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Modificando...`
    );
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
                        `<button id="btnRecupera" name="btnRecupera" class="btn btn-primary btn-block"><i class="fas fa-key"></i>&nbsp;Recuperar</button>`
                    );
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
                        confirmButtonText: 'Ok'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.href = response.redirect;
                        }
                    })

                }
                else {
                    $('#btnRecupera').prop("disabled", false);
                    // add spinner to button
                    $('#btnRecupera').html(
                        `<button id="btnRecupera" name="btnRecupera" class="btn btn-primary btn-block"><i class="fas fa-key"></i>&nbsp;Recuperar</button>`
                    );
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
                $('#btnRecupera').prop("disabled", false);
                // add spinner to button
                $('#btnRecupera').html(
                    `<button id="btnRecupera" name="btnRecupera" class="btn btn-primary btn-block"><i class="fas fa-key"></i>&nbsp;Recuperar</button>`
                );
                Swal.fire(
                    'Error',
                    response,
                    'error'
                );
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
