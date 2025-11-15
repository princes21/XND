using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class ShacoCOTGDot : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new()
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            float basePercent = 0.012f + 0.0025f * (buff.OriginSpell.CastInfo.SpellLevel - 1);
            float apRatio = buff.SourceUnit.Stats.AbilityPower.Total * 0.00002f;
            float damage = buff.TargetUnit.Stats.HealthPoints.Total * (basePercent + apRatio);

            var data = buff.TargetUnit.TakeDamage(buff.SourceUnit, damage,
                                                 DamageType.DAMAGE_TYPE_MAGICAL,
                                                 DamageSource.DAMAGE_SOURCE_PROC, false);
            buff.SourceUnit.Stats.CurrentHealth += data.PostMitigationDamage;
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell) { }
        public void OnUpdate(float diff) { }
    }
}