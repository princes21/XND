using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;

namespace Spells
{
    public class SionR : ISpellScript
    {
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        Spell thisSpell;

        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnKillUnit.AddListener(this, owner, OnKillMinion, false);
            thisSpell = spell;
        }

        public void OnKillMinion(DeathData deathData)
        {
            StatsModifier.AbilityPower.FlatBonus = 8f;
            deathData.Killer.AddStatModifier(StatsModifier);
        }
    }
}

