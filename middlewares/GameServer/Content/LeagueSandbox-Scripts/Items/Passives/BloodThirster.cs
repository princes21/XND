using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace ItemPassives
{
    public class ItemID_3072 : IItemScript
    {
        AttackableUnit Target;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            if(owner.Stats.Range.Total <= 255)
            {
                StatsModifier.SpellVamp.FlatBonus = 0.10f;
                StatsModifier.LifeSteal.FlatBonus = 0.20f;
            }

            owner.AddStatModifier(StatsModifier);
        }
        public void OnLaunchAttack(Spell spell)
        { 
            var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;

            if(owner.Stats.Range.Total <= 255) 
            {
                AddBuff("BloodThirster_MeleeBleed", 3.0f, 1, spell, Target, owner);
            } else 
            {
                AddBuff("BloodThirster_RangedBleed", 5.5f, 1, spell, Target, owner);
            }

        }
        public void OnDeactivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }
    }
}