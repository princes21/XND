using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Spells
{
    public class DesperatePower : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {};
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("DesperatePower", 12.0f, 1, spell, owner, owner);
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
            AddParticleTarget(owner, owner, "Ryze_Dark_Crystal_DesperatePwr_Glow.troy", owner, 8f, 1f);
        }
    }
}
