using AltV.Net;
using AltV.Net.Enums;
using AltV.Net.Elements.Entities;
using AltV.Net.Resources.Chat.Api;
using Utility.Factions;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Globalization;
using AltV.Net.Data;

public class AdminCmds : IScript
{
    [Command("veh")]
    public static void Veh_CMD(IPlayer player, string vehName)
    {
        var hash = Alt.Hash(vehName);
        if (Enum.IsDefined(typeof(VehicleModel), hash))
        {
            IVehicle vehicle = Alt.CreateVehicle(hash, player.Position, player.Rotation);
            if (vehicle != null)
            {
                vehicle.PrimaryColorRgb = new AltV.Net.Data.Rgba((byte)255, (byte)255, (byte)255, (byte)255);
                player.SetIntoVehicle(vehicle, 1);
                player.SendChatMessage("{FFFF00}" + $"Транспортное средство {vehName.ToUpper()} создано!");
            }
        }
    }

    [Command("freeze")]
    public static void Freeze_CMD(IPlayer player, bool toggle = true)
    {
        player.Emit("freezePlayer", toggle);
        if (toggle)
        {
            player.SendChatMessage("{FF0000} Вы были заморожены администратором!");
        }
        else
        {
            player.SendChatMessage("{FF0000} Вы были разморожены администратором!");
        }
    }

    public static void Uninvite(CPlayer target)
    {
        target._Faction = Faction.ID.None;
        target._Rank = 0;
        target.Model = (uint)PedModel.FreemodeMale01;
    }

