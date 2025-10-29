﻿using System.Linq;
using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits; // DamageData
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    public class ItemStatikShankCharge : IBuffGameScript
    {
        private const byte MAX_STACKS = 100;
        private const float CHAIN_RANGE = 550f;
        private const int MAX_EXTRA_TARGETS = 3;

        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = MAX_STACKS
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private ObjAIBase _owner;
        private Buff _self;
        private bool _procReady;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _owner = unit as ObjAIBase;
            _self = buff;

            ApiEventManager.OnPreAttack.AddListener(this, _owner, OnPreAttack, false);
            ApiEventManager.OnHitUnit.AddListener(this, _owner, OnHitUnit, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnPreAttack.RemoveListener(this, _owner);
            ApiEventManager.OnHitUnit.RemoveListener(this, _owner);
        }

        private void OnPreAttack(Spell aa)
        {
            _procReady = _self.StackCount >= MAX_STACKS;
        }

        private void OnHitUnit(DamageData data)
        {
            if (!_procReady || _self.StackCount < MAX_STACKS) return;
            if (data.Attacker != _owner || !data.IsAutoAttack) return;

            var primary = data.Target;
            if (primary == null || primary.IsDead || primary.Stats == null || !primary.Stats.IsTargetable)
            {
                _self.SetStacks(1); // resetăm la 1 ca să evităm crash
                _procReady = false;
                return;
            }


            _self.SetStacks(1);
            _procReady = false;


            ApplyShiv(primary, data);


            var extras = GetUnitsInRange(primary.Position, CHAIN_RANGE, true)
                .Where(u => u != null && u != primary && !u.IsDead && u.Team != _owner.Team && u.Stats != null && u.Stats.IsTargetable)
                .OrderBy(u => Vector2.DistanceSquared(u.Position, primary.Position))
                .Take(MAX_EXTRA_TARGETS)
                .ToList();

            GameObject beamFrom = primary;
            foreach (var u in extras)
            {
                ApplyShiv(u, data);
                AddParticleTarget(_owner, beamFrom, "kennen_btl_beam.troy", u, 1.0f);
                AddParticleTarget(_owner, u, "kennen_btl_tar.troy", u, 1.0f);
                beamFrom = u;
            }
        }

     private void ApplyShiv(AttackableUnit target, DamageData data)
    {
        if (target == null || target.IsDead)
            return;

        var AD_Damage = _owner.Stats.AttackDamage.Total * 0.10f;
        var AP_Damage = _owner.Stats.AbilityPower.Total * 0.25f;
        var MaxHealth_Damage = _owner.Stats.HealthPoints.Total * 0.015f;
        float dmg = AD_Damage + AP_Damage + MaxHealth_Damage;

        target.TakeDamage(
            _owner,
            dmg,
            DamageType.DAMAGE_TYPE_TRUE,
            DamageSource.DAMAGE_SOURCE_PROC,
            false
        );
        //AddBuff("LastWhisperDebuff", 1f, 1, spell, _owner, _owner);

                AddParticleTarget(_owner, target, "kennen_btl_tar.troy", target, 1.0f);
    }

    }
}