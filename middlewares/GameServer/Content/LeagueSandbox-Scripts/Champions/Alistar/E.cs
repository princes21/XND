using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeaguePackets.Game.Common.CastInfo;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class TriumphantRoar : ISpellScript
    {
        AttackableUnit Target;
        private ObjAIBase _owner;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
        }


        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var APRatio = owner.Stats.AbilityPower.Total * 0.4f;
            var heal = 60 + spell.CastInfo.SpellLevel * 40 + APRatio;
            var healGrievousWounds = heal * 0.5f;

            if (spell.CastInfo.Owner is Champion c)
            {
                var units = GetUnitsInRange(c.Position, 550f, true);
                {
                    for (int i = 0; i < units.Count; i++)
                    {
                        if (units[i].Team == c.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                        {
                            {
                                if (units[i].HasBuff("GrievousWounds"))
                                {
                                    units[i].TakeHeal(_owner, healGrievousWounds);
                                }
                                else
                                {
                                    units[i].TakeHeal(_owner, heal);
                                }
                                AddParticleTarget(owner, units[i], "Meditate_eff.troy", units[i]);
                            }
                        }
                    }
                }
            }
        }
    }
}
