using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using Buffs;

namespace Spells
{
    public class SorakaW : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var Target = spell.CastInfo.Targets[0].Unit;
            if (Target.Team != owner.Team)
            {
                //spell.AddProjectileTarget("LuluWTwo", spell.CastInfo.SpellCastLaunchPosition, spell.CastInfo.Targets[0].Unit);
            }
            else
            {
                if (owner.HasBuff("SorakaQEmpoweredW"))
                {
                    if (owner.HasBuff("GreviousWound"))
                    {
                        Target.Stats.CurrentHealth += 150 + owner.Stats.AbilityPower.Total * 0.40f;
                        owner.Stats.CurrentHealth += 150 + owner.Stats.AbilityPower.Total * 0.40f;
                    }
                    else
                    {
                        Target.Stats.CurrentHealth += 400 + owner.Stats.AbilityPower.Total * 0.70f;
                        owner.Stats.CurrentHealth += 400 + owner.Stats.AbilityPower.Total * 0.70f;
                    }
                    AddParticleTarget(owner, Target, "soraka_base_q_Lifesteal_eff.troy", owner, 1.0f);
                    RemoveBuff(owner, "SorakaQEmpoweredW");
                }
                else
                {
                    if (owner.HasBuff("GreviousWound"))
                    {
                        Target.Stats.CurrentHealth += 100 + owner.Stats.AbilityPower.Total * 0.20f;
                    }
                    else
                    {
                        Target.Stats.CurrentHealth += 200 + owner.Stats.AbilityPower.Total * 0.45f;
                    }
                    AddParticleTarget(owner, Target, "soraka_base_q_Lifesteal_eff.troy", owner, 1.0f);
                }
            }
        }

    }
}
