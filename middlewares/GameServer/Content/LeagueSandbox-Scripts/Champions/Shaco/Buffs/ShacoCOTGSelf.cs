namespace Buffs
{
    internal class ShacoCOTGSelf : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Pet ghost = buff.SourceUnit.GetPet();

            var armor = unit.Stats.Armor.BaseValue * 0.50f;
            var mr = unit.Stats.MagicResist.BaseValue * 0.50f;
            var s = unit.Stats.AbilityPower.BaseValue * 0.75f;
            var a = unit.Stats.AttackDamage.BaseValue * 0.75f;
            var d = unit.Stats.ArmorPenetration.BaseValue;
            var y = unit.Stats.MagicPenetration.BaseValue;
            var o = unit.Stats.Level;

            ghost.Stats.CurrentHealth = unit.Stats.CurrentHealth;
            ghost.Stats.CurrentMana = unit.Stats.CurrentMana;
            ghost.Stats.HealthPoints.BaseValue = unit.Stats.HealthPoints.BaseValue;
            ghost.Stats.ManaPoints.BaseValue = unit.Stats.ManaPoints.BaseValue;
            ghost.Stats.Armor.BaseValue = armor;
            ghost.Stats.Armor.BaseValue = mr;
            ghost.Stats.ArmorPenetration.BaseValue = d;
            ghost.Stats.MagicPenetration.BaseValue = y;
            ghost.Stats.Level = o;
            ghost.Stats.AbilityPower.BaseValue = s;
            ghost.Stats.AttackDamage.BaseValue = a; 
            ghost.Stats.MoveSpeed.BaseValue = unit.Stats.MoveSpeed.BaseValue;   

            ghost.AddStatModifier(StatsModifier);
        }
    }
}
