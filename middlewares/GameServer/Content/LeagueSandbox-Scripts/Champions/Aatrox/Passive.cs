using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace CharScripts
{
    public class CharScriptAatrox : ICharScript
    {
        Spell Spell;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Spell = spell;
            owner.Stats.CurrentMana = owner.Stats.ManaPoints.Total;

            owner.RemoveStatModifier(StatsModifier);
            float bloodPercent = owner.Stats.CurrentMana / owner.Stats.ManaPoints.Total * 100f;
            float bonusAttackSpeed = (bloodPercent / 2f) * 0.01f;
            StatsModifier.AttackSpeed.PercentBonus = bonusAttackSpeed;
            owner.AddStatModifier(StatsModifier);

            var Healthdiff = owner.Stats.CurrentMana * 1f;
            var Health = owner.Stats.CurrentHealth * 1f;
            if (Healthdiff == Health)
            {
                owner.Stats.CurrentMana -= Healthdiff;
            }

            AddBuff("AatroxPassive", 25000f, 1, Spell, Spell.CastInfo.Owner, Spell.CastInfo.Owner, false);
        }
        public void OnUpdate(float diff)
        {
            var owner = Spell.CastInfo.Owner;

            owner.RemoveStatModifier(StatsModifier);
            float bloodPercent = owner.Stats.CurrentMana / owner.Stats.ManaPoints.Total * 100f;
            float bonusAttackSpeed = (bloodPercent / 2f) * 0.01f;
            StatsModifier.AttackSpeed.PercentBonus = bonusAttackSpeed;
            owner.AddStatModifier(StatsModifier);

            var Healthdiff = owner.Stats.CurrentMana * 1f;
            var Health = owner.Stats.CurrentHealth * 1f;
            if (Healthdiff == Health)
            {
                owner.Stats.CurrentMana -= Healthdiff;
            }
        }
    }
}

