async function getCart() {
	if (window.isAuthenticated) {
		try {
			const response = await $.ajax({
				url: '/api/cart/all',
				type: "GET",
			});

			if (response.result === 1) {
				return response.data;
			} else {
				Swal.fire({
					title: 'Hata',
					html: "İşlem başarısız",
					icon: 'error',
					confirmButtonText: 'Tamam'
				});
				return [];
			}
		} catch (error) {
			Swal.fire({
				title: 'Hata',
				html: "Bir şeyler ters gitti",
				icon: 'error',
				confirmButtonText: 'Tamam'
			}).then(() => {
				location.reload();
			});
			return [];
		}
	} else {
		const cart = localStorage.getItem("cart");
		return cart ? JSON.parse(cart) : [];
	}
}


function saveCart(cart) {
	localStorage.setItem("cart", JSON.stringify(cart));
}



renderCart();


async function renderCart() {
	const cart = await getCart();
	const $container = $(".basket-modal-area .products");
	$container.empty();
	if (cart.length === 0) {
		$container.append(`<p class="empty-cart-message">Sepette ürün bulunamadı.</p>`);
		return;
	}

	cart.forEach(product => {
		const item = `
		<div class="product" data-id="${product.id}">
		<div class="img">
		<img src="${product.img}" alt="${product.name}" />
		</div>
		<div class="content">
		<h2>${product.name}</h2>
		<span>${product.price}₺</span>
		<div class="quantity-controls">
		<button class="decrease" data-id="${product.id}">-</button>
		<span class="quantity">${product.quantity}</span>
		<button class="increase" data-id="${product.id}">+</button>
		</div>
		</div>
		</div>
		`;
		$container.append(item);
	});
}



$(document).ready(() => {

    $(".basket-modal-area").on("click", ".increase", async function () {
        const id = $(this).data("id");
        const cart = await getCart();
        const product = cart.find(p => p.id === id);
        if (product) {
            product.quantity += 1;
            saveCart(cart);
            await renderCart();
            var formData = new FormData();

            var productId = $(this).closest(".product").attr("data-id")

            formData.append("productId", productId)
            formData.append("count", product.quantity)

            if (window.isAuthenticated) {
                $(".quantity-controls button").attr("disabled", true)
                $.ajax({
                    url: '/api/cart/update',
                    type: "POST",
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: async function (response) {
                        if (response.result == 1) {


                            await renderCart();
                            $(".quantity-controls button").attr("disabled", false)
                        } else {
                            Swal.fire({
                                title: 'Uyarı',
                                html: response.error,
                                icon: 'warning',
                                confirmButtonText: 'Tamam'
                            })
                        }
                    }
                })
            }
        }
    });

    $(".basket-modal-area").on("click", ".decrease", async function () {
        const id = $(this).data("id");
        let cart = await getCart();
        const product = cart.find(p => p.id === id);
        if (product) {
            product.quantity -= 1;

            if (product.quantity <= 0) {
                cart = cart.filter(p => p.id !== id);
            }

            saveCart(cart);
            await renderCart();
            var formData = new FormData();

            var productId = $(this).closest(".product").attr("data-id")

            formData.append("productId", productId)
            formData.append("count", product.quantity)


            if (window.isAuthenticated) {
                $(".quantity-controls button").attr("disabled", true)
                $.ajax({
                    url: '/api/cart/update',
                    type: "POST",
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: async function (response) {
                        if (response.result == 1) {
                            await renderCart();
                            $(".quantity-controls button").attr("disabled", false)
                        } else {
                            Swal.fire({
                                title: 'Uyarı',
                                html: response.error,
                                icon: 'warning',
                                confirmButtonText: 'Tamam'
                            })
                        }
                    }
                })
            }

        }
    });



	if (!window.isAuthenticated) {
		$("#reset-password-form").on("submit", function (e) {
			e.preventDefault();
			if ($(this).valid()) {
				var submitBtn = $(this).find("[type='submit']");
				var submitForm = $(this);
				var formData = new FormData(this);
				if (FormSubmitLoader(submitBtn, submitForm, " Gönderiliyor")) {
					$.ajax({
						url: '/api/user/createresetcode',
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

								$(submitForm).find(".send-code-icon-box").addClass("active")
							} else {
								FormSubmitLoaderReturn(submitBtn, submitForm)
								Swal.fire({
									title: 'Uyarı',
									html: response.msg,
									icon: 'error',
									confirmButtonText: 'Tamam'
								})
							}
						}
					});
				}
			}

		})

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
									location.reload()
								});
							} else {
								FormSubmitLoaderReturn(submitBtn, submitForm)
								Swal.fire({
									title: 'Uuarı',
									html: response.msg,
									icon: 'error',
									confirmButtonText: 'Tamam'
								})
							}
						}
					});
				}
			}
		})

		$("#register-form").on("submit", function (e) {
			e.preventDefault();
			if ($(this).valid()) {
				var submitBtn = $(this).find("[type='submit']");
				var submitForm = $(this);
				var formData = new FormData(this);
				if (FormSubmitLoader(submitBtn, submitForm, "Kayıt Ol")) {
					$.ajax({
						url: '/api/auth/register',
						type: "POST",
						data: formData,
						processData: false,
						contentType: false,
						beforeSend: function (xhr, settings) {
							settings._formId = "#register-form";
							settings._btnId = "#register-form-btn";
						},
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
									title: "Başarıyla kayıt olundu"
								})

								$("#register-modal .create-account-info-box .karahan-btn").click()
							} else {
								Swal.fire({
									title: 'Uuarı',
									html: response.msg,
									icon: 'error',
									confirmButtonText: 'Tamam'
								})
							}
							FormSubmitLoaderReturn(submitBtn, submitForm)
						},
					});
				}
			}
		})

		$(document).ready(function () {

			addValid("#login-form", {
				email: {
					required: true,
					email: true,
				},
				password: {
					required: true,
				},

			})

			addValid("#reset-password-form", {
				email: {
					required: true,
					email: true,
				},
			})

			addValid("#register-form", {
				email: {
					required: true,
					email: true
				},
				password: {
					required: true
				},
				surname: {
					required: true
				},
				name: {
					required: true
				},
				phoneNumber: {
					required: true
				},
				confirmPassword: {
					required: true,
					equalTo: "#register-password"
				}
			});

		})
	} else {
		$(".menu-user-icon").on("click", function () {
			location.href = "/profilim"
		})
	}
})
