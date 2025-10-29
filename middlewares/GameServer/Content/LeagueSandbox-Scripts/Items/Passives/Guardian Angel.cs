using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using Buffs;

namespace ItemPassives
{
    public class ItemID_3026 : IItemScript
    {
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private ObjAIBase owner;
        private Spell spell;

        public void OnActivate(ObjAIBase owner)
        {
            this.owner = owner;
            this.spell = null;

            StatsModifier.AttackDamage.FlatBonus = 45f;
            StatsModifier.Armor.FlatBonus = 20f;
            StatsModifier.MagicResist.FlatBonus = 25f;
            owner.AddStatModifier(StatsModifier);
        }
        public void OnUpdate(float diff)
        {
            if (!owner.HasBuff("GuardianAngelCooldown"))
            {
                if (owner.Stats.CurrentHealth <= owner.Stats.HealthPoints.Total * 0.15f)
                {
                    if (owner.HasBuff("GreviousWound"))
                    {
                        owner.Stats.CurrentHealth = owner.Stats.HealthPoints.Total * 0.50f;
                    }
                    else
                    {
                        owner.Stats.CurrentHealth = owner.Stats.HealthPoints.Total;
                    }
                    
                    AddBuff("Invulnerable", 2.0f, 1, null, owner, owner);
                    AddBuff("GuardianAngelCooldown", 60.0f, 1, null, owner, owner);
                }
            }
        }
    }
}
