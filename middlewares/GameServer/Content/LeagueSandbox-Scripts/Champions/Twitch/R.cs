using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using Buffs;

namespace Spells
{
    public class TwitchFullAutomatic : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true
        };
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
			owner.CancelAutoAttack(false, false);
        }
        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            owner.CancelAutoAttack(true);
            AddBuff("TwitchFullAutomatic", 6, 1, spell, owner, owner);
            AddBuff("AbilityUsed", 4, 1, spell, owner, owner);
        }
    }
}


