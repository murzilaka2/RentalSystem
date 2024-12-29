function calculateTotalPrice() {
    var totalPriceDom = document.getElementById('total-payable');
    var totalToPayDom = document.getElementById('total-to-pay');
    const pricePerDay = parseFloat(document.getElementById('carPrice').value);
    const startDateInput = document.getElementById('startDate').value;
    const endDateInput = document.getElementById('endDate').value;

    if (!startDateInput || !endDateInput || isNaN(pricePerDay)) {
        if (totalPriceDom) {
            totalPriceDom.innerText = "Incorrect data.";
        }
        return;
    }

    // Разбиваем даты по дефису и создаем объект Date
    const startDateParts = startDateInput.split('-');
    const endDateParts = endDateInput.split('-');

    // Конструктор Date ожидает: new Date(год, месяц (0-11), день)
    const startDate = new Date(startDateParts[0], startDateParts[1] - 1, startDateParts[2]);  // year, month, day
    const endDate = new Date(endDateParts[0], endDateParts[1] - 1, endDateParts[2]);

    // Проверка на корректность дат
    if (startDate > endDate) {
        if (totalPriceDom) {
            totalPriceDom.innerText = "The end date must be later than the start date.";
        }
        return;
    }

    // Разница в днях
    const dayDifference = Math.ceil((endDate - startDate) / (1000 * 60 * 60 * 24)) + 1;

    // Базовая цена
    let totalPrice = (dayDifference - 1) * pricePerDay;

    // Добавление стоимости чекбоксов
    const checkboxes = document.querySelectorAll('.extra-item');
    checkboxes.forEach((checkbox) => {
        if (checkbox.checked) {
            totalPrice += parseFloat(checkbox.getAttribute('data-price'));
        }
    });

    // Выводим итоговую цену
    if (totalPriceDom) {
        totalPriceDom.innerText = `$${totalPrice.toFixed(2)}`;
        if (totalToPayDom) {
            totalToPayDom.value = totalPrice;
        }
    }
}

document.addEventListener('DOMContentLoaded', function () {
    // Обработчики для изменения дат
    document.getElementById('startDate').addEventListener('change', calculateTotalPrice);
    document.getElementById('endDate').addEventListener('change', calculateTotalPrice);

    // Обработчики для изменения чекбоксов
    document.querySelectorAll('.extra-item').forEach((checkbox) => {
        checkbox.addEventListener('change', calculateTotalPrice);
    });

    // Первый расчет при загрузке страницы
    calculateTotalPrice();
});