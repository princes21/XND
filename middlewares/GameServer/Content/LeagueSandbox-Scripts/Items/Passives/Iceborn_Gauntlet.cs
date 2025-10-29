using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3025 : IItemScript
    {

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private Shield NewShield;
        public void OnActivate(ObjAIBase owner)
        {
            //ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
        }
        //public void OnLaunchAttack(Spell spell, AttackableUnit target)
        //{
        //    var owner = spell.CastInfo.Owner;
        //    float MaxRequiredHealth = 3000;
        //    float MaxRequiredAP = 500;
        //        
        //    if (owner.HasBuff("AbilityUsed"))
        //    {
        //        if (owner.Stats.HealthPoints.Total >= MaxRequiredHealth && owner.Stats.AbilityPower.Total >= MaxRequiredAP)
        //        {
        //            target.RemoveShield(NewShield);
        //        }
        //    }
        //}

        public void OnDeactivate(ObjAIBase owner)
        {
			//ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }
    }
}