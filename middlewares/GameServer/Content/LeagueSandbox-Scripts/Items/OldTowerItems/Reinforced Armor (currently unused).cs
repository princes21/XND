using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_1502 : IItemScript //unsure if it's even needed but maybe I'll leave it here and do something about it later
    {
        ObjAIBase owner;
        private Buff sourceBuff;
        DamageData _damageData;
        AttackableUnit Attacker;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();



        public void OnActivate(ObjAIBase owner)
        {
            this.owner = owner as LaneTurret;
            owner.AddStatModifier(StatsModifier);
        }

        private void OnUpdate(float diff)
        {
            var nearestObjects = GetUnitsInRange(owner.Position, owner.Stats.Range.Total, true);

            foreach (var obj in nearestObjects)
            {

            }
        }
    }
}