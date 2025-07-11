$('body').on('click', ".flag-dropdown .caption", function () {
    $(this).parent().toggleClass('open');
});

let currentLang = "tr"

$('body').on('click', ".special-bar .flag-dropdown .list .item", function () {
    currentLang = $(this).data("item")

    $(".modal [name='languageId']").val(currentLang)

    fetchData(getDataUrl);


    $('.flag-dropdown > .list > .item').removeClass('selected');
    $(this).addClass('selected').parent().parent().removeClass('open').children('.caption').html($(this).html());
});


$(document).on('keyup', function (evt) {
    if ((evt.keyCode || evt.which) === 27) {
        $('.flag-dropdown').removeClass('open');
    }
});

$(document).on('click', function (evt) {
    if ($(evt.target).closest(".flag-dropdown > .caption").length === 0) {
        $('.flag-dropdown').removeClass('open');
    }
});