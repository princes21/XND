using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;


namespace Buffs
{
    class MordekaiserCreepingDeath : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        ObjAIBase Owner;
        Spell Spell;
        AttackableUnit Target;
        float ticks = 0;
        float damage;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Target = unit;
            Owner = ownerSpell.CastInfo.Owner;
            Spell = ownerSpell;
            var APratio = Owner.Stats.AbilityPower.Total * 0.2f;
            damage = 24f + (14f * (ownerSpell.CastInfo.SpellLevel - 1)) + APratio;

            StatsModifier.Armor.FlatBonus += 10 + (5 * (ownerSpell.CastInfo.SpellLevel - 1));
            StatsModifier.MagicResist.FlatBonus += 10 + (5 * (ownerSpell.CastInfo.SpellLevel - 1));
            unit.AddStatModifier(StatsModifier);

            var units = GetUnitsInRange(Target.Position, 350f, true);
            for (int i = units.Count - 1; i >= 0; i--)
            {
                if (units[i].Team != Spell.CastInfo.Owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret) && units[i] is ObjAIBase ai)
                {
                    units[i].TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    units.RemoveAt(i);
                }
            }
            AddParticleTarget(Owner, unit, "mordekaiser_creepingDeath_aura.troy", unit, buff.Duration);
        }

        public void OnUpdate(float diff)
        {
            ticks += diff;
            if (ticks >= 1000.0f)
            {
                var units = GetUnitsInRange(Target.Position, 350f, true);
                for (int i = units.Count - 1; i >= 0; i--)
                {
                    if (units[i].Team != Spell.CastInfo.Owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret) && units[i] is ObjAIBase ai)
                    {
                        units[i].TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                        units.RemoveAt(i);
                    }
                }
                ticks = 0f;
            }
        }
    }
}
