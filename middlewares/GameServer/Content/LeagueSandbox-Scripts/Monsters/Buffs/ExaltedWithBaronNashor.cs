using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects;

namespace Buffs
{
    class ExaltedWithBaronNashor : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            IsHidden = false
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            float BonusHealth = 2500f; // 2500
            StatsModifier.HealthPoints.FlatBonus = BonusHealth;
            unit.Stats.CurrentHealth += BonusHealth;
            StatsModifier.AttackSpeed.FlatBonus = 1.50f; // 1.20f
            StatsModifier.AttackDamage.FlatBonus = 150f; // 150
            StatsModifier.AbilityPower.FlatBonus = 300f; // 300
            StatsModifier.Armor.FlatBonus = 200f;
            StatsModifier.MagicResist.FlatBonus = 300f;
            StatsModifier.Size.PercentBonus = 0.25f;
            unit.AddStatModifier(StatsModifier);
        }
    }
}