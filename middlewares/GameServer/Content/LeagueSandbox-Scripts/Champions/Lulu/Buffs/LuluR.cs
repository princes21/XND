using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class LuluR : IBuffGameScript
    {
        private readonly int TrueResist;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Spell OwnerSpell;
        Particle cast;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            OwnerSpell = ownerSpell;
            var TrueResist = 185;
            cast = AddParticleTarget(owner, unit, "Lulu_R_cas", unit);

            if (owner.Stats.AbilityPower.Total >= 100)
            {
                //Melee Stuff
                if (unit.Stats.Range.Total <= 255 & unit.Stats.HealthPoints.Total <= 4000 & unit.Stats.AttackDamage.Total >= 300)
                {
                    StatsModifier.AttackDamage.FlatBonus = owner.Stats.AbilityPower.Total * 0.25f;
                }
                if (unit.Stats.Range.Total <= 255 & unit.Stats.Armor.Total >= TrueResist & unit.Stats.MagicResist.Total >= TrueResist)
                {
                    StatsModifier.Armor.FlatBonus = owner.Stats.AbilityPower.Total * 0.15f;
                    StatsModifier.MagicResist.FlatBonus = owner.Stats.AbilityPower.Total * 0.15f;
                }
                if (unit.Stats.Range.Total <= 255 & unit.Stats.HealthPoints.Total >= 5500 & unit.Stats.AttackDamage.Total <= 350)
                {
                    StatsModifier.HealthPoints.FlatBonus = owner.Stats.AbilityPower.Total * 2.00f;
                }
                //------------------------------------------------------------------------------------------------------------------------
            }

            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(cast);
            AddParticleTarget(OwnerSpell.CastInfo.Owner, unit, "Lulu_R_expire", unit);
        }
    }
}

