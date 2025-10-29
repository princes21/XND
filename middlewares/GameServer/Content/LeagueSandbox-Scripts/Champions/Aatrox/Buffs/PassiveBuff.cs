using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    class AatroxPassive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        Minion Leblanc;
        Spell Spell;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
			Spell = ownerSpell;
			if (ownerSpell.CastInfo.Owner is Champion owner)
            {
			    ApiEventManager.OnTakeDamage.AddListener(this, owner, OnTakeDamage, false);
			}
            
        }
		public void OnTakeDamage(DamageData damageData)       
        {
            if (Spell.CastInfo.Owner is Champion owner)
            {
                var currentHealth = owner.Stats.CurrentHealth;
                var limitHealth = owner.Stats.HealthPoints.Total * 0.2;
                if (limitHealth >= currentHealth)
                {
                    if (owner.HasBuff("AatroxPassive"))
                    {
                        owner.RemoveBuffsWithName("AatroxPassive");
                    }
                }
			}
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
		   if (Spell.CastInfo.Owner is Champion owner)
           {
                AddBuff("AatroxPassiveDeath", 3f, 1, ownerSpell, owner, owner,false); 
		   }
        }
    }
}