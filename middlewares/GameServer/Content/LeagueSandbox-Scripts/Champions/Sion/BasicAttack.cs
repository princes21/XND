using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Enums;

namespace Spells
{
    public class SionBasicAttack : ISpellScript
    {
        AttackableUnit Target;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
        }

        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;

            owner.GetSpell("SionW").LowerCooldown(3.5f);
            if (owner.Stats.HealthPoints.Total >= 10000f )
            {
                Target.TakeDamage(owner, owner.Stats.AttackDamage.Total, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                Target.TakeDamage(owner, owner.Stats.HealthPoints.Total * 0.025f, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
        }
    }
}

