using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;


namespace Spells
{
    public class MordekaiserCreepingDeathCast : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("MordekaiserCreepingDeath", 6f, 1, spell, target, owner);
            AddBuff("MordekaiserMixedShield", 3.5f, 1, spell, owner, owner);
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
            RemoveBuff(owner, "MordekaiserChildrenOfTheGrave");
            RemoveBuff(owner, "Invisibility");
            RemoveBuff(owner, "Targetable");
        }
    }
}
