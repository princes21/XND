using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class SummonerReviveExtraBoost : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HASTE,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            StatsModifier.AttackDamage.PercentBonus = 0.25f;
            StatsModifier.AbilityPower.PercentBonus = 0.25f;
            StatsModifier.Armor.FlatBonus = 100f;
            StatsModifier.MagicResist.FlatBonus = 100f;
            StatsModifier.MagicPenetration.PercentBonus = 1.0f;
            unit.AddStatModifier(StatsModifier);
        }
    }
}
