using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System;

namespace Spells
{
    public class VeigarBalefulStrike : ISpellScript
    {
        int ticks;
        ObjAIBase Owner;
        StatsModifier statsModifier = new StatsModifier();
        Spell Spell;
        float stacks;
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            Owner = owner;
        }

        public void OnSpellCast(ObjAIBase owner, Spell spell)
        {
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            if (target == null || spell == null || spell.CastInfo.Owner == null)
            {
                return; // Exit early if target or owner is null
            }

            var owner = spell.CastInfo.Owner;
            var APratio = owner.Stats.AbilityPower.Total * 0.6f;
            var damage = 80f + ((spell.CastInfo.SpellLevel - 1) * 45) + APratio;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            if (target.IsDead)
            {
                if (target is Champion)
                {
                    statsModifier.AbilityPower.FlatBonus = 150f;
                    owner.AddStatModifier(statsModifier);
                }
                else
                {
                    statsModifier.AbilityPower.FlatBonus = 10f;
                    owner.AddStatModifier(statsModifier);
                }
            }
        }
        public void OnUpdate(float diff)
        {
            Owner.Stats.ManaRegeneration.FlatBonus = Owner.Stats.ManaRegeneration.BaseValue * ((100 / Owner.Stats.ManaPoints.Total) * ((Owner.Stats.ManaPoints.Total - Owner.Stats.CurrentMana) / 100)); //I'm too lazy to make this properly
        }
    }
}
