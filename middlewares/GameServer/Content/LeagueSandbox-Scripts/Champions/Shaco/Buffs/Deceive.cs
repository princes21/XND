using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Buffs
{
    internal class Deceive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INVISIBILITY,
            BuffAddType = BuffAddType.RENEW_EXISTING
        };
        public BuffType BuffType => BuffType.DAMAGE;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;
        Particle p;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            if (unit is Champion champion)
            {
                ownerSpell.SetSpellToggle(true);
                ApiEventManager.OnPreAttack.AddListener(this, champion, OnPreAttack, true);
            }
            unit.AddStatModifier(StatsModifier);
            BecomeInvisible(unit);
        }
        public void OnPreAttack(Spell spell)
        {

        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var champion = unit as Champion;
            ownerSpell.SetCooldown(ownerSpell.GetCooldown());
            champion.GetSpell(ownerSpell.SpellName).SetSpellToggle(false);
            BecomeVisible(unit);

            unit.SetStatus(StatusFlags.NoRender, false);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
