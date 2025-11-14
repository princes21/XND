using System.Numerics;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Characters
{
    public class CharScriptShacoBox : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata();

        public void OnSpellPostCast(Spell spell)
        {
            if (spell.CastInfo.Owner != null)
            {
                var owner = spell.CastInfo.Owner;
                Vector2 spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
                Minion box = AddMinion(owner, "ShacoBox", "ShacoBox", spellPos, owner.Team);

                float attackRange = box.Stats.Range.Total;
                float damage = 35 + ((15 * (spell.CastInfo.SpellLevel - 1)) + owner.Stats.AbilityPower.Total * 0.2f);
                float attackSpeed = 0.56f;
                float duration = 5f;

                CreateTimer(0.1f, () =>
                {
                    var units = GetUnitsInRange(box.Position, attackRange, true);
                    foreach (var target in units)
                    {
                        if (target.Team != owner.Team && target is AttackableUnit au)
                        {
                            for (float t = 0; t < duration; t += attackSpeed)
                            {
                                float timerDelay = t;
                                CreateTimer(timerDelay, () =>
                                {
                                    if (!target.IsDead && !box.IsDead)
                                    {
                                        target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                                    }
                                });
                            }
                        }
                    }
                });

                CreateTimer(duration, () =>
                {
                    if (!box.IsDead)
                    {
                        box.TakeDamage(owner, 1000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
                    }
                });
            }
        }
    }
}
