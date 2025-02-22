﻿const navSlide = () => {
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


$("#carField").change(function () {
	if ($(this).val() == "no") {
		$('#noCarDiv').show();


		$('#yesCarDiv').hide();
		$('#carmodelmake').removeAttr('required');
		$('#carmodelmake').removeAttr('data-error');
		$('#carpurchase').removeAttr('required');
		$('#carpurchase').removeAttr('data-error');
		$('#cardeposit').removeAttr('required');
		$('#cardeposit').removeAttr('data-error');
		$('#carinterest').removeAttr('required');
		$('#carinterest').removeAttr('data-error');
		$('#carinsurance').removeAttr('required');
		$('#carinsurance').removeAttr('data-error');
	}
	else {
		$('#yesCarDiv').show();
		$('#carmodelmake').attr('required', '');
		$('#carmodelmake').attr('data-error', 'This field is required.');
		$('#carpurchase').attr('required', '');
		$('#carpurchase').attr('data-error', 'This field is required.');
		$('#cardeposit').attr('required', '');
		$('#cardeposit').attr('data-error', 'This field is required.');
		$('#carinterest').attr('required', '');
		$('#carinterest').attr('data-error', 'This field is required.');
		$('#carinsurance').attr('required', '');
		$('#carinsurance').attr('data-error', 'This field is required.');


		$('#noCarDiv').hide();
	}
});

$("#carField").trigger("change");



