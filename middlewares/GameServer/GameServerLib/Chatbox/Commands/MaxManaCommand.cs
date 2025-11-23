using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.Players;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class MaxManaCommand : ChatCommandBase
    {
        private readonly PlayerManager _playerManager;

        public override string Command => "maxmana";
        public override string Syntax => $"{Command}";

        public MaxManaCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _playerManager = game.PlayerManager;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            {
                _playerManager.GetPeerInfo(userId).Champion.Stats.CurrentMana = _playerManager.GetPeerInfo(userId).Champion.Stats.ManaPoints.Total;
            }

            ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.INFO, "Champion received full mana!");

        }
    }
}
