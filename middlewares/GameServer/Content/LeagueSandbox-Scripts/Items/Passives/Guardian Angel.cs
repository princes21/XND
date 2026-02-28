using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using Buffs;

namespace ItemPassives
{
    public class ItemID_3026 : IItemScript
    {
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private ObjAIBase owner;
        private Particle p;
        private bool wasOnCooldown = false;

        public void OnActivate(ObjAIBase owner)
        {
            this.owner = owner;
            StatsModifier.AttackDamage.FlatBonus = 45f;
            StatsModifier.Armor.FlatBonus = 20f;
            StatsModifier.MagicResist.FlatBonus = 25f;
            owner.AddStatModifier(StatsModifier);
            p = AddParticleTarget(owner, owner, "rebirthready.troy", owner, float.MaxValue, bone: "spine");
        }

        public void OnUpdate(float diff)
        {
            bool onCooldown = owner.HasBuff("GuardianAngelCooldown");

            if (wasOnCooldown && !onCooldown)
            {
                p = AddParticleTarget(owner, owner, "rebirthready.troy", owner, float.MaxValue, bone: "spine");
            }
            wasOnCooldown = onCooldown;

            if (!onCooldown && owner.Stats.CurrentHealth <= owner.Stats.HealthPoints.Total * 0.15f)
            {
                owner.Stats.CurrentHealth = owner.HasBuff("GreviousWound")
                    ? owner.Stats.HealthPoints.Total * 0.50f
                    : owner.Stats.HealthPoints.Total;

                AddBuff("Invulnerable", 4.0f, 1, null, owner, owner);
                AddBuff("GuardianAngelCooldown", 60.0f, 1, null, owner, owner);
                AddBuff("HasBeenRevived", 60.0f, 1, null, owner, owner);
                RemoveParticle(p);

                AddParticleTarget(owner, owner, "GuardianAngel_tar.troy", owner, 2.0f);
                AddParticleTarget(owner, owner, "GuardianAngel_cas.troy", owner, 2.0f);
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);
        }
    }
}