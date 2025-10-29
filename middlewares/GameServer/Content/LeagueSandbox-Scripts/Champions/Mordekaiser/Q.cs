using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class MordekaiserMaceOfSpades : ISpellScript
    {
        ObjAIBase Owner;
        Buff Buff;
        Spell Spell;
        int i;
        string particles = "mordakaiser_maceOfSpades_tar.troy";
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = false,
            NotSingleTargetSpell = true
            // TODO
        };
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Owner = owner;
            Spell = spell;
            spell.CastInfo.Owner.CancelAutoAttack(true);
            ApiEventManager.OnHitUnit.AddListener(this, spell.CastInfo.Owner, TargetExecute, true);
            Buff = AddBuff("MordekaiserMaceOfSpades", 10.0f, 1, spell, owner, owner);
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
            RemoveBuff(owner, "MordekaiserChildrenOfTheGrave");
            RemoveBuff(owner, "Invisibility");
            RemoveBuff(owner, "Targetable");
        }
        public void TargetExecute(DamageData damageData)
        {
            if (Owner.HasBuff("MordekaiserMaceOfSpades"))
            {
                var ADratio = Owner.Stats.AttackDamage.FlatBonus;
                var APratio = Owner.Stats.AbilityPower.Total * 0.4f;
                var damage = 80f + (30 * (Spell.CastInfo.SpellLevel - 1)) + ADratio + APratio;
                bool isCrit = false;

                AddParticleTarget(Owner, Owner, "mordakaiser_siphonOfDestruction_self.troy", Owner, 1f);
                
                var units = GetUnitsInRange(Owner.Position, 300f, true);
                for (i = units.Count - 1; i >= 0; i--)
                {
                    if (units[i].Team == Owner.Team || units[i] is BaseTurret || units[i] is Nexus || units[i] is ObjBuilding || units[i] is LaneTurret)
                    {
                        units.RemoveAt(i);
                    }
                }
                for (i = 0; i < units.Count; i++)
                {
                    if ((units.Count) == 1)
                    {
                        damage *= 1.65f;
                        isCrit = true;
                        particles = "mordakaiser_maceOfSpades_tar2.troy";
                    }
                    units[i].TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, isCrit);
                    AddParticleTarget(Owner, Owner, particles, units[i], 1f);
                }
                RemoveBuff(Buff);
            }
        }
    }
}