document.addEventListener("DOMContentLoaded", function () {
    let tg = window.Telegram?.WebApp;

    if (tg) {
        tg.expand();

        let form = document.getElementById("bot-feedback-form");

        form.addEventListener("submit", (event) => {
            event.preventDefault();

            let responses = [];

            let questions = document.querySelectorAll(".form-label");
            for (let i = 0; i < questions.length; i++) {
                let questionText = questions[i].textContent.trim();
                let answerInput = document.getElementById(`answer_${i + 1}`);
                if (answerInput) {
                    let answer = answerInput.value;
                    if (answer) {
                        responses.push({
                            question: questionText,
                            answer: answer
                        });
                    }
                }
            }

            let responseData = {
                ResponseDataId: 0,
                Responses: responses,
                ResponseId: 0,
                FeedbackResponse: null,
            }

            let formType = document.getElementById("formType").innerText;
            let callBackMethod = document.getElementById("callBackMethod").value;

            let messageDataInfoType = {
                formType: formType,
                callBackMethod: callBackMethod,
            }

            let currentDate = new Date();
            let currentDateTimeString = currentDate.toISOString();

            let formData = {
                ResponseId: 0,
                ResponseDateTime: currentDateTimeString,
                UserId: 0,
                ResponseData: responseData,
                User: null,
            };

            let jsonArray = [messageDataInfoType, formData];
            let jsonString = JSON.stringify(jsonArray);

            tg.sendData(jsonString);

            form.reset();

            tg.close();
        });
    }
});