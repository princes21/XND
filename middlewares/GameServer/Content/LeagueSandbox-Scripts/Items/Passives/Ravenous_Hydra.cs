using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;   
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace ItemPassives
{
    public class ItemID_3074 : IItemScript
    {
		private ObjAIBase owner;
        private Spell spell;
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
            AddParticleTarget(owner, Target, "TiamatMelee_itm_hydra.troy", owner);
			var units = GetUnitsInRange(Target.Position, 350f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Team != owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                {
                            
                    var AdDamage = owner.Stats.AttackDamage.Total * 0.3f;
                    units[i].TakeDamage(owner, AdDamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);										
                }
            }
        }    
        public void OnDeactivate(ObjAIBase owner)
        {
			ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }
    }
}