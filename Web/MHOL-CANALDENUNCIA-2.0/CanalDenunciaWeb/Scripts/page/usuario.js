(function () {

    $(document).on('click', 'form .prevent-default, input:submit .prevent-default, button:submit .prevent-default', function (e) {
        e.preventDefault();
    });

})();

$(document).ready(function () {
});

function Descargar(archivo) {
    const ruta = `/Usuario/Descargar`;
    window.location.assign(ruta);
}

//$("#formuploadajax").on("submit", function (e) {

//    e.preventDefault();
//    var file = ($('#archivo'))[0].files[0];
//    var formData = new FormData(document.getElementById("formuploadajax"));
//    formData.append("file", file);
//    if (!file) {
//        alert("Debe cargar un archivo");
//    }
//    else {

//        $.ajax({
//            url: BASE_PATH + 'Usuario/CargarArchivo',
//            type: "post",
//            dataType: "html",
//            data: formData,
//            cache: false,
//            contentType: false,
//            processData: false,
//            success: function (response) {
//                if (response.respuesta) {

//                    $('#validationSummary').hide();
//                    $('.form-group').attr("class", "form-group");

//                    displayErrors(response.errorControl);

//                    Swal.fire({
//                        title: 'Éxito',
//                        text: response.errorMessage,
//                        icon: 'success',
//                        showCancelButton: false,
//                        confirmButtonColor: '#3085d6',
//                        cancelButtonColor: '#d33',
//                        confirmButtonText: 'Entrar al Sitio'
//                    }).then((result) => {
//                        if (result.isConfirmed) {
//                            window.location.href = response.redirect;
//                        }
//                    })

//                }
//                else {

//                    $('#validationSummary').show();

//                    displaySummaryErrors(response.errorSumary);
//                    displayErrors(response.errorControl);

//                    Swal.fire(
//                        'Error',
//                        response.errorMessage,
//                        'error'
//                    );
//                }
//            },
//            error: function (response, b, c) {
//                Swal.fire(
//                    'Error',
//                    response,
//                    'error'
//                );
//            }
//        });

//    }
//});

$('#formuploadajax').on("submit", function (e){
    e.preventDefault();

    var file = ($('#archivo'))[0].files[0];
    var formData = new FormData(document.getElementById("formuploadajax"));
    formData.append("file", file);
    

    $.ajax({
        url: BASE_PATH + 'Usuario/CargarArchivo',
        data: formData,
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
                    confirmButtonText: 'Aceptar'
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
                'Error, archivo vacío',
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
