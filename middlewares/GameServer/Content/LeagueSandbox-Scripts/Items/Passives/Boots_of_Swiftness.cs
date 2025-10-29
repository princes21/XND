using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace ItemPassives
{
    public class ItemID_3009 : IItemScript
    {
        Spell spell;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
            //[Melee]-------------------------------------------
            if(owner.Stats.Range.Total <= 255)
            {
                StatsModifier.Armor.FlatBonus = 15f;
                StatsModifier.MagicResist.FlatBonus = 15f;
                StatsModifier.MoveSpeed.FlatBonus = 65f;
            }
            //[Ranged]------------------------------------------
            if(owner.Stats.Range.Total >= 400)
            {
                StatsModifier.MoveSpeed.FlatBonus = 30f;
                StatsModifier.AttackSpeed.FlatBonus = 0.50f;
            }
            owner.AddStatModifier(StatsModifier);
        }
    }
}