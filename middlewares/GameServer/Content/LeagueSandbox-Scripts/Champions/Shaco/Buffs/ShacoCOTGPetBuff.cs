using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class ShacoCOTGPetBuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new()
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            if (unit is not Pet pet) return;

            if (owner.HasBuff("Invisibility")) return;

            AddBuff("Invisibility", 0.70f, 1, ownerSpell, pet, owner);
            AddBuff("Invisibility", 0.70f, 1, ownerSpell, owner, owner);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell) { }
        public void OnUpdate(float diff) { }
    }
}