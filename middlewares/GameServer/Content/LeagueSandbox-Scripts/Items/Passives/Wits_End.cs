using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace ItemPassives
{
    public class ItemID_3091 : IItemScript
    {
        AttackableUnit Target;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            StatsModifier.Armor.FlatBonus = 15;
            StatsModifier.MagicResist.FlatBonus = 15;
            //StatsModifier.AttackSpeed.FlatBonus = 0.40f;
            owner.AddStatModifier(StatsModifier);
        }

        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;
            var MeleeDamage = 42 + owner.Stats.AttackDamage.Total * 0.015f + owner.Stats.AbilityPower.Total * 0.015f;
            var RangedDamage =  42 + owner.Stats.AttackDamage.Total * 0.035f + owner.Stats.AbilityPower.Total * 0.065f;

            if(owner.Stats.Range.Total <= 255) 
            {
            Target.TakeDamage(owner, MeleeDamage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }

            if(owner.Stats.Range.Total >= 400) 
            {
            Target.TakeDamage(owner, RangedDamage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
        }
        public void OnDeactivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }
    }
}