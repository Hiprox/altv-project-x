using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using MySql.Data.MySqlClient;
using Bcrypt;
using System.ComponentModel.DataAnnotations;
using Utility.Factions;
using AltServer;

internal class AltDataBase : AltServer.Server
{
    private static MySqlConnection _connection { get; set; }
    private static AltDataBase _instance;
    public static bool IsConnected { get; private set; } = false;
    public static string Host { get; } = "localhost";
    public static string Username { get; } = "altv";
    public static string Password { get; } = "altv";
    public static string DatabaseName { get; } = "altv";
    public static string LocalSalt { get; } = "altv";
    private AltDataBase() {}
    private static string GetConnectionString()
    {
        return $"SERVER={Host}; DATABASE={DatabaseName}; UID={Username}; Password={Password}";
    }
    public static AltDataBase Instance()
    {
        if (_instance == null)
        {
            _instance = new AltDataBase();
        }
        return _instance;
    }
    // Подключиться к БД
    public static void Connect()
    {
        try
        {
            if (string.IsNullOrEmpty(DatabaseName))
            {
                throw new Exception("[MySQL Error] DatabaseName не указано или равно null!");
            }
            var connectionString = GetConnectionString();
            _connection = new MySqlConnection(connectionString);
            _connection.Open();

            IsConnected = true;
        }
        catch (MySqlException e)
        {
            Alt.Log("[MySQL Error] Подключение к MySQL не выполнено!");
            Alt.Log(e.ToString());
            Thread.Sleep(5000);
            Environment.Exit(1);
        }
        catch(Exception e)
        {
            Alt.Log(e.Message);
            Alt.Log(e.ToString());
            Thread.Sleep(5000);
            Environment.Exit(2);
        }
        Alt.Log("[База данных] Подключено!");
    }
    // Закрыть соединение с БД
    public static void Close()
    {
        IsConnected = false;
        _connection.Close();
    }
    /* Создать пользователя
     * @isUserCreated был ли создан пользователь true/false
     * @return null, если пользователь не создан / ID новой записи в БД 
     */
    public static long CreateUser(string email, string password, string nickname, CPlayer.Gender gender)
    {
        email = email.ToLower();
        string salt = BCrypt.GenerateSalt();
        string hashedPassword = BCrypt.HashPassword(password + AltDataBase.LocalSalt, salt);
        try
        {
            MySqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "INSERT INTO users (email, nickname, password, gender) VALUES (@email, @nickname, @password, @gender)";
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@nickname", nickname);
            cmd.Parameters.AddWithValue("@password", hashedPassword);
            cmd.Parameters.AddWithValue("@gender", gender);
            cmd.ExecuteNonQuery();
            return cmd.LastInsertedId;
        }
        catch(Exception e)
        {
            Alt.Log($"[MySQL Error] Ошибка при создании пользователя email=`{email}` password=`{password}` gender=`{gender}`.");
            Alt.Log(e.ToString());
        }
        return -1;
    }
    // Получить аккаунт пользователя
    public static bool GetUser(CPlayer player, string email, string password)
    {
        email = email.ToLower();
        MySqlCommand cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM users WHERE `email` = @email LIMIT 1";
        cmd.Parameters.AddWithValue("@email", email);
        cmd.ExecuteNonQuery();
        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.HasRows)
            {
                reader.Read();
                string dbHashPassword = reader.GetString("password");
                // Если хэши паролей совпадают, то получаем данные аккаунта
                if (BCrypt.CheckPassword(password + AltDataBase.LocalSalt, dbHashPassword))
                {
                    player._DataBaseID = reader.GetInt64("id");
                    player._Nickname = reader.GetString("nickname");
                    player._Admin = (CPlayer.AdminRank)reader.GetByte("admin");
                    player._Money = reader.GetInt64("money");
                    player._Gender = (CPlayer.Gender)reader.GetByte("gender");
                    player._Faction = (Faction.ID)reader.GetSByte("faction");
                    player._Rank = reader.GetSByte("rank");
                    return true;
                }
            }
        }
        return false;
    }
    // Существует ли NickName
    public static bool IsNickNameExist(string nickname)
    {
        MySqlCommand cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM users WHERE `nickname` = @nickname LIMIT 1";
        cmd.Parameters.AddWithValue("@nickname", nickname);
        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.HasRows)
            {
                return true;
            }
        }
        return false;
    }
    // Существует ли E-mail
    public static bool IsEmailExist(string email)
    {
        email = email.ToLower();
        MySqlCommand cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM users WHERE `email` = @email LIMIT 1";
        cmd.Parameters.AddWithValue("@email", email);
        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.HasRows)
            {
                return true;
            }
        }
        return false;
    }
}
