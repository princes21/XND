using AIScripts;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeaguePackets.Game;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using static LeagueSandbox.GameServer.API.ApiMapFunctionManager;


namespace CharScripts
{

    public class CharScriptBlitzcrank : ICharScript

    {
        ObjAIBase _owner;
        Spell spell;

        // 0 = Normal, 1 = Light Slow, 2 = Heavy Slow, 3 = Stunned
        int _currentTier = 0;

        // Time management variables
        float _lastTickTime = 0f;
        const float TICK_INTERVAL = 0.25f; // Check mana 4 times a second (sufficient and saves performance)

        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            _owner = owner;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);

            // Initialize state
            _currentTier = 0;
            _lastTickTime = 0f;
        }

        public void OnUpdate(float diff)
        {
            if (_owner == null || _owner.IsDead) return;

            // --- 2. CALCULATE NEW TIER ---
            float percentMana = (_owner.Stats.CurrentMana / _owner.Stats.ManaPoints.Total) * 100f;
            int newTier = 0;

            // Determine which tier we SHOULD be in right now
            if (percentMana < 5f) newTier = 3; // Stun
            else if (percentMana < 15f) newTier = 2; // Heavy Slow
            else if (percentMana < 30f) newTier = 1; // Light Slow
            else newTier = 0; // Healthy

            // --- 3. STATE CHANGE ONLY ---
            // Crucial Fix: We ONLY modify stats if the Tier has actually changed.
            // This stops us from fighting the other script every frame.
            if (newTier != _currentTier)
            {
                ApplyTierChange(_currentTier, newTier, spell, _owner);
                _currentTier = newTier;
            }
        }

        void ApplyTierChange(int oldTier, int newTier, Spell spell, ObjAIBase owner)
        {
            // B. APPLY NEW PENALTY
            if (newTier == 1 && _owner.GetBuffWithName("BlitzcrankSlowTier1") == null)
            {
                AddBuff("BlitzcrankSlowTier1", 5.0f, 1, spell, _owner, _owner);
            }

            if (newTier == 2 && _owner.GetBuffWithName("BlitzcrankSlowTier2") == null)
            {
                AddBuff("BlitzcrankSlowTier2", 5.0f, 1, spell, _owner, _owner);
            }
            if (newTier == 3)
            {
                // Stun Logic: Apply buff if not present
                if (_owner.GetBuffWithName("Stun") == null)
                {
                    AddBuff("Stun", 1.0f, 1, null, _owner, _owner);
                }
                // We do NOT apply a speed slow here, because Stun sets move speed to 0 anyway.
            }
        }

        public void OnLaunchAttack(Spell spell)
        {
            // Optional: Force an immediate update on attack so the slow hits instantly
            // rather than waiting 0.25s
            _lastTickTime = 0f;
        }
    }
}