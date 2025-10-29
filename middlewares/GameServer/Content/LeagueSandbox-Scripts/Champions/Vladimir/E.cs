using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Spells
{
    public class VladimirTidesofBlood : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        private ObjAIBase own;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            own = owner;
        }

        public void OnSpellPreCast(Spell spell, AttackableUnit unit, Vector2 start, Vector2 end)
        {
            var owner = spell.CastInfo.Owner;
            owner.Stats.CurrentHealth = owner.Stats.CurrentHealth - (30f + spell.CastInfo.SpellLevel);
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

            PlayAnimation(owner, "Spell3", 1.23f);
            AddBuff("TidesofBloodBuff", 1f, 1, spell, owner, owner);
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
        }

        public void OnSpellPostCast(Spell spell)
        {
            spell.SetCooldown(spell.GetCooldown());
        }
    }
}