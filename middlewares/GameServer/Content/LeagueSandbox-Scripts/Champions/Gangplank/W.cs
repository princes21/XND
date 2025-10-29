using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class RemoveScurvy : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {};
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            float AD = owner.Stats.AttackDamage.Total * 0.05f;
            float AP = owner.Stats.AbilityPower.Total * 1.65f;
            target.Stats.CurrentHealth += 35 + AD + AP;
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
            AddBuff("GangplankW", 1.5f, 1, spell, owner, owner);
        }
    }
}
