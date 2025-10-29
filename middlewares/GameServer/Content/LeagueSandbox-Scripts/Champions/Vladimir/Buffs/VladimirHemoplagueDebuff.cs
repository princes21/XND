using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs
{
    internal class VladimirHemoplagueDebuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData { BuffType = BuffType.COMBAT_DEHANCER, BuffAddType = BuffAddType.REPLACE_EXISTING, MaxStacks = 1 };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;

            AddParticleTarget(owner, unit, "Vladimir_Base_R_debuff.troy", unit, 5f, 1, "");
            AddParticleTarget(owner, unit, "Vladimir_Base_R_damageproc.troy", unit, 5f, 1, "");
            AddParticleTarget(owner, unit, "VladTidesofBloodHeart_mis.troy", unit, 5f, 1, "");

            StatsModifier.Armor.PercentBonus = -0.12f;
            StatsModifier.MagicResist.PercentBonus = -0.12f;
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;

            AddBuff("VladimirRDamage", 0.85f, 1, ownerSpell, unit, owner);

            StatsModifier.Armor.PercentBonus = -0.12f;
            StatsModifier.MagicResist.PercentBonus = -0.12f;
        }

    }

}