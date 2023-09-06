﻿namespace WORLDGAMEDEVELOPMENT
{
    internal static class DialogData
    {
        public const string FAILED = "НЕ УДАЧНО\n";
        public const string SUCCESS = "УСПЕШНО\n";

        public const string THE_TECHNOLOGY_IS_UNDER_DEVELOPMENT = "Технология находится на стадии разработки. Скоро появится.\n";

        public const string GOOD_MORNING = "Доброе Утро {0}!\n";
        public const string DIALOG_DAY_2_STEP_1 = "Сегодня я научу тебя как заполнять дневник питания.\n" +
                            "Мы будем делать это постепенно, чтобы ты смог с легкостью все освоить\r\n" +
                            "Я буду всячески тебе помогать в ведении дневника, чтобы это не стало для тебя рутиной, а было только в радость!\r\n\r\n" +
                            "Я понимаю что у каждого человека, утро начинается по разному, но первый день запрограммирован именно на 9 утра.\r\n" +
                            "Спешу тебя обрадовать и несказанно удивить, ты можешь в любой момент меня перепрограммировать.\r\n\r\n" +
                            "Например, ты можешь установить время в которое ты просыпаешься, и которое считаешь утренним,\r\n" +
                            "чтобы я смог напоминать тебе о важных событиях и рационах питания или других физических активностях, в комфортный для тебя час.\r\n\r\n" +
                            "Сделать это очень просто, через специальный пункт меню настройки.\r\n" +
                            "ты готов узнать подробнее и настроить меня для комфортного общения с тобой?\n";

        public const string HERBY_WELCOME_TO_VPO = "Добро пожаловать в <b>VitalityProgram.Online!</b>\n\nМы - команда разработчиков, " +
            "и мы уверены, что наше общение принесет вам радость и пользу! 🌟\r\n";

        public const string HERBY_PROGRAM_PRESENTATION_RIDDLES = "У нас здесь много увлекательных головоломок и интересных задач, " +
            "чтобы помочь вам обрести осознанное здоровье и энергию жизни!\r\n";

        public const string HERBY_IINTRODUCE_YOURSELF = "Привет!\nЯ <b>Герби</b> - ваш личный виртуальный помощник и сопровождающий! <b>Рад встрече!</b>";
        public const string WHAT_IS_YOUR_NAME = "Как вас зовут?";
        public const string CHOOSE_ONE_OF_THE_OPTIONS = "Выберите один из вариантов: ";
        public const string USER_VPO_OFFLINE_DAY_1_STEP_1 = "Мы работаем на результат и готовы помочь Вам менять ваш мир к лучшему!";
        public const string RESULTS_OF_OUR_PARTICIPANTS = "Вот некоторые результаты наших участников:\n\n";
        public const string GEETING_TO_KNOW_HERBY = "А теперь, друзья, позвольте представить вам меня - Герби, вашего виртуального помощника и гида в этом увлекательном путешествии!\n";
        public const string GEETING_TO_KNOW_HERBY_TOGETHER = "Я буду рядом с вами на каждом шагу, помогая вам обрести осознанное здоровье и энергию жизни! " +
            "Вместе мы сделаем это просто и легко!\n";
        public const string PREPARING_FOR_THE_TRIP = "Для начала, давайте проверим вашу готовность путешествовать к здоровому образу жизни!\n\nГотовы? — НАЧИНАЕМ !!!\n";
        public const string PREPARING_FOR_THE_TRIP_LOST_STEP = "Если Вам нужно больше времени или Вы хотите общаться анонимно, ничего страшного! " +
            "Вы можете вернуться в любой момент.\n\n" +
           "<i>Жаль, что время ⏳,\nкак и <b>жизнь 🧬 нельзя поставить на паузу ⏸</b></i>";
        public const string THE_JOURNEY_BEGINS_USERFIELDS_0 = "Отлично, {0}! Вы выбрали идти к переменам, и я буду вашим гидом в этом захватывающем приключении!\r\n";
        public const string THE_JOURNEY_BEGINS_PHIRST_STEP = "<b>Первый этап - Заросли Знаний!</b>\n\nВместе мы найдем самый простой, короткий и безопасный маршрут к здоровому образу жизни!\r\n";
        public const string THE_JOURNEY_BEGINS_CONCLISION = "Готовы начать?\n\nНе стоит просто пытаться, <b>нужно действовать!</b>\n\n<u>Вперед, к лучшей версии себя!</u> 💪\r\n";
        public const string INTRODUCTORY_INFORMATION_ABOUT_THE_TRIP = "Мы есть то, - что мы любим есть!\n\nПеред тем, как начать наше приключение, давайте составим первичный дневник питания в течение 3 дней.\n\n";
        public const string INTRODUCTORY_INFORMATION_ABOUT_THE_TRIP_2 = "Я помогу вам анализировать, что вы едите и как это влияет на ваше самочувствие.\n\n";
        public const string BOT_ANSWER_GOODBUY = "<b><i>Успехов тебе мой Друг!</i></b>";
        public const string REMINDER_OF_DAY_1 = "<b><i>Напоминаю!</i></b>\n\n" +
            "Первый день, ты начинаешь с ведения дневника питания, в котором указываешь:\n\n" +
            "1. Дату.\n" +
            "2. Время подъема.\n" +
            "2. Время приема пищи и название блюда.\n";



        #region BUTTON

        public const string IINTRODUCE_YOURSELF_BUTTON = "/🤝 Представиться";

        public const string START_HELP_MESSAGE = "Вы можете использовать следующие команды:\r\n\r\n" +
           "/start - Начать общение.\r\n" +
           "/menu - Вызвать меню.\r\n" +
           "/help - Показать это справочное сообщение.";

        public static string[] USER_CONTINUER_RESPONSE_BUTTON = new string[]
        {
            "Да, продолжим!", "Конечно, давайте!", "С удовольствием продолжим!",
            "Определенно, продолжим!", "Продолжаем!", "Давайте идти дальше!",
            "Да, давайте продолжим наше путешествие!", "С радостью продолжим!",
            "Продолжаем движение!", "Безусловно, продолжаем!", "Уверенно продолжаем наше путешествие!",
            "Да, пойдем дальше!", "Продолжаем двигаться вперед!", "Давайте продолжим, не останавливаемся!",
            "Конечно, продолжим наше приключение!", "С удовольствием продолжим искать новые открытия!"
        };

        #endregion

        public static readonly string[] BOT_ANSWER_SMILE_PRANK_ARRAY = new string[]
        {
            "🤖 Ах, точно, запись на твою консультацию! Забыли на секундочку, но смеха ради, конечно! 😄📆",
            "О, точно, что тут у нас? Консультация! Конечно, мы всегда помним, но шутить не можем отказаться! 🤖💡",
            "Момент... Что-то казалось, мы об этом забыли. Но нет, конечно, помним – это твоя консультация! Всё идёт по плану! 😅📅",
            "Тут задача на память: кто-то хочет записаться на консультацию... Ага, правильно, это ты! И, кстати, мы всё помним, даже веселые ответы на такие запросы! 😎💬🤖",
            "Это было на кончике языка! Консультация для тебя! Ну да, мы смеёмся внутри сами от себя, но забавно же! 🤭🎊",
            "Ах, это была такая долгая пауза, правда? Но спасибо за напоминание! Так вот, консультация твоя – уже запланирована, конечно! 🗓️😄",
            "Мы немного задумались, но теперь всё ясно – твоя консультация! Мы даже умудряемся шутить, будучи ИИ! 🤖🤣",
            "Консультация? Ой, я забыл о ней на мгновение! Но всё в порядке, мы снова нацелены на помощь и, конечно, на смех! 🚀😂",
            "Конечно, консультация – это наше приоритетное направление! Порой мы так увлечены работой, что на секунду забываем о всем, кроме твоей важной записи! 😄🗓️",
            "Вперед, на станцию Юмории, где каждая консультация – это маленький праздник! И, хоть мы и ИИ, но юмор у нас программирован в максимальных дозах! 🚉🎉🤖"
        };

        public const string CONSULTATION_OFFLINE_WELCOME = "Доброго времени суток!\n\n" +
                                                            "Сегодня я буду твоим верным помощником, виртуальным наставником.\n\n" +
                                                            "Я интеллектуальный 🧠 Чат Бот 🤖 по программе VitalityProgram.Online (далее VPO)\n\n" +
                                                            "Можешь звать меня Герби (Herby)\n\n" +
                                                            "Давай знакомиться, расскажи о себе.\n\n";
        public static string[] GREETING_TEMPLATES_STRING_FORMAT = new string[]
        {
            "Отлично! Рад знакомству {0}!",
            "Приветствую! Рад знакомству {0}!",
            "Приятно познакомиться {0}! Отлично!",
            "Рад познакомиться {0}! Отличный день!",
            "Привет {0}! Давно ждал знакомства!",
            "Добро пожаловать {0}! Рад встрече.",
            "Здравствуйте! Рад знакомству с вами {0}!",
            "Добрый день! Отлично познакомиться {0}!",
            "Привет {0}! Очень рад знакомству."
        };

        public static string[] USER_MOTIVATIONAL_PHRASES = new string[]
        {
            "Вперед к Лучшей Жизни: Все возможно!",
            "Шаг за шагом, вперед к Лучшей Жизни!",
            "Успех - это результат наших действий. Вперед к Лучшей Жизни!",
            "Верь в себя и двигайся к Лучшей Жизни!",
            "Лучшая Жизнь ждет тебя, иди вперед!",
            "Не останавливайся, иди к Лучшей Жизни!",
            "Каждый день - новая возможность двигаться к Лучшей Жизни!",
            "Смело иди вперед, к Лучшей Жизни!",
            "Вперед к Лучшей Жизни: ты достоин всего самого лучшего!",
            "Не бойся перемен, они ведут к Лучшей Жизни!",
            "Твоя Лучшая Жизнь ждет тебя, иди к ней без страха!",
            "Каждый шаг приближает к Лучшей Жизни: не останавливайся!",
            "Успех начинается с малого, вперед к Лучшей Жизни!",
            "Ты можешь все, что захочешь - иди к Лучшей Жизни!",
            "Верь в свои силы и двигайся к Лучшей Жизни!",
            "Твой путь к Лучшей Жизни только начинается - иди вперед!"
        };

        public static string[] USER_CONGRATILATORY_RESPONSES_ANSWER = new string[]
        {
            "Молодец! Так держать!",
            "Отличная работа! Продолжай в том же духе!",
            "Я горжусь за вас! Продолжайте смело идти вперед!",
            "Вау, вы великолепны! Ничего не бойтесь и идите к своим целям!",
            "Вы молодец! Уверен, что вы сможете достичь всего, чего захотите!",
            "Просто восхитительно! Путь к успеху только начинается, и вы уже на верном пути!",
            "Замечательно! Развивайтесь и совершенствуйтесь дальше!",
            "Это потрясающе! Ваши усилия приносят свои плоды!",
            "Я вижу, как вы растете и преодолеваете все трудности! Продолжайте так же!",
            "Вы просто волшебник! Верьте в себя и ничто не остановит Вас на пути к успеху!",
            "Поздравляю! Ваше стремление к лучшему всегда приводит к отличным результатам!",
            "Отлично! Не сдавайтесь и двигайтесь дальше, вы на верном пути!",
            "Удивительно! Ваше решимость преодолевает любые препятствия!",
            "Правильно, так держать! Ваши достижения восхищают!",
            "Уверен, что у вас все получится! Продолжайте смело шагать вперед!"
        };

        public const string USER_INSTRUCTION = "Для участия в программе необходимо установить приложение <b>ZOOM</b>.\n" +
                                               "Расписание мероприятий будет отправлено Вам в Telegram.\n" +
                                               "Вам нужны будут напольные весы.\n" +
                                               "Фотографии <b><i>“ДО”</i></b> и <b><i>“ПОСЛЕ”</i></b> необходимо будет отправить помощнику.";

        public const string USER_BOOKSTART_SEND = "Вот ваш дневник питания на первые 3 дня.\n\n" +
           "Вам необходимо будет заполнять его <b>каждый день.</b>\n\n" +
           "Утром <b>натощак</b> записываем свой вес.\n\n" +
           "<b>Каждый прием пищи</b> записываем в отдельную графу";

        public const string USER_HOWTOFILLOUTAFOODDIARY = "Посмотрите видео-инструкцию, как заполнять дневник питания.";

        public const string THANKSTOUSER = "<i>Спасибо за то, что Доверяете нам.</i>";
        public const string YOUR_MESSAGE_HAS_BEEN_RECEIVED = "Ваше сообщение получено. Ожидайте ответа!";

        public const string HELP_MENU_BUTTON = "Вы можете использовать следующие команды:\n\n" +
                        "<b>/start</b> - Начать разговор\n" +
                        "<b>/menu</b> - Вызвать меню\n" +
                        "<b>/help</b> - Показать это справочное сообщение\n";


    }
}
