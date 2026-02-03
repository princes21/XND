using GameServerCore;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class JackInTheBox : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };
        private Minion box;
        private bool hasTriggered = false;
        private float scanTimer = 0f;
        private Buff invisBuff;

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("AbilityUsed", 4.0f, 1, spell, owner, owner);
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X,
                                       spell.CastInfo.TargetPosition.Z);
            box = AddMinion((Champion)owner, "ShacoBox", "ShacoBox", spellPos,
                            owner.Team, aiPaused: true);
            AddParticle(owner, null, "JackintheboxPoof", spellPos);
            
            // Add permanent invisibility buff
            invisBuff = AddBuff("Invisibility", float.MaxValue, 1, spell, box, owner);

            hasTriggered = false;
            scanTimer = 0f;

            // Create timer to kill box after 300s if it never triggers
            CreateTimer(300f, () =>
            {
                if (!box.IsDead && !hasTriggered)
                {
                    box.TakeDamage(box.Owner, 1000f, DamageType.DAMAGE_TYPE_TRUE,
                                  DamageSource.DAMAGE_SOURCE_INTERNALRAW,
                                  DamageResultType.RESULT_NORMAL);
                }
            });
        }

        public void OnUpdate(float diff)
        {
            if (box == null || box.IsDead || hasTriggered) return;
            scanTimer += diff;
            if (scanTimer < 250f) return;          // 250 ms tick
            scanTimer = 0f;
            var enemies = GetUnitsInRange(box.Position, 300f, true)
                .FindAll(u => u.Team != box.Team &&
                              u is AttackableUnit &&
                              !(u is BaseTurret) &&
                              !(u is ObjAnimatedBuilding));
            if (enemies.Count > 0)
            {
                hasTriggered = true;
                TriggerBox(enemies[0]);
            }
        }

        private void TriggerBox(AttackableUnit target)
        {
            // Remove invisibility buff instead of using BecomeVisible
            if (invisBuff != null && !invisBuff.Elapsed())
            {
                invisBuff.DeactivateBuff();
            }

            box.SetStatus(StatusFlags.Targetable, true);
            var fearDur = 0.5f + 0.25f * (box.Owner.Spells[1].CastInfo.SpellLevel - 1);
            AddBuff("Fear", fearDur, 1, box.Owner.Spells[1], target, box.Owner);
            box.PauseAI(false);
            box.SetTargetUnit(target);

            // Auto-die after X seconds once triggered (adjust this value as needed)
            CreateTimer(5f, () =>  // Change 5f to however long you want it active after triggering
            {
                if (!box.IsDead)
                    box.TakeDamage(box.Owner, 1000f, DamageType.DAMAGE_TYPE_TRUE,
                                  DamageSource.DAMAGE_SOURCE_INTERNALRAW,
                                  DamageResultType.RESULT_NORMAL);
            });
        }
    }
}