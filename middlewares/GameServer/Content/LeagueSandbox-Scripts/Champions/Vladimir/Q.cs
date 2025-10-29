using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class VladimirTransfusion : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        private ObjAIBase own;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            own = owner;
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var ap = owner.Stats.AbilityPower.Total * 0.6f;
            var damage = 90 * spell.CastInfo.SpellLevel + ap;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddParticleTarget(owner, target, "Vladimir_Base_Q_cas.troy", target);
            AddParticleTarget(owner, owner, "Vladimir_Base_Q_heal_mis.troy", owner);
            AddParticleTarget(owner, owner, "Vladimir_Base_Q_heal_tar.troy", owner);
            AddParticleTarget(owner, target, "VladTransfusion_cas.troy", target);
            AddParticleTarget(owner, owner, "VladTransfusionHeal_mis.troy", owner);
            AddParticleTarget(owner, target, "Vladimir_Base_Q_tar.troy", target);
            AddParticleTarget(owner, target, "VladTransfusion_tar.troy", target);
            OnSpellHit(spell, target);
        }

        public void OnSpellHit(Spell spell, AttackableUnit target)
        {
            var owner = spell.CastInfo.Owner as Champion;
            AddBuff("VladimirQHeal", 0.5f,1, spell,owner,owner);
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
        }
    }
}
