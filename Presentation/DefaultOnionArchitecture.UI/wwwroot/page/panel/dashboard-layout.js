$(document).ready(function () {
	var currentHref = window.location.href;

	$('#sidebar-menu a').each(function () {
		if (this.href === currentHref) {
			$(this).addClass('active');

			var parentCollapse = $(this).closest('.collapse');
			if (parentCollapse.length) {
				parentCollapse.addClass('show');
				parentCollapse.prev('[data-bs-toggle="collapse"]').attr('aria-expanded', 'true');
			}

			$(this).on('click', function (e) {
				e.preventDefault();
			});
		}
	});
});