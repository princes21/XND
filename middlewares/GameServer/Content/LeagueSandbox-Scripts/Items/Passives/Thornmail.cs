using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerLib.GameObjects.AttackableUnits;

namespace ItemPassives
{
    public class ItemID_3075 : IItemScript
    {
        AttackableUnit Target;
        private ObjAIBase owner;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
            this.owner = owner;
            ApiEventManager.OnTakeDamage.AddListener(this, owner, OnTakeDamage, false);
            {
                // DOOM BOT STUFF
                //if(owner.Stats.Range.Total == 251) { StatsModifier.Size.FlatBonus = 0.75f; StatsModifier.AbilityPower.FlatBonus = 2500f; }
            }
            owner.AddStatModifier(StatsModifier);
        }

        public void OnTakeDamage(DamageData damageData)
        {
            var damage = owner.Stats.Armor.Total * 0.50f;
            var Attacker = damageData.Attacker;
            if(owner.IsMelee) 
            {
            AddBuff("GreviousWound", 9.5f, 1, null, Attacker, owner);
            Attacker.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
        }
        public void OnDeactivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }
    }
}