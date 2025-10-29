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
    public class SorakaE : ISpellScript
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
                Length = 300f,
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
                float ApRatio = owner.Stats.AbilityPower.Total * 0.70f;
                var damage = 100 + ApRatio;

                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticle(owner, enemy, "Soraka_Base_E_tar.troy", enemy.Position);
                AddBuff("Stun", 2.4f, 1, spell, enemy, owner);
                owner.GetSpell("SorakaQ").SetCooldown(0.00f);
            }
        }
    }
}
