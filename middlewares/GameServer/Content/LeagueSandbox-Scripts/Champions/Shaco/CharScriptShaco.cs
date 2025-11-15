using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using static LeagueSandbox.GameServer.API.ApiEventManager;

namespace Characters
{
    public class CharScriptShaco : ICharScript
    {
        public void OnActivate(Champion owner) { }

        public void OnDeactivate(Champion owner) { }

        public void OnUpdate(float diff) { }

        public void OnMove(Champion owner, Vector2 newPosition) { }

        public void OnAttack(Champion owner, AttackableUnit target) { }
    }
}