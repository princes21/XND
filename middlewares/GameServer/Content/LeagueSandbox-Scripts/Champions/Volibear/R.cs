using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Spells
{
    public class VolibearR : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
			if (owner.HasBuff("VolibearQ"))
            {
				OverrideAnimation(owner, "spell1_spell4", "spell4");
			}
			else
			{
				OverrideAnimation(owner, "spell4", "spell1_spell4");
			}
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            //AddParticleTarget(owner, owner, pcastname, owner);
        }

        public void OnSpellPostCast(Spell spell)
        {
            if (spell.CastInfo.Owner is Champion c)
            {
				AddBuff("VolibearR", 12.0f, 1, spell, c, c);
                AddBuff("AbilityUsed", 4f, 1, spell, c, c);
				AddParticleTarget(c, c, "Volibear_R_cas", c);
				AddParticleTarget(c, c, "Volibear_R_cas_02", c);
				AddParticleTarget(c, c, "Volibear_R_cas_03", c);
				AddParticleTarget(c, c, "Volibear_R_cas_04", c);
				AddParticleTarget(c, c, "volibear_R_lightning_arms", c);
                var damage = 75 + (40 * (spell.CastInfo.SpellLevel - 1)) + (c.Stats.AbilityPower.Total * 0.3f);

                var units = GetUnitsInRange(c.Position, 450f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {
                        units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false); 
                        AddParticleTarget(c, units[i], "Volibear_R_tar", units[i]);
                        AddParticleTarget(c, units[i], "Volibear_R_tar_02", units[i]);								
                    }
                }

                //AddBuff("AatroxR", 12f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner); test
            }
        }
    }
}
