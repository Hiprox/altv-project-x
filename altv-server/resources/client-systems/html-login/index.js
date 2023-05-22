const State = {LOGIN : 0, REGISTRATION : 1, RESTORE_PASSWORD : 2};
const Error = {
    INVALID_EMAIL : 1,
    INVALID_NICKNAME : 2,
    INVALID_PASSWORD : 3,
    PASSWORDS_ARENT_EQUALS : 4,
    EMAIL_OR_PASSWORD_IS_WRONG : 5,
    UNDEFINED : 6,
    NICKNAME_EXIST : 7,
    EMAIL_EXIST : 8,
    PLAYER_CONNECTED : 9
};
const NAME_MIN_LEN = 5;
const NAME_MAX_LEN = 24;
const PASS_MIN_LEN = 8;
const PASS_MAX_LEN = 32;
const REGEX_PASS = /^[a-zA-Z0-9]+$/;
const REGEX_NAME = /^[A-Z][a-z]{1,}\_[A-Z]([a-z]{1,}[A-Z]){0,1}[a-z]{1,}$/;
const REGEX_MAIL = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
const icon_mail = `<svg class="icon-input-style"  width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
        <path stroke="none" d="M0 0h24v24H0z"/>
        <rect x="3" y="5" width="18" height="14" rx="2" />
        <polyline points="3 7 12 13 21 7" />
    </svg>`;
const icon_key = `<svg class="icon-input-style"  fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 7a2 2 0 012 2m4 0a6 6 0 01-7.743 5.743L11 17H9v2H7v2H4a1 1 0 01-1-1v-2.586a1 1 0 01.293-.707l5.964-5.964A6 6 0 1121 9z"/>
    </svg>`;
const icon_user = `<svg class="icon-input-style"  viewBox="0 0 24 24"  fill="none"  stroke="currentColor"  stroke-width="2"  stroke-linecap="round"  stroke-linejoin="round">
        <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
        <circle cx="12" cy="7" r="4" />
    </svg>`;
const icon_warning = `<svg class="icon-warning-style"  viewBox="0 0 24 24"  fill="none"  stroke="currentColor"  stroke-width="2"  stroke-linecap="round"  stroke-linejoin="round">
        <circle cx="12" cy="12" r="10" />
        <line x1="12" y1="8" x2="12" y2="12" />
        <line x1="12" y1="16" x2="12.01" y2="16" />
    </svg>`;
const icon_code = `<svg class="icon-input-style"  width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">  <path stroke="none" d="M0 0h24v24H0z"/>  <rect x="3" y="4" width="18" height="16" rx="2" />
        <line x1="7" y1="8" x2="7" y2="8.01" />
        <line x1="12" y1="8" x2="12" y2="8.01" />
        <line x1="17" y1="8" x2="17" y2="8.01" />
        <line x1="7" y1="12" x2="7" y2="12.01" />
        <line x1="12" y1="12" x2="12" y2="12.01" />
        <line x1="17" y1="12" x2="17" y2="12.01" />
        <line x1="7" y1="16" x2="17" y2="16" />
    </svg>`;
const kExampleNickNames = `Пример: Will_Smith или Conor_McGregor`;
const kRulesNickName = `Имя персонажа должно состоять из:<br>
                    - символов латинского алфавита;<br>
                    - знака "_" между Именем и Фамилией;<br>
                    - более 2-ух символов в каждой части;<br>
                    - не менее ${NAME_MIN_LEN} и не более ${NAME_MAX_LEN} символов.<br>
                    Разрешаются составные фамилии (McGregor, McDonald и т.д.).`;

let IsSpinerON = false;

function drawContent(element_id, state) {
    document.getElementById(element_id).innerHTML = state;
}

