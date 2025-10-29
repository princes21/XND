using GameServerCore.Enums;
using System.Collections.Generic;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class GangplankE : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private List<Particle> Particles => new List<Particle>();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            StatsModifier.AttackSpeed.PercentBonus = 0.75f;
            StatsModifier.MoveSpeed.PercentBonus = 0.15f;
            StatsModifier.AttackDamage.PercentBonus = 0.30f;
            unit.AddStatModifier(StatsModifier);

            Particles.Add(AddParticleTarget(owner, null, "pirate_raiseMorale_cas", unit));
            Particles.Add(AddParticleTarget(owner, null, "pirate_raiseMorale_mis", unit));
            Particles.Add(AddParticleTarget(owner, null, "pirate_raiseMorale_tar", unit));
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Particles.ForEach(particle => RemoveParticle(particle));
        }
    }
}