    [Command("makeleader")]
    public static void MakeLeader_CMD(CPlayer player, int targetId, byte factionId)
    {
        var target = player.GetPlayerByID(targetId);
        if (target == null)
        {
            player.SendChatMessage($"{TxtClr.Gray}Игрок ID:{targetId} не в сети");
            return;
        }
        if (!target._LoggedIn)
        {
            player.SendChatMessage($"{TxtClr.Warn}Игрок ID:{targetId} в процессе авторизации");
            return;
        }
        if (System.Enum.IsDefined(typeof(Faction.ID), factionId))
        {
            var admins = CPlayer.GetAllAdmins();
            Faction.ID faction = (Faction.ID)factionId;
            string factionName = Faction.Name(faction) ?? string.Empty;
            if (faction != Faction.ID.None)
            {
                target._Faction = faction;
                target._Rank = Faction.MaxRank(faction);
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                target.Model = (uint)Faction.Skins(faction)[0];
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
                player.SendChatMessage($"{TxtClr.Warn}Вы назначили {target._Nickname} [{target._ID}] лидером {factionName}");
                target.SendChatMessage($"{TxtClr.Warn}Администратор {player._Nickname} [{player._ID}] назначил вас лидером {factionName}");
                foreach (var admin in admins)
                {
                    if (admin._ID != player._ID)
                    {
                        admin.SendChatMessage($"{TxtClr.AdmCht}[A] {TxtClr.AdmErr}Админ {player._Nickname} [{player._ID}] назначил {target._Nickname} [{target._ID}] лидером {factionName}");
                    }
                }
            }
            else
            {
                Faction.ID lastFaction = target._Faction;
                Uninvite(target);
                target.SendChatMessage("" + $"Администратор {player._Nickname} снял вас с поста лидера {Faction.Name(lastFaction)}");
                foreach (var admin in admins)
                {
                    if (admin._ID != player._ID)
                    {
                        admin.SendChatMessage($"{TxtClr.AdmCht}[A] {TxtClr.AdmErr}Админ {player._Nickname} [{player._ID}] снял {target._Nickname} [{target._ID}] лидера {factionName}");
                    }
                }
            }
        }
    }
    [Command("weapon", aliases: new[] { "weap", "gun", "guns" })]
    public static void Weapon_CMD(CPlayer player)
    {
        player.GiveWeapon(WeaponModel.Pistol, 999, true);
        player.GiveWeapon(WeaponModel.MicroSMG, 999, false);
        player.GiveWeapon(WeaponModel.AssaultRifle, 999, false);
        player.GiveWeapon(WeaponModel.Baseball, 999, false);
        player.SendChatMessage($"Вам выдано оружие");
    }
    [Command("hp", greedyArg: false, aliases: new[] { "sethp" })]
    public static void Hp_CMD(CPlayer player, params object[] @params)
    {
        if (@params.Length > 2) return;
        if (!player._LoggedIn) return;
        switch (@params.Length)
        {
            case 0:
                player.Health = 200;
                player.SendChatMessage($"{TxtClr.Warn}Ваш уровень HP восстановлен до 100");
                break;
            case 1:
                if (UInt16.TryParse((string?)@params[0], out ushort value))
                {
                    player.Health = (ushort)(100 + value);
                    player.SendChatMessage($"{TxtClr.Warn}Ваш уровень HP восстановлен до {value}");
                }
                break;
            case 2:
                {
                    if (Int32.TryParse((string?)@params[0], out int targetId) && UInt16.TryParse((string?)@params[1], out ushort value2))
                    {
                        var target = player.GetPlayerByID(targetId);
                        switch (target)
                        {
                            case null:
                                player.SendChatMessage($"{TxtClr.Gray}Игрок ID:{targetId} не в сети");
                                return;
                            case { _LoggedIn: false }:
                                player.SendChatMessage($"{TxtClr.Warn}Игрок ID:{targetId} в процессе авторизации");
                                return;
                            case { _ID: var id } when id == player._ID:
                                player.Health = (ushort)(100 + value2);
                                player.SendChatMessage($"{TxtClr.Warn}Ваш уровень HP восстановлен до {value2}");
                                return;
                            default:
                                var admins = CPlayer.GetAllAdmins();
                                if (target.IsAdmin() && player.AdminCompareTo(target._Admin) <= 0)
                                {
                                    if (target.IsAdmin(CPlayer.AdminRank.Creator))
                                    {
                                        player.SendChatMessage($"{TxtClr.Gray}Игрок ID:{targetId} не в сети");
                                        target.SendChatMessage($"{TxtClr.Err}Админ {player._Nickname} [{player._ID}] пытался изменить ваше HP на {value2}");
                                        return;
                                    }
                                    player.SendChatMessage($"{TxtClr.Gray}Нельзя применить команду к администратору равного/высшего уровня!");
                                    foreach (var admin in admins)
                                    {
                                        if (admin._ID != player._ID)
                                        {
                                            admin.SendChatMessage($"{TxtClr.AdmCht}[A] {TxtClr.AdmErr}Админ {target._Nickname} [{targetId}] пытался изменить HP админу {target._Nickname} [{target._ID}] на {value2}");
                                        }
                                    }
                                    return;
                                }
                                target.Health = (ushort)(100 + value2);
                                if (player.AdminCompareTo(CPlayer.AdminRank.Creator) == -1 && player._ID != target._ID)
                                {
                                    foreach (var admin in admins)
                                    {
                                        if (admin._ID != player._ID)
                                        {
                                            admin.SendChatMessage($"{TxtClr.AdmCht}[A] {TxtClr.AdmEvnt}Админ {player._Nickname} [{player._ID}] изменил HP игроку {target._Nickname} [{target._ID}] на {value2}");
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
                break;
        }
    }
    [Command("goto", aliases: new[] { "gt", "tp" })]
    public static void Tp_CMD(CPlayer player, int targetId)
    {
        if (!player._LoggedIn) return;
        var target = player.GetPlayerByID(targetId);
        switch (target)
        {
            case null:
                player.SendChatMessage($"{TxtClr.Gray}Игрок ID:{targetId} не в сети!");
                return;
            case { _LoggedIn: false }:
                player.SendChatMessage($"{TxtClr.Warn}Игрок ID:{targetId} в процессе авторизации");
                return;
            case { _ID: var id } when id == player._ID:
                player.SendChatMessage($"{TxtClr.Gray}Нельзя применить команду к самому себе");
                return;
            case { _Admin: var admin } when target.IsAdmin(CPlayer.AdminRank.Creator) && player.AdminCompareTo(admin) == -1:
                // Если target является Основателем и player._Admin < target._Admin
                // Скрываем Основателя с сервера
                player.SendChatMessage($"{TxtClr.Gray}Игрок ID:{targetId} не в сети");
                target.SendChatMessage($"{TxtClr.Err}Админ {player._Nickname} [{player._ID}] пытался к вам телепортироваться");
                return;
            default:
                player.Position = target.Position;
                player.SendChatMessage($"{TxtClr.Warn}Вы были телепортированы к {target._Nickname} [{target._ID}]");
                if (player.AdminCompareTo(CPlayer.AdminRank.Creator) == -1 && player._ID != target._ID)
                {
                    foreach (var admin in CPlayer.GetAllAdmins())
                    {
                        if (admin._ID != player._ID)
                        {
                            admin.SendChatMessage($"{TxtClr.AdmCht}[A] {TxtClr.AdmEvnt}Админ {player._Nickname} [{player._ID}] телепортировался к игроку {target._Nickname} [{target._ID}]");
                        }
                    }
                }
                break;
        }
    }
    [Command("gethere", aliases: new[] { "gh" })]
    public static void Goto_CMD(CPlayer player, int targetId)
    {
        if (!player._LoggedIn) return;
        var target = player.GetPlayerByID(targetId);
        switch (target)
        {
            case null:
                player.SendChatMessage($"{TxtClr.Gray}Игрок ID:{targetId} не в сети!");
                return;
            case { _LoggedIn: false }:
                player.SendChatMessage($"{TxtClr.Warn}Игрок ID:{targetId} в процессе авторизации");
                return;
            case { _ID: var id } when id == player._ID:
                player.SendChatMessage($"{TxtClr.Gray}Нельзя применить команду к самому себе");
                return;
            case { _Admin: var admin } when target.IsAdmin(CPlayer.AdminRank.Creator) && player.AdminCompareTo(admin) == -1:
                // Скрываем Основателя с сервера
                player.SendChatMessage($"{TxtClr.Gray}Игрок ID:{targetId} не в сети");
                target.SendChatMessage($"{TxtClr.Err}Админ {player._Nickname} [{player._ID}] пытался к вам телепортироваться");
                return;
            default:
                // Выполняем команду
                target.Position = player.Position;
                // Уведомляем
                player.SendChatMessage($"{TxtClr.Warn}Вы телепортировали к себе {target._Nickname} [{target._ID}]");
                target.SendChatMessage($"{TxtClr.Warn}Администратор {player._Nickname} [{player._ID}] телепортировал вас к себе");
                if (player.AdminCompareTo(CPlayer.AdminRank.Creator) == -1 && player._ID != target._ID)
                {
                    foreach (var admin in CPlayer.GetAllAdmins())
                    {
                        if (admin._ID != player._ID)
                        {
                            admin.SendChatMessage($"{TxtClr.AdmCht}[A] {TxtClr.AdmEvnt}Админ {player._Nickname} [{player._ID}] телепортировал к себе игрока {target._Nickname} [{target._ID}]");
                        }
                    }
                }
                break;
        }
    }
    public static async Task AppendToJsonAsync(string filePath, dynamic content)
    {
        List<dynamic> list = new();
        if (File.Exists(filePath))
        {
            var lastJson = await File.ReadAllTextAsync(filePath);
            list = JsonConvert.DeserializeObject<List<dynamic>>(lastJson) ?? list;
        }
        list.Add(content);
        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
        {
           
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(list, Formatting.Indented));
            await stream.WriteAsync(bytes, 0, bytes.Length);
        }
    }
    [Command("save")]
    public static async void GetPos(CPlayer player, string positionName)
    {
        if (!player._LoggedIn) return;
        player.SendChatMessage($"Position X:{player.Position.X.ToString("0.000")}; Y:{player.Position.Y.ToString("0.000")}; Z:{player.Position.Z.ToString("0.000")}");
        player.SendChatMessage($"Rotation Roll:{player.Rotation.Roll.ToString("0.000")}; Pitch:{player.Rotation.Pitch.ToString("0.000")}; Yaw:{player.Rotation.Yaw.ToString("0.000")}");
        Alt.Log($"\nPosition {player.Position.X.ToString("0.000")}, {player.Position.Y.ToString("0.000")}, {player.Position.Z.ToString("0.000")}");
        Alt.Log($"Rotation {player.Rotation.Roll.ToString("0.000")}, {player.Rotation.Pitch.ToString("0.000")}, {player.Rotation.Yaw.ToString("0.000")}");

        dynamic item = new { Name = positionName, Position = $"{player.Position.X.ToString("0.000")} {player.Position.Y.ToString("0.000")} {player.Position.Z.ToString("0.000")}", Rotation = $"{player.Rotation.Roll.ToString("0.000")}, {player.Rotation.Pitch.ToString("0.000")}, {player.Rotation.Yaw.ToString("0.000")}" };
        try
        {
            await AppendToJsonAsync("savedpositions.json", item);
        }
        catch (JsonReaderException)
        {
            Alt.Log("[Save Position] Файл с позициями поврежден, запись не произведена!");
            player.SendChatMessage($"{TxtClr.Err}[Srv] Файл с позициями поврежден, запись не произведена!");
            return;
        }
        //  $"{positionName} = Position({player.Position.X.ToString("0.000")} , {player.Position.Y.ToString("0.000")}, {player.Position.X.ToString("0.000")}), Rotation({player.Rotation.Roll.ToString("0.000")}, {player.Rotation.Pitch.ToString("0.000")}, {player.Rotation.Yaw.ToString("0.000")})"
    }
    [Command("setpos")]
    public static void SetPos(CPlayer player, float x, float y, float z)
    {
        if (!player._LoggedIn) return;
        player.Emit("Client:BeforeChangePosition", x, y, z);
        player.Position = new(x, y, z);
        player.Emit("Client:PositionChanged");
    }
    [Command("tp")]
    public static void TeleportToWayPoint_CMD(CPlayer player)
    {
        if (!player._LoggedIn) return;
        player.Emit("Client:TeleportToWayPoint");
    }
    [ClientEvent("Server:TeleportToWayPoint")]
    public static void ServerTeleportToWayPoint(CPlayer player, Dictionary<string, float> coords)
    {
        if (!player._LoggedIn) return;
        player.Position = new AltV.Net.Data.Position(coords["x"], coords["y"], coords["z"]);
        player.SendChatMessage($"{TxtClr.Warn} Вы были тепортированы на метку {coords["x"]} , {coords["y"]}, {coords["z"]}");
    }
    [Command("sout")]
    public static void SwitchOut(CPlayer player)
    {
        player.SendChatMessage("sout");
        player.Emit("Client:SwitchOutPlayer");
    }
    [Command("sto")]
    public static void SwitchTo(CPlayer player)
    {
        player.SendChatMessage("sto");
        player.Emit("Client:SwitchToPlayer");
    }
    [Command("interior", aliases: new[] { "inter", "setint", "setinterior", "int" })]
    public static void SetInterior(CPlayer player, int interiorId)
    {
        if (!player._LoggedIn) return;
        var interiors = InteriorsManager.Instance();
        if (interiorId < 0 || interiorId >= interiors.Count)
        {
            player.SendChatMessage($"{TxtClr.Gray} Используйте /interior [0 - ${interiors.Count - 1}]");
            return;
        }
        var name = interiors.GetName(interiorId);
        var pos = interiors.GetPosition(interiorId);
        player.Emit("Client:BeforeChangePosition", pos.X, pos.Y, pos.Z);
        player.Position = pos;
        player.Emit("Client:PositionChanged");
        player.SendChatMessage($"{TxtClr.Warn}Вы были телепортированы в интерьер {name}_{interiorId}");
    }
}
