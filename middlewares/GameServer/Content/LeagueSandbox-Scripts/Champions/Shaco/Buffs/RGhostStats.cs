using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class ShacoChildrenOfTheGraveGhost : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new()
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new();

        private float timeSinceLastTick;
        private AttackableUnit unit;
        private Spell spell;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            this.unit = unit;
            spell = ownerSpell;
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell) { }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;
            if (timeSinceLastTick >= 30000f)
                unit.TakeDamage(spell.CastInfo.Owner, 10000f, DamageType.DAMAGE_TYPE_TRUE,
                              DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
        }
    }
}