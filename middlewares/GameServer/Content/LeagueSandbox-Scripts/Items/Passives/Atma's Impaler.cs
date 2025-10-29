using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace ItemPassives
{
    public class ItemID_3005 : IItemScript
    {
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
            StatsModifier.HealthPoints.FlatBonus = 400f;
            StatsModifier.Armor.FlatBonus = 5f;
            StatsModifier.MagicResist.FlatBonus = 25f;
            if (owner.Stats.HealthPoints.Total >= 4850f && !owner.HasBuff("AtmasImpalerPermanent"))
            {
                StatsModifier.AttackDamage.FlatBonus = owner.Stats.HealthPoints.Total * 0.02f;
                AddBuff("AtmasImpalerPermanent", 10.0f, 1, null, owner, owner, true);
            }
            owner.AddStatModifier(StatsModifier);
        }
    }
}