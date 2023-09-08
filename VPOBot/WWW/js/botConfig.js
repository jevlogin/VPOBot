let tg = window.Telegram?.WebApp;

if (tg) {
    tg.expand();

    let form = document.getElementById("bot-settings-form");

    form.addEventListener("submit", (event) => {
        event.preventDefault();

        let formType = document.getElementById("formType").innerText;
        let callBackMethod = document.getElementById("callBackMethod").value;

        let morningTime = document.getElementById("morning-time").value;
        let eveningTime = document.getElementById("evening-time").value;

        let messageDataInfoType = {
            formType: formType,
            callBackMethod: callBackMethod,
        }

        let formData = {
            morningTime: morningTime,
            eveningTime: eveningTime
        };

        let jsonArray = [messageDataInfoType, formData];
        let jsonString = JSON.stringify(jsonArray);

        tg.sendData(jsonString);

        form.reset();

        tg.close();
    });
}
