using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeaguePackets.Game.Common.CastInfo;

namespace Buffs
{
    internal class ShacoCOTGPetBuff2 : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.AURA,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public SpellSector DRMundoWAOE;
        Buff Buff;
        Particle p;
        Particle p2;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Buff = buff;
            AddBuff("ShacoCOTGPetBuff", buff.Duration, 1, ownerSpell, unit, buff.SourceUnit);
            buff.SourceUnit.SetSpell("HallucinateGuide", 3, true);
            AddBuff("ShacoCOTGSelf", buff.Duration, 1, ownerSpell, buff.SourceUnit, buff.SourceUnit);
            OnDeath.AddListener(this, unit, OnGhostDeath, true);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            //In theory the killer should be null, but that causes the ghost to not die(?)
            unit.Die(CreateDeathData(false, 0, unit, unit, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));
            RemoveParticle(p);
            RemoveParticle(p2);
            RemoveBuff(buff.SourceUnit, "ShacoCOTGSelf");
            Spell spell = buff.SourceUnit.SetSpell("HallucinateFull", 3, true);

            //Check if this is done on-script or should be handled automatically
            spell.SetCooldown(spell.GetCooldown() - buff.TimeElapsed);
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {

        }
        public void OnGhostDeath(DeathData data)
        {
           AddBuff("ShacoExplode", 0.5f, 1, Buff.OriginSpell, Buff.TargetUnit, Buff.SourceUnit);
           Buff.DeactivateBuff();
        }
    }
}
