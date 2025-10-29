using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace ItemPassives
{
    public class ItemID_3222 : IItemScript
    {
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
            StatsModifier.CooldownReduction.FlatBonus -= 0.20f;
            StatsModifier.MoveSpeed.PercentBonus += 0.10f;
            StatsModifier.MagicResist.FlatBonus += 25;
            StatsModifier.ManaRegeneration.PercentBonus += 0.50f;
            owner.AddStatModifier(StatsModifier);
        }
    }
}