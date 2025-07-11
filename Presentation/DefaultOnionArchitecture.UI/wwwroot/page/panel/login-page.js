$("#login-form").on("submit", function (e) {
    e.preventDefault();
    if ($(this).valid()) {
        var submitBtn = $(this).find("[type='submit']");
        var submitForm = $(this);
        var formData = new FormData(this);
        if (FormSubmitLoader(submitBtn, submitForm, "Giriş Yap")) {
            $.ajax({
                url: '/api/auth/login',
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {

                    if (response.result == 1) {
                        const Toast = Swal.mixin({
                            toast: true,
                            position: "top-end",
                            showConfirmButton: false,
                            timer: 1000,
                            timerProgressBar: true,
                            didOpen: (toast) => {
                                toast.onmouseenter = Swal.stopTimer;
                                toast.onmouseleave = Swal.resumeTimer;
                            }
                        });

                        Toast.fire({
                            icon: "success",
                            title: "Başarıyla giriş yapıldı"
                        }).then(() => {
                            location.href = "/dashboard"
                        });
                    } else {
                        FormSubmitLoaderReturn(submitBtn, submitForm)
                        $(".error-box").html(`  <div class="alert alert-danger">
                                                                            ${response.msg}
                                                                        </div>`)
                    }
                },
                error: function () {
                    Swal.fire({
                        title: 'Hata',
                        html: "Bir şeyler ters gitti",
                        icon: 'error',
                        confirmButtonText: 'Tamam'
                    }).then(() => {
                        location.reload();
                    })
                }
            });
        }
    }
})


$(document).ready(function () {
    $("#login-form").validate({
        rules: {
            email: {
                required: true,
            },
            password: {
                required: true,
            },

        },
        errorPlacement: function (error, element) {
            element.closest(".form-group").append(error);
        },
    });
})