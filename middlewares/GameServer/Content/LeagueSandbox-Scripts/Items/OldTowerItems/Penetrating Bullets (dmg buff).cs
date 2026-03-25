using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using static LeaguePackets.Game.Common.CastInfo;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_1500 : IItemScript
    {
        ObjAIBase owner;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();



        public void OnActivate(ObjAIBase owner)
        {
            this.owner = owner;
            StatsModifier.AttackSpeed.PercentBonus = 1.2f;
            StatsModifier.Range.FlatBonus = 200f;
            owner.AddStatModifier(StatsModifier);
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnHitUnit, false);
        }

        private void OnHitUnit(DamageData data)
        {
            float minutes = GameTime() / (1000f * 60f);
            float MinionHealth = data.Target.Stats.HealthPoints.Total;
            var target = data.Target;
            float DamageDealtPercent = MinionHealth * 0.33f;

            if (data.DamageSource == DamageSource.DAMAGE_SOURCE_ATTACK && minutes < 30f && target is LaneMinion)
            {
                data.PostMitigationDamage = DamageDealtPercent;
            }
        }
    }
}