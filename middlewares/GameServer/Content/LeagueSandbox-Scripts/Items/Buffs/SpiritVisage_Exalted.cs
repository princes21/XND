using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects;

namespace Buffs
{
    class SpiritVisage_Exalted : IBuffGameScript
    {
        Particle p;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            IsHidden = false
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            p = AddParticleTarget(unit, unit, "Ironfist01.troy", unit);
            var PercentageAmount = 0.65f;
            var AllStatsTo_MaxHealth = 1000f + unit.Stats.AttackDamage.Total * PercentageAmount + unit.Stats.AbilityPower.Total * PercentageAmount+ unit.Stats.Armor.Total * PercentageAmount + unit.Stats.MagicResist.Total * PercentageAmount + unit.Stats.Range.Total * PercentageAmount + unit.Stats.MoveSpeed.Total * PercentageAmount;

            StatsModifier.Armor.FlatBonus = 250f;
            StatsModifier.MagicResist.FlatBonus = 250f;
            StatsModifier.Size.PercentBonus = 0.40f;
            StatsModifier.HealthPoints.FlatBonus = AllStatsTo_MaxHealth;
            unit.AddStatModifier(StatsModifier);
        }
    }
}