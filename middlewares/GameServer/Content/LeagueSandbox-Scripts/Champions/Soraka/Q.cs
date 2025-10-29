using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using GameServerCore.Enums;

namespace Spells
{
    public class SorakaQ : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,
            MissileParameters = new MissileParameters { Type = MissileType.Circle }
        };

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var sector = spell.CreateSpellSector(new SectorParameters
            {
                BindObject = null,
                Length = 250f,
                SingleTick = true,
                Type = SectorType.Area,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectHeroes | SpellDataFlags.AffectMinions | SpellDataFlags.AffectNeutral,
                Lifetime = 0.3f,
                Tickrate = (int)1000f,
                CanHitSameTargetConsecutively = false,
                MaximumHits = 0
            });
            ApiEventManager.OnSpellSectorHit.AddListener(this, sector, OnSpellSectorHit, false);
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
        }

        public void OnSpellSectorHit(SpellSector sector, AttackableUnit target)
        {
            var spell = sector.SpellOrigin;

            var owner = spell.CastInfo.Owner;
            if (target.Team != owner.Team && target is ObjAIBase enemy)
            {
                float ApRatio = owner.Stats.AbilityPower.Total * 0.50f;
                var damage = 120 + ApRatio;

                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticle(owner, enemy, "Soraka_Base_Q_Tar.troy", enemy.Position);
                AddBuff("SorakaQSlow", 2.0f, 1, spell, enemy, owner);
                AddBuff("SorakaQSpeed", 2.5f, 1, spell, owner, owner);
                AddBuff("SorakaQEmpoweredW", 10f, 1, spell, owner, owner, true);

                if (enemy is Champion)
                {
                    float healAmount = 75f + 0.60f * owner.Stats.AbilityPower.Total;
                    owner.Stats.CurrentHealth += healAmount;
                    AddParticleTarget(owner, owner, "soraka_base_q_Lifesteal_eff.troy", owner, 1.0f);
                }
            }
        }
    }
}
