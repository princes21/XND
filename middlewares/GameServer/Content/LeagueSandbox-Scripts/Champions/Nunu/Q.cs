using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
namespace Spells
{
    public class Consume : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };
        public StatsModifier StatsModifier { get; private set; }

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            AddBuff("AbilityUsed", 4.0f, 1, spell, owner, owner);
            AddBuff("Invulnerable", 2.0f, 1, spell, owner, owner);
            AddBuff("Targetable", 2.0f, 1, spell, owner, owner);
            owner.GetSpell("IceBlast").LowerCooldown(99.0f);
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            float damage = 250 + 150 * spell.CastInfo.SpellLevel;
            if (owner.HasBuff("GreviousWound"))
            {
                float heal = 30 + owner.Stats.HealthPoints.Total * 0.05f + owner.Stats.AbilityPower.Total * 0.20f;
                owner.Stats.CurrentHealth += heal;
            }
            else
            {
                float heal = 65 + owner.Stats.HealthPoints.Total * 0.15f + owner.Stats.AbilityPower.Total * 0.60f;
                owner.Stats.CurrentHealth += heal;
            }
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddParticleTarget(owner, Target, "yeti_Consume_tar.troy", Target, 1f, 1f);
        }
    }
}

