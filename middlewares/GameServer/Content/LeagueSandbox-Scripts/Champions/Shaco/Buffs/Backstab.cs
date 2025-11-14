using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Scripting.CSharp;

namespace Buffs
{
    internal class Backstab : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE, 
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1,
            IsHidden = true
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private Buff thisBuff;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;

            if (unit is Champion champion)
            {
                ApiEventManager.OnPreAttack.AddListener(this, champion, OnPreAttack, true);

                
                StatsModifier.CriticalDamage.FlatBonus += 1.0f; 
                unit.AddStatModifier(StatsModifier);
            }
        }

        private void OnPreAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

            
            thisBuff?.DeactivateBuff();
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit is Champion champion)
            {
                ApiEventManager.OnPreAttack.RemoveListener(this, champion, null);
            }
        }

        public void OnUpdate(float diff) { }
    }
}
