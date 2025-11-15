using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class ShacoExplode : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new()
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new();

        private ObjAIBase Owner;
        private SpellSector aoe;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;

            // nova particle
            AddParticleTarget(Owner, unit, "Hallucinate_nova.troy", unit, 1f);

            // 280 radius explosion
            aoe = ownerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = unit,
                Length = 280f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = false,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral |
                                   SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 0.25f
            });

            ApiEventManager.OnSpellSectorHit.AddListener(this, aoe, OnHit, false);
        }

        private void OnHit(SpellSector sector, AttackableUnit target)
        {
            float ap = Owner.Stats.AbilityPower.Total * 0.45f;
            float damage = 300f + ap;
            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL,
                              DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            aoe.SetToRemove();
            ApiEventManager.OnSpellSectorHit.RemoveListener(this);
        }

        public void OnUpdate(float diff) { }
    }
}