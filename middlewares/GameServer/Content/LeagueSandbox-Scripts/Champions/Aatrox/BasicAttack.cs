using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
	public class AatroxBasicAttack2 : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
			TriggersSpellCasts = true,
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
            Target = target;
        }
        public void OnLaunchAttack(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
			if (owner.HasBuff("AatroxWONHLifeBuff"))
			{
                if (owner.HasBuff("GreviousWound"))
                {
                    float ArMr = owner.Stats.Armor.Total * 0.40f + owner.Stats.MagicResist.Total * 0.40f;
                    owner.Stats.CurrentHealth += ArMr;
                    owner.Stats.CurrentMana += owner.Stats.HealthPoints.Total * 0.02f;
                }
                else
                {
                    float ArMr = owner.Stats.Armor.Total * 0.85f + owner.Stats.MagicResist.Total * 0.90f;
                    owner.Stats.CurrentHealth += ArMr;
                    owner.Stats.CurrentMana += owner.Stats.HealthPoints.Total * 0.15f;
                }

                AddParticleTarget(owner, owner, "Global_Heal.troy", owner);
                AddParticleTarget(owner, owner, "Aatrox_Base_W_Buff_Life_sound.troy", owner);
                AddParticleTarget(owner, owner, "Aatrox_Base_W_Life_Self.troy", owner);
                AddParticle(owner, Target, "Aatrox_Base_W_Active_Hit.troy", Target.Position, 6f);
                AddParticleTarget(owner, Target, "Aatrox_Base_W_Active_Hit_Life.troy", Target, 6f);
                AddParticleTarget(owner, Target, "Aatrox_Base_W_Life__Passive_Hit.troy", Target, 6f);
                AddParticleTarget(owner, Target, "Aatrox_Base_W_Life_Passive_Hit.troy", Target, 6f);
                AddParticleTarget(owner, Target, "Aatrox_Base_W_hit_impact_tar_bloodless.troy", Target, 6f);
			}
            
			if (owner.HasBuff("AatroxWONHPowerBuff"))
            {
                var ArMrratio = owner.Stats.Armor.Total * 0.80f + owner.Stats.MagicResist.Total * 0.85f;
                var BloodDamage = owner.Stats.CurrentMana * 1.25f;
                var Damage = 40 + ArMrratio + BloodDamage;
                Target.TakeDamage(owner, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                owner.Stats.CurrentMana -= owner.Stats.CurrentMana * 0.25f;
                //AddParticleTarget(owner, owner, "Global_Heal.troy", owner);
                AddParticleTarget(owner, owner, "Aatrox_Base_W_Buff_Power_sound.troy", owner);
                AddParticleTarget(Target, owner, "Aatrox_Base_W_Power_Passive_Hit.troy", Target);
                AddParticle(owner, Target, "Aatrox_Base_W_Active_Hit.troy", Target.Position, 6f);
                AddParticleTarget(owner, Target, "Aatrox_Base_W_Active_Hit_Power.troy", Target, 6f);
                AddParticleTarget(owner, Target, "Aatrox_Base_W_hit_impact_tar_bloodless.troy", Target, 6f);
            }
        }
    }
}

