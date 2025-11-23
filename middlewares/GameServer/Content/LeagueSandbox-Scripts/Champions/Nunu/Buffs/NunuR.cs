using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class NunuR : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public StatsModifier StatsModifier { get; private set; }

        Champion Owner;
        float damage;
        float TimeSinceLastTick = 500f;
        Particle p;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner as Champion;
            var APratio = owner.Stats.AbilityPower.Total * (0.35f + 0.05f * (ownerSpell.CastInfo.SpellLevel - 1));
            damage = 275f + 12.5f * (ownerSpell.CastInfo.SpellLevel - 1) + APratio;
            Owner = owner;

            AddParticleTarget(owner, owner, "AbsoluteZero2_green_cas.troy", unit, buff.Duration, bone: "hip");

            AddParticleTarget(owner, owner, "AbsoluteZero3_cas.troy", unit, buff.Duration, bone: "hip");

        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            AddParticleTarget(Owner, Owner, "AbsoluteZero_nova.troy", unit, buff.Duration);
            AddParticleTarget(Owner, Owner, "AbsoluteZero_cas.troy", unit, buff.Duration);

            RemoveParticle(p);

            var owner = ownerSpell.CastInfo.Owner;

            var units = GetUnitsInRange(Owner.Position, 600f, true).OrderBy(unit => Vector2.DistanceSquared(unit.Position, unit.Position)).ToList(); 
            {
            var FinalExplosionDamage = 325f;
                for (int i = units.Count - 1; i >= 0; i--)
                    if (units[i].Team != Owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret) && units[i] is ObjAIBase)
                    {
                        units[i].TakeDamage(Owner, FinalExplosionDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    }


            }
        }

        public void OnUpdate(float diff)
        {
            TimeSinceLastTick += diff;
            if (TimeSinceLastTick >= 1000.0f)
            {
                var units = GetUnitsInRange(Owner.Position, 600f, true).OrderBy(unit => Vector2.DistanceSquared(unit.Position, unit.Position)).ToList();
                var tickDamage = damage;

                for (int i = units.Count - 1; i >= 0; i--)
                {
                    if (units[i].Team != Owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret) && units[i] is ObjAIBase)
                    {
                        var customTickDamage = tickDamage;
                        if (units[i] is Minion)
                        {
                            customTickDamage *= 0.75f;
                        }
                        units[i].TakeDamage(Owner, customTickDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    }
                }
                TimeSinceLastTick = 0;
            }
        }
    }
}