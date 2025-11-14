using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class ShacoExplode : IBuffGameScript
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
            p1 = AddParticleTarget(Owner, unit, "Hallucinate_nova.troy",unit, 1f, 1, "");



            DRMundoWAOE = OwnerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = unit,
                Length = 280f,
                Tickrate = 2,
                CanHitSameTargetConsecutively = false,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 1f
            });
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            float AP = Owner.Stats.AbilityPower.Total * 0.45f;
            float damage = 300f + AP;
            if (Owner.HasBuff("Liandryowner"))
            {
                if (Owner.Team != target.Team && target is AttackableUnit && !(target is BaseTurret) && !(target is ObjAnimatedBuilding))
                {
                    AddBuff("Liandry", 3f, 1, spell, target, Owner);
                }
            }
            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell OwnerSpell)
        {
            RemoveParticle(p1);
            ApiEventManager.OnSpellHit.RemoveListener(this);
            DRMundoWAOE.SetToRemove();
        }

        public void OnUpdate(float diff)
        {
        }
    }
}