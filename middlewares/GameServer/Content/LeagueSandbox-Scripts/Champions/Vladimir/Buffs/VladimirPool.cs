using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;



namespace Buffs
{
    class VladimirPool : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        float Speed;
        AttackableUnit Target;
        private Spell spell;
        Buff Buff;
        Particle p;
        public SpellSector DRMundoWAOE;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Buff = buff;
            spell = ownerSpell;

            if (unit is ObjAIBase obj)
            {
                var owner = spell.CastInfo.Owner;

                StatsModifier.MoveSpeed.PercentBonus = 0.375f;

                owner.AddStatModifier(StatsModifier);
                var h = owner.Stats.CurrentHealth = owner.Stats.CurrentHealth;
                var Health = h * 0.20f;
                p = AddParticleTarget(obj, obj, "VladSanguinePool_buf.troy", obj, 2f, 1, "");
                p = AddParticleTarget(obj, obj, "Vladimir_Base_W_buf.troy", obj, 2f, 1, "");
                buff.SetStatusEffect(StatusFlags.Targetable, false);
                buff.SetStatusEffect(StatusFlags.Ghosted, true);
                buff.SetStatusEffect(StatusFlags.CanCast, false);
                buff.SetStatusEffect(StatusFlags.CanAttack, false);
                owner.Stats.CurrentHealth = owner.Stats.CurrentHealth - Health;
                ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);



                DRMundoWAOE = ownerSpell.CreateSpellSector(new SectorParameters
                {
                    BindObject = ownerSpell.CastInfo.Owner,
                    Length = 350f,
                    Tickrate = 2,
                    CanHitSameTargetConsecutively = true,
                    OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                    Type = SectorType.Area
                });
            }
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var health = owner.Stats.HealthPoints.FlatBonus;
            var health1 = health * 0.05f;
            float damage = 26f * spell.CastInfo.SpellLevel + health1;

            if (!target.IsDead && owner.Team != target.Team )
            {
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            }
            if (owner.Team != target.Team)
            {
                AddBuff("VladimirPoolDebuff", 1.65f, 1, spell, target, owner);
            }
            if (target is Champion && !target.IsDead && owner.Team != target.Team)
            {
                AddBuff("VladimirPoolHeal", 0.85f, 1, spell, owner, owner);
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            buff.SetStatusEffect(StatusFlags.Targetable, true);
            buff.SetStatusEffect(StatusFlags.Ghosted, false);
            buff.SetStatusEffect(StatusFlags.CanCast, true);
            buff.SetStatusEffect(StatusFlags.CanAttack, true);
            StatsModifier.MoveSpeed.PercentBonus = 0.375f;
            DRMundoWAOE.SetToRemove();
            ApiEventManager.OnSpellHit.RemoveListener(this);
            RemoveParticle(p);
        }
    }
}