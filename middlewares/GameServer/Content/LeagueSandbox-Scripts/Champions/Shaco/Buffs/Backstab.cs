using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using static LeagueSandbox.GameServer.API.ApiEventManager;

namespace Buffs
{
    internal class Backstab : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new()
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1,
            IsHidden = true
        };

        public StatsModifier StatsModifier { get; private set; } = new();
        private Buff thisBuff;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            if (unit is Champion champion)
            {
                OnPreAttack.AddListener(this, champion, BreakStealth, true);
                StatsModifier.CriticalDamage.PercentBonus += 1.0f;
                unit.AddStatModifier(StatsModifier);
            }
        }

        private void BreakStealth(Spell spell) => thisBuff?.DeactivateBuff();

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit is Champion c)
                OnPreAttack.RemoveListener(this, c, BreakStealth);
        }

        public void OnUpdate(float diff) { }
    }
}