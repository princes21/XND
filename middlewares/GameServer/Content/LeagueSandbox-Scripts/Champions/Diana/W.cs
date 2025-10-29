using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Spells
{
    public class DianaOrbs : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
            AddBuff("DianaExtendedAADamage", 4f, 1, spell, owner, owner);
            AddBuff("DianaMixedShield", 4f, 1, spell, owner, owner);
            owner.GetSpell("DianaVortex").LowerCooldown(99.0f);
            owner.GetSpell("DianaTeleport").LowerCooldown(99.0f);
        }

    }
}
