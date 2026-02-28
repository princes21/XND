using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Buffs
{
    internal class Haste : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HASTE,
            BuffAddType = BuffAddType.STACKS_AND_OVERLAPS,
            MaxStacks = 100
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle haste;
        AttackableUnit owner;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            owner = unit;
            haste = AddParticleTarget(unit, unit, "Global_Haste", unit, buff.Duration);
            var hasteAmount = 0.35f;
            SetHasteMod(hasteAmount);

            // Normally this would be here and would use data given by another script.
            //StatsModifier.MoveSpeed.PercentBonus -= slowAmount;
            //owner.AddStatModifier(StatsModifier);

            // ApplyAssistMarker(buff.SourceUnit, unit, 10.0f);

            // For attack speed and move speed mod changes:
            // ApiEventManager.OnUpdateBuffs.AddListener(this, buff, OnUpdateBuffs, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(haste);
        }

        // TODO: Find a better way to transfer data between scripts.
        public void SetHasteMod(float hasteAmount)
        {
            StatsModifier.MoveSpeed.PercentBonus += hasteAmount;
            owner.AddStatModifier(StatsModifier);
        }
    }
}