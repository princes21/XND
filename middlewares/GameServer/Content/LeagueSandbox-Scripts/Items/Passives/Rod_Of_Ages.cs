using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Enums;

// Bless Kot438 for the Help on Showing in the Chat the Stacks amount.
namespace ItemPassives
{
    public class ItemID_3027 : IItemScript
    {
        AttackableUnit Target;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            StatsModifier.AttackSpeed.FlatBonus = 0.60f;
            owner.AddStatModifier(StatsModifier);
        }
        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;
            var NameofBuffICON = "ArchAngelsDummySpell";
            var RodOfAges_Stacks = owner.GetBuffWithName("RodOfAges_Stacks");
            float TotalStacks = RodOfAges_Stacks != null ? RodOfAges_Stacks.StackCount : 0;

            // If target has Banshee's Veil anti-buff, consume it and apply a cooldown so it can only block once per X seconds.
            const float RoA_BANSHEE_COOLDOWN = 60.0f; // X seconds; adjust as needed

            if (Target.HasBuff("AntiRodOfAges"))
            {
                if (!Target.HasBuff("RodOfAgesCooldown"))
                {
                    // Put Banshee's interaction on cooldown for the target
                    AddBuff("RodOfAgesCooldown", RoA_BANSHEE_COOLDOWN, 1, spell, Target, owner);
                }

                // Show floating text on the target that blocked it
                ApiFunctionManager.DisplayFloatingText(Target, "Rod of Ages Blocked!", FloatTextType.Absorbed);

                // Consume the anti-buff so it blocks only once until re-armed after cooldown
                RemoveBuff(Target, "AntiRodOfAges");

                // Prevent the proc from immediately re-attempting this attack window
                if (owner.HasBuff("AbilityUsed"))
                {
                    RemoveBuff(owner, "AbilityUsed");
                }
                RemoveBuff(owner, "RodOfAges_Stacks");
                RemoveBuff(owner, NameofBuffICON);
                return;
            }

            //Add Stacks to the Enemy Targeted.
            if (owner.Stats.AttackDamage.Total >= 295 || owner.Stats.AbilityPower.Total >= 600)
            {
                AddBuff("RodOfAges_Stacks", 25000f, 1, spell, owner, owner, true);
                AddBuff("RodOfAges_Stacks", 25000f, 1, spell, owner, owner, true);
                AddBuff("RodOfAges_Stacks", 25000f, 1, spell, owner, owner, true);
                AddBuff("RodOfAges_Stacks", 25000f, 1, spell, owner, owner, true);
                AddBuff("RodOfAges_Stacks", 25000f, 1, spell, owner, owner, true);
                // Show the Icon as a Placeholder on an another buff icon.
                AddBuff(NameofBuffICON, 25000f, 1, spell, owner, owner, true);
                AddBuff(NameofBuffICON, 25000f, 1, spell, owner, owner, true);
                AddBuff(NameofBuffICON, 25000f, 1, spell, owner, owner, true);
                AddBuff(NameofBuffICON, 25000f, 1, spell, owner, owner, true);
                AddBuff(NameofBuffICON, 25000f, 1, spell, owner, owner, true);
            } else
            {
                AddBuff("RodOfAges_Stacks", 25000f, 1, spell, owner, owner, true);
                AddBuff("RodOfAges_Stacks", 25000f, 1, spell, owner, owner, true);
                AddBuff(NameofBuffICON, 25000f, 1, spell, owner, owner, true);
                AddBuff(NameofBuffICON, 25000f, 1, spell, owner, owner, true);
            }

            RodOfAges_Stacks = owner.GetBuffWithName("RodOfAges_Stacks");
            TotalStacks = RodOfAges_Stacks != null ? RodOfAges_Stacks.StackCount : 0;

