

using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;

namespace Buffs
{
    internal class Feast : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_OVERLAPS,
            MaxStacks = 999
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        Buff thisBuff;
        AttackableUnit Unit;
        Spell Spell;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Spell = ownerSpell;
            Unit = unit;
            var owner = ownerSpell.CastInfo.Owner;
            thisBuff = buff;

            var MaxHealthRatio = owner.Stats.HealthPoints.Total * 0.015f;
            var HealthBuff = 90f * ownerSpell.CastInfo.SpellLevel;
            StatsModifier.HealthPoints.BaseBonus += HealthBuff + MaxHealthRatio;

            unit.AddStatModifier(StatsModifier);
            if (!owner.IsDead) unit.Stats.CurrentHealth += HealthBuff;

        }
    }
}

















