(function ($) {
  ('use strict');
  // Page loading
  $(window).on('load', function () {
    $('#preloader-active').fadeOut('slow');
  });
  /*-----------------
        Menu Stick
    -----------------*/
  var header = $('.sticky-bar');
  var win = $(window);
  win.on('scroll', function () {
    var scroll = win.scrollTop();
    if (scroll < 200) {
      header.removeClass('stick');
      $('.header-style-2 .categories-dropdown-active-large').removeClass('open');
      $('.header-style-2 .categories-button-active').removeClass('open');
    } else {
      header.addClass('stick');
    }
  });
  /*------ ScrollUp -------- */
  $.scrollUp({
    scrollText: '<svg width="20" height="25" viewBox="0 0 20 25" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M10.5998 24.2694C10.7522 24.1169 10.8485 23.9083 10.8485 23.6676L10.8503 1.20328C10.8504 0.737952 10.4653 0.35288 9.99997 0.352917C9.53464 0.352955 9.14951 0.738087 9.14947 1.20342L9.14766 23.6678C9.14762 24.1331 9.53269 24.5182 9.99802 24.5181C10.2387 24.5181 10.4473 24.4218 10.5998 24.2694Z" fill=""/><path d="M18.8405 10.0441C19.1695 9.71509 19.1695 9.16953 18.8406 8.84061L10.6017 0.601675C10.2728 0.272759 9.7272 0.272803 9.39823 0.601772L1.15796 8.84204C0.828992 9.17101 0.828948 9.71657 1.15786 10.0455C1.48678 10.3744 2.03234 10.3744 2.36131 10.0454L9.99981 2.40689L17.6371 10.0442C17.966 10.3731 18.5115 10.373 18.8405 10.0441Z" fill=""/></svg>',
    easingType: 'linear',
    scrollSpeed: 900,
    animation: 'fade',
  });
  //sidebar sticky
  if ($('.sticky-sidebar').length) {
    $('.sticky-sidebar').theiaStickySidebar();
  }
  //Header search form
  $(document).on('click', function (event) {
    var menu_text = $('.menu-texts');
    var btnOpen = $('.btn-search');
    var formSearch = $('.form-search');
    var _ele_lang = $('.icon-lang');
    var _ele_currency = $('.icon-cart');
    if (!_ele_lang.is(event.target) && _ele_lang.has(event.target).length === 0) {
      $('.dropdown-account').removeClass('dropdown-open');
    }
    if (!_ele_currency.is(event.target) && _ele_currency.has(event.target).length === 0) {
      $('.dropdown-cart').removeClass('dropdown-open');
    }
    if (!menu_text.is(event.target) && menu_text.has(event.target).length === 0) {
      menu_text.addClass('menu-close');
      menu_text.css('style', '');
    }
    if (!formSearch.is(event.target) && formSearch.has(event.target).length === 0 && !btnOpen.is(event.target) && btnOpen.has(event.target).length === 0) {
      $('.form-search').slideUp();
    }
  });

  // btn search
  $('.btn-search').on('click', function (e) {
    e.preventDefault();
    var _form_search = $('.form-search');
    if (_form_search.css('display') == 'none') {
      _form_search.slideDown();
    } else {
      _form_search.slideUp();
    }
  });
  /*----------------------------
        Category toggle function
    ------------------------------*/
  if ($('.categories-button-active').length) {
    var searchToggle = $('.categories-button-active');
    searchToggle.on('click', function (e) {
      e.preventDefault();
      if ($(this).hasClass('open')) {
        $(this).removeClass('open');
        $(this).siblings('.categories-dropdown-active-large').removeClass('open');
      } else {
        $(this).addClass('open');
        $(this).siblings('.categories-dropdown-active-large').addClass('open');
      }
    });
  }
  /*------ Wow Active ----*/
  new WOW().init();
  /*---------------------
        Select active
    --------------------- */
  if ($('.select-active').length) {
    $('.select-active').select2();
  }

  // Isotope active
  if ($('.grid').length) {
    $('.grid').imagesLoaded(function () {
      // init Isotope
      var $grid = $('.grid').isotope({
        itemSelector: '.grid-item',
        percentPosition: true,
        layoutMode: 'masonry',
        masonry: {
          // use outer width of grid-sizer for columnWidth
          columnWidth: '.grid-item',
        },
      });
    });
  }
  /*====== SidebarSearch ======*/
  function sidebarSearch() {
    var searchTrigger = $('.search-active'),
      endTriggersearch = $('.search-close'),
      container = $('.main-search-active');
    searchTrigger.on('click', function (e) {
      e.preventDefault();
      container.addClass('search-visible');
    });
    endTriggersearch.on('click', function () {
      container.removeClass('search-visible');
    });
  }
  sidebarSearch();
  /*====== Sidebar menu Active ======*/
  function mobileHeaderActive() {
    var navbarTrigger = $('.burger-icon'),
      navCanvas = $('.burger-icon-2'),
      closeCanvas = $('.close-canvas'),
      endTrigger = $('.mobile-menu-close'),
      container = $('.mobile-header-active'),
      containerCanvas = $('.sidebar-canvas-wrapper'),
      wrapper4 = $('body');
    wrapper4.prepend('<div class="body-overlay-1"></div>');
    navbarTrigger.on('click', function (e) {
      navbarTrigger.toggleClass('burger-close');
      e.preventDefault();
      container.toggleClass('sidebar-visible');
      wrapper4.toggleClass('mobile-menu-active');
    });
    navCanvas.on('click', function (e) {
      navCanvas.toggleClass('burger-2-close');
      e.preventDefault();
      containerCanvas.toggleClass('sidebar-canvas-visible');
      wrapper4.toggleClass('canvas-menu-active');
    });
    closeCanvas.on('click', function (e) {
      containerCanvas.removeClass('sidebar-canvas-visible');
      wrapper4.removeClass('canvas-menu-active');
      navCanvas.removeClass('burger-2-close');
    });
    endTrigger.on('click', function () {
      container.removeClass('sidebar-visible');
      wrapper4.removeClass('mobile-menu-active');
    });
    $('.body-overlay-1').on('click', function () {
      container.removeClass('sidebar-visible');
      containerCanvas.removeClass('sidebar-canvas-visible');
      wrapper4.removeClass('mobile-menu-active');
      wrapper4.removeClass('canvas-menu-active');
      navbarTrigger.removeClass('burger-close');
      navCanvas.removeClass('burger-2-close');
    });
  }
  mobileHeaderActive();
  /*---------------------
        Mobile menu active
    ------------------------ */
  var $offCanvasNav = $('.mobile-menu'),
    $offCanvasNavSubMenu = $offCanvasNav.find('.sub-menu');
  /*Add Toggle Button With Off Canvas Sub Menu*/
  $offCanvasNavSubMenu.parent().prepend('<span class="menu-expand"><i class="arrow-small-down"></i></span>');
  /*Close Off Canvas Sub Menu*/
  $offCanvasNavSubMenu.slideUp();
  /*Category Sub Menu Toggle*/
  $offCanvasNav.on('click', 'li a, li .menu-expand', function (e) {
    var $this = $(this);
    if (
      $this
        .parent()
        .attr('class')
        .match(/\b(menu-item-has-children|has-children|has-sub-menu)\b/) &&
      ($this.attr('href') === '#' || $this.hasClass('menu-expand'))
    ) {
      e.preventDefault();
      if ($this.siblings('ul:visible').length) {
        $this.parent('li').removeClass('active');
        $this.siblings('ul').slideUp();
      } else {
        $this.parent('li').addClass('active');
        $this.closest('li').siblings('li').removeClass('active').find('li').removeClass('active');
        $this.closest('li').siblings('li').find('ul:visible').slideUp();
        $this.siblings('ul').slideDown();
      }
    }
  });
  /*--- language currency active ----*/
  $('.mobile-language-active').on('click', function (e) {
    e.preventDefault();
    $('.lang-dropdown-active').slideToggle(900);
  });
  /*--- categories-button-active-2 ----*/
  $('.categories-button-active-2').on('click', function (e) {
    e.preventDefault();
    $('.categori-dropdown-active-small').slideToggle(900);
  });
  /*--- Mobile demo active ----*/
  var demo = $('.tm-demo-options-wrapper');
  $('.view-demo-btn-active').on('click', function (e) {
    e.preventDefault();
    demo.toggleClass('demo-open');
  });
  /*-----More Menu Open----*/
  $('.more_slide_open').slideUp();
  $('.more_categories').on('click', function () {
    $(this).toggleClass('show');
    $('.more_slide_open').slideToggle();
  });
  /* --- SwiperJS --- */
  $('.swiper-group-8').each(function () {
    var swiper_8_items = new Swiper(this, {
      slidesPerView: 8,
      spaceBetween: 10,
      slidesPerGroup: 1,
      loop: true,
      navigation: {
        nextEl: '.swiper-button-next-group-8',
        prevEl: '.swiper-button-prev-group-8',
      },
      autoplay: {
        delay: 10000,
      },
      breakpoints: {
        1399: {
          slidesPerView: 8,
        },
        1199: {
          slidesPerView: 6,
        },
        992: {
          slidesPerView: 5,
        },
        800: {
          slidesPerView: 4,
        },
        650: {
          slidesPerView: 4,
        },
        400: {
          slidesPerView: 2,
        },
        250: {
          slidesPerView: 2,
          slidesPerGroup: 1,
          spaceBetween: 15,
        },
      },
    });
  });
  $('.swiper-group-6').each(function () {
    var swiper_6_items = new Swiper(this, {
      spaceBetween: 30,
      slidesPerView: 6,
      spaceBetween: 30,
      slidesPerGroup: 2,
      loop: true,
      navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
      },
      autoplay: {
        delay: 10000,
      },
      breakpoints: {
        1599: {
          slidesPerView: 6,
        },
        1499: {
          slidesPerView: 5,
        },
        1299: {
          slidesPerView: 4,
        },
        800: {
          slidesPerView: 3,
        },
        400: {
          slidesPerView: 2,
        },
        350: {
          slidesPerView: 1,
          slidesPerGroup: 1,
          spaceBetween: 15,
        },
      },
    });
  });

  $('.swiper-group-5').each(function () {
    var swiper_6_items = new Swiper(this, {
      spaceBetween: 30,
      slidesPerView: 5,
      spaceBetween: 30,
      slidesPerGroup: 1,
      loop: true,
      navigation: {
        nextEl: '.swiper-button-next-group-5',
        prevEl: '.swiper-button-prev-group-5',
      },
      autoplay: {
        delay: 10000,
      },
      breakpoints: {
        1599: {
          slidesPerView: 5,
        },
        1499: {
          slidesPerView: 5,
        },
        1299: {
          slidesPerView: 4,
        },
        800: {
          slidesPerView: 3,
        },
        400: {
          slidesPerView: 2,
        },
        250: {
          slidesPerView: 1,
          slidesPerGroup: 1,
          spaceBetween: 15,
        },
      },
    });
  });

  $('.swiper-group-4').each(function () {
    var swiper_4_items = new Swiper(this, {
      spaceBetween: 30,
      slidesPerView: 4,
      spaceBetween: 30,
      slidesPerGroup: 2,
      loop: true,
      navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
      },
      autoplay: {
        delay: 10000,
      },
      breakpoints: {
        1199: {
          slidesPerView: 4,
        },
        800: {
          slidesPerView: 3,
        },
        500: {
          slidesPerView: 2,
        },
        350: {
          slidesPerView: 1,
        },
        250: {
          slidesPerView: 1,
        },
      },
      on: {
        afterInit: function () {
          // set padding left slide fleet
          var leftTitle = 0;
          var titleFleet = $('.container');
          if (titleFleet.length > 0) {
            leftTitle = titleFleet.offset().left;
          }
          if ($('.box-swiper-padding').length > 0) {
            $('.box-swiper-padding').css('padding-left', leftTitle + 'px');
          }
        },
      },
    });
  });

  $('.swiper-group-3').each(function () {
    var swiper_3_items = new Swiper(this, {
      spaceBetween: 24,
      slidesPerView: 3,
      spaceBetween: 30,
      slidesPerGroup: 1,
      loop: true,
      navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
      },
      autoplay: {
        delay: 10000,
      },
      breakpoints: {
        1199: {
          slidesPerView: 3,
        },
        800: {
          slidesPerView: 2,
        },
        400: {
          slidesPerView: 1,
        },
        250: {
          slidesPerView: 1,
        },
      },
    });
  });

  $('.swiper-group-2').each(function () {
    var swiper_2_items = new Swiper(this, {
      slidesPerView: 2,
      spaceBetween: 30,
      slidesPerGroup: 1,
      loop: true,
      navigation: {
        nextEl: '.swiper-button-next-2',
        prevEl: '.swiper-button-prev-2',
      },
      autoplay: {
        delay: 10000,
      },
      breakpoints: {
        1199: {
          slidesPerView: 2,
        },
        800: {
          slidesPerView: 1,
        },
        400: {
          slidesPerView: 1,
        },
        250: {
          slidesPerView: 1,
        },
      },
    });
  });

  $('.swiper-group-1').each(function () {
    var swiper_1_item = new Swiper(this, {
      slidesPerView: 1,
      spaceBetween: 50,
      slidesPerGroup: 1,
      loop: true,
      navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
      },
      pagination: {
        el: '.swiper-pagination-group-1',
        clickable: true,
      },
      autoplay: {
        delay: 100000,
      },
    });
  });

  $('.swiper-group-testimonials-1').each(function () {
    var swiper_1_item = new Swiper(this, {
      slidesPerView: 1,
      spaceBetween: 50,
      slidesPerGroup: 1,
      loop: true,
      navigation: {
        nextEl: '.swiper-button-next-group-1',
        prevEl: '.swiper-button-prev-group-1',
      },
      pagination: {
        el: '.swiper-pagination-testimonials-1',
        clickable: true,
      },
      autoplay: {
        delay: 100000,
      },
    });
  });

  $('.swiper-group-payment').each(function () {
    var swiper_payment_items = new Swiper(this, {
      spaceBetween: 10,
      slidesPerView: 4,
      slidesPerGroup: 1,
      loop: true,
      navigation: {
        nextEl: '.swiper-button-next-payment',
        prevEl: '.swiper-button-prev-payment',
      },
      autoplay: {
        delay: 5000,
      },
      breakpoints: {
        1199: {
          slidesPerView: 4,
        },
        800: {
          slidesPerView: 4,
        },
        500: {
          slidesPerView: 4,
        },
        350: {
          slidesPerView: 3,
        },
        320: {
          slidesPerView: 2,
        },
        250: {
          slidesPerView: 2,
        },
      },
    });
  });

  $('.swiper-group-payment-9').each(function () {
    var swiper_payment_items = new Swiper(this, {
      spaceBetween: 20,
      slidesPerView: 9,
      slidesPerGroup: 1,
      loop: true,
      navigation: {
        nextEl: '.swiper-button-next-payment',
        prevEl: '.swiper-button-prev-payment',
      },
      autoplay: {
        delay: 5000,
      },
      breakpoints: {
        1199: {
          slidesPerView: 9,
        },
        800: {
          slidesPerView: 7,
        },
        650: {
          slidesPerView: 6,
        },
        575: {
          slidesPerView: 5,
        },
        450: {
          slidesPerView: 3,
        },
        320: {
          slidesPerView: 3,
        },
        250: {
          slidesPerView: 2,
        },
      },
    });
  });

  $('.swiper-group-payment-10').each(function () {
    var swiper_payment_items = new Swiper(this, {
      spaceBetween: 20,
      slidesPerView: 10,
      slidesPerGroup: 2,
      loop: true,
      navigation: {
        nextEl: '.swiper-button-next-payment',
        prevEl: '.swiper-button-prev-payment',
      },
      autoplay: {
        delay: 5000,
      },
      breakpoints: {
        1199: {
          slidesPerView: 10,
        },
        800: {
          slidesPerView: 8,
        },
        650: {
          slidesPerView: 6,
        },
        575: {
          slidesPerView: 5,
        },
        450: {
          slidesPerView: 3,
        },
        320: {
          slidesPerView: 3,
        },
        250: {
          slidesPerView: 2,
        },
      },
    });
  });

  $('.swiper-group-payment-7').each(function () {
    var swiper_payment_items = new Swiper(this, {
      spaceBetween: 20,
      slidesPerView: 7,
      slidesPerGroup: 1,
      loop: true,
      navigation: {
        nextEl: '.swiper-button-next-payment',
        prevEl: '.swiper-button-prev-payment',
      },
      autoplay: {
        delay: 5000,
      },
      breakpoints: {
        1199: {
          slidesPerView: 7,
        },
        800: {
          slidesPerView: 6,
        },
        650: {
          slidesPerView: 5,
        },
        575: {
          slidesPerView: 4,
        },
        450: {
          slidesPerView: 3,
        },
        320: {
          slidesPerView: 3,
        },
        250: {
          slidesPerView: 2,
        },
      },
    });
  });
  $('.swiper-group-testimonials').each(function () {
    var swiper_items_testimonials = new Swiper(this, {
      spaceBetween: 30,
      slidesPerView: 2,
      spaceBetween: 30,
      slidesPerGroup: 1,
      loop: true,
      navigation: {
        nextEl: '.swiper-button-next-testimonials',
        prevEl: '.swiper-button-prev-testimonials',
      },
      autoplay: {
        delay: 10000,
      },
      breakpoints: {
        1199: {
          slidesPerView: 2,
        },
        1000: {
          slidesPerView: 1,
        },
        400: {
          slidesPerView: 1,
        },
        350: {
          slidesPerView: 1,
        },
      },
      on: {
        afterInit: function () {
          // set padding left slide fleet
          var leftTitle = 0;
          var titleFleet = $('.container');
          if (titleFleet.length > 0) {
            leftTitle = titleFleet.offset().left;
          }
          if ($('.box-swiper-padding').length > 0) {
            $('.box-swiper-padding').css('padding-left', leftTitle + 'px');
          }
        },
      },
    });
  });
  var swiper_animate_items = null;
  $('.swiper-group-animate').each(function () {
    swiper_animate_items = new Swiper(this, {
      spaceBetween: 24,
      slidesPerView: 'auto',
      slidesPerGroup: 1,
      loop: true,
      speed: 1000,
      navigation: {
        nextEl: '.swiper-button-next-animate',
        prevEl: '.swiper-button-prev-animate',
      },
      autoplay: {
        delay: 10000,
      },
      on: {
        afterInit: function () {
          // set padding left slide fleet
          var leftTitle = 15;
          var titleFleet = $('.container');
          if (titleFleet.length > 0) {
            leftTitle = titleFleet.offset().left + 15;
          }
          if ($('.box-swiper-padding').length > 0) {
            $('.box-swiper-padding').css('padding-left', leftTitle + 'px');
          }
        },
      },
      breakpoints: {
        1199: {
          slidesPerView: 'auto',
        },
        600: {
          slidesPerView: 'auto',
        },
        575: {
          slidesPerView: 1,
        },
        350: {
          slidesPerView: 1,
        },
      },
    });
  });

  var swiper_center = new Swiper('.swiper-center', {
    navigation: {
      nextEl: '.swiper-button-next-center',
      prevEl: '.swiper-button-prev-center',
    },
    slidesPerView: 2,
    centeredSlides: true,
    paginationClickable: true,
    loop: true,
    spaceBetween: 16,
    slideToClickedSlide: true,
  });

  var swiper_center_4 = new Swiper('.swiper-group-center-4', {
    navigation: {
      nextEl: '.swiper-button-next-center-4',
      prevEl: '.swiper-button-prev-center-4',
    },
    slidesPerView: 'auto',
    watchOverflow: true,
    // centeredSlides: true,
    // paginationClickable: true,
    loop: true,
    spaceBetween: 18,
    slideToClickedSlide: true,
  });

  //Dropdown selected item
  $('.dropdown-menu li a').on('click', function (e) {
    e.preventDefault();
    $(this)
      .parents('.dropdown')
      .find('.btn span')
      .html($(this).text() + ' <span class="caret"></span>');
    $(this).parents('.dropdown').find('.btn').val($(this).data('value'));
  });
  $('.list-tags-job .remove-tags-job').on('click', function (e) {
    e.preventDefault();
    $(this).closest('.job-tag').remove();
  });
  // Video popup
  if ($('.popup-youtube').length) {
    $('.popup-youtube').magnificPopup({
      type: 'iframe',
      mainClass: 'mfp-fade',
      removalDelay: 160,
      preloader: false,
      fixedContentPos: false,
    });
  }

  $('.cb-plan').on('click', function () {
    var _parents = $(this).parents('li');
    $('.list-choose-plan li').removeClass('active');
    _parents.addClass('active');
    var val = this.value;
    switchBilledType2(val);
  });
  if ($('.box-pricing-2').length > 0 || $('.box-pricing').length > 0) {
    switchBilled();
  }
  if ($('.box-choose-plan').length > 0) {
    switchBilledType2(1);
  }

  $(window)
    .resize(function () {
      var _pd_left = $('.padding-left-auto');
      var _pd_right = $('.padding-right-auto');
      var _container = $('.container');
      var _offset_left = _container.offset().left;
      var _container2 = $('.container-top .container');
      if (_container2.length > 0) {
        var _offset_left2 = _container2.offset().left;
        $('.slider-thumnail').css('right', '' + _offset_left2 + 'px');
        setTimeout(function () {
          var _prev_slide = $('.banner-main .slick-prev');
          var _next_slide = $('.banner-main .slick-next');
          _offset_left2 = _offset_left2 + 15;
          _prev_slide.css('left', '' + _offset_left2 + 'px');
          _next_slide.css('left', '' + (_offset_left2 + 45) + 'px');
        }, 200);
      }

      _pd_left.css('padding-left', '' + _offset_left + 'px');
      _pd_right.css('padding-right', '' + _offset_left + 'px');
      $('.box-comming-soon-banner').css('padding-left', '' + _offset_left + 'px');
    })
    .resize();
  var msnry = null;
  if ($('#masonry').length > 0) {
    msnry = new Masonry('#masonry', {
      itemSelector: '.col-lg-4',
      percentPosition: true,
    });
  }
  $('.box-button-filters a.btn').on('click', function (e) {
    e.preventDefault();
    var _filter = $(this).attr('data-filter');
    $('.box-button-filters a.btn').removeClass('active');
    $(this).addClass('active');
    if (_filter == 'all') {
      $('.item-filter').show();
    } else {
      $('.item-filter').hide();
      $('.' + _filter).show();
    }
    msnry.layout();
  });

  /*------ Timer Countdown ----*/
  $('[data-countdown]').each(function () {
    var $this = $(this),
      finalDate = $(this).data('countdown');
    $this.countdown(finalDate, function (event) {
      $(this).html(event.strftime('' + '<span class="countdown-section"><span class="countdown-amount font-sm-bold lh-16">%D</span><span class="countdown-period lh-14 font-xs"> days </span></span>' + '<span class="countdown-section"><span class="countdown-amount font-sm-bold lh-16">%H</span><span class="countdown-period font-xs lh-14"> hours </span></span>' + '<span class="countdown-section"><span class="countdown-amount font-sm-bold lh-16">%M</span><span class="countdown-period font-xs lh-14"> mins </span></span>' + '<span class="countdown-section"><span class="countdown-amount font-sm-bold lh-16">%S</span><span class="countdown-period font-xs lh-14"> secs </span></span>'));
    });
  });
  var _container_left = $('.container').offset().left + 15;
  $('.block-testimonials').css('padding-left', _container_left + 'px');

  $('.icon-account').on('click', function () {
    $('.dropdown-cart').removeClass('dropdown-open');
    $(this).next('.dropdown-account').toggleClass('dropdown-open');
  });

  $('.icon-cart').on('click', function () {
    $('.dropdown-account').removeClass('dropdown-open');
    $(this).next('.dropdown-cart').toggleClass('dropdown-open');
  });

  $('.btn-click').on('click', function (e) {
    e.preventDefault();
    $('.btn-click').removeClass('active');
    $(this).addClass('active');
  });

  $('#calendar-events').datepicker({});

  $('.calendar-date').each(function () {
    $(this).datepicker({
      format: 'dd/mm/yyyy',
      autoclose: true,
      orientation: 'bottom',
    });
  });

  // datepicker
  $('.datepicker').each(function () {
    $(this)
      .datepicker({
        format: 'dd MM yyyy',
        autoclose: true,
        templates: {
          leftArrow: '',
          rightArrow: '',
        },
        orientation: 'bottom',
      })
      .attr('readonly', 'readonly');
  });

  $('.close-popup').on('click', function (e) {
    $('.popup-firstload').hide();
  });

  $('.close-popup-signin').on('click', function (e) {
    $('.popup-signin').hide();
  });

  $('.close-popup-signup').on('click', function (e) {
    $('.popup-signup').hide();
  });

  $('.banner-main').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: true,
    fade: false,
    asNavFor: '.slider-nav-thumbnails',
    prevArrow: '<button type="button" class="slick-prev"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewbox="0 0 16 16" fill="none"><path d="M7.99992 3.33325L3.33325 7.99992M3.33325 7.99992L7.99992 12.6666M3.33325 7.99992H12.6666" stroke="" stroke-linecap="round" stroke-linejoin="round"></path></svg></button>',
    nextArrow: '<button type="button" class="slick-next"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewbox="0 0 16 16" fill="none"><path d="M7.99992 12.6666L12.6666 7.99992L7.99992 3.33325M12.6666 7.99992L3.33325 7.99992" stroke="" stroke-linecap="round" stroke-linejoin="round"> </path></svg></button>',
  });

  $('.slider-nav-thumbnails').slick({
    slidesToShow: 3,
    slidesToScroll: 1,
    asNavFor: '.banner-main',
    dots: false,
    focusOnSelect: true,
    vertical: true,
    prevArrow: '<button type="button" class="slick-prev"><svg class="w-6 h-6 icon-16" fill="none" stroke="currentColor" viewbox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg></button>',
    nextArrow: '<button type="button" class="slick-next"><svg class="w-6 h-6 icon-16" fill="none" stroke="currentColor" viewbox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14 5l7 7m0 0l-7 7m7-7H3"></path></svg></button>',
  });

  $('.banner-activities-detail').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: true,
    fade: false,
    // asNavFor: '.slider-nav-thumbnails-activities-detail',
    prevArrow: '<button type="button" class="slick-prev"><svg width="12" height="12" viewBox="0 0 12 12" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M5.99967 1.33325L1.33301 5.99992M1.33301 5.99992L5.99967 10.6666M1.33301 5.99992H10.6663" stroke="" stroke-linecap="round" stroke-linejoin="round"/></svg></button>',
    nextArrow: '<button type="button" class="slick-next"><svg width="12" height="12" viewBox="0 0 12 12" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M5.99967 10.6666L10.6663 5.99992L5.99968 1.33325M10.6663 5.99992L1.33301 5.99992" stroke="" stroke-linecap="round" stroke-linejoin="round"/></svg></button>',
  });

  $('.slider-nav-thumbnails-activities-detail').slick({
    slidesToShow: 6,
    slidesToScroll: 1,
    asNavFor: '.banner-activities-detail',
    dots: false,
    focusOnSelect: true,
    vertical: false,
    responsive: [
      {
        breakpoint: 1200,
        settings: {
          slidesToShow: 5,
        },
      },
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 4,
        },
      },
      {
        breakpoint: 700,
        settings: {
          slidesToShow: 3,
        },
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 2,
        },
      },
    ],
    prevArrow: '<button type="button" class="slick-prev"><svg class="w-6 h-6 icon-16" fill="none" stroke="currentColor" viewbox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg></button>',
    nextArrow: '<button type="button" class="slick-next"><svg class="w-6 h-6 icon-16" fill="none" stroke="currentColor" viewbox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14 5l7 7m0 0l-7 7m7-7H3"></path></svg></button>',
  });

  $('.list-checkbox li').on('click', function () {
    var _input = $(this).find('input');
    if (_input.is(':checked')) {
      $(this).addClass('active');
    } else {
      $(this).removeClass('active');
    }
  });

  $('.item-collapse').on('click', function () {
    var _parent = $(this).parents('.block-filter');
    if (_parent.find('.box-collapse').css('display') == 'none') {
      $(this).removeClass('collapsed-item');
      _parent.find('.box-collapse').slideDown();
    } else {
      $(this).addClass('collapsed-item');
      _parent.find('.box-collapse').slideUp();
    }
  });


  if (localStorage.getItem('popState') != 'shown') {
    localStorage.setItem('popState', 'shown');
  } else {
    $('.popup-firstload').hide();
  }

  $('.image-gallery').magnificPopup({
    type: 'image',
    mainClass: 'mfp-with-zoom',
    gallery: {
      enabled: true,
    },

    zoom: {
      enabled: true,

      duration: 300, // duration of the effect, in milliseconds
      easing: 'ease-in-out', // CSS transition easing function

      opener: function (openerElement) {
        return openerElement.is('img') ? openerElement : openerElement.find('img');
      },
    },
  });
  $('.change-price-plan li a').on('click', function (e) {
    e.preventDefault();
    $('.change-price-plan li a').removeClass('active');
    $(this).addClass('active');
    var type = $(this).attr('data-type');
    if (type == 'monthly') {
      $('.text-price-basic').html('19');
      $('.text-type-basic').html('/ month');
      $('.text-price-standard').html('29');
      $('.text-type-standard').html('/ month');
      $('.text-price-business').html('49');
      $('.text-type-business').html('/ month');
      $('.text-price-enterprise').html('99');
      $('.text-type-enterprise').html('/ month');
    } else {
      $('.text-price-basic').html('288');
      $('.text-type-basic').html('/ year');
      $('.text-price-standard').html('348');
      $('.text-type-standard').html('/ year');
      $('.text-price-business').html('588');
      $('.text-type-business').html('/ year');
      $('.text-price-enterprise').html('1.188');
      $('.text-type-enterprise').html('/ year');
    }
  });
})(jQuery);
// Check billed
function switchBilled() {
  var checkBox = $('#cb_billed_type');
  var forMonth = $('.for-month');
  var forYear = $('.for-year');
  var billMonth = $('.text-billed-month');
  var billYear = $('.text-billed-year');
  for (var i = 0; i < forMonth.length; i++) {
    if (checkBox.is(':checked')) {
      forYear.eq(i).addClass('display-year');
      billYear.addClass('active');
      billMonth.removeClass('active');
      forMonth.eq(i).removeClass('display-month');
    } else {
      forYear.eq(i).removeClass('display-year');
      billMonth.addClass('active');
      billYear.removeClass('active');
      forMonth.eq(i).addClass('display-month');
    }
  }
}
function switchBilledType2(val) {
  var forMonth = $('.for-month');
  var forYear = $('.for-year');
  var billMonth = $('.text-billed-month');
  var billYear = $('.text-billed-year');
  for (var i = 0; i < forMonth.length; i++) {
    if (val == 1) {
      forYear.eq(i).addClass('display-year');
      billYear.addClass('active');
      billMonth.removeClass('active');
      forMonth.eq(i).removeClass('display-month');
    } else {
      forYear.eq(i).removeClass('display-year');
      billMonth.addClass('active');
      billYear.removeClass('active');
      forMonth.eq(i).addClass('display-month');
    }
  }
}

