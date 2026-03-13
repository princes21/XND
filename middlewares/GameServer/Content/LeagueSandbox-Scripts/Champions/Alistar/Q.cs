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
    public class Pulverize : ISpellScript
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
            var PositionInFrontOfAlistar = new Vector2(owner.Position.X + 150f, owner.Position.Y);
            var APRatio = owner.Stats.AbilityPower.Total * 1f;
            var HealthRatio = owner.Stats.HealthPoints.Total * 0.05f;
            var damage = 60 + spell.CastInfo.SpellLevel * 40 + APRatio + HealthRatio;
            AddParticle(owner, null, "alistar_trample_01.troy", PositionInFrontOfAlistar);
            AddParticleTarget(owner, owner, "alistar_nose_puffs.troy", owner, bone: "BUFFBONE_CSTM_NOSE1");
            AddParticleTarget(owner, owner, "alistar_trample_head.troy", owner, bone: "head");
            AddParticleTarget(owner, owner, "alistar_nose_puffs.troy", owner, bone: "BUFFBONE_CSTM_NOSE2");
            AddParticleTarget(owner, owner, "alistar_trample_hand.troy", owner, bone: "L_hand");
            AddParticleTarget(owner, owner, "alistar_trample_hand.troy", owner, bone: "R_hand");

            if (spell.CastInfo.Owner is Champion c)
            {
                var units = GetUnitsInRange(c.Position, 260f, true);
                {
                    for (int i = 0; i < units.Count; i++)
                    {
                        if (units[i].Team != c.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                        {
                            {
                                units[i].TakeDamage(_owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                                ForceMovement(units[i], "RUN", new Vector2(units[i].Position.X + 10f, units[i].Position.Y + 10f), 13f, 0, 16.5f, 0);
                            }
                        }
                    }
                }
            }
        }
    }
}
