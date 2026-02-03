using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeaguePackets.Game.Common.CastInfo;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

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
            ApiEventManager.OnDeath.AddListener(this, unit, OnGhostDeath, true);
            Buff = buff;
            AddBuff("ShacoCOTGPetBuff", buff.Duration, 1, ownerSpell, unit, buff.SourceUnit);
            buff.SourceUnit.SetSpell("HallucinateGuide", 3, true);
            AddBuff("ShacoCOTGSelf", buff.Duration, 1, ownerSpell, buff.SourceUnit, buff.SourceUnit);
            ApiEventManager.OnDeath.AddListener(this, unit, OnGhostDeath, true);

        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            //In theory the killer should be null, but that causes the ghost to not die(?)

            RemoveParticle(p);
            RemoveParticle(p2);
            RemoveBuff(buff.SourceUnit, "ShacoCOTGSelf");
            Spell spell = buff.SourceUnit.SetSpell("HallucinateFull", 3, true);

            //Check if this is done on-script or should be handled automatically
            spell.SetCooldown(spell.GetCooldown() - buff.TimeElapsed);
            if (spell.GetCooldown() > 0)

            unit.Die(CreateDeathData(false, 0, unit, unit, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));
        }

        public void OnUpdate(float diff)
        {

        }

        public void OnGhostDeath(DeathData data)
        {
            AddBuff("ShacoExplode", 0.5f, 1, Buff.OriginSpell, Buff.TargetUnit, Buff.SourceUnit);
            Buff.DeactivateBuff();
        }
    }
}