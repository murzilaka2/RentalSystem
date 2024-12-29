// Requires jQuery

// Initialize slider:
$(document).ready(function () {
    $(".noUi-handle").on("click", function () {
        $(this).width(50);
    });
    var rangeSlider = document.getElementById("slider-range");
    var rangeSlider2 = $("#slider-range");
    if (rangeSlider2.length > 0) {
        var moneyFormat = wNumb({
            decimals: 0,
            thousand: ",",
            prefix: ""
        });
        noUiSlider.create(rangeSlider, {
            start: 280,
            tooltips: [wNumb({decimals: 0})],
            step: 1,
            range: {
                min: 0,
                max: 500
            },
            format: moneyFormat,
            connect: 'lower',
            //connect: true
        });

        // Set visual min and max values and also update value hidden form inputs
        rangeSlider.noUiSlider.on("update", function (values, handle) {
            $(".value-money").val(values[0]);
        });
    }
});
