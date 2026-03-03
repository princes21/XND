using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class KatarinaE : ISpellScript
    {
        private AttackableUnit Target;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            PlayAnimation(owner, "Spell2");
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
            if (target.Team != owner.Team)
            {
                float AP = owner.Stats.AbilityPower.Total * 0.4f;
                float damage = 45f + 25 * spell.CastInfo.SpellLevel + AP;
                var MarkAP = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.15f;
                float MarkDamage = 15f * (owner.GetSpell("KatarinaQ").CastInfo.SpellLevel) + MarkAP;

                if (target.HasBuff("KatarinaQMark"))
                {
                    target.TakeDamage(owner, MarkDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PROC, false);
                }

                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticleTarget(owner, null, "katarina_shadowStep_tar.troy", Target);
            }

            if (target.HasBuff("KatarinaQMark"))
            {
                owner.GetSpell("KatarinaW").SetCooldown(0.00f);
                RemoveBuff(target, "KatarinaQMark");
            }
            AddParticleTarget(owner, null, "katarina_shadowStep_cas.troy", owner);

            TeleportTo(owner, Target.Position.X, Target.Position.Y);
            AddBuff("KatarinaEReduction", 1.5f, 1, spell, owner, owner);
            PlayAnimation(owner, "Spell3", 1f);
        }
    }
}