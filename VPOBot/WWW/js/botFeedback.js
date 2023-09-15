let tg = window.Telegram?.WebApp;

if (tg) {
    tg.expand();

    let form = document.getElementById("bot-feedback-form");

    form.addEventListener("submit", (event) => {
        event.preventDefault();

        // Создаем объект для ответов
        let answers = {};

        // Заполняем объект ответами из полей формы
        for (let i = 1; i <= 20; i++) {
            let answer = document.getElementById(`answer_${i}`).value;

            if (answer) {
                // Если поле существует, значит, вопрос есть, и мы записываем в него вопрос
                answers[`answer${i}`] = answer;

            } else {
                // Если поле не существует, значит, вопросов больше нет, и мы завершаем цикл

                break;
            }
        }


        let formType = document.getElementById("formType").innerText;
        let callBackMethod = document.getElementById("callBackMethod").value;

        let messageDataInfoType = {
            formType: formType,
            callBackMethod: callBackMethod,
        }

        let formData = {
            answers: answers
        };

        let jsonArray = [messageDataInfoType, formData];
        let jsonString = JSON.stringify(jsonArray);

        tg.sendData(jsonString);

        form.reset();

        tg.close();
    });
}
