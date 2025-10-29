using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3089 : IItemScript
    {

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            AddBuff("RabadonsDeathcapEffect", 10.0f, 1, null, owner, owner, true);
        }
        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            float MaxAP = 3000f;
            float MaxHealth = 3250;
                
            if (owner.Stats.AbilityPower.Total <= MaxAP && owner.HasBuff("AbilityUsed") && owner.Stats.Range.Total >= 250f)
            {
                if(owner.Stats.HealthPoints.Total <= MaxHealth)
                {
                    if (owner.HasBuff("MejaisSoulstealerEffect"))
                    {
                        StatsModifier.AbilityPower.FlatBonus = 28f;
                    }
                    else
                    {
                        StatsModifier.AbilityPower.FlatBonus = 14f;
                    }
                    
                    owner.AddStatModifier(StatsModifier);
                    RemoveBuff(owner, "AbilityUsed");
                }
                else
                {
                    owner.Stats.HealthPoints.BaseValue -= 101f;
                }
            }
        }

        public void OnDeactivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
            RemoveBuff(owner, "RabadonsDeathcapEffect");
        }
    }
}