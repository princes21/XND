using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Collections.Generic;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class InvulnerableTowersCommand : ChatCommandBase
    {
        public override string Command => "invtowers";
        public override string Syntax => $"{Command} 0 (disable) / 1 (enable)";
        private Dictionary<uint, BaseTurret> _turrets;


        public InvulnerableTowersCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {

        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var split = arguments.ToLower().Split(' ');

            if (split.Length < 2 || !byte.TryParse(split[1], out var input) || input > 1)
            {
                ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.SYNTAXERROR);
                ShowSyntax();
            }
            else
            {
                foreach (var turret in _turrets)
                {
                  //  _turrets. TODO: finish
                }
            }
        }
    }
}
