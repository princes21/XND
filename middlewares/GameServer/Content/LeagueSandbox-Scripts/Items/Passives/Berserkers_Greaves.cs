using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace ItemPassives
{
    public class ItemID_3006 : IItemScript
    {
        Spell spell;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
            if(owner.Stats.Range.Total >= 400)
            {
                StatsModifier.CriticalChance.FlatBonus = 0.35f;
            }
            owner.AddStatModifier(StatsModifier);
        }
    }
}