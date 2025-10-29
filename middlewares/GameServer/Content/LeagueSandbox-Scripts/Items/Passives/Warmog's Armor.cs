using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace ItemPassives
{
    public class ItemID_3083 : IItemScript
    {
        AttackableUnit unit;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
			ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
        }
        public void OnLaunchAttack(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
            float _IncrementHealth = 10f;
            float _IncrementCurrentMana = 10;
            if (owner.Stats.HealthPoints.Total >= 4000)
            {
                if (owner.Stats.Range.Total <= 255)
                {
                    owner.Stats.HealthPoints.FlatBonus += _IncrementHealth;
                    owner.Stats.CurrentMana += _IncrementCurrentMana;
                    owner.Stats.CurrentHealth += _IncrementHealth;
                }
            }
        }

        public void OnDeactivate(ObjAIBase owner)
        {
			ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }
    }
}