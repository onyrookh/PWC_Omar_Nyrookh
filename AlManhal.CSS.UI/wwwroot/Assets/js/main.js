(function ($) {

    "use strict";

    $(window).on('load', function () {
        $('[data-loader="circle-side"]').fadeOut(); // will first fade out the loading animation
        $('#preloader').delay(350).fadeOut('slow'); // will fade out the white DIV that covers the website.
        $('body').delay(350);
        $('#hero_in h1,#hero_in form').addClass('animated');
        $('.hero_single, #hero_in').addClass('start_bg_zoom');
        $(window).scroll();
    });

    // Sticky nav
    if ($("#IsStickyHeader").val() != 1) {
        $(window).on('scroll', function () {
            if ($(this).scrollTop() > 1) {
                $('.header').addClass("sticky");
            } else {
                $('.header').removeClass("sticky");
            }
        });
    }
    // Sticky sidebar
    $('#sidebar-search').theiaStickySidebar({
        additionalMarginTop: 110,
        containerSelector: "#main",
        updateSidebarHeight:false
    });

    $('#sidebar').theiaStickySidebar({
        additionalMarginTop: 65,
        containerSelector: "#main",
        updateSidebarHeight: false
    });

    $('#sidebar-admin').theiaStickySidebar({
        additionalMarginTop: 50
    });

    var position = globalFunctions.getAppLanguage() == 2 ? "right" : "left";

    // Mobile Mmenu
    var $menu = $("nav#menu").mmenu({
        "extensions": ["pagedim-black"],
        offCanvas: {
            position: position
        },
        counters: false,
        keyboardNavigation: {
            enable: true,
            enhance: true
        },
        navbar: {
            title: 'MENU'
        },
    },
		{
		    // configuration
		    clone: true,
		    classNames: {
		        fixedElements: {
		            fixed: "menu_2",
		            sticky: "sticky"
		        }
		    }
		});
    var $icon = $("#hamburger");
    var API = $menu.data("mmenu");
    if (API) {
        $icon.on("click", function () {
            API.open();
        });
        API.bind("open:finish", function () {
            setTimeout(function () {
                $icon.addClass("is-active");
            }, 100);
        });
        API.bind("close:finish", function () {
            setTimeout(function () {
                $icon.removeClass("is-active");
            }, 100);
        });
    }

    // Header button explore
    $('a[href^="#"].btn_explore').on('click', function (e) {
        e.preventDefault();
        var target = this.hash;
        var $target = $(target);
        $('html, body').stop().animate({
            'scrollTop': $target.offset().top
        }, 800, 'swing', function () {
            window.location.hash = target;
        });
    });

    // WoW - animation on scroll
    var wow = new WOW(
	  {
	      boxClass: 'wow',      // animated element css class (default is wow)
	      animateClass: 'animated', // animation css class (default is animated)
	      offset: 0,          // distance to the element when triggering the animation (default is 0)
	      mobile: true,       // trigger animations on mobile devices (default is true)
	      live: true,       // act on asynchronously loaded content (default is true)
	      callback: function (box) {
	          // the callback is fired every time an animation is started
	          // the argument that is passed in is the DOM node being animated
	      },
	      scrollContainer: null // optional scroll container selector, otherwise use window
	  }
	);
    wow.init();

    /*  video popups */
    $('.video').magnificPopup({ type: 'iframe' });	/* video modal*/

    // tooltips
    globalFunctions.updateTooltip($('a'));
    globalFunctions.updateTooltip($('button'));
    globalFunctions.updateTooltip($('span'));
    globalFunctions.updateTooltip($('label'));

    // Accordion
    function toggleChevron(e) {
        $(e.target)
			.prev('.card-header')
			.find("i.indicator")
			.toggleClass('ti-minus ti-plus');
    }
    $('#accordion_lessons').on('hidden.bs.collapse shown.bs.collapse', toggleChevron);
    function toggleIcon(e) {
        $(e.target)
            .prev('.panel-heading')
            .find(".indicator")
            .toggleClass('ti-minus ti-plus');
    }
    // Accordion 2 (updated v1.2)
    $('.accordion_2').on('hidden.bs.collapse shown.bs.collapse', toggleChevron);
    function toggleIcon(e) {
        $(e.target)
            .prev('.panel-heading')
            .find(".indicator")
            .toggleClass('ti-minus ti-plus');
    }
    $('.panel-group').on('hidden.bs.collapse', toggleIcon);
    $('.panel-group').on('shown.bs.collapse', toggleIcon);


    // Input field effect
    (function () {
        if (!String.prototype.trim) {
            (function () {
                // Make sure we trim BOM and NBSP
                var rtrim = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g;
                String.prototype.trim = function () {
                    return this.replace(rtrim, '');
                };
            })();
        }
        [].slice.call(document.querySelectorAll('input.input_field, textarea.input_field')).forEach(function (inputEl) {
            // in case the input is already filled..
            if (inputEl.value.trim() !== '') {
                classie.add(inputEl.parentNode, 'input--filled');
            }

            // events:
            inputEl.addEventListener('focus', onInputFocus);
            inputEl.addEventListener('blur', onInputBlur);
        });
        function onInputFocus(ev) {
            classie.add(ev.target.parentNode, 'input--filled');
        }
        function onInputBlur(ev) {
            if (ev.target.value.trim() === '') {
                classie.remove(ev.target.parentNode, 'input--filled');
            }
        }
    })();

    // Selectbox
    $(".selectbox").selectbox();

    // Check and radio input styles
    $('input.icheck').iCheck({
        checkboxClass: 'icheckbox_square-grey',
        radioClass: 'iradio_square-grey'
    });

    // Sticky filters
    $(window).bind('load resize', function () {
        //var width = $(window).width();
        //if (width <= 991) {
        //    $('.sticky_horizontal').stick_in_parent({
        //        offset_top: 58
        //    });
        //} else {
        //    $('.sticky_horizontal').stick_in_parent({
        //        offset_top: 71
        //    });
        //}
        $('.sticky_horizontal').stick_in_parent({
            offset_top: 58
        });
    });

    // Secondary nav scroll
    var $sticky_nav = $('.secondary_nav');
    $sticky_nav.find('a').on('click', function (e) {
        e.preventDefault();
        var target = this.hash;
        var $target = $(target);
        $('html, body').animate({
            'scrollTop': $target.offset().top - 150
        }, 800, 'swing');
    });
    $sticky_nav.find('ul li a').on('click', function () {
        $sticky_nav.find('ul li a.active').removeClass('active');
        $(this).addClass('active');
    });

    // Faq section (updated v1.2)
    $('#faq_box a[href^="#"]').on('click', function () {
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '')
			|| location.hostname == this.hostname) {
            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                $('html,body').animate({
                    scrollTop: target.offset().top - 185
                }, 800);
                return false;
            }
        }
    });
    $('ul#cat_nav li a').on('click', function () {
        $('ul#cat_nav li a.active').removeClass('active');
        $(this).addClass('active');
    });


    //Details page scroll to M-A
    $('a.scroll-to').on('click', function (event) {
        var target = $($(this).data("scroll-to"));

        //remove active class
        $("#DetailsPageActions li").removeClass("active");
        $(this).parent().addClass("active");

        if (target.length) {
            event.preventDefault();
            $('html, body').animate({
                'scrollTop': target.offset().top - 150
            }, 800, 'swing');
        }
    });

    $('.modal').on('shown.bs.modal', function () {
        $(this).appendTo("body")
    });

})(window.jQuery);