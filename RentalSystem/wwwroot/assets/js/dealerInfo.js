document.addEventListener('DOMContentLoaded', function () {
    const selectElement = document.getElementById('dealer-select');
    const dealerInfo = document.getElementById('dealer-info');
    const dealerPhoto = document.getElementById('dealer-photo');
    const dealerName = document.getElementById('dealer-name');
    const dealerExperience = document.getElementById('dealer-experience');
    const dealerMobile = document.getElementById('dealer-mobile');
    const dealerEmail = document.getElementById('dealer-email');

    const dealers = JSON.parse(selectElement.dataset.dealers);

    selectElement.addEventListener('change', function () {
        const selectedDealerId = this.value;
        const selectedDealer = dealers.find(dealer => dealer.id == selectedDealerId);

        if (selectedDealer) {
            dealerPhoto.src = selectedDealer.photoUrl;
            dealerName.textContent = `${selectedDealer.firstName} ${selectedDealer.lastName}`;
            dealerExperience.textContent = `Experience: ${selectedDealer.workExperience} years`;
            dealerMobile.innerHTML = `<i class="bi bi-phone"></i> ${selectedDealer.mobile}`;
            dealerEmail.innerHTML = `<i class="bi bi-envelope"></i> ${selectedDealer.email}`;
            dealerInfo.style.display = "block";
        } else {
            dealerInfo.style.display = "none";
        }
    });
});