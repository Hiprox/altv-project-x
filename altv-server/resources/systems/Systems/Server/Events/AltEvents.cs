using System.Collections.Generic;
using AltV.Net;
using AltV.Net.CApi.ClientEvents;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Org.BouncyCastle.Tls;
using AltV.Net.Async;

public class AltEvents : IScript
{
    [ScriptEvent(ScriptEventType.PlayerConnect)]
    public static void OnPlayerConnect(CPlayer player, string reason)
    {
        return;
    }

    [ScriptEvent(ScriptEventType.PlayerDisconnect)]
    public static void OnPlayerDisconnect(CPlayer player, string reason)
    {
        player._ID.Free(); // Освобождаем ID
        if (string.IsNullOrEmpty(reason))
        {
            reason = "Выход";
        }
        Alt.Log($"> Игрок '{player.Name}' отключился. Причина: {reason}.");
    }

    [AsyncScriptEvent(ScriptEventType.PlayerDead)]
    public static async Task OnPlayerDead(CPlayer player, IEntity killer, uint weapon)
    {
        //player.Emit("Client:Spawn", 0, 0, 75);
        await Task.Delay(2500);
        player.Emit("Client:BeforeSpawn", 0f, 0f, 75f, 500);
        await Task.Delay(500);
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
}