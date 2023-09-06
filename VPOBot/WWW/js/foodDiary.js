document.addEventListener("DOMContentLoaded", function () {
    const mealTypeRadios = document.querySelectorAll("input[name='mealType']");
    const foodFields = document.getElementById("foodFields");
    const waterAmountField = document.getElementById("waterAmountField");

    const proteinAmountField = document.getElementById("proteinAmountField");
    const fatAmountField = document.getElementById("fatAmountField");
    const carbohydrateAmountField = document.getElementById("carbohydrateAmountField");

    const proteinAmountInput = document.getElementById("proteinAmount");
    const fatAmountInput = document.getElementById("fatAmount");
    const carbohydrateAmountInput = document.getElementById("carbohydrateAmount");

    function setInitialValuesAndRequired() {
        if (proteinCheck.checked) {
            proteinAmountField.style.display = "block";
            proteinAmountInput.setAttribute("required", "required");
        } else {
            proteinAmountField.style.display = "none";
            proteinAmountInput.removeAttribute("required");
        }

        if (fatCheck.checked) {
            fatAmountField.style.display = "block";
            fatAmountInput.setAttribute("required", "required");
        } else {
            fatAmountField.style.display = "none";
            fatAmountInput.removeAttribute("required");
        }

        if (carbohydrateCheck.checked) {
            carbohydrateAmountField.style.display = "block";
            carbohydrateAmountInput.setAttribute("required", "required");
        } else {
            carbohydrateAmountField.style.display = "none";
            carbohydrateAmountInput.removeAttribute("required");
        }
    }

    const proteinCheck = document.getElementById("proteinCheck");
    const fatCheck = document.getElementById("fatCheck");
    const carbohydrateCheck = document.getElementById("carbohydrateCheck");

    setInitialValuesAndRequired();

    proteinCheck.addEventListener("change", setInitialValuesAndRequired);
    fatCheck.addEventListener("change", setInitialValuesAndRequired);
    carbohydrateCheck.addEventListener("change", setInitialValuesAndRequired);

    mealTypeRadios.forEach(function (radio) {
        radio.addEventListener("change", function () {
            if (radio.value === "food") {
                foodFields.style.display = "block";
                waterAmountField.style.display = "none";
                waterAmount.removeAttribute("required");
                document.getElementById("dishName").setAttribute("required", "required");
                document.getElementById("eatForHunger").setAttribute("required", "required");
            } else if (radio.value === "water") {
                foodFields.style.display = "none";
                proteinAmountField.style.display = "none";
                fatAmountField.style.display = "none";
                carbohydrateAmountField.style.display = "none";
                waterAmountField.style.display = "block";
                waterAmount.setAttribute("required", "required");
                document.getElementById("dishName").removeAttribute("required");
                document.getElementById("eatForHunger").removeAttribute("required");
            }
        });
    });

    const dateInput = document.getElementById("date");
    const today = new Date();

    const year = today.getFullYear();
    const month = (today.getMonth() + 1).toString().padStart(2, "0");
    const day = today.getDate().toString().padStart(2, "0");
    const currentDate = `${year}-${month}-${day}`;

    dateInput.value = currentDate;

    const hours = today.getHours().toString().padStart(2, "0");
    const minutes = today.getMinutes().toString().padStart(2, "0");
    const currentTime = `${hours}:${minutes}`;

    document.getElementById("mealTimeHours").value = hours;
    document.getElementById("mealTimeMinutes").value = minutes;
});
