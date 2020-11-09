const navSlide = () => {
	const burger = document.querySelector('.burger');
	const nav = document.querySelector('.nav-links');
	const navLinks = document.querySelectorAll('.nav-links li');

	burger.addEventListener('click', () => {

		nav.classList.toggle('nav-active');

		navLinks.forEach((link, index) => {
			if (link.style.animation) {
				link.style.animation = '';
			} else {
				link.style.animation = `navLinkFade 0.2s ease forwards ${index / 7 + 0.1}s`;
			}
		});

		burger.classList.toggle('toggle');

	});
}
navSlide();

$("#seeAnotherField").change(function () {
    if ($(this).val() == "yes") {
		$('#rentingDiv').show();
		$('#otherField').attr('required', '');
		$('#otherField').attr('data-error', 'This field is required.');


		$('#buyingDiv').hide();
		$('#purchase').removeAttr('required');
		$('#purchase').removeAttr('data-error');
		$('#deposit').removeAttr('required');
		$('#deposit').removeAttr('data-error');
		$('#interest').removeAttr('required');
		$('#interest').removeAttr('data-error');
	}
	else {
		$('#rentingDiv').hide();
		$('#otherField').removeAttr('required');
		$('#otherField').removeAttr('data-error');


		$('#buyingDiv').show();
		$('#purchase').attr('required', '');
		$('#purchase').attr('data-error', 'This field is required.');
		$('#deposit').attr('required', '');
		$('#deposit').attr('data-error', 'This field is required.');
		$('#interest').attr('required', '');
		$('#interest').attr('data-error', 'This field is required.');
        
    }
});
$("#seeAnotherField").trigger("change");

