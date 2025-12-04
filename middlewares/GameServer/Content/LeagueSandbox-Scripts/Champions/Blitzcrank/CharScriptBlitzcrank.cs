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

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            _owner = owner;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            if (_owner is ObjAIBase)
            {
                StatsModifier.MoveSpeed.PercentBonus = 0f;
                _owner.AddStatModifier(StatsModifier);
            }
            // Initialize state
            _currentTier = 0;
        }

        public void OnUpdate(float diff)
        {
            if (_owner == null || _owner.IsDead) return;

            float percentMana = (_owner.Stats.CurrentMana / _owner.Stats.ManaPoints.Total) * 100f;
            int newTier = 0;

            // Determine tier (0 = best, 5 = worst)
            if (percentMana < 5f) newTier = 5;      // Stun
            else if (percentMana < 15f) newTier = 4; // Heavy Slow (-50%)
            else if (percentMana < 30f) newTier = 3; // Light Slow (-30%)
            else if (percentMana < 50f) newTier = 2; // Normal (no buff)
            else if (percentMana < 80f) newTier = 1; // Light MS (+30%)
            else newTier = 0;                        // Heavy MS (+50%)

            if (newTier != _currentTier)
            {
                ApplyTierChange(_currentTier, newTier);
                _currentTier = newTier;
            }
        }

        void ApplyTierChange(int oldTier, int newTier)
        {
            // STEP 1: Remove ALL old buffs
            RemoveAllTierBuffs();

            // STEP 2: Apply ONLY the new tier's buff
            switch (newTier)
            {
                case 0: // >80% mana: +50% MS
                    AddBuff("BlitzcrankMSBuffTier1", 25.0f, 1, spell, _owner, _owner);
                    break;
                case 1: // 50-80% mana: +30% MS
                    AddBuff("BlitzcrankMSBuffTier2", 25.0f, 1, spell, _owner, _owner);
                    break;
                case 2: // 30-50% mana: Normal (no buff)
                    RemoveAllTierBuffs();
                    break;
                case 3: // 15-30% mana: -30% MS
                    AddBuff("BlitzcrankSlowTier2", 25.0f, 1, spell, _owner, _owner);
                    break;
                case 4: // 5-15% mana: -50% MS
                    AddBuff("BlitzcrankSlowTier1", 25.0f, 1, spell, _owner, _owner);
                    break;
                case 5: // <5% mana: Stun
                    AddBuff("Stun", 1.0f, 1, spell, _owner, _owner);
                    break;
            }
        }

        void RemoveAllTierBuffs()
        {
            // Remove all possible tier buffs before applying new one
            var buff1 = _owner.GetBuffWithName("BlitzcrankMSBuffTier1");
            if (buff1 != null) _owner.RemoveBuff(buff1);

            var buff2 = _owner.GetBuffWithName("BlitzcrankMSBuffTier2");
            if (buff2 != null) _owner.RemoveBuff(buff2);

            var slow1 = _owner.GetBuffWithName("BlitzcrankSlowTier1");
            if (slow1 != null) _owner.RemoveBuff(slow1);

            var slow2 = _owner.GetBuffWithName("BlitzcrankSlowTier2");
            if (slow2 != null) _owner.RemoveBuff(slow2);

            var stun = _owner.GetBuffWithName("Stun");
            if (stun != null) _owner.RemoveBuff(stun);
        }


        public void OnLaunchAttack(Spell spell)
        {

        }
    }
}