function showError(key) {
    if (IsSpinerON === true)
    {
        OffSpiner();
    }
    let text = null;
    switch(key)
    {
        case Error.INVALID_EMAIL:
            text = `Укажите ваш действующий E-mail`;
            break;
        case Error.INVALID_NICKNAME:
            text = `Некоректное имя персонажа!<br>${kRulesNickName}`;
            break;
        case Error.INVALID_PASSWORD:
            text = `Некоректный пароль!<br>Правила использования пароля разрешают:<br>
            - длину от 8 до 32 символов;<br>
            - символы латинского алфавита: A-Z, a-z;<br>
            - цифровые символы: 0-9`;
            break;
        case Error.PASSWORDS_ARENT_EQUALS:
            text = `Пароли не совпадают!`;
            break;
        case Error.EMAIL_OR_PASSWORD_IS_WRONG:
            text = `Не верный e-mail или пароль!`;
            break;
        case Error.NICKNAME_EXIST:
            text = `Имя персонажа уже занято!`;
            break;
        case Error.EMAIL_EXIST:
            text = `E-mail уже зарегистрирован!`;
        case Error.PLAYER_CONNECTED:
            text = `Игрок уже в сети!`;
        case Error.UNDEFINED:
        default:
            text = 'Неопределенная ошибка / сервер не отвечает!';
    }
    document.getElementById('error_container').classList.remove('hidden');
    document.getElementById('error_msg').innerHTML = `${icon_warning} ${text}`;
}

// Alt:V
alt.on('Player:ShowError',   function(key) { showError(key); });

function hideError() {
    let container = document.getElementById('error_container');
    if (!container.classList.contains('hidden')) container.classList.add('hidden');
    document.getElementById('error_msg').innerHTML = '';
}

function checkStrMoreDouble(str) {
    const lowerStr = str.toLowerCase();
    for (let i = 0; i < lowerStr.length - 2; i++) {
        if (lowerStr[i] === lowerStr[i + 1] && lowerStr[i] === lowerStr[i + 2]) {
            return false;
        }
    }
    return true;
}

function IsValidName(name) {
    return (//name.split('_').length === 2 &&
        checkStrMoreDouble(name) &&
        (name.length >= NAME_MIN_LEN  && name.length <= NAME_MAX_LEN) &&
        REGEX_NAME.test(name)
    ) ? true : false;
}

function IsValidPassword(password) {
    return (password.length >= PASS_MIN_LEN && password.length <= PASS_MAX_LEN &&
        REGEX_PASS.test(password)
        ) ? true : false;
}

function IsValidMail(email) {
    return (email.length > 0 &&
        REGEX_MAIL.test(email)
    ) ? true : false;
}

// Доступна в асинхронном режиме
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function drawLogin()
{
    drawContent('title', 'Авторизация');
    drawContent('form', formLogin());
    drawContent('have_account', `Нет аккаунта?`);
    drawContent('state_account', `Регистрация`);
    drawContent('submit_button', `Войти`);
}

function drawRegistration()
{
    drawContent('title', 'Регистрация');
    drawContent('form', formRegistration());
    drawContent('have_account', `Есть аккаунт?`);
    drawContent('state_account', `Авторизация`);
    drawContent('submit_button', `Зарегистрироваться`);
}

function drawNewPasswordSended(email) {
    drawContent('title', 'Восстановление пароля');
    drawContent('form', formNewPasswordSended(email));
    drawContent('have_account', 'Назад?');
    drawContent('state_account', 'Авторизация');
}

function drawForgetPassword()
{
    drawContent('title', 'Восстановление пароля');
    drawContent('form', formForgetPassword());
    drawContent('have_account', 'Назад?');
    drawContent('state_account', 'Авторизация');
    drawContent('submit_button', 'Отправить новый пароль');
}

function OnSpiner(callback)
{
    IsSpinerON = true;
    let button = document.getElementById('submit');
    button.setAttribute('disabled', true)
    let spiner = document.getElementById('spiner');
    spiner.classList.remove('hidden');
    callback();
}

function OffSpiner()
{
    IsSpinerON = false;
    let button = document.getElementById('submit');
    let spiner = document.getElementById('spiner');
    spiner.classList.add('hidden');
    button.removeAttribute('disabled');
}

