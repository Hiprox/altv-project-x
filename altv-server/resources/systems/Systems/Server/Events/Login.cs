using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using System.Text.RegularExpressions;
using AltV.Net.Async;

public class Login : IScript
{
    public enum PasswordLength : int
    {
        Min = 5,
        Max = 32
    }
    public enum NickNameLength : int
    {
        Min = 5,
        Max = 24
    }
    private enum Error : int
    {   
        NONE = 0,
        INVALID_EMAIL,
        INVALID_NICKNAME,
        INVALID_PASSWORD,
        PASSWORDS_ARENT_EQUALS,
        EMAIL_OR_PASSWORD_IS_WRONG,
        UNDEFINED,
        NICKNAME_EXIST,
        EMAIL_EXIST,
        PLAYER_CONNECTED
    };
    private const string ClientErrorEvent = "LoginSystem:Error";
    public static async Task GoToSpawn(CPlayer player)
    {
        player.Model = (uint)PedModel.FreemodeMale01;
        //player.Emit("Client:BeforeSpawn", 0f, 0f, 75f, 0);
        player.Visible = false;
        player.Spawn(new AltV.Net.Data.Position(0f, 0f, 75f), 0);
        player.Health = 200;
        await Task.Delay(500);
        player.Visible = true;

        player.GiveWeapon(WeaponModel.Pistol, 999, true);
        player.GiveWeapon(WeaponModel.MicroSMG, 999, false);
        player.GiveWeapon(WeaponModel.AssaultRifle, 999, false);
        player.GiveWeapon(WeaponModel.Baseball, 999, false);
    }
    [AsyncClientEvent("Event:Login")]
    public static async Task OnPlayerLogin(CPlayer player, string email, string password)
    {
        var errorId = GetErrorIfWrongLoginParams(email, password);
        if (errorId != Error.NONE)
        {
            player.Emit(ClientErrorEvent, (int)errorId);
            return;
        }
        if (!AltDataBase.GetUser(player, email, password))
        {
            player.Emit(ClientErrorEvent, (int)Error.EMAIL_OR_PASSWORD_IS_WRONG);
            return;
        }
        player._LoggedIn = true;

        player.Emit("Player:CloseLoginHud");
        await GoToSpawn(player);
    }
    [AsyncClientEvent("Event:Register")]
    public static async Task OnPlayerRegister(CPlayer player, string email, string password, string nickname, string gender)
    {
        var errorId = GetErrorIfWrongRegisterParams(email, password, nickname, gender);
        if (errorId != Error.NONE)
        {
            player.Emit(ClientErrorEvent, (int)errorId);
            return;
        }
        email = email.ToLower();
        var pGender = gender == "male" ? CPlayer.Gender.Male : CPlayer.Gender.Female;
        var id = AltDataBase.CreateUser(email, password, nickname, pGender);
        if (id == -1)
        {
            player.Emit(ClientErrorEvent, (int)Error.UNDEFINED);
            return;
        }
        // Сохраняем данные игрока
        player._Gender = pGender;
        player._DataBaseID = id;
        player._Admin = CPlayer.AdminRank.None;
        player._Money = 0;
        player._Nickname= nickname;
        player._LoggedIn = true;

        player.Emit("Player:CloseLoginHud");
        await GoToSpawn(player);
    }
    private static bool CheckStrMoreDouble(string str)
    {
        string lowerStr = str.ToLower();
        for (int i = 0; i < lowerStr.Length - 2; i++)
        {
            if (lowerStr[i] == lowerStr[i + 1] && lowerStr[i] == lowerStr[i + 2])
            {
                return false;
            }
        }
        return true;
    }
    private static bool IsValidPassword(string password)
    {
        return password.Length >= (int)PasswordLength.Min &&
            password.Length <= (int)PasswordLength.Max &&
            RegexType.Password.IsMatch(password);
    }
    private static bool IsValidNickName(string nickname)
    {
        return nickname.Length >= (int)NickNameLength.Min &&
            nickname.Length <= (int)NickNameLength.Max &&
            CheckStrMoreDouble(nickname) &&
            RegexType.NickName.IsMatch(nickname);
    }
    private static Login.Error GetErrorIfWrongLoginParams(string email, string password)
    {
        // Проверка E-mail
        if (email.Length > 320 || !RegexType.Email.IsMatch(email) || !IsValidPassword(password))
        {
            return Error.EMAIL_OR_PASSWORD_IS_WRONG;
        }
        return Error.NONE;
    }
    private static Login.Error GetErrorIfWrongRegisterParams(string email, string password, string nickname, string sex)
    {
        if (sex != "male" && sex != "female")
        {
            return Error.UNDEFINED;
        }
        // Проверка E-mail
        else if (email.Length > 320)
        {
            return Error.INVALID_EMAIL;
        }
        else if (!RegexType.Email.IsMatch(email))
        {
            return Error.INVALID_EMAIL;
        }
        else if (AltDataBase.IsEmailExist(email))
        {
            return Error.EMAIL_EXIST;
        }
        // Проверка NickName
        else if (!IsValidNickName(nickname))
        {
            return Error.INVALID_NICKNAME;
        }
        else if (AltDataBase.IsNickNameExist(nickname))
        {
            return Error.NICKNAME_EXIST;
        }
        // Проверка пароля
        else if (!IsValidPassword(password))
        {
            return Error.INVALID_PASSWORD;
        }
        return Error.NONE;
    }
}

public class RegexType
{
    private RegexType(string value) { Value = value; }
    public string Value { get; private set; }
    public static RegexType Password { get { return new RegexType(@"^[a-zA-Z0-9]+$"); } }
    public static RegexType NickName { get { return new RegexType(@"^[A-Z][a-z]{1,}_[A-Z]([a-z]{1,}[A-Z]){0,1}[a-z]{1,}$"); } }
    public static RegexType Email { get { return new RegexType(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$"); } }
    public bool IsMatch(string text)
    {
        return Regex.IsMatch(text, this.Value);
    }
    public override string ToString()
    {
        return Value;
    }
}