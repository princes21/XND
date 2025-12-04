using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiEventManager;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class ShacoCOTGPetBuff2 : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new()
        {
            BuffType = BuffType.AURA,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new();

        private Buff Buff;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Buff = buff;

            // switch owner to pet-control spell
            buff.SourceUnit.SetSpell("HallucinateGuide", 3, true);
            AddBuff("ShacoCOTGSelf", buff.Duration, 1, ownerSpell, buff.SourceUnit, buff.SourceUnit);

            // stealth BOTH for 0.7 s
            AddBuff("Invisibility", 0.70f, 1, ownerSpell, unit, buff.SourceUnit);
            AddBuff("Invisibility", 0.70f, 1, ownerSpell, buff.SourceUnit, buff.SourceUnit);

            OnDeath.AddListener(this, unit, OnCloneDeath, true);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            // restore original R spell
            var spell = buff.SourceUnit.SetSpell("HallucinateFull", 3, true);
            spell.SetCooldown(spell.GetCooldown() - buff.TimeElapsed);
        }

        private void OnCloneDeath(DeathData data)
        {
            AddBuff("ShacoExplode", 0.5f, 1, Buff.OriginSpell, Buff.TargetUnit, Buff.SourceUnit);
            Buff.DeactivateBuff();
        }

        public void OnUpdate(float diff) { }
    }
}