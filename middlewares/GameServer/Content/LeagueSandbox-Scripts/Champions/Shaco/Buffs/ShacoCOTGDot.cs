namespace Buffs
{
    internal class ShacoCOTGDot : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            float basePercentDamage = 0.012f + (0.0025f * (buff.OriginSpell.CastInfo.SpellLevel - 1));
            float AP = buff.SourceUnit.Stats.AbilityPower.Total * 0.00002f;
            float damage = buff.TargetUnit.Stats.HealthPoints.Total * (basePercentDamage + AP);

            var data = buff.TargetUnit.TakeDamage(buff.SourceUnit, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PROC, false);
            buff.SourceUnit.Stats.CurrentHealth += data.PostMitigationDamage;
        }
    }
}
