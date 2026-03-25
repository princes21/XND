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

        // Per-box state container (allows multiple boxes to work independently)
        private class BoxState
        {
            public Minion Box;
            public AttackableUnit CurrentTarget;
            public bool BoxActive;
            public float ScanTimer;
            public Buff InvisBuff;
        }

        private readonly List<BoxState> allBoxes = new List<BoxState>();

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("AbilityUsed", 4.0f, 1, spell, owner, owner);
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X,
                                       spell.CastInfo.TargetPosition.Z);

            var newBox = AddMinion((Champion)owner, "ShacoBox", "ShacoBox", spellPos,
                                   owner.Team, aiPaused: true);
            AddParticle(owner, null, "JackintheboxPoof", spellPos);

            var state = new BoxState
            {
                Box = newBox,
                InvisBuff = AddBuff("Invisibility", float.MaxValue, 1, spell, newBox, owner),
                BoxActive = false,
                ScanTimer = 0f,
                CurrentTarget = null
            };

            allBoxes.Add(state);

            CreateTimer(300f, () =>
            {
                if (!state.Box.IsDead && !state.BoxActive)
                {
                    state.Box.TakeDamage(state.Box.Owner, 1000f, DamageType.DAMAGE_TYPE_TRUE,
                                         DamageSource.DAMAGE_SOURCE_INTERNALRAW,
                                         DamageResultType.RESULT_NORMAL);
                }
                allBoxes.Remove(state);
            });
        }

        public void OnUpdate(float diff)
        {
            foreach (var state in allBoxes.ToList())
            {
                if (state.Box == null || state.Box.IsDead)
                {
                    allBoxes.Remove(state);
                    continue;
                }

                state.ScanTimer += diff;
                if (state.ScanTimer < 250f) continue;
                state.ScanTimer = 0f;

                UpdateBox(state);
            }
        }

        private void UpdateBox(BoxState state)
        {
            if (!state.BoxActive)
            {
                var champions = GetNearbyChampions(state);
                if (champions.Count > 0)
                {
                    state.BoxActive = true;
                    state.CurrentTarget = champions[0];
                    TriggerBox(state, state.CurrentTarget);
                }
                return;
            }

            if (state.CurrentTarget == null || state.CurrentTarget.IsDead)
            {
                var champions = GetNearbyChampions(state);
                if (champions.Count > 0)
                {
                    state.CurrentTarget = champions[0];
                }
                else
                {
                    var minions = GetNearbyMinions(state);
                    state.CurrentTarget = minions.Count > 0 ? minions[0] : null;
                }

                if (state.CurrentTarget != null)
                    TriggerBox(state, state.CurrentTarget);
            }
            else
            {
                float dist = Vector2.Distance(state.Box.Position, state.CurrentTarget.Position);
                if (dist > 400f)
                {
                    state.Box.SetTargetUnit(null);
                    state.Box.CancelAutoAttack(false, true);
                    state.CurrentTarget = null;
                }
                else
                {
                    if (state.CurrentTarget is LaneMinion)
                    {
                        var champions = GetNearbyChampions(state);
                        if (champions.Count > 0)
                            state.CurrentTarget = champions[0];
                    }
                    TriggerBox(state, state.CurrentTarget);
                }
            }
        }

        private List<Champion> GetNearbyChampions(BoxState state)
        {
            return GetUnitsInRange(state.Box.Position, 400f, true)
                .OfType<Champion>()
                .Where(c => c.Team != state.Box.Team && !c.IsDead)
                .ToList();
        }

        private List<LaneMinion> GetNearbyMinions(BoxState state)
        {
            return GetUnitsInRange(state.Box.Position, 400f, true)
                .OfType<LaneMinion>()
                .Where(m => m.Team != state.Box.Team && !m.IsDead)
                .ToList();
        }

        private void TriggerBox(BoxState state, AttackableUnit target)
        {
            if (state.InvisBuff != null && !state.InvisBuff.Elapsed())
            {
                state.InvisBuff.DeactivateBuff();
                state.Box.SetStatus(StatusFlags.Targetable, true);

                var fearDur = 0.5f + 0.25f * (state.Box.Owner.Spells[1].CastInfo.SpellLevel - 1);
                AddBuff("Fear", fearDur, 1, state.Box.Owner.Spells[1], target, state.Box.Owner);

                CreateTimer(8f, () =>
                {
                    if (!state.Box.IsDead)
                        state.Box.TakeDamage(state.Box.Owner, 1000f, DamageType.DAMAGE_TYPE_TRUE,
                                             DamageSource.DAMAGE_SOURCE_INTERNALRAW,
                                             DamageResultType.RESULT_NORMAL);
                    allBoxes.Remove(state);
                });
            }

            state.Box.PauseAI(false);
            state.Box.SetTargetUnit(target);
        }
    }
}