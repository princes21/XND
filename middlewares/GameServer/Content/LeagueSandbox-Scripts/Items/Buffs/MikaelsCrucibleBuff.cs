using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class MikaelsCrucibleBuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        Particle MikaelsCrucibleEffect;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            unit.Stats.CurrentHealth = 350;
            StatsModifier.MoveSpeed.PercentBonus = 0.20f;
            unit.AddStatModifier(StatsModifier);
            MikaelsCrucibleEffect = AddParticleTarget(owner, unit, "Global_SS_Ghost.troy", unit);
        }
        public void OnDeactivate(AttackableUnit unit)
        {
            RemoveParticle(MikaelsCrucibleEffect);
        }
    }
}
