using GameServerCore.Enums;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.Players;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class KillCommand : ChatCommandBase
    {
        private readonly PlayerManager _playerManager;

        public override string Command => "kill";
        public override string Syntax => $"{Command} (minions (blue|purple)|champions|towers (blue|purple)|inhibitors (blue|purple)";
        public KillCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _playerManager = game.PlayerManager;
        }
        private void KillMinions()
        {
            var objects = Game.ObjectManager.GetObjects();
            foreach (var o in objects)
            {
                if (o.Value is Minion minion)
                {
                    KillUnit(minion);
                }
            }
        }

        private void KillInhibitors()
        {
            var objects = Game.ObjectManager.GetObjects();
            foreach (var o in objects)
            {
                if (o.Value is Inhibitor inhibitor)
                {
                    KillUnit(inhibitor);
                }
            }
        }

        private void KillTowersForTeam(string team)
        {
            var objects = Game.ObjectManager.GetObjects();
            foreach (var o in objects)
            {
                if (o.Value is LaneTurret tower)
                {
                    var towerTeam = tower.Team;

                    if (team == null || towerTeam == TeamId.TEAM_BLUE && team.ToLower() == "blue" ||
                                          towerTeam == TeamId.TEAM_PURPLE && team.ToLower() == "purple")
                    {
                        KillUnit(tower);
                    }
                }
            }
        }


        private void KillTowers()
        {
            var objects = Game.ObjectManager.GetObjects();
            foreach (var o in objects)
            {
                if (o.Value is LaneTurret tower)
                {
                    KillUnit(tower);
                }
            }
        }
        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var split = arguments.ToLower().Split(' ');

            if (split.Length < 2)
            {
                ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.SYNTAXERROR);
                ShowSyntax();
            }
            else
            {
                switch (split[1])
                {
                    case "minions":
                        if (split.Length >= 3 && (split[2] == "blue" || split[2] == "purple"))
                        {
                            KillMinionsForTeam(split[2]);
                        }
                        else
                        {
                            KillMinions();
                        }
                        break;
                    case "champions":
                        if (split.Length >= 3 && (split[2] == "blue" || split[2] == "purple"))
                        {
                            KillChampionsForTeam(split[2]);
                        }
                        else
                        {
                            KillChampions();
                        }
                        break;
                    case "towers":
                        if (split.Length >= 3 && (split[2] == "blue" || split[2] == "purple"))
                        {
                            KillTowersForTeam(split[2]);
                        }
                        else
                        {
                            KillTowers();
                        }
                        break;
                    case "inhibitors":
                        if (split.Length >= 3 && (split[2] == "blue" || split[2] == "purple"))
                        {
                            KillInhibitorsForTeam(split[2]);
                        }
                        else
                        {
                            KillInhibitors();
                        }
                        break;
                    default:
                        ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.SYNTAXERROR);
                        ShowSyntax();
                        break;
                }
            }
        }

        private void KillMinionsForTeam(string team)
        {
            var objects = Game.ObjectManager.GetObjects();
            foreach (var o in objects)
            {
                if (o.Value is Minion minion)
                {
                    var minionTeam = minion.Team;

                    if (team == null || minionTeam == TeamId.TEAM_BLUE && team.ToLower() == "blue" ||
                                          minionTeam == TeamId.TEAM_PURPLE && team.ToLower() == "purple")
                    {
                        KillUnit(minion);
                    }
                }
            }
        }

        private void KillInhibitorsForTeam(string team)
        {
            var objects = Game.ObjectManager.GetObjects();
            foreach (var o in objects)
            {
                if (o.Value is Inhibitor inhibitor)
                {
                    var inhibitorTeam = inhibitor.Team;

                    if (team == null || inhibitorTeam == TeamId.TEAM_BLUE && team.ToLower() == "blue" ||
                                          inhibitorTeam == TeamId.TEAM_PURPLE && team.ToLower() == "purple")
                    {
                        KillUnit(inhibitor);
                    }
                }
            }
        }

        private void KillChampions()
        {
            var objects = Game.ObjectManager.GetObjects();
            foreach (var o in objects)
            {
                if (o.Value is Champion champion)
                {
                    KillUnit(champion);
                }
            }
        }

        private void KillChampionsForTeam(string team)
        {
            var objects = Game.ObjectManager.GetObjects();
            foreach (var o in objects)
            {
                if (o.Value is Champion champion)
                {
                    var championTeam = champion.Team;

                    if (team == null || championTeam == TeamId.TEAM_BLUE && team.ToLower() == "blue" ||
                                          championTeam == TeamId.TEAM_PURPLE && team.ToLower() == "purple")
                    {
                        KillUnit(champion);
                    }
                }
            }
        }

        private void KillUnit(AttackableUnit unit)
        {
            unit.Die(new DeathData
            {
                BecomeZombie = false,
                DieType = 0,
                Unit = unit,
                Killer = unit,
                DamageType = (byte)DamageType.DAMAGE_TYPE_PHYSICAL,
                DamageSource = (byte)DamageSource.DAMAGE_SOURCE_RAW,
                DeathDuration = 0
            });
        }
    }
}