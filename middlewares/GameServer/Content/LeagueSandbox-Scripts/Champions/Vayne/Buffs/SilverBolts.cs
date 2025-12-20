using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Buffs
{
    internal class VayneSilveredBolts : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 3
        };
        public StatsModifier StatsModifier { get; private set; }
        private Buff _buff;


        Particle p;
        AttackableUnit Unit;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            _buff = buff; // Fixed: Assign the buff reference
            switch (buff.StackCount)
            {
                case 1:
                    p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "vayne_W_ring1.troy", unit, float.MaxValue);
                    break;
                case 2:
                    RemoveParticle(p);
                    p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "vayne_W_ring2.troy", unit, float.MaxValue);
                    break;
                case 3:
                    // Remove the stack 2 ring
                    if (p != null)
                    {
                        RemoveParticle(p);
                        p = null;
                    }
                    AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "vayne_W_tar.troy", unit, float.MaxValue);
                    TargetTakeDamage(ownerSpell);
                    buff.DeactivateBuff();
                    break;
            }
        }

        public void OnUpdate(float diff)
        {
            if (Unit != null && Unit.HasBuff("VayneSilveredBolts") && Unit.IsDead)
            {
                _buff.DeactivateBuff();

            }
        }


        public void TargetTakeDamage(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

            float missingHealthPercent = 0.10f; // 10% of missing health, adjust as needed
            float missingHealth = Unit.Stats.HealthPoints.Total - Unit.Stats.CurrentHealth;
            float damage = missingHealth * missingHealthPercent;
            Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (p != null)
            {
                RemoveParticle(p);
                p = null;
            }
        }
    }
}