function CheckRegistrationFields() {
    const email = document.getElementById('email').value;
    const nickname = document.getElementById('nickname').value;
    const password = document.getElementById('password').value;
    const repeatPassword = document.getElementById('repeat_password').value;
    const sex = document.querySelector('input[name="sex"]:checked').value;

    if (IsValidMail(email) === false) {
        showError(Error.INVALID_EMAIL);
    } else if (IsValidName(nickname) === false) {
        showError(Error.INVALID_NICKNAME);
    } else if (IsValidPassword(password) === false) {
        showError(Error.INVALID_PASSWORD);
    } else if (password !== repeatPassword) {
        showError(Error.PASSWORDS_ARENT_EQUALS);
    } else {
        console.log(`${email} ${nickname} ${password} ${repeatPassword} ${sex}`);
        hideError();
        // Alt:V
        alt.emit('Player:Register', email, password, nickname, sex);
    }
}

function CheckLoginFields() {
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    if (IsValidMail(email) === false || IsValidPassword(password) === false) {
        showError(Error.EMAIL_OR_PASSWORD_IS_WRONG);
    } else {
        hideError();
        // Alt:V
        alt.emit('Player:Login', email, password);
    }
}

function CheckForgetPasswordFields() {
    const email = document.getElementById('email').value;
    if (IsValidMail(email) == false)
    {
        errorMailNotExistOrPasswordWrong();
        return;
    }
    hideError();
    // Alt:V
    alt.emit('Player:SendNewPassword', email);
    drawNewPasswordSended(email);
}

function drawPage(toggle) {
    let stateAccountRef = document.getElementById('state_account');
    switch(toggle)
    {
        case State.LOGIN: {
            drawLogin();
            stateAccountRef.onclick = function() { drawPage(State.REGISTRATION); };
            document.getElementById('submit').onclick = function() { OnSpiner(CheckLoginFields); };
            break;
        }
        case State.REGISTRATION: {
            drawRegistration();
            stateAccountRef.onclick = function() { drawPage(State.LOGIN); };

            let button = document.getElementById('submit');
            button.setAttribute('disabled', true)

            let checkbox = document.getElementById('checkbox');
            checkbox.addEventListener("change", function () {
                if (this.checked) {
                    console.log('checked');
                    document.getElementById('submit').removeAttribute("disabled");
                } else {
                    console.log('unchecked');
                    document.getElementById('submit').setAttribute("disabled", true);
                }
            });
            button.onclick = function() { OnSpiner(CheckRegistrationFields); }
            break;
        }
        case State.RESTORE_PASSWORD: {
            drawForgetPassword();
            stateAccountRef.onclick = function() { drawPage(State.LOGIN); };
            document.getElementById('submit').onclick = () => { OnSpiner(CheckForgetPasswordFields); };
            break;
        }
    }
}

function formNewPasswordSended(email) {
    return `<div class="win-block-password-sended">
                <label class="win-block-password-sended-text"> Новый пароль был отправлен на ${email}<br><br>Пароль будет действителен в течении 7 мин.</label>
            </div>`;
}

function formForgetPassword() {
    return `<!-- Ошибка -->
            <div id="error_container" class="hidden win-err-msg">
                <span id="error_msg">
                    <!-- Информация об ошибке -->
                </span>
            </div>
            <!-- Введите E-mail на который будет отправлен код -->
            <div class="win-block">
                <label class="win-block-text"> Введите E-mail от которого забыли пароль:</label>
                ${icon_mail}
                <input id="email" placeholder="E-mail" required="required" maxlength="125" type="email" class="win-block-input">
            </div>
            
            <div class="win-btns-block-center">
                <!-- Отправить код -->
                <button id="submit" class="general-btn">
                    <!-- Спинер -->
                    <span id="spiner" class="hidden spiner-btn"></span>
                    <span id="submit_button" class="text-general-btn">
                        <!-- Отправить код -->
                    </span>
                </button>
            </div>`;
}

