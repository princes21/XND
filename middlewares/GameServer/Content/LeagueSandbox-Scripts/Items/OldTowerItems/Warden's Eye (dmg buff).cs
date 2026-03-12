using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using static LeaguePackets.Game.Common.CastInfo;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_1503 : IItemScript
    {
        ObjAIBase owner;
        AttackableUnit Target;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnActivate(ObjAIBase owner)
        {
            this.owner = owner;
            StatsModifier.AttackSpeed.PercentBonus = 1.2f;
            owner.AddStatModifier(StatsModifier);
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnHitUnit, false);

        }

        private void OnHitUnit(DamageData data)
        {
            float minutes = GameTime() / (1000f * 60f);
            float Damage = data.Damage;
            float AttackerDamage = data.Attacker.Stats.AttackDamage.Total;
            float AttackerAP = data.Attacker.Stats.AbilityPower.Total;
            var turret = data.Attacker;
            float turredAd = turret.Stats.AttackDamage.Total;
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