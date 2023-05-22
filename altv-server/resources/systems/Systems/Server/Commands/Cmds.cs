using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Resources.Chat.Api;
using Newtonsoft.Json;
using Utility.Factions;

internal class Cmds : IScript
{
    /*[CommandEvent(CommandEventType.CommandNotFound)]
    public static void OnCommandNotFound(IPlayer player, string cmd)
    {

    }*/
    [Command("id")]
    public static void id_CMD(CPlayer source, int id)
    {
        if (id < 0 || id > 128)
        {
            return;
        }
        if (source._LoggedIn && source._ID.Value == id)
        {
            source.SendChatMessage($"{source._Nickname} [{id}]");
            return;
        }
        foreach (var obj in Alt.GetAllPlayers())
        {
            var player = obj as CPlayer;
            if (player != null && player._LoggedIn && player._ID.Value == id && player._ID.Value != source._ID.Value)
            {
                source.SendChatMessage($"{player._Nickname} [{id}]");
                return;
            }
        }
        source.SendChatMessage("{FFFF00}" + $"Игрок ID:{id} не в сети!");
    }
    public static void CreateUIMenu(CPlayer player, string callbackEvent, int targetId, string title, string description, string json)
    {
        Alt.Log($"Json: {json}");
        if (player._InMenu == false)
        {
            player._InMenu = true;
            player.Emit("Client:UIMenu:Create", callbackEvent, targetId, title, description, json);
        }
    }
    [ClientEvent("Server:UIMenu:Close")]
    public static void CloseUIMenu(CPlayer player)
    {
        player._InMenu = false;
    }
    [ClientEvent("Server:CMD:faction:Process")]
    public static void factionProcess(CPlayer player, int targetId, string itemName, int index)
    {
        if (!player._InMenu) return;
        if (!player._LoggedIn) return;
        if (player._Faction == Faction.ID.None) return;
        if (!Faction.IsLeader(player._Faction, player._Rank) &&
            !Faction.IsDeputy(player._Faction, player._Rank) &&
            !Faction.IsAssistantDeputy(player._Faction, player._Rank)) return;
        CPlayer target = null;
        if (player._ID == targetId)
        {
            target = player;
        }
        else
        {
           target = (CPlayer)Alt.GetAllPlayers().FirstOrDefault(p => (p as CPlayer)._ID == targetId);
        }
        if (target == null) return;
        if (target._Faction != player._Faction) return;
        switch (itemName)
        {
            case "Ранг":
                if (index < 0 || index > Faction.MaxRank(target._Faction)) return;
                target._Rank = (byte)index;
                target.SendChatMessage("{FFFF00}" + $"{player._Nickname} понизил/повысил вас до {target._Rank + 1} ранга");
                break;
            case "Форма":
                if (index < 0 || index > Faction.MaxSkin(player._Faction)) return;
                target.Model = (uint)Faction.Skins(player._Faction)[index];
                target.SendChatMessage("{FFFF00}" + $"{player._Nickname} выдал вам новую форму");
                break;
        }
    }

    [Command("faction")]
    public static void Faction_CMD(CPlayer player, int targetId) 
    {
        if (!player._LoggedIn)
        {
            return;
        }
        Alt.Log($"{player._Nickname} авторизован!");
        if (!Faction.IsLeader(player._Faction, player._Rank) &&
            !Faction.IsDeputy(player._Faction, player._Rank) &&
            !Faction.IsAssistantDeputy(player._Faction, player._Rank))
        {
            return;
        }
        Alt.Log($"{player._Nickname} лидер/зам/помошник зама!");
        CPlayer target = null;
        if (player._ID == targetId)
        {
            Alt.Log($"{player._Nickname} == {targetId}");
            target = player;
        }
        else {
            Alt.Log($"{player._Nickname} != {targetId}");
            CPlayer result = (CPlayer)Alt.GetAllPlayers().FirstOrDefault(p => (p as CPlayer)._ID == targetId);
            if (result == null)
            {
                Alt.Log("resutl == null");
                return;
            }
            Alt.Log($"{result._Nickname} == {targetId}");
            target = result;
        }
        if (!target._LoggedIn || player._Faction != target._Faction)
        {
            Alt.Log($"Последний if не прошел");
            return;
        }
        bool isSimilarRank = player._Rank == target._Rank;
        //string[] ranks = isSimilarRank ? null : Faction.Ranks(player._Faction);
        //int? skinsLength = !Faction.IsLeader(player._Faction, player._Rank) && !Faction.IsDeputy(player._Faction, player._Rank) ? null : Faction.Skins(player._Faction).Length;
        Alt.Log($"Открывается UI Menu");
        //Alt.Log($"id {target._ID}, name {target._Nickname}, currank {target._Rank}, ranks {ranks?.Length}, skins {skinsLength}");

        string[] menuItems = { "Ранг", "Форма" };
        List<dynamic> list = new List<dynamic>();
        if (!isSimilarRank)
        {
            list.Add(new { Name = "Ранг", Description = "Ранг игрока во фракции", Items = Faction.Ranks(player._Faction), Current = player._Rank });
        }
        if (Faction.IsLeader(player._Faction, player._Rank) || Faction.IsDeputy(player._Faction, player._Rank))
        {
            list.Add(new { Name = "Форма", Description = "Форма игрока во фракции", Items = Enumerable.Range(1, Faction.MaxSkin(player._Faction)).Select(i => i.ToString()).ToArray() });
        }
        CreateUIMenu(player, "Server:CMD:faction:Process", targetId,  target._Nickname, "Управление членом фракции", JsonConvert.SerializeObject(list));
    }
}

