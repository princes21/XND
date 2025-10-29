using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class BloodBoil : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
        }

        public void OnSpellPostCast(Spell spell)
        {
            if (spell.CastInfo.Targets[0].Unit.Team != spell.CastInfo.Owner.Team)
            {
                //spell.AddProjectileTarget("LuluWTwo", spell.CastInfo.SpellCastLaunchPosition, spell.CastInfo.Targets[0].Unit);
            }
            else
            {
                AddBuff("BloodBoilEnhancement", 4.0f, 1, spell, spell.CastInfo.Targets[0].Unit, spell.CastInfo.Owner);
            }
        }
    }
}
