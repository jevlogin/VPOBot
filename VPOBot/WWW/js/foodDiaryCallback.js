let tg = window.Telegram?.WebApp;

if (tg) {
    tg.expand();

    let form = document.getElementById("food-diary");

    form.addEventListener("submit", (event) => {
        event.preventDefault();

        let formType = document.getElementById("formType").value;
        let date = document.getElementById("date").value;
        let weight = document.getElementById("weight").value;
        let mealTimeHours = document.getElementById("mealTimeHours").value;
        let mealTimeMinutes = document.getElementById("mealTimeMinutes").value;

        let mealTypeRadios = document.querySelectorAll("input[name='mealType']");
        let mealType = "";
        for (const radio of mealTypeRadios) {
            if (radio.checked) {
                mealType = radio.value;
                break;
            }
        }

        let waterAmount = document.getElementById("waterAmount").value;
        let dishName = document.getElementById("dishName").value;

        let eatPurposeRadios = document.querySelectorAll("input[name='eatPurpose']");
        let eatPurposeType = "";
        for (const radio of eatPurposeRadios) {
            if (radio.checked) {
                eatPurposeType = radio.value;
                break;
            }
        }

        let eatPurpose = "";
        if (eatPurposeType === "hunger") {
            eatPurpose = "Hunger"
        } else if (eatPurposeType === "health") {
            eatPurpose = "Health"
        } else if (eatPurposeType === "weight") {
            eatPurpose = "Weight"
        } else {
            eatPurpose = "None";
        }

        let proteinCheck = document.getElementById("proteinCheck").checked;
        let proteinAmount = document.getElementById("proteinAmount").value;
        let fatCheck = document.getElementById("fatCheck").checked;
        let fatAmount = document.getElementById("fatAmount").value;
        let carbohydrateCheck = document.getElementById("carbohydrateCheck").checked;
        let carbohydrateAmount = document.getElementById("carbohydrateAmount").value;
        let cost = document.getElementById("cost").value;


        let dishType;
        if (mealType === "food") {
            dishType = "Eat";
        } else if (mealType === "water") {
            dishType = "Water";
            eatPurpose = "Health";
        } else {
            dishType = "None";
        }

        let messageDataInfoType = {
            formType: formType,
            callBackMethod: "FoodDiaryFilling",
        }

        let formData = {
            id: 0,
            userID: 0,
            date: date,
            weight: weight,
            mealTimeHours: mealTimeHours,
            mealTimeMinutes: mealTimeMinutes,
            mealType: dishType,
            waterAmount: waterAmount,
            dishName: dishName,
            eatPurpose: eatPurpose,
            proteinCheck: proteinCheck,
            proteinAmount: proteinAmount,
            fatCheck: fatCheck,
            fatAmount: fatAmount,
            carbohydrateCheck: carbohydrateCheck,
            carbohydrateAmount: carbohydrateAmount,
            cost: cost,
        };

        let jsonArray = [messageDataInfoType, formData];
        let jsonString = JSON.stringify(jsonArray);

        tg.sendData(jsonString);

        form.reset();

        tg.close();
    });
}