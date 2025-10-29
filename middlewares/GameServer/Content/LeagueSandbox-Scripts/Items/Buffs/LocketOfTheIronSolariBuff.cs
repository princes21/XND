using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class LocketOfTheIronSolariBuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        private Shield NewShield;
        private Buff _buff;
        Particle LocketOfTheIronSolariEffectp_1;
        Particle LocketOfTheIronSolariEffectp_2;
        Particle LocketOfTheIronSolariEffectp_3;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            _buff = buff;
            float AP = owner.Stats.AbilityPower.Total * 1.45f;
            var ShieldAmount = 750f + AP;
            NewShield = new Shield(owner, owner, true, true, ShieldAmount);
            unit.AddShield(NewShield);

            LocketOfTheIronSolariEffectp_1 = AddParticleTarget(owner, owner, "Ironfist01.troy", owner);
            LocketOfTheIronSolariEffectp_2 = AddParticleTarget(owner, owner, "IronStylus_buf.troy", owner);
            LocketOfTheIronSolariEffectp_3 = AddParticleTarget(owner, owner, "IronStylus_cas.troy", owner);
        }

        public void OnUpdate(float diff)
        {
            if (!NewShield.IsConsumed()) return;
            _buff.DeactivateBuff();
        }

        public void OnDeactivate(AttackableUnit unit)
        {
            RemoveParticle(LocketOfTheIronSolariEffectp_1);
            RemoveParticle(LocketOfTheIronSolariEffectp_2);
            RemoveParticle(LocketOfTheIronSolariEffectp_3);
            unit.RemoveShield(NewShield);
        }
    }
}
