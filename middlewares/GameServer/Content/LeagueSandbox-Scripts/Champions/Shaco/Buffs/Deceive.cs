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

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            if (unit is Champion champion)
            {
                ownerSpell.SetSpellToggle(true);
                ApiEventManager.OnPreAttack.AddListener(this, champion, BreakStealth, true);
            }
            unit.AddStatModifier(StatsModifier);
            BecomeInvisible(unit);
        }

        private void BreakStealth(Spell spell)
        {
            thisBuff?.DeactivateBuff();
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ownerSpell.SetCooldown(ownerSpell.GetCooldown());
            if (unit is Champion champ)
                champ.GetSpell(ownerSpell.SpellName).SetSpellToggle(false);

            ForceVisible(unit);
        }

        public void OnUpdate(float diff) { }
    }
}