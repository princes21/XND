using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Spells
{
    public class Parley : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            float damage = owner.Stats.AttackDamage.Total * 1.35f;
            float miniondamage = owner.Stats.AttackDamage.Total * 2.25f;
            float doubledamage = owner.Stats.AttackDamage.Total * 3.00f;
            float miniondoubledamage = owner.Stats.AttackDamage.Total * 6.50f;
            if (!target.HasBuff("GangplankPierce"))
            {
                if (target is Minion)
                {
                    target.TakeDamage(owner, miniondamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                }
                else
                {
                    target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                }
            }
            else
            {
                if (target is Minion)
                {
                    target.TakeDamage(owner, miniondoubledamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
                }
                else
                {
                    target.TakeDamage(owner, doubledamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
                }
                RemoveBuff(target, "GangplankPierce");
            }
            AddParticleTarget(owner, target, "Pantheon_Base_Q_mis.troy", target, 6f);

            if (target.IsDead)
            {
                spell.SetCooldown(0f, true);
                if (target is Champion)
                {
                    StatsModifier.AttackDamage.FlatBonus = 25f;
                    owner.AddStatModifier(StatsModifier);
                }
                else
                {
                    StatsModifier.AttackDamage.FlatBonus = 5f;
                    owner.AddStatModifier(StatsModifier);
                }
            }
            AddBuff("GangplankPierce", 6f, 1, spell, target, owner);
        }
    }
}
