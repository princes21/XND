using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Buffs
{
    internal class DesperatePower : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            StatsModifier.AbilityPower.FlatBonus = 360;
            StatsModifier.SpellVamp.PercentBonus = 0.2f + (0.1f * ownerSpell.CastInfo.SpellLevel);
            StatsModifier.MoveSpeed.PercentBonus = 0.2f + (0.1f * ownerSpell.CastInfo.SpellLevel);
            unit.AddStatModifier(StatsModifier);
            AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ryze_DarkCrystal_Death.troy", unit, buff.Duration);
            AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "manaleach_tar.troy", unit, buff.Duration, bone: "hip");
        }
    }
}