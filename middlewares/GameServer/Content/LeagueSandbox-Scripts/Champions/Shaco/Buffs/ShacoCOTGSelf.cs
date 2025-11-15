using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class ShacoCOTGSelf : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new()
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (buff.SourceUnit.GetPet() is not Pet ghost) return;

            ghost.Stats.CurrentHealth = unit.Stats.CurrentHealth;
            ghost.Stats.CurrentMana = unit.Stats.CurrentMana;
            ghost.Stats.HealthPoints.BaseValue = unit.Stats.HealthPoints.BaseValue;
            ghost.Stats.ManaPoints.BaseValue = unit.Stats.ManaPoints.BaseValue;
            ghost.Stats.Armor.BaseValue = unit.Stats.Armor.BaseValue * 0.5f;
            ghost.Stats.MagicResist.BaseValue = unit.Stats.MagicResist.BaseValue * 0.5f;
            ghost.Stats.ArmorPenetration.BaseValue = unit.Stats.ArmorPenetration.BaseValue;
            ghost.Stats.MagicPenetration.BaseValue = unit.Stats.MagicPenetration.BaseValue;
            ghost.Stats.Level = unit.Stats.Level;
            ghost.Stats.AbilityPower.BaseValue = unit.Stats.AbilityPower.BaseValue * 0.75f;
            ghost.Stats.AttackDamage.BaseValue = unit.Stats.AttackDamage.BaseValue * 0.75f;
            ghost.Stats.MoveSpeed.BaseValue = unit.Stats.MoveSpeed.BaseValue;
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell) { }
        public void OnUpdate(float diff) { }
    }
}