document.addEventListener('DOMContentLoaded', function () {
    const recipientsContainer = document.getElementById('recipientsContainer');
    const addEmailButton = document.getElementById('addEmailButton');

    addEmailButton.addEventListener('click', function () {
        const inputCount = recipientsContainer.querySelectorAll('.form-group').length;
        const newInputDiv = document.createElement('div');
        newInputDiv.className = 'form-group';
        newInputDiv.id = `recipient-${inputCount}`;
        const newLabel = document.createElement('label');
        newLabel.innerText = `Email ${inputCount + 1}:`;


        const newInput = document.createElement('input');
        newInput.name = `Recipients[${inputCount}]`; // Update the name to match the model binding
        newInput.className = 'form-control';

        const removeButton = document.createElement('button');
        removeButton.type = 'button';
        removeButton.className = 'btn btn-danger btn-sm mt-2 remove-recipient';
        removeButton.innerText = 'Remove';
        removeButton.dataset.index = inputCount;

        removeButton.addEventListener('click', function () {
            if (recipientsContainer.querySelectorAll('.form-group').length > 1) {
                recipientsContainer.removeChild(newInputDiv);
                updateInputNames();
            }
        });

        newInputDiv.appendChild(newLabel);
        newInputDiv.appendChild(newInput);
        newInputDiv.appendChild(removeButton);
        recipientsContainer.appendChild(newInputDiv);
    });

    function updateInputNames() {
        const inputs = recipientsContainer.querySelectorAll('.form-group');
        inputs.forEach((group, index) => {
            const input = group.querySelector('input');
            const label = group.querySelector('label');
            const button = group.querySelector('.remove-recipient');

            if (input) {
                input.name = `Recipients[${index}]`;
            }
            if (label) {
                label.innerText = `Email ${index + 1}:`;
            }
            if (button) {
                button.dataset.index = index;
            }
        });
    }

    const existingRemoveButtons = recipientsContainer.querySelectorAll('.remove-recipient');
    existingRemoveButtons.forEach(button => {
        button.addEventListener('click', function () {
            if (recipientsContainer.querySelectorAll('.form-group').length > 1) {
                const parentDiv = button.parentElement;
                recipientsContainer.removeChild(parentDiv);
                updateInputNames();
            }
        });
    });
});