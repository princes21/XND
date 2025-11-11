using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace ItemPassives
{
    public class ItemID_3102 : IItemScript
    {

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private ObjAIBase _owner;
        private const float BANSHEE_COOLDOWN = 30.0f; // Match this with RoA_BANSHEE_COOLDOWN
        public void OnActivate(ObjAIBase owner)
        {
            _owner = owner;  // <-- You forgot to add this line!
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            StatsModifier.HealthPoints.FlatBonus = 500f;
            StatsModifier.ManaPoints.FlatBonus = 800f;
            owner.AddStatModifier(StatsModifier);
            owner.Stats.CurrentMana += 800f;
            if (owner.Stats.HealthPoints.Total <= 8575f && !owner.HasBuff("RodOfAgesCooldown") && !owner.HasBuff("AntiRodOfAges"))
            {
                AddBuff("AntiRodOfAges", 10.0f, 1, null, owner, owner, true);
            }
        }

        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            if (owner.Stats.HealthPoints.Total >= 8575f)
            {
                owner.Stats.HealthPoints.BaseValue -= 50f;
            }
        }

        public void OnUpdate(float diff)
        {
            if (_owner == null) return; // Safety check

            // Re-arm the anti-ROA shield ONLY when:
            // 1. Cooldown has expired
            // 2. Shield is not already active
            if (!_owner.HasBuff("RodOfAgesCooldown") && !_owner.HasBuff("AntiRodOfAges"))
            {
                AddBuff("AntiRodOfAges", 999.0f, 1, null, _owner, _owner, true); // Long duration, cooldown controls it
            }
        }

        public void OnDeactivate(ObjAIBase owner)
        {
            RemoveBuff(owner, "AntiRodOfAges");
        }
    }
}