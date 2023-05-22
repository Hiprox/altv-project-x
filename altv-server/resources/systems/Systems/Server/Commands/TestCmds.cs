using AltV.Net;
using AltV.Net.Resources.Chat.Api;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    internal class TestCmds : IScript
    {
        [Command("editor")]
        public static void Editor_CMD(CPlayer player)
        {
            Alt.Log("editor worked");
            player.Emit("UiMenu.Editor.Open", (int)player._Gender);
        }
    }
}
