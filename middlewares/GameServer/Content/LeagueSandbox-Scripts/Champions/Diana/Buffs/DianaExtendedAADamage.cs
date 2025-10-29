using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs
{
    class DianaExtendedAADamage : IBuffGameScript
    {
        AttackableUnit Target;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);

            StatsModifier.AttackSpeed.FlatBonus = 1.20f;
            StatsModifier.Range.FlatBonus = 500f;
            unit.AddStatModifier(StatsModifier);
        }
        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            float Damage = 100f + owner.Stats.AttackDamage.Total * 0.40f + owner.Stats.AbilityPower.Total * 0.15f;
            Target = spell.CastInfo.Targets[0].Unit;
            Target.TakeDamage(owner, Damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }
        public void OnDeactivate()
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }
    }
}