function formLogin() {
    return `<!-- Ошибка -->
            <div id="error_container" class="hidden win-err-msg">
                <span id="error_msg" class="text-red-500 font-mono font-bold text-l">
                    <!-- Информация об ошибке -->
                </span>
            </div>
            <!-- E-mail -->
            <div class="win-block">
                <label class="win-block-text"> E-mail </label>
                ${icon_mail}
                <input id="email" placeholder="E-mail" required="required" maxlength="125" type="email" class="win-block-input">
            </div>
            
            <!-- Пароль -->
            <div class="win-block">
                <label class="win-block-text"> Пароль </label>
                ${icon_key}
                <input id="password" placeholder="Пароль" minlength="8" required="required" type="password" class="win-block-input">
            </div>

            <div class="win-btns-block-between">
                <!-- Войти -->
                <button id="submit" class="general-btn">
                    <span id="spiner" class="hidden spiner-btn"></span>
                    <span id="submit_button" class="text-general-btn">
                        <!-- Войти -->
                    </span>
                </button>
                <!-- Забыли пароль -->
                <a id="forget_password" href="#" class="win-forget-password-link">Забыли пароль?</a>
            </div>`;
}

function formRegistration() {
    return `<!-- Ошибка -->
            <div id="error_container" class="hidden win-err-msg">
                <span id="error_msg" class="text-red-500 font-mono font-bold text-l">
                    <!-- Информация об ошибке -->
                </span>
            </div>
            <!-- E-mail -->
            <div class="win-block">
                <label class="win-block-text"> E-mail </label>
                ${icon_mail}
                <input id="email" placeholder="E-mail" type="email" required="required" class="win-block-input">
            </div>

            <!-- NickName -->
            <div class="win-block">
                <label class="block font-bold xl:text-3xl"> Имя персонажа</label>
                ${icon_user}
                <input id="nickname" placeholder="Пример: Lee_Bush, Conor_McGregor и т.п." type="name" minlength="5" maxlength="32" required="required" class="win-block-input">
            </div>

            <!-- Sex-->
            <div class="win-block">
                <label class="win-block-text"> Пол персонажа: </label>
                <div class="win-block-sex-selecter">
                    <div class="win-block-sex-selecter-left-item">
                        <input checked id="sexMale" name="sex" type="radio" value="male" class="win-block-sex-selecter-input peer">
                        <label for="sexMale" class="win-block-sex-selecter-text">Мужской</label>
                    </div>
                    <div class="win-block-sex-selecter-right-item">
                        <input id="sexFemale" name="sex" type="radio" value="female" class="win-block-sex-selecter-input peer">
                        <label for="sexFemale" class="win-block-sex-selecter-text">Женский</label>
                    </div>
                </div>
            </div>

            <!-- Пароль -->
            <div class="win-block">
                <label class="win-block-text"> Пароль </label>
                ${icon_key}
                <input id="password" placeholder="Пароль" minlength="8" required="required" type="password" class="win-block-input">
            </div>

            <!-- Повторите пароль -->
            <div class="win-block">
                <label class="win-block-text"> Повторите пароль </label>
                ${icon_key}
                <input id="repeat_password" placeholder="Повторите пароль" minlength="8" required="required" type="password" class="win-block-input">
            </div>

            <!-- Пользовательское соглашение -->
            <div class="win-termsofservice-block">
                
                <input id="checkbox"
                    class="win-termsofservice-block-input"
                    type="checkbox"
                    id="accepted_rules">
                <label class="win-termsofservice-block-text">
                    Я ознакомился с <a href="#" class="win-termsofservice-block-link">правилами сервера</a>
                </label>
            </div>

            <div class="win-btns-block-center">
                <!-- Зарегистрироваться -->
                <button id="submit" class="general-btn text-general-btn">
                    <span id="spiner" class="hidden spiner-btn"></span>
                    <span id="submit_button">
                        <!-- Зарегистрироваться -->
                    </span>
                </button>
            </div>`;
}

window.addEventListener('load', () => {
    drawPage(State.LOGIN);
});