using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3041 : IItemScript
    {

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(ObjAIBase owner)
        {
            StatsModifier.AbilityPower.FlatBonus = 60f;
            StatsModifier.ManaPoints.FlatBonus = 850f;
            owner.AddStatModifier(StatsModifier);
            owner.Stats.CurrentMana += 850f;
            AddBuff("MejaisSoulstealerEffect", 10.0f, 1, null, owner, owner, true);
        }

        public void OnDeactivate(ObjAIBase owner)
        {
            RemoveBuff(owner, "MejaisSoulstealerEffect");
        }
    }
}