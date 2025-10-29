using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3078 : IItemScript
    {
        AttackableUnit Target;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
        }
        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;
            var Melee_Damage = owner.Stats.AttackDamage.Total * 0.65f;
            var Melee_Heal_Reduced = owner.Stats.AttackDamage.Total * -0.50f;
            var Melee_Heal_GreviousWound = owner.Stats.AttackDamage.Total * -0.60f;
            var Ranged_Damage = owner.Stats.AttackDamage.Total * 0.35f;
            var Magic_Damage = owner.Stats.AbilityPower.Total * 0.55f;
            if(owner.Stats.Range.Total <= 255)
            {
                AddBuff("TrinityForceMS", 1f, 1, spell, owner, owner);
                if(owner.HasBuff("AbilityUsed")) 
                {
                    Target.TakeDamage(owner, Melee_Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    if(owner.HasBuff("GreviousWound"))
                    {
                        owner.Stats.CurrentHealth += Melee_Damage + Melee_Heal_GreviousWound;
                    } else
                    {
                        owner.Stats.CurrentHealth += Melee_Damage + Melee_Heal_Reduced;
                    }
                    RemoveBuff(owner,"AbilityUsed");
                }
            }
            if(owner.Stats.Range.Total >= 400 & owner.HasBuff("AbilityUsed"))
            {
                Target.TakeDamage(owner, Magic_Damage + Ranged_Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                RemoveBuff(owner,"AbilityUsed");
            }
        }
        public void OnDeactivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }
    }
}