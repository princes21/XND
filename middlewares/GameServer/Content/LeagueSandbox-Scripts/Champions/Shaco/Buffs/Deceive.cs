using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class Deceive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new()
        {
            BuffType = BuffType.INVISIBILITY,
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new();

        private Buff thisBuff;
        private bool hasUsedGuaranteedCrit = false;
        private Champion ownerChampion;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            if (unit is Champion champion)
            {
                ownerChampion = champion;
                ownerSpell.SetSpellToggle(true);

                // Set crit chance to 100% to guarantee next attack is a crit
                StatsModifier.CriticalChance.FlatBonus = 1.0f; // 100% crit chance
                unit.AddStatModifier(StatsModifier);

                // Listen for the first attack to remove the crit modifier
                ApiEventManager.OnPreAttack.AddListener(this, champion, OnFirstAttack, true);
            }
            unit.AddStatModifier(StatsModifier);
            BecomeInvisible(unit);
        }

        private void OnFirstAttack(Spell spell)
        {
            // Remove the crit chance modifier after first attack
            if (!hasUsedGuaranteedCrit && ownerChampion != null)
            {
                hasUsedGuaranteedCrit = true;
                ownerChampion.RemoveStatModifier(StatsModifier);

                // Remove the listener since we only want this once
                ApiEventManager.OnPreAttack.RemoveListener(this, ownerChampion, OnFirstAttack);
            }

            // Break stealth on attack
            BreakStealth(spell);
        }

        private void BreakStealth(Spell spell)
        {
            thisBuff?.DeactivateBuff();
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ownerSpell.SetCooldown(ownerSpell.GetCooldown());
            if (unit is Champion champ)
            {
                champ.GetSpell(ownerSpell.SpellName).SetSpellToggle(false);

                // Clean up listeners and modifiers
                ApiEventManager.OnPreAttack.RemoveListener(this, champ, OnFirstAttack);
                if (!hasUsedGuaranteedCrit)
                {
                    champ.RemoveStatModifier(StatsModifier);
                }
            }

            ForceVisible(unit);
        }

        public void OnUpdate(float diff) { }
    }
}