using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace ItemPassives
{
    public class ItemID_3156 : IItemScript
    {
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private ObjAIBase owner;
        private Spell spell;

        public void OnActivate(ObjAIBase owner)
        {
            this.owner = owner;
            this.spell = null;

            StatsModifier.Armor.FlatBonus = 10f;
            StatsModifier.MagicResist.FlatBonus = 10f;
            owner.AddStatModifier(StatsModifier);
        }
        public void OnUpdate(float diff)
        {
            if (!owner.HasBuff("MawofMalmortiusCooldown"))
            {
                if (owner.Stats.CurrentHealth <= owner.Stats.HealthPoints.Total * 0.25f && owner.Stats.AbilityPower.Total <= 200)
                {
                    AddBuff("MawofMalmortiusBuff", 3.5f, 1, null, owner, owner);
                    AddBuff("MawofMalmortiusCooldown", 60.0f, 1, null, owner, owner);
                }
            }
        }
    }
}
