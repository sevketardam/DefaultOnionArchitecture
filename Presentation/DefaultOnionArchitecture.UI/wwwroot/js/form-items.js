var buttonOldContent;

function activeQuil() {
    $(document).find('.quill-editor:not(.ql-container)').each(function () {
        const $editorContainer = $(this);
        const dataName = $editorContainer.attr('data-name');

        const $textarea = $(`textarea[quill-name="${dataName}"]`);

        const quill = createQuill($editorContainer[0], {
            placeholder: $editorContainer.closest('.form-group').find('label').text() + ' giriniz...'
        });

        var textAreavalue = $textarea.val()

        quill.clipboard.dangerouslyPasteHTML(textAreavalue);
        editorConnectTextarea(quill, $textarea);
    });
}

function applyPhoneMask($input) {
    $input.on('input', function () {
        let input = $(this).val().replace(/\D/g, '');
        if (input.startsWith('90')) input = input.substring(2);
        if (input.startsWith('0')) input = input.substring(1);
        input = input.substring(0, 10); 

        let formatted = '+90 ';
        if (input.length > 0) formatted += input.substring(0, 3);
        if (input.length >= 4) formatted += ' ' + input.substring(3, 6);
        if (input.length >= 7) formatted += ' ' + input.substring(6, 8);
        if (input.length >= 9) formatted += ' ' + input.substring(8, 10);

        $(this).val(formatted);
    });
}

function createQuill(editor,settings){
	var quill = new Quill(editor, {
	    modules: {
	      syntax: true,
	    },
	    placeholder: settings.placeholder,
	    theme: 'snow',
    });

    if (settings.value) {
         quill.clipboard.dangerouslyPasteHTML(settings.value);
    }
    return quill;
}

function editorConnectTextarea(editor) {
    editor.on('text-change', function (delta, oldDelta, source) {
        let textName = $(editor.container).attr("data-name")

        if (!editor.getText().replace("\n", "")) {
            $("textarea[quill-name='" + textName + "']").val("").trigger("keyup")
            return;
        }

        $("textarea[quill-name='" + textName + "']").val(editor.root.innerHTML).trigger("keyup")
    })
}

async function callSelectbox(selectbox,apiUrl,lang = "tr",value){
	FormSelectboxLoader(selectbox)
	await $.ajax({
		url: apiUrl,
		type: "GET",
		data: { lang:lang },
		success: function (response) {
			if (response.result == 1) {

				$(selectbox).find("option:not([disabled])").remove()

				$.each(response.data,function(key,value){
					$(selectbox).append(`<option value='${value.value}' ${value.selected ? "selected" : ""} >${value.text}</option>`)
				})

                if(value){
                    $(selectbox).val(value)
                }


			} else {
				Swal.fire({
					title: 'Uyarı',
					html: response.error,
					icon: 'warning',
					confirmButtonText: 'Tamam'
				});
			}
        },
        complete: function () {
            FormSelectboxLoaderReturn(selectbox);
        }
	});
}

function addValid(form,rules,errorPlacement,ignore = []) {
    $(form).validate({
        ignore,
        rules,
        errorPlacement: errorPlacement ?? function (error, element) {
            element.closest(".form-group").append(error);
        },
    });
}

function showImagePreview(input, src) {
    const $input = $(input);
    if ($input.length === 0) return;

    const $formGroup = $input.closest('.form-group');
    const $previewContainer = $formGroup.find('.file-preview');

    const $img = $('<img>', {
        src: src,
        alt: 'Ön Gösterim',
        css: {
            'max-width': '100%',
            'max-height': '200px',
            'margin-bottom':"1rem"
        }
    });
    $previewContainer.html($img);
}

$("body").on("change", ".preview-input-file", function () {
    const file = this.files[0];
    if (file && file.type.startsWith('image/')) {
        const input = $(this)
        const reader = new FileReader();
        reader.onload = function (e) {
            showImagePreview(input, e.target.result);
        };
        reader.readAsDataURL(file);
        $(this).closest(".form-group").find("label.error").remove()
    } else {
        $(this).closest(".form-group").append("<label class='error'>Resim seçiniz!</label>")
        $(this).val("")
    }
})

function toSlug(text) {
    return text
        .toLowerCase()
        .replace(/ç/g, 'c')
        .replace(/ğ/g, 'g')
        .replace(/ı/g, 'i')
        .replace(/ö/g, 'o')
        .replace(/ş/g, 's')
        .replace(/ü/g, 'u')
        .replace(/[^a-z0-9\s-]/g, '')
        .trim()
        .replace(/\s+/g, '-')
        .replace(/-+/g, '-');
}
function setSlugVal(text, slugInput) {
    var value = toSlug(text)

    $(slugInput).val(value)
}

