using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace Spells
{
    public class MordekaiserSyphonOfDestruction : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            FaceDirection(spellPos, owner, false);
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
            RemoveBuff(owner, "MordekaiserChildrenOfTheGrave");
            RemoveBuff(owner, "Invisibility");
            RemoveBuff(owner, "Targetable");

            var sector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 600f,
                SingleTick = true,
                ConeAngle = 25f,
                Type = SectorType.Cone
            });
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            var APratio = owner.Stats.AbilityPower.Total * 0.6f;
            var damage = 70 + (spell.CastInfo.SpellLevel - 1) * 45 + APratio;
            AddParticleTarget(owner, target, "mordakaiser_siphonOfDestruction_tar_02.troy", target, 1f);
            AddParticleTarget(owner, target, "mordakaiser_siphonOfDestruction_tar.troy", target, 1f);
            AddParticleTarget(owner, owner, "mordakaiser_siphonOfDestruction_self.troy", owner, 1f);

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
        }
    }
}
