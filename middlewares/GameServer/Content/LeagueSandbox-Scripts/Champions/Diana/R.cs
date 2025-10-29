using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Spells
{
    public class DianaTeleport : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
			owner.CancelAutoAttack(false);
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;     
            var ap = owner.Stats.AbilityPower.Total * 0.85f;
            var damage = 100 + 60* (spell.CastInfo.SpellLevel-1) + ap;
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var distt = dist - 125;
			var targetPos = GetPointFromUnit(owner,distt);
            var time = dist / 2200f;
			PlayAnimation(owner, "Spell4",time);
			AddBuff("Ghosted", time, 1, spell, owner, owner);
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
			CreateTimer((float) time , () =>
            {                           
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddBuff("Stun", 0.75f, 1, spell, Target, owner);
			AddParticleTarget(owner, Target, "Diana_Base_R_Tar.troy", Target, 10f);
                //AddParticleTarget(owner, owner, "Diana_Base_R_End.troy", owner, time);
                //AddParticleTarget(owner, owner, "Diana_Base_R_Teleport_Success.troy", owner, time);
			if (Target.HasBuff("DianaMoonlight"))
                {
                    spell.SetCooldown(0f, true);
                    Target.RemoveBuffsWithName("DianaMoonlight");
                    AddParticleTarget(owner, owner, ".troy", owner, time);
                    AddParticleTarget(owner, Target, "Diana_Base_R_Teleport_Success", owner, 10f);
                }
                else
                {
                    AddParticleTarget(owner, owner, "Diana_Base_R_End.troy", owner, 10f);
                }
            });
			FaceDirection(targetPos, owner, true);
            TeleportTo(owner, Target.Position.X, Target.Position.Y);
			AddParticle(owner, null, ".troy", owner.Position, lifetime: 10f);
			AddParticle(owner, null, ".troy", owner.Position, lifetime: 10f);
			AddParticle(owner, null, ".troy", owner.Position, lifetime: 10f);
			AddParticleTarget(owner, owner, "Diana_Base_R_Cas.troy", owner, time);
			AddParticleTarget(owner, owner, ".troy", owner, time); 			
        }
    }
}