using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.Players;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class MaxHealthCommand : ChatCommandBase
    {
        private readonly PlayerManager _playerManager;

        public override string Command => "maxhealth";
        public override string Syntax => $"{Command}";

        public MaxHealthCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _playerManager = game.PlayerManager;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            {
                _playerManager.GetPeerInfo(userId).Champion.Stats.CurrentHealth = _playerManager.GetPeerInfo(userId).Champion.Stats.HealthPoints.Total;
            }

            ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.INFO, "Champion received full health!");

        }
    }
}
