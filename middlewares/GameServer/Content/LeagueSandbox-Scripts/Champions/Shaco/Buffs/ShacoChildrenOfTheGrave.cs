using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Buffs
{
    internal class ShacoChildrenOfTheGrave : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private Buff Buff;
        private float timer = 1000.0f;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Buff = buff;

            // Listen for unit death
            OnDeath.AddListener(this, unit, OnTargetDeath, true);

            // Deal immediate damage based on percentage and AP
            float basePercentDamage = 0.12f + (0.025f * (ownerSpell.CastInfo.SpellLevel - 1));
            float AP = buff.SourceUnit.Stats.AbilityPower.Total * 0.0002f;
            float damage = buff.TargetUnit.Stats.HealthPoints.Total * (basePercentDamage + AP);

            var data = buff.TargetUnit.TakeDamage(buff.SourceUnit, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PROC, false);

            // Heal Shaco for damage dealt
            buff.SourceUnit.Stats.CurrentHealth += data.PostMitigationDamage;

            // Add revive buff to spawn the clone
            AddBuff("ShacoCOTGRevive", 18.0f, 1, ownerSpell, buff.SourceUnit, buff.SourceUnit);
        }

        public void OnTargetDeath(DeathData data)
        {
            RemoveBuff(Buff);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            OnDeath.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
            timer -= diff;

            if (timer <= 0)
            {
                AddBuff("ShacoCOTGDot", 0.01f, 1, Buff.OriginSpell, Buff.TargetUnit, Buff.SourceUnit);
                timer = 1000.0f;
            }
        }
    }
}
