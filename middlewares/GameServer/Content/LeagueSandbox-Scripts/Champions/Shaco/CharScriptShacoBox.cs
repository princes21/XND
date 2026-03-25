using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.Logging;
using log4net;

namespace CharScripts
{
    public class CharScriptShacoBox : ICharScript
    {
        ObjAIBase _owner;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            _owner = owner;
            if (owner is ObjAIBase)
            {
                owner.AddStatModifier(StatsModifier);
                
            }
        }

        public void OnUpdate(float diff)
        {
            if (_owner == null || _owner.IsDead) return;

            if (!_owner.HasBuff("ShacoBoxBuff"))
            {
                AddBuff("ShacoBoxBuff", float.MaxValue, 1, null, _owner, _owner, true);
            }
        }
    }
}


namespace Buffs
{
    class ShacoBoxBuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        ObjAIBase Unit;


        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit is ObjAIBase ai)
            {
                Unit = ai; //this was somehow important I guess, I also changed objAIBase to Unit, taking it from blitz code and the listener finally began working
                buff.SetStatusEffect(StatusFlags.CanMove, false);
                buff.SetStatusEffect(StatusFlags.CanMoveEver, false); 
                unit.AddStatModifier(StatsModifier);  // <-- this was missing
                ApiEventManager.OnHitUnit.AddListener(this, ai, OnHitUnit, false);
            }
        }

        private void OnHitUnit(DamageData data)
        {
            float TargetHealth = data.Target.Stats.HealthPoints.Total;
            var target = data.Target;
            float DamageDealtPercent = TargetHealth * 0.08f;

            data.PostMitigationDamage = DamageDealtPercent;

        }


        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit is ObjAIBase)
            {
            }
        }
    }
}
