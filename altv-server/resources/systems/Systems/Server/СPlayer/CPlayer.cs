using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using System.Numerics;
using Utility;
using Utility.Factions;
using AltV.Net.Async;
using AltV.Net.Async.Elements.Entities;

public class CPlayer : AsyncPlayer
{
    public CPlayer? GetPlayerByID(int targetId)
    {
        return (CPlayer?)(this._ID == targetId ?
            this :
            Alt.GetAllPlayers().FirstOrDefault(p =>
            {
                return p is not CPlayer temp ? false : temp._ID == targetId;
            }));
    }
    public static CPlayer[] GetAllAdmins()
    {
        return Alt.GetAllPlayers().Where(p =>
            {
                return p is not CPlayer temp ? false : temp._Admin != AdminRank.None;
            }).OfType<CPlayer>().ToArray();
    }
    public CPlayer(ICore core, IntPtr nativePointer, ushort id) : base(core, nativePointer, id)
    {
        _ID = new UserID();
        _Nickname = string.Empty;
        _Admin = AdminRank.None;
        _LoggedIn = false;
        _Gender = Gender.None;
        _DataBaseID = -1;
        _Money = 0;
        _Faction = Faction.ID.None;
        _Rank = 0;
        _InMenu = false;
    }
    public enum Gender : byte
    {
        Male = 0,
        Female,
        None
    }
    public enum AdminRank : byte
    {
        None = 0,
        Helper,
        Moderator,
        SeniorModerator,
        Admin,
        Curator,
        TechnicalAdmin,
        DeputyChiefAdmin,
        ChiefAdmin,
        Creator
    }
    public UserID _ID;
    public bool _LoggedIn { get; set; }
    public Gender _Gender { get; set; }
    // DataBase fields
    public long _DataBaseID { get; set; }
    public string _Nickname { get; set; }
    public long _Money { get; set; }
    public AdminRank _Admin { get; set; }
    public Faction.ID _Faction { get; set; }
    public int _Rank { get; set; }
    public bool _InMenu { get; set; }
    public bool IsInFaction()
    {
        return _Faction != Faction.ID.None;
    }
    public bool IsLeaders() => Faction.IsLeader(_Faction, _Rank);
    public bool IsDeputy() => Faction.IsDeputy(_Faction, _Rank);
    public bool IsAssistantDeputy() => Faction.IsAssistantDeputy(_Faction, _Rank);
    public bool IsValidRank(int rank) => Faction.IsInRange(_Faction, rank);
    public string? GetFactionName() => Faction.Name(_Faction);
    public string? GetFactionRankName() => Faction.RankName(_Faction, _Rank);
    public int MinFactionSkin() => Faction.MinSkin();
    public int MaxFactionSkin() => Faction.MaxSkin(_Faction);
    public int CurrentFactionSkinIndex() => Array.IndexOf(Faction.Skins(_Faction) ?? new PedModel[] { }, (PedModel)this.Model);
    public int MinFactionRank() => Faction.MinRank();
    public int MaxFactionRank() => Faction.MaxRank(_Faction);
    public bool IsAdmin() => _Admin != AdminRank.None;
    public bool IsAdmin(AdminRank adminRank) => _Admin == adminRank;
    public int AdminCompareTo(AdminRank adminRank) => _Admin.CompareTo(adminRank);
}

