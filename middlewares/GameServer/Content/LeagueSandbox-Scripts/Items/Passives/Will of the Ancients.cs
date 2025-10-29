using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace ItemPassives
{
    public class ItemID_3152 : IItemScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
            StatsModifier.CooldownReduction.FlatBonus -= 0.10f;
            StatsModifier.SpellVamp.FlatBonus = 0.20f;
            StatsModifier.MoveSpeed.PercentBonus = 0.10f;
            owner.AddStatModifier(StatsModifier);
        }
    }
}