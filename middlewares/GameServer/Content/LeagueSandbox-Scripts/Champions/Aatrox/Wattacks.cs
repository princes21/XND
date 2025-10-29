using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Spells
{
    public class AatroxWONHAttackLife : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };
        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

            owner.SetSpell("AatroxW2", 1, true);
            spell.CastInfo.Owner.GetSpell("AatroxW2").SetCooldown(1f);
        }
    }
    public class AatroxWONHAttackPower : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

            owner.SetSpell("AatroxW", 1, true);
            spell.CastInfo.Owner.GetSpell("AatroxW").SetCooldown(1f);
        }
    }
}

