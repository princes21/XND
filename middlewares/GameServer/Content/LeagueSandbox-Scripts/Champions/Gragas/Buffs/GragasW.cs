using LeagueSandbox.GameServer.API;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Buffs
{
    internal class GragasW : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private Buff _buff;
        private ObjAIBase _owner;
        private Spell _ownerSpell;
        private AttackableUnit Target;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _buff = buff;
            _ownerSpell = ownerSpell;
            _owner = ownerSpell.CastInfo.Owner; // Get the owner from the spell, not the unit

            // Listen for the owner's auto attacks to HIT
            ApiEventManager.OnHitUnit.AddListener(this, _owner, OnHitUnit, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            // Remove the listener when the buff ends
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        public void OnHitUnit(DamageData damageData)
        {
            if (!damageData.IsAutoAttack || _buff == null || _buff.Elapsed())
            {
                return;
            }

            float ap = _owner.Stats.AbilityPower.Total * 0.65f;
            float damage = 125 + ap;
            //var target = damageData.Target;

            //target.TakeDamage(_owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            Target = _ownerSpell.CastInfo.Targets[0].Unit;
			var units = GetUnitsInRange(Target.Position, 350f, true);
                for (int i = 0; i < units.Count; i++)
                {
                if (units[i].Team != _owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                {
                    units[i].TakeDamage(_owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);		
                    AddParticleTarget(_owner, Target, "TiamatMelee_itm_hydra.troy", Target);
                }
            }

            // DEACTIVATE THE BUFF AFTER IT PROCS
            _buff.DeactivateBuff();
        }
    }
}