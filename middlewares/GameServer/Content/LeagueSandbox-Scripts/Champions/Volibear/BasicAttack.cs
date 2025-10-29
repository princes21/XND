using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;


namespace Spells
{
    public class VolibearBasicAttack : ISpellScript
    {
		private AttackableUnit Target = null;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
			Target = target;
			if (owner.HasBuff("VolibearQ"))
            {
				OverrideAnimation(owner, "spell1_attack", "Attack1");
			}
			else
			{
				OverrideAnimation(owner, "Attack1", "spell1_attack");
			}
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
			var spellLevel = owner.GetSpell("VolibearQ").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.3f;
            var damage =(30 * spellLevel) + ADratio;
			if (owner.HasBuff("VolibearQ"))
            {
			    Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
				AddParticleTarget(owner, Target, "Volibear_Q_tar", Target, 10f,1,"");
				if (Target.Team != owner.Team && !(Target is ObjBuilding || Target is BaseTurret))
				{
				ForceMovement(Target, "RUN", GetPointFromUnit(owner, -125f), 400f, 0, 25f, 0);
				}
			}
			else
			{
			}
			//spell.CastInfo.Owner.SetAutoAttackSpell("TalonBasicAttack2", false);
        }
    }

    public class VolibearBasicAttack2 : ISpellScript
    {
		private AttackableUnit Target = null;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
			Target = target;
			if (owner.HasBuff("VolibearQ"))
            {
				OverrideAnimation(owner, "spell1_attack", "Attack2");
			}
			else
			{
				OverrideAnimation(owner, "Attack2", "spell1_attack");
			}
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
			var spellLevel = owner.GetSpell("VolibearQ").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.3f;
            var damage =(30 * spellLevel) + ADratio;
			if (owner.HasBuff("VolibearQ"))
            {
			    Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
				AddParticleTarget(owner, Target, "Volibear_Q_tar", Target, 10f,1,"");
			    if (Target.Team != owner.Team && !(Target is ObjBuilding || Target is BaseTurret))
				{
				ForceMovement(Target, "RUN", GetPointFromUnit(owner, -125f), 400f, 0, 25f, 0);
				}
			}
			else
			{
			}
        }
    }
	public class VolibearCritAttack : ISpellScript
    {
		private AttackableUnit Target = null;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
			Target = target;
			if (owner.HasBuff("VolibearQ"))
            {
				OverrideAnimation(owner, "spell1_attack", "Crit");
			}
			else
			{
				OverrideAnimation(owner, "Crit", "spell1_attack");
			}
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
			var spellLevel = owner.GetSpell("VolibearQ").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.3f;
            var damage =(30 * spellLevel) + ADratio;
			var damager =damage * 2f;
			if (owner.HasBuff("VolibearQ"))
            {
			    Target.TakeDamage(owner, damager, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
				AddParticleTarget(owner, Target, "Volibear_Q_tar", Target, 10f,1,"");
			    if (Target.Team != owner.Team && !(Target is ObjBuilding || Target is BaseTurret))
				{
				ForceMovement(Target, "RUN", GetPointFromUnit(owner, -125f), 400f, 0, 25f, 0);
				}
			}
			else
			{
			}
        }
    }
}
