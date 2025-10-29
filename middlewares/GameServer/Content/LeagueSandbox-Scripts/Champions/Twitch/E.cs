using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Enums;

namespace Spells
{
    public class TwitchExpunge : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellCast(Spell spell)
        {
            var caster = spell.CastInfo.Owner as Champion;
            if (caster == null) return;

            float range = 1200f;
            var units = GetUnitsInRange(caster.Position, range, true);

            RemoveBuff(caster, "Invisibility");
            RemoveBuff(caster, "Targetable");
            AddBuff("AbilityUsed", 4f, 1, spell, caster, caster);

            foreach (var target in units)
            {
                if (target.Team != caster.Team && target is ObjAIBase enemy)
                {
                    float baseDamage = 130f;
                    float apRatio = caster.Stats.AbilityPower.Total * 0.90f;
                    float totalDamage = baseDamage + apRatio;

                    enemy.TakeDamage(caster, totalDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    AddBuff("Root", 0.75f, 1, spell, enemy, caster);
                    AddBuff("TwitchPoison", 3.50f, 1, spell, enemy, caster);
                    AddParticleTarget(caster, target, "Twitch_Base_E_tar", target);
                }
            }
        }
    }
}