            if (!Target.HasBuff("AntiRodOfAges"))
            {
                if (TotalStacks >= 15 && TotalStacks <= 44 && owner.HasBuff("AbilityUsed"))
                {
                    Target.TakeDamage(owner, Target.Stats.HealthPoints.Total * 0.025f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    RemoveBuff(owner, "AbilityUsed");
                    RemoveBuff(owner, "RodOfAges_Stacks");
                    RemoveBuff(owner, NameofBuffICON);

                    RodOfAges_Stacks = owner.GetBuffWithName("RodOfAges_Stacks");
                    TotalStacks = RodOfAges_Stacks != null ? RodOfAges_Stacks.StackCount : 0;
                }

                if (TotalStacks >= 45 && TotalStacks <= 74 && owner.HasBuff("AbilityUsed"))
                {
                    Target.TakeDamage(owner, Target.Stats.HealthPoints.Total * 0.125f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    RemoveBuff(owner, "AbilityUsed");
                    RemoveBuff(owner, "RodOfAges_Stacks");
                    RemoveBuff(owner, NameofBuffICON);

                    RodOfAges_Stacks = owner.GetBuffWithName("RodOfAges_Stacks");
                    TotalStacks = RodOfAges_Stacks != null ? RodOfAges_Stacks.StackCount : 0;
                }

                if (TotalStacks >= 75 && TotalStacks <= 124 && owner.HasBuff("AbilityUsed"))
                {
                    Target.TakeDamage(owner, Target.Stats.HealthPoints.Total * 0.50f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    RemoveBuff(owner, "AbilityUsed");
                    RemoveBuff(owner, "RodOfAges_Stacks");
                    RemoveBuff(owner, NameofBuffICON);

                    RodOfAges_Stacks = owner.GetBuffWithName("RodOfAges_Stacks");
                    TotalStacks = RodOfAges_Stacks != null ? RodOfAges_Stacks.StackCount : 0;
                }

                if (TotalStacks >= 125 && TotalStacks <= 249 && owner.HasBuff("AbilityUsed"))
                {
                    AddBuff("LiandrysTormentBurn", 4.0f, 1, spell, Target, owner);
                    AddBuff("GreviousWound", 9.5f, 1, spell, Target, owner);
                    RemoveBuff(owner, "AbilityUsed");
                    RemoveBuff(owner, "RodOfAges_Stacks");
                    RemoveBuff(owner, NameofBuffICON);

                    RodOfAges_Stacks = owner.GetBuffWithName("RodOfAges_Stacks");
                    TotalStacks = RodOfAges_Stacks != null ? RodOfAges_Stacks.StackCount : 0;
                }

                if (TotalStacks >= 250 && TotalStacks <= 349 && owner.HasBuff("AbilityUsed"))
                {
                    AddBuff("LiandrysTormentBurnCrit", 5.0f, 1, spell, Target, owner);
                    AddBuff("SummonerDot", 5.0f, 1, spell, Target, owner);
                    AddBuff("GreviousWound", 13.5f, 1, spell, Target, owner);
                    RemoveBuff(owner, "AbilityUsed");
                    RemoveBuff(owner, "RodOfAges_Stacks");
                    RemoveBuff(owner, NameofBuffICON);

                    RodOfAges_Stacks = owner.GetBuffWithName("RodOfAges_Stacks");
                    TotalStacks = RodOfAges_Stacks != null ? RodOfAges_Stacks.StackCount : 0;
                }

                if (TotalStacks == 350 && owner.HasBuff("AbilityUsed"))
                {
                    Target.TakeDamage(owner, Target.Stats.HealthPoints.Total, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    RemoveBuff(owner, "AbilityUsed");
                    RemoveBuff(owner, "RodOfAges_Stacks");
                    RemoveBuff(owner, NameofBuffICON);

                    RodOfAges_Stacks = owner.GetBuffWithName("RodOfAges_Stacks");
                    TotalStacks = RodOfAges_Stacks != null ? RodOfAges_Stacks.StackCount : 0;
                }
            }

            //Log the Stacks somewhere to be visible to Humans.
                LogInfo("Total Stacks: " + TotalStacks);
        }

        public void OnDeactivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }
    }

}