using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AltV.Net.Enums;
using static Utility.Factions.Faction;

namespace Utility.Factions
{
    public struct FactionInfo
    {
        public string Name;
        public string[] Ranks;
        public int[] CanInvite;
        public int[] CanUninvite;
        public int[] CanGiveRank;
        public PedModel[] Skins;
    }
    public static class Faction
    {
        public enum ID : byte
        {
            None = 0,
            FBI, LSPD, BSCO,
        }
        private static Dictionary<ID, FactionInfo> Data;
        static Faction()
        {
            Data = new Dictionary<ID, FactionInfo>(new KeyValuePair<ID, FactionInfo>[] {
                    new (ID.FBI,
                        new FactionInfo{
                            Name = "F.B.I.",
                            Ranks = new[]{
                                "Стажёр", "Младший Агент", "Агент", "Старший Агент", "Специальный Агент",
                                "Секретный Агент", "Агент Безопасности", "Управляющий Агентурой", "Глава Академии F.B.I.",
                                "Зам. Главы Управления DEP", "Глава Управления DEP",
                                "Зам. Главы Управления CID", "Глава Управления CID",
                                "Начальник NS", "Инспектор F.B.I.", "Директор F.B.I."
                            },
                            CanInvite = new[]{1, 2},
                            CanUninvite = new[]{1, 2, 3},
                            CanGiveRank = new[]{1, 2, 3},
                            Skins = new[]{
                                PedModel.DaveNorton, PedModel.CiaSec01SMM, PedModel.Devinsec01SMY, PedModel.DoaMan,
                                PedModel.FbiSuit01, PedModel.FibArchitect, PedModel.FibMugger01, PedModel.FibOffice01SMM,
                                PedModel.FibOffice02SMM, PedModel.FibSec01, PedModel.FibSec01SMM, PedModel.Highsec01SMM,
                                PedModel.Highsec02SMM, PedModel.JewelSec01, PedModel.JewelThief, PedModel.KarenDaniels,
                                PedModel.SteveHains }
                        }
                    ),
                    new (ID.LSPD,
                        new FactionInfo{
                            Name = "L.S.P.D.",
                            Ranks = new[]{
                                "Кадет", "Офицер",
                                "Мл. Сержант", "Сержант", "Ст. Сержант",
                                "Прапорщик", "Ст. Прапорщик",
                                "Мл. Лейтенант", "Лейтенант", "Ст. Лейтенант", "Капитан",
                                "Майор", "Подполковник", "Полковник", "Шериф"
                            },
                            CanInvite = new[]{1, 2},
                            CanUninvite = new[]{1, 2, 3},
                            CanGiveRank = new[]{1, 2, 3},
                            Skins = new[]
                            {
                                PedModel.Cop01SFY, PedModel.Cop01SMY
                            }
                        }
                    ),
                    new (ID.BSCO,
                        new FactionInfo{
                            Name = "B.C.S.O.",
                            Ranks = new[]{
                                "Кадет", "Офицер",
                                "Мл. Сержант", "Сержант", "Ст. Сержант",
                                "Прапорщик", "Ст. Прапорщик",
                                "Мл. Лейтенант", "Лейтенант", "Ст. Лейтенант", "Капитан",
                                "Майор", "Подполковник", "Полковник", "Шериф"
                            },
                            CanInvite = new[]{1, 2},
                            CanUninvite = new[]{1, 2, 3},
                            CanGiveRank = new[]{1, 2, 3},
                            Skins = new[]
                            {
                                PedModel.Cop01SFY, PedModel.Cop01SMY
                            }
                        }
                    )
                });
        }
        public static bool IsNotNoneID(ID id)
        {
            return id != ID.None;
        }
        public static string[]? Ranks(ID id)
        {
            return IsNotNoneID(id) ? Data[id].Ranks : null;
        }
        public static int MinRank()
        {
            return 0;
        }
        public static int MaxRank(ID id)
        {
            return IsNotNoneID(id) ? Data[id].Ranks.Length - 1 : -1;
        }
        public static bool IsInRange(ID id, int rank)
        {
            return IsNotNoneID(id) ? rank >= MinRank() && rank <= MaxRank(id) : false;
        }
        public static bool IsLeader(ID id, int rank)
        {
            return IsNotNoneID(id) && rank == MaxRank(id);
        }
        public static bool IsDeputy(ID id, int rank)
        {
            return IsNotNoneID(id) && rank == (MaxRank(id) - 1);
        }
        public static bool IsAssistantDeputy(ID id, int rank)
        {
            return IsNotNoneID(id) && rank == (MaxRank(id) - 2);
        }
        public static int GiveLeaderRank(ID id)
        {
            return IsNotNoneID(id) ? Data[id].Ranks.Length - 1 : -1;
        }
        public static PedModel[]? Skins(ID id)
        {
            return IsNotNoneID(id) ? Data[id].Skins : null;
        }
        public static int MinSkin()
        {
            return 0;
        }
        public static int MaxSkin(ID id)
        {
            return IsNotNoneID(id) ? Data[id].Skins.Length - 1 : -1;
        }
        public static string? Name(ID id)
        {
            return IsNotNoneID(id) ? Data[id].Name : null;
        }
        public static string? RankName(ID id, int rank)
        {
            return IsNotNoneID(id) ? Data[id].Ranks[rank] : null;
        }
    }
}
