using GameServerCore;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

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

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("AbilityUsed", 4.0f, 1, spell, owner, owner);
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X,
                                       spell.CastInfo.TargetPosition.Z);

            box = AddMinion((Champion)owner, "ShacoBox", "ShacoBox", spellPos,
                            owner.Team, aiPaused: true);
            AddParticle(owner, null, "JackintheboxPoof", spellPos);
            BecomeInvisible(box);

            hasTriggered = false;
            scanTimer = 0f;
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
            BecomeVisible(box);

            var fearDur = 0.5f + 0.25f * (box.Owner.Spells[1].CastInfo.SpellLevel - 1);
            AddBuff("Fear", fearDur, 1, box.Owner.Spells[1], target, box.Owner);

            box.PauseAI(false);
            box.SetTargetUnit(target);

            // auto-die after 5 s
            CreateTimer(5f, () =>
            {
                if (!box.IsDead)
                    box.TakeDamage(box.Owner, 1000f, DamageType.DAMAGE_TYPE_TRUE,
                                  DamageSource.DAMAGE_SOURCE_INTERNALRAW,
                                  DamageResultType.RESULT_NORMAL);
            });
        }
    }
}