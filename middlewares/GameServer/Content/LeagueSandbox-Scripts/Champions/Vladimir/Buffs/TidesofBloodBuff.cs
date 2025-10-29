using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class TidesofBloodBuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;
        private Particle p1;
        private Particle p2;
        private Particle p3;
        private Particle p4;
        private Particle p5;
        private Particle p6;
        private Spell originSpell;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private ObjAIBase Owner;
        public SpellSector DRMundoWAOE;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell OwnerSpell)
        {
            Owner = OwnerSpell.CastInfo.Owner;
            originSpell = OwnerSpell;

            ApiEventManager.OnSpellHit.AddListener(this, OwnerSpell, TargetExecute, false);
            var spellPos = new Vector2(originSpell.CastInfo.TargetPositionEnd.X, originSpell.CastInfo.TargetPositionEnd.Z);
            Owner.AddStatModifier(StatsModifier);



            DRMundoWAOE = OwnerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = OwnerSpell.CastInfo.Owner,
                Length = 610f,
                Tickrate = 2,
                CanHitSameTargetConsecutively = false,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            float AP = Owner.Stats.AbilityPower.Total * 0.45f;
            float damage = 60f * spell.CastInfo.SpellLevel + AP;
            var caster = spell.CastInfo.Owner;

            AddParticleTarget(Owner, target, "Vladimir_Base_E_cas.troy", target, 1f, 1, "");
            AddParticleTarget(Owner, target, "Vladimir_Base_E_mis.troy", target, 1f, 1, "");
            AddParticleTarget(Owner, target, "Vladimir_Base_E_tar.troy", target, 1f, 1, "");
            AddParticleTarget(Owner, target, "VladTidesofBlood_cas.troy", target, 1f, 1, "");
            AddParticleTarget(Owner, target, "VladTidesofBloodHeart_mis.troy", target, 1f, 1);
            AddParticleTarget(Owner, target, "VladTidesofBlood_mis.troy", target, 1f, 1);
            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell OwnerSpell)
        {
            ApiEventManager.OnSpellHit.RemoveListener(this);
            DRMundoWAOE.SetToRemove();
        }
    }
}