$(".to-slug-main-input").on("input", function () {
    var slugName = $(this).attr("slug-name")
    var slugInput = $(this).closest("form").find(".to-slug-input[data-name='" + slugName + "']")
    setSlugVal($(this).val(), slugInput)
})

const TableToast = Swal.mixin({
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

function formButtonLoader(button) {
    var button = $(button)
    buttonOldContent = button.html();
    button.attr("disabled", true);
    button.addClass("buttonload");
    button.html(`<div class="spinner-border"></div> Yükleniyor`);
}

function formButtonLoaderReturn(button) {
    var button = $(button)
    button.attr("disabled", false);
    button.removeClass("buttonload");
    button.html(buttonOldContent);
}

var buttonOldContent2;

function formButtonLoaderSpecial(button, text = "") {
    if ($(".buttonload").length == 0) {

        text != "" ? "&nbsp;" + text : ""
        var button = $(button)
        buttonOldContent2 = button.html();
        button.attr("disabled", true);
        button.addClass("buttonload");
        button.html(`<div class="spinner-border"></div>  ${text}`);
        $(button).closest(".modal").find(".btn-close").attr("disabled", true)
        return true;
    } else {
        $(button).addClass("error-icon")
        setTimeout(function () {
            $(button).removeClass("error-icon")
        }, 600)
        return false;
    }
}

function formButtonLoaderSpecialReturn(button) {
    var button = $(button)
    button.attr("disabled", false);
    button.removeClass("buttonload");
    button.html(buttonOldContent2);
    $(button).closest(".modal").find(".btn-close").attr("disabled", false)
}

let iconOldContent;
function TableIconLoader(icon, text = "") {
    if ($(".iconload").length == 0) {
        var icon = $(icon);
        icon.data("old-content", icon.html());
        icon.addClass("iconload");
        icon.html(`<div class="spinner-border"></div>&nbsp;${text}`);
        return true;
    } else {
        $(icon).addClass("error-icon");
        setTimeout(function () {
            $(icon).removeClass("error-icon");
        }, 600);
        return false;
    }
}

function TableIconLoaderReturn(icon) {
    var icon = $(icon);
    icon.removeClass("iconload");
    icon.html(icon.data("old-content"));
}

let formSubmitOldContent;
let formSubmitForm;

function FormSubmitLoader(submitBtn, form, loadText = "") {
    if ($(".submitted-form").length == 0) {
        loadText != "" ? "&nbsp;" + loadText : ""

        $(form).find("[disabled]").addClass("not-return")

        var button = $(submitBtn)

        formSubmitOldContent = button.html();

        button.closest("div").find("button").attr("disabled", true)
        button.attr("disabled", true);
        button.addClass("button-load");
        button.html(`<div class="spinner-border"></div> ${loadText}`);
        $(form).addClass("submitted-form")
        $(form).find("input, textarea, select, button").attr("disabled", true)
        $(form).closest(".modal").find(".btn-close").attr("disabled", true)

        return true;
    } else {
        return false;
    }
}

function FormSubmitLoaderReturn(submitBtn, form) {
    $(form).find("input:not(.not-return), textarea:not(.not-return), select:not(.not-return), button:not(.not-return)").attr("disabled", false);
    $(form).closest(".modal").find(".btn-close").attr("disabled", false)
    $(form).removeClass("submitted-form")
    $(form).find(".not-return").removeClass(".not-return")

    var button = $(submitBtn)
    button.closest("div").find("button").attr("disabled", false)
    button.attr("disabled", false);
    button.removeClass("buttonload");
    button.html(formSubmitOldContent);
}



function FormSelectboxLoader(selectbox) {
    $(selectbox).closest(".form-group").append(`<div class="selectbox-loader">
												<div class="selectbox-box">
													<div class="spinner-border" ></div>
												</div>
											</div>`)
    $(selectbox).attr("disabled", true)
}

function FormSelectboxLoaderReturn(selectbox) {
    $(selectbox).closest(".form-group").find(".selectbox-loader").remove()
    $(selectbox).attr("disabled", false)
}

$('.show-password-btn').on("click", function () {
    let dataType = $(this).attr("data-type")
    if (dataType == "0") {
        $(this).html(`<i class="fa-solid fa-eye"></i>`)
        $(this).attr("data-type", "1")
        $(this).closest(".password-input-box").find("input").attr("type", "text")
    } else {
        $(this).html(`<i class="fa-solid fa-eye-slash"></i>`)
        $(this).attr("data-type", "0")
        $(this).closest(".password-input-box").find("input").attr("type", "password")
    }
})


function randomPsw() {
    var character = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@@#$%&*()";
    var lengthPsw = 8;
    var randomPsw = '';
    for (var i = 0; i < lengthPsw; i++) {
        var numPws = Math.floor(Math.random() * character.length);
        randomPsw += character.substring(numPws, numPws + 1);
    }
    return randomPsw;
}


$('.generate-password').on("click", function () {

    let randomPassword = randomPsw();
    let passwordInput = $(this).closest(".password-input-box").find("input")
    $(passwordInput).val(randomPassword)

    let passwordInputName = $(passwordInput).attr("name")
    let passwordInputAgainInput = $(this).closest("form").find("input[name='" + passwordInputName + "Again" + "']")
    $(passwordInputAgainInput).val(randomPassword)

})

function GetFormData(formElement) {
    var formData = new FormData();
    $(formElement).find("select,input,textarea").each(function (index, element) {

        var name = $(element).attr('name')
        var value = $(element).val()
        var type = $(element).attr("type")
        if ($(element).is("select") && $(element).prop('multiple')) {
            var selectedOptions = $(element).find('option:selected');
            selectedOptions.each(function (i, option) {
                formData.append(name, $(option).val());
            });

        } else if (type == "radio") {
            if ($(element).is(":checked")) {
                if (formData.has(name)) {
                    formData.set(name, value);
                } else {
                    formData.append(name, value);
                }
            }
        }
        else if (type == "checkbox") {
            formData.append(name, $(element).is(":checked"));
        } else if (type == "file") {
            formData.append(name, $(element).prop("files")[0]);
        } else {
            formData.append(name, value);
        }
    });

    return formData;
}

function createQuill(editor, settings) {
    return new Quill(editor, {
        modules: {
            syntax: true,
        },
        placeholder: settings.placeholder,
        theme: 'snow',
    });
}

$(".modal-tab-box .modal-tab-link").on("click", function () {
    var thisModal = $(this).closest(".modal")

    $(thisModal).find(".modal-tab-link").removeClass("active")
    $(this).addClass("active")
    var tabId = $(this).attr("tab-link")
    $(thisModal).find(".tab-body").addClass("d-none")
    $(thisModal).find(".tab-body[tab-id='" + tabId + "']").removeClass("d-none")
})

$(document).ready(function () {
    $(document).ajaxError(function (event, jqXHR, settings, thrownError) {
        if (settings && settings.handleErrors && Array.isArray(settings.handleErrors) && settings.handleErrors.includes(jqXHR.status)) {
            return;
        }

        if (jqXHR.responseJSON && jqXHR.responseJSON.isValid !== undefined && jqXHR.responseJSON.isValid) {

            var errorHtml = "<div class='alert alert-danger'>"
            $.each(jqXHR.responseJSON.errors, function (key,value) {
                errorHtml += "<div class='text-start'><b>"+value+"</b></div>"
            })

            errorHtml += "</div>"

            Swal.fire({ title: 'Uyarı', html: errorHtml, icon: 'error', confirmButtonText: 'Tamam' });

            FormSubmitLoaderReturn(settings._btnId, settings._formId)
            return;
        }

        switch (jqXHR.status) {
            case 403:
                Swal.fire({ title: 'Hata', html: "Yetkiniz yok", icon: 'error', confirmButtonText: 'Tamam' })
                    .then(() => setTimeout(() => location.reload(), 500));
                break;
            case 404:
                Swal.fire({ title: 'Hata', html: "İçerik bulunamadı", icon: 'error', confirmButtonText: 'Tamam' })
                    .then(() => setTimeout(() => location.reload(), 500));
                break;
            case 422:
                Swal.fire({ title: 'Hata', html: "Format Yanlış", icon: 'error', confirmButtonText: 'Tamam' })
                break;
            case 429:
                TableToast.fire({ icon: "error", title: "Çok fazla istek"});
                break;
            default:
                Swal.fire({ title: 'Hata', html: "Bir şeyler ters gitti", icon: 'error', confirmButtonText: 'Tamam' })
                    .then(() => setTimeout(() => location.reload(), 500));
        }
    });
});
