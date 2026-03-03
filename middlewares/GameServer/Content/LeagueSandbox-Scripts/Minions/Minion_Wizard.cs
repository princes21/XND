using AIScripts;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeaguePackets.Game;
using LeagueSandbox.GameServer;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Logging;
using log4net;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using static LeagueSandbox.GameServer.API.ApiMapFunctionManager;


namespace CharScripts
{
    public class CharScriptRed_Minion_Wizard : ICharScript
    {
        ObjAIBase _owner;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            _owner = owner;
            if (owner is ObjAIBase)
            {
                owner.AddStatModifier(StatsModifier);
            }
        }

        public void OnUpdate(float diff)
        {
            if (_owner == null || _owner.IsDead) return;

            float currentMinutes = GameTime() / (1000f * 60f);

            if (!_owner.HasBuff("MinionWizardBuff"))
            {
                AddBuff("MinionWizardBuff", float.MaxValue, 1, null, _owner, _owner, true);
            }
        }
    }
}

namespace CharScripts
{
    public class CharScriptBlue_Minion_Wizard : ICharScript
    {
        ObjAIBase _owner;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            _owner = owner;
            if (owner is ObjAIBase)
            {
                owner.AddStatModifier(StatsModifier);
            }
        }

        public void OnUpdate(float diff)
        {
            if (_owner == null || _owner.IsDead) return;

            float currentMinutes = GameTime() / (1000f * 60f);

            if (!_owner.HasBuff("MinionWizardBuff"))
            {
                AddBuff("MinionWizardBuff", float.MaxValue, 1, null, _owner, _owner, true);
            }
        }
    }
}


namespace Buffs
{
    class MinionWizardBuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        ObjAIBase unit;


        // At 30 min we want ~100 AD, so percent multiplier should bring 23 -> 100
        // 100 / 23 = ~4.35, so PercentBonus (on top of base) = 3.35 at 30 min
        private const float TARGET_MINUTES = 30f;
        private const float AD_PERCENT_AT_TARGET = 3.35f;  // x4.35 total AD at 30 min
        private const float HP_PERCENT_AT_TARGET = 2.0f;   // x3.0 total HP at 30 min (290 -> ~870)


        // How fast it grows past the target — no hard cap, just keeps scaling
        // Every additional 10 minutes adds another 50% of the target bonus
        private const float AD_PERCENT_PER_MINUTE = 0.1117f;  // AD_PERCENT_AT_TARGET / TARGET_MINUTES
        private const float HP_PERCENT_PER_MINUTE = 0.0467f;  // HP_PERCENT_AT_TARGET / TARGET_MINUTES

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            float minutes = GameTime() / (1000f * 60f);


            if (unit is ObjAIBase)
            {
                float adPercent = AD_PERCENT_PER_MINUTE * minutes;
                float hpPercent = HP_PERCENT_PER_MINUTE * minutes;

                StatsModifier.AttackDamage.PercentBonus = adPercent;
                StatsModifier.HealthPoints.PercentBonus = hpPercent;

                unit.AddStatModifier(StatsModifier);  // <-- this was missing


            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit is ObjAIBase)
            {
            }
        }
    }
}