//Qty Up-Down
$('.detail-qty').each(function () {
  var qtyval = parseInt($(this).find('.qty-val').val(), 10);
  var $qtyInput = $(this).find('.qty-val');

  $(this)
    .find('.qty-up')
    .on('click', function (event) {
      event.preventDefault();
      qtyval = qtyval + 1;
      $qtyInput.val(qtyval);
    });

  $(this)
    .find('.qty-down')
    .on('click', function (event) {
      event.preventDefault(); /*  */
      qtyval = Math.max(1, qtyval - 1);
      $qtyInput.val(qtyval);
    });
});

//Perfect Scrollbar
const ps = new PerfectScrollbar('.mobile-header-wrapper-inner');
const ps2 = new PerfectScrollbar('.sidebar-canvas-container');

//Carausel slider
function carauselScroll() {
  $('.carouselTicker-left').each(function () {
      $(this).carouselTicker({
          direction: 'prev',
          speed: 1,
          delay: 30,
      });
  });
  $('.carouselTicker-right').each(function () {
      $(this).carouselTicker({
          direction: 'next',
          speed: 1,
          delay: 30,
      });
  });
}
carauselScroll()

function odometerCounter() {
  if ($('.odometer').length > 0) {
      $('.odometer').appear(function (e) {
          var odo = $('.odometer');
          odo.each(function () {
              var countNumber = $(this).attr('data-count');
              $(this).html(countNumber);
          });
      });
  }
}
odometerCounter()