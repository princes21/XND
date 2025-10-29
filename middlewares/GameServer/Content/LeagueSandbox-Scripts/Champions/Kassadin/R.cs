using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Spells
{
    public class RiftWalk : ISpellScript
    {
        Buff Buff;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var trueCoords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var startPos = owner.Position;

            var to = trueCoords - startPos;
            if (to.Length() > 700f)
            {
                trueCoords = GetPointFromUnit(owner, 475f);
            }
            PlayAnimation(owner, "Spell3", 0, 0, 1);
            AddBuff("RiftWalk", 20.0f, 1, spell, owner, owner);
            AddBuff("RiftWalkArMr", 1.0f, 1, spell, owner, owner);
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
            TeleportTo(owner, trueCoords.X, trueCoords.Y);
            AddParticle(owner, null, "Kassadin_Base_R_appear.troy", owner.Position);

            var AOEdmg = spell.CreateSpellSector(new SectorParameters
            {
                Length = 250f,
                SingleTick = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
            if (owner.HasBuff("RabadonsDeathcapEffect"))
            {
                var MaxAP = 3000f;
                var MaxHealth = 3250f;
                if (owner.Stats.AbilityPower.Total <= MaxAP)
                {
                    if (owner.Stats.HealthPoints.Total <= MaxHealth)
                    {

                        StatsModifier.AbilityPower.FlatBonus = 14f;
                        owner.AddStatModifier(StatsModifier);
                    }
                    else
                    {
                        owner.Stats.HealthPoints.BaseValue -= 100f;
                    }
                }
            }
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var RiftWalk_Stacks = spell.CastInfo.Owner.GetBuffWithName("RiftWalk");
            float TotalStacks = RiftWalk_Stacks != null ? RiftWalk_Stacks.StackCount : 0;
            float ApScale = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.45f;
            float StrongApScale = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.70f;
            if (TotalStacks == 0)
            {
                float damage = 60f + ApScale;
                target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            }
            if (TotalStacks == 1)
            {
                float damage = 135f + ApScale;
                target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            }
            if (TotalStacks == 2)
            {
                float damage = 235f + ApScale;
                target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            }
            if (TotalStacks == 3)
            {
                float damage = 350 + ApScale;
                target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            }
            if (TotalStacks == 4)
            {
                float damage = 500 + StrongApScale;
                target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            }

        }
    }
}